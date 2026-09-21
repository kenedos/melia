//--- Melia Script ----------------------------------------------------------
// Bearkaras at the Small Hall
//--- Description -----------------------------------------------------------
// What Simon Shaw chased and could not handle comes back down the hall.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_SQ_03_TRACK")]
public class Ftower45Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER45_SQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1389.50f, 558.37f, -173.24f));

		actors.Add(AddTrackActor(character, 57082, -1711.25, 606.92, -191.94, 97, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1556.21f, 606.92f, -196.41f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat Bearkaras for Simon Shaw!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
