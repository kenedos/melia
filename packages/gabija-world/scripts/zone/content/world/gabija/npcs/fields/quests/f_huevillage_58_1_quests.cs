//--- Melia Script ----------------------------------------------------------
// Veja Ravine Quest NPCs
//--- Description -----------------------------------------------------------
// The Old Man of Andale Village, the Holy Pond, the injured villager and the
// Nugria altar the map's quests run on.
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

public class FHuevillage581QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(18100);
	private readonly static QuestId Mq02 = new QuestId(18110);
	private readonly static QuestId Mq03 = new QuestId(18120);
	private readonly static QuestId Mq04 = new QuestId(18130);
	private readonly static QuestId Sq01 = new QuestId(18150);
	private readonly static QuestId Sq02 = new QuestId(18160);
	private readonly static QuestId Sq03 = new QuestId(18170);
	private readonly static QuestId Mq11 = new QuestId(18190);

	protected override void Load()
	{
		// Old Man of Andale Village
		//-------------------------------------------------------------------------
		AddNpc(147396, L("Old Man of Andale Village"), "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", 200, -1300, 102, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Old Man of Andale Village"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You seem to live up to your reputation."));
				await dialog.Msg(L("The people in our village just cowardly hope that the goddess will take care of everything."));
				character.Quests.Complete(Mq02);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("Aren't you the Revelator?"));
				await dialog.Msg(L("So, what brings you to our village?"));

				var answer = await dialog.Select(L("Goddess Saule? We haven't seen Her in a long time. The portal that leads to Goddess Saule doesn't open. I think it's because the Holy Pond is corrupted."),
					Option(L("I'll purify the pond"), "accept"),
					Option(L("About Goddess Saule"), "explain"),
					Option(L("Think of another way"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Just like there are many demons, the same goes for the goddesses."));
					await dialog.Msg(L("The five goddesses are the only ones who became well known."));
					await dialog.Msg(L("Our village serves the Goddess Saule, who overlooks the sun."));
					await dialog.Msg(L("We used to go see her a lot through the portal, but one day that portal closed."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("The Tanus in Zvelgian Vacant Lot carry Purifying Stones."));
					await dialog.Msg(L("They will be able to purify the Pond."));
					character.ServerMessage(L("Use the cable car to move across to Zvelgian Vacant Lot."));
					return;
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("By the way, did you see a young man from our village?"));

				var answer = await dialog.Select(L("I told him to go check if the portal is working, but I have not heard from him since then."),
					Option(L("I'll look for him"), "accept"),
					Option(L("Cancel"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("Thank you."));
					await dialog.Msg(L("The portal is at Nugria Sanctum."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The Tanus of Zvelgian Vacant Lot carry the Purifying Stones. Bring twelve of them to the pond."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Has the water cleared yet? Come and tell me once it has."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The portal is at Nugria Sanctum. He would have gone by the lower path."));
				return;
			}

			await dialog.Msg(L("God looks after our village, so the water is clear. The air is fresh. And there are no disasters."));
		});

		// Holy Pond
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Holy Pond"), "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", -973.57, 983.30, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Holy Pond"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Twelve stones, and the water takes every one of them without a sound."));
				character.Quests.Complete(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.Select(L("The stones lie on the bed of the pond, waiting to be set working."),
					Option(L("Purify the pond"), "accept"),
					Option(L("Leave it for now"), "leave")
				);

				if (answer == "accept")
				{
					var purified18110 = await character.TimeActions.StartAsync(L("Purifying..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (purified18110 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq02);
					return;
				}
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.Select(L("The corruption in the pond goes deeper than the surface, and something below it is moving."),
					Option(L("Look into the pond"), "accept"),
					Option(L("Step back"), "leave")
				);

				if (answer == "accept")
				{
					var looked18170 = await character.TimeActions.StartAsync(L("Looking it over..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (looked18170 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Sq03);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The water is still clouded. The stones have not finished their work."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("Merregina is still in the water."));
				character.Quests.ReplayQuestTrack(Sq03);
				return;
			}

			await dialog.Msg(L("The pond lies still, and the reeds along its edge stand undisturbed."));
		});

		// Injured Villager
		//-------------------------------------------------------------------------
		AddNpc(147407, L("Injured Villager"), "HUEVILLAGE_58_1_MQ03_NPC", "f_huevillage_58_1", -232, -434, 190, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Injured Villager"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Thank you for saving me."));
				character.Quests.Complete(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("It doesn't work?"));
				await dialog.Msg(L("Well, that's going to be a big problem."));
				await dialog.Msg(L("Can you inform the elderly in Vieta Gorge about this for me?"));
				await dialog.Msg(L("If you veer left and move up from here, you'll quickly arrive there."));
				character.Quests.Complete(Mq04);
				character.ServerMessage(L("Go and find the village elder of Vieta Gorge."));
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("So you're the Revelator the headman spoke about."));

				var answer = await dialog.Select(L("I'm fine, please check the portal up at the sanctum."),
					Option(L("I'll check the sanctum for you"), "accept"),
					Option(L("About the portal"), "explain"),
					Option(L("Worry about the wound"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("That portal is... a fantasy... No. A portal going to Goddess Saule."));
					await dialog.Msg(L("Anyway, opening the portal is very important."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("The portal can be opened from the altar at Nugria Sanctum."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Behind you - they came out of the brush!"));
				character.Quests.ClearQuestTrack(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("My injuries are not that severe, don't worry. Please go on ahead and check the portal at the sanctum."));
				return;
			}

			await dialog.Msg(L("I will move again if I take a little rest."));
			await dialog.Msg(L("But first, the portal..."));
		});

		// Nugria Altar
		//-------------------------------------------------------------------------
		AddNpc(47124, L("Nugria Altar"), "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", -1250, 490, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Nugria Altar"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("You lay both hands on the altar. The ring of script around it stays cold."));
				var checked18130 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (checked18130 != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq04, "checkPortal");
				character.ServerMessage(L("The portal is not working."));
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.Select(L("Something has taken root under the sanctum floor, and the stone above it has begun to split."),
					Option(L("Check the portal at Nugria Sanctum"), "accept"),
					Option(L("Leave the sanctum alone"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Moyabruka is still standing over the altar."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			await dialog.Msg(L("The altar of Nugria Sanctum. The portal above it is dark."));
		});

		// Source of Corruption
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Source of Corruption"), "HUEVILLAGE_58_1_SQ02_NPC", "f_huevillage_58_1", -179.40, 827.41, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Source of Corruption"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("With nothing left feeding it, the growth comes away from the ground in one piece."));
				character.Quests.Complete(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.Select(L("The growth draws strength from every beast around it, and pulling at it now would only tighten its hold."),
					Option(L("Clear the ground around it first"), "accept"),
					Option(L("Leave it standing"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					character.ServerMessage(L("Defeat the monsters around the source of corruption and remove the source!"));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("The growth is still fed. Thin out the beasts of Veidma Uphill first."));
				return;
			}

			await dialog.Msg(L("A knot of blackened growth, sunk into the slope of Veidma Uphill."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The bend of the lower path where the villager is found.
		AddQuestTrigger("HUEVILLAGE_58_1_MQ03_TRIGGER", "f_huevillage_58_1", -232, -434, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
				character.Quests.StartQuestTrack(Mq03);

			await Task.CompletedTask;
		});

		// The approach to Nugria Sanctum the escape from the village ends at.
		AddQuestTrigger("HUEVILLAGE_58_1_MQ11_TRIGGER", "f_huevillage_58_1", -1044.60, 415.31, 350, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq11) && character.Quests.MeetsPrerequisites(Mq11))
				character.Quests.Start(Mq11);

			if (character.Quests.IsActive(Mq11) && !character.Quests.IsCompletable(Mq11))
				character.Quests.StartQuestTrack(Mq11);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 18100: Searching for Goddess Saule
//-----------------------------------------------------------------------------
public class Huevillage581Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18100);
		SetName(L("Searching for Goddess Saule"));
		SetDescription(L("The portal to Goddess Saule is dead. Collect Purifying Stones from the Tanu to cleanse the Holy Pond."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Search the portal in Veja Ravine"), L("As written on the revelation, go through Naslaite Cliff Path and arrive at Veja Ravine."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Collect Purifying Stones from Tanu"), L("The Old Man said you need Purifying Stones of Tanu to activate the portal."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Purify the Holy Pond"), L("Collected all the purifying stones needed. Use it to purify the Holy Pond."));

		AddPrerequisite(new QuestStatusPrerequisite(30031, QuestStatus.Completed));

		AddPityDrop("HUEVILLAGE_58_1_MQ01_ITEM1", 1.0f, 0, 1, "Tanu");

		AddObjective("collectStones", L("Collect Tanu's Purifying Stone"), new CollectItemObjective("HUEVILLAGE_58_1_MQ01_ITEM1", 12));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 18110: Purify the Holy Pond
//-----------------------------------------------------------------------------
public class Huevillage581Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18110);
		SetName(L("Purify the Holy Pond"));
		SetDescription(L("The Purifying Stones are in the water. Set them working and report to the Old Man."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Purify the Holy Pond"), L("Purify the Holy Pond, which should restore the portal."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Purify the Holy Pond"), L("Purify the Holy Pond, which should restore the portal."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Report to the Old Man"), L("Purified the Holy Pond. Return to the Old Man and ask about the portal mentioned in the revelation."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_1_MQ02_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(18100, QuestStatus.Completed));

		AddObjective("purifyPond", L("Purify the Holy Pond"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
		AddReward(new TakeItemReward("HUEVILLAGE_58_1_MQ01_ITEM1"));
	}
}

// 18120: Search for the Missing Villager
//-----------------------------------------------------------------------------
public class Huevillage581Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18120);
		SetName(L("Search for the Missing Villager"));
		SetDescription(L("A villager was sent to the portal and never came back. Find him on the lower path."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ01_NPC", "f_huevillage_58_1", L("Talk to the Old Man"), L("Purified the Holy Pond. Return to the Old Man and ask about the portal mentioned in the revelation."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ03_NPC", "f_huevillage_58_1", L("Find the villager sent by the Old Man"), L("The Old Man says he sent a villager to Nugria Sanctum to see if the portal is working but he has not heard since."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ03_NPC", "f_huevillage_58_1", L("Talk to the Injured Villager"), L("Defeated the monsters that attacked the villager. Talk to the villager."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_1_MQ03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(18110, QuestStatus.Completed));

		AddObjective("killAttackers", L("Defeat the monsters that attacked the villager"), new KillObjective(8, "Tipio", "Tanu") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 18130: Checking the Portal
//-----------------------------------------------------------------------------
public class Huevillage581Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18130);
		SetName(L("Checking the Portal"));
		SetDescription(L("The villager cannot walk that far. Check the altar at Nugria Sanctum in his place."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ03_NPC", "f_huevillage_58_1", L("Talk to the Injured Villager"), L("You've found a villager, but they appear to be wounded. Ask the villager what happened."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", L("Check the altar at Nugria Sanctum"), L("The villager says he was attacked on his way to check on the portal. Go and check if the portal at Nugria Sanctum is open."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ03_NPC", "f_huevillage_58_1", L("Talk to the Injured Villager"), L("Purified the Holy Pond, but the portal at Nugria Sanctum did not open. Return to the Injured Villager and ask what to do."));

		AddPrerequisite(new QuestStatusPrerequisite(18120, QuestStatus.Completed));

		AddObjective("checkPortal", L("Check the altar at Nugria Sanctum"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 18150: Nugria Sanctum's Moyabruka
//-----------------------------------------------------------------------------
public class Huevillage581Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18150);
		SetName(L("Nugria Sanctum's Moyabruka"));
		SetDescription(L("Something has rooted itself under the altar of Nugria Sanctum."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", L("Check the Portal at Nugria Sanctum"), L("Check the Portal at Nugria Sanctum."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", L("Defeat Moyabruka"), L("Moyabruka rose out of the sanctum floor. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", L("Defeat Moyabruka"), L("Moyabruka rose out of the sanctum floor. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_1_SQ01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killMoyabruka", L("Defeat Moyabruka"), new KillObjective(1, "boss_Moyabruka") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Moyabruka is down. The altar stone is clear of the rot that grew over it."));
		character.Quests.Complete(this.QuestId);
	}
}

// 18160: Removing Pollutants in Veidma Uphill
//-----------------------------------------------------------------------------
public class Huevillage581Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18160);
		SetName(L("Removing Pollutants in Veidma Uphill"));
		SetDescription(L("A source of corruption on the Veidma Uphill slope grows stronger with every beast around it."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_SQ02_NPC", "f_huevillage_58_1", L("Check the source of corruption in Veidma Uphill"), L("Something is corrupting the surroundings of Veidma Uphill. Defeat the source."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_SQ02_NPC", "f_huevillage_58_1", L("Removing Pollutants in Veidma Uphill"), L("The source of corruption is becoming stronger with the monsters. Defeat the monsters nearby and remove the source when it is weakened."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_SQ02_NPC", "f_huevillage_58_1", L("Removing Pollutants in Veidma Uphill"), L("The source of corruption is weakened. Remove it."));

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("clearUphill", L("Defeat the monsters around the source of corruption"), new KillObjective(12, "Beetow", "Siaulav_bow"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 18170: Holy Pond's Merregina
//-----------------------------------------------------------------------------
public class Huevillage581Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18170);
		SetName(L("Holy Pond's Merregina"));
		SetDescription(L("The corruption of the Holy Pond has a shape of its own under the water."));
		SetType(QuestType.Sub);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Check out Holy Pond"), L("The corruption in the Holy Pond seems serious. Check out Holy Pond."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Defeat Merregina"), L("Merregina rose out of the Holy Pond. Put it down."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_MQ02_NPC", "f_huevillage_58_1", L("Defeat Merregina"), L("Merregina rose out of the Holy Pond. Put it down."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_1_SQ03_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(16));

		AddObjective("killMerregina", L("Defeat Merregina"), new KillObjective(1, "boss_merregina") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("BRC02_112", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Whatever was nesting under the water is gone. The pond is only a pond again."));
		character.Quests.Complete(this.QuestId);
	}
}

// 18190: Girl in Danger
//-----------------------------------------------------------------------------
public class Huevillage581Mq11Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(18190);
		SetName(L("Girl in Danger"));
		SetDescription(L("The villagers were demons in disguise. Run for the portal at Nugria Sanctum."));
		SetType(QuestType.Main);
		SetLocation("f_huevillage_58_1");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "HUEVILLAGE_58_1_MQ11_TRIGGER", "f_huevillage_58_1", L("Go to the altar of Nugria Sanctum"), L("The villagers suddenly turned into demons and attacked. Run away to Nugria Sanctum in Veja Ravine where the portal is located."));
		SetPhase(QuestStatus.InProgress, "HUEVILLAGE_58_1_MQ11_TRIGGER", "f_huevillage_58_1", L("Go to the altar of Nugria Sanctum"), L("The villagers suddenly turned into demons and attacked. Run away to Nugria Sanctum in Veja Ravine where the portal is located."));
		SetPhase(QuestStatus.Success, "HUEVILLAGE_58_1_PORTAL", "f_huevillage_58_1", L("Go to the altar of Nugria Sanctum"), L("The girl opened the portal. Step through it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "HUEVILLAGE_58_1_MQ11_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20286, QuestStatus.Completed));

		AddObjective("reachSanctum", L("Reach the altar of Nugria Sanctum"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The cutscene's own frame-35 warp is played here instead, so the
		// track can wind down before the map changes.
		character.Quests.Complete(this.QuestId);
		character.Warp("f_huevillage_58_4", 29, 34, -370);
	}
}
