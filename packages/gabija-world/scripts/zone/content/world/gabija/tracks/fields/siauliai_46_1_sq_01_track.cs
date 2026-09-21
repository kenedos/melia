//--- Melia Script ----------------------------------------------------------
// The Chafer behind the altar
//--- Description -----------------------------------------------------------
// What was moving behind the Austeja Altar comes round the front of it.
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

[TrackScript("SIAULIAI_46_1_SQ_01_TRACK")]
public class Siauliai461Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_1_SQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151024, -130, 325, 10, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Austeja Altar") }));
		actors.Add(AddTrackActor(character, 57288, -228.08, 324.02, 343.13, 193, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Chafer"), EndPosition = new Position(-199.10f, 324.02f, 191.98f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Chafer!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
