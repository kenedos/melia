//--- Melia Script ----------------------------------------------------------
// The Vubbe Base
//--- Description -----------------------------------------------------------
// A Vubbe camp drums and cooks outside the village, then turns on the player.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SOUT_Q_13_TRACK")]
public class SoutQ13Track : TrackScript
{
	protected override void Load()
	{
		SetId("SOUT_Q_13_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 11120, 1864.6669, 147.36159, 381.16547, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 11120, 1842.3387, 147.36159, 448.75406, 75, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 57266, 1939.2345, 147.35159, 337.00311, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 70 }));
		actors.Add(AddTrackActor(character, 41393, 1811.429, 147.36159, 271.83566, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41393, 1841.8293, 147.36159, 449.9783, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 41394, 1998.8926, 147.36159, 310.88181, 34, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 50, EndPosition = new Position(1899.7725f, 147.36159f, 334.64713f) }));
		actors.Add(AddTrackActor(character, 41394, 1782.4021, 147.36159, 350.03671, 38, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 20, EndPosition = new Position(1717.7146f, 147.36159f, 177.68526f) }));
		actors.Add(AddTrackActor(character, 41393, 1880.7627, 147.36159, 243.27626, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 11120, 1898.1774, 147.36159, 333.91632, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 11120, 1811.8148, 147.36159, 268.48898, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 57266, 1799.2059, 147.36159, 320.52585, 82, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 11120, 1878.4175, 147.36159, 247.15431, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 60 }));
		actors.Add(AddTrackActor(character, 57266, 1924.4874, 147.35159, 389.44876, 0, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 70 }));
		actors.Add(AddTrackActor(character, 41394, 1786.9285, 147.36159, 419.60352, 45, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 30, EndPosition = new Position(1664.4487f, 147.36159f, 239.54556f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 23:
				RemoveTrackActor(character, track, 13);
				break;
			case 24:
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 6);
				break;
			case 26:
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 7);
				break;
			case 27:
				RemoveTrackActor(character, track, 5);
				break;
			case 28:
				character.ServerMessage(L("Defeat the Vubbes marching in!"));
				break;
			case 29:
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 13);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
