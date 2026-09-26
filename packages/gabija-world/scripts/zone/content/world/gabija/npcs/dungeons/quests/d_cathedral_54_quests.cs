//--- Melia Script ----------------------------------------------------------
// Great Cathedral Grand Corridor Quest NPCs
//--- Description -----------------------------------------------------------
// Maven's machines, the priests sent to unpick Naktis' curse, and the altar
// of the revelation at the end of the corridor.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral54QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20310);
	private readonly static QuestId Mq02 = new QuestId(20311);
	private readonly static QuestId Mq03 = new QuestId(20312);
	private readonly static QuestId Mq04 = new QuestId(20313);
	private readonly static QuestId Sq01 = new QuestId(20314);
	private readonly static QuestId Sq02 = new QuestId(20315);
	private readonly static QuestId Sq03 = new QuestId(20316);
	private readonly static QuestId Sq04 = new QuestId(20317);
	private readonly static QuestId Sq05 = new QuestId(20318);
	private readonly static QuestId Cathedral56Mq01 = new QuestId(20329);
	private readonly static QuestId Mq05Part3 = new QuestId(20340);
	private readonly static QuestId Mq06Part3 = new QuestId(20341);
	private readonly static QuestId ToVelniasPrison = new QuestId(50027);

	private const int DocumentsNeeded = 10;
	private const int DocumentsPerBook = 2;

	private const string FootholdVar = "Gabija.Cathedral54.Foothold";
	private const string BookVar = "Gabija.Cathedral54.Book";

	protected override void Load()
	{
		// Bishop Aurelius' Spirit, at the corridor entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL54_PART1_BISHOP", "d_cathedral_54", -970.28, -618.53, 116, c => !c.Quests.HasCompleted(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bishop Aurelius' Spirit"));
			dialog.SetPortrait("Dlg_port_aurelius");

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				var answer = await dialog.SelectQuestOffer(Mq01, L("Maven was one of the greatest priests, yet he was also interested in machinery. I am sure that the demons, who only think about magical power, don't understand his machines. The Platform of Benevolence is one of the machines Maven invented."),
					Option(L("I will solve the secret"), "accept"),
					Option(L("I'll do it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Variables.Perm.Set(FootholdVar + 1, false);
					character.Variables.Perm.Set(FootholdVar + 2, false);

					character.Quests.Start(Mq01);
					await dialog.Msg(L("Proceed through Arka Chapel and to the Reception Room to obtain the second key."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("Step on the footholds of the Arka Chapel and the Reception Room, both of them."));
				return;
			}

			await dialog.Msg(L("I tried my best to hide Maven's secret, but Naktis is also staying at the Great Cathedral too long."));
			await dialog.Msg(L("I believe that the evil will eventually fall and the goddess will be victorious. The mark of victory is right in front of me."));
		});

		// The two footholds of the Platform of Benevolence
		//-------------------------------------------------------------------------
		this.AddFoothold(1, L("Arka Chapel Foothold"), -912, -915);
		this.AddFoothold(2, L("Reception Room Foothold"), -1499, -913);

		// Platform of Benevolence
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Platform of Benevolence"), "CHATHEDRAL54_MQ01_PUZZLE", "d_cathedral_54", -1500.94, -1050.35, 178, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Platform of Benevolence"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				var checkedDevice = await character.TimeActions.StartAsync(L("Checking..."), L("Cancel"), "LOOK_SIT", TimeSpan.FromSeconds(2));

				if (checkedDevice != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The machine turns over and gives up the second of Maven's keys."));
				await dialog.CompleteQuest(Mq01);
				character.ServerMessage(L("Acquired Maven's key!"));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("One of Maven's machines, and it is not moving."));
		});

		// Bishop Aurelius' Spirit, at Uola Chapel
		//-------------------------------------------------------------------------
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", -1203.82, 993.48, 86, c => c.Quests.HasCompleted(Mq01) && !c.Quests.HasCompleted(Mq04), this.UolaBishop);

		// Holy Symbol of Spiritual Power
		//-------------------------------------------------------------------------
		// The cutscene spawns its own copy, so this one only stands once the
		// demons around it are down.
		AddConditionalNpc(151001, L("Holy Symbol of Spiritual Power"), "CATHEDRAL54MQ02_OBJECT", "d_cathedral_54", -877.76, 992.09, 90, c => c.Quests.IsActive(Mq03) && c.Quests.IsCompletable(Mq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Holy Symbol of Spiritual Power"));

			await dialog.Msg(L("The symbol is whole. Whatever the demons were doing to it, they did not finish."));
			await dialog.CompleteQuest(Mq03);
			character.LookAround();
		});

		// Karuna Altar
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Karuna Altar"), "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", 826.34, 1200.76, 357, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Karuna Altar"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				var placed = await character.TimeActions.StartAsync(L("Setting the symbol in place..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(2));

				if (placed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The symbol sinks into the altar and the third of Maven's keys rises out of it."));
				await dialog.CompleteQuest(Mq04);
				character.ServerMessage(L("Acquired Maven's key!"));
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Sq05) && character.Quests.IsCompletable(Sq05))
			{
				await dialog.Msg(L("Riteris is down and the altar is quiet again."));
				await dialog.CompleteQuest(Sq05);
				return;
			}

			if (!character.Quests.Has(Sq05) && character.Quests.MeetsPrerequisites(Sq05))
			{
				var answer = await dialog.SelectQuestOffer(Sq05, L("Something has been at the Karuna Altar since the key was taken out of it."),
					Option(L("Check the Karuna Altar"), "accept"),
					Option(L("Leave it alone"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Sq05);

				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The altar waits. The symbol is still empty of power."));
				return;
			}

			if (character.Quests.IsActive(Sq05))
			{
				await dialog.Msg(L("Naktis' servants have rushed in!"));
				character.Quests.ReplayQuestTrack(Sq05);
				return;
			}

			await dialog.Msg(L("The Karuna Altar, and a socket in it the shape of a symbol."));
		});

		// Priest Ruodell
		//-------------------------------------------------------------------------
		AddNpc(147398, L("Priest Ruodell"), "CHATHEDRAL54_SQ04_PART2", "d_cathedral_54", 927.06, 494.96, 168, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Ruodell"));

			if (character.Quests.IsActive(Sq01) && character.Quests.IsCompletable(Sq01))
			{
				await dialog.Msg(L("This is a big help."));
				await dialog.Msg(L("When I treat the people who are cursed, I will tell them your name."));
				await dialog.CompleteQuest(Sq01);
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("We were sent from the congregation to investigate Naktis' curse. We should first find the documents that were scattered here, but those demons are a problem."),
					Option(L("Leave it to me"), "accept"),
					Option(L("I will help him later"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= 6; ++i)
						character.Variables.Perm.Set(BookVar + i, false);

					character.Quests.Start(Sq01);
					await dialog.Msg(L("They are all over the reading floor above. Anything about Naktis will do."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("The lost documents all contain important content."));
				await dialog.Msg(L("I want to bring all of them, but we should first remove the curse."));
				return;
			}

			await dialog.Msg(L("Four years of dust on everything, and every page of it still matters."));
		});

		// The old books of the reading floor
		//-------------------------------------------------------------------------
		this.AddOldBook(1, 147311, L("Book on the Floor"), 184.10, 1074.55, 84);
		this.AddOldBook(2, 153014, L("Old Book"), -255.98, 1353.86, 148);
		this.AddOldBook(3, 153014, L("Old Book"), 371.45, 1332.72, 90);
		this.AddOldBook(4, 153014, L("Old Book"), -376.65, 1191.56, 90);
		this.AddOldBook(5, 153014, L("Old Book"), 279.99, 856.27, 29);
		this.AddOldBook(6, 153014, L("Old Book"), 66.13, 1371.25, 33);

		// Priest Yosana
		//-------------------------------------------------------------------------
		AddNpc(147397, L("Priest Yosana"), "CHATHEDRAL54_SQ03_PART1", "d_cathedral_54", 1051.28, -65.46, 31, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Yosana"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("Ah! This is it."));
				await dialog.Msg(L("If you get a chance to go to the Main Chamber, please hand it over to Aden."));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("I made a stupid mistake. I lost my concentration for a moment and lost the report that I should've sent to Aden. I have been looking around, but I think the demons may have played a part in this."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq02);
					await dialog.Msg(L("The Stoulets took it. Six pages of it, if they have not eaten them."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("It seems that Naktis has found out why we came to the Great Cathedral."));
				await dialog.Msg(L("Otherwise, there'd no reason for those demons to go after reports that should've been passed to Aden."));
				return;
			}

			await dialog.Msg(L("The congregation told us that they will retake the Great Cathedral once Naktis' curse is resolved."));
			await dialog.Msg(L("It just isn't right to leave the Great Cathedral, which supported the goddesses, like this for four years."));
		});

		// Priest Daram
		//-------------------------------------------------------------------------
		AddNpc(147386, L("Priest Daram"), "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", 920, -164.45, 60, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Daram"));

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("It's either ineffective or... actually it seems okay."));
				await dialog.Msg(L("It even explodes? Then we should work on it little more."));
				await dialog.CompleteQuest(Sq04);
				return;
			}

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("Thanks."));
				await dialog.Msg(L("The purity is a little lacking, but if we refine it one more time, it will be useful."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("To remove Naktis' curse, I am making a reagent which resists against evil energy. But, it's hard to guarantee its effects right now. If it's okay with you, can you check the reagent's effects?"),
					Option(L("I will test it out"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					character.Inventory.Add(ItemId.CATHEDRAL54_SQ04_PART2_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Ah, I didn't mean trying it on yourself. I meant trying it on the demons."));
				}
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("We will need a lot of solvent in order to make the reagent that removes Naktis' curse. But, as you can see, we can't get the solvent at the moment. But, there is still a way, if you could help me."),
					Option(L("Ask her how you can help"), "accept"),
					Option(L("I don't have time for that"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					character.Inventory.Add(ItemId.CATHEDRAL54_SQ04_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("I will give you this scroll of purification, use this on the demons and then defeat them."));
					await dialog.Msg(L("The demons' fluids will become a solvent purified with the holy energy of the Great Cathedral."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("This reagent pulls out evil energy from the body."));
				await dialog.Msg(L("But, it could cause internal injuries."));
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("I was confident in making reagents so I thought I would be successful."));
				await dialog.Msg(L("I think I overestimated myself."));
				return;
			}

			await dialog.Msg(L("I heard that a priest succeeded in removing the energy of the curse."));
			await dialog.Msg(L("But, her method only works on the curse of gluttony."));
		});

		// Priest of Evidence
		//-------------------------------------------------------------------------
		AddConditionalNpc(103046, L("Priest of Evidence"), "MQ05_PROOF_PRIST", "d_cathedral_54", 1590, -1935, 358, c => c.Quests.Has(20340), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest of Evidence"));

			if (character.Quests.IsActive(Mq05Part3) && character.Quests.IsCompletable(Mq05Part3))
			{
				await dialog.Msg(L("The darkness did not cross with you. The way in is yours."));
				await dialog.CompleteQuest(Mq05Part3);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Mq06Part3) && character.Quests.MeetsPrerequisites(Mq06Part3))
			{
				var answer = await dialog.SelectQuestOffer(Mq06Part3, L("You've proven that you are the Revelator of the goddesses. You may enter the room where the revelation is located."),
					Option(L("Go in"), "accept"),
					Option(L("Not yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06Part3);
					await dialog.Msg(L("Open the Spirit's Scripture at the altar of the revelation. Nothing else is needed."));
				}
				return;
			}

			if (character.Quests.IsActive(ToVelniasPrison))
			{
				await dialog.Msg(L("The Demon Prison lies past Gytis, and Gytis is reached from Klaipeda."));
				return;
			}

			await dialog.Msg(L("The room beyond is open to you now. It will not be open to anything else."));
		});

		// The altar of the revelation
		//-------------------------------------------------------------------------
		AddConditionalNpc(154043, L("Altar of the Revelation"), "CHATHEDRAL_FINAL_NPC", "d_cathedral_54", 1549.33, -1358.99, 0, c => c.Quests.Has(Mq06Part3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of the Revelation"));

			if (character.Quests.IsActive(Mq06Part3) && character.Quests.IsCompletable(Mq06Part3))
			{
				await dialog.Msg(L("The Revelation of the Great Cathedral is yours, and Aurelius' spirit has gone with it."));
				await dialog.CompleteQuest(Mq06Part3);
				character.AddStatPoints(3);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq06Part3))
			{
				await dialog.Msg(L("Open the Spirit's Scripture here, and the goddess will answer."));
				character.Quests.ClearQuestTrack(Mq06Part3);
				character.Quests.StartQuestTrack(Mq06Part3);
				return;
			}

			await dialog.Msg(L("The altar the Great Cathedral was built around."));
		});

		// Maven's Message
		//-------------------------------------------------------------------------
		AddNpc(47254, L("Maven's Message"), "CHATHEDRAL54_MQ06_BOOK", "d_cathedral_54", 1514, -1900, 88, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Maven's Message"));

			if (character.Quests.IsActive(Mq05Part3) && !character.Quests.IsCompletable(Mq05Part3))
			{
				var proved = await character.TimeActions.StartAsync(L("Standing the Verification Test..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (proved != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq05Part3, "passTheTest");
				character.ServerMessage(L("The test is over, and the Priest of Evidence is waiting."));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("Words Maven left for whoever came this far."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The gate Maven's last test is staged at, past the portal.
		AddQuestTrigger("CHATHEDRAL54_MQ05_PART3_ARRIVE", "d_cathedral_54", 1549.08, -1999.04, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05Part3) && !character.Quests.IsCompletable(Mq05Part3))
				character.Quests.StartQuestTrack(Mq05Part3);

			await Task.CompletedTask;
		});

		// The gate into the Demon Prison district, at the far end of Gytis.
		AddQuestTrigger("CATHEDRAL_TO_VELNIASPRISON_ARRIVE", "f_farm_47_2", -1615.76, -1192.17, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(ToVelniasPrison) && !character.Quests.IsCompletable(ToVelniasPrison))
			{
				character.Quests.CompleteObjective(ToVelniasPrison, "reachDemonPrison");
				character.ServerMessage(L("You have reached the gate into Demon Prison District 1."));
			}

			await Task.CompletedTask;
		});

		// The wall sealing the room of the revelation
		//-------------------------------------------------------------------------
		AddConditionalNpc(MonsterId.HiddenWall_10_170_300, "", "CATHEDRAL54_HIDDEN_WALL", "d_cathedral_54", 1584.30, -1864.70, 90, c => !c.Quests.HasCompleted(Mq05Part3));
	}

	/// <summary>
	/// The bishop as he waits at Uola Chapel, who owns the corridor's middle
	/// chain and the step into the Sanctuary.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task UolaBishop(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bishop Aurelius' Spirit"));
		dialog.SetPortrait("Dlg_port_aurelius");

		if (character.Quests.IsActive(Mq02))
		{
			if (!character.Quests.IsCompletable(Mq02))
				character.Quests.CompleteObjective(Mq02, "findTheKey");

			await dialog.Msg(L("We have trouble. Hurry to Uola Chapel. I will tell you more when you get there."));
			await dialog.CompleteQuest(Mq02);
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			var answer = await dialog.SelectQuestOffer(Mq03, L("You are late. Maven's secret here is already being attacked. If we lose the Holy Symbol of Spiritual Power, we will never be able to get the third key."),
				Option(L("I will retrieve the Holy Symbol of Spiritual Power"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq03);
				await dialog.Msg(L("I regret that I could not do anything. We should get the Holy Symbol of Spiritual Power fast."));
			}
			return;
		}

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("It is very fortunate that the Holy Symbol of Spiritual Power is alright. However, I can't seem to sense its magical power anymore, likely due to the long passage of time."),
				Option(L("I will recharge the Holy Symbol of Spiritual Power"), "accept"),
				Option(L("About the divine magical power and the demons"), "explain"),
				Option(L("Tell him that it doesn't make sense to recharge magical power using demons"), "leave")
			);

			if (answer == "explain")
			{
				await dialog.Msg(L("I would say that was the wisdom of Maven. Maven thought that the Great Cathedral would not be normal all the time."));
				await dialog.Msg(L("So if we could just recharge the magical power, any kinds of magics will be okay. Even if that's a magical power from the demons."));
				return;
			}

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				await dialog.Msg(L("Defeat the demons nearby using the Holy Symbol of Spiritual Power. This will collect the magical power that is needed to obtain the third key."));
				await dialog.Msg(L("Go to Karuna Altar when you've recharged the Holy Symbol of Spiritual Power. The third key is waiting for you."));
			}
			return;
		}

		if (!character.Quests.Has(Cathedral56Mq01) && character.Quests.MeetsPrerequisites(Cathedral56Mq01))
		{
			var answer = await dialog.SelectQuestOffer(Cathedral56Mq01, L("The number of Maven's keys that remain now are two. The secret for one of those keys cannot be solved right now, with the condition of the Cathedral as it is. We have only one method, using the demons."),
				Option(L("Ask him what you should look for"), "accept"),
				Option(L("I don't have a good feeling about it"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Cathedral56Mq01);
				await dialog.Msg(L("First, it would be better to find some documents at the Sanctuary."));
				await dialog.Msg(L("I remember there are documents there about methods of disguising as demons."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("Seven of them are at the symbol. Drive every one of them off it."));
			character.Quests.ReplayQuestTrack(Mq03);
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("The symbol drinks whatever is spilled near it, even a demon's power."));
			return;
		}

		await dialog.Msg(L("I tried my best to hide Maven's secret, but Naktis is also staying at the Great Cathedral too long."));
	}

	/// <summary>
	/// Adds one of the two footholds the Platform of Benevolence answers to.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddFoothold(int number, string name, double x, double z)
	{
		AddQuestTrigger("CHATHEDRAL54_MQ01_FOOT" + number, "d_cathedral_54", x, z, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq01) || character.Quests.IsCompletable(Mq01))
				return;

			if (character.Variables.Perm.GetBool(FootholdVar + number, false))
				return;

			character.Variables.Perm.Set(FootholdVar + number, true);
			character.ServerMessage(LF("The {0} sinks under your weight.", name));

			if (!character.Variables.Perm.GetBool(FootholdVar + 1, false) || !character.Variables.Perm.GetBool(FootholdVar + 2, false))
				return;

			character.Quests.CompleteObjective(Mq01, "solveSecret");
			character.ServerMessage(L("Both footholds are down, and the Platform of Benevolence has moved."));

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Adds one of the documents scattered over the reading floor.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="monsterId"></param>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	private void AddOldBook(int number, int monsterId, string name, double x, double z, double direction)
	{
		AddConditionalNpc(monsterId, name, "CHATHEDRAL54_SQ01_PART1_BOOK" + number, "d_cathedral_54", x, z, direction, c => c.Quests.IsActive(Sq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(name);

			if (character.Variables.Perm.GetBool(BookVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already taken what was readable here*{/}"));
				return;
			}

			var gathered = await character.TimeActions.StartAsync(L("Gathering the loose pages..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (gathered != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(BookVar + number, true);
			character.Inventory.Add(ItemId.CHATHEDRAL54_SQ01_PART1_ITEM, DocumentsPerBook, InventoryAddType.PickUp);
			character.ServerMessage(LF("Documents recovered: {0}/{1}", character.Inventory.CountItem(ItemId.CHATHEDRAL54_SQ01_PART1_ITEM), DocumentsNeeded));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20310: The Secret of the Machine
//-----------------------------------------------------------------------------
public class Cathedral54Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20310);
		SetName(L("The Secret of the Machine"));
		SetDescription(L("Maven built machines as well as chapels, and the demons never learned to read them."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_PART1_BISHOP", "d_cathedral_54", L("Talk to Bishop Aurelius at the Grand Corridor"), L("Acquired Maven's First Key. Talk to Bishop Aurelius who is waiting at the Grand Corridor."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_PART1_BISHOP", "d_cathedral_54", L("Solve the secret at the Platform of Benevolence"), L("Solve the secret at the Platform of Benevolence by stepping on the footholds of the Arka Chapel and the Reception Room. If you have a trouble solving for the secret, get some help from Bishop Aurelius."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_MQ01_PUZZLE", "d_cathedral_54", L("Obtain Maven's Second Key"), L("The secret is solved. Obtain Maven's Second Key at the Platform of Benevolence."));

		AddPrerequisite(new QuestStatusPrerequisite(20305, QuestStatus.Completed));

		AddObjective("solveSecret", L("Solve the secret at the Platform of Benevolence"), new ManualObjective());

		AddReward(new ItemReward("CHATHEDRAL54_MQ01_PART1_ITEM", 1));
		AddReward(new ItemReward("expCard8", 2));
	}
}

// 20311: Maven's Device (2)
//-----------------------------------------------------------------------------
public class Cathedral54Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20311);
		SetName(L("Maven's Device (2)"));
		SetDescription(L("The bishop has something urgent to say, and he says it at Uola Chapel."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Talk with the Bishop"), L("Talk to the Bishop."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Find the Key"), L("Find the Key."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Find the Key"), L("Find the Key."));

		AddPrerequisite(new QuestStatusPrerequisite(20310, QuestStatus.Completed));

		AddObjective("findTheKey", L("Talk with the Bishop"), new ManualObjective());
	}
}

// 20312: Critical Situation
//-----------------------------------------------------------------------------
public class Cathedral54Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20312);
		SetName(L("Critical Situation"));
		SetDescription(L("Naktis' servants reached the Holy Symbol of Spiritual Power first."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Talk to Bishop Aurelius at Uola Chapel"), L("Pass through the Penitence Route at the upper side of the Grand Corridor and reenter into the Grand Corridor to meet Aurelius."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Defeat the demons that are after the Holy Symbol of Spiritual Power"), L("Naktis' servants are trying to solve Maven's secret by obtaining the Holy Symbol of Spiritual Power. Defeat the demons and obtain the Symbol of Spiritual Power."));
		SetPhase(QuestStatus.Success, "CATHEDRAL54MQ02_OBJECT", "d_cathedral_54", L("Obtain the Holy Symbol of Spiritual Power"), L("You've defeated all the demons that are going after the Holy Symbol of Spiritual Power. Obtain the Holy Symbol of Spiritual Power."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL54_MQ03_PART2_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20310, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons that are after the Holy Symbol of Spiritual Power"), new KillObjective(7, "Stoulet_blue") { LayerOnly = true });

		AddReward(new ItemReward("CATHEDRAL54_MQ02_PART2_ITEM", 1));
		AddReward(new ItemReward("expCard8", 1));
	}
}

// 20313: Karuna Altar Key
//-----------------------------------------------------------------------------
public class Cathedral54Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20313);
		SetName(L("Karuna Altar Key"));
		SetDescription(L("The symbol is empty of power, and the demons are full of it."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Talk to Bishop Aurelius"), L("Fortunately, the Holy Symbol of Spiritual Power seems to be okay. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", L("Recharge the Holy Symbol of Spiritual Power"), L("Recharge the Holy Symbol of Spiritual Power by deploying it and defeating monsters nearby. If you have difficulty recharging the symbol, get help from Bishop Aurelius."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", L("Obtain Maven's Third Key at Karuna Altar"), L("Obtain Maven's Third Key using the Holy Symbol of Spiritual Power at Karuna Altar."));

		AddPrerequisite(new QuestStatusPrerequisite(20312, QuestStatus.Completed));

		// The client records no kill count for the recharge; the port sets one
		// so the phase asks for the fight its own text describes.
		AddObjective("rechargeSymbol", L("Recharge the Holy Symbol of Spiritual Power"), new KillObjective(10, "Stoulet_blue"));

		AddReward(new ItemReward("CHATHEDRAL54_MQ04_PART2_ITEM", 1));
		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("CATHEDRAL54_MQ02_PART2_ITEM", 1));
	}
}

// 20314: Dusty Old Books
//-----------------------------------------------------------------------------
public class Cathedral54Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20314);
		SetName(L("Dusty Old Books"));
		SetDescription(L("Everything written about Naktis is on the floor of the reading room."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_SQ04_PART2", "d_cathedral_54", L("Talk to Priest Ruodell"), L("Priest Ruodell needs your help. Talk to Priest Ruodell."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_SQ04_PART2", "d_cathedral_54", L("Look for documents that are related to Naktis"), L("Find the documents that will be helpful undoing Naktis' curse."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_SQ04_PART2", "d_cathedral_54", L("Talk to Priest Ruodell"), L("You've found documents that would help removing the Naktis' curse. Talk to Priest Ruodell."));

		AddPrerequisite(new LevelPrerequisite(132));

		AddObjective("collectDocuments", L("Look for documents that are related to Naktis"), new CollectItemObjective("CHATHEDRAL54_SQ01_PART1_ITEM", 10));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL54_SQ01_PART1_ITEM"));
	}
}

// 20315: Eyes Off for a Moment
//-----------------------------------------------------------------------------
public class Cathedral54Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20315);
		SetName(L("Eyes Off for a Moment"));
		SetDescription(L("The report Yosana owed Aden is inside a Stoulet somewhere."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_SQ03_PART1", "d_cathedral_54", L("Talk to Priest Yosana"), L("Priest Yosana seems to be at a loss. Talk to Priest Yosana again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_SQ03_PART1", "d_cathedral_54", L("Retrieve Priest Yosana's Research"), L("Priest Yosana told you that her research documents have disappeared and she suspects the demons have a role. Defeat the demons and retrieve her research documents."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_SQ03_PART1", "d_cathedral_54", L("Talk to Priest Yosana"), L("You've found all of Priest Yosana's documents. Hand them over to Priest Yosana."));

		AddPrerequisite(new LevelPrerequisite(130));

		AddPityDrop("CATHEDRAL54_SQ02_ITEM", 0.4f, 5, 1, "Stoulet_blue");

		AddObjective("retrieveResearch", L("Retrieve Priest Yosana's Research"), new CollectItemObjective("CATHEDRAL54_SQ02_ITEM", 6));

		AddReward(new ItemReward("PRIST_REPORT01", 1));
		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CATHEDRAL54_SQ02_ITEM"));
	}
}

