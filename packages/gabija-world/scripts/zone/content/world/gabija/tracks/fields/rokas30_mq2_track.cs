//--- Melia Script ----------------------------------------------------------
// The mercenary's body
//--- Description -----------------------------------------------------------
// The Hogma that killed the mercenary come back for whoever mourns him.
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

[TrackScript("ROKAS30_MQ2_TRACK")]
public class Rokas30Mq2Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_MQ2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 41435, 474.19, 325.60, 518.57, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(582.53f, 326.24f, 630.55f) }));
		actors.Add(AddTrackActor(character, 41434, 355.80, 325.14, 523.76, 74, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(482.57f, 326.24f, 648.59f) }));
		actors.Add(AddTrackActor(character, 41433, 409.07, 325.15, 453.07, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(499.83f, 326.13f, 562.50f) }));
		actors.Add(AddTrackActor(character, 41433, 527.18, 325.56, 402.12, 65, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(632.28f, 326.18f, 518.78f) }));
		actors.Add(AddTrackActor(character, 41434, 368.56, 325.53, 591.90, 32, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(438.02f, 326.18f, 626.39f) }));
		actors.Add(AddTrackActor(character, 20020, 740.25, 326.24, 629.08, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Body of a Soldier") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				character.ServerMessage(L("The monsters that led the soldiers to death have appeared! Defeat them and pray for their rest!"));
				break;
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
