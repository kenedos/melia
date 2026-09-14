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
		AddNpc(20108, L("[Street Warden] Sebo"), "f_flash_61", 82, 1365, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1001);

			dialog.SetTitle(L("Sebo"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ruklys Street was a trade run before the curse came down off the plateau. Now it's a goblin corridor."));
				await dialog.Msg(L("Sword-Goblins moved in after the shopkeepers stopped moving. They sleep on the statues, fight over the alleys, and lash anyone who tries to walk through."));
				await dialog.Msg(L("Thin twenty-two and a column of three can pass without drawing steel. That's all I want."));

				var response = await dialog.Select(L("Will you clear the corridor?"),
					Option(L("I'll handle the goblins"), "help"),
					Option(L("Why are you still here?"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Don't bleed long around them - one that's tasted blood will trail you for hours."));
						await dialog.Msg(L("Charm in your pocket. It helps."));
						break;

					case "info":
						await dialog.Msg(L("Seventy-three shopkeepers were standing here when the curse hit. Seventy-three statues now. The plaque at the north end has every name."));
						await dialog.Msg(L("Somebody has to keep the street open until those names are remembered properly. That's me."));
						break;

					case "leave":
						await dialog.Msg(L("I'll be here. So will the goblins."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSwordGoblins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("A column walked through this morning without drawing steel. First time in two years."));
					await dialog.Msg(L("Pay's yours. Thanks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still too thick. Keep cutting."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two caravans through yesterday. Drivers stopped to read the names off the plaque. That's the part I wanted."));
			}
		});

		// Quest 2: Moyabu Curse-Brands
		//-------------------------------------------------------------------------
		AddNpc(20122, L("[Curse-Inspector] Thane"), "f_flash_61", -908, 0, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1004);

			dialog.SetTitle(L("Thane"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Inspector out of Fedimian. I'm building a case across three districts and I need physical evidence."));
				await dialog.Msg(L("The Moyabu wandering Ruklys carry brand-marks on their flanks. Burned in deliberately. The brands let the curse ride them - the animals walk into other districts and shed it as they go."));
				await dialog.Msg(L("Twelve Moyabu, five brands. The brands match a sigil-style I've already documented in Roxona. One more matched batch and the cabal goes to court."));

				var response = await dialog.Select(L("Will you bring me the brands?"),
					Option(L("I'll bring the brands"), "help"),
					Option(L("Who's branding livestock?"), "info"),
					Option(L("Find another courier"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Brands sit on the left flank. Cut the patch out clean - the sigil has to be intact for the case."));
						await dialog.Msg(L("Glove up. Don't grip the brand-side."));
						break;

					case "info":
						await dialog.Msg(L("Saltisdaughter cabal. Curse-worshippers, organized. Pavel in Roxona is burning their plates this season - same group, different limb."));
						await dialog.Msg(L("Five brands gives me chain-of-custody across two districts. Then I cross-file in Fedimian."));
						break;

					case "leave":
						await dialog.Msg(L("Branded animals don't stay put. Reconsider."));
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
					await dialog.Msg(L("Five. Sigil-style matches Pavel's plates - that's the link. Case files tonight."));
					await dialog.Msg(L("Pay's yours. The cabal gets a visit next week."));

					character.Inventory.Remove(650665, character.Inventory.CountItem(650665), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Moyabu still branded. ");
					if (!brandObj.Done)
						status += L("More brands need recovering. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three arrests last week. Not the whole cabal - that takes the Enceinte case to close it. But a start."));
			}
		});

		// Quest 3: The Branded Warlord
		//-------------------------------------------------------------------------
		AddNpc(147509, L("[Bounty Hunter] Roma"), "f_flash_61", -739, 768, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_61", 1005);
			var warlordSpawnedKey = "Laima.Quests.f_flash_61.Quest1005.WarlordSpawned";

			dialog.SetTitle(L("Roma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's a warlord running the Sword-Goblin pack on Ruklys. Big one. Branded himself with a full cabal-sigil across the chest."));
				await dialog.Msg(L("That brand's the link Inspector Thane needs to tie the cabal directly to the goblin warbands. Without it, his case has a hole."));
				await dialog.Msg(L("Thin ten of his outriders and pride drags him out. Bounty's the contract, the brand goes to Thane."));

				var response = await dialog.Select(L("So? Want the contract?"),
					Option(L("I'll take the warlord"), "help"),
					Option(L("Why brand himself?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten Moyabu first. He won't show until he sees the pack thinning."));
						await dialog.Msg(L("Stay off his stone-left. He leads with it."));
						break;

					case "info":
						await dialog.Msg(L("Vanity, mostly. Status mark in the cabal. He thinks the brand makes him untouchable."));
						await dialog.Msg(L("It doesn't. That's the part you're going to demonstrate."));
						break;

					case "leave":
						await dialog.Msg(L("Bounty climbs every week. I'll be here."));
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
					await dialog.Msg(L("Brand intact, chest-plate cooled clean. Thane will have the proof he needs by tomorrow."));
					await dialog.Msg(L("Bounty paid, plus my cut. Corridor's manageable now."));

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
							await dialog.Msg(L("Outriders gone. Hear that bellow? He's coming."));
							await dialog.Msg(L("{#FF9966}Move - and don't let him slip back into the pack.{/}"));
							character.ServerMessage(L("{#FF9966}The Branded Warlord emerges, stone-chest glowing!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out. Don't lose him - he'll heal up if he gets back behind cover."));
					}
				}
				else
				{
					await dialog.Msg(L("Outriders too thick. He won't budge."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Brand's at the inspector's office. Chain of proof complete. Cabal raid is on the books for next month."));
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
		SetDescription(L("Sebo can't keep the Ruklys corridor open while Sword-Goblins choke the street. Thin them out so caravans can pass."));
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

public class MoyabuCurseBrandsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_61", 1004);
		SetName(L("Moyabu Curse-Brands"));
		SetType(QuestType.Sub);
		SetDescription(L("Inspector Thane is building a cross-district case against the Saltisdaughter cabal. Kill the branded Moyabu and recover five brand-sigils for evidence."));
		SetLocation("f_flash_61");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Inspector] Thane"), "f_flash_61");

		AddObjective("killMoyabu", L("Kill branded Moyabu"),
			new KillObjective(12, new[] { MonsterId.Moyabu }));

		AddObjective("gatherBrands", L("Recover Moyabu brand-sigils"),
			new CollectItemObjective(650665, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650665, 0.50f, MonsterId.Moyabu);
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
		SetDescription(L("Roma has a contract on a Sword-Goblin warlord branded with a Saltisdaughter sigil. Thin his outriders to draw him out, then bring his brand to the inspector's case."));
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
