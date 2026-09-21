//--- Melia Script ----------------------------------------------------------
// Burning the Ramus Crossroads circle
//--- Description -----------------------------------------------------------
// Varas' woodpile catches, and the shield over the circle burns through with
// it.
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

[TrackScript("FARM47_2_SQ_070_TRACK")]
public class Farm472Sq070Track : TrackScript
{
	protected override void Load()
	{
		SetId("FARM47_2_SQ_070_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-602.40f, 0.71f, 1067.23f));

		actors.Add(AddTrackActor(character, 153047, -668.16, 0.71, 1062.21, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Unstable Magic Circle") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47223, -627.14, 0.71, 1065.65, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Stacked Firewood") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				// The client plays the fire as a minigame; the notice is the
				// server's.
				character.ServerMessage(L("The magic circle is on fire! Get kindling from Orange Dandel and keep the fire alive!"));
				break;

			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
