//--- Melia Script ----------------------------------------------------------
// Archon at the corrupted altar
//--- Description -----------------------------------------------------------
// The shamans finish their summoning and are spent by it.
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

[TrackScript("THORN20_MQ07_TRACK")]
public class Thorn20Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN20_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153019, 2528.4094, 488.93805, 738.82678, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 153029, 2535.123, 488.93063, 738.04193, 0, new TrackActorSpec { Ai = "MON_DUMMY", MaxHp = 20 }));
		actors.Add(AddTrackActor(character, 41439, 2467.7927, 489.02545, 801.01373, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, 2589.9565, 488.89221, 810.7077, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, 2457.6287, 488.97504, 663.61017, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400381, 2623.0735, 488.81363, 681.39124, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2641.4128f, 488.7959f, 693.30847f) }));
		actors.Add(AddTrackActor(character, 400381, 2551.6382, 488.90958, 631.32782, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2516.6248f, 488.91656f, 662.13623f) }));
		actors.Add(AddTrackActor(character, 41245, 2537.2622, 488.92334, 724.46545, 395, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_bg_firetower_teleport", 1, EffectLocation.Bottom);
				break;
			case 5:
				track.Actors[2].AttachEffect("F_spread_out002", 0.5f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_spread_out002", 0.5f, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_spread_out002", 0.5f, EffectLocation.Bottom);
				break;
			case 8:
				character.ServerMessage(L("Destroy the Demon Summoning Crystal."));
				break;
			case 11:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				break;
			case 23:
				track.Actors[7].AttachEffect("F_smoke037_1", 1.5f, EffectLocation.Bottom);
				break;
			case 27:
				track.Actors[7].AttachEffect("F_burstup001_dark", 1.5f, EffectLocation.Bottom);
				break;
			case 29:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
