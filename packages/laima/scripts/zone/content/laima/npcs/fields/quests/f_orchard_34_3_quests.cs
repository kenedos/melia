//--- Melia Script ----------------------------------------------------------
// Barha Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The three researchers building a neutralizer for what was spilled one ridge
// over in Alemeth, and the fourth one lying in the old sanctuary.
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

public class FOrchard343QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Barha Herb
		//---------------------------------------------------------------------
		AddNpc(20157, L("[Researcher] Aidas"), "f_orchard_34_3", -748, 425, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1001);

			dialog.SetTitle(L("Aidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's crouched at the edge of a stripped herb bed, turning a bare stem over in his fingers like it personally offended him*{/}"));
				await dialog.Msg(L("A traveler, out here? Marvelous. You've found the right disaster to wander into. Three of us came out to Barha to build a neutralizer for what got spilled in Alemeth. Six months ago. We're still stuck on the first ingredient. The FIRST one."));
				await dialog.Msg(L("Barha Herb's the base, and the Orange Siaulav Archers strip the beds faster than we can cut. Kill 20 of them, bring me 10 bundles out of their stores. Simple, in theory."));

				var response = await dialog.Select(L("Will you get the herb, or is that too much to ask of this forest too?"),
					Option(L("I'll hunt the archers and bring 10 bundles"), "help"),
					Option(L("Six months on one ingredient?"), "info"),
					Option(L("Buy the herb elsewhere"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They shoot from the bed edges and retreat into the middle of the patch — which is where their stores are, conveniently. Follow them in, don't wait for a better opening. There isn't one."));
						break;

					case "info":
						await dialog.Msg(L("Six months on an ingredient we can only harvest in a fortnight-long window, in a forest where everything else wants it too. Yes. That about sums up my year."));
						await dialog.Msg(L("Sirea told the academy it'd take a year. The academy funded six months. We're working the difference out of our own pockets — and out of Sarma, apparently."));
						break;

					case "leave":
						await dialog.Msg(L("Barha Herb grows in Barha. That's the entire reason the word 'Barha' is in its name. Elsewhere isn't an option, believe me, I've checked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("collectHerb", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He weighs the bundles on a field balance and writes the figure down twice*{/}"));
					await dialog.Msg(L("Ten bundles is a full base. First time in six months we've had a full anything."));
					await dialog.Msg(L("Take the equipment budget. We've no equipment left to buy — just a list of things we can't make, growing longer by the week."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty of archers down and my balance is still empty. The stores are in the middle of the beds, not on the bodies. Obviously."));
				}
				else
				{
					await dialog.Msg(L("Still Siaulav on the herb beds. They won't leave one while there's a stem still standing in it — stubborn as the forest itself."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Base is made and settling. Vide says it smells right, and Vide's been wrong about that exactly once in four years. So — cautiously, I believe her."));
			}
		});

		// Quest 1002: Griba's Slimy Juice
		//---------------------------------------------------------------------
		AddNpc(147485, L("[Researcher] Vide"), "f_orchard_34_3", 518, 371, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1002);

			dialog.SetTitle(L("Vide"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She glances up from a stained workbench, still holding a flask at arm's length, and quickly sets it down out of sight*{/}"));
				await dialog.Msg(L("Oh — don't mention this flask to Aidas, would you? Our little secret. Since you're here, though: a neutralizer has to bind to the thing it's neutralizing, and the only substance in this forest that binds to the Alemeth solution is the juice off a Big Red Griba. Isn't that deliciously inconvenient?"));
				await dialog.Msg(L("I found that out entirely by accident — by spilling a sample on one, if you must know. Bring me 10 lots of the juice before Aidas thinks to ask how I found out."));

				var response = await dialog.Select(L("Will you collect the juice? Quietly, ideally."),
					Option(L("I'll bring you 10 lots of juice"), "help"),
					Option(L("You spilled a sample?"), "info"),
					Option(L("Tell Aidas yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It comes off the cap, not the stalk, and it goes hard in about an hour. Bring it sealed, or you'll bring me a lump — and then I'll have to explain the lump."));
						break;

					case "info":
						await dialog.Msg(L("I dropped a flask. The Griba went from red to grey in four seconds flat and stopped being a Griba, and I just... stood there and watched. Then I went and got another flask, obviously."));
						await dialog.Msg(L("Most useful thing anyone on this team has done in six months, and I got there by being clumsy. I've made peace with about half of that. The other half is delightful."));
						break;

					case "leave":
						await dialog.Msg(L("I will tell him, eventually. I'd like the results in hand first, though — Aidas is a good man who turns into a very bad one the moment procedure's involved."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectJuice", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She tips one jar into another and the two liquids refuse to mix at all*{/}"));
					await dialog.Msg(L("Ten, all still liquid! That's the binder, and there's enough for three attempts — two more than I actually expected."));
					await dialog.Msg(L("Take my share of the stipend. I've been sleeping in a tent for six months. Nothing to spend it on out here but more tent."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. The big ones on the eastern shelf carry the most — the small ones are barely worth the walk, trust me."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Told Aidas. He asked three questions about procedure and then said 'well done' in a voice like a man passing a kidney stone. Worth it, honestly."));
			}
		});

		// Quest 1003: Rafflesia on the Path
		//---------------------------------------------------------------------
		AddNpc(152064, L("[Researcher] Sirea"), "f_orchard_34_3", -206, -153, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1003);

			dialog.SetTitle(L("Sirea"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's folding a letter back into its envelope with the sharp motions of someone who already knows the answer will be no*{/}"));
				await dialog.Msg(L("You're not with the academy, are you. Good — then you can actually be useful. I lead this team, and I've spent six months writing to an academy that funds half of what it approves. That's not the immediate problem, though."));
				await dialog.Msg(L("The immediate problem is Green Rafflesia on the path between camp and the sanctuary, and Sarma is in the sanctuary. Kill 30 of them. I want to reach her twice a day, not once."));

				var response = await dialog.Select(L("Will you clear the sanctuary path? I don't have time to ask twice."),
					Option(L("I'll kill the Green Rafflesia"), "help"),
					Option(L("What happened to Sarma?"), "info"),
					Option(L("Move her to the camp"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They spray before they close, so the first thing you'll know about one is the smell. If you can smell it, you're already inside its reach. Move fast."));
						break;

					case "info":
						await dialog.Msg(L("Rafflesia sap, three weeks ago, on that path — carrying a crate she should have let one of us carry. She hasn't been able to stand since Tuesday."));
						await dialog.Msg(L("She spilled the Alemeth solution in spring and has been trying to outwork it ever since. This is what outworking it looks like. I won't sugarcoat it."));
						break;

					case "leave":
						await dialog.Msg(L("The sanctuary is stone, cool, and has a roof. The camp is three tents in a forest that grew forty feet in a summer. She stays exactly where she is."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRafflesia", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("I walked it twice today. Two visits — and the second one, she was awake for."));
					await dialog.Msg(L("Take the team's contingency fund. There's no contingency left to have. It's just money in a box now."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Rafflesia on the path. They sit where the ground's wet — on that path, that's most of it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Twice a day, four days straight. She's started arguing with me about the dilution table again, which I am choosing to read as an improvement."));
			}
		});

		// Quest 1004: The Barha Antidote
		//---------------------------------------------------------------------
		AddNpc(152064, L("[Researcher] Sirea"), "f_orchard_34_3", 1179, 480, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1004);

			dialog.SetTitle(L("Sirea"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She meets you at the sanctuary door before you can knock, already pulling it shut behind her*{/}"));
				await dialog.Msg(L("Good, the path's walkable again — I felt the difference this morning. This is the sanctuary, and this is Sarma, and the ordinary Barha antidote has stopped touching her. Three weeks of sap is past what the ordinary recipe was written for."));
				await dialog.Msg(L("Here's the recipe list for the enhanced one. Bring me 8 lots of Sticky Rafflesia Sap and 6 Neutralizer Catalysts off the Gray Winged Frogs. I can make it tonight, if you move quickly."));

				var response = await dialog.Select(L("Will you gather both? I need them today, not eventually."),
					Option(L("I'll bring the sap and the catalysts"), "help"),
					Option(L("Sap cures sap?"), "info"),
					Option(L("Send for a physician"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(661138, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Sap first, catalyst second — do not carry them in the same bag. The catalyst will start working on the sap early, and you'll bring me warm water instead of medicine."));
						break;

					case "info":
						await dialog.Msg(L("The sap is what's in her. The enhanced antidote uses more of it, not less — you teach the body the shape of the thing by giving it a shape it can survive."));
						await dialog.Msg(L("Same principle as the neutralizer, at a smaller scale, on a person instead of a forest. Sarma worked it out. From a cot. On Tuesday. Don't ask me how."));
						break;

					case "leave":
						await dialog.Msg(L("The nearest physician is five days away and has never seen Rafflesia sap in his life. I've seen it three times this year alone. We don't have five days."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectSap", out var sapObj)) return;
				if (!quest.TryGetProgress("collectCatalyst", out var catObj)) return;

				if (sapObj.Done && catObj.Done)
				{
					await dialog.Msg(L("{#666666}*She mixes them on the altar stone because it's the only flat surface in the building*{/}"));
					await dialog.Msg(L("It's gone the color the list says it should. Six months of nothing going the color it should, and it's the one recipe I didn't write."));
					await dialog.Msg(L("Take everything in the sanctuary box. It's offerings from people who walked out here for a roof, and I don't think any of them would mind."));

					character.Quests.Complete(questId);
				}
				else if (sapObj.Done)
				{
					await dialog.Msg(L("Sap's in. Still need catalyst — the frogs carry it, in the wet ground west of here."));
				}
				else
				{
					await dialog.Msg(L("Still short of sap. Take it off the Rafflesia bodies before it sets — it sets fast in open air."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("She sat up this morning and asked for the flask count. I gave her the flask count. She said it was wrong. She was right. Of course she was."));
			}
		});

		// Quest 1005: The Overgrowth Solution Neutralizer
		//---------------------------------------------------------------------
		AddNpc(147486, L("[Assistant] Gatre"), "f_orchard_34_3", -453, -463, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1005);

			dialog.SetTitle(L("Gatre"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_orchard_34_3", 1004)))
				{
					await dialog.Msg(L("Sarma has to be able to sit up before we mix anything, please. Get Sirea what she needs for the enhanced antidote first, and — then come back to me, I promise it'll be worth the wait."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's grinning at nothing in particular, still dusty from the ridge crossing, a corked flask cradled like it might break*{/}"));
				await dialog.Msg(L("Oh — you made it over too! Good, good. I carried six flasks of the original batch over the ridge, and Sarma checked my arithmetic from a cot, which is a sentence I intend to repeat for the rest of my life, honestly."));
				await dialog.Msg(L("The neutralizer's mixed, and it has to be poured at the old sanctuary well — it feeds both valleys. The Big Red Griba have taken the well. Kill 25 of them, and — I should say — something older is going to come out of the well after them. Sorry, again, I should lead with these things."));

				var response = await dialog.Select(L("Will you take the well? Please — we're so close."),
					Option(L("I'll clear the well and hold it"), "help"),
					Option(L("Something older?"), "info"),
					Option(L("Pour it in the stream instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Clear the Griba first, keep your back to the well head. Whatever comes up will come up behind you if you're standing anywhere else."));
						break;

					case "info":
						await dialog.Msg(L("Kurmis. It's been in that well since before the sanctuary was roofed, and it's never once come out — never had a reason to, until now."));
						await dialog.Msg(L("Sarma's solution has been in the groundwater for six months. That's six months of reason, apparently."));
						break;

					case "leave":
						await dialog.Msg(L("The stream is one valley. The well is the water table, and the water table is both — and Auste's mother is eighty-one and lives on the other one. So, no, I don't think we skip this."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearWell", out var wellObj)) return;
				if (!quest.TryGetProgress("holdTheWell", out var bossObj)) return;

				if (wellObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("Poured, all six flasks — the well ran grey, then ran clear! Vide's downstream taking readings and shouting numbers back up the path."));
					await dialog.Msg(L("Take this. It came up out of the well with the Kurmis, and none of the four of us wants it anywhere near the camp."));

					character.Quests.Complete(questId);
				}
				else if (wellObj.Done)
				{
					await dialog.Msg(L("Well head is clear. Stand on it. It's already coming — I can feel it, don't ask me how."));
				}
				else
				{
					await dialog.Msg(L("Too many Griba on the well. We can't get a flask within 20 paces of the head."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Alemeth's growth is four inches down on the week. Four inches isn't much. Four inches is the first number that's gone the right way in a year, and I intend to celebrate every single one of them."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Barha Herb
//-----------------------------------------------------------------------------

public class BarhaHerbQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1001);
		SetName(L("Barha Herb"));
		SetType(QuestType.Sub);
		SetDescription(L("Barha Herb is the base of the neutralizer and it can only be harvested in a fortnight-long window, in a forest where the Orange Siaulav Archers strip the beds faster than three researchers can cut."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Aidas"), "f_orchard_34_3");

		AddObjective("killArchers", L("Kill Orange Siaulav Archers on the herb beds"),
			new KillObjective(20, new[] { MonsterId.Siaulav_Bow_Orange }));

		AddObjective("collectHerb", L("Collect bundles of Barha Herb"),
			new CollectItemObjective(661135, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661135, 0.50f, MonsterId.Siaulav_Bow_Orange);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661135, character.Inventory.CountItem(661135), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661135, character.Inventory.CountItem(661135), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Griba's Slimy Juice
//-----------------------------------------------------------------------------

public class GribasSlimyJuiceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1002);
		SetName(L("Griba's Slimy Juice"));
		SetType(QuestType.Sub);
		SetDescription(L("A neutralizer has to bind to the thing it neutralizes, and the only substance in Barha Forest that binds to the Alemeth solution is the juice off a Big Red Griba's cap."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Vide"), "f_orchard_34_3");

		AddObjective("collectJuice", L("Collect Griba's Slimy Juice from Big Red Griba"),
			new CollectItemObjective(661136, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661136, 0.50f, MonsterId.Mushroom_Ent_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661136, character.Inventory.CountItem(661136), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661136, character.Inventory.CountItem(661136), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Rafflesia on the Path
//-----------------------------------------------------------------------------

public class RafflesiaOnThePathQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1003);
		SetName(L("Rafflesia on the Path"));
		SetType(QuestType.Sub);
		SetDescription(L("Green Rafflesia have taken the path between the research camp and the old sanctuary, and the team leader can only make the walk to her poisoned colleague once a day instead of twice."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Sirea"), "f_orchard_34_3");

		AddObjective("killRafflesia", L("Kill Green Rafflesia on the sanctuary path"),
			new KillObjective(30, new[] { MonsterId.Rafflesia_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: The Barha Antidote
//-----------------------------------------------------------------------------

public class TheBarhaAntidoteQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1004);
		SetName(L("The Barha Antidote"));
		SetType(QuestType.Sub);
		SetDescription(L("Three weeks of Rafflesia sap is past what the ordinary Barha antidote was written for. The enhanced recipe teaches the body the shape of the thing by giving it a shape it can survive."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Researcher] Sirea"), "f_orchard_34_3");

		AddObjective("collectSap", L("Collect Sticky Rafflesia Sap"),
			new CollectItemObjective(661141, 8));

		AddObjective("collectCatalyst", L("Collect Neutralizer Catalysts from Gray Winged Frogs"),
			new CollectItemObjective(661139, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661141, 0.50f, MonsterId.Rafflesia_Green);
		AddDrop(661139, 0.45f, MonsterId.Flying_Flog_White);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661138, character.Inventory.CountItem(661138), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661141, character.Inventory.CountItem(661141), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661139, character.Inventory.CountItem(661139), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661138, character.Inventory.CountItem(661138), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661141, character.Inventory.CountItem(661141), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661139, character.Inventory.CountItem(661139), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Overgrowth Solution Neutralizer
//-----------------------------------------------------------------------------

public class TheOvergrowthSolutionNeutralizerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1005);
		SetName(L("The Overgrowth Solution Neutralizer"));
		SetType(QuestType.Sub);
		SetDescription(L("The neutralizer has to go into the old sanctuary well, which feeds both valleys. Big Red Griba have taken the well head, and the Kurmis that has sat in the water since before the sanctuary was roofed now has a reason to come out."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Assistant] Gatre"), "f_orchard_34_3");

		AddPrerequisite(new CompletedPrerequisite("f_orchard_34_3", 1004));

		AddObjective("clearWell", L("Kill Big Red Griba at the sanctuary well"),
			new KillObjective(25, new[] { MonsterId.Mushroom_Ent_Red }));

		AddObjective("holdTheWell", L("Hold the well head while the neutralizer is poured"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Boss_Kurmis, 1),
					new KillSpec(MonsterId.Rafflesia_Green, 2, BuffId.EliteMonsterBuff),
				},
				resetIdent: "clearWell",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583115, 1)); // Rupesciu Necklace
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}
}
