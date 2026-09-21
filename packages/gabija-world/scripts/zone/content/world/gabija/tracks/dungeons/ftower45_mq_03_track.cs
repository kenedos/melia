//--- Melia Script ----------------------------------------------------------
// The third magic suppressor
//--- Description -----------------------------------------------------------
// The device in the Small Hall and the Black Drakes circling it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_MQ_03_TRACK")]
public class Ftower45Mq03Track : TrackScript
{
	private readonly static double[,] DrakeSpots =
	{
		{ -74.80, 383.59, -104.61 }, { -41.83, 387.00, 151.29 }, { 139.00, 382.00, 33.98 },
	};

	protected override void Load()
	{
		SetId("FTOWER45_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151003, -12, 378, 26, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Magic Suppressor") }));

		for (var i = 0; i < DrakeSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 401623, DrakeSpots[i, 0], DrakeSpots[i, 1], DrakeSpots[i, 2], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
