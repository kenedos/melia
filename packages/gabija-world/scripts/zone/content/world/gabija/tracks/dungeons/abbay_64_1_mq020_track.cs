//--- Melia Script ----------------------------------------------------------
// Monk Goss in Restraints
//--- Description -----------------------------------------------------------
// Monk Goss is bound by devices in the Ankel Small Corridor while
// apparitions gather around him.
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

[TrackScript("ABBAY_64_1_MQ020_TRACK")]
public class Abbay641Mq020Track : TrackScript
{
	protected override void Load()
	{
		SetId("ABBAY_64_1_MQ020_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-168.87f, 102.34f, -1129.13f));

		var device = new TrackActorSpec { Ai = "BT_Dummy", Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 155046, -258.77, 80.60, -929.58, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Monk Goss") }));
		actors.Add(AddTrackActor(character, 153119, -160.90, 97.02, -1115.83, 48, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose"), EndPosition = new Position(-243.92f, 93.89f, -1009.38f) }));
		actors.Add(AddTrackActor(character, 47106, -283.68, 80.75, -958.91, 0, device));
		actors.Add(AddTrackActor(character, 151006, -258, 80.32, -972, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 103025, -146.62, 96.42, -614.35, 218, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 103025, -212.33, 96.38, -618.46, 180, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20046, -251.60, 80.58, -933.42, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 47106, -220.91, 80.72, -911.20, 0, device));
		actors.Add(AddTrackActor(character, 47106, -277.01, 80.95, -905.20, 0, device));
		actors.Add(AddTrackActor(character, 47106, -225, 80.40, -961.24, 0, device));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the apparitions that come to stop the rescue.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("ST01")
			.Monster(103025, -151.02, 84.15, -693.72, 3, count: 3)
			.Monster(103025, -151.09, 65.06, -699.90, -9, count: 3)
			.Monster(103025, -158.96, 70.05, -705.96, -17, count: 3)
			.On(s => s.Elapsed >= 30, s => s.Game.StartStage("ST02"), 1);

		game.Stage("ST02")
			.Monster(57674, -152.72, 93.20, -710.04, 7, count: 2)
			.Monster(57674, -156.76, 76.65, -710.23, -9, count: 2);

		game.Start("ST01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
			case 8:
			case 16:
			case 24:
			case 32:
			case 39:
			case 46:
				track.Actors[6].PlayEffect("F_light081_ground_orange2", 1f, 1, EffectLocation.Bottom);
				break;

			case 48:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Destroy the restrainment devices attached to Monk Goss!"), 3);
				break;

			case 49:
				// The two apparitions fade out on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);

				StartMinigame(character, track);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
