//--- Melia Script ----------------------------------------------------------
// The Poata at Margas Hill
//--- Description -----------------------------------------------------------
// The beast the Watchers fear will bring the cable car down.
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

[TrackScript("GELE571_MQ_05_TRACK")]
public class Gele571Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE571_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(782.32f, 368.01f, -1392.89f));

		actors.Add(AddTrackActor(character, 57115, 792.61, 368.02, -1355.09, 72, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 47203, 793, 368, -1362, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Poata's Nest") }));

		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 23:
				RemoveTrackActor(character, track, 1);
				break;
			case 34:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
