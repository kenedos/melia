//--- Melia Script ----------------------------------------------------------
// The guardians of the upper hall
//--- Description -----------------------------------------------------------
// Mauros, Medakia and Rusrat together, in front of the epitaph.
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

[TrackScript("ZACHA5F_EQ_05_TRACK")]
public class Zacha5fEq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_EQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2510.91f, 332.08f, -2899.01f));

		actors.Add(AddTrackActor(character, 400801, -2636.21, 332.08, -2882.52, 210, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400801, -2396.71, 332.08, -2816.88, 235, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -2465.95, 332.08, -2779.18, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -2525.44, 332.08, -2684.39, 167, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -2454.90, 332.08, -2675.81, 147, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400861, -2479.72, 332.08, -2582.31, 155, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400821, -2564.74, 332.08, -2786.89, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400861, -2656.53, 332.08, -2674.96, 142, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47260, -2511.84, 334.13, -2872.10, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Corrupted Royal Mausoleum Guardian") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
