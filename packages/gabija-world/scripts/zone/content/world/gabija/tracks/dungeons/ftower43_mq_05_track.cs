//--- Melia Script ----------------------------------------------------------
// The Mineloader wakes
//--- Description -----------------------------------------------------------
// The guard that had not moved in years starts up in a cloud of smoke.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER43_MQ_05_TRACK")]
public class Ftower43Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER43_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(264.38f, 355.05f, -703.91f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 41225, 669.17, 359.32, -730.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 34:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("The Mineloader is online! Defeat the Mineloader!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
