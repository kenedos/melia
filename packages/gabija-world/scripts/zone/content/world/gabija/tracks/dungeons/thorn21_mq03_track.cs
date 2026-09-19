//--- Melia Script ----------------------------------------------------------
// Gaigalas over the Sviesa Hill root
//--- Description -----------------------------------------------------------
// Touching the root brings its keeper down the hill.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ03_TRACK")]
public class Thorn21Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41220, 2478.4407, 122.01666, -1192.073, 7, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153011, 2800, 122, -1325, 77, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Bramble's Root") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
