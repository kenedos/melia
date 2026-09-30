//--- Melia Script ----------------------------------------------------------
// Draining Blut's Altar
//--- Description -----------------------------------------------------------
// Hauberk takes the power the altar gathered, and the altar fights him for
// it the whole way.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("VPRISON511_MQ_04_TRACK")]
public class Vprison511Mq04Track : TrackScript
{
	private const int DrainSeconds = 150;

	protected override void Load()
	{
		SetId("VPRISON511_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1791.08f, 351.35f, 510.83f));

		actors.Add(AddTrackActor(character, 41327, -1831.11, 351.35, 510.37, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Blut's Altar") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57840, -1888.74, 351.35, 498.85, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Hauberk") }));

		return actors.ToArray();
	}

	private readonly static double[,] WaveSpots =
	{
		{ 57319, -1401.24, 34.84 }, { 57319, -1317.60, 24.34 }, { 57319, -1432.10, -18.33 },
		{ 57319, -1571.09, 17.90 }, { 57319, -1736.44, -22.29 }, { 57319, -1716.20, 36.03 },
		{ 57319, -1876.33, 9.55 }, { 57319, -1502.55, -2.52 }, { 57319, -1626.53, -17.90 },
		{ 57313, -1378.98, 21.51 },
	};

	/// <summary>
	/// Counts the seconds Hauberk needs to absorb the altar, while the altar's guards come in waves.
	/// </summary>
	private static void StartDrain(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);
		var ticks = 0;

		var drain = game.Stage("drain")
			.On(s => s.Elapsed >= 15, s => s.Game.StartStage("waves"), 1)
			.On(s => true, s =>
			{
				if (++ticks > DrainSeconds)
					return;

				foreach (var member in s.Game.Members)
					member.Quests.AddObjectiveProgress(new QuestId(60005), "drainAltar");

				if (ticks == DrainSeconds)
					_ = Announce(s.Game);
			});

		var waves = game.Stage("waves");

		for (var i = 0; i < WaveSpots.GetLength(0); ++i)
			waves.Monster((int)WaveSpots[i, 0], WaveSpots[i, 1], 346.41, WaveSpots[i, 2], count: 2, respawnSeconds: 20, aggressive: false);

		game.Start("drain");
	}

	private static async Task Announce(TrackMinigame game)
	{
		foreach (var member in game.Members)
			member.ServerMessage(LF("{0}: {1}", L("Demon Lord Hauberk"), L("We have fully absorbed the power of Blut.")));

		await Task.Delay(TimeSpan.FromSeconds(3));

		foreach (var member in game.Members)
			member.ServerMessage(LF("{0}: {1}", L("Demon Lord Hauberk"), L("Now let's attack Blut!")));
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				character.ServerMessage(L("Protect Hauberk while he absorbs power of the altar!"));
				break;

			case 24:
				HoldTrackOpen(track);
				StartDrain(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
