//--- Melia Script ----------------------------------------------------------
// Gesti cornered in the central hall
//--- Description -----------------------------------------------------------
// The altar trap springs, and the Divine Sphere charges.
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

[TrackScript("CHAPLE577_MQ_09_TRACK")]
public class Chaple577Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE577_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(113.50f, 164.86f, -635.30f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147390, 110, 165, -579, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Follower Algis") }));
		actors.Add(AddTrackActor(character, 57055, -29.07, 48.71, -137.31, 3, new TrackActorSpec { Ai = "BasicBoss", Name = L("Demon Queen Gesti") }));
		actors.Add(AddTrackActor(character, 40071, -232.52, 35.92, -57.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-233.40f, 35.92f, -54.00f) }));
		actors.Add(AddTrackActor(character, 40071, -234.81, 35.92, -226.78, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40071, -117.63, 35.92, -311.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40071, 76.77, 35.92, -312.60, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147352, 134, 165, -576, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147373, 112.36, 164.86, -606.13, 4, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 152003, 207.14, 164.86, -582.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 3:
				track.Dialog.SetTitle(L("Follower Algis"));
				track.Dialog.SetPortrait("Dlg_port_algis");
				StartDialog(track, L("The barrier is up and running."),
					L("Please deal with Gesti while I activate the Divine Sphere."));
				break;
			case 32:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
			case 39:
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("The Revelator has come here alone."),
					L("Exactly what I wanted."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
