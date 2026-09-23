//--- Melia Script ----------------------------------------------------------
// The Workshop Barrier
//--- Description -----------------------------------------------------------
// The two pieces of Zanas meet at the barrier, and the fourth one falls.
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

[TrackScript("PRISON_81_MQ_10_TRACK")]
public class Prison81Mq10Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_81_MQ_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(613.62f, 184.00f, 24.92f));

		actors.Add(AddTrackActor(character, 147469, 584.00, 184.00, -168.00, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 151107, 615.37, 184.00, -53.88, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_pattern008_violet_loop", 8, EffectLocation.Bottom);
				break;

			case 5:
				track.Actors[1].PlayEffect("F_lineup020_blue_mint", 0.6f);
				break;

			case 17:
				track.Actors[1].AttachEffect("F_ground117_loop", 1, EffectLocation.Bottom);
				break;

			case 20:
				track.Actors[0].AttachEffect("F_lineup004", 12, EffectLocation.Bottom);
				break;

			case 39:
				track.Actors[0].PlayEffect("F_light061", 0.7f);
				break;

			case 40:
				track.Actors[0].AttachEffect("F_pattern013_ground_white", 8, EffectLocation.Bottom);
				break;

			case 48:
				track.Dialog.SetTitle(L("Zanas' Soul"));
				track.Dialog.SetPortrait("Dlg_port_zanas_prison");
				StartDialog(track,
					L("There's only one demon barrier left now."),
					L("I still have a bad feeling somehow."),
					L("We should be careful in the Interrogation Room.")
				);
				break;

			case 49:
				character.ServerMessage(L("Disabled the demon barrier with Dominance Magic."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
