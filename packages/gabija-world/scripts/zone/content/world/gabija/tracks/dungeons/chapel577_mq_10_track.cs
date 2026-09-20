//--- Melia Script ----------------------------------------------------------
// The hidden sanctuary
//--- Description -----------------------------------------------------------
// The Seal of Space opens the wall, and the revelation is inside.
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

[TrackScript("CHAPLE577_MQ_10_TRACK")]
public class Chaple577Mq10Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE577_MQ_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(831.48f, 35.92f, -1258.44f));

		actors.Add(AddTrackActor(character, 47234, 949.82, 35.92, -1243.26, 15, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelation of the Goddess") }));
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
			case 0:
				track.Dialog.SetTitle(L("Writings on the Wall"));
				StartDialog(track, L("If you are truly the Revelator, then you must be able to attain the divine secrets at the end of this revelation."));
				break;
			case 37:
				RemoveTrackActor(character, track, 0);
				break;
			case 43:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track, L("400 years would have passed by the time this revelation reaches you."),
					L("Thank you in advance for your continued pursuit of finding this revelation."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
