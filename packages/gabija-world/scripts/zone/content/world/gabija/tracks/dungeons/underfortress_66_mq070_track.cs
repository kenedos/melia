//--- Melia Script ----------------------------------------------------------
// What the hidden room was holding
//--- Description -----------------------------------------------------------
// Amanda opens the box in the room behind the camp and finds scrolls of a
// defensive magic circle.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("UNDERFORTRESS_66_MQ070_TRACK")]
public class Underfortress66Mq070Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_66_MQ070_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153040, -414.75, 303.23, 624.47, 73, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grave Robber Amanda") }));
		actors.Add(AddTrackActor(character, 147394, -336.02, 303.23, 796.36, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Unknown Box") }));
		actors.Add(AddTrackActor(character, 147469, -453.61, 303.23, 617.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Hidden Area") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 38:
				character.ServerMessage(L("The box is full of scrolls of a defensive magic circle nobody drew."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
