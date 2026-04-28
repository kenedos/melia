//--- Melia Script ----------------------------------------------------------
// Grynas Hills - Quest NPCs
//--- Description -----------------------------------------------------------
// A mix of the living and the dead haunt the Grynas Hills.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.Scripting.Dialogues;

public class FKatyn453QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Ring-Breaker — GHOST "it's over"
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Ring-Breaker"), "f_katyn_45_3", -1551, 1468, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1001);
			dialog.SetTitle(L("Ring-Breaker"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("It's over... It's over... I crossed the ring... It's over..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Big ring... I should have kicked it... I tripped instead... I crossed it..."));
					await dialog.Msg(L("It took something from me... I cannot remember what... Only that it's over... It's over..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will break the rings before they close"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty ring-starters... Kick them out of their compass... So no other walker... Says it's over...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty ring-starters... Kick them out of their compass... So no other walker... Says it's over..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGriba", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("Twenty rings broken... I almost remember what was taken from me... Almost..."));
					await dialog.Msg(L("Take this purse... Ring-Breaker's coin... A kick-knife... Pass it on..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More rings... Don't trip... Don't trip..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("It is not over... It is not over... I rest knowing this..."));
			}
		});

		// Quest 1002 giver: Fisherwoman Onute — ALIVE, scared near goddess statue
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Fisherwoman] Onute"), "f_katyn_45_3", -516, -463, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1002);
			dialog.SetTitle(L("Onute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An old woman in a fisher-coat presses her forehead to the goddess-statue's base, eyes shut, lips moving*{/}"));
				await dialog.Msg(L("The statue keeps the spirits off me. I do not leave its shadow. I cannot. They drowned my husband in a pond that went dry thirty years before he died. They will drown me in dry air if I step away."));
				var response = await dialog.Select(L("Carry this rosary to my sister Genute. She tends his memorial - her ghost does, since the spirits took her too. The memorial accepts a renewal, and Genute can rest a season more. Will you?"),
					Option(L("I'll carry the rosary"), "help"),
					Option(L("Why ghost-tend a memorial?"), "info"),
					Option(L("Stay in the statue's shadow then"), "leave")
				);
				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Genute waits at the dry pond. She wears a blue kerchief still. Don't speak to the ghost-fishers casting in the dry bowl."));
						break;
					case "info":
						await dialog.Msg(L("She would not leave the memorial when she died. The wood took her body but her devotion stayed. She tends. I pray. The statue holds."));
						break;
					case "leave":
						await dialog.Msg(L("Then his memorial decays another season. The fisher-spirits will notice. The statue may not be enough then."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverRosary", out var deliverObj)) return;
				if (deliverObj.Done)
				{
					await dialog.Msg(L("Aleksas's bead. Warm. The memorial accepted. He settles another season. Genute settles with him."));
					await dialog.Msg(L("Fisherwoman's coin, what I can spare. Keep the bead a night - the dream is Aleksas's thanks."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Eastern dry-pond rim. Blue kerchief. Don't kneel."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Aleksas was quiet last night. Genute too. The statue holds, and so do they. So do I."));
			}
		});

		// Lore NPC: Scared villager at second statue position
		//---------------------------------------------------------------------
		AddNpc(20114, L("[Faithful] Joana"), "f_katyn_45_3", -329, -386, 0, async dialog =>
		{
			dialog.SetTitle(L("Joana"));
			await dialog.Msg(L("{#666666}*A young woman kneels at the goddess-statue's foot, prayer-cord wound through her fingers*{/}"));
			await dialog.Msg(L("My grandmother sat where I sit. Her grandmother before her. The spirits do not cross the statue's shadow."));
			await dialog.Msg(L("Onute prays the long prayer. I pray the short one. We trade off, so the statue is never alone with the wood at its back."));
			await dialog.Msg(L("If you walk on into the hills... Walk fast. Don't kneel near a dry bowl. Don't whistle."));
		});

		// Quest 1002 recipient: Memorial-Keeper Genute — GHOST "way is shut"
		//---------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Memorial-Keeper"), "f_katyn_45_3", -601, 2087, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1002);
			dialog.SetTitle(L("Memorial-Keeper"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("The way is shut... The way is shut... The pond went dry... My way home is shut..."));
					await dialog.Msg(L("My sister sends the rosary... At last... At last... I bind it to the marker..."));
					await dialog.Msg(L("{#666666}*She unties Aleksas's bead from her wrist-cord, wraps it in the same wax-paper, presses it back to you*{/}"));
					await dialog.Msg(L("Carry the bead... Tell Onute the memorial accepted... Tell her the way is shut still... But quieter now... Quieter..."));
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Aleksas's bead received. Return to Fisherwoman Onute.{/}"));
				}
				else await dialog.Msg(L("The bead... Carry it back... My sister waits..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Aleksas whistled twice last dusk... His work-whistle... The way is shut... But we hear each other through the wood..."));
			}
			else
			{
				await dialog.Msg(L("The way is shut... The way is shut..."));
			}
		});

		// Quest 1003: Dream-Diviner Rima — ALIVE, powerful adventurer fighting spirits
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Spirit-Hunter] Rima"), "f_katyn_45_3", 953, -786, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1003);
			dialog.SetTitle(L("Rima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A scarred woman in salt-crusted robes stands sword-drawn over a dispersing shade. The shade's last whisper hangs in the cold air, then dissolves*{/}"));
				await dialog.Msg(L("These hills swarm with spirits. I cull them so the living can pass. Forest-jellyfish drift between trunks - their tear-fronds carry true dreams of the drowned. I need eight."));
				var response = await dialog.Select(L("Cut the trailing fronds, never the bell. Approach from below. Will you gather while I hold the line?"),
					Option(L("I'll gather eight tear-fronds"), "help"),
					Option(L("True dreams of the drowned?"), "info"),
					Option(L("I won't cross your path"), "leave")
				);
				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("If a bell pulses toward you - drop the frond and walk. The bell remembers. Fronds can be replaced."));
						break;
					case "info":
						await dialog.Msg(L("Drownings, wherever and whenever. The fisher-widows pay for true dreams. I find their kin. I cut down what crosses me. Both halves of the work."));
						break;
					case "leave":
						await dialog.Msg(L("Then walk wide. The spirits do not cross my blade, but they cross fast walkers."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var frondCount = character.Inventory.CountItem(650852);
				if (frondCount >= 8)
				{
					await dialog.Msg(L("Eight fronds. Six steep clean. Two cried before you cut. Those go in the locked drawer."));
					await dialog.Msg(L("Hunter's share. Tear-water vial - rinse your eyes if a dream overstays."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Below the bell. Trailing frond. Eight."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three widows received true answers this week. One son came home after seven years. The fronds earn their keep."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class BreakTheClosingRingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1001);
		SetName("The Ring-Breaker's Last Word");
		SetType(QuestType.Sub);
		SetDescription("A ring-breaker's ghost says it's over. He begs for twenty Griba ring-starters to be kicked apart so no other walker says the same.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Ring-Breaker", "f_katyn_45_3");
		AddObjective("killGriba", "Defeat ring-starter Griba mushrooms",
			new KillObjective(20, new[] { MonsterId.Mushroom_Boy_Yellow }));
		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class ARosaryForTheDrownedPondQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1002);
		SetName("A Rosary for the Drowned Pond");
		SetType(QuestType.Sub);
		SetDescription("Fisherwoman Onute's rosary must reach her sister's ghost at the dry pond memorial, and Aleksas's bead must return to confirm the renewal.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Fisherwoman] Onute", "f_katyn_45_3");
		AddObjective("deliverRosary", "Deliver the rosary to the Memorial-Keeper's ghost",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1002.Delivered");
	}
}

public class JellyfishTearFrondsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1003);
		SetName("Jellyfish Tear-Fronds");
		SetType(QuestType.Sub);
		SetDescription("Spirit-Hunter Rima needs eight tear-fronds cut from drifting forest-jellyfish for her dream-divinations.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Spirit-Hunter] Rima", "f_katyn_45_3");
		AddObjective("collectFronds", "Gather tear-fronds from forest-jellyfish",
			new CollectItemObjective(650852, 8));
		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650852, 0.40f, MonsterId.Jellyfish_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650852, character.Inventory.CountItem(650852), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650852, character.Inventory.CountItem(650852), InventoryItemRemoveMsg.Destroyed);
	}
}
