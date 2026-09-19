//--- Melia Script ----------------------------------------------------------
// The Panto Totem at Valyma Sanctum
//--- Description -----------------------------------------------------------
// Burning the totem is what makes the charm take hold.
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

[TrackScript("GELE574_MQ_06_TRACK")]
public class Gele574Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE574_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147356, -1563, 8, -792, 18, new TrackActorSpec { Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 57572, -1727.44, 7.25, -768.49, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1650.74, 7.18, -914.01, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1481.21, 7.18, -887.09, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57572, -1412.72, 7.18, -777.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
