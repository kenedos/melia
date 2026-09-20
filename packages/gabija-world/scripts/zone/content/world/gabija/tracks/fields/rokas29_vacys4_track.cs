//--- Melia Script ----------------------------------------------------------
// Unknocker at Neck Cliff of Snake
//--- Description -----------------------------------------------------------
// Unknocker comes up out of the cliff over Varkis' second cache.
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

[TrackScript("ROKAS29_VACYS4_TRACK")]
public class Rokas29Vacys4Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_VACYS4_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1721.08f, 784.01f, 698.58f));

		actors.Add(AddTrackActor(character, 57097, -1885.70, 784.01, 901.06, 98, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 155026, -1746.23, 784.01, 700.19, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				character.ServerMessage(L("Unknocker has appeared! It may be holding the research materials - put it down!"));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
