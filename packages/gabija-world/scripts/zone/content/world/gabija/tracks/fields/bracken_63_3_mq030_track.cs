//--- Melia Script ----------------------------------------------------------
// Rose Taken at the Giant Bracken
//--- Description -----------------------------------------------------------
// The mysterious wizard's demons seize Rose beside the giant bracken.
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

[TrackScript("BRACKEN_63_3_MQ030_TRACK")]
public class Bracken633Mq030Track : TrackScript
{
	protected override void Load()
	{
		SetId("BRACKEN_63_3_MQ030_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(336.09f, 189.32f, 119.92f));

		actors.Add(AddTrackActor(character, 153119, 49.85, 189.58, 489.81, 14, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(AddTrackActor(character, 153120, -32.52, 189.58, 445.97, 85, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Wizard"), EndPosition = new Position(-104.12f, 189.58f, 430.31f) }));
		actors.Add(AddTrackActor(character, 103024, 119.82, 187.87, 479.78, 7, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(163.39f, 187.60f, 392.19f) }));
		actors.Add(AddTrackActor(character, 103024, 97.39, 188.47, 419.15, 5, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 103024, 45.30, 189.58, 373.98, 14, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(65.45f, 189.58f, 352.47f) }));
		actors.Add(AddTrackActor(character, 103024, -16.44, 189.58, 398.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2.07f, 189.58f, 356.80f) }));
		actors.Add(AddTrackActor(character, 40095, -77.59, 189.58, 393.35, 9, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153115, -70, 189.58, 591, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Giant Bracken") }));
		actors.Add(AddTrackActor(character, 20046, -57.94, 189.58, 477.32, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the two waves of demons that come for Rose.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("stage_01")
			.Monster(57640, 87.90, 189.58, 193.06, 70)
			.Monster(57640, 95.82, 189.58, 157.20, 90)
			.Monster(103024, 102.33, 189.58, 157.38, 90)
			.Monster(103024, 98.91, 189.58, 162.41, 69)
			.Monster(103024, 108.43, 189.58, 155.14, 81)
			.Monster(57640, 108.43, 189.58, 155.14, 81)
			.On(s => s.Elapsed >= 20, s => s.Game.StartStage("stage_02"), 1);

		game.Stage("stage_02")
			.Monster(57640, 145.68, 189.58, 147.47, 185, respawnSeconds: 20)
			.Monster(103024, 138.73, 189.58, 153.66, 173, respawnSeconds: 20)
			.Monster(57640, 172.50, 189.58, 158.99, 188, respawnSeconds: 20)
			.Monster(103024, 135.64, 189.58, 161.49, 179, respawnSeconds: 20)
			.Monster(103024, 124.12, 189.58, 159.84, 181, respawnSeconds: 20);

		game.Start("stage_01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
			case 7:
			case 12:
				track.Actors[0].PlayEffect("F_light081_ground_orange2", 1f, 1, EffectLocation.Bottom);
				break;

			case 4:
				track.Actors[0].PlayEffect("F_smoke019_dark", 0.5f, 1, EffectLocation.Bottom);
				break;

			case 5:
				track.Actors[0].PlayEffect("F_pattern008_violet", 0.5f, 1, EffectLocation.Bottom);
				break;

			case 47:
				StartMinigame(character, track);
				break;

			case 49:
				// The wizard fades out on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 1);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The demons are trying to kidnap Rose!{nl}Rescue Rose from the demons!"), 5);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
