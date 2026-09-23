//--- Melia Script ----------------------------------------------------------
// What the magic circle does to Eminent
//--- Description -----------------------------------------------------------
// The device and the circle light together, the keeper comes apart in it,
// and Mandara is left standing.
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

[TrackScript("UNDERFORTRESS_69_MQ050_TRACK")]
public class Underfortress69Mq050Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_69_MQ050_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-129.61f, 740.43f, -69.41f));

		actors.Add(AddTrackActor(character, 153059, -107.35, 740.35, -42.18, 16, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Ruklys' Device") }));
		actors.Add(AddTrackActor(character, 153139, -141.96, 739.92, -44.74, 79, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Premier Eminent") }));
		actors.Add(AddTrackActor(character, 57707, -173.53, 741.30, 18.78, 5, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Mandara") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 13:
				track.Dialog.SetTitle(L("Premier Eminent"));
				track.Dialog.SetPortrait("Dlg_port_Premier_Eminent");
				StartDialog(track,
					L("I want you to meet the revelation of the goddess fast."),
					L("To fulfill my lifelong wish...")
				);
				break;

			case 40:
				track.Dialog.SetTitle(L("Premier Eminent"));
				track.Dialog.SetPortrait("Dlg_port_Premier_Eminent");
				StartDialog(track,
					L("Huh? What is this..."),
					L("My strength... My body... It's crumbling?")
				);
				break;

			case 54:
				// The keeper the circle burns away, before the fight is armed.
				RemoveTrackActor(character, track, 1);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Mandara, summoned by Premier Eminent!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
