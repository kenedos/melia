//--- Melia Script ----------------------------------------------------------
// Downtown Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed quests for the Downtown ruins.
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

public class FFlash63QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Lemur Howl
		//-------------------------------------------------------------------------
		AddNpc(20100, L("[District Warden] Grelle"), "f_flash_63", -46, 1211, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1001);

			dialog.SetTitle(L("Grelle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Downtown used to be the civic heart. Now it's Lemurs. Hundreds of them, loud as a riot, and every one curse-shifted mean."));
				await dialog.Msg(L("The noise alone drives off anyone trying to restore the wards. Thin them out - twenty-two will drop the ambient volume enough for my ward-crew to work."));
				await dialog.Msg(L("And don't flinch at the howl. Their howl carries curse-strain. Ward-cotton in the ears helps."));

				var response = await dialog.Select(L("Twenty-two?"),
					Option(L("I'll kill the Lemurs"), "help"),
					Option(L("Curse-strain in the howl?"), "info"),
					Option(L("Try another district"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two. Move fast - they pack in, and a packed Lemur pack is worse than the curse."));
						await dialog.Msg(L("Ward-cotton in your ears. You'll thank me."));
						break;

					case "info":
						await dialog.Msg(L("Prolonged exposure to their howl stiffens the joints. Short exposure is just a ringing in the ears."));
						await dialog.Msg(L("My senior ward-hand worked without cotton for a month. He doesn't bend his left knee anymore."));
						break;

					case "leave":
						await dialog.Msg(L("Downtown's the civic heart. If it doesn't come back, nothing else does."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killLemurs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Volume's down. Ward-crew can work. First time in six months."));
					await dialog.Msg(L("Take your pay. Two fresh cotton plugs on the house."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still howling. Keep thinning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three ward-lines reset. First ward-work in Downtown since the curse took the magistrate."));
			}
		});

		// Quest 2: Downtown Curseglass
		//-------------------------------------------------------------------------
		AddNpc(20101, L("[Glazier] Yuri"), "f_flash_63", 239, 259, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1002);

			dialog.SetTitle(L("Yuri"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Rootcrystals in Downtown fuse at their cores - the curse here was fast enough to vitrify them. The cores come out as curseglass."));
				await dialog.Msg(L("Curseglass fits into ward-windows. Looks clear, holds ward. The civic buildings need it to reopen safely."));
				await dialog.Msg(L("Five cores. My apprentice's chest has greyed - I send her home daily. She can't swing the pick anymore."));

				var response = await dialog.Select(L("Five curseglass cores?"),
					Option(L("I'll crack them"), "help"),
					Option(L("Greyed chest?"), "info"),
					Option(L("Use plain glass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look for the ones with a clear sheen inside. That's the vitrified core."));
						await dialog.Msg(L("Five clear ones. I'll work them into windows tonight."));
						break;

					case "info":
						await dialog.Msg(L("Long time in Downtown. She's wrapped in ward-cloth most of the day now. The stone spreads finger-width a month."));
						await dialog.Msg(L("She's my niece. I'll keep her employed as long as there's work she can do."));
						break;

					case "leave":
						await dialog.Msg(L("Plain glass lets the curse through the window. Defeats the whole point of a reopening."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var coreCount = character.Inventory.CountItem(650257);

				if (coreCount >= 5)
				{
					await dialog.Msg(L("Five clear cores. You have a glazier's eye - these are perfect."));
					await dialog.Msg(L("Take your pay. Civic building's reopening committee will owe you a ribbon at the ceremony."));

					character.Inventory.Remove(650257, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Keep cracking. {0} of five. Look for the clear sheen inside.", coreCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three ward-windows installed. The civic building holds its curse-line through a storm."));
			}
		});

		// Quest 3: Civic Records
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Civic Scribe] Agatha"), "f_flash_63", 303, -980, 89, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1003);

			dialog.SetTitle(L("Agatha"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Downtown civic records are everything - deeds, marriage rolls, debt ledgers. They're scattered in the record vaults with Hammer-Goblins nesting on top of them."));
				await dialog.Msg(L("Thin fifteen Hammer-Goblins, recover four record-volumes, and Downtown can legally function again. Without records, the district doesn't exist on paper."));
				await dialog.Msg(L("Bureaucratic necessity. But real."));

				var response = await dialog.Select(L("Hammer-Goblins and records?"),
					Option(L("I'll recover the records"), "help"),
					Option(L("Why do goblins nest on records?"), "info"),
					Option(L("Let the district fold"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen. Record-volumes are leather-bound, clasp-locked - they don't crumble. Look in the vault niches."));
						await dialog.Msg(L("Bring them unopened. Chain-of-custody matters for legal validity."));
						break;

					case "info":
						await dialog.Msg(L("Curse-warmth. The books radiate slight warmth where the curse-script touches the paper. Goblins curl up on anything warm."));
						await dialog.Msg(L("Not malicious. Just inconvenient."));
						break;

					case "leave":
						await dialog.Msg(L("If Downtown legally folds, the families who owned property here lose their claims. I won't let that happen."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHammerGoblins", out var killObj)) return;
				if (!quest.TryGetProgress("recoverRecords", out var recObj)) return;

				if (killObj.Done && recObj.Done)
				{
					await dialog.Msg(L("Four volumes. Intact, unopened, chain-of-custody clean. This restores legal identity to two hundred families."));
					await dialog.Msg(L("Take your pay. I'll log your name in the margin of the restoration order."));

					character.Inventory.Remove(650785, character.Inventory.CountItem(650785), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Hammer-Goblins. ");
					if (!recObj.Done)
						status += L("Recover more civic record-volumes. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two hundred families legally restored. One of them sent me a hand-stitched blanket - their great-grandmother's trade."));
			}
		});

		// Quest 4: Ritual Brand Pages
		//-------------------------------------------------------------------------
		AddNpc(20102, L("[Curse-Scholar] Hedvig"), "f_flash_63", 104, -912, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1004);

			dialog.SetTitle(L("Hedvig"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Wand-Goblins have a ritual operation going in the old bath-house. They carry ritual-brand pages - transcripts of sigils used for mass-petrifying."));
				await dialog.Msg(L("If they complete the ritual, whole blocks of Downtown shift from partial-curse to full-curse in a day. Population zero."));
				await dialog.Msg(L("Twelve Wand-Goblins, five pages. The pages feed my counter-ritual - we burn their sigils before they can fire them."));

				var response = await dialog.Select(L("Twelve and five?"),
					Option(L("I'll bring the pages"), "help"),
					Option(L("Mass-petrifying?"), "info"),
					Option(L("Evacuate Downtown"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Pages are in oiled scroll-cases. Check every goblin's belt. Pages are the priority over kills."));
						await dialog.Msg(L("Don't open the scroll-cases. Live sigils. Bring them sealed."));
						break;

					case "info":
						await dialog.Msg(L("One sigil-chain fires an area petrification over a block-radius. Fully chanted, unopposed."));
						await dialog.Msg(L("Nobody in that radius survives as flesh. Not a theoretical risk."));
						break;

					case "leave":
						await dialog.Msg(L("Where? Half the adjacent districts are already half-cursed. Downtown is the fallback."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWandGoblins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPages", out var pageObj)) return;

				if (killObj.Done && pageObj.Done)
				{
					await dialog.Msg(L("Five sealed scroll-cases. The ritual's broken - they don't have the full sigil-chain anymore."));
					await dialog.Msg(L("Take your pay. I'll run the counter-ritual tonight. Smoke goes blue when it works."));

					character.Inventory.Remove(650825, character.Inventory.CountItem(650825), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Wand-Goblins. ");
					if (!pageObj.Done)
						status += L("Recover more ritual-brand pages. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Smoke went blue. Counter-ritual took. The block-petrifier threat is off the table, for now."));
			}
		});

		// Quest 5: The Stonefrosted Alpha
		//-------------------------------------------------------------------------
		AddNpc(20103, L("[Bounty Hunter] Nikolai"), "f_flash_63", 952, -844, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_63", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_63.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Nikolai"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("An Alpha Lemur in Downtown's taken frost-petrification - a rare cold-curse variant that crusts its fur with rime that never melts."));
				await dialog.Msg(L("The lesser Lemurs defer to it. Thin ten, and the Alpha emerges to re-establish the howl-order."));
				await dialog.Msg(L("Bounty's big. The rime-fur is the only material that makes true cold-wards. Limited supply."));

				var response = await dialog.Select(L("Sounds like a fight."),
					Option(L("I'll take the Alpha"), "help"),
					Option(L("What's a cold-ward?"), "info"),
					Option(L("Leave him to the howl"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten. He hits hard with a rime-slam. Stay mobile."));
						await dialog.Msg(L("Good hunting."));
						break;

					case "info":
						await dialog.Msg(L("Ward against cold-curse variant petrification. The rare kind that traps you conscious inside the stone."));
						await dialog.Msg(L("Not a nice way to go. Cold-wards prevent it."));
						break;

					case "leave":
						await dialog.Msg(L("Maybe next month. The howl-order runs deeper every week."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var packObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (packObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Rime-fur intact. That's him. Cold-wards for a year off that pelt."));
					await dialog.Msg(L("Bounty paid, plus my share."));

					character.Variables.Perm.Remove(alphaSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Lemur, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Pack thinned. Listen - that's a howl three octaves down. He's coming."));
							await dialog.Msg(L("{#FF9966}Move! Rime-fur glints before the slam.{/}"));
							character.ServerMessage(L("{#FF9966}The Stonefrosted Alpha charges out, rime-fur steaming!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there. Don't let him retreat."));
					}
				}
				else
				{
					await dialog.Msg(L("Pack's still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rime-pelt shipped to the cold-ward forge. A dozen wards going out next week."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class LemurHowlQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1001);
		SetName(L("Lemur Howl"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Lemurs so the Downtown ward-crew can work through the curse-strained howl."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[District Warden] Grelle"), "f_flash_63");

		AddObjective("killLemurs", L("Kill howl-cursed Lemurs"),
			new KillObjective(22, new[] { MonsterId.Lemur }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

public class DowntownCurseglassQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1002);
		SetName(L("Downtown Curseglass"));
		SetType(QuestType.Sub);
		SetDescription(L("Crack Rootcrystals and bring vitrified curseglass cores to the glazier for ward-window fitting."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Glazier] Yuri"), "f_flash_63");

		AddObjective("gatherCores", L("Gather curseglass cores"),
			new CollectItemObjective(650257, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650257, 0.40f, MonsterId.Lemur);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650257, character.Inventory.CountItem(650257), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650257, character.Inventory.CountItem(650257), InventoryItemRemoveMsg.Destroyed);
	}
}

public class CivicRecordsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1003);
		SetName(L("Civic Records"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Hammer-Goblins nesting on the record vaults and recover four civic record-volumes."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Civic Scribe] Agatha"), "f_flash_63");

		AddObjective("killHammerGoblins", L("Kill Hammer-Goblins"),
			new KillObjective(15, new[] { MonsterId.Goblin2_Hammer }));

		AddObjective("recoverRecords", L("Recover civic record-volumes"),
			new CollectItemObjective(650785, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650785, 0.40f, MonsterId.Goblin2_Hammer);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650785, character.Inventory.CountItem(650785), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650785, character.Inventory.CountItem(650785), InventoryItemRemoveMsg.Destroyed);
	}
}

public class RitualBrandPagesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1004);
		SetName(L("Ritual Brand Pages"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Wand-Goblins running a mass-petrification ritual and recover their sigil-chain pages."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Scholar] Hedvig"), "f_flash_63");

		AddObjective("killWandGoblins", L("Kill ritual Wand-Goblins"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand3 }));

		AddObjective("gatherPages", L("Recover ritual-brand pages"),
			new CollectItemObjective(650825, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650825, 0.50f, MonsterId.Goblin2_Wand3);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650825, character.Inventory.CountItem(650825), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650825, character.Inventory.CountItem(650825), InventoryItemRemoveMsg.Destroyed);
	}
}

public class TheStonefrostedAlphaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_63", 1005);
		SetName(L("The Stonefrosted Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Lemur pack to draw out the Stonefrosted Alpha, then bring him down for his rare rime-fur."));
		SetLocation("f_flash_63");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Nikolai"), "f_flash_63");

		AddObjective("killPack", L("Thin the Lemur pack"),
			new KillObjective(10, new[] { MonsterId.Lemur }));

		AddObjective("killAlpha", L("Defeat the Stonefrosted Alpha"),
			new KillObjective(1, new[] { MonsterId.Lemur }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

