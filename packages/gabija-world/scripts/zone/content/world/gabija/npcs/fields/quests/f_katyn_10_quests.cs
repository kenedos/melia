//--- Melia Script ----------------------------------------------------------
// Karolis Springs Quest NPCs
//--- Description -----------------------------------------------------------
// Liaison Officer Mardas, the Owl Chief Sculpture and the owls the demons
// bent to their will.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
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

public class FKatyn10QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(30049);
	private readonly static QuestId Mq02 = new QuestId(30050);
	private readonly static QuestId Mq03 = new QuestId(30051);
	private readonly static QuestId Mq04 = new QuestId(30052);
	private readonly static QuestId Mq05 = new QuestId(30053);
	private readonly static QuestId Mq06 = new QuestId(30054);
	private readonly static QuestId Mq07 = new QuestId(30055);
	private readonly static QuestId Mq08 = new QuestId(30056);
	private readonly static QuestId Mq09 = new QuestId(30057);
	private readonly static QuestId Mq10 = new QuestId(30058);
	private readonly static QuestId Mq11 = new QuestId(30059);
	private readonly static QuestId Sq01 = new QuestId(30071);
	private readonly static QuestId Rp1 = new QuestId(60164);
	private readonly static QuestId Katyn12Mq04 = new QuestId(30063);
	private readonly static QuestId Katyn12Mq10 = new QuestId(30069);
	private readonly static QuestId Katyn12Hq1 = new QuestId(50271);

	public const string OwlCountVar = "Gabija.Quests.Katyn10Mq08.Owls";
	private const string OwlVar = "Gabija.Quests.Katyn10Mq08.Owl";
	private const string LightVar = "Gabija.Quests.Katyn10Mq07.Light";
	private const string RingTombVar = "Gabija.Quests.Katyn12Hq1.RingTomb";
	private const string TombVar = "Gabija.Quests.Katyn12Hq1.Tomb";

	private static readonly TimeSpan LightDuration = TimeSpan.FromSeconds(30);
	private static readonly Position CrystalSpot = new Position(4507.03f, 134.52f, -867.50f);

	private static readonly double[,] Owls =
	{
		{ 12081, 2726.46, -1027.67 }, { 48002, 2763, -819 }, { 48003, 3517.92, -299.14 },
		{ 48004, 3072.55, 294.20 }, { 48002, 2307.06, 163.06 }, { 48003, 1978.54, 269.55 },
	};

	private static readonly double[,] Lights =
	{
		{ 1260.51, -566.17 }, { 1275.75, -430.62 }, { 1412.14, -428.29 }, { 1402.55, -572.09 },
	};

	private static readonly double[,] Tombs =
	{
		{ 3546.14, 946.23 }, { 3693.55, 1132.86 }, { 3639.30, 1343 }, { 3587.69, 1413.75 }, { 2813.34, -166.32 },
		{ 2770.54, 481.67 }, { 2416.12, 579.79 }, { 4331.45, -466.69 }, { 3652.95, -1242.61 }, { 4244.94, -1309.52 },
		{ 4256.04, -993.24 }, { 2910.61, 668.39 }, { 4920.41, 281.36 }, { 2855.56, -466.54 }, { 3728.94, -145.03 },
	};

	private static readonly string[] SpiritMonsters = { "digo", "VelStool", "kodomor_purple", "eldigo", "beeteros_blue", "Spector_gh_red", "truffle_blue" };

	protected override void Load()
	{
		// Liaison Officer Mardas
		//-------------------------------------------------------------------------
		AddConditionalNpc(151077, L("Liaison Officer Mardas"), "KATYN_10_NPC_01", "f_katyn_10", 3710.09, -1275.39, 90, c => !c.Quests.Has(Mq10), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Liaison Officer Mardas"));
			dialog.SetPortrait("Dlg_port_Mardas");

			if (character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("There were no demons and only other monsters? Oh, I'm glad..."));
				await dialog.Msg(L("Even so, you don't look like you are hurt... it seems that you are stronger than I expected. I see you differently now."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("I... didn't expect to see someone here.. You better escape as well."));

				var answer = await dialog.SelectQuestOffer(Mq01, L("The place is full of demons. Just moments ago, the demons were controlling the monsters in the Bastymosi Field."),
					Option(L("I'm curious, I'll have a look"), "accept"),
					Option(L("Chicken out"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);

					await dialog.Msg(L("Why there? Are you insane? They are demons."));
					await dialog.Msg(L("I don't know how strong you are, for you to do such a dangerous thing... I am not moving an inch from this very spot. If you wanna go, go alone."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("The light bead, I saw it too. The demons are collecting them all over the place."));

				var answer = await dialog.SelectQuestOffer(Mq02, L("The Owl Sculptures are sending them off to somewhere... Perhaps, you could find its trail from the Owl Sculpture you've just seen, can you?"),
					Option(L("I will follow it"), "accept"),
					Option(L("About the Owl Sculpture"), "explain"),
					Option(L("Looks dangerous. I'm out"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Ah, I know about that as well. It's famous you know, the Owl Sculpture..."));
					await dialog.Msg(L("When people die, their spirits are left behind you know. It's said that the Owl Sculpture leads them back to the goddesses."));
					await dialog.Msg(L("My grandmother told me that a sculptor crafted it on the orders of Goddess Ausrine. She said that he received eternal life in return."));
					await dialog.Msg(L("I heard that he's still making those Owl Sculptures... I don't know why that sculpture is like that since it was most likely crafted by the same sculptor."));
					return;
				}

				if (answer == "accept")
				{
					var asked = await character.TimeActions.StartAsync(L("Asking about the light bead"), L("Cancel"), "TALK", TimeSpan.FromSeconds(3));
					if (asked != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq02);

					await dialog.Msg(L("If it were anyone else, I would have stopped you but to you, words of warning will only fall flat. Not sure if it is bravery or recklessness..."));
					await dialog.Msg(L("I should get some rest here for a little bit. I'm running out of breath. It's dangerous for me to stay with you, because I don't want to risk my life."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("I have been chased from Delmore Castle by the demons. I refuse to be in the same situation ever again."));
				character.Quests.ClearQuestTrack(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("I don't know about the identity of the light bead. However, it does not have that same feeling that the demons produce."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			if (character.Quests.HasCompleted(Mq02))
			{
				await dialog.Msg(L("I can't tell if you're brave or just reckless... I feel like I'm in danger just being around you."));
				return;
			}

			if (character.Quests.HasCompleted(Mq01))
			{
				await dialog.Msg(L("I don't want to stay here either. I'm just going to rest for a while and leave right away."));
				return;
			}

			await dialog.Msg(L("Hey! Stop wandering around and run, now! This place is full of demons!"));
		});

		AddConditionalNpc(151077, L("Liaison Officer Mardas"), "KATYN_10_NPC_01_AFTER", "f_katyn_10", 2025, -535, 270, c => c.Quests.Has(Mq10) && !c.Quests.Has(Katyn12Mq04), async dialog =>
		{
			dialog.SetTitle(L("Liaison Officer Mardas"));
			dialog.SetPortrait("Dlg_port_Mardas");

			await dialog.Msg(L("I have nothing left to lose. I don't want to run away anymore."));
			await dialog.Msg(L("Please, I beg you. I don't want to see the Divine Tree take the spirits of our people..."));
		});

		// Bastymosi Field
		//-------------------------------------------------------------------------
		AddQuestTrigger("KATYN_10_MQ_01_TRIGGER", "f_katyn_10", 2700.16, -1447.02, 150, this.BastymosiField);
		AddQuestTrigger("KATYN_10_MQ_01_TRIGGER_2", "f_katyn_10", 2890.51, -1240.78, 150, this.BastymosiField);
		AddQuestTrigger("KATYN_10_MQ_01_TRIGGER_3", "f_katyn_10", 2424.56, -1525.10, 150, this.BastymosiField);

		// Bonewide Cliff
		//-------------------------------------------------------------------------
		AddQuestTrigger("KATYN_10_MQ_03_TRIGGER", "f_katyn_10", 2340.01, -521.91, 120, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
				character.Quests.Start(Mq03);

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
				character.Quests.StartQuestTrack(Mq03);

			await Task.CompletedTask;
		});

		// Owl Chief Sculpture
		//-------------------------------------------------------------------------
		AddConditionalNpc(20135, L("Owl Chief Sculpture"), "KATYN_10_NPC_02", "f_katyn_10", 1942.89, -530.45, 91, c => c.Quests.HasCompleted(Mq02), this.OwlChief);

		// The tomb of the unknown soldier, where the altar's crystal lies
		//-------------------------------------------------------------------------
		AddQuestTrigger("KATYN_10_MQ_04_TRIGGER", "f_katyn_10", 4507.03, -867.50, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The Guiding Owl's Will is reacting strongly. Use it here."), 5);

			await Task.CompletedTask;
		});

		// Karolis Altar
		//-------------------------------------------------------------------------
		AddNpc(46213, L("Karolis Altar"), "KATYN_10_OBJ_01", "f_katyn_10", 4529.43, 575.88, 90, async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
				return;

			var restored = await character.TimeActions.StartAsync(L("Restoring the Karolis Altar"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(5));
			if (restored != TimeActionResult.Completed)
				return;

			character.Inventory.RemoveItem(ItemId.KATYN_10_MQ_04_ITEM_2, character.Inventory.CountItem(ItemId.KATYN_10_MQ_04_ITEM_2));
			character.Inventory.RemoveItem(ItemId.KATYN_10_MQ_05_ITEM, character.Inventory.CountItem(ItemId.KATYN_10_MQ_05_ITEM));

			dialog.Npc.PlayEffect("F_ground139_light_green", 1.5f);
			character.Quests.CompleteObjective(Mq06, "restoreAltar");
		});

		// Karolis' Lights around the Altar of the Forest
		//-------------------------------------------------------------------------
		for (var i = 0; i < Lights.GetLength(0); ++i)
		{
			var number = i + 1;

			AddConditionalNpc(147469, L("Karolis' Light"), "KATYN_10_OBJ_02_" + number, "f_katyn_10", Lights[i, 0], Lights[i, 1], 90,
				character => character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07),
				async dialog =>
				{
					await this.LightKarolis(dialog, number);
				});
		}

		// Owl Sculptures
		//-------------------------------------------------------------------------
		for (var i = 0; i < Owls.GetLength(0); ++i)
		{
			var number = i + 1;

			AddNpc((int)Owls[i, 0], L("Owl Sculpture"), "KATYN_10_NPC_03_" + number, "f_katyn_10", Owls[i, 1], Owls[i, 2], 0, async dialog =>
			{
				await this.OwlSculpture(dialog, number);
			});
		}

		// Mind Control Tower at Duokliu Hall
		//-------------------------------------------------------------------------
		AddConditionalNpc(147414, L("Mind Control Tower"), "KATYN_10_OBJ_03", "f_katyn_10", 1232.32, 449.71, 0, c => !IsTowerDestroyed(c), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq09) || character.Quests.IsCompletable(Mq09))
				return;

			dialog.Npc.PlayEffect("F_explosion014", 2f);
			character.Quests.CompleteObjective(Mq09, "destroyTower");
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The Mind Control Tower has been destroyed. Return to the Owl Chief Sculpture."), 5);
			character.LookAround();

			await Task.CompletedTask;
		});

		AddConditionalNpc(147501, L("Mind Control Tower"), "KATYN_10_OBJ_03_AFTER", "f_katyn_10", 1232.32, 449.71, 0, IsTowerDestroyed);

		// Karolis Cradle
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Energy of Karolis Springs"), "KATYN_10_OBJ_04", "f_katyn_10", -980, -340, 90, c => c.Quests.IsActive(Mq11) && !c.Quests.IsCompletable(Mq11), async dialog =>
		{
			var character = dialog.Player;

			if (!character.Quests.IsActive(Mq11) || character.Quests.IsCompletable(Mq11))
				return;

			var gathered = await character.TimeActions.StartAsync(L("Gathering the energy of Karolis Springs..."), L("Cancel"), "ABSORB", TimeSpan.FromSeconds(5));
			if (gathered != TimeActionResult.Completed)
				return;

			character.PlayEffect("F_buff_basic009_blue", 1f);
			character.Quests.CompleteObjective(Mq11, "gatherEnergy");
		});

		// Locked Chest
		//-------------------------------------------------------------------------
		AddConditionalNpc(147392, L("Locked Chest"), "KATYN_10_SQ_NPC_01", "f_katyn_10", 2540.18, 736.49, 90, c => !c.Quests.HasCompleted(Sq01), async dialog =>
		{
			var character = dialog.Player;

			if (character.Quests.IsCompletable(Sq01))
			{
				dialog.PlayAnimation("KICKBOX");
				dialog.Npc.PlayEffect("F_ground139_light_green", 0.5f);
				character.ServerMessage(L("The chest springs open."));
				await dialog.CompleteQuest(Sq01);

				if (character.Quests.HasCompleted(Sq01))
					character.LookAround();
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var looked = await character.TimeActions.StartAsync(L("Observing"), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(2));
				if (looked != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq01);
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("An unknown force is keeping the chest shut!{nl}Destroy the monsters nearby to feed it magical power"), 10);
				return;
			}

			if (character.Quests.IsActive(Sq01))
				character.ServerMessage(L("An unknown force is keeping the chest shut."));
		});

		// Gloomy Owl Sculpture
		//-------------------------------------------------------------------------
		AddNpc(48004, L("Gloomy Owl Sculpture"), "KATYN10_RP_1_NPC", "f_katyn_10", -319.79, -473.18, 3, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Gloomy Owl Sculpture"));

			if (character.Quests.IsCompletable(Rp1))
			{
				await dialog.Msg(L("Thank you. I pray that they rest in peace with the goddess..."));
				await dialog.CompleteQuest(Rp1);
				return;
			}

			if (!character.Quests.Has(Rp1) && character.Quests.MeetsPrerequisites(Rp1))
			{
				var answer = await dialog.SelectQuestOffer(Rp1, L("Help! My spirits were taken away by monsters... Please save them!"),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I'm sorry"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Rp1);

				return;
			}

			if (character.Quests.IsActive(Rp1))
			{
				await dialog.Msg(L("You need to observe the monsters carefully to find them..."));
				return;
			}

			await dialog.Msg(L("I'm sorry... I'm so sorry..."));
		});

		// Graves around the springs, where the lost spirit's ring lies
		//-------------------------------------------------------------------------
		for (var i = 0; i < Tombs.GetLength(0); ++i)
		{
			var number = i + 1;

			AddQuestTrigger("HT_F_KATYN_12_TOMB_" + number, "f_katyn_10", Tombs[i, 0], Tombs[i, 1], 40, async args =>
			{
				if (args.Initiator is Character character)
					this.VisitTomb(character, number);

				await Task.CompletedTask;
			});
		}
	}

	/// <summary>
	/// Starts the ambush at Bastymosi Field.
	/// </summary>
	private async Task BastymosiField(TriggerActorArgs args)
	{
		if (args.Initiator is not Character character)
			return;

		if (character.Quests.IsActive(Mq01) && !character.Quests.IsCompletable(Mq01))
			character.Quests.StartQuestTrack(Mq01);

		await Task.CompletedTask;
	}

	/// <summary>
	/// Returns whether the character destroyed the Mind Control Tower.
	/// </summary>
	private static bool IsTowerDestroyed(Character character)
		=> character.Quests.IsCompletable(Mq09) || character.Quests.HasCompleted(Mq09);

	/// <summary>
	/// The Owl Chief Sculpture's dialog.
	/// </summary>
	private async Task OwlChief(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Owl Chief Sculpture"));
		dialog.SetPortrait("Dlg_port_owlseer");

		if (character.Quests.IsCompletable(Mq03))
		{
			await dialog.Msg(L("Thank you. Thank you so much."));
			await dialog.Msg(L("If I went under the demons' control, what could have happened to those poor souls..."));
			await dialog.CompleteQuest(Mq03);
			return;
		}

		if (character.Quests.IsCompletable(Mq06))
		{
			await dialog.Msg(L("I've been waiting for you. I knew you succeeded when I sensed that the energy of the forest had strengthened."));
			await dialog.CompleteQuest(Mq06);
			return;
		}

		if (character.Quests.IsCompletable(Mq07))
		{
			await dialog.Msg(L("You did it! You spread the energy around. It seems that all the other owls are now back to normal."));
			await dialog.Msg(L("Now, the only thing left to do is to destroy that mind-altering abomination. Our safety is now guaranteed."));
			await dialog.CompleteQuest(Mq07);
			return;
		}

		if (character.Quests.IsCompletable(Mq08))
		{
			await dialog.Msg(L("Did any of the owls mention something rude? They all are deeply grateful for what you have done."));
			await dialog.Msg(L("Now, I will combine their powers along with mine and make them into one."));
			await dialog.CompleteQuest(Mq08);

			if (character.Quests.HasCompleted(Mq08))
				dialog.Npc.PlayEffect("F_light015_violet1", 2f);
			return;
		}

		if (character.Quests.IsCompletable(Mq09))
		{
			await dialog.Msg(L("You've destroyed the Mind Control Tower as well? Now, peace will finally return."));
			await dialog.Msg(L("That's... that's a great relief."));
			await dialog.CompleteQuest(Mq09);
			return;
		}

		if (character.Quests.IsCompletable(Mq10))
		{
			await dialog.Msg(L("Mardas is the one who escaped from Delmore Castle. The souls we have seen before must be from there as well."));
			await dialog.Msg(L("I have no idea why such an atrocity has occurred in Delmore Castle... These souls, as I have told you previously, do not possess any memories and will of their own."));
			await dialog.Msg(L("All that is left is the energy of a pure soul. This is definitely not an ordinary occurrence."));
			await dialog.Msg(L("It seems some magical power is draining the life force out of living humans to extract souls... I have lived in this forest for many years but this is unheard of."));
			await dialog.Msg(L("I have explained it to this man... The souls are being collected at Letas Stream and offered to the Divine Tree."));
			await dialog.Msg(L("The owls have sent the souls to that direction. We have to at least stop them. But Letas Stream has fallen into the hands of the demons."));
			await dialog.Msg(L("Could we ask you to help us a bit longer? You are our only hope...."));
			await dialog.CompleteQuest(Mq10);
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			await dialog.Msg(L("The light bead you saw is actually... a human soul. No memory and will of its own. A soul in its purest form. An essence of a soul."));
			await dialog.Msg(L("These must be those who have lived around here... Even if they all perished by the demons, it is very strange that they have become such pure souls."));
			await dialog.Msg(L("We, owls, lead the souls to the arms of the goddess. It's a divine duty that Goddess Ausrine gave us."));
			await dialog.Msg(L("But suddenly the demons invaded the forest, corrupting it in the process. Then they controlled us."));
			await dialog.Msg(L("The controlled owls started to send the souls to wherever the demons told them to."));
			await dialog.Msg(L("I am fully aware of how shameless of me to ask you this. But, I cannot bear to see another soul getting sacrificed."));

			var answer = await dialog.SelectQuestOffer(Mq04, L("Please, destroy the Mind Control Tower."),
				Option(L("Don't worry, I will do it"), "accept"),
				Option(L("That sounds dangerous"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				character.Inventory.Add(ItemId.KATYN_10_MQ_04_ITEM, 1, InventoryAddType.PickUp);

				await dialog.Msg(L("The goddess has not forsaken us after all.. She sent you to us!"));
				await dialog.Msg(L("The ripple damage from destroying the Mind Control Tower might harm the owls. Please, fill my comrades with the energy of the forest before you destroy the Mind Control Tower."));
				await dialog.Msg(L("Please, gather the fragments of the Karolis Altar destroyed by the demons. Restoring the altar will help to recover some of the forest."));
				await dialog.Msg(L("There must be a tomb of an unknown soldier here somewhere... The will of the Guide Owl will help you find the fragments of the ruined Karolis Altar."));
			}
			return;
		}

		if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
		{
			await dialog.Msg(L("I should not have held my hopes up too high... Restoring the Karolis Altar was not enough to fully revive the forest."));
			await dialog.Msg(L("The Altar of the Forest needs to be revived as well. Ah, the Altar of the Forest is located near the pond across there."));
			await dialog.Msg(L("It spreads Karolis' lights throughout the forest. The lights are almost all out..."));

			var answer = await dialog.SelectQuestOffer(Mq07, L("If you can reignite them, the power of the altar will spread out!"),
				Option(L("I'll spread the energy from the sanctuary"), "accept"),
				Option(L("I think it's no use"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq07);
				character.LookAround();

				await dialog.Msg(L("There are four Karolis' lights. By illuminating only one light, it will lose its light quickly, but illuminating all four will maintain the altar's power."));
				await dialog.Msg(L("Be sure to illuminate them all before they give out."));
			}
			return;
		}

		if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
		{
			await dialog.Msg(L("The tower that has been controlling the owls is filled with very malicious energy. Even you should not touch it without any precaution."));
			await dialog.Msg(L("The rest of the owls have regained themselves. And they are eager to lend you their power."));

			var answer = await dialog.SelectQuestOffer(Mq08, L("Small individually but when collected, strong enough to fend off the malicious force the tower contains."),
				Option(L("Lend me your power"), "accept"),
				Option(L("I don't think it's necessary"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= Owls.GetLength(0); ++i)
					character.Variables.Perm.Set(OwlVar + i, false);
				character.Variables.Perm.SetInt(OwlCountVar, 0);

				character.Quests.Start(Mq08);

				await dialog.Msg(L("Speak with the owls you have come across on the way here, they want to thank you personally."));
				await dialog.Msg(L("Explain the situation to them and they will surely lend their power."));
			}
			return;
		}

		if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
		{
			await dialog.Msg(L("I was fearful of our combined power not being strong enough... I am glad to be wrong. It is certainly strong."));

			var answer = await dialog.SelectQuestOffer(Mq09, L("Now, use this power to blow the tower in Duokliu Hall to smithereens!"),
				Option(L("I will blow it up"), "accept"),
				Option(L("I'm not ready yet"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq09);

				await dialog.Msg(L("I am sorry for not being able to repay you for all that you've done for us."));
				await dialog.Msg(L("However, what I can say for sure is that you certainly are being led by the goddess... I have that feeling."));
			}
			return;
		}

		if (!character.Quests.Has(Mq10) && character.Quests.MeetsPrerequisites(Mq10))
		{
			await dialog.Msg(L("Someone named Mardas came here to speak to you."));
			await dialog.Msg(L("Mardas has been talking about what he saw. The details on what the demons were doing."));

			character.Quests.Start(Mq10);
			character.LookAround();
			return;
		}

		if (!character.Quests.Has(Mq11) && character.Quests.MeetsPrerequisites(Mq11))
		{
			await dialog.Msg(L("Perhaps if Mardas and I can combine both of our knowledges on what we know about the demons, we can find a solution. We will be planning here."));
			await dialog.Msg(L("First, we need the Namott of Suppression. It contains a high level of divine energy from the forest. The Guide Owl near Letas Stream can make you one."));
			await dialog.Msg(L("But seeing how it is not responding to my call, it must still be under the demons' control."));

			var answer = await dialog.SelectQuestOffer(Mq11, L("Please, find the Guide Owl quickly. When it regains itself, tell it that you need the Namott of Suppression."),
				Option(L("I'm off to save the Guide Owl Sculpture"), "accept"),
				Option(L("I'll leave the end to Mardas and go"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq11);
				character.LookAround();

				await dialog.Msg(L("Karolis Cradle is a spot enriched with the energy of the forest. Stand there and the energy will come to you."));
				await dialog.Msg(L("As the energy concentration increases, it gets condensed and crystalized. Take the crystalized energy to the Guide Owl."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq04) || character.Quests.IsActive(Mq05) || character.Quests.IsActive(Mq06))
		{
			await dialog.Msg(L("The Karolis Altar used to pump energy to the forest. Ever since it was destroyed, the energy of the forest was reduced quite significantly."));
			await dialog.Msg(L("If both the Karolis Altar and the Forest Sanctuary are restored, my comrades will be back to normal. We, the owls, share lives with the forest."));
			return;
		}

		if (character.Quests.IsActive(Mq07))
		{
			await dialog.Msg(L("Back in the yesteryears, there were numerous altars but now, there is only one. The demons did indeed ruin the forest and us."));
			return;
		}

		if (character.Quests.IsActive(Mq08))
		{
			await dialog.Msg(L("After getting the powers from all the owls, come back to me. I will combine them together."));
			return;
		}

		if (character.Quests.IsActive(Mq09))
		{
			await dialog.Msg(L("Actually, there is one owl a bit far from here... It's not responding."));
			await dialog.Msg(L("I am worried about that child but.. The crisis on our hand requires our full attention."));
			return;
		}

		if (character.Quests.IsActive(Mq10))
		{
			character.Quests.ReplayQuestTrack(Mq10);
			return;
		}

		if (character.Quests.IsActive(Mq11))
		{
			await dialog.Msg(L("Mardas has told me about the atrocities that the demons did. Even from just the descriptions, it puts a cold spear through my heart. We cannot allow it to continue."));
			await dialog.Msg(L("O, the poor souls... they are the ones whom we handed over to the demons? I am made of wood but my wooden heart is aching."));
			return;
		}

		if (character.Quests.HasCompleted(Katyn12Mq10))
		{
			await dialog.Msg(L("Thank you for saving this forest. I'll take care of the spirits that escaped and make sure they find their way back to the goddess."));
			await dialog.Msg(L("And that red gem Mardas saw... It must be somehow connected to what happened to the spirits."));
			await dialog.Msg(L("From what I heard the water at the Pelke Shrine Ruins turned red... I wonder if that has something to do with that gem?"));
			return;
		}

		if (character.Quests.HasCompleted(Mq11))
		{
			await dialog.Msg(L("With my knowledge of demons and the things Mardas saw, we might just be able to find a solution."));
			await dialog.Msg(L("I'm counting on you not to let our souls be taken by the Divine Tree."));
			return;
		}

		if (character.Quests.HasCompleted(Mq03))
		{
			await dialog.Msg(L("I don't know how much longer I can hold on..."));
			return;
		}

		await dialog.Msg(L("I cannot let the demons get me but..."));
	}

	/// <summary>
	/// Lights one of Karolis' lights, which only hold while all four are
	/// lit within a short time of each other.
	/// </summary>
	private async Task LightKarolis(Dialog dialog, int number)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq07) || character.Quests.IsCompletable(Mq07))
			return;

		character.Variables.Temp.SetLong(LightVar + number, DateTime.Now.Ticks);
		dialog.Npc.PlayEffect("F_light018_yellow", 1f);

		var lit = 0;
		for (var i = 1; i <= Lights.GetLength(0); ++i)
		{
			var litAt = character.Variables.Temp.GetLong(LightVar + i, 0);
			if (litAt != 0 && DateTime.Now - new DateTime(litAt) < LightDuration)
				lit++;
		}

		character.ServerMessage(LF("Karolis' Lights lit: {0}/{1}", lit, Lights.GetLength(0)));

		if (lit >= Lights.GetLength(0))
		{
			character.Quests.CompleteObjective(Mq07, "lightLights");
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The energy from the Karolis Sanctuary is spreading out."), 5);
			character.LookAround();
		}

		await Task.CompletedTask;
	}

	/// <summary>
	/// One of the Owl Sculptures freed from the demons' control, which
	/// lends its power while the Owl Chief gathers it.
	/// </summary>
	private async Task OwlSculpture(Dialog dialog, int number)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Owl Sculpture"));

		if (character.Quests.IsActive(Mq08) && !character.Variables.Perm.GetBool(OwlVar + number, false))
		{
			switch (number)
			{
				case 1: await dialog.Msg(L("Are you the one who rescued us? Then we shall lend you our power.")); break;
				case 2: await dialog.Msg(L("Thank you for saving us from such a daunting ordeal. Take some of my power as well.")); break;
				case 3: await dialog.Msg(L("How hard it has been... If it weren't for you, we would've spent eternity working. Thank you. Let me share you my power.")); break;
				case 4: await dialog.Msg(L("Take this power. If this awful tower can be destroyed, I will do anything.")); break;
				case 5: await dialog.Msg(L("I trust you won't just take our power and run away with it? We are only sharing it with you because it's what the Owl Chief commanded.")); break;
				default: await dialog.Msg(L("It hasn't been long since I came back to my senses, I'm afraid this is all the power I have... I hope you will find it useful.")); break;
			}

			character.Variables.Perm.Set(OwlVar + number, true);
			var gathered = character.Variables.Perm.GetInt(OwlCountVar, 0) + 1;
			character.Variables.Perm.SetInt(OwlCountVar, gathered);

			dialog.Npc.PlayEffect("F_light015_violet1", 1f);
			character.ServerMessage(LF("Owl powers gathered: {0}/{1}", Math.Min(gathered, Owls.GetLength(0)), Owls.GetLength(0)));
			return;
		}

		if (!character.Quests.HasCompleted(Mq07))
		{
			await dialog.Msg(L("(There is no reaction.)"));
			return;
		}

		switch (number)
		{
			case 1: await dialog.Msg(L("Thank you for saving me. I hope my power will be of use to you, even if by only a little...")); break;
			case 2: await dialog.Msg(L("That was unbearable... If it wasn't for you I would've stayed like that forever.")); break;
			case 3: await dialog.Msg(L("I'll do anything, if only I can stand up to the demons. Thank you for saving me!")); break;
			case 4: await dialog.Msg(L("I'm still a bit confused. I just can't believe what I was doing.")); break;
			case 5: await dialog.Msg(L("To think I was helping the demons instead of guiding spirits to the goddess... I feel terrible, even if that wasn't my intention.")); break;
			default: await dialog.Msg(L("How sad is the Dievdirbys who made us going to be when they find out...? I need to be more focused from now on...")); break;
		}
	}

	/// <summary>
	/// Pays respects at one of the graves, one of which holds the lost
	/// spirit's ring.
	/// </summary>
	private void VisitTomb(Character character, int number)
	{
		if (!character.Quests.HasCompleted(Katyn12Mq10) || character.Quests.Has(Katyn12Hq1) || character.Quests.HasCompleted(Katyn12Hq1))
			return;

		if (character.Inventory.CountItem(ItemId.KATYN12_HIDDENQ1_ITEM) > 0)
			return;

		if (character.Variables.Temp.GetBool(TombVar + number, false))
			return;

		character.Variables.Temp.Set(TombVar + number, true);

		var ringTomb = character.Variables.Perm.GetInt(RingTombVar, 0);
		if (ringTomb == 0)
		{
			ringTomb = GameRandom.Get().Next(1, Tombs.GetLength(0) + 1);
			character.Variables.Perm.SetInt(RingTombVar, ringTomb);
		}

		if (number == ringTomb)
		{
			character.Inventory.Add(ItemId.KATYN12_HIDDENQ1_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(L("There is a ring lying by the grave."));
			return;
		}

		string[] prayers =
		{
			L("Rest in peace..."),
			L("Are the owners of these graves with the goddess now?"),
			L("May your spirits find eternal rest."),
			L("Are all the spirits here resting in peace?"),
			L("The owls must have guided them to the goddess."),
			L("May you rest in peace with the goddess."),
		};

		character.ServerMessage(prayers[GameRandom.Get().Next(prayers.Length)]);
	}

	/// <summary>
	/// Points the Guiding Owl's Will towards the Karolis Altar's crystal,
	/// and at the monsters carrying the altar's fragments.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_KATYN_10_MQ_04_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "f_katyn_10")
		{
			character.ServerMessage(L("The Guiding Owl's Will does not react here."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
		{
			var distance = character.Position.Get2DDistance(CrystalSpot);

			if (distance <= 200)
			{
				character.PlayEffect("F_light018_yellow", 1f);
				character.Inventory.Add(ItemId.KATYN_10_MQ_04_ITEM_2, 1, InventoryAddType.PickUp);
				return ItemUseResult.OkayNotConsumed;
			}

			if (distance <= 1000)
				character.ServerMessage(L("The Guiding Owl's Will is glowing brightly. The crystal must be close."));
			else if (distance <= 2500)
				character.ServerMessage(L("The Guiding Owl's Will is glowing faintly. Look for the tomb of the unknown soldier."));
			else
				character.ServerMessage(L("The Guiding Owl's Will barely reacts. The crystal is far from here."));

			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
		{
			var target = character.Map.GetAttackableEnemiesInPosition(character, character.Position, 300)
				.FirstOrDefault(entity => entity is Mob mob && SpiritMonsters.Contains(mob.Data.ClassName));

			if (target == null)
			{
				character.ServerMessage(L("The Guiding Owl's Will does not sense any fragments nearby."));
				return ItemUseResult.OkayNotConsumed;
			}

			target.PlayEffect("F_light015_violet1", 1f);
			character.ServerMessage(L("The Guiding Owl's Will reacts to a nearby monster. Defeat it to recover the fragments."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.ServerMessage(L("The Guiding Owl's Will does not react."));
		return ItemUseResult.OkayNotConsumed;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 30049: Messenger Running Away
//-----------------------------------------------------------------------------
public class Katyn10Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30049);
		SetName(L("Messenger Running Away"));
		SetDescription(L("Mardas told you to run because the surrounding areas are filled with demons. Go to Bastymosi Field and see what's really happening."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_01", "f_katyn_10", L("Speak with Mardas"), L("Mardas seems to be chased by something. Listen to what Mardas has got to say."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_MQ_01_TRIGGER", "f_katyn_10", L("Investigate Bastymosi Field"), L("Mardas told you to run because the surrounding areas are filled with demons. Go to Bastymosi Field and see what's really happening."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_01", "f_katyn_10", L("Speak with Mardas"), L("The monsters seem to be using the Owl Sculptures to send the light beads to somewhere. Ask Mardas if he knows anything about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "KATYN_10_MQ_01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(50144, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(47));

		AddObjective("killDigos", L("Defeat the monsters that are rushing in"), new KillObjective(5, "digo") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 300));
	}
}

// 30050: Following the Light
//-----------------------------------------------------------------------------
public class Katyn10Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30050);
		SetName(L("Following the Light"));
		SetDescription(L("Mardas says that the demons are gathering the light bead and getting them to somewhere else. Follow the light bead."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_01", "f_katyn_10", L("Speak with Mardas"), L("Ask Mardas if he's ever seen the Owl Sculptures sending light beads anywhere."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_NPC_01", "f_katyn_10", L("Follow the light bead from the Owl Sculpture"), L("Mardas says that the demons are gathering the light bead and getting them to somewhere else. Follow the light bead."));
		SetPhase(QuestStatus.Success, "KATYN_10_MQ_03_TRIGGER", "f_katyn_10", L("Follow the light bead from the Owl Sculpture"), L("Mardas says that the demons are gathering the light bead and getting them to somewhere else. Follow the light bead."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "KATYN_10_MQ_02_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30049, QuestStatus.Completed));

		AddObjective("followLight", L("Follow the light bead from the Owl Sculpture"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 300));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The light bead is the quest; there is no turn-in NPC.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The light bead disappeared over the slope!{nl}Turn around and go up Bonewide Cliff."), 5);
		character.LookAround();
	}
}

// 30051: Owl Sculpture in Danger
//-----------------------------------------------------------------------------
public class Katyn10Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30051);
		SetName(L("Owl Sculpture in Danger"));
		SetDescription(L("The light beads were delivered to the Owl Sculptures on top of a slope. Turn around and follow them to Bonewide Cliff."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_MQ_03_TRIGGER", "f_katyn_10", L("Follow the light bead to Bonewide Cliff"), L("The light beads were delivered to the Owl Sculptures on top of a slope. Turn around and follow them to Bonewide Cliff."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_MQ_03_TRIGGER", "f_katyn_10", L("Follow the light bead to Bonewide Cliff"), L("The light beads were delivered to the Owl Sculptures on top of a slope. Turn around and follow them to Bonewide Cliff."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("Saved the Owl Chief Sculture. It seems that it is capable of talking. Listen to what it says."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "KATYN_10_MQ_03_TRACK", "m_boss_b", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(30050, QuestStatus.Completed));

		AddObjective("killMoa", L("Defeat Moa who is after the Owl Sculpture"), new KillObjective(1, "boss_moa_Q3") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 300));
		AddReward(new SelectItemReward("HAND02_169", "HAND02_170", "HAND02_171"));
	}
}

// 30052: Recover the Karolis Altar (1)
//-----------------------------------------------------------------------------
public class Katyn10Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30052);
		SetName(L("Recover the Karolis Altar (1)"));
		SetDescription(L("To save the Owl Sculptures possessed by the demons you must restore the energy of Karolis Forest. To do that, you'll have to reconstruct the Karolis Altar destroyed by the demons. Follow the will of the leading owls and find the Karolis Altar Crystal."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("The Owl Chief Sculpture, unlike the others, is in its right mind. Ask if it remembers what happened to Karolis Springs."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_MQ_04_TRIGGER", "f_katyn_10", L("Retrieve the Karolis Altar Crystal"), L("To save the Owl Sculptures possessed by the demons you must restore the energy of Karolis Forest. To do that, you'll have to reconstruct the Karolis Altar destroyed by the demons. Follow the will of the leading owls and find the Karolis Altar Crystal."));
		SetPhase(QuestStatus.Success, "KATYN_10_MQ_04_TRIGGER", "f_katyn_10", L("Retrieve the Karolis Altar Crystal"), L("To save the Owl Sculptures possessed by the demons you must restore the energy of Karolis Forest. To do that, you'll have to reconstruct the Karolis Altar destroyed by the demons. Follow the will of the leading owls and find the Karolis Altar Crystal."));

		AddPrerequisite(new QuestStatusPrerequisite(30051, QuestStatus.Completed));

		AddObjective("findCrystal", L("Use the Guide Owl's Will to find Karolis Altar Crystal"), new CollectItemObjective("KATYN_10_MQ_04_ITEM_2", 1));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 300));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The crystal's recovery ends the quest and opens the hunt for the fragments.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("You found the Karolis Altar Crystal{nl}Now, it's time to find the fragments from the Karolis Altar"), 5);

		var fragmentsQuestId = new QuestId(30053);
		if (!character.Quests.Has(fragmentsQuestId))
			character.Quests.Start(fragmentsQuestId);
	}
}

// 30053: Recover the Karolis Altar (2)
//-----------------------------------------------------------------------------
public class Katyn10Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30053);
		SetName(L("Recover the Karolis Altar (2)"));
		SetDescription(L("The monsters have the fragments from the broken Karolis Altar. Defeat the monsters that have the fragments and collect them all."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Retrieve Karolis Altar Fragments from monsters"), L("The monsters have the fragments from the broken Karolis Altar. Defeat the monsters that have the fragments and collect them all."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_OBJ_01", "f_katyn_10", L("Retrieve Karolis Altar Fragments from monsters"), L("The monsters have the fragments from the broken Karolis Altar. Defeat the monsters that have the fragments and collect them all."));
		SetPhase(QuestStatus.Success, "KATYN_10_OBJ_01", "f_katyn_10", L("Retrieve Karolis Altar Fragments from monsters"), L("The monsters have the fragments from the broken Karolis Altar. Defeat the monsters that have the fragments and collect them all."));

		AddPrerequisite(new QuestStatusPrerequisite(30052, QuestStatus.Completed));

		AddPityDrop("KATYN_10_MQ_05_ITEM", 0.25f, 8, 1, "digo", "VelStool", "kodomor_purple", "eldigo", "beeteros_blue", "Spector_gh_red", "truffle_blue");

		AddObjective("collectFragments", L("Retrieve Karolis Altar Fragment from the monsters by using the Guide Owl's Will"), new CollectItemObjective("KATYN_10_MQ_05_ITEM", 10));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 300));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The last fragment ends the quest and sends the player to the altar.
		character.Quests.Complete(this.QuestId);

		var altarQuestId = new QuestId(30054);
		if (!character.Quests.Has(altarQuestId))
			character.Quests.Start(altarQuestId);
	}
}

// 30054: Recover the Karolis Altar (3)
//-----------------------------------------------------------------------------
public class Katyn10Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30054);
		SetName(L("Recover the Karolis Altar (3)"));
		SetDescription(L("Restore the altar on the remains of the altar."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_OBJ_01", "f_katyn_10", L("Recover the Karolis Altar"), L("Restore the altar on the remains of the altar."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_OBJ_01", "f_katyn_10", L("Recover the Karolis Altar"), L("Restore the altar on the remains of the altar."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Report to the Owl Chief Sculpture"), L("You've successfully restored Karolis Altar. Report back to the Owl Chief Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30053, QuestStatus.Completed));

		AddObjective("restoreAltar", L("Recover the Karolis Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Vis", 300));
		AddReward(new ItemReward("Drug_SP1_Q", 45));
		AddReward(new TakeItemReward("KATYN_10_MQ_04_ITEM", -1));
	}
}

// 30055: Altar of the Forest
//-----------------------------------------------------------------------------
public class Katyn10Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30055);
		SetName(L("Altar of the Forest"));
		SetDescription(L("To spread out the forest's divine force, you need to find and light up all of Karolis' light rays. Hurry and light up all four of Karolis' rays before their light dies out."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("It is not enough to revive the Karolis Springs back to life. Speak with Owl Chief further."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_OBJ_02_1", "f_katyn_10", L("Illuminate Karolis' light"), L("To spread out the forest's divine force, you need to find and light up all of Karolis' light rays. Hurry and light up all four of Karolis' rays before their light dies out."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("The energy from the Karolis Sanctuary is spreading out. Report back to the Owl Chief Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30054, QuestStatus.Completed));

		AddObjective("lightLights", L("Illuminate Karolis' light"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
	}
}

// 30056: Gather the Strength of the Owl (1)
//-----------------------------------------------------------------------------
public class Katyn10Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30056);
		SetName(L("Gather the Strength of the Owl (1)"));
		SetDescription(L("The Owl Chief Sculpture says that to ensure the utter destruction of the Mind Control Tower, the collected power from the Owl Sculptures are needed. Find the owl sculptures and get their power."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("The Altar is restored. Ask the Owl Chief if it worked."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_NPC_03_1", "f_katyn_10", L("Collect the power of the Owl Sculptures"), L("The Owl Chief Sculpture says that to ensure the utter destruction of the Mind Control Tower, the collected power from the Owl Sculptures are needed. Find the owl sculptures and get their power."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Report to the Owl Chief Sculpture"), L("Collected all the power from the owl sculptures. Report back to the Owl Chief."));

		AddPrerequisite(new QuestStatusPrerequisite(30055, QuestStatus.Completed));

		AddObjective("gatherPower", L("Collect the power of the Owl Sculptures"), new VariableCheckObjective(FKatyn10QuestNpcsScript.OwlCountVar, 6, isPermanent: true));

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
	}
}

// 30057: Gather the Strength of the Owl (2)
//-----------------------------------------------------------------------------
public class Katyn10Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30057);
		SetName(L("Gather the Strength of the Owl (2)"));
		SetDescription(L("Destroy the Mind Control Tower at Duoklu Hall with the power of the Owl Sculptures."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("Even collected the power from the owl chief sculpture. Speak with the Owl Chief Sculpture."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_OBJ_03", "f_katyn_10", L("Destroy the Mind Control Tower at Duoklu Hall"), L("Destroy the Mind Control Tower at Duoklu Hall with the power of the Owl Sculptures."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Report to the Owl Chief Sculpture"), L("The Mind Control Tower has been destroyed. Return to the Owl Chief Sculpture."));

		AddPrerequisite(new QuestStatusPrerequisite(30056, QuestStatus.Completed));

		AddObjective("destroyTower", L("Destroy the Mind Control Tower at Duoklu Hall"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
	}
}

// 30058: Another Owl
//-----------------------------------------------------------------------------
public class Katyn10Mq10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30058);
		SetName(L("Another Owl"));
		SetDescription(L("Mardas is with the Owl Chief Sculpture. Listen to what he was saying."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("Mardas is standing beside the Owl Chief Sculpture. Ask him about what they are discussing."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("Mardas is with the Owl Chief Sculpture. Listen to what he was saying."));
		SetPhase(QuestStatus.Success, "KATYN_10_NPC_02", "f_katyn_10", L("Speak with the Owl Chief Sculpture again"), L("The Owl Chief Sculpture says that it will think of a plan after listening to what Mardas has to say. Meanwhile, ask what you should you do."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "KATYN_10_MQ_10_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(30057, QuestStatus.Completed));

		AddObjective("hearMardas", L("Talk to the Owl Chief Sculpture"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
		AddReward(new SelectItemReward("FOOT02_169", "FOOT02_170", "FOOT02_171"));
	}
}

// 30059: Energy of Karolis Springs
//-----------------------------------------------------------------------------
public class Katyn10Mq11Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30059);
		SetName(L("Energy of Karolis Springs"));
		SetDescription(L("To save the Guide Owl Sculptures at Letas Stream, go to the birthplace of Karolis, where the energy of Karolis Springs is stronger, and collect that energy."));
		SetType(QuestType.Main);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_NPC_02", "f_katyn_10", L("Talk to the Owl Chief Sculpture"), L("The Owl Sculpture is currently coming up with a plan to solve the problem caused by the demons based on the story from Mardas. Ask what you should do while they are discussing."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_OBJ_04", "f_katyn_10", L("Collect the Energy of Karolis Springs"), L("To save the Guide Owl Sculptures at Letas Stream, go to the birthplace of Karolis, where the energy of Karolis Springs is stronger, and collect that energy."));
		SetPhase(QuestStatus.Success, "KATYN_10_OBJ_04", "f_katyn_10", L("Collect the Energy of Karolis Springs"), L("To save the Guide Owl Sculptures at Letas Stream, go to the birthplace of Karolis, where the energy of Karolis Springs is stronger, and collect that energy."));

		AddPrerequisite(new QuestStatusPrerequisite(30058, QuestStatus.Completed));

		AddObjective("gatherEnergy", L("Collect the Energy of Karolis Springs"), new ManualObjective());

		AddReward(new ItemReward("KATYN_10_MQ_11_ITEM", 1));
		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("Vis", 300));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The crystallised energy ends the quest; the Guide Owl is the next stop.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The energy of Karolis Springs has crystallized.{nl}Take it to the Guide Owl Sculpture at Letas Stream."), 8);

		var guideOwlQuestId = new QuestId(30060);
		if (!character.Quests.Has(guideOwlQuestId))
			character.Quests.Start(guideOwlQuestId);
	}
}

// 30071: A Chest Locked By A Spell
//-----------------------------------------------------------------------------
public class Katyn10Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30071);
		SetName(L("A Chest Locked By A Spell"));
		SetDescription(L("The chest is locked with an unknown force. Defeat some monsters around it to fill the chest with magic."));
		SetType(QuestType.Sub);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN_10_SQ_NPC_01", "f_katyn_10", L("Look inside the locked chest"), L("There is a mysterious chest. Open it and look inside."));
		SetPhase(QuestStatus.InProgress, "KATYN_10_SQ_NPC_01", "f_katyn_10", L("Defeat monsters to supply magic to the Locked Chest"), L("The chest is locked with an unknown force. Defeat some monsters around it to fill the chest with magic."));
		SetPhase(QuestStatus.Success, "KATYN_10_SQ_NPC_01", "f_katyn_10", L("Open the locked chest"), L("It seems enough of the spell has been injected to the chest. Try opening the chest again."));

		AddPrerequisite(new LevelPrerequisite(47));

		AddObjective("killMonsters", L("Defeat monsters to supply magic to the Locked Chest"), new KillObjective(10, "digo", "VelStool", "kodomor_purple", "eldigo", "beeteros_blue", "Spector_gh_red", "truffle_blue"));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Vis", 150));
		AddReward(new ItemReward("ORSHA_BOOK_karolis02_BOOK", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 45));
	}
}

