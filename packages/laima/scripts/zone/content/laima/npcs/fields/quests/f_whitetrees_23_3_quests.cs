//--- Melia Script ----------------------------------------------------------
// Syla Forest - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_whitetrees_23_3 map.
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

public class FWhitetrees233QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Poisonous Blooms
		// =====================================================================
		// Gardener Petras - Keeping the carriage road clear
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Gardener] Petras"), "f_whitetrees_23_3", -755, 1148, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1001);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A stout gardener holds a pair of shears stained with pink pollen*{/}"));
				await dialog.Msg(L("Bless the forest, but not the Rafflet. Their pollen's been making the carriage horses wheeze - two drivers turned back to Orsha already this week."));

				var response = await dialog.Select(L("The garrison pays me to keep the road safe for trade. My back won't take swinging shears at twenty of them anymore. A young arm like yours would make short work of the blooms."),
					Option(L("I'll kill the Rafflet"), "help"),
					Option(L("Can't you just clear the path?"), "info"),
					Option(L("Sorry, not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He grins wide, revealing a missing tooth*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("They favor the damp patches along the eastern path. You'll hear them before you see them - they hum when they bloom."));
						await dialog.Msg(L("Oh, and leave the Fragolin and Cloverin be. They're harmless - the berry-sisters especially. Never hurt a soul."));
						break;

					case "info":
						await dialog.Msg(L("Oh, I've tried. Clear them one day, twice as many the next. They spread by spore and they spread fast."));
						await dialog.Msg(L("What they need is a proper thinning. Get their numbers down so the hedge can breathe."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. Tell any young adventurer you pass - there's good coin in it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRafflet", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He inspects your sleeves for pollen stains, approving*{/}"));
					await dialog.Msg(L("Twenty clean cuts! The carriage drivers will be singing your name by supper."));
					await dialog.Msg(L("Here - not much, but more than the garrison pays me by the week. Spend it well."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still blooming, are they? Eastern path, damp patches. Listen for the hum."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The road's sweet-smelling again. Thank you kindly."));
			}
		});

		// =====================================================================
		// QUEST 1002: First Letter Home
		// =====================================================================
		// Novice Milda - First-time adventurer's letter to her mother
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Novice] Milda"), "f_whitetrees_23_3", -537, 344, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1002);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young adventurer folds and unfolds a letter, sitting on a log near a cluster of curious-looking Fragolin*{/}"));
				await dialog.Msg(L("{#666666}*The small berry-sprites are watching her intently - six nearly identical little plant-girls, heads tilted the same way*{/}"));
				await dialog.Msg(L("Don't worry about them - the Fragolin are just nosy. They all look the same because they are the same, sort of. Sisters from the same root-cluster, I think."));

				var response = await dialog.Select(L("I wrote my mother yesterday to tell her I made it to Syla, but... I can't bring myself to walk back to Orsha. She'd take one look at my scraped knees and drag me home. Could you take the letter for me? Courier Sigute is at the Orsha gate-warp."),
					Option(L("I'll carry your letter"), "help"),
					Option(L("Scraped knees aren't so bad"), "info"),
					Option(L("Mail it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She brightens and carefully seals the letter with a wax-dot*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Oh - and if the berry-sisters follow you, don't shoo them. They just like watching people. They followed me for an hour yesterday."));
						await dialog.Msg(L("{#666666}*Three Fragolin tilt their heads at exactly the same angle*{/}"));
						break;

					case "info":
						await dialog.Msg(L("Have you met my mother? Scraped knee, lecture for an hour. Broken branch, lecture for two."));
						await dialog.Msg(L("I have to prove I can handle it out here. Starting with not going home yet."));
						break;

					case "leave":
						await dialog.Msg(L("It's one walk. You could do it on the way to Orsha anyway."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverLetter", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She exhales visibly*{/}"));
					await dialog.Msg(L("Sigute took it! Oh, I can breathe again. Mother will get it by tomorrow and won't send a search party."));
					await dialog.Msg(L("Take these - I saved them from my travel stipend. You earned them more than the tavern in Orsha would."));
					await dialog.Msg(L("{#666666}*Two Fragolin-sisters wave at you in perfect unison as you turn to leave*{/}"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Sigute's at the Orsha warp. North edge of the forest - big clearing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Mother wrote back. Three pages of lecture, one page of recipes. I'll take it as a win."));
			}
		});

		// =====================================================================
		// Courier Sigute - Recipient for Quest 1002
		//---------------------------------------------------------------------
		AddNpc(147418, L("[Courier] Sigute"), "f_whitetrees_23_3", -552, 1400, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1002);

			dialog.SetTitle(L("Sigute"));

			if (!character.Quests.IsActive(questId))
			{
				await dialog.Msg(L("{#666666}*A courier checks her bag, tallying scrolls and small parcels*{/}"));
				await dialog.Msg(L("Mail to Orsha? Bring it here if you've got one. I leave at sunset."));
				return;
			}

			var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_3.Quest1002.Delivered", 0) >= 1;

			if (delivered)
			{
				await dialog.Msg(L("The letter's safely in my bag. Tell Milda her mother will have it by tomorrow noon."));
				return;
			}

			await dialog.Msg(L("{#666666}*She accepts the letter and tucks it into an inner pocket*{/}"));
			await dialog.Msg(L("Milda's handwriting - I recognize the loop on the M. Third letter she's sent this month through me."));
			await dialog.Msg(L("{#666666}*She smiles knowingly*{/}"));
			await dialog.Msg(L("Tell her not to worry - I've never lost a letter yet. And tell her to come back when she's ready, not before."));

			character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_3.Quest1002.Delivered", 1);
			character.ServerMessage(L("{#FFD700}Letter delivered. Return to Milda.{/}"));
		});

		// =====================================================================
		// QUEST 1003: Sweet Harvest
		// =====================================================================
		// Baker Ona - Orsha harvest festival preparations
		//---------------------------------------------------------------------
		AddNpc(20161, L("[Baker] Ona"), "f_whitetrees_23_3", 161, 1222, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1003);

			dialog.SetTitle(L("Ona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A flour-dusted baker inspects a basket of red berries, sorting ripe from green*{/}"));
				await dialog.Msg(L("The harvest festival opens in Orsha in three days and I've only half the fireberries I need for the warding-tarts."));

				var response = await dialog.Select(L("Syla's fireberries only grow where the Fragolin-sisters tend them. You've seen the little plant-girls? They're all sisters, every last one - same root, same face, same curious eyes. They don't mind sharing the ripe berries if you ask properly."),
					Option(L("I'll gather five for you"), "help"),
					Option(L("How do you 'ask properly'?"), "info"),
					Option(L("Not my kind of work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pats flour off her apron*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Look for bushes with a sister or two lingering nearby. That's the sign the fruit is ready."));
						await dialog.Msg(L("If a Fragolin startles - sometimes a shy sister jumps out when you reach for a berry - don't hurt her. They hum when they're upset, just wait and they'll settle."));
						break;

					case "info":
						await dialog.Msg(L("Oh, nothing mystical. Crouch down so you're their height, show your empty hands, wait a moment."));
						await dialog.Msg(L("A fireberry sister will nod at the ripe ones - they know which are ready to give. Take only those. Leave the green. Leave the last."));
						await dialog.Msg(L("My grandmother taught me the song, but it's not required. Just manners."));
						break;

					case "leave":
						await dialog.Msg(L("Then enjoy the festival next week. You'll miss the tarts."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var berryCount = character.Inventory.CountItem(663299);

				if (berryCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She holds a berry to the light - the juice inside glows faintly orange*{/}"));
					await dialog.Msg(L("Five beauties. All ripe, all offered - I can tell. The sisters only shed their glow when the taking is kind."));
					await dialog.Msg(L("Take these, dear. The festival will remember your name in every warding-tart that leaves my oven."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Five fireberries, ripe and red. The Fragolin-sisters will help you find them - just be patient."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The warding-tarts sold out in an hour. Orsha's children have sticky red fingers for a week every year because of your help."));
			}
		});

		// =====================================================================
		// FRAGOLIN-TENDED BERRY BUSHES
		// =====================================================================
		// For Quest 1003 - Sweet Harvest
		// =====================================================================

		void AddFireberryBush(int bushNumber, int x, int z, int direction)
		{
			AddNpc(152017, L("Fireberry Bush"), "f_whitetrees_23_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_3", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A low bush heavy with red berries. A Fragolin-sister peeks at you from between the leaves, then vanishes*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{bushNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The ripe berries are already gone. A green one blinks into red, promising tomorrow's fruit*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{bushNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Fragolin, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A startled Fragolin-sister jumps from the bush!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Asking the berry-sisters..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(663299, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(663299);
					character.ServerMessage(LF("Fireberries gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All berries gathered! Return to Baker Ona.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering cancelled."));
				}
			});
		}

		AddFireberryBush(1, -828, 310, 0);
		AddFireberryBush(2, 158, 1227, 90);
		AddFireberryBush(3, -648, 1071, 180);
		AddFireberryBush(4, -546, 330, 270);
		AddFireberryBush(5, -486, 1334, 0);

		// =====================================================================
		// QUEST 1004: The Lost Apprentice
		// =====================================================================
		// Wise Woman Rasa - Searching for Eglė's trail
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Wise Woman] Rasa"), "f_whitetrees_23_3", -1720, -479, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_whitetrees_23_3", 1004);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An elder woman with herb-stained fingers watches a trio of Fragolin-sisters skip in a circle nearby*{/}"));
				await dialog.Msg(L("My apprentice Eglė ran into Syla yesterday chasing a Cloverin she wanted to sketch. She didn't come back for supper."));

				var response = await dialog.Select(L("The berry-sisters saw her - I can tell by how they keep looking east. But they don't speak in words a human ear understands. I need someone to walk her trail and confirm she reached Parias safely. She dropped things as she ran - a scarf, a waterskin, her practice staff, and her sketchbook."),
					Option(L("I'll follow her trail"), "help"),
					Option(L("Is she in danger?"), "info"),
					Option(L("Children get lost all the time"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She touches your wrist briefly in thanks*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The Fragolin-sisters will watch over you as you walk. They followed Eglė yesterday - they'll follow you too."));
						await dialog.Msg(L("{#666666}*Two Fragolin-sisters nod in perfect synchrony from the underbrush*{/}"));
						break;

					case "info":
						await dialog.Msg(L("Cloverin won't hurt her - they flee before they fight. But the Rafflet pollen gives little lungs trouble, and Eglė is small."));
						await dialog.Msg(L("If her trail turns sharply, or doubles back, I'll need to know. That would tell me she was fleeing something."));
						break;

					case "leave":
						await dialog.Msg(L("True. But this child is mine to worry over. I'll find another walker."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var trailVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited", 0);

				if (trailVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*She closes her eyes and listens as you describe each item's position*{/}"));
					await dialog.Msg(L("Straight line west. No running, no panic. She was walking when she dropped each one - reaching for Fragolin-sisters to carry them, probably."));
					await dialog.Msg(L("She made it to Parias, then. My apprentice is cleverer than I give her credit for. Take these, traveler - a grandmother's thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four items between here and Parias. Follow the path west, look for what shouldn't be on the forest floor."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Eglė came back this morning. The Parias warden fed her porridge for two days. She drew six Cloverin and eleven Fragolin-sisters."));
			}
		});

		// =====================================================================
		// APPRENTICE'S TRAIL ITEMS
		// =====================================================================
		// For Quest 1004 - The Lost Apprentice
		// =====================================================================

		void AddTrailItem(int itemNumber, string itemName, string observation, int x, int z, int direction)
		{
			AddNpc(154035, L(itemName), "f_whitetrees_23_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_whitetrees_23_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A small personal item lies half-hidden in the underbrush*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_whitetrees_23_3.Quest1004.Trail{itemNumber}";
				var observed = character.Variables.Perm.GetBool(variableKey, false);

				if (observed)
				{
					await dialog.Msg(L("{#666666}*You've already examined this item*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Examining..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var trailVisited = character.Variables.Perm.GetInt("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited", trailVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Trail items examined: {0}/4", trailVisited + 1));

					if (trailVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}Trail complete! Return to Wise Woman Rasa.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Examination interrupted."));
				}
			});
		}

		AddTrailItem(1, "Eglė's Scarf", "The scarf lies folded, not torn. She set it down calmly.", -347, 1094, 180);
		AddTrailItem(2, "Eglė's Waterskin", "The waterskin is half-empty, cap neatly fastened. No panic.", -810, 637, 90);
		AddTrailItem(3, "Eglė's Practice Staff", "The staff leans against a trunk, as if she meant to come back for it.", -1518, -266, 0);
		AddTrailItem(4, "Eglė's Sketchbook", "The last page shows a careful drawing of a Cloverin. No smudges, no haste.", -1720, -479, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Poisonous Blooms
//-----------------------------------------------------------------------------

public class PoisonousBloomsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1001);
		SetName("Poisonous Blooms");
		SetType(QuestType.Sub);
		SetDescription("Gardener Petras needs the Rafflet thinned before their pollen turns away any more Orsha caravans.");
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Gardener] Petras", "f_whitetrees_23_3");

		AddObjective("killRafflet", "Kill Rafflet blooms",
			new KillObjective(20, new[] { MonsterId.Rafflet }));

		AddReward(new ExpReward(500, 340));
		AddReward(new SilverReward(1200));
		AddReward(new ItemReward(640081, 1));  // Lv2 EXP Card
		AddReward(new ItemReward(640002, 2)); // Small HP Potion
		AddReward(new ItemReward(640005, 2)); // Small SP Potion
		AddReward(new ItemReward(640008, 1));  // Small Stamina Potion
	}
}

// Quest 1002 CLASS: First Letter Home
//-----------------------------------------------------------------------------

public class FirstLetterHomeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1002);
		SetName("First Letter Home");
		SetType(QuestType.Sub);
		SetDescription("Novice Milda needs her letter home carried to Courier Sigute at the Orsha warp - she can't bring herself to walk back yet.");
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Novice] Milda", "f_whitetrees_23_3");

		AddObjective("deliverLetter", "Deliver Milda's letter to Courier Sigute",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_3.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(1600));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Card
		AddReward(new ItemReward(640002, 2)); // Small HP Potion
		AddReward(new ItemReward(640005, 2)); // Small SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Sweet Harvest
