//--- Melia Script ----------------------------------------------------------
// The Barrier at the Tagika Crossroads
//--- Description -----------------------------------------------------------
// Yane's group is sealed in by an enchantment while Pag Emitters close in
// on Mihail and the player.
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

[TrackScript("CASTLE65_3_MQ03_TRACK")]
public class Castle653Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_3_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-701.09f, 54.14f, 125.20f));

		var device = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155094, -661.97, 46.07, 126.07, 81, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(-731.61f, 0.55f, 367.19f) }));
		actors.Add(AddTrackActor(character, 155095, -736.29, 93.08, -286.22, 22, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane") }));
		actors.Add(AddTrackActor(character, 155096, -634.95, 93.08, -253.72, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Connor") }));
		actors.Add(AddTrackActor(character, 58077, -736.41, 93.48, -348.45, 38, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-733.79f, 93.12f, -325.36f) }));
		actors.Add(AddTrackActor(character, 58077, -587.19, 93.08, -371.31, 29, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-613.11f, 93.08f, -325.88f) }));
		actors.Add(AddTrackActor(character, 58078, -739.98, 94.62, -435.84, 21, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-736.98f, 94.45f, -401.42f) }));
		actors.Add(AddTrackActor(character, 47106, -848.72, 93.08, -291.36, 0, device));
		actors.Add(AddTrackActor(character, 47106, -756.79, 93.08, -199.82, 0, device));
		actors.Add(AddTrackActor(character, 47106, -848.72, 93.08, -421.75, 0, device));
		actors.Add(AddTrackActor(character, 47106, -756.79, 93.08, -513.67, 0, device));
		actors.Add(AddTrackActor(character, 47106, -626.79, 93.08, -513.67, 0, device));
		actors.Add(AddTrackActor(character, 47106, -534.87, 93.08, -421.75, 0, device));
		actors.Add(AddTrackActor(character, 47106, -534.87, 93.08, -291.36, 0, device));
		actors.Add(AddTrackActor(character, 47106, -626.79, 93.08, -199.82, 0, device));
		actors.Add(AddTrackActor(character, 153028, -675.80, 62.89, 21.92, 0, device));
		actors.Add(AddTrackActor(character, 47107, -752.97, 0.55, 387.70, 0, device));
		actors.Add(AddTrackActor(character, 20026, -690.81, 93.32, -358.91, 0, device));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Pag Emitters that come for Mihail at the crossroads.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		var demons = game.Stage("Stage02")
			.Monster(58077, -820.26, 0.55, 473.05)
			.Monster(58077, -722.18, 2.37, 266.25)
			.Monster(58077, -624.28, 8.08, 287.89)
			.Monster(58077, -620.45, 0.56, 383.93)
			.Monster(58077, -677.92, 0.55, 458.60);

		Castle653Minigames.SpawnInTurns(demons, 5, 2, s => s.Game.ClearStage("Stage02"));

		game.Start("Stage02");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 5:
				track.Actors[0].AttachEffect("I_emo_exclamation", 2.5f, EffectLocation.Top);
				track.Actors[1].AttachEffect("I_emo_exclamation", 2.5f, EffectLocation.Top);
				break;

			case 19:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track,
					L("As expected... the enchantment is a lot trouble to deal with."),
					L("I saw a device on my way here... Maybe that's the main core of the enchantment. Let's go back.")
				);
				break;

			case 50:
				// Mihail and Yane's fight leave on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);

				CreateBattleBoxInLayer(character, track);
				StartMinigame(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
