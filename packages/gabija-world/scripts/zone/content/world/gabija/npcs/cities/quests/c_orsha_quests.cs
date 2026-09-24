//--- Melia Script ----------------------------------------------------------
// Orsha Quest NPCs
//--- Description -----------------------------------------------------------
// The lord of Orsha, the church's priests and the merchants the search for
// Bishop Urbonas runs through.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.Scripting.Hooking;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class COrshaQuestNpcsScript : GeneralScript
{
	private readonly static QuestId Siau16Mq07 = new QuestId(60076);
	private readonly static QuestId Mq1_01 = new QuestId(60085);
	private readonly static QuestId Mq1_02 = new QuestId(60086);
	private readonly static QuestId Mq1_03 = new QuestId(60087);
	private readonly static QuestId Mq1_04 = new QuestId(60088);
	private readonly static QuestId Mq2_01 = new QuestId(60112);
	private readonly static QuestId Mq2_02 = new QuestId(60113);
	private readonly static QuestId Mq2_03 = new QuestId(60114);
	private readonly static QuestId Mq3_01 = new QuestId(60145);
	private readonly static QuestId Hq1 = new QuestId(50258);
	private readonly static QuestId Siau11reMq06 = new QuestId(60104);
	private readonly static QuestId Prison623Mq07 = new QuestId(60141);
	private readonly static QuestId Siauliai15Hq1 = new QuestId(50270);
	private readonly static QuestId Bracken631Sq040 = new QuestId(50097);
	private readonly static QuestId Bracken631Sq050 = new QuestId(50098);
	private readonly static QuestId Abbay643Sq020 = new QuestId(50139);
	private readonly static QuestId Abbay643Sq030 = new QuestId(50140);
	private readonly static QuestId Abbay643Sq040 = new QuestId(50141);
	private readonly static QuestId Katyn12Sq01 = new QuestId(30072);

	protected override void Load()
	{
		// [Lord of Orsha] Inesa Hamondale
		//-------------------------------------------------------------------------
		AddNpc(20065, L("[Lord of Orsha]{nl}Inesa Hamondale"), "C_ORSHA_HAMONDAIL", "c_orsha", 86.99, 713.25, 10, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Inesa Hamondale"));
			dialog.SetPortrait("Dlg_port_InesaHamondail");

			if (character.Quests.IsCompletable(Siau16Mq07))
			{
				await dialog.Msg(L("Greetings. I am the ruler of Orsha, Inesa Hamondale."));
				await dialog.Msg(L("I've heard some rumors about a traveler recently. Said traveler has been saving villagers not even the kingdom nor the lord could protect."));
				await dialog.Msg(L("There are even some that say that the traveler was sent by the goddesses. I have heard that the traveler... is you."));
				await dialog.CompleteQuest(Siau16Mq07);
				return;
			}

			if (character.Quests.IsCompletable(Mq1_01))
			{
				await dialog.Msg(L("I must find Bishop Urbonas. The only place the settlers and I can turn to in a world where even the goddesses aren't answering is Bishop Urbonas."));
				await dialog.Msg(L("Four years ago during Medzio Diena... I rose to lordship as I was the only heir after my father and brother were lost on that fateful day."));
				await dialog.Msg(L("Back then, there were only the threats of people trying to manipulate or threaten me... Bishop Urbonas was the only person that protected me and helped Orsha endure."));
				await dialog.CompleteQuest(Mq1_01);
				return;
			}

			if (character.Quests.IsCompletable(Mq2_01))
			{
				var relayed = await character.TimeActions.StartAsync(L("Relaying the journal's contents..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (relayed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("You mean to say that Bishop Urbonas... is being chased by demons? The Ashaq Underground Prison has been closed off many years ago."));
				await dialog.Msg(L("I don't know what he's trying to achieve by hiding there. I... I don't know what to do..."));
				await dialog.CompleteQuest(Mq2_01);
				return;
			}

			if (character.Quests.IsCompletable(Prison623Mq07))
			{
				await dialog.Msg(L("I have heard of what you've done from Bishop Urbonas. All that you've done for us... I don't know how to thank you."));
				await dialog.Msg(L("I also knew of the legends passed down by the bishops of Orsha. I never thought that the Revelator of the same legends would be the one to save Bishop Urbonas..."));
				await dialog.CompleteQuest(Prison623Mq07);
				dialog.ShowHelp("TUTO_INCOMPATIBLE");
				return;
			}

			if (character.Quests.IsCompletable(Siauliai15Hq1))
			{
				await dialog.Msg(L("This report...? It was sent by the agent in Woods of the Linked Bridges."));
				await dialog.Msg(L("I'll read it now."));
				character.ServerMessage(L("The Lord of Orsha is reading the report."));
				await dialog.Msg(L("It seems eliminating even 1000 monsters didn't too much to reduce their numbers... We need to find a solution to protect the areas around Orsha from the monsters."));
				await dialog.CompleteQuest(Siauliai15Hq1);
				return;
			}

			if (!character.Quests.Has(Mq1_01) && character.Quests.MeetsPrerequisites(Mq1_01))
			{
				await dialog.Msg(L("The reason I was looking for skillful people like you... Well, before we get to the point, let me explain a little about the situation that Orsha is in."));
				await dialog.Msg(L("The appearance of Orsha... You must have been shocked as well. As you can see, Orsha is still being rebuilt."));
				await dialog.Msg(L("The entire population of Orsha is being mobilized for the reconstruction, so we barely have enough soldiers to protect the city. In other words, we really can't dispatch troops or officials to other regions."));
				await dialog.Msg(L("Of course, the monsters don't care about our woes. The collective damage to the settlers is growing, and there are already several villages that have been destroyed."));
				await dialog.Msg(L("We are moving all of the surviving settlers to Orsha... But we don't have the hands, nor the resources, to work out the complaints that are piling up."));
				await dialog.Msg(L("That's the short version of the situation... Now let's get back to the point at hand."));
				await dialog.Msg(L("In Orsha, there is a grave matter that can't be announced publicly. We're turning to you for help since, as I've told you, we are barely holding everything together."));

				var answer = await dialog.SelectQuestOffer(Mq1_01, L("Someone with your skills should be able to take care of this problem. I know it's a bit sudden to ask of you to help like this, but we're grasping for straws at this point."),
					Option(L("I'll help you if you take me to the bishop of Orsha"), "accept"),
					Option(L("I'm afraid that'll be impossible"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1_01);
					character.Quests.CompleteObjective(Mq1_01, "hearOut");

					await dialog.Msg(L("You said you're looking for Bishop Urbonas? Well that makes things easy."));
					await dialog.Msg(L("What I wish to ask of you is to find Bishop Urbonas."));
					await dialog.Msg(L("There is a figure that the people of Orsha turn to more than the lord. That figure is Bishop Urbonas."));
					await dialog.Msg(L("Bishop Urbonas is the pillar sustaining the people of Orsha at the moment. That pillar is now missing."));
					await dialog.Msg(L("I've been asking around to try to figure out what happened, but it seems as if the priests have all disappeared as well."));
					await dialog.Msg(L("My men are all looking for the whereabouts of Bishop Urbonas... But it's pretty much a dead-end at this point."));
					await dialog.Msg(L("If you're as good as the rumors make you out to be, you'll have no trouble in finding where Bishop Urbonas is. Just make sure nobody knows about this... The lord doesn't want the people being any more agitated than they already are."));
				}
				return;
			}

			if (!character.Quests.Has(Mq1_02) && character.Quests.MeetsPrerequisites(Mq1_02))
			{
				await dialog.Msg(L("First of all, there are reports of demons at the Woods of the Linked Bridges and Paupys Crossing. Perhaps they have something to do with the disappearance of Bishop Urbonas."));

				var answer = await dialog.SelectQuestOffer(Mq1_02, L("An ominous feeling... is tightening around me. Please find Bishop Urbonas for Orsha and the settlers."),
					Option(L("Alright"), "accept"),
					Option(L("I don't think that'll be needed"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1_02);

					await dialog.Msg(L("Before you leave, please stop by the Statue of Goddess Ausrine in the Central Plaza. Goddess Statues will become inseparable guides for your travels in the future."));
					await dialog.Msg(L("I've also taken the liberty of preparing a few meager supplies, so stop by at the Item Merchant to pick them up. I'll contact you again once you've prepared."));
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Worship the Statue of Goddess Ausrine at the Central Plaza!"), 8);
				}
				return;
			}

			if (!character.Quests.Has(Mq1_04) && character.Quests.MeetsPrerequisites(Mq1_04))
			{
				await dialog.Msg(L("There has just been a report from an agent that was searching through the Woods of the Linked Bridges. It says that they've found a clue to the whereabouts of Bishop Urbonas."));
				await dialog.Msg(L("Priest Pranas was the one who headed the search this time. He only returned recently after being away for a long time."));
				await dialog.Msg(L("He has known Bishop Urbonas for a long time, so I'm sure that he'll be of great help in the search."));

				var answer = await dialog.SelectQuestOffer(Mq1_04, L("Go to Agent Cherasia, one of my immediate subordinates, who is at the Woods of the Linked Bridges when you're ready. You'll be able to find out what happened since the report and what they're planning to do next."),
					Option(L("I will go there right away"), "accept"),
					Option(L("I'm not quite ready yet"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1_04);
					character.Quests.CompleteObjective(Mq1_04, "meetCherasia");
				}
				return;
			}

			if (!character.Quests.Has(Mq2_02) && character.Quests.MeetsPrerequisites(Mq2_02))
			{
				await dialog.Msg(L("It will take too long to gather troops to go there. They may not be of much help with demons since they only have experience against monsters."));
				await dialog.Msg(L("I'm sorry, but could you go to the Ashaq Underground Prison with Priest Pranas? You're the only one left that I can rely on in this situation..."));

				var answer = await dialog.SelectQuestOffer(Mq2_02, L("Whatever it takes, please save Bishop Urbonas. I, Inesa Hamondale, the lord of Orsha, plead for your assistance."),
					Option(L("I'll do it no matter what"), "accept"),
					Option(L("I don't want to get caught up in anything big"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2_02);
					character.Quests.CompleteObjective(Mq2_02, "visitAlf");

					await dialog.Msg(L("Bishop Urbonas always preached about the importance of the ties brought by coincidence. He always said that Goddess Laima would send a Revelator."));
					await dialog.Msg(L("Yes. Maybe you... Maybe you are the Revelator that is mentioned in the legends passed down among the bishops of Orsha."));
					await dialog.Msg(L("The church... Bishop Urbonas has been waiting for a Revelator his whole life. I will tell you more in detail once you have saved the bishop."));
					await dialog.Msg(L("But you must prepare thoroughly no matter how much of a rush you are in, so please stop by at the Item Merchant. I have prepared some handy supplies for you."));
					await dialog.Msg(L("Please forgive me for the fact that I can only help you in such a fashion even though I am a lord of a terrority. I shall pray... that the goddesses bless you and Bishop Urbonas."));
				}
				return;
			}

			if (!character.Quests.Has(Mq3_01) && character.Quests.MeetsPrerequisites(Mq3_01))
			{
				await dialog.Msg(L("The person that will save not only Orsha, but also the goddesses and the world... It is an honor to meet that Revelator."));
				await dialog.Msg(L("It seems as if the goddesses have not forsaken Orsha even if they have all disappeared. I wish I could let everyone know of your deeds but..."));
				await dialog.Msg(L("I am sorry. I cannot announce that demons have come this close to the city. I hope that you will understand the fact that I cannot spread confusion and fear among my people."));

				var answer = await dialog.SelectQuestOffer(Mq3_01, L("I thank you once more as the lord of Orsha for what you have done. Bishop Urbonas will be waiting for you."),
					Option(L("I will go there right away"), "accept"),
					Option(L("There are still some things to do"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3_01);
					character.Quests.CompleteObjective(Mq3_01, "meetUrbonas");
				}
				return;
			}

			if (character.Quests.IsActive(Mq1_02))
			{
				if (character.Quests.IsCompletable(Mq1_02))
				{
					await dialog.Msg(L("Please keep this a secret. You must not speak of the disappearance of Bishop Urbonas until this case is solved."));
					return;
				}

				await dialog.Msg(L("The settlers will panic if they find out about this."));
				await dialog.Msg(L("A few towns have already fallen to monsters because the migration orders went out later than expected... So the news of the disappearance of Bishop Urbonas will most likely spiral things out of control."));
				await dialog.Msg(L("One of the priests studying beneath him, Pranas, came back not too long ago. I've tried to ask him about what happened, but he doesn't seem to know either."));
				await dialog.Msg(L("My immediate subordinates and hired trackers are searching the Woods of the Linked Bridges and Paupys Crossing from top to bottom. But there hasn't been any headway because of the monsters coming towards Orsha from all directions."));
				return;
			}

			if (character.Quests.IsActive(Mq1_04))
			{
				await dialog.Msg(L("The other agents are currently disguised as being out on other assignments for the sake of secrecy. So I hope you don't do anything rash that will undo all of their efforts."));
				return;
			}

			if (character.Quests.IsActive(Mq2_02))
			{
				await dialog.Msg(L("It is so frustrating. The fact I'm restricted so much even though I am the lord of Orsha..."));
				await dialog.Msg(L("Please forgive me for the fact that I can only help you in such a fashion even though I am a lord of a terrority. I shall pray... that the goddesses bless you and Bishop Urbonas."));
				return;
			}

			if (character.Quests.IsActive(Mq3_01))
			{
				await dialog.Msg(L("I hope that you will keep a place in your heart for Orsha. The people of Orsha will always welcome you here."));
				return;
			}

			if (character.Quests.HasCompleted(Prison623Mq07))
			{
				await dialog.Msg(L("O Revelator, the one who saved Orsha. What can I do for you?"));
				return;
			}

			await dialog.Msg(L("This land has been protected by the ancestors of my family. I will protect this land, no matter what hardship comes my way. Please lend me your power."));
		});

		// Lord's Guards
		//-------------------------------------------------------------------------
		AddNpc(20059, L("Lord's Guard"), "C_ORSHA_SOLDIER_01", "c_orsha", 40.71, 713.98, 14, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lord's Guard"));

			if (character.Quests.HasCompleted(Prison623Mq07))
			{
				await dialog.Msg(L("Oh, you're the Revelator. I've heard a lot about you."));
				await dialog.Msg(L("She may not be able to tell you in person, but the Lord of Orsha is extremely grateful for all you've done."));
				return;
			}

			await dialog.Msg(L("Too many people have been asking to speak to our lord in person. She's outside the walls now trying to answer to them."));
		});

		AddNpc(20060, L("Lord's Guard"), "C_ORSHA_SOLDIER_02", "c_orsha", 140.98, 712.85, -20, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Lord's Guard"));

			if (character.Quests.HasCompleted(Prison623Mq07))
			{
				await dialog.Msg(L("You are always welcome to meet with the Lord directly. Even when she's out answering complaints, for smaller issues you always need to address the other officers, you see."));
				await dialog.Msg(L("But you're welcome to speak to her about any problem if you wish."));
				return;
			}

			await dialog.Msg(L("The lord is extremely busy. Trivial matters should be inquired with a different manager."));
		});

		// Priest Pranas
		//-------------------------------------------------------------------------
		AddConditionalNpc(155044, L("Priest Pranas"), "C_ORSHA_PRANAS", "c_orsha", 915.74, 708.51, 21, IsPranasInOrsha, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Pranas"));

			if (character.Quests.IsCompletable(Siau11reMq06))
			{
				await dialog.Msg(L("Thank you for saving my life back there. My wounds seem to be a bit more bearable since I've rested a little."));
				await dialog.Msg(L("By the way, have you managed to find something?"));
				await dialog.CompleteQuest(Siau11reMq06);
				return;
			}

			if (!character.Quests.Has(Mq2_01) && character.Quests.MeetsPrerequisites(Mq2_01))
			{
				await dialog.Msg(L("This seems to be... a chest. There's a seal, but it's familiar to priests. Let's open it."));
				await dialog.Msg(L("It contains a journal and a map of Ashaq Underground Prison 1F. I do hope that he's not at Ashaq Underground Prison... There's something marked on it."));
				await dialog.Msg(L("The handwriting on this map is most definitely of the bishop. Even though it is written in a language that ordinary people can't read."));

				var answer = await dialog.SelectQuestOffer(Mq2_01, L("I can only translate a part of it right away, would you like to hear it?"),
					Option(L("I want to hear it"), "accept"),
					Option(L("I need to prepare myself"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2_01);
					character.Quests.CompleteObjective(Mq2_01, "relayJournal");

					await dialog.Msg(L("...Once the waves of disaster wash across... The stranger arranged by the goddess shall seek Orsha dreaming of the catastrophe."));
					await dialog.Msg(L("Only one of the countless strangers... To find the one and lead them is our mission."));
					await dialog.Msg(L("I shall go to the Ashaq Underground Prison with my priests. There, I shall await the one and only stranger arranged by the goddess..."));
					await dialog.Msg(L("We started being chased by demons even before we were ready. There is no time to contact the disciples that aren't nearby Inesa."));
					await dialog.Msg(L("If someone finds this diary, I beg of you to pass it on to Inesa Hamondale, the lord of Orsha..."));
					await dialog.Msg(L("...That's all it says... I expected them to be chased by demons, but I didn't know that there was so much more to it."));
					await dialog.Msg(L("As the journal says, report the contents of the journal to the lord. We'll also be preparing to go directly to the Ashaq Underground Prison."));
				}
				return;
			}

			if (!character.Quests.Has(Mq2_03) && character.Quests.MeetsPrerequisites(Mq2_03))
			{
				await dialog.Msg(L("Are you coming with us? Having you with us makes us feel as safe as having a whole army with us."));
				await dialog.Msg(L("According to the bishop's journal, the demons must be after the bishop since he's searching for the Revelator. It would be easier for them to go after the bishop instead of trying to find the Revelator who might be anybody."));

				var answer = await dialog.SelectQuestOffer(Mq2_03, L("Let's go to the Ashaq Underground Prison whenever you're ready. Bishop... I pray that the goddesses are giving their blessings..."),
					Option(L("I will follow soon"), "accept"),
					Option(L("Tell him to wait a bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2_03);
					character.Quests.CompleteObjective(Mq2_03, "joinPranas");

					await dialog.Msg(L("I'll be right behind you with the trackers."));
					await dialog.Msg(L("Let's meet up at Gebene Cliff at Paupys Crossing. The Ashaq Underground Prison is just ahead of that point."));

					character.LookAround();
					character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Join Pranas in front of Gebene Cliff in Paupys Crossing!"), 8);
				}
				return;
			}

			if (character.Quests.IsActive(Mq2_01))
			{
				await dialog.Msg(L("I'll get ready to go to the Ashaq Underground Prison. You should hurry and inform the lord of this information!"));
				return;
			}

			if (character.Quests.HasCompleted(Prison623Mq07))
			{
				await dialog.Msg(L("It's a relief knowing that Irma is safe. I think... I need to rest a little..."));
				return;
			}

			await dialog.Msg(L("I'll get ready to go to the Ashaq Underground Prison. You should hurry and inform the lord of this information!"));
		});

		// [Orsha Bishop] Urbonas
		//-------------------------------------------------------------------------
		AddConditionalNpc(154057, L("[Orsha Bishop]{nl}Urbonas"), "C_ORSHA_URBONAS", "c_orsha", 895.18, 702.91, 0, c => c.Quests.Has(Prison623Mq07), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bishop Urbonas"));

			if (character.Quests.IsCompletable(Mq3_01))
			{
				await dialog.Msg(L("Greetings. Impeccable timing! I've just finished the final interpretation of the Demon Orders."));
				await dialog.Msg(L("There may be a few parts where its inaccurate but... Allow me to explain with a bit of a liberal translation."));
				await dialog.Msg(L("You may leave the human lord alone since we have obtained the holy power we were after. Miss Giltine wants more tangible results."));
				await dialog.Msg(L("Deploy the Kruvina in the predetermined reservoir and execute the spore plan. Death will engulf this world."));
				await dialog.Msg(L("P.S. Miss Giltine has ordered the annihilation of the Revelator. You must get rid of the Revelator before they meet the girl."));
				await dialog.Msg(L("This is all of it. Its contents are so intimidating that I wonder if I actually translated it properly..."));
				await dialog.Msg(L("I will pass this information to the lord... It is frustrating since we can barely protect Orsha much less send out investigation parties."));
				await dialog.Msg(L("What could this 'Kruvina' possibly be... I couldn't find anything mentioned in any of the old texts either."));
				await dialog.Msg(L("Now, Revelator... What is your next plan? It may be simply the worries of an old man, but the spore plan mentioned in the Demon Orders..."));
				await dialog.Msg(L("Spores are merely the seeds of mushrooms or ferns and such. I can't help but wonder if the plan has anything to do with the Koru Jungle famed for its ferns..."));
				await dialog.Msg(L("The choice is yours, of course. Although our meeting may have been a coincidence, you did save me."));
				await dialog.Msg(L("I am sure that the goddess has arranged it so that all of your travels lead to the salvation of all..."));
				await dialog.Msg(L("I bid thee well. I shall pray for the deliverance and blessings of the goddesses to lead and protect you on your journey."));
				await dialog.CompleteQuest(Mq3_01);
				return;
			}

			if (character.Quests.HasCompleted(Mq3_01))
			{
				await dialog.Msg(L("If the orders from the Demon Orders are achieved, Orsha will no longer be safe. I hope our lord will make her decisions wisely. The situation right now is disastrous enough as it is with the migration order."));
				await dialog.Msg(L("The choice is yours. I am sure the goddesses want you to lead our salvation too."));
				return;
			}

			await dialog.Msg(L("The events of the Ashaq Underground Prison were only the beginning. How much more will we have to sacrifice..."));
		});

		// [Cleric Submaster] Tamara Easton
		//-------------------------------------------------------------------------
		AddNpc(155083, L("[Cleric Submaster]{nl}Tamara Easton"), "JOB_2_CLERIC_NPC", "c_orsha", -269.96, -378.04, 80, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cleric Submaster"));

			if (character.Quests.IsCompletable(Bracken631Sq040))
			{
				var told = await character.TimeActions.StartAsync(L("Telling about the symptoms of Herbalist Tales..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (told != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("Tales...? Oh, the one who always found me rare herbs."));
				await dialog.Msg(L("You said the wounds are swollen, hot and discolored? If the Ronjia Grass doesn't work, then he must be affected by evil energy."));
				await dialog.CompleteQuest(Bracken631Sq040);

				if (!character.Quests.HasCompleted(Bracken631Sq040) || character.Quests.Has(Bracken631Sq050))
					return;
			}

			if (!character.Quests.Has(Bracken631Sq050) && character.Quests.MeetsPrerequisites(Bracken631Sq050))
			{
				await dialog.Msg(L("His condition is only expected to get worse with time."));

				var answer = await dialog.SelectQuestOffer(Bracken631Sq050, L("I'm going to give you some medicine to purify the evil energy in his body; please give it to Tales."),
					Option(L("Let's go back to Tales"), "accept"),
					Option(L("Tell him to wait a bit longer"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Bracken631Sq050);
					character.Inventory.Add(ItemId.BRACKEN631_SQ6_ITEM01, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Bracken631Sq050))
			{
				await dialog.Msg(L("Give it him as soon as possible. The longer you wait, the more the evil energy will spread."));
				return;
			}

			await dialog.Msg(L("I see that a follower of the goddesses has come to see me. Greetings, I am the Cleric Submaster."));
		});

		// [Wizard Submaster] Dejamis
		//-------------------------------------------------------------------------
		AddNpc(155069, L("[Wizard Submaster]{nl}Dejamis"), "JOB_2_WIZARD_MASTER", "c_orsha", -274.29, -758.49, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wizard Submaster"));

			if (character.Quests.IsCompletable(Abbay643Sq020))
			{
				var explained = await character.TimeActions.StartAsync(L("Explaining about Rose and her brother..."), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
				if (explained != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("I know Edmundas very well. He would always find me rare herbs faster than anyone."));
				await dialog.Msg(L("What a terrible thing to happen to someone like him..."));
				await dialog.CompleteQuest(Abbay643Sq020);
				return;
			}

			if (!character.Quests.Has(Abbay643Sq030) && character.Quests.MeetsPrerequisites(Abbay643Sq030))
			{
				await dialog.Msg(L("Don't worry. I won't tell anyone about what happened to the siblings."));
				await dialog.Msg(L("I don't know how to cure them right away..."));

				var answer = await dialog.SelectQuestOffer(Abbay643Sq030, L("What I can do for now is make a protective barrier to keep the two of them safe from demon attacks."),
					Option(L("Please tell me how"), "accept"),
					Option(L("I'll be back after I'm ready"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Abbay643Sq030);
					character.Inventory.Add(ItemId.R_ABBAY643_SQ4_ITEM1, 1, InventoryAddType.PickUp);
					character.Inventory.Add(ItemId.ABBAY643_SQ3_ITEM3, 1, InventoryAddType.PickUp);

					await dialog.Msg(L("They say if you want to hide a tree, you put it in a forest. I'm saying we trick the demons using the demons themselves."));
					await dialog.Msg(L("I'm going to give you the recipe and an empty barrier crystal. Collect demon stones from Deadborn Scaps to make the protection barrier crystal."));
				}
				return;
			}

			if (character.Quests.IsActive(Abbay643Sq030))
			{
				await dialog.Msg(L("I'll keep looking for a way to cure the siblings. Tell Edmundas I said hi."));
				return;
			}

			if (character.Quests.IsActive(Abbay643Sq040))
			{
				await dialog.Msg(L("A protective barrier crystal should keep the siblings safe from any threats. Of course the ideal would be to find a real solution..."));
				return;
			}

			await dialog.Msg(L("So, you want to learn new magic, eh? But I need to be certain that you are prepared for it."));
		});

		// Quest dialog hooks for the city's merchants and the goddess statue
		//-------------------------------------------------------------------------
		ScriptHooks.Register(new DialogHook("Alf", "BeforeDialog", AlfDialog));
		ScriptHooks.Register(new DialogHook("Jurus", "BeforeDialog", JurusDialog));
		ScriptHooks.Register(new DialogHook("c_orsha:WARP_C_ORSHA", "BeforeDialog", AusrineStatueDialog));
	}

	/// <summary>
	/// Returns whether Priest Pranas is resting in Orsha.
	/// </summary>
	private static bool IsPranasInOrsha(Character character)
		=> (character.Quests.Has(Siau11reMq06) && !character.Quests.Has(Mq2_03)) || character.Quests.Has(Prison623Mq07);

	/// <summary>
	/// Alf's quest dialog, run before his shop dialog.
	/// </summary>
	private static async Task<HookResult> AlfDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Quests.IsCompletable(Mq1_02))
		{
			await dialog.Msg(L("Oh, hello there, welcome. I've heard that you've saved refugees from danger several times."));
			await dialog.Msg(L("The lord have been very pleased because she sent a person to bestow some supplies. There you go. I'm sure they'll come in handy!"));
			await dialog.CompleteQuest(Mq1_02);
			return HookResult.Break;
		}

		if (character.Quests.IsCompletable(Mq2_02))
		{
			await dialog.Msg(L("I've quickly prepared a few things since the lord sent someone over. This anvil will allow you to enhance your equipment so that they become more effective."));
			await dialog.Msg(L("I'd like to give you something more, but this is all I can offer. I don't know what is happening, but I'm sure you'll pull through."));
			await dialog.CompleteQuest(Mq2_02);
			dialog.ShowHelp("TUTO_REIN");
			return HookResult.Break;
		}

		if (!character.Quests.Has(Mq1_03) && character.Quests.MeetsPrerequisites(Mq1_03))
		{
			await dialog.Msg(L("They say that there are barely any intact cities left in the kingdom. All because of Medzio Diena four years ago... It's a real shame."));
			await dialog.Msg(L("You'll need a lot of utilities on your travels like potions and such. Make sure to stop by often since I have the cheapest prices around."));

			var answer = await dialog.SelectQuestOffer(Mq1_03, L("Oh yes, the lord also requested something of Jurus the Accessory Merchant. Make sure you stop by."),
				Option(L("Thank you for letting me know"), "accept"),
				Option(L("I'm afraid I can't help you with that"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq1_03);
				character.Quests.CompleteObjective(Mq1_03, "visitJurus");
				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Go see the Accessory Merchant"), 8);
			}
			return HookResult.Break;
		}

		if (character.Quests.IsActive(Mq1_03))
		{
			await dialog.Msg(L("Grandfather says that he doesn't know how many more people Orsha can handle. There are more travelers flocking here as well since it's relatively safer than the rest of the kingdom."));
			await dialog.Msg(L("It doesn't feel very good even if the store is doing better. Do you think everyone can be happy if the goddesses return?"));
			return HookResult.Break;
		}

		return HookResult.Skip;
	}

	/// <summary>
	/// Jurus' quest dialog, run before his shop dialog.
	/// </summary>
	private static async Task<HookResult> JurusDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (character.Quests.IsCompletable(Katyn12Sq01))
		{
			var told = await character.TimeActions.StartAsync(L("Talking about the letter"), L("Cancel"), "TALK", TimeSpan.FromSeconds(2));
			if (told != TimeActionResult.Completed)
				return HookResult.Break;

			dialog.SetPortrait("Dlg_port_Yurrs");
			await dialog.Msg(L("This letter is... left by Eras. This is the first time since ages ago that I have heard from him..."));
			await dialog.Msg(L("But now he has gone into the arms of the goddess... Fate is so cruel and gruesome to bear."));
			await dialog.Msg(L("Thank you for delivering this letter to me. At least, now I know what had happened to him."));
			await dialog.CompleteQuest(Katyn12Sq01);
			return HookResult.Break;
		}

		if (character.Quests.IsCompletable(Mq1_03))
		{
			await dialog.Msg(L("Greetings. I have heard of you from the lord. Things aren't going great but I'll give you this since the lord specially asked for something nice."));
			await dialog.Msg(L("Everyone in Orsha is having it rough... But I feel as if we are getting by thanks to the people like you that go out of their way to help others."));
			await dialog.Msg(L("Oh, yes. The lord was looking for you again. You should go back to her."));
			await dialog.CompleteQuest(Mq1_03);
			character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Lord Inesa Hamondale is looking for you"), 10);
			return HookResult.Break;
		}

		if (character.Quests.IsCompletable(Hq1))
		{
			await dialog.Msg(L("They truly are ethereal. To think a simple couple of branches and flowers could be so fascinating..."));
			await dialog.CompleteQuest(Hq1);
			return HookResult.Break;
		}

		if (!character.Quests.Has(Hq1) && character.Quests.MeetsPrerequisites(Hq1))
		{
			await dialog.Msg(L("So it's true, flower branches really are mesmerizing. They're definitely different from any accessory I've ever seen."));

			var answer = await dialog.SelectQuestOffer(Hq1, L("It's the first time I see an accessory so beautiful but mysterious at the same time. If you don't mind, may I look at them for a little longer?"),
				Option(L("I'll show you the Flower Branch."), "accept"),
				Option(L("I'm sorry, but I don't think I can"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Hq1);
				character.Quests.CompleteObjective(Hq1, "showBranch");
				await dialog.Msg(L("Thank you for letting me have another look at them."));
			}
			return HookResult.Break;
		}

		return HookResult.Skip;
	}

	/// <summary>
	/// The Statue of Goddess Ausrine's quest dialog, run before its warp dialog.
	/// </summary>
	private static async Task<HookResult> AusrineStatueDialog(Dialog dialog)
	{
		var character = dialog.Player;

		if (!character.Quests.IsActive(Mq1_02) || character.Quests.IsCompletable(Mq1_02))
			return HookResult.Skip;

		var prayed = await dialog.TimeAction(L("Worshipping the goddess statue..."), L("Cancel"), "WORSHIP", TimeSpan.FromSeconds(2));
		if (prayed != TimeActionResult.Completed)
			return HookResult.Break;

		character.Quests.CompleteObjective(Mq1_02, "worship");
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Go see the Item Merchant"), 8);

		return HookResult.Break;
	}
}

/// <summary>
/// Met while the character wears the Flower Branch hair accessory.
/// </summary>
public class OrshaFlowerBranchPrerequisite : QuestPrerequisite
{
	private const int FlowerBranchItemId = 628093;

	public override bool Met(Character character)
	{
		foreach (var slot in new[] { EquipSlot.HairAccessory, EquipSlot.SubsidiaryAccessory, EquipSlot.Hat })
		{
			var item = character.Inventory.GetEquip(slot);
			if (item != null && item.Id == FlowerBranchItemId)
				return true;
		}

		return false;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 50258: Unusual Eyes
//-----------------------------------------------------------------------------
public class OrshaHq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50258);
		SetName(L("Unusual Eyes"));
		SetDescription(L("Accessory Merchant Jurus seems interested in your hair accessories. Talk to Accessory Merchant Jurus."));
		SetType(QuestType.Sub);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "Jurus", "c_orsha", L("Talk to Accessory Merchant Jurus"));
		SetPhase(QuestStatus.InProgress, "Jurus", "c_orsha", L("Show the Flower Branch Accessory"));
		SetPhase(QuestStatus.Success, "Jurus", "c_orsha", L("Talk to Accessory Merchant Jurus"));

		AddPrerequisite(new OrshaFlowerBranchPrerequisite());

		AddObjective("showBranch", L("Show the Flower Branch Accessory"), new ManualObjective());

		AddReward(new ItemReward("misc_scrollskulp", 1));
	}
}

// 50098: The Injured Herbalist (3)
//-----------------------------------------------------------------------------
public class Bracken631Sq050Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50098);
		SetName(L("The Injured Herbalist (3)"));
		SetDescription(L("The Cleric Submaster gave you a medicine that can purify evil energy. Deliver the medicine to Herbalist Tales."));
		SetType(QuestType.Sub);
		SetLocation("c_orsha", "f_bracken_63_1");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "JOB_2_CLERIC_NPC", "c_orsha", L("Talk with Cleric Submaster"));
		SetPhase(QuestStatus.InProgress, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Deliver the medicine to Herbalist Tales"));
		SetPhase(QuestStatus.Success, "BRACKEN631_PEAPLE01", "f_bracken_63_1", L("Deliver the medicine to Herbalist Tales"));

		AddPrerequisite(new QuestStatusPrerequisite(50097, QuestStatus.Completed));

		AddObjective("deliverMedicine", L("Deliver the medicine to Herbalist Tales"), new CollectItemObjective("BRACKEN631_SQ6_ITEM01", 1));

		AddReward(new ItemReward("expCard2", 1));
		AddReward(new ItemReward("Vis", 70));
		AddReward(new TakeItemReward("BRACKEN631_SQ6_ITEM01", 1));
	}
}

// 50140: Edmundas' Worry (3)
//-----------------------------------------------------------------------------
public class Abbay643Sq030Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50140);
		SetName(L("Edmundas' Worry (3)"));
		SetDescription(L("The Wizard Submaster gave you a recipe for a Protection Barrier Crystal. Defeat Deadborn Scaps at the Novaha Institute to obtain their demon essences, then craft the Protection Barrier Crystal."));
		SetType(QuestType.Sub);
		SetLocation("c_orsha", "d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "JOB_2_WIZARD_MASTER", "c_orsha", L("Talk to the Wizard Submaster"));
		SetPhase(QuestStatus.InProgress, "JOB_2_WIZARD_MASTER", "c_orsha", L("Craft a Protection Barrier Crystal"));
		SetPhase(QuestStatus.Success, "JOB_2_WIZARD_MASTER", "c_orsha", L("Place the Protection Barrier Crystal"));

		AddPrerequisite(new QuestStatusPrerequisite(50139, QuestStatus.Completed));

		AddObjective("craftCrystal", L("Craft a Protection Barrier Crystal"), new CollectItemObjective("ABBAY643_SQ4_ITEM1", 1));
		AddPityDrop("ABBAY643_SQ3_ITEM01", 0.5f, 4, 1, "Sec_Deadbornscab");

		AddReward(new ItemReward("expCard3", 3));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		// The client names no turn-in NPC; placing the crystal is the next quest.
		character.Quests.Complete(this.QuestId);
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("The Protective Barrier Crystal is completed.{nl}Go back to Edmundas and set it!"), 5);

		if (character.Quests.HasCompleted(this.QuestId) && !character.Quests.Has(new QuestId(50141)))
			character.Quests.Start(new QuestId(50141));
	}
}

