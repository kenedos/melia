//--- Melia Script ----------------------------------------------------------
// Fedimian Quest NPCs
//--- Description -----------------------------------------------------------
// The magician who comes looking for a Revelator, and the road out of the
// city once the Mage Tower has given up its revelation.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CFedimianQuestNpcsScript : GeneralScript
{
	private readonly static QuestId ToTheTower1 = new QuestId(8471);
	private readonly static QuestId ToPilgrimsWay = new QuestId(8512);

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
