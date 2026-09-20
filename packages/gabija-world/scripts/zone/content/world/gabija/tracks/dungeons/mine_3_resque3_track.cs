//--- Melia Script ----------------------------------------------------------
// Netherbovine's Attack
//--- Description -----------------------------------------------------------
// A Netherbovine answers the mine crystal and comes up out of the dark,
// drawn by the red crystal the Vubbe broke open.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_3_RESQUE3_TRACK")]
public class Mine3Resque3Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_3_RESQUE3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-665.3248f, 181.6071f, -19.90883f));

		actors.Add(AddTrackActor(character, 41237, -698.4034, 182.6882, -284.2242, 0));
		actors.Add(AddTrackActor(character, 151013, -659.2715, 181.6068, 4.320052, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 36:
				RemoveTrackActor(character, track, 1);
				break;
			case 39:
				RemoveTrackActor(character, track, 1);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
