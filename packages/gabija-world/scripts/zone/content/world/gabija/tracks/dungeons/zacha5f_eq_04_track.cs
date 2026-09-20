//--- Melia Script ----------------------------------------------------------
// The empty slate
//--- Description -----------------------------------------------------------
// Rusrat close in on the desk where the Revelator's name was erased.
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

[TrackScript("ZACHA5F_EQ_04_TRACK")]
public class Zacha5fEq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_EQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-3422.98f, 295.42f, -2582.04f));

		actors.Add(AddTrackActor(character, 400801, -3264.68, 324.91, -2583.99, 328, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3360.86f, 297.67f, -2579.39f) }));
		actors.Add(AddTrackActor(character, 400801, -3443.58, 326.34, -2429.91, 288, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3441.25f, 310.93f, -2491.32f) }));
		actors.Add(AddTrackActor(character, 400801, -3604.51, 324.91, -2581.42, 108, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3542.27f, 313.59f, -2594.19f) }));
		actors.Add(AddTrackActor(character, 400801, -3440.46, 326.34, -2738.95, 131, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3443.06f, 295.42f, -2639.83f) }));
		actors.Add(AddTrackActor(character, 400801, -3397.23, 324.91, -2775.99, 101, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3420.86f, 324.91f, -2734.34f) }));
		actors.Add(AddTrackActor(character, 400801, -3235.70, 324.91, -2551.67, 326, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3293.16f, 324.91f, -2569.18f) }));
		actors.Add(AddTrackActor(character, 47254, -3442.65, 296.00, -2582.92, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Desk") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		CreateBattleBoxInLayer(character, track);
		SetTrackTendency(character, track);
	}
}
