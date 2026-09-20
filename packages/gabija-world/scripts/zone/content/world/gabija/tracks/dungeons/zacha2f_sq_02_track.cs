//--- Melia Script ----------------------------------------------------------
// The Tomb Lord of the hidden place
//--- Description -----------------------------------------------------------
// The secret door opens onto the thing that was left to guard it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA2F_SQ_02_TRACK")]
public class Zacha2fSq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1544.08f, 649.36f, -531.62f));

		actors.Add(AddTrackActor(character, 57114, -1532.62, 659.08, -780.89, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
