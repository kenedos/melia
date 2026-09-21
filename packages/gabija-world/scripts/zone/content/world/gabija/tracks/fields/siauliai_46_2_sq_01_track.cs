//--- Melia Script ----------------------------------------------------------
// What the mutated plant was hiding
//--- Description -----------------------------------------------------------
// The grass comes apart and a Golem stands up out of the middle of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_2_SQ_01_TRACK")]
public class Siauliai462Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_2_SQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47203, 1075, 6, 5290, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Giant Mutated Plant") }));
		actors.Add(AddTrackActor(character, 450221, 1075, 6, 5290, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Golem") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				// The grass the Golem tears apart on its way up.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Golem!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
