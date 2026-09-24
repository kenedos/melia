//--- Melia Script ----------------------------------------------------------
// Ferret-Controlling Totem
//--- Description -----------------------------------------------------------
// The Ferret Marauder rises to guard the demon totem on the Banaga
// Forest Trail.
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

[TrackScript("ORCHARD_323_MQ_05_TRACK")]
public class Orchard323Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_323_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(425.84f, 0.87f, -270.63f));

		actors.Add(AddTrackActor(character, 47150, 392.45, 0.87, -251.90, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Demon Totem"), Level = 100 }));
		actors.Add(AddTrackActor(character, 57864, 285.72, 0.87, -200.16, 7, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 95, EndPosition = new Position(339.15f, 0.87f, -223.45f) }));
		actors.Add(AddTrackActor(character, 57854, 399.61, 0.87, -301.29, 6, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57854, 400.04, 0.87, -158.11, 26, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(406.32f, 0.87f, -220.00f) }));
		actors.Add(AddTrackActor(character, 57852, 501.58, 0.87, -348.43, 30, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(449.69f, 0.87f, -299.16f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57853, 369.62, 0.87, -300.43, 7, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57853, 409.74, 0.87, -325.48, 5, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57852, 442.19, 0.87, -231.64, 7, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147469, 289.88, 0.87, -201.48, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		ArmFight(character, track);
	}

	/// <summary>
	/// Clears the summoning circle and turns the Marauder and its ferrets
	/// loose around the totem.
	/// </summary>
	private static void ArmFight(Character character, Track track)
	{
		if (track.HasBattleBoxInLayer)
			return;

		RemoveTrackActor(character, track, 9);
		SetTrackTendency(character, track);
		CreateBattleBoxInLayer(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_levitation005_dark_blue", 1.5f, EffectLocation.Bottom);
				break;

			case 3:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Ferret Marauder, protector of the Demon Totems has appeared!"), 3);
				break;

			case 18:
			case 19:
			case 20:
				track.Actors[9].PlayEffect("I_breath008_circle_3", 2.5f, 1, EffectLocation.Bottom);
				break;

			case 52:
			case 53:
			case 54:
			case 55:
				track.Actors[9].PlayEffect("I_breath008_circle_3", 2.5f, 1, EffectLocation.Top);
				break;

			case 74:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Eliminate Ferret Marauder and destroy the Demon Totem!"), 3);
				break;

			case 75:
				ArmFight(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
