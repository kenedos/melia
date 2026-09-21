//--- Melia Script ----------------------------------------------------------
// After the crack closes
//--- Description -----------------------------------------------------------
// The crack shrinks away, Hauberk's soul goes into it, and the Kupoles are
// left with nothing to hold.
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

[TrackScript("VPRISON515_MQ_06_AFTER")]
public class Vprison515Mq06AfterTrack : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON515_MQ_06_AFTER");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-123.47f, 23.12f, -187.34f));

		actors.Add(AddTrackActor(character, 154010, -65.46, 23.12, -190.29, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Vakarine") }));
		actors.Add(AddTrackActor(character, 20026, -2.22, 23.06, -97.66, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Dimensional Crack"), EndPosition = new Position(-44.00f, 23.12f, -57.55f) }));
		actors.Add(AddTrackActor(character, 154015, -212.49, 26.79, -146.71, 260, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 154013, -240.99, 26.79, -42.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Daiva") }));
		actors.Add(AddTrackActor(character, 154016, -197.30, 26.79, 94.58, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Medeina") }));
		actors.Add(AddTrackActor(character, 154011, -64.28, 26.79, 160.07, 120, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Audra") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 62.19, 22.96, -148.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 57827, -68.32, 23.12, -96.63, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Hauberk") }));
		actors.Add(AddTrackActor(character, 20025, -65.30, 23.12, -99.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -50.47, 23.12, -114.94, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-72.32f, 23.12f, -121.07f) }));
		actors.Add(AddTrackActor(character, 20025, -84.59, 26.79, -57.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -212.49, 26.79, -146.71, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -240.99, 26.79, -42.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -197.30, 26.79, 94.58, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -64.28, 26.79, 160.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		// Every actor the cutscene closes away carries Client="BOTH", so the
		// removals only ever run from here.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 5);
		RemoveTrackActor(character, track, 8);
		RemoveTrackActor(character, track, 9);
	}
}
