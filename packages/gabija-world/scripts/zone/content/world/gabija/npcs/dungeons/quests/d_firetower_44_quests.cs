//--- Melia Script ----------------------------------------------------------
// Mage Tower 4F Quest NPCs
//--- Description -----------------------------------------------------------
// The Jewel of Prominence going unstable after the third floor, and the
// transmuter who stayed behind when the rest of the magicians left.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DFiretower44QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8488);
	private readonly static QuestId Mq02 = new QuestId(8489);
	private readonly static QuestId Mq03 = new QuestId(8490);
	private readonly static QuestId Mq04 = new QuestId(8491);
	private readonly static QuestId Mq05 = new QuestId(8492);
	private readonly static QuestId GoddessGabija = new QuestId(8498);
	private readonly static QuestId Sq01 = new QuestId(17017);
	private readonly static QuestId Sq02 = new QuestId(17018);
	private readonly static QuestId Sq03 = new QuestId(17019);
	private readonly static QuestId Sq04 = new QuestId(17020);
	private readonly static QuestId Sq05 = new QuestId(17021);
	private readonly static QuestId Hq01 = new QuestId(19051);

	private const int DrakeBadgeRecipeItemId = 911010;

	private readonly static double[,] FlameCrystalSpots =
	{
		{ -391.83, -617.74 }, { -554.45, -736.58 }, { -495.42, -1278.58 },
		{ -418.77, -1561.87 }, { -267.79, -1334.42 },
	};

	protected override void Load()
	{
		// Grita, at the fourth floor landing
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FTOWER44_GRITA_01", "d_firetower_44", 527.48, -1362.51, 90, c => !c.Quests.Has(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("The Jewel of Prominence is acting a bit strange. Something must have gone wrong during the explosion that killed Antares."),
					Option(L("I agree with Grita"), "accept"),
					Option(L("It will be ok"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("What was that vibration? Could it be... Watch the jewel as you fight, and tell me what it does."));
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("The goddess is one floor above us. The jewel has to hold together until then."));
		});

		// Grita, once she is walking the floor with you
		//-------------------------------------------------------------------------
		// The client has her follow the player; the port stands her at the
		// floor's warning sign instead.
		AddConditionalNpc(147449, L("Grita"), "FTOWER44_G_AI", "d_firetower_44", -568.20, -367.18, 90, c => c.Quests.Has(Mq01) && !c.Quests.Has(Mq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Did you just see that monster explode? That's definitely strange."));
				await dialog.Msg(L("It's as if the power of its essence is out of control."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("When the demons' attacks started becoming serious, the goddess prepared this in advance."));
				await dialog.Msg(L("Fortunately, the device is working alright."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("The Jewel of Prominence is now alright. I am... relieved."));
				await dialog.Msg(L("It's the vibration again! Antares... Now I know. When he exploded, the magic of the tower was disrupted."));
				await dialog.CompleteQuest(Mq03);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				var answer = await dialog.SelectQuestOffer(Mq02, L("If it stays like this, the Jewel of Prominence may break. There must be a stabilizing device somewhere here. Let's go."),
					Option(L("I'll find the Magic Stabilizing Device"), "accept"),
					Option(L("Let's just go up"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("That vibration from a while ago keeps distracting me. I hope it's nothing."));
				}
				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.SelectQuestOffer(Mq03, L("The Jewel of Prominence has stopped behaving erratically. Still, we will need Flame Crystals from the tower's Large Central Brazier in order to completely stabilize it."),
					Option(L("I'll collect the Flame Crystals"), "accept"),
					Option(L("It will be enough this way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("We're almost at the final floor so we just have to press forward a little more. The goddess must be enduring more than we are."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("What is happening to the Jewel of Prominence? It is very unstable."));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Magic Stabilizing Device is at the center of this floor."));
				character.Quests.ClearQuestTrack(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Five Flame Crystals from the Large Central Brazier. That should settle it for good."));
				return;
			}

			await dialog.Msg(L("The magic of the tower has not run true since the third floor."));
		});

		// The Magic Stabilizing Device
		//-------------------------------------------------------------------------
		AddNpc(147372, L("Magic Stabilizing Device"), "FTOWER44_MQ_02", "d_firetower_44", 158.01, 113.65, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Stabilizing Device"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				var worked = await character.TimeActions.StartAsync(L("Working the device..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (worked != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The device takes the jewel's excess, and the noise of it carries."));
				character.Quests.StartQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("A device the goddess set up for the day the tower's magic ran wild."));
		});

		// The Flame Crystals of the Large Central Brazier
		//-------------------------------------------------------------------------
		for (var i = 0; i < FlameCrystalSpots.GetLength(0); i++)
		{
			var uniqueName = "FTOWER44_MQ_03_" + (i + 1);
			AddNpc(151001, L("Flame Crystal"), uniqueName, "d_firetower_44", FlameCrystalSpots[i, 0], FlameCrystalSpots[i, 1], 90, this.TakeFlameCrystal);
		}

		// The Magic Control Circle
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Magic Control Circle"), "FTOWER44_MQ_04_NPC", "d_firetower_44", -1603, 614, 90, c => !c.Quests.Has(GoddessGabija), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Magic Control Circle"));

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var checkedCircle = await character.TimeActions.StartAsync(L("Checking the magic circle..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (checkedCircle != TimeActionResult.Completed)
					return;

				character.Quests.Start(Mq04);
				character.ServerMessage(L("Grita sets to work on the circle, and the floor answers at once."));
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Grita is still working the circle. Keep the Miniverns off her."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("The circle the tower's magic is steered from."));
		});

		// Grita, staying behind at the control circle
		//-------------------------------------------------------------------------
		AddConditionalNpc(147449, L("Grita"), "FTOWER44_GRITA_REMAIN", "d_firetower_44", -1596.22, 587.60, 180, c => c.Quests.Has(Mq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Grita"));
			dialog.SetPortrait("Dlg_port_Grita");

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("I can do this alone. Until Gabija regains her powers again, I will maintain the tower here."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				var answer = await dialog.SelectQuestOffer(Mq05, L("Okay, go to the fifth floor. Please pass the Jewel of Prominence to Gabija!"),
					Option(L("I will go to the goddess"), "accept"),
					Option(L("Tell her it's too dangerous"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05);
					await dialog.Msg(L("This cannot be done without me. I will somehow manage to hold the tower up."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("Keep them off me a little longer."));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The stairs up are at the west end. Something is standing across them."));
				character.Quests.ClearQuestTrack(Mq05);
				return;
			}

			await dialog.Msg(L("Go on up. I will hold the tower from here."));
		});

		// Furry Odd, in the resting area
		//-------------------------------------------------------------------------
		AddConditionalNpc(20145, L("Furry Odd"), "FTOWER44_SQ_01", "d_firetower_44", 1262, -184, 90, c => !c.Quests.Has(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Furry Odd"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("Thanks. You are so reliable."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				await dialog.Msg(L("Pleased to meet you. I am Furry Odd. I am a magician specialized in object variation."));

				var answer = await dialog.SelectQuestOffer(Sq01, L("I am out of Variation Mediators, and I cannot hold this hall without them."),
					Option(L("I'll find the Variation Mediators"), "accept"),
					Option(L("That's tough"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq01);
					await dialog.Msg(L("The magicians that ran away? What can we do. For them, their lives are more important than the Goddess Gabija."));
				}
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("We have enough Mediators now so we can go defeat the remaining monsters. Do you want to come along?"),
					Option(L("I'll collect it, so go ahead"), "accept"),
					Option(L("I can't because I have other things to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("Let's meet at the 1st Machinery Room. Ah, it would be great if you could get the Hardened Black Crystal from monsters on your way."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Ten Mediators. Anything on this floor will have one on it somewhere."));
				return;
			}

			await dialog.Msg(L("Object variation is a quiet art, and this is not a quiet place."));
		});

		// Furry Odd, at the 1st Machinery Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(20145, L("Furry Odd"), "FTOWER44_SQ_02", "d_firetower_44", 1896, 253, 90, c => c.Quests.Has(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Furry Odd"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("You are so great. I hope our magicians can be just as good as you."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thank you so much for helping me. It's so good that you are here."));
				await dialog.Msg(L("May the goddess bless you!"));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (character.Quests.IsActive(Hq01) && character.Quests.IsCompletable(Hq01))
			{
				await dialog.Msg(L("Well made. You will definitely need it sometime in the future."));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				await dialog.Msg(L("There's a problem. I think I saw a Yonazolem inside the Machinery Room... I can't handle it by myself."));

				var answer = await dialog.SelectQuestOffer(Sq03, L("I will stop the monsters at the entrance if you go in after it."),
					Option(L("Sure, I'll defeat it"), "accept"),
					Option(L("Let's just run away"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("I am not strong enough to face Yonazolem. It's fortunate that you are here."));
				}
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				await dialog.Msg(L("Isn't that Drake Horn? You're going to sell all of them?"));

				var answer = await dialog.SelectQuestOffer(Hq01, L("A horn like that is worth a great deal more once it has been made into something."),
					Option(L("I'll try making it"), "accept"),
					Option(L("I don't need it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq01);
					character.Inventory.Add(DrakeBadgeRecipeItemId, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Crafting is only possible during rest mode. You know this already, right?"));
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("Ten Hardened Black Crystals. Take them off whatever you meet on the way."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("It is still in there. I will hold the entrance."));
				character.Quests.ClearQuestTrack(Sq03);
				return;
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("Sit down somewhere safe and work the recipe. It will not take long."));
				return;
			}

			await dialog.Msg(L("The Machinery Room is right behind me, and I would rather it stayed shut."));
		});

		// The sealed stone by the Magic Stabilizing Device
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER44_SQ_04", "d_firetower_44", 32, 310, 90, c => !c.Quests.HasCompleted(Sq04), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("Now, I can be released."));
				await dialog.CompleteQuest(Sq04);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("I can't endure anymore. Please save me from these restraints. I will be released when you defeat the watchers nearby."),
					Option(L("Defeat the surrounding monsters"), "accept"),
					Option(L("Ignore it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("The time to retrieve my body and spirit is near."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("The watchers are all around this device."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// The sealed stone in the 2nd Machinery Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151050, L("Sealed Stone"), "FTOWER44_SQ_05", "d_firetower_44", -453.08, -1449.62, 90, c => !c.Quests.HasCompleted(Sq05), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sealed Stone"));

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("I promise. That there will be something in the future."));
				await dialog.CompleteQuest(Sq05);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("I can't stand it anymore. Please destroy the monitors that imprison me."),
					Option(L("I'll defeat them"), "accept"),
					Option(L("Ignore"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq05);
					await dialog.Msg(L("So I guess I can be released now."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("Five of them. That is all it takes."));
				return;
			}

			await dialog.Msg(L("A stone with a voice in it, and a seal written over the voice."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The Machinery Room entrance Furry Odd holds.
		AddQuestTrigger("FTOWER44_SQ_03", "d_firetower_44", 1780, 507, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03))
				character.Quests.StartQuestTrack(Sq03);

			await Task.CompletedTask;
		});

		// The stairs to the fifth floor.
		AddQuestTrigger("FTOWER44_MQ_05_TRIGGER", "d_firetower_44", -2194.91, 62.19, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Takes one of the Flame Crystals out of the Large Central Brazier.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task TakeFlameCrystal(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Flame Crystal"));

		if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
		{
			var taken = await character.TimeActions.StartAsync(L("Taking the crystal..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

			if (taken != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.FTOWER44_MQ_03_ITEM, 1, InventoryAddType.PickUp);

			await dialog.Msg(L("The crystal comes out of the brazier still warm."));
			return;
		}

		await dialog.Msg(L("A crystal the Large Central Brazier has grown around itself."));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8488: Mage Tower 4th Floor (1)
//-----------------------------------------------------------------------------
public class Ftower44Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8488);
		SetName(L("Mage Tower 4th Floor (1)"));
		SetDescription(L("The Jewel of Prominence has not been right since Antares went up in the explosion."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_GRITA_01", "d_firetower_44", L("Talk to Grita at Mage Tower 4F"), L("Grita is waiting at the fourth floor of the Mage Tower. Go to Grita."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_G_AI", "d_firetower_44", L("Defeat monsters and watch the state of the Jewel of Prominence"), L("The Jewel of Prominence's status became abnormal since the Antares incident on the 3rd floor. Check its status as you defeat the monsters nearby."));
		SetPhase(QuestStatus.Success, "FTOWER44_G_AI", "d_firetower_44", L("Talk to Grita"), L("You felt the power of flames as you defeated the monsters. Talk to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8499, QuestStatus.Completed));

		AddObjective("watchTheJewel", L("Defeat monsters and watch the state of the Jewel of Prominence"), new KillObjective(12, "new_desmodus_black", "wizards_marmotte", "flask", "minivern"));

		AddReward(new ItemReward("expCard7", 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		base.OnComplete(character, quest);

		character.StopBuff(BuffId.FTOWER44_MQ_01);
	}
}

// 8489: Mage Tower 4th Floor (2)
//-----------------------------------------------------------------------------
public class Ftower44Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8489);
		SetName(L("Mage Tower 4th Floor (2)"));
		SetDescription(L("The goddess left a stabilizer on this floor for the day the tower's magic ran wild."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_G_AI", "d_firetower_44", L("Talk to Grita"), L("The Jewel of Prominence's status is unusual. Talk to Grita again."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_MQ_02", "d_firetower_44", L("Activate the Magic Stabilizer"), L("Grita told you that the energy of the Jewel is unstable, so you need to go through a process to stabilize it. Activate the stabilizer and defeat the monsters to stabilize the Jewel."));
		SetPhase(QuestStatus.Success, "FTOWER44_G_AI", "d_firetower_44", L("Talk to Grita"), L("You successfully stabilized the energy of the Jewel. Talk to Grita."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER44_MQ_02_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8488, QuestStatus.Completed));

		AddObjective("killMiniverns", L("Defeat the monsters that rushed in after hearing the sound"), new KillObjective(20, "minivern") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8490: Mage Tower 4th Floor (3)
//-----------------------------------------------------------------------------
public class Ftower44Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8490);
		SetName(L("Mage Tower 4th Floor (3)"));
		SetDescription(L("Flame Crystals from the Large Central Brazier will settle the jewel for good."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_G_AI", "d_firetower_44", L("Talk to Grita"), L("You stabilized the Jewel of Prominence using the stabilizer, but the Jewel is still emitting an unusual aura. Ask Grita what to do about it."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_MQ_03_1", "d_firetower_44", L("Collect Flame Crystals"), L("Grita told you that in order to stabilize the Jewel fully, you need to get Flame Crystals. Collect the Crystals from the Large Central Brazier."));
		SetPhase(QuestStatus.Success, "FTOWER44_G_AI", "d_firetower_44", L("Hand over the Flame Crystals to Grita"), L("You collected enough Flame Crystals. Hand them over to Grita."));

		AddPrerequisite(new QuestStatusPrerequisite(8489, QuestStatus.Completed));

		AddObjective("collectCrystals", L("Collect Flame Crystals"), new CollectItemObjective("FTOWER44_MQ_03_ITEM", 5));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new ItemReward("FTOWER_FIRE_ESSENCE_2", 1));
		AddReward(new TakeItemReward("FTOWER44_MQ_03_ITEM"));
		AddReward(new TakeItemReward("FTOWER_FIRE_ESSENCE_3"));
	}
}

// 8491: Mage Tower 4th Floor (4)
//-----------------------------------------------------------------------------
public class Ftower44Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8491);
		SetName(L("Mage Tower 4th Floor (4)"));
		SetDescription(L("Grita works the control circle to settle the tower, and the floor comes for her while she does."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_MQ_04_NPC", "d_firetower_44", L("Find the Magic Control Room"), L("The Jewel of Prominence is stabilized but the explosion at the 3rd Floor has made the Mage Tower unstable. Go to the Magic Control Room to control the disrupted magic of the Tower."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_MQ_04_NPC", "d_firetower_44", L("Protect Grita while she is stabilizing the magic"), L("Grita said we have to go to the Magic Control Room. Protect Grita while she stabilizes the magic."));
		SetPhase(QuestStatus.Success, "FTOWER44_GRITA_REMAIN", "d_firetower_44", L("Talk to Grita"), L("You defeated all the monsters, but the Tower is still shaking. Ask Grita what to do next."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER44_MQ_04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8490, QuestStatus.Completed));

		AddObjective("killMiniverns", L("Defeat Minivern"), new KillObjective(5, "minivern") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 8492: Mage Tower 4th Floor (5)
//-----------------------------------------------------------------------------
public class Ftower44Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8492);
		SetName(L("Mage Tower 4th Floor (5)"));
		SetDescription(L("The stairs to the fifth floor are held by something that was left to hold them."));
		SetType(QuestType.Main);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_GRITA_REMAIN", "d_firetower_44", L("Talk to Grita"), L("Defeated all the monsters that threatened Grita. Ask Grita what to do next."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_MQ_05_TRIGGER", "d_firetower_44", L("Go to Mage Tower 5F"), L("Grita told you to give the Jewel of Prominence to the goddess while she takes care of the tower. Go to the 5th floor of the Mage Tower where the goddess is located."));
		SetPhase(QuestStatus.Success, "FTOWER44_MQ_05_TRIGGER", "d_firetower_44", L("Go to Mage Tower 5F"), L("Grita told you to give the Jewel of Prominence to the goddess while she takes care of the tower. Go to the 5th floor of the Mage Tower where the goddess is located."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER44_MQ_05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8491, QuestStatus.Completed));

		AddObjective("killGrinender", L("Defeat Grinender that is blocking the way to the 5th Floor"), new KillObjective(1, "boss_Grinender") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
		AddReward(new ItemReward("TreasureboxKey3", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The stairs to the fifth floor are clear."));
		character.Quests.Complete(this.QuestId);
	}
}

// 17017: Transmuter Furry Odd (1)
//-----------------------------------------------------------------------------
public class Ftower44Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17017);
		SetName(L("Transmuter Furry Odd (1)"));
		SetDescription(L("A transmuter in the resting area has run out of the mediators her work needs."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_01", "d_firetower_44", L("Talk to Transmuter Furry Odd"), L("There is a lady magician in the resting area. Talk to her."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_01", "d_firetower_44", L("Defeat the monsters nearby and get Variation Mediators"), L("Furry Odd needs more Variation Mediators to defeat the monsters effectively. Get Variation Mediators from monsters nearby."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_01", "d_firetower_44", L("Give the Variation Mediators to Furry Odd"), L("Collected all the Variation Mediators needed. Give it to Furry Odd."));

		AddPrerequisite(new LevelPrerequisite(113));

		AddPityDrop("FTOWER44_SQ_01_01", 0.5f, 4, 1, "new_desmodus_black", "wizards_marmotte", "flask", "minivern");

		AddObjective("collectMediators", L("Defeat the monsters nearby and get Variation Mediators"), new CollectItemObjective("FTOWER44_SQ_01_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER44_SQ_01_01"));
	}
}

// 17018: Transmuter Furry Odd (2)
//-----------------------------------------------------------------------------
public class Ftower44Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17018);
		SetName(L("Transmuter Furry Odd (2)"));
		SetDescription(L("Furry Odd moves on to the 1st Machinery Room and wants Hardened Black Crystals on the way."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_01", "d_firetower_44", L("Talk to Transmuter Furry Odd"), L("Furry Odd seems to have more favors to ask. Talk to her again."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_02", "d_firetower_44", L("Defeat monsters and get Hardened Black Crystal"), L("Furry Odd told you to meet at the 1st Machinery Room and asked you to bring Hardened Black Crystals. Obtain the Hardened Black Crystals from the monsters nearby."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_02", "d_firetower_44", L("Meet Furry Odd in front of the 1st Machinery Room"), L("You have obtained the Black Crystals. Meet Furry Odd at the 1st Machinery Room."));

		AddPrerequisite(new QuestStatusPrerequisite(17017, QuestStatus.Completed));

		AddPityDrop("FTOWER44_SQ_02_01", 0.5f, 4, 1, "new_desmodus_black", "wizards_marmotte", "flask", "minivern");

		AddObjective("collectCrystals", L("Defeat monsters and get Hardened Black Crystal"), new CollectItemObjective("FTOWER44_SQ_02_01", 10));

		AddReward(new ItemReward("expCard7", 1));
		AddReward(new TakeItemReward("FTOWER44_SQ_02_01"));
	}
}

// 17019: Transmuter Furry Odd (3)
//-----------------------------------------------------------------------------
public class Ftower44Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17019);
		SetName(L("Transmuter Furry Odd (3)"));
		SetDescription(L("Furry Odd holds the Machinery Room door while the Yonazolem inside is dealt with."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_02", "d_firetower_44", L("Talk to Transmuter Furry Odd"), L("Met Furry Odd near the Machinery Room. Talk to her."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_03", "d_firetower_44", L("Defeat Yonazolem"), L("Furry Odd says she saw a Yonazolem inside the machine room. Defeat the Yonazolem while Furry Odd stops the monsters at the entrance."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_02", "d_firetower_44", L("Talk to Transmuter Furry Odd"), L("Defeated Yonazolem. Return to Furry Odd."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "FTOWER44_SQ_03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(17018, QuestStatus.Completed));

		AddObjective("killYonazolem", L("Defeat Yonazolem"), new KillObjective(1, "boss_yonazolem_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard7", 3));
		AddReward(new SelectItemReward("R_LEG02_178", "R_LEG02_179", "R_LEG02_180"));
	}
}

// 17020: A Suspicious Seal
//-----------------------------------------------------------------------------
public class Ftower44Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17020);
		SetName(L("A Suspicious Seal"));
		SetDescription(L("Another of the stones, this one beside the Magic Stabilizing Device."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_04", "d_firetower_44", L("Talk to the Sealed Stone"), L("There is a Sealed Stone near the Magic Stabilizing Device. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_04", "d_firetower_44", L("Defeat the nearby monsters"), L("The Sealed Stone asked you to defeat the monsters nearby so that it can be freed."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_04", "d_firetower_44", L("Talk to the Sealed Stone"), L("Finished the request of the Sealed Stone. Release the soul."));

		AddPrerequisite(new LevelPrerequisite(113));

		AddObjective("killWatchers", L("Defeat the nearby monsters"), new KillObjective(10, "new_desmodus_black", "wizards_marmotte", "flask", "minivern"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 17021: Now It's Really Suspicious
//-----------------------------------------------------------------------------
public class Ftower44Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(17021);
		SetName(L("Now It's Really Suspicious"));
		SetDescription(L("The stone in the 2nd Machinery Room asks for the same thing as all the others."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_05", "d_firetower_44", L("Find the Sealed Stone"), L("There is a Sealed Stone in the 2nd Machinery Room. Talk to it."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_05", "d_firetower_44", L("Defeat the monsters nearby"), L("The Sealed Stone asked you to defeat the monsters nearby so that it can be freed."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_05", "d_firetower_44", L("Talk to the Sealed Stone"), L("Defeated the monsters around. Go to the Stone and release the possessed soul."));

		AddPrerequisite(new LevelPrerequisite(113));

		AddObjective("killWatchers", L("Defeat the nearby monsters"), new KillObjective(5, "new_desmodus_black", "wizards_marmotte", "flask", "minivern"));

		AddReward(new ItemReward("expCard7", 1));
	}
}

// 19051: Crafting and Materials
//-----------------------------------------------------------------------------
public class Firetower44Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19051);
		SetName(L("Crafting and Materials"));
		SetDescription(L("Furry Odd would rather see a bag of Drake Horns made into something than sold."));
		SetType(QuestType.Sub);
		SetLocation("d_firetower_44");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "FTOWER44_SQ_02", "d_firetower_44", L("Talk to Furry Odd"), L("Furry Odd is surprised with the bag full of Drake's Horn. Talk to her."));
		SetPhase(QuestStatus.InProgress, "FTOWER44_SQ_02", "d_firetower_44", L("Make the badge of Drake"), L("Received a recipe from Furry Odd."));
		SetPhase(QuestStatus.Success, "FTOWER44_SQ_02", "d_firetower_44", L("Talk to Furry Odd"), L("Crafted the badge of Drake. Talk to Furry Odd."));

		AddPrerequisite(new QuestStatusPrerequisite(17019, QuestStatus.Completed));
		AddPrerequisite(new ItemPrerequisite("misc_0100", 300));

		AddObjective("craftTheBadge", L("Make the badge of Drake and give it to Furry Odd"), new CollectItemObjective("misc_drakeResc", 1));

		AddReward(new TakeItemReward("misc_drakeResc"));
	}
}
