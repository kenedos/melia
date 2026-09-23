//--- Melia Script ----------------------------------------------------------
// The Revelation of the Mage Tower
//--- Description -----------------------------------------------------------
// Gabija takes the Jewel of Prominence and passes on Laima's message.
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

[TrackScript("FTOWER45_MQ_05_GABIA_END_TRACK")]
public class Ftower45Mq05GabiaEndTrack : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER45_MQ_05_GABIA_END_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(806.57f, 254.17f, 2226.49f));

		actors.Add(AddTrackActor(character, 147452, 837, 254.17, 2330, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Gabija") }));
		actors.Add(AddTrackActor(character, 151002, 834.62, 254.17, 2303.82, 13, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Jewel of Prominence"), EndPosition = new Position(831.76f, 254.17f, 2328.63f) }));
		actors.Add(AddTrackActor(character, 47234, 834.62, 254.17, 2321.68, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelation Slate") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20024, 832.87, 254.17, 2305.56, 2, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147449, 248.05, 341.90, 166.27, 86, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita"), EndPosition = new Position(315.52f, 301.76f, 148.19f) }));
		actors.Add(AddTrackActor(character, 151053, 319.78, 301.76, 181.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita") }));
		actors.Add(AddTrackActor(character, 47395, 238.30, 356.75, 129.09, 41, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(291.46f, 320.38f, 153.21f) }));
		actors.Add(AddTrackActor(character, 47399, 221.58, 366.38, 130.24, 68, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(296.77f, 314.37f, 163.42f) }));
		actors.Add(AddTrackActor(character, 47399, 233.92, 355.81, 141.69, 33, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(275.74f, 327.49f, 160.83f) }));
		actors.Add(AddTrackActor(character, 151053, 860.45, 254.17, 2355.36, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 35:
				track.Dialog.SetTitle(L("Goddess Gabija"));
				track.Dialog.SetPortrait("Dlg_port_gabija");
				StartDialog(track, L("Savior... Laima told us that she had dreamt about the collapse of the world. And among many Revelators, one Savior would save everyone."));
				break;
			case 52:
				RemoveTrackActor(character, track, 5);
				break;
			case 63:
				RemoveTrackActor(character, track, 7);
				break;
			case 66:
				RemoveTrackActor(character, track, 8);
				break;
			case 78:
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 9);
				break;
			case 101:
				RemoveTrackActor(character, track, 1);
				break;
			case 119:
				track.Dialog.SetTitle(L("Goddess Gabija"));
				track.Dialog.SetPortrait("Dlg_port_gabija");
				StartDialog(track, L("This too is my ordeal and duty which Laima foresaw... I will tell you Laima's message."));
				break;
			case 132:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track, L("I hope this revelation will reach you under the blessings of fire... I'll begin by trying to foresee the disasters that will occur in your time."));
				break;
			case 139:
				RemoveTrackActor(character, track, 4);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
