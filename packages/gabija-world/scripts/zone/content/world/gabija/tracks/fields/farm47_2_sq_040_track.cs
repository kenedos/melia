//--- Melia Script ----------------------------------------------------------
// The wing that was worn
//--- Description -----------------------------------------------------------
// The carved wing standing out of the ground pulls the Corrupted up with it.
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

[TrackScript("FARM47_2_SQ_040_TRACK")]
public class Farm472Sq040Track : TrackScript
{
	protected override void Load()
	{
		SetId("FARM47_2_SQ_040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-20.51f, -92.48f, -1774.68f));

		actors.Add(AddTrackActor(character, 57435, -58.52, -92.48, -1763.91, 114, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Corrupted") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153049, -58.28, -92.48, -1763.47, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Wing of Goddess Statue") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				// The wing the Corrupted stands up wearing, before the fight
				// is armed.
				RemoveTrackActor(character, track, 2);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Put the Corrupted down and take the wing off it!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
