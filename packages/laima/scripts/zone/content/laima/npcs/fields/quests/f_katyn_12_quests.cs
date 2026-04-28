//--- Melia Script ----------------------------------------------------------
// Letas Stream - Quest NPCs
//--- Description -----------------------------------------------------------
// Restless ghosts of those who died along the Letas stream.
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

public class FKatyn12QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Stream-Warden ghost — killed by Chupacabras
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Stream-Warden"), "f_katyn_12", -289, -1009, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1001);
			dialog.SetTitle(L("Stream-Warden"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Iron... Iron in the water... I tasted iron..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("The Chupacabras... They drank from the Letas... I knelt to wipe my blade... I drank too..."));
					await dialog.Msg(L("Bleeding... Bleeding from the gums... For three days... Then... Then nothing..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will avenge you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Twenty... Twenty Chupacabras... So no other warden... Drinks the iron...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Twenty... Twenty Chupacabras... So no other warden... Drinks the iron..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killChupacabra", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The water... The water tastes like water again... Take this purse... A warden's coin..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("More... More of them at the bend..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Quiet stream... I can almost hear my own footsteps... Almost..."));
			}
		});

		// Quest 1002 giver: Waterclerk ghost
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Waterclerk"), "f_katyn_12", -2241, 526, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);
			dialog.SetTitle(L("Waterclerk"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The vial... The vial... I never delivered the vial..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Sample for the chemist... Brone... Waiting on the path... Waiting still... I never reached him..."));
					await dialog.Msg(L("Robbers... Or beasts... I cannot remember which... I only remember falling..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will deliver it for you"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Brone waits... On the dead-end path... East... East... Take the vial...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Brone waits... On the dead-end path... East... East... Take the vial..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverSample", out var deliverObj)) return;
				if (deliverObj.Done)
				{
					await dialog.Msg(L("He read it... He read it at last... I can let go..."));
					await dialog.Msg(L("Take this... A clerk's purse... Soaked through... But the coin still rings..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("East path... Dead-end... Brone is waiting..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Both of us... We can both let go now..."));
			}
		});

		// Quest 1002 recipient: Chemist ghost on dead-end path
		//-------------------------------------------------------------------------
		AddGhostNpc(155132, L("[Restless Soul] Chemist"), "f_katyn_12", 1976, -112, 179, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1002);
			dialog.SetTitle(L("Chemist"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("This path... I died on this path... I died on this path..."));
					await dialog.Msg(L("Walls on either side... A dead end... I could not turn back... I could not turn back..."));
					await dialog.Msg(L("The vial... You bring the vial... At last... At last..."));
					await dialog.Msg(L("I read the numbers... Iron... So much iron... It was always going to climb... Always..."));
					character.Variables.Perm.Set("Laima.Quests.f_katyn_12.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Sample read. Return to the Waterclerk.{/}"));
				}
				else await dialog.Msg(L("Back to her... Back to her... Tell her... Tell her I read the numbers..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("This path... It does not feel so long now... Not so long..."));
			}
			else
			{
				await dialog.Msg(L("Path... Path... I died on this path..."));
				await dialog.Msg(L("She never came... With the vial... She never came..."));
			}
		});

		// Quest 1003: Mist-Reader ghost — Operor fog gatherer
		//-------------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Mist-Reader"), "f_katyn_12", -3044, 1496, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1003);
			dialog.SetTitle(L("Mist-Reader"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Words... Words taken from me... Eight words... I cannot say them anymore..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Operor... I trapped their fog... For readings... For the missing... One inhaled while I stoppered..."));
					await dialog.Msg(L("It took the word for my own name... Then another... And another... Until I had no name to call when I fell..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will gather the fog you could not"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Eight vials... Stopper them carefully... So the fog... The fog gives back what it stole...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Eight vials... Stopper them carefully... So the fog... The fog gives back what it stole..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var vialCount = character.Inventory.CountItem(650854);
				if (vialCount >= 8)
				{
					await dialog.Msg(L("Eight... Eight stoppered tight... I can hear my name again... I can almost say it..."));
					await dialog.Msg(L("Take this... Reader's purse... My last reading was for myself... It said: rest...."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Stopper fast... Stopper fast... Or it takes more words..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My name... I have my name back... I will not say it... I will save it for the door..."));
			}
		});

		// Quest 1004: Chapel-Keeper ghost in front of her own grave
		//-------------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Chapel-Keeper"), "f_katyn_12", 1333, 1294, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_12", 1004);
			dialog.SetTitle(L("Chapel-Keeper"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("My grave... My own grave... I keep watch over my own grave..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Crypt-break... The Rodelin came up through the chapel floor... I tried to bless them back... My benediction... Not enough..."));
					await dialog.Msg(L("They buried me here... In front of the chapel I could not save... My aunt is one of them now..."));
					await dialog.Msg(L("Four chapel-stones still stand... If any hold benediction... The chapel survives..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will read the stones"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Hand above each... Warm... Cold... Silent... Tell me which is which... Which is which...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Hand above each... Warm... Cold... Silent... Tell me which is which... Which is which..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var stonesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 0);
				if (stonesChecked >= 4)
				{
					await dialog.Msg(L("Two warm... One cold... One silent... The chapel stands on two stones... Thin... But enough..."));
					await dialog.Msg(L("Take this... Keeper's stipend... Every coin... Every coin..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Hand above each... Tell me what the stones say..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two stones holding... My aunt drifts further now... For her sake... For her sake..."));
			}
		});

		// Chapel-stone inspection points (kept as inspection pattern)
		//-------------------------------------------------------------------------
		void AddChapelStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(stoneName), "f_katyn_12", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_12", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A chapel-stone half-buried in ruin*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_12.Quest1004.Stone{stoneNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have already read this stone*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Holding a hand above the stone..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var stonesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", stonesChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Stones read: {0}/4", stonesChecked + 1));
					if (stonesChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All stones read! Return to the Chapel-Keeper.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddChapelStone(1, "Nave Stone", "Nave Stone: warm to the palm. Benediction holds.", 1156, 1158, 0);
		AddChapelStone(2, "Altar Stone", "Altar Stone: warm. Strongest of the four.", 1377, 1603, 90);
		AddChapelStone(3, "West-Aisle Stone", "West-Aisle Stone: cold. Drained but recoverable.", 1579, 94, 180);
		AddChapelStone(4, "Crypt-Step Stone", "Crypt-Step Stone: silent. Nothing answers the hand.", 917, 90, 270);
	}
}

// Quest 1001
public class ChupacabrasOnTheStreamPathQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1001);
		SetName("The Stream-Warden's Iron");
		SetType(QuestType.Sub);
		SetDescription("A drowned warden's ghost begs for the Corrupt Chupacabras that poisoned the stream to be put down.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Stream-Warden", "f_katyn_12");
		AddObjective("killChupacabra", "Defeat Corrupt Chupacabras",
			new KillObjective(20, new[] { MonsterId.Chupacabra_Green }));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

// Quest 1002
public class TheWaterSampleQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1002);
		SetName("The Undelivered Sample");
		SetType(QuestType.Sub);
		SetDescription("A waterclerk's ghost begs you to deliver the sample she never got to bring to the chemist's ghost on the dead-end path.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Waterclerk", "f_katyn_12");
		AddObjective("deliverSample", "Deliver the sample to the Chemist's ghost",
			new VariableCheckObjective("Laima.Quests.f_katyn_12.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1002.Delivered");
	}
}

// Quest 1003
public class OperorFogVialsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1003);
		SetName("The Mist-Reader's Words");
		SetType(QuestType.Sub);
		SetDescription("A mist-reader's ghost begs for eight Operor fog-vials so the fog may give back the words it stole from her.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Mist-Reader", "f_katyn_12");
		AddObjective("collectFog", "Trap Operor fog into vials",
			new CollectItemObjective(650854, 8));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650854, 0.40f, MonsterId.Operor_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650854, character.Inventory.CountItem(650854), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650854, character.Inventory.CountItem(650854), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004
public class TheChapelStonesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_12", 1004);
		SetName("The Chapel-Stones");
		SetType(QuestType.Sub);
		SetDescription("A chapel-keeper's ghost watches over her own grave and begs for the four chapel-stones to be read.");
		SetLocation("f_katyn_12");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Chapel-Keeper", "f_katyn_12");
		AddObjective("readStones", "Read the four chapel-stones",
			new VariableCheckObjective("Laima.Quests.f_katyn_12.Quest1004.StonesChecked", 4, true));
		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_12.Quest1004.StonesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_12.Quest1004.Stone{i}");
	}
}
