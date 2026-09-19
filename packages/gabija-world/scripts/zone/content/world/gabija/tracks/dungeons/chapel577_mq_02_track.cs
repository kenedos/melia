//--- Melia Script ----------------------------------------------------------
// Recapturing the Bell Tower
//--- Description -----------------------------------------------------------
// The Necroventer holds the tower. Take it back.
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

[TrackScript("CHAPLE577_MQ_02_TRACK")]
public class Chaple577Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE577_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-633.69f, 35.92f, -962.92f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147352, 134, 165, -576, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147358, -30.72, 35.93, -165.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Central Altar") }));
		actors.Add(AddTrackActor(character, 11281, -633, 36, -934, 90, new TrackActorSpec
		{
			Name = L("Follower Algis"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			Level = 41,
			WalkSpeed = 70,
			CombatNpc = true,
			EndPosition = new Position(-623.87f, 35.92f, -981.62f),
		}));
		actors.Add(AddTrackActor(character, 152003, 207.31, 164.86, -582.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41230, -560, 36, -900, 0, new TrackActorSpec { Ai = "BasicBoss" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 15:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
