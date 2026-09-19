//--- Melia Script ----------------------------------------------------------
// Moyabruka at Nugria Sanctum
//--- Description -----------------------------------------------------------
// The sanctum floor splits and the growth beneath it stands up.
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

[TrackScript("HUEVILLAGE_58_1_SQ01_TRACK")]
public class Huevillage581Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_1_SQ01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47325, -946.89886, 230.9787, 377.6842, 70, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47124, -1250, 230, 490, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Nugria Altar") }));
		actors.Add(AddTrackActor(character, 20025, -397.5957, 230.9787, -162.95171, 121, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-969.63092f, 230.9787f, 330.80563f) }));
		actors.Add(AddTrackActor(character, 20025, -363.27374, 230.9787, -162.91429, 189, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-947.07019f, 230.9787f, 362.02243f) }));
		actors.Add(AddTrackActor(character, 147487, -805.69928, 229.36487, 227.8291, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20025, -810.88, 278.13226, 234.40625, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20025, -628.14569, 229.36487, -53.80838, 3, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-621.53571f, 230.9787f, -53.663376f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[4].AttachEffect("I_smoke039", 4, EffectLocation.Bottom);
				break;
			case 7:
				track.Actors[7].AttachEffect("F_archer_entangle_active_smoke", 1.3f, EffectLocation.Bottom);
				track.Actors[7].AttachEffect("F_smoke022", 3, EffectLocation.Bottom);
				track.Actors[7].AttachEffect("F_explosion041_smoke", 3, EffectLocation.Bottom);
				break;
			case 10:
				track.Actors[4].AttachEffect("I_smoke039", 3, EffectLocation.Bottom);
				break;
			case 15:
				track.Actors[6].AttachEffect("F_explosion041_smoke", 1.7f, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("I_Moyabruka_born_mash", 3, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("I_statue_parts_mash", 4, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 5);
				break;
			case 16:
				track.Actors[0].AttachEffect("F_smoke043_green", 1.5f, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[0].AttachEffect("F_smoke079", 3, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_drop_leaf004", 2, EffectLocation.Bottom);
				break;
			case 22:
				character.ServerMessage(L("Defeat Moyabruka!"));
				break;
			case 29:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
