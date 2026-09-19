//--- Melia Script ----------------------------------------------------------
// The Unknocker under the church
//--- Description -----------------------------------------------------------
// Tomas lures the beast away from the altar.
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

[TrackScript("CHAPLE575_MQ_04_TRACK")]
public class Chaple575Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE575_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-468.49f, -39.42f, 581.33f));

		actors.Add(AddTrackActor(character, 147358, -602.02, -17.56, 422.40, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Underground Central Barrier") }));
		actors.Add(AddTrackActor(character, 41371, -572.24, -17.57, 418.33, 0, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 11282, -489, -37, 618, 0, new TrackActorSpec
		{
			Name = L("Follower Tomas"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			Level = 37,
			CombatNpc = true,
			EndPosition = new Position(-542.74f, -39.42f, 605.62f),
		}));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 44:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
