//--- Melia Script ----------------------------------------------------------
// The first attempt at the crack
//--- Description -----------------------------------------------------------
// The demons the goddess feeds into the crack are not enough to fill it.
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

[TrackScript("VPRISON515_MQ_01_TRACK")]
public class Vprison515Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON515_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-98.99f, 29.52f, -288.70f));

		actors.Add(AddTrackActor(character, 154010, -108.86, 23.12, -181.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Vakarine") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20026, -151.44, 26.79, 41.15, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 57720, -251.39, 26.79, 74.87, 54, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-196.41f, 26.79f, -20.31f) }));
		actors.Add(AddTrackActor(character, 57720, -206.77, 26.79, 35.61, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-148.53f, 26.79f, -65.14f) }));
		actors.Add(AddTrackActor(character, 57720, -190.17, 26.79, -9.05, 37, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-184.20f, 26.79f, -83.44f) }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		// The three demons the ritual takes carry Client="BOTH", so they are
		// only ever removed from here.
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 5);
	}
}
