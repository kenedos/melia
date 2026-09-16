using System;
using System.Threading.Tasks;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using Melia.Zone.World.Actors.Monsters;
using Melia.Shared.Util;

namespace Melia.Zone.World.Actors.Characters.Components
{
	public class TrackComponent : CharacterComponent
	{
		private readonly static TimeSpan DialogTimeout = TimeSpan.FromMinutes(2);
		private readonly static TimeSpan DialogPollInterval = TimeSpan.FromMilliseconds(100);
		private readonly static TimeSpan BattleEndDelay = TimeSpan.FromSeconds(2);

		public Track ActiveTrack { get; private set; }

		private bool _disposed;
		private int _trackLayer;
		private int _returnLayer;
		private Track _endingTrack;

		/// <summary>
		/// Raised when the character starts a track.
		/// </summary>
		public event Action<Character, Track> TrackStarted;

		/// <summary>
		/// Raised when the character completes a track.
		/// </summary>
		public event Action<Character, Track> TrackCompleted;

		public TrackComponent(Character character) : base(character)
		{
		}

		/// <summary>
		/// Start a track.
		/// </summary>
		/// <param name="trackId"></param>
		/// <returns></returns>
		public async Task<bool> Start(string trackId, TimeSpan startDelay, string propertyId = "")
		{
			return await this.Start(trackId, startDelay, 0, QuestStatus.Possible, QuestStatus.Possible, propertyId);
		}

		/// <summary>
		/// Start a track using quest track data.
		/// </summary>
		/// <param name="questTrackData"></param>
		/// <returns></returns>
		public async Task<bool> Start(QuestTrackData questTrackData, string overrideTrackProperty = "")
		{
			return await this.Start(questTrackData.TrackName, questTrackData.StartDelay, questTrackData.QuestId, questTrackData.OnTrackStart, questTrackData.OnTrackEnd, overrideTrackProperty);
		}

		/// <summary>
		/// Start a track for a specific quest.
		/// </summary>
		/// <param name="trackId"></param>
		/// <returns></returns>
		public async Task<bool> Start(string trackId, TimeSpan startDelay, int questId, QuestStatus onStart, QuestStatus onComplete, string overrideTrackProperty = "")
		{
			if (!this.Character.EyesOpen)
				return false;
			if (this.ActiveTrack != null)
				return false;
			if (this._endingTrack != null)
				return false;
			if (_disposed)
				return false;
			if (!string.IsNullOrEmpty(overrideTrackProperty) && this.Character.Etc.Properties.GetFloat(overrideTrackProperty) == 1)
				return false;
			if (string.IsNullOrEmpty(overrideTrackProperty) && this.Character.Etc.Properties.GetFloat(trackId) == 1)
				return false;

			// The delay is what separates accepting a quest from its cutscene,
			// so it has to run before the cutscene is sent, not after.
			if (startDelay > TimeSpan.Zero)
			{
				await GameClock.Delay(startDelay);

				if (_disposed || this.ActiveTrack != null || this._endingTrack != null)
					return false;
			}

			// A dialog can be opened while the delay runs - talking to the
			// quest giver once more is enough - and a track builds a dialog
			// of its own, which throws while another is active. Hold the
			// cutscene until the player is out of it.
			if (!await this.WaitForDialogClose())
				return false;

			var track = Track.Create(trackId);

			track.Status = TrackStatus.Started;
			track.Data.StartDelay = startDelay;
			track.Data.QuestId = questId;
			track.Data.OnStartQuestStatus = onStart;
			track.Data.OnCompleteQuestStatus = onComplete;
			track.Data.PropertyId = string.IsNullOrEmpty(overrideTrackProperty) ? trackId : overrideTrackProperty;

			// Remember the status the quest was in before the track touched
			// it, so cancelling the track only drops a quest that hadn't been
			// accepted yet.
			track.Data.OriginalQuestStatus = QuestStatus.Possible;
			if (questId != 0 && this.Character.Quests.TryGetById(questId, out var quest))
				track.Data.OriginalQuestStatus = quest.Status;

			track.Dialog = new Dialog(this.Character, null);

			this.ActiveTrack = track;

			var returnLayer = this.Character.Layer;
			IActor[] actors;
			if (TrackScript.TryGet(track.Id, out var trackScript))
				actors = trackScript.OnStart(this.Character, this.ActiveTrack);
			else
				actors = Array.Empty<IActor>();
			track.Actors = actors;

			// The track builds its own layer in OnStart; remember it and
			// the layer to hand the character back to, so a disconnect can
			// destroy the track's layer without touching anything shared.
			this._returnLayer = returnLayer;
			this._trackLayer = this.Character.Layer;

			// The cutscene addresses its cast by handle, so the client has to
			// have been told about every one of them before it starts.
			this.Character.LookAround();

			Send.ZC_NORMAL.SetupCutscene(this.Character, true, false, true);
			Send.ZC_NORMAL.LoadCutscene(this.Character, 0x77, true, track.Id);
			Send.ZC_NORMAL.LoadCutscene(this.Character, 0x6B, true, this.Character.Name);
			Send.ZC_NORMAL.StartCutscene(this.Character, track.Id, actors);

			this.TrackStarted?.Invoke(this.Character, this.ActiveTrack);

			return true;
		}

