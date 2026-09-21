//--- Melia Script ----------------------------------------------------------
// The Biteregina in the warehouse
//--- Description -----------------------------------------------------------
// The smell of the mead boxes carried further than the warehouse walls.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_4_SQ_02_TRACK")]
public class Siauliai464Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_4_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57285, 1478.43, 121.37, 2830.71, 101, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Biteregina"), EndPosition = new Position(1356.70f, 121.37f, 2723.80f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Biteregina!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
