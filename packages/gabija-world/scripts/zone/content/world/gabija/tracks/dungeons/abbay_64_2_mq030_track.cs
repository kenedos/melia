//--- Melia Script ----------------------------------------------------------
// Rose Taken in the Apega State Chamber
//--- Description -----------------------------------------------------------
// The mysterious wizard seizes Rose and leaves Magic Stones of Pain around
// the shackled Edmundas.
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

[TrackScript("ABBAY_64_2_MQ030_TRACK")]
public class Abbay642Mq030Track : TrackScript
{
	protected override void Load()
	{
		SetId("ABBAY_64_2_MQ030_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(14.89f, 981.50f, -1249.02f));

		var prop = new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };
		var stone = new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Monster, Name = L("Magic Stone of Pain") };

		actors.Add(AddTrackActor(character, 153110, -13.76, 982.53, -1335.67, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Edmundas") }));
		actors.Add(AddTrackActor(character, 153119, 11, 982.54, -1272, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20046, -16.41, 982.53, -1331.77, 0, prop));
		actors.Add(AddTrackActor(character, 153120, 7.69, 982.54, -1227.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Wizard") }));
		actors.Add(AddTrackActor(character, 153069, 68.56, 981.50, -1273.99, 16, stone));
		actors.Add(AddTrackActor(character, 153069, -77.88, 981.50, -1404.26, 22, stone));
		actors.Add(AddTrackActor(character, 153069, 66.37, 981.50, -1406.62, 23, stone));
		actors.Add(AddTrackActor(character, 20026, 68.56, 981.50, -1273.99, 23, prop));
		actors.Add(AddTrackActor(character, 20026, -77.88, 981.50, -1404.26, 23, prop));
		actors.Add(AddTrackActor(character, 20026, 66.37, 981.50, -1406.62, 39, prop));
		actors.Add(AddTrackActor(character, 103026, -49.60, 982.54, -1151.09, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 103026, 33.13, 982.54, -1146.74, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 103026, 91.73, 982.54, -1154.82, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153069, -97.78, 981.50, -1253.15, 27, stone));
		actors.Add(AddTrackActor(character, 20026, -97.78, 981.50, -1253.15, 32, prop));
		actors.Add(AddTrackActor(character, 20026, 8.27, 981.50, -1287.41, 5, prop));
		actors.Add(AddTrackActor(character, 47106, 42, 981.50, -1286, 0, prop));
		actors.Add(AddTrackActor(character, 47106, -64, 981.50, -1285, 0, prop));
		actors.Add(AddTrackActor(character, 47106, -61, 981.50, -1379, 0, prop));
		actors.Add(AddTrackActor(character, 47106, 36, 981.50, -1377, 0, prop));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Minos the wizard sends after the Magic Stones of Pain
	/// are set.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("ST01")
			.Monster(103026, -18.48, 981.50, -1040.95, -8, count: 3)
			.Monster(103026, 49.33, 981.50, -1017.27, -4, count: 3)
			.On(s => s.Elapsed >= 30, s => s.Game.StartStage("ST02"), 1);

		game.Stage("ST02")
			.Monster(103026, -12.14, 981.50, -1044.64, -20, count: 2)
			.Monster(57674, 22.04, 981.50, -967.85, 5, count: 2);

		game.Start("ST01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		if (frame % 10 == 0 && frame <= 70)
		{
			track.Actors[3].PlayEffect("F_pattern008_violet", 1.2f, 1, EffectLocation.Bottom);
			for (var i = 17; i <= 20; i++)
				track.Actors[i].AttachEffect("E_pc_darksmoke", 1, EffectLocation.Bottom);
		}

		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_light081_ground_orange_loop2", 3, EffectLocation.Bottom);
				break;

			case 13:
				track.Actors[4].AttachEffect("F_smoke023_red", 1, EffectLocation.Bottom);
				break;

			case 42:
				track.Actors[8].PlayEffect("F_explosion014", 1f, 1, EffectLocation.Bottom);
				break;

			case 43:
				track.Actors[10].PlayEffect("F_explosion014", 1f, 1, EffectLocation.Bottom);
				break;

			case 44:
				track.Actors[9].PlayEffect("F_explosion014", 1f, 1, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_levitation032_red_loop", 1.5f, EffectLocation.Bottom);
				break;

			case 45:
				track.Actors[15].PlayEffect("F_explosion014", 1f, 1, EffectLocation.Bottom);
				break;

			case 48:
				for (var i = 11; i <= 13; i++)
					track.Actors[i].PlayEffect("F_burstup025_dark", 1f, 1, EffectLocation.Bottom);
				break;

			case 58:
				track.Dialog.SetTitle(L("Mysterious Wizard"));
				StartDialog(track,
					L("We can just keep growing more bracken, we have someone to do that!"),
					L("No one can stop miss Giltine's plan.")
				);
				break;

			case 66:
				track.Actors[4].AttachEffect("F_light097_red", 2.3f, EffectLocation.Bottom);
				track.Actors[16].PlayEffect("F_levitation032_red", 0.8f, 1, EffectLocation.Bottom);
				track.Actors[16].AttachEffect("F_ground083_smoke", 1, EffectLocation.Bottom);
				break;

			case 68:
				CreateBattleBoxInLayer(character, track);
				break;

			case 69:
				StartMinigame(character, track);
				SetTrackTendency(character, track);
				break;

			case 70:
				// The wizard vanishes with Rose on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 4);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
