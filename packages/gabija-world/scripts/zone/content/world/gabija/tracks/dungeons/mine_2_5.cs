//--- Melia Script ----------------------------------------------------------
// The Stone Whale of District 6
//--- Description -----------------------------------------------------------
// The Stone Whale that dragged off the main purifier's parts is lying on
// them, and rouses as the player comes for them.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("mine_2_5")]
public class Mine25Track : TrackScript
{
	protected override void Load()
	{
		SetId("mine_2_5");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1466.6749f, -29.577721f, -566.56805f));

		actors.Add(AddTrackActor(character, 41209, 1589.3827, -29.253149, -672.43201, 11));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
