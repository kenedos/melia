//--- Melia Script ----------------------------------------------------------
// The Wild Carnivore at Pasiulyma Field
//--- Description -----------------------------------------------------------
// A beast soaked in demonic energy, and the vines that answer for it.
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

[TrackScript("GELE572_MQ_07_TRACK")]
public class Gele572Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE572_MQ_07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1012.64, 462.12, 1643.34));

		actors.Add(AddTrackActor(character, 41238, 1014.73, 462.13, 1678.66, 0, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 47310, 1012.51, 462.12, 1661.84, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47310, 1014.13, 462.12, 1675.01, 11, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47310, 1012.81, 462.12, 1668.66, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 30:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				break;
			case 44:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
