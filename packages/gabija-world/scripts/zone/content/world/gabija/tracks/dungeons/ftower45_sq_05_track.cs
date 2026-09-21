//--- Melia Script ----------------------------------------------------------
// The Stone Whale wakes
//--- Description -----------------------------------------------------------
// The last of Hauberk's watchers turns over and comes for the road.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_SQ_05_TRACK")]
public class Ftower45Sq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER45_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57091, -37.75, 150.03, 1544.60, 57, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(29.40f, 150.36f, 1522.48f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 27:
				SetTrackTendency(character, track);
				break;
			case 28:
				CreateBattleBoxInLayer(character, track);
				break;
			case 29:
				character.ServerMessage(L("The Stone Whale guarding Hauberk has woken up. Defeat the Stone Whale!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
