//--- Melia Script ----------------------------------------------------------
// Tess' Pursuers
//--- Description -----------------------------------------------------------
// The demons that hunted Tess catch up with her hiding spot.
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

[TrackScript("BRACKEN_63_2_MQ040_TRACK")]
public class Bracken632Mq040Track : TrackScript
{
	protected override void Load()
	{
		SetId("BRACKEN_63_2_MQ040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(438.02f, 284.16f, -543.76f));

		actors.Add(AddTrackActor(character, 20064, 423.53, 284.15, -563.39, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Tess"), EndPosition = new Position(594.71f, 284.16f, -490.84f) }));
		actors.Add(AddTrackActor(character, 153119, 485.19, 284.16, -348.77, 58, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose"), EndPosition = new Position(604.32f, 284.16f, -473.25f) }));
		actors.Add(AddTrackActor(character, 103023, 125.74, 284.31, -203.25, 90, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(316.21f, 284.16f, -396.48f) }));
		actors.Add(AddTrackActor(character, 103023, 75.65, 284.16, -423.55, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(257.30f, 284.16f, -481.69f) }));
		actors.Add(AddTrackActor(character, 103023, 120.57, 284.31, -249.71, 72, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(271.00f, 284.16f, -404.74f) }));
		actors.Add(AddTrackActor(character, 103023, 267.80, 284.16, -133.72, 86, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(379.63f, 284.16f, -367.74f) }));
		actors.Add(AddTrackActor(character, 57640, 146.95, 284.16, -117.61, 84, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(297.20f, 284.16f, -318.23f) }));
		actors.Add(AddTrackActor(character, 57640, 24.60, 282.49, -209.75, 80, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(185.56f, 284.16f, -401.22f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 45:
				// Tess and Rose run off on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
