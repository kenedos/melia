//--- Melia Script ----------------------------------------------------------
// The Specter Monarch of District 6
//--- Description -----------------------------------------------------------
// The compass leads into a bat roost, where the Specter Monarch is holding
// the purifier's missing part.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("MINE_1_CRYSTAL_18_TRACK")]
public class Mine1Crystal18Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_1_CRYSTAL_18_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1418.953f, 154.7101f, 583.65863f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 400703, -1367.6425, 154.7001, 650.73431, 64));
		actors.Add(AddTrackActor(character, 41418, -1468.4128, 187.21631, 674.0929, 38, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1394.5361f, 154.7001f, 617.73993f) }));
		actors.Add(AddTrackActor(character, 41418, -1454.8802, 183.5545, 703.99963, 60, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1322.4061f, 154.7001f, 644.84308f) }));
		actors.Add(AddTrackActor(character, 41418, -1455.9125, 177.50731, 687.37335, 45, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1347.131f, 154.7001f, 681.88794f) }));
		actors.Add(AddTrackActor(character, 41418, -1453.5515, 164.40802, 664.3374, 55, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1344.0747f, 148.04086f, 589.90656f) }));
		actors.Add(AddTrackActor(character, 41418, -1479.7964, 189.3593, 702.474, 25, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1428.3499f, 154.7001f, 668.92505f) }));
		actors.Add(AddTrackActor(character, 41418, -1492.4966, 189.3593, 673.01727, 84, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1291.1913f, 162.2374f, 663.43445f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 54:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
