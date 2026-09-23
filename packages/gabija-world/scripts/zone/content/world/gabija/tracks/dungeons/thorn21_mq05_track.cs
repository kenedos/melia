//--- Melia Script ----------------------------------------------------------
// Molich over the Tankinta root
//--- Description -----------------------------------------------------------
// Molich comes down the rise the moment the second root is touched.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ05_TRACK")]
public class Thorn21Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(3291.9084f, 332.3721f, 1148.6813f));

		actors.Add(AddTrackActor(character, 400421, 2469.7488, 418.3634, 1383.1254, 341, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3112.3901f, 332.3721f, 1158.4996f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the fight at Bramble's root, which calls in Ammons once it is hurt.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(153011, 3305.59, 332.37, 1084.99, -10, level: 64, maxHp: 25)
			.On(s => s.HpRate(0) > 0 && s.HpRate(0) < 70, s => s.Game.StartStage("stage_01"))
			.On(s => s.Alive(0) <= 0, s => s.Game.ClearStage("stage_01"));

		game.Stage("stage_01")
			.Monster(41266, 3269.96, 332.37, 1239.04, 0)
			.Monster(41266, 3260.64, 332.37, 1018.16, 0)
			.Monster(41266, 3168.56, 332.37, 1050.06, 0)
			.Monster(41266, 3206.43, 332.37, 1236.49, 0)
			.On(s => s.Alive(0) <= 0, s => s.Spawn(0, 1), 3)
			.On(s => s.Alive(1) <= 0, s => s.Spawn(1, 1), 3)
			.On(s => s.Alive(2) <= 0, s => s.Spawn(2, 1), 3)
			.On(s => s.Alive(3) <= 0, s => s.Spawn(3, 1), 3);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				StartMinigame(character, track);
				break;
			case 24:
				CreateBattleBoxInLayer(character, track);
				break;
			case 29:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
