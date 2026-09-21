//--- Melia Script ----------------------------------------------------------
// The first transport circle
//--- Description -----------------------------------------------------------
// Grita reads the circle while the floor's monsters close in.
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

[TrackScript("FTOWER41_MQ_02_TRACK")]
public class Ftower41Mq02Track : TrackScript
{
	private readonly static double[,] PhyraconSpots =
	{
		{ -1860, -1250 }, { -1750, -1180 }, { -1480, -1200 },
		{ -1370, -1300 }, { -1400, -1560 }, { -1520, -1640 },
	};

	private readonly static double[,] DrakeSpots =
	{
		{ -1880, -1420 }, { -1810, -1560 }, { -1680, -1660 }, { -1550, -1180 },
		{ -1330, -1440 }, { -1740, -1300 }, { -1460, -1660 }, { -1900, -1560 },
	};

	protected override void Load()
	{
		SetId("FTOWER41_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1472.34f, 1552.73f, -1384.10f));

		actors.Add(AddTrackActor(character, 147449, -1605.86, 1552.73, -1465.50, 28, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Grita"), EndPosition = new Position(-1610.51f, 1552.73f, -1426.80f) }));
		actors.Add(AddTrackActor(character, 147500, -1611.45, 1552.73, -1402.07, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("1st Transport Magic Circle") }));
		actors.Add(character);

		// The client plays this phase as a minigame; the monsters its notice
		// describes are spawned into the track's own layer instead.
		for (var i = 0; i < PhyraconSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 47397, PhyraconSpots[i, 0], 1552.73, PhyraconSpots[i, 1], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		for (var i = 0; i < DrakeSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 401621, DrakeSpots[i, 0], 1552.73, DrakeSpots[i, 1], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat the monsters interrupting Grita while she reads the magic circle!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
