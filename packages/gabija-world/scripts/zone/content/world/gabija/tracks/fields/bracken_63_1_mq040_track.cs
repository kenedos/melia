//--- Melia Script ----------------------------------------------------------
// Cassius Heads Back
//--- Description -----------------------------------------------------------
// Cassius and Laswi set off for the cabin as Vubbe Chasers pour down the hill.
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

[TrackScript("BRACKEN_63_1_MQ040_TRACK")]
public class Bracken631Mq040Track : TrackScript
{
	protected override void Load()
	{
		SetId("BRACKEN_63_1_MQ040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-206.72f, 544.59f, -1030.61f));

		actors.Add(AddTrackActor(character, 155037, -193.66, 544.58, -1016.55, 120, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Cassius"), EndPosition = new Position(-106.29f, 544.59f, -1143.37f) }));
		actors.Add(AddTrackActor(character, 103036, -181.17, 544.58, -1047.91, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName", EndPosition = new Position(-72.70f, 559.85f, -1168.61f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 103023, -639.49, 531.97, -649.26, 76, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-315.76f, 544.58f, -1013.37f) }));
		actors.Add(AddTrackActor(character, 103023, -594.31, 531.97, -642.40, 70, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-294.37f, 544.59f, -976.20f) }));
		actors.Add(AddTrackActor(character, 103023, -553.72, 531.97, -631.47, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-347.12f, 544.58f, -1053.22f) }));
		actors.Add(AddTrackActor(character, 103023, -704.94, 538.38, -671.41, 75, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-391.25f, 544.58f, -1033.00f) }));
		actors.Add(AddTrackActor(character, 103023, -627.25, 531.97, -593.19, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-313.13f, 544.59f, -941.54f) }));
		actors.Add(AddTrackActor(character, 103023, -587.50, 531.97, -598.88, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-263.15f, 544.59f, -953.26f) }));
		actors.Add(AddTrackActor(character, 103023, -671.62, 531.97, -594.89, 82, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-354.64f, 544.58f, -1009.98f) }));
		actors.Add(AddTrackActor(character, 103023, -696.07, 531.97, -621.27, 73, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-393.34f, 544.59f, -979.76f) }));
		actors.Add(AddTrackActor(character, 103023, -664.83, 534.29, -682.37, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-360.93f, 544.59f, -956.23f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 45:
				// Cassius and Laswi leave on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
