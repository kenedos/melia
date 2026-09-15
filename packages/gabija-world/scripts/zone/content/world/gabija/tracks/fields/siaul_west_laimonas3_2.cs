//--- Melia Script ----------------------------------------------------------
// The Mushcaria at the Statue
//--- Description -----------------------------------------------------------
// What comes up the road while the statue is still lit.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_WEST_LAIMONAS3_2_TRACK")]
public class SiaulWestLaimonas32Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_LAIMONAS3_2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1676, 283, 341));
		actors.Add(character);

		var escort = new TrackActorSpec { Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 41289, 1413, 285, 423, 0, escort));
		actors.Add(AddTrackActor(character, 400003, 1433, 285, 378, 0, escort));
		actors.Add(AddTrackActor(character, 400003, 1464, 285, 403, 0, escort));
		actors.Add(AddTrackActor(character, 400001, 1441, 285, 392, 0, escort));
		actors.Add(AddTrackActor(character, 400003, 1448, 285, 435, 0, escort));
		actors.Add(AddTrackActor(character, 400001, 1400, 285, 388, 0, escort));
		actors.Add(AddTrackActor(character, 400001, 1429, 285, 448, 0, escort));

		actors.Add(AddTrackActor(character, 41217, 1170, 285, 634, 116, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		actors.Add(AddTrackActor(character, 40110, 1687, 285, 366, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				break;
			case 15:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				break;
			case 19:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
