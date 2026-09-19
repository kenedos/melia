//--- Melia Script ----------------------------------------------------------
// The villagers on Melagingas Hill
//--- Description -----------------------------------------------------------
// The bomb goes off, and the villagers who came to watch shed their skins.
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

[TrackScript("HUEVILLAGE_58_3_MQ04_TRACK")]
public class Huevillage583Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_3_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1347.454f, -1.3659f, -1275.1821f));

		// The Upents stay asleep through the fight the bomb starts.
		actors.Add(AddTrackActor(character, 57019, -1395.5902, -1.3659, -1312.5349, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 57019, -1459.6354, -1.3659, -1383.5389, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 57019, -1431.6594, -1.3659, -1311.5895, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 57019, -1388.6016, -1.3659, -1346.4641, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147396, -1213.2578, -4.1535001, -968.52869, 24, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, EndPosition = new Position(-1229.2622f, -4.1535001f, -1032.1509f) }));
		actors.Add(AddTrackActor(character, 147407, -1228.9401, -4.1535001, -983.09235, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147420, -1197.1558, -4.1535001, -987.03021, 11, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 400102, -1229, -4, -1032, 105, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400102, -1228, -4, -983, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400102, -1197, -4, -987, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				track.Actors[0].AttachEffect("F_explosion039", 1, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_archer_scarecrow_loop_ground", 2, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_archer_scarecrow_loop_ground", 2, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_archer_scarecrow_loop_ground", 2, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_archer_scarecrow_loop_ground", 2, EffectLocation.Bottom);
				break;
			case 14:
				track.Actors[4].AttachEffect("F_levitation028_smoke", 2, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[5].AttachEffect("F_levitation028_smoke", 2, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_levitation028_smoke", 2, EffectLocation.Bottom);
				break;
			case 27:
				track.Actors[4].AttachEffect("F_burstup004_dark", 1, EffectLocation.Bottom);
				break;
			case 28:
				RemoveTrackActor(character, track, 4);
				break;
			case 29:
				track.Actors[5].AttachEffect("F_burstup004_dark", 1, EffectLocation.Bottom);
				break;
			case 30:
				track.Actors[6].AttachEffect("F_burstup004_dark", 1, EffectLocation.Bottom);
				break;
			case 31:
				RemoveTrackActor(character, track, 5);
				break;
			case 32:
				RemoveTrackActor(character, track, 6);
				break;
			case 33:
				character.ServerMessage(L("The effect of the bomb slows down your movement when you approach the nearby Upents!"));
				break;
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
