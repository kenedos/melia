//--- Melia Script ----------------------------------------------------------
// Ashaq Underground Prison 3F Quest NPCs
//--- Description -----------------------------------------------------------
// The confession altars, Marnox's ambush over the Demon Orders, and the
// inmates' notes hidden across the prison.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison623QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Prison622Mq06 = new QuestId(60131);
	private readonly static QuestId Mq01 = new QuestId(60136);
	private readonly static QuestId Mq02 = new QuestId(60137);
	private readonly static QuestId Mq03 = new QuestId(60138);
	private readonly static QuestId Mq05 = new QuestId(60139);
	private readonly static QuestId Mq06 = new QuestId(60140);
	private readonly static QuestId Sq01 = new QuestId(60142);
	private readonly static QuestId Sq02 = new QuestId(60143);
	private readonly static QuestId Sq03 = new QuestId(60144);
	private readonly static QuestId Sq04 = new QuestId(30040);
	private readonly static QuestId Rp1 = new QuestId(60154);
	private readonly static QuestId Hq1 = new QuestId(50272);
	private readonly static QuestId OrshaMq3_01 = new QuestId(60145);

	private const string PouchVar = "Gabija.Quests.Prison623Sq02.Pouch";
	public const string PurifiedCountVar = "Gabija.Quests.Prison623Sq03.Purified";
	private const string FragmentVar = "Gabija.Quests.Prison623Sq03.Fragment";
	private const string NoteCodeVar = "Gabija.Quests.Prison622Hq1.Code";

	private const int FragmentsNeeded = 11;

	private static readonly Position HiddenRoomEntry = new Position(916.68f, 997.54f, 691.16f);
	private static readonly Position HiddenRoomExit = new Position(885.34f, 981.91f, 400f);

	private static readonly double[,] Pouches =
	{
		{ 1648.72, -114.95 }, { 1861.48, -94.59 }, { 1764.09, 190.33 }, { 1481.25, 135.88 }, { 1619.59, 325.32 },
		{ 1624.52, 485.88 }, { 1649.26, 699.22 }, { 1616.14, 966.43 }, { 1624.55, 1187.63 }, { 1642.76, 1411.16 },
		{ 1919.79, 1308.03 }, { 1955.35, 1098.01 }, { 1924.69, 944.51 }, { 1798.71, 951.22 }, { 1509.55, -292.70 },
	};

	private static readonly double[,] Fragments =
	{
		{ 1725.79, -1856.39 }, { 2172.58, -1470.17 }, { 1718.84, -1294.52 }, { 2145.03, -989.25 }, { 1726.87, -500.76 },
		{ 1821.18, -180.78 }, { 1514.07, -147.66 }, { 1634.10, 189.02 }, { 1638.14, 705.88 }, { 1679, 1023.80 },
		{ 1902.90, 1196.88 }, { 1651.16, 1373.74 }, { 34.47, -1830.91 }, { 297.01, -1638.99 }, { -293.14, -1577.47 },
		{ 202.45, -1195.56 }, { -42.58, -412.61 }, { 194.63, -529.92 }, { 246.36, -234.83 }, { 71.06, 216.57 },
		{ 253.76, 739.22 }, { -268.75, 600.94 }, { -67.62, 781.44 }, { -336.89, 1437.13 }, { -195.07, 1642.80 },
		{ -314.97, 2307.76 }, { -68.89, 2326.33 }, { -874.96, 790.20 }, { -1041.57, 1024.58 },
	};

	private static readonly double[,] CageBars =
	{
		{ 153103, 1726.38, -144.82, 0 }, { 153104, 1525.98, 165.84, 0 }, { 151099, 1614.74, 30.09, 0 }, { 151099, 1655.46, 92.80, 90 },
		{ 151099, 1614.69, -22.87, 180 }, { 151100, 1614.37, 77.65, 0 }, { 151100, 1750.72, 74.29, 180 }, { 151100, 1707.47, 96.15, 90 },
		{ 151101, 1750.84, -12.73, 180 }, { 151101, 1682.78, -59.46, 90 },
	};

	protected override void Load()
	{
		// Priest Irma
		//-------------------------------------------------------------------------
		AddConditionalNpc(156006, L("Priest Irma"), "PRISON623_IRMA_01", "d_prison_62_3", 855.15, 412.40, 6, c => c.Quests.Has(Prison622Mq06) && !c.Quests.Has(Mq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Irma"));

			if (character.Quests.IsCompletable(Prison622Mq06))
			{
				await dialog.Msg(L("Priest Gelija is safe! Now I should focus on finding the Demon Orders."));
				await dialog.Msg(L("But... Oh, I forgot that you probably won't be able to sense it."));
				await dialog.Msg(L("I think there's a cursed idol here as well. The problem is that we don't have any Orbs of Return left."));
				await dialog.Msg(L("Feeling as if my body is becoming heavier is one thing... But we cannot open the portal to the Penitence Room where the Demon Orders is because of the cursed idol."));
				await dialog.CompleteQuest(Prison622Mq06);
				return;
			}

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("The demons seemed to have noticed something is happening. But the cursed idol still stands..."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You've succeeded! I feel the curse being lifted."));
				await dialog.Msg(L("Now, the cursed idols should be nothing more than mere pieces of wood!"));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("I thought it was strange that the demons weren't reacting... They've been preparing there all along."));
				await dialog.Msg(L("But Marnox still didn't come back. Demons will never be able to open the chest that holds the Demon Orders, so where else could he be?"));
				await dialog.Msg(L("I guess he might be planning his next move while he sends his servants. I'll open the portal since we can't afford to tarry any longer."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Do you... by any chance remember? I'm sure that the bishop would have mentioned it."));
				await dialog.Msg(L("If you really are the Revelator, you will be able to stand against Demon Lords with your strength."));
				await dialog.Msg(L("The reason we chose this place to hide the Demon Orders is because of the Confession Altar. It's the altar that the goddess gave her final blessing to the inmates that made their confessions."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("It's been a while since the prison was shut down, but the effects from the blessings are still there. Your powers as Revelator should be enough to liberate that energy."),
					Option(L("I will try"), "accept"),
					Option(L("I'm afraid that'll be impossible"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("The cursed idols must be destroyed in order for me to open the portal to the Penitence Room. I can't do a thing because of this heavy and blasphemous energy."));
					await dialog.Msg(L("There are two Confession Altars... But let's try the one on the left since it is closer."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("I told you there were two altars. Now all we can do is hope that the one in the oratory will do it's job."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("I pray that the goddess look over us and grant us with success..."),
					Option(L("I'll go there"), "accept"),
					Option(L("I need to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Please be careful that Marnox doesn't notice you. That arrogant Demon Lord will not look on as he lost me."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("You know what's strange? Marnox wouldn't leave us to interrupt his plans like this..."));
				await dialog.Msg(L("I guess it's all for the best. Maybe he underestimated you and got caught off guard."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("Let's not let down the pace and go destroy the cursed idol in the 2nd Watchtower. I'll get the portal to the Penitence Room ready."),
					Option(L("I'll go there"), "accept"),
					Option(L("I'm going to get some rest and go"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Please be right behind me when I open the portal. Let's head back to the bishop as soon as we have the Demon Orders."),
					Option(L("I am ready"), "accept"),
					Option(L("Tell him to wait a bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("I am grateful that everything seems to be going well ever since I've met you... But I am also nervous at the same time. Especially about the fact that Marnox is so quiet."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Please be careful that Marnox doesn't notice you. That arrogant Demon Lord will not look on as he lost me."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("I won't forget the arrogant gestures and expressions Marnox used when he caught me. He said that he could crush the Revelator or anything else that stands in his way..."));
				character.Quests.ClearQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("I won't forget the arrogant gestures and expressions Marnox used when he caught me. He said that he could crush the Revelator or anything else that stands in his way..."));
		});

		AddConditionalNpc(156006, L("Priest Irma"), "PRISON623_IRMA_02", "d_prison_62_3", 968.07, 930.36, -1, IsIrmaInThePenitenceRoom, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Irma"));

			if (character.Quests.IsCompletable(Mq05))
			{
				if (character.Inventory.CountItem(ItemId.PRISON623_MQ_05_ITEM) == 0)
				{
					await dialog.Msg(L("The Demon Orders are in the sealed chest. Please take them before anything else."));
					return;
				}

				await dialog.Msg(L("You've defeated Marnox... Euah..."));
				await dialog.Msg(L("I think my wounds are quite deep..."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("It was an honor that I could see the church achieve its mission. It's all thanks to you."));

				var answer = await dialog.SelectQuestOffer(Mq06, L("Don't worry about me, take the Demon Orders to the bishop. I'm a priest, remember? I'll be fine if I get to rest a little."),
					Option(L("I will do that"), "accept"),
					Option(L("I can't just go"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					character.Quests.CompleteObjective(Mq06, "deliverOrders");
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("I'm really okay. I'll recover my strength if I rest here a while, so please go and give the Demon Orders to the bishop. Don't forget to give him my regards."));
				return;
			}

			if (character.Quests.HasCompleted(Mq06))
			{
				await dialog.Msg(L("Oh, thank you for helping out back then. You saved me from Marnox and even managed to retrieve the Demon Orders..."));
				await dialog.Msg(L("I feel much better now. I'm thinking of staying at the Ashaq Underground Prison for a while and driving the demons out of it."));
				await dialog.Msg(L("I hope we get a chance to meet again. May the goddesses bless the path you choose..."));
				return;
			}

			await dialog.Msg(L("All the wounds have yet to heal. We will leave when we are able to walk."));
		});

		// Priest Gelija
		//-------------------------------------------------------------------------
		AddConditionalNpc(156007, L("Priest Gelija"), "PRISON623_GELIYA", "d_prison_62_3", 691.59, 112.62, 90, c => c.Quests.Has(Prison622Mq06) && !c.Quests.HasCompleted(OrshaMq3_01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Gelija"));

			if (character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Thank you! I'll go straight to Orsha to research this subject as soon as things are settled here."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Thank you for getting me my missing bag. Right after I heal myself I have to go to the other priests."));
				await dialog.CompleteQuest(Sq02);
				character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("One less thing to worry about. Restoring this place will not be an easy task."));
				await dialog.CompleteQuest(Sq03);
				character.LookAround();
				return;
			}

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("I think I just heard them. Are you sure you got rid of all of them?"));
				await dialog.Msg(L("They were so many; there's no way, huh... Still, thanks for clearing them out a little..."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("Until now, we thought of the demons below Demon Lords... Little more than larger and more vicious monsters."));
				await dialog.Msg(L("We never thought that they would have such a detailed command structure and threaten us like this. We will have to research their command structure in more detail to respond to them more effectively."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("If you ever have the chance to face the demons at Felon Prison... Could you possibly collect Marnox's Insignias for me?"),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq01);

				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				await dialog.Msg(L("I'm wounded myself... But I'm more worried about the condition the other priests are in."));
				await dialog.Msg(L("I brought some potions and bandages in case something like this happened... But I had to leave them behind since we were chased by the demons."));

				var answer = await dialog.SelectQuestOffer(Sq02, L("I know it's a lot to ask, but you're the only one that can help. Could you find my medicine bags so I can cure the other priests?"),
					Option(L("I will go look for it"), "accept"),
					Option(L("I think it's best to get out of the prison"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Pouches.GetLength(0); ++i)
						character.Variables.Perm.Set(PouchVar + i, false);

					character.Quests.Start(Sq02);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("All the idols that can be destroyed have been destroyed but it was a rushed job... I am still hung up about the fragments left behind."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("The fragments could come back to bite us in the back, if not its energy remains. I am terribly sorry about this but could you take care of the remaining pieces?"),
					Option(L("I'll purify it"), "accept"),
					Option(L("Don't worry so much"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Fragments.GetLength(0); ++i)
						character.Variables.Perm.Set(FragmentVar + i, false);
					character.Variables.Perm.SetInt(PurifiedCountVar, 0);

					character.Quests.Start(Sq03);
					character.Inventory.Add(ItemId.PRISON623_SQ_03_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await dialog.Msg(L("I am grateful for your kindness. Wait for a second for I will give you a piece of cloth soaked with Holy Water."));
					await dialog.Msg(L("Cover the Idol Fragments with this Holy Water Cloth and it will be purified."));
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The fragments of the destroyed idol are scattered around the 3rd floor.{nl}Find the fragments and purify them!"), 10);
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("Do you know what's harder than running from Marnox? Trying to hide among the Varvs at the Felon Prison. I want you to just get rid of all the Varvs you can find."));

				var answer = await dialog.SelectQuestOffer(Rp1, L("If you see any Varvs, just get rid of them."),
					Option(L("I'll help you"), "accept"),
					Option(L("I don't want to get my hands dirty"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Rp1);

				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("The entire church was swayed by a single Demon Lord... How powerful do you think a Demon King or Demon Goddess might be?"));
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Not all priests can heal by miraculous powers. We have to put a stick next to the broken part and get medicine for the injury."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Rushing anything is bound to end in trouble eventually."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("If I get out of here, I can talk proudly of having faced Marnox. But the thing with the Varvs is not something I want to talk about over dinner, or ever, to be honest."));
				return;
			}

			if (character.Quests.HasCompleted(Mq05))
			{
				await dialog.Msg(L("It's a relief seeing Irma safe after being captured by Marnox. Now all we have to do is retrieve the Demon Orders."));
				return;
			}

			await dialog.Msg(L("I shudder to think that I was almost captured by demons. You... Does the curse not affect you at all?"));
		});

		// Confession Altars
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Confession Altar"), "PRISON623_MQ_01_NPC", "d_prison_62_3", 88.51, -424.20, 46, async dialog =>
		{
			await this.ReleaseAltar(dialog, Mq01);
		});

		AddNpc(147357, L("Confession Altar"), "PRISON623_MQ_02_NPC", "d_prison_62_3", -889.20, 908.99, 50, async dialog =>
		{
			await this.ReleaseAltar(dialog, Mq02);
		});

		// Cursed Idol at the Second Watchtower
		//-------------------------------------------------------------------------
		AddConditionalNpc(47150, L("Cursed Idol"), "PRISON623_MQ_03_NPC", "d_prison_62_3", -26.79, -907.05, -23, c => c.Quests.IsActive(Mq03) && !c.Quests.IsCompletable(Mq03), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03))
				return;

			var destroyed = await character.TimeActions.StartAsync(L("Destroying..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));
			if (destroyed != TimeActionResult.Completed)
				return;

			character.Quests.StartQuestTrack(Mq03);
		});

		// The Penitence Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(154069, L("Hidden Room"), "PRISON623_TO_PRISON623_1", "d_prison_62_3", 900, 451, 90, c => c.Quests.Has(Mq05), async dialog =>
		{
			dialog.Player.Warp("d_prison_62_3", HiddenRoomEntry.X, HiddenRoomEntry.Y, HiddenRoomEntry.Z);
			await Task.CompletedTask;
		});

		AddConditionalNpc(154069, L("Ashaq Underground Prison 3F"), "PRISON623_1_TO_PRISON623", "d_prison_62_3", 930, 674, 90, c => c.Quests.Has(Mq05), async dialog =>
		{
			dialog.Player.Warp("d_prison_62_3", HiddenRoomExit.X, HiddenRoomExit.Y, HiddenRoomExit.Z);
			await Task.CompletedTask;
		});

		AddConditionalNpc(45324, L("Sealed Chest"), "PRISON623_MQ_05_NPC", "d_prison_62_3", 991.58, 997.43, 0, c => c.Quests.Has(Mq05) && !c.Quests.HasCompleted(Mq06), async dialog =>
		{
			var character = dialog.Player;

			var needsOrders = character.Quests.IsCompletable(Mq05) || character.Quests.IsActive(Mq06);
			if (!needsOrders || character.Inventory.CountItem(ItemId.PRISON623_MQ_05_ITEM) > 0)
				return;

			character.Inventory.Add(ItemId.PRISON623_MQ_05_ITEM, 1, InventoryAddType.PickUp);
			await Task.CompletedTask;
		});

		// The Cerberus cage
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Red Energy"), "PRISON623_SQ_04_OBJ", "d_prison_62_3", 1428.69, 163.72, 90, c => !c.Quests.Has(Sq04), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.Has(Sq04) || !character.Quests.MeetsPrerequisites(Sq04))
				return;

			var looked = await character.TimeActions.StartAsync(L("Examining..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));
			if (looked != TimeActionResult.Completed)
				return;

			character.Quests.Start(Sq04);
			character.LookAround();
		});

		for (var i = 0; i < CageBars.GetLength(0); ++i)
			AddConditionalNpc((int)CageBars[i, 0], "UnvisibleName", "PRISON_62_3_KERBEROS_BARS_" + (i + 1), "d_prison_62_3", CageBars[i, 1], CageBars[i, 2], CageBars[i, 3], c => !c.Quests.Has(Sq04));

		// Priest Gelija's Medicine Pouches
		//-------------------------------------------------------------------------
		for (var i = 0; i < Pouches.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47160, L("Medicine Pouch"), "PRISON623_SQ_02_NPC_" + number, "d_prison_62_3", Pouches[i, 0], Pouches[i, 1], 90,
				character => character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02) && !character.Variables.Perm.GetBool(PouchVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq02) || character.Quests.IsCompletable(Sq02) || character.Variables.Perm.GetBool(PouchVar + number, false))
						return;

					character.Variables.Perm.Set(PouchVar + number, true);
					character.Inventory.Add(ItemId.PRISON623_SQ_02_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Destroyed Idol Fragments
		//-------------------------------------------------------------------------
		for (var i = 0; i < Fragments.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(154066, L("Destroyed Idol Fragment"), "PRISON623_SQ_03_NPC_" + number, "d_prison_62_3", Fragments[i, 0], Fragments[i, 1], 90,
				character => character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03) && !character.Variables.Perm.GetBool(FragmentVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq03) || character.Quests.IsCompletable(Sq03) || character.Variables.Perm.GetBool(FragmentVar + number, false))
						return;

					if (character.Inventory.CountItem(ItemId.PRISON623_SQ_03_ITEM) == 0)
						return;

					character.Variables.Perm.Set(FragmentVar + number, true);
					var purified = character.Variables.Perm.GetInt(PurifiedCountVar, 0) + 1;
					character.Variables.Perm.SetInt(PurifiedCountVar, purified);

					dialog.Npc.PlayEffect("F_light018_yellow", 1f);
					character.ServerMessage(LF("Idol fragments purified: {0}/{1}", Math.Min(purified, FragmentsNeeded), FragmentsNeeded));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// The inmates' notes
		//-------------------------------------------------------------------------
		AddNpc(20025, L("Inmate Note"), "PRISON622_HIDDEN_OBJ1", "d_prison_62_1", 1324.18, -52.01, 90, async dialog => await this.ReadNote(dialog, 1));
		AddNpc(20025, L("Inmate Note"), "PRISON622_HIDDEN_OBJ2", "d_prison_62_2", 77.81, 1389.59, 90, async dialog => await this.ReadNote(dialog, 2));
		AddNpc(20025, L("Inmate Note"), "PRISON622_HIDDEN_OBJ3", "d_prison_62_2", -1841.87, 1872.64, 90, async dialog => await this.ReadNote(dialog, 3));
		AddNpc(20025, L("Inmate Note"), "PRISON622_HIDDEN_OBJ4", "d_prison_62_3", -454.08, -852.58, 90, async dialog => await this.ReadNote(dialog, 4));
		AddNpc(20025, L("Inmate Note"), "PRISON622_HIDDEN_OBJ5", "d_prison_62_3", 1541.09, 1262.88, 90, async dialog => await this.ReadNote(dialog, 5));

		// Locking Device
		//-------------------------------------------------------------------------
		AddNpc(20025, L("Locking Device"), "PRISON622_HIDDEN_OBJ6", "d_prison_62_3", 1979.04, -778.98, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Device Note"));

			if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
			{
				await dialog.Msg(L("Good find. You read all my notes, right?"));

				var answer = await dialog.SelectQuestOffer(Hq1, L("Put the amount of all the foods we ate in order: that's the code to open the locking device."),
					Option(L("I'll open it."), "accept"),
					Option(L("Just leave it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq1);
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The box has a locking device.{nl}Find a way to unlock it."), 7);
				}
				return;
			}

			if (!character.Quests.IsActive(Hq1))
				return;

			var code = await dialog.Input(L("Put the amount of all the foods we ate in order: that's the code to open the locking device."));
			if (string.IsNullOrWhiteSpace(code) || code.Trim() != character.Variables.Perm.GetString(NoteCodeVar, ""))
			{
				character.ServerMessage(L("The locking device doesn't open."));
				return;
			}

			character.Quests.CompleteObjective(Hq1, "unlock");

			var opened = await character.TimeActions.StartAsync(L("Picking up the things inside the box..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));
			if (opened != TimeActionResult.Completed)
				return;

			await dialog.CompleteQuest(Hq1);
		});
	}

	/// <summary>
	/// Returns whether Priest Irma rests in the Penitence Room after the
	/// fight with Marnox.
	/// </summary>
	private static bool IsIrmaInThePenitenceRoom(Character character)
		=> (character.Quests.IsCompletable(Mq05) || character.Quests.HasCompleted(Mq05)) && !character.Quests.HasCompleted(OrshaMq3_01);

	/// <summary>
	/// Releases the blessing of one of the Confession Altars.
	/// </summary>
	private async Task ReleaseAltar(Dialog dialog, QuestId questId)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(questId, "releaseAltar"))
			return;

		dialog.Npc.PlayEffect("F_light018_yellow", 1.5f);
		character.Quests.CompleteObjective(questId, "releaseAltar");

		await Task.CompletedTask;
	}

	/// <summary>
	/// Hands over one of the inmates' notes, in the order the notes lead
	/// to each other.
	/// </summary>
	private async Task ReadNote(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (character.Quests.HasCompleted(Hq1) || character.Quests.IsActive(Hq1))
			return;

		var itemId = NoteItemId(number);
		if (character.Inventory.CountItem(itemId) > 0)
		{
			ShowNote(character, number);
			return;
		}

		if (number > 1 && character.Inventory.CountItem(NoteItemId(number - 1)) == 0)
			return;

		if (number == 1)
		{
			var code = "";
			for (var i = 0; i < 4; ++i)
				code += System.Random.Shared.Next(1, 10);

			character.Variables.Perm.SetString(NoteCodeVar, code);
		}

		character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
		ShowNote(character, number);

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns the item id of the given inmate note.
	/// </summary>
	private static int NoteItemId(int number)
	{
		switch (number)
		{
			case 1: return ItemId.PRISON622_HIDDENQ1_ITEM1;
			case 2: return ItemId.PRISON622_HIDDENQ1_ITEM2;
			case 3: return ItemId.PRISON622_HIDDENQ1_ITEM3;
			case 4: return ItemId.PRISON622_HIDDENQ1_ITEM4;
			default: return ItemId.PRISON622_HIDDENQ1_ITEM5;
		}
	}

	/// <summary>
	/// Shows the text of the given inmate note, with the amounts the
	/// character's code is made of.
	/// </summary>
	private static void ShowNote(Character character, int number)
	{
		if (number == 1)
		{
			Send.ZC_NORMAL.ShowBook(character, "PRISON622_HIDDEN_OBJ1");
			return;
		}

		var code = character.Variables.Perm.GetString(NoteCodeVar, "1111");
		if (code.Length < 4)
			code = "1111";

		Send.ZC_NORMAL.ShowBook(character, "PRISON622_HIDDEN_OBJ" + (number - 1) + "_" + code[number - 2]);
	}

	/// <summary>
	/// Reads inmate note 2.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_UES_PRISON622_HIDDEN_ITEM_BOOK2(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		ShowNote(character, 2);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads inmate note 3.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_UES_PRISON622_HIDDEN_ITEM_BOOK3(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		ShowNote(character, 3);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads inmate note 4.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_UES_PRISON622_HIDDEN_ITEM_BOOK4(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		ShowNote(character, 4);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Reads inmate note 5.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_UES_PRISON622_HIDDEN_ITEM_BOOK5(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		ShowNote(character, 5);
		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30040: Prison Scout
//-----------------------------------------------------------------------------
public class Prison623Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30040);
		SetName(L("Prison Scout"));
		SetDescription(L("Some areas in Ashaq Underground Prison 3F are glowing with an eery red energy. Go and have a look at them."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_SQ_04_OBJ", "d_prison_62_3", L("Investigate the Red Energy"));
		SetPhase(QuestStatus.InProgress, "PRISON623_SQ_04_OBJ", "d_prison_62_3", L("Defeat Cerberus"));
		SetPhase(QuestStatus.Success, "PRISON623_SQ_04_OBJ", "d_prison_62_3", L("Defeat Cerberus"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "d_prison_62_3_kerberos", "m_boss_c", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(15));

		AddObjective("killCerberus", L("Defeat Cerberus"), new KillObjective(1, "boss_Kerberos_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; slaying the Cerberus ends the quest.
		character.Quests.Complete(this.QuestId);
	}
}

// 50272: The Inmates' Secret Spots
//-----------------------------------------------------------------------------
public class Prison622Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50272);
		SetName(L("The Inmates' Secret Spots"));
		SetDescription(L("You followed the suspicious notes and found a locking device. Read the notes carefully and enter the code to unlock the device."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON622_HIDDEN_OBJ6", "d_prison_62_3", L("Open the Inmates' Hidden Locking Device"));
		SetPhase(QuestStatus.InProgress, "PRISON622_HIDDEN_OBJ6", "d_prison_62_3", L("Open the Locking Device"));
		SetPhase(QuestStatus.Success, "PRISON622_HIDDEN_OBJ6", "d_prison_62_3", L("Open the Locking Device"));

		AddPrerequisite(new ItemPrerequisite("PRISON622_HIDDENQ1_ITEM1"));
		AddPrerequisite(new ItemPrerequisite("PRISON622_HIDDENQ1_ITEM2"));
		AddPrerequisite(new ItemPrerequisite("PRISON622_HIDDENQ1_ITEM3"));
		AddPrerequisite(new ItemPrerequisite("PRISON622_HIDDENQ1_ITEM4"));
		AddPrerequisite(new ItemPrerequisite("PRISON622_HIDDENQ1_ITEM5"));

		AddObjective("unlock", L("Unlock the locking device"), new ManualObjective());

		AddReward(new ItemReward("Collection_Base_PRISON62_2_HQ1", 1));
		AddReward(new TakeItemReward("PRISON622_HIDDENQ1_ITEM1", 1));
		AddReward(new TakeItemReward("PRISON622_HIDDENQ1_ITEM2", 1));
		AddReward(new TakeItemReward("PRISON622_HIDDENQ1_ITEM3", 1));
		AddReward(new TakeItemReward("PRISON622_HIDDENQ1_ITEM4", 1));
		AddReward(new TakeItemReward("PRISON622_HIDDENQ1_ITEM5", 1));
	}
}

// 60136: A Deeper Place (1)
//-----------------------------------------------------------------------------
public class Prison623Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60136);
		SetName(L("A Deeper Place (1)"));
		SetDescription(L("Priest Irma wants to release the spirit of the closest altar to clear out the energy of the cursed idol. With the power of the Revelators, release the spirit on your way to the Second Watchtower."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON623_MQ_01_NPC", "d_prison_62_3", L("Release the Altar Spirit on your way to the Second Watchtower"));
		SetPhase(QuestStatus.Success, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));

		AddPrerequisite(new QuestStatusPrerequisite(60131, QuestStatus.Completed));

		AddObjective("releaseAltar", L("Release the blessing energy on the altar on the way to the Second Watchtower"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 60137: A Deeper Place (2)
//-----------------------------------------------------------------------------
public class Prison623Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60137);
		SetName(L("A Deeper Place (2)"));
		SetDescription(L("Priest Irma believes your only hope lies on the blessing spirit of the last remaining altar. Release the spirit bestowed on the Confession Altar in the Prayer Room."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON623_MQ_02_NPC", "d_prison_62_3", L("Release the Altar Spirit in the Prayer Room"));
		SetPhase(QuestStatus.Success, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));

		AddPrerequisite(new QuestStatusPrerequisite(60136, QuestStatus.Completed));

		AddObjective("releaseAltar", L("Release the blessing energy on the altar in the Prayer Room"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 1));
	}
}

// 60138: A Deeper Place (3)
//-----------------------------------------------------------------------------
public class Prison623Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60138);
		SetName(L("A Deeper Place (3)"));
		SetDescription(L("Priest Irma believes the Idol will have become completely harmless now. Go to the Second Watchtower and destroy the cursed idol."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON623_MQ_03_NPC", "d_prison_62_3", L("Destroy the Cursed Idol in the Second Watchtower"));
		SetPhase(QuestStatus.Success, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON623_MQ_03_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60137, QuestStatus.Completed));

		AddObjective("killDenoptic", L("Defeat Denoptic"), new KillObjective(1, "boss_Denoptic_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
	}
}

// 60139: A Deeper Place (4)
//-----------------------------------------------------------------------------
public class Prison623Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60139);
		SetName(L("A Deeper Place (4)"));
		SetDescription(L("Demon Lord Marnox was hiding in the Penitence Room waiting to attack you! Defeat Demon Lord Marnox."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_IRMA_01", "d_prison_62_3", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON623_TO_PRISON623_1", "d_prison_62_3", L("Defeat Demon Lord Marnox"));
		SetPhase(QuestStatus.Success, "PRISON623_IRMA_02", "d_prison_62_3", L("Talk with Priest Irma"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON623_MQ_05_TRACK", "track", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(60138, QuestStatus.Completed));

		AddObjective("killMarnox", L("Defeat Demon Lord Marnox"), new KillObjective(1, "boss_Marnoks") { LayerOnly = true });

		AddReward(new ItemReward("expCard2", 3));
	}
}

// 60140: Everything Intact (1)
//-----------------------------------------------------------------------------
public class Prison623Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60140);
		SetName(L("Everything Intact (1)"));
		SetDescription(L("Priest Irma wants you to go and deliver the Demon Orders to Bishop Urbonas. Bring the orders to the room in Ashaq Underground Prison 1F where Bishop Urbonas is hiding."));
		SetType(QuestType.Main);
		SetLocation("d_prison_62_3", "d_prison_62_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_IRMA_02", "d_prison_62_3", L("Talk with Priest Irma"));
		SetPhase(QuestStatus.InProgress, "PRISON621_URBONAS", "d_prison_62_1", L("Deliver to Priest Urbonas at Ashaq Underground Prison 1F"));
		SetPhase(QuestStatus.Success, "PRISON621_URBONAS", "d_prison_62_1", L("Deliver to Priest Urbonas at Ashaq Underground Prison 1F"));

		AddPrerequisite(new QuestStatusPrerequisite(60139, QuestStatus.Completed));

		AddObjective("deliverOrders", L("Deliver to Priest Urbonas at Ashaq Underground Prison 1F"), new ManualObjective());

		AddReward(new TakeItemReward("PRISON623_MQ_05_ITEM", -1));
	}
}

// 60142: Collecting Information
//-----------------------------------------------------------------------------
public class Prison623Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60142);
		SetName(L("Collecting Information"));
		SetDescription(L("Priest Gelija is looking to do research on the demons' command hierarchy. Defeat demons to collect Marnox insignias."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_GELIYA", "d_prison_62_3", L("Talk with Priest Gelija"));
		SetPhase(QuestStatus.InProgress, "PRISON623_GELIYA", "d_prison_62_3", L("Collect Marnox's Insignia"));
		SetPhase(QuestStatus.Success, "PRISON623_GELIYA", "d_prison_62_3", L("Deliver to Priest Gelija"));

		AddPrerequisite(new LevelPrerequisite(15));

		AddObjective("collectInsignias", L("Collect Marnox's Insignia"), new CollectItemObjective("PRISON623_SQ_01_ITEM", 13));
		AddPityDrop("PRISON623_SQ_01_ITEM", 0.75f, 3, 1, "Sec_varv", "Sec_banshee_purple", "Sec_bubbe_mage_priest", "GoblinWarrior_blue");

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("PRISON623_SQ_01_ITEM", -1));
	}
}

// 60143: Precious and Valuable
//-----------------------------------------------------------------------------
public class Prison623Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60143);
		SetName(L("Precious and Valuable"));
		SetDescription(L("Priest Gelija is trying to help the injured but it seems they lost a pouch with medicine while running away. Find Priest Gelija's medicine pouch."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_GELIYA", "d_prison_62_3", L("Talk with Priest Gelija"));
		SetPhase(QuestStatus.InProgress, "PRISON623_GELIYA", "d_prison_62_3", L("Retrieve Priest Gelija's Medicine Pouch"));
		SetPhase(QuestStatus.Success, "PRISON623_GELIYA", "d_prison_62_3", L("Deliver to Priest Gelija"));

		AddPrerequisite(new LevelPrerequisite(15));

		AddObjective("collectPouches", L("Retrieve Priest Gelija's Medicine Pouch"), new CollectItemObjective("PRISON623_SQ_02_ITEM", 7));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("PRISON623_SQ_02_ITEM", -1));
	}
}

// 60144: Change of One's Thinking
//-----------------------------------------------------------------------------
public class Prison623Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60144);
		SetName(L("Change of One's Thinking"));
		SetDescription(L("While the cursed idol has been destroyed, its future still worries Priest Gelija. Cover any idol fragments with the cloth soaked in holy water to clear out their evil energy."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_GELIYA", "d_prison_62_3", L("Talk with Priest Gelija"));
		SetPhase(QuestStatus.InProgress, "PRISON623_GELIYA", "d_prison_62_3", L("Purify the Cursed Idol with the Holy Water Cloth"));
		SetPhase(QuestStatus.Success, "PRISON623_GELIYA", "d_prison_62_3", L("Talk with Priest Gelija"));

		AddPrerequisite(new LevelPrerequisite(15));

		AddObjective("purifyFragments", L("Purify the idol fragments with the Holy Water Cloth"), new VariableCheckObjective(DPrison623QuestNpcsScript.PurifiedCountVar, 11, isPermanent: true));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("PRISON623_SQ_03_ITEM", -1));
	}
}

// 60154: Days to Forget
//-----------------------------------------------------------------------------
public class Prison623Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60154);
		SetName(L("Days to Forget"));
		SetDescription(L("Priest Gelija has asked you to clear out the Varvs that have been terrorizing the priests in Ashaq Underground Prison. They are often found at the Felon Prison."));
		SetType(QuestType.Repeat);
		SetLocation("d_prison_62_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON623_GELIYA", "d_prison_62_3", L("Talk with Priest Gelija"));
		SetPhase(QuestStatus.InProgress, "PRISON623_GELIYA", "d_prison_62_3", L("Varv Extermination"));
		SetPhase(QuestStatus.Success, "PRISON623_GELIYA", "d_prison_62_3", L("Report to Priest Gelija"));

		AddPrerequisite(new LevelPrerequisite(15));

		AddObjective("killVarv", L("Exterminate Varv"), new KillObjective(15, "Sec_varv"));

		AddReward(new ItemReward("expCard2", 1));
	}
}
