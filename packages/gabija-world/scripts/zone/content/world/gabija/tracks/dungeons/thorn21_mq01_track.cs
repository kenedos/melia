//--- Melia Script ----------------------------------------------------------
// The Infro Holders after Bronius
//--- Description -----------------------------------------------------------
// The pack that chased the Believer up the path catches him.
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

[TrackScript("THORN21_MQ01_TRACK")]
public class Thorn21Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(3611.7993f, 400.75159f, -266.54651f));

		actors.Add(AddTrackActor(character, 147389, 3653, 400, -229, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Believer Bronius") }));
		actors.Add(AddTrackActor(character, 57598, 3730.8828, 388.72293, -455.8764, 38, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3708.551f, 400.75159f, -373.68051f) }));
		actors.Add(AddTrackActor(character, 57598, 3687.1494, 399.9967, -511.83658, 22, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3692.7358f, 400.75159f, -472.48441f) }));
		actors.Add(AddTrackActor(character, 57598, 3616.8093, 400.75159, -524.48285, 35, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3619.0154f, 400.75159f, -439.76431f) }));
		actors.Add(AddTrackActor(character, 41268, 3523.9397, 400.75159, -506.11801, 53, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3593.1431f, 400.75159f, -412.48346f) }));
		actors.Add(AddTrackActor(character, 41268, 3710.344, 393.16653, -493.37128, 22, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3709.4241f, 397.46347f, -441.05093f) }));
		actors.Add(AddTrackActor(character, 41268, 3646.0342, 400.75159, -522.96045, 28, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3670.3245f, 400.75159f, -465.11847f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
