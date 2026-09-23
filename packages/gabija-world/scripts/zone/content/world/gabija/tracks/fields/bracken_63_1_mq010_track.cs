//--- Melia Script ----------------------------------------------------------
// Ambush at the Herb Gatherers' Cabin
//--- Description -----------------------------------------------------------
// Vubbe Chasers close in on Rose and the merchants hiding at the cabin.
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

[TrackScript("BRACKEN_63_1_MQ010_TRACK")]
public class Bracken631Mq010Track : TrackScript
{
	protected override void Load()
	{
		SetId("BRACKEN_63_1_MQ010_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(295.89f, 175.38f, 198.45f));

		actors.Add(AddTrackActor(character, 153119, -92.42, 175.13, -144.25, 330, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(AddTrackActor(character, 155039, -149.68, 175.14, -282.47, 285, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Gomez"), EndPosition = new Position(-74.88f, 175.14f, -147.44f) }));
		actors.Add(AddTrackActor(character, 155034, -93.61, 175.14, -307.08, 34, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Andres"), EndPosition = new Position(-89.38f, 175.14f, -216.89f) }));
		actors.Add(AddTrackActor(character, 155035, -12.54, 175.14, -21.01, 75, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant"), EndPosition = new Position(-76.66f, 175.14f, -115.39f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 103023, -376.78, 212.82, -212.88, 95, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-77.87f, 175.14f, -258.11f) }));
		actors.Add(AddTrackActor(character, 103023, -388.70, 205.53, -246.98, 118, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-66.13f, 175.14f, -302.49f) }));
		actors.Add(AddTrackActor(character, 103023, -405.99, 206.13, -257.04, 88, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-105.29f, 175.14f, -289.44f) }));
		actors.Add(AddTrackActor(character, 103023, -320.74, 180.46, -359.13, 94, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-188.46f, 175.14f, -343.66f) }));
		actors.Add(AddTrackActor(character, 103023, -356.31, 186.32, -346.25, 80, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-245.64f, 175.15f, -329.50f) }));
		actors.Add(AddTrackActor(character, 103023, -287.56, 178.66, -320.26, 62, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-169.03f, 175.14f, -281.69f) }));
		actors.Add(AddTrackActor(character, 103023, -364.87, 191.56, -316.79, 76, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-213.28f, 175.14f, -301.22f) }));
		actors.Add(AddTrackActor(character, 153128, -89.02, 175.14, -130.42, 10, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Laswi") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 33:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
