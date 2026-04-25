//--- Melia Script ----------------------------------------------------------
// Knidos Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Knidos Jungle continuing the poison corruption storyline.
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

public class FBracken632QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Knidos Frontier Watch
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Scout] Kaelin"), "f_bracken_63_2", 560, 2090, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1001);

			dialog.SetTitle(L("Kaelin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You came through the Koru pass? Brave traveler. The deeper jungle is worse."));
				await dialog.Msg(L("Loktanun swarm the frontier here. Their bite carries the same corruption rotting these trees."));

				var response = await dialog.Select(L("What can I do to help?"),
					Option(L("I'll thin their numbers"), "help"),
					Option(L("Why are they so aggressive?"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-two Loktanun along the frontier?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Head south from here, they're everywhere along the tree line."));
							await dialog.Msg(L("Twenty-two should cripple their pack for a while."));
						}
						break;

					case "info":
						await dialog.Msg(L("The corruption seeps up from the roots of the jungle itself."));
						await dialog.Msg(L("Every creature that drinks the poisoned water turns hostile, eventually."));
						await dialog.Msg(L("The Loktanun drank early and fell fastest."));
						break;

					case "leave":
						await dialog.Msg(L("Don't wander off the marked trails. The mist hides worse than Loktanun."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLoktanun", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("You did it. The frontier will be quiet for at least a few days now."));
					await dialog.Msg(L("Take this - you earned it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep pushing the Loktanun back. The frontier isn't secure yet."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Loktanun packs haven't fully reformed. You bought us real time."));
			}
		});

		// Quest 2: Corrupted Sap
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Alchemist] Vila"), "f_bracken_63_2", 240, 320, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1002);

			dialog.SetTitle(L("Vila"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A living sample, please! I need fresh corrupted sap from the Root Crystals deeper in the jungle."));
				await dialog.Msg(L("Their sap is a concentrated form of the poison. If I can isolate what corrupts it, I can start brewing an antidote."));

				var response = await dialog.Select(L("Can you gather the samples?"),
					Option(L("I'll collect five samples"), "help"),
					Option(L("Why Root Crystals?"), "info"),
					Option(L("Too risky"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Bring back five vials of corrupted sap from Root Crystal formations?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Five should be enough to begin. Tap the crystal formations carefully with the sample vial."));
							await dialog.Msg(L("They're scattered around the center of the jungle. Be cautious - some react poorly when disturbed."));
						}
						break;

					case "info":
						await dialog.Msg(L("Root Crystals draw poison straight from the earth, concentrating it."));
						await dialog.Msg(L("Their sap is potent enough that I can study the corruption itself, not just its effects."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. The jungle eats careless people."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sapCount = character.Inventory.CountItem(650527);

				if (sapCount >= 5)
				{
					await dialog.Msg(L("All five vials! And the hue is exactly what I feared - sickly green, not the healthy amber."));
					await dialog.Msg(L("This will keep my lab busy for weeks. Thank you."));

					character.Inventory.Remove(650527, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("I still need more samples. You have {0} of five.", sapCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The antidote research is progressing. Slow work, but real progress."));
			}
		});

		// Sap Collection Points
		//-------------------------------------------------------------------------
		void AddSapNode(int nodeNum, int x, int z, int direction)
		{
			AddNpc(47224, L("Bleeding Root Crystal"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1002);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1002.Sap{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_bracken_63_2.Quest1002.Sap{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A root crystal oozes thick green sap*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This crystal has already been tapped*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Loktanun, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}Loktanun charge from the undergrowth!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Extracting sap..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650527, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Collected: Corrupted Sap Vial"));

					var currentCount = character.Inventory.CountItem(650527);
					character.ServerMessage(LF("Sap vials collected: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All samples gathered! Return to Vila.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Extraction cancelled."));
				}
			});
		}

		AddSapNode(1, 666, 2012, 0);
		AddSapNode(2, 543, 1732, 0);
		AddSapNode(3, 308, 1067, 0);
		AddSapNode(4, 639, 726, 0);
		AddSapNode(5, 393, 411, 0);

		// Quest 3: Silencing the Shamans
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Witch Hunter] Alaric"), "f_bracken_63_2", -1280, 0, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1003);

			dialog.SetTitle(L("Alaric"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Lapasape Mages on the western ridge aren't merely corrupted - they're channeling it."));
				await dialog.Msg(L("They've planted blood totems across the ridge that drink in the poison and amplify it back into their spells."));

				var response = await dialog.Select(L("What needs to be done?"),
					Option(L("I'll cut them down and smash the totems"), "help"),
					Option(L("Why do the totems matter?"), "info"),
					Option(L("Handle it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Slay twelve Lapasape Mages and shatter four blood totems?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("The totems are planted across the western ridge. Break them with your weapon - they resist magic."));
							await dialog.Msg(L("Without the totems the shamans are just tainted beasts. Thin them first so you can work in peace."));
						}
						break;

					case "info":
						await dialog.Msg(L("Take a totem down and every spell within a hundred paces loses half its bite."));
						await dialog.Msg(L("Take all four down and the shamans can barely conjure at all."));
						break;

					case "leave":
						await dialog.Msg(L("If you change your mind, the ridge is west of here. You can smell it before you see it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLapasape", out var killObj)) return;
				if (!quest.TryGetProgress("shatterTotems", out var totemObj)) return;

				if (killObj.Done && totemObj.Done)
				{
					await dialog.Msg(L("Totems broken, shamans slain. The ridge is just a ridge again."));
					await dialog.Msg(L("Corruption still lingers in the earth, but the amplification is gone. That's a day's good work."));

					character.Inventory.Remove(650378, character.Inventory.CountItem(650378), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Cut down more Lapasape Mages. ");
					if (!totemObj.Done)
						status += L("Shatter the remaining blood totems. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The ridge is calmer now. The shamans that remain are just beasts, not priests."));
			}
		});

		// Blood Totem Shatter Points
		//-------------------------------------------------------------------------
		void AddBloodTotem(int totemNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Blood Totem"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1003);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1003.Totem{totemNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered totem stained with dried blood*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This totem has already been shattered*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Shattering totem..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650378, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Shattered: Blood Totem"));

					var currentCount = character.Inventory.CountItem(650378);
					character.ServerMessage(LF("Totems shattered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All totems broken! Return to Alaric.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Shattering interrupted."));
				}
			});
		}

		AddBloodTotem(1, -1500, -1400, 0);
		AddBloodTotem(2, -1620, -500, 0);
		AddBloodTotem(3, -1400, 200, 0);
		AddBloodTotem(4, -1300, -700, 0);

		// Quest 4: The Missing Survey
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Scribe] Petras"), "f_bracken_63_2", 310, -340, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1004);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I was mapping the corruption spread when a flock of Ponpon tore through my camp."));
				await dialog.Msg(L("My survey pages scattered everywhere. Months of work, blown across this wretched jungle."));

				var response = await dialog.Select(L("Can you help recover them?"),
					Option(L("I'll find your pages and clear the flock"), "help"),
					Option(L("What's in the survey?"), "info"),
					Option(L("Write it again"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Recover five scattered pages and thin the Ponpon flock by ten?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("They blew north and west of my camp. Five pages - each is sealed, you can't miss them."));
							await dialog.Msg(L("And please, put down at least ten Ponpon so I can work without being dive-bombed."));
						}
						break;

					case "info":
						await dialog.Msg(L("I'm charting how fast the corruption advances, where it stalls, where it accelerates."));
						await dialog.Msg(L("If we understand the pattern, maybe we can predict where it strikes next."));
						break;

					case "leave":
						await dialog.Msg(L("Redo six months of field work? You're joking."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPonpon", out var killObj)) return;
				var pageCount = character.Inventory.CountItem(650445);

				if (killObj.Done && pageCount >= 5)
				{
					await dialog.Msg(L("All five pages. And the Ponpon sound quieter already."));
					await dialog.Msg(L("I can resume the survey. This might actually save lives, you understand? Real lives."));

					character.Inventory.Remove(650445, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Thin the Ponpon flock. ");
					if (pageCount < 5)
						status += L("Find more scattered survey pages. ");

					await dialog.Msg(LF("Keep searching. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The survey is whole again. Already I see a pattern - the corruption flows downhill, always toward water."));
			}
		});

		// Survey Page Collection Points
		//-------------------------------------------------------------------------
		void AddSurveyPage(int pageNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Scattered Survey Page"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1004.Page{pageNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A sealed parchment flutters in the breeze*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this page*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering page..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650445, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Survey Page"));

					var currentCount = character.Inventory.CountItem(650445);
					character.ServerMessage(LF("Pages recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All pages recovered! Return to Petras.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Recovery cancelled."));
				}
			});
		}

		AddSurveyPage(1, 700, -1500, 0);
		AddSurveyPage(2, 400, -1200, 0);
		AddSurveyPage(3, 900, -1000, 0);
		AddSurveyPage(4, 150, -1300, 0);
		AddSurveyPage(5, 520, -780, 0);

		// Quest 5: The Tainted Alpha
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Tracker] Niko"), "f_bracken_63_2", 1050, -1080, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_bracken_63_2.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Niko"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Something in the southern nesting ground is wrong - bigger than wrong."));
				await dialog.Msg(L("The Kanchobirds down there have been dragged into something else. Something's been feeding them, pushing them."));
				await dialog.Msg(L("There's an alpha. I've seen its tracks. Twice the size of the flock, and the corruption's thicker in it."));

				var response = await dialog.Select(L("Want me to hunt it?"),
					Option(L("I'll hunt the alpha"), "help"),
					Option(L("What kind of alpha?"), "info"),
					Option(L("Not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin the Kanchobird flock first, then slay the alpha when it shows itself?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Kill ten birds to bait it out. The alpha won't emerge while the flock is full."));
							await dialog.Msg(L("When the flock thins, it'll come hunting. Come back here then and I'll flush it into the open for you."));
						}
						break;

					case "info":
						await dialog.Msg(L("Not a Kanchobird anymore. Something twisted - bigger claws, poison drooling from the beak."));
						await dialog.Msg(L("Might be Tanu-kin turned rogue. Whatever it is, it shouldn't exist."));
						break;

					case "leave":
						await dialog.Msg(L("Your choice. Just don't blame me when it wanders north."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killKanchobirds", out var flockObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (flockObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Dead? You're certain? Show me a claw."));
					await dialog.Msg(L("Gods. That's bigger than I remembered. Good work - the nesting ground will recover without that thing spreading corruption."));

					character.Variables.Perm.Remove(alphaSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (flockObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Tanu, 1, 150, TimeSpan.FromMinutes(3)))
						{
							await dialog.Msg(L("The flock is thinned enough. I can hear it moving - south, toward the old nesting ground."));
							await dialog.Msg(L("{#FF6666}Go, quickly! It's emerging now!{/}"));
							character.ServerMessage(L("{#FF6666}The Tainted Alpha emerges from the nesting ground!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("The alpha's out there. Don't let it slip back into the brush."));
					}
				}
				else
				{
					await dialog.Msg(L("Thin the flock first. The alpha won't show itself until it thinks the nest is threatened."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Haven't seen another alpha since. Whatever was feeding them must have given up on this spot."));
			}
		});

		// Quest 6: The Stoup Camp Route
		//-------------------------------------------------------------------------
		AddNpc(155145, L("[Pathfinder] Elvira"), "f_bracken_63_2", 705, 495, 135, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1006);

			dialog.SetTitle(L("Elvira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stoup Camp is the last safe waypoint east of Knidos, but the route to it is a mess."));
				await dialog.Msg(L("Loktanun from the north and Ponpon from the ridges have flooded the whole length of trail."));

				var response = await dialog.Select(L("Can you help clear the route?"),
					Option(L("I'll clear both packs"), "help"),
					Option(L("How bad is the overlap?"), "info"),
					Option(L("Use another route"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Clear twelve Loktanun and twelve Ponpon along the Stoup route?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Their territories overlap along the trail, so you'll run into both."));
							await dialog.Msg(L("Even numbers should give the caravans a clean passage for a week or two."));
						}
						break;

					case "info":
						await dialog.Msg(L("The Loktanun own the ground, the Ponpon own the air above it."));
						await dialog.Msg(L("Travelers get pincered between them. Nothing moves east without paying a toll in blood."));
						break;

					case "leave":
						await dialog.Msg(L("There isn't another route. That's the whole problem."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLoktanun", out var lokObj)) return;
				if (!quest.TryGetProgress("killPonpon", out var ponObj)) return;

				if (lokObj.Done && ponObj.Done)
				{
					await dialog.Msg(L("Route's clear. The caravan masters are going to cry when they hear."));
					await dialog.Msg(L("Well-earned work. Take this with my thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!lokObj.Done)
						status += L("Clear more Loktanun. ");
					if (!ponObj.Done)
						status += L("Thin the Ponpon flocks. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Caravans are running again. First time in months."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Knidos Frontier Watch
//-----------------------------------------------------------------------------

public class KnidosFrontierWatchQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1001);
		SetName(L("Knidos Frontier Watch"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill corrupted Loktanun pressing on the northern frontier of Knidos Jungle."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scout] Kaelin"), "f_bracken_63_2");

		AddObjective("killLoktanun", L("Kill frontier Loktanun"),
			new KillObjective(22, new[] { MonsterId.Loktanun }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: Corrupted Sap
//-----------------------------------------------------------------------------

public class CorruptedSapQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1002);
		SetName(L("Corrupted Sap"));
		SetType(QuestType.Sub);
		SetDescription(L("Collect corrupted sap samples from Root Crystal formations for the alchemist Vila."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Alchemist] Vila"), "f_bracken_63_2");

		AddObjective("collectSap", L("Collect corrupted sap vials"),
			new CollectItemObjective(650527, 5));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(13000));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650527, character.Inventory.CountItem(650527), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650527, character.Inventory.CountItem(650527), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: Silencing the Shamans
//-----------------------------------------------------------------------------

public class SilencingTheShamansQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1003);
		SetName(L("Silencing the Shamans"));
		SetType(QuestType.Sub);
		SetDescription(L("Slay Lapasape Mages on the western ridge and shatter the blood totems amplifying their corruption."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Witch Hunter] Alaric"), "f_bracken_63_2");

		AddObjective("killLapasape", L("Slay Lapasape Mages"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Mage }));

		AddObjective("shatterTotems", L("Shatter blood totems"),
			new CollectItemObjective(650378, 4));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650378, character.Inventory.CountItem(650378), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1003.Totem{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650378, character.Inventory.CountItem(650378), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1003.Totem{i}");
		}
	}
}

// Quest 1004 CLASS: The Missing Survey
//-----------------------------------------------------------------------------

public class TheMissingSurveyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1004);
		SetName(L("The Missing Survey"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover the scribe's scattered survey pages and thin the Ponpon flock harassing his camp."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Scribe] Petras"), "f_bracken_63_2");

		AddObjective("killPonpon", L("Thin the Ponpon flock"),
			new KillObjective(10, new[] { MonsterId.Ponpon }));

		AddObjective("recoverPages", L("Recover scattered survey pages"),
			new CollectItemObjective(650445, 5));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(16000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650445, character.Inventory.CountItem(650445), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1004.Page{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650445, character.Inventory.CountItem(650445), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1004.Page{i}");
		}
	}
}

// Quest 1005 CLASS: The Tainted Alpha
//-----------------------------------------------------------------------------

public class TheTaintedAlphaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1005);
		SetName(L("The Tainted Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Kanchobird flock to bait out the tainted alpha, then slay it before it spreads the corruption."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Tracker] Niko"), "f_bracken_63_2");

		AddObjective("killKanchobirds", L("Thin the Kanchobird flock"),
			new KillObjective(10, new[] { MonsterId.Kanchobird }));

		AddObjective("killAlpha", L("Slay the tainted alpha"),
			new KillObjective(1, new[] { MonsterId.Tanu }));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(531122, 1)); // Plate Armor
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_2.Quest1005.AlphaSpawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_bracken_63_2.Quest1005.AlphaSpawned");
	}
}

// Quest 1006 CLASS: The Stoup Camp Route
//-----------------------------------------------------------------------------

public class TheStoupCampRouteQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1006);
		SetName(L("The Stoup Camp Route"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Loktanun and Ponpon from the trail between Knidos Jungle and Stoup Camp."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Elvira"), "f_bracken_63_2");

		AddObjective("killLoktanun", L("Clear Loktanun from the trail"),
			new KillObjective(12, new[] { MonsterId.Loktanun }));

		AddObjective("killPonpon", L("Thin Ponpon from the trail"),
			new KillObjective(12, new[] { MonsterId.Ponpon }));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(24000));
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 14)); // Normal HP Potion
		AddReward(new ItemReward(640006, 10)); // Normal SP Potion
		AddReward(new ItemReward(501122, 1)); // Plate Gauntlets
	}
}
