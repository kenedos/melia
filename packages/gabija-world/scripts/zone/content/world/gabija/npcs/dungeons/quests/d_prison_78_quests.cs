//--- Melia Script ----------------------------------------------------------
// Kalejimas Visiting Room Quest NPCs
//--- Description -----------------------------------------------------------
// Zanas' Soul, the guardian of Laima's revelation, and the first of the
// five demon barriers over Kalejimas Prison.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DPrison78QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq1 = new QuestId(30145);
	private readonly static QuestId Mq2 = new QuestId(30146);
	private readonly static QuestId Mq3 = new QuestId(30147);
	private readonly static QuestId Mq4 = new QuestId(30148);
	private readonly static QuestId Mq5 = new QuestId(30149);
	private readonly static QuestId Mq6 = new QuestId(30150);
	private readonly static QuestId Mq7 = new QuestId(30151);
	private readonly static QuestId Mq8 = new QuestId(30152);
	private readonly static QuestId Mq9 = new QuestId(30153);
	private readonly static QuestId Sq1 = new QuestId(30195);
	private readonly static QuestId Sq2 = new QuestId(30196);

	private const string ZanasPortrait = "Dlg_port_zanas_prison";
	private const string CircleVar = "Gabija.Prison78.Circle";
	private const string CircleTimeVar = "Gabija.Prison78.CircleTime";
	private const string ChestVar = "Gabija.Prison78.SupplyChest";
	private const string TombstoneVar = "Gabija.Prison78.Tombstone";
	private const string PasswordVar = "Gabija.Prison78.Password";
	private const string PasswordTriesVar = "Gabija.Prison78.PasswordTries";
	private const int PasswordMin = 1;
	private const int PasswordMax = 100;
	private const int PasswordTries = 7;

	private readonly static TimeSpan CircleWindow = TimeSpan.FromSeconds(20);
	private readonly static TimeSpan BarrierDownTime = TimeSpan.FromSeconds(15);
	private readonly static Position DemonSeal = new Position(-166, 620, 2156);
	private readonly static Position MandaraLair = new Position(634, 640, 1687);

	// The protection magic circles in the Interrogation Room.
	private readonly static double[,] CircleSpots =
	{
		{ 159.39, -1789.98 }, { 60.27, -1778.75 }, { 137.17, -1627.42 },
	};

	private readonly static double[,] TombstoneSpots =
	{
		{ -1827.75, -1419.61 }, { -1840.04, -1357.96 }, { -1835.99, -1288.89 },
		{ -1826.68, -1219.83 }, { -1793.93, -1144.37 },
	};

	private readonly static double[] TombstoneFacings = { 75, 45, 45, 45, 0 };

	// The tombstone of the millenary maintenance, the works Zanas' grandfather was on.
	private const int TealStoneTombstone = 4;

	private readonly static double[,] ChestSpots =
	{
		{ -479.37, -762.97 }, { -318.77, 1024.43 }, { -284.97, 2482.49 },
	};

	protected override void Load()
	{
		// The Spiritual Voice
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Spiritual Voice"), "PRISON_78_OBJ_1", "d_prison_78", 1144.66, -937.71, 90, this.IsTheVoiceCalling, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				var looked = await character.TimeActions.StartAsync(L("Observing"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Savior, if you can hear my voice, please, find me.."));
				await dialog.Msg(L("Nebulas... the revelation... before it is too late."));

				character.Quests.Start(Mq1);
				character.Quests.CompleteObjective(Mq1, "findTheVoice");
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A mysterious light hangs in the air, and something in it is calling."));
		});

		// Zanas' Soul, at the Visiting Room
		//-------------------------------------------------------------------------
		AddConditionalNpc(151107, L("Zanas' Soul"), "PRISON_78_NPC_1", "d_prison_78", 411.59, -1281.60, 0, this.IsZanasInTheVisitingRoom, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Zanas' Soul"));
			dialog.SetPortrait(ZanasPortrait);

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("Someone, here?"));
				await dialog.Msg(L("You have heard my voice, haven't you?"));
				await dialog.CompleteQuest(Mq1);
				return;
			}

			if (character.Quests.IsActive(Mq2) && character.Quests.IsCompletable(Mq2))
			{
				await dialog.Msg(L("The protective magic seems to be all working properly."));
				await dialog.Msg(L("Now, shall we continue?"));
				await dialog.Msg(L("This prison was built by King Kadumel hundreds of years ago."));
				await dialog.Msg(L("King Kadumel, always being suspiciouos about other people lived in fear that one day he himself might end up in this place, if insurgency ever succeeds."));
				await dialog.Msg(L("So, he has made many secret devices inside this place, in case that ever became reality."));
				await dialog.Msg(L("It can hide something or someone.."));
				await dialog.Msg(L("Case in point, the devices that have either protective or offensive magic you have just seen."));
				await dialog.Msg(L("It is the secret kept by King Kadumel."));
				await dialog.Msg(L("However, my grandfather who worked on the prison maintenance project found them accidentally."));
				await dialog.Msg(L("That is the story I have been raised on... that is why I chose this place."));
				await dialog.Msg(L("After becoming the protector of Goddess Laima's revelation, I thought this was a good place to hide it."));
				await dialog.Msg(L("The first step was not a bad one. Getting myself into the prison with a false charge.."));
				await dialog.Msg(L("And it might have been 4 years ago... I don't know how but Nebulars found this place and rode in with the demons."));
				await dialog.Msg(L("I could not bear to lose the revelation."));
				await dialog.Msg(L("If I get captured, I do not know what would happen... that is why I decided to take my own life."));
				await dialog.Msg(L("After my willful demise, I have split my own soul into many parts by using the secret device."));
				await dialog.Msg(L("That is the secret behind my survival thus far."));
				await dialog.Msg(L("One small side effect of splitting your own soul into many parts is that your memory gets split as well. So, I cannot find the revelation."));
				await dialog.Msg(L("What we can do is to find all of my souls and memories with them and locate the revelation before Nebulars does."));
				await dialog.Msg(L("That is all I can tell for now."));
				await dialog.Msg(L("I will do anything to help you find the revelation."));
				await dialog.CompleteQuest(Mq2);
				return;
			}

			if (character.Quests.IsActive(Mq3) && character.Quests.IsCompletable(Mq3))
			{
				await dialog.Msg(L("Have you found it?"));
				await dialog.Msg(L("It is the right one.."));
				await dialog.Msg(L("But there is a problem."));
				await dialog.CompleteQuest(Mq3);
				return;
			}

			if (character.Quests.IsActive(Mq4) && character.Quests.IsCompletable(Mq4))
			{
				await dialog.Msg(L("Let's see."));
				await dialog.Msg(L("I think it's enough."));
				await dialog.Msg(L("Now, if you need me, use Teal Magic Stone at a safe place."));
				await dialog.Msg(L("I cannot get inside dangerous places like barrier."));
				await dialog.CompleteQuest(Mq4);
				return;
			}

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				await dialog.Msg(L("Then, you truly are a savior."));
				await dialog.Msg(L("I have been waiting for you for a long time.."));
				await dialog.Msg(L("My name is Zanas."));
				await dialog.Msg(L("As you can see, I am a lost soul.."));
				await dialog.Msg(L("I have been protecting the revelation of Laima."));
				await dialog.Msg(L("In this Kalejimas Prison."));
				await dialog.Msg(L("I don't know how the demons got the info but they invaded."));
				await dialog.Msg(L("I will explain in detail."));
				await dialog.Msg(L("But first, we should avoid getting detected by demons. Please, receive protective magic."));
				await dialog.Msg(L("Each floor has demon barrier protecting and detecting intruders. Without protective magic, they would know your location."));

				var answer = await dialog.SelectQuestOffer(Mq2, L("There is a magic circle that I have activated while running away from the demons. Use it to protect yourself from the demon barriers."),
					Option(L("Say that you will go and receive the Protection Magic"), "accept"),
					Option(L("Say that you do not believe him"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("Great. There would be one at the interrogation room."));
					await dialog.Msg(L("When you get on the magic circle, you will be protected."));
					await dialog.Msg(L("But just one magic circle is not going to last long.."));
					await dialog.Msg(L("You have to hop to the next one as fast as possible."));
					await dialog.Msg(L("After getting the protective magic, come talk to me again."));
				}
				return;
			}

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				await dialog.Msg(L("Well, that was a rather long-winded speech."));
				await dialog.Msg(L("I am unable to go anywhere because of the demon barrier."));
				await dialog.Msg(L("Especially, I cannot go near the barrier at all."));
				await dialog.Msg(L("So, it would be wise for you to get to safety and give me a signal, I will be there."));

				var answer = await dialog.SelectQuestOffer(Mq3, L("As a signalling device, I think Teal Magic Stone would do just perfectly. It emits a special signal."),
					Option(L("Say that you will get the Teal Magic Stone"), "accept"),
					Option(L("Say that you do not need such things"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					await dialog.Msg(L("Teal Magic Stone can be found on the back of the tombstone in the confessional."));
					await dialog.Msg(L("I am not which one it was though."));
					await dialog.Msg(L("Whatever the case, I'm sure it's on the back of the Tombstone so check carefully."));
					await dialog.Msg(L("Don't get it confused. It's on the back."));
				}
				return;
			}

			if (!character.Quests.Has(Mq4) && character.Quests.MeetsPrerequisites(Mq4))
			{
				await dialog.Msg(L("The power from Teal Magic Stone is stronger that expected."));
				await dialog.Msg(L("It might alert the demons considering its strength."));
				await dialog.Msg(L("We must do something before they are alerted."));
				await dialog.Msg(L("I know what we should do! We should cover the stone with demon blood to send a mixed signal."));
				await dialog.Msg(L("If the signal is mixed with that of the demons.."));
				await dialog.Msg(L("Those who are not familar with Teal Magic Stone like I am will not detect anything suspicious."));

				var answer = await dialog.SelectQuestOffer(Mq4, L("Defeat the demons and acquire their blood."),
					Option(L("Say that you will gather Demon Blood"), "accept"),
					Option(L("Say that you cannot do such things"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq4);
					await dialog.Msg(L("It doesn't matter which demon as long as it from around here."));
					await dialog.Msg(L("You wouldn't have a difficult time locating one, the place is crawling with them."));
				}
				return;
			}

			if (!character.Quests.Has(Mq5) && character.Quests.MeetsPrerequisites(Mq5))
			{
				await dialog.Msg(L("To dispell a demon barrier, you must destroy Mandara."));
				await dialog.Msg(L("It is a powerful demon protecting Kalejimas Visiting Room."));
				await dialog.Msg(L("On top of that, a demon barrier strengthens Mandara."));
				await dialog.Msg(L("However, there is a way to deactivate a barrier albeit temporarily."));
				await dialog.Msg(L("Secret device in the isolated area has a Magic Control Scroll hidden inside."));

				var answer = await dialog.SelectQuestOffer(Mq5, L("With that, King Kadumel can be defeated."),
					Option(L("Say that you will retrieve the Magic Control Scrolls"), "accept"),
					Option(L("Say that you do not need such things"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq5);
					await dialog.Msg(L("In order to unlock secret device in the isolated area, you must match the brightness of the lights on both sides.."));
					await dialog.Msg(L("The escape plan can only be used by him, no one else."));
					await dialog.Msg(L("Now, that is a mark of a true tyrant, always thinking of escape strategies."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("The magic circle is designed for living people. A soul like me cannot get the protective magic."));
				return;
			}

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("Let's find all the other souls."));
				await dialog.Msg(L("At this point, it is virtually impossible for me to help you."));
				return;
			}

			if (character.Quests.IsActive(Mq4))
			{
				await dialog.Msg(L("I am an ordinary man to begin with.."));
				await dialog.Msg(L("And me being dead, well, is not helping at all."));
				await dialog.Msg(L("It's all my fault, I should have kept it more safely."));
				return;
			}

			dialog.SetPortrait(null);
			await dialog.Msg(L("I can't move around because of the demon barriers."));
			await dialog.Msg(L("There's five of them, you see?"));
		});

		// Visiting Room Tombstones
		//-------------------------------------------------------------------------
		for (var i = 0; i < TombstoneSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc(152032, L("Visiting Room Tombstone"), "PRISON_78_OBJ_3_" + number, "d_prison_78",
				TombstoneSpots[i, 0], TombstoneSpots[i, 1], TombstoneFacings[i], dialog => this.ReadTheTombstone(dialog, number));
		}

		// The Isolated Area's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_78_OBJ_4", "d_prison_78", -1350.66, -77.94, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.IsActive(Mq5) || character.Quests.IsCompletable(Mq5))
			{
				await dialog.Msg(L("A secret device of King Kadumel's, with a light on either side of it."));
				return;
			}

			var activated = await character.TimeActions.StartAsync(L("Activating Secret Device"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

			if (activated != TimeActionResult.Completed)
				return;

			character.ServerMessage(L("The device has been activated. Align the lights on each side to show the same lights."));

			if (!await this.MatchTheLights(dialog))
				return;

			character.ServerMessage(L("The seal on the secret device has been disarmed. Retrieve the Magic Control Scroll from the secret device."));

			var opened = await character.TimeActions.StartAsync(L("Opening the secret device"), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

			if (opened != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.PRISON_78_MQ_5_ITEM, 1, InventoryAddType.PickUp);
		});

		// The Subsidiary Material Storage Room's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151108, L("Secret Device"), "PRISON_78_OBJ_5", "d_prison_78", 607.77, 372.09, 180, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.IsActive(Mq8) || character.Quests.IsCompletable(Mq8))
			{
				await dialog.Msg(L("A secret device with a row of number dials, locked shut."));
				return;
			}

			if (!await this.GuessThePassword(dialog))
				return;

			var opened = await character.TimeActions.StartAsync(L("Opening the secret device"), L("Cancel"), "HANDLING_LEFT", TimeSpan.FromSeconds(2));

			if (opened != TimeActionResult.Completed)
				return;

			character.Inventory.Add(ItemId.PRISON_78_MQ_8_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(L("The device opens on the Orb of Dominance Magic. Call Zanas' Soul with the Teal Magic Stone."));
		});

		// Writings on the Wall
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Writings on the Wall"), "PRISON_78_SQ_OBJ_1", "d_prison_78", -490, 1039.88, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Writings on the Wall"));

			if (!character.Quests.Has(Sq1) && character.Quests.MeetsPrerequisites(Sq1))
			{
				var looked = await character.TimeActions.StartAsync(L("Checking Writing on the Walls"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				await this.ReadTheBoast(dialog);
				character.Quests.Start(Sq1);
				return;
			}

			await this.ReadTheBoast(dialog);
		});

		// The prisoner's stash
		//-------------------------------------------------------------------------
		AddNpc(147469, L("Hole in the Ground"), "PRISON_78_SQ_OBJ_2", "d_prison_78", 426.57, 401.13, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Hole in the Ground"));

			if (!character.Quests.IsActive(Sq1))
			{
				await dialog.Msg(L("A hole in the ground, half hidden under junk."));
				return;
			}

			var searched = await character.TimeActions.StartAsync(L("Examining the hole in the ground"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

			if (searched != TimeActionResult.Completed)
				return;

			await dialog.Msg(L("Down in the hole is a bundle nobody has touched in half a year. The prisoner hid it well after all."));
			character.Quests.CompleteObjective(Sq1, "findTheStash");
			await dialog.CompleteQuest(Sq1);
		});

		// Jibuza Square's Secret Device
		//-------------------------------------------------------------------------
		AddNpc(151110, L("Secret Device"), "PRISON_78_SQ_OBJ_3", "d_prison_78", 348.88, -524.37, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Device"));

			if (!character.Quests.Has(Sq2) && character.Quests.MeetsPrerequisites(Sq2))
			{
				var looked = await character.TimeActions.StartAsync(L("Examining the Secret Device"), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

				if (looked != TimeActionResult.Completed)
					return;

				character.Variables.Perm.SetInt(ChestVar, System.Random.Shared.Next(ChestSpots.GetLength(0)) + 1);
				character.Quests.Start(Sq2);
				character.LookAround();
				character.ServerMessage(L("Something seems to have been activated but nothing seems to have changed. Try checking elsewhere."));
				return;
			}

			await dialog.Msg(L("The device has already done whatever it was built to do."));
		});

		// The Waiting Room's Supply Chests
		//-------------------------------------------------------------------------
		for (var i = 0; i < ChestSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(46212, L("Supply Chest"), "PRISON_78_SQ_OBJ_4_" + number, "d_prison_78", ChestSpots[i, 0], ChestSpots[i, 1], 180, character => character.Quests.IsActive(Sq2), async dialog =>
			{
				var character = dialog.Player;

				dialog.SetTitle(L("Supply Chest"));

				var opened = await character.TimeActions.StartAsync(L("Examining the supply crate"), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

				if (opened != TimeActionResult.Completed)
					return;

				if (character.Variables.Perm.GetInt(ChestVar, 0) != number)
				{
					await dialog.Msg(L("Empty. Whatever the device woke up, it was not this chest."));
					return;
				}

				await dialog.Msg(L("The chest the device unlocked, still stocked with the Waiting Room's supplies."));
				character.Quests.CompleteObjective(Sq2, "findTheChest");
				await dialog.CompleteQuest(Sq2);
				character.Variables.Perm.Remove(ChestVar);
				character.LookAround();
			});
		}

		// Hidden triggers
		//-------------------------------------------------------------------------
		for (var i = 0; i < CircleSpots.GetLength(0); ++i)
		{
			var number = i + 1;

			AddQuestTrigger(i == 0 ? "PRISON_78_OBJ_2" : "PRISON_78_OBJ_2_" + number, "d_prison_78", CircleSpots[i, 0], CircleSpots[i, 1], 40, args => this.StepOnTheCircle(args, number));
		}

		AddQuestTrigger("PRISON_78_MQ_7_TRIGGER", "d_prison_78", 634, 1687, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq7) && !character.Quests.IsCompletable(Mq7))
				character.Quests.StartQuestTrack(Mq7);

			await Task.CompletedTask;
		});

		AddQuestTrigger("PRISON_78_MQ_9_TRIGGER", "d_prison_78", -166, 2156, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq9) && !character.Quests.IsCompletable(Mq9))
				character.Quests.CompleteObjective(Mq9, "reachTheBarrier");
			else if (character.Quests.IsActive(Mq9))
				character.Quests.ReplayQuestTrack(Mq9);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Calls Zanas' Soul through the Teal Magic Stone.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON_78_MQ_3_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_prison_78")
		{
			character.ServerMessage(L("You cannot call Zanas right now"));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq7) && character.Position.Get2DDistance(MandaraLair) < 600)
		{
			character.ServerMessage(L("You cannot call Zanas near Mandara and the demon seal"));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(DemonSeal) < 400)
		{
			character.ServerMessage(L("You cannot call Zanas near the demon seal"));
			return ItemUseResult.OkayNotConsumed;
		}

		character.StartDialog(item, this.TalkThroughTheStone);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Suppresses the Demon Barrier protecting Mandara for a while.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_PRISON_78_MQ_5_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var mandara = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 350)
			.FirstOrDefault(entity => entity is Mob mob && mob.Id == MonsterId.Boss_Mandara_Q1);

		if (mandara == null)
		{
			character.ServerMessage(L("There are no suitable targets nearby."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (!mandara.IsBuffActive(BuffId.PRISON_78_MQ_7_BUFF))
		{
			character.ServerMessage(L("The magic is already being suppressed"));
			return ItemUseResult.OkayNotConsumed;
		}

		mandara.StopBuff(BuffId.PRISON_78_MQ_7_BUFF);
		mandara.PlayEffect("F_lineup020_blue_mint", 1.5f);
		character.ServerMessage(L("The power that had been protecting Mandara has disappeared!"));

		_ = this.RaiseTheBarrierAgain(mandara);
		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Puts the Demon Barrier back on Mandara once the scroll wears off.
	/// </summary>
	/// <param name="mandara"></param>
	private async Task RaiseTheBarrierAgain(ICombatEntity mandara)
	{
		await Task.Delay(BarrierDownTime);

		if (!mandara.IsDead)
			mandara.StartBuff(BuffId.PRISON_78_MQ_7_BUFF);
	}

	/// <summary>
	/// Zanas' Soul, answering the Teal Magic Stone.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task TalkThroughTheStone(Dialog dialog)
	{
		var character = dialog.Player;

		var called = await character.TimeActions.StartAsync(L("Calling Zanas"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

		if (called != TimeActionResult.Completed)
			return;

		dialog.SetTitle(L("Zanas' Soul"));
		dialog.SetPortrait(ZanasPortrait);

		if (character.Quests.IsActive(Mq5) && character.Quests.IsCompletable(Mq5))
		{
			await dialog.Msg(L("Do you have it?"));
			await dialog.Msg(L("You are the Revelator selected by the goddess."));
			await dialog.Msg(L("You will get the revelation."));
			await dialog.CompleteQuest(Mq5);
			return;
		}

		if (character.Quests.IsActive(Mq6) && character.Quests.IsCompletable(Mq6))
		{
			await dialog.Msg(L("The Magic Control Scroll seems to be ready to go."));
			await dialog.Msg(L("All that is left is the downfall of King Kadumel."));
			await dialog.CompleteQuest(Mq6);
			return;
		}

		if (character.Quests.IsActive(Mq7) && character.Quests.IsCompletable(Mq7))
		{
			await dialog.Msg(L("Are you hurt?"));
			await dialog.Msg(L("That's a relief."));
			await dialog.Msg(L("Now disabling demon barriers."));
			await dialog.CompleteQuest(Mq7);
			return;
		}

		if (character.Quests.IsActive(Mq8) && character.Quests.IsCompletable(Mq8))
		{
			await dialog.Msg(L("You got the Orb of Dominance Magic."));
			await dialog.Msg(L("Nice job."));
			await dialog.Msg(L("Also, I figured out a way to use Orb of Dominance Magic."));
			await dialog.CompleteQuest(Mq8);
			return;
		}

		if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
		{
			await dialog.Msg(L("The spell that you want to suppress must be inscribed in Magic Control Scroll first."));
			await dialog.Msg(L("No ordinary method can do the job.."));
			await dialog.Msg(L("The demons have very similar spells among themselves."));
			await dialog.Msg(L("So, it doesn't have to be from King Kadumel."));
			await dialog.Msg(L("Defeat the demons and inscribe its spell on the scroll."));

			var answer = await dialog.SelectQuestOffer(Mq6, L("It's much safer and wiser than charging straight to King Kadumel."),
				Option(L("Say that you will register their magic"), "accept"),
				Option(L("Argue that you can go to Mandara now"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq6);
				await dialog.Msg(L("Your safety comes first."));
				await dialog.Msg(L("For safely delivering the revelation to you is the sole reason why I am staying here instead of the bossoms of the goddess."));
			}
			return;
		}

		if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
		{
			await dialog.Msg(L("Demon barriers are protecting King Kadumel from any harm."));
			await dialog.Msg(L("That is why we made the Magic Control Scroll."));
			await dialog.Msg(L("That will suppress the barriers and then you must strike King Kadumel."));
			await dialog.Msg(L("However, the Magic Control Scroll will not keep the barriers down completely."));
			await dialog.Msg(L("It will regenerate again and again."));
			await dialog.Msg(L("So, you have to use Magic Control Scroll every time it rises up."));
			await dialog.Msg(L("Got it?"));

			var answer = await dialog.SelectQuestOffer(Mq7, L("Now, let's go and defeat King Kadumel."),
				Option(L("Say that you will defeat Mandara"), "accept"),
				Option(L("Say that you need some more time"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq7);
				await dialog.Msg(L("King Kadumel is in Offender Institution."));
				await dialog.Msg(L("Be careful and keep what I have told you in mind at all time."));
			}
			return;
		}

		if (!character.Quests.Has(Mq8) && character.Quests.MeetsPrerequisites(Mq8))
		{
			await dialog.Msg(L("To disable demon barrier, you must go down to Supply Room."));
			await dialog.Msg(L("The barrier is quite strong and requires something in return."));
			await dialog.Msg(L("The secret device in the supply room has Orb of Dominance Magic."));
			await dialog.Msg(L("I believe it is the last card up King Kadumel's sleeve."));
			await dialog.Msg(L("Dominance Magic is a powerful magic, it is enough to dispel demon barrier."));

			var answer = await dialog.SelectQuestOffer(Mq8, L("You might face a slight problem but first, let's find the orb."),
				Option(L("Go to the Subsidiary Material Storage Room's Secret Device"), "accept"),
				Option(L("Say that you do not need such things"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq8);
				await dialog.Msg(L("The secret device in the supply room can only be opened by entering a password."));
				await dialog.Msg(L("The problem is the memory about the password, I don't have."));
				await dialog.Msg(L("Perhaps, there might be a part of my soul that remembers the password..."));
				await dialog.Msg(L("But at the moment, we have no time, we have to solve it ourselves."));
				await dialog.Msg(L("The password is comprised of numbers."));
				await dialog.Msg(L("There might be some clues around the device."));
			}
			return;
		}

		if (!character.Quests.Has(Mq9) && character.Quests.MeetsPrerequisites(Mq9))
		{
			await dialog.Msg(L("I did say something about giving something in return, right?"));
			await dialog.Msg(L("Dominance Magic usually needs the user's life force to activate itself."));
			await dialog.Msg(L("Quite fitting for the last card in King Kadumel's sleeve."));
			await dialog.Msg(L("But offering the life of Revelator is not an option."));
			await dialog.Msg(L("Last of all, all of this is mine own fault. I should have protected the revelation.."));
			await dialog.Msg(L("Dominance Magic requires lifeforce or something equivalent or higher."));

			var answer = await dialog.SelectQuestOffer(Mq9, L("That is why... I have decided to use my own soul... I will feed Dominance Magic my soul."),
				Option(L("Ask if it isn't dangerous"), "accept"),
				Option(L("Say that you don't know what he's talking about"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq9);
				await dialog.Msg(L("If it takes my entire soul, my very existence will cease to be."));
				await dialog.Msg(L("However, the soul is something much valuable than the lifeforce, so it will only drain a portion of my soul."));
				await dialog.Msg(L("But, repeat doing that, it will drain my existence completely.."));
				await dialog.Msg(L("If giving up my existence means saving you, Revelator, I have no regret."));
				await dialog.Msg(L("Don't worry about me, let's just disable that demon barrier."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq5))
		{
			await dialog.Msg(L("As you can tell by secret devices, what kind of a person King Kadumel was like."));
			await dialog.Msg(L("Nota trusting one, I can tell you that much."));
			return;
		}

		if (character.Quests.IsActive(Mq6))
		{
			await dialog.Msg(L("The Magic Control Scroll will not completely subdue King Kadumel."));
			await dialog.Msg(L("In Kalejimas Prison, there are five demon barriers."));
			return;
		}

		if (character.Quests.IsActive(Mq7))
		{
			await dialog.Msg(L("King Kadumel was one of Nebulas' minions."));
			await dialog.Msg(L("Demon Lord Nebulas is far stronger than King Kadumel."));
			character.Quests.ClearQuestTrack(Mq7);
			return;
		}

		if (character.Quests.IsActive(Mq8))
		{
			await dialog.Msg(L("I should think about how to use the Orb of Dominance Magic when we acquire it."));
			await dialog.Msg(L("It is very powerful. A powerful magic requires something of equal worth in return."));
			return;
		}

		if (character.Quests.IsActive(Mq9))
		{
			await dialog.Msg(L("Getting the revelation of Goddess Laima to you, Revelator, is my duty, even after death."));
			await dialog.Msg(L("I have died for the revelation once.."));
			await dialog.Msg(L("I would do whatever it takes to deliver the revelation to you..."));
			return;
		}

		dialog.SetPortrait(null);
		await dialog.Msg(L("I think we can talk here."));
	}

	/// <summary>
	/// Reads one of the Visiting Room tombstones, and checks its back for
	/// the Teal Magic Stone.
	/// </summary>
	/// <param name="dialog"></param>
	/// <param name="number"></param>
	private async Task ReadTheTombstone(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Visiting Room Tombstone"));

		switch (number)
		{
			case 1:
				await dialog.Msg(L("Long live the kingdom of the Great King Kadumel."));
				break;
			case 2:
				await dialog.Msg(L("Year 474"));
				await dialog.Msg(L("The construction of Kalejimas Prison is completed under the name of King Kadumel."));
				break;
			case 3:
				await dialog.Msg(L("Year 478"));
				await dialog.Msg(L("King Kadumel, responsible for the construction of Kalejimas prison, is deceased."));
				break;
			case 4:
				await dialog.Msg(L("Year 1001"));
				await dialog.Msg(L("Kalejimas Prison undergoes maintenance in celebration of the millenary."));
				break;
			default:
				await dialog.Msg(L("May the offenders of Kalejimas Prison repent before the name of the goddesses."));
				break;
		}

		if (!character.Quests.IsActive(Mq3) || character.Quests.IsCompletable(Mq3))
			return;

		if (character.Variables.Temp.GetBool(TombstoneVar + number, false))
		{
			character.ServerMessage(L("You have already examined this tombstone"));
			return;
		}

		var answer = await dialog.Select(L("Check the back of the tombstone?"),
			Option(L("Check the back"), "check"),
			Option(L("Leave it"), "leave")
		);

		if (answer != "check")
			return;

		var checkedBack = await character.TimeActions.StartAsync(L("Checking the back of the tombstone..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(2));

		if (checkedBack != TimeActionResult.Completed)
			return;

		character.Variables.Temp.SetBool(TombstoneVar + number, true);

		if (number != TealStoneTombstone)
		{
			character.ServerMessage(L("There was nothing on this tombstone."));
			return;
		}

		character.Inventory.Add(ItemId.PRISON_78_MQ_3_ITEM, 1, InventoryAddType.PickUp);
		character.ServerMessage(L("Found the Teal Magic Stone"));
	}

	/// <summary>
	/// Reads the prisoner's boast off the wall near the Waiting Room.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task ReadTheBoast(Dialog dialog)
	{
		await dialog.Msg(L("the gards must be reely stoopid."));
		await dialog.Msg(L("or maybe im just reely smart?"));
		await dialog.Msg(L("their trying to find a haystak in a needel haha."));
		await dialog.Msg(L("they hid some stuf in a pile of more stuf, nao its been half a year and they stil cant find it."));
		await dialog.Msg(L("im also trying to find my hiden stuf."));
		await dialog.Msg(L("i hid dem reely well. i must be a geenias..."));
	}

	/// <summary>
	/// Runs the Storage Room device's password lock, which answers every
	/// wrong number with whether the password is larger or smaller.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task<bool> GuessThePassword(Dialog dialog)
	{
		var character = dialog.Player;

		if (!character.Variables.Temp.Has(PasswordVar))
		{
			character.Variables.Temp.SetInt(PasswordVar, System.Random.Shared.Next(PasswordMin, PasswordMax + 1));
			character.Variables.Temp.SetInt(PasswordTriesVar, PasswordTries);
		}

		var password = character.Variables.Temp.GetInt(PasswordVar, 0);

		while (true)
		{
			var tries = character.Variables.Temp.GetInt(PasswordTriesVar, PasswordTries);
			var input = await dialog.Input(LF("Please enter the password. Remaining Attempts : {0} Range : {1} ~ {2}", tries, PasswordMin, PasswordMax));

			if (!int.TryParse(input?.Trim(), out var guess))
			{
				await dialog.Msg(L("Enter a number."));
				return false;
			}

			if (guess < PasswordMin || guess > PasswordMax)
			{
				await dialog.Msg(LF("Please enter numbers within {0} ~ {1}.", PasswordMin, PasswordMax));
				continue;
			}

			if (guess == password)
			{
				character.Variables.Temp.Remove(PasswordVar);
				character.Variables.Temp.Remove(PasswordTriesVar);
				character.ServerMessage(L("The secret device has been disarmed"));
				return true;
			}

			tries--;

			if (tries <= 0)
			{
				character.Variables.Temp.Remove(PasswordVar);
				character.Variables.Temp.Remove(PasswordTriesVar);
				await dialog.Msg(L("You have exceeded your limit of attempts. Please try again."));
				return false;
			}

			character.Variables.Temp.SetInt(PasswordTriesVar, tries);
			await dialog.Msg(guess < password ? LF("The password is larger than {0}.", guess) : LF("The password is smaller than {0}.", guess));
		}
	}

	/// <summary>
	/// Runs the Isolated Area device's lights until both sides match,
	/// returns false if the player walks away.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task<bool> MatchTheLights(Dialog dialog)
	{
		var levels = new[] { L("dim"), L("bright"), L("blinding") };
		var left = System.Random.Shared.Next(levels.Length);
		var right = (left + 1 + System.Random.Shared.Next(levels.Length - 1)) % levels.Length;

		while (left != right)
		{
			var answer = await dialog.Select(LF("The light on the left is {0}. The light on the right is {1}.", levels[left], levels[right]),
				Option(L("Pour magic into the left light"), "left"),
				Option(L("Pour magic into the right light"), "right"),
				Option(L("Step away"), "leave")
			);

			if (answer == "left")
				left = (left + 1) % levels.Length;
			else if (answer == "right")
				right = (right + 1) % levels.Length;
			else
				return false;
		}

		return true;
	}

	/// <summary>
	/// Marks one of the Interrogation Room's protection magic circles, and
	/// completes the Protection Magic once all three are held in time.
	/// </summary>
	/// <param name="args"></param>
	/// <param name="number"></param>
	private async Task StepOnTheCircle(TriggerActorArgs args, int number)
	{
		if (args.Initiator is not Character character)
			return;

		if (!character.Quests.IsActive(Mq2) || character.Quests.IsCompletable(Mq2))
			return;

		var now = DateTime.Now.Ticks;
		var last = character.Variables.Temp.GetLong(CircleTimeVar, 0);

		if (last == 0 || now - last > CircleWindow.Ticks)
		{
			for (var i = 1; i <= CircleSpots.GetLength(0); ++i)
				character.Variables.Temp.Remove(CircleVar + i);
		}

		character.Variables.Temp.SetBool(CircleVar + number, true);
		character.Variables.Temp.SetLong(CircleTimeVar, now);

		var held = 0;
		for (var i = 1; i <= CircleSpots.GetLength(0); ++i)
		{
			if (character.Variables.Temp.GetBool(CircleVar + i, false))
				held++;
		}

		if (held < CircleSpots.GetLength(0))
		{
			character.ServerMessage(LF("The protective magic takes hold ({0}/{1}). Hurry to the next magic circle!", held, CircleSpots.GetLength(0)));
			return;
		}

		character.Quests.CompleteObjective(Mq2, "protection");
		character.ServerMessage(L("The Protection Magic is complete. Return to Zanas' Soul."));

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether the Soul's Voice is still calling out.
	/// </summary>
	/// <param name="character"></param>
	private bool IsTheVoiceCalling(Character character)
		=> !character.Quests.Has(Mq1);

	/// <summary>
	/// Returns whether Zanas' Soul is waiting in the Visiting Room.
	/// </summary>
	/// <param name="character"></param>
	private bool IsZanasInTheVisitingRoom(Character character)
		=> character.Quests.Has(Mq1) && !character.Quests.Has(Mq5);
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30145: Revelation Guardian Zanas(1)
//-----------------------------------------------------------------------------
public class Prison78Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30145);
		SetName(L("Revelation Guardian Zanas(1)"));
		SetDescription(L("A voice in a mysterious light is calling for the savior."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_OBJ_1", "d_prison_78", L("Listen to the Soul's voice"), L("There is a mysterious light in the air. Go closer to check what it is."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_NPC_1", "d_prison_78", L("Search for the source of the voice"), L("You hear the voice of someone looking for you from the mysterious light. Look for where it is coming from."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_1", "d_prison_78", L("Search for the source of the voice"), L("You hear the voice of someone looking for you from the mysterious light. Look for where it is coming from."));

		AddPrerequisite(new LevelPrerequisite(249));
		AddPrerequisite(new QuestStatusPrerequisite(50084, QuestStatus.Completed));

		AddObjective("findTheVoice", L("Search for the source of the voice"), new ManualObjective());
	}
}

// 30146: Revelation Guardian Zanas(2)
//-----------------------------------------------------------------------------
public class Prison78Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30146);
		SetName(L("Revelation Guardian Zanas(2)"));
		SetDescription(L("Protection Magic keeps the demon barriers from finding you."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_1", "d_prison_78", L("Talk to Zanas' Soul"), L("The source of the voice was Zanas' Soul. Listen to what Zanas' Soul has to say."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_OBJ_2", "d_prison_78", L("Complete the Protection Magic"), L("Zanas' Soul says that you require Protection Magic that will protect you from the Demon Barrier. Go to the Protection Magic Circle in the Interrogation Room and complete the Protection Magic."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_1", "d_prison_78", L("Report back to Zanas' Soul"), L("The Protection Magic has been completed. Return to Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30145, QuestStatus.Completed));

		AddObjective("protection", L("Complete the Protection Magic"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30147: Revelation Guardian Zanas(3)
//-----------------------------------------------------------------------------
public class Prison78Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30147);
		SetName(L("Revelation Guardian Zanas(3)"));
		SetDescription(L("A Teal Magic Stone lets Zanas be called without alerting the demons."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_1", "d_prison_78", L("Talk to Zanas' Soul"), L("You have successfully completed the Protection Magic. Talk to Zanas' Soul."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_OBJ_3_1", "d_prison_78", L("Search for the Teal Magic Stone on the back of the Tombstone"), L("Zanas' Soul says that you should obtain the Teal Magic Stone which will allow you to contact him without alerting the Demons. Look for the Teal Magic Stone on the back of the Visiting Room Tombstone."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_1", "d_prison_78", L("Report back to Zanas"), L("You have found the Teal Magic Stone. Listen to Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30146, QuestStatus.Completed));

		AddObjective("findTheStone", L("Search for the Teal Magic Stone on the back of the Tombstone"), new CollectItemObjective("PRISON_78_MQ_3_ITEM", 1));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30148: Bloody Magic Stone
//-----------------------------------------------------------------------------
public class Prison78Mq4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30148);
		SetName(L("Bloody Magic Stone"));
		SetDescription(L("Demon blood on the Teal Magic Stone hides its signal among the demons'."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_1", "d_prison_78", L("Talk to Zanas' Soul"), L("Talk to Zanas' Soul about what to do next now that you have the Teal Magic Stone."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_NPC_1", "d_prison_78", L("Obtain Kalejimas Demon Blood by defeating Demons"), L("Zanas' Soul wishes to mix Demon energy into the Teal Magic Stone to avoid suspicion. Defeat Demons to obtain Kalejimas Demon Blood."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_1", "d_prison_78", L("Give it to Zanas' Soul"), L("You have collected enough Kalejimas Demon Blood. Take it to Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30147, QuestStatus.Completed));

		AddObjective("collectBlood", L("Obtain Kalejimas Demon Blood by defeating Demons"), new CollectItemObjective("PRISON_78_MQ_4_ITEM", 20));

		AddPityDrop("PRISON_78_MQ_4_ITEM", 1.0f, 0, 1, "TerraNymph_brown", "NightMaiden_mage_red", "Elet_blue");

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
		AddReward(new TakeItemReward("PRISON_78_MQ_4_ITEM"));
	}
}

// 30149: How to beat stronger foes(1)
//-----------------------------------------------------------------------------
public class Prison78Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30149);
		SetName(L("How to beat stronger foes(1)"));
		SetDescription(L("The Isolated Area's Secret Device hides a Magic Control Scroll."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_1", "d_prison_78", L("Talk to Zanas' Soul"), L("You have mixed Demon energy into the Teal Magic Stone to avoid suspicion. Listen to what Zanas has to say about what to do next."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_OBJ_4", "d_prison_78", L("Disarm the Isolated Area's Secret Device"), L("Zanas' Soul says that you require Kalejimas Magic Control Scrolls to defeat the Demon Mandara guarding the Visiting Room. Match the lights to the same color when they appear after you activate the Isolated Area's Secret Device. You will be able to obtain the Magic Control Scroll once the Secret Device is disarmed."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("You have found the Magic Control Scroll. Talk to Zanas' Soul with the Teal Magic Stone."));

		AddPrerequisite(new QuestStatusPrerequisite(30148, QuestStatus.Completed));

		AddObjective("findTheScroll", L("Obtain the Magic Control Scroll by disarming the Isolated Area's Secret Device"), new CollectItemObjective("PRISON_78_MQ_5_ITEM", 1));

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30150: How to beat stronger foes(2)
//-----------------------------------------------------------------------------
public class Prison78Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30150);
		SetName(L("How to beat stronger foes(2)"));
		SetDescription(L("The Magic Control Scroll has to learn the magic it is meant to suppress."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("Use the Teal Magic Stone to call Zanas' Soul and listen about what to do next."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_NPC_2", "d_prison_78", L("Regsiter Demon Magic on the Magic Control Scroll"), L("Zanas' Soul says that you need to register the magic you wish to control on the Magic Control Scroll. Defeat Demons to register their magic on the Magic Control Scroll."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("You have successfully registered Demon Magic on the Magic Control Scroll. Use the Teal Magic Stone to call Zanas' Soul."));

		AddPrerequisite(new QuestStatusPrerequisite(30149, QuestStatus.Completed));

		AddObjective("registerMagic", L("Defeat Demons to register their magic on the Magic Control Scroll"), new KillObjective(10, "TerraNymph_brown", "NightMaiden_mage_red", "Elet_blue"));

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8260));
	}
}

// 30151: How to beat stronger foes(3)
//-----------------------------------------------------------------------------
public class Prison78Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30151);
		SetName(L("How to beat stronger foes(3)"));
		SetDescription(L("Mandara guards the Visiting Room behind a barrier that keeps rising."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("You have successfully registered Demon Magic on the Magic Control Scroll. Use the Teal Magic Stone to call Zanas' Soul and ask if you can now defeat Mandara."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_MQ_7_TRIGGER", "d_prison_78", L("Defeat Mandara"), L("Zanas' Soul says that you should now defeat Mandara who is guarding the Kalejimas Visiting Room. Find Mandara in the Offender Institution and use the Magic Control Scroll to reduce the power of the Barrier and defeat him."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("You have defeated Mandara. Tell Zanas' Soul by using the Teal Magic Stone."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "PRISON_78_MQ_7_TRACK", "m_boss_a", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30150, QuestStatus.Completed));

		AddObjective("killMandara", L("Defeat Mandara"), new KillObjective(1, "boss_Mandara_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
		AddReward(new TakeItemReward("PRISON_78_MQ_5_ITEM"));
	}
}

// 30152: Dominance Magic
//-----------------------------------------------------------------------------
public class Prison78Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30152);
		SetName(L("Dominance Magic"));
		SetDescription(L("King Kadumel's last card is an Orb of Dominance Magic behind a password."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("Use the Teal Magic Stone to call Zanas' Soul and talk to him."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_OBJ_5", "d_prison_78", L("Disarm the Secret Device to obtain the Dominance Magic Orb in the Subsidiary Material Storage Room"), L("Zanas' Soul says that you have to use Dominance Magic to disarm the Demon Barrier. Disarm the Subsidiary Material Storage Room's Secret Device to find the Dominance Magic Orb."));
		SetPhase(QuestStatus.Success, "PRISON_78_NPC_2", "d_prison_78", L("Give it to Zanas' Soul"), L("You have obtained the Dominance Magic Orb. Tell this fact to Zanas' Soul by using the Teal Magic Stone."));

		AddPrerequisite(new QuestStatusPrerequisite(30151, QuestStatus.Completed));

		AddObjective("findTheOrb", L("Obtain the Orb of Dominance Magic by disarming the Subsidiary Material Storage Room's Secret Device"), new CollectItemObjective("PRISON_78_MQ_8_ITEM", 1));

		AddReward(new ItemReward("expCard12", 2));
		AddReward(new ItemReward("Vis", 8260));
		AddReward(new TakeItemReward("PRISON_78_MQ_8_ITEM"));
	}
}

// 30153: Visiting Room Barrier
//-----------------------------------------------------------------------------
public class Prison78Mq9Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30153);
		SetName(L("Visiting Room Barrier"));
		SetDescription(L("Zanas pays for the Dominance Magic with a piece of his own soul."));
		SetType(QuestType.Main);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_NPC_2", "d_prison_78", L("Talk to Zanas' Soul"), L("Use the Teal Magic Stone to call Zanas' Soul and talk to him."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_MQ_9_TRIGGER", "d_prison_78", L("Move to the Demon Barrier"), L("Zanas' Soul says that it will disarm the Demon Barrier with Dominance Magic. Go to the Demon Barrier."));
		SetPhase(QuestStatus.Success, "PRISON_78_MQ_9_TRIGGER", "d_prison_78", L("Move to the Demon Barrier"), L("Zanas' Soul says that it will disarm the Demon Barrier with Dominance Magic. Go to the Demon Barrier."));

		SetTrack(QuestStatus.Success, QuestStatus.Completed, "PRISON_78_MQ_9_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30152, QuestStatus.Completed));

		AddObjective("reachTheBarrier", L("Move to the Demon Barrier"), new ManualObjective());
	}
}

// 30195: Prisoner Contraband
//-----------------------------------------------------------------------------
public class Prison78Sq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30195);
		SetName(L("Prisoner Contraband"));
		SetDescription(L("A prisoner boasted on the wall about a stash nobody could find."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_SQ_OBJ_1", "d_prison_78", L("Check the Writings on the Wall"), L("There is something written on a cell near the Waiting Room. Read it and see what it has to say."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_SQ_OBJ_2", "d_prison_78", L("Check the location indicated by the writings on the wall"), L("According to the contents, it seems as if it was written by a prisoner from Kalejimas Prison. Try to find the place where the prisoner hid the contraband."));
		SetPhase(QuestStatus.Success, "PRISON_78_SQ_OBJ_2", "d_prison_78", L("Check the location indicated by the writings on the wall"), L("According to the contents, it seems as if it was written by a prisoner from Kalejimas Prison. Try to find the place where the prisoner hid the contraband."));

		AddPrerequisite(new LevelPrerequisite(249));

		AddObjective("findTheStash", L("Check the location indicated by the writings on the wall"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8290));
		AddReward(new ItemReward("Drug_Premium_HP1", 20));
	}
}

// 30196: Visiting Room's Secret Device
//-----------------------------------------------------------------------------
public class Prison78Sq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30196);
		SetName(L("Visiting Room's Secret Device"));
		SetDescription(L("The Jibuza Square device unlocked something somewhere in the Waiting Room."));
		SetType(QuestType.Sub);
		SetLocation("d_prison_78");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "PRISON_78_SQ_OBJ_3", "d_prison_78", L("Check the Secret Device at Jibuza Square"), L("There is a Secret Device at Jibuza Square. Carefully check what it's functions are."));
		SetPhase(QuestStatus.InProgress, "PRISON_78_SQ_OBJ_4_1", "d_prison_78", L("Check the Supply Chest that is hidden somewhere in the Waiting Room"), L("You heard something being activated as soon as you activated the Device but nothing seems to have changed. Try checking the Waiting Room from top to bottom."));
		SetPhase(QuestStatus.Success, "PRISON_78_SQ_OBJ_4_1", "d_prison_78", L("Check the Supply Chest that is hidden somewhere in the Waiting Room"), L("You heard something being activated as soon as you activated the Device but nothing seems to have changed. Try checking the Waiting Room from top to bottom."));

		AddPrerequisite(new LevelPrerequisite(249));

		AddObjective("findTheChest", L("Check the Supply Chest that is hidden somewhere in the Waiting Room"), new ManualObjective());

		AddReward(new ItemReward("expCard12", 1));
		AddReward(new ItemReward("Vis", 8290));
		AddReward(new ItemReward("Drug_Premium_SP1", 20));
	}
}
