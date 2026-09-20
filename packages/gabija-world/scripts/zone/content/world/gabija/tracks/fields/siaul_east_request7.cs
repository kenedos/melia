//--- Melia Script ----------------------------------------------------------
// The Refugees' Flight
//--- Description -----------------------------------------------------------
// Bube chase refugees toward the outpost while the guards hold the line.
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

[TrackScript("SIAUL_EAST_REQUEST7_TRACK")]
public class SiaulEastRequest7Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_REQUEST7_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 11284, 166.36302, 151.7621, 695.29218, 31, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			CombatNpc = true,
			EndPosition = new Position(306.02808f, 130.02271f, 388.56702f),
		}));

		actors.Add(AddTrackActor(character, 10020, 137.43283, 151.7621, 671.72125, 61, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			EndPosition = new Position(261.76868f, 130.02271f, 393.26120f),
		}));

		actors.Add(AddTrackActor(character, 10020, 198.59071, 151.7621, 669.93793, 55, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			EndPosition = new Position(243.74289f, 130.02271f, 419.94662f),
		}));

		actors.Add(AddTrackActor(character, 20114, 645.25177, 130.02271, 320.44131, 76, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Refugee") }));
		actors.Add(AddTrackActor(character, 20117, 617.24512, 130.02271, 354.97476, 86, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Refugee") }));
		actors.Add(AddTrackActor(character, 20115, 585.32758, 130.02271, 334.73215, 78, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Refugee") }));
		actors.Add(AddTrackActor(character, 152000, 646.31812, 130.02271, 304.93607, 84, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Refugee") }));

		actors.Add(AddTrackActor(character, 57193, 549.11865, 162.4583, 709.63055, 44, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(579.76666f, 130.03270f, 563.90283f) }));
		actors.Add(AddTrackActor(character, 57192, 841.61285, 130.0327, 207.54842, 87, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(587.05231f, 130.03270f, 393.56433f) }));
		actors.Add(AddTrackActor(character, 57192, 787.44415, 130.02271, 260.29916, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(456.40231f, 130.02271f, 374.17734f) }));
		actors.Add(AddTrackActor(character, 57193, 814.35852, 130.0327, 202.93602, 61, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(553.55096f, 130.03270f, 336.36453f) }));
		actors.Add(AddTrackActor(character, 57192, 771.22968, 130.0327, 212.34544, 75, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(537.12018f, 130.03270f, 374.36612f) }));
		actors.Add(AddTrackActor(character, 40120, 233, 157, 724, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, 579.28333, 162.4583, 679.73444, 59, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(495.11145f, 130.02271f, 406.11945f) }));
		actors.Add(AddTrackActor(character, 57192, 690.33923, 130.02271, 306.6019, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(391.39587f, 130.02271f, 370.54623f) }));

		character.Movement.MoveTo(new Position(164.4886f, 151.7621f, 678.05426f));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 18:
				InsertTrackHate(character, track, 7);
				InsertTrackHate(character, track, 8);
				InsertTrackHate(character, track, 9);
				InsertTrackHate(character, track, 10);
				InsertTrackHate(character, track, 11);
				InsertTrackHate(character, track, 12);
				InsertTrackHate(character, track, 13);
				break;
			case 21:
				InsertTrackHate(character, track, 7);
				InsertTrackHate(character, track, 8);
				InsertTrackHate(character, track, 9);
				InsertTrackHate(character, track, 10);
				InsertTrackHate(character, track, 11);
				InsertTrackHate(character, track, 12);
				InsertTrackHate(character, track, 13);
				break;
			case 22:
				InsertTrackHate(character, track, 11);
				InsertTrackHate(character, track, 12);
				InsertTrackHate(character, track, 13);
				InsertTrackHate(character, track, 7);
				InsertTrackHate(character, track, 8);
				InsertTrackHate(character, track, 9);
				InsertTrackHate(character, track, 10);
				break;
			case 43:
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
			case 48:
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				break;
			case 49:
				RemoveTrackActor(character, track, 3);
				InsertTrackHate(character, track, 11);
				break;
			case 50:
				RemoveTrackActor(character, track, 6);
				break;
			case 51:
				InsertTrackHate(character, track, 7);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
