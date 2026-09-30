//--- Melia Script ----------------------------------------------------------
// Charging the Evening Star Key
//--- Description -----------------------------------------------------------
// Zydrone holds the goddess' power into the key and cannot look up while she
// does it.
//---------------------------------------------------------------------------

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

[TrackScript("VPRISON514_MQ_03_TRACK")]
public class Vprison514Mq03Track : TrackScript
{
	private const int DefenseSeconds = 120;

	protected override void Load()
	{
		SetId("VPRISON514_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-539.43f, 334.60f, 373.70f));

		actors.Add(AddTrackActor(character, 154015, -942.16, 335.57, -451.51, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 20026, -982.13, 335.57, -434.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	private readonly static double[,] WaveSpots =
	{
		{ -684.65, 328.79, -97.32 }, { -439.30, 328.79, -313.06 }, { -569.82, 328.79, -171.26 },
		{ -499.95, 328.79, -379.89 }, { -580.52, 335.57, -295.84 }, { -647.41, 335.57, -229.78 },
	};

	/// <summary>
	/// Sends endless waves at Zydrone while she completes the key, until the time is up.
	/// </summary>
	private static void StartDefense(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);
		var ticks = 0;

		var defense = game.Stage("DefGroup")
			.On(s => true, s =>
			{
				if (++ticks > DefenseSeconds)
					return;

				foreach (var member in s.Game.Members)
					member.Quests.AddObjectiveProgress(new QuestId(60014), "guardZydrone");
			});

		for (var i = 0; i < WaveSpots.GetLength(0); ++i)
			defense.Monster(57448, WaveSpots[i, 0], WaveSpots[i, 1], WaveSpots[i, 2], count: 3, respawnSeconds: 15, level: 157);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				HoldTrackOpen(track);
				StartDefense(character, track);
				character.ServerMessage(L("Protect Zydrone until she completes the Evening Star Key!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
