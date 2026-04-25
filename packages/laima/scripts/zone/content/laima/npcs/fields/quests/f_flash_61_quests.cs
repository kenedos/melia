//--- Melia Script ----------------------------------------------------------
// Ruklys Street Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Ruklys Street ruins.
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

public class FFlash61QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Sword-Goblin Infestation
		//-------------------------------------------------------------------------
		AddNpc(20108, L("[Street Warden] Sebo"), "f_flash_61", -100, 1290, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1001);

			dialog.SetTitle(L("Sebo"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ruklys Street used to be a trade run. Now it's a goblin corridor - curse-drawn Sword-Goblins came down from the plateau and never left."));
				await dialog.Msg(L("They don't know about the curse, or they don't care. They sleep on the statued shopkeepers and piss on the ward-lines."));
				await dialog.Msg(L("Thin twenty-two. Enough to make the corridor passable for a column of three."));

				var response = await dialog.Select(L("Twenty-two?"),
					Option(L("I'll thin the Sword-Goblins"), "help"),
					Option(L("They don't care about the curse?"), "info"),
					Option(L("Abandon the street"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Swing hard and don't linger - a goblin that grey-streaks you will chase as long as you bleed."));
						await dialog.Msg(L("Ward-charm in your pocket. It helps."));
						break;

					case "info":
						await dialog.Msg(L("Some goblins have started to grey-streak themselves. The lucky ones die fast. The unlucky go the slow way - still fighting when stone reaches their jaw."));
						await dialog.Msg(L("Grim. Also their problem. Not ours."));
						break;

					case "leave":
						await dialog.Msg(L("Ruklys is seventy-three shopkeepers, now seventy-three statues. I stay until their names get carved somewhere."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSwordGoblins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Corridor's passable. A column of three walked through this morning without drawing steel."));
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
				await dialog.Msg(L("Two caravans through yesterday. Shop numbers carved into a plaque on the north end - all seventy-three."));
			}
		});

		// Quest 2: Alley Cursecores
		//-------------------------------------------------------------------------
		AddNpc(156024, L("[Ward-Mason] Vera"), "f_flash_61", 300, 600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1002);

			dialog.SetTitle(L("Vera"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I'm rebuilding the alley wards stone by stone. The Rootcrystals in Ruklys run thicker than most - their cores are heavy with curse-weight."));
				await dialog.Msg(L("Five cores sets a proper keystone for the east alley. One keystone holds an alley for a decade if it's fit right."));
				await dialog.Msg(L("My own hands have greyed past the knuckles. Can't swing the pick right. Can you?"));

				var response = await dialog.Select(L("Five alley cursecores?"),
					Option(L("I'll crack them"), "help"),
					Option(L("Past the knuckles?"), "info"),
					Option(L("Find another mason"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Listen for the deep ring. Ruklys cores ring lower than most - almost a drum."));
						await dialog.Msg(L("Five drum-cores. That's what I need."));
						break;

					case "info":
						await dialog.Msg(L("Fingers are stone to the second knuckle. I set them in warm water every evening - doesn't cure it, but keeps the spread slow."));
						await dialog.Msg(L("The wards are worth more than my hands. Work continues."));
						break;

					case "leave":
						await dialog.Msg(L("No other mason works these alleys. If I don't, the whole east quarter loses its wards."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(650355);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five drum-ringers. Listen - you can hear the difference. These will hold."));
					await dialog.Msg(L("Take your pay. The east alley gets its keystone tonight."));

					character.Inventory.Remove(650355, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. {0} of five. Listen for the deep ring.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("East alley's keystone is set. Wards hold steady - you can feel the boundary when you walk past."));
			}
		});

		// Quest 3: The Statued Shopkeepers
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Apprentice Cataloguer] Ingrid"), "f_flash_61", -400, 300, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1003);

			dialog.SetTitle(L("Ingrid"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ruklys has seventy-three statued shopkeepers. Twenty-nine of them tucked a name-token under their aprons before stone took them."));
				await dialog.Msg(L("Denden have built nests around the statues - the curse-warmth draws them. Thin the Denden, pull four name-tokens, and the plaque project gets four more names."));
				await dialog.Msg(L("Seventy-three names is the goal. Every token is a step."));

				var response = await dialog.Select(L("Denden and tokens?"),
					Option(L("I'll recover the tokens"), "help"),
					Option(L("Why under aprons?"), "info"),
					Option(L("Let them be nameless"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Name-tokens are small, coin-sized, metal or polished wood. Under the apron line, tucked against the chest."));
						await dialog.Msg(L("Treat them with respect. That's someone's whole identity."));
						break;

					case "info":
						await dialog.Msg(L("Shopkeepers tuck receipts under aprons by habit. When they felt the curse taking them, they reached for the habit - and tucked their names in instead."));
						await dialog.Msg(L("Last honest moment. Last honest pocket."));
						break;

					case "leave":
						await dialog.Msg(L("Seventy-three statues with no names is a city losing its own memory. I won't accept that."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killDenden", out var killObj)) return;
				if (!quest.TryGetProgress("recoverTokens", out var tokenObj)) return;

				if (killObj.Done && tokenObj.Done)
				{
					await dialog.Msg(L("Four tokens. Four more names for the plaque. I'll carve them tonight."));
					await dialog.Msg(L("Take your pay. Stop by when the plaque reaches seventy-three - we'll have a reading."));

					character.Inventory.Remove(650545, character.Inventory.CountItem(650545), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Denden. ");
					if (!tokenObj.Done)
						status += L("Recover more name-tokens. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sixty-one names on the plaque. Twelve to go."));
			}
		});

		// Name-Token Points
		//-------------------------------------------------------------------------
		void AddStatuedShopkeeper(int tokenNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Statued Shopkeeper"), "f_flash_61", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_flash_61", 1003);
				var variableKey = $"Laima.Quests.f_flash_61.Quest1003.Token{tokenNum}";
				var spawnedKey = $"Laima.Quests.f_flash_61.Quest1003.Token{tokenNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A figure frozen mid-transaction, stone hand extended as if offering change.*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this shopkeeper's token*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Denden, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}Denden swarm from the curse-warm nest!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering name-token..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650545, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Shopkeeper's Name-Token"));

					var currentCount = character.Inventory.CountItem(650545);
					character.ServerMessage(LF("Tokens recovered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All four tokens recovered! Return to Ingrid.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You paused. Try again."));
				}
			});
		}

		AddStatuedShopkeeper(1, 200, 400, 0);
		AddStatuedShopkeeper(2, -200, -100, 0);
		AddStatuedShopkeeper(3, 500, -300, 0);
		AddStatuedShopkeeper(4, -500, 200, 0);

		// Quest 4: Moyabu Curse-Brands
		//-------------------------------------------------------------------------
		AddNpc(20122, L("[Curse-Inspector] Thane"), "f_flash_61", 100, -400, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1004);

			dialog.SetTitle(L("Thane"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Moyabu in Ruklys carry brand-marks. Not natural - someone's branded them with petrifier-sigils."));
				await dialog.Msg(L("The brands turn them into walking curse-carriers. Every Moyabu we kill with its brand intact is a sigil we can study and neutralize."));
				await dialog.Msg(L("Twelve Moyabu, five brands. Inspector's orders. Fedimian wants to know who's branding livestock in our streets."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the brands"), "help"),
					Option(L("Who's branding them?"), "info"),
					Option(L("Leave them branded"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Brands are burned into the left flank. Cut them out clean - we need the sigils intact."));
						await dialog.Msg(L("Don't touch the brands with bare skin. Glove up."));
						break;

					case "info":
						await dialog.Msg(L("Suspicion points at the Saltisdaughter cabal out of Roxona. Can't prove it until I have brands to match."));
						await dialog.Msg(L("Five brands, one forensic match. That's the case."));
						break;

					case "leave":
						await dialog.Msg(L("Branded Moyabu wander beyond Ruklys eventually. The curse travels with them. Not an option."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMoyabu", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBrands", out var brandObj)) return;

				if (killObj.Done && brandObj.Done)
				{
					await dialog.Msg(L("Five brands. Sigil style matches Roxona's cabal - case is made. I'll file it with the Fedimian office tonight."));
					await dialog.Msg(L("Take your pay. The cabal's going to get a visit next week."));

					character.Inventory.Remove(650665, character.Inventory.CountItem(650665), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Moyabu. ");
					if (!brandObj.Done)
						status += L("Recover more curse-brands. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Case filed. Fedimian's investigators paid the cabal a visit. Three arrests. Not the end of it, but a start."));
			}
		});

		// Quest 5: The Branded Warlord
		//-------------------------------------------------------------------------
		AddNpc(147509, L("[Bounty Hunter] Roma"), "f_flash_61", 800, -400, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1005);
			var warlordSpawnedKey = "Laima.Quests.f_flash_61.Quest1005.WarlordSpawned";

			dialog.SetTitle(L("Roma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("A Sword-Goblin warlord leads the Ruklys corridor pack. He's branded himself - a full petrifier-sigil burned into his chest. Half-stone, twice the menace."));
				await dialog.Msg(L("Thin ten Moyabu from his outriders, and pride will drag him out to re-establish order."));
				await dialog.Msg(L("Bounty's big. His sigil-brand alone is worth more than the street's annual trade."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Warlord"), "help"),
					Option(L("Why a goblin sigil-brand?"), "info"),
					Option(L("Let him rule the corridor"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten Moyabu. Once he shows, he shows hard. Stay off his stone-left."));
						await dialog.Msg(L("Good hunting."));
						break;

					case "info":
						await dialog.Msg(L("Sigil evidence. Ties the goblin warbands to whoever's making the brands. Fedimian wants a chain of proof."));
						await dialog.Msg(L("You kill him, they get the proof. Clean work."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. Bounty keeps climbing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killOutriders", out var outObj)) return;
				if (!quest.TryGetProgress("killWarlord", out var warObj)) return;

				if (outObj.Done && warObj.Done)
				{
					await dialog.Msg(L("Sigil-brand intact. Chest-plate cooled cleanly. That's the proof Fedimian wanted."));
					await dialog.Msg(L("Bounty paid, plus my share. Corridor's manageable now."));

					character.Variables.Perm.Remove(warlordSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (outObj.Done && !warObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(warlordSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(warlordSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Goblin2_Sword, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Outriders thinned. Listen - that's his bellow. He's coming."));
							await dialog.Msg(L("{#FF9966}Move! Don't let him get back in the pack.{/}"));
							character.ServerMessage(L("{#FF9966}The Branded Warlord emerges, stone-chest glowing!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Don't let him retreat."));
					}
				}
				else
				{
					await dialog.Msg(L("Outriders still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sigil-brand's at the Fedimian office. Chain of proof complete. The branding-network's getting raided next month."));
			}
		});

		// Quest 6: The Ruklys Corridor
		//-------------------------------------------------------------------------
		AddNpc(155170, L("[Caravan Master] Volka"), "f_flash_61", -700, -200, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1006);

			dialog.SetTitle(L("Volka"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ruklys corridor connects Roxona to Downtown. Right now it's shoulder-to-shoulder hostile."));
				await dialog.Msg(L("Sword-Goblins in the main run, Denden in the alleys. Drivers refuse until both are thinned."));

				var response = await dialog.Select(L("Both?"),
					Option(L("I'll clear both"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Reroute the column"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve and twelve. Goblins in the open, Denden in the alleys. Mind both flanks."));
						await dialog.Msg(L("Clear the corridor and three columns roll by week's end."));
						break;

					case "info":
						await dialog.Msg(L("Goblins hit hard in straight lines. Denden blindside from alleys. Whichever's in your face is worse."));
						await dialog.Msg(L("Ward-charm in the pocket. It flares when Denden drop from above."));
						break;

					case "leave":
						await dialog.Msg(L("Reroute crosses deep curse-ground. Half my drivers would come home statued. Not an option."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGoblins", out var gobObj)) return;
				if (!quest.TryGetProgress("killDenden", out var denObj)) return;

				if (gobObj.Done && denObj.Done)
				{
					await dialog.Msg(L("Both flanks thinned. Columns roll at dawn."));
					await dialog.Msg(L("Take your pay. Drivers will remember you."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!gobObj.Done)
						status += L("Kill more Sword-Goblins. ");
					if (!denObj.Done)
						status += L("Kill more Denden. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four columns through already. Corridor feels like a corridor again, not a gauntlet."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class SwordGoblinInfestationQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1001);
		SetName(L("Sword-Goblin Infestation"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sword-Goblins choking the Ruklys Street corridor to make it passable for caravan columns."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Street Warden] Sebo"), "f_flash_61");

		AddObjective("killSwordGoblins", L("Kill Sword-Goblins"),
			new KillObjective(22, new[] { MonsterId.Goblin2_Sword }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class AlleyCursecoresQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1002);
		SetName(L("Alley Cursecores"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack Rootcrystals and bring their deep-ringing alley cursecores to the ward-mason."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ward-Mason] Vera"), "f_flash_61");

		AddObjective("gatherCores", L("Gather alley cursecores"),
			new CollectItemObjective(650355, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650355, character.Inventory.CountItem(650355), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650355, character.Inventory.CountItem(650355), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheStatuedShopkeepersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1003);
		SetName(L("The Statued Shopkeepers"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Denden nesting on the statued shopkeepers and recover four name-tokens for the memorial plaque."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Apprentice Cataloguer] Ingrid"), "f_flash_61");

		AddObjective("killDenden", L("Kill Denden"),
			new KillObjective(15, new[] { MonsterId.Denden }));

		AddObjective("recoverTokens", L("Recover shopkeeper name-tokens"),
			new CollectItemObjective(650545, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650545, character.Inventory.CountItem(650545), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_61.Quest1003.Token{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_61.Quest1003.Token{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650545, character.Inventory.CountItem(650545), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_61.Quest1003.Token{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_flash_61.Quest1003.Token{i}.Spawned");
		}
	}
}

public class MoyabuCurseBrandsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1004);
		SetName(L("Moyabu Curse-Brands"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill branded Moyabu spreading the curse through Ruklys and recover five brand-sigils for forensic analysis."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Inspector] Thane"), "f_flash_61");

		AddObjective("killMoyabu", L("Kill branded Moyabu"),
			new KillObjective(12, new[] { MonsterId.Moyabu }));

		AddObjective("gatherBrands", L("Recover Moyabu curse-brands"),
			new CollectItemObjective(650665, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650665, character.Inventory.CountItem(650665), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650665, character.Inventory.CountItem(650665), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheBrandedWarlordQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1005);
		SetName(L("The Branded Warlord"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Moyabu outriders to draw out the Branded Warlord, then bring him down for forensic proof."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Roma"), "f_flash_61");

		AddObjective("killOutriders", L("Thin the Moyabu outriders"),
			new KillObjective(10, new[] { MonsterId.Moyabu }));

		AddObjective("killWarlord", L("Defeat the Branded Warlord"),
			new KillObjective(1, new[] { MonsterId.Goblin2_Sword }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

public class TheRuklysCorridorQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1006);
		SetName(L("The Ruklys Corridor"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Sword-Goblins in the main run and Denden in the alleys to reopen the Ruklys corridor."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Caravan Master] Volka"), "f_flash_61");

		AddObjective("killGoblins", L("Kill Sword-Goblins"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Sword }));

		AddObjective("killDenden", L("Kill Denden"),
			new KillObjective(12, new[] { MonsterId.Denden }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
