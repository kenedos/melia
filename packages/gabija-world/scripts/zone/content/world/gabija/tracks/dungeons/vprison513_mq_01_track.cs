//--- Melia Script ----------------------------------------------------------
// The chase at Nevirau Collapsed Area
//--- Description -----------------------------------------------------------
// Hauberk leaves his servants behind him rather than answer for himself.
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

[TrackScript("VPRISON513_MQ_01_TRACK")]
public class Vprison513Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON513_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2531.81f, 93.49f, -473.47f));

		actors.Add(AddTrackActor(character, 57827, -2516.74, 93.49, -406.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Demon Lord Hauberk") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57716, -2707.89, 116.50, -704.96, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2389.68, 93.49, -719.62, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2725.70, 116.50, -663.09, 14, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2327.35, 93.49, -722.71, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2320.35, 93.49, -663.76, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2533.69, 93.49, -822.27, 87, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57716, -2573.43, 93.49, -831.33, 94, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57719, -2544.53, 93.49, -759.18, 93, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 12:
				track.Dialog.SetTitle(L("Demon Lord Hauberk"));
				StartDialog(track,
					L("So you're here to put me back in your little pocket eh.."),
					L("Don't make me laugh. There is nothing to discuss between us!")
				);
				break;

			case 29:
				// Hauberk walks out of the scene rather than fight it.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Hauberk is gone. Put his servants down."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
