//--- Melia Script ----------------------------------------------------------
// Cobalt Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The Andale Village priest and headman, the herbs and barrels their bomb is
// built from, and the old well of the forest.
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

public class FHuevillage583QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20283);
	private readonly static QuestId Mq02 = new QuestId(20284);
	private readonly static QuestId Mq03 = new QuestId(20285);
	private readonly static QuestId Mq04 = new QuestId(20286);
	private readonly static QuestId Sq01 = new QuestId(20287);
	private readonly static QuestId Sq02 = new QuestId(20288);

	private const int LanguidHerbsNeeded = 5;

	protected override void Load()
	{
		// Andale Village Priest
		//-------------------------------------------------------------------------
		AddNpc(147408, L("Andale Village Priest"), "HUEVILLAGE_58_3_MQ01_NPC", "f_huevillage_58_3", 438.90, -598.18, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Andale Village Priest"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Glad you're back safely."));
				await dialog.Msg(L("Did any monsters attack?"));
				character.Quests.Complete(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("That'll do."));
				await dialog.Msg(L("I'll send the materials to the elderly, please go and get the explosives."));
				character.Quests.Complete(Mq02);
				character.ServerMessage(L("Go and find the village headman."));
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("You're the Revelator, right?"));

				var answer = await dialog.Select(L("Upents are preying on the village so we can't hold the ritual right now."),
					Option(L("Ask how to help defeat the Upent"), "accept"),
					Option(L("About the Ritual"), "explain"),
					Option(L("I'll wait then"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Hmm... So... The procedure is..."));
					await dialog.Msg(L("Wash your body in the water, pray, and then..."));
					await dialog.Msg(L("It's too complicated so I'll explain it later."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("I'm thinking about making bombs from Languid Herbs."));
					await dialog.Msg(L("Please bring me Languid Herbs from Narvas Curved Path."));
					character.ServerMessage(L("Gather Languid Herbs at Narvas Curved Path."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Please get the Strongly Scented Soul Flowers in Dvyni Wetland too."));

				var answer = await dialog.Select(L("Languid Herbs alone will not be effective enough."),
					Option(L("I'll try to find them"), "accept"),
					Option(L("About the Goddess Statues"), "explain"),
					Option(L("The force is enough"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Is it alright to walk over the Goddess Statue?"));
					await dialog.Msg(L("I wouldn't mind it. Why do you ask?"));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("I hope we'll be successful this time."));
					character.ServerMessage(L("Look for the Strongly Scented Soul Flower in Dvyni Wetland."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("It is safe where the Languid Herb blooms so do not worry."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I'll do the ritual right away if we can get rid of the Upents."));
				await dialog.Msg(L("So hurry and get me the Strongly Scented Soul Flowers."));
				return;
			}

			await dialog.Msg(L("The ritual cannot be held while the Upents come down on the village."));
		});

		// Andale Village Headman
		//-------------------------------------------------------------------------
		AddNpc(147396, L("Andale Village Headman"), "HUEVILLAGE_58_3_MQ03_NPC", "f_huevillage_58_3", 562.93, -844.83, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Andale Village Headman"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("It was in a small barrel?"));
				await dialog.Msg(L("I must be getting old and mixed up."));
				await dialog.Msg(L("Normal people would have been incinerated, but for a Revelator, it's different."));
				character.Quests.Complete(Mq03);
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("Explosives? Ah."));

				var answer = await dialog.Select(L("Go to the warehouse lot at the upper side of the village."),
					Option(L("I will go get the explosives"), "accept"),
					Option(L("That sounds dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("It's probably in one of the bigger barrels."));
					await dialog.Msg(L("Don't worry, they won't explode."));
					character.ServerMessage(L("Bring the explosives from the barrels located at the upper side of the village."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Here, this is a Languid Herb Bomb."));

				var answer = await dialog.Select(L("Take it to Melaginags Cliff."),
					Option(L("I'll use the bomb"), "accept"),
					Option(L("I will prepare myself for a while"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("Hurry now."));
					await dialog.Msg(L("We can finally end it this time."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Don't worry, it's safe. They will not explode."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("This time, it can finally come to an end."));
				return;
			}

			await dialog.Msg(L("The barrels in the warehouse lot have sat there since my father's day."));
		});

		// Languid Herb
		//-------------------------------------------------------------------------
		AddNpc(47201, L("Languid Herb"), "HUEVILLAGE_58_3_MQ01_GRASS", "f_huevillage_58_3", 1889.34, -362.38, 100, this.PickLanguidHerb);
		AddNpc(47201, L("Languid Herb"), "f_huevillage_58_3", 2011.75, -566.91, 95, this.PickLanguidHerb);
		AddNpc(47201, L("Languid Herb"), "f_huevillage_58_3", 1820.73, -781.04, 90, this.PickLanguidHerb);
		AddNpc(47201, L("Languid Herb"), "f_huevillage_58_3", 1702.58, -481.52, 80, this.PickLanguidHerb);
		AddNpc(47201, L("Languid Herb"), "f_huevillage_58_3", 1673.03, -268.11, 85, this.PickLanguidHerb);
		AddNpc(47201, L("Languid Herb"), "f_huevillage_58_3", 1662.65, -662.73, 90, this.PickLanguidHerb);

		// Strongly Scented Soul Flower
		//-------------------------------------------------------------------------
		AddNpc(147412, L("Strongly Scented Soul Flower"), "HUEVILLAGE_58_3_MQ02_FLOWER", "f_huevillage_58_3", 498, 512, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Strongly Scented Soul Flower"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				character.Inventory.Add(ItemId.HUEVILLAGE_58_3_MQ02_ITEM1, 1, InventoryAddType.PickUp);
				await dialog.Msg(L("You cut the flower at the stem. The scent of it clings to your hands."));
				character.ServerMessage(L("You obtained the Strongly Scented Soul Flower. Return to the village priest."));
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.Select(L("The wetland around the flower bed has been pressed flat, and the water in the hollows has not settled."),
					Option(L("Go to Dvyni Wetland"), "accept"),
					Option(L("Keep to the path"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Colimencia is still in the wetland."));
				character.Quests.ReplayQuestTrack(Sq02);
				return;
			}

			await dialog.Msg(L("A soul flower, grown tall on the wetland edge. Its scent carries a long way."));
		});

		// Barrels of explosives
		//-------------------------------------------------------------------------
		AddNpc(147459, L("Barrel of Explosives"), "HUEVILLAGE_58_3_MQ03_DRUM", "f_huevillage_58_3", -322.36, 215.87, 90, this.OpenSmallBarrel);
		AddNpc(147459, L("Barrel of Explosives"), "f_huevillage_58_3", -241.44, 351.85, 90, this.OpenSmallBarrel);

		AddNpc(147458, L("Barrel of Explosives"), "HUEVILLAGE_58_3_MQ03_DRUM_FAKE", "f_huevillage_58_3", -155.86, 234.15, 90, this.OpenLargeBarrel);
		AddNpc(147458, L("Barrel of Explosives"), "f_huevillage_58_3", -240.24, 228.34, 90, this.OpenLargeBarrel);
		AddNpc(147458, L("Barrel of Explosives"), "f_huevillage_58_3", -119.61, 327.85, 90, this.OpenLargeBarrel);
		AddNpc(147458, L("Barrel of Explosives"), "f_huevillage_58_3", -235.66, 140.62, 90, this.OpenLargeBarrel);
		AddNpc(147458, L("Barrel of Explosives"), "f_huevillage_58_3", -328.38, 117.96, 90, this.OpenLargeBarrel);

		// Sleeping Upent
		//-------------------------------------------------------------------------
		AddNpc(57019, L("Sleeping Upent"), "HUEVILLAGE_58_3_MQ04_NPC01", "f_huevillage_58_3", -1395.59, -1312.53, 104, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sleeping Upent"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You set the Languid Herb Bomb down among the sleeping Upents and step back."));
				character.Inventory.RemoveItem(ItemId.HUEVILLAGE_58_3_MQ03_ITEM2, 1);
				character.Quests.StartQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("An Upent, asleep on the cliff shelf. Its flanks rise and fall slowly."));
		});

		// Old Well
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Old Well"), "HUEVILLAGE_58_3_SQ01_NPC01", "f_huevillage_58_3", 1070, -101, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Well"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("The bucket comes up on the rope with something tangled in the handle - a necklace, and a scrap of writing with it."));
				character.Quests.Complete(Sq01);
				character.ServerMessage(L("You found someone's keepsake in the well!"));
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.Select(L("Something is lying at the bottom of the old well, too far down to make out."),
					Option(L("Go to the old well"), "accept"),
					Option(L("Leave the well alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					character.ServerMessage(L("Look for a tool that can get the thing out of the well."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("You need something to reach the bottom with - a bucket, and a rope long enough."));
				return;
			}

			await dialog.Msg(L("An old well, its wall half fallen in. The water at the bottom is black."));
		});

		// Bucket
		//-------------------------------------------------------------------------
		AddNpc(147354, L("Bucket"), "HUEVILLAGE_58_3_SQ01_NPC02", "f_huevillage_58_3", 200.30, -421.99, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bucket"));

			if (character.Quests.IsActive(Sq01) && character.Inventory.CountItem(ItemId.HUEVILLAGE_58_3_SQ01_BUCKET) == 0)
			{
				character.Inventory.Add(ItemId.HUEVILLAGE_58_3_SQ01_BUCKET, 1, InventoryAddType.PickUp);
				await dialog.Msg(L("The bucket is sound, and its handle will take a rope."));
				return;
			}

			await dialog.Msg(L("A bucket left out by the forest path."));
		});

		// Old Canopy
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Old Canopy"), "HUEVILLAGE_58_3_SQ01_NPC03", "f_huevillage_58_3", -681, -1153, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Canopy"));

			if (character.Quests.IsActive(Sq01) && character.Inventory.CountItem(ItemId.HUEVILLAGE_58_3_SQ01_ROPE) == 0)
			{
				character.Inventory.Add(ItemId.HUEVILLAGE_58_3_SQ01_ROPE, 1, InventoryAddType.PickUp);
				await dialog.Msg(L("You work a long rope loose from the canopy's guy lines."));
				return;
			}

			await dialog.Msg(L("A rotting canopy, still held up by its guy lines."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The way back onto Veja Ravine, where the escape from the village ends.
		AddQuestTrigger("HUEVILLAGE_58_3_MQ04_TO_HUE1", "f_huevillage_58_1", -511.32, -277.65, 70, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
				character.Quests.Complete(Mq04);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Hands out the Languid Herbs the priest's bomb is built from.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PickLanguidHerb(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Languid Herb"));

		if (!character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("A stand of Languid Herb, grown along the curve of the path."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.HUEVILLAGE_58_3_MQ01_ITEM1) >= LanguidHerbsNeeded)
		{
			await dialog.Msg(L("You have as much of the herb as the priest asked for."));
			return;
		}

		character.Inventory.Add(ItemId.HUEVILLAGE_58_3_MQ01_ITEM1, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("You cut a handful of the herb and wrap it."));
	}

	/// <summary>
	/// Hands out the Useful Bomb the headman misremembered the barrel of.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task OpenSmallBarrel(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Barrel of Explosives"));

		if (character.Quests.IsActive(Mq03) && character.Inventory.CountItem(ItemId.HUEVILLAGE_58_3_MQ03_ITEM1) == 0)
		{
			character.Inventory.Add(ItemId.HUEVILLAGE_58_3_MQ03_ITEM1, 1, InventoryAddType.PickUp);
			await dialog.Msg(L("The small barrel is the one with the explosives in it after all."));
			return;
		}

		await dialog.Msg(L("A small barrel, its lid wedged on tight."));
	}

	/// <summary>
	/// The large barrels the headman sent the player to, which are empty.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task OpenLargeBarrel(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Barrel of Explosives"));

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("Nothing in this one but sawdust. The headman said the bigger barrels."));
			return;
		}

		await dialog.Msg(L("A large barrel, empty and open to the weather."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20283: Andale Village Priest's Favor (1)
//-----------------------------------------------------------------------------
public class Huevillage583Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20283);
		SetName(L("Andale Village Priest's Favor (1)"));
		SetDescription(L("The priest wants Languid Herbs from Narvas Curved Path to build a bomb against the Upents."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_MQ01_NPC", "f_huevillage_58_3", L("Find the Andale Village Priest"), L("The Priest in Vieta Gorge told you to go to the Priest in Andale Village. Go meet the Andale Village Priest in Cobalt Forest."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_MQ01_GRASS", "f_huevillage_58_3", L("Collect the Languid Herbs"), L("Collect Languid Herbs needed to make the bomb that drives off the monsters threatening Andale Village."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_MQ01_NPC", "f_huevillage_58_3", L("Talk to the Andale Village Priest"), L("Gathered enough Languid Herbs. Talk to the Andale Village Priest in Andale Village."));

		AddPrerequisite(new QuestStatusPrerequisite(20279, QuestStatus.Completed));

		AddObjective("collectHerbs", L("Collect Languid Herb"), new CollectItemObjective("HUEVILLAGE_58_3_MQ01_ITEM1", 5));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("HUEVILLAGE_58_3_MQ01_ITEM1"));
	}
}

// 20284: Andale Village Priest's Favor (2)
//-----------------------------------------------------------------------------
public class Huevillage583Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20284);
		SetName(L("Andale Village Priest's Favor (2)"));
		SetDescription(L("The herbs alone will not do. Bring the Strongly Scented Soul Flower of Dvyni Wetland as well."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_MQ01_NPC", "f_huevillage_58_3", L("Talk to the Andale Village Priest"), L("Ask the Andale Village Priest what else is needed to make the Languid Bomb."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_MQ02_FLOWER", "f_huevillage_58_3", L("Collect the Strongly Scented Soul Flower"), L("Strongly scented soul flowers are needed to make strong bombs. Look for the flowers with a strong scent."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_MQ01_NPC", "f_huevillage_58_3", L("Talk to the Andale Village Priest"), L("Collected the flowers. Ask the Andale Village Priest what to do."));

		AddPrerequisite(new QuestStatusPrerequisite(20283, QuestStatus.Completed));

		AddObjective("collectFlower", L("Collect Strongly Scented Soul Flower"), new CollectItemObjective("HUEVILLAGE_58_3_MQ02_ITEM1", 1));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("HUEVILLAGE_58_3_MQ02_ITEM1"));
	}
}

// 20285: Creating the Languid Herb Bomb
//-----------------------------------------------------------------------------
public class Huevillage583Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20285);
		SetName(L("Creating the Languid Herb Bomb"));
		SetDescription(L("The headman keeps his old explosives in the warehouse lot above the village."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_MQ03_NPC", "f_huevillage_58_3", L("Talk to Andale Village Headman"), L("He says the bomb used before must be somewhere. Talk to the Headman who knows where the bomb is."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_MQ03_DRUM", "f_huevillage_58_3", L("Find a Useful Bomb"), L("The Headman says that the bombs are in the northern warehouse. Look for a Useful Bomb."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_MQ03_NPC", "f_huevillage_58_3", L("Talk to Andale Village Headman"), L("Unlike what the Headman said, the bombs were in a small container. Ask the Headman what happened."));

		AddPrerequisite(new QuestStatusPrerequisite(20284, QuestStatus.Completed));

		AddObjective("findBomb", L("Collect Useful Bomb"), new CollectItemObjective("HUEVILLAGE_58_3_MQ03_ITEM1", 1));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("HUEVILLAGE_58_3_MQ03_ITEM2", 1));
		AddReward(new TakeItemReward("HUEVILLAGE_58_3_MQ03_ITEM1"));
	}
}

// 20286: A Drowsy Scent
//-----------------------------------------------------------------------------
public class Huevillage583Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20286);
		SetName(L("A Drowsy Scent"));
		SetDescription(L("Set the Languid Herb Bomb among the sleeping Upents of Melagingas Hill."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_3", "f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_MQ03_NPC", "f_huevillage_58_3", L("Talk to Andale Village Headman"), L("Made the Languid Bomb. Now ask the Headman what to do next."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_MQ04_NPC01", "f_huevillage_58_3", L("Use the Languid Bomb on the sleeping Upents"), L("The Headman says to use the bomb on the Upents in Melagingas Hill. Take it to Melagingas Hill."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_MQ04_TO_HUE1", "f_huevillage_58_1", L("Check the portal of Veja Ravine"), L("The villagers were demons in disguise. Make an escape to the portal in Veja Ravine."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_3_MQ04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20285, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat Demon Resident"), new KillObjective(3, "Banshee_pink") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 20287: Mysterious Well
//-----------------------------------------------------------------------------
public class Huevillage583Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20287);
		SetName(L("Mysterious Well"));
		SetDescription(L("Something lies at the bottom of the old well of Cobalt Forest."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_SQ01_NPC01", "f_huevillage_58_3", L("Go to the old well"), L("Check the old well in Cobalt Forest."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_SQ01_NPC02", "f_huevillage_58_3", L("Look for something to put in the old well"), L("There is something in the old well but you can't see it clearly. Look for something that you can use to get the thing out of the well."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_SQ01_NPC01", "f_huevillage_58_3", L("Go to the old well"), L("Use the long rope and bucket to get the thing out of the well."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("findBucket", L("Collect Bucket"), new CollectItemObjective("HUEVILLAGE_58_3_SQ01_BUCKET", 1));
		AddObjective("findRope", L("Collect Long Rope"), new CollectItemObjective("HUEVILLAGE_58_3_SQ01_ROPE", 1));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("COLLECT_101", 1));
		AddReward(new ItemReward("HUEVILLAGE_book03", 1));
		AddReward(new TakeItemReward("HUEVILLAGE_58_3_SQ01_BUCKET"));
		AddReward(new TakeItemReward("HUEVILLAGE_58_3_SQ01_ROPE"));
	}
}

// 20288: Dvyni Wetland's Colimencia
//-----------------------------------------------------------------------------
public class Huevillage583Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20288);
		SetName(L("Dvyni Wetland's Colimencia"));
		SetDescription(L("The wetland around the soul flowers has been trodden flat by something large."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_3_MQ02_FLOWER", "f_huevillage_58_3", L("Go to Dvyni Wetland"), L("Look for the Strongly Scented Soul Flower in Dvyni Wetland."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_3_MQ02_FLOWER", "f_huevillage_58_3", L("Defeat Colimencia"), L("Colimencia rose out of the wetland. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_3_MQ02_FLOWER", "f_huevillage_58_3", L("Defeat Colimencia"), L("Colimencia rose out of the wetland. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_3_SQ02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killColimencia", L("Defeat Colimencia"), new KillObjective(1, "boss_Colimencia") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Colimencia is down, and the wetland is still again."));
		character.Quests.Complete(this.QuestId);
	}
}
