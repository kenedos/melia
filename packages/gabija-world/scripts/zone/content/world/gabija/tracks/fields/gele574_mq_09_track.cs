//--- Melia Script ----------------------------------------------------------
// Chasing Gesti into the Tenet Church
//--- Description -----------------------------------------------------------
// Algis joins you at the temple courtyard, and the gate is sealed behind her.
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

[TrackScript("GELE574_MQ_09_TRACK")]
public class Gele574Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE574_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(942.33f, -80.05f, 1144.88f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147371, 1255.23, -79.95, 1401.72, 32, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1300.89f, -79.65f, 1536.91f) }));
		actors.Add(AddTrackActor(character, 147353, 1300.31, -79.65, 1502.05, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 41382, 1271.94, -79.65, 2136.56, 49, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1299.88f, -79.65f, 2001.97f) }));
		actors.Add(AddTrackActor(character, 11281, 949.26, -80.05, 1187.92, 45, new TrackActorSpec
		{
			Name = L("Follower Algis"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			Level = 33,
			WalkSpeed = 70,
			CombatNpc = true,
		}));
		actors.Add(AddTrackActor(character, 147371, 1304.16, -79.65, 1999.78, 31, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1292.47f, -79.65f, 2130.51f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 23:
				RemoveTrackActor(character, track, 1);
				break;
			case 49:
				RemoveTrackActor(character, track, 2);
				break;
			case 74:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
