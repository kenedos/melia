//--- Melia Script ----------------------------------------------------------
// Down the Zinuma Passage
//--- Description -----------------------------------------------------------
// Daiva closes the passage behind Hauberk, and Sigita is standing at the far
// end of it.
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

[TrackScript("VPRISON513_MQ_04_TRACK")]
public class Vprison513Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON513_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1386.90f, 91.04f, 908.82f));

		actors.Add(AddTrackActor(character, 154013, -1382.73, 91.04, 940.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Daiva") }));
		actors.Add(AddTrackActor(character, 154001, -1455.84, 91.04, 936.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealing Barrier") }));
		actors.Add(AddTrackActor(character, 154001, 1513.61, 91.04, 924.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealing Barrier") }));
		actors.Add(AddTrackActor(character, 154012, 1307.47, 91.04, 916.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Sigita") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				// The client plays the chase as a minigame; the drive ends
				// with the cutscene either way.
				character.ServerMessage(L("Hauberk is driven into Galutin Solitary Confinement. Talk to Sigita."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
