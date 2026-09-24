//--- Melia Script ----------------------------------------------------------
// Delmore Rephaim's Trap at the Outskirts Central Plaza
//--- Description -----------------------------------------------------------
// An enchantment seals the plaza around Mihail and the player until its
// magic core is broken.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CASTLE65_3_MQ02_TRACK")]
public class Castle653Mq02Track : TrackScript
{
	private const long QuestId = 70441;

	protected override void Load()
	{
		SetId("CASTLE65_3_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1237.23f, 164.07f, 1026.93f));

		var device = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };
		var wall = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155094, -1285.78, 164.37, 1027.01, 71, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(-849.90f, 166.69f, 1171.55f) }));
		actors.Add(AddTrackActor(character, 47106, -1035.45, 166.69, 1239.05, 0, device));
		actors.Add(AddTrackActor(character, 47106, -894.03, 166.69, 1380.47, 0, device));
		actors.Add(AddTrackActor(character, 47106, -1035.45, 166.69, 1039.05, 0, device));
		actors.Add(AddTrackActor(character, 47106, -894.03, 166.69, 897.63, 0, device));
		actors.Add(AddTrackActor(character, 47106, -694.03, 166.69, 897.63, 0, device));
		actors.Add(AddTrackActor(character, 47106, -552.61, 166.69, 1039.05, 0, device));
		actors.Add(AddTrackActor(character, 47106, -552.61, 166.69, 1239.05, 0, device));
		actors.Add(AddTrackActor(character, 47106, -694.03, 166.69, 1380.47, 0, device));
		actors.Add(AddTrackActor(character, 20049, -964.74, 166.69, 1309.76, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -1035.45, 166.69, 1139.05, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -964.74, 166.69, 968.34, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -794.03, 166.69, 897.63, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -623.32, 166.69, 968.34, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -552.61, 166.69, 1139.05, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -623.32, 166.69, 1309.76, 0, wall));
		actors.Add(AddTrackActor(character, 20049, -794.03, 166.69, 1380.47, 0, wall));
		actors.Add(AddTrackActor(character, 20026, -792.41, 166.69, 1139.85, 0, device));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the demons guarding the enchantment and, once they fall, its
	/// magic core.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		var demons = game.Stage("Stage01")
			.Monster(58077, -933.12, 166.69, 1137.99)
			.Monster(58077, -802.46, 166.69, 1008.70)
			.Monster(58077, -796.51, 166.69, 1270.89)
			.Monster(58077, -663.03, 166.69, 1144.68)
			.Monster(58076, -794.00, 166.00, 1139.00);

		Castle653Minigames.SpawnInTurns(demons, 5, 4, s =>
		{
			s.Game.ClearStage("Stage01");
			s.Game.StartStage("Stage02");
			character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The magic core of the enchantment has appeared!"), 3);
		});

		game.Stage("Stage02")
			.Monster(155097, -794.00, 166.00, 1139.00, 315, aggressive: false)
			.On(s => s.Elapsed >= 1 && s.Alive() == 0, s =>
			{
				s.Game.CompleteObjective(QuestId, "breakTrap");
				s.Game.ClearStage("Stage02");
			}, 1);

		game.Start("Stage01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 23:
				track.Actors[2].AttachEffect("F_rize002", 1f, EffectLocation.Bottom);
				break;

			case 44:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track,
					L("To have an enchantment prepared in such a short time is definitely extraordinary. So, enchantment and demons... I think I got it."),
					L("The core of it is magic. To keep an enchantment this strong it takes a lot of magical power."),
					L("The demons' stamina may be lower, but the magic core could still make an appearance. Let's attack that or just defeat all the demons!")
				);
				break;

			case 49:
				// Mihail leaves on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 1);

				CreateBattleBoxInLayer(character, track);
				StartMinigame(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}

/// <summary>
/// Minigame helpers shared by the Delmore Outskirts tracks.
/// </summary>
public static class Castle653Minigames
{
	/// <summary>
	/// Keeps one monster standing at each of the stage's spawn points
	/// until every point has sent its share, then runs the given action
	/// once the last of them falls.
	/// </summary>
	public static void SpawnInTurns(MinigameStage stage, int pointCount, int perPoint, Action<MinigameStage> onCleared)
	{
		var spawned = Enumerable.Repeat(1, pointCount).ToArray();

		stage.On(s => true, s =>
		{
			for (var i = 0; i < pointCount; ++i)
			{
				if (s.Alive(i) == 0 && spawned[i] < perPoint)
				{
					s.Spawn(i);
					spawned[i]++;
				}
			}
		});

		stage.On(s => spawned.All(n => n >= perPoint) && s.Alive() == 0, onCleared, 1);
	}
}
