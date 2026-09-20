//--- Melia Script ----------------------------------------------------------
// Vieta Gorge Quest NPCs
//--- Description -----------------------------------------------------------
// The Andale Village elder and priest, the Ershike Altar, the sap buckets of
// White Oak Forest and the Obelisk of Slepingas Stream.
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

public class FHuevillage582QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20276);
	private readonly static QuestId Mq02 = new QuestId(20277);
	private readonly static QuestId Mq03 = new QuestId(20278);
	private readonly static QuestId Mq04 = new QuestId(20279);
	private readonly static QuestId Sq01 = new QuestId(20280);
	private readonly static QuestId Sq02 = new QuestId(20281);
	private readonly static QuestId Sq03 = new QuestId(20282);

	private const int WhiteOakSapNeeded = 3;

	protected override void Load()
	{
		// Old Man of Andale Village
		//-------------------------------------------------------------------------
		AddNpc(147396, L("Old Man of Andale Village"), "HUEVILLAGE_58_2_MQ01_NPC", "f_huevillage_58_2", -186.69, -1570.87, 72, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Man of Andale Village"));

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("So you're the one who wants to meet Goddess Saule."));
				await dialog.Msg(L("If you're here about the pond, don't worry. Our priest is working on it."));

				var answer = await dialog.Select(L("The priest is at Cerpe Crossroads. I heard he needed Black Maize Venom, so try getting that to him as a gift."),
					Option(L("I'll meet the priest"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					character.Inventory.Add(ItemId.HUEVILLAGE_58_2_MQ01_ITEM2, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Use this tranquilizer on the Black Maize and extract its venom."));
					await dialog.Msg(L("The Maizes need to be weakened for the tranquilizer to work."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Bring that venom to the Village Priest at Cerpe Crossroads."));
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("We would have never been able to see the portal even in our dreams if it wasn't for you."));
				await dialog.Msg(L("You truly are God's grace."));
				return;
			}

			await dialog.Msg(L("The Black Maize graze along the gorge floor. Weaken one before you reach for the tranquilizer."));
		});

		// Andale Village Priest
		//-------------------------------------------------------------------------
		AddNpc(147409, L("Andale Village Priest"), "HUEVILLAGE_58_2_MQ02_NPC", "f_huevillage_58_2", -239.14, -200.18, 72, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Andale Village Priest"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Aren't these Black Maize Venom?"));
				await dialog.Msg(L("Oh, thank you very much!"));
				await dialog.Msg(L("You can count on me on helping you meet Goddess Saule."));
				await dialog.Msg(L("Now that the Holy Pond has been purified, we just need to solve the problem with the Obelisk."));
				character.Quests.Complete(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You're almost done."));
				await dialog.Msg(L("After completing the rituals to cleanse your body, you will be able to meet the goddess."));
				await dialog.Msg(L("Follow the road on the right to go to Cobalt Forest."));
				await dialog.Msg(L("Our village priest there is waiting for you."));
				character.Quests.Complete(Mq04);
				character.ServerMessage(L("Go and find the Andale Village Priest of Cobalt Forest."));
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Magic letters are carved on the Obelisk."));
				await dialog.Msg(L("But some letters were erased since the Holy Pond became corrupted."));

				var answer = await dialog.Select(L("If the letters are gone, then we will need to write them back on. Now that you've collected the Black Maize Venom, we will need White Oak Sap."),
					Option(L("How do we restore it?"), "accept"),
					Option(L("I will leave it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("When you have all materials ready, combine them at the Ershike Altar."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("I see you've completed making the dye."));

				var answer = await dialog.Select(L("Now go and draw in new letters according to the faint patterns left on the Obelisk."),
					Option(L("Where is the Obelisk?"), "accept"),
					Option(L("Which god is this for?"), "explain"),
					Option(L("Restore it yourself"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("God or goddess, their title isn't really important."));
					await dialog.Msg(L("Eventually, aren't we all just one being?"));
					await dialog.Msg(L("Is this hard to comprehend? You will understand it one day."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("The Obelisk is located at Slepingas Stream."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I think we'll be able to meet Goddess Saule. We just need to restore the Obelisk."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("I'm sure the goddesses will be very pleased with you."));
				return;
			}

			await dialog.Msg(L("The Obelisk has stood at Slepingas Stream longer than the village has."));
		});

		// Tree Sap Collection Containers
		//-------------------------------------------------------------------------
		AddNpc(151028, L("Tree Sap Collection Container"), "HUEVILLAGE_58_2_MQ02_BUCKET01", "f_huevillage_58_2", -89, 1399, 90, this.CollectSap);
		AddNpc(151028, L("Tree Sap Collection Container"), "f_huevillage_58_2", -243, 1305, 90, this.CollectSap);
		AddNpc(151028, L("Tree Sap Collection Container"), "f_huevillage_58_2", -345, 1134, 90, this.CollectSap);
		AddNpc(151028, L("Tree Sap Collection Container"), "f_huevillage_58_2", 83, 1278, 90, this.CollectSap);
		AddNpc(147354, L("Tree Sap Collection Container"), "HUEVILLAGE_58_2_MQ02_BUCKET02", "f_huevillage_58_2", 181, 1363, 90, this.CollectSap);

		// Ershike Altar
		//-------------------------------------------------------------------------
		AddNpc(147417, L("Ershike Altar"), "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", -439, 234, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Ershike Altar"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The mixture on the altar stone has set into a dark, even coat. You lift it off."));
				character.Quests.Complete(Mq03);
				character.ServerMessage(L("Return to the Andale Village Priest."));
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.Select(L("The venom and the sap go into the bowl of the altar together, the way the priest described it."),
					Option(L("Combine the materials"), "accept"),
					Option(L("Wait a while"), "leave")
				);

				if (answer == "accept")
				{
					var mixed20278 = await character.TimeActions.StartAsync(L("Mixing the ingredients..."), L("Cancel"), "FLASK", TimeSpan.FromSeconds(3));

					if (mixed20278 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq03);
					character.Quests.CompleteObjective(Mq03, "mixDye");
					character.ServerMessage(L("The dye is complete. Take the dye from the altar."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.Select(L("An ominous energy comes off the altar stone, and the beasts of the crossroads have turned toward it."),
					Option(L("Find out what is happening"), "accept"),
					Option(L("Step away from the altar"), "leave")
				);

				if (answer == "accept")
				{
					var combined20281 = await character.TimeActions.StartAsync(L("Combining the ingredients..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

					if (combined20281 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Sq02);
					character.ServerMessage(L("Defeat the monsters that reacted to the ominous energy!"));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The energy still hangs over the stone. The beasts drawn to it are not thinned out yet."));
				return;
			}

			await dialog.Msg(L("The Ershike Altar. Its bowl is worn smooth from generations of mixing."));
		});

		// Obelisk
		//-------------------------------------------------------------------------
		AddNpc(147414, L("Obelisk"), "HUEVILLAGE_58_2_OBELISK_BEFORE", "f_huevillage_58_2", 859, 205, 20, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Obelisk"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You follow the faint patterns left in the stone and draw the letters back in with the dye."));
				var restored20279 = await character.TimeActions.StartAsync(L("Restoring the Obelisk..."), L("Cancel"), "READ", TimeSpan.FromSeconds(3));

				if (restored20279 != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq04, "restoreObelisk");
				character.ServerMessage(L("Return to the Andale Village Priest."));
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.Select(L("Something has been circling the Obelisk, and the ground on its stream side is trodden flat."),
					Option(L("Check the Obelisk"), "accept"),
					Option(L("Leave it be"), "leave")
				);

				if (answer == "accept")
				{
					var looked20282 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "READ", TimeSpan.FromSeconds(3));

					if (looked20282 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Sq03);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("The Blue Woodspirit is still on its feet."));
				character.Quests.ReplayQuestTrack(Sq03);
				return;
			}

			await dialog.Msg(L("Magic letters run the height of the Obelisk, and whole lines of them have worn away."));
		});

		// Swift currents of Nefrito Valley
		//-------------------------------------------------------------------------
		AddNpc(40095, L("Nefrito Valley"), "HUEVILLAGE_58_2_SQ01_NPC", "f_huevillage_58_2", 58.32, 653.91, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Nefrito Valley"));

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.Select(L("A strong energy runs with the water here, and it is not coming from the current."),
					Option(L("Find out what it is"), "accept"),
					Option(L("Keep to the bank"), "leave")
				);

				if (answer == "accept")
				{
					var looked20280 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

					if (looked20280 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Sq01);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Moldihorn is still standing in the water."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			await dialog.Msg(L("The current runs fast and cold through Nefrito Valley."));
		});
	}

	/// <summary>
	/// Hands out White Oak Sap for the Obelisk dye.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task CollectSap(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Tree Sap Collection Container"));

		if (!character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("A collection container strapped to a white oak. The sap in it has long gone hard."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.HUEVILLAGE_58_2_MQ02_ITEM) >= WhiteOakSapNeeded)
		{
			await dialog.Msg(L("You have as much sap as the priest asked for."));
			return;
		}

		character.Inventory.Add(ItemId.HUEVILLAGE_58_2_MQ02_ITEM, 1, InventoryAddType.PickUp);
		await dialog.Msg(L("You tip the container and pour off the sap that has gathered in it."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20276: The Unopened Portal
//-----------------------------------------------------------------------------
public class Huevillage582Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20276);
		SetName(L("The Unopened Portal"));
		SetDescription(L("The Andale Village elder sends you to the priest at Cerpe Crossroads, with Black Maize Venom as a gift."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_MQ01_NPC", "f_huevillage_58_2", L("Talk to the Old Man of Andale Village"), L("Villagers told you to go to the elderly of Andale Village. Go to the elderly of Andale Village in Vieta Gorge."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_MQ01_NPC", "f_huevillage_58_2", L("Collect Black Maize Venom"), L("Collect some Black Maize Venom as a gift to the Village Priest."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_MQ02_NPC", "f_huevillage_58_2", L("Go to the Andale Village Priest"), L("Collected enough Black Maize Venom. Go to the Andale Village Priest at Cerpe Crossroads."));

		AddPrerequisite(new QuestStatusPrerequisite(18130, QuestStatus.Completed));

		AddPityDrop("HUEVILLAGE_58_2_MQ01_ITEM1", 0.35f, 5, 1, "Zibu_Maize");

		AddObjective("collectVenom", L("Defeat Black Maize to obtain Black Maize Venom"), new CollectItemObjective("HUEVILLAGE_58_2_MQ01_ITEM1", 5));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new TakeItemReward("HUEVILLAGE_58_2_MQ01_ITEM2"));
	}
}

// 20277: Activate the Obelisk (1)
//-----------------------------------------------------------------------------
public class Huevillage582Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20277);
		SetName(L("Activate the Obelisk (1)"));
		SetDescription(L("The dye that restores the Obelisk needs White Oak Sap. Draw it from the collection containers of White Oak Forest."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_MQ02_NPC", "f_huevillage_58_2", L("Talk to the Village Priest"), L("The Village Priest says you can activate the Obelisk if you follow his instructions. Talk to him again."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_MQ02_BUCKET01", "f_huevillage_58_2", L("Collect tree sap from White Oak Forest"), L("Tree sap is needed to make the dye for restoring the Obelisk. Collect tree sap at White Oak Forest."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_MQ02_BUCKET01", "f_huevillage_58_2", L("Collect tree sap from White Oak Forest"), L("Tree sap is needed to make the dye for restoring the Obelisk. Collect tree sap at White Oak Forest."));

		AddPrerequisite(new QuestStatusPrerequisite(20276, QuestStatus.Completed));

		AddObjective("collectSap", L("Obtain White Oak Sap from the tree sap collection containers"), new CollectItemObjective("HUEVILLAGE_58_2_MQ02_ITEM", 3));

		AddReward(new ItemReward("expCard3", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The collection is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("You have collected all the tree sap. Go to the Ershike Altar and combine the materials!"));
		character.Quests.Complete(this.QuestId);
	}
}

// 20278: Activate the Obelisk (2)
//-----------------------------------------------------------------------------
public class Huevillage582Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20278);
		SetName(L("Activate the Obelisk (2)"));
		SetDescription(L("Combine the venom and the sap at the Ershike Altar to make the dye."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Go to the Ershike Altar"), L("Collected all the materials needed to restore the Obelisk. Go to the Ershike Altar to combine them."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Go to the Ershike Altar"), L("Collected all the materials needed to restore the Obelisk. Go to the Ershike Altar to combine them."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Retrieve dye from the Ershike Altar"), L("Made the dye for restoring the Obelisk. Get the dye from the altar."));

		AddPrerequisite(new QuestStatusPrerequisite(20277, QuestStatus.Completed));

		AddObjective("mixDye", L("Combine the materials at the Ershike Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("HUEVILLAGE_58_2_MQ03_ITEM", 1));
		AddReward(new TakeItemReward("HUEVILLAGE_58_2_MQ01_ITEM1"));
		AddReward(new TakeItemReward("HUEVILLAGE_58_2_MQ02_ITEM"));
	}
}

// 20279: Activate the Obelisk (3)
//-----------------------------------------------------------------------------
public class Huevillage582Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20279);
		SetName(L("Activate the Obelisk (3)"));
		SetDescription(L("Write the erased letters back onto the Obelisk at Slepingas Stream."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_MQ02_NPC", "f_huevillage_58_2", L("Talk to the Village Priest"), L("Got the dye for restoring the Obelisk. Return and talk to the Priest again for what to do next."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_OBELISK_BEFORE", "f_huevillage_58_2", L("Restore the Obelisk"), L("Seems like the Obelisk can now be restored. Write back the erased letters on the Obelisk at Slepingas Stream."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_MQ02_NPC", "f_huevillage_58_2", L("Talk to the Village Priest"), L("Successfully restored the Obelisk. Return to Village Priest."));

		AddPrerequisite(new QuestStatusPrerequisite(20278, QuestStatus.Completed));

		AddObjective("restoreObelisk", L("Restore the Obelisk"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("HUEVILLAGE_58_2_MQ03_ITEM"));
	}
}

// 20280: Moldyhorn of Nefrito Valley
//-----------------------------------------------------------------------------
public class Huevillage582Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20280);
		SetName(L("Moldyhorn of Nefrito Valley"));
		SetDescription(L("A strong energy runs with the water of Nefrito Valley."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_SQ01_NPC", "f_huevillage_58_2", L("Go to Nefrito Valley"), L("A strong energy is felt in the swift currents of Nefrito Valley. Find out what it is."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_SQ01_NPC", "f_huevillage_58_2", L("Defeat Moldihorn"), L("Moldihorn rose out of the current. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_SQ01_NPC", "f_huevillage_58_2", L("Defeat Moldihorn"), L("Moldihorn rose out of the current. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_2_SQ01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killMoldyhorn", L("Defeat Moldihorn"), new KillObjective(1, "boss_Moldyhorn") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Moldihorn is down, and the current runs clean again."));
		character.Quests.Complete(this.QuestId);
	}
}

// 20281: Ominous Energy at Ershike Altar
//-----------------------------------------------------------------------------
public class Huevillage582Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20281);
		SetName(L("Ominous Energy at Ershike Altar"));
		SetDescription(L("The beasts of Cerpe Crossroads turned aggressive when the Ershike Altar began to fume."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Check the Ershike Altar"), L("An ominous energy is felt from the Ershike altar. Find out what is happening."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Defeat the monsters that reacted to the ominous energy"), L("The surrounding monsters turned aggressive when an ominous energy fumed from the Ershike Altar. Defeat those monsters."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_MQ03_NPC", "f_huevillage_58_2", L("Defeat the monsters that reacted to the ominous energy"), L("The surrounding monsters turned aggressive when an ominous energy fumed from the Ershike Altar. Defeat those monsters."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("clearCrossroads", L("Defeat the monsters that reacted to the ominous energy"), new KillObjective(12, "Ultanun", "Zibu_Maize"));

		AddReward(new ItemReward("expCard3", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kills are the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The energy over the altar stone has gone out."));
		character.Quests.Complete(this.QuestId);
	}
}

// 20282: Slepingas Stream's Blue Woodspirit
//-----------------------------------------------------------------------------
public class Huevillage582Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20282);
		SetName(L("Slepingas Stream's Blue Woodspirit"));
		SetDescription(L("Something has been circling the Obelisk of Slepingas Stream."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_2_OBELISK_BEFORE", "f_huevillage_58_2", L("Check the Obelisk"), L("Check the Obelisk in Slepingas Stream."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_2_OBELISK_BEFORE", "f_huevillage_58_2", L("Defeat Blue Woodspirit"), L("Defeat the Blue Woodspirit that appeared near the Obelisk."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_2_OBELISK_BEFORE", "f_huevillage_58_2", L("Defeat Blue Woodspirit"), L("Defeat the Blue Woodspirit that appeared near the Obelisk."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_2_SQ03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killWoodspirit", L("Defeat Blue Woodspirit"), new KillObjective(1, "boss_woodspirit_blue") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Blue Woodspirit is down. The ground by the Obelisk is quiet."));
		character.Quests.Complete(this.QuestId);
	}
}
