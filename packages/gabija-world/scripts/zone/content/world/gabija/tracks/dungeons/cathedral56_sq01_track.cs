//--- Melia Script ----------------------------------------------------------
// Naktis at the Pasala Altar door
//--- Description -----------------------------------------------------------
// The Demon Lord of Curses has been waiting behind the door the whole time.
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

[TrackScript("CHATHEDRAL56_SQ01_TRACK")]
public class Cathedral56Sq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL56_SQ01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1536.97f, 0.50f, 336.37f));

		actors.Add(AddTrackActor(character, 41351, -1535.55, 0.50, 64.64, 264, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1520.31f, 0.50f, 186.20f) }));
		actors.Add(AddTrackActor(character, 153012, -1527.70, 0.49, 469.50, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, -1704.89, 0.50, 111.51, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1406.62, 0.50, 4.62, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1407.84, 0.50, 67.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1524.57, 0.50, 119.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-1548.65f, 0.50f, 19.04f) }));
		actors.Add(AddTrackActor(character, 20025, -1552.68, 0.50, -148.95, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				CreateBattleBoxInLayer(character, track);
				break;

			case 40:
				track.Dialog.SetTitle(L("Demon Lord Naktis"));
				track.Dialog.SetPortrait("Dlg_port_naktis");
				StartDialog(track,
					L("You have been running around hard in my land."),
					L("Sorry kid, but my name calls for the curse.")
				);
				break;

			case 49:
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Naktis!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
