//--- Melia Script ----------------------------------------------------------
// The circle Simas wants studied
//--- Description -----------------------------------------------------------
// A corrupted altar still summoning, kept whole enough to read.
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

[TrackScript("THORN20_MQ06_TRACK")]
public class Thorn20Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN20_MQ06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2407.877f, 489.04871f, -1351.1113f));

		actors.Add(AddTrackActor(character, 41439, 2459.4475, 489.04871, -1180.7545, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, 2617.1077, 489.04871, -1159.5337, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41439, 2614.345, 489.04871, -1321.8699, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153019, 2529.1287, 489.04871, -1249.7942, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 153029, 2532.7163, 489.04871, -1249.4095, 0, new TrackActorSpec { Ai = "MON_DUMMY", MaxHp = 20 }));
		actors.Add(AddTrackActor(character, 147389, 2163.3899, 450, -1415.4399, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Believer Simas") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[3].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_bg_firetower_teleport", 1, EffectLocation.Bottom);
				break;
			case 5:
				character.ServerMessage(L("Destroy the Demon Summoning Crystal!"));
				break;
			case 14:
				RemoveTrackActor(character, track, 5);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
