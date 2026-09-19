//--- Melia Script ----------------------------------------------------------
// Demons of the Closed Area
//--- Description -----------------------------------------------------------
// A swarm of bats bursts off the sealed crystal pillar, and the demon mirtis
// claws its way out behind them.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_3_BOSS_TRACK")]
public class Mine3BossTrack : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_3_BOSS_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2114.7554f, 56.932098f, 1761.942f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 47233, 2048.03, 56.93, 1753.45, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 20025, 2097.8989, 56.932098, 1657.4999, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1734.2827, 56.932098, 1671.7023, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1739.7993, 56.932098, 1638.14, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1740.7522, 56.932098, 1624.3708, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1777.005, 56.932098, 1523.7927, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1773.1691, 56.932098, 1515.7732, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1756.1017, 56.932098, 1500.333, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1741.385, 56.932098, 1503.693, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1716.9059, 56.932098, 1512.8732, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1697.2289, 56.932098, 1504.7197, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1663.9558, 56.932098, 1614.9406, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1763.3859, 56.932098, 1593.2794, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1652.4214, 57.390385, 1634.5442, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1599.6975, 104.28435, 1662.4569, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1615.5145, 77.26265, 1636.4456, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1570.7455, 127.38148, 1703.672, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 1690.9602, 56.932098, 1461.9296, 0, prop));
		actors.Add(AddTrackActor(character, 20025, 1614.1042, 85.12413, 1646.1803, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2125.824, 56.932098, 1623.8081, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2196.0283, 56.932098, 1623.5662, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2156.0264, 56.932098, 1620.8489, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2138.4165, 56.932098, 1581.6838, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2143.3433, 56.932098, 1673.27, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2173.7219, 56.932098, 1664.6318, 0, prop));
		actors.Add(AddTrackActor(character, 57801, 2137.9468, 56.932098, 1633.8052, 0, prop));
		actors.Add(AddTrackActor(character, 20025, 2086.342, 56.932098, 1663.5641, 0, prop));
		actors.Add(AddTrackActor(character, 20025, 2099.4365, 56.932098, 1686.4833, 0, prop));
		actors.Add(AddTrackActor(character, 20025, 2160.7832, 56.932098, 1689.0011, 0.85106385, prop));
		actors.Add(AddTrackActor(character, 400401, 2156.0671, 56.932098, 1675.913, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20025, 2150.6357, 56.932098, 1681.2111, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 20:
				RemoveTrackActor(character, track, 3);
				break;
			case 25:
				RemoveTrackActor(character, track, 5);
				break;
			case 35:
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 8);
				RemoveTrackActor(character, track, 9);
				RemoveTrackActor(character, track, 10);
				RemoveTrackActor(character, track, 11);
				RemoveTrackActor(character, track, 12);
				RemoveTrackActor(character, track, 13);
				RemoveTrackActor(character, track, 14);
				RemoveTrackActor(character, track, 15);
				RemoveTrackActor(character, track, 16);
				RemoveTrackActor(character, track, 17);
				RemoveTrackActor(character, track, 18);
				break;
			case 44:
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 20);
				RemoveTrackActor(character, track, 21);
				RemoveTrackActor(character, track, 22);
				RemoveTrackActor(character, track, 23);
				RemoveTrackActor(character, track, 24);
				RemoveTrackActor(character, track, 25);
				RemoveTrackActor(character, track, 26);
				break;
			case 64:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 18);
				RemoveTrackActor(character, track, 19);
				RemoveTrackActor(character, track, 27);
				RemoveTrackActor(character, track, 28);
				RemoveTrackActor(character, track, 29);
				RemoveTrackActor(character, track, 31);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
