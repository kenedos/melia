//--- Melia Script ----------------------------------------------------------
// Track Command
//--- Description -----------------------------------------------------------
// GM command that plays a track by its name, for debugging a cutscene
// without playing the quest that normally starts it.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

public class TrackCommandScript : GeneralScript
{
	protected override void Load()
	{
		AddChatCommand("track", "<track name|stop>", "Plays a track, replaying it if already seen.", 50, 50, HandleTrack);
	}

	private CommandResult HandleTrack(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (args.Count < 1)
			return CommandResult.InvalidArgument;

		var trackName = args.Get(0);

		if (trackName.Equals("stop", StringComparison.OrdinalIgnoreCase))
		{
			var activeTrack = target.Tracks.ActiveTrack;
			if (activeTrack == null)
			{
				sender.ServerMessage(L("No track is playing."));
				return CommandResult.Okay;
			}

			target.Tracks.Cancel();
			sender.ServerMessage(L("Cancelled track '{0}'."), activeTrack.Id);
			return CommandResult.Okay;
		}

		if (!TrackScript.TryGet(trackName, out var trackScript))
		{
			sender.ServerMessage(L("Track '{0}' not found."), trackName);
			return CommandResult.Okay;
		}

		if (target.Tracks.ActiveTrack != null)
		{
			sender.ServerMessage(L("Track '{0}' is already playing, use '{1} stop' first."), target.Tracks.ActiveTrack.Id, commandName);
			return CommandResult.Okay;
		}

		var propertyName = !string.IsNullOrEmpty(trackScript.Data.PropertyId) ? trackScript.Data.PropertyId : trackScript.TrackId;
		target.SetEtcProperty(propertyName, 0);

		_ = this.PlayTrack(sender, target, trackScript.TrackId);

		return CommandResult.Okay;
	}

	private async Task PlayTrack(Character sender, Character target, string trackId)
	{
		var started = await target.Tracks.Start(trackId, TimeSpan.Zero);

		if (started)
			sender.ServerMessage(L("Playing track '{0}'."), trackId);
		else
			sender.ServerMessage(L("Track '{0}' didn't start: close any dialog and make sure you're not inside another track's layer."), trackId);
	}
}
