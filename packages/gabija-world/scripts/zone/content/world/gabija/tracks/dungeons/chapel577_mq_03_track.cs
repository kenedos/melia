//--- Melia Script ----------------------------------------------------------
// The Sventove Central Altar
//--- Description -----------------------------------------------------------
// Gesti reaches the altar before you do.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHAPLE577_MQ_03_TRACK")]
public class Chaple577Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE577_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(119.08f, 164.87f, -600.97f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147358, -27, 38, -137, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Sventove Central Altar") }));
		actors.Add(AddTrackActor(character, 147390, 110, 165, -579, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Follower Algis") }));
		actors.Add(AddTrackActor(character, 147352, 134, 164, -576, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147371, -129.44, 35.92, -139.33, 31, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-73.39f, 48.71f, -143.65f) }));
		actors.Add(AddTrackActor(character, 152003, 207.33, 164.86, -582.63, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 1);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 12:
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("I can feel Laima's power."));
				break;
			case 42:
				RemoveTrackActor(character, track, 1);
				break;
			case 45:
				StartDialog(track, L("This is the Seal of Space..."));
				break;
			case 53:
				track.Dialog.SetTitle(L("Follower Algis"));
				track.Dialog.SetPortrait("Dlg_port_algis");
				StartDialog(track, L("Gesti has sensed it. Stay calm. It is not yet time."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
