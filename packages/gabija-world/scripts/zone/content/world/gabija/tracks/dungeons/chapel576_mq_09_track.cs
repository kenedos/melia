//--- Melia Script ----------------------------------------------------------
// The Mallet Wyvern trap
//--- Description -----------------------------------------------------------
// Touching the Central Altar springs an ambush.
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

[TrackScript("CHAPLE576_MQ_09_TRACK")]
public class Chaple576Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE576_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-548.95f, 10.97f, 440.23f));

		actors.Add(AddTrackActor(character, 147358, -523, 12, 446, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Central Altar") }));
		actors.Add(AddTrackActor(character, 47502, -525.00, 0.49, 304.46, 0, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(AddTrackActor(character, 40069, -837.16, 2.83, 414.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -559.04, 1.39, 784.21, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -222.37, 3.25, 492.01, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -225.66, 3.25, 362.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -492.71, 0.02, 88.71, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 40069, -498.94, 1.02, 782.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -841.19, 2.83, 475.63, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -551.38, 0.02, 91.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 40069, -225.91, 3.25, 422.12, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 12:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
