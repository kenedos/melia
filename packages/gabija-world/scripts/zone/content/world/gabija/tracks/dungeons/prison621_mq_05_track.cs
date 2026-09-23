//--- Melia Script ----------------------------------------------------------
// The Hidden Chamber's Guardian
//--- Description -----------------------------------------------------------
// Clymen ambushes the player at the entrance the bishop's map points to.
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

[TrackScript("PRISON621_MQ_05_TRACK")]
public class Prison621Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON621_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1947.97f, 199.73f, 600.80f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 57999, 317.31, 207.62, 1529.89, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57999, -282.49, 207.62, 1560.63, 260, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1883.67f, 199.73f, 732.30f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 22:
				// The Clymen that only shows its shadow leaves on a Client="BOTH" row.
				RemoveTrackActor(character, track, 1);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Clymen has appeared!"), 8);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
