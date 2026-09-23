//--- Melia Script ----------------------------------------------------------
// Holding the Sealed Tower
//--- Description -----------------------------------------------------------
// The demons start on the tower itself, and it has to stand through it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("PARTY_Q_100_TRACK")]
public class PartyQ100Track : TrackScript
{
	protected override void Load()
	{
		SetId("PARTY_Q_100_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147414, 1079, -73, 4705, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, MaxHp = 200, Name = L("Seal Tower") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the waves of monsters that lay siege to the Seal Tower.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("STAGE01")
			.Monster(400324, 1079.44, 3.44, 4483.92, 95, count: 3, respawnSeconds: 19)
			.Monster(400324, 1006.55, 0.72, 4438.76, 86, count: 3, respawnSeconds: 19)
			.Monster(400324, 1035.24, 1.97, 4483.96, 92, count: 3, respawnSeconds: 19)
			.Monster(400324, 990.38, 0.72, 4491.3, 73, count: 3, respawnSeconds: 19)
			.Monster(57202, 1074.99, 3.02, 4447.92, 102, count: 2, respawnSeconds: 19)
			.Monster(57202, 967, 0.72, 4454.71, 92, count: 2, respawnSeconds: 19)
			.On(s => s.Elapsed >= 60, s => { s.Game.ClearStage("STAGE01"); s.Game.StartStage("STAGE02"); }, 1);

		game.Stage("STAGE02")
			.Monster(57202, 1347.78, -73.23, 4859.02, 177, count: 3, respawnSeconds: 20)
			.Monster(57202, 1346.08, -73.23, 4804.39, -166, count: 3, respawnSeconds: 20)
			.Monster(400324, 1019.77, 1.64, 4507.54, 105, count: 3, respawnSeconds: 20)
			.Monster(400324, 1062.2, 2.92, 4490.8, 93, count: 3, respawnSeconds: 20)
			.Monster(57203, 1378.58, -73.23, 4830.8, -152, count: 3, respawnSeconds: 20)
			.Monster(57202, 1044, 2.1, 4462.99, 110, count: 3, respawnSeconds: 20)
			.On(s => s.Elapsed >= 70, s => { s.Game.ClearStage("STAGE02"); s.Game.StartStage("STAGE03"); });

		game.Stage("STAGE03")
			.Monster(41442, 1038.94, 2.29, 4510.3, 106, count: 3, respawnSeconds: 18)
			.Monster(41442, 998.81, 0.93, 4506.88, 73, count: 3, respawnSeconds: 18)
			.Monster(41442, 1072.43, 3.21, 4484.73, 71, count: 3, respawnSeconds: 18)
			.Monster(57205, 1368.77, -73.23, 4832.71, 166, count: 2, respawnSeconds: 18)
			.Monster(57203, 1404.1, -73.23, 4798.33, 176, count: 3, respawnSeconds: 18)
			.Monster(57202, 1066.06, 2.74, 4450.49, 94, count: 2, respawnSeconds: 18)
			.Monster(57203, 1416.55, -73.23, 4853.94, 177, count: 3, respawnSeconds: 18)
			.Monster(57202, 1006.08, 0.89, 4469.68, 91, count: 2, respawnSeconds: 18)
			.On(s => s.Elapsed >= 60, s => { s.Game.ClearStage("STAGE03"); s.Game.StartStage("STAGE04"); });

		game.Stage("STAGE04")
			.Monster(41442, 1077.64, -4.47, 4521.7, 98)
			.Monster(41442, 1015.23, 1.43, 4500.78, 89)
			.Monster(41442, 1041.74, -1.08, 4518.36, 104)
			.Monster(57205, 1068.73, 2.91, 4460.46, 101, count: 5, respawnSeconds: 20)
			.Monster(57205, 1001.74, 0.78, 4473.46, 99, count: 5, respawnSeconds: 20)
			.On(s => s.Elapsed >= 130, s => { s.Game.ClearStage("STAGE04"); s.Game.StartStage("STAGE06"); })
			.On(s => s.Elapsed >= 30, s => s.Game.StartStage("STAGE05"), 1)
			.On(s => s.Elapsed >= 20 && s.Alive(0) <= 0, s => s.Spawn(0, 1), 4)
			.On(s => s.Elapsed >= 20 && s.Alive(1) <= 0, s => s.Spawn(1, 1), 4)
			.On(s => s.Elapsed >= 20 && s.Alive(2) <= 0, s => s.Spawn(2, 1), 4);

		game.Stage("STAGE05")
			.Monster(400324, 1325.24, -73.23, 4872.02, 166, count: 4, respawnSeconds: 20)
			.Monster(400324, 1317.35, -73.23, 4818.99, -175, count: 4, respawnSeconds: 20)
			.Monster(57202, 1360.55, -73.23, 4786.24, -167, count: 4, respawnSeconds: 20)
			.Monster(57202, 1366.7, -73.23, 4877.19, -162, count: 4, respawnSeconds: 20)
			.On(s => s.Elapsed >= 100, s => s.Game.ClearStage("STAGE05"));

		game.Stage("STAGE06")
			.Monster(41258, 1397.04, -73.23, 4876.04, 178, count: 4)
			.Monster(41258, 1356.94, -73.23, 4847.96, 165, count: 4)
			.Monster(41258, 1379.64, -73.23, 4787.17, 162, count: 4)
			.Monster(57208, 1010.79, 1.4, 4515.52, 113, count: 5, respawnSeconds: 20)
			.Monster(57208, 1076.21, -1.31, 4514.42, 116, count: 5, respawnSeconds: 20)
			.Monster(57219, 1039.63, 1.98, 4465.44, 76, count: 5)
			.Monster(57205, 1440.78, -73.23, 4858.95, -165, count: 5, respawnSeconds: 20)
			.Monster(57205, 1434, -73.23, 4804, 176, count: 5, respawnSeconds: 20)
			.On(s => s.Elapsed >= 120, s => { s.Game.ClearStage("STAGE06"); s.Game.StartStage("STAGE07"); })
			.On(s => s.Elapsed >= 20 && s.Alive(0) <= 0, s => s.Spawn(0, 1), 4)
			.On(s => s.Elapsed >= 20 && s.Alive(1) <= 0, s => s.Spawn(1, 1), 4)
			.On(s => s.Elapsed >= 20 && s.Alive(2) <= 0, s => s.Spawn(2, 1), 4)
			.On(s => s.Alive(5) <= 0 && s.Elapsed >= 20, s => s.Spawn(5, 1), 5);

		game.Stage("STAGE07")
			.Monster(57202, 1396.89, -73.23, 4830.89, -152, count: 4, respawnSeconds: 20)
			.Monster(57205, 1320.1, -73.23, 4833.01, -160, count: 4, respawnSeconds: 20)
			.Monster(57205, 1365.35, -73.23, 4876.03, -172, count: 4, respawnSeconds: 20)
			.Monster(57205, 1373.42, -73.23, 4779.78, -154, count: 4, respawnSeconds: 20)
			.Monster(57203, 1042.79, 2.32, 4496.46, 65, count: 4, respawnSeconds: 20)
			.Monster(57202, 1042.01, 1.97, 4454.62, 87, count: 4, respawnSeconds: 20)
			.Monster(41442, 996.14, 0.83, 4504.6, 78, count: 3)
			.Monster(41442, 1077.54, 3.28, 4471.42, 75, count: 3)
			.Monster(41442, 1033.27, 1.38, 4414.32, 68, count: 3)
			.On(s => s.Elapsed >= 100, s => { s.Game.ClearStage("STAGE07"); s.Game.StartStage("STAGE08"); })
			.On(s => s.Alive(6) <= 0, s => s.Spawn(6, 1), 3)
			.On(s => s.Alive(7) <= 0, s => s.Spawn(7, 1), 3)
			.On(s => s.Alive(8) > 0, s => s.Spawn(8, 1), 3);

		game.Stage("STAGE08")
			.Monster(41252, 1025.11, 1.8, 4505.71, 73, count: 4, respawnSeconds: 15)
			.Monster(41252, 1101, -0.24, 4508.65, 109, count: 4, respawnSeconds: 15)
			.Monster(41252, 1314.13, -73.23, 4852.8, -165, count: 4, respawnSeconds: 15)
			.Monster(41252, 1312.55, -73.23, 4804.1, -164, count: 4, respawnSeconds: 15)
			.On(s => s.Elapsed >= 30, s => s.Game.StartStage("STAGE09"));

		game.Stage("STAGE09")
			.Monster(57205, 1366.16, -73.23, 4768.29, -179, count: 3, respawnSeconds: 20)
			.Monster(57205, 1326.93, -73.23, 4892.39, 144, count: 3, respawnSeconds: 20)
			.Monster(57205, 1309.67, -73.23, 4801.14, -154, count: 3, respawnSeconds: 20)
			.Monster(400324, 1018.64, 1.56, 4502.37, 79, count: 3, respawnSeconds: 20)
			.Monster(400324, 1095.48, 2.59, 4502.69, 103, count: 3, respawnSeconds: 20)
			.Monster(57219, 1359.41, -73.23, 4826.19, 176, count: 3, respawnSeconds: 20)
			.Monster(57219, 1060.71, 2.83, 4485.61, 100, count: 3, respawnSeconds: 20)
			.On(s => s.Elapsed >= 5 && s.Alive() <= 0, s => s.Game.CompleteObjective(50044, "holdTheTower"), 1);

		game.Start("STAGE01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				character.ServerMessage(L("Monsters started to attack the Seal Tower!"));
				break;

			case 1:
				StartMinigame(character, track);
				break;
			case 14:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
