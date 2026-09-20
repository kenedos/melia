//--- Melia Script ----------------------------------------------------------
// Pipoti's colleague at the Forest of Fireflies
//--- Description -----------------------------------------------------------
// The stonemason's colleague is found with ten Hogma Scouts around him.
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

[TrackScript("ROKAS30_PIPOTI01_TRACK")]
public class Rokas30Pipoti01Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_PIPOTI01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1365.62f, 250.80f, 61.55f));

		actors.Add(AddTrackActor(character, 152000, 1244.05, 148.16, -364.90, 67, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Pipoti's Colleague"), EndPosition = new Position(1271.62f, 148.16f, -232.88f) }));
		actors.Add(AddTrackActor(character, 47309, 1274.19, 148.16, -126.23, 28, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1271.71f, 148.16f, -207.26f) }));
		actors.Add(AddTrackActor(character, 47309, 1208.71, 148.16, -72.07, 26, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1243.63f, 148.16f, -162.81f) }));
		actors.Add(AddTrackActor(character, 47309, 1447.36, 148.16, -116.58, 31, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1348.49f, 148.16f, -214.06f) }));
		actors.Add(AddTrackActor(character, 47309, 1565.28, 148.15, -323.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1375.86f, 148.16f, -265.40f) }));
		actors.Add(AddTrackActor(character, 47309, 1022.86, 148.16, -328.60, 40, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1204.98f, 148.16f, -266.14f) }));
		actors.Add(AddTrackActor(character, 47309, 1215.12, 148.16, -497.35, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1264.79f, 148.16f, -305.28f) }));
		actors.Add(AddTrackActor(character, 47309, 1152.75, 148.16, -499.93, 32, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1243.45f, 148.16f, -280.52f) }));
		actors.Add(AddTrackActor(character, 47309, 1179.77, 148.16, -505.19, 38, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1229.53f, 148.16f, -335.06f) }));
		actors.Add(AddTrackActor(character, 47309, 1396.45, 148.16, -469.59, 39, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1312.46f, 148.16f, -291.30f) }));
		actors.Add(AddTrackActor(character, 47309, 1145.54, 148.16, -546.19, 35, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1225.43f, 148.16f, -387.62f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 17:
				// The colleague does not survive the scene.
				RemoveTrackActor(character, track, 0);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