// 20316: More and More
//-----------------------------------------------------------------------------
public class Cathedral54Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20316);
		SetName(L("More and More"));
		SetDescription(L("The reagent needs solvent, and the solvent has to be taken out of demons."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Talk to Priest Daram"), L("Priest Daram needs your help. Talk to her."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Collect solvents by using the Scroll of Purification on the demons"), L("Priest Daram told you that many solvents are needed to make the reagent. Use the scroll of purification on the demons and collect the solvents."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Talk to Priest Daram"), L("You've collected enough turbid solvents. Hand them over to Priest Daram."));

		AddPrerequisite(new LevelPrerequisite(130));
		AddPrerequisite(new QuestStatusPrerequisite(20317, QuestStatus.Completed));

		// The client leaves the solvent to its own script; the port drops it
		// from the demons the phase names.
		AddPityDrop("CHATHEDRAL54_SQ03_PART1_ITEM", 0.4f, 5, 1, "Stoulet_blue");

		AddObjective("collectSolvents", L("Collect solvents by using the Scroll of Purification on the demons"), new CollectItemObjective("CHATHEDRAL54_SQ03_PART1_ITEM", 5));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL54_SQ03_PART1_ITEM", 5));
		AddReward(new TakeItemReward("CATHEDRAL54_SQ04_ITEM", 1));
	}
}

