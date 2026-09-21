//--- Melia Script ----------------------------------------------------------
// Maven's Verification Test
//--- Description -----------------------------------------------------------
// The gate into the room with the revelation, and the message that guards it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL54_MQ05_PART3_TRACK")]
public class Cathedral54Mq05Part3Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL54_MQ05_PART3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		// The Priest of Evidence and Maven's Message stand on the map; the
		// cutscene's own copies of them are left out.
		actors.Add(AddTrackActor(character, 147350, 1580, 0.19, -1857, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Cathedral Gate") }));
		actors.Add(AddTrackActor(character, 153028, 1584.30, 0.19, -1864.70, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				// The client locks the Priest of Evidence's HP for a scripted
				// bout the server has no equivalent for; the test is read off
				// Maven's Message instead.
				track.Dialog.SetTitle(L("Maven's Message"));
				StartDialog(track,
					L("Are you the Revelator of the goddesses or the one who belongs to the mighty power of the darkness?"),
					L("The evil darkness can not cross here.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
