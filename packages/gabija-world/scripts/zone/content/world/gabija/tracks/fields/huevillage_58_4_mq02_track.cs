//--- Melia Script ----------------------------------------------------------
// The goddess behind the barrier
//--- Description -----------------------------------------------------------
// Harpeia is holding down whoever the Grand Shrine was sealed around.
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

[TrackScript("HUEVILLAGE_58_4_MQ02_TRACK")]
public class Huevillage584Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(29.13477f, 34.2037f, -370.022f));

		actors.Add(AddTrackActor(character, 147385, 21.41992, 34.2, -186.01, 10, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Goddess Saule") }));
		actors.Add(AddTrackActor(character, 47321, -164.7324, 25.67221, -747.052, 994, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(604.9736f, -2.095596f, -117.1024f) }));
		actors.Add(AddTrackActor(character, 147388, 107.0298, 34.2, -203.09, 55, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Level = 42 }));
		actors.Add(AddTrackActor(character, 147387, -53.25977, 34.2, -201.09, 45, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Level = 42 }));
		actors.Add(AddTrackActor(character, 47321, 44.73486, -5.666137, -519.1309, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 155052, 28.47998, 34.2, -188.4, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 47122, 21.2793, 34.20367, -183.1071, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_levitation032_red_loop", 2.5f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_smoke143_dark_red_loop", 2, EffectLocation.Middle);
				track.Actors[3].AttachEffect("F_smoke143_dark_red_loop", 3, EffectLocation.Middle);
				break;
			case 15:
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 1);
				break;
			case 31:
				track.Actors[0].AttachEffect("I_sphere011_mash", 1.2f, EffectLocation.Bottom);
				break;
			case 44:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
