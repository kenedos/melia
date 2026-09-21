//--- Melia Script ----------------------------------------------------------
// Helgasercle in the Great Hall
//--- Description -----------------------------------------------------------
// The demon lord who took the tower turns on whoever broke her suppressors.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_MQ_05_TRACK")]
public class Ftower45Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER45_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(764.71f, 250.37f, 1925.55f));

		actors.Add(AddTrackActor(character, 41228, 835.00, 254.17, 2310.31, 66, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 37:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Helgasercle, who is occupying the Mage Tower!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
