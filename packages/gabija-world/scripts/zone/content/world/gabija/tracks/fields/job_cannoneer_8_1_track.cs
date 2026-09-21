//--- Melia Script ----------------------------------------------------------
// The observation orb at Ghresmei Passage
//--- Description -----------------------------------------------------------
// One of the strange objects the Cannoneer Master heard about, sitting in
// plain sight of the kingdom camp.
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

[TrackScript("JOB_CANNONEER_8_1_TRACK")]
public class JobCannoneer81Track : TrackScript
{
	protected override void Load()
	{
		SetId("JOB_CANNONEER_8_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1432.50f, 662.24f, 444.97f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 58533, 1375.19, 662.20, 423.74, 0, new TrackActorSpec { Ai = "BT_Dummy", Name = L("Observation Orb") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				// The client plays the shot as a minigame; the orb is the
				// server's to hand over.
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Destroy the Observation Orb!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
