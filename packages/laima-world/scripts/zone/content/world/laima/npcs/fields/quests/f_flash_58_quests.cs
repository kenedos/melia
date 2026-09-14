//--- Melia Script ----------------------------------------------------------
// Dingofasil District Quest NPCs
//--- Description -----------------------------------------------------------
// Quests for Dingofasil District (cursed petrification).
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
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

public class FFlash58QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quarantine Cordon
		//-------------------------------------------------------------------------
		AddNpc(147338, L("Boulder"), "f_flash_58", -777, -1411, 45);

		AddNpc(40080, L("Quarantine Notice"), "f_flash_58", -703, -1428, 45, async dialog =>
		{
			dialog.SetTitle(L("Notice"));
			await dialog.Msg(L("{#AA0000}— BY ORDER OF THE SANCTIFIED TRIBUNAL —{/}"));
			await dialog.Msg(L("This district is under quarantine writ for active heretical investigation. The petrification curse has been classified a Class III rite-borne hazard. Unauthorized entry is forbidden."));
			await dialog.Msg(L("All persons crossing this line do so at their own peril and become subject to inquiry. Report to Inquisitor Vytautas at the inner cordon for writ-of-passage."));
			await dialog.Msg(L("{#666666}— Sealed under hand and ring of Vytautas, Inquisitor of the Sanctified Tribunal —{/}"));
		});

		// Quest 1: Red Infroholder Raid
		//-------------------------------------------------------------------------
		var haltGuard = AddNpc(58291, L("Inquisitorial Guard"), "f_flash_58", 162, -1054, 270);
		AddCombatNpc(58292, L("Inquisitorial Guard"), "f_flash_58", 142, -1019, 240, level: 90);
		AddCombatNpc(58293, L("Inquisitorial Guard"), "f_flash_58", 102, -1019, 300, level: 90);
		AddCombatNpc(58291, L("Inquisitorial Guard"), "f_flash_58", 82, -1054, 90, level: 90);
		AddCombatNpc(58292, L("Inquisitorial Guard"), "f_flash_58", 102, -1089, 60, level: 90);
		AddCombatNpc(58293, L("Inquisitorial Guard"), "f_flash_58", 142, -1089, 120, level: 90);

		AddAreaTrigger("f_flash_58", 276, -1086, 80, async (args) =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.IsDead)
				return;

			var writQuestId = new QuestId("f_flash_58", 1001);
			if (character.Quests.IsActive(writQuestId) || character.Quests.HasCompleted(writQuestId))
				return;

			if (haltGuard != null && !haltGuard.IsDead)
				haltGuard.Say(L("Halt! No one passes the inner cordon without the Inquisitor's writ!"));

			character.StartBuff(BuffId.Hold, 1, 0, TimeSpan.FromSeconds(2), character);
			await Task.Delay(TimeSpan.FromSeconds(2));
			character.Warp("f_flash_58", 20, character.Position.Y, -1076);
		});

		AddNpc(153169, L("[Inquisitor] Vytautas"), "f_flash_58", 122, -1054, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1001);

			dialog.SetTitle(L("Vytautas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Stop right there. I'm Vytautas, Inquisitor of the Sanctified Tribunal. This district is mine until the curse is broken."));
				await dialog.Msg(L("I lost twelve of my men when the curse hit. Twelve good soldiers, turned to stone where they stood. And the citizens — hundreds of them, frozen in the streets. Mothers, children, all of them."));
				await dialog.Msg(L("A heretic cult did this. The Stoneheart Choir. They're going to pay for every single one of those people, and I'm going to make sure of it."));
				await dialog.Msg(L("If you've got the stomach for it, help me hunt them down. Kill the monsters running loose in this district. Every twenty-five you put down, I'll give you a recipe for one of the Goddess's Retribution potions. Fair trade."));

				var response = await dialog.Select(L("Will you help?"),
					Option(L("I'll help you hunt them."), "help"),
					Option(L("What's a Goddess's Retribution potion?"), "info_potion"),
					Option(L("Tell me about your men."), "info_fallen"),
					Option(L("Not interested."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Good. Get to work. Don't come back empty-handed."));
						break;

					case "info_potion":
						await dialog.Msg(L("It's a potion blessed by the Goddess Herself. When She's marked something as evil, this potion destroys it. No defense, no cure — it just works."));
						await dialog.Msg(L("There are five different recipes, one for each kind of creature it ruins. I've got all five, and I'll hand them out as you earn them. One recipe per twenty-five kills."));
						break;

					case "info_fallen":
						await dialog.Msg(L("My second-in-command was Vakaris. Twenty-three years he served the Tribunal. He was reading the writ at the cordon when the curse caught him mid-sentence. His body's still standing there. We can't move him."));
						await dialog.Msg(L("There were eleven others. I knew every one of them. Every heretic in this district is going to answer for them — that's a promise."));
						break;

					case "leave":
						await dialog.Msg(L("Then get out of my way. I've got no use for cowards."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("burnHeretics", out var killObj)) return;

				var pendingKills = killObj.Count;
				var rewards = pendingKills / 25;

				if (rewards > 0)
				{
					var consumed = rewards * 25;
					var recipes = new[] { 911011, 911012, 911013, 911014, 911015 };
					for (var i = 0; i < rewards; i++)
					{
						var recipeId = recipes[GameRandom.Get().Next(recipes.Length)];
						character.Inventory.Add(recipeId, 1, InventoryAddType.PickUp);
					}

					killObj.Count = Math.Max(0, killObj.Count - consumed);
					character.Quests.UpdateClient_UpdateQuest(quest);

					if (rewards == 1)
						await dialog.Msg(L("Twenty-five down, one recipe earned. Take it. Use it well."));
					else
						await dialog.Msg(LF("{0} recipes earned. Take them all. Don't waste them.", rewards));
				}
				else
				{
					var nextThreshold = 25 - pendingKills;
					if (pendingKills == 0)
						await dialog.Msg(L("Empty-handed already? Twenty-five kills for the first recipe. Get moving."));
					else
						await dialog.Msg(LF("{0} more kills before you earn the next recipe. Keep at it.", nextThreshold));
				}

				var response = await dialog.Select(L("Anything else?"),
					Option(L("Remind me about the potion."), "info_potion"),
					Option(L("Back to it."), "leave")
				);

				if (response == "info_potion")
				{
					await dialog.Msg(L("A potion blessed by the Goddess. It destroys whatever She's marked as evil. No defense against it. Five recipes, five kinds of creature."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("You walked away from my writ. The Goddess noted it. So did I. If you find your spine again, take the writ again — until then, stay out of my line."));
			}
		});

		// Quest 2: Socket Cores
		//-------------------------------------------------------------------------
		AddNpc(20117, L("[Cursebreaker] Algimantas"), "f_flash_58", 962, 527, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1002);

			dialog.SetTitle(L("Algimantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I've spent three years studying the curse that turned this district to stone. The Purple Sockets you see clinging to the walls aren't monsters in the usual sense. They're vessels — each one carries a glowing core that anchors the petrification field around it."));
				await dialog.Msg(L("I won't lie to you — I don't have the strength to weave a counter-curse that can match what's been laid here. Not on my own. But if you bring me eight intact cores, I can try. A weaker ward than the curse deserves, but better than standing here watching it spread."));

				var response = await dialog.Select(L("Will you bring me the cores?"),
					Option(L("I'll bring them."), "help"),
					Option(L("How does the anchor work?"), "info"),
					Option(L("Not today."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Bring only the cores — the outer shells crumble the moment the Socket dies, so don't waste time on them. The cores are tougher and stay intact if you handle them carefully."));
						break;

					case "info":
						await dialog.Msg(L("Each Socket projects a petrification field in a small radius around itself. Where the fields overlap, the curse hardens fastest. Break eight cores in the right pattern and the overlapping fields split apart."));
						break;

					case "leave":
						await dialog.Msg(L("Then the curse deepens. Come back when you change your mind — I'll still be here, weaving what I can with what little I have."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("gatherCores", out var cObj)) return;

				if (cObj.Done)
				{
					await dialog.Msg(L("Eight cores, all intact. More than I expected. I'll start weaving tonight — no promises on how strong it'll hold, but I'll do what I can with these. Thank you for trusting a half-finished plan."));
					character.Inventory.Remove(662127, character.Inventory.CountItem(662127), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep breaking Sockets. Not every one carries a usable core, so don't be surprised if you need to break a few before you find what I need."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The ward came out weaker than I'd hoped — it's only holding two blocks, not the four I aimed for. But two is two more than we had yesterday. People are walking those streets again. That's something."));
			}
		});

		// Quest 5: The Stoneheart Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Ignas"), "f_flash_58", 1126, 1095, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_58", 1005);

			dialog.SetTitle(L("Ignas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("An Infroholder Stoneheart anchors the district curse. Kill ten to draw him from his plaza."));

				var response = await dialog.Select(L("Will you face the Stoneheart?"),
					Option(L("I'll face him"), "help"),
					Option(L("Stoneheart?"), "info"),
					Option(L("Skip"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten."));
						break;

					case "info":
						await dialog.Msg(L("His heartbeat drives the petrification pulse. End him, drop the pulse."));
						break;

					case "leave":
						await dialog.Msg(L("Curse beats on."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var pObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var aObj)) return;

				if (pObj.Done && aObj.Done)
				{
					await dialog.Msg(L("Pulse stopped."));
					character.Quests.Complete(questId);
				}
				else if (pObj.Done && !aObj.Done)
				{
					await dialog.Msg(L("Find him."));
				}
				else
				{
					await dialog.Msg(L("Ten first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Plaza's unlocked. Cursebreakers pouring in."));
			}
		});

	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class FFlash58Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1001);
		SetName(L("Inquisitor's Writ of Wrath"));
		SetType(QuestType.Sub);
		SetDescription(L("Hunt the monsters in Dingofasil District. Every twenty-five kills earns a Goddess's Retribution potion recipe from Inquisitor Vytautas."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Inquisitor] Vytautas"), "f_flash_58");

		AddObjective("burnHeretics", L("Slay monsters in Dingofasil District"),
			new UnlimitedKillObjective((mob, character) =>
				mob.Id == MonsterId.Infroholder_Red
				|| mob.Id == MonsterId.Infroholder_Mage_Green
				|| mob.Id == MonsterId.Socket_Purple));
	}
}

public class FFlash58Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1002);
		SetName(L("Socket Curse-Cores"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Purple Sockets and bring curse-cores for the counter-ward."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Cursebreaker] Algimantas"), "f_flash_58");

		AddObjective("gatherCores", L("Gather curse-cores from Purple Sockets"),
			new CollectItemObjective(662127, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(662127, 0.40f, MonsterId.Socket_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662127, character.Inventory.CountItem(662127), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662127, character.Inventory.CountItem(662127), InventoryItemRemoveMsg.Destroyed);
	}
}

public class FFlash58Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_58", 1005);
		SetName(L("The Stoneheart Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Red Infroholders to draw out the Stoneheart Alpha anchoring the curse."));
		SetLocation("f_flash_58");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Ignas"), "f_flash_58");

		AddObjective("killPack", L("Kill Red Infroholders"),
			new KillObjective(10, new[] { MonsterId.Infroholder_Red }));

		AddObjective("killAlpha", L("Defeat the Stoneheart Alpha"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Stonefroster, 1) },
				resetIdent: "killPack",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(222125, 1));
		AddReward(new ItemReward(640086, 5));
		AddReward(new ItemReward(640004, 6));
		AddReward(new ItemReward(640007, 6));
		AddReward(new ItemReward(640013, 3));
	}
}

