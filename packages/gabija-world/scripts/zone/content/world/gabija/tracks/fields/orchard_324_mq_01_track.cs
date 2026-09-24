//--- Melia Script ----------------------------------------------------------
// Divine Encounter
//--- Description -----------------------------------------------------------
// Demon Lord Zaura rises in flames to stop the Revelator from reaching
// the captive Goddess Lada.
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

[TrackScript("ORCHARD_324_MQ_01_TRACK")]
public class Orchard324Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_324_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-34.88f, 0.81f, 794.77f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };
		var ward = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Redemption Ward") };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 156043, -46.73, 0.81, 870.85, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Lada") }));
		actors.Add(AddTrackActor(character, 20054, -47.86, 0.81, 869.72, 0, prop));
		actors.Add(AddTrackActor(character, 155024, -46.98, 0.81, 813.68, 0, ward));
		actors.Add(AddTrackActor(character, 155024, -91.38, 0.81, 897.45, 0, ward));
		actors.Add(AddTrackActor(character, 155024, 1.99, 0.81, 897.57, 7, ward));
		actors.Add(AddTrackActor(character, 58087, -46.05, -2.15, 569.57, 71, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Demon Lord Zaura") }));
		actors.Add(AddTrackActor(character, 147469, -48.24, -2.15, 576.01, 0, prop));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		ArmBoss(character, track);
	}

	/// <summary>
	/// Clears the barrier and the fire pillar and turns Zaura loose.
	/// </summary>
	private static void ArmBoss(Character character, Track track)
	{
		if (track.HasBattleBoxInLayer)
			return;

		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 7);

		CreateBattleBoxInLayer(character, track);
		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				character.ServerMessage(L("An unknown power is blocking it."));
				track.Actors[1].AttachEffect("F_ground12_red", 8, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("I_chain004_mash_loop", 4, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_pattern008_violet_loop", 2, EffectLocation.Bottom);
				for (var i = 3; i <= 5; i++)
					track.Actors[i].AttachEffect("I_ground001_yellow_loop", 1, EffectLocation.Bottom);
				break;

			case 20:
				track.Actors[6].AttachEffect("F_explosion072_fire", 25, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_fire011", 20, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_burstup045", 10, EffectLocation.Bottom);
				break;

			case 25:
				// The fire pillar dies on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 7);
				track.Actors[6].AttachEffect("F_bg_fire001_2", 6, EffectLocation.Middle);
				break;

			case 26:
				track.Actors[6].AttachEffect("F_ground071_fire", 3, EffectLocation.Bottom);
				break;

			case 30:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("Halt! You must not be an average fly to make it this far."));
				break;

			case 33:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("If you adore the goddess so much, then allow me send you back to Her!"));
				break;

			case 34:
				ArmBoss(character, track);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Fight against Demon Lord Zaura who imprisoned Goddess Lada!"), 3);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
