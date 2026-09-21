//--- Melia Script ----------------------------------------------------------
// Taking the chain off Dionys
//--- Description -----------------------------------------------------------
// The suppressor opens, Aldona goes in with the Revelator, and Hauberk is
// already standing where the chain will land.
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

[TrackScript("VPRISON514_MQ_05_TRACK")]
public class Vprison514Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON514_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2114.26f, 444.21f, 802.91f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 154001, -2180.71, 441.19, 790.76, 9, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Magic Suppressor"), EndPosition = new Position(-2197.07f, 441.13f, 715.89f) }));
		actors.Add(AddTrackActor(character, 103013, -2597.50, 415.42, 762.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Dionys") }));
		actors.Add(AddTrackActor(character, 57581, -2139.40, 443.64, 792.79, 10, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 99999, Level = 100, Name = L("Kupole Aldona"), EndPosition = new Position(-2461.52f, 408.64f, 790.28f) }));
		actors.Add(AddTrackActor(character, 154001, -2180.71, 441.19, 790.76, 72, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Magic Suppressor"), EndPosition = new Position(-2173.01f, 439.48f, 861.64f) }));
		actors.Add(AddTrackActor(character, 154001, -2180.71, 441.19, 790.76, 18, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Magic Suppressor") }));
		actors.Add(AddTrackActor(character, 57827, -2491.06, 408.64, 948.61, 36, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 99999, Level = 100, Name = L("Demon Lord Hauberk"), EndPosition = new Position(-2487.76f, 408.64f, 898.29f) }));
		actors.Add(AddTrackActor(character, 20024, -2497.34, 408.64, 962.62, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		// The notice and the arming sit past the last frame the client
		// reports, so the fight is handed over from here.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 5);
		SetTrackTendency(character, track);
		character.ServerMessage(L("Get Dionys!"));
	}
}
