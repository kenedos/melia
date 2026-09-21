//--- Melia Script ----------------------------------------------------------
// The Cyclops of the northern altar
//--- Description -----------------------------------------------------------
// Whatever flattened the ground around the Gaudeji Altar is still standing on
// the north side of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_3_SQ_01_TRACK")]
public class Siauliai463Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_3_SQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57286, 269.77, 24.31, 1523.31, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Cyclops") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Cyclops!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