// 20317: Tremendous Effects
//-----------------------------------------------------------------------------
public class Cathedral54Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20317);
		SetName(L("Tremendous Effects"));
		SetDescription(L("Priest Daram's reagent has never been tried on anything that could object."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Talk to Priest Daram"), L("Priest Daram is thinking about something. Talk to her."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Use the reagent Priest Daram created on the demons"), L("Priest Daram told you that she successfully created a reagent, but she needs to experiment with it. Test the reagent on the demons."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_SQ01_PART1", "d_cathedral_54", L("Talk to Priest Daram"), L("The reagent caused various effects. Tell Priest Daram about the reagent's effects."));

		AddPrerequisite(new LevelPrerequisite(130));

		// The client records no count for the test; the port asks for the
		// demons the phase names.
		AddObjective("testReagent", L("Use the reagent Priest Daram created on the demons"), new KillObjective(5, "Stoulet_blue"));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CATHEDRAL54_SQ04_PART2_ITEM", 1));
	}
}

// 20318: Surprise Attack
//-----------------------------------------------------------------------------
public class Cathedral54Sq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20318);
		SetName(L("Surprise Attack"));
		SetDescription(L("Riteris comes for the Karuna Altar the moment its key is gone."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", L("Check the Karuna Altar"), L("Check the Karuna Altar."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", L("Defeat Naktis' servants that suddenly appeared"), L("Defeat Naktis' servants that suddenly appeared."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL54_MQ04_PART2", "d_cathedral_54", L("Defeat Naktis' servants that suddenly appeared"), L("Defeat Naktis' servants that suddenly appeared."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL54_SQ05_PART2_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(132));
		AddPrerequisite(new QuestStatusPrerequisite(20313, QuestStatus.Completed));

		AddObjective("killRiteris", L("Defeat Naktis' servant, Riteris"), new KillObjective(1, "boss_Riteris") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 1));
	}
}

