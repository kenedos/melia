//--- Melia Script ----------------------------------------------------------
// The Missing Girl (1)
//--- Description -----------------------------------------------------------
// Demon Lord Zaura catches up with the girl at the Broken Bridge and
// sets the ferrets on the Revelator.
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

[TrackScript("ORCHARD_342_MQ_03_TRACK")]
public class Orchard342Mq03Track : TrackScript
{
	private const int Ferret = 57850;
	private const int FerretLoader = 57851;

	protected override void Load()
	{
		SetId("ORCHARD_342_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-412.74f, -69.99f, -723.75f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 47236, -455.07, -69.99, -777.11, 87, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Girl"), EndPosition = new Position(-624.03f, -69.99f, -821.08f) }));
		actors.Add(AddTrackActor(character, Ferret, -667.60, -69.99, -810.42, 4, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, Ferret, -630.70, -69.99, -858.98, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, FerretLoader, -582.78, -69.99, -778.26, 155, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-602.99f, -69.99f, -795.62f) }));
		actors.Add(AddTrackActor(character, 58087, -588.00, -69.99, -785.71, 2, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Zaura"), EndPosition = new Position(-610.59f, -69.99f, -810.00f) }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		ArmFight(character, track);
	}

	/// <summary>
	/// Sends the girl and Zaura away and lets the ferret horde loose.
	/// </summary>
	private static void ArmFight(Character character, Track track)
	{
		if (track.HasBattleBoxInLayer)
			return;

		// Zaura and the girl fade out, and the loader dies, on rows the client never reports.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 5);

		SetTrackTendency(character, track);
		CreateBattleBoxInLayer(character, track);
		StartMinigame(character, track);
	}

	/// <summary>
	/// Starts the ferrets that keep pouring in until the Revelator has
	/// fought them off.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(Ferret, -795.45, -69.99, -671.81, 0, 1, 10)
			.Monster(Ferret, -478.88, -69.99, -928.81, 0, 1, 10)
			.Monster(Ferret, -766.99, -69.99, -745.61, 0, 1, 10)
			.Monster(Ferret, -548.61, -69.99, -938.26, 0, 1, 10)
			.Monster(Ferret, -642.66, -69.99, -812.46, 0, 1, 10)
			.Monster(FerretLoader, -554.46, -69.99, -720.97, 0, 1, 10)
			.Monster(Ferret, -610.70, -69.99, -594.54, 0, 1, 10)
			.Monster(Ferret, -371.60, -69.99, -752.93, 0, 1, 10)
			.Monster(Ferret, -586.38, -69.99, -679.18, 0, 1, 10)
			.Monster(Ferret, -741.11, -69.99, -713.00, 0, 1, 10)
			.Monster(Ferret, -521.93, -69.99, -905.52, 0, 1, 20)
			.Monster(Ferret, -719.45, -69.99, -593.37, 0, 1, 10)
			.Monster(Ferret, -362.82, -69.99, -796.63, 0, 1, 10)
			.Monster(FerretLoader, -707.41, -69.99, -810.87, 0, 1, 25)
			.Monster(FerretLoader, -641.17, -69.99, -863.07, 0, 1, 20)
			.Monster(Ferret, -644.26, -69.99, -629.63, 0, 1, 25)
			.Monster(Ferret, -488.49, -69.99, -730.09, 0, 1, 25)
			.Monster(Ferret, -420.19, -69.99, -801.72, 0, 1, 25);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				track.Actors[1].AttachEffect("F_light058_blue", 4, EffectLocation.Middle);
				break;

			case 35:
				track.Actors[5].AttachEffect("F_levitation032_red", 10, EffectLocation.Bottom);
				break;

			case 36:
				track.Actors[1].AttachEffect("F_levitation032_red_loop", 2, EffectLocation.Bottom);
				break;

			case 43:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("Finally found you! The ferrets do have their uses."));
				break;

			case 57:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("There seems to be a fly hidden among us... Find and eliminate them."));
				break;

			case 67:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Demon Lord Zaura disappeared with the girl! Eliminate the horde of ferrets!"), 3);
				break;

			case 68:
				ArmFight(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
