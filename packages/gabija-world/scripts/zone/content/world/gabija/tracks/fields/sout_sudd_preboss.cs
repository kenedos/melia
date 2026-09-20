//--- Melia Script ----------------------------------------------------------
// Chafer on the Valley Road
//--- Description -----------------------------------------------------------
// Chafer bursts from the ground and swats the Kepa and Jukopus ahead of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SOUT_SUDD_PREBOSS")]
public class SoutSuddPrebossTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SOUT_SUDD_PREBOSS");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1606.0817f, 42.836342f, -1760.5621f));

		actors.Add(AddTrackActor(character, 41320, -1257.7924, 37.428799, -1721.7395, 163, new TrackActorSpec { Ai = "BasicBoss", EndPosition = new Position(-1601.0605f, 42.826374f, -1733.7404f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 400003, -1433.7616, 38.95134, -1750.6017, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1443.0249f, 38.951347f, -1746.2064f) }));
		actors.Add(AddTrackActor(character, 400003, -1477.8879, 42.421009, -1747.7338, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400061, -1521.3625, 42.836319, -1759.181, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400061, -1523.328, 42.836353, -1734.1959, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400003, -1415.4871, 37.428799, -1740.358, 155, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400061, -1556.5969, 42.836349, -1744.272, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 6);
				break;
			case 11:
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 7);
				break;
			case 14:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
