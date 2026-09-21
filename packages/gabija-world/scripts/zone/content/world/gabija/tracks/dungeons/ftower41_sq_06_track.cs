//--- Melia Script ----------------------------------------------------------
// The lizard at the barrier device
//--- Description -----------------------------------------------------------
// A Salamander comes down the hall for the device that was just switched on.
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

[TrackScript("FTOWER41_SQ_06_TRACK")]
public class Ftower41Sq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER41_SQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2348.73f, 1334.28f, -2283.46f));

		actors.Add(AddTrackActor(character, 57088, 2185.74, 1334.28, -2288.54, 188, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20025, 1632.02, 1334.28, -2287.04, 299, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(2239.68f, 1334.28f, -2292.04f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Salamander that is going after the Barrier Activation Device!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
