//--- Melia Script ----------------------------------------------------------
// The second magic suppressor
//--- Description -----------------------------------------------------------
// The device in the Reception Room and the Black Shaman Dolls around it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_MQ_02_TRACK")]
public class Ftower45Mq02Track : TrackScript
{
	private readonly static double[,] PuppetSpots =
	{
		{ -523.47, 467.36, -600.17 }, { -589.27, 467.36, -590.76 },
		{ -653.10, 467.00, -648.89 }, { -644.17, 467.00, -704.86 },
	};

	protected override void Load()
	{
		SetId("FTOWER45_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151003, -501, 467, -745, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Magic Suppressor") }));

		for (var i = 0; i < PuppetSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 47399, PuppetSpots[i, 0], PuppetSpots[i, 1], PuppetSpots[i, 2], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
