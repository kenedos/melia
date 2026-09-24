//--- Melia Script ----------------------------------------------------------
// Fedimian Quest NPCs
//--- Description -----------------------------------------------------------
// The magician who comes looking for a Revelator, and the road out of the
// city once the Mage Tower has given up its revelation.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CFedimianQuestNpcsScript : GeneralScript
{
	private readonly static QuestId ToTheTower1 = new QuestId(8471);
	private readonly static QuestId ToPilgrimsWay = new QuestId(8512);
	private readonly static QuestId SeirTempleRebuilding2 = new QuestId(80049);

	protected override void Load()
	{
		// Grita
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FEDIMIAN_GRITA", "c_fedimian", 127, 517, 33, c => !c.Quests.HasCompleted(ToTheTower1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (!character.Quests.Has(ToTheTower1) && character.Quests.MeetsPrerequisites(ToTheTower1))
			{
				var answer = await dialog.SelectQuestOffer(ToTheTower1, L("Finally... I am so relieved that I finally managed to find you! Please help. Goddess Gabija is in desperate need of your help."),
					Option(L("Let's go help Grita at Fedimian Suburbs"), "accept"),
					Option(L("About the Mage Tower"), "explain"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("In the past, the demons have aimed to take the Mage Tower. Though, we were always able to stop their attacks whenever they attacked."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(ToTheTower1);
					await dialog.Msg(L("I will go ahead to the Karsta Hall Site in the Suburbs. Follow the east road out of the city and you will find me."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(ToTheTower1))
			{
				await dialog.Msg(L("I will be waiting at the Karsta Hall Site in the Fedimian Suburbs."));
				return;
			}

			await dialog.Msg(L("The Mage Tower stands past the Suburbs. Goddess Gabija has held it alone for far too long."));
		});

		// The Fedimian masters
		//-------------------------------------------------------------------------
		AddNpc(57227, L("[Druid Master]{nl}Gina Greene"), "JOB_DRUID3_1_NPC", "c_fedimian", 432.94, 808.29, 20, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Druid Master"));
			dialog.SetPortrait("Dlg_port_druid");

			if (character.Quests.IsActive(SeirTempleRebuilding2))
			{
				if (character.Inventory.CountItem(ItemId.ORCHARD_324_SQ_SCROLL) > 0)
				{
					await dialog.Msg(L("A human sided with the demons... With the world as it is, who would have thought...?"));
					return;
				}

				var explained = await character.TimeActions.StartAsync(L("Explaining the situation"), L("Cancel"), "TALK", TimeSpan.FromSeconds(1));
				if (explained != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("So the land was contaminated not by demons, but a human... With the world as it is, who would have thought...?"));
				await dialog.Msg(L("I have a solution. I'm going to write you a purification scroll. That should be enough to solve the problem."));

				character.Inventory.Add(ItemId.ORCHARD_324_SQ_SCROLL, 1, InventoryAddType.PickUp);
				return;
			}

			switch (GameRandom.Get().Next(3))
			{
				case 0:
					await dialog.Msg(L("Medzio Diena was not nature's will. After all, nature is governed by the goddesses, and they would never do harm to us all."));
					break;
				case 1:
					await dialog.Msg(L("If the Mage Tower collapses, Fedimian won't be safe. The Demon Lord Helgasercle... the end of the obsession ends in such vain."));
					break;
				default:
					await dialog.Msg(L("Ever since the Revelators who dreamt of the goddesses appeared, the hands of the demons released them one by one."));
					await dialog.Msg(L("The same goes for the Great Cathedral. The proof that the Goddesses didn't abandon us is them."));
					break;
			}
		});

		AddNpc(147439, L("[Squire Master]{nl}Justina Legwyn"), "JOB_SQUIRE3_1_NPC", "c_fedimian", 694, 114, 90, async dialog =>
		{
			dialog.SetTitle(L("Squire Master"));
			dialog.SetPortrait("Dlg_port_JustinaLegwyn");

			if (dialog.Player.Quests.IsActive(SeirTempleRebuilding2))
			{
				await dialog.Msg(L("A method to purify the earth... I believe that the clerics might know more about this topic."));
				return;
			}

			switch (GameRandom.Get().Next(3))
			{
				case 0:
					await dialog.Msg(L("If you ask me what the strongest weapon is, my answer would be my faith. Faith to an ally that you can truly rely on."));
					break;
				case 1:
					await dialog.Msg(L("We are in chaos nowadays, but we used to live in honorable times before. Well... that's all in the past now."));
					break;
				default:
					await dialog.Msg(L("I heard not that long ago that thanks to the Revelators the demons failed to interfere with the Mage Tower."));
					await dialog.Msg(L("If it were me I would have made sure no one would have even dared to think about it in the first place. So that what happened to my family won't happen ever again."));
					break;
			}
		});

		AddNpc(57238, L("[Rogue Master]{nl}Gema"), "MASTER_ROGUE", "c_fedimian", 169, 162.18, 90, async dialog =>
		{
			dialog.SetTitle(L("Rogue Master"));
			dialog.SetPortrait("Dlg_port_Gema");

			if (dialog.Player.Quests.IsActive(SeirTempleRebuilding2))
			{
				await dialog.Msg(L("I know a method to purify the demons. You need to hold your breath, so that no one will sense you. Then, quietly get closer to the lights..."));
				await dialog.Msg(L("Darn. A method to purify the earth, but not the demons? You should've found Druid Master if that's the case. The order is wrong."));
				return;
			}

			switch (GameRandom.Get().Next(3))
			{
				case 0:
					await dialog.Msg(L("If I'm paid well, there's no reason I would stop a person who wants to learn my techniques. Rather, I would even welcome that person."));
					await dialog.Msg(L("That doesn't mean they should follow my every move, though."));
					break;
				case 1:
					await dialog.Msg(L("I have heard about the Revelator who drove out the demons from the Mage Tower. The Masters didn't do anything. Of course, I didn't do anything neither."));
					break;
				default:
					await dialog.Msg(L("Everyone is just busy trying to survive after Medzio Diena, so they don't have time to think about going to the great cathedral."));
					await dialog.Msg(L("But everyone relies on the Great Cathedral. The Revelator completed a huge task."));
					break;
			}
		});

		AddNpc(147443, L("[Doppelsoeldner Master]{nl}Guerra"), "MASTER_DOPPELSOELDNER", "c_fedimian", 220.70, -81.84, 0, async dialog =>
		{
			dialog.SetTitle(L("Doppelsoeldner Master"));
			dialog.SetPortrait("Dlg_port_Guerra");

			if (dialog.Player.Quests.IsActive(SeirTempleRebuilding2))
			{
				await dialog.Msg(L("You need my power? The price is bit steep. Is it a job worth that much?"));
				await dialog.Msg(L("Hold on... A method to purify the earth? Hey... I'm just a mercenary. Go find the answers somewhere else."));
				return;
			}

			await dialog.Msg(L("Worthwhile things always carry a degree of risk. Naturally, for big rewards, you'll have to put up with lots of danger."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The far side of the Fedimian gate onto Starving Demon's Way.
		AddQuestTrigger("FTOWER45_MQ_NEXT_ARRIVE", "f_pilgrimroad_46", -1980, -2354, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(ToPilgrimsWay) && !character.Quests.IsCompletable(ToPilgrimsWay))
			{
				character.Quests.CompleteObjective(ToPilgrimsWay, "reachPilgrimRoad");
				character.ServerMessage(L("You have reached the entrance to Kathin Forest, the place Goddess Gabija named."));
			}

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8471: Goddess Gabija (1)
//-----------------------------------------------------------------------------
public class ToTheTower01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8471);
		SetName(L("Goddess Gabija (1)"));
		SetDescription(L("A magician has come to Fedimian looking for a Revelator, and she will not wait long."));
		SetType(QuestType.Main);
		SetLocation("c_fedimian", "f_remains_40");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FEDIMIAN_GRITA", "c_fedimian", L("Someone is looking for you"), L("There is a person who is looking for you in Fedimian. Go to the gigantic Goddess Statue in Fedimian."));
		SetPhase(QuestStatus.InProgress, "REMAINS40_GRITA", "f_remains_40", L("Talk to Grita at the Karsta Hall Site"), L("Grita has gone ahead to the Karsta Hall Site in the Fedimian Suburbs. Follow her there."));
		SetPhase(QuestStatus.Success, "REMAINS40_GRITA", "f_remains_40", L("Talk to Grita at the Karsta Hall Site"), L("Grita has gone ahead to the Karsta Hall Site in the Fedimian Suburbs. Follow her there."));

		// The chain link from the Royal Mausoleum; the client gates this on
		// the level band alone.
		AddPrerequisite(new LevelPrerequisite(103));
		AddPrerequisite(new QuestStatusPrerequisite(50007, QuestStatus.Completed));

		AddObjective("meetGrita", L("Talk to Grita at the Karsta Hall Site"), new ManualObjective());
	}
}

// 8512: To Pilgrim's Way
//-----------------------------------------------------------------------------
public class Ftower45MqNextQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8512);
		SetName(L("To Pilgrim's Way"));
		SetDescription(L("The Revelation of the Mage Tower names the Great Cathedral, and Pilgrim's Way is the road to it."));
		SetType(QuestType.Sub);
		SetLocation("c_fedimian", "f_pilgrimroad_46");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "FEDMIAN_PILGRIM46", "c_fedimian", L("Move to Starving Demon's Way"), L("To go to the Great Cathedral mentioned in the Mage Tower Revelation, you must pass through Pilgrim's Way, which connects to Fedimian. Move to Starving Demon's Way."));
		SetPhase(QuestStatus.InProgress, "FEDMIAN_PILGRIM46", "c_fedimian", L("Move to Starving Demon's Way"), L("To go to the Great Cathedral mentioned in the Mage Tower Revelation, you must pass through Pilgrim's Way, which connects to Fedimian. Move to Starving Demon's Way."));
		SetPhase(QuestStatus.Success, "FEDMIAN_PILGRIM46", "c_fedimian", L("Move to Starving Demon's Way"), L("To go to the Great Cathedral mentioned in the Mage Tower Revelation, you must pass through Pilgrim's Way, which connects to Fedimian. Move to Starving Demon's Way."));

		AddPrerequisite(new QuestStatusPrerequisite(8498, QuestStatus.Completed));

		AddObjective("reachPilgrimRoad", L("Move to Starving Demon's Way"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The arrival is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}
