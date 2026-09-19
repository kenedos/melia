//--- Melia Script ----------------------------------------------------------
// Rikaus at Thorny Pillar Garden
//--- Description -----------------------------------------------------------
// The shamans call up what has been fouling the garden.
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

[TrackScript("THORN20_MQ02_TRACK")]
public class Thorn20Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN20_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 400661, -108.29575, 559.2428, 421.53198, 65, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, -69.34314, 558.86487, 255.09528, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, 32.668724, 559.11865, 388.27692, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, -71.128784, 559.05237, 370.80301, 4, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153019, -16.84008, 558.99103, 314.33484, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 153029, -12.971466, 558.99878, 310.38293, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[4].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				break;
			case 3:
				track.Actors[5].AttachEffect("F_ground122_dark", 1, EffectLocation.Bottom);
				break;
			case 10:
				track.Actors[1].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				track.Actors[5].AttachEffect("F_ground122_dark", 1, EffectLocation.Bottom);
				break;
			case 16:
				track.Actors[1].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("I_cleric_skl_Ironskin1_mash", 0.5f, EffectLocation.Bottom);
				break;
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
