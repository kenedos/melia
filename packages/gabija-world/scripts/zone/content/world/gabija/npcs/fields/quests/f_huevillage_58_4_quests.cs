//--- Melia Script ----------------------------------------------------------
// Septyni Glen Quest NPCs
//--- Description -----------------------------------------------------------
// Goddess Saule in her cage at the Grand Shrine, the two binding magic
// circles, the shrine offering tools and the demon barrier of Ishpirki.
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

public class FHuevillage584QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(18001);
	private readonly static QuestId Mq02 = new QuestId(18002);
	private readonly static QuestId Mq03 = new QuestId(18003);
	private readonly static QuestId Mq04 = new QuestId(18004);
	private readonly static QuestId Mq05 = new QuestId(18005);
	private readonly static QuestId Mq06 = new QuestId(18006);
	private readonly static QuestId Mq07 = new QuestId(18007);
	private readonly static QuestId Mq08 = new QuestId(18008);
	private readonly static QuestId Mq09 = new QuestId(50003);
	private readonly static QuestId Mq11 = new QuestId(18010);
	private readonly static QuestId GirlInDanger = new QuestId(18190);

	private const int OfferingToolsNeeded = 5;

	protected override void Load()
	{
		// Grand Shrine Barrier
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Grand Shrine Barrier"), "HUEVILLAGE_58_4_MQ01_NPC01", "f_huevillage_58_4", 43, -343, 90, c => c.Quests.HasCompleted(GirlInDanger) && !c.Quests.IsCompletable(Mq02) && !c.Quests.HasCompleted(Mq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grand Shrine Barrier"));

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("The barrier holds the shrine shut, and behind it something is being held down."),
					Option(L("Check the barrier at Saule Grand Shrine"), "accept"),
					Option(L("Stay outside the barrier"), "leave")
				);

				if (answer == "accept")
				{
					var checked18002 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (checked18002 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq02);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Harpeia still stands over the one held inside."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("The barrier across the Grand Shrine door. Its light has gone out."));
		});

		// Goddess Saule
		//-------------------------------------------------------------------------
		AddConditionalNpc(147385, L("Goddess Saule"), "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", 21.42, -186.01, 0, c => c.Quests.HasCompleted(GirlInDanger), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Goddess Saule"));
			dialog.SetPortrait("Dlg_port_Saule2");

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Thank you for saving me."));
				await dialog.Msg(L("You are... the only true savior who will save this world."));
				await dialog.Msg(L("The one who Goddess Laima talked about."));
				await dialog.Msg(L("I am Saule, the goddess of the sun. I was the guardian of the revelation."));
				await dialog.CompleteQuest(Mq02);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Both circles are broken. The weight of them is off me at last."));
				await dialog.Msg(L("But the cage itself is not theirs to hold, and it holds me still."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("I should be able to work with this amount of divine energy."));
				await dialog.CompleteQuest(Mq05);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Clymen... I can feel its presence at the Ishpirki Access Road."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("That is the key."));
				await dialog.Msg(L("Please free me from this cage."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("I am finally free."));
				await dialog.Msg(L("I will prepare to interpret the revelation from this point on."));
				await dialog.Msg(L("As I have told you before, the revelation is in the hands of the Demon Lord Bramble."));
				await dialog.Msg(L("I feel sorry that I have to burden you, but your strength is the only thing I can rely on."));
				await dialog.Msg(L("But Bramble is also not in its usual condition due to the previous battle."));
				await dialog.Msg(L("It is recovering by turning the forest into a thorn forest and extracting nourishment."));
				await dialog.Msg(L("My Believers are waiting for you in Gate Route."));
				await dialog.Msg(L("Please take back the revelation from Bramble."));
				await dialog.CompleteQuest(Mq08);

				if (!character.Quests.Has(Mq09))
					character.Quests.Start(Mq09);

				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("The portal that you tried to open was a false gateway."));
				await dialog.Msg(L("That portal leads to where all of the world's knowledge is."));
				await dialog.Msg(L("Trusting Laima's words, I have been fighting the Demon Lord Bramble, who tried to take over the revelation."));
				await dialog.Msg(L("I have fought with it night and day to be able to hand the revelation to you."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("But, ultimately, Medzio Diena, that the demons caused broke the balance of power. The goddesses lost most of their strength since that day."),
					Option(L("I'll destroy it"), "accept"),
					Option(L("I need more time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					SyncBindingCircles(character);
					await dialog.Msg(L("The Binding Magic Circles are located in Drugys Courtyard and Vapsva Vacant Lot."));
					await dialog.Msg(L("Please hurry before I lose my consciousness."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("This cage is still confining me."));

				var answer = await dialog.SelectQuestOffer(Mq05, L("We've got to find Clymen who has the key and is hiding in between the dimensional crack."),
					Option(L("What should I do?"), "accept"),
					Option(L("Wait until my strength is fully recovered"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					character.LookAround();
					await dialog.Msg(L("Please bring me the sacrifice tools in the Altar Grand Corridor."));
					await dialog.Msg(L("I will try to use the divine power that dwells in the tool."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				var answer = await dialog.SelectQuestOffer(Mq06, L("I will now focus on finding Clymen. Please remove any nearby demons in order to moderate their evil energies."),
					Option(L("I'll defeat the monsters nearby"), "accept"),
					Option(L("I'm sorry, but I don't think I can"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				var answer = await dialog.SelectQuestOffer(Mq08, L("The key turns in the lock of the cage, and the light of the restraining sphere begins to give."),
					Option(L("Remove the restraining sphere"), "accept"),
					Option(L("Wait a moment longer"), "leave")
				);

				if (answer == "accept")
				{
					var freed18008 = await character.TimeActions.StartAsync(L("Removing the restraints..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (freed18008 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq08);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("My Believers are waiting for you in Gate Route."));
				await dialog.Msg(L("Please take back the revelation from Bramble."));
				character.Quests.Start(Mq09);
				return;
			}

			if (!character.Quests.Has(Mq11) && character.Quests.MeetsPrerequisites(Mq11))
			{
				await dialog.Msg(L("Bramble..."));
				await dialog.Msg(L("It is a pity that it went against its foreseen fate."));

				var answer = await dialog.SelectQuestOffer(Mq11, L("Please show me the revelation. And I will show you its meaning."),
					Option(L("Show the revelation"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq11);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Binding Magic Circles are located in Drugys Courtyard and Vapsva Vacant Lot."));
				await dialog.Msg(L("Please hurry before I lose my consciousness."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("If there are any shrine offering tools around that still contains divine energy, then we can surely use them to find Clymen."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Just a little bit more. Please suppress the evil energy."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Clymen holds the key. It is at the Ishpirki Access Road."));
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("The sphere is still closed. Turn the key once more."));
				character.Quests.ReplayQuestTrack(Mq08);
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("To retrieve the revelation, please go to Sirdgela Forest."));
				await dialog.Msg(L("Now is the chance when the Demon Lord Bramble is recovering its power."));
				return;
			}

			if (character.Quests.IsActive(Mq11))
			{
				await dialog.Msg(L("Hold the revelation up to the light and I will read it for you."));
				character.Quests.ReplayQuestTrack(Mq11);
				return;
			}

			await dialog.Msg(L("I will be able to guide you to the revelation if I use all the strength left in me."));
			await dialog.Msg(L("That is why you should release me."));
		});

		// Binding Magic Circle at Drugys Courtyard
		//-------------------------------------------------------------------------
		AddConditionalNpc(147417, L("Binding Magic Circle"), "HUEVILLAGE_58_4_MQ03_NPC01", "f_huevillage_58_4", -879, -737, 91, c => !c.Quests.HasCompleted(Mq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Binding Magic Circle"));

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("Dark energy runs along the lines of the circle, and it has been kept fed."),
					Option(L("Check the binding magic circle"), "accept"),
					Option(L("Keep clear of the circle"), "leave")
				);

				if (answer == "accept")
				{
					var looked18003 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (looked18003 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq03);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Mothstem still guards the circle."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("A binding magic circle, cut into the courtyard stone."));
		});

		// Binding Magic Circle at Vapsva Vacant Lot
		//-------------------------------------------------------------------------
		AddConditionalNpc(147417, L("Binding Magic Circle"), "HUEVILLAGE_58_4_MQ04_NPC01", "f_huevillage_58_4", 426, 705, 90, c => !c.Quests.HasCompleted(Mq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Binding Magic Circle"));

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("The second circle burns lower than the first, but the same hand drew it."),
					Option(L("Check the binding magic circle"), "accept"),
					Option(L("Keep clear of the circle"), "leave")
				);

				if (answer == "accept")
				{
					var looked18004 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

					if (looked18004 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq04);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Merge still guards the circle."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("A binding magic circle, laid out on the bare ground of the lot."));
		});

		// Shrine Offering Tools
		//-------------------------------------------------------------------------
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC01", "f_huevillage_58_4", 528.19, -45.39, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC02", "f_huevillage_58_4", 690.47, -60.85, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC03", "f_huevillage_58_4", 860.05, -24.10, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC04", "f_huevillage_58_4", 946.71, -390.39, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC05", "f_huevillage_58_4", 808.60, -312.94, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC06", "f_huevillage_58_4", 583.82, -401.98, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);
		AddConditionalNpc(151022, L("Shrine Offering Tools"), "HUEVILLAGE_58_4_MQ05_NPC07", "f_huevillage_58_4", 371.24, -365.04, 90, c => c.Quests.IsActive(Mq05), this.TakeOfferingTool);

		// Demon Barrier
		//-------------------------------------------------------------------------
		AddConditionalNpc(147372, L("Demon Barrier"), "HUEVILLAGE_58_4_MQ07_NPC01", "f_huevillage_58_4", 1296, -245, 90, c => !c.Quests.IsCompletable(Mq07) && !c.Quests.HasCompleted(Mq07), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Barrier"));

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				var answer = await dialog.SelectQuestOffer(Mq07, L("The goddess pointed out the seam of the barrier. Clymen is behind it, with the key."),
					Option(L("Release the Demon Barrier"), "accept"),
					Option(L("Leave the barrier closed"), "leave")
				);

				if (answer == "accept")
				{
					var broke18007 = await character.TimeActions.StartAsync(L("Breaking the barrier..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (broke18007 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq07);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Clymen is still in the crack behind the barrier."));
				character.Quests.ReplayQuestTrack(Mq07);
				return;
			}

			await dialog.Msg(L("A demon barrier laid across the Ishpirki Access Road. The air inside it will not settle."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The way into Sirdgela Forest from Gate Route.
		AddQuestTrigger("THORN20_THORN21", "d_thorn_20", -1441, -1879, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09))
				character.Quests.CompleteObjective(Mq09, "reachKvailas");

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Hands out the offering tools Goddess Saule draws strength from.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task TakeOfferingTool(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Shrine Offering Tools"));

		if (!character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("An offering tool of the shrine, its divine energy long spent."));
			return;
		}

		if (character.Inventory.CountItem(ItemId.HUEVILLAGE_58_4_MQ05_ITEM1) >= OfferingToolsNeeded)
		{
			await dialog.Msg(L("You are carrying as many tools as the goddess can draw on."));
			return;
		}

		var lifted = await character.TimeActions.StartAsync(L("Lifting the offering tool..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

		if (lifted != TimeActionResult.Completed)
			return;

		character.Inventory.Add(ItemId.HUEVILLAGE_58_4_MQ05_ITEM1, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("There is divine energy left in this one. You lift it out of its stand."));
	}

	/// <summary>
	/// Marks each binding magic circle the character already broke on
	/// Release Goddess Saule (1), whichever order the two were done in.
	/// </summary>
	public static void SyncBindingCircles(Character character)
	{
		if (!character.Quests.IsActive(Mq01))
			return;

		if (character.Quests.HasCompleted(Mq03))
			character.Quests.CompleteObjective(Mq01, "breakDrugys");

		if (character.Quests.HasCompleted(Mq04))
			character.Quests.CompleteObjective(Mq01, "breakVapsva");
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 18002: Entrapped Goddess
//-----------------------------------------------------------------------------
public class Huevillage584Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18002);
		SetName(L("Entrapped Goddess"));
		SetDescription(L("A barrier holds the Saule Grand Shrine shut, and something is being held down behind it."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_MQ01_NPC01", "f_huevillage_58_4", L("Check the barrier at Saule Grand Shrine"), L("You are blocked by the barrier. Check the barrier at Saule Grand Shrine."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ01_NPC01", "f_huevillage_58_4", L("Defeat the demon restraining the goddess"), L("The barrier disappeared and you can see someone trapped by the demon. First, defeat the demon."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("It was Goddess Saule who was restrained by the demons. Talk to Goddess Saule."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ02_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(18190, QuestStatus.Completed));

		AddObjective("killHarpeia", L("Defeat Harpeia"), new KillObjective(1, "boss_Harpeia") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 18001: Release Goddess Saule (1)
//-----------------------------------------------------------------------------
public class Huevillage584Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18001);
		SetName(L("Release Goddess Saule (1)"));
		SetDescription(L("Two binding magic circles hold the goddess down. Break both of them."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("The lady trapped was Goddess Saule. Talk to Goddess Saule."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ03_NPC01", "f_huevillage_58_4", L("Destroy the restraining magic circles"), L("Goddess Saule has been trapped by the demons and can't use her powers. Destroy the binding magic circle that is restraining the goddess in Drugys Courtyard and Vapsva Vacant Lot."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Destroyed all the restraining magic circles. Talk to Goddess Saule."));

		AddPrerequisite(new QuestStatusPrerequisite(18002, QuestStatus.Completed));

		AddObjective("breakDrugys", L("Destroy the magic circle in Drugys Courtyard"), new ManualObjective());
		AddObjective("breakVapsva", L("Destroy the magic circle in Vapsva Vacant Lot"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 18003: Drugys Courtyard's Binding Magic Circle
//-----------------------------------------------------------------------------
public class Huevillage584Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18003);
		SetName(L("Drugys Courtyard's Binding Magic Circle"));
		SetDescription(L("The binding magic circle of Drugys Courtyard is guarded by what feeds it."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_MQ03_NPC01", "f_huevillage_58_4", L("Check the binding magic circle in Drugys Courtyard"), L("There is a binding magic circle imbued with dark energy in Drugys Courtyard. Demons have prepared it. Go check it out."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ03_NPC01", "f_huevillage_58_4", L("Defeat Mothstem"), L("Mothstem rose from the circle. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_MQ03_NPC01", "f_huevillage_58_4", L("Defeat Mothstem"), L("Mothstem rose from the circle. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killMothstem", L("Defeat Mothstem"), new KillObjective(1, "boss_Mothstem") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The circle in Drugys Courtyard has gone dark."));
		character.Quests.Complete(this.QuestId);
		FHuevillage584QuestNpcsScript.SyncBindingCircles(character);
	}
}

// 18004: Vapsva Vacant Lot's Binding Magic Circle
//-----------------------------------------------------------------------------
public class Huevillage584Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18004);
		SetName(L("Vapsva Vacant Lot's Binding Magic Circle"));
		SetDescription(L("The binding magic circle of Vapsva Vacant Lot is guarded by what feeds it."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_MQ04_NPC01", "f_huevillage_58_4", L("Check the binding magic circle in Vapsva Vacant Lot"), L("There is a binding magic circle imbued with dark energy in Vapsva Vacant Lot. Demons have prepared it. Go check it out."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ04_NPC01", "f_huevillage_58_4", L("Defeat Merge"), L("Merge rose from the circle. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_MQ04_NPC01", "f_huevillage_58_4", L("Defeat Merge"), L("Merge rose from the circle. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killMerge", L("Defeat Merge"), new KillObjective(1, "boss_Merge") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The circle in Vapsva Vacant Lot has gone dark."));
		character.Quests.Complete(this.QuestId);
		FHuevillage584QuestNpcsScript.SyncBindingCircles(character);
	}
}

// 18005: Release Goddess Saule (2)
//-----------------------------------------------------------------------------
public class Huevillage584Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18005);
		SetName(L("Release Goddess Saule (2)"));
		SetDescription(L("The goddess needs the divine energy still left in the offering tools of the Altar Grand Corridor."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Removed the magic circle that was trapping Goddess Saule, but it is not enough. Talk to Goddess Saule again."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ05_NPC01", "f_huevillage_58_4", L("Collect the offering tools at Altar Grand Gallery"), L("Find the offering tools at Altar Grand Gallery to help the goddess regain her powers."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Give it to Goddess Saule"), L("Found the materials to help the goddess. Give them to the goddess."));

		AddPrerequisite(new QuestStatusPrerequisite(18001, QuestStatus.Completed));

		AddObjective("collectTools", L("Collect Shrine Offering Tools"), new CollectItemObjective("HUEVILLAGE_58_4_MQ05_ITEM1", 5));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 18006: Release Goddess Saule (3)
//-----------------------------------------------------------------------------
public class Huevillage584Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18006);
		SetName(L("Release Goddess Saule (3)"));
		SetDescription(L("The goddess cannot find Clymen while the demons around the shrine keep their hold."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Done preparing to help the goddess regain her strength. Talk to Goddess Saule."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Defeat the monsters nearby"), L("Goddess Saule says defeating the monsters nearby will help in finding Clymen. Defeat the monsters and suppress the evil energy."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Seems like Goddess Saule found the demons. Return to Goddess Saule."));

		AddPrerequisite(new QuestStatusPrerequisite(18005, QuestStatus.Completed));

		AddObjective("clearShrine", L("Defeat monsters nearby the Grand Shrine"), new KillObjective(15, "Beeteros", "Mentiwood", "Carcashu", "Tiny_mage"));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 18007: Release Goddess Saule (4)
//-----------------------------------------------------------------------------
public class Huevillage584Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18007);
		SetName(L("Release Goddess Saule (4)"));
		SetDescription(L("Clymen holds the key to the cage, behind a demon barrier on the Ishpirki Access Road."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_MQ07_NPC01", "f_huevillage_58_4", L("Release the Demon Barrier"), L("Goddess Saule got her strength back with the help of the offering tools. She found the barrier with the demon who has the key. Go defeat it."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_MQ07_NPC01", "f_huevillage_58_4", L("Defeat the demon that held the key"), L("Defeat the demon that appears from the barrier and get the key."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Give it to Goddess Saule"), L("You found the key to finally release Goddess Saule from the restraining sphere. Bring the key to Goddess Saule."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ07_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(18006, QuestStatus.Completed));

		AddPityDrop("HUEVILLAGE_58_4_MQ07_ITEM1", 1.0f, 0, 1, "boss_Clymen");

		AddObjective("takeKey", L("Collect the Confinement Key"), new CollectItemObjective("HUEVILLAGE_58_4_MQ07_ITEM1", 1));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("TreasureboxKey2", 1));
		AddReward(new TakeItemReward("HUEVILLAGE_58_4_MQ05_ITEM1"));
	}
}

// 18008: Goddess Saule
//-----------------------------------------------------------------------------
public class Huevillage584Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18008);
		SetName(L("Goddess Saule"));
		SetDescription(L("Use the Confinement Key on the restraining sphere and free the goddess."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Remove restraining sphere that is trapping Goddess Saule"), L("Use the key to release Goddess Saule from the restraining sphere."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Remove restraining sphere that is trapping Goddess Saule"), L("Use the key to release Goddess Saule from the restraining sphere."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Goddess Saule is freed. Talk to her."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ08_TRACK", 3000);

		AddPrerequisite(new QuestStatusPrerequisite(18007, QuestStatus.Completed));

		AddObjective("freeSaule", L("Remove the restraining sphere"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("HUEVILLAGE_58_4_MQ07_ITEM1"));
	}
}

// 50003: To Kvailas Forest
//-----------------------------------------------------------------------------
public class Huevillage584Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50003);
		SetName(L("To Kvailas Forest"));
		SetDescription(L("Bramble took the revelation into the Thorn Forest. Make for Sirdgela Forest by way of Gate Route."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4", "d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("Goddess Saule says that Bramble stole the revelation in the Thorn Forest. Move to the Thorn Forest."));
		SetPhase(QuestStatus.InProgress, "THORN20_THORN21", "d_thorn_20", L("Move to Kvailas Forest"), L("Help the believers at Gate Route and Sirdgela Forest and look for the revelation at Kvailas Forest."));
		SetPhase(QuestStatus.Success, "THORN20_THORN21", "d_thorn_20", L("Move to Kvailas Forest"), L("Help the believers at Gate Route and Sirdgela Forest and look for the revelation at Kvailas Forest."));

		AddPrerequisite(new QuestStatusPrerequisite(18008, QuestStatus.Completed));

		AddObjective("reachKvailas", L("Move to Sirdgela Forest"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The arrival is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("You have arrived at Sirdgela Forest, where Bramble is hiding!"));
		character.Quests.Complete(this.QuestId);
	}
}

// 18010: To Gateway of the Great King
//-----------------------------------------------------------------------------
public class Huevillage584Mq11Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18010);
		SetName(L("To Gateway of the Great King"));
		SetDescription(L("Goddess Saule reads the Revelation of Kvailas Forest for you."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_4");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Ask Goddess Saule about the revelation"), L("You defeated Bramble and obtained the revelation. Return to Goddess Saule to interpret the revelation."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Ask Goddess Saule about the revelation"), L("You defeated Bramble and obtained the revelation. Return to Goddess Saule to interpret the revelation."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_4_SAULE_BEFORE", "f_huevillage_58_4", L("Talk to Goddess Saule"), L("The revelation tells you to go to Great King Zachariel's Royal Mausoleum. Talk to the goddess."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_4_MQ11_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(20275, QuestStatus.Completed));

		AddObjective("readRevelation", L("Have the revelation read"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("stonetablet03", 1));
		AddReward(new TakeItemReward("stonetablet031"));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The reading is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}
