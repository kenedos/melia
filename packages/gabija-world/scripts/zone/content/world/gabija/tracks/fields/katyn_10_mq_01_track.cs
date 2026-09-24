//--- Melia Script ----------------------------------------------------------
// Monsters at Bastymosi Field
//--- Description -----------------------------------------------------------
// Digos send light beads through the Owl Sculptures, then turn on the
// intruder.
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

[TrackScript("KATYN_10_MQ_01_TRACK")]
public class Katyn10Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("KATYN_10_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2757.39f, 73.81f, -1192.59f));

		actors.Add(AddTrackActor(character, 12081, 2726.46, 73.81, -1027.67, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 48002, 2763, 134.52, -819, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Owl Sculpture") }));
		actors.Add(AddTrackActor(character, 57525, 2599.14, 73.81, -1165.84, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57525, 2541.64, 73.81, -1108.30, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57525, 2671.91, 73.81, -1078.12, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57525, 2648, 73.81, -1009.33, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57525, 2588.85, 73.81, -1014.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("It seems that the monsters are using the{nl}Owl Sculptures to send light beads to somewhere else."), 5);
				break;
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
