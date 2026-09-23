//--- Melia Script ----------------------------------------------------------
// The Woodspirit on Adata Highway
//--- Description -----------------------------------------------------------
// The great fire on Adata Highway draws out a Woodspirit.
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

[TrackScript("SIAU16_SQ_04_TRACK")]
public class Siau16Sq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU16_SQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-437.72f, 74.85f, 1552.33f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 47223, -482.46, 74.85, 1531.59, 92, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 57994, -548.58, 74.85, 1737.23, 12, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 31:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Defeat the Woodspirit that suddenly appeared!"), 3);
				break;

			case 34:
				RemoveTrackActor(character, track, 1);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
