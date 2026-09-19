//--- Melia Script ----------------------------------------------------------
// Moldihorn in Nefrito Valley
//--- Description -----------------------------------------------------------
// What was moving with the current comes up out of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("HUEVILLAGE_58_2_SQ01_TRACK")]
public class Huevillage582Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_2_SQ01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(80.326218f, -54.431656f, 649.87933f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 47323, 296.74927, -66.805687, 667.21967, 248, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 31:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
