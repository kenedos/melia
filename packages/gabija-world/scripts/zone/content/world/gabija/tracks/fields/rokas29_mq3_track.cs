//--- Melia Script ----------------------------------------------------------
// The Epitaph on the Dykyne Fork Road
//--- Description -----------------------------------------------------------
// The third epitaph throws off its guardians as Rexipher reaches it.
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

[TrackScript("ROKAS29_MQ3_TRACK")]
public class Rokas29Mq3Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_MQ3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-187.32f, 681.29f, 495.91f));

		actors.Add(AddTrackActor(character, 47106, -206, 681, 495, 1, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Epitaph") }));
		actors.Add(AddTrackActor(character, 47413, 57.99, 681.77, 594.89, 34, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Historian Rexipher"), EndPosition = new Position(-71.95f, 681.72f, 571.70f) }));
		actors.Add(AddTrackActor(character, 401301, -334.11, 681.29, 598.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -203.54, 681.29, 648.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -292.01, 681.29, 500.84, 43, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -128.68, 681.29, 601.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -107.68, 681.59, 477.39, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Hogma waves the epitaph's alarm draws in.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("1st")
			.Monster(41433, 184.74, 681.78, 647.18, -160, respawnSeconds: 20)
			.Monster(41433, 200.74, 681.78, 574.92, -174, respawnSeconds: 20)
			.Monster(41433, 177.6, 681.78, 513.26, -173, respawnSeconds: 20)
			.On(s => s.Alive(0, 1, 2) <= 0, s => s.Game.StartStage("2nd"), 1);

		game.Stage("2nd")
			.Monster(47308, 225.58, 681.78, 675.68, -152, respawnSeconds: 20)
			.Monster(47308, 216.84, 681.78, 573.56, 165, respawnSeconds: 20)
			.On(s => s.Elapsed >= 15, s => s.Game.StartStage("3rd"), 1);

		game.Stage("3rd")
			.Monster(47308, -239.61, 681.29, 636.37, 0, respawnSeconds: 20)
			.Monster(47308, -265.9, 681.29, 581.34, 0, respawnSeconds: 20)
			.Monster(47308, -305.15, 681.29, 526.61, 0, respawnSeconds: 20)
			.On(s => s.Alive(0, 1, 2) <= 1, s => s.Game.StartStage("4th"), 1);

		game.Stage("4th")
			.Monster(41433, 190.55, 681.78, 620.32, -158, respawnSeconds: 20)
			.Monster(41433, 208.61, 681.78, 548.08, -168, respawnSeconds: 20)
			.Monster(41433, -230.65, 681.29, 632.21, 0, respawnSeconds: 20)
			.Monster(41433, -277.32, 681.29, 575.11, 0, respawnSeconds: 20)
			.On(s => s.Elapsed >= 10, s => s.Game.StartStage("5th"), 1);

		game.Stage("5th")
			.Monster(47308, -278.18, 681.29, 597.8, 0, respawnSeconds: 20)
			.Monster(47308, 188.58, 681.78, 538.55, -151, respawnSeconds: 20)
			.Monster(41433, -314.32, 681.29, 534.26, 0, respawnSeconds: 20)
			.Monster(41433, -261.53, 681.29, 653.89, 0, respawnSeconds: 20)
			.Monster(41433, 142.35, 681.78, 607.88, -159, respawnSeconds: 20)
			.Monster(41433, 154.5, 681.78, 488.83, 162, respawnSeconds: 20)
			.On(s => s.Alive(0, 1, 2, 3, 4, 5) <= 3, s => s.Game.StartStage("6th"));

		game.Stage("6th")
			.Monster(47308, -194.05, 681.29, 611.79, 0, respawnSeconds: 20)
			.Monster(47308, 42.26, 681.78, 476.09, 82, respawnSeconds: 20)
			.Monster(47308, 166.25, 681.78, 613.42, 157, respawnSeconds: 20)
			.On(s => s.Elapsed >= 10, s => s.Game.StartStage("6th"), 1);

		game.Stage("7th")
			.Monster(41433, -157.88, 681.29, 607.77, 0, respawnSeconds: 20)
			.Monster(41433, -17.46, 681.78, 495.12, 0, respawnSeconds: 20)
			.Monster(41433, 175.56, 681.78, 609.75, -164, respawnSeconds: 20)
			.Monster(41433, 136.54, 681.78, 525.75, -176, respawnSeconds: 20);

		game.Start("1st");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.7f, EffectLocation.Bottom);
				break;
			case 2:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.8f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("I_smoke008_red##2", 1.1f, EffectLocation.Bottom);
				break;
			case 10:
				track.Actors[0].AttachEffect("F_warrior_reward_shot_lineup", 1.3f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 1.7f, EffectLocation.Bottom);
				break;
			case 12:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 1.1f, EffectLocation.Bottom);
				break;
			case 13:
				track.Actors[0].AttachEffect("F_buff_basic029_red_line", 3, EffectLocation.Bottom);
				break;
			case 16:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.8f, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[1].AttachEffect("F_buff_basic029_red_line", 5, EffectLocation.Bottom);
				break;
			case 24:
				track.Actors[1].AttachEffect("F_cleric_ShapeShifting_ground", 1.2f, EffectLocation.Bottom);
				break;
			case 33:
				StartMinigame(character, track);
				break;
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
