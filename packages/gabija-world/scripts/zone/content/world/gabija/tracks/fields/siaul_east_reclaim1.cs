//--- Melia Script ----------------------------------------------------------
// The Pokubu at the Bulbes Farm
//--- Description -----------------------------------------------------------
// A pack of Pokubu swarms the player at the farm, then hands the fight over.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_EAST_RECLAIM1_TRACK")]
public class SiaulEastReclaim1Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_RECLAIM1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(158.888f, 151.7621f, 634.6085f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 401341, 216.4544, 130.0327, 132.7971, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401341, 184.7356, 130.0327, 188.8459, 39, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401341, 244.9962, 130.0327, 205.5328, 30, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401341, 387.402, 130.0327, 505.1969, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40120, 233, 157, 724, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				SetTrackTendency(character, track);
				InsertTrackHate(character, track, 1);
				InsertTrackHate(character, track, 2);
				InsertTrackHate(character, track, 3);
				InsertTrackHate(character, track, 4);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
