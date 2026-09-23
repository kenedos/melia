//--- Melia Script ----------------------------------------------------------
// Dadan Jungle Quest NPCs
//--- Description -----------------------------------------------------------
// The giant bracken and the demons' laboratory, the road to the Novaha
// Monastery, and the herbalist caught in a demon trap.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FBracken633QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Bracken632Mq060 = new QuestId(50145);
	private readonly static QuestId Mq010 = new QuestId(50108);
	private readonly static QuestId Mq020 = new QuestId(50109);
	private readonly static QuestId Mq030 = new QuestId(50110);
	private readonly static QuestId Mq040 = new QuestId(50111);
	private readonly static QuestId Mq050 = new QuestId(50112);
	private readonly static QuestId Sq010 = new QuestId(50113);
	private readonly static QuestId Sq020 = new QuestId(50114);
	private readonly static QuestId Sq030 = new QuestId(50115);
	private readonly static QuestId Sq040 = new QuestId(50116);
	private readonly static QuestId Rp1 = new QuestId(60163);

	public const string DeviceCountVar = "Gabija.Quests.Bracken633Mq040.Devices";
	private const string DeviceVar = "Gabija.Quests.Bracken633Mq040.Device";
	public const string ShieldCountVar = "Gabija.Quests.Bracken633Sq020.Shields";
	private const string ShieldVar = "Gabija.Quests.Bracken633Sq020.Shield";
	public const string CauldronCountVar = "Gabija.Quests.Bracken633Sq030.Cauldrons";
	private const string CauldronVar = "Gabija.Quests.Bracken633Sq030.Cauldron";
	public const string BoxCountVar = "Gabija.Quests.Bracken633Sq040.Boxes";
	private const string BoxVar = "Gabija.Quests.Bracken633Sq040.Box";
	private const string LeafVar = "Gabija.Quests.Bracken633Rp1.Leaf";

	private static readonly double[,] Pages =
	{
		{ 147312, 860.34, -259.11, 47 }, { 147312, 750.43, -12.56, -11 }, { 153014, 959.66, -94.58, 21 }, { 153014, 599.92, -292.11, 13 },
	};

	private static readonly double[,] Devices =
	{
		{ -1157.50, 713.11 }, { -877.70, 648.84 }, { -650.70, 888.39 },
	};

	private static readonly double[,] Shields =
	{
		{ 95.73, -509.06 }, { 48.44, -785.31 }, { 163.64, -817.58 }, { 230.45, -534.99 },
	};

	private static readonly double[,] Cauldrons =
	{
		{ 104.85, -776.45 }, { 141.91, -622.05 }, { 159.60, -546.67 }, { 119.83, -698.50 },
	};

	private static readonly double[,] Boxes =
	{
		{ 151030, 989.16, -528.39, 74 }, { 151030, 1010.38, -794.81, 24 }, { 151030, 662.03, -655.88, 192 }, { 151030, 835.26, -821.46, -78 },
		{ 151029, 931.88, -990.40, -85 }, { 151029, 697.50, -1014.79, 232 }, { 151029, 552.93, -865.30, 65 },
	};

	private static readonly double[,] Dadania =
	{
		{ -759.25, -719.29 }, { -436.33, -847.62 }, { -250.71, -770.42 }, { -148.49, -657.82 }, { -92.31, -551.15 },
		{ -222.05, -434.08 }, { -721.21, -274.45 }, { -805.93, -466.26 }, { -598.36, -219.42 }, { -596.86, -503.41 },
		{ -689.65, -593.66 }, { -314.49, -435.04 }, { -726.01, -901.73 },
	};

	protected override void Load()
	{
		// Traveling Merchant Rose at the giant bracken
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN633_ROZE01", "f_bracken_63_3", 49.85, 489.81, 246, c => c.Quests.HasCompleted(Bracken632Mq060) && !c.Quests.HasCompleted(Mq040), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("Did you say you found a place that looks like a demon base?"));
				await dialog.CompleteQuest(Mq010);

				if (!character.Quests.HasCompleted(Mq010) || character.Quests.Has(Mq020))
					return;

				character.Quests.Start(Mq020);
				await dialog.Msg(L("I think a demon base could have documents about the giant bracken."));
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The demons looking after the laboratory have been defeated{nl}Look around for materials about the bracken"), 5);
				return;
			}

			if (character.Quests.IsCompletable(Mq030))
			{
				await dialog.Msg(L("Thank you for saving me. It seems all I do is receive your help..."));
				await dialog.Msg(L("I think that person is the wizard Tess was talking about. Someone qualified... Even if it's me they want, I can't just leave my people behind."));
				await dialog.CompleteQuest(Mq030);
				return;
			}

			if (character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("Did you do it? If the journal is correct, soon we won't have to worry about the giant bracken anymore."));
				await dialog.Msg(L("Let's go to the Novaha Monastery, then."));
				await dialog.CompleteQuest(Mq040);

				if (character.Quests.HasCompleted(Mq040))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Have you ever seen such a big bracken? I know this area is known for it, but even for a local like me, it's a first."));
				await dialog.Msg(L("Remember what Tess said earlier? About the giant bracken spreading out the spores of death... This looks like what Tess was talking about."));
				await dialog.Msg(L("We need to think of a way to stop that wizard's plans. The device set up next to the bracken... It could be an important key."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("I wonder if there's something like a lab around here."),
					Option(L("I'll find the laboratory and take care of the demons there."), "accept"),
					Option(L("Let's just go"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq010);

				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("We still don't know what the red liquid inside the device is, so destroying it could be dangerous. Let me have a look at the journal you found in the laboratory."));
				await dialog.Msg(L("Let's see... It's not hard to turn off the device after all. Seems like all you need to do is remove the power sources, which are set up separately."));

				var answer = await dialog.SelectQuestOffer(Mq040, L("But we have to be careful. The journal says the power supply devices are protected by powerful freezing magic circles."),
					Option(L("Tell me about the cold magic circle"), "accept"),
					Option(L("That sounds dangerous"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Devices.GetLength(0); ++i)
						character.Variables.Perm.Set(DeviceVar + i, false);
					character.Variables.Perm.SetInt(DeviceCountVar, 0);

					character.Quests.Start(Mq040);
					character.LookAround();

					await dialog.Msg(L("It doesn't say how to disable the magic circles. But we should be able to use monsters as bait, right?"));
					await dialog.Msg(L("We can probably lure them into the circles and quickly remove the power sources."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("I feel like I was too reckless coming out to rescue my brother and the villagers without a plan. I want to do something fast, but I'm afraid I'll end up making a mistake."));
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("Did you find anything? So there really is a laboratory. There could be more information about the device there."));
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				character.Quests.ReplayQuestTrack(Mq030);
				return;
			}

			await dialog.Msg(L("That red liquid inside the device, though... It makes me feel sick just looking at it. What could it be...?"));
		});

		AddQuestTrigger("BRACKEN633_ROZE_TRIGGER", "f_bracken_63_3", 49.42, 493.06, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
				character.Quests.Start(Mq030);

			await Task.CompletedTask;
		});

		AddNpc(153115, "UnvisibleName", "f_bracken_63_3", -70, 591, -9);

		// The demons' laboratory
		//-------------------------------------------------------------------------
		AddQuestTrigger("BRACKEN633_MQ1_EVENT", "f_bracken_63_3", 834, -176, 400, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq010) || character.Quests.IsCompletable(Mq010) || character.Variables.Temp.GetBool("Gabija.Quests.Bracken633Mq010.Lab", false))
				return;

			character.Variables.Temp.SetBool("Gabija.Quests.Bracken633Mq010.Lab", true);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("(This seems to be the demons' laboratory. Time to defeat some demons around here.)"), 5);

			await Task.CompletedTask;
		});

		AddNpc(153131, "UnvisibleName", "f_bracken_63_3", 561.57, -177.22, 67);
		AddNpc(153131, "UnvisibleName", "f_bracken_63_3", 1065.91, -143.31, -73);
		AddNpc(153131, "UnvisibleName", "f_bracken_63_3", 985.06, -115.65, 97);
		AddNpc(153132, "UnvisibleName", "f_bracken_63_3", 834.84, -361.99, 73);
		AddNpc(153132, "UnvisibleName", "f_bracken_63_3", 568.02, -154.44, 66);
		AddNpc(153132, "UnvisibleName", "f_bracken_63_3", 582.09, -298.25, 90);
		AddNpc(153013, "UnvisibleName", "f_bracken_63_3", 938.03, -312.41, 90);
		AddNpc(153014, "UnvisibleName", "f_bracken_63_3", 888.68, -350.36, 8);
		AddNpc(147312, "UnvisibleName", "f_bracken_63_3", 858.69, -22.38, 90);
		AddNpc(147312, "UnvisibleName", "f_bracken_63_3", 1020.12, -217.91, 165);
		AddNpc(147312, "UnvisibleName", "f_bracken_63_3", 642.23, -73.15, 125);

		// The Dadan Jungle Experiment Journal
		//-------------------------------------------------------------------------
		for (var i = 0; i < Pages.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc((int)Pages[i, 0], "UnvisibleName", "BRACKEN633_LOSTPAPER0" + number, "f_bracken_63_3", Pages[i, 1], Pages[i, 2], Pages[i, 3],
				character => character.Quests.IsActive(Mq020) && !character.Quests.IsCompletable(Mq020) && character.Inventory.CountItem(PageItemId(number)) == 0,
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Mq020) || character.Quests.IsCompletable(Mq020) || character.Inventory.CountItem(PageItemId(number)) > 0)
						return;

					character.Inventory.Add(PageItemId(number), 1, InventoryAddType.PickUp);
					Send.ZC_NORMAL.ShowBook(character, "BRACKEN_63_3_MQ020_book0" + number);
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Power Supply Devices
		//-------------------------------------------------------------------------
		for (var i = 0; i < Devices.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(47107, L("Power Supply Device"), "BRACKEN633_DEVICE01_" + number, "f_bracken_63_3", Devices[i, 0], Devices[i, 1], 90,
				character => !character.Quests.HasCompleted(Mq040) && !character.Variables.Perm.GetBool(DeviceVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Mq040) || character.Quests.IsCompletable(Mq040) || character.Variables.Perm.GetBool(DeviceVar + number, false))
						return;

					character.Variables.Perm.Set(DeviceVar + number, true);
					var removed = character.Variables.Perm.GetInt(DeviceCountVar, 0) + 1;
					character.Variables.Perm.SetInt(DeviceCountVar, removed);

					dialog.Npc.PlayEffect("F_light018_yellow", 1f);
					character.ServerMessage(LF("Power sources removed: {0}/{1}", Math.Min(removed, 3), 3));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Traveling Merchant Rose at the monastery entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(153119, L("Traveling Merchant Rose"), "BRACKEN633_ROZE02", "f_bracken_63_3", -145, -728, -54, c => c.Quests.HasCompleted(Mq040) && !c.Quests.HasCompleted(Mq050), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Traveling Merchant Rose"));
			dialog.SetPortrait("Dlg_port_Roze");

			if (character.Quests.IsCompletable(Mq050))
			{
				await dialog.Msg(L("Are you alright? Your skills are really incredible."));
				await dialog.Msg(L("Let's go inside the Novaha Monastery, then. Please, let everyone be safe..."));
				await dialog.CompleteQuest(Mq050);

				if (character.Quests.HasCompleted(Mq050))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				await dialog.Msg(L("Security is too strict. The demons are guarding the entrance to the Novaha Monastery."));
				await dialog.Msg(L("My brother and the villagers... They'll be safe, right?"));

				var answer = await dialog.SelectQuestOffer(Mq050, L("Please try not to make too much of a commotion."),
					Option(L("I'll defeat the demons"), "accept"),
					Option(L("Let's wait until the demons disappear"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq050);

				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("You need to be as quiet as possible. I'm sure my brother and the villagers are still safe since they have no use for them yet..."));
				await dialog.Msg(L("But this could go very wrong if we're not careful."));
				character.Quests.ReplayQuestTrack(Mq050);
				return;
			}

			await dialog.Msg(L("Security is too strict. The demons are guarding the entrance to the Novaha Monastery."));
		});

		// Herbalist Talas
		//-------------------------------------------------------------------------
		AddNpc(147482, L("Herbalist Talas"), "BRACKEN633_PEAPLE01", "f_bracken_63_3", -143.47, -155.28, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Herbalist Talas"));

			if (character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("How did it go? Was the cauldron destroyed?"));
				await dialog.CompleteQuest(Sq020);

				if (!character.Quests.HasCompleted(Sq020) || character.Quests.Has(Sq030))
					return;

				for (var i = 1; i <= Cauldrons.GetLength(0); ++i)
					character.Variables.Perm.Set(CauldronVar + i, false);
				character.Variables.Perm.SetInt(CauldronCountVar, 0);

				character.Quests.Start(Sq030);
				character.LookAround();

				await dialog.Msg(L("So the protective magic circle around the cauldron has been lifted, right?"));
				await dialog.Msg(L("You should go now and destroy the cauldron before the demons find out about the missing magic circle."));
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The protective barrier has been lifted.{nl}Eliminate the cauldron."), 5);
				return;
			}

			if (character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("Thank you. This should be enough to repay you for the trap."));
				await dialog.Msg(L("Helping stop the demons makes me feel like I achieved something great."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("I'm relieved to hear that. Thank you. I ought to stop playing a hero and take care of myself now."));
				await dialog.CompleteQuest(Sq040);
				return;
			}

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Thank you! I should tend to my legs now."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("Help! I was caught in a trap running away from the demons!"));

				var answer = await dialog.SelectQuestOffer(Sq010, L("Please help me! I can't open the trap by myself..."),
					Option(L("Removing the trap"), "accept"),
					Option(L("Something could go really wrong if we're not careful"), "leave")
				);

				if (answer != "accept")
					return;

				character.Quests.Start(Sq010);
				await this.FreeTalas(dialog);
				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await this.FreeTalas(dialog);
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				await dialog.Msg(L("I did see something odd before I escaped. The demons were adding herbs to a cauldron."));
				await dialog.Msg(L("I had never seen demons use any herbs, but I'm sure whatever it is they're doing, it's evil."));

				var answer = await dialog.SelectQuestOffer(Sq020, L("I was on my way to stop them when this happened..."),
					Option(L("It's best to destroy the cauldron"), "accept"),
					Option(L("It will be fine"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Shields.GetLength(0); ++i)
						character.Variables.Perm.Set(ShieldVar + i, false);
					character.Variables.Perm.SetInt(ShieldCountVar, 0);

					character.Quests.Start(Sq020);
					character.LookAround();

					await dialog.Msg(L("Destroying the cauldrons won't be easy. They're protected by some sort of barrier."));
					await dialog.Msg(L("Every time a barrier appeared I noticed a sort of glowing stone. I wonder if that's what we should target first."));
				}
				return;
			}

			if (!character.Quests.Has(Sq040) && character.Quests.MeetsPrerequisites(Sq040))
			{
				await dialog.Msg(L("Oh, I have one last favor to ask. We may have destroyed the cauldrons, but the materials remain. This could be a problem."));
				await dialog.Msg(L("I believe it would be better to burn down the materials as well."));

				var answer = await dialog.SelectQuestOffer(Sq040, L("They're at the Saloto Lowland. Make sure they're all gone."),
					Option(L("Leave it to me"), "accept"),
					Option(L("You should go back and get treated"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Boxes.GetLength(0); ++i)
						character.Variables.Perm.Set(BoxVar + i, false);
					character.Variables.Perm.SetInt(BoxCountVar, 0);

					character.Quests.Start(Sq040);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("It's useless. The demons won't let me collect the herbs I need from Badoca Hill."));
				await dialog.Msg(L("Without those herbs I can't make medicine for the refugees."));

				var answer = await dialog.SelectQuestOffer(Rp1, L("It would be great if you could help..."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= Dadania.GetLength(0); ++i)
						character.Variables.Perm.Set(LeafVar + i, false);

					character.Quests.Start(Rp1);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("Destroying the cauldrons won't be easy. They're protected by some sort of barrier."));
				await dialog.Msg(L("Every time a barrier appeared I noticed a sort of glowing stone. I wonder if that's what we should target first."));
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("Fearful people like me can tell that much. Those cauldrons are nothing but evil."));
				return;
			}

			if (character.Quests.IsActive(Sq040))
			{
				await dialog.Msg(L("I hear the demons aren't going to stop at simply existing. Are the goddesses missing the work of demons too, I wonder?"));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("There's not much I can do with my legs like this..."));
				return;
			}

			if (character.Quests.HasCompleted(Sq030))
			{
				await dialog.Msg(L("The demons simply cannot keep existing. Isn't it because of them that the goddesses are missing right now?"));
				return;
			}

			await dialog.Msg(L("It seems the bones are fine but... We should go somewhere safe after getting some rest."));
		});

		AddConditionalNpc(152066, "UnvisibleName", "BRACKEN633_TRAP", "f_bracken_63_3", -137.69, -152.56, 16, c => !c.Quests.HasCompleted(Sq010));

		// Shield Creation Devices and the Demon Cauldrons at Ruivara Field
		//-------------------------------------------------------------------------
		for (var i = 0; i < Shields.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(151050, L("Shield Creation Device"), "BRACKEN633_SQ02_MAGICSHILDE0" + number, "f_bracken_63_3", Shields[i, 0], Shields[i, 1], 90,
				character => !character.Quests.HasCompleted(Sq020) && !character.Variables.Perm.GetBool(ShieldVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq020) || character.Quests.IsCompletable(Sq020) || character.Variables.Perm.GetBool(ShieldVar + number, false))
						return;

					character.Variables.Perm.Set(ShieldVar + number, true);
					var removed = character.Variables.Perm.GetInt(ShieldCountVar, 0) + 1;
					character.Variables.Perm.SetInt(ShieldCountVar, removed);

					dialog.Npc.PlayEffect("F_light018_yellow", 1f);
					character.ServerMessage(LF("Protective barriers removed: {0}/{1}", Math.Min(removed, 4), 4));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		for (var i = 0; i < Cauldrons.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153013, L("Demon Cauldron"), "BRACKEN633_SQ03_BOX0" + number, "f_bracken_63_3", Cauldrons[i, 0], Cauldrons[i, 1], 90,
				character => !character.Quests.HasCompleted(Sq030) && !character.Variables.Perm.GetBool(CauldronVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq030) || character.Quests.IsCompletable(Sq030) || character.Variables.Perm.GetBool(CauldronVar + number, false))
						return;

					character.Variables.Perm.Set(CauldronVar + number, true);
					var destroyed = character.Variables.Perm.GetInt(CauldronCountVar, 0) + 1;
					character.Variables.Perm.SetInt(CauldronCountVar, destroyed);

					dialog.Npc.PlayEffect("F_ground058_smoke", 1f);
					character.ServerMessage(LF("Demon Cauldrons destroyed: {0}/{1}", Math.Min(destroyed, 4), 4));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Boxes of poisonous herbs at the Saloto Lowland
		//-------------------------------------------------------------------------
		for (var i = 0; i < Boxes.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc((int)Boxes[i, 0], "UnvisibleName", "BRACKEN633_SQ04_BOX_" + number, "f_bracken_63_3", Boxes[i, 1], Boxes[i, 2], Boxes[i, 3],
				character => character.Quests.IsActive(Sq040) && !character.Quests.IsCompletable(Sq040) && !character.Variables.Perm.GetBool(BoxVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Sq040) || character.Quests.IsCompletable(Sq040) || character.Variables.Perm.GetBool(BoxVar + number, false))
						return;

					character.Variables.Perm.Set(BoxVar + number, true);
					var burned = character.Variables.Perm.GetInt(BoxCountVar, 0) + 1;
					character.Variables.Perm.SetInt(BoxCountVar, burned);

					dialog.Npc.PlayEffect("F_burstup001_fire", 1f);
					character.ServerMessage(LF("Boxes of Poisonous Herbs burned: {0}/{1}", Math.Min(burned, 6), 6));
					character.LookAround();

					await Task.CompletedTask;
				});
		}

		// Dadania Grass on Badoca Hill
		//-------------------------------------------------------------------------
		for (var i = 0; i < Dadania.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153054, L("Dadania Grass"), "BRACKEN633_RP_1_OBJ_" + number, "f_bracken_63_3", Dadania[i, 0], Dadania[i, 1], 90,
				character => character.Quests.IsActive(Rp1) && !character.Quests.IsCompletable(Rp1) && !character.Variables.Perm.GetBool(LeafVar + number, false),
				async dialog =>
				{
					var character = dialog.Player;

					if (!character.Quests.IsActive(Rp1) || character.Quests.IsCompletable(Rp1) || character.Variables.Perm.GetBool(LeafVar + number, false))
						return;

					character.Variables.Perm.Set(LeafVar + number, true);
					character.Inventory.Add(ItemId.BRACKEN633_RP_1_ITEM, 1, InventoryAddType.PickUp);
					character.LookAround();

					await Task.CompletedTask;
				});
		}
	}

	/// <summary>
	/// Returns the item id of the given page of the Dadan Jungle
	/// Experiment Journal.
	/// </summary>
	private static int PageItemId(int number)
	{
		switch (number)
		{
			case 1: return ItemId.BRACKEN633_MQ2_ITEM01;
			case 2: return ItemId.BRACKEN633_MQ2_ITEM02;
			case 3: return ItemId.BRACKEN633_MQ2_ITEM03;
			default: return ItemId.BRACKEN633_MQ2_ITEM04;
		}
	}

	/// <summary>
	/// Pries the demon trap off Herbalist Talas' ankle.
	/// </summary>
	private async Task FreeTalas(Dialog dialog)
	{
		var character = dialog.Player;

		var freed = await character.TimeActions.StartAsync(L("Remove the trap"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));
		if (freed != TimeActionResult.Completed)
			return;

		character.Quests.CompleteObjective(Sq010, "freeTalas");

		await dialog.Msg(L("Thank you. Fortunately my ankle isn't completely gone yet. If it weren't for you I would be in the hands of the demons."));
		await dialog.CompleteQuest(Sq010);
		character.LookAround();
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50108: Giant Bracken (1)
//-----------------------------------------------------------------------------
public class Bracken633Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50108);
		SetName(L("Giant Bracken (1)"));
		SetDescription(L("Traveling Merchant Rose saw a giant bracken attached to a device and thinks there might be a laboratory nearby. Find the demon laboratory and defeat the demons there."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Follow Traveling Merchant Rose to Dadan Jungle"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_MQ1_EVENT", "f_bracken_63_3", L("Find the demon laboratory and defeat the demons there"));
		SetPhase(QuestStatus.Success, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Find the demon laboratory and defeat the demons there"));

		AddPrerequisite(new QuestStatusPrerequisite(50145, QuestStatus.Completed));

		AddObjective("killWarriors", L("Find the demon laboratory and defeat the demons there"), new KillObjective(10, "Sec_bubbe_fighter"));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 270));
	}
}

