//--- Melia Script ----------------------------------------------------------
// The Soul Pot
//--- Description -----------------------------------------------------------
// The Soul Pot fills with the souls of what comes for it.
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

[TrackScript("ZACHA5F_MQ_03_TRACK")]
public class Zacha5fMq03Track : TrackScript
{
	private const int PotQuestId = 8390;

	protected override void Load()
	{
		SetId("ZACHA5F_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2674.49f, 332.09f, -2513.34f));

		actors.Add(AddTrackActor(character, 47258, -2498.63, 334.14, -2598.34, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Level = 95, Name = L("Soul Pot") }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Rusrats and Medakia whose souls fill the pot.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("victim")
			.Monster(400821, -2666.07, 334.14, -2595.56, 0)
			.Monster(400821, -2334.12, 334.14, -2606.14, 0);

		game.Stage("mon")
			.Monster(400801, -2514.76, 334.14, -2393.37, 0)
			.Monster(400801, -2316.27, 334.14, -2610.44, 0)
			.Monster(400801, -2510.19, 334.14, -2811.27, 0)
			.Monster(400801, -2705.45, 334.14, -2601.86, 0)
			.On(s => s.Elapsed >= 10, s => s.Game.StartStage("victim"))
			.On(s => s.Alive(0) <= 0, s => s.Spawn(0, 1), 8)
			.On(s => s.Alive(1) <= 0, s => s.Spawn(1, 1), 8)
			.On(s => s.Alive(2) <= 0, s => s.Spawn(2, 1), 8)
			.On(s => s.Alive(3) <= 0, s => s.Spawn(3, 1), 8)
			.On(s => s.Elapsed >= 15 && s.Alive() <= 0 && s.Game.Stage("victim").Alive() <= 0, s => s.Game.CompleteObjective(PotQuestId, "placePot"), 1);

		game.Start("mon");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				StartMinigame(character, track);
				break;
			case 10:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
