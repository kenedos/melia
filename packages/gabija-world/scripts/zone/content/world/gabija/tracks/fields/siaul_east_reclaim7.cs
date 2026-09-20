//--- Melia Script ----------------------------------------------------------
// The Weaver by the Lower Stream
//--- Description -----------------------------------------------------------
// Weaver swarm out from the water below the supply depot.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_EAST_RECLAIM7_TRACK")]
public class SiaulEastReclaim7Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_RECLAIM7_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(470.5879f, 130.0327f, -867.5151f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 41280, 432.6587, 143.9776, -773.5017, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41280, 401.2621, 143.9776, -793.3062, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41280, 492.1505, 130.0327, -801.9854, 54, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41280, 424.5429, 143.9776, -831.7863, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41280, 444.0817, 130.0327, -916.475, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(458.43521f, 130.03270f, -891.92999f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 5:
				SetTrackTendency(character, track);
				break;
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
