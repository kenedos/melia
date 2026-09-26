//--- Melia Script ----------------------------------------------------------
// Miners' Village Quest NPCs
//--- Description -----------------------------------------------------------
// The villagers, soldiers and hidden triggers the map's field quests run on.
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

public class FSiauliaiOutQuestNpcsScript : GeneralScript
{
	private readonly static QuestId Sout01 = new QuestId(8067);
	private readonly static QuestId Sout05 = new QuestId(8071);
	private readonly static QuestId Sout07 = new QuestId(8073);
	private readonly static QuestId Sout08 = new QuestId(8074);
	private readonly static QuestId Sout09 = new QuestId(8075);
	private readonly static QuestId Sout10 = new QuestId(8076);
	private readonly static QuestId Sout13 = new QuestId(8079);
	private readonly static QuestId Sout14 = new QuestId(8080);
	private readonly static QuestId Sout15 = new QuestId(8081);
	private readonly static QuestId Sout16 = new QuestId(8082);
	private readonly static QuestId SoutSudd = new QuestId(8347);
	private readonly static QuestId Sout20 = new QuestId(40050);
	private readonly static QuestId Sout21 = new QuestId(40051);
	private readonly static QuestId Sout22 = new QuestId(40052);
	private readonly static QuestId Sout23 = new QuestId(40053);
	private readonly static QuestId Sout24 = new QuestId(40054);
	private readonly static QuestId Sout31 = new QuestId(50004);
	private readonly static QuestId Sout32 = new QuestId(50005);
	private readonly static QuestId Slate3 = new QuestId(20052);
	private readonly static QuestId ToGele = new QuestId(50006);

