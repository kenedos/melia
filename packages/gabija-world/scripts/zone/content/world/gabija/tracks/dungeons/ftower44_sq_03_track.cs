//--- Melia Script ----------------------------------------------------------
// The machinery room
//--- Description -----------------------------------------------------------
// Yonazolem comes down the machinery room while Furry Odd holds the door.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER44_SQ_03_TRACK")]
public class Ftower44Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER44_SQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1785.94f, 505.48f, 506.59f));

		actors.Add(AddTrackActor(character, 57420, 1806.91, 506.54, 776.62, 33, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1793.74f, 503.47f, 626.95f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 27:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Yonazolem while Furry Odd blocks the entrance!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
