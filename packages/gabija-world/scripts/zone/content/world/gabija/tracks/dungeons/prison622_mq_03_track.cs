//--- Melia Script ----------------------------------------------------------
// Priest Irma's Cell
//--- Description -----------------------------------------------------------
// Marnox waits beside the captive Irma and sends his Fire Lord instead.
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

[TrackScript("PRISON622_MQ_03_TRACK")]
public class Prison622Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON622_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1925.96f, 418.56f, -1141.81f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 156006, -1904.57, 418.56, -1204.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Irma") }));
		actors.Add(AddTrackActor(character, 58071, -1624.90, 423.57, -946.14, 6, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 58000, -1679.40, 423.57, -1020.62, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[1].AttachEffect("F_pattern008_violet_loop", 0.7f, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_smoke019_dark_loop", 1, EffectLocation.Bottom);
				break;

			case 34:
				track.Dialog.SetTitle(L("Demon Lord Marnox"));
				StartDialog(track,
					L("I thought that someone would come and try to rescue their colleague... It seems that I have caught someone completely different."),
					L("Tell me... How are you free of my power within my sphere of influence?"),
					L("Yes... Of course. You are the Revelator that miss Giltine was speaking of."),
					L("Today is my lucky day. Not only have I almost found where the orders are hidden, but I also get to catch a Revelator...")
				);
				break;

			case 60:
				track.PendingDialog = ShowAfter(track, track.PendingDialog, L("There is no need for I, Marnox, to step forward."));
				break;

			case 76:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Defeat Fire Lord!"), 3);
				break;

			case 81:
				// Marnox leaves the cell on a Client="BOTH" row.
				RemoveTrackActor(character, track, 2);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	/// <summary>
	/// Shows a message once the previous part of the conversation was read.
	/// </summary>
	private static async Task ShowAfter(Track track, Task previous, string message)
	{
		if (previous != null)
			await previous;

		await ShowDialog(track, message);
	}
}
