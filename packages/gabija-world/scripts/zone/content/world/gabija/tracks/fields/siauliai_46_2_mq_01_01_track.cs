//--- Melia Script ----------------------------------------------------------
// The guardian stone of the arable land
//--- Description -----------------------------------------------------------
// The stone lights, and everything in the treeline comes at it at once.
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

[TrackScript("SIAULIAI_46_2_MQ_01_01_TRACK")]
public class Siauliai462Mq0101Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_2_MQ_01_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147413, -1954, 166, 3248, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Guardian Stone") }));
		actors.Add(AddTrackActor(character, 41442, -2246.64, 165.57, 3221.64, 74, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2103.36f, 165.57f, 3258.74f) }));
		actors.Add(AddTrackActor(character, 41442, -2258.21, 165.57, 3145.23, 74, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2095.59f, 165.57f, 3143.07f) }));
		actors.Add(AddTrackActor(character, 41442, -2293.52, 165.57, 3201.57, 82, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2146.29f, 165.57f, 3201.78f) }));
		actors.Add(AddTrackActor(character, 41442, -2312.91, 165.57, 3173.19, 61, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2168.98f, 165.57f, 3145.91f) }));
		actors.Add(AddTrackActor(character, 57219, -2335.34, 165.57, 3216.14, 83, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2155.33f, 165.57f, 3248.21f) }));
		actors.Add(AddTrackActor(character, 57219, -2361.39, 165.57, 3203.21, 70, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2216.37f, 165.57f, 3149.99f) }));
		actors.Add(AddTrackActor(character, 57219, -2378.64, 165.57, 3241.98, 88, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2205.15f, 165.57f, 3211.68f) }));
		actors.Add(AddTrackActor(character, 57219, -2406.98, 165.57, 3219.59, 79, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2267.02f, 165.57f, 3182.04f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Hold the guardian stone until the surge is broken!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
