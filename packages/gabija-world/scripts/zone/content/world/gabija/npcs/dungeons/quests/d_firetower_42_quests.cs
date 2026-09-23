//--- Melia Script ----------------------------------------------------------
// Mage Tower 2F Quest NPCs
//--- Description -----------------------------------------------------------
// The making of the Jewel of Prominence, and the sealed stones that ask for
// their watchers to be taken off them.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower42QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8478);
	private readonly static QuestId Mq02 = new QuestId(8479);
	private readonly static QuestId Mq03 = new QuestId(8480);
	private readonly static QuestId Mq04 = new QuestId(8481);
	private readonly static QuestId Mq05 = new QuestId(8482);
	private readonly static QuestId Sq01 = new QuestId(17007);
	private readonly static QuestId Sq02 = new QuestId(17008);
	private readonly static QuestId Sq03 = new QuestId(17009);
	private readonly static QuestId Sq04 = new QuestId(17010);
	private readonly static QuestId Sq05 = new QuestId(17011);
	private readonly static QuestId Sq06 = new QuestId(8504);

	private readonly static double[,] FlameVaporSpots =
	{
		{ 1931, -1195 }, { 2018, -1558 }, { 1711, -1458 }, { 1733, -1275 },
		{ 1532, -1376 }, { 1224.35, -1330.71 }, { 1156.60, -1482.16 },
	};

	protected override void Load()
	{
		// Grita, at the second floor landing
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FTOWER42_GRITA_01", "d_firetower_42", 2458, -53, 45, c => !c.Quests.Has(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Did I tell you back in Fedimian that there's a way to defeat the demons? We can help Goddess Gabija by completing the Jewel of Prominence here."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("The Jewel is somewhere on this floor, and a monster is carrying its shell."),
					Option(L("How do you complete the Jewel of Prominence?"), "accept"),
					Option(L("Better go up quickly"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					character.Inventory.Add(ItemId.FTOWER42_MQ_01_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("This detection rod should be able to find the Jewel of Prominence. The Jewel will appear when you use this detector rod near the Jewel's location."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("The Large Reading Room is down the hall. That is where the rod will want to be."));
		});

		// Grita, once she is walking the floor with you
		//-------------------------------------------------------------------------
		// The client has her follow the player; the port stands her on the way
		// to the Large Reading Room instead.
		AddConditionalNpc(147449, L("Grita"), "FTOWER42_G_AI", "d_firetower_42", 1928, -1115, 90, c => c.Quests.Has(Mq01) && !c.Quests.HasCompleted(Mq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Well done. However, this jewel is just an empty shell."));
				await dialog.Msg(L("You can complete it by empowering it with various materials."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("We can work with this. Good job!"));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Phew... We've almost completed the Jewel of Prominence!"));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Alright! We will combine the Flame Sources into the Jewel of Prominence."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("This is the Jewel of Prominence... I can feel its great energy."));
				await dialog.Msg(L("Let's hurry and give it to Gabija. There's a stairway up on the right side."));
				await dialog.CompleteQuest(Mq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("For now, let's fill the Jewel of Prominence with Flame Vapor. There is a Flame Vapor which possesses magic in it at the reading room."),
					Option(L("I'll gather the Flame Vapor"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("You should walk around as much as you can. Flame Vapor only appears for a moment."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Next, we will need to fill the Jewel with the Essence of Fire, which is slightly complicated."),
					Option(L("I'll fill it with the Essence of Fire"), "accept"),
					Option(L("Let's rest for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.Quests.CompleteObjective(Mq03, "fillWithEssence");
					await dialog.Msg(L("Don't worry. It won't hurt you or explode."));
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("Lastly, we need to collect Flame Sources. You can obtain them from the monsters that possess the power of fire."),
					Option(L("I'll gather the Flame Sources"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("If we can complete the Jewel of Prominence, we can use it to help Gabija recover."));
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Let's head to the Fusion Machine. Once we put every material in and work the machine, the Jewel should be complete."),
					Option(L("Go complete the Jewel of Prominence"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("We must succeed..."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("I pray that the goddess can endure until then. Use the rod where the monsters are thickest."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Flame Vapor only appears for a moment. Walk the reading room and watch for it."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Eleven Flame Sources. The fire-bearing monsters of this floor carry them."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The Flame Fusion Machine is at the west end of the floor."));
				return;
			}

			await dialog.Msg(L("The Jewel of Prominence is the one thing here that can still help her."));
		});

		// The Flame Fusion Machine
		//-------------------------------------------------------------------------
		AddNpc(147307, L("Flame Fusion Machine"), "FTOWER42_MQ_05", "d_firetower_42", -1386, -2368, 88, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Flame Fusion Machine"));

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				var fused = await character.TimeActions.StartAsync(L("Working the Flame Fusion Machine..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (fused != TimeActionResult.Completed)
					return;

				character.Inventory.Remove(ItemId.FTOWER_FIRE_ESSENCE, 1, InventoryItemRemoveMsg.Given);
				character.Inventory.Add(ItemId.FTOWER_FIRE_ESSENCE_2, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq05, "fuseTheJewel");

				character.ServerMessage(L("The Jewel of Prominence is complete."));
				return;
			}

			if (!character.Quests.Has(Sq06) && character.Quests.MeetsPrerequisites(Sq06))
			{
				var answer = await dialog.SelectQuestOffer(Sq06, L("The Flame Fusion Machine in Mage Tower 2F has a function to enhance the power of the flame. Check the Flame Fusion Machine."),
					Option(L("Check the machine over"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq06);
					character.ServerMessage(L("Something comes down off the ceiling of the fusion room."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq06))
			{
				await dialog.Msg(L("The Archon is still in the fusion room."));
				character.Quests.ReplayQuestTrack(Sq06);
				return;
			}

			await dialog.Msg(L("The machine the tower's magicians used to fold one fire into another."));
		});

		// The sealed stone in the Large Reading Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER42_SQ_01", "d_firetower_42", 2024, -1564, 90, c => !c.Quests.HasCompleted(Sq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("I shall be released soon."));
				await dialog.CompleteQuest(Sq01);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("I am much more than you could ever imagine. I wish to be released from these restraints. Please defeat the nearby monitors."),
					Option(L("Defeat them"), "accept"),
					Option(L("It's strange so just ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("There's an Eye of Surveillance on Blindlems and Beleggs."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Defeat the monitors. There's an Eye of Surveillance on Blindlems and Beleggs."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The sealed stone in the forum
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER42_SQ_02", "d_firetower_42", 1190, -20, 90, c => !c.Quests.HasCompleted(Sq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Amazing. You possess great power."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("I cannot say my name. Please defeat the observers and release the seal on me."),
					Option(L("Defeat the monitors"), "accept"),
					Option(L("It's annoying so just leave"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq02);

				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("My soul is shredded and trapped in the seal. Defeat the Experimental Slime so I can get released."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("There are more of me on this floor. One of them will call you when this one is quiet."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The Golem is still watching me."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("The Experimental Slimes are down the west hall."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The sealed stone by the office hall
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER42_SQ_04", "d_firetower_42", 482, -569, 90, c => c.Quests.Has(Sq03) && !c.Quests.HasCompleted(Sq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Another me from somewhere else led you to this place. Now, I can be released."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Now, I can be released."));
				await dialog.CompleteQuest(Sq04);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				await dialog.Msg(L("But, if I get released like this, I will be split into pieces."));

				var answer = await dialog.SelectQuestOffer(Sq04, L("The Holy Ark that I was sealed within is broken, and the observers have taken those pieces."),
					Option(L("Regain the Holy Ark"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq04);

				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("Ten pieces. The monsters of this floor carry them between them."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The sealed stone in the office
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER42_SQ_05", "d_firetower_42", -1033, -674, 90, c => !c.Quests.HasCompleted(Sq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Now, I can be released."));
				await dialog.CompleteQuest(Sq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("As long as there are Eyes of Surveillance, I can't be released from my restraints. Please defeat the monitors and release me."),
					Option(L("I'll release you"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("I am tied to a contract. I seem to be free, but am not really free."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("The Blindlems and the shaman dolls. Both of them watch me."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Flame Vapor that leaks in the Large Reading Room.
		for (var i = 0; i < FlameVaporSpots.GetLength(0); i++)
		{
			var uniqueName = "FTOWER42_MQ_02_" + (i + 1);
			AddQuestTrigger(uniqueName, "d_firetower_42", FlameVaporSpots[i, 0], FlameVaporSpots[i, 1], 60, this.CatchFlameVapor);
		}
	}

	/// <summary>
	/// Catches the Flame Vapor that leaks in the Large Reading Room, filling
	/// the Jewel of Prominence with it.
	/// </summary>
	/// <param name="args"></param>
	private async Task CatchFlameVapor(TriggerActorArgs args)
	{
		if (args.Initiator is not Character character)
			return;

		if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
		{
			character.Quests.CompleteObjective(Mq02, "catchFlameVapor");
			character.ServerMessage(L("Flame Vapor rises out of the floor and the Jewel of Prominence drinks it in."));
		}

		await Task.CompletedTask;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8478: Mage Tower 2nd Floor (1)
//-----------------------------------------------------------------------------
public class Ftower42Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8478);
		SetName(L("Mage Tower 2nd Floor (1)"));
		SetDescription(L("The Jewel of Prominence is what can help the goddess, and its shell is on this floor."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_GRITA_01", "d_firetower_42", L("Talk to Grita at Mage Tower 2F"), L("You arrived on the 2nd floor of the Mage Tower. Talk to Grita about what to do next."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_G_AI", "d_firetower_42", L("Search for the Jewel of Prominence"), L("Find the Jewel of Prominence. Use the detector to look for the Jewel of Prominence on the 2nd floor of the Mage Tower."));
		SetPhase(QuestStatus.Success, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You found the Jewel of Prominence. Talk to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8477, QuestStatus.Completed));

		// The client reveals the Jewel through a detector-rod minigame; the
		// port has the monster that carries it drop it instead.
		AddPityDrop("FTOWER_FIRE_ESSENCE", 0.2f, 10, 1, "blindlem", "tower_of_firepuppet", "Chromadog", "slime_elite", "belegg");

		AddObjective("findTheJewel", L("Search for the Jewel of Prominence"), new CollectItemObjective("FTOWER_FIRE_ESSENCE", 1));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER42_MQ_01_ITEM"));
	}
}

// 8479: Mage Tower 2nd Floor (2)
//-----------------------------------------------------------------------------
public class Ftower42Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8479);
		SetName(L("Mage Tower 2nd Floor (2)"));
		SetDescription(L("The empty jewel has to be filled, and the reading room leaks the Flame Vapor it wants."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You got the Jewel of Prominence. Talk to Grita about how you can input the power into the Jewel of Prominence."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_MQ_02_1", "d_firetower_42", L("Collect Flame Vapor"), L("Grita told you that in order to complete the Jewel of Prominence, you should fill it with Flame Vapor. Fill it with the Flame Vapor that leaks in the Large Reading Room."));
		SetPhase(QuestStatus.Success, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You filled the Jewel of Prominence with Flame Vapor. Return to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8478, QuestStatus.Completed));

		AddObjective("catchFlameVapor", L("Collect Flame Vapor"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8480: Mage Tower 2nd Floor (3)
//-----------------------------------------------------------------------------
public class Ftower42Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8480);
		SetName(L("Mage Tower 2nd Floor (3)"));
		SetDescription(L("The vapor in the jewel is let out again, and what is left behind is the Essence of Fire."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You collected all the Flame Vapor. Ask Grita what to do next to complete the Jewel of Prominence."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_G_AI", "d_firetower_42", L("Charge the Jewel"), L("You should be able to gather the Essences of Fire when Flame Vapor is let out of the jewel. Fill the Jewel of Prominence with these essences."));
		SetPhase(QuestStatus.Success, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You filled the Jewel of Prominence with the Essence of Fire. Talk to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8479, QuestStatus.Completed));

		AddObjective("fillWithEssence", L("Talk to Grita"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8481: Mage Tower 2nd Floor (4)
//-----------------------------------------------------------------------------
public class Ftower42Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8481);
		SetName(L("Mage Tower 2nd Floor (4)"));
		SetDescription(L("The jewel still wants Flame Sources, and the fire-bearing monsters of the floor carry them."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You filled all the Essences of Fire. Ask Grita what to do next."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_G_AI", "d_firetower_42", L("Obtain Flame Sources by defeating monsters"), L("Grita says the power gathered so far is not enough and that the source should be enhanced. Defeat the monsters in Mage Tower 2F and collect Flame Sources."));
		SetPhase(QuestStatus.Success, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You collected enough Flame Sources. Talk to Grita again."));

		AddPrerequisite(new QuestStatusPrerequisite(8480, QuestStatus.Completed));

		AddPityDrop("FTOWER42_MQ_04_ITEM", 0.5f, 4, 1, "blindlem", "tower_of_firepuppet", "Chromadog", "slime_elite", "belegg");

		AddObjective("collectSources", L("Obtain Flame Sources by defeating monsters"), new CollectItemObjective("FTOWER42_MQ_04_ITEM", 11));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER42_MQ_04_ITEM"));
	}
}

// 8482: Mage Tower 2nd Floor (5)
//-----------------------------------------------------------------------------
public class Ftower42Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8482);
		SetName(L("Mage Tower 2nd Floor (5)"));
		SetDescription(L("Everything the jewel needs goes into the Flame Fusion Machine at the west end of the floor."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("Now, there is one more step to complete the Jewel of Prominence. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_MQ_05", "d_firetower_42", L("Combine materials with the Jewel of Prominence in the Flame Fusion Machine"), L("Grita told you that since the Jewel of Prominence is not complete yet, you should combine materials with it in the Flame Fusion Machine."));
		SetPhase(QuestStatus.Success, "FTOWER42_G_AI", "d_firetower_42", L("Talk to Grita"), L("You successfully completed the Jewel of Prominence. Talk to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8481, QuestStatus.Completed));

		AddObjective("fuseTheJewel", L("Combine materials with the Jewel of Prominence in the Flame Fusion Machine"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8504: Flame Fusion Room Archon
//-----------------------------------------------------------------------------
public class Ftower42Sq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8504);
		SetName(L("Flame Fusion Room Archon"));
		SetDescription(L("Something had been left sitting over the Flame Fusion Machine."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_MQ_05", "d_firetower_42", L("Check the Flame Fusion Machine"), L("The Flame Fusion Machine in Mage Tower 2F has a function to enhance the power of the flame. Check the Flame Fusion Machine."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_MQ_05", "d_firetower_42", L("Defeat Archon"), L("Archon appeared as you were checking the Flame Fusion Machine. Defeat Archon."));
		SetPhase(QuestStatus.Success, "FTOWER42_MQ_05", "d_firetower_42", L("Defeat Archon"), L("Archon appeared as you were checking the Flame Fusion Machine. Defeat Archon."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER42_SQ_06_TRACK", "m_boss_b", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(106));

		AddObjective("killArchon", L("Defeat Archon"), new KillObjective(1, "boss_archon_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The fusion room is quiet again."));
		character.Quests.Complete(this.QuestId);
	}
}

// 17007: Suspicious Voice
//-----------------------------------------------------------------------------
public class Ftower42Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17007);
		SetName(L("Suspicious Voice"));
		SetDescription(L("A stone in the Large Reading Room asks for the things that watch it to be taken away."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_SQ_01", "d_firetower_42", L("Talk to the Sealed Stone"), L("There's a strange sound coming from the Large Reading Room. Talk to the Sealed Stone."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_SQ_01", "d_firetower_42", L("Defeat Belegg and Blindlem"), L("A stone emitting strange sounds said its soul was trapped. It's hard to believe, but defeat the Blindlem and Beleggs as the stone requested."));
		SetPhase(QuestStatus.Success, "FTOWER42_SQ_01", "d_firetower_42", L("Talk to the Sealed Stone"), L("Defeated the Blindlem and Beleggs. Return to the Sealed Stone and release its trapped soul."));

		AddPrerequisite(new LevelPrerequisite(106));

		AddObjective("killBlindlems", L("Defeat Blindlem"), new KillObjective(5, "blindlem"));
		AddObjective("killBeleggs", L("Defeat Belegg"), new KillObjective(5, "belegg"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17008: Sealed Soul (1)
//-----------------------------------------------------------------------------
public class Ftower42Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17008);
		SetName(L("Sealed Soul (1)"));
		SetDescription(L("The stone in the forum is watched by a Golem that does not leave it."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_SQ_02", "d_firetower_42", L("Talk to the Sealed Stone"), L("The Sealed Stone is in the forum. It seems like it needs help so talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_SQ_02", "d_firetower_42", L("Defeat Golem"), L("The Sealed Stone asked you to defeat the monitors and free its spirit. Do the Sealed Stone a favor."));
		SetPhase(QuestStatus.Success, "FTOWER42_SQ_02", "d_firetower_42", L("Talk to the Sealed Stone"), L("Defeated the Golem. Talk to the Sealed Stone."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER42_SQ_02_TRACK", "m_boss_b", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(106));

		AddObjective("killGolem", L("Defeat Golem"), new KillObjective(1, "boss_Golem_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
	}
}

// 17009: Sealed Soul (2)
//-----------------------------------------------------------------------------
public class Ftower42Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17009);
		SetName(L("Sealed Soul (2)"));
		SetDescription(L("The next piece of the same soul is guarded by the floor's Experimental Slimes."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_SQ_02", "d_firetower_42", L("Talk to the Sealed Stone"), L("The Sealed Stone seems to have another favor. Talk to the Sealed Stone."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_SQ_02", "d_firetower_42", L("Defeat Experimental Slime"), L("There are more seal stones. Defeat the Experimental Slime that is guarding it to find other seal stones by talking to it."));
		SetPhase(QuestStatus.Success, "FTOWER42_SQ_04", "d_firetower_42", L("Talk to another seal stone"), L("There are more seal stones. Defeat the Experimental Slime that is guarding it to find other seal stones by talking to it."));

		AddPrerequisite(new QuestStatusPrerequisite(17008, QuestStatus.Completed));

		AddObjective("killSlimes", L("Defeat Experimental Slime"), new KillObjective(10, "slime_elite"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17010: Sealed Soul (3)
//-----------------------------------------------------------------------------
public class Ftower42Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17010);
		SetName(L("Sealed Soul (3)"));
		SetDescription(L("The broken Holy Ark has to be put back together before the soul in the stone is let go."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_SQ_04", "d_firetower_42", L("Talk to the Sealed Stone"), L("The Sealed Stone needs help. Talk to the Sealed Stone."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_SQ_04", "d_firetower_42", L("Defeat the monsters and get pieces of the Holy Ark"), L("The Seal Stone will just scatter away when released so you need the Holy Ark to hold its soul. Collect the pieces of the Holy Ark from the monsters in Mage Tower 2F."));
		SetPhase(QuestStatus.Success, "FTOWER42_SQ_04", "d_firetower_42", L("Talk to another seal stone"), L("Return to the Sealed Stone and release the spirit."));

		AddPrerequisite(new QuestStatusPrerequisite(17009, QuestStatus.Completed));

		AddPityDrop("FTOWER42_SQ_04_01", 0.5f, 4, 1, "blindlem", "tower_of_firepuppet", "Chromadog", "slime_elite", "belegg");

		AddObjective("collectArkPieces", L("Defeat the monsters and get pieces of the Holy Ark"), new CollectItemObjective("FTOWER42_SQ_04_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER42_SQ_04_01"));
	}
}

// 17011: Release Me
//-----------------------------------------------------------------------------
public class Ftower42Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17011);
		SetName(L("Release Me"));
		SetDescription(L("The stone in the office is held by the Blindlems and the shaman dolls around it."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_42");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER42_SQ_05", "d_firetower_42", L("Talk to the Sealed Stone"), L("There is a Sealed Stone in the office. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER42_SQ_05", "d_firetower_42", L("Defeat the monsters nearby to release it"), L("The spirit that is tied to the seal stone wants to be freed. Defeat Blindlems and the spell dolls to set the spirit free from its restraints."));
		SetPhase(QuestStatus.Success, "FTOWER42_SQ_05", "d_firetower_42", L("Talk to the Sealed Stone"), L("Defeated the Blindlems and Spell dolls. Return to the seal stone and free its spirit."));

		AddPrerequisite(new LevelPrerequisite(106));

		AddObjective("killBlindlems", L("Defeat Blindlem"), new KillObjective(5, "blindlem"));
		AddObjective("killPuppets", L("Defeat Shaman Doll"), new KillObjective(10, "tower_of_firepuppet"));

		AddReward(new ItemReward("expCard7", 1));
	}
}
