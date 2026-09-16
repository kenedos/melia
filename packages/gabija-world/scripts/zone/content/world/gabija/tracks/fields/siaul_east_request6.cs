//--- Melia Script ----------------------------------------------------------
// The Vubbe Fighter at Nudegi
//--- Description -----------------------------------------------------------
// The scout and the player close in on the Vubbe Fighter at the logging camp.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_EAST_REQUEST6_TRACK")]
public class SiaulEastRequest6Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_REQUEST6_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2107.0833f, 185.09489f, -69.346664f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 400201, 2067.6062, 185.09489, -169.27565, 50, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, 2081.7092, 185.09489, -448.51422, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, 1985.8104, 185.09489, -330.603, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, 1920.6758, 185.09489, -361.58221, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, 2116.1965, 185.09489, -387.33612, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 39:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
