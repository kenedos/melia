//--- Melia Script ----------------------------------------------------------
// Riteris at the Karuna Altar
//--- Description -----------------------------------------------------------
// Naktis' servant comes for the altar the moment its key is gone.
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

[TrackScript("CHATHEDRAL54_SQ05_PART2_TRACK")]
public class Cathedral54Sq05Part2Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL54_SQ05_PART2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(825.28f, 13.98f, 1183.32f));

		actors.Add(AddTrackActor(character, 47254, 826.33, 13.98, 1200.75, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Karuna Altar") }));
		actors.Add(AddTrackActor(character, 57311, 820.87, 13.98, 1240.55, 38, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, 813.67, 3.09, 996.90, 37, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(804.65f, 3.09f, 1091.72f) }));
		actors.Add(AddTrackActor(character, 57367, 840.80, 3.09, 963.64, 35, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(849.13f, 3.09f, 1054.65f) }));
		actors.Add(AddTrackActor(character, 57367, 460.58, 3.09, 1205.44, 53, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(607.40f, 3.09f, 1191.73f) }));
		actors.Add(AddTrackActor(character, 20024, 822.60, 13.98, 1220.21, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, 829.03, 3.10, 1401.22, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		// The arming frame sits past the cutscene's last reported one.
		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				character.ServerMessage(L("Naktis' servants have rushed in!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
