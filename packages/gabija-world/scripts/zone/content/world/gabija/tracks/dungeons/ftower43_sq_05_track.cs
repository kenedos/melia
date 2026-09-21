//--- Melia Script ----------------------------------------------------------
// The table's warning
//--- Description -----------------------------------------------------------
// The Gray Golem the suspicious table was hiding from comes around the corner.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER43_SQ_05_TRACK")]
public class Ftower43Sq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER43_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1357.15f, 323.61f, -192.30f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57120, 1138.65, 353.83, -71.72, 48, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1267.77f, 323.61f, -108.53f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				track.Dialog.SetTitle(L("Suspicious Table"));
				StartDialog(track, L("Look out! Behind you!"));
				break;
			case 19:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
