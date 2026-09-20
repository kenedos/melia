//--- Melia Script ----------------------------------------------------------
// The Beholder of Sesija Entrance
//--- Description -----------------------------------------------------------
// The guardian device calls Bearkaras up out of the ruins.
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

[TrackScript("ROKAS31_REXITHER1_TRACK")]
public class Rokas31Rexither1Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS31_REXITHER1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57080, 376.24, 107.10, -948.39, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(235.80f, 107.10f, -828.62f) }));
		actors.Add(AddTrackActor(character, 401041, 322.55, 107.10, -962.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401041, 414.06, 107.10, -897.24, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401041, 251.88, 107.10, -907.29, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401041, 318.77, 107.10, -829.15, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
