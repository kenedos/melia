//--- Melia Script ----------------------------------------------------------
// Cyclops' Attack in the Crystal Mine
//--- Description -----------------------------------------------------------
// A Cyclops drives the Vubbe miners out of the crystal store and turns on
// whoever is left standing.
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

[TrackScript("MINE_1_CRYSTAL_10_TRACK")]
public class Mine1Crystal10Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_1_CRYSTAL_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(797.87866f, 14.926186f, 72.469917f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 11125, 1047.9653, 3.5999, 79.241791, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(696.13867f, 58.78331f, -88.797699f) }));
		actors.Add(AddTrackActor(character, 11125, 1122.5378, 3.5999451, 94.292755, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(698.30261f, 56.105515f, -83.444496f) }));
		actors.Add(AddTrackActor(character, 11125, 1116.1083, 3.599905, 77.737038, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(609.60681f, 17.045462f, 3.4723752f) }));
		actors.Add(AddTrackActor(character, 41244, 1236.3376, 3.5899, 190.59451, 67, new TrackActorSpec { EndPosition = new Position(963.79156f, 4.9040103f, 36.910728f) }));
		actors.Add(AddTrackActor(character, 147453, 748.1676, 17.458778, 103.72282, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 151004, 1072.24, 3.5899, 79.316086, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 20024, 1115.4824, 3.5899, 68.214417, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				RemoveTrackActor(character, track, 5);
				break;
			case 22:
				RemoveTrackActor(character, track, 6);
				break;
			case 39:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 7);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
