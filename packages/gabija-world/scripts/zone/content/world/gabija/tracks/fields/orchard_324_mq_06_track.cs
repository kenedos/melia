//--- Melia Script ----------------------------------------------------------
// The Thing That Should Not Let It Be
//--- Description -----------------------------------------------------------
// The Beholder steals the Incomplete Kruvina and leaves Zaura empowered by
// Giltine to finish off the Revelator.
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

[TrackScript("ORCHARD_324_MQ_06_TRACK")]
public class Orchard324Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_324_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1458.94f, 612.30f, 903.61f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155104, -1704.19, 612.30, 887.56, 0, prop));
		actors.Add(AddTrackActor(character, 147382, -1739.06, 612.30, 887.17, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Beholder") }));
		actors.Add(AddTrackActor(character, 58087, -1812.61, 612.30, 887.64, 2, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Zaura") }));
		actors.Add(AddTrackActor(character, 20025, -1812.61, 612.30, 887.64, 0, prop));
		actors.Add(AddTrackActor(character, 58203, -1812.61, 612.30, 887.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153118, -1704.19, 612.30, 887.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Incomplete Kruvina") }));
		actors.Add(AddTrackActor(character, 20024, -1739.06, 612.30, 887.17, 0, prop));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		ArmBoss(character, track);
	}

	/// <summary>
	/// Clears everyone the cutscene sends away and turns the empowered
	/// Zaura loose.
	/// </summary>
	private static void ArmBoss(Character character, Track track)
	{
		if (track.Actors[3].Map == null)
			return;

		// The Beholder, the old Zaura and the Kruvina leave on Client="BOTH" rows the client never reports.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 6);
		RemoveTrackActor(character, track, 7);

		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[6].AttachEffect("F_light096_red_loop", 1, EffectLocation.Bottom);
				break;

			case 1:
				CreateBattleBoxInLayer(character, track);
				break;

			case 8:
				track.Actors[3].AttachEffect("F_burstup045", 10, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_explosion072_fire", 10, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_fire011", 10, EffectLocation.Bottom);
				break;

			case 13:
				track.Actors[3].AttachEffect("F_bg_fire001_2", 1, EffectLocation.Middle);
				break;

			case 15:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track,
					L("Yes... It'll be destroyed by your hands..."),
					L("I will crush everything with Kruvina's power!!")
				);
				break;

			case 19:
				track.Actors[2].PlayEffect("F_explosion078_dark", 1f, 1, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_light082_line_red", 3, EffectLocation.Middle);
				track.Actors[3].AttachEffect("F_levitation032_red_loop", 10, EffectLocation.Bottom);
				break;

			case 20:
				track.Actors[2].AttachEffect("F_levitation005_dark_red", 1.8f, EffectLocation.Middle);
				break;

			case 25:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("What are you doing... Beholder... This is none of your concern!!"));
				break;

			case 29:
				track.Dialog.SetTitle(L("Beholder"));
				track.Dialog.SetPortrait("Dlg_port_blackman");
				StartDialog(track,
					L("You stupid... You forgot the mission from Giltine?"),
					L("The power is not something you dare to handle. I'll take care of Kruvina."),
					L("I'll also report directly to Giltine. And the Revelator... certainly needs to be eliminated."),
					L("Zaura. The sin for forgetting my mission will be repaid with that Revelator's head."),
					L("Here. This is the power that miss Giltine has endowed you. This power is sufficient even without Kruvina's power.")
				);
				break;

			case 34:
				track.Actors[4].AttachEffect("F_smoke017_red", 3.5f, EffectLocation.Bottom);
				break;

			case 39:
				track.Actors[2].AttachEffect("F_smoke017_red", 1.5f, EffectLocation.Bottom);
				break;

			case 40:
				track.Actors[7].AttachEffect("F_smoke017_red_1", 2, EffectLocation.Bottom);
				break;

			case 44:
				track.Actors[5].AttachEffect("F_burstup002_dark", 5, EffectLocation.Bottom);
				break;

			case 48:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The Beholder disappeared after taking the Incomplete Kruvina! Defeat Zaura who received powers from Giltine!"), 3);
				break;

			case 49:
				ArmBoss(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
