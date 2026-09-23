//--- Melia Script ----------------------------------------------------------
// The Solitary Cell Barrier
//--- Description -----------------------------------------------------------
// Zanas gives the Dominance Magic another piece of his soul, and the third
// demon barrier comes down.
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

[TrackScript("PRISON_80_MQ_10_TRACK")]
public class Prison80Mq10Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_80_MQ_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-886.98f, 156.00f, -169.38f));

		actors.Add(AddTrackActor(character, 147469, -863.26, 156.00, -330.14, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 151107, -882.22, 156.00, -231.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
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
					L("This makes three demon barriers down."),
					L("Let's go to the Workshop."),
					L("Nebulas has another one of my spirits there."),
					L("We'll know more if we rescue that spirit."),
					L("Meet me at the Worshop.")
				);
				break;

			case 49:
				character.ServerMessage(L("Disabled the demon barrier with Dominance Magic."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
