//--- Melia Script ----------------------------------------------------------
// The trap at the second circle
//--- Description -----------------------------------------------------------
// The larger transport circle closes on Grita instead of opening.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER41_MQ_03_TRACK")]
public class Ftower41Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER41_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147500, -582, 1410, -1856, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("2nd Transport Magic Circle") }));
		actors.Add(AddTrackActor(character, 147449, -582, 1410, -1856, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Grita") }));
		actors.Add(AddTrackActor(character, 401621, -721.98, 1410.11, -1809.73, 205, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401621, -703.58, 1410.11, -1931.10, 135, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47397, -600.82, 1410.11, -1721.20, 295, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57040, -582.21, 1410.11, -1994.69, 265, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41303, -473.83, 1410.11, -1942.21, 180, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41303, -446.08, 1410.11, -1803.49, 195, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
			case 20:
				character.ServerMessage(L("Destroy the 2nd Transport Magic Circle restraining Grita!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
