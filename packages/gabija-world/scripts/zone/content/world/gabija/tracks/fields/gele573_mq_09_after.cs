//--- Melia Script ----------------------------------------------------------
// Gesti at Uzbaiga Hillside
//--- Description -----------------------------------------------------------
// The trap is sprung, and Gesti learns the revelation was a lure.
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

[TrackScript("GELE573_MQ_09_AFTER")]
public class Gele573Mq09After : TrackScript
{
	protected override void Load()
	{
		SetId("GELE573_MQ_09_AFTER");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(103.57f, 350.07f, -155.22f));

		actors.Add(AddTrackActor(character, 57223, 62, 350, -135, 0, new TrackActorSpec { Faction = FactionType.Neutral, Level = 27 }));
		actors.Add(AddTrackActor(character, 147373, 66.51, 350.07, -153.72, 0, new TrackActorSpec { Faction = FactionType.Neutral, Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147371, 50.22, 350.07, -256.35, 2, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 50.22, 350.07, -256.35, 1, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20024, 52.46, 350.07, -288.26, 17, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 20024, 77.41, 350.07, -90.21, 6, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 2);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 32:
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("So, this is where the revelation is hidden."));
				break;
			case 77:
				StartDialog(track, L("A fake revelation... all this so you can lure me out and get rid of me?"),
					L("How disappointing to see such trickery is the best that Laima can prepare."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