// 50141: Edmundas' Worry (4)
//-----------------------------------------------------------------------------
public class Abbay643Sq040Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50141);
		SetName(L("Edmundas' Worry (4)"));
		SetDescription(L("The Protection Barrier Crystal is completed. Go back to Edmundas and place the crystal."));
		SetType(QuestType.Sub);
		SetLocation("d_abbey_64_3");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "JOB_2_WIZARD_MASTER", "c_orsha", L("Place the Protection Barrier Crystal"));
		SetPhase(QuestStatus.InProgress, "ABBEY643_MAGIC_POINT01", "d_abbey_64_3", L("Place the Protection Barrier Crystal"));
		SetPhase(QuestStatus.Success, "ABBEY643_EDMONDA04", "d_abbey_64_3", L("Talk to Edmundas"));

		AddPrerequisite(new QuestStatusPrerequisite(50140, QuestStatus.Completed));

		AddObjective("placeCrystal", L("Set up the Protection Barrier Crystal"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 60085: The Missing Bishop (1)
//-----------------------------------------------------------------------------
public class OrshaMq1_01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60085);
		SetName(L("The Missing Bishop (1)"));
		SetDescription(L("Inesa Hamondale has asked you to find the bishop of Orsha, Urbonas, who is missing. Talk to Inesa Hamondale again."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.Success, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));

		AddPrerequisite(new QuestStatusPrerequisite(60076, QuestStatus.Completed));

		AddObjective("hearOut", L("Talk with Inesa Hamondale"), new ManualObjective());
	}
}

