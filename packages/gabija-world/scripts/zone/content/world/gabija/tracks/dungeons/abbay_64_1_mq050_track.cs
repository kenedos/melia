//--- Melia Script ----------------------------------------------------------
// Escape from the Reading Room
//--- Description -----------------------------------------------------------
// The villagers flee the Special Reading Room as a Mummyghast cuts off
// the way out.
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

[TrackScript("ABBAY_64_1_MQ050_TRACK")]
public class Abbay641Mq050Track : TrackScript
{
	protected override void Load()
	{
		SetId("ABBAY_64_1_MQ050_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-392.86f, 209.29f, -1937.72f));

		actors.Add(AddTrackActor(character, 155046, -421.99, 209.84, -1981.23, 28, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Monk Goss"), EndPosition = new Position(97.74f, 209.91f, -1467.58f) }));
		actors.Add(AddTrackActor(character, 153119, -396.47, 209.60, -1984.68, 16, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose"), EndPosition = new Position(105.90f, 209.91f, -1569.34f) }));
		actors.Add(AddTrackActor(character, 153111, -504.5, 210.31, -2037.18, 61, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rona"), EndPosition = new Position(180.55f, 208.48f, -1526.79f) }));
		actors.Add(AddTrackActor(character, 20063, -463, 210.31, -2035, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kornas"), EndPosition = new Position(132.72f, 208.48f, -1480.86f) }));
		actors.Add(AddTrackActor(character, 20061, -452.81, 210.31, -2078.81, 39, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Anne"), EndPosition = new Position(228.74f, 208.48f, -1599.85f) }));
		actors.Add(AddTrackActor(character, 20064, -474.93, 210.31, -2090.22, 28, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zacaras"), EndPosition = new Position(181.63f, 208.48f, -1560.61f) }));
		actors.Add(AddTrackActor(character, 153110, -517, 210.31, -2008, 92, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Allonas"), EndPosition = new Position(125.27f, 208.48f, -1513.37f) }));
		actors.Add(AddTrackActor(character, 153109, -530.74, 210.31, -2035.32, 61, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Litas"), EndPosition = new Position(147.98f, 208.48f, -1582.16f) }));
		actors.Add(AddTrackActor(character, 153117, -431, 210.03, -1992, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 103025, -180.61, 93.44, -1080.34, 118, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-201.23f, 208.48f, -1527.78f) }));
		actors.Add(AddTrackActor(character, 103025, -140.48, 93.52, -1067.03, 113, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-124.55f, 208.48f, -1520.79f) }));
		actors.Add(AddTrackActor(character, 57674, -178.64, 93.69, -1040.51, 158, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-224.28f, 208.48f, -1608.57f) }));
		actors.Add(AddTrackActor(character, 103025, -231.30, 93.47, -1075.69, 162, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-213.93f, 208.48f, -1659.49f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 103020, -199.60, 103.17, -1131.66, 134, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-157.21f, 208.48f, -1640.35f) }));
		actors.Add(AddTrackActor(character, 400662, -204.20, 142.95, -1232.69, 57, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-203.00f, 185.28f, -1334.96f) }));
		actors.Add(AddTrackActor(character, 57674, -177.97, 132.98, -1206.57, 78, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-175.79f, 190.36f, -1346.88f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 75:
				// Goss, Rose and the villagers run off, and two monsters die, on Client="BOTH" rows the client never reports.
				for (var i = 0; i <= 7; i++)
					RemoveTrackActor(character, track, i);
				RemoveTrackActor(character, track, 15);
				RemoveTrackActor(character, track, 16);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
