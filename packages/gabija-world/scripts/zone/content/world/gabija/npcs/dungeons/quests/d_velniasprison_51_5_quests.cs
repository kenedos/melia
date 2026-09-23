//--- Melia Script ----------------------------------------------------------
// Demon Prison District 5 Quest NPCs
//--- Description -----------------------------------------------------------
// The Vakarion Cathedral ritual that closes the dimensional crack, and the
// Evening Star Rune the demons are fed into it with.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison515QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(60023);
	private readonly static QuestId Mq02 = new QuestId(60024);
	private readonly static QuestId Mq03 = new QuestId(60025);
	private readonly static QuestId Mq04 = new QuestId(60026);
	private readonly static QuestId Mq05 = new QuestId(60027);
	private readonly static QuestId Mq06 = new QuestId(60028);
	private readonly static QuestId Mq07 = new QuestId(60042);
	private readonly static QuestId Sq01 = new QuestId(60039);
	private readonly static QuestId Sq02 = new QuestId(60040);
	private readonly static QuestId Sq03 = new QuestId(60041);

	private const int CracksToClose = 3;

	// The small dimensional cracks of the Gavara Isolation District.
	private readonly static double[,] CrackSpots =
	{
		{ 1361.89, -24.64 }, { 1290.05, -314.69 }, { 1284.42, -139.14 }, { 1222.70, 96.90 },
		{ 1325.37, 212.34 }, { 1566.15, 205.57 }, { 1621.37, 17.23 }, { 1516.29, -58.71 },
		{ 1463.33, 84.52 }, { 1334.13, 380.92 },
	};

	protected override void Load()
	{
		// Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(154010, L("Goddess Vakarine"), "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", -109.89, -181.62, 168, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Goddess Vakarine"));
			dialog.SetPortrait("Dlg_port_vakarine2");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The waves of the dimensional crack have become stronger than ever."));
				await dialog.Msg(L("We can no longer fill it with Hauberk's soul alone."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Thanks to you, I can now complete the Evening Star Rune."));
				await dialog.Msg(L("Take it. You should collect the souls of the demons with the Evening Star Rune."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The Kupoles have taken the demons you suppressed at Lankine."));
				await dialog.Msg(L("It is not enough yet. Go on to the Ishisula Broken District."));
				await dialog.CompleteQuest(Mq03);
				character.Quests.Start(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("The Kupoles captured many demons at Ishisula."));
				await dialog.Msg(L("The Hehmastar Isolation District is the last of them. Their Sealing Tokens are what I need."));
				await dialog.CompleteQuest(Mq04);
				character.Quests.Start(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Hauberk's spirit has become strong enough to handle the dimensional crack because of your efforts."));
				await dialog.Msg(L("The Chain of Reversion is also ready."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("The crack is holding. The Chain of Reversion has taken the last of it."));
				await dialog.CompleteQuest(Mq06);
				character.Quests.Start(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Every disaster that struck this land is over now."));
				await dialog.Msg(L("Savior. We owe everything to you."));
				await dialog.CompleteQuest(Mq07);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Savior. We really appreciate your help in stopping Hauberk."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Because of you, we are now able to close the dimensional crack and retrieve the spirit of Hauberk and the Chain of Reversion."),
					Option(L("Let's begin the ritual"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("We must supplement Hauberk's sealed spirit by adding the power of demons nearby."),
					Option(L("I will collect the Traces of Transference"), "accept"),
					Option(L("I will prepare a little more"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("I wish you good luck in the name of the goddess."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("You must go to the Lankine Separation District, the Ishisula Broken District and the Hehmastar Isolation District. I will send the Kupoles once you suppress the demons there using the power of the Evening Star Rune."),
					Option(L("I'll go now"), "accept"),
					Option(L("Give me some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("I also want you to collect the demons' Sealing Tokens at the Hehmastar Isolation District."));
					await dialog.Msg(L("Those wield a large amount of power as well."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("The Kupoles and I will try our best to seal the dimensional crack. This is our only chance... if we fail this time, we may not have another chance."),
					Option(L("I am ready"), "accept"),
					Option(L("Tell her that it will be ready soon"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.LookAround();
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Stand with us. The ritual will not hold if it is only the goddess in it."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("There is something I realized after meeting you."));
				await dialog.Msg(L("That I should have accepted Laima's warning in the first place."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("A goddess and a demon cannot easily interfere with each other's souls."));
				await dialog.Msg(L("Only a Revelator like you can do so."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The dimensional crack is connected with the demon's world so it can't be removed without the power of the demons."));
				await dialog.Msg(L("That is why your role, not the goddess' nor the demons', is important."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("We weren't able to monitor all the demons in the prison."));
				await dialog.Msg(L("That's why we engraved Sealing Tokens on them so they couldn't unleash their power."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The crack is still open. Stand between it and us."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Hold a moment longer. The seal is nearly set."));
				character.Quests.ReplayQuestTrack(Mq07);
				return;
			}

			await dialog.Msg(L("The Evening Star, standing under the crack she has spent everything she had holding shut."));
		});

		// Kupole Sigita
		//-------------------------------------------------------------------------
		AddConditionalNpc(154012, L("Kupole Sigita"), "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", -426.00, 57.63, 79, this.IsSigitaResting, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Kupole Sigita"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The crack will open again if we, the Kupoles, don't keep our eyes on it."));
				await dialog.Msg(L("I hope that we can find the Goddess Laima as soon as possible..."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("If Dionys recovers completely, it'll be all thanks to you."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("I will make sure I do everything possible to prevent another dimensional crack."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Ever since we closed the dimensional crack, the demons have been assembling. But I'm powerless to move..."),
					Option(L("I'll take care of it"), "accept"),
					Option(L("Everything will be alright"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("Demons are trying to expand their power using the disaster spirit."));
					return;
				}
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("I heard Dionys is suffering from slow recovery. Can you obtain the empty spirits from the demons at Pasaru Isolated Area?"),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("He'll be cured soon"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Once Dionys recovers, Vakarine will feel less pressured."));
					return;
				}
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("The small dimensional cracks are again carving away at us. But we don't know why, because now we're focusing on the stabilization."),
					Option(L("I will get rid of it"), "accept"),
					Option(L("I need some more time"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					character.LookAround();
					await dialog.Msg(L("We really owe you one."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Demons are trying to expand their power using the disaster spirit."));
				await dialog.Msg(L("Even after overcoming the current disaster, the peace is still far away."));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Once Dionys recovers, Vakarine will feel less pressured."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("We really owe you one."));
				return;
			}

			await dialog.Msg(L("A Kupole sitting where she fell, with nothing left to give the seal."));
		});

		// The dimensional crack of Vakarion Cathedral
		//-------------------------------------------------------------------------
		AddConditionalNpc(20026, L("Dimensional Crack"), "VPRISON515_MQ_06_NPC", "d_velniasprison_51_5", -2.22, -97.66, 135, this.IsCrackOpen, async dialog =>
		{
			await dialog.Msg(L("A tear that goes all the way through, and something on the far side of it that is not this world."));
		});

		// Small Dimensional Cracks of the Gavara Isolation District
		//-------------------------------------------------------------------------
		for (var i = 0; i < CrackSpots.GetLength(0); ++i)
		{
			AddConditionalNpc(147372, L("Small Dimensional Crack"), i == 0 ? "VPRISON515_SQ_03_NPC" : "VPRISON515_SQ_03_NPC_" + (i + 1), "d_velniasprison_51_5",
				CrackSpots[i, 0], CrackSpots[i, 1], 90, this.AreGavaraCracksOpen, this.CloseGavaraCrack);
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Lankine Separation District, where the rune is first used.
		AddQuestTrigger("VPRISON515_MQ_03_AREA", "d_velniasprison_51_5", -776, 634, 450, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				character.Quests.CompleteObjective(Mq03, "suppressLankine");
				character.ServerMessage(L("The rune takes the demons of Lankine. Report it to Vakarine."));
			}

			await Task.CompletedTask;
		});

		// The Ishisula Broken District.
		AddQuestTrigger("VPRISON515_MQ_04_AREA", "d_velniasprison_51_5", 724, 609, 500, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				character.Quests.CompleteObjective(Mq04, "suppressIshisula");
				character.ServerMessage(L("The rune takes the demons of Ishisula. Report it to Vakarine."));
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Sigita has come out to the cathedral floor.
	/// </summary>
	/// <param name="character"></param>
	private bool IsSigitaResting(Character character)
		=> character.Quests.HasCompleted(Mq07);

	/// <summary>
	/// Returns whether the cathedral's dimensional crack is still open.
	/// </summary>
	/// <param name="character"></param>
	private bool IsCrackOpen(Character character)
		=> !character.Quests.Has(Mq06);

	/// <summary>
	/// Returns whether the Gavara Isolation District still has cracks in it.
	/// </summary>
	/// <param name="character"></param>
	private bool AreGavaraCracksOpen(Character character)
		=> character.Quests.IsActive(Sq03);

	/// <summary>
	/// Closes one of the Gavara Isolation District's small cracks.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CloseGavaraCrack(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Small Dimensional Crack"));

		if (!character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("A crack the width of a finger, and it was not there yesterday."));
			return;
		}

		var closed = await character.TimeActions.StartAsync(L("Closing the dimensional crack..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (closed != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= CracksToClose; ++i)
		{
			if (character.Quests.IsActive(Sq03, "closeGavara" + i))
			{
				character.Quests.CompleteObjective(Sq03, "closeGavara" + i);
				character.ServerMessage(L("The crack closes on itself."));
				return;
			}
		}

		await dialog.Msg(L("The cracks Sigita named are shut. Go back and tell her."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 60023: The Dimensional Crack (1)
//-----------------------------------------------------------------------------
public class Vprison515Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60023);
		SetName(L("The Dimensional Crack (1)"));
		SetDescription(L("The ritual at Vakarion Cathedral is begun, and Hauberk's soul alone will not fill the crack."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("Prepared to close the dimensional crack. Talk to Goddess Vakarine in Demon Prison District 5."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Perform ritual to close the dimensional crack"), L("Help Goddess Vakarine in her ritual to close the dimensional crack."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("Goddess Vakarine's strength alone is not enough to close the dimensional crack. Talk to Goddess Vakarine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON515_MQ_01_TRACK", partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60022, QuestStatus.Completed));

		AddObjective("holdRitual", L("Perform ritual to close the dimensional crack"), new ManualObjective());
	}
}

// 60024: The Dimensional Crack (2)
//-----------------------------------------------------------------------------
public class Vprison515Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60024);
		SetName(L("The Dimensional Crack (2)"));
		SetDescription(L("The Evening Star Rune is empty, and the demons of Ausura Chapel carry what fills it."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("The ritual to close the dimensional crack doesn't seem to go well. Talk to Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Collect the traces of the metastasis"), L("Goddess Vakarine told you to collect the Traces of Transference to fill the Evening Star Rune. Defeat the demons in Ausura Chapel and collect the traces."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("Collected all the Traces of Transference needed. Give them to Goddess Vakarine."));

		AddPrerequisite(new QuestStatusPrerequisite(60023, QuestStatus.Completed));

		AddObjective("collectTraces", L("Collect the traces of the metastasis"), new CollectItemObjective("VPRISON515_MQ_RUNE_EMPTY_ITEM", 8));

		AddPityDrop("VPRISON515_MQ_RUNE_EMPTY_ITEM", 0.75f, 3, 1, "Hohen_gulak", "Mushroom_boy_green", "Hohen_mage");

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new ItemReward("VPRISON515_MQ_RUNE_ITEM", 1));
		AddReward(new TakeItemReward("VPRISON515_MQ_RUNE_EMPTY_ITEM"));
	}
}

// 60025: The Dimensional Crack (3)
//-----------------------------------------------------------------------------
public class Vprison515Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60025);
		SetName(L("The Dimensional Crack (3)"));
		SetDescription(L("The rune is used on the demons of the Lankine Separation District, and the Kupoles come for them."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("The Evening Star Rune is ready. Talk to Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_03_AREA", "d_velniasprison_51_5", L("Use Evening Star Rune on the demons."), L("Use the Evening Star Rune on the demons gathered at the Lankine Separation District and suppress them. The demons will go to Goddess Vakarine themselves when doing so."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("The Evening Star Rune has made many demons obedient. Talk to Goddess Vakarine again."));

		AddPrerequisite(new QuestStatusPrerequisite(60024, QuestStatus.Completed));

		AddObjective("suppressLankine", L("Use Evening Star Rune on the demons."), new ManualObjective());

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 60026: The Dimensional Crack (4)
//-----------------------------------------------------------------------------
public class Vprison515Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60026);
		SetName(L("The Dimensional Crack (4)"));
		SetDescription(L("The same again at the Ishisula Broken District, which is where most of them went."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("More demon's souls are still needed. Talk to Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_04_AREA", "d_velniasprison_51_5", L("Suppress demons using the Evening Star Rune"), L("Use the Evening Star Rune to suppress the demons gathered in Ishisula Broken District. The Kupoles will take care of those demons afterwards."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("The Kupoles captured many demons. Return to Goddess Vakarine."));

		AddPrerequisite(new QuestStatusPrerequisite(60025, QuestStatus.Completed));

		AddObjective("suppressIshisula", L("Suppress demons using the Evening Star Rune"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 60027: The Dimensional Crack (5)
//-----------------------------------------------------------------------------
public class Vprison515Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60027);
		SetName(L("The Dimensional Crack (5)"));
		SetDescription(L("The Sealing Tokens of the Hehmastar demons hold more power than the demons do."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("There should be more ways to gather the souls of demons. Talk to Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Obtain Sealing Tokens"), L("Goddess Vakarine requested you to use the Evening Star Rune to defeat the demons that are gathered at the Hehmastar Isolation District and collect Sealing Tokens."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Give it to Goddess Vakarine"), L("Acquired all the Sealing Tokens. Hand them over to the Goddess Vakarine."));

		AddPrerequisite(new QuestStatusPrerequisite(60026, QuestStatus.Completed));

		AddObjective("collectTokens", L("Collect the symbols of the condemned criminal"), new CollectItemObjective("VPRISON515_MQ_05_ITEM", 10));

		AddPityDrop("VPRISON515_MQ_05_ITEM", 1.0f, 0, 1, "Hohen_gulak", "Mushroom_boy_green", "Hohen_mage");

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("VPRISON515_MQ_05_ITEM"));
		AddReward(new TakeItemReward("VPRISON515_MQ_RUNE_ITEM"));
	}
}

// 60028: The Dimensional Crack (6)
//-----------------------------------------------------------------------------
public class Vprison515Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60028);
		SetName(L("The Dimensional Crack (6)"));
		SetDescription(L("Vakarine and all six Kupoles seal the crack, and nothing may reach them while they do."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("Prepared to close the dimensional crack. Talk to Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Protect Goddess Vakarine"), L("Goddess Vakarine asked you to help her while she and her Kupoles are sealing the dimensional crack at the Vakarion Cathedral."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("You successfully closed the dimensional crack! Talk to Goddess Vakarine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON515_MQ_06_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60027, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60026, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(60025, QuestStatus.Completed));

		AddObjective("guardVakarine", L("Protect Goddess Vakarine"), new ManualObjective());
	}
}

// 60042: The Dimensional Crack (7)
//-----------------------------------------------------------------------------
public class Vprison515Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60042);
		SetName(L("The Dimensional Crack (7)"));
		SetDescription(L("The crack is shut, and the Kupoles fall where they stood."));
		SetType(QuestType.Main);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Seal the dimensional crack"), L("It's almost ready to seal the dimensional crack. Talk to the Goddess Vakarine."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Seal the dimensional crack"), L("It's almost ready to seal the dimensional crack. Talk to the Goddess Vakarine."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_VAKARINE", "d_velniasprison_51_5", L("Talk to Goddess Vakarine"), L("You successfully closed the dimensional crack! Talk to Goddess Vakarine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "VPRISON515_MQ_06_AFTER", partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60028, QuestStatus.Completed));

		AddObjective("watchTheSeal", L("Seal the dimensional crack"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 60039: A Dead End
//-----------------------------------------------------------------------------
public class Vprison515Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60039);
		SetName(L("A Dead End"));
		SetDescription(L("The demons are gathering at the Vanaga Monitor District and Sigita cannot stand up."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Talk to Kupole Sigita"), L("Kupole Sigita, who is exhausted after using all her power filling the large dimensional crack, seems to have a request for you. Talk to Kupole Sigita."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Defeat the demons"), L("Kupole Sigita told you that the demons shouldn't be allowed to assemble together for the future. Defeat the demons that have assembled at the Vanaga Monitor District."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Talk to Kupole Sigita"), L("You've defeated all demons near Vanaga Monitor District. Talk to Kupole Sigita."));

		AddPrerequisite(new LevelPrerequisite(153));
		AddPrerequisite(new QuestStatusPrerequisite(60028, QuestStatus.Completed));

		AddObjective("killGathering", L("Defeat the demons of Vanaga Monitor District"), new KillObjective(8, "Hohen_gulak", "Mushroom_boy_green", "Hohen_mage"));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 60040: Food for Dionys
//-----------------------------------------------------------------------------
public class Vprison515Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60040);
		SetName(L("Food for Dionys"));
		SetDescription(L("Dionys is recovering slowly, and the empty spirits of the Pasaru demons would help."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Talk to Kupole Sigita"), L("Kupole Sigita is worried about Dionys, who is suffering. Talk to Kupole Sigita."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Collect the empty spirits"), L("To help Dionys recover, Kupole Sigita asked you to collect the empty spirits from the demons that are gathered at Pasaru isolated area."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Deliver to Kupole Sigita"), L("You've collected all of the empty spirits. Hand them over to Kupole Sigita."));

		AddPrerequisite(new LevelPrerequisite(153));
		AddPrerequisite(new QuestStatusPrerequisite(60028, QuestStatus.Completed));

		AddObjective("collectSpirits", L("Obtain the empty spirit"), new CollectItemObjective("VPRISON515_SQ_02_ITEM", 9));

		AddPityDrop("VPRISON515_SQ_02_ITEM", 0.75f, 3, 1, "Hohen_gulak", "Mushroom_boy_green", "Hohen_mage");

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("VPRISON515_SQ_02_ITEM"));
	}
}

// 60041: Eliminate the Small Gaps
//-----------------------------------------------------------------------------
public class Vprison515Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60041);
		SetName(L("Eliminate the Small Gaps"));
		SetDescription(L("Small cracks are opening in the Gavara Isolation District while the Kupoles are busy with the large one."));
		SetType(QuestType.Sub);
		SetLocation("d_velniasprison_51_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Talk to Kupole Sigita"), L("It seems that small dimensional cracks keep expanding from various places. Talk to Kupole Sigita again."));
		SetPhase(QuestStatus.InProgress, "VPRISON515_SQ_03_NPC", "d_velniasprison_51_5", L("Remove the Small Dimensional Crack"), L("Kupole Sigita said to remove the small dimensional crack of Gavara Isolation District to stop the disaster."));
		SetPhase(QuestStatus.Success, "VPRISON515_MQ_SIGITA", "d_velniasprison_51_5", L("Talk to Kupole Sigita"), L("You've eliminated all the dimensional cracks. Return to Kupole Sigita."));

		AddPrerequisite(new LevelPrerequisite(153));
		AddPrerequisite(new QuestStatusPrerequisite(60028, QuestStatus.Completed));

		AddObjective("closeGavara1", L("Remove the first Small Dimensional Crack"), new ManualObjective());
		AddObjective("closeGavara2", L("Remove the second Small Dimensional Crack"), new ManualObjective());
		AddObjective("closeGavara3", L("Remove the third Small Dimensional Crack"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}
