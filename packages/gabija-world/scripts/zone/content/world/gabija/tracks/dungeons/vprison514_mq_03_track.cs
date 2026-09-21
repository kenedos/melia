//--- Melia Script ----------------------------------------------------------
// Charging the Evening Star Key
//--- Description -----------------------------------------------------------
// Zydrone holds the goddess' power into the key and cannot look up while she
// does it.
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

[TrackScript("VPRISON514_MQ_03_TRACK")]
public class Vprison514Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON514_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-539.43f, 334.60f, 373.70f));

		actors.Add(AddTrackActor(character, 154015, -942.16, 335.57, -451.51, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 20026, -982.13, 335.57, -434.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				// The client plays the defence as a minigame; the key finishes
				// with the cutscene either way.
				character.ServerMessage(L("Protect Zydrone until she completes the Evening Star Key!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
