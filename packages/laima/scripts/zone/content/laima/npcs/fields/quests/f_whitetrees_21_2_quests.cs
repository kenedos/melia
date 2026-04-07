//--- Melia Script ----------------------------------------------------------
// Nobreer Forest - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_21_2 map.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FWhitetrees212QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Pilgrim's Rest
		// =====================================================================
		// Pilgrim Saulene - Path to Orsha
		//---------------------------------------------------------------------
		AddNpc(20168, L("[Pilgrim] Saulene"), "f_whitetrees_21_2", -960, 460, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_21_2", 1001);

			dialog.SetTitle(L("Saulene"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A woman sits cross-legged on the forest path, her eyes closed in quiet meditation*{/}"));
				await dialog.Msg(L("..."));
				await dialog.Msg(L("{#666666}*She opens one eye*{/}"));
				await dialog.Msg(L("Ah. A traveler. Forgive my silence - I was listening to the forest breathe."));

				var response = await dialog.Select(L("I've walked from Orsha to find stillness in these white-leaf woods. The trees here hold an ancient calm... if you know where to look."),
					Option(L("I'd like to learn"), "help"),
					Option(L("What brings you here?"), "info"),
					Option(L("I'll leave you to it"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*A gentle smile crosses her face*{/}"));
						await dialog.Msg(L("Then you feel it too - that quiet pull beneath the rustling leaves."));

						if (await dialog.YesNo(L("There are four ancient stones in these western woods, places where the forest's energy gathers. Sit at each one and simply... be still. Will you try?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Find the stones among the white-leaf trees. Sit, close your eyes, and let the forest speak to you."));
							await dialog.Msg(L("When you've visited all four, return to me. I think you'll understand what I mean."));
						}
						break;

					case "info":
						await dialog.Msg(L("After the demon war, everything became noise - soldiers marching, hammers rebuilding, people shouting orders."));
						await dialog.Msg(L("I needed quiet. These woods still remember what peace sounds like."));
						await dialog.Msg(L("The Kugheri leave me alone, mostly. I think they sense I mean no harm."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*She closes her eyes again*{/}"));
						await dialog.Msg(L("Safe travels. The forest watches over those who walk gently."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited", 0);

				if (stonesVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*She looks at you with knowing eyes*{/}"));
					await dialog.Msg(L("You're different now. Quieter inside. I can tell."));
					await dialog.Msg(L("The forest spoke to you, didn't it? Not in words - in something deeper."));
					await dialog.Msg(L("{#666666}*She presses her palms together*{/}"));
					await dialog.Msg(L("Carry that stillness with you, wherever the road leads. It will serve you better than any sword."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The stones are waiting. Find them among the white-leaf trees and sit for a moment."));
					await dialog.Msg(L("Don't rush. The whole point is to slow down."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*She sits peacefully, eyes half-closed*{/}"));
				await dialog.Msg(L("The forest remembers you. Welcome back."));
			}
		});

		// =====================================================================
		// MEDITATION STONES
		// =====================================================================
		// For Quest 1001 - The Pilgrim's Rest
		// =====================================================================

		void AddMeditationStone(int stoneNumber, int x, int z, int direction)
		{
			AddNpc(150232, L("Ancient Stone"), "f_whitetrees_21_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_21_2", 1001);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered stone sits quietly among the white-leaf trees, covered in pale moss*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_21_2.Quest1001.Stone{stoneNumber}";
				var visited = character.Variables.Perm.GetBool(variableKey, false);

				if (visited)
				{
					await dialog.Msg(L("{#666666}*You've already meditated here. The stone's warmth lingers*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Meditating..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var stonesVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited", stonesVisited + 1);

					character.ServerMessage(LF("Meditation stones visited: {0}/4", stonesVisited + 1));

					if (stonesVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All stones visited! Return to Pilgrim Saulene.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Meditation interrupted."));
				}
			});
		}

		AddMeditationStone(1, -907, 335, 0);
		AddMeditationStone(2, -1276, 476, 45);
		AddMeditationStone(3, -1113, 142, 90);
		AddMeditationStone(4, -1364, 314, 0);

		// =====================================================================
		// QUEST 1002: Bells of the Wild
		// =====================================================================
		// Sage Virinne - Crossroads near hollow logs
		//---------------------------------------------------------------------
		AddNpc(150183, L("[Sage] Virinne"), "f_whitetrees_21_2", -367, 156, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_21_2", 1002);

			dialog.SetTitle(L("Virinne"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A woman kneels beside a hollow log, sketching something in a leather journal*{/}"));
				await dialog.Msg(L("Fascinating... absolutely fascinating. The resonance patterns are unlike anything I've documented before."));

				var response = await dialog.Select(L("Oh! I didn't hear you approach. I'm studying the Kugheri - have you noticed the bells they wear? They're not decorative. I believe they serve a deeper purpose."),
					Option(L("What kind of purpose?"), "info"),
					Option(L("Need help with your research?"), "help"),
					Option(L("Sounds like your problem"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*Her eyes light up*{/}"));
						await dialog.Msg(L("You'd help? Wonderful! I need physical samples to confirm my theory."));

						if (await dialog.YesNo(L("The Kugheri Balzer - the shaman-like ones - carry the most intricate bells. I need you to collect five of them. You'll likely need to fight them for it. Can you do this?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The Kugheri Balzer roam throughout this area. They're the ones with the elaborate adornments."));
							await dialog.Msg(L("Bring me five bells and I can complete my analysis. Be careful - they practice a form of wild magic."));
						}
						break;

					case "info":
						await dialog.Msg(L("{#666666}*She flips through her journal excitedly*{/}"));
						await dialog.Msg(L("The bells produce specific tones when the Kugheri move. I think it's a form of communication - or perhaps even a kind of prayer."));
						await dialog.Msg(L("These creatures aren't mindless beasts. They have rituals, hierarchy, spiritual practices. The bells are central to all of it."));
						await dialog.Msg(L("But I can't prove any of this without physical samples to study."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*She waves absently, already back to her sketching*{/}"));
						await dialog.Msg(L("Mm. Mind the Balzer if you pass through. Their magic stings."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBalzer", out var killObj)) return;
				if (!quest.TryGetProgress("collectBells", out var collectObj)) return;

				if (killObj.Done && collectObj.Done)
				{
					await dialog.Msg(L("{#666666}*She takes the bells with trembling hands*{/}"));
					await dialog.Msg(L("Five specimens... and in such fine condition!"));
					await dialog.Msg(L("{#666666}*She holds one up to her ear and shakes it gently*{/}"));
					await dialog.Msg(L("Listen to that tone. Each bell is tuned differently - like notes in a melody. The Kugheri are making music."));
					await dialog.Msg(L("Thank you. This will keep me busy for months. Take these supplies - you've earned far more than I can offer."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hunting? The Kugheri Balzer are the shaman-like ones with the elaborate adornments."));
					await dialog.Msg(L("They tend to gather around the crossroads and the central path through the forest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*She's surrounded by bells and scattered notes*{/}"));
				await dialog.Msg(L("I've confirmed it - each bell corresponds to a different harmonic frequency. The Kugheri are far more sophisticated than anyone suspected."));
			}
		});

		// =====================================================================
		// QUEST 1003: Waterfall Offering
		// =====================================================================
		// Hermit Dovydas - Near the waterfall
		//---------------------------------------------------------------------
		AddNpc(147512, L("[Hermit] Dovydas"), "f_whitetrees_21_2", 99, 1680, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_21_2", 1003);

			dialog.SetTitle(L("Dovydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An old monk sits near the waterfall, his robes damp with mist. A small shrine of white stones stands beside him*{/}"));
				await dialog.Msg(L("The water speaks today. It says someone new has come."));

				var response = await dialog.Select(L("I tend the shrines in this part of the forest. Every day, I place fresh white flowers at each one. The offering keeps the forest's spirit at ease."),
					Option(L("Can I help with the offering?"), "help"),
					Option(L("Why white flowers?"), "info"),
					Option(L("I'm just passing through"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He chuckles softly*{/}"));
						await dialog.Msg(L("My old legs aren't what they used to be. I'd welcome the help."));

						if (await dialog.YesNo(L("There are five flower patches near the waterfall. Gather one white flower from each and bring them back. The flowers know when they're being picked with good intent - they'll come easily.")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Look for the white blooms growing near the water. They favor the mist."));
							await dialog.Msg(L("Gather gently. The flowers are a gift, not something to be taken by force."));
						}
						break;

					case "info":
						await dialog.Msg(L("{#666666}*He gazes at the waterfall*{/}"));
						await dialog.Msg(L("White is the color of this forest - the leaves, the mist, the moonlight on the water."));
						await dialog.Msg(L("The flowers are part of the forest's breath. When we place them at the shrines, we return what was given."));
						await dialog.Msg(L("Without the offering, the balance shifts. The Kugheri grow restless. The water turns cold. The trees stop singing."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He nods slowly*{/}"));
						await dialog.Msg(L("Walk softly. The forest listens."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var flowerCount = character.Inventory.CountItem(667232);

				if (flowerCount >= 5)
				{
					await dialog.Msg(L("{#666666}*His eyes brighten as you present the flowers*{/}"));
					await dialog.Msg(L("Five perfect blooms. You gathered them with care - I can tell."));
					await dialog.Msg(L("{#666666}*He arranges the flowers carefully at the shrine*{/}"));
					await dialog.Msg(L("There. The offering is complete. Can you feel it? The forest is pleased."));
					await dialog.Msg(L("{#666666}*The waterfall seems to shimmer just a little brighter*{/}"));
					await dialog.Msg(L("You have a gentle spirit. Take these - they're small thanks for keeping an old man's tradition alive."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The white flowers grow near the water, where the mist gathers."));
					await dialog.Msg(L("Take your time. The flowers won't wilt while you search."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("{#666666}*He sits by the shrine, eyes closed, listening to the waterfall*{/}"));
				await dialog.Msg(L("The forest remembers your kindness. It always will."));
			}
		});

		// =====================================================================
		// WHITE FLOWER COLLECTION SPOTS
		// =====================================================================
		// For Quest 1003 - Waterfall Offering
		// =====================================================================

		void AddWhiteFlowerSpot(int spotNumber, int x, int z)
		{
			AddNpc(47246, L("White Flowers"), "f_whitetrees_21_2", x, z, 0, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_21_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Delicate white flowers sway gently in the waterfall's mist*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_21_2.Quest1003.Flower{spotNumber}";
				var collected = character.Variables.Perm.GetBool(variableKey, false);

				if (collected)
				{
					await dialog.Msg(L("{#666666}*You've already gathered flowers from this patch*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Gathering flowers..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(667232, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(667232);
					character.ServerMessage(LF("White flowers gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All flowers gathered! Return to Hermit Dovydas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering cancelled."));
				}
			});
		}

		AddWhiteFlowerSpot(1, 86, 1710);
		AddWhiteFlowerSpot(2, 253, 1633);
		AddWhiteFlowerSpot(3, 322, 1616);
		AddWhiteFlowerSpot(4, 438, 1681);
		AddWhiteFlowerSpot(5, 216, 1528);

		// =====================================================================
		// QUEST 1004: The Unsettled Grove
		// =====================================================================
		// Hunter Kazys - Near the trees
		//---------------------------------------------------------------------
		AddNpc(47245, L("[Hunter] Kazys"), "f_whitetrees_21_2", -64, -631, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_21_2", 1004);

			dialog.SetTitle(L("Kazys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A hunter crouches near a tree, examining deep claw marks in the bark*{/}"));
				await dialog.Msg(L("These marks are fresh. The Kugheri Zeffi have been ranging further than usual."));

				var response = await dialog.Select(L("I patrol this forest for the Orsha garrison. Something's agitated the Zeffi - they're pushing into areas they normally avoid and threatening the path between here and Emmet Forest."),
					Option(L("I can help thin their numbers"), "help"),
					Option(L("What's causing it?"), "info"),
					Option(L("Not my concern"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He stands and checks his weapon*{/}"));
						await dialog.Msg(L("Good. I could use another pair of hands."));

						if (await dialog.YesNo(L("The Zeffi have grown bold. If we don't push them back, they'll start threatening travelers on the main road. Help me cull fifteen of them to restore the balance?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The Zeffi roam the groves further south of here. You'll know them by their icy temperament - literally."));
							await dialog.Msg(L("Watch yourself. They're tougher than the other Kugheri."));
						}
						break;

					case "info":
						await dialog.Msg(L("{#666666}*He shakes his head*{/}"));
						await dialog.Msg(L("Hard to say. Could be territorial pressure from the other Kugheri types. Could be something deeper - the forest's balance shifting."));
						await dialog.Msg(L("The hermit by the waterfall says the forest's spirit is unsettled. I'm not one for mysticism, but I've seen enough strange things in these woods to keep an open mind."));
						await dialog.Msg(L("Whatever the cause, the result is the same - aggressive Zeffi where they shouldn't be."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He shrugs*{/}"));
						await dialog.Msg(L("Keep your eyes open if you're heading south. The Zeffi don't discriminate."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killZeffi", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He inspects the surrounding trees, nodding slowly*{/}"));
					await dialog.Msg(L("I can already feel the difference. The forest is calmer. The Zeffi are retreating to their normal territory."));
					await dialog.Msg(L("{#666666}*He reaches into his pack*{/}"));
					await dialog.Msg(L("You've earned this. It's a solid weapon - served me well before I switched to the bow. Take it."));
					await dialog.Msg(L("The forest thanks you. And so does every traveler who won't get ambushed on the road."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hunting? The Zeffi favor the groves to the south. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Zeffi have settled down. The patrols are much easier now, thanks to you."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Pilgrim's Rest
//-----------------------------------------------------------------------------

public class ThePilgrimsRestQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_21_2", 1001);
		SetName("The Pilgrim's Rest");
		SetType(QuestType.Sub);
		SetDescription("Pilgrim Saulene has asked you to meditate at four ancient stones in the western woods of Nobreer Forest.");
		SetLocation("f_whitetrees_21_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pilgrim] Saulene", "f_whitetrees_21_2");

		AddObjective("visitStones", "Meditate at the ancient stones",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited", 4, true));

		AddReward(new ExpReward(1100, 750));
		AddReward(new SilverReward(8500));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 11)); // Normal HP Potion
		AddReward(new ItemReward(640006, 7));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_21_2.Quest1001.Stone{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_21_2.Quest1001.StonesVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_21_2.Quest1001.Stone{i}");
		}
	}
}

// Quest 1002 CLASS: Bells of the Wild
//-----------------------------------------------------------------------------

public class BellsOfTheWildQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_21_2", 1002);
		SetName("Bells of the Wild");
		SetType(QuestType.Sub);
		SetDescription("Sage Virinne is studying the ceremonial bells worn by the Kugheri. Defeat Kugheri Balzer and collect their bells for her research.");
		SetLocation("f_whitetrees_21_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Sage] Virinne", "f_whitetrees_21_2");

		AddDrop(663351, 40f, MonsterId.Kucarry_Balzer);

		AddObjective("killBalzer", "Defeat Kugheri Balzer",
			new KillObjective(12, new[] { MonsterId.Kucarry_Balzer }));
		AddObjective("collectBells", "Collect Bright Ellom Bells",
			new CollectItemObjective(663351, 5));

		AddReward(new ExpReward(1100, 750));
		AddReward(new SilverReward(8500));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 8));  // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663351,
			character.Inventory.CountItem(663351),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663351,
			character.Inventory.CountItem(663351),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Waterfall Offering
//-----------------------------------------------------------------------------

public class WaterfallOfferingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_21_2", 1003);
		SetName("Waterfall Offering");
		SetType(QuestType.Sub);
		SetDescription("Hermit Dovydas has asked you to gather five white flowers from near the waterfall to complete his daily offering.");
		SetLocation("f_whitetrees_21_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Hermit] Dovydas", "f_whitetrees_21_2");

		AddObjective("collectFlowers", "Gather white flowers near the waterfall",
			new CollectItemObjective(667232, 5));

		AddReward(new ExpReward(1100, 750));
		AddReward(new SilverReward(8500));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Card
		AddReward(new ItemReward(640003, 10)); // Normal HP Potion
		AddReward(new ItemReward(640006, 6));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667232,
			character.Inventory.CountItem(667232),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_21_2.Quest1003.Flower{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667232,
			character.Inventory.CountItem(667232),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_21_2.Quest1003.Flower{i}");
		}
	}
}

// Quest 1004 CLASS: The Unsettled Grove
//-----------------------------------------------------------------------------

public class TheUnsettledGroveQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_21_2", 1004);
		SetName("The Unsettled Grove");
		SetType(QuestType.Sub);
		SetDescription("Hunter Kazys needs help driving back the aggressive Kugheri Zeffi that have been pushing into areas they normally avoid.");
		SetLocation("f_whitetrees_21_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Hunter] Kazys", "f_whitetrees_21_2");

		AddObjective("killZeffi", "Defeat Kugheri Zeffi",
			new KillObjective(15, new[] { MonsterId.Kucarry_Zeffi }));

		AddReward(new ExpReward(1400, 1000));
		AddReward(new SilverReward(10500));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 11)); // Normal HP Potion
		AddReward(new ItemReward(640006, 7));  // Normal SP Potion
		AddReward(new ItemReward(640009, 3));  // Stamina Potion
		AddReward(new ItemReward(201106, 1));  // Mallet
	}
}
