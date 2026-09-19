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
	private readonly static QuestId Slate2 = new QuestId(20051);
	private readonly static QuestId Slate3 = new QuestId(20052);

	protected override void Load()
	{
		// Knight Commander Uska
		//-------------------------------------------------------------------------
		AddNpc(20113, L("[Templar Master]{nl}Knight Commander Uska"), "KLAPEDA_USKA", "c_Klaipe", -425, 172, 30, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Knight Commander Uska"));
			dialog.SetPortrait("Dlg_port_KNIGHT_USKA");

			if (character.Quests.IsActive(Slate2) && character.Quests.IsCompletable(Slate2))
			{
				await dialog.Msg(L("So you finally returned. What did the Bokor Master tell you?"));
				await dialog.Msg(L("What you've said is hard to believe. This revelation is from Goddess Laima and only when we collect all of them, she will return. Is that what you mean? Goddess Laima is also referred as the 'goddess in a book'. There is no other story mentioning her name other than the record that states that she picked the spot for the kingdom to be built to the Great King Zachariel. I'd like to ask you a favor. I have sinned because I was not able to fulfill my duties of guarding the kingdom on Medzio Diena. But I am not a Revelator. Even if I want to make up for my sins, my duty at the moment is to protect the many citizens of Klaipeda. So please find all the revelations. I will put my position as the Knight Commander of Klaipeda on the line and help you deal with it."));
				character.Quests.Complete(Slate2);
				return;
			}

			if (!character.Quests.Has(Slate3) && character.Quests.MeetsPrerequisites(Slate3))
			{
				await dialog.Msg(L("If the Bokor Master told you that I would know it... There's one place that comes to mind. The high gardens mentioned by the goddess may be a place in Gele Plateau. I heard the Paladin Master is there, upholding a long term promise."));

				var answer = await dialog.Select(L("Will you go and meet the Paladin Master?"),
					Option(L("I'll go and meet the Paladin Master"), "accept"),
					Option(L("Cancel"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Slate3);
					character.Quests.CompleteObjective(Slate3, "askMayor");
					await dialog.Msg(L("Ask the mayor of the Miners' Village for directions to Gele Plateau. I will send the message to the Paladin Master first. May the blessings of the goddess be with you."));
					await dialog.Msg(L("Oh, please do not reveal the fact that a revelation has been found unless it is absolutely necessary. Doing so may only create confusion if the rumors spread."));
				}
				return;
			}

			if (!character.Quests.Has(Slate2) && character.Quests.MeetsPrerequisites(Slate2))
			{
				await dialog.Msg(L("This stone slate is the Light of Salvation? It looks like just an old slate to me. It would be good to ask the Bokor Master to interpret this slate. She lives at the end of Klaipeda's Residential Area so go pay her a visit."));

				var answer = await dialog.Select(L("Will you have the slate interpreted?"),
					Option(L("I'll go visit the Bokor Master"), "accept"),
					Option(L("I'll think about it little more"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Slate2);

				return;
			}

			if (character.Quests.IsActive(Slate3))
			{
				await dialog.Msg(L("Ask the mayor of the Miners' Village for directions to Gele Plateau."));
				return;
			}

			if (character.Quests.IsActive(Slate2))
			{
				await dialog.Msg(L("The Bokor Master lives at the end of Klaipeda's Residential Area. Ask her to interpret the slate."));
				return;
			}

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
					character.Quests.CompleteObjective(GoToEast, "hearDream");
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

			await dialog.Msg(L("There is nothing more I can tell you for now. Go, and may the goddess watch over you."));
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
			await dialog.Msg(L("Klaipeda is full of hope and expectation for the Revelators who dreamed of the goddess. I am among those hoping, too."));
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
				character.Quests.CompleteObjective(EastPrepare1, "visitRonesa");
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

// 20051: Mysterious Slate (2)
//-----------------------------------------------------------------------------
public class Cmine6ToKatyn72Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20051);
		SetName(L("Mysterious Slate (2)"));
		SetDescription(L("Knight Commander Uska wants the slate from the Crystal Mine interpreted by the Bokor Master."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"), L("You followed the revelation of the goddess that the bishop saw in this dreams and obtained the Mysterious Slate. Talk to Knight Commander Uska about the Mysterious Slate."));
		SetPhase(QuestStatus.InProgress, "MASTER_BOCORS", "c_voodoo", L("Ask the Bokor Master for interpretation of the revelation"), L("Ask the Bokor Master about the Mysterious Slate. She lives at the residential area in Klaipeda's left end."));
		SetPhase(QuestStatus.Success, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"), L("The slate found in the Crystal Mines is both the revelation and Goddess Laima herself. It contains a message about finding the next revelation in a holy church. Return to Sir Uska as the Bokor Master says."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CMINE6_TO_KATYN7_2_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(20050, QuestStatus.Completed));

		AddObjective("askBokor", L("Ask the Bokor Master for interpretation of the revelation"), new ManualObjective());
	}
}

// 20052: Mysterious Slate (3)
//-----------------------------------------------------------------------------
public class Cmine6ToKatyn73Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20052);
		SetName(L("Mysterious Slate (3)"));
		SetDescription(L("The revelation points to Gele Plateau. The Miners' Village Mayor knows the way there."));
		SetType(QuestType.Main);
		SetLocation("c_Klaipe");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KLAPEDA_USKA", "c_Klaipe", L("Talk to Knight Commander Uska"), L("The details of slate found in Crystal Mines mentions revelation of the goddess about blocking the threats to Gele Plateau. Report about it to Commander Uska."));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"), L("Knight Commander Uska thinks the high gardens in the revelation refers to an area in Gele Plateau and told you to go to the Paladin Master. Drop by the Mayor of the Miners' Village and ask for directions to Gele Plateau."));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"), L("Knight Commander Uska thinks the high gardens in the revelation refers to an area in Gele Plateau and told you to go to the Paladin Master. Drop by the Mayor of the Miners' Village and ask for directions to Gele Plateau."));

		AddPrerequisite(new QuestStatusPrerequisite(20051, QuestStatus.Completed));

		AddObjective("askMayor", L("Talk to the Miners' Village Mayor"), new ManualObjective());
	}
}
