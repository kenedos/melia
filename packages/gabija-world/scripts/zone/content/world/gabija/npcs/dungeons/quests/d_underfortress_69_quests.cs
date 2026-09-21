//--- Melia Script ----------------------------------------------------------
// Fortress Battlegrounds Quest NPCs
//--- Description -----------------------------------------------------------
// The defensive magic circle Amanda builds behind Premier Eminent's back,
// and the Revelation of the Land the device finally gives up.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DUnderfortress69QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq010 = new QuestId(50079);
	private readonly static QuestId Mq020 = new QuestId(50080);
	private readonly static QuestId Mq030 = new QuestId(50081);
	private readonly static QuestId Mq040 = new QuestId(50082);
	private readonly static QuestId Mq050 = new QuestId(50083);
	private readonly static QuestId Mq060 = new QuestId(50084);
	private readonly static QuestId Sq010 = new QuestId(50085);
	private readonly static QuestId Sq020 = new QuestId(50086);
	private readonly static QuestId Sq030 = new QuestId(50087);
	private readonly static QuestId Hq1 = new QuestId(50269);

	private const int PartsNeeded = 5;

	// The battlefield traps the device's parts come out of.
	private readonly static string[] TrapNames =
	{
		"UNDER69_MQ020_DEVICE01", "UNDER69_MQ020_DEVICE02", "UNDER69_MQ020_DEVICE03",
		"UNDER69_MQ020_DEVICE04", "UNDER69_MQ020_DEVICE05",
	};

	private readonly static double[,] TrapSpots =
	{
		{ 1868.62, 1546.31 }, { 1984.75, 1232.88 }, { 1747.96, 1711.68 },
		{ 1517.15, 1187.81 }, { 1666.86, 1190.85 },
	};

	// The Demon Totems around the Ikveta Podium foundation stone.
	private readonly static string[] TotemNames =
	{
		"UNDER69_MQ4_FLAG01", "UNDER69_MQ4_FLAG02", "UNDER69_MQ4_FLAG03", "UNDER69_MQ4_FLAG04",
	};

	private readonly static double[,] TotemSpots =
	{
		{ -939.82, -1999.22 }, { -758.55, -2294.01 }, { -614.66, -2082.99 }, { -1049.65, -2229.14 },
	};

	private readonly static double[] TotemFacings = { 39, 72, 30, 90 };

	// The stones Amanda could carve her account into.
	private readonly static double[,] Stones =
	{
		{ -11.42, -885.65 }, { 11.86, -649.95 }, { 458.86, -269.08 },
		{ 253.09, -250.86 }, { -112.30, -1005.00 },
	};

	private readonly static double[] StoneFacings = { 236, 22, 198, -26, 59 };

	// The pages of the Ruklys journal, scattered over the battlegrounds.
	private readonly static string[] PageNames =
	{
		"UNDER69_PAPER01", "UNDER69_PAPER02", "UNDER69_PAPER03", "UNDER69_PAPER04",
	};

	private readonly static double[,] PageSpots =
	{
		{ -386.65, -2124.75 }, { 1154.35, -106.59 }, { 1727.05, 1811.64 }, { -315.27, 1481.46 },
	};

	private readonly static double[] PageFacings = { 90, 266, 90, -23 };

	private readonly static int[] PageItems =
	{
		ItemId.UNDER69_SQ2_ITEM01, ItemId.UNDER69_SQ2_ITEM02,
		ItemId.UNDER69_SQ2_ITEM03, ItemId.UNDER69_SQ2_ITEM04,
	};

	// The spots the retreat horn is blown from.
	private readonly static double[,] HornSpots =
	{
		{ -1044.49, 2063.11 }, { -729.01, 2037.02 }, { -853.30, 1930.36 },
	};

	protected override void Load()
	{
		// Amanda, at the battleground gate
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_69_1", "d_underfortress_69", 2120.20, 37.30, -38, this.IsAmandaAtTheGate, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq010) && character.Quests.IsCompletable(Mq010))
			{
				await dialog.Msg(L("This is as I thought... He is not like the guardian of the revelation."));
				await dialog.Msg(L("Remember this? It is the defense magic scroll that was hidden inside the treasure chest at the secret location."));
				await dialog.CompleteQuest(Mq010);
				return;
			}

			if (!character.Quests.Has(Mq010) && character.Quests.MeetsPrerequisites(Mq010))
			{
				await dialog.Msg(L("Treasures mean dirt to me at the moment."));

				var answer = await dialog.SelectQuestOffer(Mq010, L("Even I care about the kingdom."),
					Option(L("Alright"), "accept"),
					Option(L("Let's go a bit later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq010);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq010))
			{
				await dialog.Msg(L("Put the Monocle on him and see for yourself."));
				character.Quests.ReplayQuestTrack(Mq010);
				return;
			}

			await dialog.Msg(L("A grave robber who has stopped caring what is down here to steal."));
		});

		// Amanda, at the watchtower approach
		//-------------------------------------------------------------------------
		AddConditionalNpc(153040, L("[Amanda Grave Robbers]{nl}Amanda"), "AMANDA_69_2", "d_underfortress_69", 507.57, 479.63, 90, this.IsAmandaAtTheWatchtower, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Amanda"));

			if (character.Quests.IsActive(Mq020) && character.Quests.IsCompletable(Mq020))
			{
				await dialog.Msg(L("You're here?"));
				await dialog.Msg(L("What did Eminent say?"));
				await dialog.CompleteQuest(Mq020);
				return;
			}

			if (character.Quests.IsActive(Mq060) && character.Quests.IsCompletable(Mq060))
			{
				await dialog.Msg(L("I've seen the magic circles activating. You are okay, right?"));
				await dialog.Msg(L("How about Eminent?"));
				await dialog.CompleteQuest(Mq060);
				return;
			}

			if (character.Quests.IsActive(Sq010) && character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("I know that it is hard to bring something that is that big."));
				await dialog.Msg(L("Thank you. I will think what to write on it."));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsActive(Sq020) && character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("Okay. This is enough."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (character.Quests.IsActive(Hq1) && character.Quests.IsCompletable(Hq1))
			{
				await dialog.Msg(L("Yeah, this should do it. Hold on."));
				await dialog.Msg(L("I carved out our achievements on it."));
				await dialog.Msg(L("I want our successors to remember them."));
				await dialog.CompleteQuest(Hq1);
				return;
			}

			if (!character.Quests.Has(Mq030) && character.Quests.MeetsPrerequisites(Mq030))
			{
				await dialog.Msg(L("I took a look at the location of the foundation stone on the scroll."));

				var answer = await dialog.SelectQuestOffer(Mq030, L("The magic circle only gets properly activated with this."),
					Option(L("How do you activate the magic circle?"), "accept"),
					Option(L("Do it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq030);
					await dialog.Msg(L("Ah, I almost forgot."));
					await dialog.Msg(L("The scroll says that anything with magic in it would suffice."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq040) && character.Quests.MeetsPrerequisites(Mq040))
			{
				await dialog.Msg(L("How did it go?"));

				var answer = await dialog.SelectQuestOffer(Mq040, L("The remaining foundation stone is located near the Ikveta Podium."),
					Option(L("Destroy the totems? Got it"), "accept"),
					Option(L("We need to find another way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq040);
					await dialog.Msg(L("Ah, keep in mind that Eminent might get suspicious if you take too much time."));
					await dialog.Msg(L("As soon as the foundation stone gets activated, you need to get back to Eminent."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				await dialog.Msg(L("Ah, you're still here? What good news."));

				var answer = await dialog.SelectQuestOffer(Sq010, L("Lend me your hand. I am the first grave robber to get this deep into the Fortress of the Land."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("Find it yourself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq010);
					await dialog.Msg(L("What should I write on the stones?"));
					await dialog.Msg(L("If you have any thoughts, please let me know."));
					return;
				}
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				var answer = await dialog.SelectQuestOffer(Sq020, L("This is a page from the journal written by the Ruklys army! I am quite certain there are more pages."),
					Option(L("I will get it to you if I find them"), "accept"),
					Option(L("I will find it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq020);
					await dialog.Msg(L("If you find them, please do give them to me."));
					await dialog.Msg(L("Wilhelmina Carriot will be happy to see this."));
					return;
				}
			}

			if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
			{
				var answer = await dialog.SelectQuestOffer(Hq1, L("Ah... Thanks to you I did find a proper stone, but I don't have the tools to carve it."),
					Option(L("I'll go see the Dievdirbys Master"), "accept"),
					Option(L("I have somewhere else to be"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq1);
					await dialog.Msg(L("The Dievdirbys Master keeps carving tools. He will lend them out."));
					return;
				}
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("Bring whatever the traps on Ataka Side Road will give up."));
				return;
			}

			if (character.Quests.IsActive(Mq030))
			{
				await dialog.Msg(L("Be sure not to be caught by Eminent."));
				return;
			}

			if (character.Quests.IsActive(Mq040))
			{
				await dialog.Msg(L("I believe that Ruklys has not colluded with the demons."));
				await dialog.Msg(L("If not, what I saw are all lies."));
				return;
			}

			if (character.Quests.IsActive(Mq060))
			{
				await dialog.Msg(L("The device is at the far end. Go and work it."));
				return;
			}

			if (character.Quests.IsActive(Sq010))
			{
				await dialog.Msg(L("What should I write on the stones?"));
				return;
			}

			if (character.Quests.IsActive(Sq020))
			{
				await dialog.Msg(L("If you find the other pages, please do give them to me."));
				return;
			}

			if (character.Quests.IsActive(Hq1))
			{
				await dialog.Msg(L("The Dievdirbys Master keeps carving tools. He will lend them out."));
				return;
			}

			await dialog.Msg(L("A grave robber reading a defensive magic circle off a stolen scroll."));
		});

		// Premier Eminent, at the battleground
		//-------------------------------------------------------------------------
		AddConditionalNpc(153139, L("Premier Eminent"), "EMINENT_69_1", "d_underfortress_69", 1738.01, -374.94, 74, this.IsEminentAtTheBattleground, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Premier Eminent"));

			if (!character.Quests.Has(Mq020) && character.Quests.MeetsPrerequisites(Mq020))
			{
				await dialog.Msg(L("Oh, you came. Let us trace back the memory of the spirits."));

				var answer = await dialog.SelectQuestOffer(Mq020, L("The parts are needed to revive the device. Please find them from the traps set up in the area near Ataka Side Road."),
					Option(L("I will get the parts"), "accept"),
					Option(L("I will find it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq020);
					await dialog.Msg(L("I will prepare the magic with the memories of the spirits, so you go to Ataka Side Road."));
					await dialog.Msg(L("Bring everything you can."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq020))
			{
				await dialog.Msg(L("Bring everything the traps will give up."));
				return;
			}

			await dialog.Msg(L("A keeper standing on the last battlefield of Ruklys as if he had been there before."));
		});

		// Premier Eminent, at Ruklys' Device
		//-------------------------------------------------------------------------
		AddConditionalNpc(153139, L("Premier Eminent"), "EMINENT_69_2", "d_underfortress_69", -141.96, -44.75, 90, this.IsEminentAtTheDevice, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Premier Eminent"));

			if (!character.Quests.Has(Mq050) && character.Quests.MeetsPrerequisites(Mq050))
			{
				var answer = await dialog.SelectQuestOffer(Mq050, L("Have you collected all the parts? Give it to me. I can restore it."),
					Option(L("Hand over the parts"), "accept"),
					Option(L("Do not hand over the parts"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq050);
					character.Inventory.RemoveItem(ItemId.UNDER69_MQ2_ITEM01, PartsNeeded);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq050))
			{
				await dialog.Msg(L("I want you to meet the revelation of the goddess fast."));
				await dialog.Msg(L("To fulfill my lifelong wish..."));
				character.Quests.ReplayQuestTrack(Mq050);
				return;
			}

			await dialog.Msg(L("A keeper standing over Ruklys' device with the light of the magic circle on him."));
		});

		// The Slepti Watchtower foundation stone
		//-------------------------------------------------------------------------
		AddNpc(147414, L("Defensive Magic Circle Foundation Stone"), "UNDER69_MQ3_DEVICE_REPAIR", "d_underfortress_69", 724.86, 1618.59, 26, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Defensive Magic Circle Foundation Stone"));

			if (character.Quests.IsActive(Mq030) && !character.Quests.IsCompletable(Mq030))
			{
				if (character.Inventory.CountItem(ItemId.UNDER69_MQ3_ITEM01) < 13)
				{
					await dialog.Msg(L("The letters on the pillar will not take anything but demon blood."));
					return;
				}

				var carved = await character.TimeActions.StartAsync(L("Inscribing the letters..."), L("Cancel"), "HAMMERING", TimeSpan.FromSeconds(2));

				if (carved != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.UNDER69_MQ3_ITEM01, 13);
				character.Quests.CompleteObjective(Mq030, "carveTheStone");
				character.ServerMessage(L("The foundation stone of the magic circle is active!"));
				return;
			}

			await dialog.Msg(L("A foundation stone of the defensive magic circle, its letters worn out of it."));
		});

		// The Ikveta Podium foundation stone
		//-------------------------------------------------------------------------
		AddNpc(147414, L("Defensive Magic Circle Foundation Stone"), "UNDER69_MQ4_DEVICE", "d_underfortress_69", -822.07, -2174.77, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Defensive Magic Circle Foundation Stone"));

			if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
			{
				await dialog.Msg(L("The Demon Totems around the stone are holding it shut. Break them first."));
				return;
			}

			if (character.Quests.IsActive(Mq040) && character.Quests.IsCompletable(Mq040))
			{
				var activated = await character.TimeActions.StartAsync(L("Activating the magic circle..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (activated != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The magic circle is active!"));
				await dialog.CompleteQuest(Mq040);
				return;
			}

			await dialog.Msg(L("The second foundation stone of the defensive magic circle, at the Ikveta Podium."));
		});

		// Ruklys' Device
		//-------------------------------------------------------------------------
		AddNpc(153059, L("Ruklys' Device"), "UNDER69_MQ5", "d_underfortress_69", -107.35, -42.18, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ruklys' Device"));

			if (!character.Quests.Has(Mq060) && character.Quests.MeetsPrerequisites(Mq060))
			{
				var worked = await character.TimeActions.StartAsync(L("Working the device..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (worked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq060);
				character.ServerMessage(L("The device opens a way through to the secret chamber."));
				return;
			}

			if (character.Quests.IsActive(Mq060))
			{
				await dialog.Msg(L("The way to the secret chamber is open at the far end of the battlegrounds."));
				return;
			}

			await dialog.Msg(L("A device of Ruklys' own building, and it was broken on his own orders."));
		});

		// The Ruklys Army Soldier's Spirit
		//-------------------------------------------------------------------------
		AddNpc(147399, L("Ruklys Army Soldier's Spirit"), "UNDER69_SQ030_GHOST", "d_underfortress_69", -895.83, 2277.89, 22, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ruklys Army Soldier's Spirit"));

			if (character.Quests.IsActive(Sq030) && character.Quests.IsCompletable(Sq030))
			{
				await dialog.Msg(L("Thank you. Where do you belong to?"));
				await dialog.Msg(L("I should get back to Ruklys now."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				await dialog.Msg(L("I wasn't able to send an order to retreat."));

				var answer = await dialog.SelectQuestOffer(Sq030, L("I should have blown the horn..."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Tell him to see to it himself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq030);
					character.Inventory.Add(ItemId.UNDER69_SQ3_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("If we don't make an order to retreat, they will all be annihilated."));
					await dialog.Msg(L("Please blow the horn.."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("Please blow the horn.."));
				return;
			}

			await dialog.Msg(L("A soldier's spirit still holding a retreat order he never gave."));
		});

		// The Revelation Slate
		//-------------------------------------------------------------------------
		AddConditionalNpc(47234, L("Revelation Slate"), "UNDER69_MQ060", "d_underfortress_69", -2364.02, 42.20, 90, this.IsRevelationWaiting, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Revelation Slate"));

			if (character.Quests.IsActive(Mq060) && !character.Quests.IsCompletable(Mq060))
			{
				character.Quests.StartQuestTrack(Mq060);
				return;
			}

			await dialog.Msg(L("A slate of the goddess' own writing, in a chamber that was sealed before Ruklys fell."));
		});

		// The battlefield traps
		//-------------------------------------------------------------------------
		for (var i = 0; i < TrapSpots.GetLength(0); ++i)
		{
			AddNpc(47107, L("Defensive Battle Field Trap"), TrapNames[i], "d_underfortress_69",
				TrapSpots[i, 0], TrapSpots[i, 1], 90, this.StripTheTrap);
		}

		// Demon Totems
		//-------------------------------------------------------------------------
		for (var i = 0; i < TotemSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(153070, L("Demon Totem"), TotemNames[i], "d_underfortress_69",
				TotemSpots[i, 0], TotemSpots[i, 1], TotemFacings[i], character => this.IsTotemStanding(character, number), async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Demon Totem"));

				if (character.Quests.IsActive(Mq040) && !character.Quests.IsCompletable(Mq040))
				{
					var broken = await character.TimeActions.StartAsync(L("Breaking the totem..."), L("Cancel"), "HAMMERING", TimeSpan.FromSeconds(2));

					if (broken != TimeActionResult.Completed)
						return;

					character.Quests.CompleteObjective(Mq040, "breakTotem" + number);
					character.LookAround();
					character.ServerMessage(L("The totem comes apart."));
					return;
				}

				await dialog.Msg(L("A Vubbe totem driven into the ground beside the foundation stone."));
			});
		}

		// The stones Amanda could carve
		//-------------------------------------------------------------------------
		for (var i = 0; i < Stones.GetLength(0); ++i)
		{
			AddNpc(10043, L("Slab of Stone"), i == 0 ? "UNDER69_SQ030_STONE01" : "UNDER69_SQ030_STONE01_" + (i + 1), "d_underfortress_69",
				Stones[i, 0], Stones[i, 1], StoneFacings[i], this.MeasureStone);
		}

		AddNpc(152034, L("Slab of Stone"), "UNDER69_SQ030_STONE02", "d_underfortress_69", -180.96, -1340.41, -23, this.MeasureStone);

		// The pages of the Ruklys journal
		//-------------------------------------------------------------------------
		for (var i = 0; i < PageSpots.GetLength(0); ++i)
		{
			var itemId = PageItems[i];

			AddConditionalNpc(147312, L("Journal Page"), PageNames[i], "d_underfortress_69",
				PageSpots[i, 0], PageSpots[i, 1], PageFacings[i], character => this.IsPageLying(character, itemId), async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Journal Page"));

				var picked = await character.TimeActions.StartAsync(L("Reading the page..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(2));

				if (picked != TimeActionResult.Completed)
					return;

				character.Inventory.Add(itemId, 1, InventoryAddType.PickUp);
				character.LookAround();
				await dialog.Msg(L("A page of a journal kept by somebody in the Ruklys army."));
			});
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The three rises the retreat horn carries from.
		for (var i = 0; i < HornSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddQuestTrigger("UNDER69_SQ3_GHOST_CALL" + number, "d_underfortress_69", HornSpots[i, 0], HornSpots[i, 1], 100, args => this.BlowTheHorn(args, number));
		}

		// The Dievdirbys Master's carving tools, back at the West Forest.
		AddQuestTrigger("JOB_DIEVDIRBYS2_NPC_TOOLS", "f_siauliai_west", -1076, 425, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Hq1) || character.Quests.IsCompletable(Hq1))
				return;

			var asked = await character.TimeActions.StartAsync(L("Asking for the carving tools..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));

			if (asked != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.UNDER69_HIDDENQ1_ITEM1, 1, InventoryAddType.PickUp);
			character.Quests.CompleteObjective(Hq1, "borrowTheTools");
			character.ServerMessage(L("Sculptor Tesla lends the carving tools. Take them back to Amanda."));
		});
	}

	/// <summary>
	/// Blows the retreat horn from one of the three rises.
	/// </summary>
	/// <param name="args"></param>
	/// <param name="number"></param>
	private async Task BlowTheHorn(TriggerActorArgs args, int number)
	{
		if (args.Initiator is not Character character)
			return;

		if (character.Quests.IsActive(Sq030, "blowHorn" + number))
		{
			character.Quests.CompleteObjective(Sq030, "blowHorn" + number);
			character.ServerMessage(L("The horn carries across the battlegrounds."));
		}

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether Amanda is still at the battleground gate.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheGate(Character character)
		=> !character.Quests.HasCompleted(Mq010);

	/// <summary>
	/// Returns whether Amanda has moved up to the watchtower approach.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAmandaAtTheWatchtower(Character character)
		=> character.Quests.HasCompleted(Mq010);

	/// <summary>
	/// Returns whether Eminent is still waiting on the battleground.
	/// </summary>
	/// <param name="character"></param>
	private bool IsEminentAtTheBattleground(Character character)
		=> !character.Quests.Has(Mq010) || character.Quests.IsActive(Mq010);

	/// <summary>
	/// Returns whether Eminent has moved to Ruklys' device.
	/// </summary>
	/// <param name="character"></param>
	private bool IsEminentAtTheDevice(Character character)
		=> character.Quests.HasCompleted(Mq010) && !character.Quests.IsCompletable(Mq050) && !character.Quests.HasCompleted(Mq050);

	/// <summary>
	/// Returns whether the numbered Demon Totem is still standing.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="number"></param>
	private bool IsTotemStanding(Character character, int number)
	{
		if (!character.Quests.Has(Mq040))
			return true;

		return character.Quests.IsActive(Mq040, "breakTotem" + number);
	}

	/// <summary>
	/// Returns whether the revelation chamber has been opened.
	/// </summary>
	/// <param name="character"></param>
	private bool IsRevelationWaiting(Character character)
		=> character.Quests.Has(Mq060);

	/// <summary>
	/// Returns whether the journal page carrying the given item is still
	/// lying where it fell.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="itemId"></param>
	private bool IsPageLying(Character character, int itemId)
		=> character.Inventory.CountItem(itemId) == 0;

	/// <summary>
	/// Strips a part out of one of the battlefield traps.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task StripTheTrap(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Defensive Battle Field Trap"));

		if (!character.Quests.IsActive(Mq020))
		{
			await dialog.Msg(L("A trap of the fortress' own defence, and it has not been armed for 600 years."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.UNDER69_MQ2_ITEM01) >= PartsNeeded)
		{
			await dialog.Msg(L("You have as many parts as Eminent asked for."));
			return;
		}

		var stripped = await character.TimeActions.StartAsync(L("Taking the part out..."), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(3));

		if (stripped != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.UNDER69_MQ2_ITEM01, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The part comes out whole, which is more than can be said for the trap."));
	}

	/// <summary>
	/// Measures one of the slabs Amanda could carve her account into.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task MeasureStone(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Slab of Stone"));

		if (!character.Quests.IsActive(Sq010) || character.Quests.IsCompletable(Sq010))
		{
			await dialog.Msg(L("A slab of the fortress' own stone, squared off by whoever cut it."));
			return;
		}

		var measured = await character.TimeActions.StartAsync(L("Measuring the stone..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

		if (measured != TimeActionResult.Completed)
			return;

		character.Quests.CompleteObjective(Sq010, "findTheStone");
		character.ServerMessage(L("This one is the right size and far too heavy to carry. Tell Amanda where it is."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50079: Confidence
//-----------------------------------------------------------------------------
public class Underfortress69Mq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50079);
		SetName(L("Confidence"));
		SetDescription(L("The Monocle held on Premier Eminent shows what he really is."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_1", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Premier Eminent does not seem to be human. To reveal his identity, speak with Amanda."));
		SetPhase(QuestStatus.InProgress, "AMANDA_69_1", "d_underfortress_69", L("Inspect Premier Eminent with the monocle"), L("Amanda asks you to see the real Premier Eminent for yourself. Use the monocle to find out the true identity of Premier Eminent."));
		SetPhase(QuestStatus.Success, "AMANDA_69_1", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Premier Eminent is a demon! Consult with Amanda on what to do."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_69_MQ010_TRACK", 2000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50089, QuestStatus.Completed));

		AddObjective("useTheMonocle", L("Inspect Premier Eminent with the monocle"), new ManualObjective());

		AddReward(new ItemReward("UNDER69_MQ1_ITEM01", 1));
		AddReward(new ItemReward("expCard11", 1));
	}
}

// 50080: Repair Parts
//-----------------------------------------------------------------------------
public class Underfortress69Mq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50080);
		SetName(L("Repair Parts"));
		SetDescription(L("The device Ruklys broke can be rebuilt out of the traps on Ataka Side Road."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_69_1", "d_underfortress_69", L("Talk to Premier Eminent"), L("Premier Eminent will be waiting for you. Speak with him to avoid any suspicion."));
		SetPhase(QuestStatus.InProgress, "UNDER69_MQ020_DEVICE01", "d_underfortress_69", L("Look for the parts that are needed to repair the device"), L("Search for the suitable parts from the defensive traps at the battle field on Ataka Side Road."));
		SetPhase(QuestStatus.Success, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("You've got all the parts you can get. Speak with Amanda before returning to Premier Eminent."));

		AddPrerequisite(new QuestStatusPrerequisite(50079, QuestStatus.Completed));

		AddObjective("collectParts", L("Look for the parts that are needed to repair the device"), new CollectItemObjective("UNDER69_MQ2_ITEM01", 5));

		AddReward(new ItemReward("expCard11", 2));
	}
}

// 50081: Preparation (1)
//-----------------------------------------------------------------------------
public class Underfortress69Mq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50081);
		SetName(L("Preparation (1)"));
		SetDescription(L("The Slepti Watchtower foundation stone takes its letters back in demon blood."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Amanda has been working on the ways of the magic circle so far. Speak with Amanda."));
		SetPhase(QuestStatus.InProgress, "UNDER69_MQ3_DEVICE_REPAIR", "d_underfortress_69", L("Collect Demon Blood"), L("To activate the foundation stone, you must re-inscribe the letters with something that has spells in it. Firstly, collect the Demon Blood."));
		SetPhase(QuestStatus.Success, "UNDER69_MQ3_DEVICE_REPAIR", "d_underfortress_69", L("Inscribe the letters with the demon blood"), L("You've collected enough demon blood. Inscribe the letters on the pillars of the foundation stones at the Slepti Watchtower."));

		AddPrerequisite(new QuestStatusPrerequisite(50080, QuestStatus.Completed));

		AddObjective("collectBlood", L("Collect Demon Blood"), new CollectItemObjective("UNDER69_MQ3_ITEM01", 13));
		AddObjective("carveTheStone", L("Inscribe the letters with the demon blood"), new ManualObjective());

		AddPityDrop("UNDER69_MQ3_ITEM01", 0.9f, 3, 1, "flask_blue");

		AddReward(new ItemReward("expCard11", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; carving the stone is the whole of it.
		character.Quests.Complete(this.QuestId);
	}
}

// 50082: Preparation (2)
//-----------------------------------------------------------------------------
public class Underfortress69Mq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50082);
		SetName(L("Preparation (2)"));
		SetDescription(L("The Ikveta Podium stone will not light while the Demon Totems stand around it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("You've activated the foundation stones at the Slepti Watchtower. Ask Amanda what to do next."));
		SetPhase(QuestStatus.InProgress, "UNDER69_MQ4_DEVICE", "d_underfortress_69", L("Destroy the Demon Totems"), L("The foundation stones at the Ikveta Podium are not working properly due to the demon totems around them. Destroy the totems first."));
		SetPhase(QuestStatus.Success, "UNDER69_MQ4_DEVICE", "d_underfortress_69", L("Activate the Foundation Stones"), L("You've destroyed the Demon Totems. Try to activate the foundation stones once again."));

		AddPrerequisite(new QuestStatusPrerequisite(50081, QuestStatus.Completed));

		AddObjective("breakTotem1", L("Destroy the first Demon Totem"), new ManualObjective());
		AddObjective("breakTotem2", L("Destroy the second Demon Totem"), new ManualObjective());
		AddObjective("breakTotem3", L("Destroy the third Demon Totem"), new ManualObjective());
		AddObjective("breakTotem4", L("Destroy the fourth Demon Totem"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 2));
		AddReward(new TakeItemReward("UNDER69_MQ1_ITEM01", 1));
	}
}

// 50083: Eminent's Identity
//-----------------------------------------------------------------------------
public class Underfortress69Mq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50083);
		SetName(L("Eminent's Identity"));
		SetDescription(L("The magic circle lights, Eminent burns in it, and what he summons is left to fight."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "EMINENT_69_2", "d_underfortress_69", L("Talk to Premier Eminent"), L("The defensive magic circle is active. Go back to Premier Eminent before he gets suspicious."));
		SetPhase(QuestStatus.InProgress, "UNDER69_MQ5", "d_underfortress_69", L("Defeat Mandara"), L("The light from the device and the magic circle burns Premier Eminent! For now, defeat Mandara summoned by Premier Eminent!"));
		SetPhase(QuestStatus.Success, "UNDER69_MQ5", "d_underfortress_69", L("Defeat Mandara"), L("Mandara is down and Eminent is gone."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_69_MQ050_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50082, QuestStatus.Completed));

		AddObjective("killMandara", L("Defeat Mandara summoned by Premier Eminent"), new KillObjective(1, "boss_Mandara") { LayerOnly = true });

		AddReward(new ItemReward("expCard11", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Mandara is down. Ruklys' device is standing open."));
		character.Quests.Complete(this.QuestId);
	}
}

// 50084: Revelation of Fortress of the Land
//-----------------------------------------------------------------------------
public class Underfortress69Mq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50084);
		SetName(L("Revelation of Fortress of the Land"));
		SetDescription(L("The device opens the chamber, and Goddess Laima is waiting in it."));
		SetType(QuestType.Main);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "UNDER69_MQ5", "d_underfortress_69", L("Activate the Ruklys device to find the revelation"), L("All you need to do now is to find the revelation. Activate Ruklys' device and find the revelation."));
		SetPhase(QuestStatus.InProgress, "UNDER69_MQ060", "d_underfortress_69", L("Activate the Ruklys device to find the revelation"), L("All you need to do now is to find the revelation. Activate Ruklys' device and find the revelation."));
		SetPhase(QuestStatus.Success, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Found the revelation. Get out of the secret chamber and speak with Amanda."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "UNDERFORTRESS_69_MQ060_TRACK", 4000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(50083, QuestStatus.Completed));

		AddObjective("takeTheRevelation", L("Activate the Ruklys device to find the revelation"), new ManualObjective());

		AddReward(new ItemReward("stonetablet07", 1));
		AddReward(new ItemReward("expCard11", 1));
	}
}

// 50085: The Grave Robber in History
//-----------------------------------------------------------------------------
public class Underfortress69Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50085);
		SetName(L("The Grave Robber in History"));
		SetDescription(L("Amanda is the first grave robber this far into the fortress and wants it written down."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Amanda has some unfinished business in the Fortress of the Land. Speak with Amanda."));
		SetPhase(QuestStatus.InProgress, "UNDER69_SQ030_STONE01", "d_underfortress_69", L("Find stone that can be used to inscribe Amanda's achievements"), L("Find a stone she can inscribe her achievements into."));
		SetPhase(QuestStatus.Success, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Found a perfectly sized stone, but it's too big to carry. Tell Amanda the location of the stone."));

		AddPrerequisite(new QuestStatusPrerequisite(50084, QuestStatus.Completed));

		AddObjective("findTheStone", L("Find stone that can be used to inscribe Amanda's achievements"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 1));
	}
}

// 50086: Traitor's Diary
//-----------------------------------------------------------------------------
public class Underfortress69Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50086);
		SetName(L("Traitor's Diary"));
		SetDescription(L("The pages of a Ruklys army journal are scattered over the whole battleground."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_2", "d_underfortress_69", L("Talk to Grave Robber Amanda"), L("Grave Robber Amanda saw something from you. Speak with Grave Robber Amanda."));
		SetPhase(QuestStatus.InProgress, "UNDER69_PAPER01", "d_underfortress_69", L("Look for the diary of the soldiers of Ruklys"), L("Grave Robber Amanda asks you to bring the pages of Ruklys' Journal to her if you happen to find them."));
		SetPhase(QuestStatus.Success, "AMANDA_69_2", "d_underfortress_69", L("Hand them over to the Grave Robber Amanda"), L("You've found all the pages of the journal. Bring them to Grave Robber Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50084, QuestStatus.Completed));
		AddPrerequisite(new ItemPrerequisite("UNDER69_SQ2_ITEM01"));

		AddObjective("findPage1", L("Look for the first page of the journal"), new CollectItemObjective("UNDER69_SQ2_ITEM01", 1));
		AddObjective("findPage2", L("Look for the second page of the journal"), new CollectItemObjective("UNDER69_SQ2_ITEM02", 1));
		AddObjective("findPage3", L("Look for the third page of the journal"), new CollectItemObjective("UNDER69_SQ2_ITEM03", 1));
		AddObjective("findPage4", L("Look for the fourth page of the journal"), new CollectItemObjective("UNDER69_SQ2_ITEM04", 1));

		AddReward(new ItemReward("expCard11", 2));
		AddReward(new TakeItemReward("UNDER69_SQ2_ITEM01", 1));
		AddReward(new TakeItemReward("UNDER69_SQ2_ITEM02", 1));
		AddReward(new TakeItemReward("UNDER69_SQ2_ITEM03", 1));
		AddReward(new TakeItemReward("UNDER69_SQ2_ITEM04", 1));
	}
}

// 50087: Order to Retreat
//-----------------------------------------------------------------------------
public class Underfortress69Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50087);
		SetName(L("Order to Retreat"));
		SetDescription(L("A soldier's spirit never blew the retreat, and the horn is still where he dropped it."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_69");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "UNDER69_SQ030_GHOST", "d_underfortress_69", L("Talk with the Ruklys Army Soldier's Spirit"), L("You can see the Ruklys Army Soldier's Spirit that can't go back beside the goddess. Talk to him."));
		SetPhase(QuestStatus.InProgress, "UNDER69_SQ3_GHOST_CALL1", "d_underfortress_69", L("Pass the retreat order to the spirits by blowing the horn"), L("Blow the horn and let the other spirits know."));
		SetPhase(QuestStatus.Success, "UNDER69_SQ030_GHOST", "d_underfortress_69", L("Talk with the Ruklys Army Soldier's Spirit"), L("You blew the horn to order the retreat. Tell this to the Ruklys Army Soldier's Spirit."));

		AddPrerequisite(new LevelPrerequisite(204));

		AddObjective("blowHorn1", L("Blow the horn from the first rise"), new ManualObjective());
		AddObjective("blowHorn2", L("Blow the horn from the second rise"), new ManualObjective());
		AddObjective("blowHorn3", L("Blow the horn from the third rise"), new ManualObjective());

		AddReward(new ItemReward("expCard11", 1));
		AddReward(new TakeItemReward("UNDER69_SQ3_ITEM", 1));
	}
}

// 50269: Notable Grave Robbers and Adventurers
//-----------------------------------------------------------------------------
public class Underfortress69Hq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50269);
		SetName(L("Notable Grave Robbers and Adventurers"));
		SetDescription(L("Amanda has her stone and no tools, and the Dievdirbys Master keeps tools."));
		SetType(QuestType.Sub);
		SetLocation("d_underfortress_69", "f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "AMANDA_69_2", "d_underfortress_69", L("Talk to Amanda"), L("Amanda seems to be struggling with something. Talk to her and ask her what's wrong."));
		SetPhase(QuestStatus.InProgress, "JOB_DIEVDIRBYS2_NPC_TOOLS", "f_siauliai_west", L("Ask the Dievdirbys Master for Help"), L("Borrow the carving tools from the Dievdirbys Master and bring them to Amanda."));
		SetPhase(QuestStatus.Success, "AMANDA_69_2", "d_underfortress_69", L("Talk to Amanda"), L("You have obtained the tools from the Dievdirbys Master. Bring them to Amanda."));

		AddPrerequisite(new QuestStatusPrerequisite(50085, QuestStatus.Completed));

		AddObjective("borrowTheTools", L("Ask the Dievdirbys Master for Help"), new ManualObjective());

		AddReward(new TakeItemReward("UNDER69_HIDDENQ1_ITEM1", 1));
	}
}
