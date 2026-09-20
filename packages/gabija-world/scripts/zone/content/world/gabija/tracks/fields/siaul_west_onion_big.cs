//--- Melia Script ----------------------------------------------------------
// The Hidden Large Kepa
//--- Description -----------------------------------------------------------
// The Large Kepa the searcher suspected, with its Kepa escort.
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

[TrackScript("SIAUL_WEST_ONION_BIG_TRACK")]
public class SiaulWestOnionBigTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_WEST_ONION_BIG_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1905f, 261f, 61f));

		actors.Add(AddTrackActor(character, 57407, -1867, 261, 248, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		var kepa = new TrackActorSpec { Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 400001, -1946, 261, 201, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1931, 261, 155, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1906, 261, 245, 0, kepa));
		actors.Add(AddTrackActor(character, 400001, -1825, 261, 141, 8, kepa));
		actors.Add(AddTrackActor(character, 400001, -1798, 261, 193, 40, kepa));
		actors.Add(AddTrackActor(character, 400001, -1818, 261, 251, 110, kepa));

		actors.Add(AddTrackActor(character, 40080, -1693, 261, -150, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
