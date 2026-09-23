//--- Melia Script ----------------------------------------------------------
// Hogma's Treasure Chest
//--- Description -----------------------------------------------------------
// The Hogma of the crossroads come down on whoever opens their chest.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ROKAS31_SUB_01_TRACK")]
public class Rokas31Sub01Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS31_SUB_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147392, -736.16, 107.10, -1088.93, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Hogma waves that come out of the ruins.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("1st")
			.Monster(41433, -684.18, 107.1, -906.11, -119, respawnSeconds: 20)
			.Monster(41433, -583.71, 107.1, -980.26, -163, respawnSeconds: 20)
			.Monster(41433, -568.57, 107.1, -1096.54, 156, respawnSeconds: 20)
			.Monster(41434, -420.65, 107.1, -1055.01, 167, respawnSeconds: 20)
			.Monster(41434, -487.81, 107.1, -947.52, -155, respawnSeconds: 20)
			.On(s => s.Elapsed >= 10, s => s.Game.StartStage("2nd"), 1);

		game.Stage("2nd")
			.Monster(41435, -698.21, 107.1, -902.77, 48, respawnSeconds: 20)
			.Monster(41435, -364.95, 107.1, -861.88, -161, respawnSeconds: 20)
			.Monster(41435, -332.23, 107.1, -1002.49, 177, respawnSeconds: 20)
			.Monster(41435, -630.8, 107.1, -1105.77, 24, respawnSeconds: 20)
			.On(s => s.Alive(0, 1, 2, 3) <= 2, s => s.Game.StartStage("3rd"), 1);

		game.Stage("3rd")
			.Monster(47308, -450.46, 107.1, -1073.05, 165, respawnSeconds: 20)
			.Monster(47308, -311.71, 107.1, -1014.38, 139, respawnSeconds: 20)
			.Monster(47308, -381.87, 107.1, -870.05, -137, respawnSeconds: 20)
			.On(s => s.Elapsed >= 15, s => s.Game.StartStage("4th"), 1);

		game.Stage("4th")
			.Monster(47309, -420.15, 107.1, -1111.7, 142, respawnSeconds: 20)
			.Monster(47309, -493.23, 107.1, -936.12, 172, respawnSeconds: 20)
			.Monster(47309, -313.92, 107.1, -1002.99, 156, respawnSeconds: 20)
			.On(s => s.Alive(0, 1, 2) <= 1, s => s.Game.StartStage("5th"), 1);

		game.Stage("5th")
			.Monster(41435, -469.36, 107.1, -990.38, -170, respawnSeconds: 20)
			.Monster(41435, -566.24, 107.1, -897.69, -5, respawnSeconds: 20)
			.Monster(41435, -476.85, 107.1, -1113.02, 165, respawnSeconds: 20)
			.Monster(41434, -425.35, 107.1, -887.45, 90, respawnSeconds: 20)
			.Monster(41434, -359.23, 107.1, -968.89, -168, respawnSeconds: 20);

		game.Start("1st");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				StartMinigame(character, track);
				break;
			case 4:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
