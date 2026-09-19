//--- Melia Script ----------------------------------------------------------
// The Revelation of Kvailas Forest read
//--- Description -----------------------------------------------------------
// Laima's first revelation, and the portal it opens.
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

[TrackScript("HUEVILLAGE_58_4_MQ11_TRACK")]
public class Huevillage584Mq11Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ11_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(32.63232f, 34.203701f, -176.0237f));

		actors.Add(AddTrackActor(character, 147385, 28, 34, -146, 50, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Goddess Saule") }));
		actors.Add(AddTrackActor(character, 47234, 25.8501, 34.203701, -162.76289, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 147469, 28.591801, 34.203701, -189.0907, 600, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 15:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track, L("This is the first revelation I made. For you, it will be the third."));
				break;
			case 17:
				track.Dialog.SetTitle(L("Goddess Saule"));
				track.Dialog.SetPortrait("Dlg_port_Saule");
				StartDialog(track, L("I will stay here with the Believers. I will somehow prevent the Thorn Forest from expanding any more."));
				break;
			case 32:
				track.Actors[2].AttachEffect("E_HUEVILLAGE_58_4_MQ11_potal", 8, EffectLocation.Bottom);
				break;
			case 49:
				character.ServerMessage(L("Use the portal to move to Gate of the Great King."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
