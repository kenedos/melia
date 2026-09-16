//--- Melia Script ----------------------------------------------------------
// The Tutu Ambush
//--- Description -----------------------------------------------------------
// A Tutu bursts from the water and sends the Chupacabra at the supply officer.
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

[TrackScript("ACT2_DISS1_2_BOSS_TRACK")]
public class Act2Diss1_2BossTrack : TrackScript
{
	protected override void Load()
	{
		SetId("ACT2_DISS1_2_BOSS_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41210, -40.459595, 101.3127, -551.65527, 612, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 400961, 199.22469, 130.0327, -649.96228, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 211.9808, 130.0327, -597.46405, 15, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 263.82397, 130.0327, -682.19836, 15, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 210.1192, 130.0327, -575.46832, 11, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 306.94797, 130.0327, -650.95648, 3, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400961, 268.3602, 130.0327, -602.18146, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		actors.Add(AddTrackActor(character, 20016, 661.28607, 130.02271, -453.11606, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Peaceful }));

		actors.Add(AddTrackActor(character, 153056, 484.20767, 130.02271, -515.74225, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 153056, 484.59668, 130.02271, -475.65164, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 153056, 584.47369, 130.02271, -449.40775, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));

		character.Movement.MoveTo(new Position(670.43182f, 130.02271f, -473.47852f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 20025, -41.616821, 101.3127, -532.22437, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				track.Actors[12].AttachEffect("F_burstup043_water_blue", 1, EffectLocation.Bottom);
				break;
			case 11:
				track.Actors[12].AttachEffect("F_burstup041_water_blue_reverse", 1, EffectLocation.Bottom);
				break;
			case 12:
				track.Actors[12].AttachEffect("F_burstup041_water_blue", 1, EffectLocation.Bottom);
				break;
			case 14:
				track.Actors[12].AttachEffect("F_ground115_water", 1, EffectLocation.Bottom);
				break;
			case 16:
				track.Actors[12].AttachEffect("F_ground115_water", 1, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_burstup001_smoke2", 1, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				break;
			case 17:
				RemoveTrackActor(character, track, 5);
				break;
			case 18:
				RemoveTrackActor(character, track, 6);
				break;
			case 32:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
