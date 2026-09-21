//--- Melia Script ----------------------------------------------------------
// What was in the box
//--- Description -----------------------------------------------------------
// The Monocle picks a box out of the quarter, and the box is full of Rambear.
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

[TrackScript("UNDERFORTRESS_67_MQ010_TRACK")]
public class Underfortress67Mq010Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_67_MQ010_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(122.79f, 287.77f, -1474.78f));

		actors.Add(AddTrackActor(character, 153040, 94.87, 287.77, -1444.47, 55, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Amanda") }));
		actors.Add(AddTrackActor(character, 57964, 7.24, 287.77, -1200.92, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 207 }));
		actors.Add(AddTrackActor(character, 57964, -129.82, 283.41, -1078.51, 22, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 207, EndPosition = new Position(-23.37f, 287.77f, -1109.56f) }));
		actors.Add(AddTrackActor(character, 57965, 163.58, 281.37, -946.62, 19, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 207, EndPosition = new Position(91.88f, 287.77f, -1010.99f) }));
		actors.Add(AddTrackActor(character, 57966, 127.09, 287.77, -1153.41, 29, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 207 }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 151029, 1.11, 287.77, -1208.44, 32, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Pile of Boxes") }));
		actors.Add(AddTrackActor(character, 151030, 123.32, 287.77, -1151.77, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Pile of Boxes") }));
		actors.Add(AddTrackActor(character, 151030, 28.74, 287.77, -1195.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Pile of Boxes") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				track.Dialog.SetTitle(L("Grave Robber Amanda"));
				StartDialog(track, L("Now. Let's see."));
				break;

			case 15:
				track.Dialog.SetTitle(L("Grave Robber Amanda"));
				StartDialog(track,
					L("Oh, I think I found it."),
					L("You see that box over there? That's it.")
				);
				break;

			case 45:
				// The boxes the Rambear come out of, before the fight is armed.
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 8);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("The box was full of demons. Put them down!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
