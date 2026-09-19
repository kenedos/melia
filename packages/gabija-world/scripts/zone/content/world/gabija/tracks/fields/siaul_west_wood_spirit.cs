//--- Melia Script ----------------------------------------------------------
// The Rocktortuga at the Klaipeda Checkpoint
//--- Description -----------------------------------------------------------
// Infrorocktors break on the guard line, and something larger follows.
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

[TrackScript("SIAUL_WEST_WOOD_SPIRIT_TRACK")]
public class SiaulWestWoodSpiritTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_WOOD_SPIRIT_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1577f, 210f, -801f));
		actors.Add(character);

		var guard = new TrackActorSpec { Faction = FactionType.Neutral, Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 10032, 1584, 210, -742, 0, guard));
		actors.Add(AddTrackActor(character, 10032, 1586, 210, -866, 0, guard));

		actors.Add(AddTrackActor(character, 41233, 1418, 210, -916, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		var rocktor = new TrackActorSpec { Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 41319, 1275, 210, -960, 43, rocktor));

		actors.Add(AddTrackActor(character, 10032, 1614, 210, -750, 57, guard));
		actors.Add(AddTrackActor(character, 10032, 1682, 210, -753, 80, guard));
		actors.Add(AddTrackActor(character, 10032, 1655, 210, -773, 76, guard));

		actors.Add(AddTrackActor(character, 41319, 1272, 210, -787, 0, rocktor));
		actors.Add(AddTrackActor(character, 41319, 1287, 210, -913, 58, rocktor));
		actors.Add(AddTrackActor(character, 41319, 1335, 210, -1028, 0, rocktor));
		actors.Add(AddTrackActor(character, 41319, 1297, 210, -985, 39, rocktor));
		actors.Add(AddTrackActor(character, 41319, 1243, 210, -860, 43, rocktor));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 20:
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 8);
				RemoveTrackActor(character, track, 9);
				RemoveTrackActor(character, track, 10);
				RemoveTrackActor(character, track, 11);
				RemoveTrackActor(character, track, 12);
				break;
			case 39:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 8);
				RemoveTrackActor(character, track, 9);
				RemoveTrackActor(character, track, 10);
				RemoveTrackActor(character, track, 11);
				RemoveTrackActor(character, track, 12);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
			case 44:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
