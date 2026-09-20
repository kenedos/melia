//--- Melia Script ----------------------------------------------------------
// The third mark on the treasure map
//--- Description -----------------------------------------------------------
// The chest on the lower road, with a Hogma patrol already on it.
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

[TrackScript("ROKAS30_PIPOTI04_TRACK")]
public class Rokas30Pipoti04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_PIPOTI04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147392, 36, 215, -310, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));
		actors.Add(AddTrackActor(character, 41433, -169.25, 215.74, -283.98, 40, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-72.97f, 215.75f, -356.28f) }));
		actors.Add(AddTrackActor(character, 41433, -180.19, 215.74, -198.17, 21, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-110.33f, 215.75f, -252.82f) }));
		actors.Add(AddTrackActor(character, 41435, -151.07, 215.74, -149.33, 47, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-30.46f, 215.75f, -222.86f) }));
		actors.Add(AddTrackActor(character, 41433, -121.96, 215.74, -259.64, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-69.29f, 215.75f, -317.34f) }));
		actors.Add(AddTrackActor(character, 41433, -111.93, 215.74, -196.84, 89, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-38.98f, 215.75f, -275.91f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				character.ServerMessage(L("Another chest, and company with it! Get past the monsters and look inside."));
				CreateBattleBoxInLayer(character, track);
				break;
			case 19:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
