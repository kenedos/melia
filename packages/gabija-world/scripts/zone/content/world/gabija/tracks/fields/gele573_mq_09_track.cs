//--- Melia Script ----------------------------------------------------------
// The Throneweaver at Uzbaiga Hillside
//--- Description -----------------------------------------------------------
// The Master focuses on the Divine Sphere while the swarm closes in.
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

[TrackScript("GELE573_MQ_09_TRACK")]
public class Gele573Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE573_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(75.61, 350.07, -128.19));

		actors.Add(AddTrackActor(character, 57223, 62.45, 350.07, -135.58, 0, new TrackActorSpec { Faction = FactionType.Neutral, Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147373, 66.96, 350.07, -155.85, 0, new TrackActorSpec { Faction = FactionType.Neutral, Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57094, 158.18, 350.08, -410.40, 50, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(131.26, 350.07, -353.28) }));
		actors.Add(AddTrackActor(character, 41280, -50.30, 350.20, -445.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(3.52, 350.07, -317.48) }));
		actors.Add(AddTrackActor(character, 41280, 29.13, 350.08, -472.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(23.58, 350.08, -398.40) }));
		actors.Add(AddTrackActor(character, 41280, 151.72, 350.08, -417.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(112.73, 350.08, -351.48) }));
		actors.Add(AddTrackActor(character, 41280, 65.15, 350.08, -457.24, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(67.16, 350.07, -338.11) }));
		actors.Add(AddTrackActor(character, 41280, -36.42, 350.08, -451.92, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(-19.64, 350.08, -395.59) }));
		actors.Add(AddTrackActor(character, 41280, 55.11, 350.08, -456.79, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(43.53, 350.08, -409.97) }));
		actors.Add(AddTrackActor(character, 41280, 174.92, 350.08, -405.12, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 31, EndPosition = new Position(123.10, 350.08, -313.30) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 44:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