		/// <summary>
		/// Waits until the character is out of any open dialog, so a track
		/// can build the dialog it speaks through.
		/// </summary>
		/// <remarks>
		/// The dialog that accepted the quest is already waited out by
		/// QuestComponent.BeginTrack, but a start delay reopens the window
		/// and a second conversation can be running by the time it ends.
		/// </remarks>
		/// <returns>False if a track became active while waiting.</returns>
		private async Task<bool> WaitForDialogClose()
		{
			while (this.Character.Connection.CurrentDialog != null)
			{
				await GameClock.Delay(DialogPollInterval);

				if (_disposed || this.ActiveTrack != null || this._endingTrack != null)
					return false;
			}

			return true;
		}

		/// <summary>
		/// Progress through a track
		/// </summary>
		/// <param name="trackId"></param>
		/// <param name="frame"></param>
		/// <returns></returns>
		public async Task Progress(string trackId, int frame)
		{
			if (this.ActiveTrack == null || this.ActiveTrack.Id != trackId)
				return;

			if (TrackScript.TryGet(this.ActiveTrack.Data.Id, out var trackScript))
			{
				this.ActiveTrack.Frame = frame;
				await trackScript.OnProgress(this.Character, this.ActiveTrack, frame);
			}
		}

		/// <summary>
		/// End a track.
		/// </summary>
		/// <param name="trackId"></param>
		public void End(string trackId)
		{
			if (this.ActiveTrack == null || this.ActiveTrack.Id != trackId)
				return;

			// Detached before OnComplete runs, so the quest status it sets
			// cannot come back around and end the same track again.
			var track = this.ActiveTrack;
			this.ActiveTrack = null;

			var pendingDialog = track.PendingDialog;

			if (pendingDialog != null && !pendingDialog.IsCompleted)
			{
				_ = this.CompleteAfterDialog(track, pendingDialog);
				return;
			}

			// A track that ran into a fight is held open a moment past the
			// last kill, so the client can finish the death animation before
			// the cast is pulled out from under it. A track can override the
			// delay with SetEndDelay.
			var endDelay = track.Data.EndDelay;
			if (endDelay == TimeSpan.Zero && track.HasBattleBoxInLayer)
				endDelay = BattleEndDelay;

			if (endDelay > TimeSpan.Zero)
			{
				_ = this.CompleteAfterDelay(track, endDelay);
				return;
			}

			this.Complete(track);
		}

