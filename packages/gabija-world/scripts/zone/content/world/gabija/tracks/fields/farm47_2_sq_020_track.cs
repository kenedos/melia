//--- Melia Script ----------------------------------------------------------
// The chest in the field
//--- Description -----------------------------------------------------------
// Reaching for the lid brings the field's monsters in, and the chest has to
// be broken open instead.
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

[TrackScript("FARM47_2_SQ_020_TRACK")]
public class Farm472Sq020Track : TrackScript
{
	protected override void Load()
	{
		SetId("FARM47_2_SQ_020_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(903.70f, 72.61f, -893.32f));

		actors.Add(AddTrackActor(character, 152019, 925.30, 72.60, -850.04, 0, new TrackActorSpec { Ai = "TrackWaitMonster", MaxHp = 20, Name = L("Old Chest") }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Sends the field's monsters at the player, in two waves.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("1st")
			.Monster(57327, 807.81, 72.61, -1059.12)
			.Monster(57327, 848.09, 72.61, -1106.61)
			.Monster(57327, 906.85, 72.61, -1141.33, 56)
			.Monster(57327, 965.88, 72.61, -1160.77, 101)
			.Monster(57327, 1014.56, 72.61, -1133.53, 112)
			.Monster(57327, 1058.65, 72.61, -1097.80, 146)
			.Monster(57327, 1107.53, 72.61, -1056.46, 173)
			.Monster(57327, 1148.66, 73.45, -989.93, -170)
			.On(s => s.Elapsed >= 10, s => s.Game.StartStage("2nd"), 1);

		game.Stage("2nd")
			.Monster(57488, 751.21, 72.61, -1148.54)
			.Monster(57488, 705.29, 72.61, -1083.47)
			.Monster(57488, 833.72, 72.61, -1211.13, 55)
			.Monster(57488, 961.63, 72.61, -1225.78, 80)
			.Monster(57488, 1071.85, 72.61, -1178.34, 131)
			.Monster(57488, 1151.25, 72.77, -1077.51, 168);

		game.Start("1st");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				StartMinigame(character, track);
				character.ServerMessage(L("Fight the monsters off and break the chest open!"));
				break;

			case 4:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