// 50109: Giant Bracken (2)
//-----------------------------------------------------------------------------
public class Bracken633Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50109);
		SetName(L("Giant Bracken (2)"));
		SetDescription(L("This laboratory seems to be related to the giant bracken. Look around and try to find any materials that might be related to it."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Search for materials on the Giant Bracken"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_MQ1_EVENT", "f_bracken_63_3", L("Search for materials on the Giant Bracken"));
		SetPhase(QuestStatus.Success, "BRACKEN633_ROZE_TRIGGER", "f_bracken_63_3", L("Deliver the diary to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50108, QuestStatus.Completed));

		AddObjective("findPage1", L("Search for materials on the Giant Bracken"), new CollectItemObjective("BRACKEN633_MQ2_ITEM01", 1));
		AddObjective("findPage2", L("Search for materials on the Giant Bracken"), new CollectItemObjective("BRACKEN633_MQ2_ITEM02", 1));
		AddObjective("findPage3", L("Search for materials on the Giant Bracken"), new CollectItemObjective("BRACKEN633_MQ2_ITEM03", 1));
		AddObjective("findPage4", L("Search for materials on the Giant Bracken"), new CollectItemObjective("BRACKEN633_MQ2_ITEM04", 1));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 160));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; the diary is handed over in the next quest.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Go back to Rose"), 3);
	}
}