		/// <summary>
		/// Waits for the track's conversation to be read before completing
		/// the track.
		/// </summary>
		/// <param name="track"></param>
		/// <param name="pendingDialog"></param>
		/// <returns></returns>
		private async Task CompleteAfterDialog(Track track, Task pendingDialog)
		{
			this._endingTrack = track;

			await Task.WhenAny(pendingDialog, Task.Delay(DialogTimeout));

			if (this._endingTrack != track)
				return;

			this._endingTrack = null;

			if (_disposed || this.Character.Map == null)
				return;

			this.Complete(track);
		}

		/// <summary>
		/// Holds the track open for the given delay before completing it.
		/// </summary>
		/// <param name="track"></param>
		/// <param name="delay"></param>
		/// <returns></returns>
		private async Task CompleteAfterDelay(Track track, TimeSpan delay)
		{
			this._endingTrack = track;

			await GameClock.Delay(delay);

			if (this._endingTrack != track)
				return;

			this._endingTrack = null;

			if (_disposed || this.Character.Map == null)
				return;

			this.Complete(track);
		}

		/// <summary>
		/// Completes the given track and cleans up after it.
		/// </summary>
		/// <param name="track"></param>
		private void Complete(Track track)
		{
			if (TrackScript.TryGet(track.Id, out var trackScript))
				trackScript.OnComplete(this.Character, track);

			// OnComplete stops the track's layer, which makes the client
			// hide the tracker; re-show it now that the quest state it
			// carries is final.
			this.Character.Quests.RefreshChase();

			this.TrackCompleted?.Invoke(this.Character, track);

			// Clean up the track dialog to prevent blocking future NPC interactions
			if (track.Dialog != null)
			{
				track.Dialog.State = DialogState.Ended;
				track.Dialog.Cancel();
				this.Character.Connection.CurrentDialog?.Cancel();
				this.Character.Connection.CurrentDialog = null;
			}
		}

		/// <summary>
		/// Cancel a track.
		/// </summary>
		public void Cancel()
		{
			if (this.ActiveTrack == null)
				return;

			var track = this.ActiveTrack;
			this.ActiveTrack = null;

			if (TrackScript.TryGet(track.Id, out var trackScript))
				trackScript.OnCancel(this.Character, track);

			// Clean up the track dialog to prevent blocking future NPC interactions
			if (track.Dialog != null)
			{
				track.Dialog.State = DialogState.Ended;
				this.Character.Connection.CurrentDialog?.Cancel();
				this.Character.Connection.CurrentDialog = null;
			}
		}

		/// <summary>
		/// Tears down the character's track and the actors it spawned on
		/// its private layer, for a disconnect that never reaches End or
		/// Cancel and so would otherwise leave the cast behind.
		/// </summary>
		public void Cleanup()
		{
			_disposed = true;

			var track = this.ActiveTrack;
			this.ActiveTrack = null;

			track ??= this._endingTrack;
			this._endingTrack = null;

			if (track == null)
				return;

			if (track.Dialog != null)
			{
				track.Dialog.State = DialogState.Ended;
				track.Dialog.Cancel();

				var connection = this.Character.Connection;
				if (connection != null)
				{
					connection.CurrentDialog?.Cancel();
					connection.CurrentDialog = null;
				}
			}

			var map = this.Character.Map;

			// The track created its own layer, so everything left on it
			// belongs to the cutscene and can go. A track that did not move
			// the character - a dungeon's shared party layer - only loses
			// its own cast, never the layer itself.
			if (this._trackLayer != this._returnLayer)
			{
				if (map != null)
				{
					map.RemoveEntitiesOnLayer(this._trackLayer);
					this.Character.SetLayer(this._returnLayer, enabled: false);
				}
			}
			else if (map != null && track.Actors != null)
			{
				foreach (var actor in track.Actors)
				{
					if (actor != this.Character && actor is IMonster monster)
						map.RemoveMonster(monster);
				}
			}
		}
	}
}
