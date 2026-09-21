//--- Melia Script ----------------------------------------------------------
// The goddess at the seal tower
//--- Description -----------------------------------------------------------
// The tower takes the orb, holds, and Austeja steps out of it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAULIAI_46_2_MQ_04_TRACK")]
public class Siauliai462Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_2_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147414, 1079, -73, 4705, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Seal Tower") }));
		actors.Add(AddTrackActor(character, 151041, 1070, -73, 4905, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Austeja") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				character.ServerMessage(L("The seal holds, and Goddess Austeja is standing beside the tower."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
