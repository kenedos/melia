//--- Melia Script ----------------------------------------------------------
// East Siauliai Woods Quest NPCs
//--- Description -----------------------------------------------------------
// The knights, guards and supply soldiers the map's field quests run on.
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

	private const int SupplyCrateCount = 4;
	private const int SupplyCratePlaced = 11;
	private const string SupplyCrateVar = "Gabija.Quests.Act2Diss1.Crate";

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
				await dialog.CompleteQuest(Reclaim1);
				return;
			}

			if (character.Quests.IsActive(Camp4) && character.Quests.IsCompletable(Camp4))
			{
				await dialog.Msg(L("Are you hurt anywhere? I am grateful you killed the Poata, but that was truly reckless."));

				await dialog.CompleteQuest(Camp4);
				return;
			}

			if (character.Quests.IsActive(Request7) && character.Quests.IsCompletable(Request7))
			{
				await dialog.Msg(L("The fighting at the mining village seems very bad. It is too late to move troops, so you must go and support the mining village yourself."));
				await dialog.CompleteQuest(Request7);
				return;
			}

			if (!character.Quests.Has(Reclaim1) && character.Quests.MeetsPrerequisites(Reclaim1))
			{
				await dialog.Msg(L("You mean to go to the crystal mine to find the light of salvation? But now is not a good time. The Vubbe horde is pouring out of the crystal mine."));
				await dialog.Msg(L("And here in the eastern woods, monsters are multiplying and threatening even Klaipeda. Because of that, the mining village in between has become the middle of a battlefield."));
				await dialog.Msg(L("We split our forces - some to hold the mining village, and I with the rest am investigating why the monsters are multiplying in the eastern woods."));
				await dialog.Msg(L("So we cannot spare an escort for the Revelators. And yet we cannot disobey the bishop and the knight commander."));

				var answer = await dialog.SelectQuestOffer(Reclaim1, L("Hmm, how about this. Since we must test whether you can reach the mining village safely without an escort... would you help with our investigation here?"),
					Option(L("I'll help with your investigation"), "accept"),
					Option(L("I'll wait until this is settled"), "leave")
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

				var answer = await dialog.SelectQuestOffer(Request1, L("Cut a few of them open and see what they are hoarding. Will you?"),
					Option(L("I'll find your clue"), "accept"),
					Option(L("Later"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Request1);
					await dialog.Msg(L("If you find any clues, let the Operations Officer in the northern area know about it."));
					return;
				}
			}

			if (!character.Quests.Has(Camp4) && character.Quests.MeetsPrerequisites(Camp4))
			{
				await dialog.Msg(L("Have you by any chance seen a Poata cub? We must drive it far away quickly, or the mother will keep prowling about."));

				var answer = await dialog.SelectQuestOffer(Camp4, L("It is prowling the camp even now. Could you put it down?"),
					Option(L("I haven't seen the cub, but I'll deal with the mother"), "accept"),
					Option(L("That is not my concern"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Camp4);
					return;
				}
			}

			if (!character.Quests.Has(Request7) && character.Quests.MeetsPrerequisites(Request7))
			{
				await dialog.Msg(L("You really killed the Vubbe Fighter? Then the monsters will no longer multiply. We can rest a little easier now."));

				var answer = await dialog.SelectQuestOffer(Request7, L("The road to the mining village is yours to take now. Are you ready?"),
					Option(L("I'll go to the mining village"), "accept"),
					Option(L("Give me a little time to prepare"), "leave")
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
				await dialog.Msg(L("The Vubbes are pushing the refugees back. Clear the monsters chasing them."));
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

				var answer = await dialog.SelectQuestOffer(Reclaim2, L("Would you thin them out for us?"),
					Option(L("I'll hunt the Chupacabra"), "accept"),
					Option(L("See to it yourself"), "leave"),
					Option(L("What is Sir Ares' unit doing out here?"), "explain")
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
				await dialog.CompleteQuest(Reclaim2);
				return;
			}

			if (!character.Quests.Has(Reclaim3) && character.Quests.MeetsPrerequisites(Reclaim3))
			{
				await dialog.Msg(L("Trusting your skill, I have one more request. The supply depot - I want its Chupacabra driven out as well."));

				var answer = await dialog.SelectQuestOffer(Reclaim3, L("Drive them out of the depot and we can breathe again. Will you?"),
					Option(L("I'll retake the depot"), "accept"),
					Option(L("Not this time"), "leave")
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
					character.Quests.ClearQuestTrack(Reclaim3);
					return;
				}

				await dialog.Msg(L("Finished already? I have never seen anyone win results so quickly."));
				await dialog.CompleteQuest(Reclaim3);
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

			if (character.Quests.IsActive(Act2Diss1) && character.Quests.IsCompletable(Act2Diss1))
			{
				await dialog.Msg(L("Every crate accounted for. They cannot have gone far, I said, and there you have it."));
				await dialog.CompleteQuest(Act2Diss1);
				return;
			}

			if (!character.Quests.Has(Act2Diss1) && character.Quests.MeetsPrerequisites(Act2Diss1))
			{
				await dialog.Msg(L("I am recovering the supplies bound for the mining village. The monsters stole them all."));
				await dialog.Msg(L("But some of the crates contain dangerous explosives, so the recovery is very hard."));

				var answer = await dialog.SelectQuestOffer(Act2Diss1, L("They are scattered all around the depot. Would you gather them up for me?"),
					Option(L("I'll gather the crates"), "accept"),
					Option(L("I would rather not handle explosives"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= SupplyCratePlaced; ++i)
						character.Variables.Perm.Set(SupplyCrateVar + i, false);

					character.Quests.Start(Act2Diss1);
					await dialog.Msg(LF("Bring me {0} of them and I can account for the rest. They are scattered all round the depot.", SupplyCrateCount));
				}

				return;
			}

			if (character.Quests.IsActive(Act2Diss1))
			{
				await dialog.Msg(L("They cannot have gone far. If only a monster had stolen one and set it off, it would have been better."));
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
				await dialog.CompleteQuest(Act2Diss1Boss);
				return;
			}

			if (!character.Quests.Has(Reclaim6) && character.Quests.MeetsPrerequisites(Reclaim6))
			{
				await dialog.Msg(L("Since you are helping, there is one more thing. The Chupacabra are the main thieves of the supplies."));

				var answer = await dialog.SelectQuestOffer(Reclaim6, L("Thin them out and the crates might stop walking off. Will you?"),
					Option(L("I'll thin out the Chupacabra"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Reclaim6);
					await dialog.Msg(L("They prowl all around the depot. I leave it to you."));
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
				await dialog.CompleteQuest(Reclaim6);
				return;
			}

			if (!character.Quests.Has(Reclaim7) && character.Quests.MeetsPrerequisites(Reclaim7))
			{
				await dialog.Msg(L("The Weaver movement by the lower stream looks suspicious. As if they mean to interfere with the supplies to the mining village."));

				var answer = await dialog.SelectQuestOffer(Reclaim7, L("Clear them off the lower stream before the next shipment moves. Can you?"),
					Option(L("I'll clear the Weaver out"), "accept"),
					Option(L("Another time"), "leave")
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
					character.Quests.ClearQuestTrack(Reclaim7);
					return;
				}

				await dialog.Msg(L("Thank you. With the Revelator helping like this, we will not be outdone either."));
				await dialog.CompleteQuest(Reclaim7);
				return;
			}

			await dialog.Msg(L("The supplies to the mining village are in a bad way. I have had no rest."));
		});

		// Operations Officer
		//-------------------------------------------------------------------------
		AddNpc(20014, L("Operations Officer"), "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", -1300, 828, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Operations Officer"));

			if (character.Quests.IsActive(Request1))
			{
				if (!character.Quests.IsCompletable(Request1))
				{
					await dialog.Msg(L("It's very important to study the monsters at times like this. It's like weather forecasts where you try to understand the source to see what trends will happen."));
					return;
				}

				await dialog.Msg(L("Were you sent by Aras? This is a piece of Vubbe clothing. Ah... I see..."));
				await dialog.CompleteQuest(Request1);
				return;
			}

			if (!character.Quests.Has(Request2) && character.Quests.MeetsPrerequisites(Request2))
			{
				await dialog.Msg(L("I think the Vubbes from the Miners' Village have made their way into the woods. Maybe that's also a reason behind the abnormal surge in monsters."));
				await dialog.Msg(L("I better ask Aras to search for Vubbes in other regions too. In the meantime, I would like you to take a look at the upper areas."));

				var answer = await dialog.SelectQuestOffer(Request2, L("Would you take a look at the upper area for me?"),
					Option(L("I'll go and look"), "accept"),
					Option(L("See to it yourself"), "leave"),
					Option(L("What are the Vubbes?"), "explain")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("They have low intelligence, but these monsters build groups and live as a community. They used to appear only occasionally in the deeper areas of the mines... Could they be expanding their forces?"));
					await dialog.Msg(L("Whatever the reason is, the Vubbes expanding their territory is a big problem. Monsters that lose their territory to the Vubbes will be forced to make their way into our base."));
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
					await dialog.Msg(L("We haven't searched the upper areas yet. Of course we should be sending troops, but I ask for your help as this is an urgent matter."));
					character.Quests.ClearQuestTrack(Request2);
					return;
				}

				await dialog.Msg(L("You saw a Vubbe Fighter but missed it? We also got a report of Vubbe Fighter appearing in the Southern area."));
				await dialog.Msg(L("First, talk to the Search Scout by the bridge in the lower area of Bulves Farm. This will become a tedious, drawn out campaign if we let that Vubbe Fighter run loose."));
				await dialog.CompleteQuest(Request2);
				dialog.UnHideNPC("SIAUL_EAST_SOLDIER8");
				return;
			}

			if (character.Quests.HasCompleted(Request6))
			{
				await dialog.Msg(L("I heard the stories. So you defeated the Vubbe Fighter, huh? That's incredible."));
				return;
			}

			await dialog.Msg(L("It's very important to study the monsters at times like this. It's like weather forecasts where you try to understand the source to see what trends will happen."));
		});

		// Supply Soldier
		//-------------------------------------------------------------------------
		AddNpc(20011, L("Supply Soldier"), "SIAUL_EAST_SOLDIER5", "f_siauliai_2", 670, 440, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Supply Soldier"));

			if (!character.Quests.Has(Request4) && character.Quests.MeetsPrerequisites(Request4))
			{
				await dialog.Msg(L("Among the supplies to be sent to the mining village is Weaver Claw, but I have had no chance to get it. What am I to do."));

				var answer = await dialog.SelectQuestOffer(Request4, L("I cannot leave the crates to go hunting. Could you bring me the claws?"),
					Option(L("I'll bring you the claws"), "accept"),
					Option(L("Another time"), "leave")
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
				await dialog.CompleteQuest(Request4);
				return;
			}

			if (!character.Quests.Has(Request5) && character.Quests.MeetsPrerequisites(Request5))
			{
				await dialog.Msg(L("Truthfully, if not for the Pokubu, everything would be fine. They make such a nuisance that I have not done the supply recovery work properly - not a single one, in fact."));

				var answer = await dialog.SelectQuestOffer(Request5, L("If somebody thinned them out I could finally get to work. Would you?"),
					Option(L("I'll deal with the Pokubu"), "accept"),
					Option(L("Another time"), "leave")
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
				await dialog.CompleteQuest(Request5);
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
				await dialog.Msg(L("Ah, it is you. I heard about it from the operations officer. The Vubbe Fighter is hiding deep inside the Nudegi logging camp."));
				await dialog.Msg(L("The order to kill it has come down, but waiting for reinforcements might be safer."));

				var answer = await dialog.SelectQuestOffer(Request6, L("Waiting might be safer. What will you do?"),
					Option(L("I'll kill it now"), "accept"),
					Option(L("I'll wait for the reinforcements"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Request6);

				return;
			}

			if (character.Quests.IsActive(Request6))
			{
				if (!character.Quests.IsCompletable(Request6))
				{
					await dialog.Msg(L("I am only a scout, but this was the first time I saw a Vubbe Fighter up close."));
					character.Quests.ClearQuestTrack(Request6);
					return;
				}

				await dialog.Msg(L("Amazing. To take that down alone. Go and report to Sir Ares at once. He will be delighted."));
				await dialog.CompleteQuest(Request6);
				return;
			}

			await dialog.Msg(L("The Vubbe Fighter keeps to the deep parts of the Nudegi logging camp."));
		});

		// Scattered supply crates
		//-------------------------------------------------------------------------
		AddSupplyCrate(1, 301, -569);
		AddSupplyCrate(2, 289, -758);
		AddSupplyCrate(3, 176, -492);
		AddSupplyCrate(4, 125, -247);
		AddSupplyCrate(5, -158, -876);
		AddSupplyCrate(6, -260, -581);
		AddSupplyCrate(7, -223, -452);
		AddSupplyCrate(8, 952, -742);
		AddSupplyCrate(9, 729, -815);
		AddSupplyCrate(10, 310, -335);
		AddSupplyCrate(11, 607, -19);

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

		AddQuestTrigger("SIAUL_EAST_REQUEST6", "f_siauliai_2", 1887, -477, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Request6) && !character.Quests.IsCompletable(Request6))
				character.Quests.StartQuestTrack(Request6);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_CAMP4", "f_siauliai_2", 164, 440, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Camp4) && !character.Quests.IsCompletable(Camp4))
				character.Quests.StartQuestTrack(Camp4);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_EAST_CAMP4_2", "f_siauliai_2", -46, 818, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Camp4) && !character.Quests.IsCompletable(Camp4))
				character.Quests.StartQuestTrack(Camp4);

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Places one of the supply crates the monsters dragged off, which the
	/// player gathers back up for the supply officer.
	/// </summary>
	private void AddSupplyCrate(int number, int x, int z)
	{
		AddNpc(46212, L("Supply Crate"), "ACT2_DISS1_BOX_" + number, "f_siauliai_2", x, z, 177, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Supply Crate"));

			if (!character.Quests.IsActive(Act2Diss1) || character.Quests.IsCompletable(Act2Diss1))
			{
				await dialog.Msg(L("An empty crate, tipped on its side."));
				return;
			}

			if (character.Variables.Perm.GetBool(SupplyCrateVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already recovered this crate*{/}"));
				return;
			}

			var result = await character.TimeActions.StartAsync(L("Recovering the supply crate..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

			if (result != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(SupplyCrateVar + number, true);

			var recovered = 0;
			for (var i = 1; i <= SupplyCratePlaced; ++i)
			{
				if (character.Variables.Perm.GetBool(SupplyCrateVar + i, false))
					recovered++;
			}

			character.ServerMessage(LF("Supply crates recovered: {0}/{1}", recovered, SupplyCrateCount));

			if (character.Quests.TryGetById(Act2Diss1, out var quest) && quest.TryGetProgress("recoverSupplies", out var progress))
			{
				progress.Count = Math.Min(recovered, SupplyCrateCount);
				character.Quests.UpdateQuestProgress(Act2Diss1, progress.Objective.Id);
			}

			if (recovered >= SupplyCrateCount)
				character.Quests.CompleteObjective(Act2Diss1, "recoverSupplies");
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

		AddObjective("killPoata", L("Kill the Poata at the camp"), new KillObjective(1, "boss_poata") { LayerOnly = true });

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

		AddObjective("killPokubu", L("Kill the Pokubu on the farm"), new KillObjective(4, "Pokubu") { LayerOnly = true });

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

		AddObjective("killChupacabra", L("Clear the supply depot of Chupacabra"), new KillObjective(10, "Chupacabra_Blue", "Chupacabra_Ibory") { LayerOnly = true });

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
		SetDescription(L("The supply officer wants the Chupacabra that steal the supplies thinned out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Hear the supply officer's request"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Kill the Chupacabra raiding the supplies"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER", "f_siauliai_2", L("Report to the supply officer"));

		AddPrerequisite(new QuestStatusPrerequisite(20131, QuestStatus.Completed));

		AddObjective("killChupacabra", L("Kill the Chupacabra raiding the supplies"), new KillObjective(7, "Chupacabra_Blue"));

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

		AddObjective("killWeaver", L("Kill the Weaver"), new KillObjective(5, "Weaver") { LayerOnly = true });

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
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Deliver the Piece of Vubbe Cloth"));

		AddPrerequisite(new QuestStatusPrerequisite(1032, QuestStatus.Completed));

		AddPityDrop("SIAUL_EAST_REQUEST1_Blood", 0.35f, 3, 1, "Popolion_Blue");

		AddObjective("findClue", L("Kill Popolion to find a clue"), new CollectItemObjective("SIAUL_EAST_REQUEST1_Blood", 1));

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
		SetDescription(L("The operations officer suspects the Vubbes are pushing in from the mining village. Scout the northern woods."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Talk to the operations officer"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_REQUEST2", "f_siauliai_2", L("Scout the northern area"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_SUPPLY_MANAGER2", "f_siauliai_2", L("Report to the operations officer"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1038, QuestStatus.Completed));

		AddObjective("killBube", L("Kill the Vubbe Fighter's minions"), new KillObjective(8, "Goblin_Miners", "Popolion_Blue", "Goblin_Spear_Q1", "Goblin_Spear_summon") { LayerOnly = true });

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

		AddPityDrop("SIAUL_EAST_REQUEST4_Claw", 1.0f, 0, 1, "Weaver");

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

		AddObjective("killFighter", L("Kill the Vubbe Fighter"), new KillObjective(1, "boss_Goblin_Warrior") { LayerOnly = true });

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
		SetDescription(L("The Vubbes have pushed the refugees back. Clear the monsters chasing them, then speak with Ares again."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Report back to Knight Ares"));
		SetPhase(QuestStatus.InProgress, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Kill the monsters chasing the refugees"));
		SetPhase(QuestStatus.Success, "SIAUL_EAST_MANAGER", "f_siauliai_2", L("Talk to Knight Ares"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_EAST_REQUEST7_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1043, QuestStatus.Completed));

		AddObjective("killChasers", L("Kill the monsters chasing the refugees"), new KillObjective(7, "Goblin_Spear_Q1", "Goblin_Archer_Q1") { LayerOnly = true });

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

		AddObjective("recoverSupplies", L("Recover four of the scattered supply crates"), new ManualObjective());

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

		AddObjective("killTutu", L("Kill the Tutu"), new KillObjective(1, "boss_tutu") { LayerOnly = true });

		AddReward(new ItemReward("expCard1", 1));
	}
}
