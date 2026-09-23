//--- Melia Script ----------------------------------------------------------
// The Warehouse No. 1 Secret Device
//--- Description -----------------------------------------------------------
// The demons of the Storage gather around the device holding the King's
// Red Jewel.
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

[TrackScript("PRISON_79_MQ_8_TRACK")]
public class Prison79Mq8Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_79_MQ_8_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1469.10f, 475.61f, 1922.78f));

		actors.Add(AddTrackActor(character, 151108, -1764.00, 402.36, 1951.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57983, -1600.35, 475.61, 1693.79, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1589.78f, 475.61f, 1698.64f) }));
		actors.Add(AddTrackActor(character, 57983, -1606.69, 475.61, 1625.96, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1633.42f, 475.61f, 1650.73f) }));
		actors.Add(AddTrackActor(character, 57983, -1687.98, 475.61, 1658.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1706.33f, 475.61f, 1665.79f) }));
		actors.Add(AddTrackActor(character, 57983, -1770.94, 402.36, 1856.75, 44, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1718.58f, 402.36f, 1903.13f) }));
		actors.Add(AddTrackActor(character, 57983, -1790.13, 402.36, 2032.59, 25, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1777.33f, 402.36f, 1999.25f) }));
		actors.Add(AddTrackActor(character, 57983, -1673.74, 402.36, 2079.62, 93, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1715.06f, 402.36f, 1996.48f) }));
		actors.Add(AddTrackActor(character, 57932, -1839.22, 402.36, 1904.15, 27, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1812.34f, 402.36f, 1908.75f) }));
		actors.Add(AddTrackActor(character, 57932, -1750.29, 475.61, 1704.41, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1717.66f, 475.61f, 1725.78f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 13:
				character.ServerMessage(L("Monsters are gathering around the secret device. Defeat all of them."));
				break;

			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
