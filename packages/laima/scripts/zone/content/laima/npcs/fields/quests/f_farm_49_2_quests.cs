//--- Melia Script ----------------------------------------------------------
// Shaton Farm - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_49_2 map.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm492QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Walking Stumps
		// =====================================================================
		// Farmer Andrius - Orange Stumpy Trees invading tomato fields
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Farmer] Andrius"), "f_farm_49_2", 559, -1267, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1001);

			dialog.SetTitle(L("Andrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A broad-shouldered farmer rests on the handle of a splintered axe, staring at fresh furrows in his field*{/}"));
				await dialog.Msg(L("You ever seen a tree walk? I have. Now I've seen eighteen. They come at night, uproot themselves, and stroll into the tomato rows like they own them."));

				var response = await dialog.Select(L("My axe breaks on the third swing. These aren't normal trees - the corruption from the demon prison has put roots in their spine. I need someone stronger-armed than me to kill eighteen before next harvest moon."),
					Option(L("I'll kill the walking stumps"), "help"),
					Option(L("Walking trees? Seriously?"), "info"),
					Option(L("That sounds like nonsense"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods grimly and hoists his broken axe*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Aim for the trunk. The Earth-magic's in the roots, but the mouth - yes, they've mouths - is on the trunk."));
						await dialog.Msg(L("Good boots help. Pendinmire crossed this field last night. You'll want to move fast if you see those tracks."));
						break;

					case "info":
						await dialog.Msg(L("Seriously. Stumpy Tree, they call it. Normally it sits in a forest. This batch roots in my soil, pulls up, walks east, roots again in my field."));
						await dialog.Msg(L("Demon pollen wakes them up. Same pollen Morta warned Jonas about. Now it's here and the trees aren't sleeping anymore."));
						break;

					case "leave":
						await dialog.Msg(L("Come back tomorrow morning. You'll see the new furrows where they walked in overnight."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStump", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He exhales, shoulders dropping*{/}"));
					await dialog.Msg(L("Eighteen. The field's quiet tonight. My tomatoes stand a chance."));
					await dialog.Msg(L("Take these - skirmisher boots, good leather. A farmer who walks fields all day knows good boots when he holds them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Eastern rows, where the pollen settles. Aim for the trunk-mouth."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stumps have stayed in their forest three nights running. Hope it stays that way."));
			}
		});

		// =====================================================================
		// QUEST 1002: Jonas's Reply
		// =====================================================================
		// Farmer Jonas - Tomato season reply for Morta
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Farmer] Jonas"), "f_farm_49_2", 626.1717f, 647.68665f, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1002);

			dialog.SetTitle(L("Jonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*The old farmer you met at the Myrkiti farm looks up from a basket of tomatoes, grinning*{/}"));
				await dialog.Msg(L("Hey, you again! Look at this field - the whole thing's red. Last week I thought half of it would rot before harvest."));
				await dialog.Msg(L("That warning Morta sent us? That's what saved it. We got the husks and masks on in time, and the pollen just blew right past."));

				var response = await dialog.Select(L("I wrote her a thank-you letter last night. The courier, Benas, is heading north tomorrow morning. Can you bring it to him before he leaves?"),
					Option(L("Sure, I'll take it"), "help"),
					Option(L("Why not write her yourself later?"), "info"),
					Option(L("Find your own courier"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He pulls an envelope out of his vest and hands it over*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("I tied it with tomato vine instead of wax. Morta'll laugh, but she'll know it's from me."));
						await dialog.Msg(L("Watch yourself on the road north. Andrius is still dealing with those walking stumps."));
						break;

					case "info":
						await dialog.Msg(L("My hands aren't fast anymore. Benas writes a whole letter in the time I write three lines."));
						await dialog.Msg(L("Plus he adds news from up north - who's sick, what the weather's like, that kind of thing. Morta likes hearing it."));
						break;

					case "leave":
						await dialog.Msg(L("Alright, I'll wait for next week's run then. She won't mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverReply", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He sets his basket down and smiles*{/}"));
					await dialog.Msg(L("Good. Benas has it. Morta'll be reading it by sundown, and complaining about my handwriting by supper."));
					await dialog.Msg(L("Here, take these tomatoes. Three of the best ones. Slice them thick, add a little salt - that's all you need."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Benas is up at the new settlement, north of here. They're laying out vineyard rows up there. Go before he leaves."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Morta sent us two crates of husks yesterday. Half my farmhands have masks now. That's how neighbors should be."));
			}
		});

		// =====================================================================
		// Courier Benas - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(20125, L("[Courier] Benas"), "f_farm_49_2", -1889, 916, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1002);

			dialog.SetTitle(L("Benas"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A courier checks his satchel while workers hammer stakes into the slope behind him*{/}"));
				await dialog.Msg(L("Big plans for this place - they're putting in a vineyard. Grapes next year, wine the year after, if the weather plays nice."));
				await dialog.Msg(L("I head out at dawn. If you've got mail, hand it over now. I'm closing up the bag at sundown."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("Jonas's letter is in the bag. Morta'll have it by lunch tomorrow."));
				return;
			}

			await dialog.Msg(L("{#666666}*He looks at the envelope and laughs*{/}"));
			await dialog.Msg(L("Tied with tomato vine. Yeah, that's Jonas alright. Feels heavy too - he must've written her a long one."));
			await dialog.Msg(L("Tell him I'll deliver it first thing. And I'll let her know about the vineyard going in up here. Maybe she'll send me a bottle when it's done."));

			character.Variables.Perm.Set("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Letter delivered. Return to Jonas.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Cyst Sap Vials
		// =====================================================================
		// Farmer Janina - Apothecary-farmer distilling anti-pollen poultices
		//---------------------------------------------------------------------
		AddNpc(157100, L("[Farmer] Janina"), "f_farm_49_2", 1831, 1421, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1003);

			dialog.SetTitle(L("Janina"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farmer with ink-stained fingertips labels tiny glass vials in careful rows*{/}"));
				await dialog.Msg(L("Goda stitches masks at Myrkiti. I distill what goes between the stitches. Cyst sap, boiled down with river-oil, makes a poultice that stops pollen burn in its tracks."));

				var response = await dialog.Select(L("The Cysts grow on old trees - big, round, pulsing. They don't move; they can't. But they wake up angry when you tap them. I need five clean sap vials. Tap, fill, step back, next."),
					Option(L("I'll gather five vials"), "help"),
					Option(L("Do Cysts really wake up?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She hands you five empty vials, stoppered with wax*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Quarter turn to uncork, press the tip into the sap-pore, quarter turn to seal. Easy enough if you're not panicking."));
						await dialog.Msg(L("About one in five wakes up when tapped. Back away and it'll settle again. Don't try to fight it with the vial in your hand."));
						break;

					case "info":
						await dialog.Msg(L("Oh yes. They're not really dead - just rooted. Tap them gentle and they sleep through it. Tap them wrong and they remember they used to move."));
						await dialog.Msg(L("My grandmother said the old Cysts used to be goblins who fell asleep during a harvest festival. I don't believe her, but I also don't tap rough."));
						break;

					case "leave":
						await dialog.Msg(L("Then breathe shallow if you cross the south fields. My poultices go to people who ask for them, not the ones who can't be bothered."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sapCount = character.Inventory.CountItem(650801);

				if (sapCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She holds a vial to the lamplight, watching the sap glow faintly*{/}"));
					await dialog.Msg(L("Clean. All five. No crushed glass, no spoiled sap, no panicked-tap discoloration. You've done this before, haven't you?"));
					await dialog.Msg(L("Take this - it's honest payment plus an extra vial for your own kit. Pollen burn on the eyes will thank you someday."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five Cysts. Quarter-turn tap. Don't run from the angry ones - just step back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The poultices are distilled. Every mask at Myrkiti and Shaton has a dab of sap now. The pollen burns less."));
			}
		});

		// =====================================================================
		// CYST GROWTHS
		// =====================================================================
		// For Quest 1003 - Cyst Sap Vials
		// =====================================================================

		void AddCystGrowth(int cystNumber, int x, int z, int direction)
		{
			AddNpc(MonsterId.Cyst, L("Cyst Growth"), "f_farm_49_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_49_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A pulsing cyst bulges from a tree trunk, sap glistening at a pore*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_49_2.Quest1003.Cyst{cystNumber}";
				var tapped = character.Variables.Perm.GetBool(variableKey, false);

				if (tapped)
				{
					await dialog.Msg(L("{#666666}*The cyst here is sealed. A fresh bead of sap is already forming, but it belongs to the next season*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_49_2.Quest1003.Cyst{cystNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 20)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					var spawnCount = RandomProvider.Get().Next(1, 3);
					if (SpawnTempMonsters(character, MonsterId.Cyst, spawnCount, 50, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}The Cyst wakes up, furious!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Tapping sap..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650801, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650801);
					character.ServerMessage(LF("Sap vials filled: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All vials filled! Return to Farmer Janina.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Tapping interrupted."));
				}
			});
		}

		AddCystGrowth(1, 637, 1348, 45);
		AddCystGrowth(2, 418, 1235, 45);
		AddCystGrowth(3, 603, 1057, 45);
		AddCystGrowth(4, 995, 1185, 45);
		AddCystGrowth(5, -159, 1044, 45);
		AddCystGrowth(6, -284, 807, 45);
		AddCystGrowth(7, 1681, 1149, 45);
		AddCystGrowth(8, 1854, 1289, 45);

		// =====================================================================
		// QUEST 1004: Pendinmire's Range
		// =====================================================================
		// Farmer Vytis - Mapping the apex creature's territory
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Farmer] Vytis"), "f_farm_49_2", 914, -1334, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_2", 1004);

			dialog.SetTitle(L("Vytis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grey-haired farmer measures a massive paw-print with his belt, the flattened grass around it still fresh*{/}"));
				await dialog.Msg(L("Pendinmire. The old hunters said it'd never cross the thorn ridge. The old hunters were wrong."));

				var response = await dialog.Select(L("It ate two oxen last week. If we don't put it down before harvest, it takes a farmhand next. I need it killed - one hunter, one shot, finished. Are you that hunter?"),
					Option(L("I'll kill the Pendinmire"), "help"),
					Option(L("Why one hunter?"), "info"),
					Option(L("That thing could eat a cow"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods once, slow, and points south*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Track it past the bone pile. It rests at midday. Hit it from the flank - the throat, if you can reach it."));
						await dialog.Msg(L("If you don't come back by sundown, I'll assume the worst and ring the evacuation bell."));
						break;

					case "info":
						await dialog.Msg(L("A pack draws its attention - it scatters them and picks them off one by one. One hunter it doesn't see coming. That's the only odds I'll bet a life on."));
						break;

					case "leave":
						await dialog.Msg(L("Yeah, it could eat a cow. Already has. I'm trying to figure out if it'll eat a person before it actually does."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPendinmire", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He exhales, shoulders dropping*{/}"));
					await dialog.Msg(L("It's down. The southern fields are ours again. Tomatoes stand a chance, and so do my farmhands."));
					await dialog.Msg(L("Take these - skirmisher boots, broken in but sound. A man who walks his fields knows good leather."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South of the bone pile, around midday. Flank it. Don't let it see you first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The southern fields are quiet again. Pendinmire's gone. The village stays."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Walking Stumps
//-----------------------------------------------------------------------------

public class WalkingStumpsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1001);
		SetName("Walking Stumps");
		SetType(QuestType.Sub);
		SetDescription("Farmer Andrius needs the Orange Stumpy Trees killed before they walk out of the forest and destroy Jonas's tomato crop.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Andrius", "f_farm_49_2");

		AddObjective("killStump", "Defeat Orange Stumpy Trees",
			new KillObjective(18, new[] { MonsterId.Stub_Tree_Orange }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1));  // Recovery Potion
		AddReward(new ItemReward(501148, 1));  // Superior Skirmisher Gloves
	}
}

// Quest 1002 CLASS: Jonas's Reply
//-----------------------------------------------------------------------------

public class JonassReplyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1002);
		SetName("Jonas's Reply");
		SetType(QuestType.Sub);
		SetDescription("Jonas's reply to Morta needs to reach Courier Benas before he leaves for Myrkiti at dawn.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Jonas", "f_farm_49_2");

		AddObjective("deliverReply", "Deliver Jonas's reply to Courier Benas",
			new VariableCheckObjective("Laima.Quests.f_farm_49_2.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_2.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Cyst Sap Vials
//-----------------------------------------------------------------------------

public class CystSapVialsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1003);
		SetName("Cyst Sap Vials");
		SetType(QuestType.Sub);
		SetDescription("Farmer Janina needs five vials of Cyst sap to distill anti-pollen poultices for the masks at Myrkiti and Shaton.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Janina", "f_farm_49_2");

		AddObjective("collectSap", "Tap Cyst growths for sap vials",
			new CollectItemObjective(650801, 5));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650801,
			character.Inventory.CountItem(650801),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650801,
			character.Inventory.CountItem(650801),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_2.Quest1003.Cyst{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Pendinmire's Range
//-----------------------------------------------------------------------------

public class PendinmiresRangeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_2", 1004);
		SetName("Pendinmire's Range");
		SetType(QuestType.Sub);
		SetDescription("Farmer Vytis needs the Pendinmire killed before it claims Shaton's southern fields and the farmhands working them.");
		SetLocation("f_farm_49_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Farmer] Vytis", "f_farm_49_2");

		AddObjective("killPendinmire", "Defeat the Pendinmire",
			new KillObjective(1, new[] { MonsterId.Pendinmire }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(511148, 1));  // Superior Skirmisher Boots
	}
}
