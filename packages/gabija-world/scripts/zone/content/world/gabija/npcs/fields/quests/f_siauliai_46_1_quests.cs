//--- Melia Script ----------------------------------------------------------
// Spring Light Woods Quest NPCs
//--- Description -----------------------------------------------------------
// Priest Dazine's two broken seal towers, the Revelators the evil energy took,
// and the symbol of Austeja that brings them back.
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

public class FSiauliai461QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(16600);
	private readonly static QuestId Mq02 = new QuestId(16610);
	private readonly static QuestId Mq03 = new QuestId(16620);
	private readonly static QuestId Mq04 = new QuestId(16630);
	private readonly static QuestId Mq05 = new QuestId(16640);
	private readonly static QuestId Sq01 = new QuestId(16700);
	private readonly static QuestId Sq02 = new QuestId(16710);
	private readonly static QuestId Sq03 = new QuestId(16720);
	private readonly static QuestId Sq04 = new QuestId(16730);
	private readonly static QuestId Sq05 = new QuestId(16740);

	private const int FragmentsNeeded = 10;
	private const int GrassNeeded = 5;

	// The merchant's parcels, scattered along the road he ran down.
	private readonly static string[] ParcelNames = { "SIAULIAI_46_1_SQ_03_BAG01", "SIAULIAI_46_1_SQ_03_BAG02", "SIAULIAI_46_1_SQ_03_BAG03" };

	private readonly static double[,] ParcelSpots =
	{
		{ -646.70, -1296.51 }, { -280.23, -369.48 }, { 945.76, -168.53 },
	};

	private readonly static int[] ParcelModels = { 47160, 47160, 47161 };

	// The spots the honey jelly draws Spring Light Grass out of.
	private readonly static double[,] GrassSpots =
	{
		{ 1110.47, 466.92 }, { 853.99, 1024.04 }, { 582.76, 724.59 }, { 602.79, 1008.74 },
		{ 913.39, 485.61 }, { 906.49, 711.86 }, { 758.41, 293.91 }, { 549.09, 466.57 },
	};

	protected override void Load()
	{
		// Priest Dazine
		//-------------------------------------------------------------------------
		AddNpc(147492, L("Priest Dazine"), "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", -1352.52, -156.19, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Dazine"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("This is it, Revelator."));
				await dialog.Msg(L("This will save the addled Revelators."));
				await dialog.CompleteQuest(Mq02);
				character.Quests.Start(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Luckily, they did minimal damage."));
				await dialog.Msg(L("Now, if we can restore the seal, this will never have to repeat."));
				await dialog.CompleteQuest(Mq03);
				character.Quests.Start(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Revelator. I assume you've heard the story from Goddess Austeja."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("There are a number of Revelators who were addled by the evil energy attacking the village while trying to restore the seal here."),
					Option(L("Ask how to regain the symbol"), "accept"),
					Option(L("About the Seal in Spring Light Woods"), "explain"),
					Option(L("It's not related to me"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Since the seal towers here were damaged more seriously than the one at Uskis Arable Land, the evil energy here flows stronger."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("First regain the goddess' symbol and then use it to restore the Austeja Altar."));
					await dialog.Msg(L("Purifying the Revelators will come next."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("The addled Revelators will march towards here soon. Please purify them before anyone gets hurt."),
					Option(L("Leave it to me"), "accept"),
					Option(L("Give me some time to prepare"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("Restore the weakened seal towers using the symbol of Goddess Austeja. There is the Rankis Seal and the Ranka Seal."),
					Option(L("I will restore the seal"), "accept"),
					Option(L("Why the seal is destroyed"), "explain"),
					Option(L("Decline"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The demons found out about the source of evil energy and so relentlessly attacked to destroy the seal."));
					await dialog.Msg(L("I'm sure if the seal is strengthened well this time, they will retreat after a while."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("There is one thing I am worried about.."));
					await dialog.Msg(L("That symbol was once damaged so it may not restore the seal perfectly."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Whether it's to restore the weakened seal or to cure the addled Revelators, we will need that symbol."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Austeja Altar is the only place the fragments will go back together."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The addled Revelators will march towards here soon."));
				await dialog.Msg(L("Please purify them before anyone gets hurt."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("There is one thing I am worried about.."));
				await dialog.Msg(L("That symbol was once damaged so it may not restore the seal perfectly."));
				return;
			}

			await dialog.Msg(L("A priest of Austeja holding a wood with two broken seal towers in it."));
		});

		// Merchant Dulke
		//-------------------------------------------------------------------------
		AddNpc(20102, L("Merchant Dulke"), "SIAULIAI_46_1_SQ_03_NPC", "f_siauliai_46_1", -1776.32, -1027.20, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Merchant Dulke"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Oh, I could never thank you enough... yes, this is my stuff."));
				await dialog.Msg(L("And I thought it was weird that you were fine, unlike the other Revelators. You truly are a special one."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("Oh no, what do I do?"));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I lost all my wares while running away from the monsters. But I'm afraid I might lose my mind like the other Revelators if I go back for it now."),
					Option(L("I will find it back so don't worry"), "accept"),
					Option(L("About the addled Revelators"), "explain"),
					Option(L("Decline"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("At a glance, you wouldn't be able to tell if they are a human or a monster."));
					await dialog.Msg(L("They would have a blank, soulless stare and when called, it would disappear while making a strange noise."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Please help me."));
					await dialog.Msg(L("I just started my business with everything I got, and now I'm afraid I'm about to end up with nothing."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Please help me. I just started my business with everything I got."));
				return;
			}

			await dialog.Msg(L("A pedlar who dropped his whole stock on the road and will not go back for it."));
		});

		// Villager Emil
		//-------------------------------------------------------------------------
		AddNpc(147484, L("Villager Emil"), "SIAULIAI_46_1_SQ_04_NPC", "f_siauliai_46_1", 1974.63, 855.25, -30, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Villager Emil"));

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("I've seen other Revelators here that got all weird, but you seem fine."));
				await dialog.Msg(L("Well, you did some great work. I wish I was as strong as you are."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("I used to travel in and out of town, but the priests told me to stay put, saying the seal is dangerous. That seal really is nothing for me, it's the demons that are scary."),
					Option(L("I'll get rid of the monsters"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("I have to have a livelihood."));
					await dialog.Msg(L("The other villagers here just spend all day looking up to the priests and the goddess."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("I have to have a livelihood."));
				return;
			}

			await dialog.Msg(L("A villager who wants the road to Vilna Forest cleared and is not waiting for the priests to do it."));
		});

		// Pharmacist Tiana
		//-------------------------------------------------------------------------
		AddNpc(147493, L("Pharmacist Tiana"), "SIAULIAI_46_1_SQ_05_NPC", "f_siauliai_46_1", 1714.45, 889.59, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pharmacist Tiana"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Thank you for so much help."));
				await dialog.Msg(L("I'm sure this will be more than enough for the priest."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("Priest Dazine asked me for medicine to cure the future addled Revelators with, but I ran out of Spring Light Grass... and I'm also scared of the demons."),
					Option(L("Don't worry, I'll get it"), "accept"),
					Option(L("Just ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					character.Inventory.Add(ItemId.SIAULIAI_46_1_SQ_05_ITEM01, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("You're such a sweet person."));
					await dialog.Msg(L("Oh, Spring Light Grass is not easy to find. It only appears when it's reacting to this honey jelly."));
					return;
				}
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("Spring Light Grass only appears when it's reacting to that honey jelly."));
				return;
			}

			await dialog.Msg(L("A chemist mixing what the priest asked for, out of herbs she has run out of."));
		});

		// Austeja Altar
		//-------------------------------------------------------------------------
		AddNpc(151024, L("Austeja Altar"), "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", -130, 10, 180, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Austeja Altar"));

			var restoring = character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02);
			var offering = !character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02);

			if (restoring || offering)
			{
				var restored = await character.TimeActions.StartAsync(L("Restoring the symbol..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3));

				if (restored != TimeActionResult.Completed)
					return;

				if (offering)
					character.Quests.Start(Mq02);

				character.Inventory.RemoveItem(ItemId.SIAULIAI_46_1_MQ_01_ITEM, FragmentsNeeded);
				character.Inventory.Add(ItemId.SIAULIAI_46_1_MQ_02_ITEM, 1, InventoryAddType.PickUp);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("The Symbol of Goddess Austeja has been restored!"));
				return;
			}

			if (character.Quests.IsActive(Sq01) && !character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The Chafer is still in the trees behind the altar."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("Something moves behind the altar every time you look away from it."),
					Option(L("Check the Spring Light Woods Altar"), "accept"),
					Option(L("Keep away from the altar"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					character.ServerMessage(L("A Chafer comes out from behind the altar!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("An altar of Austeja, whole where the seal towers are not."));
		});

		// Rankis Seal Tower
		//-------------------------------------------------------------------------
		AddNpc(147501, L("Rankis Seal Tower"), "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", -217.27, -868.98, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Rankis Seal Tower"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				var restored = await character.TimeActions.StartAsync(L("Giving the tower the goddess' power..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(2));

				if (restored != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq04, "restoreRankis");
				character.ServerMessage(L("The Rankis Seal holds again. The Ranka Seal is the other one."));
				return;
			}

			if (character.Quests.IsActive(Sq02) && !character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("The Manticen is still standing over the tower."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var looked = await character.TimeActions.StartAsync(L("Checking the tower..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(3));

				if (looked != TimeActionResult.Completed)
					return;

				var answer = await dialog.SelectQuestOffer(Sq02, L("The tower was not worn down. Something pulled it apart, and the marks on it are claws."),
					Option(L("Look for what broke it"), "accept"),
					Option(L("Leave it for the priests"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					character.ServerMessage(L("A Manticen comes back for the tower!"));
					return;
				}
				return;
			}

			await dialog.Msg(L("A seal tower of the wood, pulled apart rather than worn down."));
		});

		// Ranka Seal Tower
		//-------------------------------------------------------------------------
		AddNpc(147501, L("Ranka Seal Tower"), "SIAULIAI_46_1_DEADTREE02", "f_siauliai_46_1", 311, -871, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ranka Seal Tower"));

			var restoringRanka = character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05);
			var offeringRanka = !character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05);

			if (restoringRanka || offeringRanka)
			{
				var restored = await character.TimeActions.StartAsync(L("Giving the tower the goddess' power..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3));

				if (restored != TimeActionResult.Completed)
					return;

				if (offeringRanka)
					character.Quests.Start(Mq05);

				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("The second of the wood's seal towers, in the same state as the first."));
		});

		// Merchant Item Parcels
		//-------------------------------------------------------------------------
		for (var i = 0; i < ParcelSpots.GetLength(0); ++i)
		{
			AddConditionalNpc(ParcelModels[i], L("Merchant Item Parcel"), ParcelNames[i], "f_siauliai_46_1",
				ParcelSpots[i, 0], ParcelSpots[i, 1], 90, this.AreParcelsLying, this.PickMerchantParcel);
		}

		// Spring Light Grass
		//-------------------------------------------------------------------------
		for (var i = 0; i < GrassSpots.GetLength(0); ++i)
		{
			AddNpc(20026, L("Spring Light Grass"), i == 0 ? "SIAULIAI_46_1_SQ_05_TRIGGER" : "SIAULIAI_46_1_SQ_05_TRIGGER_" + (i + 1), "f_siauliai_46_1",
				GrassSpots[i, 0], GrassSpots[i, 1], 90, this.DrawOutSpringLightGrass);
		}

		// Literature
		//-------------------------------------------------------------------------
		AddNpc(47192, L("Weathered Stone"), "SIAULIAI46_1_HIDDEN_EVENT", "f_siauliai_46_1", 732.26, 518.38, 79, async dialog =>
		{
			dialog.SetTitle(L("Weathered Stone"));

			await dialog.Msg(L("A stone cut with the names of the seal towers, and a line under them that has worn away entirely."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The road the addled Revelators wander, where the fragments are taken.
		AddQuestTrigger("SIAULIAI_46_1_MQ_01_AREA", "f_siauliai_46_1", -282, -397, 1000, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq01))
				character.ServerMessage(L("The evil energy is thick here. Take the fragments off whatever carries them."));

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the merchant's parcels are still out on the road.
	/// </summary>
	/// <param name="character"></param>
	private bool AreParcelsLying(Character character)
		=> !character.Quests.HasCompleted(Sq03);

	/// <summary>
	/// Hands out the merchant's lost parcels.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PickMerchantParcel(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Merchant Item Parcel"));

		if (!character.Quests.IsActive(Sq03))
		{
			await dialog.Msg(L("A parcel of a pedlar's stock, dropped where he stopped carrying it."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_1_SQ_03_ITEM) >= 3)
		{
			await dialog.Msg(L("You have every parcel Dulke described."));
			return;
		}

		var picked = await character.TimeActions.StartAsync(L("Picking up the parcel..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (picked != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_1_SQ_03_ITEM, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("The parcel is unopened, and Dulke's mark is still on the knot."));
	}

	/// <summary>
	/// Draws the Spring Light Grass out with the pharmacist's honey jelly.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task DrawOutSpringLightGrass(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Spring Light Grass"));

		if (!character.Quests.IsActive(Sq05))
		{
			await dialog.Msg(L("Nothing grows here that you can see."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_1_SQ_05_ITEM01) == 0)
		{
			await dialog.Msg(L("Without the honey jelly there is nothing here to find."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.SIAULIAI_46_1_SQ_05_ITEM02) >= GrassNeeded)
		{
			await dialog.Msg(L("You have as much grass as Tiana asked for."));
			return;
		}

		var drawn = await character.TimeActions.StartAsync(L("Setting out the honey jelly..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (drawn != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.SIAULIAI_46_1_SQ_05_ITEM02, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("The grass opens out of the ground where the jelly was set down."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 16600: Addled Revelators
//-----------------------------------------------------------------------------
public class Siauliai461Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16600);
		SetName(L("Addled Revelators"));
		SetDescription(L("The Symbol of Austeja is in pieces, and the monsters of the wood carry them."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Talk to Priest Dazine"), L("Go to Priest Dazine in the Spring Light Woods."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_MQ_01_AREA", "f_siauliai_46_1", L("Collect the Fragmented Symbol of Austeja"), L("You need the Symbol of Austeja to cure the Revelators who are addled by the evil energy. First, collect the fragmented symbols of Austeja."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Collect the Fragmented Symbol of Austeja"), L("You have every fragment of the symbol."));

		AddPrerequisite(new QuestStatusPrerequisite(16440, QuestStatus.Completed));

		AddObjective("collectFragments", L("Retrieve the Fragmented Symbol of Austeja"), new CollectItemObjective("SIAULIAI_46_1_MQ_01_ITEM", 10));

		AddPityDrop("SIAULIAI_46_1_MQ_01_ITEM", 0.5f, 4, 1, "infro_Blud", "Shardstatue", "Siaulav");

		AddReward(new ItemReward("expCard9", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; the fragments are taken to the altar
		// by the next quest instead.
		character.ServerMessage(L("Every fragment of the symbol is in hand. The Austeja Altar is where they go back together."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16610: Symbol of Goddess Austeja
//-----------------------------------------------------------------------------
public class Siauliai461Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16610);
		SetName(L("Symbol of Goddess Austeja"));
		SetDescription(L("The fragments go back together on the Austeja Altar and nowhere else."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", L("Restore the symbol at the Austeja Altar"), L("Restore the fragments of the symbol at the Austeja Altar."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", L("Restore the symbol at the Austeja Altar"), L("Restore the fragments of the symbol at the Austeja Altar."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Talk to Priest Dazine"), L("Restored the symbol at the Austeja Altar. Give it to Priest Dazine."));

		AddPrerequisite(new QuestStatusPrerequisite(16600, QuestStatus.Completed));

		AddObjective("restoreSymbol", L("Restore the Symbol of Goddess Austeja"), new CollectItemObjective("SIAULIAI_46_1_MQ_02_ITEM", 1));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16620: The Revelators come First
//-----------------------------------------------------------------------------
public class Siauliai461Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16620);
		SetName(L("The Revelators come First"));
		SetDescription(L("The addled Revelators are walking on the village, and the symbol purifies them once they are down."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Talk to Priest Dazine"), L("Talk to Priest Dazine about how to purify the addled Revelators."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Purify the addled Revelators"), L("Fight the addled Revelators to subdue them, then use the Austeja's Symbol to purify them."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Talk to Priest Dazine"), L("Purified all the addled Revelators. Talk to Priest Dazine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_1_MQ_03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16610, QuestStatus.Completed));

		AddObjective("purifyRevelators", L("Purify the addled Revelators"), new KillObjective(5, "npc_dazz_KRV", "npc_dazz_SCT", "npc_dazz_ROD", "npc_dazz_MNK", "npc_dazz_BAR"));

		AddReward(new ItemReward("expCard9", 2));
	}
}

// 16630: Destroyed Seal Tower (1)
//-----------------------------------------------------------------------------
public class Siauliai461Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16630);
		SetName(L("Destroyed Seal Tower (1)"));
		SetDescription(L("The Rankis Seal is given what is left of the symbol's power."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_MQ01_NPC", "f_siauliai_46_1", L("Talk to Priest Dazine"), L("With the addled Revelators now purified, talk to Priest Dazine about the seal."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", L("Restore the Rankis Seal and the Ranka Seal"), L("The seal towers that sealed the forces of evil were all destroyed by the monsters. The Symbol of Austeja can restore a little bit of power to the Seal."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", L("Restore the Rankis Seal and the Ranka Seal"), L("The Rankis Seal holds again."));

		AddPrerequisite(new QuestStatusPrerequisite(16620, QuestStatus.Completed));

		AddObjective("restoreRankis", L("Restore the Rankis Seal"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; the second tower is the next quest.
		character.Quests.Complete(this.QuestId);
	}
}

// 16640: Destroyed Seal Tower (2)
//-----------------------------------------------------------------------------
public class Siauliai461Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16640);
		SetName(L("Destroyed Seal Tower (2)"));
		SetDescription(L("The Ranka Seal takes the last of the symbol, and Austeja comes to say where she is going."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_DEADTREE02", "f_siauliai_46_1", L("Restore the goddess' powers to Rankis Seal Tower and Ranka Seal Tower"), L("Restored power to the first destroyed tower. Now the other destroyed tower has to be restored as well."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_DEADTREE02", "f_siauliai_46_1", L("Listen to Goddess Austeja's story"), L("Goddess Austeja appeared and is trying to tell you something. Listen to what she is saying."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_DEADTREE02", "f_siauliai_46_1", L("Listen to Goddess Austeja's story"), L("Goddess Austeja has said what she came to say."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_1_MQ_05_TRACK", 4000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(16630, QuestStatus.Completed));

		AddObjective("restoreRanka", L("Restore the Ranka Seal"), new ManualObjective());

		AddReward(new ItemReward("expCard9", 1));
		AddReward(new TakeItemReward("SIAULIAI_46_1_MQ_02_ITEM"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC; the goddess' farewell is the end of it.
		character.Quests.Complete(this.QuestId);
	}
}

// 16700: Chafer of the Spring Light Woods
//-----------------------------------------------------------------------------
public class Siauliai461Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16700);
		SetName(L("Chafer of the Spring Light Woods"));
		SetDescription(L("Something has been keeping out of sight behind the Austeja Altar."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", L("Check the Spring Light Woods Altar"), L("There is a suspicious shadow in Spring Light Woods Altar. Take a close look at it."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", L("Defeat Chafer"), L("A Chafer was hiding near the altar. Defeat Chafer."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_ALTAR", "f_siauliai_46_1", L("Defeat Chafer"), L("A Chafer was hiding near the altar. Defeat Chafer."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_1_SQ_01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16610, QuestStatus.InProgress));

		AddObjective("killChafer", L("Defeat Chafer"), new KillObjective(1, "boss_Chafer_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Chafer is down and the altar is clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16710: The Tower's Destroyer
//-----------------------------------------------------------------------------
public class Siauliai461Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16710);
		SetName(L("The Tower's Destroyer"));
		SetDescription(L("The marks on the Rankis Seal Tower are claws, and the Manticen that made them is still about."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", L("Check the destroyed seal tower"), L("Someone destroyed the tower. Check its status."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", L("Defeat Manticen"), L("It seems like this Manticen destroyed the tower. Defeat it."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_DEADTREE01", "f_siauliai_46_1", L("Defeat Manticen"), L("It seems like this Manticen destroyed the tower. Defeat it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAULIAI_46_1_SQ_02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(16630, QuestStatus.InProgress));

		AddObjective("killManticen", L("Defeat Manticen"), new KillObjective(1, "boss_Manticen_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard9", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Manticen is down. It will not come back for the tower."));
		character.Quests.Complete(this.QuestId);
	}
}

// 16720: Merchant's Lost Wares
//-----------------------------------------------------------------------------
public class Siauliai461Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16720);
		SetName(L("Merchant's Lost Wares"));
		SetDescription(L("Dulke dropped his whole stock along the road and will not walk back down it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_SQ_03_NPC", "f_siauliai_46_1", L("Talk to Merchant Dulke"), L("Seems like Merchant Dulke is troubled. Ask him what's wrong."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_SQ_03_BAG01", "f_siauliai_46_1", L("Find Merchant Dulke's Package"), L("Merchant Dulke says that he lost his belongings while running away from the monsters. Find Dulke's belongings for him."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_SQ_03_NPC", "f_siauliai_46_1", L("Give the packages to Merchant Dulke"), L("Found all Merchant Dulke's packages. Give it to him."));

		AddPrerequisite(new LevelPrerequisite(159));

		AddObjective("findParcels", L("Find the Merchant Item Parcels"), new CollectItemObjective("SIAULIAI_46_1_SQ_03_ITEM", 3));

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("SIAULIAI_46_1_SQ_03_ITEM"));
	}
}

// 16730: Securing a Safe Route
//-----------------------------------------------------------------------------
public class Siauliai461Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16730);
		SetName(L("Securing a Safe Route"));
		SetDescription(L("Emil wants the road to Vilna Forest walkable and is not waiting for the priests to do it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_SQ_04_NPC", "f_siauliai_46_1", L("Talk to Villager Emil"), L("Villager Emil in the Spring Light Woods wants to ask you a favor. Go to Villager Emil."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_SQ_04_NPC", "f_siauliai_46_1", L("Defeat the monsters around the village"), L("Villager Emil says it's too dangerous to go to Vilna Forest. Defeat the monsters nearby for him to go there safely."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_SQ_04_NPC", "f_siauliai_46_1", L("Talk to Villager Emil"), L("Defeated enough monsters. Go back and talk to Emil of Spring Light Woods."));

		AddPrerequisite(new LevelPrerequisite(159));

		AddObjective("clearTheRoad", L("Defeat the monsters around the village"), new KillObjective(20, "infro_Blud", "Shardstatue", "Siaulav"));

		AddReward(new ItemReward("expCard9", 1));
	}
}

// 16740: Medicine Made of Spring Light Grass
//-----------------------------------------------------------------------------
public class Siauliai461Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(16740);
		SetName(L("Medicine Made of Spring Light Grass"));
		SetDescription(L("Spring Light Grass only shows itself to honey jelly, and Tiana has run out of both."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAI_46_1_SQ_05_NPC", "f_siauliai_46_1", L("Talk to Pharmacist Tiana"), L("Pharmacist Tiana makes medicine using herbs from Spring Light Woods. Talk to her."));
		SetPhase(QuestStatus.InProgress, "SIAULIAI_46_1_SQ_05_TRIGGER", "f_siauliai_46_1", L("Collect Spring Light Grass"), L("Pharmacist Tiana says she needs more Spring Light Grass to make her medicine. Use bee honey jelly to find and collect the elusive Spring Light Grass."));
		SetPhase(QuestStatus.Success, "SIAULIAI_46_1_SQ_05_NPC", "f_siauliai_46_1", L("Give the Spring Light Grass to Pharmacist Tiana"), L("Gathered enough Spring Light Grass. Give it to Tiana."));

		AddPrerequisite(new LevelPrerequisite(159));

		AddObjective("collectGrass", L("Collect Spring Light Grass"), new CollectItemObjective("SIAULIAI_46_1_SQ_05_ITEM02", 5));

		AddReward(new ItemReward("expCard9", 2));
		AddReward(new TakeItemReward("SIAULIAI_46_1_SQ_05_ITEM01"));
		AddReward(new TakeItemReward("SIAULIAI_46_1_SQ_05_ITEM02"));
	}
}
