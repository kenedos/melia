//--- Melia Script ----------------------------------------------------------
// The Honeypin at Shirsie Sunny Place
//--- Description -----------------------------------------------------------
// The Honeypin that chased Riesz off is still standing over his bag.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_3_SQ_02_TRACK")]
public class Siauliai463Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_3_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57287, 3235.71, 210.81, 1045.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Honeypin") }));
		actors.Add(AddTrackActor(character, 47161, 3588, 211, 1212, 200, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Bag of Farming Tools") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				// The bag the cutscene knocks aside, before the fight is armed.
				RemoveTrackActor(character, track, 1);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Honeypin and take the bag back!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
