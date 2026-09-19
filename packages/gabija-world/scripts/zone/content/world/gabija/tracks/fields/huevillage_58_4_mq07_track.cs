//--- Melia Script ----------------------------------------------------------
// Clymen behind the demon barrier
//--- Description -----------------------------------------------------------
// The crack in the Ishpirki Access Road gives up what was hiding in it.
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

[TrackScript("HUEVILLAGE_58_4_MQ07_TRACK")]
public class Huevillage584Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1293.854f, -22.4692f, -276.5406f));

		actors.Add(AddTrackActor(character, 20026, 1296, -22, -245, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 47316, 1298.744, -23.269, -254.9223, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_pattern007_dark_loop", 12, EffectLocation.Bottom);
				break;
			case 3:
				track.Actors[0].AttachEffect("F_smoke019_dark", 12, EffectLocation.Bottom);
				break;
			case 20:
				track.Actors[1].AttachEffect("F_burstup001_dark", 1.5f, EffectLocation.Bottom);
				break;
			case 21:
				track.Actors[1].AttachEffect("F_spread_out004_dark", 1.5f, EffectLocation.Bottom);
				break;
			case 23:
				track.Actors[1].AttachEffect("F_archer_explosiontrap_shot_smoke", 1.2f, EffectLocation.Bottom);
				break;
			case 24:
				track.Actors[1].AttachEffect("F_archer_entangle_active_smoke", 1.2f, EffectLocation.Bottom);
				break;
			case 31:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