	protected override void Load()
	{
		// Miners' Village Mayor
		//-------------------------------------------------------------------------
		AddNpc(20118, L("Miners' Village Mayor"), "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", -87.65, -802.09, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Miners' Village Mayor"));

			if (character.Quests.IsActive(Slate3) && character.Quests.IsCompletable(Slate3))
			{
				await dialog.Msg(L("Welcome. Aren't you our town's savior? Go down from the middle of the Twin Bridge and you'll arrive at Srautas Gorge. Go further right through the Gorge and you'll reach Gele Plateau."));
				dialog.ShowHelp("TUTO_INCOMPATIBLE");
				await dialog.CompleteQuest(Slate3);

				if (character.Quests.HasCompleted(Slate3) && !character.Quests.Has(ToGele))
					character.Quests.Start(ToGele);
				return;
			}

			if (!character.Quests.Has(ToGele) && character.Quests.MeetsPrerequisites(ToGele))
			{
				await dialog.Msg(L("Go straight down from the Twin Bridge and you'll arrive at Srautas Gorge. Go further right through Srautas Gorge and you'll be able to get to Gele Plateau."));
				character.Quests.Start(ToGele);
				return;
			}

			if (character.Quests.IsActive(ToGele))
			{
				await dialog.Msg(L("Srautas Gorge lies down the Twin Bridge. The cable car there will carry you up to Gele Plateau."));
				return;
			}

			if (character.Quests.IsActive(Sout01) && character.Quests.IsCompletable(Sout01))
			{
				await dialog.Msg(L("What do we do?"));
				await dialog.Msg(L("The Vubbes rushed in and kidnapped the villagers!"));

				await dialog.CompleteQuest(Sout01);
				return;
			}

			if (character.Quests.IsActive(Sout13) && character.Quests.IsCompletable(Sout13))
			{
				await dialog.Msg(L("How can I express my gratitude."));
				await dialog.Msg(L("I'm sure the goddess sent you to us."));
				await dialog.CompleteQuest(Sout13);
				return;
			}

			if (!character.Quests.Has(Sout14) && character.Quests.MeetsPrerequisites(Sout14))
			{
				await dialog.Msg(L("You are the Revelator, right?"));
				await dialog.Msg(L("The goddess must have helped."));
				await dialog.Msg(L("This is all because of the dream of the bishop of Klaipeda."));
				await dialog.Msg(L("What's all this about a 'Light of Salvation' in the Crystal Mine? I have never seen such a thing."));
				await dialog.Msg(L("I'm not sure if that is the reason, but the Vubbes suddenly rushed out of the Crystal Mine."));
				await dialog.Msg(L("Those Vubbes took all the villagers they saw into the mines."));
				var answer = await dialog.SelectQuestOffer(Sout14, L("Please... save the villagers. I beg of you!"),
					Option(L("How do we get into the Crystal Mine?"), "accept"),
					Option(L("Let me think on it for a while"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Hold on! There is something you should know."));
					await dialog.Msg(L("We went into the mines to save the kidnapped villagers too."));
					await dialog.Msg(L("But the purifiers were broken and the mine was filled with toxic fumes, so we failed every time."));
					await dialog.Msg(L("Ye..Yes.."));
					await dialog.Msg(L("Find a young man named Vaidotas!"));
					await dialog.Msg(L("That fellow knows well about the purifiers in the Crystal Mine."));
					await dialog.Msg(L("It hasn't been long since he was taken away by the Vubbes, so he must be in the Vubbe Outpost."));
					character.Quests.Start(Sout14);
				}
				return;
			}

			if (!character.Quests.Has(Sout13) && character.Quests.MeetsPrerequisites(Sout13))
			{
				await dialog.Msg(L("The Vubbes have built their base outside the village."));
				await dialog.Msg(L("Now that we don't even have able-bodied soldiers, something serious might happen if we don't drive them out of their base."));

				var answer = await dialog.SelectQuestOffer(Sout13, L("Will you drive the Vubbes out of their base?"),
					Option(L("I'll go to the Vubbe's base and defeat them"), "accept"),
					Option(L("That is not my concern"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Thank you so much for your bravery."));
					character.Quests.Start(Sout13);
				}
				return;
			}

			if (character.Quests.IsActive(Sout14))
			{
				await dialog.Msg(L("The goddess must have sent help..."));
				await dialog.Msg(L("Follow the pathway on the right to find the Vubbe Outpost."));
				character.Quests.ClearQuestTrack(Sout14);
				return;
			}

			if (character.Quests.IsActive(Sout13))
			{
				await dialog.Msg(L("To get to the Vubbe's base, go far right from here."));
				await dialog.Msg(L("It is miserable that I can't do anything as a mayor."));
				character.Quests.ClearQuestTrack(Sout13);
				return;
			}

			if (character.Quests.IsActive(Sout01))
			{
				await dialog.Msg(L("The Vubbes are still in the streets. Drive them off!"));
				character.Quests.ClearQuestTrack(Sout01);
				return;
			}

			await dialog.Msg(L("The village is in ruins. The goddesses have forsaken us."));
		});

		// Hunter
		//-------------------------------------------------------------------------
		AddNpc(47245, L("Hunter"), "SIAULIAIOUT_HUNTER", "f_siauliai_out", -945, -1812, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Hunter"));

			if (character.Quests.IsActive(Sout05) && character.Quests.IsCompletable(Sout05))
			{
				await dialog.Msg(L("I'll stay and aid the people left here."));
				await dialog.Msg(L("There are more people who need more than my aid, so please go on ahead."));
				await dialog.CompleteQuest(Sout05);
				return;
			}

			if (!character.Quests.Has(Sout05) && character.Quests.MeetsPrerequisites(Sout05))
			{
				await dialog.Msg(L("Oh, you're just in time. We need your help."));
				await dialog.Msg(L("Monsters stole all the aids and supplies meant for the refugees."));

				var answer = await dialog.SelectQuestOffer(Sout05, L("Will you help recover the relief supplies?"),
					Option(L("I'll help retrieve the relief supplies"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sout05);
					character.Quests.CompleteObjective(Sout05, "recoverSupplies");
				}
				return;
			}

			if (character.Quests.IsActive(Sout05))
			{
				await dialog.Msg(L("Fortunately, we found them quickly."));
				return;
			}

			await dialog.Msg(L("The road is still thick with monsters. Be careful."));
		});

		// Mine Manager Brinker
		//-------------------------------------------------------------------------
		AddNpc(20109, L("Mine Manager Brinker"), "SIAULIAIOUT_MINER_B", "f_siauliai_out", -1768.11, -815.68, 270, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Mine Manager Brinker"));

			if (character.Quests.IsActive(Sout07) && character.Quests.IsCompletable(Sout07))
			{
				await dialog.Msg(L("Alright. That'll be enough."));
				await dialog.CompleteQuest(Sout07);
				return;
			}

			if (character.Quests.IsActive(Sout08) && character.Quests.IsCompletable(Sout08))
			{
				await dialog.Msg(L("Now I can return to the village with peace of mind."));
				await dialog.Msg(L("This grace I shall never forget."));
				await dialog.CompleteQuest(Sout08);
				return;
			}

			if (!character.Quests.Has(Sout08) && character.Quests.MeetsPrerequisites(Sout08))
			{
				await dialog.Msg(L("I must go back to the village."));
				await dialog.Msg(L("But as you can see my arms are shaking."));
				await dialog.Msg(L("Can you defeat the monsters while I rest my arms for a while?"));

				var answer = await dialog.SelectQuestOffer(Sout08, L("Will you clear the monsters around him?"),
					Option(L("I'll defeat the monsters around"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sout08);

				return;
			}

			if (!character.Quests.Has(Sout07) && character.Quests.MeetsPrerequisites(Sout07))
			{
				await dialog.Msg(L("I'm holding this because it looks like it's about to collapse."));
				await dialog.Msg(L("We need some stones to do something about it."));

				var answer = await dialog.SelectQuestOffer(Sout07, L("Will you gather stones to shore up the wall?"),
					Option(L("I'll gather some stones for it"), "accept"),
					Option(L("Better run away quickly"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sout07);
					character.Quests.CompleteObjective(Sout07, "collectStones");
				}
				return;
			}

			if (character.Quests.IsActive(Sout08))
			{
				await dialog.Msg(L("The goddesses are so heartless."));
				await dialog.Msg(L("Had the goddesses not disappeared, the monsters wouldn't be invading."));
				return;
			}

			if (character.Quests.IsActive(Sout07))
			{
				await dialog.Msg(L("We won't be able to handle it if this wall opens."));
				await dialog.Msg(L("How will you stop them then if people are already running away now?"));
				return;
			}

			await dialog.Msg(L("The wall is barely holding. Mind the monsters while I stand here."));
		});

		// Healer Lady
		//-------------------------------------------------------------------------
		AddNpc(20168, L("Healer Lady"), "SIAULIAIOUT_HEALER_B", "f_siauliai_out", -1913, -1428, 136, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Healer Lady"));

			if (character.Quests.IsActive(Sout09) && character.Quests.IsCompletable(Sout09))
			{
				await dialog.Msg(L("I see you came back safely."));
				await dialog.Msg(L("Well then, I must also prepare to leave."));
				await dialog.CompleteQuest(Sout09);
				return;
			}

			if (character.Quests.IsActive(Sout10) && character.Quests.IsCompletable(Sout10))
			{
				await dialog.Msg(L("Thank you. The patient also regained his consciousness."));
				await dialog.Msg(L("Though... I think there is a problem."));
				await dialog.CompleteQuest(Sout10);
				return;
			}

			if (character.Quests.IsActive(SoutSudd) && character.Quests.IsCompletable(SoutSudd))
			{
				await dialog.Msg(L("Thank you. Now I can feel at ease and return to the village."));
				await dialog.Msg(L("May the blessing of the goddess always be with you."));
				await dialog.CompleteQuest(SoutSudd);
				return;
			}

			if (!character.Quests.Has(Sout09) && character.Quests.MeetsPrerequisites(Sout09))
			{
				await dialog.Msg(L("I came here to hide from the monsters, but even this area is becoming dangerous."));
				await dialog.Msg(L("I'm trying to make it back to the village, so can you bring the other refugees to me?"));

				var answer = await dialog.SelectQuestOffer(Sout09, L("Will you bring the refugees to her?"),
					Option(L("I'll bring the refugees"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("I must treat this patient at once... so thank you."));
					character.Quests.Start(Sout09);
				}
				return;
			}

			if (!character.Quests.Has(Sout10) && character.Quests.MeetsPrerequisites(Sout10))
			{
				await dialog.Msg(L("What do we do? I think there are even more monsters now than when we first came."));
				await dialog.Msg(L("And the patients have not recovered yet."));
				await dialog.Msg(L("Somehow I'll treat this man. Can you defeat the monsters around?"));

				var answer = await dialog.SelectQuestOffer(Sout10, L("Will you clear the monsters around her?"),
					Option(L("I'll defeat the menacing monsters"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sout10);

				return;
			}

			if (!character.Quests.Has(SoutSudd) && character.Quests.MeetsPrerequisites(SoutSudd))
			{
				await dialog.Msg(L("This patient told us that Chafer has appeared."));
				await dialog.Msg(L("Can you help us out and go check on it?"));

				var answer = await dialog.SelectQuestOffer(SoutSudd, L("Will you deal with Chafer?"),
					Option(L("Okay, I'll go look around again"), "accept"),
					Option(L("That would not happen."), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("I can't help but think about it since he's worked here for so long."));
					character.Quests.Start(SoutSudd);
				}
				return;
			}

			if (character.Quests.IsActive(Sout09))
			{
				await dialog.Msg(L("Those refugees are probably having a hard time getting here because of the monsters."));
				return;
			}

			if (character.Quests.IsActive(Sout10))
			{
				await dialog.Msg(L("In order for these people to safely return to the village, we have to defeat these monsters."));
				return;
			}

			if (character.Quests.IsActive(SoutSudd))
			{
				await dialog.Msg(L("Chafer is out there on the road back to the village. Please deal with it."));
				character.Quests.ClearQuestTrack(SoutSudd);
				return;
			}

			await dialog.Msg(L("So many wounded... The goddesses' mercy is far away today."));
		});

		// Refugee
		//-------------------------------------------------------------------------
		AddNpc(152000, L("Refugee"), "SOUT_REFUGEE01", "f_siauliai_out", -2242.07, -1971.59, 72, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Refugee"));

			if (character.Quests.IsActive(Sout09) && !character.Quests.IsCompletable(Sout09))
			{
				await dialog.Msg(L("We were hiding from the monsters, but the healer lady is right - we cannot stay here."));

				var told = await character.TimeActions.StartAsync(L("Telling them where the healer is..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));

				if (told != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The refugees will follow you back to the village."));
				character.Quests.CompleteObjective(Sout09, "bringRefugees");
				return;
			}

			await dialog.Msg(L("Thank you for coming for us. We will be on our way now."));
		});

		// Vaidotas at the Vubbe Outpost
		//-------------------------------------------------------------------------
		AddConditionalNpc(20110, L("[Alchemist Master]{nl}Vaidotas"), "SIAULIAIOUT_ALCHE", "f_siauliai_out", 1309.12, 331.73, 4, IsVaidotasAtOutpost, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Vaidotas"));
			dialog.SetPortrait("Dlg_port_ALCHEMIST_1");

			if (character.Quests.IsActive(Sout14) && character.Quests.IsCompletable(Sout14))
			{
				var freed = await character.TimeActions.StartAsync(L("Freeing Vaidotas..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (freed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Thank you for saving me."));
				await dialog.Msg(L("You must also be a Revelator who has come in search of the Light of Salvation."));
				await dialog.Msg(L("Let's go to the Crystal Mine."));
				await dialog.Msg(L("I will tell you the rest of the story at the Crystal Mine entrance."));
				await dialog.CompleteQuest(Sout14);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Sout15) && character.Quests.IsCompletable(Sout15))
			{
				await dialog.Msg(L("There were no villagers here, only Vubbes lying in wait."));
				await dialog.Msg(L("At least the Red Vubbe Fighter will not trouble the mine road any longer."));
				await dialog.CompleteQuest(Sout15);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("The Vubbe Outpost is no place to linger. Move along."));
		});

		// Vaidotas at the wagon barricade
		//-------------------------------------------------------------------------
		AddConditionalNpc(20110, L("[Alchemist Master]{nl}Vaidotas"), "SIAULIAIOUT_ALCHE_A", "f_siauliai_out", -38.88, -1021.81, 90, IsVaidotasAtBarricade, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Vaidotas"));
			dialog.SetPortrait("Dlg_port_ALCHEMIST_1");

			if (!character.Quests.Has(Sout16) && character.Quests.MeetsPrerequisites(Sout16))
			{
				await dialog.Msg(L("You came."));
				await dialog.Msg(L("First, let's clear the wagons blocking the way into the mines."));
				await dialog.Msg(L("The wagons can be demolished with these explosives that I have prepared."));
				await dialog.Msg(L("I'm concerned about the Vubbes that will come after hearing the explosion, but with your skills, I think we'll be fine."));

				var answer = await dialog.SelectQuestOffer(Sout16, L("Will you blow up the wagons?"),
					Option(L("I'm ready to destroy the wagons"), "accept"),
					Option(L("I'm not yet prepared"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Oh, this anvil is my small gift to you for saving me."));
					await dialog.Msg(L("It's a useful item that can upgrade your equipment."));
					await dialog.Msg(L("Well then, see you in the mines."));
					character.Quests.Start(Sout16);
				}
				return;
			}

			if (character.Quests.IsActive(Sout16))
			{
				await dialog.Msg(L("I'll let you take care of the Vubbes."));
				await dialog.Msg(L("I have things to attend to inside."));
				character.Quests.ClearQuestTrack(Sout16);
				return;
			}

			await dialog.Msg(L("The wagons block the mine road. Explosives should see to them."));
		});

		// Pharmacist Lady
		//-------------------------------------------------------------------------
		AddNpc(147493, L("Pharmacist Lady"), "SOUT_PHARMACY", "f_siauliai_out", -47.85, -1212.51, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pharmacist Lady"));

			if (character.Quests.IsActive(Sout20) && character.Quests.IsCompletable(Sout20))
			{
				await dialog.Msg(L("I know you are doing your best but I will need more."));
				await dialog.CompleteQuest(Sout20);
				return;
			}

			if (character.Quests.IsActive(Sout24) && character.Quests.IsCompletable(Sout24))
			{
				await dialog.Msg(L("Well done. This armor should prove to be useful when worn at the Crystal Mine."));
				await dialog.CompleteQuest(Sout24);
				return;
			}

			if (character.Quests.IsActive(Sout23) && character.Quests.IsCompletable(Sout23))
			{
				await dialog.Msg(L("Thank you!"));
				await dialog.Msg(L("I hope it would be helpful for you."));
				await dialog.CompleteQuest(Sout23);
				return;
			}

			if (character.Quests.IsActive(Sout22) && character.Quests.IsCompletable(Sout22))
			{
				await dialog.Msg(L("Thank you!"));
				await dialog.Msg(L("I hope it would be helpful for you."));
				await dialog.CompleteQuest(Sout22);
				return;
			}

			if (character.Quests.IsActive(Sout21) && character.Quests.IsCompletable(Sout21))
			{
				await dialog.Msg(L("Thank you!"));
				await dialog.Msg(L("I hope it would be helpful for you."));
				await dialog.CompleteQuest(Sout21);
				return;
			}

			if (!character.Quests.Has(Sout20) && character.Quests.MeetsPrerequisites(Sout20))
			{
				await dialog.Msg(L("I want to treat the injured in our village, but I don't have enough materials to do so."));

				var answer = await dialog.SelectQuestOffer(Sout20, L("Jukopus leaves and Kepa stems are what I need most. Could you gather some for me?"),
					Option(L("I'll gather them for you"), "accept"),
					Option(L("I have other business first"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Thank you very much. The Jukopus and the Kepas are all around the village."));
					character.Quests.Start(Sout20);
				}
				return;
			}

			if (!character.Quests.Has(Sout24) && character.Quests.MeetsPrerequisites(Sout24))
			{
				await dialog.Msg(L("I still need more Jukopus Leaves and Kepa Stems."));
				await dialog.Msg(L("But I don't want to just take them from you. How about trading it with the potions I have?"));

				var answer = await dialog.SelectQuestOffer(Sout24, L("Will you gather more ingredients?"),
					Option(L("I'll get it."), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Your act of kindness really gives me a lot of strength."));
					character.Quests.Start(Sout24);
				}
				return;
			}

			if (!character.Quests.Has(Sout23) && character.Quests.MeetsPrerequisites(Sout23))
			{
				await dialog.Msg(L("I still need more Jukopus Leaves and Kepa Stems."));
				await dialog.Msg(L("But I don't want to just take them from you. How about trading it with the potions I have?"));

				var answer = await dialog.SelectQuestOffer(Sout23, L("Will you gather more ingredients?"),
					Option(L("I'll get it."), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Your act of kindness really gives me a lot of strength."));
					character.Quests.Start(Sout23);
				}
				return;
			}

			if (!character.Quests.Has(Sout22) && character.Quests.MeetsPrerequisites(Sout22))
			{
				await dialog.Msg(L("I still need more Jukopus Leaves and Kepa Stems."));
				await dialog.Msg(L("But I don't want to just take them from you. How about trading it with the potions I have?"));

				var answer = await dialog.SelectQuestOffer(Sout22, L("Will you gather more ingredients?"),
					Option(L("I'll get it."), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Your act of kindness really gives me a lot of strength."));
					character.Quests.Start(Sout22);
				}
				return;
			}

			if (!character.Quests.Has(Sout21) && character.Quests.MeetsPrerequisites(Sout21))
			{
				await dialog.Msg(L("I still need more Jukopus Leaves and Kepa Stems."));
				await dialog.Msg(L("But I don't want to just take them from you. How about trading it with the potions I have?"));

				var answer = await dialog.SelectQuestOffer(Sout21, L("Will you gather more ingredients?"),
					Option(L("I'll get it."), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Your act of kindness really gives me a lot of strength."));
					character.Quests.Start(Sout21);
				}
				return;
			}

			if (character.Quests.IsActive(Sout20))
			{
				await dialog.Msg(L("Jukopus and Kepas appear all around the Miners' Village."));
				return;
			}

			if (character.Quests.IsActive(Sout21) || character.Quests.IsActive(Sout22)
				|| character.Quests.IsActive(Sout23) || character.Quests.IsActive(Sout24))
			{
				await dialog.Msg(L("Jukopus and Kepas appear around the Miners' Village."));
				return;
			}

			await dialog.Msg(L("So many wounded, and so little medicine left."));
		});

		// Soldier Jace
		//-------------------------------------------------------------------------
		AddNpc(20016, L("Soldier Jace"), "SIAULIAIOUT_SOLDIRE_SQ31", "f_siauliai_out", 90.96, -1225.09, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Soldier Jace"));

			if (character.Quests.IsActive(Sout31) && character.Quests.IsCompletable(Sout31))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("My comrades will now be able to rest in peace."));
				await dialog.CompleteQuest(Sout31);
				return;
			}

			if (!character.Quests.Has(Sout31) && character.Quests.MeetsPrerequisites(Sout31))
			{
				await dialog.Msg(L("I'm both furious and upset."));
				await dialog.Msg(L("If only we had gotten here sooner, we wouldn't have been totally wiped out..."));
				await dialog.Msg(L("Dear Revelator, I have a favor to ask of you."));
				await dialog.Msg(L("Can you recover my comrades' mementos?"));

				var answer = await dialog.SelectQuestOffer(Sout31, L("Will you gather the soldiers' mementos?"),
					Option(L("I'll gather the mementos"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sout31);

				return;
			}

			if (character.Quests.IsActive(Sout31))
			{
				await dialog.Msg(L("It's bad enough that they were killed by the Vubbes, but even their keepsakes are being abused. How awful..."));
				return;
			}

			await dialog.Msg(L("The Vubbes stripped the fallen, even of their keepsakes."));
		});

		// Soldier Edgar
		//-------------------------------------------------------------------------
		AddNpc(20019, L("Soldier Edgar"), "SIAULIAIOUT_SOLDIRE_SQ32", "f_siauliai_out", 429.01, -1201.72, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Soldier Edgar"));

			if (character.Quests.IsActive(Sout32) && character.Quests.IsCompletable(Sout32))
			{
				await dialog.Msg(L("Thank you."));
				await dialog.Msg(L("This will at least get us through the next few days."));
				await dialog.CompleteQuest(Sout32);
				return;
			}

			if (!character.Quests.Has(Sout32) && character.Quests.MeetsPrerequisites(Sout32))
			{
				await dialog.Msg(L("Oh my God. The Vubbes stole all our food supplies."));
				await dialog.Msg(L("They even took our food for today... Please help us, we need your strength."));

				var answer = await dialog.SelectQuestOffer(Sout32, L("Will you recover the stolen food?"),
					Option(L("I'll get back the food supplies from the Vubbes"), "accept"),
					Option(L("Sorry, I'm busy"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sout32);

				return;
			}

			if (character.Quests.IsActive(Sout32))
			{
				await dialog.Msg(L("They stole the food meant for both the army and the residents."));
				await dialog.Msg(L("What will we do if we can't get our food back..."));
				return;
			}

			await dialog.Msg(L("With the supplies gone, the whole village is going hungry."));
		});

		// Suspicious Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(147392, L("Treasure Box LV1"), "TREASUREBOX_BUBE", "f_siauliai_out", 1651.87, 427.34, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Box LV1"));

			if (!character.Quests.Has(Sout15) && character.Quests.MeetsPrerequisites(Sout15))
			{
				await dialog.Msg(L("{#666666}*You pry the lid up. The chest is empty, and something behind you has stopped moving*{/}"));
				character.Quests.Start(Sout15);
				return;
			}

			if (character.Quests.IsActive(Sout15))
			{
				await dialog.Msg(L("The chest is empty, and the Vubbes are still here."));
				character.Quests.ReplayQuestTrack(Sout15);
				return;
			}

			await dialog.Msg(L("The chest stands alone on the mining road. Something about it is wrong."));
		});

		// The wagons blocking the mine road
		//-------------------------------------------------------------------------
		AddBlockingWagon(1, -82, -612, 61);
		AddBlockingWagon(2, -41, -608, 0);
		AddBlockingWagon(3, -64, -557, 0);
		AddConditionalNpc(MonsterId.HiddenWall_10_100_500, "", "SOUT_Q_16_WALL", "f_siauliai_out", -60.97, -605.29, 90, IsWagonBlockingRoad);

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The approach to the Miners' Village, where the Vubbe raid begins.
		AddQuestTrigger("SIAULIAIOUT_Q01", "f_siauliai_out", 506, -1622, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(Sout01) && character.Quests.MeetsPrerequisites(Sout01))
				character.Quests.Start(Sout01);

			if (character.Quests.IsActive(Sout01) && !character.Quests.IsCompletable(Sout01))
				character.Quests.StartQuestTrack(Sout01);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAULIAIOUT_MIRTIS", "f_siauliai_out", 1900, 130, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sout13) && !character.Quests.IsCompletable(Sout13))
				character.Quests.StartQuestTrack(Sout13);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAULIAIOUT_PREAL", "f_siauliai_out", 1298, 307, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sout14) && !character.Quests.IsCompletable(Sout14))
				character.Quests.StartQuestTrack(Sout14);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAULIAIOUT_BLOCK", "f_siauliai_out", -61, -656, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Sout16) && !character.Quests.IsCompletable(Sout16))
				character.Quests.StartQuestTrack(Sout16);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SOUT_SUDD", "f_siauliai_out", -1532, -1751, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(SoutSudd) && !character.Quests.IsCompletable(SoutSudd))
				character.Quests.StartQuestTrack(SoutSudd);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Returns whether Vaidotas is still the Vubbes' captive at the outpost
	/// for the given character.
	/// </summary>
	private static bool IsVaidotasAtOutpost(Character character)
		=> character.Quests.IsActive(Sout14) || (character.Quests.Has(Sout15) && !character.Quests.HasCompleted(Sout15));

	/// <summary>
	/// Returns whether Vaidotas has been rescued and is waiting at the wagon
	/// barricade for the given character.
	/// </summary>
	private static bool IsVaidotasAtBarricade(Character character)
		=> character.Quests.HasCompleted(Sout14) && !character.Quests.HasCompleted(Sout16);

	/// <summary>
	/// Returns whether the wagons still block the mine road for the given
	/// character, which they stop doing once the explosives went off.
	/// </summary>
	private static bool IsWagonBlockingRoad(Character character)
		=> !character.Quests.HasCompleted(Sout16) && !character.Quests.IsCompletable(Sout16);

	/// <summary>
	/// Places one of the wagons barricading the mine road, which the
	/// explosives clear.
	/// </summary>
	private void AddBlockingWagon(int number, double x, double z, int direction)
	{
		AddConditionalNpc(45315, L("Empty Wagon"), "SIAULIAIOUT_WAGON_" + number, "f_siauliai_out", x, z, direction,
			IsWagonBlockingRoad, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Empty Wagon"));

			if (character.Quests.IsActive(Sout16))
			{
				await dialog.Msg(L("Vaidotas' explosives are already packed under the wheels. Stand back."));
				return;
			}

			await dialog.Msg(L("A mining wagon, dragged across the road and left to rot."));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8067: Investigating Miners' Village (1)
//-----------------------------------------------------------------------------
public class SoutQ01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8067);
		SetName(L("Investigating Miners' Village (1)"));
		SetDescription(L("Reach the Miners' Village and find out from its mayor what happened."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		// The client's StartNPC is the hidden approach trigger; the turn-in is the mayor.
		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_Q01", "f_siauliai_out", L("Arrive at the Miners' Village"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_Q01", "f_siauliai_out", L("Arrive at the Miners' Village"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"));

		// The raid cutscene is the phase; the trigger plays it on arrival.
		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_OUT_Q1_TRACK", 3000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1044, QuestStatus.Completed));

		AddObjective("meetMayor", L("Talk to the Miners' Village Mayor"), new ManualObjective());

		AddReward(new SelectItemReward("SWD01_114", "STF01_114", "TBW01_114", "MAC01_114"));
	}
}

// 8071: Aid Recovery
//-----------------------------------------------------------------------------
public class SoutQ05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8071);
		SetName(L("Aid Recovery"));
		SetDescription(L("The Hunter needs help recovering the relief supplies the monsters stole."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_HUNTER", "f_siauliai_out", L("Talk to the Hunter"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_HUNTER", "f_siauliai_out", L("Aid Recovery"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_HUNTER", "f_siauliai_out", L("Talk to the Hunter"));

		AddObjective("recoverSupplies", L("Recover the relief supplies"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("HAND01_117", 1));
		AddReward(new ItemReward("FOOT01_117", 1));
		AddReward(new TakeItemReward("SIAUL_OUT_QUEST_SUPPLIE"));
	}
}

// 8073: Mine Manager Brinker's Dedication (1)
//-----------------------------------------------------------------------------
public class SoutQ07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8073);
		SetName(L("Mine Manager Brinker's Dedication (1)"));
		SetDescription(L("Brinker is holding up a collapsing wall. Gather stones to shore it up."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Talk to Mine Manager Brinker"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Collect Reinforcement Stones from the rubble nearby"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Give the Reinforcement Stones to Brinker"));

		AddPrerequisite(new LevelPrerequisite(5));

		AddObjective("collectStones", L("Collect Reinforcement Stones from the rubble nearby"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new TakeItemReward("SIAUL_OUT_QUEST_STONE"));
	}
}

// 8074: Mine Manager Brinker's Dedication (2)
//-----------------------------------------------------------------------------
public class SoutQ08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8074);
		SetName(L("Mine Manager Brinker's Dedication (2)"));
		SetDescription(L("Clear the monsters around the wall so Brinker can return to the village."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Talk to Mine Manager Brinker"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Defeat the monsters nearby"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_MINER_B", "f_siauliai_out", L("Tell Brinker that it is safe"));

		AddPrerequisite(new QuestStatusPrerequisite(8073, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat the monsters nearby"), new KillObjective(8, "Onion_Red", "Jukopus", "Goblin_Spear", "Goblin_Spear_Q1"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 8075: Healer Lady's Worry (1)
//-----------------------------------------------------------------------------
public class SoutQ09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8075);
		SetName(L("Healer Lady's Worry (1)"));
		SetDescription(L("Bring the refugee couple by the Statue of Goddess Zemyna back to the Healer Lady."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Talk to the Healer Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_REFUGEE01", "f_siauliai_out", L("Bring the refugees"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Turn over the refugees to the Healer Lady"));

		AddPrerequisite(new LevelPrerequisite(5));

		AddObjective("bringRefugees", L("Bring the refugees"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 8076: Healer Lady's Worry (2)
//-----------------------------------------------------------------------------
public class SoutQ10Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8076);
		SetName(L("Healer Lady's Worry (2)"));
		SetDescription(L("The Healer Lady cannot travel with so many monsters about. Thin them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Talk to the Healer Lady"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Defeat the monsters"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Talk to the Healer Lady"));

		AddPrerequisite(new QuestStatusPrerequisite(8075, QuestStatus.Completed));

		AddObjective("killMonsters", L("Defeat the monsters"), new KillObjective(11, "Jukopus", "Goblin_Spear", "Onion_Red", "Goblin_Spear_Q1"));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("LEG01_117", 1));
		AddReward(new ItemReward("TOP01_117", 1));
	}
}

// 8079: Invasion of the Vubbes
//-----------------------------------------------------------------------------
public class SoutQ13Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8079);
		SetName(L("Invasion of the Vubbes"));
		SetDescription(L("The Vubbes have built a base outside the village. Drive them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_MIRTIS", "f_siauliai_out", L("Defeat the Vubbes who invaded the village"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SOUT_Q_13_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(6));

		AddObjective("killVubbes", L("Defeat the Vubbes"), new KillObjective(8, "Goblin_Spear", "Goblin_Archer_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 8080: Kidnapped Villagers
//-----------------------------------------------------------------------------
public class SoutQ14Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8080);
		SetName(L("Kidnapped Villagers"));
		SetDescription(L("The Vubbes took the villagers into the Crystal Mine. Save Vaidotas at the Vubbe Outpost first."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_PREAL", "f_siauliai_out", L("Rescue Vaidotas who is kidnapped by the Vubbes"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_ALCHE", "f_siauliai_out", L("Talk to Vaidotas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_OUT_ALCHE_TRACK", 1000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8067, QuestStatus.Completed));

		AddObjective("killVubbes", L("Defeat the Vubbes"), new KillObjective(5, "Goblin_Spear", "Goblin_Archer_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 2));
	}
}

// 8081: Suspicious Treasure Chest
//-----------------------------------------------------------------------------
public class SoutQ15Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8081);
		SetName(L("Suspicious Treasure Chest"));
		SetDescription(L("A suspicious chest stands on the mining road. Open it and deal with what comes out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "TREASUREBOX_BUBE", "f_siauliai_out", L("Suspicious Treasure Chest"));
		SetPhase(QuestStatus.InProgress, "TREASUREBOX_BUBE", "f_siauliai_out", L("Defeat the Red Vubbe Fighter that suddenly appeared"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_ALCHE", "f_siauliai_out", L("Talk to Vaidotas"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_OUT_BOSS_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(6));

		AddObjective("killFighter", L("Defeat the Red Vubbe Fighter"), new KillObjective(1, "boss_Goblin_Warrior_red") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8082: To the Mines
//-----------------------------------------------------------------------------
public class SoutQ16Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8082);
		SetName(L("To the Mines"));
		SetDescription(L("Blow up the wagons blocking the mine road and press on to Crystal Mine 1F."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_ALCHE_A", "f_siauliai_out", L("Talk to Vaidotas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_BLOCK", "f_siauliai_out", L("Defeat any Vubbe drawn out by the explosives"));
		SetPhase(QuestStatus.Success, "MINE_1_ALCHEMIST", "d_cmine_01", L("Talk to Vaidotas in Crystal Mine 1F"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_OUT_Q16_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8080, QuestStatus.Completed));

		AddObjective("killVubbes", L("Defeat any Vubbe drawn out by the explosives"), new KillObjective(6, "Goblin_Miners_Q2"));

		AddReward(new ItemReward("expCard1", 2));
		AddReward(new ItemReward("BRC01_105", 1));
		AddReward(new ItemReward("Scroll_Warp_quest", 10));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);
		character.LookAround();
	}
}

// 8347: Healer Lady's Worry (3)
//-----------------------------------------------------------------------------
public class SoutSuddPrebossQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8347);
		SetName(L("Healer Lady's Worry (3)"));
		SetDescription(L("Chafer blocks the road back to the village. Put it down."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Listen to the Healer Lady's request"));
		SetPhase(QuestStatus.InProgress, "SOUT_SUDD", "f_siauliai_out", L("Defeat Chafer"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_HEALER_B", "f_siauliai_out", L("Inform the results to the Healer Lady"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SOUT_SUDD_PREBOSS", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8076, QuestStatus.Completed));

		AddObjective("killChafer", L("Defeat Chafer that is blocking the way"), new KillObjective(1, "boss_chafer_sout") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("TreasureboxKey2", 1));
	}
}

// 40050: Pharmacist's Favor (1)
//-----------------------------------------------------------------------------
public class SoutQ20Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40050);
		SetName(L("Pharmacist's Favor (1)"));
		SetDescription(L("The Pharmacist Lady needs Jukopus leaves and Kepa stems to treat the wounded."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SOUT_PHARMACY", "f_siauliai_out", L("Talk to the Pharmacist Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_PHARMACY", "f_siauliai_out", L("Get medicinal ingredients"));
		SetPhase(QuestStatus.Success, "SOUT_PHARMACY", "f_siauliai_out", L("Give medicinal ingredients to Pharmacist Lady"));

		AddPrerequisite(Or(new QuestStatusPrerequisite(8082, QuestStatus.InProgress), new QuestStatusPrerequisite(8082, QuestStatus.Completed)));
		AddPrerequisite(new QuestStatusPrerequisite(8074, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8347, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(8071, QuestStatus.Completed));

		AddPityDrop("misc_0010", 0.5f, 3, 1, "Jukopus");
		AddPityDrop("misc_0001", 0.5f, 3, 1, "Onion_Red");

		AddObjective("collectLeaves", L("Collect Jukopus Leaves"), new CollectItemObjective("misc_0010", 5));
		AddObjective("collectStems", L("Collect Kepa Stems"), new CollectItemObjective("misc_0001", 5));

		AddReward(new TakeItemReward("misc_0010", 5));
		AddReward(new TakeItemReward("misc_0001", 5));
	}
}

// 40051: Pharmacist's Favor (2)
//-----------------------------------------------------------------------------
public class SoutQ21Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40051);
		SetName(L("Pharmacist's Favor (2)"));
		SetDescription(L("The Pharmacist Lady needs more Jukopus leaves and Kepa stems."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SOUT_PHARMACY", "f_siauliai_out", L("Talk to the Pharmacist Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_PHARMACY", "f_siauliai_out", L("Get medicinal ingredients"));
		SetPhase(QuestStatus.Success, "SOUT_PHARMACY", "f_siauliai_out", L("Give medicinal ingredients to Pharmacist Lady"));

		AddPrerequisite(new QuestStatusPrerequisite(40050, QuestStatus.Completed));

		AddPityDrop("misc_0010", 0.5f, 3, 1, "Jukopus");
		AddPityDrop("misc_0001", 0.5f, 3, 1, "Onion_Red");

		AddObjective("collectLeaves", L("Collect Jukopus Leaves"), new CollectItemObjective("misc_0010", 5));
		AddObjective("collectStems", L("Collect Kepa Stems"), new CollectItemObjective("misc_0001", 5));

		AddReward(new ItemReward("Drug_HP1_Q", 3));
		AddReward(new TakeItemReward("misc_0010", 5));
		AddReward(new TakeItemReward("misc_0001", 5));
	}
}

// 40052: Pharmacist's Favor (3)
//-----------------------------------------------------------------------------
public class SoutQ22Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40052);
		SetName(L("Pharmacist's Favor (3)"));
		SetDescription(L("The Pharmacist Lady needs more Jukopus leaves and Kepa stems."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SOUT_PHARMACY", "f_siauliai_out", L("Talk to the Pharmacist Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_PHARMACY", "f_siauliai_out", L("Get medicinal ingredients"));
		SetPhase(QuestStatus.Success, "SOUT_PHARMACY", "f_siauliai_out", L("Give medicinal ingredients to Pharmacist Lady"));

		AddPrerequisite(new QuestStatusPrerequisite(40051, QuestStatus.Completed));

		AddPityDrop("misc_0010", 0.5f, 3, 1, "Jukopus");
		AddPityDrop("misc_0001", 0.5f, 3, 1, "Onion_Red");

		AddObjective("collectLeaves", L("Collect Jukopus Leaves"), new CollectItemObjective("misc_0010", 5));
		AddObjective("collectStems", L("Collect Kepa Stems"), new CollectItemObjective("misc_0001", 5));

		AddReward(new ItemReward("Drug_SP1_Q", 3));
		AddReward(new TakeItemReward("misc_0010", 5));
		AddReward(new TakeItemReward("misc_0001", 5));
	}
}

// 40053: Pharmacist's Favor (4)
//-----------------------------------------------------------------------------
public class SoutQ23Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40053);
		SetName(L("Pharmacist's Favor (4)"));
		SetDescription(L("The Pharmacist Lady needs more Jukopus leaves and Kepa stems."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SOUT_PHARMACY", "f_siauliai_out", L("Talk to the Pharmacist Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_PHARMACY", "f_siauliai_out", L("Get medicinal ingredients"));
		SetPhase(QuestStatus.Success, "SOUT_PHARMACY", "f_siauliai_out", L("Give medicinal ingredients to Pharmacist Lady"));

		AddPrerequisite(new QuestStatusPrerequisite(40052, QuestStatus.Completed));

		AddPityDrop("misc_0010", 0.5f, 3, 1, "Jukopus");
		AddPityDrop("misc_0001", 0.5f, 3, 1, "Onion_Red");

		AddObjective("collectLeaves", L("Collect Jukopus Leaves"), new CollectItemObjective("misc_0010", 5));
		AddObjective("collectStems", L("Collect Kepa Stems"), new CollectItemObjective("misc_0001", 5));

		AddReward(new ItemReward("Drug_STA1_Q", 3));
		AddReward(new TakeItemReward("misc_0010", 5));
		AddReward(new TakeItemReward("misc_0001", 5));
	}
}

// 40054: Pharmacist's Favor (5)
//-----------------------------------------------------------------------------
public class SoutQ24Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(40054);
		SetName(L("Pharmacist's Favor (5)"));
		SetDescription(L("The Pharmacist Lady needs more Jukopus leaves and Kepa stems."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SOUT_PHARMACY", "f_siauliai_out", L("Talk to the Pharmacist Lady"));
		SetPhase(QuestStatus.InProgress, "SOUT_PHARMACY", "f_siauliai_out", L("Get medicinal ingredients"));
		SetPhase(QuestStatus.Success, "SOUT_PHARMACY", "f_siauliai_out", L("Give medicinal ingredients to Pharmacist Lady"));

		AddPrerequisite(new QuestStatusPrerequisite(40053, QuestStatus.Completed));

		AddPityDrop("misc_0010", 0.5f, 3, 1, "Jukopus");
		AddPityDrop("misc_0001", 0.5f, 3, 1, "Onion_Red");

		AddObjective("collectLeaves", L("Collect Jukopus Leaves"), new CollectItemObjective("misc_0010", 5));
		AddObjective("collectStems", L("Collect Kepa Stems"), new CollectItemObjective("misc_0001", 5));

		AddReward(new ItemReward("Drug_HPSP1_Q", 3));
		AddReward(new ItemReward("TOP02_149", 1));
		AddReward(new TakeItemReward("misc_0010", 5));
		AddReward(new TakeItemReward("misc_0001", 5));
	}
}

// 50004: A Soldier's Favor
//-----------------------------------------------------------------------------
public class SoutQ31Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50004);
		SetName(L("A Soldier's Favor"));
		SetDescription(L("Soldier Jace wants the mementos the Vubbes tore from his fallen comrades."));
		SetType(QuestType.Repeat);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_SOLDIRE_SQ31", "f_siauliai_out", L("Talk to Soldier Jace"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_SOLDIRE_SQ31", "f_siauliai_out", L("Retrieving Soldiers' Mementos"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_SOLDIRE_SQ31", "f_siauliai_out", L("Talk to Soldier Jace"));

		AddPrerequisite(new QuestStatusPrerequisite(8080, QuestStatus.Completed));

		AddPityDrop("SOLDIRE_SQ31_RELIC", 0.6f, 4, 1, "Goblin_Spear");

		AddObjective("collectMementos", L("Collect soldiers' mementos"), new CollectItemObjective("SOLDIRE_SQ31_RELIC", 6));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("SOLDIRE_SQ31_RELIC"));
	}
}

// 50005: Stolen Food Supplies
//-----------------------------------------------------------------------------
public class SoutQ32Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50005);
		SetName(L("Stolen Food Supplies"));
		SetDescription(L("The Vubbes made off with the village's food. Get it back."));
		SetType(QuestType.Repeat);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_SOLDIRE_SQ32", "f_siauliai_out", L("Talk to Soldier Edgar"));
		SetPhase(QuestStatus.InProgress, "SIAULIAIOUT_SOLDIRE_SQ32", "f_siauliai_out", L("Retrieve the stolen food supplies from the Vubbes"));
		SetPhase(QuestStatus.Success, "SIAULIAIOUT_SOLDIRE_SQ32", "f_siauliai_out", L("Talk to Soldier Edgar"));

		AddPrerequisite(new QuestStatusPrerequisite(8080, QuestStatus.Completed));

		AddPityDrop("TOWN_PROVISIONS", 1.0f, 0, 1, "Goblin_Spear");

		AddObjective("collectFood", L("Collect Miners' Village Food"), new CollectItemObjective("TOWN_PROVISIONS", 10));

		AddReward(new ItemReward("expCard2", 2));
		AddReward(new TakeItemReward("TOWN_PROVISIONS"));
	}
}

// 50006: To Gele Plateau
//-----------------------------------------------------------------------------
public class SoutQ41Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50006);
		SetName(L("To Gele Plateau"));
		SetDescription(L("The Mayor of the Miners' Village explains the road to Gele Plateau through Srautas Gorge."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_out");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAULIAIOUT_CHIEF_A", "f_siauliai_out", L("Talk to the Miners' Village Mayor"), L("Talk to the Miners' Village Mayor."));
		SetPhase(QuestStatus.InProgress, "SOUT_Q_41_ARRIVE", "f_gele_57_1", L("Travel to Gele Plateau"), L("Go down from the Twin Bridge at the Miners' Village to get to Srautas Gorge, then take the cable car and go a bit further up to reach Gele Plateau."));
		SetPhase(QuestStatus.Success, "SOUT_Q_41_ARRIVE", "f_gele_57_1", L("Travel to Gele Plateau"), L("Go down from the Twin Bridge at the Miners' Village to get to Srautas Gorge, then take the cable car and go a bit further up to reach Gele Plateau."));

		AddPrerequisite(new QuestStatusPrerequisite(20052, QuestStatus.Completed));

		AddObjective("travelToGele", L("Travel to Gele Plateau"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The trip itself is the quest; there is no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}
