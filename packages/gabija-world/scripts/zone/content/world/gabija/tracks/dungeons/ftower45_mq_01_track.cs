//--- Melia Script ----------------------------------------------------------
// The first magic suppressor
//--- Description -----------------------------------------------------------
// The device in the Hall of Fire and the Dimmers set to keep it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER45_MQ_01_TRACK")]
public class Ftower45Mq01Track : TrackScript
{
	private readonly static double[,] DimmerSpots =
	{
		{ -570.01, 270.56, -1200.92 }, { -648.60, 270.23, -1242.41 },
		{ -664.06, 270.23, -1204.41 }, { -523.61, 270.48, -1237.72 },
	};

	protected override void Load()
	{
		SetId("FTOWER45_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-574.93f, 286.94f, -1139.60f));

		actors.Add(AddTrackActor(character, 151003, -580, 282, -1090, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Magic Suppressor") }));

		for (var i = 0; i < DimmerSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 47395, DimmerSpots[i, 0], DimmerSpots[i, 1], DimmerSpots[i, 2], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		actors.Add(character);

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
