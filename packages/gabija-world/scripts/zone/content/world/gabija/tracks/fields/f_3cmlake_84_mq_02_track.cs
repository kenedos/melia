//--- Melia Script ----------------------------------------------------------
// The Hydra at the Jeneuam Corridor
//--- Description -----------------------------------------------------------
// A red-gemmed Hydra rises from the reservoir and flees from the villagers.
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

[TrackScript("F_3CMLAKE_84_MQ_02_TRACK")]
public class F3Cmlake84Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("F_3CMLAKE_84_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1112.01f, 269.35f, -479.19f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 152002, -1106.26, 269.35, -450.01, 37, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Elder Eloizard"), EndPosition = new Position(-1347.11f, 266.83f, -462.96f) }));
		actors.Add(AddTrackActor(character, 58209, -1681.57, 151.22, -1064.53, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1429.41f, 151.22f, -616.49f) }));
		actors.Add(AddTrackActor(character, 147481, -1404.48, 266.83, -492.63, 19, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20118, -1125.41, 264.28, -526.29, 23, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1348.92f, 266.83f, -443.01f) }));
		actors.Add(AddTrackActor(character, 147482, -1119.86, 269.35, -457.40, 33, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1367.67f, 266.83f, -451.47f) }));
		actors.Add(AddTrackActor(character, 20153, -1091.86, 269.35, -507.56, 34, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1317.30f, 269.35f, -498.56f) }));
		actors.Add(AddTrackActor(character, 20151, -1126.79, 269.35, -439.94, 25, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1359.67f, 266.83f, -480.33f) }));
		actors.Add(AddTrackActor(character, 20025, -1378.11, 266.83, -461.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 51:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The Hydra ran away as soon as the villagers rushed in"), 3);
				break;

			case 57:
				track.Actors[8].AttachEffect("F_light091_dark_loop", 5, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
