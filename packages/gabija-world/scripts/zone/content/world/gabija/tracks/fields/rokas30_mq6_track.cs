//--- Melia Script ----------------------------------------------------------
// The Tzedej Altar
//--- Description -----------------------------------------------------------
// The altar cracks and the Hogma statues around it come out of the stone.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ROKAS30_MQ6_TRACK")]
public class Rokas30Mq6Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_MQ6_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47102, -310, 215, -250, 7, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Tzedej Altar") }));
		actors.Add(AddTrackActor(character, 147315, -329.39, 215.75, -279.95, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147318, -366, 215, -218, 1, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147317, -356.63, 215.75, -186.89, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147318, -302.71, 215.75, -203.75, 42, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147316, -395.33, 215.75, -186.79, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 41441, -338.28, 215.75, -279.99, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41435, -356, 215, -186, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47308, -302, 215, -203, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47308, -366, 215, -218, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41434, -395, 215, -186, 105, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 39:
				// The altar and the five statues break apart before the Hogma inside them are armed.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
