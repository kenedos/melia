//--- Melia Script ----------------------------------------------------------
// The raid on the Holy Symbol of Spiritual Power
//--- Description -----------------------------------------------------------
// Naktis' servants break the reading desk apart and turn on whoever walks in
// on them.
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

[TrackScript("CHATHEDRAL54_MQ03_PART2_TRACK")]
public class Cathedral54Mq03Part2Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL54_MQ03_PART2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1211.58f, 3.85f, 955.81f));

		actors.Add(AddTrackActor(character, 47254, -882.84, 3.85, 991.86, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Reading Desk") }));
		actors.Add(AddTrackActor(character, 57367, -808.85, 3.85, 996.82, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -922.63, 3.85, 978.40, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 2 }));
		actors.Add(AddTrackActor(character, 57367, -906.21, 3.85, 1040.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 2 }));
		actors.Add(AddTrackActor(character, 57367, -895.96, 3.85, 886.45, 16, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -1010.59, 3.85, 964.97, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -895.70, 3.85, 936.84, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -981.78, 3.85, 899.54, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -817.13, 3.85, 908.21, 20, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57367, -953.48, 3.85, 1024.26, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 151001, -877.76, 3.85, 992.09, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Holy Symbol of Spiritual Power") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				// The desk the cutscene breaks apart, before the rest is armed.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Drive the demons off the Holy Symbol of Spiritual Power!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
