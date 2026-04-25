//--- Melia Script ----------------------------------------------------------
// Inner Enceinte District Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Inner Enceinte ruins.
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

public class FFlash64QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Lemuria Swarm
		//-------------------------------------------------------------------------
		AddNpc(20127, L("[Enceinte Warden] Oswin"), "f_flash_64", -570, 1370, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1001);

			dialog.SetTitle(L("Oswin"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Inner Enceinte is the last ring before the district's cursed heart. Lemurias swarm these walls - smaller than true Lemurs, faster, and curse-mad."));
				await dialog.Msg(L("Thin twenty-two and the inner-wall ward-crew gets a working shift in. Wards hold the heart from spreading further."));

				var response = await dialog.Select(L("Twenty-two?"),
					Option(L("I'll thin the Lemurias"), "help"),
					Option(L("Cursed heart?"), "info"),
					Option(L("Leave the enceinte"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twenty-two Lemurias inside the Inner Enceinte?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twenty-two. They swarm - keep swinging and don't stop."));
							await dialog.Msg(L("Ward-charm at the neck. Their bite grey-streaks."));
						}
						break;

					case "info":
						await dialog.Msg(L("Center of the district. Full curse. Anyone who walks in walks out statued - nothing moves in there except Gargoyle and rumor."));
						await dialog.Msg(L("The inner wall keeps it contained. That's why the ward-crew matters."));
						break;

					case "leave":
						await dialog.Msg(L("The enceinte falls, the curse leaks. I'm not moving."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLemurias", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Enceinte's quiet. Ward-crew worked a full shift. Inner wall's repaired in three sections."));
					await dialog.Msg(L("Take your pay. And thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too many. Keep cutting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Inner wall's solid all the way around. First time in a decade. The heart's contained."));
			}
		});

		// Quest 2: Enceinte Wardstone Cores
		//-------------------------------------------------------------------------
		AddNpc(147410, L("[Wall-Warden] Petra"), "f_flash_64", 100, 800, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1002);

			dialog.SetTitle(L("Petra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Rootcrystals against the enceinte wall absorb curse-bleed - they pull it off the stone and lock it in dense wardcores."));
				await dialog.Msg(L("Five wardcores is one full wall-section reset. I reset sections in rotation - one each month, ten-year cycle."));
				await dialog.Msg(L("My own feet greyed up to the ankles last year. I work from a wheeled stool now. Can you crack for me?"));

				var response = await dialog.Select(L("Five wardcores?"),
					Option(L("I'll crack the crystals"), "help"),
					Option(L("Ten-year cycle?"), "info"),
					Option(L("Find another warden"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Crack five Rootcrystals and bring back their wardcores?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wardcores are dense - heavy for their size. Dull ones are duds."));
							await dialog.Msg(L("Five heavy ones. Thank you."));
						}
						break;

					case "info":
						await dialog.Msg(L("Each section degrades over years. Ten sections, ten years - by the time I'm back to the first, it needs another reset."));
						await dialog.Msg(L("Wardens before me kept it going for two hundred years. I'll keep it going as long as my hands still swing a tool."));
						break;

					case "leave":
						await dialog.Msg(L("No other wall-warden. The cycle stops with me, or it doesn't stop. Your choice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(650253);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five heavy ones. Proper wardcores. This month's section will reset clean."));
					await dialog.Msg(L("Take your pay. The cycle continues."));

					character.Inventory.Remove(650253, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. {0} of five. Weigh them - the real ones are heavy.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("This month's section is reset. On to next month's. Ten sections, ten years, always."));
			}
		});

		// Quest 3: The Bunny Nests
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Warren-Inspector] Tobi"), "f_flash_64", 500, 200, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1003);

			dialog.SetTitle(L("Tobi"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Repusbunnies look harmless. They're not. They burrow under the enceinte floor and nest on statued citizens - the curse-warmth draws them, same as everywhere else."));
				await dialog.Msg(L("Every nest has a token - something the statued citizen was holding when stone took them. Thin Rubabos defending the burrows, recover four tokens."));
				await dialog.Msg(L("Rubabos are the aggressive ones. Repusbunnies scatter. The tokens are why we're here."));

				var response = await dialog.Select(L("Rubabos and tokens?"),
					Option(L("I'll recover the tokens"), "help"),
					Option(L("Why tokens?"), "info"),
					Option(L("Leave the nests"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Rubabos and recover four citizen tokens from the burrow nests?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Fifteen Rubabos. Tokens are wrapped in nesting fur - look for the padded spots."));
							await dialog.Msg(L("Handle gentle. Sentimental value, not just archival."));
						}
						break;

					case "info":
						await dialog.Msg(L("Citizens' descendants pay for tokens. A ring, a pen, a hairpin - anything the statue clutched at the end. It's the last trace of a life."));
						await dialog.Msg(L("I run the recovery office. Tokens go to families, families get closure. Small economy of grief."));
						break;

					case "leave":
						await dialog.Msg(L("Nests spread. Every week we wait, another row of statues gets nested on and lost."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRubabos", out var killObj)) return;
				if (!quest.TryGetProgress("recoverTokens", out var tokenObj)) return;

				if (killObj.Done && tokenObj.Done)
				{
					await dialog.Msg(L("Four tokens. I'll match them to the registry tonight. Four families get closure by week's end."));
					await dialog.Msg(L("Take your pay. I'll let you know what the tokens turn out to be - curious work."));

					character.Inventory.Remove(650455, character.Inventory.CountItem(650455), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Rubabos. ");
					if (!tokenObj.Done)
						status += L("Recover more citizen tokens. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three tokens matched. One was a wedding ring - widow came by yesterday, wept for an hour, took it home."));
			}
		});

		// Citizen Token Points
		//-------------------------------------------------------------------------
		void AddBurrowNest(int tokenNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Curse-Warm Burrow"), "f_flash_64", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_64", 1003);
				var variableKey = $"Laima.Quests.f_flash_64.Quest1003.Token{tokenNum}";
				var spawnedKey = $"Laima.Quests.f_flash_64.Quest1003.Token{tokenNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A burrow dug under a statued citizen, padded with nesting fur.*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already pulled this burrow's token*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Rubabos, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}Rubabos burst from the burrow, defending the nest!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Pulling citizen token from nesting fur..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650455, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Citizen Token"));

					var currentCount = character.Inventory.CountItem(650455);
					character.ServerMessage(LF("Tokens recovered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All four tokens recovered! Return to Tobi.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You paused. Try again."));
				}
			});
		}

		AddBurrowNest(1, 300, 400, 0);
		AddBurrowNest(2, -400, 500, 0);
		AddBurrowNest(3, 700, -200, 0);
		AddBurrowNest(4, -600, -300, 0);

		// Quest 4: The Saltisdaughter Archers
		//-------------------------------------------------------------------------
		AddNpc(20142, L("[Curse-Warden] Alek"), "f_flash_64", 200, -500, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1004);

			dialog.SetTitle(L("Alek"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Saltisdaughter Archers arrived at the enceinte last month. Same cabal the Roxona and Ruklys investigators are tracking - they've brought bows this time."));
				await dialog.Msg(L("Their arrowheads are sigil-brands. Every arrow they fire that hits stone reinforces the curse-grid. Every archer we drop pulls a node."));
				await dialog.Msg(L("Twelve archers, five brand-arrows. The inspectors want physical evidence to complete the cross-district case."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the arrows"), "help"),
					Option(L("Cross-district case?"), "info"),
					Option(L("Let them fire"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Saltisdaughter Archers and recover five brand-arrows?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Arrows in the quivers - their unfired ones. Don't touch the arrowheads. Wrap them."));
							await dialog.Msg(L("The inspectors need them sealed. Chain-of-custody matters."));
						}
						break;

					case "info":
						await dialog.Msg(L("Roxona has their curse-brands. Ruklys has the Moyabu brands. We get the arrow-brands. Three districts, one cabal, one case."));
						await dialog.Msg(L("Enough evidence and Fedimian dissolves the cabal permanently."));
						break;

					case "leave":
						await dialog.Msg(L("Every arrow they fire extends the grid another node. Not an option."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("gatherArrows", out var arrowObj)) return;

				if (killObj.Done && arrowObj.Done)
				{
					await dialog.Msg(L("Five brand-arrows sealed. The cross-district case goes to Fedimian tonight. Cabal's done."));
					await dialog.Msg(L("Take your pay. Your name goes on the evidence manifest."));

					character.Inventory.Remove(650760, character.Inventory.CountItem(650760), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Saltisdaughter Archers. ");
					if (!arrowObj.Done)
						status += L("Recover more brand-arrows. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cabal dissolved. Twelve arrests across three districts. The grid's gone dark."));
			}
		});

		// Quest 5: The Stone-Mother
		//-------------------------------------------------------------------------
		AddNpc(147473, L("[Bounty Hunter] Lira"), "f_flash_64", 900, -400, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1005);
			var motherSpawnedKey = "Laima.Quests.f_flash_64.Quest1005.MotherSpawned";

			dialog.SetTitle(L("Lira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Lemuria matriarch rules the enceinte pack. She's half-stone through the neck and shoulders, but she moves like she's not. Size of two Lemurias stacked."));
				await dialog.Msg(L("Thin ten of her daughters and she emerges to enforce order. Bounty's huge - her stone-mantle carves into throat-wards, the rarest cold-curse counter."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Stone-Mother"), "help"),
					Option(L("Throat-wards?"), "info"),
					Option(L("Leave her to her pack"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin ten Lemurias and bring down the Stone-Mother when she emerges?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten. She lunges from the stone-shoulder. Stay off her right."));
							await dialog.Msg(L("Good hunting."));
						}
						break;

					case "info":
						await dialog.Msg(L("Ward against the breath-stealer curse variant. Stone lungs, one of the worst ways to go. Throat-wards prevent it."));
						await dialog.Msg(L("Stone-mantle from a Lemuria matriarch is the only material that works."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. Bounty climbs."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var packObj)) return;
				if (!quest.TryGetProgress("killMother", out var motherObj)) return;

				if (packObj.Done && motherObj.Done)
				{
					await dialog.Msg(L("Stone-mantle intact. A season's worth of throat-wards off that one piece."));
					await dialog.Msg(L("Bounty paid, plus my share. You just saved a few dozen lungs."));

					character.Variables.Perm.Remove(motherSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !motherObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(motherSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(motherSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Lemuria, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Pack's thin. That's her - lower-pitched howl, you hear the stone in it."));
							await dialog.Msg(L("{#FF9966}Move! Don't let her retreat to the pack.{/}"));
							character.ServerMessage(L("{#FF9966}The Stone-Mother emerges, mantle gleaming!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("She's out there. Don't let her retreat."));
					}
				}
				else
				{
					await dialog.Msg(L("Pack still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Mantle went to the throat-ward forge. A season's worth of lungs saved already."));
			}
		});

		// Quest 6: The Enceinte Wall Walk
		//-------------------------------------------------------------------------
		AddNpc(20018, L("[Wall Captain] Ember"), "f_flash_64", -700, -200, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_64", 1006);

			dialog.SetTitle(L("Ember"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The enceinte wall walk runs the perimeter. Right now it's blocked - Lemurias scaling from inside, Repusbunnies tunneling from outside."));
				await dialog.Msg(L("Neither alone stops a patrol. Both together turn the walk into a gauntlet."));

				var response = await dialog.Select(L("Both species?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Close the walk"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Lemurias and twelve Repusbunnies along the wall walk?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twelve and twelve. Lemurias scale from inside, Repusbunnies tunnel from outside. Mind both."));
							await dialog.Msg(L("Clear them, and the walk is patrol-ready by dawn."));
						}
						break;

					case "info":
						await dialog.Msg(L("Lemurias bite-streak you. Repusbunnies scatter but trip you. Whichever's at your heels is worse."));
						await dialog.Msg(L("Together they're impassable. Apart, endurable."));
						break;

					case "leave":
						await dialog.Msg(L("Close the walk, the enceinte goes unchecked, the heart leaks. Not happening."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLemurias", out var lemObj)) return;
				if (!quest.TryGetProgress("killBunnies", out var bunObj)) return;

				if (lemObj.Done && bunObj.Done)
				{
					await dialog.Msg(L("Both cleared. Patrol walks the full perimeter at dawn."));
					await dialog.Msg(L("Take your pay. Enceinte holds."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!lemObj.Done)
						status += L("Kill more Lemurias. ");
					if (!bunObj.Done)
						status += L("Kill more Repusbunnies. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Patrol's walking clean. Enceinte's tighter than it's been in years."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class LemuriaSwarmQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1001);
		SetName(L("Lemuria Swarm"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Lemurias swarming the Inner Enceinte so the inner-wall ward-crew can work their shift."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Enceinte Warden] Oswin"), "f_flash_64");

		AddObjective("killLemurias", L("Kill swarming Lemurias"),
			new KillObjective(22, new[] { MonsterId.Lemuria }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
	}
}

public class EnceinteWardstoneCoresQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1002);
		SetName(L("Enceinte Wardstone Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack Rootcrystals and bring dense wardcores to the wall-warden for this month's section reset."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wall-Warden] Petra"), "f_flash_64");

		AddObjective("gatherCores", L("Gather dense wardcores"),
			new CollectItemObjective(650253, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 5));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650253, character.Inventory.CountItem(650253), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650253, character.Inventory.CountItem(650253), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheBunnyNestsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1003);
		SetName(L("The Bunny Nests"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Rubabos defending the burrow nests and recover four citizen tokens for the recovery office."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Warren-Inspector] Tobi"), "f_flash_64");

		AddObjective("killRubabos", L("Kill Rubabos"),
			new KillObjective(15, new[] { MonsterId.Rubabos }));

		AddObjective("recoverTokens", L("Recover citizen tokens"),
			new CollectItemObjective(650455, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650455, character.Inventory.CountItem(650455), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Token{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Token{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650455, character.Inventory.CountItem(650455), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Token{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_64.Quest1003.Token{i}.Spawned");
		}
	}
}

public class TheSaltisdaughterArchersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1004);
		SetName(L("The Saltisdaughter Archers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Saltisdaughter Archers extending the curse-grid and recover five brand-arrows for cross-district evidence."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Warden] Alek"), "f_flash_64");

		AddObjective("killArchers", L("Kill Saltisdaughter Archers"),
			new KillObjective(12, new[] { MonsterId.Saltisdaughter_Bow }));

		AddObjective("gatherArrows", L("Recover brand-arrows"),
			new CollectItemObjective(650760, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650760, character.Inventory.CountItem(650760), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650760, character.Inventory.CountItem(650760), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheStoneMotherQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1005);
		SetName(L("The Stone-Mother"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Lemuria pack to draw out the Stone-Mother matriarch, then bring her down for the throat-ward mantle."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Lira"), "f_flash_64");

		AddObjective("killPack", L("Thin the Lemuria pack"),
			new KillObjective(10, new[] { MonsterId.Lemuria }));

		AddObjective("killMother", L("Defeat the Stone-Mother"),
			new KillObjective(1, new[] { MonsterId.Lemuria }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}
}

public class TheEnceinteWallWalkQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_64", 1006);
		SetName(L("The Enceinte Wall Walk"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Lemurias scaling from inside and Repusbunnies tunneling from outside to clear the enceinte wall walk."));
		SetLocation("f_flash_64");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wall Captain] Ember"), "f_flash_64");

		AddObjective("killLemurias", L("Kill Lemurias"),
			new KillObjective(12, new[] { MonsterId.Lemuria }));

		AddObjective("killBunnies", L("Kill Repusbunnies"),
			new KillObjective(12, new[] { MonsterId.Repusbunny }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 14));
		AddReward(new ItemReward(640007, 14));
		AddReward(new ItemReward(640013, 6));
	}
}
