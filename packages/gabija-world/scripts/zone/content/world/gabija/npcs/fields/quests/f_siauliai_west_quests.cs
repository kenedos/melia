//--- Melia Script ----------------------------------------------------------
// West Siauliai Woods Quest NPCs
//--- Description -----------------------------------------------------------
// The soldiers, villagers and hidden triggers the map's field quests run on.
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

public class FSiauliaiWestQuestNpcsScript : GeneralScript
{
	private readonly static QuestId MeetTitas = new QuestId(1001);
	private readonly static QuestId WestForest = new QuestId(1002);
	private readonly static QuestId Drasius1 = new QuestId(1003);
	private readonly static QuestId StatusTuto = new QuestId(20127);
	private readonly static QuestId Drasius2 = new QuestId(1004);
	private readonly static QuestId MeetNaglis = new QuestId(1014);
	private readonly static QuestId SkillTuto = new QuestId(8350);
	private readonly static QuestId OnionBig = new QuestId(1023);
	private readonly static QuestId Soldier3 = new QuestId(1020);
	private readonly static QuestId HamingLeaf = new QuestId(1021);
	private readonly static QuestId BossGolem = new QuestId(1022);
	private readonly static QuestId Knight = new QuestId(1013);
	private readonly static QuestId Laimonas1 = new QuestId(1015);
	private readonly static QuestId Laimonas32 = new QuestId(20128);
	private readonly static QuestId Laimonas4 = new QuestId(1018);
	private readonly static QuestId WoodSpirit = new QuestId(1019);
	private readonly static QuestId Hq01 = new QuestId(9100);

