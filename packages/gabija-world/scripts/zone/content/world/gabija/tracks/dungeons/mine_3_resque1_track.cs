//--- Melia Script ----------------------------------------------------------
// Rescue the Villagers
//--- Description -----------------------------------------------------------
// The Vubbe-held miners are tied up in a nest of Crystal Spiders on the
// third floor, and the spiders turn on whoever comes for them.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_3_RESQUE1_TRACK")]
public class Mine3Resque1Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_3_RESQUE1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1233.2985f, 303.48529f, 0.034957886f));

		actors.Add(AddTrackActor(character, 41409, -998.59998, 181.61, -257.69, 55.5, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-972.82001f, 181.63f, -153.86f) }));
		actors.Add(AddTrackActor(character, 41409, -671.47998, 181.61, -3.6499939, 39.090908, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-771f, 181.89999f, -69f) }));
		actors.Add(AddTrackActor(character, 41409, -801.95001, 181.75999, -20.190002, 32.285713, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1027f, 216.14f, -32.27f) }));
		actors.Add(AddTrackActor(character, 41409, -1098.0699, 253.09, -7.0899963, 52.499996, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1112.79f, 253.09f, -30.07f) }));
		actors.Add(AddTrackActor(character, 41409, -1165.41, 253.09, -151.89999, 52, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1144.78f, 266.07999f, -77.040001f) }));
		actors.Add(AddTrackActor(character, 41409, -1070.67, 181.61, -95.779999, 61.81818, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-991.72998f, 181.61f, -86.989998f) }));
		actors.Add(AddTrackActor(character, 41409, -832.88, 185.55, 70.209999, 51, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-878f, 181.7f, -12.96f) }));

		actors.Add(AddTrackActor(character, 20150, -1172.8044, 303.48529, 94.021439, 26.875, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Miner" }));
		actors.Add(AddTrackActor(character, 47236, -1206.5675, 303.48529, 87.323349, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20114, -1170.0, 303.0, 120.0, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Village Aunt" }));
		actors.Add(AddTrackActor(character, 147473, -1200.2533, 303.48529, 98.060905, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Village Girl" }));
		actors.Add(AddTrackActor(character, 151009, -1173.9192, 303.48529, 95.35041, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Miner" }));
		actors.Add(AddTrackActor(character, 151010, -1170.0, 303.0, 120.0, 1.6666667, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Village Aunt" }));
		actors.Add(AddTrackActor(character, 151011, -1200.25, 303.48999, 98.059998, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Village Girl" }));
		actors.Add(AddTrackActor(character, 151012, -1206.5699, 303.48999, 87.32, 2.8571429, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "Girl" }));

		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 3:
				RemoveTrackActor(character, track, 11);
				break;
			case 5:
				RemoveTrackActor(character, track, 12);
				break;
			case 11:
				RemoveTrackActor(character, track, 14);
				break;
			case 14:
				RemoveTrackActor(character, track, 13);
				break;
			case 29:
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 8);
				RemoveTrackActor(character, track, 9);
				RemoveTrackActor(character, track, 10);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