// 50110: Mysterious Wizard
//-----------------------------------------------------------------------------
public class Bracken633Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50110);
		SetName(L("Mysterious Wizard"));
		SetDescription(L("Rose was captured by demons and an unknown wizard. Rescue Traveling Merchant Rose."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_ROZE_TRIGGER", "f_bracken_63_3", L("Return to Rose and deliver the diary"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_ROZE_TRIGGER", "f_bracken_63_3", L("Rescue Traveling Merchant Rose"));
		SetPhase(QuestStatus.Success, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Talk to Traveling Merchant Rose"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "BRACKEN_63_3_MQ030_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50109, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons that tried to kidnap Rose"), new KillObjective(15, "Sec_bubbe_fighter", "lapasape_mage") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 4));
		AddReward(new ItemReward("Vis", 350));
		AddReward(new TakeItemReward("BRACKEN633_MQ2_ITEM01", 1));
		AddReward(new TakeItemReward("BRACKEN633_MQ2_ITEM02", 1));
		AddReward(new TakeItemReward("BRACKEN633_MQ2_ITEM03", 1));
		AddReward(new TakeItemReward("BRACKEN633_MQ2_ITEM04", 1));
	}
}

// 50111: Deactivate
//-----------------------------------------------------------------------------
public class Bracken633Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50111);
		SetName(L("Deactivate"));
		SetDescription(L("In order to turn off the giant bracken device it's power source needs to be removed. Use the monsters to stop the freezing magic circles and remove the power source."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Talk to Traveling Merchant Rose"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Find the Power Supply Device and remove the power source"));
		SetPhase(QuestStatus.Success, "BRACKEN633_ROZE01", "f_bracken_63_3", L("Report to Traveling Merchant Rose"));

		AddPrerequisite(new QuestStatusPrerequisite(50110, QuestStatus.Completed));

		AddObjective("removePower", L("Find the Power Supply Device and remove the power source"), new VariableCheckObjective(FBracken633QuestNpcsScript.DeviceCountVar, 3, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
	}
}

