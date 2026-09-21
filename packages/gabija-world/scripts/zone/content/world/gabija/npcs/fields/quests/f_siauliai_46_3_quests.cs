//--- Melia Script ----------------------------------------------------------
// Vilna Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Maras' investigation of the Vilna Forest monsters, and the Austeja altars
// the monsters were actually coming for.
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

public class FSiauliai463QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(16200);
	private readonly static QuestId Mq02 = new QuestId(16210);
	private readonly static QuestId Mq03 = new QuestId(16220);
	private readonly static QuestId Mq04 = new QuestId(16230);
	private readonly static QuestId Mq05 = new QuestId(16240);
	private readonly static QuestId Sq01 = new QuestId(16300);
	private readonly static QuestId Sq02 = new QuestId(16310);
	private readonly static QuestId Sq03 = new QuestId(16320);
	private readonly static QuestId Sq04 = new QuestId(16330);

	private const int CombsToCollect = 6;
	private const int FieldsToSow = 4;

	// The beehives of the Saldus Bee Farm.
	private readonly static double[,] Beehives =
	{
		{ -409.01, 1778.23 }, { -601.89, 1483.21 }, { -99.32, 1636.91 }, { -243.53, 1387.97 },
	};

	private readonly static double[] BeehiveFacings = { 0, 45, -35, 180 };

	// The fields of the Radanza Farm.
	private readonly static double[,] Fields =
	{
		{ 1502.15, 1573.56 }, { 1520.34, 1502.06 }, { 1691.75, 1474.50 }, { 1686.26, 1547.95 },
	};

	protected override void Load()
	{
		// Maras
		//-------------------------------------------------------------------------
		AddNpc(147477, L("Maras"), "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", -568.40, 488.61, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Maras"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Oh, this one is quite big."));
				await dialog.Msg(L("Good job."));
				await dialog.CompleteQuest(Mq01);
				character.Quests.Start(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("How was it? How did they react?"));
				await dialog.CompleteQuest(Mq02);
				character.Quests.Start(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("You came back faster than I thought."));
				await dialog.Msg(L("So. Did you have a look?"));
				await dialog.CompleteQuest(Mq03);
				character.Quests.Start(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("You there."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("Are you with that group of Revelators that ran away?"),
					Option(L("I'll do it"), "accept"),
					Option(L("Reason behind hiring Revelators"), "explain"),
					Option(L("I'm not interested"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I made a request to Klaipeda, but they have not been responding since they lack the manpower."));
					await dialog.Msg(L("And it is not easy to hire mercenaries in this area."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("I don't know what the priests are doing, because anyone would know that the monsters are after the honey."));
					await dialog.Msg(L("Look carefully for undamaged beehives."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("Now, go to Vulvini Farm. If we could provide proof that the monsters are after the honey, the priests should stop only thinking about their altars."),
					Option(L("I'll check if the monsters are really attracted to the scent of honey"), "accept"),
					Option(L("About the Altar"), "explain"),
					Option(L("Decline"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("There are many altars around here."));
					await dialog.Msg(L("You know about them already, no? They were all created to praise Goddess Austeja."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("With a beehive that huge, you will definitely lure monsters."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("No response? Darn.. So they weren't just going after the honey."));

				var answer = await dialog.SelectQuestOffer(Mq03, L("If they're not after the honey, then there's only one thing left it could be."),
					Option(L("I'll check Bichiu Altar"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("You will find Bichiu Altar if you go down the road from here."));
					await dialog.Msg(L("If you see something suspicious, please bring it to me."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("So the monsters were after this particular piece? This is also my first time seeing it.. I guess we should check Gaudeji Altar on the right side as well then."),
					Option(L("I'll check"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("If you find something like this again, please hand it over to Lamar near the Wood Watch Tower."));
					await dialog.Msg(L("I.. really don't know anything about those."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Look carefully for undamaged beehives."));
				await dialog.Msg(L("With luck, you might obtain some beehives that are in good condition."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("With a beehive that huge, you will definitely lure monsters."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("You will find Bichiu Altar if you go down the road from here."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("If you find something like this again, please hand it over to Lamar near the Wood Watch Tower."));
				return;
			}

			await dialog.Msg(L("An investigator with no manpower, no mercenaries and a forest full of monsters."));
		});

		// Lamar
		//-------------------------------------------------------------------------
		AddNpc(147499, L("Lamar"), "SIAULIAI_46_3_MQ05_NPC", "f_siauliai_46_3", 1878.78, 856.36, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lamar"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Maras told you to show me this?"));
				await dialog.Msg(L("This is the very object that the priest stressed to us not to touch. Was it broken like this originally?"));
				await dialog.CompleteQuest(Mq04);
				character.Quests.Start(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Priest Raeli has gone on to the Uskis Arable Land, and she wants you to follow."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("I should talk to Maras about this. Something doesn't feel right... What could this be used for?"),
					Option(L("Better meet the priest"), "accept"),
					Option(L("Not really my problem"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Wait here a moment. Someone from the order is coming out to look at it."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A watchman of the Wood Watch Tower, holding a piece of an altar he was told never to touch."));
		});

		// Valda
		//-------------------------------------------------------------------------
		AddNpc(147482, L("Valda"), "SIAULIAI_46_3_SQ_03_NPC", "f_siauliai_46_3", -651.42, 478.42, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Valda"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Wow, they are so fragrant! Thank you so much."));
				await dialog.Msg(L("Ah, please keep this a secret from Maras, okay?"));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("Maras told us to gather as many empty beehive fragments as we can, but that doesn't make sense at all. That implies we defeat the monsters, which we can't!"),
					Option(L("I'll get it"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Really? I am so touched."));
					await dialog.Msg(L("Ah, the beehives are at Saldus Bee Farm."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("I can't do it this time, even if we lose all of this year's supply of honey."));
				await dialog.Msg(L("We may be better off just worrying about lost farming tools like Riesz."));
				return;
			}

			await dialog.Msg(L("A villager who was told to collect beehives from a forest he will not walk into."));
		});

		// Riesz
		//-------------------------------------------------------------------------
		AddNpc(147484, L("Riesz"), "SIAULIAI_46_3_SQ_02_NPC", "f_siauliai_46_3", 1902.07, 604.46, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Riesz"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Ah, yes, these are the ones."));
				await dialog.Msg(L("Everything would've been okay if we villagers could take care of the monsters."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("I am relieved for now."));
				await dialog.Msg(L("The rest will be okay when Maras and Lamar get rid of the monsters."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("The monsters ruined all the fields so I was trying to start from scratch again. And then all of a sudden, a Honeypin showed up, so I left everything and ran away."),
					Option(L("I will find the farm tools"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Thank you so much. The place where I lost it is Shirsie Sunny Place."));
					await dialog.Msg(L("The Honeypin is huge and scary, so don't overdo it if you can't handle it."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("We shouldn't stop farming even if the monsters are making a mess. But the monsters are still scary. Can you help me? It's not that hard."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("Really? Thank you!"));
					await dialog.Msg(L("All you need to do is plant these seeds at the Radanza Farm. Thank you once again!"));
					return;
				}
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The place where I lost it is Shirsie Sunny Place."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("After you sow the seeds, they will grow by themselves."));
				await dialog.Msg(L("Of course, you should take care of them sometimes while avoiding monsters."));
				return;
			}

			await dialog.Msg(L("A farmer who left his tools in the field and has not been back for them."));
		});

		// Den
		//-------------------------------------------------------------------------
		AddNpc(20156, L("Den"), "SIAULIAI_46_3_INV_NPC_01", "f_siauliai_46_3", -654.24, 330.71, 91, async dialog =>
		{
			dialog.SetTitle(L("Den"));

			await dialog.Msg(L("Maras has us counting beehives while the forest walks in on us. I would rather count monsters."));
		});

		// Renzo
		//-------------------------------------------------------------------------
		AddNpc(20157, L("Renzo"), "SIAULIAI_46_3_INV_NPC_02", "f_siauliai_46_3", -420.98, 409.58, 270, async dialog =>
		{
			dialog.SetTitle(L("Renzo"));

			await dialog.Msg(L("The altars have been here longer than the village has. Nobody ever went near them until now."));
		});

		// Bichiu Altar
		//-------------------------------------------------------------------------
		AddConditionalNpc(151024, L("Bichiu Altar"), "SIAULIAI_46_3_AUSTEJA_ALTAR_01", "f_siauliai_46_3", 777.24, -647.75, 270, this.IsBichiuAltarStanding, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bichiu Altar"));

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
			{
				var searched = await character.TimeActions.StartAsync(L("Checking the altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (searched != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.SIAULIAI_46_3_MQ_03_ITEM, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("A piece has been broken out of the altar. Take it to Maras."));
				return;
			}

			await dialog.Msg(L("An altar of Goddess Austeja, and a socket in it with nothing left in the socket."));
		});

		// Gaudeji Altar
		//-------------------------------------------------------------------------
		AddNpc(151024, L("Gaudeji Altar"), "SIAULIAI_46_3_AUSTEJA_ALTAR_02", "f_siauliai_46_3", 358, 1513, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Gaudeji Altar"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				var searched = await character.TimeActions.StartAsync(L("Checking the altar..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (searched != TimeActionResult.Completed)
					return;

				character.Inventory.Add(ItemId.SIAULIAI_46_3_MQ_04_ITEM, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("The same kind of piece, from the same kind of socket. Take it to Lamar."));
				return;
			}

			if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The Cyclops is still standing over the altar."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Something very large has been standing on the north side of this altar, and the ground shows it."),
					Option(L("Check the altar"), "accept"),
					Option(L("Keep clear of the altar"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					character.ServerMessage(L("A Cyclops steps out onto the altar!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("A second altar of Austeja, built to the same pattern as the first."));
		});

		// Sweet-smelling Beehives
		//-------------------------------------------------------------------------
		for (var i = 0; i < Beehives.GetLength(0); ++i)
		{
			AddNpc(151025, L("Sweet-smelling Beehive"), i == 0 ? "SIAULIAI_46_3_BEEHIVE" : "SIAULIAI_46_3_BEEHIVE_" + (i + 1), "f_siauliai_46_3",
				Beehives[i, 0], Beehives[i, 1], BeehiveFacings[i], this.SearchBeehive);
		}

		// Bag of Farming Tools
		//-------------------------------------------------------------------------
		AddConditionalNpc(47161, L("Bag of Farming Tools"), "SIAULIAI_46_3_SQ_02_KIT", "f_siauliai_46_3", 3588, 1212, 91, this.IsToolBagLying, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bag of Farming Tools"));

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
			{
				var looked = await character.TimeActions.StartAsync(L("Checking the bag..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Sq02);
				return;
			}

			await dialog.Msg(L("A bag of farming tools, dropped where somebody stopped running."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Vulvini Farm, where the comb is carried out among the monsters.
		AddQuestTrigger("SIAULIAI_46_3_MQ_02_AREA", "f_siauliai_46_3", -625, -498, 400, this.CarryTheCombOut);
		AddQuestTrigger("SIAULIAI_46_3_MQ_02_AREA_2", "f_siauliai_46_3", -710, 1139, 400, this.CarryTheCombOut);

		// The fields of Radanza Farm
		//-------------------------------------------------------------------------
		for (var i = 0; i < Fields.GetLength(0); ++i)
		{
			AddNpc(40095, L("Farm"), i == 0 ? "SIAULIAI_46_3_SQ_04_FARM" : "SIAULIAI_46_3_SQ_04_FARM_" + (i + 1), "f_siauliai_46_3",
				Fields[i, 0], Fields[i, 1], 90, this.SowField);
		}
	}

	/// <summary>
	/// Carries the honeycomb out among the monsters of Vulvini Farm.
	/// </summary>
	/// <param name="args"></param>
	private async Task CarryTheCombOut(TriggerActorArgs args)
	{
		if (args.Initiator is not Character character)
			return;

		if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
		{
			character.Quests.CompleteObjective(Mq02, "testTheScent");
			character.ServerMessage(L("The monsters walk straight past the comb. Tell Maras."));
		}

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether the Bichiu Altar still has its piece in it.
	/// </summary>
	/// <param name="character"></param>
	private bool IsBichiuAltarStanding(Character character)
		=> !character.Quests.HasCompleted(Mq03);

	/// <summary>
	/// Returns whether the bag of farming tools is still out in the field.
	/// </summary>
	/// <param name="character"></param>
	private bool IsToolBagLying(Character character)
		=> !character.Quests.HasCompleted(Sq02);

	/// <summary>
	/// Hands out the honeycomb pieces the beekeepers are collecting.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SearchBeehive(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Sweet-smelling Beehive"));

		if (character.Quests.IsActive(Mq01) && character.Inventory.CountItem(ItemId.SIAULIAI_46_3_MQ_01_ITEM) == 0)
		{
			var searched = await character.TimeActions.StartAsync(L("Searching the beehive..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

			if (searched != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.SIAULIAI_46_3_MQ_01_ITEM, 1, InventoryAddType.PickUp);
			await dialog.Msg(L("This comb came through whole, and it is a big one."));
			return;
		}

		if (character.Quests.IsActive(Sq03))
		{
			if (character.Inventory.CountItem(ItemId.SIAULIAI_46_3_SQ_03_ITEM) >= CombsToCollect)
			{
				await dialog.Msg(L("You have as many pieces as Valda asked for."));
				return;
			}

			var gathered = await character.TimeActions.StartAsync(L("Breaking off a piece of comb..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

			if (gathered != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.SIAULIAI_46_3_SQ_03_ITEM, 1, InventoryAddType.PickUp);
			await dialog.Msg(L("You break a piece of the comb loose and wrap it."));
			return;
		}

		await dialog.Msg(L("A beehive of the Saldus Bee Farm, and the smell of it carries a long way."));
	}

	/// <summary>
	/// Sows one of Riesz's fields.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SowField(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Farm"));

		if (!character.Quests.IsActive(Sq04))
		{
			await dialog.Msg(L("A field of the Radanza Farm, turned over and waiting for seed."));
			return;
		}

		var sowed = await character.TimeActions.StartAsync(L("Sowing the seeds..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

		if (sowed != TimeActionResult.Completed)
			return;

		for (var i = 1; i <= FieldsToSow; ++i)
		{
			if (character.Quests.IsActive(Sq04, "sowField" + i))
			{
				character.Quests.CompleteObjective(Sq04, "sowField" + i);
				character.ServerMessage(L("The row is sown."));
				return;
			}
		}

		await dialog.Msg(L("Every field Riesz named is sown. Go back and tell him."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 16200: Vilna Forest: The Monsters' Purpose (1)
//-----------------------------------------------------------------------------
public class Siauliai463Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16200);
		SetName(L("Vilna Forest: The Monsters' Purpose (1)"));
		SetDescription(L("Maras wants an undamaged comb out of the Saldus Bee Farm to test the honey theory with."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("Talk with Maras who is investigating in Vilna Forest."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_BEEHIVE", "f_siauliai_46_3", L("Check the Sweet-smelling Beehives"), L("Maras wants you to check whether the monsters are going after honey or not. Obtain pieces of honey that would lure the monsters."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("You have obtained a piece of sweet honeycomb. Ask Maras what to do next."));

		AddPrerequisite(new QuestStatusPrerequisite(16040, QuestStatus.Completed));

		AddObjective("findComb", L("Obtain a Sweet Honeycomb Piece from beehives"), new CollectItemObjective("SIAULIAI_46_3_MQ_01_ITEM", 1));

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 16210: Vilna Forest: The Monsters' Purpose (2)
//-----------------------------------------------------------------------------
public class Siauliai463Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16210);
		SetName(L("Vilna Forest: The Monsters' Purpose (2)"));
		SetDescription(L("The comb is carried out among the monsters to see whether they come for it."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("Ask how Maras will use the fragments of sweet honeycomb."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_MQ_02_AREA", "f_siauliai_46_3", L("Spread the fragrance of honey to the monsters"), L("Maras suspects that the monsters are lured by the honey so he told you to check their reaction to the fragments of sweet honeycomb."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("The monsters did not show any peculiar response, even after smelling the fragments of sweet honeycomb. Tell Maras about this."));

		AddPrerequisite(new QuestStatusPrerequisite(16200, QuestStatus.Completed));

		AddObjective("testTheScent", L("Spread the fragrance of honey to the monsters"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("SIAULIAI_46_3_MQ_01_ITEM"));
	}
}

// 16220: Vilna Forest: The Monsters' Purpose (3)
//-----------------------------------------------------------------------------
public class Siauliai463Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16220);
		SetName(L("Vilna Forest: The Monsters' Purpose (3)"));
		SetDescription(L("If it is not the honey then it is the altars, and the monsters have been gathering at Bichiu."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("It seems that what the monsters were going after was not honey. Ask him whether he has a different idea."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_AUSTEJA_ALTAR_01", "f_siauliai_46_3", L("Check Bichiu Altar"), L("If they're not after the honey, then there's only one thing possible. Check Bichiu Altar, where monsters have been gathering."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Hand over the magic piece to Maras"), L("It seems that the monsters were lured to the magic piece at Bichiu Altar. Hand over the magic piece to Maras."));

		AddPrerequisite(new QuestStatusPrerequisite(16210, QuestStatus.Completed));

		AddObjective("checkBichiu", L("Check Bichiu Altar"), new CollectItemObjective("SIAULIAI_46_3_MQ_03_ITEM", 1));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16230: Altar of Vilna Forest (1)
//-----------------------------------------------------------------------------
public class Siauliai463Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16230);
		SetName(L("Altar of Vilna Forest (1)"));
		SetDescription(L("The Gaudeji Altar on the right holds the same kind of piece, and Lamar is the one to show them to."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_MQ01_NPC", "f_siauliai_46_3", L("Talk to Maras"), L("Ask about the piece that was discovered at Bichiu Altar."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_AUSTEJA_ALTAR_02", "f_siauliai_46_3", L("Check the Gaudeji Altar towards the right"), L("Maras didn't know what the piece is, but he told you that there's a similar altar, so check the Gaudeji Altar towards the right."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_MQ05_NPC", "f_siauliai_46_3", L("Talk to Lamar near Wood Watch Tower"), L("You have found another piece of similar shape at Gaudeji Altar. Meet Lamar as Maras told you."));

		AddPrerequisite(new QuestStatusPrerequisite(16220, QuestStatus.Completed));

		AddObjective("checkGaudeji", L("Check the Gaudeji Altar towards the right"), new CollectItemObjective("SIAULIAI_46_3_MQ_04_ITEM", 1));

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_3_MQ_03_ITEM"));
		AddReward(new TakeItemReward("SIAULIAI_46_3_MQ_04_ITEM"));
	}
}

// 16240: Altar of Vilna Forest (2)
//-----------------------------------------------------------------------------
public class Siauliai463Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16240);
		SetName(L("Altar of Vilna Forest (2)"));
		SetDescription(L("Priest Raeli comes out to look at the pieces, and leaves again for the Uskis Arable Land."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_MQ05_NPC", "f_siauliai_46_3", L("Talk with Lamar"), L("Talk with Lamar about the magic fragment at Gaudeji Altar."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_MQ05_NPC", "f_siauliai_46_3", L("Talk with Lamar"), L("Talk with Lamar about the magic fragment at Gaudeji Altar."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_MQ05_NPC", "f_siauliai_46_3", L("Talk with Raeli"), L("Priest Raeli left after leaving word to come to the Uskis Arable Land. Follow her to the Uskis Arable Land."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_3_MQ_05_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16230, QuestStatus.Completed));

		AddObjective("meetRaeli", L("Talk with Raeli"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16300: Vilna Forest: The Northern Altar Cyclops
//-----------------------------------------------------------------------------
public class Siauliai463Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16300);
		SetName(L("Vilna Forest: The Northern Altar Cyclops"));
		SetDescription(L("A Cyclops has taken the northern altar of the Vilna Forest for itself."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_AUSTEJA_ALTAR_02", "f_siauliai_46_3", L("Check the altar of Vilna Forest at the north side"), L("Check the altar of Vilna Forest at the north side."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_AUSTEJA_ALTAR_02", "f_siauliai_46_3", L("Defeat Cyclops of the altar at Vilna Forest"), L("Defeat Cyclops of the altar at Vilna Forest."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_AUSTEJA_ALTAR_02", "f_siauliai_46_3", L("Defeat Cyclops of the altar at Vilna Forest"), L("Defeat Cyclops of the altar at Vilna Forest."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_3_SQ_01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(153));

		AddObjective("killCyclops", L("Defeat Cyclops"), new KillObjective(1, "boss_Strongholder_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Cyclops is down and the altar is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16310: Help My Farm Recover
//-----------------------------------------------------------------------------
public class Siauliai463Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16310);
		SetName(L("Help My Farm Recover"));
		SetDescription(L("Riesz left his farming tools at Shirsie Sunny Place with a Honeypin standing over them."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_SQ_02_NPC", "f_siauliai_46_3", L("Talk to Riesz"), L("Riesz seems to have lost something and doesn't know what to do. Ask him what he is looking for."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_SQ_02_KIT", "f_siauliai_46_3", L("Find the bag of farming tools at Shirsie Sunny Place"), L("Riesz encountered a Honeypin at Shirsie Sunny Place, so he ran away without his bag of farming tools. Retrieve his farming tools."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_SQ_02_NPC", "f_siauliai_46_3", L("Hand over the farming tools to Riesz"), L("You found the bag of farming tools. Give it back to Riesz."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_3_SQ_02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(153));

		AddObjective("findTools", L("Search for the Bag of Farming Tools at Shirsie Sunny Place"), new CollectItemObjective("SIAULIAI_46_3_SQ_02_ITEM", 1));

		AddPityDrop("SIAULIAI_46_3_SQ_02_ITEM", 1.0f, 0, 1, "boss_honeypin_Q1");

		AddReward(new ItemReward("expCard9", 3));
		AddReward(new TakeItemReward("SIAULIAI_46_3_SQ_02_ITEM"));
	}
}

// 16320: Desirable Combs
//-----------------------------------------------------------------------------
public class Siauliai463Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16320);
		SetName(L("Desirable Combs"));
		SetDescription(L("Valda was told to gather combs from the Saldus Bee Farm and will not go near it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_SQ_03_NPC", "f_siauliai_46_3", L("Talk with Valda"), L("Valda has something to ask you. Listen to him."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_BEEHIVE", "f_siauliai_46_3", L("Collect the pieces of honeycomb"), L("Maras asked Valda to collect the pieces of honeycomb, but the nearby monsters scare him from going. Collect the pieces of honeycomb at Saldus Bee Farm on behalf of Valda."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_SQ_03_NPC", "f_siauliai_46_3", L("Talk with Valda"), L("You've collected all pieces of honeycomb. Hand them over to Valda."));

		AddPrerequisite(new LevelPrerequisite(153));

		AddObjective("collectCombs", L("Collect the pieces of honeycomb at the Saldus Bee Farm"), new CollectItemObjective("SIAULIAI_46_3_SQ_03_ITEM", 6));

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_3_SQ_03_ITEM"));
	}
}

// 16330: Too Scared to Sow
//-----------------------------------------------------------------------------
public class Siauliai463Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16330);
		SetName(L("Too Scared to Sow"));
		SetDescription(L("The Radanza Farm still has to be sown, and Riesz will not walk out to it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_3");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_3_SQ_02_NPC", "f_siauliai_46_3", L("Talk to Riesz"), L("Riesz seems to have a problem. Find out what happened."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_3_SQ_04_FARM", "f_siauliai_46_3", L("Sow seeds at Radanza Farm"), L("Riesz should be sowing seeds at the fields near the village, but he is scared of the monsters. Sow seeds at his farm for him."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_3_SQ_02_NPC", "f_siauliai_46_3", L("Talk to Riesz"), L("You've sowed all the seeds. Return to Riesz and let him know."));

		AddPrerequisite(new LevelPrerequisite(153));

		AddObjective("sowField1", L("Sow the first row at Radanza Farm"), new ManualObjective());
		AddObjective("sowField2", L("Sow the second row at Radanza Farm"), new ManualObjective());
		AddObjective("sowField3", L("Sow the third row at Radanza Farm"), new ManualObjective());
		AddObjective("sowField4", L("Sow the fourth row at Radanza Farm"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}
}
