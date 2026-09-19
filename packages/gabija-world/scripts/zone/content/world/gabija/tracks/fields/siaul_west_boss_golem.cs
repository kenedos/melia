//--- Melia Script ----------------------------------------------------------
// The Golem at Delong Rest Stop
//--- Description -----------------------------------------------------------
// The squad leader's missing men, and what they ran into.
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

[TrackScript("SIAUL_WEST_BOSS_GOLEM_TRACK")]
public class SiaulWestBossGolemTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_BOSS_GOLEM_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-644f, 353f, 997f));

		actors.Add(AddTrackActor(character, 57375, -541, 360, 1394, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 450221, -541, 360, 1394, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		var soldier = new TrackActorSpec { Faction = FactionType.Neutral, Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 10032, -541, 360, 1312, 13, soldier));
		actors.Add(AddTrackActor(character, 10032, -623, 360, 1435, 19, soldier));
		actors.Add(AddTrackActor(character, 10032, -463, 360, 1368, 12, soldier));

		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 45:
				RemoveTrackActor(character, track, 0);
				break;
			case 74:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
