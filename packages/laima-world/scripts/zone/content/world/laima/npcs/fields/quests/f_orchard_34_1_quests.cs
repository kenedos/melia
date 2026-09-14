//--- Melia Script ----------------------------------------------------------
// Alemeth Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Where the researcher Sarma tested an overgrowth solution on an orchard and
// it worked considerably better than anyone had planned for.
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

public class FOrchard341QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Lost Research Notes
		//---------------------------------------------------------------------
		AddNpc(147486, L("[Assistant] Gatre"), "f_orchard_34_1", -520, -1364, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1001);

			dialog.SetTitle(L("Gatre"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's kneeling in the wreck of a cold-frame, muttering to a stack of half-rotted paper like it might apologize back*{/}"));
				await dialog.Msg(L("Oh — you're not from the valley, are you? Good, maybe you haven't heard the story yet. I was Sarma's assistant. Four years! She tested an overgrowth solution on three rows of this orchard in the spring, and by midsummer the whole orchard was a forest, and by autumn she was just... gone."));
				await dialog.Msg(L("Her notes went with the growth — three chapters, and of course, of course something is nesting on every single one. One's in a Green Eldigo nest, one's in a Red Truffle nest, one's in a Green Corpse Flower nest. I need all three back, or none of this adds up to anything."));

				var response = await dialog.Select(L("Will you find the three chapters for me? Please — I mean, if you would."),
					Option(L("I'll bring back all 3 chapters"), "help"),
					Option(L("Gone where?"), "info"),
					Option(L("Write it up from memory"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Right, yes — they line their nests with paper, so a chapter will be shredded but readable. Bring me whatever comes out of the nest, please, not just the sheets that look nice. I need all of it."));
						break;

					case "info":
						await dialog.Msg(L("Barha Forest, one ridge over — with three researchers who did not, I should say, test anything on anything. She's building a neutralizer. She isn't answering letters. I've sent four."));
						await dialog.Msg(L("I'm not angry with her! I checked her arithmetic myself, four times, and I signed the sheet — and she's been carrying that alone for six months because of it. That's the part that keeps me up."));
						break;

					case "leave":
						await dialog.Msg(L("I've written it from memory three times and gotten three different formulas. Three! That is precisely the problem the notes exist to solve. So — please reconsider?"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("findChapter1", out var c1)) return;
				if (!quest.TryGetProgress("findChapter2", out var c2)) return;
				if (!quest.TryGetProgress("findChapter3", out var c3)) return;

				if (c1.Done && c2.Done && c3.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the three chapters out and reads all the way to the end without once looking up, lips moving along with the words*{/}"));
					await dialog.Msg(L("Chapter two has the dilution table. She was right, I was wrong, and the mistake — the mistake is right there on the sheet I signed, in my own hand."));
					await dialog.Msg(L("Take the field purse, please. I'm going to Barha to tell her that in person, and I— I won't be needing return fare."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still a chapter missing, isn't it. The Eldigo, the Truffles, the Corpse Flowers — one nest each, no shortcuts. Believe me, I already tried every shortcut."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("She wrote back! Four words — two of them were 'come here' — and I've read it more times than I ever read the notes, heh."));
			}
		});

		// Quest 1002: Overgrown Plant Leaves
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Fruit-Grower] Auste"), "f_orchard_34_1", -1510, 358, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1002);

			dialog.SetTitle(L("Auste"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's standing at the fence line glaring up at the canopy like it owes her money*{/}"));
				await dialog.Msg(L("Oh — wasn't expecting company. My family's had sixty rows of pear in this valley for four generations. Since spring I've had sixty rows of something I'd need a machete, a ladder, and a stronger constitution to describe."));
				await dialog.Msg(L("The Green Corpse Flowers throw leaves three times too big. Bring me twelve of those, and if I wave them at a Kingdom assessor, maybe — maybe — I finally get this valley surveyed as whatever it's actually become."));

				var response = await dialog.Select(L("You gathering, or are you just here to admire my ruined orchard too?"),
					Option(L("I'll bring you 12 leaves"), "help"),
					Option(L("Surveyed for what?"), "info"),
					Option(L("Cut it back yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them whole, stem-knuckle and all. A leaf without the knuckle's just a big leaf, and some clerk will decide I grew it that way out of spite."));
						break;

					case "info":
						await dialog.Msg(L("Compensation, obviously. My orchard's still legally an orchard, so I get taxed on sixty rows of pear I can't reach, pick, or sell. Wonderful system, really."));
						await dialog.Msg(L("I don't want the researchers strung up for it. I want one clerk in Fedimian to write the word 'forest' on a form and be done with it."));
						break;

					case "leave":
						await dialog.Msg(L("I cut back four rows myself in June. They were taller in August than when I started. So — no, please, be my guest, I'll watch."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectLeaves", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She holds one up against her own arm span — it's wider*{/}"));
					await dialog.Msg(L("Twelve, all knuckled. Let's see an assessor call that an orchard with a straight face."));
					await dialog.Msg(L("Take the harvest money. There's no harvest, so it isn't doing anything useful except mocking me from a drawer."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. They throw the biggest leaves where the old rows were — up the western slope, if you want the good ones."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Form came back stamped. Forest. Four generations of pear, one word from a clerk, and I felt absolutely nothing about it — until this morning, apparently. Thanks for that too."));
			}
		});

		// Quest 1003: Corpse Flowers in the Rows
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Fruit-Grower] Auste"), "f_orchard_34_1", -909, 346, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1003);

			dialog.SetTitle(L("Auste"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's dragging a bent fence post back toward the yard wall, out of breath before she's even set it down*{/}"));
				await dialog.Msg(L("Back again? Good timing, then. Form's stamped, valley's officially a forest, and I'm still living in the house at the bottom of it with my mother, who is eighty-one and has opinions."));
				await dialog.Msg(L("The Green Corpse Flowers have come down as far as the yard wall. Kill thirty of them. I've stopped caring about the survey and started caring about the wall, funnily enough."));

				var response = await dialog.Select(L("Will you clear the yard rows?"),
					Option(L("I'll kill the Green Corpse Flowers"), "help"),
					Option(L("Will she not leave?"), "info"),
					Option(L("Move the house"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They root when they're still. Catch them moving between rows or you'll be hacking at something that's already gripped in — good luck with that."));
						break;

					case "info":
						await dialog.Msg(L("She planted rows thirty through forty-four with her own two hands, in a year I wasn't born in yet. Says she'll leave when the wall comes down. I believe her completely, which is the annoying part."));
						await dialog.Msg(L("So the wall doesn't come down. That's the whole plan, yes, and I am aware it isn't much of one."));
						break;

					case "leave":
						await dialog.Msg(L("Move it where, exactly, and with what money? The house is stone and the money went on a survey form, remember?"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlowers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Wall's clear from the gate to the corner. Mother sat out in the yard this afternoon for the first time since June."));
					await dialog.Msg(L("Take this. It's the last of the pear money, and for once it's going somewhere useful instead of a drawer."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Corpse Flowers on the wall side. They come down the old cart lane — easiest ground, of course."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("She's started talking about which rows she'd replant, if anyone ever actually fixed this. Haven't told her nobody is. Yet."));
			}
		});

		// Quest 1004: Anesthetic Powder
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Apothecary] Jonelis"), "f_orchard_34_1", 167, -1347, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1004);

			dialog.SetTitle(L("Jonelis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's grinding something dark in a mortar, sleeves rolled to the elbow, three empty vials lined up and waiting*{/}"));
				await dialog.Msg(L("You'll do. I need hands that aren't mine. Everything in this valley's grown wrong except the Green Eldigo — grown wrong in a useful direction, for once. Their blood thins a fever better than anything in my cabinet."));
				await dialog.Msg(L("Take this anesthetic powder. Kill twenty of them, draw eight lots of blood. Powder first — a frightened Eldigo's blood clots to nothing, and I don't have time to explain that twice."));

				var response = await dialog.Select(L("Well? Will you draw the blood or not?"),
					Option(L("I'll hunt them and draw 8 lots"), "help"),
					Option(L("Grown wrong usefully?"), "info"),
					Option(L("Use something in your cabinet"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(661088, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Powder first, work second. Draw before it settles and you've hauled a rock back to me instead of medicine. Don't waste my time on it."));
						break;

					case "info":
						await dialog.Msg(L("Solution went into the water, everything drank it. Most of what it touched got bigger and stupider. The Eldigo got bigger and their blood got stronger. Lucky, for once."));
						await dialog.Msg(L("I've got fourteen people in this valley with the summer fever and three doses to my name. I'm not going to get precious about where a cure comes from."));
						break;

					case "leave":
						await dialog.Msg(L("My cabinet is three doses and a jar of dried nettle. Trust me, I'd love to use something in my cabinet."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killEldigo", out var killObj)) return;
				if (!quest.TryGetProgress("drawBlood", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He tips a flask, watches how slowly it runs down the glass, nods once*{/}"));
					await dialog.Msg(L("Clean. All eight. That's forty doses for fourteen people — first time all year I've had too much of something."));
					await dialog.Msg(L("Take the dispensary money. It's for stock I can't buy, and it's been sitting there mocking me since May. Go on."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty of Eldigo down, not enough flasks full. Powder, draw, don't rush the settling. Simple."));
				}
				else
				{
					await dialog.Msg(L("Still Eldigo in the southern draw. Keep to the wet ground under the overgrowth — that's where they nest."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Eleven of the fourteen are up and walking. The other three were never going to be, and I knew that in May. Gave them a dose anyway. Wouldn't change that."));
			}
		});

		// Quest 1005: Sarma's Experiment Solution
		//---------------------------------------------------------------------
		AddNpc(147486, L("[Assistant] Gatre"), "f_orchard_34_1", -1236, -1650, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1005);

			dialog.SetTitle(L("Gatre"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_orchard_34_1", 1001)))
				{
					await dialog.Msg(L("Get me the three chapters first, please. Everything after that depends on the dilution table and I am NOT — I am not guessing at it twice."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's rolling the recovered chapters into a satchel, already dressed for a ridge crossing*{/}"));
				await dialog.Msg(L("Oh — good, you're back! Chapter two says the solution keeps its strength in a living body. The Red Truffles drank the spill in spring and they're still carrying it, undiluted, six months on. Six months!"));
				await dialog.Msg(L("Bring me six lots of Sarma's solution back out of them. They won't enjoy it, and — I should say this now — the biggest ones will come for you all at once once they realize what you're doing. Sorry, I should have led with that."));

				var response = await dialog.Select(L("Will you go and draw it out? Please say yes."),
					Option(L("I'll take the solution back out of them"), "help"),
					Option(L("Why not just make more?"), "info"),
					Option(L("Let it dilute on its own"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work the ordinary ones first, keep your back clear. When the big ones come, they come together, and they come from behind — chapter three says so, so it's reliable, at least."));
						break;

					case "info":
						await dialog.Msg(L("Because the neutralizer has to be built against the exact batch that spilled, and the exact batch is in those Truffles. A fresh mix would just be a different poison entirely."));
						await dialog.Msg(L("Sarma knows that. It's why she hasn't come back — she can't build the answer in Barha without a sample from here, and she won't ask me for one. So I'm bringing it to her instead."));
						break;

					case "leave":
						await dialog.Msg(L("Chapter three has the decay figure. Ninety years. Auste's mother is eighty-one. So — no, I don't think we let it dilute on its own, actually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("drawSolution", out var solObj)) return;
				if (!quest.TryGetProgress("theBigOnes", out var bigObj)) return;

				if (solObj.Done && bigObj.Done)
				{
					await dialog.Msg(L("Six flasks of the original batch, every one sealed. That's the neutralizer made possible — possible in Barha, not here."));
					await dialog.Msg(L("Take this — it was Sarma's, she left it on the bench. I'm carrying the flasks over the ridge tonight, and I'm not carrying anything else of hers. Not yet, anyway."));

					character.Quests.Complete(questId);
				}
				else if (solObj.Done)
				{
					await dialog.Msg(L("You've got the flasks! Now finish whatever came after you for them, quickly."));
				}
				else
				{
					await dialog.Msg(L("Still short of solution. The oldest Truffles hold the most — they drank first, naturally."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I go over the ridge in the morning. Auste asked me to tell the researchers about her mother's rows, and I said I would, and — I am going to. I promise."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Lost Research Notes
//-----------------------------------------------------------------------------

public class TheLostResearchNotesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1001);
		SetName(L("The Lost Research Notes"));
		SetType(QuestType.Sub);
		SetDescription(L("An overgrowth solution turned 60 rows of pear into a forest in one summer, and the notes that explain it went out into the growth with everything else. Three chapters, three nests."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Assistant] Gatre"), "f_orchard_34_1");

		AddObjective("findChapter1", L("Recover Lost Research Note, Chapter 1 from Green Eldigo"),
			new CollectItemObjective(661090, 1));

		AddObjective("findChapter2", L("Recover Lost Research Note, Chapter 2 from Red Truffle"),
			new CollectItemObjective(661091, 1));

		AddObjective("findChapter3", L("Recover Lost Research Note, Chapter 3 from Green Corpse Flower"),
			new CollectItemObjective(661092, 1));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661090, 0.25f, MonsterId.Eldigo_Green);
		AddDrop(661091, 0.25f, MonsterId.Truffle_Red);
		AddDrop(661092, 0.25f, MonsterId.Corpse_Flower_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661090, character.Inventory.CountItem(661090), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661091, character.Inventory.CountItem(661091), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661092, character.Inventory.CountItem(661092), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661090, character.Inventory.CountItem(661090), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661091, character.Inventory.CountItem(661091), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661092, character.Inventory.CountItem(661092), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Overgrown Plant Leaves
//-----------------------------------------------------------------------------

public class OvergrownPlantLeavesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1002);
		SetName(L("Overgrown Plant Leaves"));
		SetType(QuestType.Sub);
		SetDescription(L("The valley is legally still an orchard, so its grower is taxed on 60 rows of pear she cannot reach. She needs leaves big enough that a Kingdom assessor will write the word 'forest' on a form."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fruit-Grower] Auste"), "f_orchard_34_1");

		AddObjective("collectLeaves", L("Collect Overgrown Plant Leaves from Green Corpse Flowers"),
			new CollectItemObjective(661100, 12));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661100, 0.50f, MonsterId.Corpse_Flower_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661100, character.Inventory.CountItem(661100), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661100, character.Inventory.CountItem(661100), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Corpse Flowers in the Rows
//-----------------------------------------------------------------------------

public class CorpseFlowersInTheRowsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1003);
		SetName(L("Corpse Flowers in the Rows"));
		SetType(QuestType.Sub);
		SetDescription(L("The overgrowth has come down the old cart lane as far as the farmhouse yard wall, and the grower's mother has said she will leave when the wall comes down."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fruit-Grower] Auste"), "f_orchard_34_1");

		AddObjective("killFlowers", L("Kill Green Corpse Flowers at the yard wall"),
			new KillObjective(30, new[] { MonsterId.Corpse_Flower_Green }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Anesthetic Powder
//-----------------------------------------------------------------------------

public class AnestheticPowderQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1004);
		SetName(L("Anesthetic Powder"));
		SetType(QuestType.Sub);
		SetDescription(L("The solution went into the water and everything drank it. Most of what it touched grew bigger and stupider; the Green Eldigo's blood grew strong enough to break a summer fever."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Apothecary] Jonelis"), "f_orchard_34_1");

		AddObjective("killEldigo", L("Kill Green Eldigo in the southern draw"),
			new KillObjective(20, new[] { MonsterId.Eldigo_Green }));

		AddObjective("drawBlood", L("Draw Green Eldigo Blood"),
			new CollectItemObjective(661089, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661089, 0.50f, MonsterId.Eldigo_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661088, character.Inventory.CountItem(661088), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661089, character.Inventory.CountItem(661089), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661088, character.Inventory.CountItem(661088), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(661089, character.Inventory.CountItem(661089), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: Sarma's Experiment Solution
//-----------------------------------------------------------------------------

public class SarmasExperimentSolutionQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1005);
		SetName(L("Sarma's Experiment Solution"));
		SetType(QuestType.Sub);
		SetDescription(L("The neutralizer has to be built against the exact batch that was spilled, and the exact batch is still undiluted inside the Red Truffles that drank it in the spring."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Assistant] Gatre"), "f_orchard_34_1");

		AddPrerequisite(new CompletedPrerequisite("f_orchard_34_1", 1001));

		AddObjective("drawSolution", L("Draw Sarma's Experiment Solution from Red Truffles"),
			new CollectItemObjective(661097, 6));

		AddObjective("theBigOnes", L("Survive what the oldest Truffles send after you"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Truffle_Red, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Corpse_Flower_Green, 3),
				},
				resetIdent: "drawSolution",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603112, 1)); // Stipiria Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(661097, 0.45f, MonsterId.Truffle_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661097, character.Inventory.CountItem(661097), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661097, character.Inventory.CountItem(661097), InventoryItemRemoveMsg.Destroyed);
	}
}
