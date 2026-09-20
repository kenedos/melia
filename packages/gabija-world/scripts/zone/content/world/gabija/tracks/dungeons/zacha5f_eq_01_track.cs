//--- Melia Script ----------------------------------------------------------
// The corrupted guardians of the lower hall
//--- Description -----------------------------------------------------------
// Seven Venucelos come off the walls when the epitaph is read.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA5F_EQ_01_TRACK")]
public class Zacha5fEq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_EQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41198, -2663.89, 326.01, -4607.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2437.71, 326.01, -4666.90, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2545.04, 326.01, -4532.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2496.61, 326.01, -4603.41, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2466.33, 326.01, -4532.88, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2345.81, 326.01, -4599.93, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2579.73, 326.01, -4672.37, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47260, -2509.58, 326.01, -4762.89, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Guardian") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				character.ServerMessage(L("Defeat the Royal Mausoleum Guardians that attack you!"));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
