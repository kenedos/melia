using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using Yggdrasil.Scripting;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// A script that sets up and manages tracks.
	/// </summary>
	public abstract class TrackScript : IScript, IDisposable
	{
		private readonly static object ScriptsSyncLock = new object();
		private readonly static Dictionary<string, TrackScript> Scripts = new Dictionary<string, TrackScript>();
		private readonly static Dictionary<Type, QuestObjective> Objectives = new Dictionary<Type, QuestObjective>();

		/// <summary>
		/// Returns this script's track data.
		/// </summary>
		public TrackData Data { get; } = new TrackData();

		/// <summary>
		/// Returns the id of the track this script created.
		/// </summary>
		public string TrackId => this.Data.Id;

		/// <summary>
		/// Initializes script, creating the track and saving it for
		/// later use.
		/// </summary>
		/// <returns></returns>
		public bool Init()
		{
			this.Load();

			lock (ScriptsSyncLock)
			{
				Scripts[this.Data.Id] = this;
			}

			return true;
		}

		/// <summary>
		/// Returns the track script with the given id via out, returns
		/// false if no script was found.
		/// </summary>
		/// <param name="trackId"></param>
		/// <param name="trackScript"></param>
		/// <returns></returns>
		public static bool TryGet(string trackId, out TrackScript trackScript)
		{
			lock (ScriptsSyncLock)
				return Scripts.TryGetValue(trackId, out trackScript);
		}

		/// <summary>
		/// Cleans up saved tracks and objectives.
		/// </summary>
		public void Dispose()
		{
			// Unload and remove all objectives that were saved for checking
			// whether they were done when even one track script is disposed.
			// The only way track scripts are gonna be disposed is if we
			// reload all of them, so it doesn't matter if we remove them
			// all at once, and this way we don't have to worry about
			// managing which script should unload which objective.
			lock (ScriptsSyncLock)
			{
				if (Objectives.Count != 0)
				{
					foreach (var objective in Objectives.Values)
						objective.Unload();

					Objectives.Clear();
				}

				if (Scripts.Count != 0)
				{
					Scripts.Clear();
				}
			}
		}

		/// <summary>
		/// Called during initialization to set the track's values.
		/// </summary>
		protected abstract void Load();

		/// <summary>
		/// Sets the track's id.
		/// </summary>
		/// <param name="id"></param>
		protected void SetId(string id)
			=> this.Data.Id = id;

		protected void SetPropertyId(string id)
			=> this.Data.PropertyId = id;

		/// <summary>
		/// Sets whether the track can be cancelled.
		/// </summary>
		/// <param name="cancelable"></param>
		protected void SetCancelable(bool cancelable)
			=> this.Data.Cancelable = cancelable;

		/// <summary>
		/// Sets the delay for automatically received tracks, between
		/// meeting the prerequisites and the start of the track.
		/// </summary>
		/// <param name="startDelay"></param>
		protected void SetDelay(TimeSpan startDelay)
			=> this.Data.StartDelay = startDelay;

		/// <summary>
		/// Sets how long the track is held open after it is told to end, so
		/// a closing beat can play before the cast is pulled.
		/// </summary>
		/// <remarks>
		/// A track with a battle box already waits a short delay by default;
		/// this overrides it.
		/// </remarks>
		/// <param name="endDelay"></param>
		protected void SetEndDelay(TimeSpan endDelay)
			=> this.Data.EndDelay = endDelay;

		/// <summary>
		/// Called when a character starts this track.
		/// </summary>
		/// <remarks>
		/// Called before the track is added to the track log, allowing
		/// for changes of its initial progress.
		/// </remarks>
		public virtual IActor[] OnStart(Character character, Track track)
		{
			character.StartLayer();
			if (track.Data.QuestId != 0)
				character.Quests.UpdateQuestStatus(track.Data.QuestId, track.Data.OnStartQuestStatus);
			return Array.Empty<IActor>();
		}

		/// <summary>
		/// Called when a character progresses this track.
		/// </summary>
		/// <remarks>
		/// Called after the track was marked as completed, but before
		/// it's removed from the track log and the rewards were given.
		/// </remarks>
		public virtual async Task OnProgress(Character character, Track track, int frame)
		{
			if (track.PendingDialog != null && !track.PendingDialog.IsCompleted)
				return;

			Send.ZC_NORMAL.SetTrackFrame(character, track.Frame);
			await Task.Yield();
		}

		/// <summary>
		/// Called when a character completes this track successfully.
		/// </summary>
		/// <remarks>
		/// Called after the track was marked as completed.
		/// </remarks>
		public virtual void OnComplete(Character character, Track track)
		{
			if (string.IsNullOrEmpty(this.Data.PropertyId))
				character.SetEtcProperty(track.Id, 1);
			else
				character.SetEtcProperty(this.Data.PropertyId, 1);

			if (track.Data.QuestId != 0)
			{
				if (track.Data.OnCompleteQuestStatus == QuestStatus.Completed)
				{
					character.Quests.Complete(track.Data.QuestId);
				}
				else
				{
					// The cutscene is the quest's objective for the phase it
					// plays through, so mark it done together with the status.
					if (track.Data.OnCompleteQuestStatus == QuestStatus.Success
						&& character.Quests.TryGetById(track.Data.QuestId, out var quest))
						quest.CompleteObjectives();

					character.Quests.UpdateQuestStatus(track.Data.QuestId, track.Data.OnCompleteQuestStatus);
				}
			}

			if (track.HasBattleBoxInLayer)
			{
				Send.ZC_REMOVE_SCROLLLOCKBOX(character);
				track.HasBattleBoxInLayer = false;
			}

			foreach (var actor in track.Actors)
			{
				if (actor != character && actor is IMonster monster)
					character.Map.RemoveMonster(monster);
			}

			character.StopLayer();
		}

		/// <summary>
		/// Called when a character gives up this track.
		/// </summary>
		/// <remarks>
		/// Called after the track was marked as cancelled.
		/// </remarks>
		public virtual void OnCancel(Character character, Track track)
		{
			if (string.IsNullOrEmpty(this.Data.PropertyId))
				character.SetEtcProperty(track.Id, 0);
			else
				character.SetEtcProperty(this.Data.PropertyId, 0);

			if (track.Data.QuestId != 0)
			{
				if (track.Data.OriginalQuestStatus == QuestStatus.Possible && character.Quests.TryGetById(track.Data.QuestId, out var quest))
					character.Quests.Cancel(quest);
				//character.Quests.UpdateQuestStatus(track.Data.QuestId, track.Data.OriginalQuestStatus);
			}

			if (track.Actors != null)
			{
				foreach (var actor in track.Actors)
				{
					if (actor != character && actor is IMonster monster)
						character.Map.RemoveMonster(monster);
				}
			}
			character.StopLayer();
		}

		/// <summary>
		/// Shows a message from the track and waits for the player to
		/// confirm it, returning quietly if the track ends first.
		/// </summary>
		/// <remarks>
		/// A track's dialog is cancelled when the track ends, and closing a
		/// dialog throws by design, so the wait has to tolerate both. The
		/// player's own OnProgress runs from a packet handler, where an
		/// escaping cancellation surfaces as an unhandled exception.
		/// </remarks>
		/// <param name="track"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		protected static async Task ShowDialog(Track track, string message)
		{
			if (track.Dialog == null)
				return;

			try
			{
				await track.Dialog.Msg(message);
			}
			catch (OperationCanceledException)
			{
			}
		}

		/// <summary>
		/// Starts a sequence of messages from the track without holding up
		/// the cutscene, for a closing frame to wait on with WaitForDialog.
		/// </summary>
		/// <remarks>
		/// The client plays on through its own frames while a message is up,
		/// so awaiting one here would let the track end underneath it and
		/// leave the rest of the conversation playing out in the open world.
		/// </remarks>
		/// <param name="track"></param>
		/// <param name="messages"></param>
		protected static void StartDialog(Track track, params string[] messages)
		{
			track.PendingDialog = ShowDialogs(track, messages);
		}

		/// <summary>
		/// Waits for the track's messages to be read, if any are still up.
		/// </summary>
		/// <param name="track"></param>
		/// <returns></returns>
		protected static async Task WaitForDialog(Track track)
		{
			var pendingDialog = track.PendingDialog;

			if (pendingDialog != null)
				await pendingDialog;
		}

		/// <summary>
		/// Shows the given messages one after another.
		/// </summary>
		/// <param name="track"></param>
		/// <param name="messages"></param>
		/// <returns></returns>
		private static async Task ShowDialogs(Track track, string[] messages)
		{
			foreach (var message in messages)
				await ShowDialog(track, message);

			var character = track.Dialog?.Player;

			if (character != null && character.Tracks.ActiveTrack == track)
				Send.ZC_NORMAL.SetTrackFrame(character, track.Frame);
		}

		protected static void CreateBattleBoxInLayer(Character character, Track track)
		{
			track.HasBattleBoxInLayer = true;
			foreach (var actor in track.Actors)
			{
				if (actor.Handle != character.Handle && actor is ICombatEntity combatEntity && character.CanTarget(combatEntity))
				{
					var distance = (float)character.Position.Get2DDistance(actor.Position);
					if (distance > 0)
						distance = (float)Math.Floor(distance / 2 + 150);
					var lPos = new Position(character.Position.X - distance, 0f, character.Position.Z - distance);
					var rPos = new Position(character.Position.X + distance, 0f, character.Position.Z + distance);
					var width = Math.Abs(lPos.X - rPos.X);
					Send.ZC_CREATE_SCROLLLOCKBOX(character, actor, lPos, rPos, width);
				}
			}
		}

		/// <summary>
		/// Set a session object on track characters.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="sessionObjectName"></param>
		/// <param name="value"></param>
		public void SetSessionObject(Character character, string sessionObjectName, int value)
		{
			var layerCharacters = character.Map.GetCharacters(c => c.Layer == character.Layer);

			foreach (var layerCharacter in layerCharacters)
			{
				layerCharacter.SetSessionObject(sessionObjectName, value);
			}
		}

		/// <summary>
		/// Puts the character at the top of the given track actor's hate
		/// list, so it comes for them the moment it can act.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="track"></param>
		/// <param name="actorIndex">Index of the actor in the track's cast.</param>
		/// <param name="hate"></param>
		protected static void InsertTrackHate(Character character, Track track, int actorIndex, int hate = 999)
		{
			if (track.Actors == null || actorIndex < 0 || actorIndex >= track.Actors.Length)
				return;

			if (track.Actors[actorIndex] is ICombatEntity entity)
				entity.InsertHate(character, hate);
		}

		/// <summary>
		/// Removes the given track actor from the map, for the cutscene
		/// commands that kill off an actor mid-track.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="track"></param>
		/// <param name="actorIndex">Index of the actor in the track's cast.</param>
		protected static void RemoveTrackActor(Character character, Track track, int actorIndex)
		{
			if (track.Actors == null || actorIndex < 0 || actorIndex >= track.Actors.Length)
				return;

			if (track.Actors[actorIndex] is IMonster monster && track.Actors[actorIndex] != character)
				character.Map.RemoveMonster(monster);
		}

		/// <summary>
		/// Usually enables aggressive behavior of track monsters
		/// </summary>
		/// <param name="character"></param>
		/// <param name="track"></param>
		protected static void SetTrackTendency(Character character, Track track)
		{
			foreach (var actor in track.Actors)
			{
				if (actor is ICombatEntity combatEntity)
				{
					// Sets the tendency to attack?
					// Can I just add the movement component here instead?
					if (actor is Character)
						continue;
					if (combatEntity is Mob monster)
						monster.Position = monster.SpawnPosition;

					combatEntity.Components.Add(new MovementComponent(combatEntity));
					combatEntity.Tendency = TendencyType.Aggressive;

					if (combatEntity.Components.TryGet<AiComponent>(out var aiComponent))
						aiComponent.Script.RefreshMovement();
				}
			}
		}
	}

	/// <summary>
	/// Used to define which track scripts handle which tracks, based on
	/// a track id.
	/// </summary>
	public class TrackScriptAttribute : Attribute
	{
		/// <summary>
		/// Returns the track id of track script.
		/// </summary>
		public string TrackId { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="trackId"></param>
		public TrackScriptAttribute(string trackId)
		{
			this.TrackId = trackId;
		}
	}
}
