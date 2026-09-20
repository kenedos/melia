//--- Melia Script ----------------------------------------------------------
// The road up to the Royal Mausoleum
//--- Description -----------------------------------------------------------
// Rexipher leaves his servants on the slope and walks on to the entrance.
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

[TrackScript("ROKAS31_REXITHER3_TRACK")]
public class Rokas31Rexither3Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS31_REXITHER3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-876.15f, 235.69f, 307.46f));

		actors.Add(AddTrackActor(character, 47413, -1201.74, 217.36, 618.37, 36, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rexipher"), EndPosition = new Position(-1280.44f, 274.07f, 728.68f) }));
		actors.Add(AddTrackActor(character, 41433, -1165.24, 247.12, 465.33, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1132.39f, 217.36f, 613.97f) }));
		actors.Add(AddTrackActor(character, 41433, -1207.95, 256.87, 413.95, 61, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1212.38f, 217.36f, 585.94f) }));
		actors.Add(AddTrackActor(character, 41433, -1204.35, 235.69, 292.58, 59, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41433, -1185.61, 235.69, 325.90, 50, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1174.46f, 255.31f, 446.77f) }));
		actors.Add(AddTrackActor(character, 41433, -1142.45, 235.69, 312.54, 83, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41435, -1149.76, 217.36, 679.81, 8, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 18:
				track.Dialog.SetTitle(L("Rexipher"));
				track.Dialog.SetPortrait("Dlg_port_LEXIPER");
				StartDialog(track, L("Still following me after all that?"), L("Fine, struggle all you want."));
				break;
			case 39:
				// Rexipher fades out before his servants are handed over.
				RemoveTrackActor(character, track, 0);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
