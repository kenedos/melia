//--- Melia Script ----------------------------------------------------------
// Meeting the Evening Star
//--- Description -----------------------------------------------------------
// Valtross is put down in front of the goddess, and Vakarine speaks to the
// Revelator for the first time.
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

[TrackScript("VPRISON514_MQ_01_TRACK")]
public class Vprison514Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON514_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1068.67f, 589.92f, 1367.97f));

		actors.Add(AddTrackActor(character, 154010, -1019.04, 579.58, 1133.49, 17, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Vakarine") }));
		actors.Add(AddTrackActor(character, 154008, -1030.15, 579.44, 1013.53, 3, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Valtross") }));
		actors.Add(AddTrackActor(character, 154015, -971.60, 579.58, 1234.44, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 154014, -1076.65, 579.58, 1193.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Aldona") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, -1016.50, 579.46, 996.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 154014, -1142.43, 579.41, 989.71, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Aldona") }));
		actors.Add(AddTrackActor(character, 154015, -916.36, 579.52, 1007.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 20025, -1016.50, 579.46, 996.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -972.57, 579.58, 1235.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1073.20, 579.58, 1192.38, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -913.72, 579.52, 1005.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1143.31, 579.41, 990.14, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -1025.62, 579.44, 1009.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 79:
				track.Dialog.SetTitle(L("Goddess Vakarine"));
				track.Dialog.SetPortrait("Dlg_port_vakarine2");
				StartDialog(track,
					L("Aldona, Zydrone..."),
					L("I'm sorry for being a burden to you both because of my feebleness.")
				);
				break;

			case 88:
				track.Dialog.SetTitle(L("Goddess Vakarine"));
				track.Dialog.SetPortrait("Dlg_port_vakarine2");
				StartDialog(track,
					L("Savior."),
					L("As you can see, I cannot do anything myself at the moment."),
					L("As you have heard, after Medzio Diena my powers have become weak...")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		// Every actor the cutscene kills off carries Client="BOTH", so the
		// removals only ever run from here.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 5);
		RemoveTrackActor(character, track, 6);
		RemoveTrackActor(character, track, 7);
	}
}
