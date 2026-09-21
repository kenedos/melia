//--- Melia Script ----------------------------------------------------------
// The trap at Apgaule Altar
//--- Description -----------------------------------------------------------
// The demons that were told where the key is spend the altar's trap on
// themselves, and then notice who told them.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL56_MQ04_TRACK")]
public class Cathedral56Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL56_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153018, -1014.98, 0.49, 254.26, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Apgaule Altar") }));
		actors.Add(AddTrackActor(character, 57370, -1098.49, 0, 177.20, 64, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57371, -973.60, 0.50, 231.88, 11, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 2 }));
		actors.Add(AddTrackActor(character, 57370, -1015.57, 0.50, 187.89, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 2 }));
		actors.Add(AddTrackActor(character, 57371, -1062.39, 0, 110.45, 69, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57371, -968.26, 0, 121.87, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57370, -1110.48, 0, 108.84, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57370, -923.19, 0, 117.22, 1, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57371, -1046.01, 0.50, 231.29, 24, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 2 }));
		actors.Add(AddTrackActor(character, 57371, -960.80, 0, 175.65, 28, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57370, -906.67, 0, 186.43, 32, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20025, -1013.89, 0.50, 248.48, 30, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Key") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				// The altar the trap tears apart, before the rest is armed.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("The trap is spent. Take Maven's Fourth Key off the demons!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