// 50112: To Novaha Monastery
//-----------------------------------------------------------------------------
public class Bracken633Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50112);
		SetName(L("To Novaha Monastery"));
		SetDescription(L("Traveling Merchant Rose says there are demons guarding the entrance to the Novaha Monastery. Defeat the sentinel demons."));
		SetType(QuestType.Main);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_ROZE02", "f_bracken_63_3", L("Follow Traveling Merchant Rose to the entrance of the Novaha Monastery"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_ROZE02", "f_bracken_63_3", L("Defeat the sentinel demons"));
		SetPhase(QuestStatus.Success, "BRACKEN633_ROZE02", "f_bracken_63_3", L("Talk to Traveling Merchant Rose"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "BRACKEN_63_3_MQ050_TRACK", "m_boss_c", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50111, QuestStatus.Completed));

		AddObjective("killGremlin", L("Defeat the Gremlin guarding the monastery entrance"), new KillObjective(1, "boss_Gremlin") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 4));
		AddReward(new ItemReward("Vis", 400));
	}
}

// 50113: Trapped Herbalist
//-----------------------------------------------------------------------------
public class Bracken633Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50113);
		SetName(L("Trapped Herbalist"));
		SetDescription(L("Herbalist Talas seems to have gotten caught in the trap while running away from demons. Release Talas from the trap."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Free Herbalist Talas from the trap"));
		SetPhase(QuestStatus.Success, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));

		AddPrerequisite(new LevelPrerequisite(26));

		AddObjective("freeTalas", L("Free Herbalist Talas from the trap"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 80));
	}
}

