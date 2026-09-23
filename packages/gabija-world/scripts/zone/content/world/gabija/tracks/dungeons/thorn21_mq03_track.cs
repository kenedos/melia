//--- Melia Script ----------------------------------------------------------
// Gaigalas over the Sviesa Hill root
//--- Description -----------------------------------------------------------
// Touching the root brings its keeper down the hill.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ03_TRACK")]
public class Thorn21Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41220, 2478.4407, 122.01666, -1192.073, 7, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the fight at Bramble's root, which calls in Matsums once it is hurt.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(153011, 2800.98, 122.08, -1326.44, -11, level: 64, maxHp: 25)
			.On(s => s.HpRate(0) > 0 && s.HpRate(0) < 70, s => s.Game.StartStage("stage_02"))
			.On(s => s.Alive(0) <= 0, s => s.Game.ClearStage("stage_02"));

		game.Stage("stage_02")
			.Monster(400381, 2646.93, 122.08, -1399.93, 0, count: 3, respawnSeconds: 20)
			.Monster(400381, 2724.78, 122.08, -1082.66, 0, count: 3, respawnSeconds: 20)
			.Monster(400381, 2745.87, 122.08, -1369.14, 0, count: 3, respawnSeconds: 20)
			.Monster(400381, 2657.39, 122.08, -1053.19, 0, count: 3, respawnSeconds: 20);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				StartMinigame(character, track);
				break;
			case 9:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
