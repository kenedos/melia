//--- Melia Script ----------------------------------------------------------
// Knidos Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Knidos Jungle continuing the poison corruption storyline.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
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
		AddNpc(20059, L("[Scout] Kaelin"), "f_bracken_63_2", 671, 1878, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1001);

			dialog.SetTitle(L("Kaelin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You came through Koru pass? You're brave. It gets worse the deeper you go."));
				await dialog.Msg(L("Loktanun are all over this frontier. Their bite carries the same corruption that's rotting these trees."));

				var response = await dialog.Select(L("Will you stand the watch with me?"),
					Option(L("I'll thin their numbers"), "help"),
					Option(L("Why are they so aggressive?"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						var waveSpawnedKey = "Laima.Quests.f_bracken_63_2.Quest1001.WavesSpawned";
						character.Quests.Start(questId);
						await dialog.Msg(L("Head south from here, they're everywhere along the tree line."));
						await dialog.Msg(L("Twenty-two should cripple their pack for a while."));

						if (!character.Variables.Perm.GetBool(waveSpawnedKey, false))
						{
							character.Variables.Perm.Set(waveSpawnedKey, true);
							await dialog.Msg(L("{#FF6666}Watch out, the Loktanun are coming!{/}"));

							_ = Task.Run(async () =>
							{
								for (var wave = 0; wave < 4; wave++)
								{
									if (character == null)
										return;

									SpawnTempMonsters(character, MonsterId.Loktanun, 6, 100, TimeSpan.FromMinutes(5));
									character.ServerMessage(L("{#FF6666}The Loktanun emerge from the trees!{/}"));

									if (wave < 3)
										await Task.Delay(TimeSpan.FromSeconds(15));
								}
							});
						}
						else
						{
							await dialog.Msg(L("They should be somewhere nearby..."));
						}
						break;

					case "info":
						await dialog.Msg(L("The corruption comes up from the roots, somehow."));
						await dialog.Msg(L("Anything that drinks the bad water goes hostile sooner or later."));
						await dialog.Msg(L("The Loktanun drank early. They were the first to turn."));
						break;

					case "leave":
						await dialog.Msg(L("Don't wander off the marked trails. There's worse than Loktanun in the mist."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLoktanun", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("You did it. Frontier should be quiet for a few days, at least."));
					await dialog.Msg(L("This dagger belonged to a scout who didn't make it back from the last sweep."));
					await dialog.Msg(L("Better in your hands than rusting in a footlocker. It's yours."));

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
		AddNpc(20114, L("[Alchemist] Vila"), "f_bracken_63_2", 273, 680, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1002);

			dialog.SetTitle(L("Vila"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I need a fresh sample, please! Sap from the Tainted Bracken clusters growing around the village."));
				await dialog.Msg(L("The bracken pulls poison up out of the soil and bleeds it out as red sap. If I can figure out what's corrupting it, I can start working on an antidote."));

				var response = await dialog.Select(L("Will you gather the samples?"),
					Option(L("I'll collect six samples"), "help"),
					Option(L("Why the bracken?"), "info"),
					Option(L("Too risky"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Six should be enough to start with. Tap the bleeding stems carefully with the vial."));
						await dialog.Msg(L("They're right at the edges of the village — you can't miss the red ones. Be careful, though. Some react badly when you disturb them."));
						break;

					case "info":
						await dialog.Msg(L("Tainted Bracken roots are shallow, but deep enough to soak up whatever's poisoning the ground."));
						await dialog.Msg(L("The sap is strong enough that I can study the corruption directly, not just what it does."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. The jungle isn't kind to careless people."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sapCount = character.Inventory.CountItem(650527);

				if (sapCount >= 6)
				{
					await dialog.Msg(L("All six vials! And the color's exactly what I was afraid of — sickly red, not the amber you'd see in healthy bracken."));
					await dialog.Msg(L("This will keep me busy in the lab for weeks. Thank you."));

					character.Inventory.Remove(650527, 6, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("I still need more samples. You have {0} of six.", sapCount));
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
			AddNpc(160150, L("Tainted Bracken Cluster"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1002);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1002.Sap{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_bracken_63_2.Quest1002.Sap{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A bracken cluster weeps thick red sap*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This cluster has already been tapped*{/}"));
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
					character.ServerMessage(LF("Sap vials collected: {0}/6", currentCount));

					if (currentCount >= 6)
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

		AddSapNode(1, 678, 811, 315);
		AddSapNode(2, 549, 1173, 315);
		AddSapNode(3, 175, 1166, 45);
		AddSapNode(4, 295, 234, 90);
		AddSapNode(5, 548, 639, 180);
		AddSapNode(6, 622, 1198, 315);
		AddSapNode(7, 705, 670, 225);

		// Quest 3: Silencing the Shamans
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Witch Hunter] Alaric"), "f_bracken_63_2", -1394, 38, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1003);

			dialog.SetTitle(L("Alaric"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Lapasape Mages on the western ridge aren't just corrupted. They're using it."));
				await dialog.Msg(L("They've set blood crystals up all over the ridge — the things soak up the poison and feed it into their spells."));

				var response = await dialog.Select(L("Will you take this on?"),
					Option(L("I'll cut them down and shatter the crystals"), "help"),
					Option(L("Why do the crystals matter?"), "info"),
					Option(L("Handle it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The crystals hover above the western ridge. Smash them with your weapon — they resist magic."));
						await dialog.Msg(L("Without the crystals, the shamans are just tainted beasts. Thin them out first so you can work in peace."));
						break;

					case "info":
						await dialog.Msg(L("Break one crystal and every spell within a hundred paces is half as strong."));
						await dialog.Msg(L("Break all four and the shamans can barely cast anything."));
						break;

					case "leave":
						await dialog.Msg(L("If you change your mind, the ridge is west of here. You'll smell it before you get there."));
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
					await dialog.Msg(L("Crystals broken, shamans dead. The ridge is just a ridge again."));
					await dialog.Msg(L("There's still corruption in the ground, but it's not getting boosted anymore. That's good work for one day."));

					character.Inventory.Remove(650601, character.Inventory.CountItem(650601), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Cut down more Lapasape Mages. ");
					if (!totemObj.Done)
						status += L("Shatter the remaining blood crystals. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ridge is quieter now. The shamans that are left are just beasts, not priests."));
			}
		});

		// Blood Crystal Shatter Points
		//-------------------------------------------------------------------------
		void AddBloodTotem(int totemNum, int x, int z, int direction)
		{
			AddNpc(46214, L("Blood Crystal"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1003);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1003.Totem{totemNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A floating crystal pulses faintly with corrupted blood*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This crystal has already been shattered*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Shattering crystal..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650601, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Crystal Fragment"));

					var currentCount = character.Inventory.CountItem(650601);
					character.ServerMessage(LF("Crystals shattered: {0}/4", currentCount));

					if (RandomProvider.Get().Next(100) < 30)
					{
						if (SpawnTempMonsters(character, MonsterId.Lapasape_Mage, 2, 80, TimeSpan.FromMinutes(2)))
						{
							character.ServerMessage(L("{#FF6666}The shattered crystal draws angry shamans!{/}"));
						}
					}

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All crystals shattered! Return to Alaric.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Shattering interrupted."));
				}
			});
		}

		AddBloodTotem(1, -1321, 156, 45);
		AddBloodTotem(2, -1338, 301, 45);
		AddBloodTotem(3, -1519, 352, 45);
		AddBloodTotem(4, -1592, 227, 45);
		AddBloodTotem(5, -1726, 176, 45);
		AddBloodTotem(6, -1612, -26, 45);

		// Quest 4: The Missing Survey
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Scribe] Petras"), "f_bracken_63_2", 57, -436, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1004);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I was mapping how the corruption spreads when a flock of Ponpon ripped through my camp."));
				await dialog.Msg(L("My survey pages scattered everywhere. Months of work, blown all over this miserable jungle."));
				await dialog.Msg(L("And worse — the Ponpon are dragging the pages back to their nests. They're shredding my parchment to line them."));

				var response = await dialog.Select(L("Can you help recover them?"),
					Option(L("I'll find your pages and clear the flock"), "help"),
					Option(L("What's in the survey?"), "info"),
					Option(L("Write it again"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look for Ponpon nests — that's where the pages are, woven into the twigs and feathers."));
						await dialog.Msg(L("Recover at least eight pages. The flock will swarm you the moment you touch a nest, so be ready for it."));
						await dialog.Msg(L("And please, kill at least ten Ponpon so I can work without getting dive-bombed."));
						break;

					case "info":
						await dialog.Msg(L("I'm tracking how fast the corruption moves. Where it slows down, where it speeds up."));
						await dialog.Msg(L("If we figure out the pattern, maybe we can guess where it'll hit next."));
						await dialog.Msg(L("The Ponpon line their nests with anything soft. Once they had the pages, they wove them right in."));
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

				if (killObj.Done && pageCount >= 8)
				{
					await dialog.Msg(L("All eight pages. And the Ponpon sound quieter already."));
					await dialog.Msg(L("I can resume the survey. This might actually save lives, you understand? Real lives."));

					character.Inventory.Remove(650445, 8, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Thin the Ponpon flock. ");
					if (pageCount < 8)
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
			AddNpc(153057, L("Scattered Survey Page"), "f_bracken_63_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_bracken_63_2", 1004);
				var variableKey = $"Laima.Quests.f_bracken_63_2.Quest1004.Page{pageNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A Ponpon nest of woven twigs - sealed parchment is bound into its lining*{/}"));
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
					character.ServerMessage(LF("Pages recovered: {0}/8", currentCount));

					if (currentCount >= 8)
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

		var surveyPagePositions = new (int num, int x, int z)[]
		{
			(1, 162, -578),
			(2, 444, -511),
			(3, 687, -488),
			(4, 732, -293),
			(5, 788, -86),
			(6, 624, 78),
			(7, 267, 192),
			(8, 100, 28),
			(9, 441, -125),
			(10, 316, -351),
		};

		foreach (var (num, x, z) in surveyPagePositions)
		{
			AddSurveyPage(num, x, z, 45);

			var pageNum = num;
			AddAreaTrigger("f_bracken_63_2", x, z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var pageQuestId = new QuestId("f_bracken_63_2", 1004);
				if (!character.Quests.IsActive(pageQuestId))
					return;

				var spawnedKey = $"Laima.Quests.f_bracken_63_2.Quest1004.Page{pageNum}.Spawned";
				if (character.Variables.Perm.GetBool(spawnedKey, false))
					return;

				character.Variables.Perm.Set(spawnedKey, true);

				if (SpawnTempMonsters(character, MonsterId.Ponpon, 3, 70, TimeSpan.FromMinutes(2)))
					character.ServerMessage(L("{#FF6666}Ponpon erupt from the page nest!{/}"));
			});
		}

		// Quest 5: The Tainted Alpha
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Tracker] Niko"), "f_bracken_63_2", 889, -1065, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1005);

			dialog.SetTitle(L("Niko"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Something in the southern nesting ground is wrong - bigger than wrong."));
				await dialog.Msg(L("The Kanchobirds down there have been dragged into something else. Something's been feeding them, pushing them."));
				await dialog.Msg(L("There's an alpha. I've seen its tracks. Twice the size of the flock, and the corruption's thicker in it."));

				var response = await dialog.Select(L("Will you take the hunt?"),
					Option(L("I'll hunt the alpha"), "help"),
					Option(L("What kind of alpha?"), "info"),
					Option(L("Not my fight"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Kill ten birds to bait it out. The alpha won't emerge while the flock is full."));
						await dialog.Msg(L("When the flock thins, it'll come hunting. Come back here then and I'll flush it into the open for you."));
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

					character.Quests.Complete(questId);
				}
				else if (flockObj.Done && !alphaObj.Done)
				{
					await dialog.Msg(L("The alpha's out there with what's left of the flock. Don't let it slip back into the brush."));
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
		AddNpc(155145, L("[Pathfinder] Elvira"), "f_bracken_63_2", 1374, 452, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_bracken_63_2", 1006);

			dialog.SetTitle(L("Elvira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The village up the road northwest is the last safe waypoint past Knidos, but the route to it is a mess."));
				await dialog.Msg(L("Loktanun from the north and Ponpon from the ridges have flooded the whole length of trail."));

				var response = await dialog.Select(L("Can you help clear the route?"),
					Option(L("I'll clear both packs"), "help"),
					Option(L("How bad is the overlap?"), "info"),
					Option(L("Use another route"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Their territories overlap along the trail, so you'll run into both."));
						await dialog.Msg(L("Even numbers should give the caravans a clean passage for a week or two."));
						break;

					case "info":
						await dialog.Msg(L("Loktanun rule the ground, Ponpon rule the air above it."));
						await dialog.Msg(L("Travelers get caught between the two. Nothing makes it east without losing people."));
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
					await dialog.Msg(L("A trader left this pistol with me as collateral months back - never came back to claim it."));
					await dialog.Msg(L("You earned it more than he ever did. Take it."));

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

		var trailAmbushPositions = new (int idx, int x, int z)[]
		{
			(1, 1203, 449),
			(2, 1070, 460),
			(3, 924, 470),
			(4, 776, 472),
			(5, 638, 481),
			(6, 514, 461),
		};

		foreach (var (idx, x, z) in trailAmbushPositions)
		{
			var ambushIdx = idx;
			AddAreaTrigger("f_bracken_63_2", x, z, 50, async (args) =>
			{
				if (args.Initiator is not Character character)
					return;

				if (character.IsDead)
					return;

				var ambushQuestId = new QuestId("f_bracken_63_2", 1006);
				if (!character.Quests.IsActive(ambushQuestId))
					return;

				var ambushKey = $"Laima.Quests.f_bracken_63_2.Quest1006.Ambush{ambushIdx}.Spawned";
				if (character.Variables.Perm.GetBool(ambushKey, false))
					return;

				character.Variables.Perm.Set(ambushKey, true);

				SpawnTempMonsters(character, MonsterId.Loktanun, 2, 70, TimeSpan.FromMinutes(2));
				SpawnTempMonsters(character, MonsterId.Ponpon, 2, 70, TimeSpan.FromMinutes(2));

				character.ServerMessage(L("{#FF6666}Loktanun and Ponpon ambush from the brush!{/}"));
			});
		}
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
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(112006, 1)); // Sketis Dagger
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
			new CollectItemObjective(650527, 6));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650527, character.Inventory.CountItem(650527), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 6; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1002.Sap{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650527, character.Inventory.CountItem(650527), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 6; i++)
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
		SetDescription(L("Slay Lapasape Mages on the western ridge and shatter the blood crystals amplifying their corruption."));
		SetLocation("f_bracken_63_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Witch Hunter] Alaric"), "f_bracken_63_2");

		AddObjective("killLapasape", L("Slay Lapasape Mages"),
			new KillObjective(12, new[] { MonsterId.Lapasape_Mage }));

		AddObjective("shatterTotems", L("Shatter blood crystals"),
			new CollectItemObjective(650601, 4));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650601, character.Inventory.CountItem(650601), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1003.Totem{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650601, character.Inventory.CountItem(650601), InventoryItemRemoveMsg.Destroyed);

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
			new CollectItemObjective(650445, 8));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650445, character.Inventory.CountItem(650445), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_bracken_63_2.Quest1004.Page{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650445, character.Inventory.CountItem(650445), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
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
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Tracker] Niko"), "f_bracken_63_2");

		AddObjective("killKanchobirds", L("Thin the Kanchobird flock"),
			new KillObjective(10, new[] { MonsterId.Kanchobird }));

		AddObjective("killAlpha", L("Slay the tainted alpha and its flock"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Kanchobird, 1, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Kanchobird, 5),
				},
				resetIdent: "killKanchobirds",
				spawnDistance: 200,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(163309, 1)); // Berthas Sketis Bow
	}
}

// Quest 1006 CLASS: The Stoup Camp Route
//-----------------------------------------------------------------------------

public class TheStoupCampRouteQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_2", 1006);
		SetName(L("The Northwest Trail"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Loktanun and Ponpon from the trail between Croa Village and Toan Valley."));
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
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(302118, 1)); // Sketis Pistol
	}
}
