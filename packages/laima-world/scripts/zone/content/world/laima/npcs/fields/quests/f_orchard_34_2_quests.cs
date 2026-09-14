//--- Melia Script ----------------------------------------------------------
// Zeraha Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Ferret carrying country, and the scholar who intends to find out what
// changed here two years ago by walking among them as one of them.
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

public class FOrchard342QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Drowsy Herb
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Herb-Scholar] Ausrine"), "f_orchard_34_2", 1011, 82, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1001);

			dialog.SetTitle(L("Ausrine"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched over a notebook balanced on her knee, sniffing at a crushed leaf and muttering the same word three times*{/}"));
				await dialog.Msg(L("Don't mind me, I talk to myself when I'm working — it's the only company that doesn't interrupt. Actually! Stay a moment, you might be useful. The Ferrets of Zeraha have hauled drowsy herb out of these woods since before anyone kept records. Burn it in camp, keeps them calm as anything."));
				await dialog.Msg(L("Two years ago they just... stopped burning it. Started stockpiling instead! Kill 20 Ferret Loaders, bring me 10 bales — I want to know exactly what a Ferret smells like after two years of not sleeping properly. Fascinating, don't you think?"));

				var response = await dialog.Select(L("Will you take the herb off the Loaders? Say yes, I'm dying to know."),
					Option(L("I'll hunt the Loaders and bring 10 bales"), "help"),
					Option(L("Stockpiling it for what?"), "info"),
					Option(L("Ask them for a bale"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("A Loader under a full bale can't turn quickly — take them from the side and the bale comes off intact, which is the only way I can use it. Wonderful, go on!"));
						break;

					case "info":
						await dialog.Msg(L("That is the entire question, isn't it! They're carrying it somewhere, in quantity, and none of it's being burned in a camp anymore."));
						await dialog.Msg(L("A trail-warden two valleys west told me the exact same thing about resin. Stopped trading, started hauling. Same two years, same direction of travel. I could just scream."));
						break;

					case "leave":
						await dialog.Msg(L("Oh, I have. Stood on the ridge three days with salt and an open hand, and forty Ferrets walked past without turning their heads. Not one! Extraordinary, really, in the worst way."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLoaders", out var killObj)) return;
				if (!quest.TryGetProgress("collectHerb", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She cuts a bale open and buries her face in it without any dignity at all*{/}"));
					await dialog.Msg(L("Fresh cut, this week, never near a fire! They're harvesting it faster than ever and using none of it. Isn't that just— sorry. Delightful data. Terrible situation."));
					await dialog.Msg(L("Take the field grant. The academy gave it to me for porters and I haven't been able to hire one within three valleys, so it's just sitting there being useless, like most academy money."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty down, not enough bales. Half of them carry crates, not herb — go for the wide loads, obviously."));
				}
				else
				{
					await dialog.Msg(L("Still Loaders on the haul road. They move in a line and don't break it for anything, stubborn creatures."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ten bales, and I've burned one myself. Works exactly as described! So whatever they're doing, it isn't because the herb stopped working. Which raises more questions than it answers, frankly."));
			}
		});

		// Quest 1002: The Slingers on the Ridge
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Caravan Master] Rudenis"), "f_orchard_34_2", 1284, 1118, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1002);

			dialog.SetTitle(L("Rudenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pacing beside a stalled cart, counting crates off a ledger and swearing under his breath every third one*{/}"));
				await dialog.Msg(L("You look like someone who can hold a weapon. Good, come here. Six carts a month through Zeraha, twenty-two years, never lost one. Last month? Lost two. Both on the ridge, where the road narrows, of course."));
				await dialog.Msg(L("Ferret Slingers up on the rock, dropping stones the size of a fist. Kill thirty of them and I can run a cart through without budgeting for a coffin."));

				var response = await dialog.Select(L("Well? Will you clear the ridge or not — I haven't got all day, and neither do my carts."),
					Option(L("I'll kill the Ferret Slingers"), "help"),
					Option(L("They never did this before?"), "info"),
					Option(L("Go round the ridge"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Get up on the rock with them. Down on the road you're a target; up there they're just small, angry, and out of stones fast."));
						break;

					case "info":
						await dialog.Msg(L("Twenty-two years on this road. They used to sit on that rock and watch us go by, and one of them always waved. I waved back for twenty of those years, like an idiot."));
						await dialog.Msg(L("Then two years ago the waving stopped, and last month the stones started. Somebody tell me those aren't related, because I can't sleep thinking they are."));
						break;

					case "leave":
						await dialog.Msg(L("Round trip's four days. My carts carry fruit. Four days turns a cart of fruit into a cart of compost with a driver's wages stapled to it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSlingers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Two carts through the narrows yesterday, nothing came off the rock. Drivers noticed. Drivers always notice — except when I'm right, apparently."));
					await dialog.Msg(L("Take the month's haulage. It's a lot of coin and, for once, I've got a month where I can actually spare it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Slingers on the rock. They'll hear the cart before they see it, so quiet won't save you either."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Drove the narrows myself Thursday. Looked up the whole way. Nothing waved. Didn't expect it to, but a man can be disappointed anyway."));
			}
		});

		// Quest 1003: Terrible Scent
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Caravan Master] Rudenis"), "f_orchard_34_2", -763, -666, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1003);

			dialog.SetTitle(L("Rudenis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He waves you over before you've even finished walking up, one hand still on an empty crate*{/}"));
				await dialog.Msg(L("You again, good — the ridge's clear, but now my crates are vanishing off the wagons. One, two a night, from camps I've got a watch posted on. A watch! Might as well post a scarecrow."));
				await dialog.Msg(L("The Ferret Empty Porters carry a scent gland every other Ferret in this forest gives a wide berth. Bring me 8 of them and I'll paint my crates with it myself."));

				var response = await dialog.Select(L("Will you get the scent?"),
					Option(L("I'll bring you 8 scent glands"), "help"),
					Option(L("Why do they avoid it?"), "info"),
					Option(L("Post more guards"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't open the gland out there, and don't put it in your pack next to food. I've made both mistakes on the same day, and I do not recommend either."));
						break;

					case "info":
						await dialog.Msg(L("An Empty Porter's one that's already delivered. Whatever the scent means to them, it means 'nothing here,' and no Ferret in Zeraha wastes a step on nothing."));
						await dialog.Msg(L("Twenty-two years and I never needed to know that. This year I've learned more about Ferrets than any sane cart-master should have to."));
						break;

					case "leave":
						await dialog.Msg(L("I've got three guards on a six-cart camp and the crates go anyway. They don't fight the guards. They wait. Almost respectful, if it weren't robbery."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectScent", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He takes the jar at arm's length and holds it there for the entire conversation*{/}"));
					await dialog.Msg(L("Eight. That's every crate on the northern run painted, and my drivers are going to hate me for a month straight."));
					await dialog.Msg(L("Take the northern run's insurance. Haven't had to claim it in twenty-two years — rather it went to you than some clerk in Fedimian."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. The Empty Porters come back down the haul road unladen — that's how you spot them, empty-handed and smug about it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six nights, no crates gone. Drivers ride upwind of their own wagons now, and I've stopped apologizing for making them."));
			}
		});

		// Quest 1004: The Ferret Cairns
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Herb-Scholar] Ausrine"), "f_orchard_34_2", -681, 1697, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1004);

			dialog.SetTitle(L("Ausrine"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She looks up from four sketches pinned to a board, tapping each one in turn like she's counting them again*{/}"));
				await dialog.Msg(L("Perfect timing, honestly! There are four Ferret cairns in this forest, each with a carved wooden piece at the base. I've drawn all four — same carving, every one. Isn't that marvelous?"));
				await dialog.Msg(L("Take this incense and burn it at each cairn. The smoke settles whatever's watching it long enough for you to lift the piece and look at the underside. Go on, go on, I'll be right here dying of curiosity."));

				var response = await dialog.Select(L("Will you visit the four cairns for me? Please say yes, I've been waiting weeks."),
					Option(L("I'll burn incense at all 4 cairns"), "help"),
					Option(L("What's on the underside?"), "info"),
					Option(L("Just take the pieces"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(667017, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Light it downwind and stand in the smoke yourself — it works on you too, and trust me, you'll want it working on you."));
						break;

					case "info":
						await dialog.Msg(L("I don't know! Every Ferret in the forest will fight over a cairn piece and none of them will turn one over — I've watched them not do it for an entire year."));
						await dialog.Msg(L("A thing nobody will look at the back of is a thing somebody was told not to look at the back of. That is not animal behavior, and it is driving me mad."));
						break;

					case "leave":
						await dialog.Msg(L("Lift a piece without the smoke and you'll have every Ferret within a mile on you inside a minute. I've watched that too — spectacular, dreadful, don't do it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("visitCairns", out var cairnObj)) return;

				if (cairnObj.Done)
				{
					await dialog.Msg(L("All four carvings the same on the face, all four different on the back — and the four backs together make a line of script I've seen exactly once before, in a demon-war archive in Fedimian!"));
					await dialog.Msg(L("Take the whole grant. I'm going to need a Ferret's skin, not a porter, and I'd rather you were paid before I explain why."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Cairns still unvisited. All four, and burn the incense before you touch anything — please, for both our sakes."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four pieces, one sentence, and it's an instruction! Somebody wrote an instruction into this forest, and the Ferrets have been carrying it around for two years. I haven't slept."));
			}
		});

		// Quest 1004 interaction points - the Ferret cairns
		//---------------------------------------------------------------------
		void AddFerretCairn(int cairnNumber, string observation, int x, int z, int direction)
		{
			AddNpc(47222, L("Ferret Cairn"), "f_orchard_34_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_2", 1004);
				var variableKey = $"Laima.Quests.f_orchard_34_2.Quest1004.Cairn{cairnNumber}";
				var counterKey = "Laima.Quests.f_orchard_34_2.Quest1004.CairnsVisited";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A stacked cairn with a carved wooden piece wedged at its base*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This piece is back where you found it, face up*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 450, 350);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The incense does not settle everything - {0} drawn in!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Burning incense and lifting the piece..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(5)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(667021, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var visited = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, visited);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Cairns read: {0}/4", visited));

					if (visited >= 4)
						character.ServerMessage(L("{#FFD700}All 4 undersides copied. Return to Ausrine.{/}"));
				}
				else
				{
					character.ServerMessage(L("The incense gutters out and you put the piece back."));
				}
			});
		}

		AddFerretCairn(1, L("First underside: three short marks and a long one, cut deep."), 403, 1532, 0);
		AddFerretCairn(2, L("Second underside: the same hand, and the cuts run the other way."), -85, 1559, 90);
		AddFerretCairn(3, L("Third underside: worn almost smooth, but the shape is unmistakable."), 1180, 1136, 180);
		AddFerretCairn(4, L("Fourth underside: fresh. Somebody cut this one within the year."), 742, -953, 270);

		// Quest 1005: The Ferret Transformation Scroll
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Herb-Scholar] Ausrine"), "f_orchard_34_2", 256, -738, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_2", 1005);

			dialog.SetTitle(L("Ausrine"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_orchard_34_2", 1004)))
				{
					await dialog.Msg(L("The four cairn pieces first, please. I am not writing a transformation scroll off half a sentence — that's how scholars end up embarrassed, or dead, and I'd rather be neither."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's holding up a half-sewn Ferret pelt against her own shoulders, checking the fit in a hand mirror*{/}"));
				await dialog.Msg(L("Don't laugh! I've thought this through more than it looks, I promise. The instruction on the cairns tells them where to carry everything. I can read where. I cannot read who's receiving it, and no Ferret is going to tell a human."));
				await dialog.Msg(L("So — I write a transformation scroll and walk in as one of them! I need six blank scroll leaves off the Searchers, and when the disguise fails — and it will — I'll need you standing right next to me."));

				var response = await dialog.Select(L("Will you go in with her?"),
					Option(L("I'll get the scrolls and stand with you"), "help"),
					Option(L("Why will it fail?"), "info"),
					Option(L("Send the pieces to Fedimian"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The Searchers carry blanks in a hip roll. When we go in, stay behind me until the scroll drops — after that, stay in front, please, I mean it."));
						break;

					case "info":
						await dialog.Msg(L("Because it's a scent-based people and I'm writing a shape-based scroll! I'll look correct and smell entirely wrong, and I'll have about four minutes."));
						await dialog.Msg(L("Four minutes is enough to see who's at the end of the haul road. It is not enough to walk back out. I've done the math three times hoping it changes."));
						break;

					case "leave":
						await dialog.Msg(L("Fedimian will read it in three months and send a commission in six. The haul road is two years old and accelerating. We don't have three months, let alone six."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectBlanks", out var blankObj)) return;
				if (!quest.TryGetProgress("theFourMinutes", out var fightObj)) return;

				if (blankObj.Done && fightObj.Done)
				{
					await dialog.Msg(L("I saw it! I'm not going to describe it standing out in the open — I'm writing it down tonight and sending four copies by four different roads."));
					await dialog.Msg(L("Take this — it was in the receiving pit, and it isn't a Ferret thing, and it isn't a Zeraha thing. Somebody carried it here. Somebody."));

					character.Quests.Complete(questId);
				}
				else if (blankObj.Done)
				{
					await dialog.Msg(L("Scroll's written! Walk in beside me and count to four minutes. Exactly four, please, I timed it."));
				}
				else
				{
					await dialog.Msg(L("Still short of blanks. The Searchers keep them dry in a hip roll — it comes off whole if you're careful."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four copies gone by four roads. If even one reaches Fedimian intact, this stops being my problem and starts being an army's. Frankly, good."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Drowsy Herb
//-----------------------------------------------------------------------------

public class DrowsyHerbQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1001);
		SetName(L("Drowsy Herb"));
		SetType(QuestType.Sub);
		SetDescription(L("The Ferrets of Zeraha have burned drowsy herb in their camps for longer than anyone has kept records. Two years ago they stopped burning it and started hauling it somewhere in quantity."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herb-Scholar] Ausrine"), "f_orchard_34_2");

		AddObjective("killLoaders", L("Kill Ferret Loaders on the haul road"),
			new KillObjective(20, new[] { MonsterId.Ferret_Loader }));

		AddObjective("collectHerb", L("Collect bales of Drowsy Herb"),
			new CollectItemObjective(667016, 10));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667016, 0.50f, MonsterId.Ferret_Loader);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667016, character.Inventory.CountItem(667016), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667016, character.Inventory.CountItem(667016), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: The Slingers on the Ridge
//-----------------------------------------------------------------------------

public class TheSlingersOnTheRidgeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1002);
		SetName(L("The Slingers on the Ridge"));
		SetType(QuestType.Sub);
		SetDescription(L("For 20 years the Ferret Slingers sat on the ridge rock and watched the carts go by, and one of them always waved. Last month they started dropping stones instead, and 2 carts did not come through."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Rudenis"), "f_orchard_34_2");

		AddObjective("killSlingers", L("Kill Ferret Slingers on the ridge"),
			new KillObjective(30, new[] { MonsterId.Ferret_Slinger }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1003 CLASS: Terrible Scent
//-----------------------------------------------------------------------------

public class TerribleScentQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1003);
		SetName(L("Terrible Scent"));
		SetType(QuestType.Sub);
		SetDescription(L("Crates go off the wagons one or two a night from camps with a posted watch. A Ferret Empty Porter carries a scent gland that means 'this one has nothing', and no Ferret in Zeraha wastes a step on nothing."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Rudenis"), "f_orchard_34_2");

		AddObjective("collectScent", L("Collect scent glands from Ferret Empty Porters"),
			new CollectItemObjective(667022, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667022, 0.50f, MonsterId.Ferret_Patter);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667022, character.Inventory.CountItem(667022), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667022, character.Inventory.CountItem(667022), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Ferret Cairns
//-----------------------------------------------------------------------------

public class TheFerretCairnsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1004);
		SetName(L("The Ferret Cairns"));
		SetType(QuestType.Sub);
		SetDescription(L("Every Ferret in Zeraha will fight over a cairn piece and not one of them will turn one over. A thing nobody will look at the back of is a thing somebody was told not to look at the back of."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herb-Scholar] Ausrine"), "f_orchard_34_2");

		AddObjective("visitCairns", L("Burn incense and read the underside of all 4 cairn pieces"),
			new VariableCheckObjective("Laima.Quests.f_orchard_34_2.Quest1004.CairnsVisited", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667017, character.Inventory.CountItem(667017), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667021, character.Inventory.CountItem(667021), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_2.Quest1004.CairnsVisited");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1004.Cairn{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667017, character.Inventory.CountItem(667017), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(667021, character.Inventory.CountItem(667021), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_2.Quest1004.CairnsVisited");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_2.Quest1004.Cairn{i}");
	}
}

// Quest 1005 CLASS: The Ferret Transformation Scroll
//-----------------------------------------------------------------------------

public class TheFerretTransformationScrollQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2", 1005);
		SetName(L("The Ferret Transformation Scroll"));
		SetType(QuestType.Sub);
		SetDescription(L("The cairn instruction says where everything is being carried but not who is receiving it. A shape-based scroll on a scent-based people buys about 4 minutes at the end of the haul road."));
		SetLocation("f_orchard_34_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Herb-Scholar] Ausrine"), "f_orchard_34_2");

		AddPrerequisite(new CompletedPrerequisite("f_orchard_34_2", 1004));

		AddObjective("collectBlanks", L("Collect Empty Scrolls from Ferret Searchers"),
			new CollectItemObjective(667019, 6));

		AddObjective("theFourMinutes", L("Hold the receiving pit when the disguise fails"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Ferret_Loader, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Ferret_Searcher, 3),
				},
				resetIdent: "collectBlanks",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603113, 1)); // Svenus Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(667019, 0.45f, MonsterId.Ferret_Searcher);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(667019, character.Inventory.CountItem(667019), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(667019, character.Inventory.CountItem(667019), InventoryItemRemoveMsg.Destroyed);
	}
}
