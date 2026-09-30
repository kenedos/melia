//--- Melia Script ----------------------------------------------------------
// Burning the Ramus Crossroads circle
//--- Description -----------------------------------------------------------
// Varas' woodpile catches, and the shield over the circle burns through with
// it.
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

[TrackScript("FARM47_2_SQ_070_TRACK")]
public class Farm472Sq070Track : TrackScript
{
	protected override void Load()
	{
		SetId("FARM47_2_SQ_070_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-602.40f, 0.71f, 1067.23f));

		actors.Add(AddTrackActor(character, 153047, -668.16, 0.71, 1062.21, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Unstable Magic Circle") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47223, -627.14, 0.71, 1065.65, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Stacked Firewood") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Sends in the monsters that defend the circle, wave after wave.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("1st")
			.Monster(57502, -395.44, 0.71, 1206.32)
			.Monster(57502, -383.10, 0.71, 943.84)
			.Monster(57502, -480.31, 0.71, 863.03)
			.Monster(57502, -717.25, -2.80, 839.81)
			.Monster(57502, -848.18, -2.34, 1161.43)
			.On(s => s.Alive() <= 1, s => s.Game.StartStage("2nd"), 1)
			.On(s => s.Elapsed >= 5, s => s.Game.StartStage("3rd"), 1);

		game.Stage("2nd")
			.Monster(57488, -339.37, 0.56, 1067.76)
			.Monster(57488, -430.50, 0.50, 877.95)
			.Monster(57488, -691.84, -0.60, 844.35)
			.Monster(57488, -888.54, -11.15, 1182.23);

		game.Stage("3rd")
			.Monster(57327, -350.23, -0.49, 1022.41)
			.Monster(57327, -344.42, 0.71, 920.34)
			.Monster(57327, -721.42, -4.31, 831.98)
			.On(s => s.Alive(0, 2) <= 0, s => s.Game.StartStage("4th"), 1);

		game.Stage("4th")
			.Monster(57327, -418.47, 0.71, 1223.86)
			.Monster(57327, -898.31, -12.10, 1183.48)
			.Monster(57327, -547.28, 0.71, 853.60);

		game.Start("1st");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				StartMinigame(character, track);
				character.ServerMessage(L("The magic circle is on fire! Get kindling from Orange Dandel and keep the fire alive!"));
				break;

			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
