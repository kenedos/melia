//--- Melia Script ----------------------------------------------------------
// Down the Zinuma Passage
//--- Description -----------------------------------------------------------
// Daiva closes the passage behind Hauberk, and Sigita is standing at the far
// end of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("VPRISON513_MQ_04_TRACK")]
public class Vprison513Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON513_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1386.90f, 91.04f, 908.82f));

		actors.Add(AddTrackActor(character, 154013, -1382.73, 91.04, 940.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Daiva") }));
		actors.Add(AddTrackActor(character, 154001, -1455.84, 91.04, 936.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealing Barrier") }));
		actors.Add(AddTrackActor(character, 154001, 1513.61, 91.04, 924.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealing Barrier") }));
		actors.Add(AddTrackActor(character, 154012, 1307.47, 91.04, 916.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Sigita") }));

		return actors.ToArray();
	}

	private readonly static (double X, double Z)[][] TriggerAreas =
	{
		new[] { (-820.65, 879.76), (-838.50, 932.23), (-765.05, 965.31), (-715.71, 829.43) },
		new[] { (-376.17, 817.28), (-436.58, 949.38), (-245.32, 959.34), (-215.07, 874.94) },
		new[] { (233.10, 855.24), (240.84, 989.74), (418.64, 958.30), (394.08, 811.41) },
	};

	private static bool IsInside((double X, double Z)[] area, Position position)
	{
		var inside = false;

		for (int i = 0, j = area.Length - 1; i < area.Length; j = i++)
		{
			if ((area[i].Z > position.Z) != (area[j].Z > position.Z)
				&& position.X < (area[j].X - area[i].X) * (position.Z - area[i].Z) / (area[j].Z - area[i].Z) + area[i].X)
			{
				inside = !inside;
			}
		}

		return inside;
	}

	/// <summary>
	/// Sends waves of the Demon Lord's guards down the passage, each set off by entering its stretch.
	/// </summary>
	private static void StartChase(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);
		var spoken = new bool[3];

		var route = game.Stage("set");

		for (var i = 0; i < TriggerAreas.Length; ++i)
		{
			var area = TriggerAreas[i];
			var stageName = i == 0 ? "DefGroup" : i.ToString();

			route.On(s => s.Game.Members.Any(member => !member.IsDead && IsInside(area, member.Position)), s => s.Game.StartStage(stageName), 1);
		}

		game.Stage("DefGroup")
			.Monster(57717, -756.23, 50.64, 526.06, aggressive: false)
			.Monster(57717, -557.52, 50.64, 505.95, aggressive: false)
			.Monster(57717, -647.59, 50.64, 589.56, aggressive: false)
			.Monster(57716, -658.01, 50.64, 451.46, aggressive: false)
			.Monster(57719, -803.76, 50.64, 454.34, aggressive: false)
			.Monster(57827, -672.38, 50.64, 523.50, -100, aggressive: false, level: 181, faction: FactionType.Our_Forces, name: L("Demon Lord Hauberk"), lifeSeconds: 50)
			.Monster(154001, -499.75, 91.04, 905.27, -79, aggressive: false, faction: FactionType.Our_Forces, name: L("Sealing Barrier"))
			.On(s => s.Alive(0, 1, 2, 3, 4) <= 0, s => s.Game.ClearStage("DefGroup"), 1)
			.On(s => !spoken[0] && SpeaksNear(s, 5), s =>
			{
				spoken[0] = true;
				Speak(s, L("You were already fooled. Are you going to be fooled again?"));
			});

		game.Stage("1")
			.Monster(57716, -6.13, 94.60, 702.60, aggressive: false)
			.Monster(57716, 24.14, 94.60, 710.58, aggressive: false)
			.Monster(57716, 3.21, 94.60, 775.95, aggressive: false)
			.Monster(57717, 37.98, 94.60, 762.74, aggressive: false)
			.Monster(57717, -2.03, 94.60, 1184.29, aggressive: false)
			.Monster(57717, 30.38, 94.60, 1201.64, aggressive: false)
			.Monster(57717, 52.55, 94.60, 1124.46, aggressive: false)
			.Monster(57717, -0.52, 94.60, 1137.37, aggressive: false)
			.Monster(57827, 24.06, 94.60, 899.84, -168, aggressive: false, level: 181, faction: FactionType.Our_Forces, name: L("Demon Lord Hauberk"), lifeSeconds: 50)
			.Monster(154001, 100.15, 91.04, 914.69, -90, aggressive: false, faction: FactionType.Our_Forces, name: L("Sealing Barrier"))
			.On(s => s.Alive(0, 1, 2, 3, 4, 5, 6, 7) <= 0, s => s.Game.ClearStage("1"), 1)
			.On(s => !spoken[1] && SpeaksNear(s, 8), s =>
			{
				spoken[1] = true;
				Speak(s, L("Followers of the goddesses act like goddesses themselves."));
			});

		game.Stage("2")
			.Monster(57827, 453.33, 50.64, 544.33, 116, aggressive: false, level: 181, faction: FactionType.Our_Forces, name: L("Demon Lord Hauberk"), lifeSeconds: 50)
			.Monster(57715, 349.01, 50.64, 507.91, aggressive: false)
			.Monster(57715, 463.76, 50.64, 402.53, aggressive: false)
			.Monster(57715, 522.09, 50.64, 390.84, aggressive: false)
			.Monster(57715, 568.41, 50.64, 431.74, aggressive: false)
			.Monster(57715, 562.67, 50.64, 527.51, aggressive: false)
			.Monster(57715, 512.87, 50.64, 577.27, aggressive: false)
			.Monster(57715, 438.29, 50.64, 602.03, aggressive: false)
			.On(s => !spoken[2] && SpeaksNear(s, 0), s =>
			{
				spoken[2] = true;
				Speak(s, L("I said I'd spare you..."));
			})
			.On(s => s.Alive(1, 2, 3, 4, 5, 6, 7) <= 0, s =>
			{
				s.Game.ClearStage("2");
				s.Game.CompleteObjective(60021, "driveHauberk");
			}, 1);

		game.Start("set");

		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Chase Demon Lord Hauberk as he flees through Zinuma Passage."), 4);
	}

	private static bool SpeaksNear(MinigameStage stage, int hauberkIndex)
		=> stage.Living(hauberkIndex).Any(hauberk => stage.Game.Members.Any(member => !member.IsDead && member.Position.Get2DDistance(hauberk.Position) <= 180));

	private static void Speak(MinigameStage stage, string line)
	{
		foreach (var member in stage.Game.Members)
			member.ServerMessage(LF("{0}: {1}", L("Demon Lord Hauberk"), line));
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				HoldTrackOpen(track);
				StartChase(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
