//--- Melia Script ----------------------------------------------------------
// Zachariel Crossroads Quest NPCs
//--- Description -----------------------------------------------------------
// Rexipher's bargain over Cyrenia Odell, the guardian device at Sesija
// Entrance, and the guard who lost her chest in the Traceless Ruins.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas31QuestNpcsScript : GeneralScript
{
	private readonly static QuestId PactEnd = new QuestId(9000);
	private readonly static QuestId Rexither1 = new QuestId(9001);
	private readonly static QuestId Rexither2 = new QuestId(9002);
	private readonly static QuestId Rexither3 = new QuestId(9003);
	private readonly static QuestId Sub01 = new QuestId(19370);
	private readonly static QuestId Sub02 = new QuestId(19380);
	private readonly static QuestId Sub03 = new QuestId(19390);
	private readonly static QuestId Rp1 = new QuestId(60169);

	protected override void Load()
	{
		// Rexipher, holding Cyrenia Odell
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Rexipher"), "ROKAS31_PACT_END", "f_rokas_31", 656, -1211, 90, c => !c.Quests.Has(PactEnd), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (!character.Quests.Has(PactEnd) && character.Quests.MeetsPrerequisites(PactEnd))
			{
				var answer = await dialog.SelectQuestOffer(PactEnd, L("You've come all the way here, just for a mere meddlesome woman? Do you honestly want to save this woman that much?"),
					Option(L("Where is she?!"), "accept"),
					Option(L("No"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(PactEnd);
					await dialog.Msg(L("If that's the case, become my bait and take care of the Royal Mausoleum Guardian Device at Sesija Entrance."));
					await dialog.Msg(L("I will only say this once, so you better heed carefully."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("What are you waiting for? Nothing good will come out when you drag time."));
		});

		// Rexipher, past the parvise
		//-------------------------------------------------------------------------
		AddConditionalNpc(47413, L("Rexipher"), "ROKAS31_REXITHER2", "f_rokas_31", -159.31, -501.17, 90, c => c.Quests.Has(PactEnd) && !c.Quests.HasCompleted(Rexither2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rexipher"));
			dialog.SetPortrait("Dlg_port_LEXIPER");

			if (character.Quests.IsActive(PactEnd) && character.Quests.IsCompletable(PactEnd))
			{
				await dialog.Msg(L("You still have business here?"));
				await dialog.CompleteQuest(PactEnd);
				return;
			}

			if (!character.Quests.Has(Rexither2) && character.Quests.MeetsPrerequisites(Rexither2))
			{
				var answer = await dialog.SelectQuestOffer(Rexither2, L("Oh? The woman. I forgot. I will release her as promised. Do try to keep her alive."),
					Option(L("Take Cyrenia Odell back"), "accept"),
					Option(L("Say nothing"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rexither2);
					character.Quests.StartQuestTrack(Rexither2);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(PactEnd))
			{
				await dialog.Msg(L("The device at Sesija Entrance is still standing. Nothing good will come out when you drag time."));
				return;
			}

			if (character.Quests.IsActive(Rexither2))
			{
				await dialog.Msg(L("My summon is still on its feet. Deal with it if you want her back."));
				character.Quests.ClearQuestTrack(Rexither2);
				return;
			}

			await dialog.Msg(L("Do try to keep her alive."));
		});

		// The Royal Mausoleum Guardian Device
		//-------------------------------------------------------------------------
		AddNpc(47107, L("Royal Mausoleum Guardian Device"), "ROKAS31_REXITHER1", "f_rokas_31", 57.37, -750.58, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Royal Mausoleum Guardian Device"));

			if (character.Quests.IsActive(Rexither1) && character.Quests.IsCompletable(Rexither1))
			{
				await dialog.Msg(L("Bearkaras is down and the device has gone dark. The way past Sesija Entrance is open."));
				await dialog.CompleteQuest(Rexither1);

				if (character.Quests.IsActive(PactEnd) && !character.Quests.IsCompletable(PactEnd))
				{
					character.Quests.CompleteObjective(PactEnd, "breakDevice");
					character.ServerMessage(L("The guardian device is destroyed. Rexipher is waiting past the parvise."));
				}

				return;
			}

			if (character.Quests.IsActive(Rexither1))
			{
				await dialog.Msg(L("Bearkaras is still on its feet."));
				character.Quests.ClearQuestTrack(Rexither1);
				return;
			}

			if (!character.Quests.Has(Rexither1) && character.Quests.MeetsPrerequisites(Rexither1))
			{
				await dialog.Msg(L("The Royal Mausoleum has had multiple devices to be protected from the demons."));
				await dialog.Msg(L("Humans can just pass by, but guardians can be summoned if necessary."));

				var answer = await dialog.SelectQuestOffer(Rexither1, L("If there are reasons to summon, then activate the summoning device."),
					Option(L("Activate the device"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					var worked = await character.TimeActions.StartAsync(L("Working the device..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

					if (worked != TimeActionResult.Completed)
						return;

					character.Quests.Start(Rexither1);
					character.Quests.StartQuestTrack(Rexither1);
				}
				return;
			}

			await dialog.Msg(L("A summoning device of the Royal Mausoleum, its light long gone out."));
		});

		// Historian Cyrenia Odell
		//-------------------------------------------------------------------------
		AddConditionalNpc(147345, L("Historian Cyrenia Odell"), "ROKAS31_ODEL2", "f_rokas_31", -127.13, -373.19, 0, c => c.Quests.HasCompleted(Rexither2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Cyrenia Odell"));
			dialog.SetPortrait("Dlg_port_Cyrenia_Odell");

			if (character.Quests.IsActive(Rexither2) && character.Quests.IsCompletable(Rexither2))
			{
				await dialog.Msg(L("Hmm... where is this place?"));
				await dialog.Msg(L("What about Rexipher? Where is he?"));
				await dialog.CompleteQuest(Rexither2);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Rexither3) && character.Quests.MeetsPrerequisites(Rexither3))
			{
				await dialog.Msg(L("I caused you trouble... I'm really sorry."));

				var answer = await dialog.SelectQuestOffer(Rexither3, L("What Rexipher wants is the thing the Great King Zachariel hid in the Royal Mausoleum for the goddess."),
					Option(L("I'll chase Rexipher at the Royal Mausoleum"), "accept"),
					Option(L("I still need more preparation"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rexither3);
					await dialog.Msg(L("The seals that haven't been destroyed yet at the King's Plateau will block Rexipher."));
					await dialog.Msg(L("That means you will have enough time to chase after Rexipher."));
					await dialog.Msg(L("I will pray for the goddess to bless you. You should stop him no matter what."));
				}
				return;
			}

			if (character.Quests.IsActive(Rexither3))
			{
				await dialog.Msg(L("The Royal Mausoleum entrance is up the north-western slope. Please hurry."));
				return;
			}

			await dialog.Msg(L("I will pray for the goddess to bless you."));
		});

		// The Treasure Chest of the Traceless Ruins
		//-------------------------------------------------------------------------
		AddConditionalNpc(147392, L("Treasure Chest"), "ROKAS31_SUB_01_BOX", "f_rokas_31", -736.16, -1088.93, 0, c => !c.Quests.HasCompleted(Sub01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Chest"));

			if (character.Quests.IsActive(Sub02) && !character.Quests.IsCompletable(Sub02))
			{
				await dialog.Msg(L("The chest is here, forced open and picked clean. There is nothing left inside it."));
				character.Quests.CompleteObjective(Sub02, "findChest");
				return;
			}

			if (character.Quests.IsActive(Sub01))
			{
				await dialog.Msg(L("The Hogma are all over the ruins. Clear them out."));
				return;
			}

			if (!character.Quests.Has(Sub01) && character.Quests.MeetsPrerequisites(Sub01))
			{
				var answer = await dialog.SelectQuestOffer(Sub01, L("A chest has been left standing open in the Traceless Ruins."),
					Option(L("Open the treasure chest"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sub01);
					character.ServerMessage(L("Hogma came out of the ruins the moment the lid moved. Clear them out."));
				}
				return;
			}

			await dialog.Msg(L("An emptied chest, left behind in the Traceless Ruins."));
		});

		// Powerless Security Guard
		//-------------------------------------------------------------------------
		AddNpc(147410, L("Powerless Security Guard"), "ROKAS31_SUB", "f_rokas_31", 508.94, 84.36, 4, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Powerless Security Guard"));

			if (character.Quests.IsActive(Sub02) && character.Quests.IsCompletable(Sub02))
			{
				await dialog.Msg(L("There was nothing in the chest? Aww, that's a bummer."));
				await dialog.CompleteQuest(Sub02);
				return;
			}

			if (character.Quests.IsActive(Sub03) && character.Quests.IsCompletable(Sub03))
			{
				await dialog.Msg(L("That's it! That's the necklace. I thought I would never see it again."));
				await dialog.CompleteQuest(Sub03);
				return;
			}

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("That is enough pouches to fill the order. You have saved me a whole day."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Sub02) && character.Quests.MeetsPrerequisites(Sub02))
			{
				var answer = await dialog.SelectQuestOffer(Sub02, L("I was attacked by Hogmas at Traceless Ruins. Fortunately, I was able to survive, but I need to find that chest."),
					Option(L("I'll find that chest of yours"), "accept"),
					Option(L("Just ignore it and go your way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sub02);
					await dialog.Msg(L("Thanks for caring. Be careful of those Hogmas."));
					await dialog.Msg(L("They probably haven't gone that far. I hope the necklace is safe even if others are not."));
				}
				return;
			}

			if (!character.Quests.Has(Sub03) && character.Quests.MeetsPrerequisites(Sub03))
			{
				var answer = await dialog.SelectQuestOffer(Sub03, L("The chest can stay lost, but not the necklace. A Hogma Captain is carrying it somewhere out there."),
					Option(L("Do not worry, I'll collect them"), "accept"),
					Option(L("Take care of such things by yourself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sub03);
					character.Inventory.Add(ItemId.ROKAS31_SUB_03_SCROLL, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Press the V key and the Hogma Captain carrying it will stand out from the rest."));
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("I'm sorry, but could you help me out a bit? I wasn't able to gather more than half of the materials required for my mission."),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("Do it yourself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Rp1);
					await dialog.Msg(L("I'm pretty sure that anyone asking for these are mages or alchemists..."));
					await dialog.Msg(L("I'd like to tell them to try gathering this stuff themselves..."));
				}
				return;
			}

			if (character.Quests.IsActive(Sub02))
			{
				await dialog.Msg(L("The Traceless Ruins are south-west of the crossroads."));
				return;
			}

			if (character.Quests.IsActive(Sub03))
			{
				await dialog.Msg(L("A Hogma Captain has it. Press V and you will see which one."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("Nine dirty pouches. Anything the monsters around the crossroads are carrying will do."));
				return;
			}

			await dialog.Msg(L("One guard on a crossroads this size. Whoever drew up that roster has never stood on it."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The slope up to the Royal Mausoleum entrance.
		AddQuestTrigger("ROKAS31_REXITHER3TRACK", "f_rokas_31", -1100, 280, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Rexither3) && !character.Quests.IsCompletable(Rexither3))
				character.Quests.StartQuestTrack(Rexither3);

			await Task.CompletedTask;
		});

		// The Royal Mausoleum entrance itself.
		AddQuestTrigger("ROKAS31_ZACHARIEL32_ENTER", "f_rokas_31", -1271, 715, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Rexither3) && character.Quests.IsCompletable(Rexither3))
			{
				character.ServerMessage(L("Rexipher went through the Royal Mausoleum entrance ahead of you."));
				character.Quests.Complete(Rexither3);
			}

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 9000: Negotiation (1)
//-----------------------------------------------------------------------------
public class Rokas31PactEndQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9000);
		SetName(L("Negotiation (1)"));
		SetDescription(L("Rexipher will trade Cyrenia Odell for the guardian device at Sesija Entrance."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_PACT_END", "f_rokas_31", L("Trace Rexipher"), L("Rexipher kidnapped the historian, Cyrenia Odell. Go follow Rexipher to the Zachariel Crossroads."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_REXITHER1", "f_rokas_31", L("Destroy the Royal Mausoleum Guardian Device at Sesija Entrance"), L("Rexipher wants you to obey him in return for keeping Cyrenia Odell alive. Take the offer first and destroy the protective device at Sesija Entrance."));
		SetPhase(QuestStatus.Success, "ROKAS31_REXITHER2", "f_rokas_31", L("Talk to Rexipher"), L("Destroyed the device in the Royal Mausoleum as Rexipher wanted. Talk to him again."));

		AddPrerequisite(new QuestStatusPrerequisite(20196, QuestStatus.Completed));

		AddObjective("breakDevice", L("Destroy the Royal Mausoleum Guardian Device at Sesija Entrance"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 9001: Beholder of Sesija Entrance
//-----------------------------------------------------------------------------
public class Rokas31Rexither1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9001);
		SetName(L("Beholder of Sesija Entrance"));
		SetDescription(L("Working the summoning device at Sesija Entrance calls its guardian up."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_REXITHER1", "f_rokas_31", L("Beholder of Sesija Entrance"), L("The Royal Mausoleum has had multiple devices to be protected from the demons. Humans can just pass by but guardians can be summoned if necessary. If there are reasons to summon, then activate the summoning device."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_REXITHER1", "f_rokas_31", L("Defeat the Royal Mausoleum Guardian at Sesija Entrance"), L("Bearkaras appeared when you activated the device. Defeat it."));
		SetPhase(QuestStatus.Success, "ROKAS31_REXITHER1", "f_rokas_31", L("Check the device again"), L("Bearkaras is down. Check the device again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS31_REXITHER1_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(68));

		AddObjective("killBearkaras", L("Defeat Bearkaras"), new KillObjective(1, "boss_bearkaras_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("BRC02_113", 1));
	}
}

// 9002: Negotiation (2)
//-----------------------------------------------------------------------------
public class Rokas31Rexither2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9002);
		SetName(L("Negotiation (2)"));
		SetDescription(L("Rexipher hands Cyrenia Odell back, and a Cactusvel with her."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_REXITHER2", "f_rokas_31", L("Trace Rexipher"), L("After you defeated the guardian of the Royal Mausoleum at Sesija Entrance as Rexipher ordered, Rexipher disappeared through the parvise. To find Cyrenia Odell, you should go through the parvise after Rexipher."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_REXITHER2", "f_rokas_31", L("Defeat the Cactusvel summoned by Rexipher"), L("Rexipher says he will spare Cyrenia Odell's life but... Rexipher's summon suddenly attacked!"));
		SetPhase(QuestStatus.Success, "ROKAS31_ODEL2", "f_rokas_31", L("Check Cyrenia Odell's safety"), L("Barely defeated Cactusvel. Check if Cyrenia Odell is safe."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS31_REXITHER2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(9000, QuestStatus.Completed));

		AddObjective("killCactusvel", L("Defeat Rexipher's summon"), new KillObjective(1, "boss_cactusvel") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
	}
}

// 9003: Swindler Rexipher
//-----------------------------------------------------------------------------
public class Rokas31Rexither3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9003);
		SetName(L("Swindler Rexipher"));
		SetDescription(L("Rexipher is after what Zachariel hid in the Royal Mausoleum. Follow him in."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_ODEL2", "f_rokas_31", L("Talk to Historian Cyrenia Odell"), L("Tell Cyrenia Odell about what happened."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_REXITHER3TRACK", "f_rokas_31", L("Pursue Rexipher at the Royal Mausoleum Entrance"), L("Cyrenia Odell says Rexipher's objective is the thing hidden in the Royal Mausoleum by the Great King Zachariel for the goddess. Follow Rexipher into the Royal Mausoleum."));
		SetPhase(QuestStatus.Success, "ROKAS31_ZACHARIEL32", "f_rokas_31", L("Pursue Rexipher at the Royal Mausoleum Entrance"), L("Cyrenia Odell says Rexipher's objective is the thing hidden in the Royal Mausoleum by the Great King Zachariel for the goddess. Follow Rexipher into the Royal Mausoleum."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS31_REXITHER3_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(9002, QuestStatus.Completed));

		AddObjective("killServants", L("Defeat Rexipher's servants"), new KillObjective(6, "hogma_warrior", "hogma_sorcerer", "warleader_hogma") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 19370: Hogma's Treasure Chest
//-----------------------------------------------------------------------------
public class Rokas31Sub01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19370);
		SetName(L("Hogma's Treasure Chest"));
		SetDescription(L("Opening the chest in the Traceless Ruins brings the Hogma of the crossroads down on it."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_SUB_01_BOX", "f_rokas_31", L("Treasure Chest of Zachariel Crossroads"), L("Open the treasure chest of Zachariel Crossroads."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_SUB_01_BOX", "f_rokas_31", L("Ambush of Hogmas"), L("Hogmas appeared when opening the Treasure Chest! Defeat them."));
		SetPhase(QuestStatus.Success, "ROKAS31_SUB_01_BOX", "f_rokas_31", L("Ambush of Hogmas"), L("Hogmas appeared when opening the Treasure Chest! Defeat them."));

		AddPrerequisite(new LevelPrerequisite(68));

		// The client's cutscene for this quest spawns no cast, so the kills are
		// counted on the field rather than inside a private layer.
		AddObjective("killHogma", L("Defeat the Hogmas"), new KillObjective(20, "hogma_warrior", "hogma_archer", "hogma_sorcerer", "warleader_hogma", "Hogma_combat", "Hogma_guard"));

		AddReward(new ItemReward("expCard5", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The ruins are clear. Whatever was in the chest is long gone."));
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 19380: Security Guard's Favor (1)
//-----------------------------------------------------------------------------
public class Rokas31Sub02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19380);
		SetName(L("Security Guard's Favor (1)"));
		SetDescription(L("The guard lost the chest she was escorting in the Traceless Ruins."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_SUB", "f_rokas_31", L("Talk to the Security Guard"), L("The Security Guard seems to be in trouble. Talk to her."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_SUB_01_BOX", "f_rokas_31", L("Find the treasure chest in the Traceless Ruins"), L("The Security Guard seems to have been chased by the monsters until here. Find the treasure chest that the Security Guard was protecting in the Traceless Ruins."));
		SetPhase(QuestStatus.Success, "ROKAS31_SUB", "f_rokas_31", L("Talk to the Security Guard"), L("Found the Treasure Chest but it is empty and you just met a lot of Hogmas. Tell the Security Guard about it."));

		AddPrerequisite(new LevelPrerequisite(68));

		AddObjective("findChest", L("Find the treasure chest in the Traceless Ruins"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 19390: Security Guard's Favor (2)
//-----------------------------------------------------------------------------
public class Rokas31Sub03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19390);
		SetName(L("Security Guard's Favor (2)"));
		SetDescription(L("A Hogma Captain is carrying the necklace the guard was escorting."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_SUB", "f_rokas_31", L("Look for the Security Guard"), L("Told her that the treasure chest was empty. But the Security Guard still seems like she needs more help. Talk to her."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_SUB", "f_rokas_31", L("Find the necklace from the Hogmas"), L("When you press the V key, the body of the Hogma Captain with the necklace will shine. Find the necklace and give it to the Security Guard."));
		SetPhase(QuestStatus.Success, "ROKAS31_SUB", "f_rokas_31", L("Bring the necklace"), L("Found the necklace the Security Guard was looking for. Bring it to the Security Guard."));

		AddPrerequisite(new QuestStatusPrerequisite(19380, QuestStatus.Completed));

		AddPityDrop("ROKAS31_SUB_03_CERT", 1.0f, 0, 1, "warleader_hogma");

		AddObjective("takeNecklace", L("Obtain the Shabby-Looking Necklace"), new CollectItemObjective("ROKAS31_SUB_03_CERT", 1));

		AddReward(new ItemReward("expCard5", 3));
		AddReward(new ItemReward("Drug_SP2_Q", 30));
		AddReward(new TakeItemReward("ROKAS31_SUB_03_SCROLL"));
		AddReward(new TakeItemReward("ROKAS31_SUB_03_CERT"));
	}
}

// 60169: Too Much For One Person
//-----------------------------------------------------------------------------
public class Rokas31Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60169);
		SetName(L("Too Much For One Person"));
		SetDescription(L("The guard is short of the dirty pouches her order calls for."));
		SetType(QuestType.Repeat);
		SetLocation("f_rokas_31");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS31_SUB", "f_rokas_31", L("Talk to the Tired Soldier"), L("The Tired Soldier is waiting for help near Zachariel Crossroads."));
		SetPhase(QuestStatus.InProgress, "ROKAS31_SUB", "f_rokas_31", L("Obtain Dirty Pouches"), L("The Tired Soldier says that he has been ordered to obtain dirty pouches from the monsters. Defeat the monsters near Zachariel Crossroads and bring back Filthy Parcels that may contain something of worth."));
		SetPhase(QuestStatus.Success, "ROKAS31_SUB", "f_rokas_31", L("Report to the Tired Soldier"), L("You have collected a sufficient amount of dirty pouches. Go back to the Tired Soldier."));

		AddPrerequisite(new LevelPrerequisite(68));

		AddPityDrop("ROKAS31_RP_1_ITEM", 0.85f, 2, 1, "warleader_hogma", "Tontulia", "Repusbunny_mage");

		AddObjective("collectPouches", L("Collect Dirty Pouches"), new CollectItemObjective("ROKAS31_RP_1_ITEM", 9));

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new TakeItemReward("ROKAS31_RP_1_ITEM"));
	}
}
