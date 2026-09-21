//--- Melia Script ----------------------------------------------------------
// The addled Revelators
//--- Description -----------------------------------------------------------
// Five Revelators the evil energy took walk out of the wood at the village.
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

[TrackScript("SIAULIAI_46_1_MQ_03_TRACK")]
public class Siauliai461Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_1_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1372.98f, 261.96f, -187.01f));

		actors.Add(AddTrackActor(character, 147492, -1352, 262, -156, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Dazine") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57306, -1102.35, 261.96, -611.06, 66, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 100, Name = L("Addled Krivis") }));
		actors.Add(AddTrackActor(character, 57307, -1552.84, 261.96, -619.19, 77, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 100, Name = L("Addled Scout") }));
		actors.Add(AddTrackActor(character, 57308, -1342.95, 261.96, -739.28, 71, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 100, Name = L("Addled Rodelero") }));
		actors.Add(AddTrackActor(character, 57309, -1217.69, 261.96, -563.89, 63, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 100, Name = L("Addled Monk") }));
		actors.Add(AddTrackActor(character, 57310, -1416.82, 261.96, -609.01, 77, new TrackActorSpec { Ai = "TrackWaitMonster", WalkSpeed = 100, Name = L("Addled Barbarian") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Put the addled Revelators down, then purify them with the symbol!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
