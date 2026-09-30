//--- Melia Script ----------------------------------------------------------
// The first transport circle
//--- Description -----------------------------------------------------------
// Grita reads the circle while the floor's monsters close in.
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

[TrackScript("FTOWER41_MQ_02_TRACK")]
public class Ftower41Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER41_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1472.34f, 1552.73f, -1384.10f));

		actors.Add(AddTrackActor(character, 147449, -1605.86, 1552.73, -1465.50, 28, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Grita"), EndPosition = new Position(-1610.51f, 1552.73f, -1426.80f) }));
		actors.Add(AddTrackActor(character, 147500, -1611.45, 1552.73, -1402.07, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("1st Transport Magic Circle") }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Sends four waves at the player, each once the last one is down.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("1st")
			.Monster(47397, -1783.47, 1552.73, -1386.01)
			.Monster(47397, -1445.27, 1552.73, -1424.37, -173)
			.Monster(47397, -1607.88, 1552.73, -1262.93, -81)
			.Monster(47397, -1592.18, 1552.73, -1551.68, 86)
			.On(s => s.Alive() <= 0, s =>
			{
				s.Game.ClearStage("1st");
				s.Game.StartStage("2st");
			}, 1);

		game.Stage("2st")
			.Monster(401621, -1758.22, 1552.73, -1393.37)
			.Monster(401621, -1482.53, 1552.73, -1415.56, -172)
			.On(s => s.Alive() <= 0, s =>
			{
				s.Game.ClearStage("2st");
				s.Game.StartStage("3rd");
			}, 1);

		game.Stage("3rd")
			.Monster(47397, -1599.59, 1552.73, -1537.78, 94)
			.Monster(47397, -1601.21, 1552.73, -1294.33, -77)
			.On(s => s.Alive() <= 0, s =>
			{
				s.Game.ClearStage("3rd");
				s.Game.StartStage("4th");
			}, 1);

		game.Stage("4th")
			.Monster(401621, -1594.50, 1552.73, -1537.84, 96)
			.Monster(401621, -1623.73, 1552.73, -1305.95, -77)
			.Monster(401621, -1736.54, 1552.73, -1410.83, 3)
			.Monster(401621, -1486.81, 1552.73, -1414.60, 175)
			.Monster(401621, -1544.74, 1552.73, -1474.61, 136)
			.Monster(401621, -1699.50, 1552.73, -1343.01, -35);

		game.Start("1st");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				StartMinigame(character, track);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat the monsters interrupting Grita while she reads the magic circle!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
