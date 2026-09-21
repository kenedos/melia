//--- Melia Script ----------------------------------------------------------
// The signal fire
//--- Description -----------------------------------------------------------
// Rubblems break out of the walls as the signal is lit.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER41_MQ_01_TRACK")]
public class Ftower41Mq01Track : TrackScript
{
	private readonly static double[,] RubblemSpots =
	{
		{ -2252.12, -1920.12 }, { -2195.41, -1863.42 }, { -2130.69, -1933.52 },
		{ -2288.50, -1853.53 }, { -2173.28, -1814.11 }, { -2108.98, -1879.66 },
	};

	protected override void Load()
	{
		SetId("FTOWER41_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2187.06f, 1490.11f, -2110.72f));

		actors.Add(character);

		for (var i = 0; i < RubblemSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 57040, RubblemSpots[i, 0], 1491.53, RubblemSpots[i, 1], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat all the monsters!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
