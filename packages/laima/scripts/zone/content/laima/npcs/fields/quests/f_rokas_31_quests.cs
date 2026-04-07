//--- Melia Script ----------------------------------------------------------
// Zachariel Crossroads - Field Content
//--- Description -----------------------------------------------------------
// Entrance to the Royal Mausoleum dungeon.
// Quests: Fedimian/3004 "Royal Mausoleum Expedition"
//         f_rokas_31/1001 "Crossroads Warning"
//         f_rokas_31/1002 "Guardian of Zemyna"
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class ZacharielCrossroadsScript : GeneralScript
{
	protected override void Load()
	{
		// Royal Mausoleum Guard - Dungeon Entrance
		//-------------------------------------------------------------------------
		AddNpc(20125, L("[Royal Guard] Balthor"), "f_rokas_31", -1200, 661, 45, async dialog =>
		{
			var character = dialog.Player;
			var mausoleumQuestId = new QuestId("Fedimian", 3004);

			dialog.SetTitle(L("Balthor"));

			// Check if player has the Great King's Key
			var hasKey = character.Inventory.HasItem(650244);

			if (hasKey)
			{
				if (await dialog.YesNo(L("The Royal Mausoleum is open to you. The monsters within are dangerous, but you bear the authority to enter. Do you wish to proceed?")))
				{
					await dialog.Msg(L("{#666666}*The guard steps aside*{/}"));
					await dialog.Msg(L("Be careful in there. Many have entered the mausoleum - not all have returned."));

					// Warp to Royal Mausoleum 1F
					character.Warp("d_zachariel_32", 32, 261, -2294);
				}
				else
				{
					await dialog.Msg(L("Very well. The mausoleum will be here when you're ready."));
				}
			}
			else
			{
				await dialog.Msg(L("Halt! The Royal Mausoleum beyond this point is sealed by order of the Grand Archive."));
				await dialog.Msg(L("{#666666}*He grips his weapon firmly*{/}"));
				await dialog.Msg(L("The monsters inside are extremely dangerous - golems, demons, and worse. Only those with official permission may enter."));
				await dialog.Msg(L("If you wish to explore the mausoleum, you must obtain the Great King's Key from Grand Archivist Elwen in Fedimian."));
			}
		});

		// Quest 1001: Crossroads Warning - Traveling Merchant
		//-------------------------------------------------------------------------
		AddNpc(20165, L("[Traveling Merchant] Darius"), "f_rokas_31", 692, -1072, 44, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_31", 1001);

			dialog.SetTitle(L("Darius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Ah, greetings traveler! I've been waiting here for hours, but I'm hesitant to continue my journey."));
				await dialog.Msg(L("{#666666}*He glances nervously toward the mausoleum entrance*{/}"));
				await dialog.Msg(L("The Royal Mausoleum lies just beyond this crossroads, and the monsters in this area have become increasingly aggressive."));
				await dialog.Msg(L("I need to warn the merchants in Fedimian about the dangers here, but I can't abandon my wares to make the journey myself."));

				var response = await dialog.Select(L("What can I do?"),
					Option(L("I'll help you"), "help"),
					Option(L("What kind of monsters?"), "info"),
					Option(L("Good luck with that"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("You will? Excellent! But first, I need you to thin out the monster population here."));
						await dialog.Msg(L("The Warleader Hogmas are the most dangerous - they've been attacking travelers regularly."));

						if (await dialog.YesNo(L("Will you deal with some of these creatures, then carry my warning to the Fedimian Merchant Guild?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! Clear out at least 12 of those Warleader Hogmas to make the crossroads safer."));
							await dialog.Msg(L("Then head to Fedimian and find Anne at the Merchant Guild. Tell her that the Zachariel Crossroads route is becoming too dangerous for regular caravans."));
							await dialog.Msg(L("Return to me once you've delivered the message. I'll make it worth your while."));
						}
						break;

					case "info":
						await dialog.Msg(L("The Warleader Hogmas are the worst - large, brutish warriors wielding massive weapons."));
						await dialog.Msg(L("They've set up camps throughout the crossroads and attack anyone who passes. Several caravans have already been forced to turn back."));
						await dialog.Msg(L("If this continues, the trade route to the mausoleum will be completely cut off."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*He sighs heavily*{/}"));
						await dialog.Msg(L("I suppose I'll just have to wait for someone willing to help..."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;

				// Check kill objective progress
				var hogmasKilled = false;
				if (quest.TryGetProgress("killHogmas", out var killObj))
				{
					hogmasKilled = killObj.Done;
				}

				// Check Fedimian delivery objective
				var messageDelivered = false;
				if (quest.TryGetProgress("deliverWarning", out var deliveryObj))
				{
					messageDelivered = deliveryObj.Done;
				}

				if (hogmasKilled && messageDelivered)
				{
					await dialog.Msg(L("You're back! And from the look of things, you've made the crossroads much safer."));
					await dialog.Msg(L("Did Anne at the Merchant Guild receive my warning?"));
					await dialog.Msg(L("{#666666}*He nods with relief*{/}"));
					await dialog.Msg(L("Good, good. The guild needs to know the dangers out here. Perhaps they'll send guards to protect future caravans."));
					await dialog.Msg(L("You've done excellent work. Here - take this weapon I've been carrying. A warrior like you will put it to better use than I ever could."));

					character.Quests.Complete(questId);
				}
				else if (!hogmasKilled)
				{
					await dialog.Msg(L("The crossroads are still too dangerous. Please, defeat at least 12 Warleader Hogmas before heading to Fedimian."));
				}
				else if (!messageDelivered)
				{
					await dialog.Msg(L("Have you delivered my warning to Anne at the Merchant Guild in Fedimian yet?"));
					await dialog.Msg(L("She should be at the guild hall. Tell her about the dangers at the Zachariel Crossroads."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thank you again for your help! The crossroads feel safer already."));
				await dialog.Msg(L("I can finally resume my trade route without constant fear of ambush."));
			}
		});

		// Quest 1002: Guardian of Zemyna - Devotee of Zemyna
		//-------------------------------------------------------------------------
		AddNpc(147491, L("[Devotee] Lyanna"), "f_rokas_31", 47, -320, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_rokas_31", 1002);

			dialog.SetTitle(L("Lyanna"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A devoted woman kneels in prayer before the eastern path*{/}"));
				await dialog.Msg(L("Oh! Forgive me, I didn't notice your approach. I'm watching over the Statue of Goddess Zemyna to the east."));
				await dialog.Msg(L("{#666666}*She looks troubled*{/}"));
				await dialog.Msg(L("The statue stands as a beacon of Zemyna's protection in these desert lands, but corruption has begun to seep into this area."));
				await dialog.Msg(L("The Repusbunny Mages that dwell here were once peaceful creatures, blessed by Zemyna's natural magic. But lately, they've become aggressive and tainted."));

				var response = await dialog.Select(L("How can I help?"),
					Option(L("I'll protect the statue"), "help"),
					Option(L("What happened to them?"), "info"),
					Option(L("That's unfortunate"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*Her eyes light up with hope*{/}"));
						await dialog.Msg(L("Truly? Oh, thank you! I've been trying to contain the corruption myself, but I lack the strength."));

						if (await dialog.YesNo(L("Will you cull the corrupted Repusbunny Mages? If we reduce their numbers, perhaps the corruption will spread more slowly.")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Bless you! Please, defeat 15 of the Repusbunny Mages. It pains me to ask this of such creatures, but they are beyond saving now."));
							await dialog.Msg(L("The Statue of Goddess Zemyna watches over all who travel these crossroads. We must not let it fall to darkness."));
							await dialog.Msg(L("Return to me when the task is done. May Zemyna guide your path."));
						}
						break;

					case "info":
						await dialog.Msg(L("I'm not certain. Perhaps it's lingering demonic corruption from the war, or something darker stirring near the Royal Mausoleum."));
						await dialog.Msg(L("The Repusbunny Mages draw their power from natural magic - the same magic that flows through Zemyna's statue."));
						await dialog.Msg(L("If the corruption spreads to them, it could eventually taint the statue itself. And if that happens..."));
						await dialog.Msg(L("{#666666}*She shudders*{/}"));
						await dialog.Msg(L("These crossroads would become truly cursed."));
						break;

					case "leave":
						await dialog.Msg(L("{#666666}*She returns to her prayers*{/}"));
						await dialog.Msg(L("I understand. Not everyone feels the call to protect sacred places."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("You've returned! I can feel it - the corruption's grip has weakened."));
					await dialog.Msg(L("{#666666}*She closes her eyes, sensing the spiritual energies*{/}"));
					await dialog.Msg(L("Yes... the natural balance is beginning to restore itself. The statue is safe, for now."));
					await dialog.Msg(L("You've done more than simply slay monsters - you've protected a sacred site of Goddess Zemyna. She will remember your service."));
					await dialog.Msg(L("Please, take this staff. It belonged to a fellow devotee who fell protecting this very crossroads. Use it to continue spreading Zemyna's protection."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The Repusbunny Mages still threaten the statue. Please, defeat at least 15 of them to stem the corruption."));
					await dialog.Msg(L("I will continue my prayers here. May Zemyna watch over you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Thank you for protecting the Statue of Goddess Zemyna. Your actions have preserved a sacred beacon in these crossroads."));
				await dialog.Msg(L("The natural balance feels stronger now. May Zemyna's blessings follow you on your journey."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Crossroads Warning
//-----------------------------------------------------------------------------

public class CrossroadsWarningQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_31", 1001);
		SetName(L("Crossroads Warning"));
		SetType(QuestType.Sub);
		SetDescription(L("Defeat the Warleader Hogmas threatening the crossroads, then deliver Darius's warning to the Fedimian Merchant Guild."));
		SetLocation("f_rokas_31", "c_fedimian");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Traveling Merchant] Darius"), "f_rokas_31");

		// Objectives
		AddObjective("killHogmas", L("Defeat Warleader Hogmas at Zachariel Crossroads"),
			new KillObjective(12, new[] { MonsterId.Warleader_Hogma }));

		AddObjective("deliverWarning", L("Deliver warning to Anne at the Merchant Guild in Fedimian"),
			new ManualObjective());

		// Rewards
		AddReward(new ExpReward(3000, 2000));
		AddReward(new SilverReward(10000));
		AddReward(new ItemReward(122114, 1)); // Dio Two-handed Sword
		AddReward(new ItemReward(640002, 10)); // Small HP potions
		AddReward(new ItemReward(640005, 15)); // Small SP potions
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Cards
	}
}

// Quest 1002: Guardian of Zemyna
//-----------------------------------------------------------------------------

public class GuardianOfZemynaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_rokas_31", 1002);
		SetName(L("Guardian of Zemyna"));
		SetType(QuestType.Sub);
		SetDescription(L("Protect the Statue of Goddess Zemyna by culling the corrupted Repusbunny Mages."));
		SetLocation("f_rokas_31");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Devotee] Lyanna"), "f_rokas_31");

		// Objectives
		AddObjective("killMages", L("Defeat corrupted Repusbunny Mages"),
			new KillObjective(45, new[] { MonsterId.Repusbunny_Mage }));

		// Rewards
		AddReward(new ExpReward(4000, 3000));
		AddReward(new SilverReward(12000));
		AddReward(new ItemReward(142105, 1)); // Magic Rod
		AddReward(new ItemReward(640002, 15)); // Small HP potions
		AddReward(new ItemReward(640005, 20)); // Small SP potions
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Cards
	}
}
