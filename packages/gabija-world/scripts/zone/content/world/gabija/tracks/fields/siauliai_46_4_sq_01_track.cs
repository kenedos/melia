//--- Melia Script ----------------------------------------------------------
// The Sparnas over the apiary
//--- Description -----------------------------------------------------------
// Whatever opened the comb is still sitting on the Rododun Apiary.
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

[TrackScript("SIAULIAI_46_4_SQ_01_TRACK")]
public class Siauliai464Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_4_SQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57284, 747.89, 148.22, -706.14, 86, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Sparnas"), EndPosition = new Position(915.74f, 148.22f, -740.84f) }));
		actors.Add(AddTrackActor(character, 151025, 1370, 149, -695, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Honeycomb") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Sparnas!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
