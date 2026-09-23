//--- Melia Script ----------------------------------------------------------
// Austeja at the Ranka Seal
//--- Description -----------------------------------------------------------
// The second tower holds, and the goddess says where she is going next.
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

[TrackScript("SIAULIAI_46_1_MQ_05_TRACK")]
public class Siauliai461Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_1_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(346.01f, 211.17f, -865.87f));

		actors.Add(AddTrackActor(character, 151041, 321.87, 211.17, -881.37, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Austeja") }));
		actors.Add(AddTrackActor(character, 147501, 311, 212, -871, 1, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Ranka Seal Tower") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				track.Dialog.SetTitle(L("Goddess Austeja"));
				track.Dialog.SetPortrait("Dlg_port_Austeja2");
				StartDialog(track,
					L("I thank you again in the name of all fate."),
					L("From here on, I will follow the stars to find Goddess Ausrine.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		// The goddess leaves on a Client="BOTH" row, so she is only ever
		// removed from here.
		RemoveTrackActor(character, track, 0);
	}
}
