//--- Melia Script ----------------------------------------------------------
// Nefritas Cliff Quest NPCs
//--- Description -----------------------------------------------------------
// The Watchers, the Followers and the barriers the cliff's quests run on.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele573QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(8538);
	private readonly static QuestId Mq02 = new QuestId(8539);
	private readonly static QuestId Mq03 = new QuestId(8540);
	private readonly static QuestId Mq04 = new QuestId(8541);
	private readonly static QuestId Mq05 = new QuestId(8542);
	private readonly static QuestId Mq06 = new QuestId(8543);
	private readonly static QuestId Mq07 = new QuestId(8544);
	private readonly static QuestId Mq08 = new QuestId(8545);
	private readonly static QuestId Mq09 = new QuestId(8546);
	private readonly static QuestId Hq01 = new QuestId(9102);
	private readonly static QuestId Hq02 = new QuestId(9104);
	private readonly static QuestId Reveal2 = new QuestId(30031);

	public const string AltarResurrectionsVar = "Gabija.Gele573.Hq02.Resurrections";
	public const string SeveredSoulVar = "Gabija.Gele573.Mq04.Soul";
	private const string SeveredSourceVar = "Gabija.Gele573.Mq04.Source";
	private const string SeveredOriginalVar = "Gabija.Gele573.Mq04.Original";
	private readonly static Position PumpuraBarrierPosition = new Position(823, 0, 90);
	private static DateTime _pumpuraBarrierReadyAt = DateTime.MinValue;
	private const int CirclesNeeded = 7;

	protected override void Load()
	{
		// A severed soul takes its demon with it when a player destroys it.
		ZoneServer.Instance.ServerEvents.EntityKilled.Subscribe((sender, args) =>
		{
			if (args.Target is not Mob soul || !soul.Vars.GetBool(SeveredSoulVar, false))
				return;

			var killer = args.Attacker == null ? null : soul.GetKillBeneficiary(args.Attacker);
			if (killer == null)
				return;

			killer.ServerMessage(L("The demon's soul has been destroyed."));

			if (soul.Vars.TryGet<Mob>(SeveredOriginalVar, out var original) && !original.IsDead)
				original.Kill(null);
		});

		// Resurrecting at the central altar of Tenet Church 1F opens the Paladin Master's hidden quest.
		ZoneServer.Instance.ServerEvents.PlayerResurrected.Subscribe((sender, args) =>
		{
			var character = args.Character;

			if (character.Map.ClassName != "d_chapel_57_6" || character.Quests.Has(Hq02))
				return;

			if (!character.Position.InRange2D(new Position(-523, 1, 446), 80))
				return;

			var count = character.Variables.Perm.GetInt(AltarResurrectionsVar, 0);
			if (count < 30)
				character.Variables.Perm.SetInt(AltarResurrectionsVar, count + 1);
		});

		// Watcher Allen
		//-------------------------------------------------------------------------
		AddNpc(147422, L("Watcher Allen"), "GELE573_ALLEN", "f_gele_57_3", -770, -1083, 92, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Allen"));

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("The barrier that used to stop the demons at Mazas Rest Place is broken."));
				var answer = await dialog.SelectQuestOffer(Mq01, L("I want to fix it, could you lend me your help?"),
					Option(L("I'll help if it's simple"), "accept"),
					Option(L("About the barriers in Nefritas Cliff"), "explain"),
					Option(L("I'm busy on my way"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("This is one of the devices the first Paladin set in preparation for the demon invasion."));
					await dialog.Msg(L("Nefritas Cliff is covered with several of these devices."));
					return;
				}

				if (answer == "accept")
				{
					await dialog.Msg(L("The pieces of the destroyed barrier are scattered at Mazas Rest Place."));
					await dialog.Msg(L("Collect them and give them to Kayetonas at Flower Greeting Hill."));
					character.Quests.Start(Mq01);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("Activating the Tree Guard Post Barrier is taking longer than I thought."));
				var answer = await dialog.SelectQuestOffer(Mq02, L("We have no time, let's charge it with demon souls instead."),
					Option(L("I'll help"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Defeat the demons around the Tree Guard Post Barrier."));
					await dialog.Msg(L("The souls of those demons should be able to fill its divine powers a little. I'm counting on you!"));
					character.Quests.Start(Mq02);
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				if (character.Quests.IsCompletable(Mq01))
				{
					await dialog.Msg(L("I'm pretty dull and unsure on fixing the barrier, so could you inform to Kayetonas about it?"));
					return;
				}

				await dialog.Msg(L("The barrier was made a long time ago."));
				await dialog.Msg(L("And I failed to protect it, what do I tell our ancestors?"));
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				if (character.Quests.IsCompletable(Mq02))
				{
					await dialog.Msg(L("Good work."));
					await dialog.Msg(L("Honestly, I had my doubts, but the results seem to be fine."));
					await dialog.Msg(L("You just need to tell it like that to Kayetonas in Flower hill."));
					return;
				}

				await dialog.Msg(L("All souls are pure. So even if it's a demon's soul, it can still provide some divine power."));
				return;
			}

			await dialog.Msg(L("The barriers stand, most of them. That has to be enough."));
		});

		// Watcher Kenneth
		//-------------------------------------------------------------------------
		AddNpc(147423, L("Watcher Kenneth"), "GELE573_KENNETH", "f_gele_57_3", 799, -183, 168, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher Kenneth"));

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("I didn't know the demons would use summoning circles to come through."));
				var answer = await dialog.SelectQuestOffer(Mq03, L("I suggest removing them all before they besiege us."),
					Option(L("I'll destroy the Demon Summoning Circles"), "accept"),
					Option(L("It will be fine"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					character.LookAround();
				}

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				var answer = await dialog.SelectQuestOffer(Mq04, L("It might be a bit extreme, but I'm thinking about eradicating all the demon's souls."),
					Option(L("Alright"), "accept"),
					Option(L("It's too difficult"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Weaken them below half and use the holy powers of the barrier to sever their souls."));
					character.Quests.Start(Mq04);
				}
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("I'm exhausted and would like to rest for a bit."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("I just need some time, so do you mind taking care of the demons around here?"),
					Option(L("I'll defeat the demons while resting"), "accept"),
					Option(L("Cheer up and hold on"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq05);

				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				if (character.Quests.IsCompletable(Mq03))
				{
					await dialog.Msg(L("You can't let your guard down just because the Demon Summoning Circles are gone."));
					await dialog.Msg(L("We don't even know what caused them to appear in the first place."));
					return;
				}

				await dialog.Msg(L("Even with this many demons, you still managed to make it through."));
				await dialog.Msg(L("You may be the Revelator, but that's still impressive."));
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				if (character.Quests.IsCompletable(Mq04))
				{
					await dialog.Msg(L("Honestly, I had my doubts, but it seems to be working?"));
					await dialog.Msg(L("Please let Kayetonas know about this."));
					await dialog.Msg(L("I think he will be pleased, he is in Flower Greeting Hill."));
					return;
				}

				await dialog.Msg(L("You better eliminate the severed souls as fast as possible."));
				await dialog.Msg(L("Wouldn't it be scary if they resurrected again?"));
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				if (character.Quests.IsCompletable(Mq05))
				{
					await dialog.Msg(L("Now, I think I can live."));
					await dialog.Msg(L("We have to keep it up and stop the demons."));
					await dialog.Msg(L("Please tell Kayetonas that protecting this place isn't easy."));
					return;
				}

				await dialog.Msg(L("Holding off against this many enemies is by itself a great feat."));
				await dialog.Msg(L("It's all thanks to the Paladin Master."));
				return;
			}

			await dialog.Msg(L("The demons keep coming. It never seems to end."));
		});

		// Follower Kayetonas
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Kayetonas"), "GELE573_KAROLINA", "f_gele_57_3", 266, 546, 85, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Kayetonas"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("These are pieces of the barrier at Mazas Rest Place."));
				await dialog.Msg(L("Do not worry. There are ways to recover the barrier, so I'm sure it will be alright."));
				await dialog.CompleteQuest(Mq01);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("So the Tree Guard Post Barrier has been charged with demon souls, huh."));
				await dialog.Msg(L("It still needs some final touches, but I'll take care of it from here. Great job."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("No wonder. I was bewildered when the demons suddenly appeared out of nowhere."));
				await dialog.Msg(L("I better let my brothers know about this. Thank you."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("It's amazing you can use the barrier like that."));
				await dialog.Msg(L("Even the Paladins did not know about that."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Even though he's still young, Kenneth is amazing."));
				await dialog.Msg(L("I think we can trust him a little more."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("The demons are very strong."));
				await dialog.Msg(L("I will protect this place to block those demons."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I have received an urgent message from the Watchers."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("A gigantic demon monster is coming this way. Quickly, follow me."),
					Option(L("I'll go with you"), "accept"),
					Option(L("About the Followers"), "explain"),
					Option(L("I need to prepare myself"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("We are the people following the Paladin Master."));
					await dialog.Msg(L("We will defeat those who disobey the goddess."));
					return;
				}

				if (answer == "accept")
					character.Quests.Start(Mq06);

				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The Minotaur is coming. Stay close to me."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("The Paladin Master is above, at Uzbaiga Hillside."));
		});

		// Paladin Master
		//-------------------------------------------------------------------------
		AddNpc(57223, L("[Paladin Master]{nl}Valentinas Naimon"), "GELE573_MASTER", "f_gele_57_3", 62, -135, 89, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Paladin Master"));
			dialog.SetPortrait("Dlg_port_Vlaentinas_Naimon");

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("I asked my precious friend Algis to go to the Tenet Church."));
				await dialog.Msg(L("I won't lose to Gesti, but it is important to have some backup plan just in case."));
				await dialog.CompleteQuest(Mq07);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("The Throneweaver is down, but the Divine Sphere has not caught her yet. Stay close."));
				await dialog.CompleteQuest(Mq09);
				character.Quests.Start(Mq08);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("Sorry. My abilities weren't good enough."));
				await dialog.Msg(L("Gesti has now realized that the Holy Relic is not the revelation, and is probably heading straight to the church."));
				await dialog.CompleteQuest(Mq08);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Hq02) && character.Quests.IsCompletable(Hq02))
			{
				await dialog.Msg(L("The Chapparition will not bring back the lives of those who died."));
				await dialog.Msg(L("It's a sad thing... But we can't just leave it like that."));
				await dialog.CompleteQuest(Hq02);
				return;
			}

			if (!character.Quests.Has(Reveal2) && character.Quests.MeetsPrerequisites(Reveal2))
			{
				await dialog.Msg(L("So Gesti just ran away."));
				await dialog.Msg(L("Fine. Since you got the revelation, we've completed the mission here."));
				character.Quests.Start(Reveal2);
				character.Quests.CompleteObjective(Reveal2, "tellStory");
				await dialog.CompleteQuest(Reveal2);
				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("Welcome. I'm glad to see that you've had a safe journey here."));
				var answer = await dialog.SelectQuestOffer(Mq07, L("Before anything else, there are a few things about this place that the Revelator should know."),
					Option(L("I'll ask"), "accept"),
					Option(L("I'm not ready to hear it yet"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("There is enough time, so go meet with Follower Algis first."));
					character.Quests.Start(Mq07);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("Starting from now, I will focus on the Divine Sphere."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("In the meantime, please use your skills so that nothing can disturb me."),
					Option(L("Yes, I'll help you concentrate"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq09);

				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				var answer = await dialog.SelectQuestOffer(Mq08, L("The Paladin Master is looking for you."),
					Option(L("What is happening?"), "accept"),
					Option(L("Wait a moment"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq08);

				return;
			}

			if (!character.Quests.Has(Hq02) && character.Quests.MeetsPrerequisites(Hq02))
			{
				var answer = await dialog.SelectQuestOffer(Hq02, L("Without the blessing from the goddess, you would probably be with the goddess now too. Nonetheless, a lot of people are already like without such blessing."),
					Option(L("I will find and defeat Chapparition"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Hq02);

				return;
			}

			if (character.Quests.IsActive(Hq02))
			{
				await dialog.Msg(L("I will pray for that monster. If it even has a soul to listen to my prayer..."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Follower Algis is beside me. Hear what he has to say."));
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("Hold the line while I focus on the Divine Sphere."));
				character.Quests.ReplayQuestTrack(Mq09);
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("Gesti is near. We end this here."));
				character.Quests.ReplayQuestTrack(Mq08);
				return;
			}

			await dialog.Msg(L("The demons press on Nefritas Cliff. The Followers hold the line."));
		});

		// Follower Algis
		//-------------------------------------------------------------------------
		AddConditionalNpc(11281, L("Follower Algis"), "GELE573_MQ_07_F", "f_gele_57_3", 86, -110, 90, c => c.Quests.IsActive(Mq07), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Algis"));
			dialog.SetPortrait("Dlg_port_algis");

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
			{
				while (!character.Quests.IsCompletable(Mq07))
				{
					var topic = await dialog.Select(L("The moment that the goddess foretold to the first Paladin has finally arrived. Come. Ask me anything."),
						Option(L("About the Holy Relic"), "relic"),
						Option(L("About the First Paladin"), "paladin"),
						Option(L("About the next course of action"), "plan"),
						Option(L("Quit"), "quit")
					);

					if (topic == "relic")
					{
						await dialog.Msg(L("A long, long time ago, there were nomads who suffered from a cursed plague. The honorable first Paladin removed the curse with a Holy Relic."));
						await dialog.Msg(L("The Watchers are the descendants of those nomads. However, the Holy Relic was not meant for healing those people."));
						character.Quests.CompleteObjective(Mq07, "aboutRelic");
					}
					else if (topic == "paladin")
					{
						await dialog.Msg(L("One day, the goddess requested the first Paladin to safeguard a divine revelation. The cursed nomads appeared just as the paladin mulled on how to keep it secret."));
						await dialog.Msg(L("They and the first Paladin made three pledges before breaking the curse. To build the Tenet Church together. To conceal the Holy Relic within these grounds and worship its holiness. To gather and fight together when danger comes close to this location."));
						await dialog.Msg(L("All this, was to protect the revelation hidden in the sanctum of the Tenet Church."));
						character.Quests.CompleteObjective(Mq07, "aboutPaladin");
					}
					else if (topic == "plan")
					{
						await dialog.Msg(L("They say the Holy Relic was made to protect the revelations. This is what you need to use to lure in and defeat Gesti."));
						await dialog.Msg(L("Anything can happen though. If the plan fails, then the final battle will likely happen in the Tenet Church."));
						character.Quests.CompleteObjective(Mq07, "aboutPlan");
					}
					else
					{
						break;
					}
				}

				return;
			}

			await dialog.Msg(L("The church is below, past the cliff road. Hold it in your mind."));
		});

		// Watcher James
		//-------------------------------------------------------------------------
		AddNpc(147406, L("Watcher James"), "GELE_57_3_HQ01_NPC01", "f_gele_57_3", -638, -1276, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Watcher James"));

			if (character.Quests.IsActive(Hq01) && character.Quests.IsCompletable(Hq01))
			{
				await dialog.Msg(L("Haha, I lost."));
				await dialog.Msg(L("You are great as I heard so. Here, it's your reward as promised."));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				var answer = await dialog.SelectQuestOffer(Hq01, L("Vubbe Tokens, Panto Horns, Merog Hearts, and Hogma Teeth. That is a lot of stuff!"),
					Option(L("I will accept that bet"), "accept"),
					Option(L("Not right now"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Try and perform a killing spree of the monsters in the Owl Burial Ground."));
					await dialog.Msg(L("Around 40 of them? I bet you can't do it."));
					character.Quests.Start(Hq01);
				}
				return;
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("The monsters in the Owl Burial Ground should be a fair bet for you. Right?"));
				return;
			}

			await dialog.Msg(L("A wager is a wager. The Owl Burial Ground is waiting."));
		});

		// Barrier Pieces at Mazas Rest Place
		//-------------------------------------------------------------------------
		QuestSpots.Add(new QuestSpotSpec
		{
			Prefix = "GELE573_MQ_01",
			MonsterId = 147380,
			Name = L("Barrier Piece"),
			Map = "f_gele_57_3",
			Points = [(-568, -915, 90), (-552, -808, 90), (-657, -807, 90), (-689, -650, 90), (-568, -593, 90), (-428, -775, 90), (-414, -923, 90), (-427, -482, 90), (-268, -498, 90), (-233, -651, 90)],
			IsActive = c => c.Quests.IsActive(Mq01) && !c.Quests.IsCompletable(Mq01),
			TimedLabel = L("Picking up"),
			TimedAnim = "SITGROPESET2",
			Seconds = 2,
			AggroRadius = 180,
			IdleMessage = L("Broken pieces of an old barrier lie scattered at Mazas Rest Place."),
			OnDone = (character, npc) =>
			{
				character.ServerMessage(L("Acquired a piece of the destroyed barrier"));
				character.Inventory.Add(650704, 1, InventoryAddType.PickUp);
			},
		}, npc => npc.AddEffect(new AttachEffect("F_levitation006_loop", 0.5f, EffectLocation.Bottom)));

		// Tree Guard Post Barrier
		//-------------------------------------------------------------------------
		AddNpc(147413, L("Tree Guard Post Barrier"), "GELE573_BASIC_1", "f_gele_57_3", 249, -733, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tree Guard Post Barrier"));

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("The barrier only takes in the souls of demons defeated near it."));
				return;
			}

			await dialog.Msg(L("The Tree Guard Post Barrier flickers, not yet fully charged."));
		});

		// Demon Summoning Circles
		//-------------------------------------------------------------------------
		QuestSpots.Add(new QuestSpotSpec
		{
			Prefix = "GELE573_MQ_03",
			MonsterId = 147372,
			Name = L("Demon Summoning Circle"),
			Map = "f_gele_57_3",
			Points = [(630, -660, 90), (736, -760, 90), (877, -665, 90), (1003, -638, 90), (1123, -572, 90), (1165, -476, 90), (1080, -433, 90), (940, -473, 90), (808, -463, 90), (719, -537, 90), (1019, -304, 90), (871, -297, 90), (943, -625, 90)],
			IsActive = c => c.Quests.IsActive(Mq03) && !c.Quests.IsCompletable(Mq03),
			TimedLabel = L("Removing"),
			TimedAnim = "MAKING",
			Seconds = 2,
			GuardClassNames = ["zigri_brown", "zigri_brown"],
			GuardMessage = L("Defeat the monsters protecting the Demon Summoning Circle first!"),
			GuardEffect = "I_cleric_hexing_cast_dark",
			GuardEffectScale = 2f,
			OnDone = (character, npc) =>
			{
				npc?.PlayEffect("I_bomb001_orange", 1f);
				character.Quests.AddObjectiveProgress(Mq03, "removeCircles");
				character.ServerMessage(LF("You've removed the Demon Summoning Circle! ({0} / {1})", character.Variables.Temp.GetInt(QuestSpots.CountVar("GELE573_MQ_03"), 0), CirclesNeeded));
			},
		}, npc =>
		{
			npc.AddEffect(new AttachEffect("F_ground053_lineup", 7f, EffectLocation.Bottom));
			npc.AddEffect(new AttachEffect("I_smoke007_green", 1f, EffectLocation.Bottom));
		});

		// Pumpura Hill Barrier
		//-------------------------------------------------------------------------
		AddNpc(147413, L("Pumpura Hill Barrier"), "GELE573_MQ_04", "f_gele_57_3", 823, 90, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pumpura Hill Barrier"));

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
			{
				if (_pumpuraBarrierReadyAt > GameClock.LocalNow)
				{
					character.ServerMessage(L("The barrier's power is spent and it is recharging!"));
					return;
				}

				var severed = await character.TimeActions.StartAsync(L("Barrier operating..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(1));

				if (severed != TimeActionResult.Completed)
					return;

				dialog.Npc.PlayEffect("F_explosion004_yellow", 1f);
				SeverSouls(character);
				return;
			}

			await dialog.Msg(L("The barrier at Pumpura Hill pulses with holy power."));
		});
	}

	/// <summary>
	/// Splits the souls of the demons around the Pumpura Hill barrier into weak copies.
	/// </summary>
	private static void SeverSouls(Character character)
	{
		var now = GameClock.LocalNow;
		var demons = character.Map.GetAttackableEnemiesInPosition(character, PumpuraBarrierPosition, 120)
			.OfType<Mob>()
			.Where(mob => !mob.IsDead && mob.Race == RaceType.Velnias && !mob.Vars.GetBool(SeveredSoulVar, false) && !(mob.Vars.TryGet<DateTime>(SeveredSourceVar, out var until) && until > now))
			.Where(mob => mob.Hp * 2 <= mob.MaxHp)
			.ToList();

		if (demons.Count == 0)
		{
			character.ServerMessage(L("The barrier's power only works when there are weakened demon monsters around it."));
			return;
		}

		_pumpuraBarrierReadyAt = now.AddSeconds(1);

		foreach (var demon in demons)
		{
			demon.Vars.Set(SeveredSourceVar, now.AddSeconds(20));

			if (demon.Components.TryGet<AiComponent>(out var ai))
			{
				ai.Script.ClearTarget();
				ai.Script.Suspend(TimeSpan.FromSeconds(20));
			}

			var soul = new Mob(demon.Data.Id, RelationType.Enemy);
			soul.Name = L("Separated Demon Soul");
			soul.Position = demon.Position;
			soul.SpawnPosition = demon.Position;
			soul.Level = 15;
			soul.Vars.SetBool(SeveredSoulVar, true);
			soul.Vars.Set(SeveredOriginalVar, demon);
			soul.Components.Add(new LifeTimeComponent(soul, TimeSpan.FromSeconds(20)));
			soul.Components.Add(new MovementComponent(soul));
			soul.Components.Add(new AiComponent(soul, "TrackWaitMonster"));

			character.Map.AddMonster(soul);
		}

		character.ServerMessage(L("The demons' souls were separated by divine power. Defeat the separated souls!"));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8538: Destroyed Barrier
//-----------------------------------------------------------------------------
public class Gele573Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8538);
		SetName(L("Destroyed Barrier"));
		SetDescription(L("Collect the pieces of the barrier the demons destroyed at Mazas Rest Place."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_ALLEN", "f_gele_57_3", L("Talk to Watcher Allen"), L("Watcher Allen is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_01_5", "f_gele_57_3", L("Collect Destroyed Barrier Piece"), L("Collect the pieces of the barrier at Mazas Rest Place."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Give the pieces to Follower Kayetonas."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("collectPieces", L("Collect Destroyed Barrier Piece"), new CollectItemObjective("GELE573_MQ_01_ITEM", 5));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new SelectItemReward("LEG02_160", "LEG02_161", "LEG02_162"));
		AddReward(new TakeItemReward("GELE573_MQ_01_ITEM"));
	}
}

// 8539: Out of Time...
//-----------------------------------------------------------------------------
public class Gele573Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8539);
		SetName(L("Out of Time..."));
		SetDescription(L("Charge the Tree Guard Post Barrier with demon souls."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_ALLEN", "f_gele_57_3", L("Talk to Watcher Allen"), L("Watcher Allen is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_BASIC_1", "f_gele_57_3", L("Charge the Tree Guard Post Barrier"), L("Defeat the demons around the Tree Guard Post Barrier and charge it."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Inform Follower Kayetonas the barrier has been charged."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("chargeBarrier", L("Charge the Tree Guard Post Barrier"), new ScoreKillObjective(7, (mob, character) => mob.Race == RaceType.Velnias && mob.Position.InRange2D(new Position(249, 0, -733), 230) ? 1 : 0));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8540: Demon Summoning Circle
//-----------------------------------------------------------------------------
public class Gele573Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8540);
		SetName(L("Demon Summoning Circle"));
		SetDescription(L("Remove the demon summoning circles at Mairunas Knoll."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_03_12", "f_gele_57_3", L("Remove the Demon Summoning Circles"), L("Remove the summoning circles at Mairunas Knoll."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Tell Follower Kayetonas there were Demon Summoning Circles."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("removeCircles", L("Remove the Demon Summoning Circles"), new ManualObjective(7));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8541: To the Goddess at Once
//-----------------------------------------------------------------------------
public class Gele573Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8541);
		SetName(L("To the Goddess at Once"));
		SetDescription(L("Use the barrier's holy power to sever the demons' souls at Pumpura Hill."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_04", "f_gele_57_3", L("Defeat severed demons' souls"), L("Weaken the demons and use the barrier to sever their souls."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Report to Follower Kayetonas."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("severSouls", L("Defeat severed demons' souls"), new KillObjective(10, "puragi_green", "banshee", "zigri_brown", "Deadbornscab_mage") { Filter = monster => monster is Mob mob && mob.Vars.GetBool(FGele573QuestNpcsScript.SeveredSoulVar, false) });

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8542: Kenneth's Protector
//-----------------------------------------------------------------------------
public class Gele573Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8542);
		SetName(L("Kenneth's Protector"));
		SetDescription(L("Defeat the demons around Kenneth while he rests."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KENNETH", "f_gele_57_3", L("Talk to Watcher Kenneth"), L("Watcher Kenneth is waiting for someone's help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_KENNETH", "f_gele_57_3", L("Defeat the demon monsters nearby Kenneth"), L("Defeat the nearby monsters while Kenneth rests."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Tell Follower Kayetonas about it."));

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killDemons", L("Defeat demons"), new KillObjective(9, "puragi_green", "banshee", "zigri_brown"));

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8543: Bull Hunting
//-----------------------------------------------------------------------------
public class Gele573Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8543);
		SetName(L("Bull Hunting"));
		SetDescription(L("A gigantic demon monster is coming. Follow Kayetonas and defeat the Minotaur."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Follower Kayetonas is looking for someone who can fight with him."));
		SetPhase(QuestStatus.InProgress, "GELE573_KAROLINA", "f_gele_57_3", L("Defeat Minotaur"), L("Defeat the Minotaur before it reaches the Paladin Master."));
		SetPhase(QuestStatus.Success, "GELE573_KAROLINA", "f_gele_57_3", L("Talk to Follower Kayetonas"), L("Talk to Follower Kayetonas."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_06_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(22));

		AddObjective("killMinotaur", L("Defeat Minotaur"), new KillObjective(1, "boss_Minotaurs") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 2));
	}
}

// 8544: Foreseen Crisis (1)
//-----------------------------------------------------------------------------
public class Gele573Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8544);
		SetName(L("Foreseen Crisis (1)"));
		SetDescription(L("The Paladin Master asks you to listen to Follower Algis about the Tenet Church."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);
		SetPossibleWarp(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is waiting for you at Uzbaiga Hillside."));
		SetPhase(QuestStatus.InProgress, "GELE573_MQ_07_F", "f_gele_57_3", L("Listen to Follower Algis' explanations"), L("Listen to Follower Algis before he leaves."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("Report to the Paladin Master."));

		AddPrerequisite(new QuestStatusPrerequisite(17200, QuestStatus.Completed));

		AddObjective("aboutRelic", L("Ask about the Holy Relic"), new ManualObjective());
		AddObjective("aboutPaladin", L("Ask about the First Paladin"), new ManualObjective());
		AddObjective("aboutPlan", L("Ask about the next course of action"), new ManualObjective());
	}
}

// 8545: Foreseen Crisis (3)
//-----------------------------------------------------------------------------
public class Gele573Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8545);
		SetName(L("Foreseen Crisis (3)"));
		SetDescription(L("The Paladin Master is looking for you. Gesti has appeared."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is looking for you."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Help the Paladin Master"), L("Gesti appeared. Help the Paladin Master."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Report to the Paladin Master"), L("Talk to the Paladin Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_09_AFTER", 2000);

		AddPrerequisite(new QuestStatusPrerequisite(8546, QuestStatus.Completed));

		AddObjective("helpMaster", L("Help the Paladin Master"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new SelectItemReward("TOP02_160", "TOP02_161", "TOP02_162"));
	}
}

// 8546: Foreseen Crisis (2)
//-----------------------------------------------------------------------------
public class Gele573Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8546);
		SetName(L("Foreseen Crisis (2)"));
		SetDescription(L("Clear the area while the Paladin Master focuses on the Divine Sphere."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master is waiting for your help at Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Defeat Throneweaver"), L("Defeat the Throneweaver coming to attack."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("Report to the Paladin Master."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "GELE573_MQ_09_TRACK", 10000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8544, QuestStatus.Completed));

		AddObjective("killThroneweaver", L("Defeat Throneweaver"), new KillObjective(1, "boss_Throneweaver_Q1") { LayerOnly = true });

		AddReward(new ItemReward("R_TOP02_118", 1));
	}
}

// 9102: Proving skills
//-----------------------------------------------------------------------------
public class Gele573Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9102);
		SetName(L("Proving skills"));
		SetDescription(L("Watcher James bets you cannot cut down forty monsters in the Owl Burial Ground."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3", "f_katyn_7_2");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE_57_3_HQ01_NPC01", "f_gele_57_3", L("Talk to Watcher James"), L("James is looking at your bag. Talk to him."));
		SetPhase(QuestStatus.InProgress, "GELE_57_3_HQ01_NPC01", "f_katyn_7_2", L("Defeat the monsters roaming around the Owl Burial Ground"), L("Defeat forty monsters in the Owl Burial Ground."));
		SetPhase(QuestStatus.Success, "GELE_57_3_HQ01_NPC01", "f_gele_57_3", L("Talk to James"), L("You've completed the bet with James. Talk to him."));

		AddPrerequisite(new ItemPrerequisite("misc_0013", 60));
		AddPrerequisite(new ItemPrerequisite("misc_0018", 60));
		AddPrerequisite(new ItemPrerequisite("misc_0048", 60));
		AddPrerequisite(new ItemPrerequisite("misc_0061", 60));

		AddObjective("killOwlGround", L("Defeat the monsters in Owl Burial Ground"), new KillObjective(40, "ellomago", "Ridimed", "jellyfish_red", "Sakmoli"));

		AddReward(new PropertyReward(PropertyName.MSP, 20));
	}
}

// 9104: The One Who Experienced Death
//-----------------------------------------------------------------------------
public class Gele573Hq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9104);
		SetName(L("The One Who Experienced Death"));
		SetDescription(L("A monster is slaying people in the Tenet Church. Hunt the Chapparition."));
		SetType(QuestType.Sub);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Talk to the Paladin Master"), L("The Paladin Master has something to say. Talk to him."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Defeat Field Boss Chapparition"), L("Find and defeat the spooky Chapparition in the Tenet Church."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Report to the Paladin Master"), L("Report to the Paladin Master."));

		AddPrerequisite(new PredicatePrerequisite(character => character.Variables.Perm.GetInt(FGele573QuestNpcsScript.AltarResurrectionsVar, 0) >= 3));

		AddObjective("killChapparition", L("Defeat spooky Chapparition"), new KillObjective(1, "boss_Chapparition"));

		AddReward(new PropertyReward(PropertyName.MSTA, 5));
	}
}

// 30031: The Hidden Sanctum's Revelation (2)
//-----------------------------------------------------------------------------
public class Chaple577Mq10AfterQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(30031);
		SetName(L("The Hidden Sanctum's Revelation (2)"));
		SetDescription(L("Tell the Paladin Master the story of the Tenet Church."));
		SetType(QuestType.Main);
		SetLocation("f_gele_57_3");
		SetAutoTracked(true);
		SetCancelable(true);
		SetPossibleWarp(true);

		SetPhase(QuestStatus.Possible, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Go to the Paladin Master in Nefritas Cliff."));
		SetPhase(QuestStatus.InProgress, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Tell the Paladin Master about the story so far."));
		SetPhase(QuestStatus.Success, "GELE573_MASTER", "f_gele_57_3", L("Tell the Paladin Master about the story so far"), L("Tell the Paladin Master about the story so far."));

		AddPrerequisite(new QuestStatusPrerequisite(8537, QuestStatus.Completed));

		AddObjective("tellStory", L("Tell the Paladin Master about the story so far"), new ManualObjective());
	}
}
