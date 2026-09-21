//--- Melia Script ----------------------------------------------------------
// Taumas at the seal tower
//--- Description -----------------------------------------------------------
// A Demon Lord has come for the tower himself rather than send anything.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_2_SQ_02_TRACK")]
public class Siauliai462Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_2_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57567, 1075.16, -73.23, 4851.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Taumas") }));
		actors.Add(AddTrackActor(character, 147414, 1079, -73, 4705, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Seal Tower") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Demon Lord Taumas!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
