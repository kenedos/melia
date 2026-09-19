//--- Melia Script ----------------------------------------------------------
// The summoning crystals at the goddess' altars
//--- Description -----------------------------------------------------------
// Two of Saule's own altars, turned to the Merogs' use.
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

[TrackScript("THORN20_MQ04_TRACK")]
public class Thorn20Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN20_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153019, -1343.3042, 583.26068, 232.43225, 146, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 153029, -1337.7374, 583.26068, 230.22537, 19, new TrackActorSpec { Ai = "MON_DUMMY", MaxHp = 20 }));
		actors.Add(AddTrackActor(character, 153019, -1078.9231, 583.2605, -383.52118, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 153029, -1073.2837, 583.2605, -381.36401, 0, new TrackActorSpec { Ai = "MON_DUMMY", MaxHp = 20 }));
		actors.Add(AddTrackActor(character, 41440, -1378.2279, 583.27069, 275.81741, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41440, -1262.374, 583.27069, 251.87198, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41440, -1169.8569, 583.27051, -384.06784, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41440, -1047.1694, 583.27051, -297.63989, 26, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41445, -1477.9971, 583.26068, 231.75346, 39, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41445, -1234.9818, 583.27069, 175.09486, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41445, -1353.9845, 583.26068, 114.87204, 13, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147386, -1223, 557, 602, 90, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Believer Zaneta") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_bg_firetower_teleport", 1, EffectLocation.Bottom);
				break;
			case 29:
				RemoveTrackActor(character, track, 11);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
