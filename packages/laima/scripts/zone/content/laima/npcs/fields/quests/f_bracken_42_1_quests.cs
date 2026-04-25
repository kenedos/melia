//--- Melia Script ----------------------------------------------------------
// Khonot Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Khonot Forest - the trade-trail forest east of Orsha where
// the Blue Gosaru troupe has overrun the canopy and disrupted caravan routes.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors;

public class FBracken421QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Gosaru Ape Troupe
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Forest-Ward] Bronius"), "f_bracken_42_1", 0, 0, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1001);

			dialog.SetTitle(L("Bronius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Trade caravans used to roll through Khonot Forest without losing more than the odd horseshoe. Not anymore."));
				await dialog.Msg(L("A troupe of Blue Gosaru has claimed the canopy along the western trail. They drop on travelers from the branches without warning, and the merchant guild has stopped sending escorts because too few come back."));
				await dialog.Msg(L("If we don't break their numbers soon, the inland villages will be cut off from Orsha entirely."));

				var response = await dialog.Select(L("Will you help thin the troupe?"),
					Option(L("I'll kill them"), "help"),
					Option(L("Tell me about these Gosaru"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty-five Blue Gosarus from the canopy?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-five should break their pack for a full season. Stay clear of the trees with the heaviest growth - that's where they nest and where they ambush from."));
							await dialog.Msg(L("And keep your eyes upward as you walk. They're patient hunters."));
						}
						break;

					case "info":
						await dialog.Msg(L("They're smart. Smarter than apes have any right to be. They post lookouts, they signal each other across half a mile of canopy, and they bait travelers into the deeper brush before they strike."));
						await dialog.Msg(L("I've watched them work. It's organized. That's what makes them dangerous."));
						break;

					case "leave":
						await dialog.Msg(L("Don't take too long deciding. Every week the trail closes, another inland village stretches its larder thinner."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosarus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("The canopy is quiet for the first time in months. The trail is open again."));
					await dialog.Msg(L("Take your pay, with my thanks. You've done a season's work in a few days."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The branches still rustle with them. Keep at it - we're not there yet."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravans are rolling through Khonot again. First time in months. The merchant guild sends their thanks every time one arrives."));
			}
		});

		// Quest 2: Tanu Tails
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Furrier] Alfreds"), "f_bracken_42_1", -1000, -400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1002);

			dialog.SetTitle(L("Alfreds"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Orsha's fur market is hungry for Blue Tanu tails this season. Long, glossy, well-kept ones fetch real silver in the right stalls."));
				await dialog.Msg(L("Trouble is, I can't leave the shop to hunt them myself, and the regular trappers won't go as deep into Khonot as the prime Tanus run. So I'm hiring out."));

				var response = await dialog.Select(L("Interested in the work?"),
					Option(L("I'll do the skinning"), "help"),
					Option(L("What makes a tail prime?"), "info"),
					Option(L("Not for me"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill thirty Blue Tanus and bring back seven prime tails?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Cut at the base of the tail, keep the fur clean of blood. A torn or stained tail is worthless on the stall - I won't pay for what I can't sell."));
							await dialog.Msg(L("Seven prime ones is the count. You'll have to skin more than that to find them."));
						}
						break;

					case "info":
						await dialog.Msg(L("Long, thick-furred, no bald patches and no scars. The mangy ones go straight in the bin."));
						await dialog.Msg(L("You'll know one when you see it - the prime tails almost shine in the light."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. There'll be another hunter along by week's end."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTanus", out var killObj)) return;
				if (!quest.TryGetProgress("gatherTails", out var tObj)) return;

				if (killObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Seven prime tails, every one of them market-ready. The stall will be stocked through next week, easy."));
					await dialog.Msg(L("Here's your pay, and a little extra for the quality. We might do business again."));
					character.Inventory.Remove(650255, character.Inventory.CountItem(650255), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at the skinning. Seven prime tails is the count - the rest you can sell elsewhere."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sold out of Tanu fur by week's end. The Orsha buyers are already asking when the next batch comes in."));
			}
		});

		// Quest 3: Doyor Fangs
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Dentist] Alda"), "f_bracken_42_1", -300, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1003);

			dialog.SetTitle(L("Alda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Half this village comes to me clutching their jaws, and I haven't had a batch of toothache powder in stock for three weeks."));
				await dialog.Msg(L("Blue Doyor fangs, ground fine and mixed with willow bark, are the old village remedy. I need six clean fangs to brew the next batch - enough to last us through the cold months."));

				var response = await dialog.Select(L("Will you help me?"),
					Option(L("I'll bring the fangs"), "help"),
					Option(L("Toothache cure?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Blue Doyors and bring back six clean fangs?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Six fangs - the long canines, mind you, not the molars. And make sure they're not splintered. Cracked fangs ruin the powder."));
						}
						break;

					case "info":
						await dialog.Msg(L("Old village remedy, passed down from my grandmother and her grandmother before her. Numbs the gum, dries up the rot, lets a sore tooth heal instead of festering."));
						await dialog.Msg(L("Always works, when I can get the fangs. The Doyors used to wander right up to the village fence. Not anymore - they've gone shy and deep."));
						break;

					case "leave":
						await dialog.Msg(L("Pain doesn't wait for anyone. Half the village will be miserable until someone helps me brew that powder."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDoyors", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFangs", out var fObj)) return;

				if (killObj.Done && fObj.Done)
				{
					await dialog.Msg(L("Six clean fangs, every one of them sound. The powder will brew through the night and be ready by morning."));
					await dialog.Msg(L("Take this for your trouble - and tell anyone in pain to come see me tomorrow."));
					character.Inventory.Remove(650265, character.Inventory.CountItem(650265), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still need those fangs. Keep hunting - and remember, the long canines, unsplintered."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Half the village smiles pain-free now. Word's spreading - I had a fellow ride in from two villages over yesterday."));
			}
		});

		// Quest 4: Folibu Wings
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Tinkerer] Kaspars"), "f_bracken_42_1", 1000, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1004);

			dialog.SetTitle(L("Kaspars"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I'm building a glider. A real one - not a child's toy, not a model. A machine a grown man can ride."));
				await dialog.Msg(L("Yellow Folibu wings are the only material in three days' ride that's both light enough and strong enough. Translucent, taut as drum-skin, and they don't tear under load."));
				await dialog.Msg(L("Five intact wings is all I need to finish the prototype. Three years of calculations, and it all comes down to this."));

				var response = await dialog.Select(L("Want to be part of history?"),
					Option(L("I'll harvest the wings"), "help"),
					Option(L("Does this thing actually fly?"), "info"),
					Option(L("Sounds like nonsense"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Yellow Folibus and bring back five intact wings?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Tear them at the joint, never through the membrane. A puncture and the wing's useless to me - the air leaks straight through."));
							await dialog.Msg(L("You'll kill more than five Folibus to get five clean wings. That's just the way of it."));
						}
						break;

					case "info":
						await dialog.Msg(L("It will fly. It has to. I've been over the figures a thousand times - the lift surface, the keel weight, the trim of the membrane."));
						await dialog.Msg(L("Five wings and I prove it. Or I don't, and I admit my whole life's work was a fool's errand. Either way, I have to know."));
						break;

					case "leave":
						await dialog.Msg(L("Then flight stays theoretical. For another year, anyway. Every furrier in Orsha calls me a fool, but the figures don't lie."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFolibus", out var killObj)) return;
				if (!quest.TryGetProgress("gatherWings", out var wObj)) return;

				if (killObj.Done && wObj.Done)
				{
					await dialog.Msg(L("Five wings, all of them whole! Look at this membrane - perfect, every one. Testing this weekend, on the ridge at dawn."));
					await dialog.Msg(L("Take this with my thanks. If the glider works, your name goes in my notebook beside mine."));
					character.Inventory.Remove(650270, character.Inventory.CountItem(650270), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Remember - tear at the joint, not through the membrane. Five intact wings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The glider went six paces before it crashed into a tree. But it flew - it actually flew! Next prototype is already on the workbench."));
			}
		});

		// Quest 5: The Gosaru King
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Jekabs"), "f_bracken_42_1", 200, 400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1005);
			var kingSpawnedKey = "Laima.Quests.f_bracken_42_1.Quest1005.KingSpawned";

			dialog.SetTitle(L("Jekabs"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Forest-Ward Bronius will tell you to kill thirty-five Gosarus. That's a fine job for a forester. But the troupe won't stay broken."));
				await dialog.Msg(L("There's a king to that pack. Three times the size of his troupe, and twice as cunning. He won't show himself while his apes are full strength."));
				await dialog.Msg(L("Thin a few of his troupe, though, and he'll come down from the canopy himself to defend them. That's when we have him."));

				var response = await dialog.Select(L("Want the bounty?"),
					Option(L("I'll face the King"), "help"),
					Option(L("How big is this thing, really?"), "info"),
					Option(L("Find another hunter"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin ten of his troupe and slay the King when he descends?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten Gosarus first. Then come back here and I'll know he's listening - he always does. The drop will follow."));
							await dialog.Msg(L("Stay clear of the heavy branches. He doesn't climb down. He drops, and he brings half a tree with him."));
						}
						break;

					case "info":
						await dialog.Msg(L("Three times the size of a normal Blue Gosaru. Smart as a man, mean as a starving bear."));
						await dialog.Msg(L("Tore the last hunter clean in half. I found his belt buckle, his coin purse, and nothing else. So go in respectful, or don't go in at all."));
						break;

					case "leave":
						await dialog.Msg(L("His troupe grows every week. Eventually he'll come down whether anyone hunts him or not - and on his terms, that's worse."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTroupe", out var gObj)) return;
				if (!quest.TryGetProgress("killKing", out var kObj)) return;

				if (gObj.Done && kObj.Done)
				{
					await dialog.Msg(L("The King is dead? Show me the hide. Aye - that's him. That's the bastard."));
					await dialog.Msg(L("Khonot's breathing again. Caravans will pass without escort soon. Pay's yours, with a hunter's bonus on top."));
					character.Variables.Perm.Remove(kingSpawnedKey);
					character.Quests.Complete(questId);
				}
				else if (gObj.Done && !kObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(kingSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(kingSpawnedKey, true);
						if (SpawnTempMonsters(character, MonsterId.Gosaru_Blue, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("There - the canopy's shaking! He drops! Go now, before he chooses his ground!"));
							character.ServerMessage(L("{#FF9966}The Gosaru King drops from the canopy!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's loose somewhere out there. Find him before he climbs back into the canopy and we lose our chance."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten of his troupe first. He won't show himself until they thin out - that's how it always works with the canopy kings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Khonot's breathing. Caravans pass without escort now. You did real work - the kind that gets remembered."));
			}
		});

		// Quest 6: Khonot Sweep
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Ranger] Peteris"), "f_bracken_42_1", -600, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_42_1", 1006);

			dialog.SetTitle(L("Peteris"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Standard caravan-protection contract. Khonot has three problem species - Gosarus, Doyors, and Tanus - and unless someone kills them on a schedule, they overrun the trails within a month."));
				await dialog.Msg(L("Twelve of each. Doesn't matter what order, just bring me the count. The merchant guild pays per sweep, and they pay fair."));

				var response = await dialog.Select(L("Take the contract?"),
					Option(L("I'll do the sweep"), "help"),
					Option(L("What's the pay?"), "info"),
					Option(L("Not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve each of Blue Gosarus, Blue Doyors, and Blue Tanus?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thirty-six in total. Keep moving and you'll find them in any direction you walk - they're the whole forest's pulse."));
						}
						break;

					case "info":
						await dialog.Msg(L("Standard ranger pay, plus a guild bonus for every successful sweep. Fair work for fair coin."));
						break;

					case "leave":
						await dialog.Msg(L("Caravan rolls through in three days regardless. Hope the trails are clear - or that the guild hires someone less choosy than you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGosarus", out var gObj)) return;
				if (!quest.TryGetProgress("killDoyors", out var dObj)) return;
				if (!quest.TryGetProgress("killTanus", out var tObj)) return;

				if (gObj.Done && dObj.Done && tObj.Done)
				{
					await dialog.Msg(L("Sweep done, full count, all three species. Caravan'll roll clean through Khonot now."));
					await dialog.Msg(L("Take your pay, with my thanks. I'll mark you down for the next contract."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at the sweep. Twelve of each is the count - Gosarus, Doyors, Tanus."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravan went through without a scratch. You'll be the first I call when the next contract opens."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FBracken421Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1001);
		SetName(L("Gosaru Troupe"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Gosarus dominating the Khonot canopy."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forest-Ward] Bronius"), "f_bracken_42_1");

		AddObjective("killGosarus", L("Kill Blue Gosarus"),
			new KillObjective(35, new[] { MonsterId.Gosaru_Blue }));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(21000));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 8));
	}
}

public class FBracken421Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1002);
		SetName(L("Prime Tanu Tails"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Tanus for prime tails for the Orsha fur market."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Furrier] Alfreds"), "f_bracken_42_1");

		AddObjective("killTanus", L("Kill Blue Tanus"),
			new KillObjective(30, new[] { MonsterId.Tanu_Blue }));

		AddObjective("gatherTails", L("Gather prime tails"),
			new CollectItemObjective(650255, 7));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 8));
		AddReward(new ItemReward(640012, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650255, character.Inventory.CountItem(650255), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650255, character.Inventory.CountItem(650255), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1003);
		SetName(L("Doyor Fangs"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Blue Doyors and bring fangs for toothache powder."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dentist] Alda"), "f_bracken_42_1");

		AddObjective("killDoyors", L("Kill Blue Doyors"),
			new KillObjective(15, new[] { MonsterId.Doyor_Blue }));

		AddObjective("gatherFangs", L("Gather fangs"),
			new CollectItemObjective(650265, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 8));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650265, character.Inventory.CountItem(650265), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650265, character.Inventory.CountItem(650265), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1004);
		SetName(L("Glider Wings"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Yellow Folibus and bring wings for a tinkerer's glider."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tinkerer] Kaspars"), "f_bracken_42_1");

		AddObjective("killFolibus", L("Kill Yellow Folibus"),
			new KillObjective(12, new[] { MonsterId.Folibu_Yellow }));

		AddObjective("gatherWings", L("Gather translucent wings"),
			new CollectItemObjective(650270, 5));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 12));
		AddReward(new ItemReward(640007, 8));
		AddReward(new ItemReward(640012, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650270, character.Inventory.CountItem(650270), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650270, character.Inventory.CountItem(650270), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FBracken421Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1005);
		SetName(L("The Gosaru King"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Gosaru troupe to bring down the canopy King."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Jekabs"), "f_bracken_42_1");

		AddObjective("killTroupe", L("Thin Blue Gosarus"),
			new KillObjective(10, new[] { MonsterId.Gosaru_Blue }));

		AddObjective("killKing", L("Defeat the Gosaru King"),
			new KillObjective(1, new[] { MonsterId.Gosaru_Blue }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(36000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 9));
		AddReward(new ItemReward(640012, 3));
	}
}

public class FBracken421Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_42_1", 1006);
		SetName(L("Khonot Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Khonot - Gosarus, Doyors, Tanus."));
		SetLocation("f_bracken_42_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Peteris"), "f_bracken_42_1");

		AddObjective("killGosarus", L("Kill Blue Gosarus"),
			new KillObjective(12, new[] { MonsterId.Gosaru_Blue }));

		AddObjective("killDoyors", L("Kill Blue Doyors"),
			new KillObjective(12, new[] { MonsterId.Doyor_Blue }));

		AddObjective("killTanus", L("Kill Blue Tanus"),
			new KillObjective(12, new[] { MonsterId.Tanu_Blue }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(36000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 13));
		AddReward(new ItemReward(640007, 9));
	}
}
