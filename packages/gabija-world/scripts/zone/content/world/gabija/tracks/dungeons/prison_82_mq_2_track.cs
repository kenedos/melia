//--- Melia Script ----------------------------------------------------------
// The Observation Detector
//--- Description -----------------------------------------------------------
// The repaired detector looks out over the last demon barrier, and it is
// stronger than anything before it.
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

[TrackScript("PRISON_82_MQ_2_TRACK")]
public class Prison82Mq2Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_82_MQ_2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(479.71f, 1231.04f, -1429.12f));

		actors.Add(AddTrackActor(character, 151003, -550.00, 618.94, -1577.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 147504, 448.00, 1231.04, -1402.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Observation Detector") }));
		actors.Add(AddTrackActor(character, 20026, -109.57, 486.84, 1211.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-198.17f, 486.89f, 1110.20f) }));
		actors.Add(AddTrackActor(character, 20026, 887.29, 763.47, 1042.61, 3, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(818.26f, 763.94f, 1119.56f) }));
		actors.Add(AddTrackActor(character, 20026, 1083.65, 944.26, -51.34, 50, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(1075.98f, 944.26f, 20.79f) }));
		actors.Add(AddTrackActor(character, 20026, 267.28, 652.68, 1361.54, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20026, 626.43, 1231.04, -1412.50, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(743.56f, 1231.04f, -1395.68f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_pattern008_violet_loop", 2.5f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_bg_rize002_violet2", 0.8f, EffectLocation.Bottom);
				break;

			case 38:
				character.ServerMessage(L("It's exuding a power much stronger than the other demon barriers!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
