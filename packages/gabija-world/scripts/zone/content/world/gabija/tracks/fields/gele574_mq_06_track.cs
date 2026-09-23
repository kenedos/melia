//--- Melia Script ----------------------------------------------------------
// The Panto Totem at Valyma Sanctum
//--- Description -----------------------------------------------------------
// Burning the totem is what makes the charm take hold.
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

[TrackScript("GELE574_MQ_06_TRACK")]
public class Gele574Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE574_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147356, -1563, 8, -792, 18, new TrackActorSpec { Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 57572, -1727.44, 7.25, -768.49, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1650.74, 7.18, -914.01, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1481.21, 7.18, -887.09, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1412.72, 7.18, -777.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Panto Archers and the Large Panto Spearman guarding the totem.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(57572, -1542.21, 7.34, -592.19, 0, respawnSeconds: 5, level: 35)
			.Monster(57572, -1433.74, 7.18, -708.15, 0, respawnSeconds: 5, level: 35)
			.Monster(57572, -1472.47, 7.4, -555.13, 0, respawnSeconds: 5, level: 35)
			.Monster(57572, -1734.78, 7.21, -703.76, 0, respawnSeconds: 5, level: 35)
			.Monster(57259, -1719.81, 7.18, -865.84, 0, aggressive: false, level: 35, maxHp: 40)
			.Monster(57572, -1609.76, 7.18, -774.64, 0, respawnSeconds: 5, level: 35)
			.Monster(57572, -1563.46, 7.18, -1072.19, 0, respawnSeconds: 5, level: 35)
			.Monster(57572, -1638.19, 7.18, -1037.6, 0, respawnSeconds: 5, level: 35)
			.On(s => s.Alive(4) <= 0, s => s.Game.CompleteObjective(8606, "controlPantos"), 1);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 13:
				StartMinigame(character, track);
				break;
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
