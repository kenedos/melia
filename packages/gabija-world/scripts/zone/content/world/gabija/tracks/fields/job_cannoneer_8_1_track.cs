//--- Melia Script ----------------------------------------------------------
// The observation orb at Ghresmei Passage
//--- Description -----------------------------------------------------------
// One of the strange objects the Cannoneer Master heard about, sitting in
// plain sight of the kingdom camp.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("JOB_CANNONEER_8_1_TRACK")]
public class JobCannoneer81Track : TrackScript
{
	protected override void Load()
	{
		SetId("JOB_CANNONEER_8_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1432.50f, 662.24f, 444.97f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 58533, 1379.94, 662.20, 433.06, 0, new TrackActorSpec { Ai = "BT_Dummy", Name = L("Observation Orb") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Keeps the orb's guards coming back, each spawn point joining in at its own time.
	/// </summary>
	private static async Task StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("guard1").Monster(57914, 1415.01, 662.27, 530.76, count: 3, respawnSeconds: 20);
		game.Stage("guard2").Monster(57904, 1494.81, 662.20, 317.47, count: 2, respawnSeconds: 20);
		game.Stage("guard3").Monster(57914, 1309.32, 662.18, 275.35, count: 2, respawnSeconds: 20);
		game.Stage("guard4").Monster(57904, 1238.09, 662.17, 493.53, count: 1, respawnSeconds: 20);

		await Task.Delay(TimeSpan.FromSeconds(5));
		game.Start("guard1");

		await Task.Delay(TimeSpan.FromSeconds(10));
		game.StartStage("guard3");

		await Task.Delay(TimeSpan.FromSeconds(15));
		game.StartStage("guard2");

		await Task.Delay(TimeSpan.FromSeconds(30));
		game.StartStage("guard4");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				_ = StartMinigame(character, track);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Destroy the Observation Orb!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
