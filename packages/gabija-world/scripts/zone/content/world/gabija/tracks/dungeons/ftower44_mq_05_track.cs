//--- Melia Script ----------------------------------------------------------
// The stairs to the fifth floor
//--- Description -----------------------------------------------------------
// Grinender stands across the only way up.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER44_MQ_05_TRACK")]
public class Ftower44Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER44_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2242.99f, 525.18f, 53.45f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 41374, -2408.23, 525.34, 57.41, 55, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat Grinender, who blocks the stairs to the 5th floor!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
