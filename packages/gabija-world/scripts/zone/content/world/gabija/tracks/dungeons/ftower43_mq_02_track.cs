//--- Melia Script ----------------------------------------------------------
// The first control valve
//--- Description -----------------------------------------------------------
// The laboratory valve and the research Antares left scattered around it.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER43_MQ_02_TRACK")]
public class Ftower43Mq02Track : TrackScript
{
	private readonly static double[,] BookSpots =
	{
		{ -1762, 537, 696 }, { -1668, 537, 866 }, { -1524, 537, 895 }, { -1526, 536, 398 },
	};

	protected override void Load()
	{
		SetId("FTOWER43_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147504, -1598.82, 554.53, 708.90, 21, new TrackActorSpec { Ai = "MON_DUMMY", Level = 115, Name = L("Magic Control Valve") }));

		for (var i = 0; i < BookSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 147311, BookSpots[i, 0], BookSpots[i, 1], BookSpots[i, 2], 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Scattered Book") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Sends the laboratory's guards at the player once the valve is exposed, and again after half a minute.
	/// </summary>
	private static async Task StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("stage1")
			.Monster(47394, -1621.58, 554.52, 733.08, count: 2)
			.Monster(57053, -1601.28, 549.66, 661.58, count: 2)
			.Monster(47394, -1553.73, 550.65, 704.72, count: 2)
			.On(s => s.Elapsed >= 30, s =>
			{
				s.Spawn(0, 2);
				s.Spawn(1, 2);
				s.Spawn(2, 2);
			}, 1);

		await Task.Delay(TimeSpan.FromSeconds(4));

		game.Start("stage1");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				_ = StartMinigame(character, track);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Destroy the Magic Control Valve!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
