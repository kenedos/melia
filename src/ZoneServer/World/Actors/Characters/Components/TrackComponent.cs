using System;
using System.Threading.Tasks;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using Melia.Shared.Util;

namespace Melia.Zone.World.Actors.Characters.Components
{
	public class TrackComponent : CharacterComponent
	{
		public Track ActiveTrack { get; private set; }

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
			if (!string.IsNullOrEmpty(overrideTrackProperty) && this.Character.Etc.Properties.GetFloat(overrideTrackProperty) == 1)
				return false;
			if (string.IsNullOrEmpty(overrideTrackProperty) && this.Character.Etc.Properties.GetFloat(trackId) == 1)
				return false;

			// The delay is what separates accepting a quest from its cutscene,
			// so it has to run before the cutscene is sent, not after.
			if (startDelay > TimeSpan.Zero)
			{
				await GameClock.Delay(startDelay);

				if (this.ActiveTrack != null)
					return false;
			}

			var track = Track.Create(trackId);

			track.Status = TrackStatus.Started;
			track.Data.StartDelay = startDelay;
			track.Data.QuestId = questId;
			track.Data.OnStartQuestStatus = onStart;
			track.Data.OnCompleteQuestStatus = onComplete;
			track.Data.PropertyId = string.IsNullOrEmpty(overrideTrackProperty) ? trackId : overrideTrackProperty;
			track.Dialog = new Dialog(this.Character, null);

			this.ActiveTrack = track;

			IActor[] actors;
			if (TrackScript.TryGet(track.Id, out var trackScript))
				actors = trackScript.OnStart(this.Character, this.ActiveTrack);
			else
				actors = Array.Empty<IActor>();
			track.Actors = actors;

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

			if (TrackScript.TryGet(trackId, out var trackScript))
				trackScript.OnComplete(this.Character, track);

			this.TrackCompleted?.Invoke(this.Character, track);

			// Clean up the track dialog to prevent blocking future NPC interactions
			if (track.Dialog != null)
			{
				track.Dialog.State = DialogState.Ended;
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
	}
}
