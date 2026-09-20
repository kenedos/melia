//--- Melia Script ----------------------------------------------------------
// Retaking the Supply Depot
//--- Description -----------------------------------------------------------
// Chupacabra swarm the supply depot while carts and stacked crates stand about.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_EAST_RECLAIM3_TRACK")]
public class SiaulEastReclaim3Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_RECLAIM3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 400961, 565.2502, 130.0327, -277.366, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 623.066, 130.0327, -283.1199, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 586.2847, 130.0327, -319.8761, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 590.2128, 130.0327, -354.5081, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 650.1762, 130.0327, -343.652, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 635.0713, 130.0227, -462.1695, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 674.4563, 130.0327, -495.4765, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 719.7477, 130.0327, -448.0863, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 728.7802, 130.0327, -510.7052, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 691.7792, 130.0327, -536.3315, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		actors.Add(AddTrackActor(character, 45315, 484.8353, 130.0227, -509.8344, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 45315, 480.3688, 130.0227, -471.7313, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 45315, 579.6265, 130.0227, -454.002, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		actors.Add(AddTrackActor(character, 20047, 579.3231, 130.0227, -448.8316, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 20047, 484.5941, 130.0227, -512.1528, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 20047, 483.5241, 130.0227, -472.4545, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
