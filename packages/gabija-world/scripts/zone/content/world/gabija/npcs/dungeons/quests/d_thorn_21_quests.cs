//--- Melia Script ----------------------------------------------------------
// Kvailas Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The Believers who watch Bramble, the altars of purification, its roots and
// the revelation it took.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn21QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20268);
	private readonly static QuestId Mq02 = new QuestId(20269);
	private readonly static QuestId Mq03 = new QuestId(20270);
	private readonly static QuestId Mq04 = new QuestId(20271);
	private readonly static QuestId Mq05 = new QuestId(20272);
	private readonly static QuestId Mq06 = new QuestId(20273);
	private readonly static QuestId Mq07 = new QuestId(20274);
	private readonly static QuestId Mq08 = new QuestId(20275);
	private readonly static QuestId Mq09 = new QuestId(20295);

	protected override void Load()
	{
		// Believer Bronius
		//-------------------------------------------------------------------------
		AddNpc(147389, L("Believer Bronius"), "THORN21_BELIEVER", "d_thorn_21", 3652.09, -233.23, -6, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Bronius"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("I only survived because of you."));
				await dialog.Msg(L("I should warn the other Believers to be careful."));
				character.Quests.Complete(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Help me, please. Infro Holders are chasing after me."));

				var answer = await dialog.Select(L("I don't stand a chance the way they're ganging up on me."),
					Option(L("I will protect you"), "accept"),
					Option(L("It will be okay if you hide"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("They are right behind me - the shamans and the Chafperors both."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("Monsters are trying to attack us, but we can't just abandon our purification duties."));
			await dialog.Msg(L("Goddess Saule gave us our duties."));
		});

		// Believer Samantha
		//-------------------------------------------------------------------------
		AddNpc(147386, L("Believer Samantha"), "THORN21_BELIEVER02", "d_thorn_21", 692.34, -944.93, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Samantha"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Now I can relax and purify Thornbush Rest Place."));
				await dialog.Msg(L("Thank you so much."));
				character.Quests.Complete(Mq02);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Whenever I try to purify the Thornbush Rest Place, those monsters attack."));

				var answer = await dialog.Select(L("Is there some way to avoid them?"),
					Option(L("I will protect the altar from the monsters"), "accept"),
					Option(L("I don't know"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("I think I can count on you."));
					await dialog.Msg(L("Please protect the altar until the fountain is purified."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Monsters are becoming more violent as they enter deeper into the forest of thorns."));
				await dialog.Msg(L("Maybe that is because of the evil energy."));
				return;
			}

			await dialog.Msg(L("Even after all of the vicious energy is removed, this forest will remain as it is."));
			await dialog.Msg(L("But someday, we will also end that."));
		});

		// Believer Kazis
		//-------------------------------------------------------------------------
		AddNpc(147398, L("Believer Kazis"), "THORN21_BELIEVER03", "d_thorn_21", 1497.87, 151.35, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Kazis"));

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Are you hurt?"));
				await dialog.Msg(L("I heard the sound of Honeypin's webs, but I am relieved to see you are okay."));
				character.Quests.Complete(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("The Honeypin is hidden somewhere in Karadas Path, but I can't find it."));

				var answer = await dialog.Select(L("My purification duties has been delayed because of that monster."),
					Option(L("I'll help the purifying job"), "accept"),
					Option(L("It's dangerous, give up"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					await dialog.Msg(L("You will need to activate the Altar of Purification."));
					await dialog.Msg(L("Do this without letting the Honeypin know."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("If you feel that you are close to danger, quickly run away."));
				await dialog.Msg(L("The place is more dangerous due to the evil energy."));
				return;
			}

			await dialog.Msg(L("Kvailas Forest is the worst."));
			await dialog.Msg(L("The evil energy is unquestionably concentrated that even your lives are at stake."));
		});

		// Believer Jurga
		//-------------------------------------------------------------------------
		AddNpc(147397, L("Believer Jurga"), "THORN21_BELIEVER04", "d_thorn_21", -110.96, 116.21, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Jurga"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("I can feel Bramble suffering."));
				await dialog.Msg(L("He is getting retribution for extracting all vigor from this land."));
				character.Quests.Complete(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("It will be hard for Bramble to recover now."));
				await dialog.Msg(L("We can save the dying trees as well."));
				character.Quests.Complete(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Great."));
				await dialog.Msg(L("Now we're going to destroy the roots of Bramble."));
				character.Quests.Complete(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("Now we can make a full-scale attack on Bramble."));
				await dialog.Msg(L("I'll go on ahead to Giliaii Courtyard to keep an eye on Bramble, please come along."));
				character.Quests.Complete(Mq09);
				character.ServerMessage(L("Move to Giliaii Courtyard."));
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("You're the Savior the goddess sent. What an honor."));

				var answer = await dialog.Select(L("Are you ready to fight Bramble?"),
					Option(L("I'm ready"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(ItemId.THORN21_MQ04_DRUG, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("You'll need to be prepared if you want to defeat Bramble. First, take this."));
					await dialog.Msg(L("Put the Matsum's Flower Stamens in this potion and shake it."));
					await dialog.Msg(L("It will help you withstand Bramble's evil energy."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("Bramble is trying to recover by ingraining his roots around the Thorn Forest."));

				var answer = await dialog.Select(L("Cutting those roots will hurt him."),
					Option(L("I'll destroy them"), "accept"),
					Option(L("Give me some time"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq09);
					await dialog.Msg(L("The most important roots are at Sviesa Hill Areas and Tankinta Vacant Lot."));
					await dialog.Msg(L("Cut them both."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Try to hold it in even if it's dirty."));
				await dialog.Msg(L("This is the best I can do."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("You mustn't let your guard down, even if Bramble is weakened."));
				await dialog.Msg(L("After all, it is a Demon Lord, no matter how injured it is."));
				return;
			}

			await dialog.Msg(L("I have recieved orders from Goddess Saule to keep a close eye on Bramble."));
			await dialog.Msg(L("He is the source of all evil beings in this forest."));
		});

		// Believer Jurga at Giliaii Courtyard
		//-------------------------------------------------------------------------
		AddNpc(147397, L("Believer Jurga"), "THORN21_BELIEVER04_AFTER", "d_thorn_21", 4565.75, -58.03, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Jurga"));

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("Bramble and the revelation are in the depths of Giliaii Courtyard."));

				var answer = await dialog.Select(L("It must be agonizing since the roots for healing the scars have all been cut off."),
					Option(L("I'll defeat Bramble and retrieve the revelation"), "accept"),
					Option(L("Give me some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					await dialog.Msg(L("Don't forget to use the stimulant whenever you breathe in its evil energy."));
					await dialog.Msg(L("May the blessings of Goddess Saule be with you."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Don't forget to use the stimulant whenever you breathe in its evil energy."));
				await dialog.Msg(L("May the blessings of Goddess Saule be with you."));
				return;
			}

			await dialog.Msg(L("It was a real honor to meet you."));
			await dialog.Msg(L("I will pray to Goddess Saule that your future be blessed."));
		});

		// Altar of Purification at Thornbush Rest Place
		//-------------------------------------------------------------------------
		AddNpc(46213, L("Altar of Purification"), "THORN21_MQ02_TRACK", "d_thorn_21", 1012, -1241, -17, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of Purification"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You work the altar until the water in the basin runs clear."));
				var startedAltar2 = await character.TimeActions.StartAsync(L("Starting the purification altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (startedAltar2 != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("An Altar of Purification, cut from a single block. The Believers keep it swept."));
		});

		// Altar of Purification at Karadas Path
		//-------------------------------------------------------------------------
		AddNpc(46213, L("Altar of Purification"), "THORN21_MQ06_TRACK", "d_thorn_21", 2003, -3, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of Purification"));

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("You set the altar working, quietly, the way Kazis asked."));
				character.ServerMessage(L("The Altar of Purification is working!"));
				var startedAltar6 = await character.TimeActions.StartAsync(L("Starting the purification altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (startedAltar6 != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("An Altar of Purification, standing in the open on Karadas Path."));
		});

		// Bramble's Root at Sviesa Hill Areas
		//-------------------------------------------------------------------------
		AddNpc(153011, L("Bramble's Root"), "THORN21_BRAMBLE01_ROOT", "d_thorn_21", 2800, -1325, 77, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bramble's Root"));

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.Select(L("A root as thick as a man, sunk into the hillside and drawing on it."),
					Option(L("Check Sviesa Hill Areas"), "accept"),
					Option(L("Leave the root alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.ServerMessage(L("Gaigalas steps in the moment you touch the root!"));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Gaigalas is still standing over the root."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("A root of Bramble, cut through and going grey."));
		});

		// Bramble's Root at Tankinta Vacant Lot
		//-------------------------------------------------------------------------
		AddNpc(153011, L("Bramble's Root"), "THORN21_BRAMBLE02_ROOT", "d_thorn_21", 3305, 1084, 82, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bramble's Root"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.Select(L("The second root runs the length of the lot, and the ground over it is dead."),
					Option(L("Check Tankinta Vacant Lot"), "accept"),
					Option(L("Leave the root alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.ServerMessage(L("Molich steps in the moment you touch the root!"));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Molich is still guarding the root."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A root of Bramble, cut through and going grey."));
		});

		// Revelation
		//-------------------------------------------------------------------------
		AddNpc(47234, L("Revelation"), "THORN21_MQ08", "d_thorn_21", 5984, -202, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Revelation"));

			if (character.Quests.IsActive(Mq08) && !character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("The slate is whole, and the script on it is the goddess' own."));
				character.Inventory.Add(ItemId.Stonetablet031, 1, InventoryAddType.PickUp);
				character.Quests.CompleteObjective(Mq08, "takeRevelation");
				return;
			}

			await dialog.Msg(L("The stand the revelation was set in, empty now."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("THORN21_MQ07_TRACK", "d_thorn_21", 5306.49, -165.17, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
				character.Quests.StartQuestTrack(Mq07);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20268: The Attack of the Infro Holders
//-----------------------------------------------------------------------------
public class Thorn21Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20268);
		SetName(L("The Attack of the Infro Holders"));
		SetDescription(L("Believer Bronius is being run down by the Infro Holders that followed him."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER", "d_thorn_21", L("Talk to Believer Bronius"), L("Goddess Saule Believer Bronius is hiding and waiting anxiously for your help."));
		SetPhase(QuestStatus.InProgress, "THORN21_BELIEVER", "d_thorn_21", L("Defeat the Infro Holders that rushed in"), L("Believer Bronius says the monsters are after him. Defeat the monsters."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER", "d_thorn_21", L("Talk to Believer Bronius"), L("Defeated the monsters. Talk to Believer Bronius."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ01_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(54));

		AddObjective("killInfroHolders", L("Defeat the monsters trying to kill the Believer"), new KillObjective(6, "Chafperor", "Infroholder_mage") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 20269: Purify Kvailas Forest (1)
//-----------------------------------------------------------------------------
public class Thorn21Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20269);
		SetName(L("Purify Kvailas Forest (1)"));
		SetDescription(L("Believer Samantha cannot purify Thornbush Rest Place while the monsters keep coming at the altar."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER02", "d_thorn_21", L("Talk to Believer Samantha"), L("Goddess Saule's Believer Samantha is waiting for your help in Kvailas Forest."));
		SetPhase(QuestStatus.InProgress, "THORN21_MQ02_TRACK", "d_thorn_21", L("Protect the altar from the monsters"), L("Protect the Altar of Purification at Thornbush Rest Place from the monsters."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER02", "d_thorn_21", L("Talk to Believer Samantha"), L("Protected the altar from the monsters. Return to Believer Samantha."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ02_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(54));

		AddObjective("holdAltar", L("Protect the altar from the monsters"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 20270: Root of Sviesa Hill Areas
//-----------------------------------------------------------------------------
public class Thorn21Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20270);
		SetName(L("Root of Sviesa Hill Areas"));
		SetDescription(L("One of Bramble's roots is sunk into Sviesa Hill Areas, and Gaigalas is over it."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BRAMBLE01_ROOT", "d_thorn_21", L("Check Sviesa Hill Areas"), L("Believer Jurga says one of Bramble's roots is in Sviesa Hill Areas. Check Sviesa Hill Areas."));
		SetPhase(QuestStatus.InProgress, "THORN21_BRAMBLE01_ROOT", "d_thorn_21", L("Cut Bramble's Root"), L("Approached the Bramble's roots but Gaigalas appeared. You must defeat Gaigalas before destroying Bramble's roots."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("You cut off Bramble's roots. Return and talk to Believer Jurga."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(54));

		AddObjective("killGaigalas", L("Defeat Gaigalas"), new KillObjective(1, "boss_Gaigalas") { LayerOnly = true });
		AddObjective("cutRoot", L("Destroy Bramble's Root"), new KillObjective(1, "npc_bramble_root") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("misc_NECK03_104_1", 1));
	}
}

// 20271: Capturing Bramble (1)
//-----------------------------------------------------------------------------
public class Thorn21Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20271);
		SetName(L("Capturing Bramble (1)"));
		SetDescription(L("The stimulant that holds off Bramble's evil energy needs Matsum's Flower Stamen."));
		SetType(QuestType.Main);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("Believer Jurga is following the goddess in watching over Bramble and is waiting for you."));
		SetPhase(QuestStatus.InProgress, "THORN21_BELIEVER04", "d_thorn_21", L("Get Matsum's Flower Stamen to create a Thorn Flower Stimulant"), L("Obtain Matsum's Flower Stamen at Rankena Hill."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("Collect all of Matsum's Flower Stamen. Return to Believer Jurga."));

		AddPrerequisite(new QuestStatusPrerequisite(50003, QuestStatus.Completed));

		AddPityDrop("THORN21_MQ04_BUGWING", 0.45f, 5, 1, "Matsum");

		AddObjective("collectStamen", L("Obtain Matsum's Flower Stamen"), new CollectItemObjective("THORN21_MQ04_BUGWING", 4));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("THORN21_MQ04_BUGWING"));
	}
}

// 20272: Root of Tankinta Vacant Lot
//-----------------------------------------------------------------------------
public class Thorn21Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20272);
		SetName(L("Root of Tankinta Vacant Lot"));
		SetDescription(L("The second of Bramble's roots runs under Tankinta Vacant Lot, with Molich guarding it."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BRAMBLE02_ROOT", "d_thorn_21", L("Check Tankinta Vacant Lot"), L("Believer Jurga says one of Bramble's roots is in Tankinta Vacant Lot. Check out Tankinta Vacant Lot."));
		SetPhase(QuestStatus.InProgress, "THORN21_BRAMBLE02_ROOT", "d_thorn_21", L("Cut Bramble's Root"), L("Defeat Molich guarding the roots of Bramble."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("You cut off Bramble's roots. Return and talk to Believer Jurga."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ05_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(54));

		AddObjective("killMolich", L("Defeat Molich"), new KillObjective(1, "boss_molich") { LayerOnly = true });
		AddObjective("cutRoot", L("Destroy Bramble's Root"), new KillObjective(1, "npc_bramble_root") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 20273: Purify Kvailas Forest (2)
//-----------------------------------------------------------------------------
public class Thorn21Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20273);
		SetName(L("Purify Kvailas Forest (2)"));
		SetDescription(L("Believer Kazis wants the Altar of Purification on Karadas Path set working, Honeypin or no Honeypin."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER03", "d_thorn_21", L("Talk to Believer Kazis"), L("Goddess Saule Believer Kazis needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN21_MQ06_TRACK", "d_thorn_21", L("Purify Karadas Path"), L("Believer Kazis says the Honeypin is making it difficult to do purifying work. Activate the altar for Believer Kazis."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER03", "d_thorn_21", L("Talk to Believer Kazis"), L("Defeated the Honeypin. Return to Believer Kazis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ06_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(54));

		AddObjective("killHoneypin", L("Defeat Honeypin"), new KillObjective(1, "boss_honeypin") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 20295: Capturing Bramble (2)
//-----------------------------------------------------------------------------
public class Thorn21Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20295);
		SetName(L("Capturing Bramble (2)"));
		SetDescription(L("Bramble heals itself through the roots at Sviesa Hill Areas and Tankinta Vacant Lot. Cut them both."));
		SetType(QuestType.Main);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("Believer Jurga says there are more things to do other than making the stimulant. Talk to Believer Jurga."));
		SetPhase(QuestStatus.InProgress, "THORN21_BRAMBLE01_ROOT", "d_thorn_21", L("Destroy Bramble's roots"), L("Jurga says we must destroy the roots placed to heal Bramble's wound. The roots are in Sviesa Hill Areas and Tankinta Vacant Lot."));
		SetPhase(QuestStatus.Success, "THORN21_BELIEVER04", "d_thorn_21", L("Talk to Believer Jurga"), L("Removed all of Bramble's roots. Talk to Believer Jurga."));

		AddPrerequisite(new QuestStatusPrerequisite(20271, QuestStatus.Completed));

		AddObjective("cutRoots", L("Destroy Bramble's roots"), new KillObjective(2, "npc_bramble_root"));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 20274: Capturing Bramble (3)
//-----------------------------------------------------------------------------
public class Thorn21Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20274);
		SetName(L("Capturing Bramble (3)"));
		SetDescription(L("Bramble and the revelation are in the depths of Giliaii Courtyard."));
		SetType(QuestType.Main);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN21_BELIEVER04_AFTER", "d_thorn_21", L("Talk to Believer Jurga"), L("You agreed to meet Believer Jurga in Giliaii Courtyard to plan a fight against Bramble."));
		SetPhase(QuestStatus.InProgress, "THORN21_MQ07_TRACK", "d_thorn_21", L("Defeat Bramble and retrieve the revelation"), L("You arrived at Giliaii Courtyard where Bramble is. Defeat Bramble and retrieve the revelation!"));
		SetPhase(QuestStatus.Success, "THORN21_MQ08", "d_thorn_21", L("Retrieve the revelation"), L("Successfully defeated Bramble! Get the goddess' revelation."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN21_MQ07_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20295, QuestStatus.Completed));

		AddObjective("killBramble", L("Defeat Bramble"), new KillObjective(1, "boss_bramble") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("misc_NECK03_104_3", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC, and the
		// revelation is picked up by the quest that follows.
		character.ServerMessage(L("Bramble is down. The revelation is still standing where it was set."));
		character.Quests.Complete(this.QuestId);

		if (!character.Quests.Has(new QuestId(20275)))
			character.Quests.Start(new QuestId(20275));
	}
}

// 20275: To Goddess Saule
//-----------------------------------------------------------------------------
public class Thorn21Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20275);
		SetName(L("To Goddess Saule"));
		SetDescription(L("Take the Revelation of Kvailas Forest from its stand and carry it back to Goddess Saule."));
		SetType(QuestType.Main);
		SetLocation("d_thorn_21");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "THORN21_MQ08", "d_thorn_21", L("Retrieve the revelation"), L("Successfully defeated Bramble! Get the goddess' revelation."));
		SetPhase(QuestStatus.InProgress, "THORN21_MQ08", "d_thorn_21", L("Retrieve the revelation"), L("Successfully defeated Bramble! Get the goddess' revelation."));
		SetPhase(QuestStatus.Success, "THORN21_MQ08", "d_thorn_21", L("Retrieve the revelation"), L("Successfully defeated Bramble! Get the goddess' revelation."));

		AddPrerequisite(new QuestStatusPrerequisite(20274, QuestStatus.Completed));

		AddObjective("takeRevelation", L("Retrieve the revelation"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("THORN21_MQ07_THORNDRUG", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The pickup is the quest; the client names no turn-in NPC.
		character.AddStatPoints(3);
		character.Quests.Complete(this.QuestId);
		character.ServerMessage(L("Carry the revelation back to Goddess Saule at Septyni Glen."));
	}
}
