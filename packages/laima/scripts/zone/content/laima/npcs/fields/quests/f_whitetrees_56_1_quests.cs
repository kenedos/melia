//--- Melia Script ----------------------------------------------------------
// Mishekan Forest - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_56_1. A gentle white-leaf forest
// of peaceful flower-creatures, patrolled at night by a Castle Gardener who
// prunes the saplings while no one watches.
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

public class FWhitetrees561QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Floron Spore-Drift
		// =====================================================================
		AddNpc(20109, L("[Forester] Milda"), "f_whitetrees_56_1", 850, 280, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_56_1", 1001);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A forester in a patched white-leaf cloak brushes drifting Floron spore off her shoulder. The spore leaves a faint glow that fades slowly*{/}"));
				await dialog.Msg(L("Florons are gentle things, but their spore-drift is thick this season. Woodcutter Drius's lads are sneezing themselves useless. Twenty Florons thinned clears the drift for a month."));

				var response = await dialog.Select(L("Twenty Florons put down, gently. They don't fight back - quick clean strikes. Will you?"),
					Option(L("I'll thin twenty Florons"), "help"),
					Option(L("Gentle how?"), "info"),
					Option(L("Can't the woodcutters wear masks?"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Twenty. Cluster east of the white-leaf grove. Clean strikes, no lingering - Florons take a long time to die if wounded shallowly. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Don't breathe the spore. Mask your mouth with a scarf if you don't have a proper filter."));
						}
						break;

					case "info":
						await dialog.Msg(L("They don't attack. They drift, they bloom, they spore. If you leave them alone, they'll leave you alone - but the spore-drift doesn't care if you're hostile or not."));
						break;

					case "leave":
						await dialog.Msg(L("They do. Masks get sweaty, lads get slow, production drops. Thinning the Florons is cheaper than the lost timber."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFloron", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*She brushes spore from her cloak - and finds almost none*{/}"));
					await dialog.Msg(L("The drift dropped noticeably. Drius's lads can work a full shift again."));
					await dialog.Msg(L("Forester's pay. And a spore-scarf of my own - stitched close, breathes easy."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East cluster. Clean strikes. Twenty."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Woodcutters hit their quota two weeks running. Clean air makes better lumber, apparently. Florons will re-drift; we'll thin again."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Pressed-Flower Book
		// =====================================================================
		AddNpc(20107, L("[Botanist-Apprentice] Gerda"), "f_whitetrees_56_1", -180, 250, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_56_1", 1002);

			dialog.SetTitle(L("Gerda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young apprentice ties a pressed-flower book shut with a linen ribbon. The book's pages are thick with dried white-leaf specimens*{/}"));
				await dialog.Msg(L("My mentor Ona waits at the west fern-hollow for this book. I sprained my ankle tripping over a Budny - I can't walk it. Take it for me?"));

				var response = await dialog.Select(L("Carry the book to Ona. She'll press a ribbon-reply back. Will you?"),
					Option(L("I'll carry the book"), "help"),
					Option(L("Tripped over a Budny?"), "info"),
					Option(L("Botany isn't my business"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("West fern-hollow. Don't let the pressed flowers scatter - each one took me a week. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ona will offer you tea. Accept - it's white-leaf brew, best in the region."));
						}
						break;

					case "info":
						await dialog.Msg(L("They're peaceful but they don't move out of the way. I walked into one while reading. The Budny was fine. My ankle wasn't."));
						break;

					case "leave":
						await dialog.Msg(L("Then my mentor waits another week. She'll forgive me; she always does. But the specimens won't press properly past their peak."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverBook", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unties the ribbon-reply and reads Ona's inked note with visible relief*{/}"));
					await dialog.Msg(L("She accepted all twelve specimens and added three of her own to press. The collection will reach the capital by market-day."));
					await dialog.Msg(L("Apprentice's coin. And the ribbon back - keep it as a travel-charm. Ona always ties forest-luck into her knots."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("West fern-hollow. Don't scatter the flowers."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The collection reached the capital. Three specimens were new to the regional catalogue. Ona wrote me proud; I'm buying new ankle-wrap with the credit."));
			}
		});

		// Quest 1002 recipient
		AddNpc(20114, L("[Botanist] Ona"), "f_whitetrees_56_1", -800, 800, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_56_1", 1002);

			dialog.SetTitle(L("Ona"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_56_1.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A silver-haired botanist unties the book with careful fingers, reads each pressed flower in lamplight, and nods slowly at the last page*{/}"));
					await dialog.Msg(L("Gerda's pressings are sharp this month. The white-leaf rose here - I haven't seen one catalogued so clean in four years."));
					await dialog.Msg(L("{#666666}*She unties a forest-luck knot from her wrist, ties it into a fresh ribbon, and passes the ribbon back*{/}"));
					await dialog.Msg(L("Ribbon-reply. Tell her the book goes to the capital by market-day, and her ankle will mend faster if she brews the white-leaf root I sent last week."));

					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_56_1.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Ona's ribbon-reply received. Return to Botanist-Apprentice Gerda.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the ribbon to Gerda. She'll know the knot by its weave."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The capital catalogue accepted all three new specimens. Gerda will be a full botanist within a year at this pace."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*She presses a fresh specimen between wax-papers*{/}"));
				await dialog.Msg(L("Fern-hollow visits only. If you've no book to deliver, mind the pressing-papers on the bench."));
			}
		});

		// =====================================================================
		// QUEST 1003: Budny Pollen-Sacs
		// =====================================================================
		AddNpc(20017, L("[Beekeeper] Dalia"), "f_whitetrees_56_1", -400, -200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_56_1", 1003);

			dialog.SetTitle(L("Dalia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A beekeeper in fine mesh lifts a Budny pollen-sac with forceps and inspects its colors under a loupe*{/}"));
				await dialog.Msg(L("Budny pollen flavors the finest white-leaf honey. The Budnies shed pollen-sacs naturally at the season's turn - no harm in collecting. But I can't bend for them anymore."));

				var response = await dialog.Select(L("Eight pollen-sacs. Pick only the shed ones at the base of a Budny; never pull an attached sac. Will you?"),
					Option(L("I'll gather eight pollen-sacs"), "help"),
					Option(L("How do you tell shed from attached?"), "info"),
					Option(L("Bee-work isn't mine"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Shed sacs only. Base of the Budny, never the body. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Shed sacs are dull-colored. Attached sacs gleam. Dull means free; gleam means still the Budny's."));
						}
						break;

					case "info":
						await dialog.Msg(L("Color. That's all. Dull-colored and you take it; gleam-colored and you walk past. Budnies get upset when you pull an attached sac - not aggressive, just sad. I don't want them sad."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll find another gatherer. Or do without honey-flavor this year. Patrons will complain - quietly, because the honey's still good, just not finest."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sacCount = character.Inventory.CountItem(650856);
				if (sacCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She inspects each sac under the loupe. All eight pass*{/}"));
					await dialog.Msg(L("All shed. No gleam. You have a careful eye."));
					await dialog.Msg(L("Beekeeper's coin. And a small jar of last year's finest-grade honey - finest because of good gatherers before you. Fair trade."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Dull-colored. Base of the Budny. Eight."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("This year's finest-grade honey will be the best batch in six years. Patrons know when the pollen was gathered kindly - it tastes different."));
			}
		});

		void AddSacSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Shed Budny Sac"), "f_whitetrees_56_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_56_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A dull-colored pollen-sac rests at the base of a Budny-trail. Without a beekeeper's order, there's no reason to collect*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);
				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This sac has been taken. A small leaf-impression remains in the moss*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);
					if (SpawnTempMonsters(character, MonsterId.Budny, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Budny shuffles over, curious about the collector!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Gathering the shed sac..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650856, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					var currentCount = character.Inventory.CountItem(650856);
					character.ServerMessage(LF("Pollen-sacs gathered: {0}/8", currentCount));
					if (currentCount >= 8)
						character.ServerMessage(L("{#FFD700}All sacs gathered! Return to Beekeeper Dalia.{/}"));
				}
				else
				{
					character.ServerMessage(L("Gathering interrupted."));
				}
			});
		}

		AddSacSpot(1, -605, -268, 0);
		AddSacSpot(2, -450, -132, 90);
		AddSacSpot(3, -974, 354, 180);
		AddSacSpot(4, -993, 526, 270);
		AddSacSpot(5, -798, 811, 0);
		AddSacSpot(6, -725, 957, 90);
		AddSacSpot(7, -192, 513, 180);
		AddSacSpot(8, 129, 338, 270);

		// =====================================================================
		// QUEST 1004: The Night-Pruner's Mark
		// =====================================================================
		AddNpc(20151, L("[Sapling-Warden] Kristjonas"), "f_whitetrees_56_1", 450, 600, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_56_1", 1004);

			dialog.SetTitle(L("Kristjonas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young warden in sapling-green kneels beside a white-leaf sapling, examining a fresh cut at its lowest branch. The cut is clean - too clean for wind-break*{/}"));
				await dialog.Msg(L("The Castle Gardener walks the forest at night. I've never seen her, only her marks. Four saplings were flagged as candidates last month - I need to know which she's pruned and which she's left."));

				var response = await dialog.Select(L("Walk the four flagged saplings. Her pruning is clean, slanted upward, never torn. Report which saplings bear her mark and which don't. Will you?"),
					Option(L("I'll walk the four saplings"), "help"),
					Option(L("Why does her mark matter?"), "info"),
					Option(L("Gardeners don't need watching"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Four saplings. Clean upward-slant cut: her mark. Torn or no cut: not her mark. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If you see her at work - walk past, don't speak. She doesn't answer, and she doesn't like being seen."));
						}
						break;

					case "info":
						await dialog.Msg(L("Saplings she marks grow into the tallest white-leaves in the forest. Her taste is perfect - she picks the ones that will become timber-grade trees. We don't know how. We just catalog which ones she chose, so we know which ones not to fell."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself by moonlight. If I'm lucky, I'll see her this time - but she's never let me."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var saplingsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked", 0);
				if (saplingsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*He listens, marking a small sapling-map with green ink, and smiles faintly at the last entry*{/}"));
					await dialog.Msg(L("Sapling One: her mark. Sapling Two: her mark. Sapling Three: untouched. Sapling Four: her mark, fresh tonight."));
					await dialog.Msg(L("Three of four selected. Same ratio as last year. Her pace is steady. I flag these three as no-fell for the next fifty years; the forester-guild will honor it."));
					await dialog.Msg(L("Warden's stipend. And take this - a sapling-medallion. Won't mean much to anyone but the Gardener herself. She leaves the bearer alone."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four saplings. Clean upward-slant cut. Will you?"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three saplings formally no-fell. The guild posted the ribbons yesterday. Fifty years from now, three of the forest's tallest trees will trace back to this week's survey."));
			}
		});

		void AddSapling(int saplingNumber, string saplingName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(saplingName), "f_whitetrees_56_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_56_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A white-leaf sapling with a small green flag tied to its trunk. Without a warden's order, there's no reason to inspect its cuts*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_56_1.Quest1004.Sapling{saplingNumber}";
				var checkedSapling = character.Variables.Perm.GetBool(variableKey, false);
				if (checkedSapling)
				{
					await dialog.Msg(L("{#666666}*You have already read this sapling's branches*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Inspecting the sapling..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var saplingsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked", saplingsChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Saplings read: {0}/4", saplingsChecked + 1));
					if (saplingsChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All saplings read! Return to Sapling-Warden Kristjonas.{/}"));
				}
				else
				{
					character.ServerMessage(L("Inspection interrupted."));
				}
			});
		}

		AddSapling(1, "North Flagged Sapling", "North Sapling: lowest branch clean upward-slant cut. The Gardener's mark.", -170, 287, 0);
		AddSapling(2, "East Flagged Sapling", "East Sapling: two branches bear clean upward-slant cuts. Her mark, twice.", 867, 913, 90);
		AddSapling(3, "South Flagged Sapling", "South Sapling: no cuts. Untouched. Not her choice.", 558, -569, 180);
		AddSapling(4, "West Flagged Sapling", "West Sapling: fresh upward-slant cut, sap still beading. She was here tonight.", -1058, 356, 270);
	}
}

// Quest 1001
public class FloronSporeDriftQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_56_1", 1001);
		SetName("Floron Spore-Drift");
		SetType(QuestType.Sub);
		SetDescription("Forester Milda needs twenty Florons thinned to clear the spore-drift so the woodcutters can work a full shift.");
		SetLocation("f_whitetrees_56_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Forester] Milda", "f_whitetrees_56_1");
		AddObjective("killFloron", "Thin Florons in the spore-drift",
			new KillObjective(20, new[] { MonsterId.Floron }));
		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}
}

// Quest 1002
public class ThePressedFlowerBookQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_56_1", 1002);
		SetName("The Pressed-Flower Book");
		SetType(QuestType.Sub);
		SetDescription("Apprentice-Botanist Gerda's pressed-flower book must reach her mentor Ona at the west fern-hollow and return with a ribbon-reply.");
		SetLocation("f_whitetrees_56_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Botanist-Apprentice] Gerda", "f_whitetrees_56_1");
		AddObjective("deliverBook", "Deliver the book and return with the ribbon-reply",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_56_1.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_56_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_56_1.Quest1002.Delivered");
	}
}

// Quest 1003
public class BudnyPollenSacsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_56_1", 1003);
		SetName("Budny Pollen-Sacs");
		SetType(QuestType.Sub);
		SetDescription("Beekeeper Dalia needs eight shed Budny pollen-sacs gathered at the base of Budny-trails for her finest-grade white-leaf honey.");
		SetLocation("f_whitetrees_56_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Beekeeper] Dalia", "f_whitetrees_56_1");
		AddObjective("collectSacs", "Gather shed Budny pollen-sacs",
			new CollectItemObjective(650856, 8));
		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(11500));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
		AddReward(new ItemReward(640009, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650856, character.Inventory.CountItem(650856), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650856, character.Inventory.CountItem(650856), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004
public class TheNightPrunersMarkQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_56_1", 1004);
		SetName("The Night-Pruner's Mark");
		SetType(QuestType.Sub);
		SetDescription("Sapling-Warden Kristjonas has asked you to inspect four flagged saplings and report which bear the Castle Gardener's clean upward-slant cut.");
		SetLocation("f_whitetrees_56_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Sapling-Warden] Kristjonas", "f_whitetrees_56_1");
		AddObjective("inspectSaplings", "Inspect the four flagged saplings",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked", 4, true));
		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640082, 2));
		AddReward(new ItemReward(640003, 10));
		AddReward(new ItemReward(640006, 10));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1004.Sapling{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_56_1.Quest1004.SaplingsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_56_1.Quest1004.Sapling{i}");
	}
}
