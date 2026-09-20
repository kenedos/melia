//--- Melia Script ----------------------------------------------------------
// Honeypin at Karadas Path
//--- Description -----------------------------------------------------------
// The altar draws it out of the canopy the moment it starts working.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ06_TRACK")]
public class Thorn21Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 46213, 2003.165, 331.97134, -3.2524185, 4, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Altar of Purification") }));
		actors.Add(AddTrackActor(character, 20026, 1972.5212, 331.97141, -125.00752, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 1853.924, 331.97137, 14.163511, 60, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 2081.8591, 331.97137, 90.051064, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 2010.8169, 331.97131, 145.16402, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 41242, 2160.2876, 331.97137, 148.29922, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				track.Actors[3].AttachEffect("F_smoke017_red", 1, EffectLocation.Bottom);
				break;
			case 2:
				track.Actors[1].AttachEffect("F_smoke017_red", 1, EffectLocation.Bottom);
				break;
			case 3:
				track.Actors[2].AttachEffect("F_smoke017_red", 1, EffectLocation.Bottom);
				break;
			case 4:
				track.Actors[4].AttachEffect("F_smoke017_red", 1, EffectLocation.Bottom);
				break;
			case 17:
				track.Actors[0].AttachEffect("F_ground011_yellow", 2, EffectLocation.Bottom);
				break;
			case 18:
				track.Actors[1].AttachEffect("F_explosion004_yellow", 0.5f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_explosion004_yellow", 0.5f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_explosion004_yellow", 0.5f, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_explosion004_yellow", 0.5f, EffectLocation.Bottom);
				break;
			case 22:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				break;
			case 31:
				character.ServerMessage(L("Honeypin has suddenly appeared!"));
				break;
			case 34:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
