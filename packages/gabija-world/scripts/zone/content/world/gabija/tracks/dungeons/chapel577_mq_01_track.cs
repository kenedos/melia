//--- Melia Script ----------------------------------------------------------
// Gesti in the cathedral
//--- Description -----------------------------------------------------------
// Algis brings you up to watch Gesti at work.
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

[TrackScript("CHAPLE577_MQ_01_TRACK")]
public class Chaple577Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE577_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-632.67f, 35.92f, -958.09f));

		actors.Add(AddTrackActor(character, 147371, -321.69, 35.92, -846.42, 310, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-317.52f, 35.92f, -776.33f) }));
		actors.Add(AddTrackActor(character, 41230, -312.39, 35.92, -943.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-272.13f, 35.92f, -1010.45f) }));
		actors.Add(AddTrackActor(character, 147390, -633, 36, -934, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}
}
