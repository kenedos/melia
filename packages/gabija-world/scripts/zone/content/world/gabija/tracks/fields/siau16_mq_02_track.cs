//--- Melia Script ----------------------------------------------------------
// Kepas at the Malkos Felled Area
//--- Description -----------------------------------------------------------
// A swarm of Kepa rushes Settler Brophen while he waits for his family.
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

[TrackScript("SIAU16_MQ_02_TRACK")]
public class Siau16Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU16_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1556.59f, 65.41f, -1292.07f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 151083, -1540.68, 65.41, -1274.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Settler Brophen") }));

		actors.Add(AddTrackActor(character, 58005, -1496.27, 65.41, -1495.96, 32, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1518.33f, 65.41f, -1347.91f) }));
		actors.Add(AddTrackActor(character, 58005, -1512.75, 65.41, -1542.09, 31, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1509.89f, 65.41f, -1392.45f) }));
		actors.Add(AddTrackActor(character, 58005, -1450.13, 65.41, -1537.97, 27, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1479.73f, 65.41f, -1421.33f) }));
		actors.Add(AddTrackActor(character, 58005, -1542.18, 65.41, -1547.86, 34, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1553.27f, 65.41f, -1390.45f) }));
		actors.Add(AddTrackActor(character, 58005, -1488.91, 65.41, -1578.35, 30, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1525.88f, 65.41f, -1440.38f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Defeat the Kepas that rushed in!"), 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
