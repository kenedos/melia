//--- Melia Script ----------------------------------------------------------
// Septyni Glen Quest NPCs
//--- Description -----------------------------------------------------------
// The glen that holds Saule's shrine, where the light that charges Andale's
// boundary oil is supposed to come from.
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

public class FHuevillage584QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Carcashu Shellfall
		//---------------------------------------------------------------------
		AddNpc(147407, L("[Glen-Ward] Saulius"), "f_huevillage_58_4", -78, -129, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1001);

			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's leaning on a broom, staring down an empty path like he's still expecting someone to appear on it*{/}"));
				await dialog.Msg(L("You're the first person I've seen on this path in a week. Saulius, I ward the glen. Pilgrims from three villages used to walk here to the shrine every midsummer. Last year eleven came. This year nobody has tried."));
				await dialog.Msg(L("The Carcashu are why. They shed shell all over the path and the shed pieces cut through a boot sole in an afternoon. Kill 30 of them and I can get the path swept clean before midsummer."));

				var response = await dialog.Select(L("Will you clear the path?"),
					Option(L("I'll kill the Carcashu"), "help"),
					Option(L("Why does midsummer matter?"), "info"),
					Option(L("Not right now"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Watch your footing where they've been shedding. The old shell is sharper than the live one."));
						break;

					case "info":
						await dialog.Msg(L("The midsummer walk is where the shrine light gets charged into the boundary oil. No pilgrims, no walk. No walk, no oil worth painting on."));
						await dialog.Msg(L("Vaidas up in the valley has been painting stones with last year's batch for a year now. That's the whole reason the ring is failing."));
						break;

					case "leave":
						await dialog.Msg(L("Then the path stays shut, and midsummer comes and goes quiet again, same as last year."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCarcashu", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Path's walkable. I swept the first two hundred paces this morning and filled a barrow with shell."));
					await dialog.Msg(L("Take this. It's shrine money, which means it's small, but it's clean."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still shell all down the path. Keep at it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two pilgrims came up the path yesterday, unasked. I hadn't even finished sweeping."));
			}
		});

		// Quest 1002: The Shrine Lamps
		//---------------------------------------------------------------------
		AddNpc(147420, L("[Shrine Keeper] Ruta"), "f_huevillage_58_4", 690, -180, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's kneeling before a cold lamp, hands folded, as if the praying alone might catch it*{/}"));
				await dialog.Msg(L("Give me a moment more, then I'm glad of the company. Ruta, shrine keeper. There are 5 lamps set around Saule's shrine, and they have burned without fuel since before Andale was a village. I have never once had to light one."));
				await dialog.Msg(L("They went out together on a clear night in spring. Take the offering tools and try 4 of them - I need to know whether they can be relit at all."));

				var response = await dialog.Select(L("Will you try the lamps?"),
					Option(L("I'll try to relight them"), "help"),
					Option(L("What happens if they can't be?"), "info"),
					Option(L("I'd rather not touch a shrine"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(650623, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("Set the tools down first, then kneel, then the words. In that order - I have watched people do it wrong my whole life and it never takes."));
						break;

					case "info":
						await dialog.Msg(L("Then the oil never charges, the obelisks never hold, and Andale moves or Andale is eaten. Those are the two options and everyone in the village knows it."));
						await dialog.Msg(L("So we don't say it out loud. We talk about the path and the pilgrims and whose turn it is to sweep."));
						break;

					case "leave":
						await dialog.Msg(L("Nor would I, truthfully, if there were anyone else to ask. There isn't, and the lamps are still dark."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("relightLamps", out var lampObj)) return;

				if (lampObj.Done)
				{
					await dialog.Msg(L("Four tried, four caught, four went out again inside a minute. So they can be lit. Something is putting them out."));
					await dialog.Msg(L("That's a better answer than I expected and a worse one than I wanted. Gerda has been watching the upper terrace - go and tell her the lamps will catch."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More lamps still dark. Tools down, then kneel, then the words."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I sit with the lamps most evenings now. They catch, they hold about a minute, and they go. Every time. Like something breathing on them."));
			}
		});

		// Quest 1002 interaction points - the shrine lamps
		//---------------------------------------------------------------------
		void AddShrineLamp(int lampNum, int x, int z, int direction)
		{
			AddNpc(151022, L("Shrine Lamp"), "f_huevillage_58_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_4", 1002);
				var variableKey = $"Laima.Quests.f_huevillage_58_4.Quest1002.Lamp{lampNum}";
				var counterKey = "Laima.Quests.f_huevillage_58_4.Quest1002.LampsRelit";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A shrine lamp, cold and dark, with no wick and no oil well*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You already tried this one. It caught, then went out*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Making the offering..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var relit = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, relit);
					character.ServerMessage(L("The lamp catches, burns for a breath, and goes out."));
					character.ServerMessage(LF("Lamps tried: {0}/4", relit));

					if (relit >= 4)
						character.ServerMessage(L("{#FFD700}All four caught and died. Return to Shrine Keeper Ruta.{/}"));
				}
				else
				{
					character.ServerMessage(L("You gather the tools back up."));
				}
			});
		}

		AddShrineLamp(1, 528, -45, 0);
		AddShrineLamp(2, 690, -61, 0);
		AddShrineLamp(3, 860, -24, 0);
		AddShrineLamp(4, 809, -313, 0);
		AddShrineLamp(5, 584, -402, 0);

		// Quest 1003: Feelers for the Awl-Stock
		//---------------------------------------------------------------------
		AddNpc(147408, L("[Carpenter] Zigmas"), "f_huevillage_58_4", 1219, -565, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1003);

			dialog.SetTitle(L("Zigmas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's turning a snapped awl over in his fingers, muttering at the knot that broke it*{/}"));
				await dialog.Msg(L("Good timing, actually — I could use another pair of eyes on this problem. Zigmas, carpenter. Ruta wants the shrine rail rebuilt before midsummer and I've got no awls left. Broke the last one on a knot two weeks back."));
				await dialog.Msg(L("A Beetow feeler dries harder than any steel I can afford out here. Kill 20 of them and bring me 5 good feelers and I'll have a full set of awls by the weekend."));

				var response = await dialog.Select(L("Will you fetch them?"),
					Option(L("I'll bring the feelers"), "help"),
					Option(L("Harder than steel?"), "info"),
					Option(L("Order proper tools"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them off clean at the base. A feeler with a crack in it will shatter the first time I lean on it."));
						break;

					case "info":
						await dialog.Msg(L("Harder than the steel a village carpenter gets, which isn't saying much, but it holds an edge for a season and steel doesn't out here in the damp."));
						await dialog.Msg(L("My grandfather worked entirely in bone and feeler. I've got his chisel roll and every piece in it still bites."));
						break;

					case "leave":
						await dialog.Msg(L("Order them from where, exactly? The road's shut and the village owes a powder merchant more than it owns."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBeetow", out var killObj)) return;
				if (!quest.TryGetProgress("gatherFeelers", out var feelerObj)) return;

				if (killObj.Done && feelerObj.Done)
				{
					await dialog.Msg(L("Five, and not a crack in any of them. That's a full set and a spare."));
					await dialog.Msg(L("Your pay. Come and look at the rail at midsummer - it'll be the best thing I've built in ten years."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Beetow still out on the east slope. ");
					if (!feelerObj.Done)
						status += L("More feelers still to gather. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rail's half done and dead straight. Ruta came and put her hand on it and didn't say anything, which from her is a compliment."));
			}
		});

		// Quest 1004: The Closing Road
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Path-Warden] Milda"), "f_huevillage_58_4", 1000, 640, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1004);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's testing the edge of a billhook against her thumb, rolling her sleeve down over an old scar*{/}"));
				await dialog.Msg(L("Careful, don't brush past that bramble on your left. Milda, I warden this path. It's a slow problem, which is why nobody deals with it until it isn't — this ring has closed 6 paces across the shrine road since the thaw."));
				await dialog.Msg(L("I can't cut it alone - the roots whip when you take them and I'm one woman with a billhook. Cut back 4 of the roots with me and the road stays open through midsummer."));

				var response = await dialog.Select(L("Will you take the billhook?"),
					Option(L("I'll cut back the brambles"), "help"),
					Option(L("The roots whip?"), "info"),
					Option(L("That's gardening"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cut low and step back on the same motion. And keep an eye out - the noise of cutting brings the Mantiwood in and they don't announce themselves."));
						break;

					case "info":
						await dialog.Msg(L("Not far, and not fast, but they come back at you and there's a hook on every inch. I've a scar across this arm that took a month to close."));
						await dialog.Msg(L("Six paces a season doesn't sound like much until you remember the road's only fourteen paces wide."));
						break;

					case "leave":
						await dialog.Msg(L("It is gardening, fair enough. It's also the only thing standing between the shrine and being cut off entirely."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("cutBrambles", out var brambleObj)) return;

				if (brambleObj.Done)
				{
					await dialog.Msg(L("Four back to the stump. That's the road held at fourteen paces for another year, which is all anyone's ever managed."));
					await dialog.Msg(L("Your pay, and put something on those arms before they go bad."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More of the ring still standing. Cut low and step back on the same motion."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Walked the whole road end to end this morning without turning sideways once. First time in three years."));
			}
		});

		// Quest 1004 interaction points - the bramble ring across the shrine road
		//---------------------------------------------------------------------
		void AddBrambleRoot(int rootNum, int modelId, int x, int z, int direction)
		{
			AddNpc(modelId, L("Bramble Root"), "f_huevillage_58_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_4", 1004);
				var variableKey = $"Laima.Quests.f_huevillage_58_4.Quest1004.Root{rootNum}";
				var counterKey = "Laima.Quests.f_huevillage_58_4.Quest1004.RootsCut";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A thick bramble root grown across the shrine road, hooked along its whole length*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Cut back to the stump already*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 400, 350);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The cutting carries - {0} coming through the brush!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Cutting back the root..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var cut = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, cut);
					character.ServerMessage(LF("Roots cut back: {0}/4", cut));

					if (cut >= 4)
						character.ServerMessage(L("{#FFD700}The road is open again. Return to Path-Warden Milda.{/}"));
				}
				else
				{
					character.ServerMessage(L("You lower the billhook."));
				}
			});
		}

		AddBrambleRoot(1, 153011, 992, 813, 0);
		AddBrambleRoot(2, 153058, 893, 749, 0);
		AddBrambleRoot(3, 153058, 1105, 847, 0);
		AddBrambleRoot(4, 153039, 1063, 721, 0);
		AddBrambleRoot(5, 153039, 1013, 925, 0);
		AddBrambleRoot(6, 153039, 891, 868, 0);

		// Quest 1005: What Breathes on the Lamps
		//---------------------------------------------------------------------
		AddNpc(147419, L("[Mage-Tutor] Gerda"), "f_huevillage_58_4", -1086, -660, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_4", 1005);

			dialog.SetTitle(L("Gerda"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_huevillage_58_4", 1002)))
				{
					await dialog.Msg(L("I teach four children their letters and their first three cantrips, and in between I watch the shrine's upper terrace through a glass."));
					await dialog.Msg(L("Ruta's trying the lamps. Come back when she's done - what she finds decides what I do next."));
					return;
				}

				await dialog.Msg(L("{#666666}*She lowers her spyglass and looks almost relieved to see you, like she's been waiting for someone to tell*{/}"));
				await dialog.Msg(L("You're back from Ruta, then — good. Gerda, I tutor the village children. So the lamps catch and die. That settles it: the lamps are fine and something is smothering them, and I've been watching that something through this glass for six weeks."));
				await dialog.Msg(L("There's a Mothstem on the upper terrace. It feeds on light and it has been sitting above the shrine drinking the lamps dry. The Tini Magicians are its brood-tenders - kill 15 of them and it will come down off the terrace itself."));

				var response = await dialog.Select(L("Will you go up there?"),
					Option(L("I'll kill the Mothstem"), "help"),
					Option(L("Why not tell anyone sooner?"), "info"),
					Option(L("Send for a proper mage"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't bring a light up there. Whatever you carry, it drinks, and it gets faster the more it takes."));
						break;

					case "info":
						await dialog.Msg(L("Because for six weeks the honest answer was that I had seen a shape and had a theory. Ruta has spent thirty years on those lamps. I wasn't going to hand her a theory."));
						await dialog.Msg(L("Now I have her result and my observation and they agree. That's different."));
						break;

					case "leave":
						await dialog.Msg(L("I sent for one in spring, if you're curious. The letter came back unopened, road closure stamped clean across the seal."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTenders", out var tenderObj)) return;
				if (!quest.TryGetProgress("killMothstem", out var bossObj)) return;

				if (tenderObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("The lamps caught while you were still coming down the terrace steps. All five, and they have not gone out."));
					await dialog.Msg(L("Take this - it was up there among a great many things that had been carried up and not carried down. Ruta is going to want to charge oil the moment she stops crying."));

					character.Quests.Complete(questId);
				}
				else if (tenderObj.Done)
				{
					await dialog.Msg(L("It's coming down off the terrace. Go, before it settles back into the dark."));
				}
				else
				{
					await dialog.Msg(L("Too many brood-tenders still around it. It won't move while they're feeding it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ruta charged a full year's oil in one night and sent half of it up the valley to Vaidas by runner. The children asked why she was crying. I told them she was tired."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Carcashu Shellfall
//-----------------------------------------------------------------------------

public class CarcashuShellfallQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1001);
		SetName(L("Carcashu Shellfall"));
		SetType(QuestType.Sub);
		SetDescription(L("Shed Carcashu shell has made the pilgrim path to Saule's shrine impassable, and without the midsummer walk Andale's boundary oil never gets charged. Clear the path for Glen-Ward Saulius."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Glen-Ward] Saulius"), "f_huevillage_58_4");

		AddObjective("killCarcashu", L("Kill Carcashu along the pilgrim path"),
			new KillObjective(30, new[] { MonsterId.Carcashu }));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
	}
}

// Quest 1002 CLASS: The Shrine Lamps
//-----------------------------------------------------------------------------

public class TheShrineLampsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1002);
		SetName(L("The Shrine Lamps"));
		SetType(QuestType.Sub);
		SetDescription(L("The five lamps around Saule's shrine burned without fuel for longer than Andale has existed, and they went out together in spring. Take Keeper Ruta's offering tools and find out whether they can be relit."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shrine Keeper] Ruta"), "f_huevillage_58_4");

		AddObjective("relightLamps", L("Try to relight the shrine lamps"),
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_4.Quest1002.LampsRelit", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650623, character.Inventory.CountItem(650623), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_4.Quest1002.LampsRelit");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_4.Quest1002.Lamp{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650623, character.Inventory.CountItem(650623), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_4.Quest1002.LampsRelit");

		for (var i = 1; i <= 5; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_4.Quest1002.Lamp{i}");
	}
}

// Quest 1003 CLASS: Feelers for the Awl-Stock
//-----------------------------------------------------------------------------

public class FeelersForTheAwlStockQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1003);
		SetName(L("Feelers for the Awl-Stock"));
		SetType(QuestType.Sub);
		SetDescription(L("Carpenter Zigmas has to rebuild the shrine rail before midsummer and has broken his last awl. Kill Beetow on the east slope and bring him feelers hard enough to cut a new set."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Carpenter] Zigmas"), "f_huevillage_58_4");

		AddObjective("killBeetow", L("Kill Beetow on the east slope"),
			new KillObjective(20, new[] { MonsterId.Beetow }));

		AddObjective("gatherFeelers", L("Recover Tough Beetow Feelers"),
			new CollectItemObjective(663335, 5));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(663335, 0.45f, MonsterId.Beetow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663335, character.Inventory.CountItem(663335), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663335, character.Inventory.CountItem(663335), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: The Closing Road
//-----------------------------------------------------------------------------

public class TheClosingRoadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1004);
		SetName(L("The Closing Road"));
		SetType(QuestType.Sub);
		SetDescription(L("A bramble ring has closed six paces across the shrine road since the thaw, and Path-Warden Milda cannot cut it alone. Cut the roots back to the stump before the road narrows further."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Path-Warden] Milda"), "f_huevillage_58_4");

		AddObjective("cutBrambles", L("Cut back the bramble roots across the shrine road"),
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_4.Quest1004.RootsCut", 4, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_4.Quest1004.RootsCut");

		for (var i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_4.Quest1004.Root{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_4.Quest1004.RootsCut");

		for (var i = 1; i <= 6; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_4.Quest1004.Root{i}");
	}
}

// Quest 1005 CLASS: What Breathes on the Lamps
//-----------------------------------------------------------------------------

public class WhatBreathesOnTheLampsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_4", 1005);
		SetName(L("What Breathes on the Lamps"));
		SetType(QuestType.Sub);
		SetDescription(L("The shrine lamps catch and die because a Mothstem on the upper terrace is drinking them dry. Kill its brood-tenders to bring it down off the terrace, then finish it."));
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Mage-Tutor] Gerda"), "f_huevillage_58_4");

		AddPrerequisite(new CompletedPrerequisite("f_huevillage_58_4", 1002));

		AddObjective("killTenders", L("Kill Tini Magicians tending the brood"),
			new KillObjective(15, new[] { MonsterId.Tiny_Mage }));

		AddObjective("killMothstem", L("Defeat the Mothstem"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Mothstem, 1) },
				resetIdent: "killTenders",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(39000, 27000));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(183103, 1)); // Grajus
		AddReward(new ItemReward(640085, 3)); // Lv5 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