// 20329: Adapting to Circumstances (1)
//-----------------------------------------------------------------------------
public class Cathedral56Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20329);
		SetName(L("Adapting to Circumstances (1)"));
		SetDescription(L("The fourth key cannot be taken honestly, so the bishop stops trying to."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54", "d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL54_BISHOP_AFTER", "d_cathedral_54", L("Talk to Bishop Aurelius"), L("Acquired the third key. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Look for the documents that describe ways to transform into a demon at Sanctuary"), L("Bishop Aurelius told you that before you find the next key, you should look for documents that describe ways to transform into a demon. Look for the documents that describe ways to transform into a demon."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You've found the documents that enable you to transform into a demon. Talk to Bishop Aurelius again."));

		AddPrerequisite(new QuestStatusPrerequisite(20313, QuestStatus.Completed));

		AddObjective("collectDocuments", L("Look for the documents that describe ways to transform into a demon"), new CollectItemObjective("CHATHEDRAL56_MQ01_ITEM", 5));

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_MQ01_ITEM", 5));
	}
}

// 20341: The Bishop's Last Mission (2)
//-----------------------------------------------------------------------------
public class Cathedral54Mq06Part3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20341);
		SetName(L("The Bishop's Last Mission (2)"));
		SetDescription(L("The Revelation of the Great Cathedral, and the last thing Aurelius stayed for."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(false);

		SetPhase(QuestStatus.Possible, "MQ05_PROOF_PRIST", "d_cathedral_54", L("Obtain the revelation of the Great Cathedral"), L("Obtain the revelation of the Great Cathedral by opening the Spirit's Scripture at the altar of the revelation."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL_FINAL_NPC", "d_cathedral_54", L("Obtain the revelation of the Great Cathedral"), L("Obtain the revelation of the Great Cathedral by opening the Spirit's Scripture at the altar of the revelation."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL_FINAL_NPC", "d_cathedral_54", L("Obtain the revelation of the Great Cathedral"), L("Obtain the revelation of the Great Cathedral by opening the Spirit's Scripture at the altar of the revelation."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL54_MQ06_PART3_TRACK", 2000, autoStart: false);

		AddPrerequisite(new QuestStatusPrerequisite(20340, QuestStatus.Completed));

		AddObjective("hearTheRevelation", L("Obtain the revelation of the Great Cathedral"), new ManualObjective());

		AddReward(new ItemReward("stonetablet06", 1));
		AddReward(new ItemReward("expCard8", 3));
		AddReward(new StatPointReward(3));
		AddReward(new TakeItemReward("CHATHEDRAL53_MQ03_ITEM", 1));
	}
}

// 50027: To Demon Prison District 1
//-----------------------------------------------------------------------------
public class CathedralToVelniasprisonQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50027);
		SetName(L("To Demon Prison District 1"));
		SetDescription(L("The Revelation of the Great Cathedral names the Fortress of the Land, and the road to it runs through Gytis."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_54", "f_farm_47_2");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "MQ05_PROOF_PRIST", "d_cathedral_54", L("Move to Demon Prison District 1"), L("Move to Demon Prison District 1."));
		SetPhase(QuestStatus.InProgress, "FARM_47_2_TO_VELNIASP511", "f_farm_47_2", L("Move to Demon Prison District 1"), L("You must pass Gytis Settlement Area in order to reach the Demon Prison. As you pass through the area from Klaipeda, assist the people there with their troubles."));
		SetPhase(QuestStatus.Success, "FARM_47_2_TO_VELNIASP511", "f_farm_47_2", L("Move to Demon Prison District 1"), L("You must pass Gytis Settlement Area in order to reach the Demon Prison. As you pass through the area from Klaipeda, assist the people there with their troubles."));

		AddPrerequisite(new QuestStatusPrerequisite(20341, QuestStatus.Completed));

		AddObjective("reachDemonPrison", L("Move to Demon Prison District 1"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The arrival is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}