	protected override void Load()
	{
		// Sentry
		//-------------------------------------------------------------------------
		AddNpc(20016, L("Sentry"), "SIAU_FRON_NPC_01", "f_siauliai_west", -652, -953, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sentry"));

			if (!character.Quests.Has(MeetTitas) && character.Quests.MeetsPrerequisites(MeetTitas))
			{
				await dialog.Msg(L("The road to Klaipeda is closed. Nobody gets through while the camp is on alert."));

				var answer = await dialog.Select(L("If you want an answer, take it to Knight Titas. He's at the West Forest camp."),
					Option(L("Say you'll go back to Titas"), "accept"),
					Option(L("Stay where you are"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(MeetTitas);
					await dialog.Msg(L("Follow the path back into the camp. He's the one in the officer's coat."));
				}
				return;
			}

			if (character.Quests.IsActive(MeetTitas))
			{
				await dialog.Msg(L("Knight Titas is at the West Forest camp. Ask him, not me."));
				return;
			}

			await dialog.Msg(L("Keep your eyes open on the road. The woods have not been quiet since the Blessed Day."));
		});

		// Knight Titas
		//-------------------------------------------------------------------------
		AddNpc(20107, L("Knight Titas"), "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", -576, -719, 255, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Knight Titas"));
			dialog.SetPortrait("Dlg_port_WESTFOREST_MANAGER");

			if (character.Quests.IsActive(MeetTitas))
			{
				await dialog.Msg(L("So the sentry sent you to me. The road to Klaipeda is not closed for fun - the woods are crawling with monsters since the Blessed Day."));
				character.Quests.Complete(MeetTitas);
				return;
			}

			if (character.Quests.IsActive(WestForest))
			{
				await dialog.Msg(L("Glad you're willing. Follow the left-hand road out of the crossroads camp. You'll meet the soldiers soon enough."));
				character.Quests.Complete(WestForest);
				return;
			}

			if (character.Quests.IsActive(Knight))
			{
				await dialog.Msg(L("So he'll pass the rest of the order down himself. Well done."));
				await dialog.Msg(L("Klaipeda is a short walk from here along the Tenet Garden road. Don't forget to call on Uska."));

				var pick = await dialog.Select(L("Take something from the camp stores before you go."),
					Option(L("Soldier's Gladius"), "swd"),
					Option(L("Soldier's Short Rod"), "stf"),
					Option(L("Soldier's Light Bow"), "tbw"),
					Option(L("Soldier's Club"), "mac")
				);

				switch (pick)
				{
					case "swd": character.Quests.SelectReward(Knight, 101113); break;
					case "stf": character.Quests.SelectReward(Knight, 141113); break;
					case "tbw": character.Quests.SelectReward(Knight, 161113); break;
					case "mac": character.Quests.SelectReward(Knight, 201113); break;
				}

				character.Quests.Complete(Knight);
				return;
			}

			if (!character.Quests.Has(WestForest) && character.Quests.MeetsPrerequisites(WestForest))
			{
				var answer = await dialog.Select(L("If you have time to spare, carry the assembly order to my soldiers. If not, go on to Klaipeda - I won't hold it against you."),
					Option(L("Offer to carry the assembly order"), "accept"),
					Option(L("Refuse"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(WestForest);
					character.Quests.CompleteObjective(WestForest, "acceptOrder");
					await dialog.Msg(L("Good. Come back and tell me once you've made up your mind about the route."));
					return;
				}
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				await dialog.Msg(L("You've seen the recruitment notice, then. We are short of soldiers, that much is true."));

				var answer = await dialog.Select(L("But I can hardly hold out my hand to a Revelator."),
					Option(L("Say you still want to help"), "accept"),
					Option(L("Carry on your way"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq01);
					await dialog.Msg(L("Even the offer is worth something. Go to Dvasia Peak instead - Julian's unit is opening the road to the Great King's Gate, and the monsters in the way need clearing."));
					await dialog.Msg(L("Julian is the one running the operation there. He'll come out to meet you barefoot, I expect."));
					return;
				}
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("Julian is at Dvasia Peak. He'll be glad of the help."));
				return;
			}

			await dialog.Msg(L("Klaipeda is along the Tenet Garden road. Call on Uska when you get there."));
		});

		// Scout
		//-------------------------------------------------------------------------
		AddNpc(10032, L("Scout"), "SIALUL_WEST_DRASIUS", "f_siauliai_west", -1121, -528, -9, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Scout"));

			if (!character.Quests.Has(Drasius1) && character.Quests.MeetsPrerequisites(Drasius1))
			{
				await dialog.Msg(L("It's dangerous here. The monsters have bred out of all measure since the Blessed Day."));

				var answer = await dialog.Select(L("Kepa will be on us any moment. Go back, quickly."),
					Option(L("Say you only came to deliver the assembly order"), "accept"),
					Option(L("Run away quickly"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Drasius1);

				return;
			}

			if (character.Quests.IsActive(Drasius1))
			{
				if (!character.Quests.IsCompletable(Drasius1))
				{
					await dialog.Msg(L("Behind you! Don't let them surround us!"));
					character.Quests.ReplayQuestTrack(Drasius1);
					return;
				}

				await dialog.Msg(L("Ah - you're a Revelator. Fighting alongside you counts for something, so let me give you a good word of advice."));
				character.Quests.Complete(Drasius1);
				return;
			}

			if (!character.Quests.Has(StatusTuto) && character.Quests.MeetsPrerequisites(StatusTuto))
			{
				var answer = await dialog.Select(L("Once you level up you can pick a stat and grow stronger in it. Want to check now?"),
					Option(L("Try spending a stat point"), "accept"),
					Option(L("Ask for a moment to think"), "leave")
				);

				if (answer == "accept")
				{
					// Catches up a character who spent their point before the scout brought it up.
					character.Variables.Perm.SetInt(NormalTxFunctionsScript.StatPointsSpentVarName, (int)character.Properties.GetFloat(PropertyName.UsedStat));
					character.Quests.Start(StatusTuto);

					if (character.Quests.IsCompletable(StatusTuto))
					{
						await dialog.Msg(L("Ah - you've done it already. Then you know the shape of it."));
						character.Quests.Complete(StatusTuto);
						return;
					}

					character.ServerMessage(L("Press 'F1' to check your stats."));
				}
				return;
			}

			if (character.Quests.IsActive(StatusTuto))
			{
				if (!character.Quests.IsCompletable(StatusTuto))
				{
					await dialog.Msg(L("Open the inventory window with 'F2', then use the 'Lv1 EXP Card' by right clicking it. This will give you enough experience to level up."));
					await dialog.Msg(L("When you've gained a level, open the info window with 'F1' and put the point somewhere. It matters that you decide early what you mean to become."));
					return;
				}

				await dialog.Msg(L("It matters that you decide early what you mean to become. It's never an easy thing."));
				await dialog.Msg(L("There. That's how it's done - simple enough. With the goddesses gone and nobody sure what comes next, you'll want to keep at it."));
				character.Quests.Complete(StatusTuto);
				return;
			}

			if (!character.Quests.Has(Drasius2) && character.Quests.MeetsPrerequisites(Drasius2))
			{
				await dialog.Msg(L("That's right, you came to deliver the assembly order, didn't you?"));

				var answer = await dialog.Select(L("Find my bundles for me and I'll head back at once."),
					Option(L("Offer to find the bundles for him"), "accept"),
					Option(L("Say you'll wait until he finds them"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Drasius2);
					await dialog.Msg(L("It'll be the Leaf Bugs at the Uoros Farm Ruins, no question. They robbed me once before."));
					character.ServerMessage(L("Press 'M' to check the map."));
				}
				return;
			}

			if (character.Quests.IsActive(Drasius2))
			{
				if (!character.Quests.IsCompletable(Drasius2))
				{
					await dialog.Msg(L("No bundles yet? I can't go back without them."));
					return;
				}

				await dialog.Msg(L("Thank you. That was faster than I expected."));
				await dialog.Msg(L("Follow the road up and you'll find the searcher. Take care of yourself."));
				character.Quests.Complete(Drasius2);
				return;
			}

			await dialog.Msg(L("The road up leads to the searcher's post. Mind the Kepa on the way."));
		});

		// Searcher
		//-------------------------------------------------------------------------
		AddNpc(41206, L("Searcher"), "SIAUL_WEST_NAGLIS2", "f_siauliai_west", -1488, -138, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Searcher"));

			if (character.Quests.IsActive(MeetNaglis) && character.Quests.IsCompletable(MeetNaglis))
			{
				await dialog.Msg(L("No monster that size belongs this far in. Strange times."));
				await dialog.Msg(L("Thank you for the help. A Large Kepa this close to the road - Titas should hear about it."));
				character.Quests.Complete(MeetNaglis);
				return;
			}

			if (character.Quests.IsActive(SkillTuto) && character.Quests.IsCompletable(SkillTuto))
			{
				await dialog.Msg(L("Skills differ by class, but any of them makes a fight easier. And they do more than hit things."));
				await dialog.Msg(L("Plenty worth having, isn't there? Use them well and they'll carry you through."));
				await dialog.Msg(L("That reminds me - the assembly order. The squad leader is off to the right. Take it to him too."));
				character.Quests.Complete(SkillTuto);
				return;
			}

			if (character.Quests.IsActive(OnionBig) && character.Quests.IsCompletable(OnionBig))
			{
				await dialog.Msg(L("There was one after all. My gut has never been wrong yet."));
				await dialog.Msg(L("That's how I lived through the Blessed Day."));
				character.Quests.Complete(OnionBig);
				return;
			}

			if (!character.Quests.Has(MeetNaglis) && character.Quests.MeetsPrerequisites(MeetNaglis))
			{
				var answer = await dialog.Select(L("This is a dangerous stretch. What brings you here?"),
					Option(L("Tell him there is an assembly order"), "accept"),
					Option(L("Say it's nothing"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(MeetNaglis);
					await dialog.Msg(L("Watch out - a Large Kepa!"));
					return;
				}
			}

			if (!character.Quests.Has(SkillTuto) && character.Quests.MeetsPrerequisites(SkillTuto))
			{
				await dialog.Msg(L("One thing, if I may. Your attacks look a little thin."));

				var answer = await dialog.Select(L("Learn a skill and you'll have something stronger to reach for."),
					Option(L("Try learning a skill"), "accept"),
					Option(L("Ask for a moment to think"), "leave")
				);

				if (answer == "accept")
				{
					// Catches up a character who learned a skill before the searcher brought it up.
					character.Variables.Perm.SetInt(NormalTxFunctionsScript.SkillPointsSpentVarName, (int)character.Properties.GetFloat(PropertyName.UsedSkillPts));
					character.Quests.Start(SkillTuto);

					if (character.Quests.IsCompletable(SkillTuto))
					{
						await dialog.Msg(L("Ah - you've done it already. Then you know the shape of it."));
						character.Quests.Complete(SkillTuto);
						return;
					}

					character.ServerMessage(L("Press 'F3' to check your skills."));
					return;
				}
			}

			if (!character.Quests.Has(OnionBig) && character.Quests.MeetsPrerequisites(OnionBig))
			{
				var answer = await dialog.Select(L("There may well be another Large Kepa out there. Would you take a look?"),
					Option(L("Offer to look for it instead"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(OnionBig);
					await dialog.Msg(L("Any other day you could search all you like and find nothing. Odd, isn't it?"));
					return;
				}
			}

			if (character.Quests.IsActive(MeetNaglis))
			{
				await dialog.Msg(L("Watch it - keep clear of its swing."));
				character.Quests.ReplayQuestTrack(MeetNaglis);
				return;
			}

			if (character.Quests.IsActive(SkillTuto))
			{
				await dialog.Msg(L("Open the skill window with 'F3' and put a point into a skill. Any skill you can use will do."));
				return;
			}

			if (character.Quests.IsActive(OnionBig))
			{
				await dialog.Msg(L("West of here, along the old farm road. That's where I'd look."));
				character.Quests.ReplayQuestTrack(OnionBig);
				return;
			}

			await dialog.Msg(L("The squad leader is off to the right, past the rise."));
		});

		// Squad Leader
		//-------------------------------------------------------------------------
		AddNpc(20016, L("Squad Leader"), "SIAUL_WEST_SOL3", "f_siauliai_west", -663, 503, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Squad Leader"));

			if (character.Quests.IsActive(Soldier3) && character.Quests.IsCompletable(Soldier3))
			{
				await dialog.Msg(L("That's the last of them. The ground around the post is clear again."));
				character.Quests.Complete(Soldier3);
				return;
			}

			if (character.Quests.IsActive(HamingLeaf) && character.Quests.IsCompletable(HamingLeaf))
			{
				await dialog.Msg(L("Yes. That's enough of them."));
				await dialog.Msg(L("Here - a pill that puts your stamina back on its feet. I have plenty, so take it as a gift."));
				await dialog.Msg(L("Stamina comes back at a root crystal or with a rest. But when something is chasing you, there's no time for either, and then nothing is worth more than this."));
				character.Quests.Complete(HamingLeaf);
				return;
			}

			if (character.Quests.IsActive(BossGolem) && character.Quests.IsCompletable(BossGolem))
			{
				await dialog.Msg(L("My men told me about it, and I'd been worried since."));
				await dialog.Msg(L("With the world in this state, anything that ends well is a good end."));
				character.Quests.Complete(BossGolem);
				return;
			}

			if (!character.Quests.Has(Soldier3) && character.Quests.MeetsPrerequisites(Soldier3))
			{
				await dialog.Msg(L("An assembly, already? The world may have ended, but an order like that with no warning is still hard to work with. I have tasks piled to the sky."));

				var answer = await dialog.Select(L("Clear the Hanaming around the post and I can move sooner."),
					Option(L("Offer to help with the task"), "accept"),
					Option(L("About the monsters"), "explain"),
					Option(L("Say you'll wait"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("There were always monsters, but the savagery only started four years ago. The plant ones took it worst - there were none of those before."));
					await dialog.Msg(L("Beasts weren't spared either. And when the ones that looked half demon turned up, I thought that was the end of it."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Soldier3);
					await dialog.Msg(L("Hunt the Hanaming nearby first. Gather their petals while you're at it."));
					return;
				}
			}

			if (!character.Quests.Has(HamingLeaf) && character.Quests.MeetsPrerequisites(HamingLeaf))
			{
				var answer = await dialog.Select(L("The survey still wants petals. Three should do it."),
					Option(L("Accept"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(HamingLeaf);
					return;
				}
			}

			if (!character.Quests.Has(Knight) && character.Quests.MeetsPrerequisites(Knight))
			{
				var answer = await dialog.Select(L("I owe you for that, so I'll send my own men with the rest of the order. Go back and tell Sir Titas as much."),
					Option(L("Say you understand"), "accept"),
					Option(L("End"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Knight);
					character.Quests.CompleteObjective(Knight, "reportToTitas");
					return;
				}
			}

			if (!character.Quests.Has(BossGolem) && character.Quests.MeetsPrerequisites(BossGolem))
			{
				await dialog.Msg(L("My men are long past due and there's been no word. Four years since the Blessed Day and discipline still slips."));

				var answer = await dialog.Select(L("They said they were going to Delong Rest Stop."),
					Option(L("Say you'll look into it"), "accept"),
					Option(L("Say you'll be back soon"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(BossGolem);
					await dialog.Msg(L("Golems come through there. Carefree lot, my men."));
					return;
				}
			}

			if (character.Quests.IsActive(Soldier3))
			{
				await dialog.Msg(L("Revelator or goddess, nobody leaves before the task is done."));
				return;
			}

			if (character.Quests.IsActive(HamingLeaf))
			{
				await dialog.Msg(L("Revelator or goddess, nobody leaves before the task is done."));
				return;
			}

			if (character.Quests.IsActive(BossGolem))
			{
				await dialog.Msg(L("One run-in with a golem will straighten them out."));
				character.Quests.ReplayQuestTrack(BossGolem);
				return;
			}

			await dialog.Msg(L("Sir Titas is back at the West Forest camp if you need him."));
		});

		// Laimonas
		//-------------------------------------------------------------------------
		AddNpc(20117, L("Laimonas"), "SIAUL_WEST_LAIMONAS", "f_siauliai_west", 327, -347, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Laimonas"));

			if (character.Quests.IsActive(Laimonas1) && character.Quests.IsCompletable(Laimonas1))
			{
				await dialog.Msg(L("So the statue is unharmed. That is a relief."));
				await dialog.Msg(L("Even the monsters here know enough to fear a goddess. Klaipeda is down the Tenet Garden road - go with her blessing."));
				character.Quests.Complete(Laimonas1);
				return;
			}

			if (!character.Quests.Has(Laimonas1) && character.Quests.MeetsPrerequisites(Laimonas1))
			{
				var answer = await dialog.Select(L("Have you ever paid your respects at a goddess statue? The goddesses have gone from sight, but I believe they are somewhere still."),
					Option(L("Ask what would happen"), "accept"),
					Option(L("About the goddess statue"), "explain"),
					Option(L("Say you're not interested"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Goddess Zemyna rules the earth. Some say the goddesses abandoned us, but that can't be so."));
					await dialog.Msg(L("Pray at her statue and it still shines, beautifully."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Laimonas1);
					await dialog.Msg(L("That depends which goddess. Follow the right-hand road to its end - the Statue of Goddess Zemyna is there. Feel her grace for yourself."));
					return;
				}
			}

			if (!character.Quests.Has(Laimonas4) && character.Quests.MeetsPrerequisites(Laimonas4))
			{
				var answer = await dialog.Select(L("If you're bound for Klaipeda, do me a favor. The Infrorocktors make the trip there and back a misery."),
					Option(L("Say you can do that much"), "accept"),
					Option(L("Refuse"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Laimonas4);
					await dialog.Msg(L("And while you're going - pass my regards to the guard captain."));
					return;
				}
			}

			if (character.Quests.IsActive(Laimonas1))
			{
				await dialog.Msg(L("I do worry the monsters will do the statue harm. The world is ruined enough without that."));
				return;
			}

			if (character.Quests.IsActive(Laimonas4))
			{
				await dialog.Msg(L("Don't forget my regards to the Klaipeda guard captain."));
				return;
			}

			await dialog.Msg(L("Tend a goddess statue well and perhaps the goddesses come back the sooner."));
		});

		// Statue of Goddess Zemyna
		//-------------------------------------------------------------------------
		AddNpc(40110, L("Statue of Goddess Zemyna"), "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", 1687, 366, 20, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Statue of Goddess Zemyna"));

			var worshipResult = await WorshipStatPointStatue(dialog, "F_SIAULIAI_WEST_EV_55_001");
			if (worshipResult == null)
				return;

			if (character.Quests.IsActive(Laimonas1) && !character.Quests.IsCompletable(Laimonas1))
			{
				await dialog.Msg(L("You bow your head. The stone warms under the moss, and for a moment the carving is lit from within."));
				character.Quests.CompleteObjective(Laimonas1, "prayAtStatue");

				await dialog.Msg(L("The light goes out of the stone. Something is moving in the brush behind you."));
				character.Quests.Start(Laimonas32);
				character.Quests.StartQuestTrack(Laimonas32);
				return;
			}

			if (character.Quests.IsActive(Laimonas32))
			{
				if (!character.Quests.IsCompletable(Laimonas32))
				{
					await dialog.Msg(L("The light goes out of the stone. Something is moving in the brush behind you."));
					character.Quests.ReplayQuestTrack(Laimonas32);
					return;
				}

				await dialog.Msg(L("The brush has gone still. Whatever it was, it will not trouble the road again."));
				character.Quests.Complete(Laimonas32);
				return;
			}

			if (!character.Quests.Has(Laimonas32) && character.Quests.MeetsPrerequisites(Laimonas32))
			{
				await dialog.Msg(L("The light goes out of the stone. Something is moving in the brush behind you."));
				character.Quests.Start(Laimonas32);
				character.Quests.StartQuestTrack(Laimonas32);
				return;
			}

			await dialog.Msg(L("The statue stands quiet, moss to the knee."));
		});

		// Klaipeda Guard Captain
		//-------------------------------------------------------------------------
		AddNpc(20019, L("Klaipeda Guard Captain"), "SIAUL_ST1_ST2", "f_siauliai_west", 1626, -798, 270, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Klaipeda Guard Captain"));

			if (character.Quests.IsCompletable(Laimonas4))
			{
				await dialog.Msg(L("Laimonas' regards, is it? Kind of him."));
				await dialog.Msg(L("Go on into Klaipeda. Uska is waiting on the Revelators."));
				character.Quests.Complete(Laimonas4);
				return;
			}

			if (!character.Quests.Has(WoodSpirit) && character.Quests.MeetsPrerequisites(WoodSpirit))
			{
				var answer = await dialog.Select(L("A Rocktortuga will be on this line any moment. Lend us your strength - it must not get past."),
					Option(L("Offer to lend a hand"), "accept"),
					Option(L("Go into Klaipeda"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(WoodSpirit);

				return;
			}

			if (character.Quests.IsActive(WoodSpirit))
			{
				if (!character.Quests.IsCompletable(WoodSpirit))
				{
					await dialog.Msg(L("Hold the line. It comes up the road from the west."));
					character.Quests.ReplayQuestTrack(WoodSpirit);
					return;
				}

				await dialog.Msg(L("The line held. Klaipeda owes you for that."));
				character.Quests.Complete(WoodSpirit);
				return;
			}

			await dialog.Msg(L("Klaipeda is through the gate. Keep to the road."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// The approach to the camp entrance, where the guards stop the player.
		AddQuestTrigger("SIAUL_WEST_MEET_TITAS_TRIGGER", "f_siauliai_west", -570, -878, 60, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.Has(MeetTitas) && character.Quests.MeetsPrerequisites(MeetTitas))
				character.Quests.Start(MeetTitas);

			if (character.Quests.IsActive(MeetTitas) && !character.Quests.IsCompletable(MeetTitas))
				character.Quests.StartQuestTrack(MeetTitas);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIALUL_WEST_ONION_BIG_TRIGGER", "f_siauliai_west", -1885, 102, 120, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(OnionBig) && !character.Quests.IsCompletable(OnionBig))
				character.Quests.StartQuestTrack(OnionBig);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_WEST_BOSS_GOLEM_TRIGGER", "f_siauliai_west", -550, 1370, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(BossGolem) && !character.Quests.IsCompletable(BossGolem))
				character.Quests.StartQuestTrack(BossGolem);

			await Task.CompletedTask;
		});

		AddQuestTrigger("SIAUL_WEST_ROCK", "f_siauliai_west", 1428, -923, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(WoodSpirit) && !character.Quests.IsCompletable(WoodSpirit))
				character.Quests.StartQuestTrack(WoodSpirit);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 1001: To Knight Titas (1)
//-----------------------------------------------------------------------------
public class SiaulWestMeetTitasQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1001);
		SetName(L("To Knight Titas (1)"));
		SetDescription(L("Ask the sentry the way to Klaipeda, then report to Knight Titas at the West Forest camp."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		// The client's StartNPC is an auto-grant trigger; the Sentry is who the player talks to.
		SetPhase(QuestStatus.Possible, "SIAU_FRON_NPC_01", "f_siauliai_west", L("Talk to the Sentry"));
		SetPhase(QuestStatus.InProgress, "SIAU_FRON_NPC_01", "f_siauliai_west", L("Talk to the Sentry"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas at the West Forest camp"));

		// The arrival cutscene completes the objective; the turn-in is at
		// Knight Titas.
		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAU_WEST_START_TRACK", 1000, autoStart: false);

		AddObjective("meetTitas", L("Talk to Knight Titas at the West Forest camp"), new ManualObjective());
	}
}

// 1002: To Knight Titas (2)
//-----------------------------------------------------------------------------
public class SiaulWestWestForestQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1002);
		SetName(L("To Knight Titas (2)"));
		SetDescription(L("Knight Titas asks you to carry the assembly order to his soldiers before you leave for Klaipeda."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas again"));

		AddPrerequisite(new QuestStatusPrerequisite(1001, QuestStatus.Completed));

		AddObjective("acceptOrder", L("Talk to Knight Titas again"), new ManualObjective());

		AddReward(new ItemReward("Drug_HP1_Q", 3));
	}
}

// 1003: To the Scout (1)
//-----------------------------------------------------------------------------
public class SiaulWestDrasius1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1003);
		SetName(L("To the Scout (1)"));
		SetDescription(L("Give the assembly order to the scout on the western road, and see him through the Kepa that swarm him."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout at the marked spot"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Kill the swarming Kepa"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Check on the Scout"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_DRASIUS1_TRACK", 4000);

		AddPrerequisite(new QuestStatusPrerequisite(1002, QuestStatus.Completed));

		AddObjective("killKepa", L("Kill the swarming Kepa"), new KillObjective(4, "Onion"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20127: Using Stats
//-----------------------------------------------------------------------------
public class SiaulWestStatusTuto1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20127);
		SetName(L("Using Stats"));
		SetDescription(L("The scout explains that a level up lets you raise one of your stats."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Open the info window with 'F1' and spend a stat point"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Talk to the Scout"));

		AddPrerequisite(new QuestStatusPrerequisite(1003, QuestStatus.Completed));

		AddObjective("spendStat", L("Open the info window with 'F1' and spend a stat point"), new VariableCheckObjective(NormalTxFunctionsScript.StatPointsSpentVarName, 1, isPermanent: true));
	}
}

// 1004: To the Scout (2)
//-----------------------------------------------------------------------------
public class SiaulWestDrasius2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1004);
		SetName(L("To the Scout (2)"));
		SetDescription(L("The scout will not fall back until he has his belongings. Take them off the Leaf Bugs that stole them."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Remind the Scout of the order to fall back to Klaipeda"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Kill Leaf Bugs and collect the bundles"));
		SetPhase(QuestStatus.Success, "SIALUL_WEST_DRASIUS", "f_siauliai_west", L("Hand the bundles to the Scout"));

		AddPrerequisite(new QuestStatusPrerequisite(20127, QuestStatus.Completed));

		AddPityDrop(650405, 0.1f, 10, 1, 401501);

		AddObjective("collectBundles", L("Kill Leaf Bugs to obtain Soldier's Belongings"), new CollectItemObjective("SIAUL_WEST_DRASIUS2_Bag", 5));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 3));
		AddReward(new TakeItemReward("SIAUL_WEST_DRASIUS2_Bag"));
	}
}

// 1013: To Knight Titas (3)
//-----------------------------------------------------------------------------
public class SiaulWestKnightQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1013);
		SetName(L("To Knight Titas (3)"));
		SetDescription(L("The squad leader will pass on the rest of the order himself. Report that to Knight Titas."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Talk to the Squad Leader"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Report to Knight Titas"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Report to Knight Titas"));

		AddPrerequisite(new QuestStatusPrerequisite(1021, QuestStatus.Completed));

		AddObjective("reportToTitas", L("Report to Knight Titas"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new SelectItemReward("SWD01_113", "STF01_113", "TBW01_113", "MAC01_113"));
	}
}

// 1014: To the Searcher
//-----------------------------------------------------------------------------
public class SiaulWestMeetNaglisQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1014);
		SetName(L("To the Searcher"));
		SetDescription(L("Give the assembly order to the searcher up the northern road, and put down the Large Kepa that charges him."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Find the Searcher at the marked spot"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Tell the Searcher his relief has come"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_MEET_NAGLIS_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1004, QuestStatus.Completed));

		AddObjective("killLargeKepa", L("Kill the charging Large Kepa"), new KillObjective(1, "Onion_Big"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1023: A Bad Feeling
//-----------------------------------------------------------------------------
public class SiaulWestOnionBigQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1023);
		SetName(L("A Bad Feeling"));
		SetDescription(L("The searcher is sure another Large Kepa is hiding nearby. Go and find out."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));
		SetPhase(QuestStatus.InProgress, "SIALUL_WEST_ONION_BIG_TRIGGER", "f_siauliai_west", L("Kill the Large Kepa"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Go back to the Searcher"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_ONION_BIG_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killHiddenKepa", L("Kill the Large Kepa"), new KillObjective(1, "Onion_Big_Q1"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1015: Laimonas' Favor
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1015);
		SetName(L("Laimonas' Favor"));
		SetDescription(L("Laimonas asks you to pay your respects at the Statue of Goddess Zemyna at the end of the eastern road."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS3_TRIGGER", "f_siauliai_west", L("Pay respects at the goddess statue"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));

		AddPrerequisite(new QuestStatusPrerequisite(1013, QuestStatus.Completed));

		AddObjective("prayAtStatue", L("Pay respects at the goddess statue"), new ManualObjective());

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 20128: The Way Back
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas32Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20128);
		SetName(L("The Way Back"));
		SetDescription(L("A Mushcaria breaks in on your prayer at the statue."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", L("To the road back"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS3_2_TRIGGER", "f_siauliai_west", L("Kill the Mushcaria"));
		SetPhase(QuestStatus.Success, "F_SIAULIAI_WEST_EV_55_001", "f_siauliai_west", L("Kill the Mushcaria"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_LAIMONAS3_2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killMushcaria", L("Kill the Mushcaria"), new KillObjective(1, "boss_mushcaria"));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Drug_SP1_Q", 3));
	}
}

// 1020: To the Squad Leader (1)
//-----------------------------------------------------------------------------
public class SiaulWestSoldier3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1020);
		SetName(L("To the Squad Leader (1)"));
		SetDescription(L("The squad leader will only fall back once his own task is done. Clear the Hanaming around his post."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Give the Squad Leader the order to fall back"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Move to where the Hanaming appear"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Report to the Squad Leader"));

		AddPrerequisite(new QuestStatusPrerequisite(8350, QuestStatus.Completed));

		AddObjective("killHanaming", L("Kill the Hanaming around you"), new KillObjective(8, "Hanaming"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1021: To the Squad Leader (2)
//-----------------------------------------------------------------------------
public class SiaulWestHamingLeafQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1021);
		SetName(L("To the Squad Leader (2)"));
		SetDescription(L("Collect Hanaming Petals for the squad leader's survey so he can leave his post."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Move to the Hanaming habitat"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_HANAMING_TRIGGER", "f_siauliai_west", L("Collect Hanaming Petals for the survey"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Hand the Hanaming Petals to the Squad Leader"));

		AddPrerequisite(new QuestStatusPrerequisite(1020, QuestStatus.Completed));

		AddPityDrop(645024, 0.35f, 5, 1, 400941);

		AddObjective("collectPetals", L("Collect Hanaming Petals for the survey"), new CollectItemObjective("leaf_hanaming", 3));

		AddReward(new ItemReward("expCard1", 1));
		AddReward(new ItemReward("Drug_STA1_Q", 3));
		AddReward(new TakeItemReward("leaf_hanaming", 3));
	}
}

// 1022: The Start of the Trouble
//-----------------------------------------------------------------------------
public class SiaulWestBossGolemQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1022);
		SetName(L("The Start of the Trouble"));
		SetDescription(L("The squad leader's men are overdue at Delong Rest Stop. Go and look for them."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Talk to the Squad Leader"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_BOSS_GOLEM_TRIGGER", "f_siauliai_west", L("Search the dangerous-looking area"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_SOL3", "f_siauliai_west", L("Report to the Squad Leader"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_BOSS_GOLEM_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killGolem", L("Kill the Golem"), new KillObjective(1, "boss_Golem"));

		AddReward(new ItemReward("expCard1", 3));
		AddReward(new ItemReward("Drug_Haste1_Q", 3));
	}
}

// 1018: The Road to Klaipeda (1)
//-----------------------------------------------------------------------------
public class SiaulWestLaimonas4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1018);
		SetName(L("The Road to Klaipeda (1)"));
		SetDescription(L("Laimonas asks you to clear the Infrorocktors off the road to Klaipeda."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Talk to Laimonas"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_LAIMONAS", "f_siauliai_west", L("Kill Infrorocktors near the marked area"));
		SetPhase(QuestStatus.Success, "SIAUL_ST1_ST2", "f_siauliai_west", L("Talk to the Klaipeda Guard Captain"));

		AddObjective("killInfrorocktors", L("Kill Infrorocktors"), new KillObjective(7, "InfroRocktor"));

		AddReward(new ItemReward("expCard1", 1));
	}
}

// 1019: The Road to Klaipeda (2)
//-----------------------------------------------------------------------------
public class SiaulWestWoodSpiritQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1019);
		SetName(L("The Road to Klaipeda (2)"));
		SetDescription(L("A Rocktortuga is about to hit the Klaipeda checkpoint. Hold the line with the guards."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_ST1_ST2", "f_siauliai_west", L("Head for Klaipeda"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_ROCK", "f_siauliai_west", L("Kill the Rocktortuga that appeared"));
		SetPhase(QuestStatus.Success, "SIAUL_ST1_ST2", "f_siauliai_west", L("Talk to the Klaipeda Guard Captain"));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "SIAUL_WEST_WOOD_SPIRIT_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1018, QuestStatus.Completed));

		AddObjective("killRocktortuga", L("Kill the Rocktortuga that appeared"), new KillObjective(1, "boss_Rocktortuga"));

		AddReward(new ItemReward("expCard1", 3));
	}
}

// 9100: Reinforcements
//-----------------------------------------------------------------------------
public class SiaulWestHq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9100);
		SetName(L("Reinforcements"));
		SetDescription(L("Knight Titas sends you to Dvasia Peak, where Julian's unit is opening the road to the Great King's Gate."));
		SetType(QuestType.Sub);
		SetLocation("f_siauliai_west", "d_thorn_22");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_CAMP_MANAGER", "f_siauliai_west", L("Talk to Knight Titas"));
		SetPhase(QuestStatus.InProgress, "THORN22_JULIAN", "d_thorn_22", L("Kill monsters in Dvasia Peak"));
		SetPhase(QuestStatus.Success, "THORN22_JULIAN", "d_thorn_22", L("Report to Commander Julian"));

		AddPrerequisite(new LevelPrerequisite(100));

		AddObjective("clearDvasiaPeak", L("Kill monsters in Dvasia Peak"), new KillObjective(100, "Meleech", "RavineLerva", "TreeGool", "wood_goblin"));
	}
}

// 8350: Let's Learn a Skill
//-----------------------------------------------------------------------------
public class TutoSkillRunQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8350);
		SetName(L("Let's Learn a Skill"));
		SetDescription(L("The searcher points out that your attacks are lacking, and that a skill would serve you better."));
		SetType(QuestType.Main);
		SetLocation("f_siauliai_west");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));
		SetPhase(QuestStatus.InProgress, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Learn how to pick up a skill"));
		SetPhase(QuestStatus.Success, "SIAUL_WEST_NAGLIS2", "f_siauliai_west", L("Talk to the Searcher"));

		AddPrerequisite(new QuestStatusPrerequisite(1014, QuestStatus.Completed));

		AddObjective("learnSkill", L("Learn how to pick up a skill"), new VariableCheckObjective(NormalTxFunctionsScript.SkillPointsSpentVarName, 1, isPermanent: true));

		AddReward(new ItemReward("Drug_HP1_Q", 1));
	}
}
