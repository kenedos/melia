//--- Melia Script ----------------------------------------------------------
// Stogas Plateau - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_28_2 map. The supply post at the
// bottom of the shelf, sending three parties a week and landing one.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FTableland282QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Stogas Issue
		// =====================================================================
		// Mage Horace - ration pouches on the wrong shoulders
		//---------------------------------------------------------------------
		AddNpc(155034, L("[Mage] Horace"), "f_tableland_28_2", 54, -435, 279, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1001);

			dialog.SetTitle(L("Horace"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A mage turns a stitched canvas pouch over in both hands, holding it up to the light at the seam, absolutely delighted about it*{/}"));
				await dialog.Msg(L("Ah, EXCELLENT, fresh eyes! Come here, come look at this, tell me what you see — no, don't tell me what I've already told myself, look at it FIRST."));
				await dialog.Msg(L("A Blue Siaulav Mage was carrying this. It's a Stogas ration pouch — our stitch, our canvas, our issue stamp under the flap! On the shoulder of a creature that has never queued for a single thing in its miserable existence! Isn't that just *fascinating*? Kill 25 of the Blue Siaulav and bring me 8 more pouches off their Mages, and let's find out how deep this goes."));

				var response = await dialog.Select(L("You'll help me chase this down, won't you?"),
					Option(L("I'll bring 8 pouches"), "help"),
					Option(L("Could they just have found it?"), "info"),
					Option(L("It's one pouch"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666050, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take my handknife — cut the strap, don't pull it! If it tears I can't tell you whether it was cut off a man or simply handed over, and that distinction matters enormously."));
						await dialog.Msg(L("The Mages sit behind the plain ones like generals who've never once held a sword. Break the front rank and they won't move — they've never moved, not once, and I intend to find out why."));
						break;

					case "info":
						await dialog.Msg(L("Oh, a found pouch is empty and chewed, obviously. This one is full, dry, and buckled the exact way a quartermaster buckles it — third hole, every time. Found things don't buckle themselves so precisely!"));
						await dialog.Msg(L("Twenty-two years a field mage, and I've learned to be VERY suspicious of the word 'just.' Nothing on this plateau has ever been just anything, in my professional experience."));
						break;

					case "leave":
						await dialog.Msg(L("One pouch, sure — out of three supply parties a week, of which exactly one arrives! I'd rather like to know where the other two go, and this is the first scrap of evidence anyone's managed to hold onto."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSiaulav", out var killObj)) return;
				if (!quest.TryGetProgress("collectPouches", out var pouchObj)) return;

				if (killObj.Done && pouchObj.Done)
				{
					await dialog.Msg(L("{#666666}*He buckles all 8 flaps shut in a row, then unbuckles them again just to check the hole each one sits on, humming with excitement*{/}"));
					await dialog.Msg(L("Third hole. Every single one! A creature does not learn a quartermaster's buckle, my friend — it takes the pouch as it comes and never once touches the strap. This is *wonderful* data."));
					await dialog.Msg(L("Which means these weren't stolen at all. They were carried — still buckled — from wherever the party set them down. Take this, and please, PLEASE don't repeat that sentence where Ades can hear it until I've finished thinking it through properly."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Front rank's gone, good! Eight pouches off the Mages behind it now — cut at the strap, never pulled, remember!"));
				}
				else
				{
					await dialog.Msg(L("Twenty-five of the plain Blue Siaulav first. The Mages won't come forward while their front rank still stands — cowards, or something smarter than cowards."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Eight pouches on a board, buckle holes marked underneath each one — a proper little exhibit! Ades looked at it twice and said nothing both times, which from Ades means he understood every word of it perfectly."));
			}
		});

		// =====================================================================
		// QUEST 1002: Consecration Without Ground
		// =====================================================================
		// Crusader Genute - a detachment sent to bless what it cannot reach
		//---------------------------------------------------------------------
		AddNpc(150217, L("[Crusader] Genute"), "f_tableland_28_2", -783, 319, 229, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1002);

			dialog.SetTitle(L("Genute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A crusader is grinding something in a field mortar with the lid of her own canteen, muttering a prayer under each turn of the wrist*{/}"));
				await dialog.Msg(L("Careful where you step — Gabija's grace, I've got the last of it laid out to dry! Pilgrim or soldier, saint or sinner, doesn't matter to me. You're here, and the Goddess sends who She sends."));
				await dialog.Msg(L("The Church sent two of us to this forsaken shelf seven weeks ago to consecrate the Vedas ground. Seven weeks, and we haven't been allowed past Mesafasla once — not once! So we make what oil we can and we wait on Her patience, which is rather more generous than mine. Bring me 6 spotted mushrooms off the Blue Lapasapes."));

				var response = await dialog.Select(L("Will the Goddess's work wait on you, traveler?"),
					Option(L("I'll bring 6 mushrooms"), "help"),
					Option(L("Not allowed past by whom?"), "info"),
					Option(L("Go anyway"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They grow ON the Lapasapes, not near them — Gods help you, under the shoulder plates, in the wet. Unpleasant work, but the only source on this whole miserable shelf."));
						await dialog.Msg(L("Take them off single ones. A Blue Lapasape will cross two hundred paces of open ground to reach a fight it can see, and it does it quietly — may the Goddess grant you sharper ears than they have manners."));
						break;

					case "info":
						await dialog.Msg(L("Ha! By whom. The order reads: the plateau action concluded, so there is no ground requiring consecration, so there is no reason for a Church detachment to proceed. Four clauses, every one of them technically true, and the whole thing a wall regardless."));
						await dialog.Msg(L("I have read it sixty times searching for the seam the Goddess surely left in it. Sixty times! A blessed lot of good that's done me."));
						break;

					case "leave":
						await dialog.Msg(L("If I march up there without the order, the Church becomes a body that defies the Kingdom's word — and the next detachment gets stopped at Roxona instead of here. Alvydas and I have fought that same argument seven weeks running, Gods witness us both."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectMushrooms", out var mushObj)) return;

				if (mushObj.Done)
				{
					await dialog.Msg(L("{#666666}*She works all 6 into the mortar, and the smell that rises is clean and very cold — she breathes it in like incense*{/}"));
					await dialog.Msg(L("Grave oil! Praise be. Enough for 41 stones with a little left over. It keeps two years in a stoppered jar, and I fully intend to be standing on that ground long before it turns — the Goddess and I have an understanding."));
					await dialog.Msg(L("Take the detachment's field allowance. Two of us drawing for twelve, and we haven't moved four miles in seven weeks — it isn't our money, not really, and I'd rather it went to someone actually doing the Goddess's work."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("6 mushrooms, off the Blue Lapasapes, under the shoulder plates. Single ones only, and quickly, if the Goddess wills it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Forty-one measures poured, stoppered, and set on a shelf where I have to look at them every blessed day. Some days that's a promise. Some days it's just furniture I paid too much attention to. Today — today it's a promise."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Shore Rank
		// =====================================================================
		// Crusader Alvydas - the archers on the Mesafasla road
		//---------------------------------------------------------------------
		AddNpc(150218, L("[Crusader] Alvydas"), "f_tableland_28_2", -697, 422, 71, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1003);

			dialog.SetTitle(L("Alvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A crusader stands with his shield grounded, watching the north road, not the camp*{/}"));
				await dialog.Msg(L("Not turning around. Lose the habit of watching that road, I lose the road. You walked up clean, though. Good."));
				await dialog.Msg(L("Skip the sermon, I'll give you the useful part instead. Blue Siaulav Archers took the first mile of the Mesafasla road. Shoot at anything that moves on it. That's why Ades's parties leave in the dark. Kill 20 of them."));

				var response = await dialog.Select(L("Will you clear the first mile?"),
					Option(L("I'll clear the road"), "help"),
					Option(L("Why does that matter to the Church?"), "info"),
					Option(L("Let the army do it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They hold a rank. Don't punch the middle of it. Take an end and roll it — same as you'd break a line of men."));
						await dialog.Msg(L("Won't break either way. Watched three parties go through them. Not one Archer ran. Whatever's running this plateau, it's running them too."));
						break;

					case "info":
						await dialog.Msg(L("Because Genute's right, and I'm tired of being right in the opposite direction. Can't walk up that road ourselves — next best thing is somebody can."));
						await dialog.Msg(L("Nine years a soldier before I was a crusader. Road's a road. Blessing it does nothing. Clearing it does everything. That's the whole of my theology."));
						break;

					case "leave":
						await dialog.Msg(L("Army here's Ades and fourteen people, all hauling loads at three in the morning so they don't get shot at. That IS the army doing it. That's the best they've got."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the first mile and back with his shield still on his arm and mud to the knee*{/}"));
					await dialog.Msg(L("Clear. Next party goes at noon, in daylight, first time since we got here. Means somebody'll actually see what happens to it, one way or the other."));
					await dialog.Msg(L("Take this. Mine, not the detachment's. Been carrying it since the last time I was any use to anybody, and it's gotten heavier every week I didn't."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("First mile of the north road. Take an end of the rank, roll it. 20 of them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Noon party went out, came back. Genute watched the whole road from the ridge, told me after that she hadn't thought about the order once the whole time. First good hour either of us has had up here."));
			}
		});

		// =====================================================================
		// QUEST 1004: Where the Loads Went
		// =====================================================================
		// Supply Clerk Vainora - four caches, none of them broken open
		//---------------------------------------------------------------------
		AddNpc(20143, L("[Supply Clerk] Vainora"), "f_tableland_28_2", 1432, 1220, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1004);

			dialog.SetTitle(L("Vainora"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A supply clerk compares two tally boards side by side, gets the same answer she's gotten for eight weeks, and does not look surprised*{/}"));
				await dialog.Msg(L("You're on neither of my boards. For once, that's a point in your favor. I need someone who isn't already part of the problem I'm about to describe to you very precisely."));
				await dialog.Msg(L("Three parties out a week. One arrives. I have signed twenty-four loads out of this post and Mesafasla has receipted eight. Riders found four of the missing ones sitting in the open, and — note this — every single one was stacked. And shut. Go look at all four. I want it in your own words, not mine."));

				var response = await dialog.Select(L("Will you walk out to the 4 caches?"),
					Option(L("I'll look at all 4"), "help"),
					Option(L("Stacked and shut?"), "info"),
					Option(L("The parties are selling them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Count what's in each one against the manifest, and take a sample out. If a load is short, I need the exact number. If it is not short — I need to know that even more, and I want it written down properly."));
						await dialog.Msg(L("They're a long way apart. No road between any of them. Take water, and don't skip the counting to save time — I will know."));
						break;

					case "info":
						await dialog.Msg(L("Stacked. Heavy at the bottom, light on top, canvas over, cords done — precisely the way a party stacks a load when it's putting one down for the night."));
						await dialog.Msg(L("Except no party put these down. The parties are gone. Something else stacked them, and it stacked them correctly, which I find far more unsettling than if it hadn't bothered."));
						break;

					case "leave":
						await dialog.Msg(L("A tidy theory. If it were true, the goods would surface in Roxona market, and I have had a friend there watching for our issue stamp for six weeks running. Not one item. Nothing off this shelf is being sold anywhere, which I have also written down."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var checkedLoads = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_2.Quest1004.Checked", 0);

				if (checkedLoads >= 4)
				{
					await dialog.Msg(L("{#666666}*She writes the four counts under each other, draws a line beneath the column, and deliberately does not write a total*{/}"));
					await dialog.Msg(L("Complete. All four. Not one item short out of four full loads. Nothing spoiled. The cords done the identical way on every single one — which, I will remind you, is not how four separate thieves would behave."));
					await dialog.Msg(L("Something on this plateau is collecting our supply. It is not eating it. It is not taking it apart. Take the clerk's fee and go say that to Ades — I have wanted to say it to someone for eight weeks and actually be believed."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Four caches, no road between them, count each against the manifest properly. {0} of 4 done so far.", checkedLoads));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Twenty-four out. Eight receipted. Four recovered complete. Twelve unaccounted. I have written 'unaccounted' in the ledger in plain ink, and I have stopped apologizing for the word — it is, after all, simply accurate."));
			}
		});

		// =====================================================================
		// STACKED CACHES
		// =====================================================================
		// For Quest 1004 - Where the Loads Went
		// =====================================================================

		void AddStackedCache(int cacheNumber, string cacheName, string count, int x, int z, int direction)
		{
			AddNpc(153105, L(cacheName), "f_tableland_28_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A supply load stacked heavy-to-light under a drawn canvas, the cords done properly*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_2.Quest1004.Cache{cacheNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have counted this one. The canvas is back over it and the cords are done the way you found them*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Counting the load..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Count interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);
				character.Inventory.Add(666047, 1, InventoryAddType.PickUp);

				var checkedLoads = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_2.Quest1004.Checked", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_28_2.Quest1004.Checked", checkedLoads);

				character.ServerMessage(L(count));
				character.ServerMessage(LF("Caches counted: {0}/4", checkedLoads));

				if (checkedLoads >= 4)
					character.ServerMessage(L("{#FFD700}All 4 caches counted. Return to Supply Clerk Vainora.{/}"));
			});
		}

		AddStackedCache(1, "Stacked Load - Ninth Party", "Complete against manifest. Canvas drawn, cords done, nothing damp.", 1303, 984, 0);
		AddStackedCache(2, "Stacked Load - Eleventh Party", "Complete. Heavy at the bottom, light on top, the way a carter does it.", -551, -692, 0);
		AddStackedCache(3, "Stacked Load - Fourteenth Party", "Complete. Two sacks re-tied. Re-tied, not torn.", -1013, 49, 0);
		AddStackedCache(4, "Stacked Load - Nineteenth Party", "Complete, and set square to the slope so it will not roll. Somebody chose this ground.", -201, 1767, 0);

		// =====================================================================
		// The Cold Device - atmosphere on the west shelf
		//---------------------------------------------------------------------
		AddNpc(150229, L("Frost-Bound Device"), "f_tableland_28_2", -1249, 1163, 0, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A squat device standing alone on the west shelf with a rime of frost holding on it in full sun*{/}"));
			await dialog.Msg(L("{#666666}*The ground for 4 paces around it is hard and nothing has walked across it. Whatever it was set here to do, it is still doing it*{/}"));
		});

		// =====================================================================
		// QUEST 1005: What Is Provisioning
		// =====================================================================
		// Squad Leader Ades - the Sparnashorn and eight weeks of supply
		//---------------------------------------------------------------------
		AddNpc(20141, L("[Squad Leader] Ades"), "f_tableland_28_2", 211, -487, 316, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_2", 1005);

			dialog.SetTitle(L("Ades"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_28_2", 1004)))
				{
					await dialog.Msg(L("{#666666}*He glances up from the roster, then straight back down*{/}"));
					await dialog.Msg(L("Not now. Vainora's got four caches and a question, and I don't act on eight weeks of gut feeling — get her the count first. Then find me."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's already on his feet when you walk up, like he's been rehearsing this for someone, anyone, to finally say it to*{/}"));
				await dialog.Msg(L("You're back. Good. Sit, don't sit, doesn't matter — this has been building for weeks and I need to say it out loud before I lose my nerve on it."));
				await dialog.Msg(L("Horace has eight buckled pouches. Vainora has four complete loads. I have twenty-four signed out and eight signed in. Put that together and there's exactly one answer that fits: something is provisioning."));
				await dialog.Msg(L("The Blue Lapasapes carry, and the Sparnashorn is what they carry TO. A whole colony feeding one thing, and what they're feeding it is my post's supply. Kill 20 Lapasapes, open the nest ground, and take the Sparnashorn down. That's an order as much as a request."));

				var response = await dialog.Select(L("Will you go into the nest ground?"),
					Option(L("I'll take the Sparnashorn"), "help"),
					Option(L("A colony that provisions?"), "info"),
					Option(L("Stop sending parties"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666045, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Carry the message. Eight weeks of returns, written plain, for Gomen at Mesafasla — and if you take that nest, it's the first true thing anyone's managed to send up that road."));
						await dialog.Msg(L("Lapasapes first. They cross open ground dead quiet and they will be behind you before you've decided they're even coming. Watch your back."));
						break;

					case "info":
						await dialog.Msg(L("Ants provision. Bees provision. Neither of them stacks a load heavy-to-light and re-ties a proper cord. I've got no explanation for that part, and I'm not going to invent one just to sleep better."));
						await dialog.Msg(L("What I will say is: this shelf lost a company, and eight weeks later something on it started behaving like a commissariat. I don't believe those two facts are strangers to each other."));
						break;

					case "leave":
						await dialog.Msg(L("Then Mesafasla's nine people and eight receipted loads walk into a winter short-handed. Rimgaile's already lining coats out of Lepusbunny pelts. No. Not while I'm standing here."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openNest", out var nestObj)) return;
				if (!quest.TryGetProgress("killSparnashorn", out var bossObj)) return;

				if (nestObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("{#666666}*He takes the report standing, and asks for the count of recovered loads three separate times before he'll believe it*{/}"));
					await dialog.Msg(L("Twelve loads. Stacked in courses, canvas over, inside a NEST. Eight weeks of this post's supply, put away by something that never ate a single grain of it."));
					await dialog.Msg(L("Take this — came up with me from Roxona, done nothing but sit in a chest since. And carry the message to Gomen. Tell him the road's open from this end. Tell him Stogas is still standing."));

					character.Quests.Complete(questId);
				}
				else if (nestObj.Done)
				{
					await dialog.Msg(L("Nest ground's open. The Sparnashorn hasn't left, and it's not going to — it's sitting on twelve loads and it knows it."));
				}
				else
				{
					await dialog.Msg(L("Twenty Lapasapes first. I am not sending you into that nest with carriers still crawling the ground behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Twelve loads recovered. Three parties a week, arriving three times a week — first time that sentence has ever been true. Vainora ruled a new column, headed it RECEIPTED, and has filled it in every single day since without saying one word about it."));
				await dialog.Msg(L("Gomen sent a rider back. First in eight weeks. Says the ground between here and Vedas is held. Signed. And underneath the signature, in his own hand — he wrote that he read it before he signed it."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Stogas Issue
//-----------------------------------------------------------------------------

public class StogasIssueQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1001);
		SetName(L("Stogas Issue"));
		SetType(QuestType.Sub);
		SetDescription(L("A Blue Siaulav Mage was carrying a full Stogas ration pouch, buckled on the third hole the way a quartermaster buckles it. Horace wants 8 more before he says out loud what that means."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Mage] Horace"), "f_tableland_28_2");

		AddObjective("killSiaulav", L("Kill Blue Siaulav holding the front rank"),
			new KillObjective(25, new[] { MonsterId.Siaulav_Blue }));

		AddObjective("collectPouches", L("Cut ration pouches off Blue Siaulav Mages"),
			new CollectItemObjective(666046, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(666046, 0.35f, MonsterId.Siaulav_Mage_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666046, character.Inventory.CountItem(666046), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666050, character.Inventory.CountItem(666050), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666046, character.Inventory.CountItem(666046), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666050, character.Inventory.CountItem(666050), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Consecration Without Ground
//-----------------------------------------------------------------------------

public class ConsecrationWithoutGroundQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1002);
		SetName(L("Consecration Without Ground"));
		SetType(QuestType.Sub);
		SetDescription(L("Genute was sent up the shelf 7 weeks ago to consecrate ground the Kingdom says does not require it. She can still make the oil. The Blue Lapasapes carry the mushrooms it is ground from."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crusader] Genute"), "f_tableland_28_2");

		AddObjective("collectMushrooms", L("Take spotted mushrooms off Blue Lapasapes"),
			new CollectItemObjective(666049, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(666049, 0.35f, MonsterId.Lapasape_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666049, character.Inventory.CountItem(666049), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666049, character.Inventory.CountItem(666049), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Shore Rank
//-----------------------------------------------------------------------------

public class TheShoreRankQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1003);
		SetName(L("The Shore Rank"));
		SetType(QuestType.Sub);
		SetDescription(L("The Blue Siaulav Archers hold the first mile of the Mesafasla road in a rank that does not break, which is why Stogas sends its supply parties out in the dark."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crusader] Alvydas"), "f_tableland_28_2");

		AddObjective("killArchers", L("Kill Blue Siaulav Archers on the north road"),
			new KillObjective(20, new[] { MonsterId.Siaulav_Bow_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Where the Loads Went
//-----------------------------------------------------------------------------

public class WhereTheLoadsWentQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1004);
		SetName(L("Where the Loads Went"));
		SetType(QuestType.Sub);
		SetDescription(L("Stogas has signed out 24 loads and Mesafasla has receipted 8. Four of the missing ones are sitting on open ground, stacked and shut. Count all 4 against the manifest."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Supply Clerk] Vainora"), "f_tableland_28_2");

		AddObjective("countCaches", L("Count the 4 stacked caches"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_2.Quest1004.Checked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666047, character.Inventory.CountItem(666047), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1004.Checked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1004.Cache{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666047, character.Inventory.CountItem(666047), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_2.Quest1004.Checked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_2.Quest1004.Cache{i}");
	}
}

// Quest 1005 CLASS: What Is Provisioning
//-----------------------------------------------------------------------------

public class WhatIsProvisioningQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_2", 1005);
		SetName(L("What Is Provisioning"));
		SetType(QuestType.Sub);
		SetDescription(L("Eight buckled pouches, four complete caches and 16 unreceipted loads add up to one answer: the Blue Lapasapes are carrying, and the Sparnashorn is what they carry to."));
		SetLocation("f_tableland_28_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Squad Leader] Ades"), "f_tableland_28_2");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_28_2", 1004));

		AddObjective("openNest", L("Kill Blue Lapasapes to open the nest ground"),
			new KillObjective(20, new[] { MonsterId.Lapasape_Blue }));

		AddObjective("killSparnashorn", L("Take the Blue Sparnashorn off the stacked loads"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Boss_Sparnashorn_Blue, 1),
					new KillSpec(MonsterId.Lapasape_Blue, 3, BuffId.EliteMonsterBuff),
				},
				resetIdent: "openNest",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583123, 1)); // Gift of Demise
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666045, character.Inventory.CountItem(666045), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666045, character.Inventory.CountItem(666045), InventoryItemRemoveMsg.Destroyed);
	}
}
