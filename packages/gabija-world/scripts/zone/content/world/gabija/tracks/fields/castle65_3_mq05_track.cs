//--- Melia Script ----------------------------------------------------------
// The Shaman Doll and the Savior
//--- Description -----------------------------------------------------------
// Yane swaps Melchioras on the Life Absorbing Altar for her shaman doll.
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

[TrackScript("CASTLE65_3_MQ05_TRACK")]
public class Castle653Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_3_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(65.52f, 67.71f, -205.21f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155095, 57.50, 67.71, -176.52, 59, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane") }));
		actors.Add(AddTrackActor(character, 155094, 157.74, 67.71, -162.11, 3, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail") }));
		actors.Add(AddTrackActor(character, 155106, 99.75, 67.71, -142.96, 0, prop));
		actors.Add(AddTrackActor(character, 155113, 102.15, 67.71, -148.15, 19, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mage Melchioras") }));
		actors.Add(AddTrackActor(character, 57411, 103.09, 67.71, -152.36, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[3].AttachEffect("F_light081_ground_orange_loop2", 1.5f, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
