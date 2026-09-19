//--- Melia Script ----------------------------------------------------------
// Tenet Garden Quest NPCs
//--- Description -----------------------------------------------------------
// The Watchers, the Followers and the beasts the garden's quests run on.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele574QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8601);
	private readonly static QuestId Mq02 = new QuestId(8602);
	private readonly static QuestId Mq03 = new QuestId(8603);
	private readonly static QuestId Mq04 = new QuestId(8604);
	private readonly static QuestId Mq05 = new QuestId(8605);
	private readonly static QuestId Mq06 = new QuestId(8606);
	private readonly static QuestId Mq07 = new QuestId(8607);
	private readonly static QuestId Mq08 = new QuestId(8608);
	private readonly static QuestId Mq09 = new QuestId(8609);

	protected override void Load()
	{
		// Watcher Rikke
		//-------------------------------------------------------------------------
		AddNpc(147422, L("Watcher Rikke"), "GELE574_REIKE", "f_gele_57_4", -1350, 389, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Rikke"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Thank you so much!"));
				await dialog.Msg(L("How did it come from such a far forest? Strange, huh?"));
				character.Quests.Complete(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("They're not listening?"));
				await dialog.Msg(L("Could the Panto Totems in Valyma Sanctum be causing problems?"));
				character.Quests.Complete(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("As I thought, I still have some materials left."));
				await dialog.Msg(L("Thank you though."));
				character.Quests.Complete(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Biteregina, which used to live in the deep forest, has appeared here."));
				await dialog.Msg(L("It even built a hive at the upper side of Rojus Plateau. Since it's dangerous, can you defeat it?"));

				var answer = await dialog.Select(L("It's easy"),
					Option(L("It's easy"), "accept"),
					Option(L("It looks dangerous"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq01);

				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("I received a mysterious charm from the elders that will brainwash Pantos to stand on our side."));
				await dialog.Msg(L("Are you up for a challenge?"));

				var answer = await dialog.Select(L("Accept immediately"),
					Option(L("Accept immediately"), "accept"),
					Option(L("It looks dangerous"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("After using the charm on the Pantos, hit them several times."));
					await dialog.Msg(L("Try that on the Pantos in Levanda Habitat."));
					character.Quests.Start(Mq05);
					character.Inventory.Add(650613, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I will give you a stronger charm, so use it after destroying the Panto Totem."));
				await dialog.Msg(L("Just like you did last time. You got it?"));

				var answer = await dialog.Select(L("Trust him one more time"),
					Option(L("Trust him one more time"), "accept"),
					Option(L("I will not be fooled again."), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Ah, and if a brainwashed Panto injures an attacking Panto, that Panto will be on our side as well."));
					character.Quests.Start(Mq06);
					character.Inventory.Add(650709, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The hive is at Rojus Plateau. Remove it and the Biteregina will come."));
				character.Quests.ReplayQuestTrack(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Try the charm on the Pantos in Levanda Habitat."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Burn the Panto Totem in Valyma Sanctum first, then use the charm."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("The Pantos are not the herbivores we took them for any longer."));
		});

		// Watcher Erra
		//-------------------------------------------------------------------------
		AddNpc(147403, L("Watcher Erra"), "GELE574_ERRA", "f_gele_57_4", 174, -266, -8, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Erra"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Very good. Flowers in the fields should act like flowers in the fields."));
				await dialog.Msg(L("Don't you think so?"));
				character.Quests.Complete(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Good, good."));
				await dialog.Msg(L("Let's see if it can resist waking up."));
				character.Quests.Complete(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You have done what no other could easily do."));
				await dialog.Msg(L("The goddess must be proud of you."));
				character.Quests.Complete(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("I can't do anything since the Seedmias are attacking us from every direction."));
				await dialog.Msg(L("Can you defeat them?"));

				var answer = await dialog.Select(L("Sure, I'll defeat it"),
					Option(L("Sure, I'll defeat it"), "accept"),
					Option(L("Tell him to do it himself"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq02);

				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("I need the sap of Nepenthes, but it's not awake."));
				await dialog.Msg(L("I have no way to collect the sap if it doesn't wake up."));

				var answer = await dialog.Select(L("Yeah, I'll collect them"),
					Option(L("Yeah, I'll collect them"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Let's wake up the Nepenthes at Piene Field by burning it with this fat."));
				await dialog.Msg(L("The fluids will be of no use if they spoil, so we need to extract them while it's alive."));

				var answer = await dialog.Select(L("I'll be back quickly"),
					Option(L("I'll be back quickly"), "accept"),
					Option(L("Impossible"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					character.Inventory.Add(650706, 1, InventoryAddType.PickUp);
				}

				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Seedmias have taken the fields. Drive them out."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The Mallardu carry the fat that will wake the Nepenthes."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The Nepenthes is at Piene Field. Burn it and take the sap."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("Tenet Garden is a garden in name only now."));
		});

		// Follower Alfonsas
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Alfonsas"), "GELE574_ADRIJA", "f_gele_57_4", -612, 559, 13, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Alfonsas"));

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Amazing."));
				await dialog.Msg(L("With this, my brothers at the church can fight without any worries."));
				character.Quests.Complete(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("They're getting swallowed up into them."));
				await dialog.Msg(L("Very effective indeed."));
				character.Quests.Complete(Mq08);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("I would like to say that my faith is like a first-class blade that can cut through these demons,"));
				await dialog.Msg(L("but in reality it isn't that simple. I hope you'll help us some."));

				var answer = await dialog.Select(L("I'll defeat the demons"),
					Option(L("I'll defeat the demons"), "accept"),
					Option(L("I'm sorry, but I can't do it all by myself"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq07);

				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				await dialog.Msg(L("The Demon Summoning Circles are the biggest problem here."));
				await dialog.Msg(L("If we scribble on the circles, do you think the summoning formulas will become tangled?"));

				var answer = await dialog.Select(L("That's a reasonable opinion"),
					Option(L("That's a reasonable opinion"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq08);

				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Defeat whatever demons you see on the way to the Temple Courtyard."));
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("Scribble over the summoning circles and the formulas come apart."));
				return;
			}

			await dialog.Msg(L("The road to the Temple Courtyard is thick with demons."));
		});

		// Follower Algis at the road
		//-------------------------------------------------------------------------
		AddNpc(147390, L("Follower Algis"), "GELE574_ALLGES", "f_gele_57_4", 947, 1189, 68, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("I heard that the plan failed."));
				await dialog.Msg(L("I guess we have to do something before Gesti recovers."));

				var answer = await dialog.Select(L("I will chase it immediately"),
					Option(L("I will chase it immediately"), "accept"),
					Option(L("I'm not ready yet"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq09);

				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("She made for the church. We have to move before she recovers."));
				character.Quests.ReplayQuestTrack(Mq09);
				return;
			}

			await dialog.Msg(L("Gesti went into the church. We are right behind her."));
		});

		// Follower Algis at the cathedral
		//-------------------------------------------------------------------------
		AddNpc(147390, L("Follower Algis"), "GELE574_ARUNE_1", "f_gele_57_4", 1305, 2033, 189, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("This barrier is impossible to open from the outside."));
				await dialog.Msg(L("Fortunately, the entrance to the basement looks secure."));
				character.Quests.Complete(Mq09);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("The 1st floor of the Tenet Church has been sealed by Gesti's powers."));
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("Find the way up to the first floor through the basement!"));
				return;
			}

			await dialog.Msg(L("The church is sealed. The basement is the way in."));
		});

		// Small Beehive
		//-------------------------------------------------------------------------
		AddNpc(400121, L("Small Beehive"), "GELE574_MQ_01", "f_gele_57_4", -1017, 2085, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Small Beehive"));

			if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("You tear the hive down. The buzzing stops - and then something far larger comes crashing through the trees."));
				character.Quests.StartQuestTrack(Mq01);
				return;
			}

			await dialog.Msg(L("A hive that smells of the deep forest."));
		});

		// Nepenthes
		//-------------------------------------------------------------------------
		AddNpc(147454, L("Nepenthes"), "GELE574_MQ_04", "f_gele_57_4", 2314, -722, -43, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Nepenthes"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You set the combustible fat alight. The Nepenthes shudders awake."));
				character.Quests.StartQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("A sleeping Nepenthes, still as stone."));
		});

		// Panto Totem
		//-------------------------------------------------------------------------
		AddNpc(147356, L("Panto Totem"), "GELE574_MQ_06", "f_gele_57_4", -1563, -792, 72, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Panto Totem"));

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("You set the totem alight. As it burns, the charm's hold snaps into place."));
				character.Quests.StartQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("A Panto totem, thick with old bindings."));
		});

		// Demon Summoning Circle
		//-------------------------------------------------------------------------
		AddNpc(147380, L("Demon Summoning Circle"), "GELE574_MQ_08", "f_gele_57_4", 81, 674, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Demon Summoning Circle"));

			if (character.Quests.IsActive(Mq08) && !character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("You scrawl over the summoning circle. The formulas tangle, and the summoned demons are swallowed back in."));
				character.Quests.CompleteObjective(Mq08, "scribbleCircle");
				return;
			}

			await dialog.Msg(L("A demon summoning circle, its sigils still whole."));
		});

		// Hidden trigger
		//-------------------------------------------------------------------------
		// The Panto grounds in Levanda Habitat, where the charm is tried.
		AddQuestTrigger("GELE574_MQ_05_LURE", "f_gele_57_4", -1202, -184, 350, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.CompleteObjective(Mq05, "charmPantos");

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8601: Unwelcome Guest from the Forest
//-----------------------------------------------------------------------------
public class Gele574Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8601);
		SetName(L("Unwelcome Guest from the Forest"));
		SetDescription(L("A Biteregina has built a hive at Rojus Plateau. Remove it and deal with the beast."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Watcher Rikke of Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_MQ_01", "f_gele_57_4", L("Remove the beehive"), L("Remove the beehive and defeat the Biteregina that appears."));
		SetPhase(QuestStatus.Success, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Tell Rikke there will be no more threats to worry about."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE574_MQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killBiteregina", L("Defeat Biteregina"), new KillObjective(1, "boss_BiteRegina_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8602: Irritating Pricks
//-----------------------------------------------------------------------------
public class Gele574Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8602);
		SetName(L("Irritating Pricks"));
		SetDescription(L("The Seedmias are attacking Erra from every direction. Drive them out."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Watcher Erra in Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_ERRA", "f_gele_57_4", L("Defeat Seedmia"), L("Defeat the Seedmias and return to Erra."));
		SetPhase(QuestStatus.Success, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Inform Erra that the Seedmias are dealt with."));

		AddObjective("killSeedmia", L("Defeat Seedmia"), new KillObjective(8, "seedmia"));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 8603: The Immortal Nepenthes (1)
//-----------------------------------------------------------------------------
public class Gele574Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8603);
		SetName(L("The Immortal Nepenthes (1)"));
		SetDescription(L("Erra needs the fat of the Mallardu to wake the sleeping Nepenthes."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Watcher Erra in Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_ERRA", "f_gele_57_4", L("Obtain Mallardu Fat"), L("Defeat the Mallardus and collect their fat."));
		SetPhase(QuestStatus.Success, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Return to Watcher Erra."));

		AddDrop(650705, 0.1f, 47528);
		AddObjective("collectFat", L("Obtain Mallardu Fat"), new CollectItemObjective("GELE574_MQ_03_ITEM", 10));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("GELE574_MQ_03_ITEM"));
	}
}

// 8604: The Immortal Nepenthes (2)
//-----------------------------------------------------------------------------
public class Gele574Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8604);
		SetName(L("The Immortal Nepenthes (2)"));
		SetDescription(L("Wake the Nepenthes at Piene Field with the combustible fat, then take its sap."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Watcher Erra in Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_MQ_04", "f_gele_57_4", L("Extract Spiritually Enchanted Sap from Nepenthes"), L("Wake the Nepenthes at Piene Field and collect its sap."));
		SetPhase(QuestStatus.Success, "GELE574_ERRA", "f_gele_57_4", L("Talk to Watcher Erra"), L("Return to Watcher Erra."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE574_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8603, QuestStatus.Completed));

		AddObjective("extractSap", L("Extract Spiritually Enchanted Sap from Nepenthes"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("R_HAND02_122", 1));
		AddReward(new TakeItemReward("GELE574_MQ_04_01_ITEM"));
		AddReward(new TakeItemReward("GELE574_MQ_04_02_ITEM"));
	}
}

// 8605: The Watcher's Potential (1)
//-----------------------------------------------------------------------------
public class Gele574Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8605);
		SetName(L("The Watcher's Potential (1)"));
		SetDescription(L("Try the elders' charm on the Pantos in Levanda Habitat."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Watcher Rikke of Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_REIKE", "f_gele_57_4", L("Brainwash the Pantos"), L("Use the charm on the Pantos in Levanda Habitat."));
		SetPhase(QuestStatus.Success, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Tell Rikke the brainwashed Pantos still attack the caster."));

		AddObjective("charmPantos", L("Brainwash the Pantos"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("GELE574_MQ_05_ITEM"));
	}
}

// 8606: The Watcher's Potential (2)
//-----------------------------------------------------------------------------
public class Gele574Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8606);
		SetName(L("The Watcher's Potential (2)"));
		SetDescription(L("Burn the Panto Totem at Valyma Sanctum and use the stronger charm."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Watcher Rikke of Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_MQ_06", "f_gele_57_4", L("Control the Pantos and defeat the Large Panto Spearmen"), L("Burn the Panto Totem in Valyma Sanctum and try again."));
		SetPhase(QuestStatus.Success, "GELE574_REIKE", "f_gele_57_4", L("Talk to Watcher Rikke"), L("Tell Rikke the Pantos took orders and helped defeat the Large Panto Spearman."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE574_MQ_06_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8605, QuestStatus.Completed));

		AddObjective("controlPantos", L("Control the Pantos and defeat the Large Panto Spearmen"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("GELE574_MQ_06_ITEM"));
	}
}

// 8607: In the Name of the Goddess!
//-----------------------------------------------------------------------------
public class Gele574Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8607);
		SetName(L("In the Name of the Goddess!"));
		SetDescription(L("Alfonsas cannot hold the road to the Temple Courtyard alone. Thin the demons out."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ADRIJA", "f_gele_57_4", L("Talk to Follower Alfonsas"), L("Follower Alfonsas of Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_ADRIJA", "f_gele_57_4", L("Defeat demons"), L("Defeat the demons along the way to the Temple Courtyard."));
		SetPhase(QuestStatus.Success, "GELE574_ADRIJA", "f_gele_57_4", L("Talk to Follower Alfonsas"), L("Return to Follower Alfonsas."));

		AddObjective("killDemons", L("Defeat demons"), new KillObjective(12, "zombiegirl2_brown", "Colifly_bow"));

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 8608: Obversion
//-----------------------------------------------------------------------------
public class Gele574Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8608);
		SetName(L("Obversion"));
		SetDescription(L("Scribbling over the demon summoning circles tangles their formulas."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ADRIJA", "f_gele_57_4", L("Talk to Follower Alfonsas"), L("Follower Alfonsas of Tenet Garden is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE574_MQ_08", "f_gele_57_4", L("Write over the Demon Summoning Circles"), L("Scribble over the demon summoning circles."));
		SetPhase(QuestStatus.Success, "GELE574_ADRIJA", "f_gele_57_4", L("Talk to Follower Alfonsas"), L("Tell Follower Alfonsas the summoned demons were absorbed back."));

		AddObjective("scribbleCircle", L("Write over the Demon Summoning Circles"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 8609: Grown Apart From Hope
//-----------------------------------------------------------------------------
public class Gele574Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8609);
		SetName(L("Grown Apart From Hope"));
		SetDescription(L("Gesti has gone into the Tenet Church. Follow her with Algis."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE574_ALLGES", "f_gele_57_4", L("Find Follower Algis"), L("Follower Algis is waiting for you at Tenet Garden."));
		SetPhase(QuestStatus.InProgress, "GELE574_ALLGES", "f_gele_57_4", L("Pursue Gesti"), L("Defeat the Chapparition and follow Gesti."));
		SetPhase(QuestStatus.Success, "GELE574_ARUNE_1", "f_gele_57_4", L("Talk to Follower Algis"), L("Ask Follower Algis what to do next."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE574_MQ_09_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8545, QuestStatus.Completed));

		AddObjective("killChapparition", L("Defeat Chapparition"), new KillObjective(1, "boss_Chapparition") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}
