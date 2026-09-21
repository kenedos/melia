//--- Melia Script ----------------------------------------------------------
// The fourth magic suppressor
//--- Description -----------------------------------------------------------
// The device in the Reading Room and everything the demon left with it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_MQ_04_TRACK")]
public class Ftower45Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER45_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151003, -223, 246, 1036, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Magic Suppressor") }));
		actors.Add(AddTrackActor(character, 401623, -76.08, 246.84, 998.38, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47399, -61.97, 246.84, 1078.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47399, -229.78, 246.48, 957.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47395, -147.02, 246.84, 1139.92, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47395, -132.49, 246.68, 937.08, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
