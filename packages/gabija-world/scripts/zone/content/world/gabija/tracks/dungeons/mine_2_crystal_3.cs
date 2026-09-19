//--- Melia Script ----------------------------------------------------------
// The Carapace on the District 2 Pipe
//--- Description -----------------------------------------------------------
// A Carapace has bedded down on the purifier pipe with a Yekubite swarm
// around it, and turns on whoever comes to clear the line.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_2_CRYSTAL_3_TRACK")]
public class Mine2Crystal3Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_2_CRYSTAL_3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1461.975f, 140.9554f, 910.65979f));

		actors.Add(AddTrackActor(character, 41246, -1500.1791, 140.9554, 825.18304, 25));
		actors.Add(AddTrackActor(character, 41257, -1567.5394, 140.96539, 858.50201, 19, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1592.5009f, 140.96539f, 1155.7047f) }));
		actors.Add(AddTrackActor(character, 41257, -1724.6705, 140.96539, 819.57904, 15, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41257, -1647.8879, 140.96539, 689.93024, 34, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1529.7858f, 140.96539f, 780.44537f) }));
		actors.Add(AddTrackActor(character, 41257, -1534.822, 140.96539, 778.15186, 20, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1530.2456f, 140.96539f, 780.12854f) }));
		actors.Add(AddTrackActor(character, 41257, -1728.7629, 140.96539, 698.43707, 50, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1609.1332f, 140.96539f, 759.36542f) }));
		actors.Add(AddTrackActor(character, 41257, -1565.3375, 140.96539, 826.98865, 13, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41257, -1556.4202, 140.96539, 832.72296, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41257, -1602.4607, 140.96539, 849.64087, 18, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1746.7074f, 140.96539f, 764.94513f) }));
		actors.Add(AddTrackActor(character, 41257, -1407.8477, 140.96539, 802.94397, 29, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1498.6447f, 140.96539f, 763.15247f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 9);
				break;
			case 25:
				RemoveTrackActor(character, track, 4);
				break;
			case 26:
				RemoveTrackActor(character, track, 6);
				break;
			case 33:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 8);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
