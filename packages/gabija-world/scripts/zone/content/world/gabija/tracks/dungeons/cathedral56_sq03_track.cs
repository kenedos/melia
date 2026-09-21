//--- Melia Script ----------------------------------------------------------
// The false secret of Maskuote Narthex
//--- Description -----------------------------------------------------------
// What the priests took for one of Maven's machines uncoils into a Linkroller.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL56_SQ03_TRACK")]
public class Cathedral56Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL56_SQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		// The desk itself stands on the map and is not spawned again here.
		actors.Add(AddTrackActor(character, 41232, -2109.80, 0.50, -434.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat the Linkroller that was hiding as one of Maven's secrets!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
