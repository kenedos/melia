//--- Melia Script ----------------------------------------------------------
// The Revelation of the Land
//--- Description -----------------------------------------------------------
// The slate in the secret chamber speaks with Laima's own voice.
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

[TrackScript("UNDERFORTRESS_69_MQ060_TRACK")]
public class Underfortress69Mq060Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_69_MQ060_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2350.33f, 753.74f, 42.20f));

		actors.Add(AddTrackActor(character, 47234, -2493.01, 753.74, 45.84, 135, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelation Slate") }));
		actors.Add(AddTrackActor(character, 153062, -2470.16, 753.74, 45.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Fountain") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 59:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track,
					L("Saviors, who followed all the way here by having faith in the revelation."),
					L("You are gradually getting closer to the truth.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		// The slate goes back into the fountain on a Client="BOTH" row, so it
		// is only ever removed from here.
		RemoveTrackActor(character, track, 0);
	}
}
