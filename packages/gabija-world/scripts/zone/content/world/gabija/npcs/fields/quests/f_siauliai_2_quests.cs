//--- Melia Script ----------------------------------------------------------
// East Siauliai Woods Quest NPCs
//--- Description -----------------------------------------------------------
// The knights, guards and supply soldiers the map's field quests run on.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FSiauliai2QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Camp4 = new QuestId(1031);
	private readonly static QuestId Reclaim1 = new QuestId(1032);
	private readonly static QuestId Reclaim2 = new QuestId(1033);
	private readonly static QuestId Reclaim3 = new QuestId(1034);
	private readonly static QuestId Reclaim6 = new QuestId(1036);
	private readonly static QuestId Reclaim7 = new QuestId(1037);
	private readonly static QuestId Request1 = new QuestId(1038);
	private readonly static QuestId Request2 = new QuestId(1039);
	private readonly static QuestId Request4 = new QuestId(1041);
	private readonly static QuestId Request5 = new QuestId(1042);
	private readonly static QuestId Request6 = new QuestId(1043);
	private readonly static QuestId Request7 = new QuestId(1044);
	private readonly static QuestId Act2Diss1 = new QuestId(4203);
	private readonly static QuestId Act2Diss1Boss = new QuestId(20131);

	protected override void Load()
	{
		// Knight Ares
		//-------------------------------------------------------------------------
		AddNpc(20125, L("Knight Ares"), "SIAUL_EAST_MANAGER", "f_siauliai_2", 167, 697, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Knight Ares"));
			dialog.SetPortrait("Dlg_port_OFFICER_IN_TACTICS");

			if (character.Quests.IsActive(Reclaim1) && character.Quests.IsCompletable(Reclaim1))
			{
				await dialog.Msg(L("Well done. Now we move on to the main task."));
				character.Quests.Complete(Reclaim1);
				return;
			}

			if (character.Quests.IsActive(Camp4) && character.Quests.IsCompletable(Camp4))
			{
				await dialog.Msg(L("Are you hurt anywhere? I am grateful you killed the Poata, but that was truly reckless."));

				var pick = await dialog.Select(L("Take one of the weapons we recovered."),
					Option(L("Aras' Falchion"), "swd"),
					Option(L("Soldier's Long Rod"), "stf"),
					Option(L("Soldier's Short Bow"), "tbw"),
					Option(L("Soldier's Iron Club"), "mac")
				);

				switch (pick)
				{
					case "swd": character.Quests.SelectReward(Camp4, 102115); break;
					case "stf": character.Quests.SelectReward(Camp4, 142110); break;
					case "tbw": character.Quests.SelectReward(Camp4, 162112); break;
					case "mac": character.Quests.SelectReward(Camp4, 202111); break;
				}

				character.Quests.Complete(Camp4);
				return;
			}

			if (character.Quests.IsActive(Request7) && character.Quests.IsCompletable(Request7))
			{
				await dialog.Msg(L("The fighting at the mining village seems very bad. It is too late to move troops, so you must go and support the mining village yourself."));
				character.Quests.Complete(Request7);
				return;
			}

			if (!character.Quests.Has(Reclaim1) && character.Quests.MeetsPrerequisites(Reclaim1))
			{
				await dialog.Msg(L("You mean to go to the crystal mine to find the light of salvation? But now is not a good time. The Bube horde is pouring out of the crystal mine."));
				await dialog.Msg(L("And here in the eastern woods, monsters are multiplying and threatening even Klaipeda. Because of that, the mining village in between has become the middle of a battlefield."));
				await dialog.Msg(L("We split our forces - some to hold the mining village, and I with the rest am investigating why the monsters are multiplying in the eastern woods."));
				await dialog.Msg(L("So we cannot spare an escort for the Revelators. And yet we cannot disobey the bishop and the knight commander."));

				var answer = await dialog.Select(L("Hmm, how about this. Since we must test whether you can reach the mining village safely without an escort... would you help with our investigation here?"),
					Option(L("Say you will accept the offer"), "accept"),
					Option(L("Say you will wait until it is settled"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Reclaim1);
					await dialog.Msg(L("First, retake the Bulbes farm the Pokubu have overrun. I trust this much will be simple for you."));
					return;
				}
			}

			if (!character.Quests.Has(Request1) && character.Quests.MeetsPrerequisites(Request1))
			{
				await dialog.Msg(L("The next task is finding why the monsters multiplied. The Popolion are greedy, so you may well find a decisive clue."));

				var answer = await dialog.Select(L("Will you search the Popolion for a clue?"),
					Option(L("Say you will find a clue"), "accept"),
					Option(L("Say you will do it later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Request1);
					character.Quests.CompleteObjective(Request1, "findClue");
					await dialog.Msg(L("If you find anything that might be a clue, take it to the operations officer up there."));
					return;
				}
			}

			if (!character.Quests.Has(Camp4) && character.Quests.MeetsPrerequisites(Camp4))
			{
				await dialog.Msg(L("Have you by any chance seen a Poata cub? We must drive it far away quickly, or the mother will keep prowling about."));

				var answer = await dialog.Select(L("Will you deal with the Poata at the camp?"),
					Option(L("Say you have not seen it"), "accept"),
					Option(L("Say you will not bother"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Camp4);
					return;
				}
			}

			if (!character.Quests.Has(Request7) && character.Quests.MeetsPrerequisites(Request7))
			{
				await dialog.Msg(L("You really killed the Bube Fighter? Then the monsters will no longer multiply. We can rest a little easier now."));

				var answer = await dialog.Select(L("Will you go on to the mining village?"),
					Option(L("Say you will go to the mining village"), "accept"),
					Option(L("Ask for a little time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Request7);
					return;
				}
			}

			if (character.Quests.IsActive(Reclaim1))
			{
				await dialog.Msg(L("If you cannot even deal with Pokubu, you had best give up on entering the mining village."));
				character.Quests.ReplayQuestTrack(Reclaim1);
				return;
			}

			if (character.Quests.IsActive(Request1))
			{
				await dialog.Msg(L("The mining village is a problem, but if the eastern woods are like this too, I cannot guarantee Klaipeda's safety. We must hope the mining village holds."));
				return;
			}

			if (character.Quests.IsActive(Camp4))
			{
				await dialog.Msg(L("It came for its cub. Watch for it near the camp."));
				return;
			}

			if (character.Quests.IsActive(Request7))
			{
				await dialog.Msg(L("The Bube are pushing the refugees back. Clear the monsters chasing them."));
				character.Quests.ReplayQuestTrack(Request7);
				return;
			}

			await dialog.Msg(L("The mining village is in the middle of the fighting. We cannot spare an escort yet."));
		});

		// Outpost Border Guard
		//-------------------------------------------------------------------------
		AddNpc(10032, L("Outpost Border Guard"), "SIAUL_EAST_SOLDIER9", "f_siauliai_2", 133, 672, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Outpost Border Guard"));

			if (!character.Quests.Has(Reclaim2) && character.Quests.MeetsPrerequisites(Reclaim2))
			{
				await dialog.Msg(L("The mining village worries me too, but the fighting never lets us rest. If only the Chupacabra were dealt with, things would get easier."));

				var answer = await dialog.Select(L("Will you hunt the Chupacabra for the guard?"),
					Option(L("Say you will hunt the Chupacabra"), "accept"),
					Option(L("Tell him to see to it himself"), "leave"),
					Option(L("Ask about Ares' unit"), "explain")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("We are tasked with finding why the monsters in the eastern woods suddenly multiplied. We also handle supply runs to the mining village, where the fighting continues."));
					await dialog.Msg(L("Another knight leads there. Honestly, I would have thought Sir Ares would settle it quickly, but the battle drags on."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Reclaim2);

				return;
			}

			if (character.Quests.IsActive(Reclaim2))
			{
				if (!character.Quests.IsCompletable(Reclaim2))
				{
					await dialog.Msg(L("Where the Chupacabra are, other monsters always gather. The mining village worries me, but there are too many things to mind."));
					return;
				}

				await dialog.Msg(L("Thank you. Thanks to you, I feel I have my strength back."));
				character.Quests.Complete(Reclaim2);
				return;
			}

			if (!character.Quests.Has(Reclaim3) && character.Quests.MeetsPrerequisites(Reclaim3))
			{
				await dialog.Msg(L("Trusting your skill, I have one more request. The supply depot - I want its Chupacabra driven out as well."));

				var answer = await dialog.Select(L("Will you retake the supply depot?"),
					Option(L("Say you will retake it"), "accept"),
					Option(L("Refuse"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Reclaim3);
					await dialog.Msg(L("Truly, you make me wonder if you are not the goddess's messenger. In any case, the supply depot is further down than the farm."));
				}
				return;
			}

			if (character.Quests.IsActive(Reclaim3))
			{
				if (!character.Quests.IsCompletable(Reclaim3))
				{
					await dialog.Msg(L("The mining village worries me, but things here are not good either. We must find a way before the monsters grow more."));
					character.Quests.ReplayQuestTrack(Reclaim3);
					return;
				}

				await dialog.Msg(L("Finished already? I have never seen anyone win results so quickly."));
				character.Quests.Complete(Reclaim3);
				return;
			}

			await dialog.Msg(L("The supply depot is further down than the farm. Mind the Chupacabra on the way."));
		});

		// Supply Officer
		//-------------------------------------------------------------------------
		AddNpc(20016, L("Supply Officer"), "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", 660.71, -453.27, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Supply Officer"));

			if (!character.Quests.Has(Act2Diss1) && character.Quests.MeetsPrerequisites(Act2Diss1))
			{
				await dialog.Msg(L("I am recovering the supplies bound for the mining village. The monsters stole them all."));
				await dialog.Msg(L("But some of the crates contain dangerous explosives, so the recovery is very hard."));

				var answer = await dialog.Select(L("Will you help recover the supplies?"),
					Option(L("Say you will help"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Act2Diss1);
					character.Quests.CompleteObjective(Act2Diss1, "recoverSupplies");
				}

				return;
			}

			if (character.Quests.IsActive(Act2Diss1))
			{
				await dialog.Msg(L("They cannot have gone far. If only a monster had stolen one and set it off, it would have been better."));
				character.Quests.Complete(Act2Diss1);
				return;
			}

			if (!character.Quests.Has(Act2Diss1Boss) && character.Quests.MeetsPrerequisites(Act2Diss1Boss))
			{
				await dialog.Msg(L("That is the last of them... or so I thought. Something is moving in the brush."));
				character.Quests.Start(Act2Diss1Boss);
				return;
			}

			if (character.Quests.IsActive(Act2Diss1Boss))
			{
				if (!character.Quests.IsCompletable(Act2Diss1Boss))
				{
					await dialog.Msg(L("Look out - it is still there!"));
					character.Quests.ReplayQuestTrack(Act2Diss1Boss);
					return;
				}

				await dialog.Msg(L("Truly, that was nearly a disaster. How well that great bulk had hidden."));
				character.Quests.Complete(Act2Diss1Boss);
				return;
			}

			if (!character.Quests.Has(Reclaim6) && character.Quests.MeetsPrerequisites(Reclaim6))
			{
				await dialog.Msg(L("Since you are helping, please deal with the large gray Chupacabra too. It is one of the main thieves of the supplies."));

				var answer = await dialog.Select(L("Will you deal with the large gray Chupacabra?"),
					Option(L("Say you will deal with it"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Reclaim6);
					await dialog.Msg(L("Kill the Chupacabra and the large gray one is sure to appear. I leave it to you."));
				}
				return;
			}

			if (character.Quests.IsActive(Reclaim6))
			{
				if (!character.Quests.IsCompletable(Reclaim6))
				{
					await dialog.Msg(L("Finding why the monsters multiplied is all well and good, but the supplies to the mining village being cut off worries me more."));
					return;
				}

				await dialog.Msg(L("Thank you. Now the work will go a little more easily."));
				character.Quests.Complete(Reclaim6);
				return;
			}

			if (!character.Quests.Has(Reclaim7) && character.Quests.MeetsPrerequisites(Reclaim7))
			{
				await dialog.Msg(L("The Weaver movement by the lower stream looks suspicious. As if they mean to interfere with the supplies to the mining village."));

				var answer = await dialog.Select(L("Will you clear the Weaver below the depot?"),
					Option(L("Say you will clear them"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Reclaim7);

				return;
			}

			if (character.Quests.IsActive(Reclaim7))
			{
				if (!character.Quests.IsCompletable(Reclaim7))
				{
					await dialog.Msg(L("This is not a situation I am used to. If it comes to it, we may have to give up our own supplies and fend for ourselves until the next shipment."));
					character.Quests.ReplayQuestTrack(Reclaim7);
					return;
				}

				await dialog.Msg(L("Thank you. With the Revelator helping like this, we will not be outdone either."));
				character.Quests.Complete(Reclaim7);
				return;
			}

			await dialog.Msg(L("The supplies to the mining village are in a bad way. I have had no rest."));
		});

		// Operations Officer
		//-------------------------------------------------------------------------
		AddNpc(20014, L("Operations Officer"), "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", -1290, 928, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Operations Officer"));

			if (character.Quests.IsActive(Request1))
			{
				await dialog.Msg(L("Did Sir Ares send you? This is a piece of Bube cloth. Hm... so that is what it is."));
				character.Quests.Complete(Request1);
				return;
			}

			if (!character.Quests.Has(Request2) && character.Quests.MeetsPrerequisites(Request2))
			{
				await dialog.Msg(L("Perhaps the Bube of the mining village have pushed all the way into these woods. That would explain the unnatural increase in monsters."));
				await dialog.Msg(L("I should ask Sir Ares to search other places for Bube. Meanwhile, please look over the north."));

				var answer = await dialog.Select(L("Will you scout the northern woods?"),
					Option(L("Say you will check"), "accept"),
					Option(L("Tell him to see to it himself"), "leave"),
					Option(L("Ask about the Bube"), "explain")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("They are not clever, but they form packs and have their own society. They used to be seen only occasionally deep in the mine... perhaps they mean to expand their territory."));
					await dialog.Msg(L("Whatever the reason, the Bube widening their range is serious. If the old inhabitants lose their home, they will be pushed into ours."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Request2);

				return;
			}

			if (character.Quests.IsActive(Request2))
			{
				if (!character.Quests.IsCompletable(Request2))
				{
					await dialog.Msg(L("We have never sent a search party north. I would normally send a soldier, but the matter is urgent, so I ask you."));
					character.Quests.ReplayQuestTrack(Request2);
					return;
				}

				await dialog.Msg(L("You saw the Bube Fighter but lost it. Reports have come in from the south too that a Bube Fighter appeared."));
				await dialog.Msg(L("For now, meet the scout at the bridge below the Bulbes farm. If the Bube Fighter is lost, this will become a long, hard fight."));
				character.Quests.Complete(Request2);
				return;
			}

			await dialog.Msg(L("The Bube of the mining village may be pushing into these woods. It does not sit well."));
		});

		// Supply Soldier
		//-------------------------------------------------------------------------
		AddNpc(20011, L("Supply Soldier"), "SIAUL_EAST_SOLDIER5", "f_siauliai_2", 741, 411, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Supply Soldier"));

			if (!character.Quests.Has(Request4) && character.Quests.MeetsPrerequisites(Request4))
			{
				await dialog.Msg(L("Among the supplies to be sent to the mining village is Weaver Claw, but I have had no chance to get it. What am I to do."));

				var answer = await dialog.Select(L("Will you gather the Weaver Claws for him?"),
					Option(L("Say you will gather them"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Request4);

				return;
			}

			if (character.Quests.IsActive(Request4))
			{
				if (!character.Quests.IsCompletable(Request4))
				{
					await dialog.Msg(L("Honestly, a supply soldier having to gather the supplies himself is nonsense. This should have come from Klaipeda."));
					return;
				}

				await dialog.Msg(L("Thank you. Unexpected help always feels good."));
				character.Quests.Complete(Request4);
				return;
			}

			if (!character.Quests.Has(Request5) && character.Quests.MeetsPrerequisites(Request5))
			{
				await dialog.Msg(L("Truthfully, if not for the Pokubu, everything would be fine. They make such a nuisance that I have not done the supply recovery work properly - not a single one, in fact."));

				var answer = await dialog.Select(L("Will you deal with the Pokubu?"),
					Option(L("Say you will deal with them"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Request5);

				return;
			}

			if (character.Quests.IsActive(Request5))
			{
				if (!character.Quests.IsCompletable(Request5))
				{
					await dialog.Msg(L("I cannot understand why they suddenly target only the supplies. Is someone commanding them?"));
					return;
				}

				await dialog.Msg(L("Thank you. I envy that skill of yours."));
				character.Quests.Complete(Request5);
				return;
			}

			await dialog.Msg(L("The supply recovery is at a standstill on account of the Pokubu."));
		});

		// Eastern Woods Scout
		//-------------------------------------------------------------------------
		AddNpc(10032, L("Eastern Woods Scout"), "SIAUL_EAST_SOLDIER8", "f_siauliai_2", 1242, 339, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Eastern Woods Scout"));

			if (!character.Quests.Has(Request6) && character.Quests.MeetsPrerequisites(Request6))
			{
				await dialog.Msg(L("Ah, it is you. I heard about it from the operations officer. The Bube Fighter is hiding deep inside the Nudegi logging camp."));
				await dialog.Msg(L("The order to kill it has come down, but waiting for reinforcements might be safer."));

				var answer = await dialog.Select(L("Will you go after the Bube Fighter without waiting?"),
					Option(L("Say you will kill it now"), "accept"),
					Option(L("Say you will wait for reinforcements"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Request6);

				return;
			}

			if (character.Quests.IsActive(Request6))
			{
				if (!character.Quests.IsCompletable(Request6))
				{
					await dialog.Msg(L("I am only a scout, but this was the first time I saw a Bube Fighter up close."));
					character.Quests.ReplayQuestTrack(Request6);
					return;
				}

				await dialog.Msg(L("Amazing. To take that down alone. Go and report to Sir Ares at once. He will be delighted."));
				character.Quests.Complete(Request6);
				return;
			}

			await dialog.Msg(L("The Bube Fighter keeps to the deep parts of the Nudegi logging camp."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("SIAUL_EAST_RECLAIM3", "f_siauliai_2", 487.26, -309.29, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Reclaim3) && !character.Quests.IsCompletable(Reclaim3))
				character.Quests.StartQuestTrack(Reclaim3);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_RECLAIM7", "f_siauliai_2", 462, -882, 120, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Reclaim7) && !character.Quests.IsCompletable(Reclaim7))
				character.Quests.StartQuestTrack(Reclaim7);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_REQUEST2", "f_siauliai_2", -2124, 1122, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Request2) && !character.Quests.IsCompletable(Request2))
				character.Quests.StartQuestTrack(Request2);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_REQUEST6", "f_siauliai_2", 1886.66, -476.83, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Request6) && !character.Quests.IsCompletable(Request6))
				character.Quests.StartQuestTrack(Request6);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_CAMP4", "f_siauliai_2", 234, 427, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Camp4) && !character.Quests.IsCompletable(Camp4))
				character.Quests.StartQuestTrack(Camp4);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_CAMP4_2", "f_siauliai_2", 175, 363, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Camp4) && !character.Quests.IsCompletable(Camp4))
				character.Quests.StartQuestTrack(Camp4);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 1031: The Camp in Danger
//-----------------------------------------------------------------------------
public class SiaulEastCamp4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1031);
		SetName(L("The Camp in Danger"));
		SetDescription(L("Monsters pour out of the eastern woods and threaten the outpost. Knight Ares wants the Poata that came for its young put down."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Move to the eastern woods camp"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Kill the Poata at the camp"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_CAMP4_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(2));

		AddObjective("killPoata", L("Kill the Poata at the camp"), new KillObjective(1, "boss_poata"));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new SelectItemReward("SWD02_115", "STF02_110", "TBW02_112", "MAC02_111"));
	}
}

// 1032: Threats of the Eastern Woods
//-----------------------------------------------------------------------------
public class SiaulEastReclaim1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1032);
		SetName(L("Threats of the Eastern Woods"));
		SetDescription(L("Knight Ares asks the Revelators to help settle the eastern woods before they can reach the mining village."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Ask Knight Ares about the mining village"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM1", "f_siauliai_2", L("Kill the Pokubu on the farm"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Report to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM1_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(40010, QuestStatus.Completed));

		AddObjective("killPokubu", L("Kill the Pokubu on the farm"), new KillObjective(4, "Pokubu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1033: A Border Guard's Request (1)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1033);
		SetName(L("A Border Guard's Request (1)"));
		SetDescription(L("The border guard asks for help against the Chupacabra that threaten the unit."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hear the guard's plan"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hunt the Chupacabra"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Report to the border guard"));

		AddPrerequisite(new LevelPrerequisite(6));

		AddObjective("killChupacabra", L("Hunt the Chupacabra"), new KillObjective(7, "Chupacabra_Blue"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1034: A Border Guard's Request (2)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1034);
		SetName(L("A Border Guard's Request (2)"));
		SetDescription(L("The supply depot has been overrun. Clear the Chupacabra out of it."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Hear the guard's explanation"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM3", "f_siauliai_2", L("Retake the supply depot"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER9", "f_siauliai_2", L("Report to the border guard"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM3_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1033, QuestStatus.Completed));

		AddObjective("killChupacabra", L("Clear the supply depot of Chupacabra"), new KillObjective(10, "Chupacabra_Blue", "Chupacabra_Ibory"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1036: Nothing Goes as Planned (3)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1036);
		SetName(L("Nothing Goes as Planned (3)"));
		SetDescription(L("The supply officer wants the large gray Chupacabra that steals the supplies dealt with."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Hear the supply officer's request"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Kill the large gray Chupacabra"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Report to the supply officer"));

		AddPrerequisite(new QuestStatusPrerequisite(20131, QuestStatus.Completed));

		AddObjective("killElite", L("Kill the large gray Chupacabra"), new KillObjective(7, "Chupacabra_Gray_Elite"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1037: Nothing Goes as Planned (4)
//-----------------------------------------------------------------------------
public class SiaulEastReclaim7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1037);
		SetName(L("Nothing Goes as Planned (4)"));
		SetDescription(L("The Weaver by the lower stream are disrupting the supply route. Clear them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_RECLAIM7", "f_siauliai_2", L("Move below the supply depot"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Report to the supply officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_RECLAIM7_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1036, QuestStatus.Completed));

		AddObjective("killWeaver", L("Kill the Weaver"), new KillObjective(5, "Weaver"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1038: Ares' Commission (1)
//-----------------------------------------------------------------------------
public class SiaulEastRequest1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1038);
		SetName(L("Ares' Commission (1)"));
		SetDescription(L("Knight Ares wants the cause of the multiplying monsters found. Search the Popolion for a clue."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Find a clue from the Popolion"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Deliver the Bube cloth piece"));

		AddPrerequisite(new QuestStatusPrerequisite(1032, QuestStatus.Completed));

		AddObjective("findClue", L("Find a clue from the Popolion"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new TakeItemReward("SIAUL_EAST_REQUEST1_Blood"));
	}
}

// 1039: Ares' Commission (2)
//-----------------------------------------------------------------------------
public class SiaulEastRequest2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1039);
		SetName(L("Ares' Commission (2)"));
		SetDescription(L("The operations officer suspects the Bube are pushing in from the mining village. Scout the northern woods."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Talk to the operations officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_REQUEST2", "f_siauliai_2", L("Scout the northern area"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Report to the operations officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1038, QuestStatus.Completed));

		AddObjective("killBube", L("Kill the Vubbe Fighter's minions"), new KillObjective(8, "Goblin_Miners", "Popolion_Blue", "Goblin_Spear_Q1", "Goblin_Spear_summon"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1041: A Supply Soldier's Request (1)
//-----------------------------------------------------------------------------
public class SiaulEastRequest4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1041);
		SetName(L("A Supply Soldier's Request (1)"));
		SetDescription(L("The supply soldier needs Weaver Claws for the mining village shipment."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Collect Weaver Claws"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Deliver the Weaver Claws"));

		AddPrerequisite(new LevelPrerequisite(7));

		AddPityDrop(650408, 0.1f, 0, 1, 41280);

		AddObjective("collectClaws", L("Kill Weaver to collect Weaver Claws"), new CollectItemObjective("SIAUL_EAST_REQUEST4_Claw", 6));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new TakeItemReward("SIAUL_EAST_REQUEST4_Claw", 6));
	}
}

// 1042: A Supply Soldier's Request (2)
//-----------------------------------------------------------------------------
public class SiaulEastRequest5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1042);
		SetName(L("A Supply Soldier's Request (2)"));
		SetDescription(L("The supply soldier cannot work with the Pokubu raiding the supplies. Thin them out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Kill the Pokubu"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER5", "f_siauliai_2", L("Talk to the supply soldier"));

		AddPrerequisite(new QuestStatusPrerequisite(1041, QuestStatus.Completed));

		AddObjective("killPokubu", L("Kill the Pokubu"), new KillObjective(12, "Pokubu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1043: Ares' Commission (3)
//-----------------------------------------------------------------------------
public class SiaulEastRequest6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1043);
		SetName(L("Ares' Commission (3)"));
		SetDescription(L("The scout found the Vubbe Fighter at the Nudegi logging camp. Deal with it before it grows bolder."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SOLDIER8", "f_siauliai_2", L("Find the scout"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_REQUEST6", "f_siauliai_2", L("Hunt the Vubbe Fighter"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SOLDIER8", "f_siauliai_2", L("Tell the scout the Vubbe Fighter is dead"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST6_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1039, QuestStatus.Completed));

		AddObjective("killFighter", L("Kill the Vubbe Fighter"), new KillObjective(1, "boss_Goblin_Warrior"));

		AddReward(new ItemReward("expCard1", 2));
	}
}

// 1044: Entering the Mining Village
//-----------------------------------------------------------------------------
public class SiaulEastRequest7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1044);
		SetName(L("Entering the Mining Village"));
		SetDescription(L("The Bube have pushed the refugees back. Clear the monsters chasing them, then speak with Ares again."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Report back to Knight Ares"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Kill the monsters chasing the refugees"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST7_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1043, QuestStatus.Completed));

		AddObjective("killChasers", L("Kill the monsters chasing the refugees"), new KillObjective(7, "Goblin_Spear_Q1", "Goblin_Archer_Q1"));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("TOP01_116", 1));
	}
}

// 4203: Nothing Goes as Planned (1)
//-----------------------------------------------------------------------------
public class Act2Diss1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(4203);
		SetName(L("Nothing Goes as Planned (1)"));
		SetDescription(L("Monsters stole the supplies bound for the mining village. Help the supply officer recover them."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Recover the scattered supply crates"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Deliver the supply crates"));

		AddPrerequisite(new LevelPrerequisite(2));

		AddObjective("recoverSupplies", L("Recover the scattered supply crates"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20131: Nothing Goes as Planned (2)
//-----------------------------------------------------------------------------
public class Act2Diss1_2BossQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20131);
		SetName(L("Nothing Goes as Planned (2)"));
		SetDescription(L("A Tutu ambushes the supply officer. Put it down."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Kill the Tutu"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Talk to the supply officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ACT2_DISS1_2_BOSS_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(4203, QuestStatus.Completed));

		AddObjective("killTutu", L("Kill the Tutu"), new KillObjective(1, "boss_tutu"));

		AddReward(new ItemReward("expCard1", 1));
	}
}
