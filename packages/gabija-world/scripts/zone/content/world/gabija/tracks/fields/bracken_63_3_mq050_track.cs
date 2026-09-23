//--- Melia Script ----------------------------------------------------------
// The Gremlin at the Monastery Gate
//--- Description -----------------------------------------------------------
// A Gremlin and its Vubbe Warriors guard the way into the Novaha Monastery.
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

[TrackScript("BRACKEN_63_3_MQ050_TRACK")]
public class Bracken633Mq050Track : TrackScript
{
	protected override void Load()
	{
		SetId("BRACKEN_63_3_MQ050_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-155.22f, 72.00f, -742.04f));

		actors.Add(AddTrackActor(character, 57455, 83.06, -0.95, -997.24, 30, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 36, EndPosition = new Position(-122.89f, -0.36f, -970.87f) }));
		actors.Add(AddTrackActor(character, 103024, -25.46, -3.50, -1042.82, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-172.83f, 0f, -967.45f) }));
		actors.Add(AddTrackActor(character, 103024, -11.13, 0, -948.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-90.79f, 0f, -899.17f) }));
		actors.Add(AddTrackActor(character, 103024, -98.22, -1.84, -1070.51, 48, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-215.99f, 0f, -1019.06f) }));
		actors.Add(AddTrackActor(character, 153119, -145, 77.33, -728, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(AddTrackActor(character, 103024, -61.03, -0.05, -913.28, 128, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-158.46f, 0f, -909.97f) }));
		actors.Add(AddTrackActor(character, 41362, -275.11, 0.56, -912.96, 44, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-111.10f, -0.49f, -974.95f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 50:
				// The Vubbe Chaser the Gremlin kills dies on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 6);

				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