// 60086: The Missing Bishop (2)
//-----------------------------------------------------------------------------
public class OrshaMq1_02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60086);
		SetName(L("The Missing Bishop (2)"));
		SetDescription(L("Before you leave, Inesa Hamondale has asked you to worship the Statue of Goddess Ausrine in the Central Plaza, then collect her gift from the Item Merchant."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.InProgress, "c_orsha:WARP_C_ORSHA", "c_orsha", L("Worship the Statue of Goddess Ausrine in the Central Plaza"));
		SetPhase(QuestStatus.Success, "Alf", "c_orsha", L("Visit the Item Merchant in the Shopping District"));

		AddPrerequisite(new QuestStatusPrerequisite(60085, QuestStatus.Completed));

		AddObjective("worship", L("Worship the Statue of Goddess Ausrine in the Central Plaza"), new ManualObjective());

		AddReward(new ItemReward("Scroll_Warp_quest", 10));
	}
}

// 60087: The Missing Bishop (3)
//-----------------------------------------------------------------------------
public class OrshaMq1_03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60087);
		SetName(L("The Missing Bishop (3)"));
		SetDescription(L("It seeems Inesa Hamondale has asked the Accessory Merchant to prepare another gift for you. Go find the Accessory Merchant in the Shopping District."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "Alf", "c_orsha", L("Talk to the Item Merchant"));
		SetPhase(QuestStatus.InProgress, "Jurus", "c_orsha", L("Visit the Accessory Merchant"));
		SetPhase(QuestStatus.Success, "Jurus", "c_orsha", L("Visit the Accessory Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(60086, QuestStatus.Completed));

		AddObjective("visitJurus", L("Visit the Accessory Merchant"), new ManualObjective());

		AddReward(new ItemReward("BRC01_122", 1));
	}
}

// 60088: The Missing Bishop (4)
//-----------------------------------------------------------------------------
public class OrshaMq1_04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60088);
		SetName(L("The Missing Bishop (4)"));
		SetDescription(L("News just came in from Agent Cherasia, who is searching for the missing bishop. Go find the agent in the Woods of the Linked Bridges."));
		SetType(QuestType.Main);
		SetLocation("c_orsha", "f_siauliai_15_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", L("Talk to Agent Cherasia at the Woods of the Linked Bridges"));
		SetPhase(QuestStatus.Success, "SIAULIAI15RE_CHERASIA", "f_siauliai_15_re", L("Talk to Agent Cherasia at the Woods of the Linked Bridges"));

		AddPrerequisite(new QuestStatusPrerequisite(60087, QuestStatus.Completed));

		AddObjective("meetCherasia", L("Talk to Agent Cherasia at the Woods of the Linked Bridges"), new ManualObjective());
	}
}

