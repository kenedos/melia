//--- Melia Script ----------------------------------------------------------
// The hidden treasure chest
//--- Description -----------------------------------------------------------
// Medakia come up out of the floor around the chest.
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

[TrackScript("ZACHA5F_EQ_03_TRACK")]
public class Zacha5fEq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_EQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1536.47f, 332.08f, -2620.76f));

		actors.Add(AddTrackActor(character, 400821, -1716.95, 332.08, -2760.41, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1742.29, 332.08, -2651.75, 221, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1626.37, 332.08, -2507.53, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1631.90, 332.09, -2591.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1592.27, 332.08, -2704.90, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1694.03, 332.08, -2607.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -1750.89, 332.09, -2532.85, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40035, -1517.85, 334.13, -2627.59, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20024, -1626.38, 332.09, -2507.54, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1742.29, 332.09, -2651.76, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1716.95, 332.09, -2760.40, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1631.90, 332.08, -2591.34, 203, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1592.28, 332.09, -2704.89, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1694.04, 332.09, -2607.64, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, -1750.88, 332.08, -2532.85, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				// The ground markers the Medakia rise out of are spent.
				for (var i = 9; i <= 15; i++)
					RemoveTrackActor(character, track, i);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
