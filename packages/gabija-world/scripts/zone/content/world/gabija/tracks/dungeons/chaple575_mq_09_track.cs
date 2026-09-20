//--- Melia Script ----------------------------------------------------------
// Cyclops at the basement barrier
//--- Description -----------------------------------------------------------
// Vaidutis runs for the first floor while the Revelator breaks through.
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

[TrackScript("CHAPLE575_MQ_09_TRACK")]
public class Chaple575Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE575_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(305.78f, 0.55f, -789.04f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57087, 487.85, 0.55, -851.05, 280, new TrackActorSpec { Ai = "BasicBoss", EndPosition = new Position(596.80f, 0.55f, -635.36f) }));
		actors.Add(AddTrackActor(character, 11283, 499.52, 0.55, -795.50, 69, new TrackActorSpec
		{
			Name = L("Follower Vaidutis"),
			Faction = FactionType.Neutral,
			EndPosition = new Position(661.91f, 76.14f, -454.12f),
		}));
		actors.Add(AddTrackActor(character, 12082, 364.88, 0.55, -790.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 41:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				break;
			case 44:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
