//--- Melia Script ----------------------------------------------------------
// The Scout's Ambush
//--- Description -----------------------------------------------------------
// Kepa roll down onto the scout's post on the western road.
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

[TrackScript("SIAUL_WEST_DRASIUS1_TRACK")]
public class SiaulWestDrasius1Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_DRASIUS1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1099f, 261f, -544f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 10046, -1121, 261, -528, 54, new TrackActorSpec
		{
			Name = L("Scout"),
			Faction = FactionType.Our_Forces,
			MaxHp = 99999,
			RunSpeed = 80,
			WalkSpeed = 60,
			CombatNpc = true,
		}));

		actors.Add(AddTrackActor(character, 400001, -1231, 261, -548, 17, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-1167f, 261f, -546f),
		}));

		actors.Add(AddTrackActor(character, 400001, -1243, 261, -563, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-1170f, 261f, -578f),
		}));

		actors.Add(AddTrackActor(character, 400001, -1263, 261, -517, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-1191f, 261f, -527f),
		}));

		actors.Add(AddTrackActor(character, 400001, -1261, 261, -570, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-1154f, 261f, -591f),
		}));

		actors.Add(AddTrackActor(character, 40070, -1277, 261, -614, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 21:
				InsertTrackHate(character, track, 2);
				break;
			case 25:
				InsertTrackHate(character, track, 3);
				break;
			case 27:
				InsertTrackHate(character, track, 4);
				break;
			case 28:
				InsertTrackHate(character, track, 5);
				break;
			case 29:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
