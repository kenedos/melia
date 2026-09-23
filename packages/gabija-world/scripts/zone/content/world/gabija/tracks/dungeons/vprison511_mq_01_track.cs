//--- Melia Script ----------------------------------------------------------
// The meeting at the prison gate
//--- Description -----------------------------------------------------------
// Kupole Audra comes out to the arrival and speaks to what is riding with
// the Revelator rather than to the Revelator.
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

[TrackScript("VPRISON511_MQ_01_TRACK")]
public class Vprison511Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON511_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-54.73f, 169.83f, -106.13f));

		actors.Add(AddTrackActor(character, 57840, -13.20, 169.83, -3.11, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Hauberk") }));
		actors.Add(AddTrackActor(character, 154011, -162.52, 169.83, -70.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Audra") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Dialog.SetTitle(L("Demon Lord Hauberk"));
				track.Dialog.SetPortrait("Dlg_port_Hauberk_dark");
				StartDialog(track, L("You shouldn't trust anyone here. Not the goddess and not even yourself."));
				break;

			case 18:
				track.Dialog.SetTitle(L("Kupole Audra"));
				StartDialog(track,
					L("Demon Lord Hauberk, the goddess does not wish to fight you."),
					L("Cease your hostile actions at once.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
