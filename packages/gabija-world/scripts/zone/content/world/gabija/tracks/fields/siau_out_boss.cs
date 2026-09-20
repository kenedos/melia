//--- Melia Script ----------------------------------------------------------
// The Red Vubbe Fighter
//--- Description -----------------------------------------------------------
// The chest on the mining road hides a Red Vubbe Fighter and its miners.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAU_OUT_BOSS_TRACK")]
public class SiauOutBossTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU_OUT_BOSS_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 11120, 1466.1458, 228.98795, 529.99652, 22, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1492.7101f, 198.75706f, 483.73193f) }));
		actors.Add(AddTrackActor(character, 11125, 1440.0255, 229.569, 502.47266, 19, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1469.5288f, 206.5446f, 472.04477f) }));
		actors.Add(AddTrackActor(character, 11125, 1462.5212, 222.92471, 503.10767, 26, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1497.1653f, 197.24959f, 456.66812f) }));
		actors.Add(AddTrackActor(character, 400203, 1454.2178, 228.86983, 508.65002, 0, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 11160, 1380.3358, 229.55901, 507.60181, 33, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1439.7379f, 229.569f, 494.93356f) }));
		actors.Add(AddTrackActor(character, 11160, 1452.542, 229.569, 600.01093, 42, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1466.429f, 225.24919f, 517.83826f) }));
		actors.Add(AddTrackActor(character, 147392, 1650, 147, 438, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 11:
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				break;
			case 12:
				character.ServerMessage(L("As you opened the chest, a Red Vubbe Warrior yells at you!"));
				break;
			case 15:
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 2);
				break;
			case 16:
				RemoveTrackActor(character, track, 1);
				break;
			case 39:
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
