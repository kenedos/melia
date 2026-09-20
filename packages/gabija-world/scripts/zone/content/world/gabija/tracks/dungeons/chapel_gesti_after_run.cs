//--- Melia Script ----------------------------------------------------------
// Gesti flees the cathedral
//--- Description -----------------------------------------------------------
// Wounded and outplayed, Gesti withdraws from the Tenet Church.
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

[TrackScript("CHAPEL_GESTI_AFTER_RUN")]
public class ChapelGestiAfterRun : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPEL_GESTI_AFTER_RUN");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57055, -26.67, 48.71, -138.74, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Demon Queen Gesti") }));
		actors.Add(AddTrackActor(character, 152003, 207.33, 164.86, -582.46, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147352, 133.94, 164.86, -576.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147390, 107.76, 164.86, -579.87, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Follower Algis") }));
		actors.Add(AddTrackActor(character, 147373, 114, 164, -611, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

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
			case 57:
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("Insolent humans."));
				break;
			case 62:
				RemoveTrackActor(character, track, 0);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
