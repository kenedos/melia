//--- Melia Script ----------------------------------------------------------
// The Minotaur at Flower Greeting Hill
//--- Description -----------------------------------------------------------
// Kayetonas meets the demon that fell from the sky.
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

[TrackScript("GELE573_MQ_06_TRACK")]
public class Gele573Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE573_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(319.72, 281.96, 557.22));

		actors.Add(AddTrackActor(character, 41383, 245.54, 284.17, 998.78, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 11283, 265, 281, 545, 72, new TrackActorSpec
		{
			Name = L("Follower Kayetonas"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			Level = 25,
			CombatNpc = true,
			EndPosition = new Position(258.73, 284.17, 913.45),
		}));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 13:
				InsertTrackHate(character, track, 0);
				break;
			case 49:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
