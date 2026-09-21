//--- Melia Script ----------------------------------------------------------
// The priest at the Wood Watch Tower
//--- Description -----------------------------------------------------------
// An Austeja follower comes out to look at the altar pieces and leaves again
// for the Uskis Arable Land.
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

[TrackScript("SIAULIAI_46_3_MQ_05_TRACK")]
public class Siauliai463Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_3_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1913.62f, 134.31f, 860.92f));

		actors.Add(AddTrackActor(character, 147499, 1878, 135, 856, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Lamar") }));
		actors.Add(AddTrackActor(character, 147485, 1902, 135, 604, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Riesz") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 147491, 2012.14, 134.31, 1043.35, 31, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Raeli"), EndPosition = new Position(2014.67f, 134.31f, 978.54f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				character.ServerMessage(L("Priest Raeli has gone on to the Uskis Arable Land."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
