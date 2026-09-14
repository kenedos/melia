//--- Melia Script ----------------------------------------------------------
// Vieta Valley Quest NPCs
//--- Description -----------------------------------------------------------
// Andale Village's outermost boundary ring, where the blessed obelisks that
// hold the forest back have started to fail.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage582QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Valley Overrun
		//---------------------------------------------------------------------
		AddNpc(147420, L("[Huntress] Silvia"), "f_huevillage_58_2", 1010, 347, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1001);

			dialog.SetTitle(L("Silvia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's testing a trap spring with her thumb, wincing when it snaps shut empty*{/}"));
				await dialog.Msg(L("Watch the teeth on that one, it's quicker than it looks — lost two fingers to a spring trap once, mine own fault, wasn't paying attention. Andale gets its winter meat off this valley. Two seasons back I could walk the gorge floor alone with nothing but a skinning knife."));
				await dialog.Msg(L("Now the Ultanun own it, plain and simple. Came down past the boundary stones like the stones weren't even there, and they don't spook anymore, not for anything. Kill 25 of them and I get my traps back in the ground where they belong."));

				var response = await dialog.Select(L("Will you clear the gorge floor?"),
					Option(L("I'll kill the Ultanun"), "help"),
					Option(L("Past the boundary stones?"), "info"),
					Option(L("Not right now"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work the gorge floor, not the slopes. On the slopes they get above you and you never see the second one."));
						break;

					case "info":
						await dialog.Msg(L("Vaidas keeps a ring of blessed obelisks around the valley. Nothing out of the forest would cross that line - that was the whole arrangement, going back further than anyone's grandmother."));
						await dialog.Msg(L("Ask him about it. He'll tell you it's his fault, and he'll be wrong, and he won't listen."));
						break;

					case "leave":
						await dialog.Msg(L("Suit yourself. The traps stay in the shed a while longer, and the village eats salt beef again."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killUltanun", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Gorge floor's walkable. I set eleven traps this morning and nothing had been at them by dusk."));
					await dialog.Msg(L("Your pay, and a cut of whatever the first line brings in."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still thick down on the gorge floor. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Meat's hanging in the village smokehouse for the first time since spring. Nobody's said thank you but everybody's eating."));
			}

			// Quest 1004 delivery - Juris's hide bundle and tally
			if (character.Quests.IsActive(new QuestId("f_huevillage_58_2", 1004)))
			{
				var deliveredKey = "Laima.Quests.f_huevillage_58_2.Quest1004.Delivered";
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("That's Juris's wrapping. He finally cured the batch, then."));
					await dialog.Msg(L("{#666666}*She counts the hides and marks a tally-stick*{/}"));
					await dialog.Msg(L("Fourteen, and four of them are poor. Tell him I said the sap he's using has gone off - it's not his knife-work, it's the sap. He'll argue. Give him the stick anyway."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(new QuestId("f_huevillage_58_2", 1004), "deliverBundle");
					character.ServerMessage(L("{#FFD700}Silvia's tally-stick received. Return to Tanner Juris.{/}"));
				}
			}
		});

		// Quest 1002: Boundary Stones
		//---------------------------------------------------------------------
		AddNpc(147409, L("[Village Priest] Vaidas"), "f_huevillage_58_2", -239, -200, 342, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1002);

			dialog.SetTitle(L("Vaidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's squinting at a paintbrush like it personally betrayed him*{/}"));
				await dialog.Msg(L("Ah — good, a set of young knees. There are 5 obelisks standing around this valley. My grandfather painted them, his mother painted them, and every one held until this spring."));
				await dialog.Msg(L("Now the paint won't take. It goes on and it beads off like water on a griddle. Go to 4 of the stones and say the renewal over them - I want to know whether it's the stones that have failed or me."));

				var response = await dialog.Select(L("Will you walk the ring for me?"),
					Option(L("I'll renew 4 obelisks"), "help"),
					Option(L("Why can't you go yourself?"), "info"),
					Option(L("That sounds like priest work"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The words don't have to be perfect. Put your hand flat on the stone and mean it - the stone does the rest, or it doesn't."));
						await dialog.Msg(L("Two are up on the south rim, two down past the far end of the gorge, and one stands by the huntress's camp."));
						break;

					case "info":
						await dialog.Msg(L("I'm seventy-one and the south rim obelisk is a two-hour climb. I made it last month and I couldn't hold the brush steady when I got there."));
						await dialog.Msg(L("That's the honest answer. The village thinks I'm resting my knee."));
						break;

					case "leave":
						await dialog.Msg(L("It is priest work, true enough. But this priest can't reach them any more, and the stones don't care whose hand says the words."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("renewObelisks", out var obeliskObj)) return;

				if (obeliskObj.Done)
				{
					await dialog.Msg(L("All four beaded off. Not one of them took, and you're a stranger with a steady hand, so it isn't me."));
					await dialog.Msg(L("Something's gone wrong with the oil itself. That means the white oak, and the white oak is Audra's ground. Go and tell her I said so - she'll know what I mean."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More stones still to try. Hand flat on the stone, and mean it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Fifty years I've mixed that oil the same way. It never once occurred to me that the tree could be the thing that changed."));
			}
		});

		// Quest 1002 interaction points - the valley's boundary obelisks
		//---------------------------------------------------------------------
		void AddBoundaryObelisk(int obeliskNum, int x, int z, int direction)
		{
			AddNpc(147414, L("Boundary Obelisk"), "f_huevillage_58_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_2", 1002);
				var variableKey = $"Laima.Quests.f_huevillage_58_2.Quest1002.Obelisk{obeliskNum}";
				var counterKey = "Laima.Quests.f_huevillage_58_2.Quest1002.ObelisksRenewed";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered standing stone, its painted bands flaked down to the grain*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already said the renewal here. The oil beaded off and ran into the grass*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Saying the renewal..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var renewed = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, renewed);
					character.ServerMessage(L("The oil beads off the stone and runs into the grass."));
					character.ServerMessage(LF("Obelisks renewed: {0}/4", renewed));

					if (renewed >= 4)
						character.ServerMessage(L("{#FFD700}None of them took. Return to Village Priest Vaidas.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the stone unpainted."));
				}
			});
		}

		AddBoundaryObelisk(1, 859, 205, 290);
		AddBoundaryObelisk(2, 440, -1641, 5);
		AddBoundaryObelisk(3, 1462, 1157, 83);
		AddBoundaryObelisk(4, -516, -1542, 35);
		AddBoundaryObelisk(5, -390, -1618, 262);

		// Quest 1003: Powder from the Maize
		//---------------------------------------------------------------------
		AddNpc(147407, L("[Forager] Gedas"), "f_huevillage_58_2", -380, -140, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1003);

			dialog.SetTitle(L("Gedas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's turning a dried seed-husk over in his palm, grimacing at the smell*{/}"));
				await dialog.Msg(L("Don't mind the stink, you get used to it. Black Maize is a weed that walks — ugly business, but the husk-dust off a dead one is the only numbing agent this village has, and we go through it fast."));
				await dialog.Msg(L("Silvia's traps take fingers off sometimes. Kill 20 of them and bring me 5 measures of the powder, and I'll have enough to get us to autumn."));

				var response = await dialog.Select(L("Will you gather it?"),
					Option(L("I'll bring the powder"), "help"),
					Option(L("A weed that walks?"), "info"),
					Option(L("Find another forager"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut low and step back. The husk splits when it dies and the first puff will numb your face for an hour."));
						break;

					case "info":
						await dialog.Msg(L("Seed-heads on stalks, and they turn to look at you. My father swore they only came out of the deep forest, never down into the valley."));
						await dialog.Msg(L("Well. Here they are, in the valley, and my father's not around to argue about it."));
						break;

					case "leave":
						await dialog.Msg(L("There isn't another forager to find. There's me, and there's whoever I manage to talk into it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMaize", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPowder", out var powderObj)) return;

				if (killObj.Done && powderObj.Done)
				{
					await dialog.Msg(L("Five measures, and dry ones. Half of what people bring me is damp and worth nothing."));
					await dialog.Msg(L("Take your pay. If you ever lose a finger out here, come to me before you go to the priest."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Black Maize still standing. ");
					if (!powderObj.Done)
						status += L("More powder still to gather. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Set a boy's arm last week and he barely whimpered. That's your powder doing that."));
			}
		});

		// Quest 1004: Juris's Mark-Slip
		//---------------------------------------------------------------------
		AddNpc(147408, L("[Tanner] Juris"), "f_huevillage_58_2", 319, 1250, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1004);
			var deliveredKey = "Laima.Quests.f_huevillage_58_2.Quest1004.Delivered";

			dialog.SetTitle(L("Juris"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He straightens up slow from a stack of hides, one hand pressed to his lower back*{/}"));
				await dialog.Msg(L("Give me a second, this back doesn't forgive bending twice in a row. Fourteen Ultanun hides, cured and wrapped, and they've sat in my shed three weeks because I can't walk the gorge with a load anymore."));
				await dialog.Msg(L("Silvia paid for them up front. Carry the bundle up to her camp at the head of the gorge and bring me back her tally-stick so the books balance."));

				var response = await dialog.Select(L("Will you run it up for me?"),
					Option(L("I'll take the bundle to Silvia"), "help"),
					Option(L("Three weeks is a long time"), "info"),
					Option(L("Walk it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Keep it dry. If the wrapping soaks through the whole batch goes stiff and I'll have to start again."));
						break;

					case "info":
						await dialog.Msg(L("It is, and she's said so. But the last two runners I hired went down the gorge and came back without the bundle, and one of them came back without a boot."));
						await dialog.Msg(L("You look like the Ultanun are less of a problem for you than they were for him."));
						break;

					case "leave":
						await dialog.Msg(L("If I could carry it myself, I would've already. That's rather the whole difficulty."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("{#666666}*He turns the tally-stick over and reads the notches twice*{/}"));
					await dialog.Msg(L("Four poor out of fourteen. She's blaming my sap. My sap is the same sap I've drawn off the white oak for thirty years."));
					await dialog.Msg(L("Which, now I say it out loud, does rather suggest the oak. Here's your money before I go and think about that."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Silvia's camp is at the head of the gorge. She'll want to count them before she marks the stick."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I drew a fresh cup off the oak and left it on the bench overnight. It went grey. Thirty years and it has never once gone grey."));
			}
		});

		// Quest 1005: What Rooted in the White Oak
		//---------------------------------------------------------------------
		AddNpc(147419, L("[Naturalist] Audra"), "f_huevillage_58_2", -548, 133, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1005);

			dialog.SetTitle(L("Audra"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_huevillage_58_2", 1002)))
				{
					await dialog.Msg(L("I count what lives in this valley and I write down when it changes. Lately it changes faster than I can write."));
					await dialog.Msg(L("Vaidas is out walking his obelisk ring again. Come back when he's finished - whatever he finds out there is going to be my problem next."));
					return;
				}

				await dialog.Msg(L("{#666666}*She's got a field notebook open, but she's been staring at the same blank page for a while*{/}"));
				await dialog.Msg(L("You caught me not writing anything, which is about right lately. Vaidas told you the oil beads off. Juris says his sap went grey overnight. Both come off the same white oak at the head of the valley, and I've been avoiding that tree for a month."));
				await dialog.Msg(L("There's a Moldyhorn rooted in it. It's been feeding on the heartwood and everything the tree gives up now is spoiled. The Black Maize around it are its overflow - kill 12 of them and it'll pull itself out of the trunk to see why."));

				var response = await dialog.Select(L("Will you go up to the oak?"),
					Option(L("I'll deal with the Moldyhorn"), "help"),
					Option(L("Why avoid the tree?"), "info"),
					Option(L("That's a big animal"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("When it comes out of the trunk it brings half the bark with it and it will be angry about the light. Give it room for the first few seconds."));
						break;

					case "info":
						await dialog.Msg(L("Because if I went up and confirmed it, I'd have to tell Vaidas that the tree his family has blessed for four generations is the thing letting the forest in."));
						await dialog.Msg(L("He's seventy-one. I kept hoping it would turn out to be something else."));
						break;

					case "leave":
						await dialog.Msg(L("It is a big animal. And it only gets bigger for every week it sits rooted in that trunk."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinOverflow", out var thinObj)) return;
				if (!quest.TryGetProgress("killMoldyhorn", out var bossObj)) return;

				if (thinObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's out and it's dead and the trunk is a ruin. But the sap running out of the split is clear."));
					await dialog.Msg(L("Take this. It was in the hollow the thing had been sleeping in, along with a lot of things I'd rather not have found."));

					character.Quests.Complete(questId);
				}
				else if (thinObj.Done)
				{
					await dialog.Msg(L("The bark's moving. Get back to the oak before it settles again."));
				}
				else
				{
					await dialog.Msg(L("Too much Black Maize still around the trunk. It won't stir while it's got that much cover."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Vaidas repainted the nearest obelisk this morning with the new oil. It took. He sat down in the grass afterward and didn't say anything for a while."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Valley Overrun
//-----------------------------------------------------------------------------

public class TheValleyOverrunQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1001);
		SetName(L("The Valley Overrun"));
		SetType(QuestType.Sub);
		SetDescription(L("Ultanun have crossed Andale's boundary ring and taken the gorge floor. Thin them out so Huntress Silvia can set her traplines again."));
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Huntress] Silvia"), "f_huevillage_58_2");

		AddObjective("killUltanun", L("Kill Ultanun on the gorge floor"),
			new KillObjective(25, new[] { MonsterId.Ultanun }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1002 CLASS: Boundary Stones
//-----------------------------------------------------------------------------

public class BoundaryStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1002);
		SetName(L("Boundary Stones"));
		SetType(QuestType.Sub);
		SetDescription(L("The blessed obelisks that ring Vieta Valley have stopped holding, and Priest Vaidas is too old to walk them. Say the renewal at the boundary stones and find out whether the stones failed or the oil did."));
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Village Priest] Vaidas"), "f_huevillage_58_2");

		AddObjective("renewObelisks", L("Say the renewal at the boundary obelisks"),
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_2.Quest1002.ObelisksRenewed", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1002.ObelisksRenewed");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1002.Obelisk{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1002.ObelisksRenewed");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1002.Obelisk{i}");
	}
}

// Quest 1003 CLASS: Powder from the Maize
//-----------------------------------------------------------------------------

public class PowderFromTheMaizeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1003);
		SetName(L("Powder from the Maize"));
		SetType(QuestType.Sub);
		SetDescription(L("Husk-dust from a killed Black Maize is the only numbing agent Andale has, and the village is nearly out. Kill Black Maize and bring Forager Gedas his measures of paralysis powder."));
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forager] Gedas"), "f_huevillage_58_2");

		AddObjective("killMaize", L("Kill Black Maize"),
			new KillObjective(20, new[] { MonsterId.Zibu_Maize }));

		AddObjective("gatherPowder", L("Gather Paralysis Powder"),
			new CollectItemObjective(650616, 5));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(650616, 0.45f, MonsterId.Zibu_Maize);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650616, character.Inventory.CountItem(650616), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650616, character.Inventory.CountItem(650616), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Juris's Mark-Slip
//-----------------------------------------------------------------------------

public class JurissMarkSlipQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1004);
		SetName(L("Juris's Mark-Slip"));
		SetType(QuestType.Sub);
		SetDescription(L("Tanner Juris can no longer walk the gorge with a load on his back, and Silvia's hides have sat in his shed for three weeks. Carry the bundle to her camp and bring back her tally-stick."));
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tanner] Juris"), "f_huevillage_58_2");

		AddObjective("deliverBundle", L("Take the hide bundle to Huntress Silvia"),
			new ManualObjective());

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1004.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1004.Delivered");
	}
}

// Quest 1005 CLASS: What Rooted in the White Oak
//-----------------------------------------------------------------------------

public class WhatRootedInTheWhiteOakQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1005);
		SetName(L("What Rooted in the White Oak"));
		SetType(QuestType.Sub);
		SetDescription(L("The blessing oil and the tanner's sap both come off one white oak, and both have spoiled. A Moldyhorn is rooted in its heartwood. Thin the Black Maize around the trunk to draw it out, then kill it."));
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Naturalist] Audra"), "f_huevillage_58_2");

		AddPrerequisite(new CompletedPrerequisite("f_huevillage_58_2", 1002));

		AddObjective("thinOverflow", L("Kill Black Maize around the white oak"),
			new KillObjective(12, new[] { MonsterId.Zibu_Maize }));

		AddObjective("killMoldyhorn", L("Defeat the Moldyhorn"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Moldyhorn, 1) },
				resetIdent: "thinOverflow",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(603102, 1)); // Bracelet of Linne
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
