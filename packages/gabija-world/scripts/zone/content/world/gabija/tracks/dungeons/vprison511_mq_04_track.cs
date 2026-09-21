//--- Melia Script ----------------------------------------------------------
// Draining Blut's Altar
//--- Description -----------------------------------------------------------
// Hauberk takes the power the altar gathered, and the altar fights him for
// it the whole way.
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

[TrackScript("VPRISON511_MQ_04_TRACK")]
public class Vprison511Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON511_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1791.08f, 351.35f, 510.83f));

		actors.Add(AddTrackActor(character, 41327, -1831.11, 351.35, 510.37, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Blut's Altar") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57840, -1888.74, 351.35, 498.85, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Hauberk") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				character.ServerMessage(L("Protect Hauberk while he absorbs power of the altar!"));
				break;

			case 24:
				// The client plays the drain as a minigame; the server ends
				// the phase where its last frame does.
				character.ServerMessage(L("Hauberk has taken what the altar held. Go to Zydrone in Nuzikalti Hall."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
