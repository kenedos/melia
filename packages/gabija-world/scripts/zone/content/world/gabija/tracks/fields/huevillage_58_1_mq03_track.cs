//--- Melia Script ----------------------------------------------------------
// The villager on the lower path
//--- Description -----------------------------------------------------------
// Tipio and Tanu close on the man the Old Man sent to the sanctum.
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

[TrackScript("HUEVILLAGE_58_1_MQ03_TRACK")]
public class Huevillage581Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_1_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147407, -232, 233, -434, 72, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Injured Villager") }));
		actors.Add(AddTrackActor(character, 47480, -632.28772, 230.99055, -311.29541, 29, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-369.93671f, 230.9787f, -394.85544f) }));
		actors.Add(AddTrackActor(character, 47480, -668.43341, 231.08531, -343.64047, 24, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-422.63559f, 230.9787f, -400.90125f) }));
		actors.Add(AddTrackActor(character, 47480, -539.78363, 230.9787, -58.336296, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-387.87592f, 230.9787f, -244.21501f) }));
		actors.Add(AddTrackActor(character, 47480, -605.10675, 230.9787, -49.412037, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-434.74942f, 230.9787f, -286.69577f) }));
		actors.Add(AddTrackActor(character, 47472, -526.00671, 230.9787, -107.90873, 40, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-396.37827f, 230.9787f, -269.63904f) }));
		actors.Add(AddTrackActor(character, 47472, -587.97974, 230.9787, -96.207893, 42, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-466.38858f, 230.9787f, -289.99957f) }));
		actors.Add(AddTrackActor(character, 47472, -573.19324, 230.9787, -74.704643, 41, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-407.45673f, 230.9787f, -209.22961f) }));
		actors.Add(AddTrackActor(character, 47472, -561.82007, 230.9787, -22.559494, 41, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-398.68173f, 230.9787f, -159.50313f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
