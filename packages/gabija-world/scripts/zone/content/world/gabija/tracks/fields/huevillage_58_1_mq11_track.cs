//--- Melia Script ----------------------------------------------------------
// The escape to Nugria Sanctum
//--- Description -----------------------------------------------------------
// The villagers who followed you shed their skins, and the girl at the altar
// opens the portal.
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

[TrackScript("HUEVILLAGE_58_1_MQ11_TRACK")]
public class Huevillage581Mq11Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_1_MQ11_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-932.04059f, 230.9787f, 214.68639f));

		actors.Add(AddTrackActor(character, 147407, -1052.5544, 230.9787, 434.35086, 23, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147408, -1017.8218, 230.9787, 505.11377, 20, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147409, -1059.8457, 230.9787, 470.22986, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147408, -1022.848, 230.9787, 440.22305, 10, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147420, -1053.2869, 230.9787, 497.22882, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147419, -1086.3914, 230.9787, 425.0972, 18, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 47236, -1017.879, 230.9787, 473.50012, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, EndPosition = new Position(-967.56055f, 230.9787f, 308.73453f) }));
		actors.Add(AddTrackActor(character, 47124, -1250, 230, 490, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Nugria Altar") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 400102, -1050.788, 230.9787, 429.55328, 18, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-991.96747f, 230.9787f, 342.88312f) }));
		actors.Add(AddTrackActor(character, 400102, -1016.781, 230.9787, 499.06146, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-950.15881f, 230.9787f, 374.28601f) }));
		actors.Add(AddTrackActor(character, 400102, -1059.85, 230.98, 470.23001, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1018.4265f, 230.9787f, 388.28033f) }));
		actors.Add(AddTrackActor(character, 400102, -1081.5288, 230.9787, 421.36969, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1021.7143f, 230.9787f, 345.07843f) }));
		actors.Add(AddTrackActor(character, 400102, -1053.29, 230.98, 497.23001, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-966.66937f, 230.9787f, 358.80353f) }));
		actors.Add(AddTrackActor(character, 400102, -1022.85, 230.98, 440.22, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-985.48645f, 230.9787f, 372.21805f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 16:
				track.Actors[3].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				break;
			case 17:
				track.Actors[5].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[0].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				break;
			case 19:
				track.Actors[2].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				break;
			case 20:
				track.Actors[1].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("I_smoke008_red", 1, EffectLocation.Bottom);
				break;
			case 27:
				RemoveTrackActor(character, track, 0);
				track.Actors[9].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[9].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[9].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				break;
			case 28:
				RemoveTrackActor(character, track, 4);
				track.Actors[13].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[13].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[13].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				break;
			case 29:
				RemoveTrackActor(character, track, 2);
				track.Actors[11].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[11].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[11].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				break;
			case 30:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 5);
				track.Actors[10].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[10].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[10].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				track.Actors[12].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[12].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[12].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				break;
			case 31:
				RemoveTrackActor(character, track, 3);
				track.Actors[14].AttachEffect("F_burstup002_dark", 3, EffectLocation.Bottom);
				track.Actors[14].AttachEffect("F_blood009_red", 4, EffectLocation.Bottom);
				track.Actors[14].AttachEffect("F_smoke064_red", 1.5f, EffectLocation.Bottom);
				break;
			case 36:
				track.Actors[6].AttachEffect("F_burstup001_yellow", 4, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_buff_basic032_yellow_line", 4, EffectLocation.Bottom);
				break;
			case 37:
				track.Actors[6].AttachEffect("F_burstup001_yellow", 8, EffectLocation.Bottom);
				break;
			case 41:
				RemoveTrackActor(character, track, 9);
				break;
			case 42:
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 13);
				break;
			case 43:
				RemoveTrackActor(character, track, 11);
				break;
			case 44:
				RemoveTrackActor(character, track, 10);
				RemoveTrackActor(character, track, 12);
				break;
			case 45:
				RemoveTrackActor(character, track, 14);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
