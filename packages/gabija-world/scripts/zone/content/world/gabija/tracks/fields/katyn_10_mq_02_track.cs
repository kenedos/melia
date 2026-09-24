//--- Melia Script ----------------------------------------------------------
// Following the Light
//--- Description -----------------------------------------------------------
// The Owl Sculptures pass a light bead along towards Bonewide Cliff.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("KATYN_10_MQ_02_TRACK")]
public class Katyn10Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("KATYN_10_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(3224.03f, 73.81f, -1322.36f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 12081, 2726.46, 73.81, -1027.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48002, 2763, 134.52, -819, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48003, 3517.92, 134.52, -299.14, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48004, 3072.55, 134.52, 294.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48002, 2307.06, 134.52, 163.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48003, 1978.54, 200.96, 269.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));

		return actors.ToArray();
	}
}
