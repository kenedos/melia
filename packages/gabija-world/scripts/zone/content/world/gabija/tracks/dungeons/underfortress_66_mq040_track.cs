//--- Melia Script ----------------------------------------------------------
// Taking the camp back
//--- Description -----------------------------------------------------------
// The whole detachment walks up on the camp at once, and everything holding
// it is still in it.
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

[TrackScript("UNDERFORTRESS_66_MQ040_TRACK")]
public class Underfortress66Mq040Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_66_MQ040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1547.09f, 143.08f, 370f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 151029, 1803.60, 217.28, 262.44, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 151029, 1802.24, 217.28, 499.58, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 151029, 1887.61, 217.28, 441.21, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 10032, 1577.82, 151.77, 369.74, 47, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard Delus"), EndPosition = new Position(1735.43f, 217.28f, 382.43f) }));
		actors.Add(AddTrackActor(character, 10032, 1531.11, 140.30, 412.81, 58, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1800.76f, 217.28f, 428.87f) }));
		actors.Add(AddTrackActor(character, 10032, 1501.31, 136.58, 403.03, 62, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1759.25f, 217.28f, 444.97f) }));
		actors.Add(AddTrackActor(character, 10032, 1464.72, 136.58, 408.72, 61, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1720.00f, 217.28f, 424.42f) }));
		actors.Add(AddTrackActor(character, 10032, 1536.54, 138.75, 349.97, 58, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1797.58f, 217.28f, 385.14f) }));
		actors.Add(AddTrackActor(character, 10032, 1511.33, 136.58, 334.30, 68, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1775.07f, 217.28f, 343.31f) }));
		actors.Add(AddTrackActor(character, 10032, 1476.11, 136.58, 323.23, 60, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 999, Name = L("Royal Army Guard"), EndPosition = new Position(1745.52f, 217.28f, 331.45f) }));
		actors.Add(AddTrackActor(character, 57956, 1909.61, 217.28, 455.08, 45, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57956, 1899.17, 217.28, 296.58, 32, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57960, 1798.34, 217.28, 529.26, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57956, 1793.92, 217.28, 219.85, 15, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57960, 1866.31, 217.28, 538.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57960, 1996.98, 217.28, 392.06, 14, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1918.42f, 217.28f, 417.14f) }));
		actors.Add(AddTrackActor(character, 57956, 1879.19, 217.28, 395.45, 138, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57960, 1901.10, 217.28, 211.71, 7, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1867.48f, 217.28f, 214.92f) }));
		actors.Add(AddTrackActor(character, 57956, 1925.09, 217.28, 351.03, 17, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Clear the monsters out of the camp!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
