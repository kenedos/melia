//--- Melia Script ----------------------------------------------------------
// Aqueduct Bridge Area Quest NPCs
//--- Description -----------------------------------------------------------
// The tenant farmers of Baron Allerno's land, the broken statue they piece
// back together, and the portal mage who carries Hauberk to the prison.
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

public class FFarm472QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Sq010 = new QuestId(40280);
	private readonly static QuestId Sq020 = new QuestId(40290);
	private readonly static QuestId Sq030 = new QuestId(40300);
	private readonly static QuestId Sq040 = new QuestId(40310);
	private readonly static QuestId Sq045 = new QuestId(40315);
	private readonly static QuestId Sq050 = new QuestId(40320);
	private readonly static QuestId Sq060 = new QuestId(40330);
	private readonly static QuestId Sq070 = new QuestId(40340);
	private readonly static QuestId Sq080 = new QuestId(40350);
	private readonly static QuestId Sq081 = new QuestId(40351);
	private readonly static QuestId Sq090 = new QuestId(40360);
	private readonly static QuestId Pre01 = new QuestId(60000);
	private readonly static QuestId Pre02 = new QuestId(60001);

	private const int KindlingNeeded = 5;

	// The withered crops of Tylila Path, replaced once the magic circle is gone.
	private readonly static double[,] WitheredCrops =
	{
		{ 351.92, 1884.95 }, { 317.19, 1859.64 }, { 280.38, 1883.07 }, { 316.25, 1911.12 },
		{ 320.19, 1954.88 }, { 270.03, 1931.05 }, { 235.47, 1938.29 }, { 282.86, 1971.90 },
	};

	private readonly static double[,] HealthyCrops =
	{
		{ 318.13, 1859.57 }, { 352.47, 1883.01 }, { 278.39, 1879.90 }, { 311.95, 1904.36 },
		{ 276.80, 1923.08 }, { 317.88, 1949.37 }, { 239.93, 1939.50 }, { 277.21, 1967.08 },
		{ 82.12, 1838.66 },
	};

	// The scorched ground the burnt magic circle leaves behind.
	private readonly static double[,] BurntLeaves =
	{
		{ -706.24, 856.13 }, { -657.38, 892.45 }, { -659.62, 845.93 }, { -657.02, 798.85 },
		{ -602.14, 835.82 }, { -564.37, 990.47 }, { -511.37, 1028.61 }, { -515.68, 980.42 },
		{ -522.57, 932.96 }, { -468.25, 969.93 }, { -474.70, 921.22 },
	};

	// The old wood the wooden hammer is cut from.
	private readonly static double[,] WoodPieces =
	{
		{ 748.04, -1462.75 }, { -96.26, -1091.48 }, { 66.52, -1376.22 },
		{ 232.32, -907.41 }, { 1038.03, -1273.30 }, { 1029.81, -911.06 },
	};

	private readonly static double[,] Bonfires =
	{
		{ 848.00, -1353.70 }, { 87.35, -919.13 }, { 310.29, -1335.70 }, { 1068.19, -1518.69 },
	};

	protected override void Load()
	{
		// Joana
		//-------------------------------------------------------------------------
		AddNpc(152001, L("Joana"), "FARM47_JOANA", "f_farm_47_2", -89.58, 149.76, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Joana"));

			if (character.Quests.IsActive(Sq010) && character.Quests.IsCompletable(Sq010))
			{
				await dialog.Msg(L("That is no ordinary rock. It is the head of a statue, and a small one at that."));
				await dialog.Msg(L("Keep hold of it. If you find the rest, bring it all to me."));
				await dialog.CompleteQuest(Sq010);
				return;
			}

			if (character.Quests.IsActive(Sq020) && character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("The upper body, this time. Whoever broke it did not want it found whole."));
				await dialog.CompleteQuest(Sq020);
				return;
			}

			if (character.Quests.IsActive(Sq040) && character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("Wings. So it was a statue of the goddess after all."));
				await dialog.Msg(L("That thing you fought was wearing them. I do not like what that says about this land."));
				await dialog.CompleteQuest(Sq040);
				return;
			}

			if (character.Quests.IsActive(Sq045) && character.Quests.IsCompletable(Sq045))
			{
				await dialog.Msg(L("Oh, this... Hold on."));
				await dialog.CompleteQuest(Sq045);
				return;
			}

			if (character.Quests.IsActive(Sq050) && character.Quests.IsCompletable(Sq050))
			{
				await dialog.Msg(L("Yes. That will be enough. Let me try sticking this together first."));
				await dialog.Msg(L("Oh, and one more thing."));
				await dialog.CompleteQuest(Sq050);
				return;
			}

			if (character.Quests.IsActive(Sq060) && character.Quests.IsCompletable(Sq060))
			{
				await dialog.Msg(L("Surely suspicious."));
				await dialog.Msg(L("If that aura is a bad aura, it wouldn't attack a monster, right?"));
				await dialog.CompleteQuest(Sq060);
				return;
			}

			if (character.Quests.IsActive(Sq080) && character.Quests.IsCompletable(Sq080))
			{
				await dialog.Msg(L("You're just in time."));
				await dialog.Msg(L("You said you needed this Goddess Statue, right? Here, take it."));
				await dialog.CompleteQuest(Sq080);
				return;
			}

			if (!character.Quests.Has(Sq045) && character.Quests.MeetsPrerequisites(Sq045))
			{
				var answer = await dialog.SelectQuestOffer(Sq045, L("Wait that.. seems like a familiar statue? Sorry but can I take a look?"),
					Option(L("Show the fragments"), "accept"),
					Option(L("Leave as this seems suspicious"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq045);
					character.Quests.CompleteObjective(Sq045, "showFragments");
					return;
				}
			}

			if (!character.Quests.Has(Sq050) && character.Quests.MeetsPrerequisites(Sq050))
			{
				var answer = await dialog.SelectQuestOffer(Sq050, L("This looks like part of the Goddess Statue that used to stand. Seems like someone destroyed it on purpose... Could it be the baron?"),
					Option(L("I'll help you"), "accept"),
					Option(L("About the things that are occurring here"), "explain"),
					Option(L("I have a long way ahead so I will get going now"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The baron is doing something to purify this farm."));
					await dialog.Msg(L("But recently the crops are dying and people are starting to fall sick."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq050);
					await dialog.Msg(L("First, I better stick the pieces together."));
					await dialog.Msg(L("It may be a little blasphemous but can you get some Sticky Sap from the monsters?"));
					return;
				}
			}

			if (!character.Quests.Has(Sq060) && character.Quests.MeetsPrerequisites(Sq060))
			{
				var answer = await dialog.SelectQuestOffer(Sq060, L("There is an unstable magic circle on the way to Ramus Crossroads. They say to not go near it because a strange aura flows out from it. But it really doesn't seem so."),
					Option(L("I will find it out for him"), "accept"),
					Option(L("About what you are doing here"), "explain"),
					Option(L("Decline"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The baron has a bad reputation for the things he has done."));
					await dialog.Msg(L("And the strange things that are happening nowadays... I'm sure something fishy is going on."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq060);
					await dialog.Msg(L("If you head toward the crossroad, you'll see an oddly sparkling smoke."));
					await dialog.Msg(L("Attack a monster and take it there before killing it. You might find something out, don't you think?"));
					return;
				}
			}

			if (character.Quests.IsActive(Sq050))
			{
				await dialog.Msg(L("I feel like it's soiling the Goddess Statue but I have nothing else so we'll have to settle for that."));
				await dialog.Msg(L("The goddess will forgive us."));
				return;
			}

			if (character.Quests.IsActive(Sq060))
			{
				await dialog.Msg(L("No results yet?"));
				await dialog.Msg(L("There should be some reaction whether good or bad if you check using the monster."));
				return;
			}

			if (character.Quests.IsActive(Sq080))
			{
				await dialog.Msg(L("Varas sent you? Then it is the Goddess Statue you are after."));
				return;
			}

			await dialog.Msg(L("The crops fail a little more every season, and the baron's men keep drawing their circles."));
		});

		// Varas
		//-------------------------------------------------------------------------
		AddNpc(152000, L("Varas"), "FARM47_JONARIS", "f_farm_47_2", 39.36, 1243.89, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Varas"));

			if (character.Quests.IsActive(Sq070) && character.Quests.IsCompletable(Sq070))
			{
				await dialog.Msg(L("Did you do it?"));
				await dialog.Msg(L("That's great. But there are more magic circles."));
				await dialog.CompleteQuest(Sq070);
				return;
			}

			if (character.Quests.IsActive(Sq081) && character.Quests.IsCompletable(Sq081))
			{
				await dialog.Msg(L("Alright. Thank you very much."));
				await dialog.Msg(L("At least now we know the magic circle is bad for sure."));
				await dialog.CompleteQuest(Sq081);
				return;
			}

			if (!character.Quests.Has(Sq070) && character.Quests.MeetsPrerequisites(Sq070))
			{
				var answer = await dialog.SelectQuestOffer(Sq070, L("No matter how much I think about it, the magic circles are the reason the harvest is bad. It's not purifying the land but rather making it worse."),
					Option(L("I will eliminate the magic circle for him"), "accept"),
					Option(L("About the magic circles"), "explain"),
					Option(L("Tell him to leave it since it may be the purification magic circle"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This area used to be a fertile land."));
					await dialog.Msg(L("But ever since the baron's men made the magic circles, harvests decreased a lot."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq070);
					await dialog.Msg(L("I stacked up firewood in front of the magic circle. But it has a protective shield, so even if you light it, the fire will fade out quickly."));
					await dialog.Msg(L("The Orange Dandel carry something that burns well. Take what you need off them first."));
					return;
				}
			}

			if (!character.Quests.Has(Sq080) && character.Quests.MeetsPrerequisites(Sq080))
			{
				var answer = await dialog.SelectQuestOffer(Sq080, L("If the magic circles are really a lie, then you should be able to remove the magic circles at Tylila Path with the goddess' power. I mean something like the Goddess Statue. Is there any way to get it?"),
					Option(L("Joana may have it"), "accept"),
					Option(L("About the possibility that the magic circle may be for the purification"), "explain"),
					Option(L("It won't be here"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("If the magic circles really are for the better, I can just pay more rent and ask for forgiveness."));
					await dialog.Msg(L("We'll know for sure if we check with the goddess' power."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq080);
					character.Quests.CompleteObjective(Sq080, "askJoana");
					await dialog.Msg(L("Indeed Joana has it. Use that power to remove the magic circle."));
					await dialog.Msg(L("Then checking the crops around will confirm it."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq070))
			{
				await dialog.Msg(L("I can't stand paying ridiculous farm rent fees at times like this."));
				await dialog.Msg(L("How can he do that, as a human?"));
				return;
			}

			if (character.Quests.IsActive(Sq080))
			{
				await dialog.Msg(L("The only problem is whether the magic circles are good or bad."));
				await dialog.Msg(L("At least this is better than kowtowing to the baron."));
				return;
			}

			if (character.Quests.IsActive(Sq081))
			{
				await dialog.Msg(L("If the magic circle can't be removed by the power of the goddess, the baron had better be prepared."));
				await dialog.Msg(L("He will probably claim that the power of the goddess is evil. I'll watch what he has to say."));
				return;
			}

			await dialog.Msg(L("Rent goes up, the harvest goes down, and the baron's men keep chalking the ground."));
		});

		// Jugas
		//-------------------------------------------------------------------------
		AddNpc(20117, L("Jugas"), "FARM47_DZIUGAS", "f_farm_47_2", 1408.88, 1969.62, -2, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Jugas"));

			if (character.Quests.IsActive(Sq090) && character.Quests.IsCompletable(Sq090))
			{
				await dialog.Msg(L("Thank you very much."));
				await dialog.Msg(L("I worked so hard to gather it all and they just threw it to the monsters.. Those awful guards.."));
				await dialog.CompleteQuest(Sq090);
				return;
			}

			if (!character.Quests.Has(Sq090) && character.Quests.MeetsPrerequisites(Sq090))
			{
				var answer = await dialog.SelectQuestOffer(Sq090, L("Have you seen the sacks of grain the monsters stole from me? Please get me back my sacks of grain. I have to pay the farm rent."),
					Option(L("I will find it"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq090);
					await dialog.Msg(L("If I don't pay, I will surely be kicked out this time."));
					await dialog.Msg(L("Please help me."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq090))
			{
				await dialog.Msg(L("If I don't pay, I will surely be kicked out this time."));
				await dialog.Msg(L("Please help me."));
				return;
			}

			await dialog.Msg(L("Every sack the monsters drag off is a sack I still owe the baron."));
		});

		// Head of Goddess Statue
		//-------------------------------------------------------------------------
		AddNpc(153050, L("Head of Goddess Statue"), "FARM47_HEAD_D", "f_farm_47_2", 2101.88, -1034.54, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Head of Goddess Statue"));

			if (character.Quests.IsActive(Sq010) && !character.Quests.IsCompletable(Sq010))
			{
				var dug = await character.TimeActions.StartAsync(L("Digging out the statue's head..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

				if (dug != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.FARM47_2_SQ_010_ITEM_1, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("The stone comes free. It is a statue's head, small enough to carry."));
				return;
			}

			if (!character.Quests.Has(Sq010) && character.Quests.MeetsPrerequisites(Sq010))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking around..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(1));

				if (looked != TimeActionResult.Completed)
					return;

				var answer = await dialog.SelectQuestOffer(Sq010, L("Something is half buried in the earth here, and the shape of it is too regular to be a rock."),
					Option(L("Dig it out"), "accept"),
					Option(L("Leave it buried"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq010);
					character.ServerMessage(L("It looks like the head of a statue. Dig it out of the ground."));
					return;
				}
				return;
			}

			await dialog.Msg(L("The hollow where the statue's head lay, empty now."));
		});

		// Old Chest
		//-------------------------------------------------------------------------
		AddNpc(152019, L("Old Chest"), "FARM47_DRUM01_D", "f_farm_47_2", 925.31, -850.04, 14, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Chest"));

			if (character.Quests.IsActive(Sq020) && !character.Quests.IsCompletable(Sq020))
			{
				await dialog.Msg(L("The lid will not lift. Whatever is inside will have to be broken out."));
				character.Quests.ReplayQuestTrack(Sq020);
				return;
			}

			if (!character.Quests.Has(Sq020) && character.Quests.MeetsPrerequisites(Sq020))
			{
				var looked = await character.TimeActions.StartAsync(L("Examining the chest..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				var answer = await dialog.SelectQuestOffer(Sq020, L("An old chest, left out in the field long enough for the boards to grey."),
					Option(L("Open the chest"), "accept"),
					Option(L("Leave the chest alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq020);
					character.ServerMessage(L("Monsters swarm in as you reach for the lid! Fight them off and break the chest open."));
					return;
				}
				return;
			}

			await dialog.Msg(L("The broken boards of the chest, scattered where it stood."));
		});

		// Wooden Wine Cask
		//-------------------------------------------------------------------------
		AddNpc(147458, L("Wooden Wine Cask"), "FARM47_DRUM02_D", "f_farm_47_2", 981.07, -1829.82, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wooden Wine Cask"));

			if (character.Quests.IsActive(Sq030) && character.Quests.IsCompletable(Sq030))
			{
				var broke = await character.TimeActions.StartAsync(L("Breaking the cask open..."), L("Cancel"), "HAMMERING", TimeSpan.FromSeconds(2));

				if (broke != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The staves give way, and the lower body of a small stone statue rolls out."));
				await dialog.CompleteQuest(Sq030);
				return;
			}

			if (!character.Quests.Has(Sq030) && character.Quests.MeetsPrerequisites(Sq030))
			{
				var looked = await character.TimeActions.StartAsync(L("Examining the wine cask..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				var answer = await dialog.SelectQuestOffer(Sq030, L("A wine cask lying well off the path, and something inside it shifts when the staves are tapped."),
					Option(L("Look for something to open it with"), "accept"),
					Option(L("Leave the cask alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq030);
					character.ServerMessage(L("Forcing it would break what is inside. Gather old wood nearby and make a hammer."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq030))
			{
				await dialog.Msg(L("Prising at the staves will only shatter whatever is packed inside. A wooden hammer would do it properly."));
				return;
			}

			await dialog.Msg(L("The staves of the cask, split and lying where they fell."));
		});

		// Wing of Goddess Statue
		//-------------------------------------------------------------------------
		AddNpc(153049, L("Wing of Goddess Statue"), "FARM47_WING_D", "f_farm_47_2", -58.28, -1763.47, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wing of Goddess Statue"));

			if (character.Quests.IsActive(Sq040) && !character.Quests.IsCompletable(Sq040))
			{
				await dialog.Msg(L("The Corrupted is still on its feet. It will not give the wing up until it is down."));
				character.Quests.ReplayQuestTrack(Sq040);
				return;
			}

			if (!character.Quests.Has(Sq040) && character.Quests.MeetsPrerequisites(Sq040))
			{
				var looked = await character.TimeActions.StartAsync(L("Looking closely at the wing..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				var answer = await dialog.SelectQuestOffer(Sq040, L("A carved wing stands out of the ground, and it is not standing in the ground at all - it is worn."),
					Option(L("Take hold of the wing"), "accept"),
					Option(L("Step back from it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq040);
					character.ServerMessage(L("The ground heaves - the Corrupted was wearing the wing all along!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("Torn earth, where the Corrupted pulled itself out of the ground."));
		});

		// Strange Aura
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Strange Aura"), "FARM47_MAGIC_FAKE", "f_farm_47_2", 381.97, -447.32, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Strange Aura"));

			if (character.Quests.IsActive(Sq060) && !character.Quests.IsCompletable(Sq060))
			{
				var tested = await character.TimeActions.StartAsync(L("Testing the strange aura..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (tested != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Sq060, "testAura");
				character.ServerMessage(L("The aura closes over the wounded monster and it simply is not there any more. Tell Joana."));
				return;
			}

			await dialog.Msg(L("Smoke that sparkles, drifting up out of nothing at all."));
		});

		// The magic circle Varas wants burnt, and the ground it leaves behind.
		//-------------------------------------------------------------------------
		AddConditionalNpc(153047, L("Unstable Magic Circle"), "FARM47_MAGIC11", "f_farm_47_2", -668.16, 1062.22, 90, this.IsRamusCircleStanding, async dialog =>
		{
			await dialog.Msg(L("Chalk and ground bone, laid out in a ring. The soil inside it is grey."));
		});

		for (var i = 0; i < BurntLeaves.GetLength(0); ++i)
		{
			AddConditionalNpc(47201, L("Scorched Ground"), i == 0 ? "FARM47_2_LEAVES" : "FARM47_2_LEAVES_" + (i + 1), "f_farm_47_2",
				BurntLeaves[i, 0], BurntLeaves[i, 1], 90, this.IsRamusCircleBurnt);
		}

		// Stacked Firewood
		//-------------------------------------------------------------------------
		AddNpc(47223, L("Stacked Firewood"), "FARM47_FIRE_ON", "f_farm_47_2", -627.15, 1065.66, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Stacked Firewood"));

			if (character.Quests.IsActive(Sq070) && !character.Quests.IsCompletable(Sq070))
			{
				if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_070_ITEM_1) < KindlingNeeded)
				{
					await dialog.Msg(L("The shield over the circle smothers a plain fire at once. It needs kindling that burns hard - the Orange Dandel carry it."));
					return;
				}

				var lit = await character.TimeActions.StartAsync(L("Lighting the fire..."), L("Cancel"), "FIRE", TimeSpan.FromSeconds(3));

				if (lit != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Sq070, "burnCircle");
				character.Quests.StartQuestTrack(Sq070);
				return;
			}

			await dialog.Msg(L("Varas' woodpile, stacked right up against the ring of the circle."));
		});

		// Old wood
		//-------------------------------------------------------------------------
		for (var i = 0; i < WoodPieces.GetLength(0); ++i)
		{
			AddNpc(151031, L("Old Wood"), i == 0 ? "FARM47_OLD_WOOD" : "FARM47_OLD_WOOD_" + (i + 1), "f_farm_47_2",
				WoodPieces[i, 0], WoodPieces[i, 1], 90, this.CutBluntPiece);
		}

		for (var i = 0; i < Bonfires.GetLength(0); ++i)
		{
			AddNpc(46011, L("Burnt-out Fire"), "f_farm_47_2",
				Bonfires[i, 0], Bonfires[i, 1], 90, this.CutRodPiece);
		}

		// The magic circle of Tylila Path, and the crops that answer for it.
		//-------------------------------------------------------------------------
		AddConditionalNpc(153047, L("Magic Circle"), "FARM47_MAGIC12", "f_farm_47_2", 63.23, 1669.86, 90, this.IsTylilaCircleStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Circle"));

			if (!character.Quests.Has(Sq081) && character.Quests.MeetsPrerequisites(Sq081))
			{
				var answer = await dialog.SelectQuestOffer(Sq081, L("The circle at Tylila Path, and the withered field beyond it."),
					Option(L("Set the Goddess Statue on the magic circle"), "accept"),
					Option(L("Keep the statue for now"), "leave")
				);

				if (answer != "accept")
					return;

				var placed = await character.TimeActions.StartAsync(L("Placing the Goddess Statue on the magic circle..."), L("Cancel"), "SITGROPE_LOOP", TimeSpan.FromSeconds(2));

				if (placed != TimeActionResult.Completed)
					return;

				character.Inventory.RemoveItem(ItemId.FARM47_2_SQ_080_ITEM_1, 1);
				character.Quests.Start(Sq081);
				character.LookAround();
				character.ServerMessage(L("The circle goes out, and the statue with it. Go to the next field and check the crops!"));
				return;
			}

			await dialog.Msg(L("Chalk laid in a ring, and the field past it is dying."));
		});

		for (var i = 0; i < WitheredCrops.GetLength(0); ++i)
		{
			AddConditionalNpc(47200, L("Withered Crop"), i == 0 ? "FARM47_2_CROP" : "FARM47_2_CROP_" + (i + 1), "f_farm_47_2",
				WitheredCrops[i, 0], WitheredCrops[i, 1], 90, this.IsTylilaCircleStanding);
		}

		for (var i = 0; i < HealthyCrops.GetLength(0); ++i)
		{
			AddConditionalNpc(47200, L("Crop"), i == 0 ? "FARM47_NORMAL_CROP" : "FARM47_NORMAL_CROP_" + (i + 1), "f_farm_47_2",
				HealthyCrops[i, 0], HealthyCrops[i, 1], 90, this.IsTylilaCircleGone);
		}

		// Asta
		//-------------------------------------------------------------------------
		AddConditionalNpc(154002, L("[Baron Allerno]{nl}Asta"), "VELNIASP511_PORTAL_MAGE", "f_farm_47_2", -1581, -1259, 195, this.IsAstaAtThePortal, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Asta"));

			if (!character.Quests.Has(Pre01) && character.Quests.MeetsPrerequisites(Pre01))
			{
				var answer = await dialog.SelectQuestOffer(Pre01, L("At last, I meet the one whom my spirit has been looking for. I am the Demon Lord Hauberk. I have a proposal for you."),
					Option(L("I'll help you"), "accept"),
					Option(L("I won't deal with the demons"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Pre01);
					character.Quests.CompleteObjective(Pre01, "hearHauberk");
					character.LookAround();
					character.ServerMessage(L("The mage's body slumps, and the Demon Lord steps out of it."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Pre01))
			{
				await dialog.Msg(L("The mage stands with his eyes shut. Whatever spoke through him is standing beside him now."));
				return;
			}

			await dialog.Msg(L("A portal mage of the baron's household, and something older looking out through him."));
		});

		// Demon Lord Hauberk
		//-------------------------------------------------------------------------
		AddConditionalNpc(57840, L("Demon Lord Hauberk"), "VELNIASP511_PORTAL_HAUBERK", "f_farm_47_2", -1581, -1259, 135, this.IsHauberkAtThePortal, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Lord Hauberk"));
			dialog.SetPortrait("Dlg_port_Hauberk_dark");

			if (character.Quests.IsActive(Pre01) && character.Quests.IsCompletable(Pre01))
			{
				await dialog.Msg(L("Alright. The deal is established."));
				await dialog.Msg(L("Do not worry. I won't hurt you."));
				await dialog.CompleteQuest(Pre01);
				return;
			}

			if (!character.Quests.Has(Pre02) && character.Quests.MeetsPrerequisites(Pre02))
			{
				var answer = await dialog.SelectQuestOffer(Pre02, L("Strange. Unlike other humans, I can't possess your body."),
					Option(L("I will do that"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Pre02);
					character.Inventory.Add(ItemId.VPRISON_HAUBERK_SEAL, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Carry this instead. It is a piece of my seal, and it will hold me as far as the prison."));
					await dialog.Msg(L("The portal stands to the north. Take me through it, into the first district."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Pre02))
			{
				await dialog.Msg(L("The portal is to the north. Vakarine will not wait on your courage forever."));
				return;
			}

			await dialog.Msg(L("A demon lord wearing a borrowed body, and waiting for the portal to open."));
		});

		// Demon Prison District 1
		//-------------------------------------------------------------------------
		AddNpc(154069, L("Demon Prison District 1"), "FARM_47_2_TO_VELNIASP511", "f_farm_47_2", -1615.76, -1192.17, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Prison District 1"));

			if (!character.Quests.IsActive(Pre02))
			{
				await dialog.Msg(L("The portal is shut, and nothing on this side will open it."));
				return;
			}

			await dialog.Msg(L("The seal in your pack warms, and the portal takes the shape of a door."));
			character.Warp("d_velniasprison_51_1", -160, 167, 140);
		});

		// Baron Secretary Andol's Journal
		//-------------------------------------------------------------------------
		AddNpc(147311, L("Baron Secretary Andol's Journal"), "FARM47_2_DIARY", "f_farm_47_2", -1141.06, -1267.80, 90, async dialog =>
		{
			dialog.SetTitle(L("Baron Secretary Andol's Journal"));

			await dialog.Msg(L("...the baron will not be told that the circles are not purifying anything. He has spent too much on them to hear it."));
			await dialog.Msg(L("...the mage says the prison holds something that would finish the work in a night. I have written that I advised against it."));
		});

		// Crumbling Document
		//-------------------------------------------------------------------------
		AddNpc(147312, L("Crumbling Document"), "VPRISON_PAPER02", "f_farm_47_2", -1350.68, -1162.00, 90, async dialog =>
		{
			dialog.SetTitle(L("Crumbling Document"));

			await dialog.Msg(L("An order of transfer, half eaten away. Only the seal of the Demon Prison is still whole."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The field Varas wanted checked once the Tylila Path circle is gone.
		AddQuestTrigger("FARM47_CHECK", "f_farm_47_2", 314.54, 1907.82, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq081) && !character.Quests.IsCompletable(Sq081))
			{
				character.Quests.CompleteObjective(Sq081, "checkCrops");
				character.ServerMessage(L("The crops here are standing green. Report it to Varas."));
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the Ramus Crossroads magic circle is still standing.
	/// </summary>
	/// <param name="character"></param>
	private bool IsRamusCircleStanding(Character character)
		=> !character.Quests.HasCompleted(Sq070);

	/// <summary>
	/// Returns whether the Ramus Crossroads magic circle has been burnt.
	/// </summary>
	/// <param name="character"></param>
	private bool IsRamusCircleBurnt(Character character)
		=> character.Quests.HasCompleted(Sq070);

	/// <summary>
	/// Returns whether the Tylila Path magic circle is still standing.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTylilaCircleStanding(Character character)
		=> !character.Quests.Has(Sq081);

	/// <summary>
	/// Returns whether the Tylila Path magic circle has been dispelled.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTylilaCircleGone(Character character)
		=> character.Quests.Has(Sq081);

	/// <summary>
	/// Returns whether the portal mage is still himself.
	/// </summary>
	/// <param name="character"></param>
	private bool IsAstaAtThePortal(Character character)
		=> !character.Quests.Has(Pre01) || character.Quests.IsActive(Pre01);

	/// <summary>
	/// Returns whether the Demon Lord has stepped out of the mage.
	/// </summary>
	/// <param name="character"></param>
	private bool IsHauberkAtThePortal(Character character)
		=> character.Quests.Has(Pre01) && !character.Quests.HasCompleted(Pre02);

	/// <summary>
	/// Hands out the blunt piece the wooden hammer's head is cut from.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CutBluntPiece(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Old Wood"));

		if (!character.Quests.IsActive(Sq030))
		{
			await dialog.Msg(L("A length of old wood, dry enough to work."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_030_ITEM_2) == 0)
		{
			character.Inventory.Add(ItemId.FARM47_2_SQ_030_ITEM_2, 1, InventoryAddType.PickUp);
			await dialog.Msg(L("You break off a blunt piece, about the weight of a hammer's head."));
		}
		else
		{
			await dialog.Msg(L("You have the head of the hammer. A shaft would do next - something burnt through is easier to trim."));
		}

		this.TryMakeHammer(character);
	}

	/// <summary>
	/// Hands out the rod piece the wooden hammer's shaft is cut from.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CutRodPiece(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Burnt-out Fire"));

		if (!character.Quests.IsActive(Sq030))
		{
			await dialog.Msg(L("A fire someone let go out, with half-burnt wood still in it."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_030_ITEM_3) == 0)
		{
			character.Inventory.Add(ItemId.FARM47_2_SQ_030_ITEM_3, 1, InventoryAddType.PickUp);
			await dialog.Msg(L("You pull a straight rod out of the ashes and trim it down."));
		}
		else
		{
			await dialog.Msg(L("You have the shaft already. A blunt piece for the head is what is missing."));
		}

		this.TryMakeHammer(character);
	}

	/// <summary>
	/// Puts the two pieces together once the player is carrying both.
	/// </summary>
	/// <param name="character"></param>
	private void TryMakeHammer(Character character)
	{
		if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_030_ITEM_4) != 0)
			return;

		if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_030_ITEM_2) == 0)
			return;

		if (character.Inventory.CountItem(ItemId.FARM47_2_SQ_030_ITEM_3) == 0)
			return;

		character.Inventory.Add(ItemId.FARM47_2_SQ_030_ITEM_4, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("You fit the head onto the shaft. The wooden hammer will open the cask."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 40280: Unexpected Discovery (1)
//-----------------------------------------------------------------------------
public class Farm472Sq010Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40280);
		SetName(L("Unexpected Discovery (1)"));
		SetDescription(L("Something shaped like a statue's head lies half buried beside the aqueduct road."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_HEAD_D", "f_farm_47_2", L("Investigate the suspicious area"), L("An object that looks like the head part of the sculpture is halfway buried under the ground. Go there and dig it out."));
		SetPhase(QuestStatus.InProgress, "FARM47_HEAD_D", "f_farm_47_2", L("Collect the head of the statue"), L("It is strange that there is the head of a sculpture in this kind of place. Dig it."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Obtain the information of the object"), L("The thing that was buried under the ground was not a Goddess Statue, but in fact the head part. You better tell this to someone."));

		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("digHead", L("Collect the head of the statue"), new CollectItemObjective("FARM47_2_SQ_010_ITEM_1", 1));

		AddReward(new ItemReward("expCard6", 1));
	}
}

// 40290: Unexpected Discovery (2)
//-----------------------------------------------------------------------------
public class Farm472Sq020Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40290);
		SetName(L("Unexpected Discovery (2)"));
		SetDescription(L("An old chest stands in the field, and it will not open without breaking."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_DRUM01_D", "f_farm_47_2", L("Investigate the old chest"), L("You can see the old chest. Get near it and open it."));
		SetPhase(QuestStatus.InProgress, "FARM47_DRUM01_D", "f_farm_47_2", L("Break the chest"), L("As you tried to open the chest, monsters appeared and intervened. Avoid the attacks of the monsters and break the chest to open it within the time frame."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Look for a person who would find the clues"), L("A small piece that looks like it's from the top of a statue is in the destroyed chest. Take it to someone to find out what it is."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FARM47_2_SQ_020_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("breakChest", L("Break the chest"), new CollectItemObjective("FARM47_2_SQ_020_ITEM_1", 1));

		AddPityDrop("FARM47_2_SQ_020_ITEM_1", 1.0f, 0, 1, "TreasureBox1_1");

		AddReward(new ItemReward("expCard6", 2));
	}
}

// 40300: Unexpected Discovery (3)
//-----------------------------------------------------------------------------
public class Farm472Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40300);
		SetName(L("Unexpected Discovery (3)"));
		SetDescription(L("The wine cask has to be opened carefully, so a wooden hammer is wanted first."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_DRUM02_D", "f_farm_47_2", L("Investigate the Wooden Wine Cask"), L("It's curious that a wine cask is lying off the beaten path. Go and take a look at it."));
		SetPhase(QuestStatus.InProgress, "FARM47_OLD_WOOD", "f_farm_47_2", L("Make the wooden hammer and open the wine cask."), L("The wine cask could contain fragile objects, so it should not be carelessly opened. Gather some materials nearby that could be used to make a wooden hammer and open the wine cask."));
		SetPhase(QuestStatus.Success, "FARM47_DRUM02_D", "f_farm_47_2", L("Look for clues"), L("You need a person who would listen about the piece of the stone statue that was discovered in the wine cask. Look for that person and ask him about it."));

		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("cutBlunt", L("Collect Blunt Wooden Piece"), new CollectItemObjective("FARM47_2_SQ_030_ITEM_2", 1));
		AddObjective("cutRod", L("Collect Wooden Rod Piece"), new CollectItemObjective("FARM47_2_SQ_030_ITEM_3", 1));
		AddObjective("makeHammer", L("Make the Wooden Hammer"), new CollectItemObjective("FARM47_2_SQ_030_ITEM_4", 1));

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new ItemReward("FARM47_2_SQ_030_ITEM_1", 1));
		AddReward(new TakeItemReward("FARM47_2_SQ_030_ITEM_4"));
		AddReward(new TakeItemReward("FARM47_2_SQ_030_ITEM_2"));
		AddReward(new TakeItemReward("FARM47_2_SQ_030_ITEM_3"));
	}
}

// 40310: Unexpected Discovery (4)
//-----------------------------------------------------------------------------
public class Farm472Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40310);
		SetName(L("Unexpected Discovery (4)"));
		SetDescription(L("The carved wing standing out of the ground belongs to the Corrupted wearing it."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_WING_D", "f_farm_47_2", L("Look closely at the wing that is stuck on the ground"), L("There is a sculpture stuck on the ground that looks like wings. Get closer to it and look at it."));
		SetPhase(QuestStatus.InProgress, "FARM47_WING_D", "f_farm_47_2", L("Defeat Corrupted"), L("The wing that was stuck on the ground belonged to Corrupted. Defeat Corrupted."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Find the person who knows about the fragments"), L("There may be a person who knows about the small wing piece that you obtained from Corrupted. Find that person and show this piece to him."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FARM47_2_SQ_040_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("killCorrupted", L("Defeat Corrupted"), new KillObjective(1, "boss_Fallen_Statue_Q2") { LayerOnly = true });

		AddPityDrop("FARM47_2_SQ_040_ITEM_1", 1.0f, 0, 1, "boss_Fallen_Statue_Q2");

		AddReward(new ItemReward("expCard6", 3));
	}
}

// 40315: Reading the Clue
//-----------------------------------------------------------------------------
public class Farm472Sq045Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40315);
		SetName(L("Reading the Clue"));
		SetDescription(L("Joana recognises the statue the four fragments make up."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_JOANA", "f_farm_47_2", L("Find the person who has a clue"), L("Find someone who knows about the fragments."));
		SetPhase(QuestStatus.InProgress, "FARM47_JOANA", "f_farm_47_2", L("Find the person who has a clue"), L("Find someone who knows about the fragments."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Talk to Joana"), L("Talk to Joana."));

		AddPrerequisite(new QuestStatusPrerequisite(40280, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(40290, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(40300, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(40310, QuestStatus.Completed));
		AddPrerequisite(new ItemPrerequisite("FARM47_2_SQ_010_ITEM_1"));
		AddPrerequisite(new ItemPrerequisite("FARM47_2_SQ_020_ITEM_1"));
		AddPrerequisite(new ItemPrerequisite("FARM47_2_SQ_030_ITEM_1"));
		AddPrerequisite(new ItemPrerequisite("FARM47_2_SQ_040_ITEM_1"));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("showFragments", L("Talk to Joana"), new ManualObjective());

		AddReward(new TakeItemReward("FARM47_2_SQ_010_ITEM_1"));
		AddReward(new TakeItemReward("FARM47_2_SQ_020_ITEM_1"));
		AddReward(new TakeItemReward("FARM47_2_SQ_030_ITEM_1"));
		AddReward(new TakeItemReward("FARM47_2_SQ_040_ITEM_1"));
	}
}

// 40320: The Irregular Stone Statue
//-----------------------------------------------------------------------------
public class Farm472Sq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40320);
		SetName(L("The Irregular Stone Statue"));
		SetDescription(L("Joana means to glue the statue back together, and wants sap off the monsters for it."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_JOANA", "f_farm_47_2", L("Talk to Joana"), L("Joana seems to know something about this sculpture. Talk to Joana about the sculpture."));
		SetPhase(QuestStatus.InProgress, "FARM47_JOANA", "f_farm_47_2", L("Obtain some Sticky Sap"), L("Joana wants to glue the sculpture together. Obtain some Sticky Sap from the monsters nearby."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Hand it over to Joana"), L("Acquired the Sticky Sap from the monsters. Hand it over to Joana."));

		AddPrerequisite(new QuestStatusPrerequisite(40315, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("collectSap", L("Obtain Sticky Sap from the monsters"), new CollectItemObjective("FARM47_2_SQ_050_ITEM_1", 9));

		AddPityDrop("FARM47_2_SQ_050_ITEM_1", 0.7f, 3, 1, "dandel_orange", "Ashrong", "Kepari_mage", "Cronewt_mage");

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new TakeItemReward("FARM47_2_SQ_050_ITEM_1"));
	}
}

// 40330: A Blessing After All
//-----------------------------------------------------------------------------
public class Farm472Sq060Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40330);
		SetName(L("A Blessing After All"));
		SetDescription(L("Joana wants the strange aura near Ramus Crossroads tested on a monster."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_JOANA", "f_farm_47_2", L("Talk to Joana"), L("It seems that Joana still has some request. Talk to Joana."));
		SetPhase(QuestStatus.InProgress, "FARM47_MAGIC_FAKE", "f_farm_47_2", L("Verifying the nature of the strange aura"), L("Joana wants to find out what the strange aura near Ramus Crossroads is. Attack the monster to drop its HP below 50%, then bring it to the strange aura below the magic circle and interact with it."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Report to Joana"), L("The energy may not be bad since the monsters are disappearing. Tell this to Joana."));

		AddPrerequisite(new QuestStatusPrerequisite(40320, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("testAura", L("Verifying the nature of the strange aura"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 3));
	}
}

// 40340: Positive Evidence (1)
//-----------------------------------------------------------------------------
public class Farm472Sq070Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40340);
		SetName(L("Positive Evidence (1)"));
		SetDescription(L("Varas' woodpile will not burn the magic circle without kindling that burns hard."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_JONARIS", "f_farm_47_2", L("Talk to Varas"), L("It seems that Varas needs your help. Talk to Varas."));
		SetPhase(QuestStatus.InProgress, "FARM47_FIRE_ON", "f_farm_47_2", L("Destroy the magic circle"), L("Pile up the logs on the magic circle and light them to burn the magic circle. You can increase the life of the fire by using kindling from the monsters."));
		SetPhase(QuestStatus.Success, "FARM47_JONARIS", "f_farm_47_2", L("Report to Varas"), L("You've burnt the magic circle successfully. Return to Varas and report."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FARM47_2_SQ_070_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(40330, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("gatherKindling", L("Obtain kindling from the Orange Dandel"), new CollectItemObjective("FARM47_2_SQ_070_ITEM_1", 5));
		AddObjective("burnCircle", L("Destroy the magic circle"), new ManualObjective());

		AddPityDrop("FARM47_2_SQ_070_ITEM_1", 1.0f, 0, 1, "dandel_orange");

		AddReward(new ItemReward("expCard6", 2));
		AddReward(new TakeItemReward("FARM47_2_SQ_070_ITEM_1"));
	}
}

// 40350: Positive Evidence (2)
//-----------------------------------------------------------------------------
public class Farm472Sq080Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40350);
		SetName(L("Positive Evidence (2)"));
		SetDescription(L("The remaining circles will come down faster with the goddess' own power, and Joana has a statue."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_JONARIS", "f_farm_47_2", L("Talk to Varas"), L("Varas told you that there are more magic circles. Talk to Varas again."));
		SetPhase(QuestStatus.InProgress, "FARM47_JOANA", "f_farm_47_2", L("Obtain the Goddess Statue from Joana"), L("Varas told you that you would be able to eliminate the magic circles easily using the mighty power of the Goddesses. Obtain the Goddess Statue from Joana."));
		SetPhase(QuestStatus.Success, "FARM47_JOANA", "f_farm_47_2", L("Obtain the Goddess Statue from Joana"), L("Varas told you that you would be able to eliminate the magic circles easily using the mighty power of the Goddesses. Obtain the Goddess Statue from Joana."));

		AddPrerequisite(new QuestStatusPrerequisite(40340, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("askJoana", L("Obtain the Goddess Statue from Joana"), new ManualObjective());

		AddReward(new ItemReward("FARM47_2_SQ_080_ITEM_1", 1));
	}
}

// 40351: Positive Evidence (3)
//-----------------------------------------------------------------------------
public class Farm472Sq081Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40351);
		SetName(L("Positive Evidence (3)"));
		SetDescription(L("Set the Goddess Statue on the Tylila Path circle, then see what the field does."));
		SetType(QuestType.Sub);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_MAGIC12", "f_farm_47_2", L("Put the Goddess Statue on the magic circle at Tylila Path"), L("Place the Goddess Statue you got from Joana on the magic circle."));
		SetPhase(QuestStatus.InProgress, "FARM47_CHECK", "f_farm_47_2", L("Check the nearby crops"), L("As you put the statue of the goddess on the magic circle, the magic circle disappeared with the Goddess Statue. Check the status of the crops that Varas was worried about."));
		SetPhase(QuestStatus.Success, "FARM47_JONARIS", "f_farm_47_2", L("Report to Varas"), L("The crops became alive again. With the power of the Goddess Statue, the magic circles have disappeared and the crops became alive again. Report this to Varas."));

		AddPrerequisite(new QuestStatusPrerequisite(40350, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("checkCrops", L("Check the nearby crops"), new ManualObjective());

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new SelectItemReward("R_LEG02_172", "R_LEG02_173", "R_LEG02_174"));
	}
}

// 40360: Restoring Willpower!
//-----------------------------------------------------------------------------
public class Farm472Sq090Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40360);
		SetName(L("Restoring Willpower!"));
		SetDescription(L("The monsters dragged Jugas' grain off, and the farm rent is still owed."));
		SetType(QuestType.Repeat);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FARM47_DZIUGAS", "f_farm_47_2", L("Talk to Jugas"), L("There's a farmer who needs your help. Talk to Jugas."));
		SetPhase(QuestStatus.InProgress, "FARM47_DZIUGAS", "f_farm_47_2", L("Look for the Sack of Grain"), L("Jugas wants you to retrieve the sacks of grain from the monsters. Look for the sacks of grain from the nearby monsters."));
		SetPhase(QuestStatus.Success, "FARM47_DZIUGAS", "f_farm_47_2", L("Give it to Jugas"), L("You've found the Sack of Grain. Give it to Jugas."));

		AddPrerequisite(new LevelPrerequisite(79));

		AddObjective("findGrain", L("Retrieve the Sack of Grain from the monsters"), new CollectItemObjective("FARM47_2_SQ_090_ITEM_1", 1));

		AddPityDrop("FARM47_2_SQ_090_ITEM_1", 0.1f, 15, 1, "dandel_orange", "Ashrong", "Cronewt_mage", "Kepari_mage");

		AddReward(new ItemReward("expCard6", 1));
		AddReward(new TakeItemReward("FARM47_2_SQ_090_ITEM_1"));
	}
}

// 60000: A Place Unreachable (1)
//-----------------------------------------------------------------------------
public class Vprison511MqPre01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60000);
		SetName(L("A Place Unreachable (1)"));
		SetDescription(L("The portal mage at the restricted area is carrying the Demon Lord Hauberk, who has an offer."));
		SetType(QuestType.Main);
		SetLocation("f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VELNIASP511_PORTAL_MAGE", "f_farm_47_2", L("Talk to Hasta"), L("Meet Hasta at Aqueduct Bridge Area's Restricted Area."));
		SetPhase(QuestStatus.InProgress, "VELNIASP511_PORTAL_HAUBERK", "f_farm_47_2", L("Talk to Demon Lord Hauberk"), L("The Demon Lord Hauberk whose spirit was inside Hasta appeared. Hauberk told you that he knows where Goddess Vakarine is. Ask him about Goddess Vakarine's location."));
		SetPhase(QuestStatus.Success, "VELNIASP511_PORTAL_HAUBERK", "f_farm_47_2", L("Talk to Demon Lord Hauberk"), L("You have found out that Goddess Vakarine is in trouble. The Demon Lord Hauberk told you that if you could take him to the Demon's Prison across the portal, he will cooperate with you so that you could meet Vakarine. Listen for more details from Hauberk."));

		AddPrerequisite(new LevelPrerequisite(141));

		AddObjective("hearHauberk", L("Talk to Demon Lord Hauberk"), new ManualObjective());
	}
}

// 60001: A Place Unreachable (2)
//-----------------------------------------------------------------------------
public class Vprison511MqPre02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60001);
		SetName(L("A Place Unreachable (2)"));
		SetDescription(L("Carry Hauberk's seal through the portal and into the first district of the Demon Prison."));
		SetType(QuestType.Main);
		SetLocation("f_farm_47_2", "d_velniasprison_51_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "VELNIASP511_PORTAL_HAUBERK", "f_farm_47_2", L("Talk to Demon Lord Hauberk"), L("Listen to Hauberk's plan."));
		SetPhase(QuestStatus.InProgress, "FARM_47_2_TO_VELNIASP511", "f_farm_47_2", L("Enter the 1st District of the Demon's Prison"), L("We've decided to side with Hauberk until we find and rescue Goddess Vakarine. Now bring Hauberk and head to the 1st District of Demon Prison!"));
		SetPhase(QuestStatus.Success, "VPRISON511_MQ_01_NPC", "d_velniasprison_51_1", L("Enter the 1st District of the Demon's Prison"), L("We've decided to side with Hauberk until we find and rescue Goddess Vakarine. Now bring Hauberk and head to the 1st District of Demon Prison!"));

		AddPrerequisite(new QuestStatusPrerequisite(60000, QuestStatus.Completed));

		AddObjective("enterPrison", L("Enter the 1st District of the Demon's Prison"), new ManualObjective());
	}
}
