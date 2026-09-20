//--- Melia Script ----------------------------------------------------------
// The Epitaph on the Dykyne Fork Road
//--- Description -----------------------------------------------------------
// The third epitaph throws off its guardians as Rexipher reaches it.
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

[TrackScript("ROKAS29_MQ3_TRACK")]
public class Rokas29Mq3Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_MQ3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-187.32f, 681.29f, 495.91f));

		actors.Add(AddTrackActor(character, 47106, -206, 681, 495, 1, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Epitaph") }));
		actors.Add(AddTrackActor(character, 47413, 57.99, 681.77, 594.89, 34, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Historian Rexipher"), EndPosition = new Position(-71.95f, 681.72f, 571.70f) }));
		actors.Add(AddTrackActor(character, 401301, -334.11, 681.29, 598.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -203.54, 681.29, 648.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -292.01, 681.29, 500.84, 43, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -128.68, 681.29, 601.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401301, -107.68, 681.59, 477.39, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.7f, EffectLocation.Bottom);
				break;
			case 2:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.8f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("I_smoke008_red##2", 1.1f, EffectLocation.Bottom);
				break;
			case 10:
				track.Actors[0].AttachEffect("F_warrior_reward_shot_lineup", 1.3f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 1.7f, EffectLocation.Bottom);
				break;
			case 12:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 1.1f, EffectLocation.Bottom);
				break;
			case 13:
				track.Actors[0].AttachEffect("F_buff_basic029_red_line", 3, EffectLocation.Bottom);
				break;
			case 16:
				track.Actors[0].AttachEffect("F_spread_out014_smoke", 0.8f, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[1].AttachEffect("F_buff_basic029_red_line", 5, EffectLocation.Bottom);
				break;
			case 24:
				track.Actors[1].AttachEffect("F_cleric_ShapeShifting_ground", 1.2f, EffectLocation.Bottom);
				break;
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
