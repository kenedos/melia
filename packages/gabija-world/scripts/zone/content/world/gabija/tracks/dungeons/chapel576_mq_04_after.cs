//--- Melia Script ----------------------------------------------------------
// Algis at the opened gate
//--- Description -----------------------------------------------------------
// The gate falls, and Algis goes in after Gesti.
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

[TrackScript("CHAPLE576_MQ_04_AFTER")]
public class Chaple576Mq04After : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE576_MQ_04_AFTER");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1648.28f, 0.42f, 419.05f));

		actors.Add(AddTrackActor(character, 40069, -1744.69, 0.42, 426.14, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147390, -1880.05, -42.13, 395.60, 0, new TrackActorSpec
		{
			Name = L("Follower Algis"),
			Ai = "TrackWaitMonster",
			WalkSpeed = 60,
			EndPosition = new Position(-1506.46f, 0.42f, 259.16f),
		}));
		actors.Add(AddTrackActor(character, 20026, -1708.23, 0.42, 404.94, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 147379, -1777.82, 0.42, 425.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147399, -1911.56, -42.13, 390.54, 0, new TrackActorSpec
		{
			Name = L("Follower Donatas"),
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-1674f, 1f, 374f),
		}));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				RemoveTrackActor(character, track, 0);
				break;
			case 24:
				track.Dialog.SetTitle(L("Follower Algis"));
				track.Dialog.SetPortrait("Dlg_port_algis");
				StartDialog(track, L("If she's not on the first floor, then she must be on the second."),
					L("I hope we're not too late."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
