//--- Melia Script ----------------------------------------------------------
// Goddess Saule freed
//--- Description -----------------------------------------------------------
// The key turns, the vines give way and the statues holding her fall.
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

[TrackScript("HUEVILLAGE_58_4_MQ08_TRACK")]
public class Huevillage584Mq08Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ08_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147385, 21.4165, 34.20367, -186.0109, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Goddess Saule") }));
		actors.Add(AddTrackActor(character, 147388, 107.0288, 34.20369, -203.0942, 55, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Level = 42 }));
		actors.Add(AddTrackActor(character, 147387, -53.25879, 34.20368, -201.0888, 45, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Level = 42 }));
		actors.Add(AddTrackActor(character, 155053, 28.48047, 34.20367, -188.3998, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 47122, 20.91748, 34.20366, -183.1234, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_levitation032_red", 2.5f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_smoke143_dark_red", 3, EffectLocation.Middle);
				track.Actors[2].AttachEffect("F_smoke143_dark_red", 4, EffectLocation.Top);
				track.Actors[3].AttachEffect("E_vine2", 1, EffectLocation.Bottom);
				break;
			case 6:
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				break;
			case 10:
				track.Actors[3].AttachEffect("E_vine2", 1, EffectLocation.Bottom);
				break;
			case 12:
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				break;
			case 15:
				track.Actors[0].AttachEffect("F_levitation032_red", 2, EffectLocation.Bottom);
				break;
			case 21:
				track.Actors[0].AttachEffect("F_spread_out004_dark", 2, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_spread_out004_dark", 1, EffectLocation.Bottom);
				break;
			case 23:
				track.Actors[4].AttachEffect("I_smoke045_spread_in", 4, EffectLocation.Bottom);
				break;
			case 24:
				track.Actors[0].AttachEffect("F_light078_holy_yellow_loop", 1.5f, EffectLocation.Bottom);
				break;
			case 33:
				track.Actors[0].AttachEffect("F_light061", 1.3f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_light039_yellow", 1, EffectLocation.Bottom);
				break;
			case 38:
				track.Actors[0].AttachEffect("F_ground012_light", 2, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_spin022_blue1", 1, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_light033_circle_blue", 3, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_archer_circling_ground", 6, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				break;
			case 46:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				break;
			case 54:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
