//--- Melia Script ----------------------------------------------------------
// Behind the reading room shelf
//--- Description -----------------------------------------------------------
// Ginklas comes out of the bookshelf Cordelier asked about.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER41_SQ_05_TRACK")]
public class Ftower41Sq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER41_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1245.25f, 1602.25f, -896.06f));

		actors.Add(AddTrackActor(character, 57063, 1401.34, 1605.47, -995.72, 87, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
