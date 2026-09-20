//--- Melia Script ----------------------------------------------------------
// Shnayim at the broken tombstone
//--- Description -----------------------------------------------------------
// The guardian of the deep hall comes down on whoever reads the tombstone.
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

[TrackScript("ZACHA2F_MQ_05_TRACK")]
public class Zacha2fMq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(26.52f, 717.38f, 1203.68f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57093, -325.93, 775.24, 1886.35, 75, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-218.23f, 756.39f, 1757.29f) }));
		actors.Add(AddTrackActor(character, 47252, 31, 720, 1219, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Tombstone") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 25:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
