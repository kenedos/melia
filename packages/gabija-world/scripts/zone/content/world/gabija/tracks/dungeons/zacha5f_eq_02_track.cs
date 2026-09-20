//--- Melia Script ----------------------------------------------------------
// The corrupted guardians of the middle hall
//--- Description -----------------------------------------------------------
// Eight Venucelos answer the gravestone's warning.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA5F_EQ_02_TRACK")]
public class Zacha5fEq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_EQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41198, -2503.11, 326.01, -3716.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2602.90, 326.01, -3728.49, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2413.88, 326.01, -3726.19, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2428.16, 326.01, -3688.41, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2463.75, 326.01, -3633.45, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2558.63, 326.01, -3633.48, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2578.56, 326.01, -3690.25, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41198, -2505.40, 326.01, -3668.63, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47260, -2509.80, 326.01, -3858.73, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Corrupted Royal Mausoleum Guardian") }));

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
