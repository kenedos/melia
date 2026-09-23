//--- Melia Script ----------------------------------------------------------
// Royal Mausoleum 1F Quest NPCs
//--- Description -----------------------------------------------------------
// The foundation stone, the epitaphs that explain the mausoleum's defenses,
// and the guardians that have forgotten what they were built for.
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

public class DZachariel32QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8600);
	private readonly static QuestId Mq02 = new QuestId(8211);
	private readonly static QuestId Mq03 = new QuestId(8212);
	private readonly static QuestId Mq04 = new QuestId(8254);
	private readonly static QuestId Mq05 = new QuestId(8255);
	private readonly static QuestId Sq01 = new QuestId(8428);
	private readonly static QuestId Sq02 = new QuestId(8429);
	private readonly static QuestId Sq03 = new QuestId(8430);
	private readonly static QuestId Sq04 = new QuestId(8431);
	private readonly static QuestId Sq05 = new QuestId(8432);
	private readonly static QuestId Rp1 = new QuestId(60170);

	private readonly static double[,] SleepingBoowookSpots =
	{
		{ -11, -1206 }, { -106.33, -1012.23 }, { 46.26, -551.92 }, { 88.26, -1006.39 },
		{ -2.18, -690.25 }, { 167.84, -1061.87 }, { 27.68, -1397.34 }, { 333.28, -978.33 },
	};

	protected override void Load()
	{
		// Royal Mausoleum Foundation Stone
		//-------------------------------------------------------------------------
		AddNpc(147467, L("Royal Mausoleum Foundation Stone"), "ZACHA1F_MQ_01", "d_zachariel_32", 37.35, -2046.59, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Foundation Stone"));

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("I pass my words to the Revelator who comes here."));
				await dialog.Msg(L("I, the Great King Zachariel, sleep here to protect the goal of the goddess."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Wake the guardians that sleep along this hall, and let them stop what has come in."),
					Option(L("Repair the broken Royal Mausoleum Guardian"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					character.Inventory.Add(ItemId.ZACHA1F_REPAIR, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("The awakening device is yours. The sleeping Boowook are further down the hall."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Use the awakening device on one of the sleeping Boowook down the hall."));
				return;
			}

			await dialog.Msg(L("I, the Great King Zachariel, sleep here to protect the goal of the goddess."));
		});

		// Sleeping Boowook
		//-------------------------------------------------------------------------
		for (var i = 0; i < SleepingBoowookSpots.GetLength(0); i++)
		{
			var uniqueName = "ZACHA1F_MQ_01_MON" + (i + 1);
			AddNpc(57565, L("Sleeping Boowook"), uniqueName, "d_zachariel_32", SleepingBoowookSpots[i, 0], SleepingBoowookSpots[i, 1], 90, this.WakeSleepingBoowook);
		}

		// Royal Mausoleum Cube Manual
		//-------------------------------------------------------------------------
		AddNpc(47252, L("Royal Mausoleum Cube Manual"), "ZACHA1F_MQ_02", "d_zachariel_32", -567, -929, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Cube Manual"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The cubes have gone quiet. What was corrupted in them is out."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("A Guardian who is corrupted by evil will forget its responsibility to protect the Royal Mausoleum. Defeat these Guardians near the large cubes."),
					Option(L("Go to purify the corrupted Guardian"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq02);

				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The large cubes are west of here, down the side hall."));
				character.Quests.ClearQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("A Guardian who is corrupted by evil will forget its responsibility to protect the Royal Mausoleum."));
		});

		// Defense System Manual
		//-------------------------------------------------------------------------
		AddNpc(47252, L("Defense System Manual"), "ZACHA1F_MQ_03", "d_zachariel_32", -1020, -477, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Defense System Manual"));

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("A Guardian that forgot its mission to protect the Royal Mausoleum deserves to be destroyed."),
					Option(L("Destroy the corrupted protecting device"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The devices that keep calling them up stand north of here, in the upper gallery."));
				return;
			}

			await dialog.Msg(L("A Guardian that forgot its mission to protect the Royal Mausoleum deserves to be destroyed."));
		});

		// Malfunctioning Guardian Devices
		//-------------------------------------------------------------------------
		AddConditionalNpc(47253, L("Malfunctioning Guardian Device"), "ZACHA1F_MQ_03_LANTERN1", "d_zachariel_32", -1054, 383, 0, c => !c.Quests.HasCompleted(Mq03), this.BreakGuardianDevice);
		AddConditionalNpc(47253, L("Malfunctioning Guardian Device"), "ZACHA1F_MQ_03_LANTERN2", "d_zachariel_32", -943, 383, 0, c => !c.Quests.HasCompleted(Mq03), this.BreakGuardianDevice);

		// Guardian's Role
		//-------------------------------------------------------------------------
		AddNpc(47252, L("Guardian's Role"), "ZACHA1F_MQ_04", "d_zachariel_32", -530, 176, 10, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Guardian's Role"));

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("By recycling the power supply of the Guardians that forgot their mission, we can return them to be part of the Royal Mausoleum."),
					Option(L("Let's remove those Guardians from the Royal Mausoleum"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("Two offering containers stand in the hall above. Put one power source into each."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Take the power sources off the guardians and put them into the two containers in the hall above."));
				return;
			}

			await dialog.Msg(L("By recycling the power supply of the Guardians that forgot their mission, we can return them to be part of the Royal Mausoleum."));
		});

		// Power Source Offering Containers
		//-------------------------------------------------------------------------
		AddNpc(40064, L("Power Source Offering Container"), "ZACHA32_MQ_04_D1", "d_zachariel_32", 57, 332, 90, dialog => this.OfferPowerSource(dialog, "offerFirst"));
		AddNpc(40064, L("Power Source Offering Container"), "ZACHA32_MQ_04_D2", "d_zachariel_32", 43, -70, 90, dialog => this.OfferPowerSource(dialog, "offerSecond"));

		// Royal Mausoleum Tombstone
		//-------------------------------------------------------------------------
		AddNpc(47251, L("Royal Mausoleum Tombstone"), "ZACHA1F_MQ_05_NPC", "d_zachariel_32", 159.72, 762.73, 6, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Tombstone"));

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Achat is the faithful Guardian of the Royal Mausoleum. But, once it gets corrupted, it will never return to its original state."),
					Option(L("Let's look for Achat"), "accept"),
					Option(L("Let's postpone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("Achat keeps the far end of this floor. It will not stand aside."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				character.Quests.ClearQuestTrack(Mq05);
				await dialog.Msg(L("Achat is at the far end of this floor."));
				return;
			}

			await dialog.Msg(L("Achat is the faithful Guardian of the Royal Mausoleum."));
		});

		// Royal Mausoleum Blueprint
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Royal Mausoleum Blueprint"), "ZACHA1F_SQ_01", "d_zachariel_32", 856, 222, 40, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Blueprint"));

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("If the Royal Mausoleum's power source is corrupted, the Guardians will forget their mission."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("You can purify the Royal Mausoleum's power source by lighting up the stone lanterns with the burning stones inside of a Guardian."),
					Option(L("Let's go light the stone lantern"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Ten burning stones, and the lantern below the east gallery."));
				return;
			}

			await dialog.Msg(L("If the Royal Mausoleum's power source is corrupted, the Guardians will forget their mission."));
		});

		// Royal Mausoleum Stone Lantern
		//-------------------------------------------------------------------------
		AddNpc(47253, L("Royal Mausoleum Stone Lantern"), "ZACHA1F_SQ_02", "d_zachariel_32", 1111, -757, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Stone Lantern"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Clymen is down and the lantern is still burning. The floor's magic is purified."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				var lit = await character.TimeActions.StartAsync(L("Feeding the fire..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (lit != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The burning stones catch, and the lantern takes the fire."));
				await dialog.CompleteQuest(Sq01);

				if (!character.Quests.Has(Sq02))
				{
					character.Quests.Start(Sq02);
					character.Quests.StartQuestTrack(Sq02);
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Clymen is still standing over the lantern."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			await dialog.Msg(L("A stone lantern of the Royal Mausoleum, long gone cold."));
		});

		// Royal Mausoleum Regulatory Magic Manual
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Royal Mausoleum Regulatory Magic Manual"), "ZACHA1F_SQ_03", "d_zachariel_32", -95, 280, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Regulatory Magic Manual"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				var handed = await character.TimeActions.StartAsync(L("Handing over the activation stones..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (handed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The stones go into the manual's setting, and their power settles into your hands."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("We can suppress the evil presence if the magic of the Royal Mausoleum is released. This requires a number of activation stones located inside active Guardians."),
					Option(L("Let's gather the activation stones"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq03);

				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("The Magic Regulator of the Royal Mausoleum can be destroyed with the power of the Activation Stones."),
					Option(L("Go to destroy the evil power suppressor of the Mausoleum"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq04);

				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("One regulator is broken and one is standing. The power of the stones is still in you."),
					Option(L("Go to destroy the last regulator"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq05);

				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Six activation stones. The guardians of this floor carry them."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The first regulator stands in the west gallery."));
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("The last regulator stands in the east gallery."));
				return;
			}

			await dialog.Msg(L("We can suppress the evil presence if the magic of the Royal Mausoleum is released."));
		});

		// Royal Mausoleum Magic Regulators
		//-------------------------------------------------------------------------
		AddConditionalNpc(47261, L("Royal Mausoleum Magic Regulator"), "ZACHA1F_SQ_04", "d_zachariel_32", -1006, 1408, 90, c => !c.Quests.HasCompleted(Sq04), dialog => this.BreakRegulator(dialog, Sq04));
		AddConditionalNpc(47261, L("Royal Mausoleum Magic Regulator"), "ZACHA1F_SQ_05", "d_zachariel_32", 1112, 1408, 90, c => !c.Quests.HasCompleted(Sq05), dialog => this.BreakRegulator(dialog, Sq05));

		// Royal Mausoleum Guardian
		//-------------------------------------------------------------------------
		AddNpc(47260, L("Royal Mausoleum Guardian"), "ZACHA32_RP_1_NPC", "d_zachariel_32", 50.28, 1806.08, 359, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Guardian"));

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("The halls are quieter than they were. That is all any of us can ask now."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("Clear the Royal Mausoleum of the Guardians that overlook sinister presences and are trying to destroy the defense systems."),
					Option(L("Sure, I'll help"), "accept"),
					Option(L("I don't think that's needed"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rp1);
					await dialog.Msg(L("The defense system is being destroyed."));
				}
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("Thirteen of them. The upper galleries are where they gather."));
				return;
			}

			await dialog.Msg(L("I have kept this floor since the Great King was laid down. I will keep it a while longer."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The hall of the four large cubes.
		AddQuestTrigger("ZACHA1F_MQ_02_CUBES", "d_zachariel_32", -1008, -973, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.StartQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		AddQuestTrigger("ZACHA1F_MQ_05", "d_zachariel_32", 50, 1191, 125, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Wakes one of the guardians sleeping along the entrance hall.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task WakeSleepingBoowook(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Sleeping Boowook"));

		if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
		{
			var woken = await character.TimeActions.StartAsync(L("Working the awakening device..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

			if (woken != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Mq01, "wakeBoowook");

			await dialog.Msg(L("The Boowook's shell comes apart and it stands, remembering what it was set here to do."));
			return;
		}

		await dialog.Msg(L("A guardian of the Royal Mausoleum, folded up and asleep where it was left."));
	}

	/// <summary>
	/// Breaks one of the two devices that keep calling corrupted guardians up.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task BreakGuardianDevice(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Malfunctioning Guardian Device"));

		if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
		{
			character.Quests.StartQuestTrack(Mq03);
			return;
		}

		await dialog.Msg(L("A stone lantern of the defense system, running on nothing and calling up whatever answers."));
	}

	/// <summary>
	/// Puts a guardian's power source into one of the offering containers.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="objectiveIdent"></param>
	private async Task OfferPowerSource(Dialog dialog, string objectiveIdent)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Power Source Offering Container"));

		if (!character.Quests.IsActive(Mq04, objectiveIdent))
		{
			await dialog.Msg(L("A container for the power sources of guardians that are past repairing."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.ZACHA1F_MQ_04_ITEM) < 1)
		{
			await dialog.Msg(L("The container is open and empty. You are carrying no power source to put in it."));
			return;
		}

		var offered = await character.TimeActions.StartAsync(L("Putting the power source in..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

		if (offered != TimeActionResult.Completed)
			return;

		character.Inventory.RemoveItem(ItemId.ZACHA1F_MQ_04_ITEM, 1);
		character.Quests.CompleteObjective(Mq04, objectiveIdent);

		await dialog.Msg(L("The power source settles into the container and its light goes out."));
	}

	/// <summary>
	/// Arms one of the floor's two magic regulators against the player.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="questId"></param>
	private async Task BreakRegulator(Dialog dialog, QuestId questId)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Royal Mausoleum Magic Regulator"));

		if (character.Quests.IsActive(questId) && !character.Quests.IsCompletable(questId))
		{
			character.Quests.StartQuestTrack(questId);
			return;
		}

		await dialog.Msg(L("A regulator of the Royal Mausoleum, holding the floor's magic down."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8600: Collapsed Protection System
//-----------------------------------------------------------------------------
public class Zacha1fMq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8600);
		SetName(L("Collapsed Protection System"));
		SetDescription(L("The foundation stone asks the Revelator to wake the guardians that sleep in the entrance hall."));
		SetType(QuestType.Main);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_MQ_01", "d_zachariel_32", L("Read the Royal Mausoleum Foundation Stone"), L("There is a foundation stone in the Royal Mausoleum. Read it."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_MQ_01_MON1", "d_zachariel_32", L("Wake the sleeping Boowook"), L("The Foundation Stone said to wake the sleeping Guardians and stop the demons when they intrude. Wake the sleeping Boowook and stop the demons."));
		SetPhase(QuestStatus.Success, "ZACHA1F_MQ_01_MON1", "d_zachariel_32", L("Find another epitaph"), L("The epitaph felt like it was foretelling something that you should do from now on. You better move as written on the epitaph."));

		AddPrerequisite(new QuestStatusPrerequisite(9003, QuestStatus.Completed));

		AddObjective("wakeBoowook", L("Wake the sleeping Boowook"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("ZACHA1F_REPAIR", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The waking is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The guardian is awake. There are more epitaphs further into the mausoleum."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8211: Guardian Purifying Device
//-----------------------------------------------------------------------------
public class Zacha1fMq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8211);
		SetName(L("Guardian Purifying Device"));
		SetDescription(L("The corrupted guardians gather at the large cubes and have to be put down there."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_MQ_02", "d_zachariel_32", L("Check the epitaph of the Royal Mausoleum"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_MQ_02_CUBES", "d_zachariel_32", L("Defeat the corrupted Guardians at the Royal Cubes"), L("The corrupted Guardians can't remember their mission. Defeat them at the large cubes."));
		SetPhase(QuestStatus.Success, "ZACHA1F_MQ_02", "d_zachariel_32", L("Read the epitaph again"), L("The cubes are clear. Read the epitaph again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_MQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(71));

		AddObjective("purifyCubes", L("Defeat the corrupted Guardians at the Royal Cubes"), new KillObjective(4, "npc_zachariel_cube_09"));

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 8212: Destroying the Guardian Device
//-----------------------------------------------------------------------------
public class Zacha1fMq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8212);
		SetName(L("Destroying the Guardian Device"));
		SetDescription(L("Two devices in the upper gallery keep calling corrupted guardians up."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_MQ_03", "d_zachariel_32", L("Read the epitaph"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_MQ_03_LANTERN1", "d_zachariel_32", L("Destroy the corrupted defense mechanism"), L("Destroy the corrupted defense mechanism, so the Guardians stop appearing."));
		SetPhase(QuestStatus.Success, "ZACHA1F_MQ_03_LANTERN1", "d_zachariel_32", L("Destroy the corrupted defense mechanism"), L("Destroy the corrupted defense mechanism, so the Guardians stop appearing."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_MQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(71));

		AddObjective("breakDevices", L("Destroy the corrupted defense mechanism"), new KillObjective(2, "npc_zachariel_lantern") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The devices are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Both devices are broken. The guardians will stop coming up here."));
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 8254: Recycling (1)
//-----------------------------------------------------------------------------
public class Zacha1fMq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8254);
		SetName(L("Recycling (1)"));
		SetDescription(L("A guardian past repairing gives its power source back to the mausoleum."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_MQ_04", "d_zachariel_32", L("Check the epitaph of the Royal Mausoleum"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA32_MQ_04_D1", "d_zachariel_32", L("Get the power source from the Guardians and dedicate it to the containers"), L("When a Royal Mausoleum Guardian forgets its duty, its power source gets put into a container and returned to the Royal Mausoleum. Defeat the Guardians and obtain their power sources, then put them into the containers."));
		SetPhase(QuestStatus.Success, "ZACHA32_MQ_04_D1", "d_zachariel_32", L("Get the power source from the Guardians and dedicate it to the containers"), L("When a Royal Mausoleum Guardian forgets its duty, its power source gets put into a container and returned to the Royal Mausoleum."));

		AddPrerequisite(new LevelPrerequisite(71));

		// The client's row carries no drop for the power source, so the guardians
		// of this floor hand it over.
		AddPityDrop("ZACHA1F_MQ_04_ITEM", 1.0f, 0, 1, "Moving_trap", "zinutekas");

		AddObjective("offerFirst", L("Dedicate a power source to the first container"), new ManualObjective());
		AddObjective("offerSecond", L("Dedicate a power source to the second container"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("ZACHA1F_MQ_04_ITEM"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The offerings are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("A heavy tremor runs through the floor."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8255: Recycling (2)
//-----------------------------------------------------------------------------
public class Zacha1fMq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8255);
		SetName(L("Recycling (2)"));
		SetDescription(L("Achat is past purifying, and it keeps the far end of the first floor."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_MQ_05_NPC", "d_zachariel_32", L("Read the epitaph"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_MQ_05", "d_zachariel_32", L("Defeat Achat"), L("Achat is already corrupted. Defeat Achat."));
		SetPhase(QuestStatus.Success, "ZACHA1F_MQ_05", "d_zachariel_32", L("Defeat Achat"), L("Achat is already corrupted. Defeat Achat."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8254, QuestStatus.Completed));

		AddObjective("killAchat", L("Guardian Achat"), new KillObjective(1, "boss_Achat") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("R_BRC03_105", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Achat will not guard anything again."));
		character.Quests.Complete(this.QuestId);
	}
}

// 8428: Light the Fire (1)
//-----------------------------------------------------------------------------
public class Zacha1fSq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8428);
		SetName(L("Light the Fire (1)"));
		SetDescription(L("Burning stones out of the guardians will light the mausoleum's stone lantern."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_SQ_01", "d_zachariel_32", L("Read the epitaph"), L("Various writings on how to prepare when the demons attack are engraved on the epitaph. Read the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_SQ_01", "d_zachariel_32", L("Obtain the Burning Stone from the Guardians of the Royal Mausoleum"), L("To purify the corrupted magic power, you should light up the stone lantern with the Burning Stone. Defeat the Royal Mausoleum Guardians first to get the Burning Stone."));
		SetPhase(QuestStatus.Success, "ZACHA1F_SQ_02", "d_zachariel_32", L("Light up the stone lantern with the Burning Stone"), L("You have collected enough Burning Stones. Find the stone lantern of the Royal Mausoleum and light it up with the Burning Stone."));

		AddPrerequisite(new LevelPrerequisite(72));

		AddPityDrop("ZACHA1F_SQ_01_ITEM", 1.0f, 0, 1, "Moving_trap", "zinutekas");

		AddObjective("collectStones", L("Defeat the Royal Mausoleum Guardians and obtain Burning Stones"), new CollectItemObjective("ZACHA1F_SQ_01_ITEM", 10));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("ZACHA1F_SQ_01_ITEM"));
	}
}

// 8429: Light the Fire (2)
//-----------------------------------------------------------------------------
public class Zacha1fSq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8429);
		SetName(L("Light the Fire (2)"));
		SetDescription(L("A demon comes for the stone lantern the moment its fire is lit."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_SQ_02", "d_zachariel_32", L("Light up the stone lantern with the Burning Stone"), L("You have collected enough Burning Stones. Find the stone lantern of the Royal Mausoleum and light it up with the Burning Stone."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_SQ_02", "d_zachariel_32", L("Protect the stone lantern of the Royal Mausoleum from the demons"), L("As the purification of the magic power became difficult, the demons rushed in to prevent it. Protect the stone lantern by defeating Clymen."));
		SetPhase(QuestStatus.Success, "ZACHA1F_SQ_02", "d_zachariel_32", L("Check the stone lantern"), L("Clymen is down. Check the stone lantern."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_SQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8428, QuestStatus.Completed));

		AddObjective("killClymen", L("Defeat Clymen"), new KillObjective(1, "boss_Clymen_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new ItemReward("TreasureboxKey2", 1));
	}
}

// 8430: Liberation of Magic (1)
//-----------------------------------------------------------------------------
public class Zacha1fSq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8430);
		SetName(L("Liberation of Magic (1)"));
		SetDescription(L("Activation stones out of the active guardians will break the mausoleum's regulators."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_SQ_03", "d_zachariel_32", L("Read the epitaph"), L("The epitaph felt like it was foretelling something that you should do from now on. You better move as written on the epitaph."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_SQ_03", "d_zachariel_32", L("Collect the activation stones to break the Regulator of the Royal Mausoleum"), L("The Regulator of the Royal Mausoleum is not needed anymore since the Revelator came into the tomb. Collect the activation stones from the Guardians nearby to destroy the Regulator of the Royal Mausoleum."));
		SetPhase(QuestStatus.Success, "ZACHA1F_SQ_03", "d_zachariel_32", L("Read the manual of the Regulator of the Royal Mausoleum"), L("You collected all the stones and gained their power. Go read the manual for the Regulator again."));

		AddPrerequisite(new LevelPrerequisite(73));

		AddPityDrop("ZACHA1F_SQ_03_ITEM", 1.0f, 0, 1, "zinutekas", "Moving_trap");

		AddObjective("collectStones", L("Obtain Activation Stones by defeating the Guardians"), new CollectItemObjective("ZACHA1F_SQ_03_ITEM", 6));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("ZACHA1F_SQ_03_ITEM"));
	}
}

// 8431: Liberation of Magic (2)
//-----------------------------------------------------------------------------
public class Zacha1fSq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8431);
		SetName(L("Liberation of Magic (2)"));
		SetDescription(L("The first Magic Regulator of the Royal Mausoleum, broken with the stones' power."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_SQ_03", "d_zachariel_32", L("Read the manual for the Regulator again"), L("You collected all the stones and gained their power. Go read the manual for the Regulator again."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_SQ_04", "d_zachariel_32", L("Destroy the Regulator of the Royal Mausoleum"), L("You can feel the power of the activation stones as you read the manual. Destroy the Regulator with this power."));
		SetPhase(QuestStatus.Success, "ZACHA1F_SQ_04", "d_zachariel_32", L("Destroy the Regulator of the Royal Mausoleum"), L("You can feel the power of the activation stones as you read the manual. Destroy the Regulator with this power."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_SQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8430, QuestStatus.Completed));

		AddObjective("breakRegulator", L("Destroy the Regulator"), new KillObjective(1, "npc_zachariel_cube_05") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The regulator is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The first regulator is rubble. One is still standing."));
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 8432: Liberation of Magic (3)
//-----------------------------------------------------------------------------
public class Zacha1fSq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8432);
		SetName(L("Liberation of Magic (3)"));
		SetDescription(L("The last Magic Regulator, and the end of the mausoleum's restraint on its own magic."));
		SetType(QuestType.Sub);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA1F_SQ_03", "d_zachariel_32", L("Read the manual for the Regulator again"), L("You've destroyed the First Regulator of the Royal Mausoleum. Go read the manual again to gain the power of the activation stones again."));
		SetPhase(QuestStatus.InProgress, "ZACHA1F_SQ_05", "d_zachariel_32", L("Destroy the Regulator of the Royal Mausoleum"), L("You can feel the power of the activation stones as you read the manual. Destroy the Regulator with this power."));
		SetPhase(QuestStatus.Success, "ZACHA1F_SQ_05", "d_zachariel_32", L("Destroy the Regulator of the Royal Mausoleum"), L("You can feel the power of the activation stones as you read the manual. Destroy the Regulator with this power."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ZACHA1F_SQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8431, QuestStatus.Completed));

		AddObjective("breakRegulator", L("Destroy the Regulator"), new KillObjective(1, "npc_zachariel_cube_05") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The regulator is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Both regulators are broken. The floor's magic runs free."));
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 60170: Vanished Glory
//-----------------------------------------------------------------------------
public class Zacha32Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60170);
		SetName(L("Vanished Glory"));
		SetDescription(L("The guardian on the ground floor wants the corrupted ones cleared out of the upper galleries."));
		SetType(QuestType.Repeat);
		SetLocation("d_zachariel_32");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ZACHA32_RP_1_NPC", "d_zachariel_32", L("Talk with the Royal Mausoleum Guardian"), L("The Royal Mausoleum Guardian is waiting for the Revelator on the ground floor."));
		SetPhase(QuestStatus.InProgress, "ZACHA32_RP_1_NPC", "d_zachariel_32", L("Defeat the nearby monsters"), L("The Royal Mausoleum Guardian asked you to deal with the corrupted ones and the monsters that breached the defenses."));
		SetPhase(QuestStatus.Success, "ZACHA32_RP_1_NPC", "d_zachariel_32", L("Talk with the Royal Mausoleum Guardian"), L("You have dealt with enough monsters. Go back to the Royal Mausoleum Guardian."));

		AddPrerequisite(new LevelPrerequisite(71));

		AddObjective("clearFloor", L("Defeat the nearby monsters"), new KillObjective(13, "zinutekas", "varv", "Moving_trap", "Karas", "zinutekas_Elite", "hogma_warrior"));

		AddReward(new ItemReward("expCard5", 1));
	}
}
