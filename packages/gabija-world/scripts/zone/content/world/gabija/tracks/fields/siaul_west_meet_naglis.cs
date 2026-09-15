//--- Melia Script ----------------------------------------------------------
// The Searcher's Ambush
//--- Description -----------------------------------------------------------
// A Large Kepa and its escort charge the searcher's post.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_WEST_MEET_NAGLIS_TRACK")]
public class SiaulWestMeetNaglisTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_MEET_NAGLIS_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 10046, -1493, 261, -145, 3, new TrackActorSpec
		{
			Name = L("Searcher"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			RunSpeed = 80,
			WalkSpeed = 60,
			Ai = "TrackWaitMonster",
		}));

		var kepa = new TrackActorSpec { Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 400002, -1436, 261, -237, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1519, 261, -222, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1472, 261, -262, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1382, 261, -232, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1377, 261, -184, 0, kepa));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				InsertTrackHate(character, track, 2);
				break;
			case 6:
				InsertTrackHate(character, track, 3);
				break;
			case 8:
				InsertTrackHate(character, track, 4);
				break;
			case 10:
				InsertTrackHate(character, track, 5);
				break;
			case 13:
				InsertTrackHate(character, track, 1);
				break;
			case 17:
				character.ServerMessage(L("Kill the charging Large Kepa!"));
				break;
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
