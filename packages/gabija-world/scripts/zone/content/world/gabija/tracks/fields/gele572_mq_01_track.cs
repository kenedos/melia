//--- Melia Script ----------------------------------------------------------
// Meeting the Paladin Master
//--- Description -----------------------------------------------------------
// Uska sent you to Gele Plateau, and the Master is waiting.
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

[TrackScript("GELE572_MQ_01_TRACK")]
public class Gele572Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE572_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(15.29f, 375.40f, -1010.97f));

		actors.Add(AddTrackActor(character, 57223, 30.06, 375.40, -751.60, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 0);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				track.Dialog.SetTitle(L("Paladin Master"));
				track.Dialog.SetPortrait("Dlg_port_Vlaentinas_Naimon");
				StartDialog(track, L("Welcome. I heard you were coming from Uska."),
					L("Don't just stand there. I wish to have a word with you."));
				break;
			case 39:
				StartDialog(track, L("So, you're the one who got the revelation from the Crystal Mine, right?"),
					L("If it's alright with you, I'd like to hear the details."));
				break;
			case 50:
				StartDialog(track, L("Amazing. We also follow the first Paladin's will and came here to guard this place."),
					L("Same goes for the Watchers here."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
