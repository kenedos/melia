//--- Melia Script ----------------------------------------------------------
// Spring Light Woods Quest NPCs
//--- Description -----------------------------------------------------------
// Austeja's grove, where two sealing towers came apart in the spring and the
// statues that stood inside them have been walking the woods ever since.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai461QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Symbol Off the Altar
		//---------------------------------------------------------------------
		AddNpc(147492, L("[Priest] Dazine"), "f_siauliai_46_1", -1353, -156, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1001);

			dialog.SetTitle(L("Dazine"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling at the bare altar, running her fingers over the empty space where the symbol used to hang*{/}"));
				await dialog.Msg(L("You'll have to forgive the state of things — we don't often have visitors see the altar looking like this. Austeja's symbol has hung here for 200 years. In the spring both sealing towers came apart and it came off the wall in 9 pieces."));
				await dialog.Msg(L("The Infro Blood took the pieces into the west hollows - they hoard anything that catches light. Kill 25 of them and bring me 8 fragments and I can set it back."));

				var response = await dialog.Select(L("Will you go into the west hollows?"),
					Option(L("I'll recover 8 fragments"), "help"),
					Option(L("What were the towers sealing?"), "info"),
					Option(L("Have a new symbol cast"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The pieces are pale gold and about the size of a thumbnail. They keep them in the leaf litter under where they sleep, not on themselves."));
						break;

					case "info":
						await dialog.Msg(L("Nothing, as far as any record in my order goes. They are called sealing towers because they are called sealing towers, and my order has kept them for 200 years without ever writing down what for."));
						await dialog.Msg(L("Now they are open and something has walked out of both, so the question has stopped being academic rather suddenly."));
						break;

					case "leave":
						await dialog.Msg(L("It would not be hers. The one on that wall was carried here by the woman who founded this grove, and I would rather have 8 of the 9 pieces than a perfect new one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBlud", out var killObj)) return;
				if (!quest.TryGetProgress("collectFragments", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She fits the fragments together on a cloth, holding each one against the light before she sets it down*{/}"));
					await dialog.Msg(L("8 of 9, and the missing one is a corner. I can lead the join and nobody standing in front of it will ever know."));
					await dialog.Msg(L("Take the grove's offering box. It has 200 years of small coins in it and no roof to mend, because the roof is a tree."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The hollows are quiet. Now go through the leaf litter where they were sleeping - that is where they keep what they take."));
				}
				else
				{
					await dialog.Msg(L("Still too many in the hollows. Clear them first, or you will be on your knees in the litter with company."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The symbol is back over the altar and the lead join faces the wall. 200 years, and it took a stranger and an afternoon."));
			}
		});

		// Quest 1002: Spring Light Grass
		//---------------------------------------------------------------------
		AddNpc(147493, L("[Pharmacist] Tiana"), "f_siauliai_46_1", 1714, 890, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1002);

			dialog.SetTitle(L("Tiana"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's grinding something in a mortar, coughing once into her elbow before she notices you*{/}"));
				await dialog.Msg(L("Pardon the cough, it's nothing — well, it's not nothing, but it's not catching. This wood is named for a grass that only grows here, and I am the only person within 3 days who can do anything with it. Not a boast. A staffing problem."));
				await dialog.Msg(L("The Siaulav graze the eastern beds down to the root and carry the cut ends about in their fleece. Bring me 10 good stems and I can dose the whole grove for the season."));

				var response = await dialog.Select(L("Will you get the grass?"),
					Option(L("I'll bring you 10 stems"), "help"),
					Option(L("Dose them for what?"), "info"),
					Option(L("Fence the beds"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Off the fleece, not off the ground. Anything trodden into the ground has been trodden into the ground and I will not brew with it."));
						break;

					case "info":
						await dialog.Msg(L("Stone dust. Everyone in this grove has been breathing it since the towers came apart and the older ones are getting a cough I do not like the sound of."));
						await dialog.Msg(L("Dazine says it will settle. Dazine has been saying that since the spring and she has the cough herself now."));
						break;

					case "leave":
						await dialog.Msg(L("With what, and paid by whom? There are 11 people in this grove and 4 of them are over 70."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectGrass", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She snaps a stem and holds the broken end under her nose before she will take any of it*{/}"));
					await dialog.Msg(L("Still green at the core, all 10. That is a season's dosing and 2 spare, and the 4 old ones get theirs tonight."));
					await dialog.Msg(L("Take it out of my dispensing money. I charge the grove nothing and Klaipeda pays me for the surplus, so this is Klaipeda's."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough. Work the eastern beds where the ground opens out - that is where they graze thickest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All 11 dosed and 2 doses in the cupboard. Dazine took hers without arguing, which is the first time she has done anything without arguing since I met her."));
			}
		});

		// Quest 1003: What Walked Out of the Towers
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Resident] Emil"), "f_siauliai_46_1", 1975, 855, 240, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1003);

			dialog.SetTitle(L("Emil"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's standing at his fence, staring east toward the clearings with his arms crossed tight*{/}"));
				await dialog.Msg(L("You're not from the grove. Good — maybe you'll actually believe me, because nobody else here will. Thirty-four years I've lived in this wood, walked past those 2 towers every single day, and never once looked up at them. Nobody did. They were just... there."));
				await dialog.Msg(L("Now there's Shardstatues crawling the east clearings and I know exactly where they came from, because it's the same grey — the exact same grey. Kill 30 of them before somebody's child finds one, please."));

				var response = await dialog.Select(L("Will you clear the clearings?"),
					Option(L("I'll kill 30 Shardstatues"), "help"),
					Option(L("The same grey?"), "info"),
					Option(L("They've never hurt anyone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Break the legs. They do not fall over, they come apart, and a Shardstatue that cannot walk is a pile of gravel that hates you."));
						break;

					case "info":
						await dialog.Msg(L("Tower grey. Not rock grey. There is no stone in this wood that colour and there never has been - somebody carted it in to build the 2 towers and then carted the rest away."));
						await dialog.Msg(L("34 years of walking past a thing and it turns out to be full of something. I have not been sleeping well."));
						break;

					case "leave":
						await dialog.Msg(L("They put my neighbour's dog through a fence 6 days ago. He is not a young man and he sat down in the road and cried about it, and he has never cried about anything."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStatues", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Clearings are walkable. I took the long way home through all 3 of them this evening on purpose, just to have done it."));
					await dialog.Msg(L("Take this. 34 years of putting a bit by for a roof I have already got, so it may as well go to somebody who used a day well."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still plenty out there. Work the open clearings, not the thickets - in a thicket you will hear them and not see them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("More came up out of the towers in the night. Fewer than before, but they came, and I have started looking up at the towers on my way past."));
			}
		});

		// Quest 1004: Dulke's Three Packs
		//---------------------------------------------------------------------
		AddNpc(20102, L("[Merchant] Dulke"), "f_siauliai_46_1", -1776, -1027, 1, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1004);

			dialog.SetTitle(L("Dulke"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's sitting on an empty handcart at the road's edge, refusing to look down the path behind him*{/}"));
				await dialog.Msg(L("You came up that road? On purpose?! Twelve years I've run this road, I know where every root crosses it, and the day the towers went I was right between them with 3 packs on this cart — I ran, and I ran the right way, thank the gods, but I left all 3 behind and I have not slept properly since!"));
				await dialog.Msg(L("They're still out there! Go find all 3 of my packs, tell me what state they're in — I physically cannot make my legs walk back down that road, I've tried, I've tried twice!"));

				var response = await dialog.Select(L("Will you go and find them?"),
					Option(L("I'll find all 3 packs"), "help"),
					Option(L("What's in them?"), "info"),
					Option(L("Write them off"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("One went off the cart early, one by the hollow, and the third is right at the foot of the Ranka tower, which is where I stopped running and started running properly."));
						break;

					case "info":
						await dialog.Msg(L("Grove supplies. Lamp oil, salt, needles, and the pharmacist's glassware, which she has been very polite about for 4 months in a way that is worse than shouting."));
						await dialog.Msg(L("And the season's takings, which is 12 years of a habit of not banking anything."));
						break;

					case "leave":
						await dialog.Msg(L("I have tried. I get as far as the first bend and my legs make the decision before I do, and I would like that to stop being true about me."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("findPacks", out var checkObj)) return;

				if (checkObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to all 3 accounts and then asks you to repeat the third one*{/}"));
					await dialog.Msg(L("Oil gone, salt gone, glassware whole, and the third pack pulled open and everything in it stacked. Stacked. Nothing in this wood stacks."));
					await dialog.Msg(L("Take the takings. I am carting the glassware to Tiana tomorrow and I am walking the whole road to do it, and I would like that to be the end of it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not all 3. One is south-west by the old hollow, one on the road bend in the middle of the wood, and the third at the foot of the Ranka tower."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Walked it. Both towers, the whole road, with a cart. Tiana got her glassware and I have not stopped shaking since, and I am going again tomorrow."));
			}
		});

		// Quest 1004 collection points - the abandoned packs
		//---------------------------------------------------------------------
		void AddMerchantPack(int packNumber, int model, string observation, int x, int z, int direction)
		{
			AddNpc(model, L("Merchant's Pack"), "f_siauliai_46_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_46_1", 1004);
				var variableKey = $"Laima.Quests.f_siauliai_46_1.Quest1004.Pack{packNumber}";
				var counterKey = "Laima.Quests.f_siauliai_46_1.Quest1004.PacksFound";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A merchant's pack lying where it was dropped, months ago*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already went through this one*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Going through the pack..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var found = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, found);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Packs found: {0}/3", found));

					if (found >= 3)
						character.ServerMessage(L("{#FFD700}All 3 packs found. Return to Dulke.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the pack where it lies."));
				}
			});
		}

		AddMerchantPack(1, 47160,
			L("South-West Pack: the lamp oil is gone, jar and all. The straps were cut, not chewed."), -647, -1297, 0);
		AddMerchantPack(2, 47160,
			L("Road Bend Pack: salt gone, needles gone, and the pharmacist's glassware sitting upright and unbroken in the middle of it."), -280, -369, 0);
		AddMerchantPack(3, 47161,
			L("Ranka Tower Pack: pulled open and every single thing in it stacked in rows by size on the flat stone beside it."), 311, -871, 270);

		// Quest 1005: The Foot of the Rankis Tower
		//---------------------------------------------------------------------
		AddNpc(147492, L("[Priest] Dazine"), "f_siauliai_46_1", -217, -869, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1005);

			dialog.SetTitle(L("Dazine"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_siauliai_46_1", 1001)))
				{
					await dialog.Msg(L("{#666666}*She's standing at the tower's foot with the symbol still in pieces in her satchel, refusing to go any closer*{/}"));
					await dialog.Msg(L("Get the symbol back on the altar first. I am not standing under an open tower with an empty wall behind me."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's crouched at the flat stone, the whole symbol now hanging steady at her chest*{/}"));
				await dialog.Msg(L("Dulke's third pack was stacked. Sorted by size, in rows, on this very stone at the foot of the Ranka tower. A Shardstatue does not sort things and neither does anything else in this wood."));
				await dialog.Msg(L("Something is using this tower and it has been using it since spring. Kill 20 Shardstatues to open the base, then take the 2 that never leave it. Carry the symbol - it is whole and it is the only thing here that is."));

				var response = await dialog.Select(L("Will you open the tower base?"),
					Option(L("I'll open it and take the 2"), "help"),
					Option(L("Using it for what?"), "info"),
					Option(L("Seal it again"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650831, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The 2 that hold the base are the only ones with unbroken faces. Everything else that came out of these towers came out shattered."));
						break;

					case "info":
						await dialog.Msg(L("Sorting. That is all I can tell you and it is the part that frightens me. Whatever is down there took a merchant's pack apart and arranged it, and then left it."));
						await dialog.Msg(L("200 years my order has kept 2 towers and written down nothing about what is in them. I am beginning to think that was the arrangement rather than an oversight."));
						break;

					case "leave":
						await dialog.Msg(L("With what? The order that built these has not sent anyone in my lifetime. There is me, a pharmacist, a frightened merchant and 8 people over 60."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openBase", out var baseObj)) return;
				if (!quest.TryGetProgress("killWardens", out var wardenObj)) return;

				if (baseObj.Done && wardenObj.Done)
				{
					await dialog.Msg(L("{#666666}*She holds the symbol into the open tower base and the light off it goes down further than the base should be deep*{/}"));
					await dialog.Msg(L("It keeps going. There is a shaft under the Rankis tower and there is one under the Ranka tower, and my order's 200 years of records describe 2 towers and no shafts at all."));
					await dialog.Msg(L("Take the robe out of the base. It is priest's cloth and it is older than my order, and whoever wore it went down and not out. I am writing to Klaipeda tonight and I am going to name both shafts."));

					character.Quests.Complete(questId);
				}
				else if (baseObj.Done)
				{
					await dialog.Msg(L("The base is open. The 2 with whole faces are still standing in it and they have not moved once in 4 months."));
				}
				else
				{
					await dialog.Msg(L("Too many round the base. Clear them, or the 2 will simply stand there while the rest bury you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Nothing new has come up either shaft in 9 days. Emil has taken to sitting at the tower foot in the evenings with a lamp, which I have decided not to discourage."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Symbol Off the Altar
//-----------------------------------------------------------------------------

public class TheSymbolOffTheAltarQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1001);
		SetName(L("The Symbol Off the Altar"));
		SetType(QuestType.Sub);
		SetDescription(L("Austeja's symbol has hung over the grove altar for 200 years and came off the wall in 9 pieces when the sealing towers went. The Infro Blood have hoarded the fragments in the west hollows."));
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Priest] Dazine"), "f_siauliai_46_1");

		AddObjective("killBlud", L("Kill Infro Blood in the west hollows"),
			new KillObjective(25, new[] { MonsterId.Infro_Blud }));

		AddObjective("collectFragments", L("Recover Austeja's Fragmented Symbol"),
			new CollectItemObjective(650829, 8));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion

		AddDrop(650829, 0.35f, MonsterId.Infro_Blud);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650829, character.Inventory.CountItem(650829), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650829, character.Inventory.CountItem(650829), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Spring Light Grass
//-----------------------------------------------------------------------------

public class SpringLightGrassQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1002);
		SetName(L("Spring Light Grass"));
		SetType(QuestType.Sub);
		SetDescription(L("The grove has been breathing stone dust since the towers came apart, and the only thing that treats it is a grass that grows nowhere else. The Siaulav graze the eastern beds to the root and carry the cut ends in their fleece."));
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pharmacist] Tiana"), "f_siauliai_46_1");

		AddObjective("collectGrass", L("Recover Spring Light Grass from the Siaulav"),
			new CollectItemObjective(650834, 10));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion

		AddDrop(650834, 0.45f, MonsterId.Siaulav);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650834, character.Inventory.CountItem(650834), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650834, character.Inventory.CountItem(650834), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: What Walked Out of the Towers
//-----------------------------------------------------------------------------

public class WhatWalkedOutOfTheTowersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1003);
		SetName(L("What Walked Out of the Towers"));
		SetType(QuestType.Sub);
		SetDescription(L("The Shardstatues in the east clearings are tower grey, and there is no stone that colour anywhere in the wood. Kill 30 of them before a child finds one."));
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Resident] Emil"), "f_siauliai_46_1");

		AddObjective("killStatues", L("Kill Shardstatues in the east clearings"),
			new KillObjective(30, new[] { MonsterId.Shardstatue }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: Dulke's Three Packs
//-----------------------------------------------------------------------------

public class DulkesThreePacksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1004);
		SetName(L("Dulke's Three Packs"));
		SetType(QuestType.Sub);
		SetDescription(L("A carter was between the 2 towers with 3 packs on a handcart on the day they came apart, and has not been able to walk that road since. All 3 packs are still lying where he left them."));
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Merchant] Dulke"), "f_siauliai_46_1");

		AddObjective("findPacks", L("Find and go through all 3 abandoned packs"),
			new VariableCheckObjective("Laima.Quests.f_siauliai_46_1.Quest1004.PacksFound", 3, true));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1004.PacksFound");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1004.Pack{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1004.PacksFound");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1004.Pack{i}");
	}
}

// Quest 1005 CLASS: The Foot of the Rankis Tower
//-----------------------------------------------------------------------------

public class TheFootOfTheRankisTowerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1005);
		SetName(L("The Foot of the Rankis Tower"));
		SetType(QuestType.Sub);
		SetDescription(L("Something took a merchant's pack apart at the Ranka tower and stacked the contents in rows by size. Open the Rankis tower base and take the 2 Shardstatues that have not moved from it in 4 months."));
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Priest] Dazine"), "f_siauliai_46_1");

		AddPrerequisite(new CompletedPrerequisite("f_siauliai_46_1", 1001));

		AddObjective("openBase", L("Kill Shardstatues around the Rankis tower base"),
			new KillObjective(20, new[] { MonsterId.Shardstatue }));

		AddObjective("killWardens", L("Take the 2 unbroken statues holding the base"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Shardstatue, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Siaulav, 3),
				},
				resetIdent: "openBase",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(10000, 7000));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(533104, 1)); // Saint Robe
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650831, character.Inventory.CountItem(650831), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650831, character.Inventory.CountItem(650831), InventoryItemRemoveMsg.Destroyed);
	}
}
