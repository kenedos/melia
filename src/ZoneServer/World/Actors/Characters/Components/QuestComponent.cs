using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Scripting;
using Melia.Shared.Game.Const;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Modifiers;
using Melia.Zone.World.Quests.Objectives;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// A character's quest manager.
	/// </summary>
	/// <remarks>
	/// Our current quest system is custom-made, as the system the game
	/// comes with is not very flexible. Using our own allows us to freely
	/// create custom quests, add features that wouldn't be available
	/// otherwise, and generally be independent of the game's ideas of
	/// quests. The downside is that our system might require some
	/// rethinking when trying to replicate the game's quests.
	/// </remarks>
	public class QuestComponent : CharacterComponent, IUpdateable
	{
		private readonly static TimeSpan AutoReceiveDelay = TimeSpan.FromMinutes(1);
		private readonly static TimeSpan LocationCheckInterval = TimeSpan.FromSeconds(1);

		private readonly object _syncLock = new();
		private readonly List<Quest> _quests = new();
		private readonly List<long> _disabledQuests = new();

		private TimeSpan _autoReceiveDelay = AutoReceiveDelay;
		private TimeSpan _timeSinceLastLocationCheck = TimeSpan.Zero;

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public QuestComponent(Character character)
			: base(character)
		{
		}

		/// <summary>
		/// Notes the given quest db id as disabled.
		/// </summary>
		/// <remarks>
		/// Used to remember quests to keep around that are not currently
		/// loaded by the server, but should still be available to the
		/// character once they are. See quest loading and saving.
		/// </remarks>
		/// <param name="questDbId"></param>
		internal void AddDisabledQuest(long questDbId)
		{
			lock (_syncLock)
				_disabledQuests.Add(questDbId);
		}

		/// <summary>
		/// Returns a list of all disabled quests.
		/// </summary>
		/// <returns></returns>
		internal IList<long> GetDisabledQuests()
		{
			lock (_syncLock)
				return _disabledQuests.ToArray();
		}

		/// <summary>
		/// Returns true if the quest with the given database id is
		/// disabled.
		/// </summary>
		/// <param name="questDbId"></param>
		/// <returns></returns>
		internal bool IsDisabled(long questDbId)
		{
			lock (_syncLock)
				return _disabledQuests.Contains(questDbId);
		}

		/// <summary>
		/// Adds quest without informing the client.
		/// </summary>
		/// <remarks>
		/// This is primarily used while the character and its quests are
		/// loaded from the database.
		/// </remarks>
		/// <param name="quest"></param>
		public void AddSilent(Quest quest)
		{
			lock (_syncLock)
			{
				var oldQuest = _quests.Where(q => q.Data.Id == quest.Data.Id).FirstOrDefault();
				if (oldQuest != null)
					_quests.Remove(oldQuest);
				_quests.Add(quest);
			}
		}

		/// <summary>
		/// Gets quest by id and returns it via out, returns false if the
		/// quest didn't exist.
		/// </summary>
		/// <param name="questObjectId"></param>
		/// <param name="quest"></param>
		/// <returns></returns>
		public bool TryGet(long questObjectId, out Quest quest)
		{
			lock (_syncLock)
			{
				quest = _quests.Find(a => a.ObjectId == questObjectId);
				return quest != null;
			}
		}

		/// <summary>
		/// Gets quest by id and returns it via out, returns false if the
		/// quest didn't exist.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="quest"></param>
		/// <returns></returns>
		public bool TryGetById(long questId, out Quest quest)
		{
			lock (_syncLock)
			{
				quest = _quests.Find(a => a.Data.Id.Value == questId);
				return quest != null;
			}
		}

		/// <summary>
		/// Gets quest by id and returns it via out, returns false if the
		/// quest didn't exist.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="quest"></param>
		/// <returns></returns>
		public bool TryGetById(QuestId questId, out Quest quest)
		{
			lock (_syncLock)
			{
				quest = _quests.Find(a => a.Data.Id == questId);
				return quest != null;
			}
		}

		/// <summary>
		/// Returns a list of all active quests.
		/// </summary>
		/// <returns></returns>
		public Quest[] GetInProgress()
		{
			lock (_syncLock)
				return _quests.Where(a => a.InProgress).ToArray();
		}

		/// <summary>
		/// Returns a list with all of the character's quests.
		/// </summary>
		/// <returns></returns>
		public Quest[] GetList()
		{
			lock (_syncLock)
				return _quests.ToArray();
		}

		/// <summary>
		/// Calls OnStart on the quest's objectives to go through the
		/// potential initial checks for whether the objective was
		/// possibly already completed.
		/// </summary>
		/// <param name="quest"></param>
		private void InitialChecks(Quest quest)
		{
			var checkedTypes = new HashSet<Type>();

			foreach (var objective in quest.Data.Objectives)
			{
				// Check every objective type only once, as they're designed
				// to check all of the quest's objectives at once.
				var type = objective.GetType();
				if (checkedTypes.Contains(type))
					continue;

				objective.OnStart(this.Character, quest);
				checkedTypes.Add(type);
			}
		}

		/// <summary>
		/// Iterates over the quests' objectives, runs the given function
		/// over all objectives with the given type, and updates the quest
		/// if any progresses changed.
		/// </summary>
		/// <typeparam name="TObjective"></typeparam>
		/// <param name="updater"></param>
		public void UpdateObjectives<TObjective>(QuestObjectivesUpdateFunc<TObjective> updater) where TObjective : QuestObjective
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Status != QuestStatus.InProgress)
						continue;

					quest.UpdateObjectives(updater);

					if (quest.ChangesOnLastUpdate)
					{
						quest.UpdateUnlock();
						this.UpdateClient_UpdateQuest(quest);
					}
				}
			}
		}

		/// <summary>
		/// Iterates over the quests' modifiers, runs the given function
		/// over all modifiers with the given type, and updates the quest
		/// if any progresses changed.
		/// </summary>
		/// <typeparam name="TModifier"></typeparam>
		/// <param name="updater"></param>
		public void UpdateModifiers<TModifier>(QuestModifiersUpdateFunc<TModifier> updater) where TModifier : QuestModifier
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];
					if (quest.Status != QuestStatus.InProgress)
						continue;

					quest.UpdateModifiers(updater);

					if (quest.ChangesOnLastUpdate)
					{
						quest.UpdateUnlock();
					}
				}
			}
		}

		/// <summary>
		/// Starts a quest using dynamically generated QuestData and associates
		/// it with the generator script instance for callbacks.
		/// </summary>
		/// <param name="generatedData">The dynamically created QuestData.</param>
		/// <param name="generatorInstance">The QuestScript instance that generated this quest.</param>
		/// <param name="delay">Optional delay before the quest becomes active.</param>
		/// <returns></returns>
		public YieldAwaitable StartGeneratedQuest(QuestData generatedData, QuestScript generatorInstance, TimeSpan delay = default)
		{
			if (generatedData == null)
				throw new ArgumentNullException(nameof(generatedData));
			if (generatorInstance == null)
				throw new ArgumentNullException(nameof(generatorInstance));
			if (generatedData.Id == QuestId.Zero)
				throw new ArgumentException("Generated QuestData must have a valid unique QuestId.", nameof(generatedData));

			// Ensure no duplicate active quest with the same *generated* ID (important!)
			lock (_syncLock)
			{
				if (_quests.Any(q => q.Data.Id == generatedData.Id && q.Status >= QuestStatus.Possible))
				{
					// Log error or handle gracefully - shouldn't start the same generated instance twice.
					Yggdrasil.Logging.Log.Warning($"Attempted to start generated quest {generatedData.Id} which already exists or is pending for character {Character.Name}.");
					return Task.Yield(); // Or throw exception
				}
			}

			delay = Math2.Max(TimeSpan.Zero, delay);

			// Use the new constructor or SetGenerator method
			var quest = new Quest(generatedData, generatorInstance);
			// quest.SetGenerator(generatorInstance); // Alternative if not using constructor

			// Add the quest silently first
			lock (_syncLock)
			{
				_quests.Add(quest); // Add it to the list
			}

			// Handle delay or immediate start
			if (delay == TimeSpan.Zero)
			{
				// Call the internal Start method which handles objectives, status, callbacks, and client updates
				this.Start(quest);
			}
			else
			{
				quest.Status = QuestStatus.Possible; // Mark as possible but not started
				quest.StartTime = DateTime.Now.Add(delay);
				// No client update needed yet, the Update() loop will handle starting it.
			}

			return Task.Yield();
		}

		/// <summary>
		/// Starts quest for the character, returns false if the quest
		/// couldn't be started.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public YieldAwaitable Start(string questId)
		{
			if (!ZoneServer.Instance.Data.QuestDb.TryFind(questId, out var questData))
				throw new ArgumentException($"Unknown quest '{questId}'.");
			return this.Start(new QuestId("Laima.Quest", questData.Id), TimeSpan.Zero);
		}

		/// <summary>
		/// Starts quest for the character, returns false if the quest
		/// couldn't be started.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public YieldAwaitable Start(QuestId questId)
			=> this.Start(questId, TimeSpan.Zero);

		/// <summary>
		/// Adds quest and starts it after the given delay.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="delay"></param>
		/// <returns></returns>
		public YieldAwaitable Start(QuestId questId, TimeSpan delay)
		{
			delay = Math2.Max(TimeSpan.Zero, delay);

			// Check prerequisites before starting the quest
			if (!this.MeetsPrerequisites(questId))
			{
				Log.Warning($"Character '{this.Character.Name}' attempted to start quest '{questId}' without meeting prerequisites.");
				return Task.Yield();
			}

			var quest = Quest.Create(questId);

			if (delay == TimeSpan.Zero)
			{
				this.Start(quest);
				this.AddSilent(quest);
			}
			else
			{
				quest.StartTime = DateTime.Now.Add(delay);
				this.AddSilent(quest);
			}
			return Task.Yield();
		}

		/// <summary>
		/// Starts the given quest, adding it to the character's quest log.
		/// </summary>
		/// <param name="quest"></param>
		/// <returns></returns>
		private void Start(Quest quest)
		{
			this.InitialChecks(quest);

			quest.Status = QuestStatus.InProgress;
			quest.UpdateUnlock();

			if (quest.StartTime == DateTime.MinValue)
				quest.StartTime = DateTime.Now;

			var questScript = quest.AssociatedGenerator;
			if (questScript == null && !QuestScript.TryGet(quest.Data.Id, out questScript))
			{
				Log.Debug($"No static QuestScript found for QuestId {quest.Data.Id} during Start.");
			}
			questScript?.OnStart(this.Character, quest);


			this.UpdateClient_AddQuest(quest);
		}

		/// <summary>
		/// Returns true if a quest with the given id is currently in
		/// progress and the objective with the given identifier is
		/// unlocked, but hasn't been completed yet.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveIdent"></param>
		/// <returns></returns>
		public bool IsActive(QuestId questId, string objectiveIdent)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (!quest.InProgress || quest.Data.Id != questId)
						continue;

					if (!quest.TryGetProgress(objectiveIdent, out var progress))
						continue;

					if (progress.Unlocked && !progress.Done)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if a quest with the given id is currently active,
		/// meaning that it was started, but not completed yet, even if
		/// all objectives were completed already.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		[Obsolete("Use IsActive(QuestId questId)")]
		public bool IsActive(long questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.InProgress && quest.Data.Id.Value == questId)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if a quest with the given id is currently active,
		/// meaning that it was started, but not completed yet, even if
		/// all objectives were completed already.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool IsActive(QuestId questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.InProgress && quest.Data.Id == questId)
						return true;
				}
			}

			return false;
		}

		public bool IsPossible(QuestId questId)
			=> this.IsPossible(questId.Value);

		/// <summary>
		/// Check if all prerequisites are met and the quest isn't started.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool IsPossible(long questId)
		{
			// Can't start a quest if a track is active.
			if (this.Character.Tracks.ActiveTrack != null)
				return false;

			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id.Value != questId)
						continue;
					return quest.IsPossible;
				}
				if (QuestScript.TryGet(new QuestId("Laima.Quest", questId), out var questScript))
				{
					for (var j = 0; j < questScript.Data.Prerequisites.Count; j++)
					{
						var prerequisite = questScript.Data.Prerequisites[j];
						if (!prerequisite.Met(this.Character))
							return false;
					}
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the character has the quest, is slated to
		/// receive it soon, or has completed it in the past.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool Has(QuestId questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id != questId)
						continue;

					if (quest.Status > QuestStatus.Possible)
						return true;
				}
			}

			return false;
		}

		[Obsolete("Use Has(QuestId questId)")]
		public bool Has(long questId) => this.Has(new QuestId(questId));

		/// <summary>
		/// Returns true if the character meets the prerequisites to start the
		/// given quest.
		/// </summary>
		/// <param name="questNamespace"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if no quest with the given id was found.
		/// </exception>
		public bool MeetsPrerequisites(string questNamespace, long id)
			=> this.MeetsPrerequisites(new QuestId(questNamespace, id));

		/// <summary>
		/// Returns true if the character meets the prerequisites to start the
		/// given quest.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if no quest with the given id was found.
		/// </exception>
		public bool MeetsPrerequisites(QuestId questId)
		{
			if (!QuestScript.TryGet(questId, out var questScript))
				throw new ArgumentException($"Quest '{questId}' not found.");

			return this.MeetsPrerequisites(questScript);
		}

		/// <summary>
		/// Returns true if the character meets the prerequisites to start the
		/// given quest.
		/// </summary>
		/// <param name="questScript"></param>
		/// <returns></returns>
		internal bool MeetsPrerequisites(QuestScript questScript)
		{
			foreach (var prerequisite in questScript.Data.Prerequisites)
			{
				if (!prerequisite.Met(this.Character))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Returns true if the character has ever completed the quest
		/// before.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool HasCompleted(QuestId questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id != questId)
						continue;

					if (quest.Status == QuestStatus.Completed)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the character has ever completed the quest
		/// before.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		[Obsolete("Use HasCompleted(QuestId questId)")]
		public bool HasCompleted(long questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id.Value != questId)
						continue;

					if (quest.Status == QuestStatus.Completed)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Completes the objective on all quests with the given id.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveIdent"></param>
		public void CompleteObjective(QuestId questId, string objectiveIdent)
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];
					if (!quest.InProgress || quest.Data.Id != questId)
						continue;

					if (!quest.TryGetProgress(objectiveIdent, out var progress))
						continue;

					if (!progress.Done)
					{
						progress.SetDone();
						quest.UpdateUnlock();
						this.UpdateQuestProgress(questId, progress.Objective.Id);
						this.UpdateClient_UpdateQuest(quest);
					}
				}
			}
		}

		/// <summary>
		/// Completes the objective on all quests with the given id.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveIdent"></param>
		[Obsolete("Use CompleteObjective(QuestId questId)")]
		public void CompleteObjective(long questId, string objectiveIdent)
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];
					if (!quest.InProgress || quest.Data.Id.Value != questId)
						continue;

					if (!quest.TryGetProgress(objectiveIdent, out var progress))
						continue;

					if (!progress.Done)
					{
						progress.SetDone();
						quest.UpdateUnlock();
						this.UpdateQuestProgress(questId, progress.Objective.Id);
						this.UpdateClient_UpdateQuest(quest);
					}
				}
			}
		}

		/// <summary>
		/// Completes the objective on all quests with the given id.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveIdent"></param>
		public bool Complete(QuestId questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (!quest.InProgress || quest.Data.Id != questId)
						continue;

					quest.CompleteObjectives();
					this.Complete(quest);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Completes all quests with the given id and gives the rewards
		/// to the character.
		/// </summary>
		/// <param name="questId"></param>
		[Obsolete("Use Complete(QuestId questId)")]
		public void Complete(long questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (!quest.InProgress || quest.Data.Id.Value != questId)
						continue;

					quest.CompleteObjectives();

					this.Complete(quest);
				}
			}
		}

		/// <summary>
		/// Completes quest and gives rewards to character.
		/// </summary>
		/// <param name="quest"></param>
		public void Complete(Quest quest)
		{
			quest.Status = QuestStatus.Completed;
			quest.CompleteTime = DateTime.Now;
			quest.CompleteObjectives();

			if (QuestScript.TryGet(quest.Data.Id, out var questScript))
				questScript.OnComplete(this.Character, quest);

			this.GiveRewards(quest);

			// Track achievement points for quest completion via server event
			ZoneServer.Instance.ServerEvents.PlayerCompletedQuest.Raise(new PlayerCompletedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));

			this.UpdateClient_RemoveQuest(quest);
			this.UpdateClient_CompleteQuest(quest);
		}

		/// <summary>
		/// Removes quest from quest log.
		/// </summary>
		/// <param name="quest"></param>
		public void Cancel(Quest quest)
		{
			quest.Status = QuestStatus.Abandoned;

			if (ZoneServer.Instance.Data.QuestDb.TryFind((int)quest.Data.Id.Value, out var questData) && !string.IsNullOrEmpty(quest.QuestStaticData.QuestProperty))
			{
				var main = this.Character.SessionObjects.Main;

				if (main.Properties.Has(quest.QuestStaticData.QuestProperty))
				{
					main.Properties.SetFloat(quest.QuestStaticData.QuestProperty, (int)quest.Status);
					Send.ZC_OBJECT_PROPERTY(this.Character, main, quest.QuestStaticData.QuestProperty);
				}
			}

			if (QuestScript.TryGet(quest.Data.Id, out var questScript))
				questScript.OnCancel(this.Character, quest);

			this.UpdateClient_RemoveQuest(quest);
		}

		/// <summary>
		/// Gives quest's rewards to character.
		/// </summary>
		/// <param name="quest"></param>
		private void GiveRewards(Quest quest)
		{
			foreach (var reward in quest.Data.Rewards)
				reward.Give(this.Character);
		}

		/// <summary>
		/// Abandon a quest
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool Abandon(long questId)
		{
			if (!this.Has(questId) || !this.TryGet(questId, out var quest) || !quest.InProgress)
				return false;

			this.Cancel(quest);

			return true;
		}

		/// <summary>
		/// Restart a quest
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool Restart(int questId, QuestStatus status = QuestStatus.Restarted)
		{
			if (!this.IsPossible(questId))
				return false;

			if (!this.TryGet(questId, out var quest))
				quest = Quest.Create(new QuestId("Laima.Quest", questId));
			quest.Status = status;
			this.UpdateQuestStatus(questId, quest.Status);

			if (QuestScript.TryGet(quest.Data.Id, out var questScript))
				questScript.OnStart(this.Character, quest);

			return true;
		}

		public void UpdateQuestStatus(long questId, QuestStatus status)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id.Value != questId)
						continue;

					quest.Status = status;

					this.UpdateClient_UpdateQuest(quest);
					break;
				}
			}
		}

		/// <summary>
		/// Updates quests: starts pending quests, handles auto-receive,
		/// and checks for completion of location-based objectives.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			var now = DateTime.Now;

			lock (_syncLock)
			{
				// --- 1. Start Pending Quests ---
				// Iterate backwards if removing, but here we are just starting
				// or modifying status, so forward is fine. Using ToList() to avoid collection modified issues if Start(quest) changes _quests.
				foreach (var quest in _quests.ToList()) // Iterate a copy if Start() can modify _quests
				{
					if (quest.Status == QuestStatus.Possible && quest.StartTime <= now) // Use <= for safety
					{
						Log.Debug($"QuestComponent: Starting delayed quest {quest.Data.Id.Value} for {Character.Name}.");
						this.Start(quest); // This updates status, client, etc.
					}
				}

				// --- 2. Check Location-Based Objectives (e.g., VisitLocationObjective) ---
				_timeSinceLastLocationCheck += elapsed;
				if (_timeSinceLastLocationCheck >= LocationCheckInterval)
				{
					_timeSinceLastLocationCheck -= LocationCheckInterval; // Reset timer correctly
					this.CheckVisitLocationObjectivesInternal(); // Call internal method
					this.CheckVariableCheckObjectivesInternal(); // Check variable-based objectives
				}
			}

			// --- 3. Handle Auto-Receive Quests (Outside main lock if QuestScript.StartAuto... is safe) ---
			_autoReceiveDelay = Math2.Max(TimeSpan.Zero, _autoReceiveDelay - elapsed);
			if (_autoReceiveDelay == TimeSpan.Zero)
			{
				QuestScript.StartAutoReceiveQuests(this.Character);
				_autoReceiveDelay = AutoReceiveDelay;
			}
		}

		/// <summary>
		/// Sends a list of all quests to the client to update it.
		/// </summary>
		public void UpdateClient()
		{
			var quests = this.GetList();
			foreach (var quest in quests.Where(a => a.InProgress))
			{
				// Re-check quest objectives to sync with current state (e.g., collection items in inventory)
				this.InitialChecks(quest);

				var questTable = this.QuestToTable(quest);

				var lua = "Melia.Quests.Restore(" + questTable.Serialize() + ")";
				Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
			}
		}

		/// <summary>
		/// Adds the quest to the client's quest log.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_AddQuest(Quest quest)
		{
			if (ZoneServer.Instance.Data.QuestDb.TryFind((int)quest.Data.Id.Value, out var questData))
			{
				if (quest.QuestStaticData != null)
				{
					var main = this.Character.SessionObjects.Main;

					if (!string.IsNullOrWhiteSpace(quest.QuestStaticData.QStartZone)
						&& quest.QuestStaticData.QStartZone != main.Properties.GetString(PropertyName.QSTARTZONETYPE))
					{
						main.Properties.SetString(PropertyName.QSTARTZONETYPE, quest.QuestStaticData.QStartZone);
						Send.ZC_OBJECT_PROPERTY(this.Character, main, PropertyName.QSTARTZONETYPE);
					}
				}
				if (quest.SessionObjectStaticData != null)
				{
					var questSessionObject = this.Character.SessionObjects.GetOrCreate(quest.SessionObjectStaticData.Id);
					if (questSessionObject != null)
					{
						if (quest.SessionObjectStaticData.QuestData.InfoMaxCount != null)
							questSessionObject.Properties.SetFloat(PropertyName.QuestInfoValue1, 0f);
						Send.ZC_SESSION_OBJ_ADD(this.Character, questSessionObject, quest.QuestStaticData.Id);
					}
					UpdateClient_UpdateQuest(quest);
				}
				return;
			}

			var questTable = this.QuestToTable(quest);

			var table = new LuaTable();
			table.Insert("Op", "QuestAdd");
			table.Insert("Data", questTable);

			var lua = "Melia.Quests.Add(" + questTable.Serialize() + ")";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);

			//Log.Debug(lua);
		}

		/// <summary>
		/// Updates the quest objectives on the client.
		/// </summary>
		/// <param name="quest"></param>
		public void UpdateClient_UpdateQuest(Quest quest)
		{
			if (ZoneServer.Instance.Data.QuestDb.TryFind((int)quest.Data.Id.Value, out var questData) && !string.IsNullOrEmpty(quest.QuestStaticData.QuestProperty))
			{
				var main = this.Character.SessionObjects.Main;

				if (!main.Properties.Has(quest.QuestStaticData.QuestProperty))
				{
					main.Properties.SetFloat(quest.QuestStaticData.QuestProperty, 1);
					Send.ZC_OBJECT_PROPERTY(this.Character, main, quest.QuestStaticData.QuestProperty);
				}
				main.Properties.SetFloat(quest.QuestStaticData.QuestProperty, (int)quest.Status);
				Send.ZC_OBJECT_PROPERTY(this.Character, main, quest.QuestStaticData.QuestProperty);
				return;
			}

			var objectivesTable = this.ObjectivesToTable(quest);

			var questTable = new LuaTable();
			questTable.Insert("ObjectId", "0x" + quest.ObjectId.ToString("X16"));
			questTable.Insert("Status", quest.Status.ToString());
			questTable.Insert("Done", quest.ObjectivesCompleted);
			questTable.Insert("Objectives", objectivesTable);

			var lua = "Melia.Quests.Update(" + questTable.Serialize() + ")";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);

			//Log.Debug(lua);
		}

		/// <summary>
		/// Removes the quest from the client's quest log.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_RemoveQuest(Quest quest)
		{
			if (ZoneServer.Instance.Data.QuestDb.TryFind((int)quest.Data.Id.Value, out var questData) && quest.SessionObjectStaticData != null)
			{
				this.Character.SessionObjects.Remove(quest.SessionObjectStaticData.Id);
				Send.ZC_SESSION_OBJ_REMOVE(this.Character, quest.SessionObjectStaticData.Id);
				return;
			}

			var lua = $"Melia.Quests.Remove('{quest.ObjectIdStr}')";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
		}

		/// <summary>
		/// Notifies the client that the quest was completed.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_CompleteQuest(Quest quest)
		{
			if (ZoneServer.Instance.Data.QuestDb.TryFind((int)quest.Data.Id.Value, out var questData) && !string.IsNullOrEmpty(questData.QuestProperty))
			{
				var main = this.Character.SessionObjects.Main;
				var propertyName = questData.QuestProperty;

				main.Properties.SetFloat(propertyName, (float)QuestStatus.Completed);
				Send.ZC_OBJECT_PROPERTY(this.Character, main, propertyName);
				return;
			}

			var lua = $"Melia.Quests.Remove('{quest.ObjectIdStr}')";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
		}

		/// <summary>
		/// Returns all information about the quest as a Lua table.
		/// </summary>
		/// <param name="quest"></param>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		private LuaTable QuestToTable(Quest quest)
		{
			/// Quest
			/// {
			///		string ObjectId
			///		int ClassId
			///		string Name
			///		string Description
			///		string Location
			///		int Level
			///		string Status
			///		bool Done
			///		bool Cancelable
			///		bool Tracked
			///		
			///		Objectives[]
			///		{
			///			string Text
			///			bool Unlocked
			///			bool Done
			///			int Count
			///			int TargetCount
			///		}
			///		
			///		Rewards[]
			///		{
			///			string Text
			///			string Icon
			///		}
			/// }

			var objectivesTable = this.ObjectivesToTable(quest);

			var rewardsTable = new LuaTable();
			foreach (var reward in quest.Data.Rewards)
			{
				var rewardTable = new LuaTable();
				rewardTable.Insert("Text", reward.ToString());
				rewardTable.Insert("Icon", reward.Icon);

				rewardsTable.Insert(rewardTable);
			}

			var questTable = new LuaTable();

			// Convert map class name(s) to display name(s)
			string locationName = null;
			if (!string.IsNullOrEmpty(quest.Data.Location))
			{
				var mapClassNames = quest.Data.Location.Split(',');
				var mapNames = new List<string>();

				foreach (var mapClassName in mapClassNames)
				{
					var trimmedClassName = mapClassName.Trim();
					if (ZoneServer.Instance.World.TryGetMap(trimmedClassName, out var map))
						mapNames.Add(map.Data.Name);
					else
						mapNames.Add(trimmedClassName);
				}

				locationName = string.Join(", ", mapNames);
			}

			// Convert quest giver map class name to display name
			string questGiverLocationName = null;
			if (!string.IsNullOrEmpty(quest.Data.QuestGiverLocation))
			{
				if (ZoneServer.Instance.World.TryGetMap(quest.Data.QuestGiverLocation, out var map))
					questGiverLocationName = map.Data.Name;
				else
					questGiverLocationName = quest.Data.QuestGiverLocation;
			}

			questTable.Insert("ObjectId", "0x" + quest.ObjectId.ToString("X16"));
			questTable.Insert("ClassId", "0x" + quest.Data.Id.Value.ToString("X16"));
			questTable.Insert("Name", quest.Data.Name);
			questTable.Insert("Description", quest.Data.Description);
			questTable.Insert("Location", locationName);
			questTable.Insert("Level", quest.Data.Level);
			questTable.Insert("Status", quest.Status.ToString());
			questTable.Insert("Done", quest.ObjectivesCompleted);
			questTable.Insert("Cancelable", quest.Data.Cancelable);
			questTable.Insert("Tracked", quest.Tracked);
			questTable.Insert("Objectives", objectivesTable);
			questTable.Insert("Rewards", rewardsTable);

			// Add quest giver information if available
			if (!string.IsNullOrEmpty(quest.Data.StartNpcUniqueName))
				questTable.Insert("QuestGiver", quest.Data.StartNpcUniqueName);

			// Add quest giver location if available
			if (!string.IsNullOrEmpty(questGiverLocationName))
				questTable.Insert("QuestGiverLocation", questGiverLocationName);

			return questTable;
		}

		/// <summary>
		/// Returns information about the quests objectives and their
		/// progress as a Lua table.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException"></exception>
		private LuaTable ObjectivesToTable(Quest quest)
		{
			var objectivesTable = new LuaTable();
			foreach (var objective in quest.Data.Objectives)
			{
				if (!quest.TryGetProgress(objective.Ident, out var progress))
					throw new InvalidOperationException($"Missing progress for objective '{objective.Ident}'.");

				var objectiveTable = new LuaTable();
				objectiveTable.Insert("Text", objective.Text);
				objectiveTable.Insert("Unlocked", progress.Unlocked);
				objectiveTable.Insert("Done", progress.Done);
				objectiveTable.Insert("Count", progress.Count);
				objectiveTable.Insert("TargetCount", objective.TargetCount);

				// Add monster names for collection objectives with drop modifiers
				if (objective is CollectItemObjective collectObjective)
				{
					var monsterNames = new List<string>();
					foreach (var modifier in quest.Data.Modifiers)
					{
						if (modifier is ItemDropModifier dropModifier && dropModifier.ItemId == collectObjective.ItemId)
						{
							foreach (var monsterId in dropModifier.MonsterIds)
							{
								if (ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
									monsterNames.Add(monsterData.Name);
							}
						}
					}

					if (monsterNames.Count > 0)
					{
						var monstersTable = new LuaTable();
						foreach (var monsterName in monsterNames)
							monstersTable.Insert(monsterName);
						objectiveTable.Insert("Monsters", monstersTable);
					}
				}

				objectivesTable.Insert(objectiveTable);
			}

			return objectivesTable;
		}

		/// <summary>
		/// Checks if the quest is completable
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		[Obsolete("Use IsCompletable(QuestId questId)")]
		public bool IsCompletable(long questId)
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];

					if (!quest.InProgress || quest.Data.Id.Value != questId)
						continue;

					return quest.ObjectivesCompleted;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks if the quest is completable
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool IsCompletable(QuestId questId)
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];

					if (!quest.InProgress || quest.Data.Id != questId)
						continue;

					return quest.ObjectivesCompleted && quest.Status != QuestStatus.Completed;
				}
			}

			return false;
		}

		public QuestStatus GetStatus(int questId)
		{
			lock (_syncLock)
			{
				for (var i = 0; i < _quests.Count; i++)
				{
					var quest = _quests[i];

					if ((int)quest.Data.Id.Value != questId)
						continue;

					return quest.Status;
				}
			}
			return QuestStatus.Possible;
		}

		/// <summary>
		/// Update quest progress
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveId"></param>
		public void UpdateQuestProgress(long questId, int objectiveId)
		{
			if (this.TryGetById(questId, out var quest))
			{
				var character = this.Character;
				var progress = quest.Progresses[objectiveId];
				if (quest.QuestStaticData != null)
				{
					var mainSessionObject = character.SessionObjects.Get(SessionObjectId.Main);
					// In case quest doesn't exist, set it's state to started (1)
					if (!mainSessionObject.Properties.Has(quest.QuestStaticData.QuestProperty))
					{
						mainSessionObject.Properties.SetFloat(quest.QuestStaticData.QuestProperty, 1);
						Send.ZC_OBJECT_PROPERTY(character, mainSessionObject, quest.QuestStaticData.QuestProperty);
					}

					var questSessionObject = character.SessionObjects.GetOrCreate(quest.SessionObjectStaticData.Id);
					if (questSessionObject != null)
					{
						string propertyName;
						if (quest.Progresses[objectiveId].Objective is KillObjective)
							propertyName = $"KillMonster{objectiveId + 1}";
						else
							propertyName = $"QuestInfoValue{objectiveId + 1}";

						questSessionObject.Properties.SetFloat(propertyName, quest.ProgressValue(objectiveId));
						Send.ZC_OBJECT_PROPERTY(character, questSessionObject, propertyName);
						if (progress.Done)
						{
							var goalPropertyName = $"Goal{objectiveId + 1}";
							questSessionObject.Properties.SetFloat(goalPropertyName, 1);
							Send.ZC_OBJECT_PROPERTY(character, questSessionObject, goalPropertyName);
						}
					}
				}
				if (QuestScript.TryGet(quest.Data.Id, out var questScript))
					questScript.OnProgress(this.Character, quest, progress.Objective.Id, quest.ProgressValue(objectiveId));
				if (quest.IsCompletable)
				{
					quest.Status = QuestStatus.Success;
					questScript?.OnSuccess(this.Character, quest);
				}
			}
		}

		/// <summary>
		/// Update quest progress
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="objectiveId"></param>
		public void UpdateQuestProgress(QuestId questId, int objectiveId)
		{
			if (this.TryGetById(questId, out var quest))
			{
				var character = this.Character;
				var progress = quest.Progresses[objectiveId];
				if (quest.QuestStaticData != null)
				{
					var mainSessionObject = character.SessionObjects.Get(SessionObjectId.Main);
					// In case quest doesn't exist, set it's state to started (1)
					if (!mainSessionObject.Properties.Has(quest.QuestStaticData.QuestProperty))
					{
						mainSessionObject.Properties.SetFloat(quest.QuestStaticData.QuestProperty, 1);
						Send.ZC_OBJECT_PROPERTY(character, mainSessionObject, quest.QuestStaticData.QuestProperty);
					}

					var questSessionObject = character.SessionObjects.GetOrCreate(quest.SessionObjectStaticData.Id);
					if (questSessionObject != null)
					{
						string propertyName;
						if (quest.Progresses[objectiveId].Objective is KillObjective)
							propertyName = $"KillMonster{objectiveId + 1}";
						else
							propertyName = $"QuestInfoValue{objectiveId + 1}";

						questSessionObject.Properties.SetFloat(propertyName, quest.ProgressValue(objectiveId));
						Send.ZC_OBJECT_PROPERTY(character, questSessionObject, propertyName);
						if (progress.Done)
						{
							var goalPropertyName = $"Goal{objectiveId + 1}";
							questSessionObject.Properties.SetFloat(goalPropertyName, 1);
							Send.ZC_OBJECT_PROPERTY(character, questSessionObject, goalPropertyName);
						}
					}
				}
				if (QuestScript.TryGet(quest.Data.Id, out var questScript))
					questScript.OnProgress(this.Character, quest, progress.Objective.Id, quest.ProgressValue(objectiveId));
				if (quest.IsCompletable)
				{
					quest.Status = QuestStatus.Success;
					questScript?.OnSuccess(this.Character, quest);
				}
			}
		}

		public IList<Quest> GetCompletedQuests()
		{
			lock (_quests)
				return _quests.Where(a => a.Status == QuestStatus.Completed).ToList();
		}

		/// <summary>
		/// Internal method to check for VisitLocationObjective completion.
		/// Called by Update, assumes _syncLock is already held if needed for quest list access.
		/// </summary>
		private void CheckVisitLocationObjectivesInternal()
		{
			if (this.Character.Map == null || this.Character.Map == Maps.Map.Limbo || _quests.Count == 0)
				return;

			// Iterate over a copy if modifications can happen, though SetDone/UpdateUnlock should be safe within the loop
			// if QuestComponent's other methods are also correctly locked.
			// For safety and clarity, let's iterate a copy.
			var questsInProgress = _quests.Where(q => q.InProgress).ToList();

			foreach (var quest in questsInProgress)
			{
				var questModifiedInThisIteration = false;
				foreach (var progress in quest.Progresses)
				{
					if (progress.Objective is VisitLocationObjective visitObjective
						&& progress.Unlocked
						&& !progress.Done)
					{
						if (this.Character.Map.Id != visitObjective.TargetMapId) continue;
						if (visitObjective.IsPositionWithinObjective(this.Character.Position))
						{
							Log.Info($"Character {this.Character.Name} completed VisitLocationObjective '{visitObjective.Ident}' " +
									 $"for Quest {quest.Data.Id.Value} by reaching {visitObjective.TargetPosition} (Radius: {visitObjective.TargetRadius}).");

							progress.SetDone();
							quest.UpdateUnlock(); // Potentially unlocks next objective
							questModifiedInThisIteration = true; // Mark that quest state changed

							// --- Handle OnProgress/OnSuccess Callbacks ---
							// This logic is similar to what's in UpdateQuestProgress in QuestComponent
							// We should ideally call a unified method for this.
							// For now, replicate parts of it.

							// Try to get the runtime script first (for procedural quests)
							// This relies on the runtime script being registered in QuestScript.Scripts
							if (QuestScript.TryGet(quest.Data.Id, out var callbackScript))
							{
								// For VisitLocationObjective, what are key/progress?
								// Let's use objective.Id and progress.Count (which would be 1 for visit).
								callbackScript.OnProgress(this.Character, quest, progress.Objective.Id, progress.Count);
							}
							else if (quest.Data.Id.NamespaceId != 0) // It's a procedural ID but script not found
							{
								Log.Warning($"No QuestScript found for procedural quest {quest.Data.Id.Value} during VisitLocationObjective completion.");
							}


							if (quest.IsCompletable && quest.Status < QuestStatus.Success)
							{
								quest.Status = QuestStatus.Success;
								Log.Debug($"Quest {quest.Data.Id.Value} now in Success state after visit.");
								callbackScript?.OnSuccess(this.Character, quest);
							}

							// Optimization: if this quest is now fully done (all objectives), no need to check its other objectives in this pass.
							// Note: This doesn't complete the quest; HandleProceduralTurnIn or another mechanism does that.
							if (quest.ObjectivesCompleted) break; // Break from inner (progress) loop
						}
					}
				}

				if (questModifiedInThisIteration)
				{
					this.UpdateClient_UpdateQuest(quest); // Send update to client if any objective in this quest changed
				}
			}
		}

		/// <summary>
		/// Internal method to check for VariableCheckObjective completion.
		/// Called by Update, assumes _syncLock is already held.
		/// </summary>
		private void CheckVariableCheckObjectivesInternal()
		{
			if (_quests.Count == 0)
				return;

			// Iterate over a copy to avoid modification issues
			var questsInProgress = _quests.Where(q => q.InProgress).ToList();

			foreach (var quest in questsInProgress)
			{
				var questModifiedInThisIteration = false;
				foreach (var progress in quest.Progresses)
				{
					if (progress.Objective is VariableCheckObjective variableObjective
						&& progress.Unlocked
						&& !progress.Done)
					{
						var currentValue = variableObjective.GetVariableValue(this.Character);
						if (currentValue != progress.Count)
						{
							progress.Count = Math.Min(variableObjective.TargetCount, currentValue);

							if (progress.Count >= variableObjective.TargetCount)
							{
								progress.SetDone();
								quest.UpdateUnlock(); // Potentially unlocks next objective
								questModifiedInThisIteration = true; // Mark that quest state changed

								// Handle OnProgress/OnSuccess Callbacks
								if (QuestScript.TryGet(quest.Data.Id, out var callbackScript))
								{
									callbackScript.OnProgress(this.Character, quest, progress.Objective.Id, progress.Count);
								}
								else if (quest.Data.Id.NamespaceId != 0)
								{
									Log.Warning($"No QuestScript found for procedural quest {quest.Data.Id.Value} during VariableCheckObjective completion.");
								}

								if (quest.IsCompletable && quest.Status < QuestStatus.Success)
								{
									quest.Status = QuestStatus.Success;
									Log.Debug($"Quest {quest.Data.Id.Value} now in Success state after variable check.");
									callbackScript?.OnSuccess(this.Character, quest);
								}

								// If this quest is now fully done, no need to check its other objectives in this pass
								if (quest.ObjectivesCompleted) break;
							}
							else
							{
								// Value changed but not complete yet - still need to update the client
								questModifiedInThisIteration = true;
							}
						}
					}
				}

				if (questModifiedInThisIteration)
				{
					this.UpdateClient_UpdateQuest(quest); // Send update to client if any objective in this quest changed
				}
			}
		}
	}
}
