//--- Melia Script ----------------------------------------------------------
// Capria at Spalva Junction
//--- Description -----------------------------------------------------------
// Molly's plan to tame the Pantos, and how it ends.
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

[TrackScript("GELE571_MQ_07_TRACK")]
public class Gele571Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE571_MQ_07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(964.26f, 266.20f, 996.48f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57210, 667.83, 267.47, 1188.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		actors.Add(AddTrackActor(character, 147451, 950.72, 266.38, 963.03, 60, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(507.39f, 267.47f, 1072.59f) }));
		actors.Add(AddTrackActor(character, 147451, 980.37, 253.33, 961.09, 52, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(569.25f, 267.47f, 1016.21f) }));
		actors.Add(AddTrackActor(character, 147451, 999.48, 251.84, 995.80, 43, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(802.64f, 267.47f, 1362.91f) }));
		actors.Add(AddTrackActor(character, 147451, 980.46, 264.53, 1025.36, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(697.45f, 267.47f, 1384.29f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 16:
				character.ServerMessage(L("The Baby Pantos are returning to Capria"));
				break;
			case 51:
				character.ServerMessage(L("It seems that Capria is upset! Give up the plan and defeat Capria!"));
				break;
			case 55:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				break;
			case 59:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