// 60164: Spirit-carrying Monster
//-----------------------------------------------------------------------------
public class Katyn10Rp1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60164);
		SetName(L("Spirit-carrying Monster"));
		SetDescription(L("The Gloomy Owl sculpture says that the monsters near Duokliu Hall have swallowed all of the spirits it is supposed to lead. Find and deal with the monsters that have swallowed the spirits to free them."));
		SetType(QuestType.Repeat);
		SetLocation("f_katyn_10");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "KATYN10_RP_1_NPC", "f_katyn_10", L("Talk to the Gloomy Owl Sculpture"), L("The Gloomy Owl sculpture at Karolis Springs is waiting for someone."));
		SetPhase(QuestStatus.InProgress, "KATYN10_RP_1_NPC", "f_katyn_10", L("Rescue Spirits Taken by the Monsters"), L("The Gloomy Owl sculpture says that the monsters near Duokliu Hall have swallowed all of the spirits it is supposed to lead. Find and deal with the monsters that have swallowed the spirits to free them."));
		SetPhase(QuestStatus.Success, "KATYN10_RP_1_NPC", "f_katyn_10", L("Report back to the Gloomy Owl Sculpture"), L("You have freed a great deal of spirits. Go back and report to the Gloomy Owl Sculpture."));

		AddPrerequisite(new LevelPrerequisite(47));

		AddObjective("freeSpirits", L("Free the Swallowed Spirits"), new KillObjective(7, "digo", "VelStool", "kodomor_purple", "eldigo", "beeteros_blue", "Spector_gh_red", "truffle_blue"));

		AddReward(new ItemReward("expCard3", 1));
	}
}
