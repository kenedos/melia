//--- Melia Script ----------------------------------------------------------
// Gele Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// The Watchers of Gele Plateau and the totems and beasts their quests run on.
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

public class FGele572QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(17200);
	private readonly static QuestId Mq03 = new QuestId(17220);
	private readonly static QuestId Mq04 = new QuestId(17230);
	private readonly static QuestId Mq05 = new QuestId(17240);
	private readonly static QuestId Mq06 = new QuestId(17250);
	private readonly static QuestId Mq07 = new QuestId(17260);
	private readonly static QuestId Mq08 = new QuestId(17270);
	private readonly static QuestId Mq09 = new QuestId(17280);
	private readonly static QuestId Rp1 = new QuestId(60152);

	protected override void Load()
	{
		// Watcher Basil
		//-------------------------------------------------------------------------
		AddNpc(147405, L("Watcher Basil"), "GELE572_NPC_BASIL", "f_gele_57_2", -138, -433, 16, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Basil"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Thank you for helping out."));
				await dialog.Msg(L("You've reduced my workload quite a bit."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("We might be able to go get more when more young men arrive."));
				await dialog.CompleteQuest(Mq08);
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("I forgot that Mushcaria was still alive."));
				await dialog.Msg(L("Anyway, you did a good job."));
				await dialog.CompleteQuest(Mq09);
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("Please bring me the Mali seeds on the way to Labure Highway."));
				var answer = await dialog.SelectQuestOffer(Mq03, L("They would be a good source of magic for the shaman doll."),
					Option(L("No problem"), "accept"),
					Option(L("About the Pledge"), "explain"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("I heard the first Paladin promised three things in return for saving our family."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				await dialog.Msg(L("We need the rope for ceremony to make the shaman doll."));
				await dialog.Msg(L("It's on the waist of the Panto Shaman in the Cottage."));
				var answer = await dialog.SelectQuestOffer(Mq08, L("Bring it to me."),
					Option(L("I will try"), "accept"),
					Option(L("About the Cursed Doll"), "explain"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Our shaman dolls are different from a Bokor's shaman dolls."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("You may use a bit of force if words aren't enough. We can't grow any further from each other anymore."));
					character.Quests.Start(Mq08);
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("The shaman doll is to be filled with Mushcaria's Enchanted Mane."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("The Mushcaria should be in the Tustinti Plateau. Can you get its mane for me?"),
					Option(L("I'll get it"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq09);
					character.LookAround();
				}

				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The Mali carry the seeds. Look for them along Labure Highway."));
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("The ritual rope hangs at the Panto Shaman's waist. Take it by force if you must."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("The Mushcaria keeps its mane to itself. It will not give it up willingly."));
				return;
			}

			await dialog.Msg(L("The Pledge binds us still, though the Pantos have forgotten it."));
		});

		// Watcher Molly
		//-------------------------------------------------------------------------
		AddNpc(147404, L("Watcher Molly"), "GELE572_NPC_MORI", "f_gele_57_2", 193, -429, 14, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Molly"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("The totems are broken? Then the shaman dolls did their part."));
				await dialog.CompleteQuest(Mq04);
				character.Quests.Start(Mq05);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("It's been a bit frightening ever since Simorph appeared around here."));
				await dialog.Msg(L("Witnessing the Pantos becoming this corrupted is no different."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("Every trace of the corruption is gone. Labure Highway can breathe again."));
				await dialog.CompleteQuest(Mq06);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("It would have been a disaster if the Wild Carnivore had legs."));
				await dialog.Msg(L("I'm glad it was killed beforehand."));
				await dialog.CompleteQuest(Mq07);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Rp1) && character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Hm... I see."));
				await dialog.Msg(L("It's not much yet, but we might be able to fix the shaman doll a little."));
				var told60152 = await character.TimeActions.StartAsync(L("Passing on the information..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (told60152 != TimeActionResult.Completed)
					return;

				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("I can't stand seeing the Panto totems pressed with evil energy."));
				var answer = await dialog.SelectQuestOffer(Mq04, L("I'm going to break it with the shaman doll. Want to give it a try?"),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("You can summon the shaman doll with this summon scroll."));
					character.Quests.Start(Mq04);
					character.Inventory.Add(650703, 1, InventoryAddType.PickUp);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("Won't you help me purify Labure Highway?"));
				var answer = await dialog.SelectQuestOffer(Mq06, L("The shaman doll finds the evil force but we have to purify it ourselves."),
					Option(L("Alright"), "accept"),
					Option(L("About the corrupted land with evil energy"), "explain"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The demons could have used their hands to make it easier to fight."));
					await dialog.Msg(L("Maybe they are after the Holy Relic."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("If possible, I want you to defeat the Wild Carnivore as well. It has become corrupted with demonic energy."));
					character.Quests.Start(Mq06);
					character.Inventory.Add(650593, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				await dialog.Msg(L("If we're going to use a shaman doll, we need more information."));
				var answer = await dialog.SelectQuestOffer(Rp1, L("There used to be nothing but Pantos here, but now I see other monsters hanging around."),
					Option(L("I will try"), "accept"),
					Option(L("I'll find the other people"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Rp1);

				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Take the summon scroll to the Panto totem and break it."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("With the totems broken, Simorph will come. Be ready."));
				character.Quests.ClearQuestTrack(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The shaman doll marks the corrupted land. Purify what it finds."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("The Wild Carnivore is out at Pasiulyma Field. It must not spread its corruption further."));
				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("Spion Archers, Leaflies and Mali. That is what we need to know about."));
				return;
			}

			await dialog.Msg(L("Gele Plateau is not what it was. The Pantos know it too."));
		});

		// Panto Totem
		//-------------------------------------------------------------------------
		AddConditionalNpc(153068, L("Panto Totem"), "GELE572_MQ_05", "f_gele_57_2", 975, -1131, 102, IsTotemShown, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Panto Totem"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				var broken = await character.TimeActions.StartAsync(L("Summoning the shaman doll..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (broken != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("You set the shaman doll against the totem. It claws at the evil energy until the totem splits apart."));
				character.Quests.CompleteObjective(Mq04, "destroyTotems");
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("The shattered totem spills its corruption across the ground. Something is coming."));
				character.Quests.Start(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				var searched = await character.TimeActions.StartAsync(L("Searching..."), L("Cancel"), "SITGROPE2_LOOP", TimeSpan.FromSeconds(2));

				if (searched != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The corruption stirs. Simorph is coming."));
				character.Quests.ReplayQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("A totem pressed with evil energy. It should not be standing."));
		});

		// Wild Carnivore
		//-------------------------------------------------------------------------
		AddConditionalNpc(147450, L("Wild Carnivore"), "GELE572_MQ_07", "f_gele_57_2", 1014, 1678, 0, c => c.Quests.HasCompleted(Mq06) && !c.Quests.IsCompletable(Mq07) && !c.Quests.HasCompleted(Mq07), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wild Carnivore"));

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				var provokedIt = await character.TimeActions.StartAsync(L("Provoking it..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (provokedIt != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The corrupted beast turns on you. There is nothing left of what it was."));
				character.Quests.ReplayQuestTrack(Mq07);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("The corrupted beast turns on you. There is nothing left of what it was."));
				character.Quests.Start(Mq07);
				return;
			}

			await dialog.Msg(L("A beast swollen with demonic energy. It has to be put down."));
		});

		// Mushcaria
		//-------------------------------------------------------------------------
		AddConditionalNpc(147462, L("Mushcaria"), "GELE572_MQ_09", "f_gele_57_2", -1136, 417, 19, c => c.Quests.IsActive(Mq09) && !c.Quests.IsCompletable(Mq09), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Mushcaria"));

			if (character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09))
			{
				var proddedIt = await character.TimeActions.StartAsync(L("Prodding it..."), L("Cancel"), "SITGROPE_LOOP", TimeSpan.FromSeconds(3));

				if (proddedIt != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The Mushcaria rears up, mane bristling with spirit energy."));
				character.Quests.StartQuestTrack(Mq09);
				return;
			}

			await dialog.Msg(L("A great shaggy beast. Its mane carries a power the Watchers want."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The approach to the Paladin Master.
		AddQuestTrigger("GELE572_MQ_01", "f_gele_57_2", -79, -871, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
				character.Quests.Start(Mq01);
			else if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
				character.Quests.ReplayQuestTrack(Mq01);

			await Task.CompletedTask;
		});

		// The corrupted land on Labure Highway.
		AddQuestTrigger("GELE572_MQ_06", "f_gele_57_2", 514, -136, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
				character.Quests.CompleteObjective(Mq06, "purifyLand");

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether the Panto Totem stands for the given character, while
	/// the shaman doll is sent at it and until Simorph answers.
	/// </summary>
	private static bool IsTotemShown(Character character)
	{
		if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			return true;

		if (character.Quests.HasCompleted(Mq04) && !character.Quests.Has(Mq05))
			return true;

		return character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 17200: Imminent Invasion
//-----------------------------------------------------------------------------
public class Gele572Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17200);
		SetName(L("Imminent Invasion"));
		SetDescription(L("Knight Commander Uska sends you to meet the Paladin Master at Gele Plateau."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_MQ_01", "f_gele_57_2", L("Meet the Paladin Master"), L("Knight Commander Uska thinks the high gardens in the revelation refers to an area in Gele Plateau."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_01", "f_gele_57_2", L("Meet the Paladin Master"), L("Meet the Paladin Master at Gele Plateau."));
		SetPhase(QuestStatus.Success, "GELE572_MQ_01", "f_gele_57_2", L("Meet the Paladin Master"), L("The Paladin Master has been waiting for you."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE572_MQ_01_TRACK", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(50006, QuestStatus.Completed));

		AddObjective("meetMaster", L("Meet the Paladin Master"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The meeting itself is the quest; there is no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}

// 17220: Natural Seeds
//-----------------------------------------------------------------------------
public class Gele572Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17220);
		SetName(L("Natural Seeds"));
		SetDescription(L("The shaman dolls need Mali seeds, and the Mali carry them along Labure Highway."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Watcher Basil in Gele Plateau is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE572_NPC_BASIL", "f_gele_57_2", L("Collect Mali Seeds"), L("Collect Mali Seeds from the Mali along Labure Highway."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Gathered enough Mali Seeds. Return to Watcher Basil."));

		AddPityDrop("GELE572_MQ_08_ITEM", 0.6f, 4, 1, "Mally");

		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("collectSeeds", L("Defeat Mali and obtain Mali Seeds"), new CollectItemObjective("GELE572_MQ_08_ITEM", 5));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE572_MQ_08_ITEM"));
	}
}

// 17230: Unsettled Totems (1)
//-----------------------------------------------------------------------------
public class Gele572Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17230);
		SetName(L("Unsettled Totems (1)"));
		SetDescription(L("Molly wants the Panto totems broken with a shaman doll summon scroll."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Watcher Molly in Gele Plateau is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_05", "f_gele_57_2", L("Destroy Panto Totems with Shaman Dolls"), L("Summon a shaman doll and guide it to the Panto Totems."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("The totems are broken. Tell Molly about it."));

		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("destroyTotems", L("Destroy Panto Totems with Shaman Dolls"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 17240: Unsettled Totems (2)
//-----------------------------------------------------------------------------
public class Gele572Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17240);
		SetName(L("Unsettled Totems (2)"));
		SetDescription(L("Simorph has come for the broken totems. Defeat it."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_MQ_05", "f_gele_57_2", L("Defeat Simorph"), L("Defeat Simorph with the shaman dolls."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_05", "f_gele_57_2", L("Defeat Simorph at Valio Mountain Cabin Hill"), L("Search for Simorph and defeat it."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Tell Molly that Simorph has been defeated."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE572_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17230, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("killSimorph", L("Defeat Simorph"), new KillObjective(1, "boss_simorph"));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE572_MQ_DOLL_01"));
	}
}

// 17250: Purifying Doll (1)
//-----------------------------------------------------------------------------
public class Gele572Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17250);
		SetName(L("Purifying Doll (1)"));
		SetDescription(L("A shaman doll will find the demon-corrupted land on Labure Highway. Purify it."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Watcher Molly in Gele Plateau is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_06", "f_gele_57_2", L("Purify the demon corrupted land"), L("Bring the shaman doll to the corrupted land on Labure Highway and purify it."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Tell Molly the corrupted land is purified."));

		AddPrerequisite(new QuestStatusPrerequisite(17240, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("purifyLand", L("Purify the demon corrupted land"), new ManualObjective());

		AddReward(new ItemReward("expCard2", 2));
	}
}

// 17260: Purifying Doll (2)
//-----------------------------------------------------------------------------
public class Gele572Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17260);
		SetName(L("Purifying Doll (2)"));
		SetDescription(L("The Wild Carnivore is soaked in demonic energy. Put it down."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_MQ_07", "f_gele_57_2", L("Defeat Wild Carnivore"), L("Defeat the Wild Carnivore tainted by demon corruption."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_07", "f_gele_57_2", L("Defeat Wild Carnivore in Pasiulyma Field"), L("Defeat the Wild Carnivore tainted by demon corruption."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Tell Molly the Wild Carnivore is dead."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE572_MQ_07_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17250, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("killCarnivore", L("Defeat Wild Carnivore"), new KillObjective(1, "boss_Carnivore"));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new SelectItemReward("HAND02_160", "HAND02_161", "HAND02_162"));
		AddReward(new TakeItemReward("GELE572_MQ_DOLL"));
	}
}

// 17270: Tie Tightly
//-----------------------------------------------------------------------------
public class Gele572Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17270);
		SetName(L("Tie Tightly"));
		SetDescription(L("The ritual rope hangs at the waist of the Panto Shaman. Take it."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Watcher Basil in Gele Plateau is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE572_NPC_BASIL", "f_gele_57_2", L("Collect ritual rope"), L("Collect the ritual rope from the Panto Wizards."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Give the ritual rope to Watcher Basil."));

		AddPityDrop("GELE572_MQ_03_ITEM", 1.0f, 0, 1, "Npanto_staff");

		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("collectRope", L("Obtain ritual rope by defeating Panto Wizards"), new CollectItemObjective("GELE572_MQ_03_ITEM", 7));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("GELE572_MQ_03_ITEM"));
	}
}

// 17280: Two Birds with One Stone
//-----------------------------------------------------------------------------
public class Gele572Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17280);
		SetName(L("Two Birds with One Stone"));
		SetDescription(L("Hunt the Mushcaria at Tustinti Plateau and take its Enchanted Mane."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Watcher Basil in Gele Plateau is waiting for someone's help."));
		SetPhase(QuestStatus.InProgress, "GELE572_MQ_09", "f_gele_57_2", L("Collect Mushcaria's Enchanted Mane"), L("Hunt the Mushcaria at Tustinti Plateau and collect its Enchanted Mane."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_BASIL", "f_gele_57_2", L("Talk to Watcher Basil"), L("Give the Enchanted Mane to Watcher Basil."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE572_MQ_09_TRACK", 6000, autoStart: false, partyPlay: true);

		AddPityDrop("GELE572_MQ_05_ITEM", 1.0f, 0, 1, "boss_Mushcaria_Q2");

		AddPrerequisite(new LevelPrerequisite(19));

		AddObjective("collectMane", L("Defeat Mushcaria and get Enchanted Mane"), new CollectItemObjective("GELE572_MQ_05_ITEM", 1));

		AddReward(new ItemReward("expCard2", 3));
		AddReward(new ItemReward("Hat_628016", 1));
		AddReward(new SelectItemReward("FOOT02_160", "FOOT02_161", "FOOT02_162"));
		AddReward(new TakeItemReward("GELE572_MQ_05_ITEM"));
	}
}

// 60152: Improvements
//-----------------------------------------------------------------------------
public class Gele572Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60152);
		SetName(L("Improvements"));
		SetDescription(L("Molly needs to know what monsters now roam Gele Plateau before she uses the shaman dolls."));
		SetType(QuestType.Repeat);
		SetLocation("f_gele_57_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE572_NPC_MORI", "f_gele_57_2", L("Talk to Watcher Molly"), L("Watcher Molly is waiting for someone's help at Gele Plateau."));
		SetPhase(QuestStatus.InProgress, "GELE572_NPC_MORI", "f_gele_57_2", L("Dealing with Nearby Monsters"), L("Defeat Spion Archers, Leaflies and Mali to give Molly advice."));
		SetPhase(QuestStatus.Success, "GELE572_NPC_MORI", "f_gele_57_2", L("Report to Watcher Molly"), L("Report what you have learned about the monsters."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killLeafly", L("Defeat Leafly"), new KillObjective(3, "Leafly"));
		AddObjective("killSpion", L("Defeat Spion Archer"), new KillObjective(4, "Spion_bow"));
		AddObjective("killMally", L("Defeat Mali"), new KillObjective(2, "Mally"));

		AddReward(new ItemReward("expCard2", 1));
	}
}
