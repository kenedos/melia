//--- Melia Script ----------------------------------------------------------
// Into Nuaele's territory
//--- Description -----------------------------------------------------------
// Aldona carries the Revelator past the gate, and Hauberk's old servants
// turn on Nuaele where she is standing.
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

[TrackScript("VPRISON512_MQ_05_TRACK")]
public class Vprison512Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON512_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1018.32f, 254.14f, -59.72f));

		actors.Add(AddTrackActor(character, 57412, 538.79, 395.46, -1592.61, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Demon Lord Nuaele"), EndPosition = new Position(646.16f, 395.46f, -1594.37f) }));
		actors.Add(AddTrackActor(character, 57581, 989.97, 254.14, -58.18, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, Level = 177, Name = L("Kupole Aldona") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57827, 750.72, 395.46, -1576.97, 31, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, Level = 177, Name = L("Demon Lord Hauberk"), EndPosition = new Position(725.95f, 395.46f, -1576.55f) }));
		actors.Add(AddTrackActor(character, 57690, 496.58, 395.46, -1527.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(726.76f, 395.46f, -1567.65f) }));
		actors.Add(AddTrackActor(character, 57690, 507.40, 395.46, -1649.04, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(722.72f, 395.46f, -1610.88f) }));
		actors.Add(AddTrackActor(character, 57690, 480.77, 395.46, -1611.50, 2, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(714.10f, 395.46f, -1592.75f) }));
		actors.Add(AddTrackActor(character, 12082, 750.72, 395.46, -1576.97, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20024, 629.10, 395.46, -1574.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Dialog.SetTitle(L("Kupole Aldona"));
				StartDialog(track,
					L("We will be entering Nuaele's territory now."),
					L("Prepare yourself.")
				);
				break;

			case 67:
				// The Nukas the cutscene kills off, before the rest is armed.
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				SetTrackTendency(character, track);
				break;

			case 69:
				character.ServerMessage(L("Defeat Demon Lord Nuaele!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
