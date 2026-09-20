//--- Melia Script ----------------------------------------------------------
// The Mysterious Slate
//--- Description -----------------------------------------------------------
// With Mirtis gone the crystal pillar splits open and gives up the slate it
// was holding.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_3_BOSS_2boss")]
public class Mine3Boss2BossTrack : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_3_BOSS_2boss");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2116.4717f, 56.932098f, 1725.8176f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 47233, 2048.03, 56.93, 1753.45, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47234, 2042.0284, 56.932098, 1745.1373, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 27:
				RemoveTrackActor(character, track, 1);
				break;
			case 43:
				RemoveTrackActor(character, track, 2);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
