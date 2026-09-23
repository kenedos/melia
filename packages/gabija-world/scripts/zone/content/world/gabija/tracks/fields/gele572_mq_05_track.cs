//--- Melia Script ----------------------------------------------------------
// Simorph at Valio Mountain Cabin Hill
//--- Description -----------------------------------------------------------
// The totems are broken, and the corruption answers.
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

[TrackScript("GELE572_MQ_05_TRACK")]
public class Gele572Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE572_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1011.90f, 503.86f, -1111.68f));

		actors.Add(AddTrackActor(character, 57151, 988.86, 503.86, -1133.54, 9, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153067, 990.90, 503.86, -1143.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 153068, 985.11, 503.86, -1126.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20026, 1003.61, 503.86, -1279.15, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				character.ServerMessage(L("Destroying the totem made Simorph angry"));
				break;
			case 30:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				break;
			case 34:
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
