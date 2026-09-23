//--- Melia Script ----------------------------------------------------------
// The Visiting Room Barrier
//--- Description -----------------------------------------------------------
// Zanas feeds a piece of his soul to the Dominance Magic, and the first
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

[TrackScript("PRISON_78_MQ_9_TRACK")]
public class Prison78Mq9Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_78_MQ_9_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-43.37f, 620.48f, 2018.37f));

		actors.Add(AddTrackActor(character, 147469, -166.00, 620.48, 2156.00, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 151107, -160.07, 620.48, 2019.33, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
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
					L("One demon barrier is down."),
					L("Four more to go now."),
					L("Let's head to the Storage now."),
					L("There's another spirit there hiding from the monsters.")
				);
				break;

			case 49:
				character.ServerMessage(L("Disabled the demon barrier with Dominance Magic."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
