//--- Melia Script ----------------------------------------------------------
// The second mark on the treasure map
//--- Description -----------------------------------------------------------
// Another chest, and six Hogma Scouts who will not let it be opened.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ROKAS30_PIPOTI03_TRACK")]
public class Rokas30Pipoti03Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_PIPOTI03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147392, -239, 348, 727, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));
		actors.Add(AddTrackActor(character, 47309, -310.04, 348.68, 683.16, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47309, -155.11, 348.68, 734.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47309, -241.52, 348.68, 783.53, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47309, -126.41, 348.68, 677.60, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47309, -156.64, 348.68, 605.21, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47309, -366.30, 348.68, 593.89, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				character.ServerMessage(L("A treasure chest appeared again! Get past the monsters and look inside."));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
