//--- Melia Script ----------------------------------------------------------
// The Magic Vessels of the second floor
//--- Description -----------------------------------------------------------
// The guardians that lost their reason come for the vessels.
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

[TrackScript("ZACHA2F_SQ_05_TRACK")]
public class Zacha2fSq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147307, 2098, 714, -244, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 50, Name = L("Magic Vessel") }));
		actors.Add(AddTrackActor(character, 147307, 2224, 714, -244, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 50, Name = L("Magic Vessel") }));
		actors.Add(AddTrackActor(character, 41277, 2282.10, 714.30, -143.82, 26, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2240.66f, 714.31f, -192.86f) }));
		actors.Add(AddTrackActor(character, 41277, 2207.72, 714.31, -297.80, 190, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41277, 2066.05, 714.31, -337.86, 45, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2091.32f, 714.31f, -299.71f) }));
		actors.Add(AddTrackActor(character, 41277, 2017.40, 714.30, -151.20, 30, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2043.64f, 714.31f, -192.26f) }));
		actors.Add(AddTrackActor(character, 41274, 2309.24, 714.31, -339.31, 48, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2260.39f, 714.31f, -279.64f) }));
		actors.Add(AddTrackActor(character, 41274, 1998.86, 714.31, -247.52, 45, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2052.77f, 714.31f, -245.22f) }));
		actors.Add(AddTrackActor(character, 41274, 2327.91, 714.31, -239.33, 54, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2261.90f, 714.31f, -244.82f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				track.Actors[0].AttachEffect("F_wizard_energybolt_hit_explosion", 2, EffectLocation.Middle);
				break;
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
