//--- Melia Script ----------------------------------------------------------
// Opening the Special Reading Room
//--- Description -----------------------------------------------------------
// Monk Goss works on the demons' seal while apparitions close in.
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

[TrackScript("ABBAY_64_1_MQ040_TRACK")]
public class Abbay641Mq040Track : TrackScript
{
	// How long Goss needs for the seal is not in the client data.
	private const int SealSeconds = 60;

	protected override void Load()
	{
		SetId("ABBAY_64_1_MQ040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-337.10f, 208.49f, -1895.31f));

		actors.Add(AddTrackActor(character, 155046, -411, 209.54, -1947, 4, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Monk Goss"), EndPosition = new Position(-421.99f, 209.84f, -1981.23f) }));
		actors.Add(AddTrackActor(character, 153119, -379, 209.52, -1989, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose"), EndPosition = new Position(-396.47f, 209.60f, -1984.68f) }));
		actors.Add(AddTrackActor(character, 153111, -552, 210.31, -2071, 29, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rona"), EndPosition = new Position(-504.50f, 210.31f, -2037.18f) }));
		actors.Add(AddTrackActor(character, 20063, -463, 210.31, -2035, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kornas") }));
		actors.Add(AddTrackActor(character, 20061, -457.35, 210.31, -2134.68, 23, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Anne"), EndPosition = new Position(-452.81f, 210.31f, -2078.81f) }));
		actors.Add(AddTrackActor(character, 20064, -533.77, 209.90, -2152.81, 45, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zacaras"), EndPosition = new Position(-474.93f, 210.31f, -2090.22f) }));
		actors.Add(AddTrackActor(character, 153110, -517, 210.31, -2008, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Allonas") }));
		actors.Add(AddTrackActor(character, 153109, -618.13, 210.31, -2056.21, 25, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Litas"), EndPosition = new Position(-530.74f, 210.31f, -2035.32f) }));
		actors.Add(AddTrackActor(character, 153117, -431, 210.03, -1992, 21, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 153028, -428.67, 215.50, -2000.60, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 103025, -198.27, 209.91, -1683.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-248.97f, 208.48f, -1741.54f) }));
		actors.Add(AddTrackActor(character, 103025, -145.18, 209.91, -1747.94, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-171.97f, 208.48f, -1774.67f) }));
		actors.Add(AddTrackActor(character, 103025, -160.35, 209.91, -1643.56, 30, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-196.51f, 208.48f, -1728.29f) }));
		actors.Add(AddTrackActor(character, 103025, -274.47, 209.91, -1617.66, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-289.54f, 208.48f, -1689.99f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the apparitions that come for Goss while he works on the
	/// seal, and opens the door once he has held out long enough.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("ST01")
			.Monster(103025, -170.68, 208.48, -1673.15, -9, count: 3)
			.Monster(103025, -127.51, 208.48, -1688.27, -49, count: 3)
			.Monster(103025, -168.84, 209.91, -1653.01, -37, count: 3)
			.On(s => s.Elapsed >= 20, s => s.Game.StartStage("ST02"), 1)
			.On(s => s.Elapsed >= SealSeconds, s => { s.Game.ClearStage("ST01"); s.Game.ClearStage("ST02"); s.Game.CompleteObjective(50120, "protectGoss"); }, 1);

		game.Stage("ST02")
			.Monster(103025, -251.67, 209.91, -1660.08, -36, count: 2)
			.Monster(103025, -114.74, 208.48, -1728.10, -23, count: 2)
			.Monster(57674, -158.85, 209.91, -1660.67, -36, count: 2)
			.Monster(57674, -175.09, 209.91, -1700.60, -13, count: 2);

		game.Start("ST01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 42:
				StartMinigame(character, track);
				break;

			case 44:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