// 60112: The Dangerous Trace (1)
//-----------------------------------------------------------------------------
public class OrshaMq2_01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60112);
		SetName(L("The Dangerous Trace (1)"));
		SetDescription(L("Most of the contents of Bishop Urbonas' journal remain unknown. For now, relay the journal as it is to Inesa Hamondale."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_PRANAS", "c_orsha", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_HAMONDAIL", "c_orsha", L("Relay the journal's contents to Inesa Hamondale"));
		SetPhase(QuestStatus.Success, "C_ORSHA_HAMONDAIL", "c_orsha", L("Relay the journal's contents to Inesa Hamondale"));

		AddPrerequisite(new QuestStatusPrerequisite(60104, QuestStatus.Completed));

		AddObjective("relayJournal", L("Relay the journal's contents to Inesa Hamondale"), new ManualObjective());
	}
}

// 60113: The Dangerous Trace (2)
//-----------------------------------------------------------------------------
public class OrshaMq2_02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60113);
		SetName(L("The Dangerous Trace (2)"));
		SetDescription(L("Inesa Hamondale prepared a quick gift that will be useful for your journey. Before departing for Ashaq Underground Prison, visit the Item Merchant to collect it."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.InProgress, "Alf", "c_orsha", L("Talk to the Item Merchant"));
		SetPhase(QuestStatus.Success, "Alf", "c_orsha", L("Talk to the Item Merchant"));

		AddPrerequisite(new QuestStatusPrerequisite(60112, QuestStatus.Completed));

		AddObjective("visitAlf", L("Talk to the Item Merchant"), new ManualObjective());

		AddReward(new ItemReward("Moru_W_01Q", 1));
		AddReward(new ItemReward("Drug_HP1_Q", 5));
	}
}

