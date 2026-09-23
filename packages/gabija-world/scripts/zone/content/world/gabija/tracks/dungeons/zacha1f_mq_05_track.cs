//--- Melia Script ----------------------------------------------------------
// Guardian Achat
//--- Description -----------------------------------------------------------
// The corrupted guardian rises out of its pieces at the far end of the floor.
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

[TrackScript("ZACHA1F_MQ_05_TRACK")]
public class Zacha1fMq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(47.78f, 327.84f, 1081.31f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147348, 48.34, 289.69, 1438.44, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 41240, 48, 289, 1438, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Guardian Achat") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				// The pieces Achat rises out of.
				RemoveTrackActor(character, track, 1);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
