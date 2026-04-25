//--- Melia Script ----------------------------------------------------------
// Gateway of the Great King Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for the Gateway of the Great King map.
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

public class FRokas24QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Hogma Warband
		//-------------------------------------------------------------------------
		AddNpc(20060, L("[Gate Marshal] Einar"), "f_rokas_24", 850, -1800, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1001);

			dialog.SetTitle(L("Einar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Hogma warbands have taken the whole gate stretch. Warriors up front, Combat-class behind them, all drilled and mean."));
				await dialog.Msg(L("They've been chipping the Great King's statue for trophy stones. I won't abide defacement. Not here."));
				await dialog.Msg(L("Kill thirty Hogma - mixed Warriors and Combat - and their chant breaks. That's when the statue gets peace again."));

				var response = await dialog.Select(L("Thirty Hogma?"),
					Option(L("I'll break the chant"), "help"),
					Option(L("Chant?"), "info"),
					Option(L("Let them have it"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Hogma Warriors and fifteen Hogma Combat?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Warriors up top, Combat flanking. Mind the two-on-one."));
							await dialog.Msg(L("Every trophy they drop is going back on the statue."));
						}
						break;

					case "info":
						await dialog.Msg(L("They chant while they chip. A guttural verse, over and over. It's how they keep rhythm."));
						await dialog.Msg(L("Thin the numbers enough and the chant collapses. Then the rest scatter."));
						break;

					case "leave":
						await dialog.Msg(L("Not while I'm the marshal here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWarriors", out var warriorObj)) return;
				if (!quest.TryGetProgress("killCombat", out var combatObj)) return;

				if (warriorObj.Done && combatObj.Done)
				{
					await dialog.Msg(L("Chant's broken. The gate stretch is quiet for the first time in weeks."));
					await dialog.Msg(L("Statue goes back together this month. Take your pay."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. Break the chant for good."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Statue's patched. Pilgrims are coming through again."));
			}
		});

		// Quest 2: Cockatrie Killings
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Falconer] Yrsa"), "f_rokas_24", 880, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1002);

			dialog.SetTitle(L("Yrsa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Cockatries nest thick along the east cliffs. Small ones in swarms, big ones bullying pilgrims off the road."));
				await dialog.Msg(L("Bring me sixteen feathers from Big Cockatries - red and normal both. The plumes are worth a fortune to Fedimian's fletchers."));

				var response = await dialog.Select(L("Sixteen plumes?"),
					Option(L("I'll bring them"), "help"),
					Option(L("Why the big ones?"), "info"),
					Option(L("Skip the cliffs"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill Big Cockatries and bring sixteen plume feathers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Red or normal, doesn't matter - I sort them later. Just sixteen clean plumes."));
							await dialog.Msg(L("Watch the dive. A big one drops straight down."));
						}
						break;

					case "info":
						await dialog.Msg(L("The small ones shed in handfuls. Worthless fluff. The big ones have structure - shafts a fletcher can trust."));
						await dialog.Msg(L("A dozen Fedimian bowmen are waiting on this shipment."));
						break;

					case "leave":
						await dialog.Msg(L("The cliffs don't skip you. Remember that."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var feathers = character.Inventory.CountItem(650100);

				if (feathers >= 16)
				{
					await dialog.Msg(L("Sixteen clean plumes, look at these shafts. The fletchers are going to weep."));
					await dialog.Msg(L("Take your pay. I'll be back here on the next shipment."));

					character.Inventory.Remove(650100, 16, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep plucking. {0} of sixteen.", feathers));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Those arrows reached Fedimian last week. Full order paid out."));
			}
		});

		// Quest 3: Tontus Tally
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Pilgrim] Halli"), "f_rokas_24", -700, -200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1003);

			dialog.SetTitle(L("Halli"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Tontus and Dandels are everywhere on this road. I'm not a fighter - I just count steps and pray."));
				await dialog.Msg(L("But I know who is: you. Thin twenty Tontus and fifteen Dandels so the pilgrims behind me can pass without weeping."));

				var response = await dialog.Select(L("How can I help?"),
					Option(L("I'll clear the road"), "help"),
					Option(L("How many pilgrims?"), "info"),
					Option(L("Turn back"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty Tontus and fifteen Dandels along the pilgrim road?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Blessings on your blade, traveler. I'll be praying."));
						}
						break;

					case "info":
						await dialog.Msg(L("Forty, give or take. Three elders among them. One pregnant. I won't leave them to the Tontus."));
						break;

					case "leave":
						await dialog.Msg(L("No turning back. The goddess calls."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTontus", out var tontusObj)) return;
				if (!quest.TryGetProgress("killDandels", out var dandelObj)) return;

				if (tontusObj.Done && dandelObj.Done)
				{
					await dialog.Msg(L("Blessed goddess, the road is open. The pilgrims are already filing through."));
					await dialog.Msg(L("Take this offering. Earned, not tithed."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Pray with me, and keep swinging."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All forty reached the shrine. Three miracles recorded."));
			}
		});

		// Quest 4: Pino-Geppetto Grove
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Toymaker] Leif"), "f_rokas_24", -200, -1950, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1004);

			dialog.SetTitle(L("Leif"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Pinos and Geppettos used to be harmless. A novelty for the shop. Now they gang up and trample my lumber."));
				await dialog.Msg(L("Kill twelve Pinos and fifteen Geppettos. Bring me six pinewood knots from the grove - those are the ones the creatures chew on."));

				var response = await dialog.Select(L("Mixed work?"),
					Option(L("I'll clear the grove"), "help"),
					Option(L("Why do they chew knots?"), "info"),
					Option(L("Buy lumber elsewhere"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Pinos, fifteen Geppettos, and gather six pinewood knots?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The knots are gummy when they drop - don't worry, they clean up."));
							await dialog.Msg(L("Six is enough to resume production."));
						}
						break;

					case "info":
						await dialog.Msg(L("Sap-rich. The knots are where the tree hoarded its strongest sap. Toy mouths, apparently, go for it like candy."));
						break;

					case "leave":
						await dialog.Msg(L("Lumber from the north is twice the price and half the grain. Please help."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPinos", out var pinoObj)) return;
				if (!quest.TryGetProgress("killGeppettos", out var gepObj)) return;
				if (!quest.TryGetProgress("gatherKnots", out var knotObj)) return;

				if (pinoObj.Done && gepObj.Done && knotObj.Done)
				{
					await dialog.Msg(L("Six knots, all clean. The grove's breathing again."));
					await dialog.Msg(L("Your pay, plus a little doll for luck. Don't laugh - it really works."));

					character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!pinoObj.Done) status += L("More Pinos. ");
					if (!gepObj.Done) status += L("More Geppettos. ");
					if (!knotObj.Done) status += L("More pinewood knots. ");
					await dialog.Msg(LF("{0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("First toys of the new batch are on the shelves. The grove's hum is a lot softer now."));
			}
		});

		// Quest 5: The Cockatrie Matron
		//-------------------------------------------------------------------------
		AddNpc(147418, L("[Huntress] Siv"), "f_rokas_24", 1100, 700, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1005);
			var matronSpawnedKey = "Laima.Quests.f_rokas_24.Quest1005.MatronSpawned";

			dialog.SetTitle(L("Siv"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's a matron above the cliffs. Biggest Cockatrie you'll ever meet. She spawns the red ones - kill her, and the red line thins for a season."));
				await dialog.Msg(L("She won't come down while her breeders are plump. Kill ten red Big Cockatries, and she'll come to scream at you."));

				var response = await dialog.Select(L("Break her brood?"),
					Option(L("I'll hunt the matron"), "help"),
					Option(L("Red ones?"), "info"),
					Option(L("Not this one"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten Big Red Cockatries and bring down the matron when she descends?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten breeders. She'll feel it. She always feels it."));
						}
						break;

					case "info":
						await dialog.Msg(L("The red ones are her direct hatch. Bigger, meaner, louder. She throws them out on the cliffs to test them."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. She's not subtle prey."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBreeders", out var breederObj)) return;
				if (!quest.TryGetProgress("killMatron", out var matronObj)) return;

				if (breederObj.Done && matronObj.Done)
				{
					await dialog.Msg(L("The matron's wing-span across the cliff-path. Biggest feather I've ever seen."));
					await dialog.Msg(L("Season's safe. Take your bounty."));

					character.Variables.Perm.Remove(matronSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (breederObj.Done && !matronObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(matronSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(matronSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Big_Cockatries_Red, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("She's screaming. You hear it? She's coming down."));
							character.ServerMessage(L("{#FF9966}The Matron descends from the cliffs!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("She's out there. Finish her before she retreats."));
					}
				}
				else
				{
					await dialog.Msg(L("Ten breeders. She doesn't move until ten."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The cliffs are singing a different song this season. Red line's broken."));
			}
		});

		// Quest 6: Great King's Passage
		//-------------------------------------------------------------------------
		AddNpc(155146, L("[Road Warden] Gunnar"), "f_rokas_24", -500, -3400, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_24", 1006);

			dialog.SetTitle(L("Gunnar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The south passage is Hogma Warriors and Cockatries both, and neither side likes the other. Pilgrims get caught in the middle."));
				await dialog.Msg(L("Kill twelve Hogma Warriors and twelve Cockatries and the passage opens again for a week."));

				var response = await dialog.Select(L("Two sides?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Who's worse?"), "info"),
					Option(L("Not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Hogma Warriors and twelve Cockatries on the south passage?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The Hogma spit, the Cockatries scream. Just don't get in the way of either."));
						}
						break;

					case "info":
						await dialog.Msg(L("Hogma are smarter, Cockatries are faster. Pilgrims prefer neither. I prefer neither."));
						break;

					case "leave":
						await dialog.Msg(L("Pilgrims wait for nobody. Including you."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHogma", out var hogmaObj)) return;
				if (!quest.TryGetProgress("killCockatries", out var cockObj)) return;

				if (hogmaObj.Done && cockObj.Done)
				{
					await dialog.Msg(L("South passage is clear. I can see the pilgrim banners from here."));
					await dialog.Msg(L("Your pay. Buy warm cider, it's cold down here."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep pushing both sides."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Passage held for ten days. A record."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FRokas24Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1001);
		SetName(L("Hogma Warband"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hogma Warriors and Hogma Combat at the Gateway so the Great King's statue can be repaired."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Gate Marshal] Einar"), "f_rokas_24");

		AddObjective("killWarriors", L("Kill Hogma Warriors"),
			new KillObjective(15, new[] { MonsterId.Hogma_Warrior }));

		AddObjective("killCombat", L("Kill Hogma Combat"),
			new KillObjective(15, new[] { MonsterId.Hogma_Combat }));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 9));
		AddReward(new ItemReward(640007, 9));
		AddReward(new ItemReward(640012, 3));
	}
}

public class FRokas24Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1002);
		SetName(L("Cockatrie Plumes"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Big Cockatries and deliver sixteen plume feathers to Fedimian's fletchers."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Falconer] Yrsa"), "f_rokas_24");

		AddObjective("gatherPlumes", L("Gather Cockatrie plumes"),
			new CollectItemObjective(650100, 16));

		AddReward(new ExpReward(3900, 2700));
		AddReward(new SilverReward(21000));
		AddReward(new ItemReward(640084, 1));
		AddReward(new ItemReward(640004, 8));
		AddReward(new ItemReward(640007, 8));
		AddReward(new ItemReward(640012, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650100, character.Inventory.CountItem(650100), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650100, character.Inventory.CountItem(650100), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas24Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1003);
		SetName(L("Pilgrim Road Cleanup"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Tontus and Dandels along the pilgrim road so travelers can pass safely."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pilgrim] Halli"), "f_rokas_24");

		AddObjective("killTontus", L("Kill Tontus"),
			new KillObjective(20, new[] { MonsterId.Tontus }));

		AddObjective("killDandels", L("Kill Dandels"),
			new KillObjective(15, new[] { MonsterId.Dandel }));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 9));
		AddReward(new ItemReward(640007, 9));
	}
}

public class FRokas24Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1004);
		SetName(L("Pino-Geppetto Grove"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Pinos and Geppettos and gather pinewood knots for the toymaker."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Toymaker] Leif"), "f_rokas_24");

		AddObjective("killPinos", L("Kill Pinos"),
			new KillObjective(12, new[] { MonsterId.Pino }));

		AddObjective("killGeppettos", L("Kill Geppettos"),
			new KillObjective(15, new[] { MonsterId.Geppetto }));

		AddObjective("gatherKnots", L("Gather pinewood knots"),
			new CollectItemObjective(650011, 6));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(36000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 10));
		AddReward(new ItemReward(640007, 10));
		AddReward(new ItemReward(640012, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650011, character.Inventory.CountItem(650011), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FRokas24Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1005);
		SetName(L("The Cockatrie Matron"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill ten Big Red Cockatries to draw out and slay their matron."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Huntress] Siv"), "f_rokas_24");

		AddObjective("killBreeders", L("Kill Big Red Cockatries"),
			new KillObjective(10, new[] { MonsterId.Big_Cockatries_Red }));

		AddObjective("killMatron", L("Defeat the Matron"),
			new KillObjective(1, new[] { MonsterId.Big_Cockatries_Red }));

		AddReward(new ExpReward(8700, 6000));
		AddReward(new SilverReward(36000));
		AddReward(new ItemReward(640084, 3));
		AddReward(new ItemReward(640004, 10));
		AddReward(new ItemReward(640007, 10));
		AddReward(new ItemReward(640012, 3));
	}
}

public class FRokas24Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_24", 1006);
		SetName(L("Great King's Passage"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hogma Warriors and Cockatries contesting the south passage."));
		SetLocation("f_rokas_24");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Road Warden] Gunnar"), "f_rokas_24");

		AddObjective("killHogma", L("Kill Hogma Warriors"),
			new KillObjective(12, new[] { MonsterId.Hogma_Warrior }));

		AddObjective("killCockatries", L("Kill Cockatries"),
			new KillObjective(12, new[] { MonsterId.Cockatries }));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(29000));
		AddReward(new ItemReward(640084, 2));
		AddReward(new ItemReward(640004, 9));
		AddReward(new ItemReward(640007, 9));
		AddReward(new ItemReward(640012, 3));
	}
}
