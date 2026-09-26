using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Scripting;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.World;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Modifiers;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
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

		private const int MaxMarkScriptLength = 1500;

		// The distance the client's own return warp puts the player in front of the NPC.
		private const float ReturnWarpNpcDistance = 20;
		private const float ReturnWarpStepDistance = 5;

		private readonly object _syncLock = new();
		private readonly List<Quest> _quests = new();
		private readonly List<long> _disabledQuests = new();
		private readonly HashSet<long> _markerNotifiedSuccess = new();

		private TimeSpan _autoReceiveDelay = AutoReceiveDelay;
		private QuestTrackData _pendingTrack;
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
		/// Clears all quests to release references for GC.
		/// </summary>
		public void Clear()
		{
			lock (_syncLock)
			{
				_quests.Clear();
				_disabledQuests.Clear();
				_markerNotifiedSuccess.Clear();
			}
		}

		/// <summary>
		/// Removes every quest (in-progress, completed, abandoned) from the
		/// character and resets their client-side state so the character
		/// behaves as if no quest had ever been touched.
		/// </summary>
		/// <returns>
		/// The total number of quests that were reset.
		/// </returns>
		public int ResetAll()
		{
			Quest[] all;
			lock (_syncLock)
				all = _quests.ToArray();

			foreach (var quest in all)
			{
				var lua = $"Melia.Quests.Remove('{quest.ObjectIdStr}')";
				Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
			}

			lock (_syncLock)
			{
				_quests.Clear();
				_disabledQuests.Clear();
			}

			return all.Length;
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
		/// Updates the quest's sequential unlocks and notifies every
		/// objective that just became unlocked.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateUnlock(Quest quest)
		{
			var unlocked = quest.UpdateUnlock();

			for (var i = 0; i < unlocked.Count; i++)
				unlocked[i].Objective.OnUnlocked(this.Character, quest);
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
				foreach (var quest in _quests.ToArray())
				{
					if (quest.Status != QuestStatus.InProgress && quest.Status != QuestStatus.Success)
						continue;

					quest.UpdateObjectives(updater);

					if (quest.ChangesOnLastUpdate && quest.Status != QuestStatus.Completed)
					{
						this.UpdateUnlock(quest);

						if (quest.Status == QuestStatus.Success && !quest.IsCompletable)
							quest.Status = QuestStatus.InProgress;

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
						this.UpdateUnlock(quest);
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
				ZoneServer.Instance.ServerEvents.PlayerStartedQuest.Raise(new PlayerStartedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));
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
				// Added to the log first, so the track the quest's status
				// transition starts finds it there.
				this.AddSilent(quest);
				this.Start(quest);
			}
			else
			{
				quest.StartTime = DateTime.Now.Add(delay);
				this.AddSilent(quest);
			}

			ZoneServer.Instance.ServerEvents.PlayerStartedQuest.Raise(new PlayerStartedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));

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
			this.UpdateUnlock(quest);

			if (quest.StartTime == DateTime.MinValue)
				quest.StartTime = DateTime.Now;

			var questScript = quest.AssociatedGenerator;
			if (questScript == null && !QuestScript.TryGet(quest.Data.Id, out questScript))
			{
				Log.Debug($"No static QuestScript found for QuestId {quest.Data.Id} during Start.");
			}
			questScript?.OnStart(this.Character, quest);

			this.UpdateClient_AddQuest(quest);
			this.ResetQuestTrack(quest.Data.Id);
			this.UpdateTrackBinding(quest);
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
						this.UpdateUnlock(quest);
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
						this.UpdateUnlock(quest);
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

			_markerNotifiedSuccess.Remove(quest.Data.Id.Value);

			if (QuestScript.TryGet(quest.Data.Id, out var questScript))
				questScript.OnComplete(this.Character, quest);

			this.GiveRewards(quest);

			this.UpdateClient_QuestStatusProperty(quest);
			this.UpdateClient_RemoveQuest(quest);
			this.UpdateClient_CompleteQuest(quest);

			// Raised after the client knows the new status, since handlers redraw the client's map icons from it.
			ZoneServer.Instance.ServerEvents.PlayerCompletedQuest.Raise(new PlayerCompletedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));
		}

		/// <summary>
		/// Removes quest from quest log.
		/// </summary>
		/// <param name="quest"></param>
		public void Cancel(Quest quest)
		{
			// A track's own OnCancel can call back into here for the same
			// quest; without this, the two would cancel each other in a loop.
			if (quest.Status == QuestStatus.Abandoned)
				return;

			quest.Status = QuestStatus.Abandoned;

			_markerNotifiedSuccess.Remove(quest.Data.Id.Value);

			// The quest window lets a cancelable quest be abandoned even
			// while its track is playing, and the track has no other way to
			// find out the quest is gone - it only ends on a quest status it
			// will now never reach, stranding the player in its layer.
			var activeTrack = this.Character.Tracks.ActiveTrack;
			if (activeTrack != null && activeTrack.Data.QuestId == quest.Data.Id.Value)
				this.Character.Tracks.Cancel();

			if (QuestScript.TryGet(quest.Data.Id, out var questScript))
				questScript.OnCancel(this.Character, quest);

			this.UpdateClient_QuestStatusProperty(quest);
			this.UpdateClient_RemoveQuest(quest);

			ZoneServer.Instance.ServerEvents.PlayerAbandonedQuest.Raise(new PlayerAbandonedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));
		}

		/// <summary>
		/// Starts the given track, holding it back until the character is
		/// out of the dialog that triggered it.
		/// </summary>
		/// <remarks>
		/// A track opens a dialog of its own, which throws while another is active.
		/// </remarks>
		/// <param name="trackData"></param>
		private void BeginTrack(QuestTrackData trackData)
		{
			if (this.Character.Connection.CurrentDialog != null)
			{
				_pendingTrack = trackData;
				return;
			}

			_pendingTrack = null;

			var startTask = this.Character.Tracks.Start(trackData);
			_ = startTask.ContinueWith(
				task => Log.Error("QuestComponent.BeginTrack: Track '{0}' failed for '{1}'. {2}", trackData.TrackName, this.Character.Name, task.Exception),
				TaskContinuationOptions.OnlyOnFaulted);
		}

		/// <summary>
		/// Clears the record of the quest's track having played, so that
		/// starting the quest again plays the track again.
		/// </summary>
		/// <param name="questId"></param>
		private void ResetQuestTrack(QuestId questId)
		{
			if (!QuestScript.TryGet(questId, out var questScript))
				return;

			var trackName = questScript.TrackData.TrackName;
			if (string.IsNullOrEmpty(trackName))
				return;

			var propertyName = trackName;
			if (TrackScript.TryGet(trackName, out var trackScript) && !string.IsNullOrEmpty(trackScript.Data.PropertyId))
				propertyName = trackScript.Data.PropertyId;

			this.Character.SetEtcProperty(propertyName, 0);
		}

		/// <summary>
		/// Starts or ends the track bound to the quest, based on the status
		/// the quest is in now.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateTrackBinding(Quest quest)
		{
			if (!QuestScript.TryGet(quest.Data.Id, out var questScript))
				return;

			var trackData = questScript.TrackData;
			if (string.IsNullOrEmpty(trackData.TrackName))
				return;

			var activeTrack = this.Character.Tracks.ActiveTrack;

			if (activeTrack != null)
			{
				if (activeTrack.Id == trackData.TrackName && quest.Status >= trackData.OnTrackEnd)
					this.Character.Tracks.End(trackData.TrackName);

				return;
			}

			if (trackData.AutoStart && quest.Status == trackData.OnTrackStart)
				this.BeginTrack(trackData);
		}

		/// <summary>
		/// Plays the track bound to the given quest, returns false if the
		/// quest has none or a track is already running.
		/// </summary>
		/// <remarks>
		/// For tracks a trigger plays rather than the quest's own status
		/// transition. See SetTrack's autoStart parameter.
		/// </remarks>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool StartQuestTrack(QuestId questId)
		{
			// A track held back until the dialog closes is not active yet,
			// so without this a second conversation queues it twice.
			if (this.Character.Tracks.ActiveTrack != null || _pendingTrack != null)
				return false;

			if (!QuestScript.TryGet(questId, out var questScript))
				return false;

			if (string.IsNullOrEmpty(questScript.TrackData.TrackName))
				return false;

			this.BeginTrack(questScript.TrackData);
			return true;
		}

		/// <summary>
		/// Clears the record of the quest's track having played, so the
		/// trigger that owns it can play it again.
		/// </summary>
		/// <remarks>
		/// For a track bound to a place rather than to the quest's status.
		/// Replaying such a track from the quest giver would stage the
		/// cutscene wherever the player happens to be standing.
		/// </remarks>
		/// <param name="questId"></param>
		public void ClearQuestTrack(QuestId questId)
		{
			if (this.Character.Tracks.ActiveTrack != null)
				return;

			this.ResetQuestTrack(questId);
		}

		/// <summary>
		/// Clears the quest's track record and plays it again, so a track
		/// that death or a relog interrupted can be restarted on demand.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool ReplayQuestTrack(QuestId questId)
		{
			if (this.Character.Tracks.ActiveTrack != null || _pendingTrack != null)
				return false;

			this.ResetQuestTrack(questId);

			return this.StartQuestTrack(questId);
		}

		/// <summary>
		/// Stores the item the character picked out of the quest's
		/// pick-one-of reward, returns false if the quest doesn't offer it.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="itemId"></param>
		/// <returns></returns>
		public bool SelectReward(QuestId questId, int itemId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id != questId)
						continue;

					if (!quest.Data.Rewards.OfType<SelectItemReward>().Any(a => a.IsOption(itemId)))
						return false;

					quest.Vars.SetInt(SelectItemReward.SelectionVarName, itemId);
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns the quest's pick-one-of reward, if it has one.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="reward"></param>
		/// <returns></returns>
		public bool TryGetSelectItemReward(QuestId questId, out SelectItemReward reward)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id != questId)
						continue;

					reward = quest.Data.Rewards.OfType<SelectItemReward>().FirstOrDefault();
					return reward != null;
				}
			}

			reward = null;
			return false;
		}

		/// <summary>
		/// Returns true if the quest has at least one reward defined,
		/// item, EXP, or otherwise.
		/// </summary>
		/// <param name="questId"></param>
		/// <returns></returns>
		public bool HasRewards(QuestId questId)
		{
			lock (_syncLock)
			{
				foreach (var quest in _quests)
				{
					if (quest.Data.Id != questId)
						continue;

					return quest.Data.Rewards.Count > 0;
				}
			}

			return false;
		}

		/// <summary>
		/// Gives quest's rewards to character.
		/// </summary>
		/// <param name="quest"></param>
		private void GiveRewards(Quest quest)
		{
			foreach (var reward in quest.Data.Rewards)
				reward.Give(this.Character, quest);
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

					if (quest.Status == QuestStatus.Completed)
						break;

					quest.Status = status;

					this.UpdateClient_UpdateQuest(quest);
					this.UpdateTrackBinding(quest);
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
						ZoneServer.Instance.ServerEvents.PlayerStartedQuest.Raise(new PlayerStartedQuestEventArgs(this.Character, (int)quest.Data.Id.Value));
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

			if (_pendingTrack != null && this.Character.Connection.CurrentDialog == null)
				this.BeginTrack(_pendingTrack);

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

			// The client tracks each quest's status in its own session
			// property, so completed quests stop offering a start marker.
			foreach (var quest in quests)
				this.UpdateClient_QuestStatusProperty(quest);

			foreach (var quest in quests.Where(a => a.InProgress))
			{
				// Re-check quest objectives to sync with current state (e.g., collection items in inventory)
				this.InitialChecks(quest);

				this.UpdateClient_AddQuestSessionObject(quest);

				var questTable = this.QuestToTable(quest);

				var lua = "Melia.Quests.Restore(" + questTable.Serialize() + ")";
				Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
			}

			this.UpdateClient_NotifyQuests();
		}

		/// <summary>
		/// Replays the client's new-quest notification for every available
		/// quest, so the client rebuilds its tracker after a relog.
		/// </summary>
		private void UpdateClient_NotifyQuests()
		{
			foreach (var questScript in QuestScript.GetAll().OrderBy(a => a.Data.Id.Value))
			{
				var questId = questScript.Data.Id;

				if (questId.NamespaceId != 0)
					continue;

				if (!ZoneServer.Instance.Data.QuestDb.Contains((int)questId.Value))
					continue;

				if (this.HasCompleted(questId))
					continue;

				if (!this.MeetsPrerequisites(questScript))
					continue;

				Send.ZC_ADDON_MSG(this.Character, AddonMessage.GET_NEW_QUEST, (int)questId.Value);
			}
		}

		/// <summary>
		/// Adds the quest to the client's quest log.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_AddQuest(Quest quest)
		{
			var questTable = this.QuestToTable(quest);

			var table = new LuaTable();
			table.Insert("Op", "QuestAdd");
			table.Insert("Data", questTable);

			var lua = "Melia.Quests.Add(" + questTable.Serialize() + ")";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);

			this.UpdateClient_AddQuestSessionObject(quest);

			this.UpdateClient_QuestStatusProperty(quest);
			this.UpdateClient_QuestMarks();

			//Log.Debug(lua);
		}

		/// <summary>
		/// Updates the quest objectives on the client.
		/// </summary>
		/// <param name="quest"></param>
		public void UpdateClient_UpdateQuest(Quest quest)
		{
			var objectivesTable = this.ObjectivesToTable(quest);

			var questTable = new LuaTable();
			questTable.Insert("ObjectId", "0x" + quest.ObjectId.ToString("X16"));
			questTable.Insert("Status", quest.Status.ToString());
			questTable.Insert("Done", quest.ObjectivesCompleted);
			questTable.Insert("Objectives", objectivesTable);

			var lua = "Melia.Quests.Update(" + questTable.Serialize() + ")";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);

			this.UpdateClient_QuestStatusProperty(quest);
			this.UpdateClient_QuestMarks();

			//Log.Debug(lua);

			if (quest.ObjectivesCompleted && quest.Status < QuestStatus.Completed && _markerNotifiedSuccess.Add(quest.Data.Id.Value))
			{
				ZoneServer.Instance.ServerEvents.PlayerQuestObjectivesCompleted.Raise(new PlayerQuestObjectivesCompletedEventArgs(this.Character, (int)quest.Data.Id.Value));
			}
		}

		/// <summary>
		/// Removes the quest from the client's quest log.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_RemoveQuest(Quest quest)
		{
			var lua = $"Melia.Quests.Remove('{quest.ObjectIdStr}')";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);

			this.UpdateClient_QuestMarks();
		}

		/// <summary>
		/// Updates the markers displayed above the quest NPCs on the
		/// character's current map.
		/// </summary>
		public void UpdateClient_QuestMarks()
		{
			var map = this.Character.Map;

			if (map == null || map == Map.Limbo)
				return;

			var marks = new Dictionary<string, (QuestMarkType Type, QuestType QuestType, string Icon)>();

			foreach (var questScript in QuestScript.GetAll())
			{
				if (!questScript.Data.TryGetPhase(QuestStatus.Possible, out var phase))
					continue;

				if (!this.IsMarkPhaseOn(phase, map))
					continue;

				if (this.Has(questScript.Data.Id) || !this.MeetsPrerequisites(questScript))
					continue;

				var markType = QuestMarkType.Available;
				AddMarkType(marks, phase.NpcUniqueName, markType, questScript.Data.Type, GetMarkIcon(markType, questScript.Data.Type));
			}

			foreach (var quest in this.GetList())
			{
				if (!quest.InProgress)
					continue;

				if (!TryGetCurrentPhase(quest, out var phase))
					continue;

				if (!this.IsMarkPhaseOn(phase, map))
					continue;

				var markType = quest.ObjectivesCompleted ? QuestMarkType.Complete : QuestMarkType.InProgress;
				AddMarkType(marks, phase.NpcUniqueName, markType, quest.Data.Type, GetMarkIcon(markType, quest.Data.Type));
			}

			var npcs = map.GetNpcs(a => a.Id != MonsterId.HiddenTrigger && a.UniqueName != null && marks.ContainsKey(a.UniqueName));

			var entries = new StringBuilder();
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, "Melia.QuestMarks.Begin()");

			foreach (var npc in npcs)
			{
				entries.Append($"[\"{npc.GetClientDialogName()}\"]=\"{marks[npc.UniqueName].Icon}\",");

				if (entries.Length > MaxMarkScriptLength)
				{
					Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, "Melia.QuestMarks.Add({" + entries + "})");
					entries.Clear();
				}
			}

			if (entries.Length > 0)
				Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, "Melia.QuestMarks.Add({" + entries + "})");

			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, "Melia.QuestMarks.Commit()");
		}

		/// <summary>
		/// Re-shows the client's quest tracker, which the client hides when
		/// the character's layer changes.
		/// </summary>
		public void RefreshChase()
		{
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, "M_CHASE_UPDATE_VISIBILITY()");
		}

		/// <summary>
		/// Mirrors the quest's status onto the session property the client
		/// tracks it by, so the client's own quest UI and minimap markers
		/// agree with the quest's real state.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_QuestStatusProperty(Quest quest)
		{
			var propertyName = quest.QuestStaticData?.QuestProperty;
			if (string.IsNullOrEmpty(propertyName) || propertyName == "None")
				return;

			this.Character.SetProperty(this.Character.SessionObjects.Main, propertyName, (float)quest.Status);
		}

		/// <summary>
		/// Returns true if the phase puts a marker on an NPC on the given
		/// map.
		/// </summary>
		/// <param name="phase"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		private bool IsMarkPhaseOn(QuestPhase phase, Map map)
		{
			if (string.IsNullOrWhiteSpace(phase.NpcUniqueName))
				return false;

			return string.IsNullOrEmpty(phase.MapClassName) || phase.MapClassName == map.ClassName;
		}

		/// <summary>
		/// Notes the marker for the NPC, keeping the more important one if
		/// it already has a marker from another quest. A main quest shadows
		/// a sub quest of the same kind, being the one the chain gives next.
		/// </summary>
		/// <param name="marks"></param>
		/// <param name="npcUniqueName"></param>
		/// <param name="markType"></param>
		/// <param name="questType"></param>
		/// <param name="icon"></param>
		private static void AddMarkType(Dictionary<string, (QuestMarkType Type, QuestType QuestType, string Icon)> marks, string npcUniqueName, QuestMarkType markType, QuestType questType, string icon)
		{
			if (marks.TryGetValue(npcUniqueName, out var existing))
			{
				if (existing.Type > markType)
					return;

				if (existing.Type == markType && (existing.QuestType == QuestType.Main || questType != QuestType.Main))
					return;
			}

			marks[npcUniqueName] = (markType, questType, icon);
		}

		/// <summary>
		/// Returns the effect name for the given marker state and quest
		/// type, mirroring the client's own mark naming.
		/// </summary>
		/// <param name="markType"></param>
		/// <param name="questType"></param>
		/// <returns></returns>
		private static string GetMarkIcon(QuestMarkType markType, QuestType questType)
		{
			var stateName = markType switch
			{
				QuestMarkType.Available => "possible",
				QuestMarkType.InProgress => "progress",
				QuestMarkType.Complete => "success",
				_ => null,
			};

			if (stateName == null)
				return null;

			return "I_quest_mask_" + stateName + GetMarkTail(questType);
		}

		/// <summary>
		/// Returns the effect name suffix for the given quest type.
		/// </summary>
		/// <param name="questType"></param>
		/// <returns></returns>
		private static string GetMarkTail(QuestType questType) => questType switch
		{
			QuestType.Sub => "_sub",
			QuestType.Repeat => "_repeat",
			QuestType.Party => "_party",
			QuestType.KeyItem => "_key",
			_ => "",
		};

		/// <summary>
		/// Notifies the client that the quest was completed.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_CompleteQuest(Quest quest)
		{
			var lua = $"Melia.Quests.Remove('{quest.ObjectIdStr}')";
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, lua);
		}

		/// <summary>
		/// Returns the phase the quest is in, based on its status and
		/// whether its objectives are done.
		/// </summary>
		/// <param name="quest"></param>
		/// <param name="phase"></param>
		/// <returns></returns>
		public static bool TryGetCurrentPhase(Quest quest, out QuestPhase phase)
		{
			var status = quest.ObjectivesCompleted ? QuestStatus.Success : quest.Status;

			if (quest.Data.TryGetPhase(status, out phase))
				return true;
			if (quest.Data.TryGetPhase(QuestStatus.InProgress, out phase))
				return true;

			return quest.Data.TryGetPhase(QuestStatus.Possible, out phase);
		}

		/// <summary>
		/// Returns the map and position the quest's current phase points at,
		/// which is where the quest's return warp sends the character.
		/// </summary>
		/// <param name="quest"></param>
		/// <param name="mapClassName"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public static bool TryGetPhaseDestination(Quest quest, out string mapClassName, out Position position)
		{
			mapClassName = null;
			position = Position.Zero;

			var hasPhase = TryGetCurrentPhase(quest, out var phase);

			// Quests with no phases still name their giver, who takes the turn-in in practice.
			var npcUniqueNames = new[] { hasPhase ? phase.NpcUniqueName : null, quest.Data.EndNpcUniqueName, quest.Data.StartNpcUniqueName };
			var phaseMapClassName = hasPhase ? phase.MapClassName : null;

			foreach (var npcUniqueName in npcUniqueNames)
			{
				if (string.IsNullOrEmpty(npcUniqueName))
					continue;

				if (!TryFindQuestNpc(quest, npcUniqueName, phaseMapClassName, out var npc))
					continue;

				mapClassName = npc.Map.ClassName;
				position = GetReturnWarpPosition(npc);

				return true;
			}

			if (!hasPhase || string.IsNullOrEmpty(phase.MapClassName) || phase.Position == Position.Zero)
				return false;

			mapClassName = phase.MapClassName;
			position = phase.Position;

			return true;
		}

		/// <summary>
		/// Returns the NPC a quest names, whether it uses the NPC's unique
		/// name or its display name, which is how custom quests point at
		/// their giver.
		/// </summary>
		/// <param name="quest"></param>
		/// <param name="npcName"></param>
		/// <param name="mapClassName"></param>
		/// <param name="npc"></param>
		/// <returns></returns>
		private static bool TryFindQuestNpc(Quest quest, string npcName, string mapClassName, out IMonster npc)
		{
			npc = null;

			if (string.IsNullOrEmpty(npcName))
				return false;

			if (ZoneServer.Instance.World.TryGetMonster(a => a.UniqueName == npcName, out npc))
				return true;

			// The same display name can be used on several maps, so prefer the one the quest places it on.
			var location = !string.IsNullOrEmpty(mapClassName) ? mapClassName : quest.Data.QuestGiverLocation;

			if (!string.IsNullOrEmpty(location)
				&& ZoneServer.Instance.World.TryGetMap(location, out var map)
				&& map.TryGetMonster(a => GetNpcDisplayName(a) == npcName, out npc))
			{
				return true;
			}

			return ZoneServer.Instance.World.TryGetMonster(a => GetNpcDisplayName(a) == npcName, out npc);
		}

		/// <summary>
		/// Returns the spot in front of the NPC that the return warp puts
		/// the character on, closing in on the NPC and then searching around
		/// the spot when what's directly in front isn't standable.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		private static Position GetReturnWarpPosition(IMonster npc)
		{
			var npcPosition = npc.Position;
			var ground = npc.Map.Ground;

			// Stepping inwards keeps the player in front of the NPC and every step reachable from them.
			for (var distance = ReturnWarpNpcDistance; distance >= ReturnWarpStepDistance; distance -= ReturnWarpStepDistance)
			{
				var stepPosition = npcPosition.GetRelative(npc.Direction, distance);

				if (ground.AnyObstacles(npcPosition, stepPosition) || !ground.TryGetHeightAt(stepPosition, out var stepHeight))
					continue;

				return stepPosition.WithHeight(stepHeight);
			}

			var targetPosition = npcPosition.GetRelative(npc.Direction, ReturnWarpNpcDistance);

			if (ground.TryGetNearestValidPosition(targetPosition, out var nearestPosition))
				return nearestPosition;

			return npcPosition;
		}

		/// <summary>
		/// Returns the NPC's display name with the client's line break
		/// removed, or null if it has none.
		/// </summary>
		/// <param name="npc"></param>
		/// <returns></returns>
		private static string GetNpcDisplayName(IMonster npc)
		{
			if (npc == null || string.IsNullOrEmpty(npc.Name))
				return null;

			return npc.Name.Replace("{nl}", " ").Trim();
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
				if (!reward.Displayed)
					continue;

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

			// Sent only for quests the client knows by id, so its absence marks a custom quest.
			if (quest.QuestStaticData != null && quest.Data.Id.NamespaceId == 0)
				questTable.Insert("ClientId", quest.QuestStaticData.Id);

			questTable.Insert("Name", quest.Data.Name);
			questTable.Insert("Description", quest.Data.Description);
			questTable.Insert("Location", locationName);
			questTable.Insert("Level", quest.Data.Level);
			questTable.Insert("Type", quest.Data.Type.ToString());
			questTable.Insert("Status", quest.Status.ToString());
			questTable.Insert("Done", quest.ObjectivesCompleted);
			questTable.Insert("Cancelable", quest.Data.Cancelable);
			questTable.Insert("Tracked", quest.Tracked);
			questTable.Insert("Objectives", objectivesTable);
			questTable.Insert("Rewards", rewardsTable);

			// Add quest giver information if available
			var questGiverName = TryFindQuestNpc(quest, quest.Data.StartNpcUniqueName, null, out var questGiver)
				? GetNpcDisplayName(questGiver)
				: null;

			if (!string.IsNullOrEmpty(questGiverName))
				questTable.Insert("QuestGiver", questGiverName);

			// Add quest giver location if available
			if (!string.IsNullOrEmpty(questGiverLocationName))
				questTable.Insert("QuestGiverLocation", questGiverLocationName);

			// The client's return warp compares this with the map it's on to
			// tell a local warp from a map change.
			if (quest.ObjectivesCompleted && TryGetPhaseDestination(quest, out var warpMapClassName, out _))
				questTable.Insert("WarpMap", warpMapClassName);

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
				objectiveTable.Insert("Unlimited", objective is UnlimitedKillObjective);

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
				var progress = quest.Progresses[objectiveId];
				this.UpdateClient_ObjectiveProperty(quest, objectiveId);
				if (QuestScript.TryGet(quest.Data.Id, out var questScript))
					questScript.OnProgress(this.Character, quest, progress.Objective.Id, quest.ProgressValue(objectiveId));
				if (quest.IsCompletable)
				{
					quest.Status = QuestStatus.Success;
					questScript?.OnSuccess(this.Character, quest);
					this.UpdateTrackBinding(quest);
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
				var progress = quest.Progresses[objectiveId];
				this.UpdateClient_ObjectiveProperty(quest, objectiveId);
				if (QuestScript.TryGet(quest.Data.Id, out var questScript))
					questScript.OnProgress(this.Character, quest, progress.Objective.Id, quest.ProgressValue(objectiveId));
				if (quest.IsCompletable)
				{
					quest.Status = QuestStatus.Success;
					questScript?.OnSuccess(this.Character, quest);
					this.UpdateTrackBinding(quest);
				}
			}
		}

		/// <summary>
		/// Raises the character's progress on the given quest to the furthest
		/// point any of the source characters reached, so a player joining a
		/// party track starts where the party already is.
		/// </summary>
		/// <param name="questId"></param>
		/// <param name="sources"></param>
		public void SyncProgressFrom(long questId, IEnumerable<Character> sources)
		{
			if (!this.TryGetById(questId, out var quest))
				return;

			var changed = false;

			foreach (var source in sources)
			{
				if (source == this.Character || source?.Quests == null)
					continue;

				if (!source.Quests.TryGetById(questId, out var sourceQuest))
					continue;

				foreach (var src in sourceQuest.Progresses)
				{
					if (!quest.TryGetProgress(src.Objective.Ident, out var dst))
						continue;

					if (src.Count > dst.Count)
					{
						dst.Count = src.Count;
						changed = true;
					}

					if (src.Done && !dst.Done)
					{
						dst.SetDone();
						changed = true;
					}

					if (src.Unlocked && !dst.Unlocked)
					{
						dst.Unlocked = true;
						changed = true;
					}
				}
			}

			if (!changed)
				return;

			lock (_syncLock)
			{
				for (var i = 0; i < quest.Progresses.Count; i++)
					this.UpdateClient_ObjectiveProperty(quest, i);
			}

			this.UpdateClient_UpdateQuest(quest);

			// Arriving on a fight the party has already won ends the track
			// here, the same way the last kill would have.
			if (quest.IsCompletable)
			{
				quest.Status = QuestStatus.Success;

				if (QuestScript.TryGet(quest.Data.Id, out var questScript))
					questScript.OnSuccess(this.Character, quest);

				this.UpdateTrackBinding(quest);
			}
		}

		/// <summary>
		/// Creates the quest's own session object and sends it, so the
		/// client's tracker has the object it reads counts from.
		/// </summary>
		/// <param name="quest"></param>
		private void UpdateClient_AddQuestSessionObject(Quest quest)
		{
			if (quest.QuestStaticData == null || quest.SessionObjectStaticData == null)
				return;

			var questSessionObject = this.Character.SessionObjects.GetOrCreate(quest.SessionObjectStaticData.Id);
			if (questSessionObject == null)
				return;

			Send.ZC_SESSION_OBJ_ADD(this.Character, questSessionObject, quest.QuestStaticData.Id);
		}

		/// <summary>
		/// Mirrors an objective's progress onto the quest's own session
		/// object, which the client's tracker reads its counts from.
		/// </summary>
		/// <param name="quest"></param>
		/// <param name="objectiveId"></param>
		private void UpdateClient_ObjectiveProperty(Quest quest, int objectiveId)
		{
			if (quest.QuestStaticData == null || quest.SessionObjectStaticData == null)
				return;

			var progress = quest.Progresses[objectiveId];

			var questSessionObject = this.Character.SessionObjects.GetOrCreate(quest.SessionObjectStaticData.Id);
			if (questSessionObject == null)
				return;

			var propertyName = progress.Objective is KillObjective
				? $"KillMonster{objectiveId + 1}"
				: $"QuestInfoValue{objectiveId + 1}";

			if (!PropertyTable.Exists("SessionObject", propertyName))
				return;

			questSessionObject.Properties.SetFloat(propertyName, quest.ProgressValue(objectiveId));
			Send.ZC_OBJECT_PROPERTY(this.Character, questSessionObject, propertyName);

			if (progress.Done)
			{
				var goalPropertyName = $"Goal{objectiveId + 1}";

				if (!PropertyTable.Exists("SessionObject", goalPropertyName))
					return;

				questSessionObject.Properties.SetFloat(goalPropertyName, 1);
				Send.ZC_OBJECT_PROPERTY(this.Character, questSessionObject, goalPropertyName);
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
							this.UpdateUnlock(quest); // Potentially unlocks next objective
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
								this.UpdateUnlock(quest); // Potentially unlocks next objective
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