// 60114: The Dangerous Trace (3)
//-----------------------------------------------------------------------------
public class OrshaMq2_03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60114);
		SetName(L("The Dangerous Trace (3)"));
		SetDescription(L("Priest Pranas says he is going to gather a few talented Chasers and head to the entrance of Ashaq Underground Prison. Join him by the Gebene Cliff in Paupys Crossing."));
		SetType(QuestType.Main);
		SetLocation("c_orsha", "f_siauliai_11_re");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_PRANAS", "c_orsha", L("Talk with Priest Pranas"));
		SetPhase(QuestStatus.InProgress, "SIAULIAI11RE_PRANAS_1", "f_siauliai_11_re", L("Join Priest Pranas"));
		SetPhase(QuestStatus.Success, "SIAULIAI11RE_PRANAS_1", "f_siauliai_11_re", L("Join Priest Pranas"));

		AddPrerequisite(new QuestStatusPrerequisite(60113, QuestStatus.Completed));

		AddObjective("joinPranas", L("Join Priest Pranas"), new ManualObjective());
	}
}

// 60145: Everything Intact (3)
//-----------------------------------------------------------------------------
public class OrshaMq3_01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(60145);
		SetName(L("Everything Intact (3)"));
		SetDescription(L("Inesa Hamondale has asked you to return to Bishop Urbonas. Go see Bishop Urbonas and hear about the contents of the Demon Orders."));
		SetType(QuestType.Main);
		SetLocation("c_orsha");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "C_ORSHA_HAMONDAIL", "c_orsha", L("Talk with Inesa Hamondale"));
		SetPhase(QuestStatus.InProgress, "C_ORSHA_URBONAS", "c_orsha", L("Talk to Bishop Urbonas"));
		SetPhase(QuestStatus.Success, "C_ORSHA_URBONAS", "c_orsha", L("Talk to Bishop Urbonas"));

		AddPrerequisite(new QuestStatusPrerequisite(60141, QuestStatus.Completed));

		AddObjective("meetUrbonas", L("Talk to Bishop Urbonas"), new ManualObjective());
	}
}
