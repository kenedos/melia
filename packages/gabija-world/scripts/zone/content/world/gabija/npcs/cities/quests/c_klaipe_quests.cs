//--- Melia Script ----------------------------------------------------------
// Klaipeda Quest NPCs
//--- Description -----------------------------------------------------------
// The quest givers and merchants the city's field quests run on.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Hooking;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class KlaipeQuestNpcsScript : GeneralScript
{
	private readonly static QuestId GoToEast = new QuestId(1027);
	private readonly static QuestId EastPrepare = new QuestId(20236);
	private readonly static QuestId EastPrepare1 = new QuestId(40010);

	protected override void Load()
	{
		// Knight Commander Uska
		//-------------------------------------------------------------------------
		AddNpc(20113, L("Knight Commander Uska"), "KLAPEDA_USKA", "c_Klaipe", -425, 172, 30, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Knight Commander Uska"));

			if (!character.Quests.Has(GoToEast) && character.Quests.MeetsPrerequisites(GoToEast))
			{
				await dialog.Msg(L("Are you the Revelator who dreamed of the goddess? I am Uska, knight commander of Shaulley and Klaipeda."));
				await dialog.Msg(L("The kingdom has lost all hope. Its will to rise again is broken, and all that is left is to cry the goddess's name."));
				await dialog.Msg(L("Honestly, I cannot look on you Revelators as pure hope. I carry a city on my shoulders - I cannot bear to fall into despair again."));

				var answer = await dialog.Select(L("Well... in any case, the bishop also had the dream of revelation. He said to send the Revelator to the crystal mine when they arrive."),
					Option(L("Say you came because of the dream"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(GoToEast);
					await dialog.Msg(L("When you are ready to leave for the crystal mine, speak to me again. I will tell you where it is and how to enter."));
				}
				return;
			}

			if (character.Quests.IsActive(GoToEast))
			{
				await dialog.Msg(L("The bishop said to send the Revelators to the crystal mine. The goddess told him so in a dream."));
				await dialog.Msg(L("But the mining village is in open conflict with the Bube, so entry is restricted. Knight Ares in the eastern woods is in charge - go and meet him."));
				character.Quests.Complete(GoToEast);
				return;
			}

			if (!character.Quests.Has(EastPrepare) && character.Quests.MeetsPrerequisites(EastPrepare))
			{
				await dialog.Msg(L("And yet, all over the kingdom, people have appeared who say they dreamed of the goddess. That is you - the Revelators."));
				await dialog.Msg(L("The bishop too had the dream of revelation during his closed prayer. He says the goddess told him there would be a light of salvation in the crystal mine."));
				await dialog.Msg(L("Knight Ares, stationed in the eastern woods, is in charge. Go and meet him."));

				var answer = await dialog.Select(L("Why the goddess sent the Revelators only now, four years on... it is not for the likes of me to know her will. But I know you are the only hope we have left to hold on to."),
					Option(L("Say you will go to the eastern woods"), "accept"),
					Option(L("Say you will not go"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(EastPrepare);
					await dialog.Msg(L("Well then. Set out for the eastern woods. Do not forget to pray at the Statue of Goddess Ausrine in the central plaza."));
					await dialog.Msg(L("And be sure to call on the general merchant - I asked her to hold warp scrolls for the Revelators."));
				}
				return;
			}

			if (character.Quests.IsActive(EastPrepare))
			{
				if (!character.Quests.IsCompletable(EastPrepare))
				{
					await dialog.Msg(L("The bishop remains in closed prayer. Before you leave, pray at the Statue of Goddess Ausrine in the central plaza."));
					return;
				}

				await dialog.Msg(L("The general merchant has your warp scrolls. She is just down the plaza - do not forget to call on her."));
				return;
			}

			await dialog.Msg(L("The bishop remains in closed prayer. When you are ready, go at once to meet Ares in the eastern woods."));
		});

		// Quest dialog hooks for the city's merchants
		//-------------------------------------------------------------------------
		ScriptHooks.Register(new DialogHook("Mirina", "BeforeDialog", MirinaDialog));
		ScriptHooks.Register(new DialogHook("Ronesa", "BeforeDialog", RonesaDialog));

		// Prayer trigger at the Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddQuestTrigger("KLAIPE_AUSRINE_PRAYER", "c_Klaipe", -206.574, 98.63973, 80, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(EastPrepare) && !character.Quests.IsCompletable(EastPrepare))
				character.Quests.CompleteObjective(EastPrepare, "pray");

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Mirina's quest dialog, run before her shop dialog.
	/// </summary>
	private static async Task<HookResult> MirinaDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Quests.IsCompletable(EastPrepare))
		{
			await dialog.Msg(L("Welcome! Ah, you must be a Revelator. I have been so hoping to meet you."));
			await dialog.Msg(L("Klaipeda is full of expectation for the Revelators who dreamed of the goddess. I am one of them, too."));
			await dialog.Msg(L("These are warp scrolls. Use one and you can travel to any goddess statue you like, or return to where you were."));
			await dialog.Msg(L("I will give you more than the knights asked for. With so many Revelators about, surely things will get better than they are now?"));
			character.Quests.Complete(EastPrepare);
			return HookResult.Break;
		}

		if (character.Quests.IsActive(EastPrepare))
		{
			await dialog.Msg(L("Pray at the Statue of Goddess Ausrine before you go, and come back to me for your warp scrolls."));
			return HookResult.Break;
		}

		if (!character.Quests.Has(EastPrepare1) && character.Quests.MeetsPrerequisites(EastPrepare1))
		{
			await dialog.Msg(L("Oh, that's right - Ronesa at the accessory shop said she has a gift she simply must give the Revelators."));

			var answer = await dialog.Select(L("Would you like to drop by?"),
				Option(L("Say you will drop by"), "accept"),
				Option(L("Refuse"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(EastPrepare1);
				await dialog.Msg(L("Go straight to the left from here and you'll find Ronesa."));
			}
			return HookResult.Break;
		}

		if (character.Quests.IsActive(EastPrepare1))
		{
			await dialog.Msg(L("Go straight to the left from here and you'll find Ronesa."));
			return HookResult.Break;
		}

		return HookResult.Skip;
	}

	/// <summary>
	/// Ronesa's quest dialog, run before her shop dialog.
	/// </summary>
	private static async Task<HookResult> RonesaDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Quests.IsCompletable(EastPrepare1))
		{
			await dialog.Msg(L("Welcome! You're the Revelator who dreamed of the goddess, aren't you? All Klaipeda talks about is you."));
			await dialog.Msg(L("I wonder if you really saw the vanished goddess in your dream... and what that dream was like. I'm curious, but I suppose you can't tell me."));
			await dialog.Msg(L("That's all right. I believe the Revelators are the ones who have come to find the goddess."));
			await dialog.Msg(L("Please, take this accessory. If armor guards against physical attacks, an accessory can protect you from magic."));
			await dialog.Msg(L("I do hope it serves you well. May the goddess's blessing go with you..."));
			character.Quests.Complete(EastPrepare1);
			return HookResult.Break;
		}

		return HookResult.Skip;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 1027: The Bishop's Dream
//-----------------------------------------------------------------------------
public class KlapedaGoToEastQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1027);
		SetName(L("The Bishop's Dream"));
		SetDescription(L("Knight Commander Uska, who dreamed of the goddess, is looking for the people who came to Klaipeda."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));
		SetPhase(QuestStatus.InProgress, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));
		SetPhase(QuestStatus.Success, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"));

		AddObjective("hearDream", L("Talk to Knight Commander Uska"), new ManualObjective());

		AddReward(new ItemReward("Scroll_Warp_Klaipe", 5));
	}
}

// 20236: The Goddess's Dream (1)
//-----------------------------------------------------------------------------
public class EastPrepareQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20236);
		SetName(L("The Goddess's Dream (1)"));
		SetDescription(L("Knight Commander Uska asks the Revelators to prepare for the journey to the eastern woods."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Move to Klaipeda and talk to Knight Commander Uska"));
		SetPhase(QuestStatus.InProgress, "KLAIPE_AUSRINE_PRAYER", "c_Klaipe", L("Pray at the Statue of Goddess Ausrine"));
		SetPhase(QuestStatus.Success, "Mirina", "c_Klaipe", L("Talk to the General Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(1015, QuestStatus.Completed));

		AddObjective("pray", L("Pray at the Statue of Goddess Ausrine"), new ManualObjective());

		AddReward(new ItemReward("Scroll_Warp_quest", 10));
	}
}

// 40010: The Goddess's Dream (2)
//-----------------------------------------------------------------------------
public class EastPrepare1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40010);
		SetName(L("The Goddess's Dream (2)"));
		SetDescription(L("The accessory merchant has been handing out gifts to the Revelators."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "Mirina", "c_Klaipe", L("Talk to the General Merchant"));
		SetPhase(QuestStatus.InProgress, "Mirina", "c_Klaipe", L("Talk to the General Merchant"));
		SetPhase(QuestStatus.Success, "Ronesa", "c_Klaipe", L("Talk to the Accessory Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(20236, QuestStatus.Completed));

		AddObjective("visitRonesa", L("Talk to the Accessory Merchant"), new ManualObjective());

		AddReward(new ItemReward("BRC01_122", 1));
	}
}
