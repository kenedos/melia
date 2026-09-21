//--- Melia Script ----------------------------------------------------------
// Mage Tower 1F Quest NPCs
//--- Description -----------------------------------------------------------
// Grita on the way up, the two transport circles and the barrier device, and
// the pair of magicians still holding the floor.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower41QuestNpcsScript : GeneralScript
{
	private readonly static QuestId ToTheTower2 = new QuestId(8472);
	private readonly static QuestId Mq01 = new QuestId(8473);
	private readonly static QuestId Mq02 = new QuestId(8474);
	private readonly static QuestId Mq03 = new QuestId(8475);
	private readonly static QuestId Mq04 = new QuestId(8476);
	private readonly static QuestId Mq05 = new QuestId(8477);
	private readonly static QuestId Sq01 = new QuestId(17002);
	private readonly static QuestId Sq02 = new QuestId(17003);
	private readonly static QuestId Sq03 = new QuestId(17004);
	private readonly static QuestId Sq04 = new QuestId(17005);
	private readonly static QuestId Sq05 = new QuestId(17006);
	private readonly static QuestId Sq06 = new QuestId(8500);

	protected override void Load()
	{
		// Grita, at the tower entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FTOWER41_GRITA_01", "d_firetower_41", -2600, -1571, 90, c => !c.Quests.Has(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(ToTheTower2))
			{
				await dialog.Msg(L("We made it inside. The tower is not what it was when I left it."));
				await dialog.CompleteQuest(ToTheTower2);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("It seems that there are more monsters here now compared to when I moved out of the tower. I don't know what happened..."),
					Option(L("Let's go start a fire"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("Light the signal at the left hallway. Even if one is in danger, if one knows that help is coming, one will have the extra energy to endure longer."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Goddess Gabija is somewhere above us, holding the tower on her own."));
		});

		// Grita, once she is walking the floor with you
		//-------------------------------------------------------------------------
		// The client has her follow the player; the port stands her at the
		// first transport circle instead.
		AddConditionalNpc(147449, L("Grita"), "FTOWER41_G_AI", "d_firetower_41", -1610, -1427, 90, c => c.Quests.Has(Mq01) && !c.Quests.HasCompleted(Mq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Splendid. Since she knows that we have come, she can probably start recovering her health."));
				await dialog.Msg(L("Let's go up and see Gabija."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Darn. They broke it. It is beyond repair."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("That was a trap. It seems they read our every move."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Good! Now we can activate the device."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("That'll do! Let's hope there won't be as many monsters now."));
				await dialog.CompleteQuest(Mq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("The Transport Magic Circle will take you to a floor of your choosing in the tower. I don't know if we can use the magic circle anymore. Let's go and find out."),
					Option(L("Move to the Magic Transport Field"), "accept"),
					Option(L("Let's find a safer way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("I hope the monsters haven't broken the magic circle yet."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Let's give up on this and move to the Large Transport Magic Circle at the center of this floor instead."),
					Option(L("Go to another magic circle"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("This isn't the first time demons have attacked the tower, but it's never lasted for this long."));
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("When you activate the Tower's Barrier Device, monsters won't be able to enter anymore. The magic power supply is currently broken, but we can input power to it through other means."));

				var answer = await dialog.SelectQuestOffer(Mq04, L("Please set this gem and defeat the monsters around it."),
					Option(L("What should I do?"), "accept"),
					Option(L("Let's just get up there quickly"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(ItemId.FTOWER41_MQ_04_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("We can collect their energy to use it to recharge the Barrier Device."));
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("The Defensive Barrier Device is at the left end of this hallway. Let's go upstairs as soon as we activate the device."),
					Option(L("Go activate the Barrier Device"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("It still seems okay. Work it and we can leave this floor behind."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The signal is at the left hallway. Light it and the goddess will know we came."));
				character.Quests.ClearQuestTrack(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I hope the monsters haven't broken the magic circle yet."));
				character.Quests.ClearQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The larger circle is at the center of the floor."));
				character.Quests.ClearQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Set the gem down and fight around it. The energy goes where the gem is."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The Defensive Barrier Device is at the left end of this hallway."));
				return;
			}

			await dialog.Msg(L("The stairs up are to the east, past the barrier device."));
		});

		// The 1st Transport Magic Circle
		//-------------------------------------------------------------------------
		AddNpc(147500, L("1st Transport Magic Circle"), "FTOWER41_MQ_02_NPC", "d_firetower_41", -1609, -1402, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("1st Transport Magic Circle"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				character.ServerMessage(L("Grita bends over the circle. The monsters of the floor are already coming."));
				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("The smaller of the floor's two transport circles, its lines broken through."));
		});

		// The 2nd Transport Magic Circle
		//-------------------------------------------------------------------------
		AddNpc(147500, L("2nd Transport Magic Circle"), "FTOWER41_MQ_03", "d_firetower_41", -582, -1856, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("2nd Transport Magic Circle"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				var worked = await character.TimeActions.StartAsync(L("Working the circle..."), L("Cancel"), "SITABSORB", TimeSpan.FromSeconds(2));

				if (worked != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The circle closes instead of opening, and it takes Grita with it."));
				character.Quests.StartQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("The larger transport circle at the center of the floor. Nothing about it looks safe."));
		});

		// The Barrier Activation Device
		//-------------------------------------------------------------------------
		AddNpc(151000, L("Barrier Activation Device"), "FTOWER41_MQ_05", "d_firetower_41", 2360, -2276, 135, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Barrier Activation Device"));

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				var activated = await character.TimeActions.StartAsync(L("Working the Mage Tower barrier device..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (activated != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq05, "activateBarrier");
				character.ServerMessage(L("The device takes hold and a barrier closes over the floor."));
				return;
			}

			if (!character.Quests.Has(Sq06) && character.Quests.MeetsPrerequisites(Sq06))
			{
				var answer = await dialog.SelectQuestOffer(Sq06, L("The Barrier Activation Device is an important piece of equipment that protects the Mage Tower. It would be worth inspecting carefully."),
					Option(L("Check the device over"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq06);
					character.ServerMessage(L("Something that had been lying against the device slides off it and is gone."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq06))
			{
				await dialog.Msg(L("The Salamander is still somewhere in this hall."));
				character.Quests.ReplayQuestTrack(Sq06);
				return;
			}

			await dialog.Msg(L("The device that closes the Mage Tower to anything that does not belong in it."));
		});

		// Owyn
		//-------------------------------------------------------------------------
		AddNpc(20117, L("Owyn"), "FTOWER41_SQ_01", "d_firetower_41", -1664, -1786, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Owyn"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Oh, you're definitely the Revelator. You were on a roll!"));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Now I feel safe. I will be okay by myself from here. Thanks!"));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Reinforcement? We could really use your strength! Please defeat the monsters nearby."),
					Option(L("I'll help you"), "accept"),
					Option(L("About the current situation of the tower"), "explain"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("All the other magicians have either died or ran away. Goddess Gabija is the only one protecting the tower right now."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("As a follower of the great Flurry, I can't back away just yet!"));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("Can we have another go, with the same momentum? I will take care of the other monsters, you go focus on those troublesome Drakes."),
					Option(L("I'll take care of the Drakes"), "accept"),
					Option(L("Well then, I shall get going"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Those monsters are not as strong when we fight together!"));
				}
				return;
			}

			if (character.Quests.IsActive(Sq01) || character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Keep at them. I will hold this side of the hall."));
				return;
			}

			await dialog.Msg(L("A follower of the great Agailla Flurry does not run from a tower under siege."));
		});

		// Cordelier
		//-------------------------------------------------------------------------
		AddNpc(20146, L("Cordelier"), "FTOWER41_SQ_03", "d_firetower_41", 491, -1696, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cordelier"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thanks! Now, I better get ready to unleash the seal."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Thank you! By the way, what was that vibration?"));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("It was indeed Ginklas. You are so amazing! Because of you, I had enough time to prepare my magic."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("Black Coal Powder. With that, we can unleash the seal that is written on this recipe. But I can't just find it. I am sorry, but could you bring me that powder?"),
					Option(L("I'll go find Black Coal Powder"), "accept"),
					Option(L("About the circumstances now"), "explain"),
					Option(L("I don't want to"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Do you know about the Demon Lord, Helgasercle? I've read that she attacked the Mage Tower several times in the past."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Would it have been better to scrape off the coal powder from Phyracon?"));
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("Oh no... I forgot I need the Eternal Ember. I should have also requested this of you before. Can you please get that for me?"),
					Option(L("I will bring the Eternal Ember"), "accept"),
					Option(L("Go find it yourself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("I should have requested this of you all at once. I'm sorry, I make mistakes all the time."));
				}
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("Could it be Ginklas? I hope it's just a bookshelf falling down in the reading room. Could you check the bookshelf, please?"),
					Option(L("I'll go and check on it"), "accept"),
					Option(L("It will be fine"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("I've never seen a monster who could cause this much vibration."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03) || character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The Phyracons carry the powder, the Drakes the embers. Neither will hand them over."));
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("The reading room is east of here. Please look at the bookshelf."));
				character.Quests.ClearQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("A seal is only as good as the hand that wrote it, and this one was written in a hurry."));
		});

		// The reading room bookshelf
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Bookshelf"), "FTOWER41_SQ_05_NPC", "d_firetower_41", 1199.67, -891.27, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bookshelf"));

			if (character.Quests.IsActive(Sq05) && !character.Quests.IsCompletable(Sq05))
			{
				var checkedShelf = await character.TimeActions.StartAsync(L("Checking the bookshelf..."), L("Cancel"), "READ", TimeSpan.FromSeconds(3));

				if (checkedShelf != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The shelf was never what was shaking the room."));
				character.Quests.StartQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("A reading room shelf, most of its books on the floor around it."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The signal at the end of the left hallway.
		AddQuestTrigger("FTOWER41_MQ_01_NPC", "d_firetower_41", -2176.40, -2123.40, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			{
				character.ServerMessage(L("The signal takes light, and the walls of the hallway break open."));
				character.Quests.StartQuestTrack(Mq01);
			}

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8473: Goddess Gabija (3)
//-----------------------------------------------------------------------------
public class Ftower41Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8473);
		SetName(L("Goddess Gabija (3)"));
		SetDescription(L("Light the signal in the left hallway so the goddess knows that help has reached the tower."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_GRITA_01", "d_firetower_41", L("Talk to Grita at Mage Tower 1F"), L("You finally arrived inside the Mage Tower. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_MQ_01_NPC", "d_firetower_41", L("Light up the signal"), L("Grita told you that Gabija was protecting the tower, but since the monsters are still around, something bad must have happened to the goddess. Light up the signal at the left hallway to let the goddess know that you are here."));
		SetPhase(QuestStatus.Success, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You successfully lit up the signal. Talk to Grita again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER41_MQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8472, QuestStatus.Completed));

		AddObjective("killRubblems", L("Defeat the Rubblems"), new KillObjective(6, "rubblem") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8474: Goddess Gabija (4)
//-----------------------------------------------------------------------------
public class Ftower41Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8474);
		SetName(L("Goddess Gabija (4)"));
		SetDescription(L("The fastest way up is the Transport Magic Circle, if it still works."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You lit up a signal to let the goddess know that help is coming. Discuss what to do next with Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_MQ_02_NPC", "d_firetower_41", L("Check the 1st Transport Magic Circle"), L("Grita told you that using the Transport Magic Circle is the fastest way to go upstairs. Go to the 1st Transport Magic Circle to see whether it is working correctly."));
		SetPhase(QuestStatus.Success, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You defeated the monsters that were rushing in and arrived at the place where the 1st Transport Magic Circle is located. Talk to Grita."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER41_MQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8473, QuestStatus.Completed));

		AddObjective("killPhyracons", L("Defeat interfering Phyracons"), new KillObjective(6, "flight_hope") { LayerOnly = true });
		AddObjective("killDrakes", L("Defeat interfering Drakes"), new KillObjective(8, "Fire_Dragon") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8475: Goddess Gabija (5)
//-----------------------------------------------------------------------------
public class Ftower41Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8475);
		SetName(L("Goddess Gabija (5)"));
		SetDescription(L("The larger transport circle at the center of the floor was left as a trap."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("Grita's face turned grim as she was checking the Transport Magic Circle. Talk to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_MQ_03", "d_firetower_41", L("Activate the 2nd Transport Magic Circle"), L("Grita told you that the 1st Transport Magic Circle is badly broken and recommended to use the other, larger Transport Magic Circle. Look for the 2nd Transport Magic Circle and activate it."));
		SetPhase(QuestStatus.Success, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You thought you could use the 2nd Transport Magic Circle, but there was a trap set by the demons. Talk to Grita again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER41_MQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8474, QuestStatus.Completed));

		AddObjective("breakTheCircle", L("Destroy the 2nd Transport Magic Circle restraining Grita"), new KillObjective(1, "whorfzone") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8476: Goddess Gabija (6)
//-----------------------------------------------------------------------------
public class Ftower41Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8476);
		SetName(L("Goddess Gabija (6)"));
		SetDescription(L("The tower's barrier device needs power, and the gem Grita hands over takes it from what dies near it."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("Grita told you that with the present danger, the plan to directly reach Goddess Gabija via the Transport Magic Circles should be abandoned. Talk to Grita for more details."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_G_AI", "d_firetower_41", L("Charge the Absorbing Gem"), L("Grita said in order to activate the Tower's Barrier Device again, it would need a great amount of energy. Set the gem Grita gave you on the ground and recharge the gem with the monsters' life force."));
		SetPhase(QuestStatus.Success, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You were able to charge the Absorbing Gem with monsters' life force. Return to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8475, QuestStatus.Completed));

		// The client leaves this phase to a minigame and records no objective;
		// the gem is charged by what dies around it instead.
		AddObjective("chargeTheGem", L("Charge the Absorbing Gem"), new KillObjective(10, "rubblem", "flight_hope", "Fire_Dragon"));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER41_MQ_04_ITEM"));
	}
}

// 8477: Agailla Flurry's Barrier
//-----------------------------------------------------------------------------
public class Ftower41Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8477);
		SetName(L("Agailla Flurry's Barrier"));
		SetDescription(L("The barrier device at the end of the left hallway will close the floor once it runs again."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You recharged the gem with the monsters' life force. Talk to Grita about how to activate the Barrier Device."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_MQ_05", "d_firetower_41", L("Activate the Barrier Device"), L("The Barrier Device is located at the end of this left hallway. Activate the Barrier Device."));
		SetPhase(QuestStatus.Success, "FTOWER41_G_AI", "d_firetower_41", L("Talk to Grita"), L("You successfully activated the Barrier Device. Talk to Grita again."));

		AddPrerequisite(new QuestStatusPrerequisite(8476, QuestStatus.Completed));

		AddObjective("activateBarrier", L("Activate the Barrier Device"), new ManualObjective());

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8500: Lizard of Fire
//-----------------------------------------------------------------------------
public class Ftower41Sq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8500);
		SetName(L("Lizard of Fire"));
		SetDescription(L("Something was lying against the barrier device, and it did not wait to be looked at."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_MQ_05", "d_firetower_41", L("Check the Barrier Activation Device"), L("The Barrier Activation Device is an important piece of equipment that protects the Mage Tower. Carefully inspect it."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_MQ_05", "d_firetower_41", L("Defeat Salamander"), L("The Salamander suddenly disappeared as you tried to inspect the Barrier Activation Device."));
		SetPhase(QuestStatus.Success, "FTOWER41_MQ_05", "d_firetower_41", L("Defeat Salamander"), L("The Salamander suddenly disappeared as you tried to inspect the Barrier Activation Device."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER41_SQ_06_TRACK", "m_boss_a");

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("killSalamander", L("Defeat Salamander"), new KillObjective(1, "boss_salamander_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Salamander is down and the barrier device is intact."));
		character.Quests.Complete(this.QuestId);
	}
}

// 17002: Young Magician Owyn (1)
//-----------------------------------------------------------------------------
public class Ftower41Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17002);
		SetName(L("Young Magician Owyn (1)"));
		SetDescription(L("A young magician is holding a stretch of the first floor by himself."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_SQ_01", "d_firetower_41", L("Talk to Young Magician Owyn"), L("There is a young magician standing at the marked area of the map. He seems to need some help."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_SQ_01", "d_firetower_41", L("Defeat the Phyracon occupying Mage Tower"), L("Owyn is trying to save the Mage Tower from the demons. Defeat the monsters nearby to help him."));
		SetPhase(QuestStatus.Success, "FTOWER41_SQ_01", "d_firetower_41", L("Report to Owyn"), L("Done what Owyn requested. Return to him."));

		AddPrerequisite(new LevelPrerequisite(103));

		AddObjective("killPhyracons", L("Defeat the Phyracon occupying Mage Tower 1st Floor"), new KillObjective(10, "flight_hope"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17003: Young Magician Owyn (2)
//-----------------------------------------------------------------------------
public class Ftower41Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17003);
		SetName(L("Young Magician Owyn (2)"));
		SetDescription(L("Owyn will take the rest of the floor if the Drakes are taken off him."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_SQ_01", "d_firetower_41", L("Owyn's Request"), L("Owyn looks like he has another request. Talk to him."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_SQ_01", "d_firetower_41", L("Defeat Drakes"), L("Owyn is having a hard time getting rid of Drakes. Defeat the Drakes nearby for him."));
		SetPhase(QuestStatus.Success, "FTOWER41_SQ_01", "d_firetower_41", L("Report to Owyn"), L("You completed all of Owyn's requests. Return to him."));

		AddPrerequisite(new QuestStatusPrerequisite(17002, QuestStatus.Completed));

		AddObjective("killDrakes", L("Defeat Drakes"), new KillObjective(10, "Fire_Dragon"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17004: Evidence of Bedazzlement (1)
//-----------------------------------------------------------------------------
public class Ftower41Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17004);
		SetName(L("Evidence of Bedazzlement (1)"));
		SetDescription(L("Cordelier needs Black Coal Powder before she can read the seal in front of her."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_SQ_03", "d_firetower_41", L("Talk to Cordelier"), L("Cordelier is busy in the middle of the Mage Tower. Talk to her."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_SQ_03", "d_firetower_41", L("Defeat Phyracon and collect Black Coal Powder"), L("Cordelier wants Black Coal Powder. Get it from Phyracon for Cordelier."));
		SetPhase(QuestStatus.Success, "FTOWER41_SQ_03", "d_firetower_41", L("Give Black Coal Powder to Cordelier"), L("Gathered enough Black Coal Powder. Talk to Cordelier again."));

		AddPrerequisite(new LevelPrerequisite(103));

		AddPityDrop("FTOWER41_SQ_03_01", 0.5f, 4, 1, "flight_hope");

		AddObjective("collectPowder", L("Defeat Phyracon and collect Black Coal Powder"), new CollectItemObjective("FTOWER41_SQ_03_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER41_SQ_03_01"));
	}
}

// 17005: Evidence of Bedazzlement (2)
//-----------------------------------------------------------------------------
public class Ftower41Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17005);
		SetName(L("Evidence of Bedazzlement (2)"));
		SetDescription(L("The seal also wants Eternal Embers, and the Drakes carry them."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_SQ_03", "d_firetower_41", L("Evidence of Bedazzlement"), L("Cordelier is looking at the manual. Talk to her again."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_SQ_03", "d_firetower_41", L("Collect Eternal Embers from Drakes"), L("Cordelier asked you to get eternal embers from Drakes."));
		SetPhase(QuestStatus.Success, "FTOWER41_SQ_03", "d_firetower_41", L("Give the Eternal Ember to Cordelier"), L("Collected the Eternal Embers. Give it to Cordelier."));

		AddPrerequisite(new QuestStatusPrerequisite(17004, QuestStatus.Completed));

		AddPityDrop("FTOWER41_SQ_04_01", 0.5f, 4, 1, "Fire_Dragon");

		AddObjective("collectEmbers", L("Collect Eternal Embers from Drakes"), new CollectItemObjective("FTOWER41_SQ_04_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER41_SQ_04_01"));
	}
}

// 17006: Evidence of Bedazzlement (3)
//-----------------------------------------------------------------------------
public class Ftower41Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17006);
		SetName(L("Evidence of Bedazzlement (3)"));
		SetDescription(L("Something heavy moved in the reading room while Cordelier was working."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_41");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER41_SQ_03", "d_firetower_41", L("Talk to Cordelier"), L("Ask Cordelier about the unpleasant vibration that you felt a while ago."));
		SetPhase(QuestStatus.InProgress, "FTOWER41_SQ_05_NPC", "d_firetower_41", L("Check the bookshelf in the Reading Room"), L("Check the bookshelf in the Reading Room to find out about the vibration felt in the Reading Room."));
		SetPhase(QuestStatus.Success, "FTOWER41_SQ_03", "d_firetower_41", L("Report to Cordelier"), L("Defeated the Ginklas that suddenly appeared. Report to Cordelier."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER41_SQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17005, QuestStatus.Completed));

		AddObjective("killGinklas", L("Defeat Ginklas"), new KillObjective(1, "boss_ginklas_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
		AddReward(new ItemReward("R_Hat_628063", 1));
	}
}
