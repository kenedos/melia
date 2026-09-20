//--- Melia Script ----------------------------------------------------------
// The Biteregina at Rojus Plateau
//--- Description -----------------------------------------------------------
// A hive the forest never should have grown.
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

[TrackScript("GELE574_MQ_01_TRACK")]
public class Gele574Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE574_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 400122, -1017, -79, 2085, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Small Beehive") }));
		actors.Add(AddTrackActor(character, 57268, -910.85, -80.05, 2417.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				RemoveTrackActor(character, track, 0);
				break;
			case 19:
				RemoveTrackActor(character, track, 0);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
