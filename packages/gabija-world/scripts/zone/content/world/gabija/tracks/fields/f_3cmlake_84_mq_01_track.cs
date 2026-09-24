//--- Melia Script ----------------------------------------------------------
// Ambush in the Nesuga Small Corridor
//--- Description -----------------------------------------------------------
// Blue Slimes spill out of the black hoods' laboratory.
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

[TrackScript("F_3CMLAKE_84_MQ_01_TRACK")]
public class F3Cmlake84Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("F_3CMLAKE_84_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-188.65f, 274.47f, 1780.52f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 153132, -209.99, 274.47, 1803.09, 0, prop));
		actors.Add(AddTrackActor(character, 58104, -254.51, 274.47, 1633.36, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-258.69f, 274.47f, 1674.46f) }));
		actors.Add(AddTrackActor(character, 58104, -310.41, 274.47, 1671.98, 46, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-308.73f, 274.47f, 1726.73f) }));
		actors.Add(AddTrackActor(character, 58104, -67.01, 274.47, 1842.42, 46, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-131.23f, 274.47f, 1835.53f) }));
		actors.Add(AddTrackActor(character, 58104, -43.52, 274.47, 1799.91, 31, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-80.41f, 274.47f, 1795.01f) }));
		actors.Add(AddTrackActor(character, 58104, -33.99, 274.47, 1747.39, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-114.50f, 274.47f, 1758.70f) }));
		actors.Add(AddTrackActor(character, 58104, -130.48, 274.47, 1642.04, 57, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-168.82f, 274.47f, 1700.42f) }));
		actors.Add(AddTrackActor(character, 57013, -117.39, 274.47, 1712.76, 0, prop));
		actors.Add(AddTrackActor(character, 152040, -209.10, 274.47, 1703.00, 0, prop));
		actors.Add(AddTrackActor(character, 153131, -144.08, 274.47, 1834.27, 0, prop));
		actors.Add(AddTrackActor(character, 153131, -91.92, 274.47, 1840.80, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				break;

			case 9:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Monsters are attacking!"), 3);
				break;

			case 14:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