// 50114: Demon Cauldron (1)
//-----------------------------------------------------------------------------
public class Bracken633Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50114);
		SetName(L("Demon Cauldron (1)"));
		SetDescription(L("Herbalist Talas saw the demons add a type of a poisonous plant to the cauldrons and tried to destroy them. Go to Ruivara Field and remove the demon cauldrons' protective barrier."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Remove the Protective Barrier around the Demon Cauldrons"));
		SetPhase(QuestStatus.Success, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Remove the Protective Barrier around the Demon Cauldrons"));

		AddPrerequisite(new QuestStatusPrerequisite(50113, QuestStatus.Completed));

		AddObjective("removeBarriers", L("Remove the Protective Barrier around the Demon Cauldrons"), new VariableCheckObjective(FBracken633QuestNpcsScript.ShieldCountVar, 4, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 240));
	}
}

// 50115: Demon Cauldron (2)
//-----------------------------------------------------------------------------
public class Bracken633Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50115);
		SetName(L("Demon Cauldron (2)"));
		SetDescription(L("You have turned off all the protection shield creation devices. Destroy the cauldrons."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Destroy the Demon Cauldrons"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Destroy the Demon Cauldrons"));
		SetPhase(QuestStatus.Success, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));

		AddPrerequisite(new QuestStatusPrerequisite(50114, QuestStatus.Completed));

		AddObjective("destroyCauldrons", L("Destroy the Demon Cauldrons"), new VariableCheckObjective(FBracken633QuestNpcsScript.CauldronCountVar, 4, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 240));
	}
}

