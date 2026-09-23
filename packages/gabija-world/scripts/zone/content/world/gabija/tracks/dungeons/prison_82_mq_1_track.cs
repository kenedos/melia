//--- Melia Script ----------------------------------------------------------
// Nebulas at the Interrogation Room
//--- Description -----------------------------------------------------------
// Nebulas comes up the hall with a power far past his own, and Zanas
// pulls the Revelator out on the Teleport Magic Scroll.
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

[TrackScript("PRISON_82_MQ_1_TRACK")]
public class Prison82Mq1Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_82_MQ_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1142.41f, 559.56f, -95.19f));

		actors.Add(AddTrackActor(character, 151107, -1152.00, 559.56, -70.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 58412, -1632.19, 494.37, -150.22, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, EndPosition = new Position(-1262.18f, 526.74f, -108.44f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 12:
				track.Actors[2].AttachEffect("F_spread_out003_darkblue", 1.6f, EffectLocation.Middle);
				break;

			case 35:
				track.Actors[2].PlayEffect("F_ground086_violet", 2f);
				break;

			case 45:
				track.Dialog.SetTitle(L("Zanas' Soul"));
				track.Dialog.SetPortrait("Dlg_port_zanas_prison");
				StartDialog(track,
					L("I... I feel a very intese power in Nebulas."),
					L("This is dangerous."),
					L("We should get out of here."),
					L("I'll use the Teleport Magic Scroll!")
				);
				break;

			case 50:
				track.Actors[0].PlayEffect("F_pc_warp_light", 0.5f, 1, EffectLocation.Top);
				track.Actors[0].PlayEffect("F_pc_warp_circle", 0.5f);
				character.PlayEffect("F_pc_warp_circle", 0.5f);
				character.PlayEffect("F_pc_warp_light", 0.5f, 1, EffectLocation.Top);
				break;

			case 64:
				// Zanas and Nebulas both leave on Client="BOTH" rows at frames 53 and 60.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 2);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 0);
		RemoveTrackActor(character, track, 2);
	}
}