//-----------------------------------------------------------------------------

public class SweetHarvestQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1003);
		SetName("Sweet Harvest");
		SetType(QuestType.Sub);
		SetDescription("Baker Ona needs five ripe fireberries for Orsha's harvest festival - gathered with patience from the bushes tended by the Fragolin-sisters.");
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Baker] Ona", "f_whitetrees_23_3");

		AddObjective("collectBerries", "Gather ripe fireberries from Fragolin-tended bushes",
			new CollectItemObjective(663299, 5));

		AddReward(new ExpReward(1200, 800));
		AddReward(new SilverReward(1600));
		AddReward(new ItemReward(640081, 2));  // Lv2 EXP Card
		AddReward(new ItemReward(640002, 2)); // Small HP Potion
		AddReward(new ItemReward(640005, 2)); // Small SP Potion
		AddReward(new ItemReward(640008, 1));  // Small Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663299,
			character.Inventory.CountItem(663299),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663299,
			character.Inventory.CountItem(663299),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1003.Bush{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Lost Apprentice
//-----------------------------------------------------------------------------

public class TheLostApprenticeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_whitetrees_23_3", 1004);
		SetName("The Lost Apprentice");
		SetType(QuestType.Sub);
		SetDescription("Wise Woman Rasa has asked you to follow her apprentice Eglė's trail through Syla Forest - four dropped items leading toward the Parias warp.");
		SetLocation("f_whitetrees_23_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wise Woman] Rasa", "f_whitetrees_23_3");

		AddObjective("followTrail", "Follow Eglė's trail of dropped items",
			new VariableCheckObjective("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited", 4, true));

		AddReward(new ExpReward(1600, 1100));
		AddReward(new SilverReward(2000));
		AddReward(new ItemReward(640081, 3));  // Lv2 EXP Card
		AddReward(new ItemReward(640002, 3)); // Small HP Potion
		AddReward(new ItemReward(640005, 3)); // Small SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1004.Trail{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_whitetrees_23_3.Quest1004.TrailVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_whitetrees_23_3.Quest1004.Trail{i}");
		}
	}
}