// 50116: Potential Threat
//-----------------------------------------------------------------------------
public class Bracken633Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50116);
		SetName(L("Potential Threat"));
		SetDescription(L("Herbalist Talas wants to burn the demons' materials to prevent them from restarting their operation. Go to the Saloto Lowland and burn down the demons' boxes of poisonous herbs."));
		SetType(QuestType.Sub);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Set the boxes on fire with the Poisonous Herbs"));
		SetPhase(QuestStatus.Success, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Report to Herbalist Talas"));

		AddPrerequisite(new QuestStatusPrerequisite(50115, QuestStatus.Completed));

		AddObjective("burnBoxes", L("Set the boxes of Poisonous Herbs on fire"), new VariableCheckObjective(FBracken633QuestNpcsScript.BoxCountVar, 6, isPermanent: true));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 160));
	}
}

// 60163: Cure-all Medicine
//-----------------------------------------------------------------------------
public class Bracken633Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60163);
		SetName(L("Cure-all Medicine"));
		SetDescription(L("Herbalist Talas has become injured by a trap after coming to collect dry Dadania leaves at Badoca Hill. Go find dry Dadania leaves from Dadania bushes at Badoca Hill for Talas."));
		SetType(QuestType.Repeat);
		SetLocation("f_bracken_63_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Talk to Herbalist Talas"));
		SetPhase(QuestStatus.InProgress, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Collect Dry Dadania Leaves"));
		SetPhase(QuestStatus.Success, "BRACKEN633_PEAPLE01", "f_bracken_63_3", L("Report back to Herbalist Talas"));

		AddPrerequisite(new QuestStatusPrerequisite(50113, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(26));

		AddObjective("collectLeaves", L("Collect dried Dadania Leaves"), new CollectItemObjective("BRACKEN633_RP_1_ITEM", 5));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("BRACKEN633_RP_1_ITEM", -1));
	}
}
