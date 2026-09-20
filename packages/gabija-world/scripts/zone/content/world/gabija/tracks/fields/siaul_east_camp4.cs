//--- Melia Script ----------------------------------------------------------
// The Camp in Danger
//--- Description -----------------------------------------------------------
// A Poata charges the outpost to protect its cub, with the camp's guards
// standing their ground in front of the player.
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

[TrackScript("SIAUL_EAST_CAMP4_TRACK")]
public class SiaulEastCamp4Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_CAMP4_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 10032, 132, 151.76, 671, 0, new TrackActorSpec
		{
			Ai = "BasicMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			CombatNpc = true,
		}));

		actors.Add(AddTrackActor(character, 10032, 204, 151.76, 666, 5, new TrackActorSpec
		{
			Ai = "BasicMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			CombatNpc = true,
		}));

		actors.Add(AddTrackActor(character, 41202, 621.6195, 130.0227, 364.5, 110, new TrackActorSpec
		{
			Ai = "BasicBoss",
		}));

		actors.Add(AddTrackActor(character, 11284, 166, 151.76, 696, 37, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			CombatNpc = true,
		}));

		actors.Add(AddTrackActor(character, 40120, 163.3164, 151.7621, 805.4705, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			Faction = FactionType.Our_Forces,
		}));

		actors.Add(AddTrackActor(character, 41248, 353.1152, 130.0227, 509.4945, 187, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
		}));

		character.Movement.MoveTo(new Position(167.5479f, 151.7621f, 664.7233f));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 53:
				RemoveTrackActor(character, track, 5);
				break;
			case 74:
				RemoveTrackActor(character, track, 5);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 3);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
