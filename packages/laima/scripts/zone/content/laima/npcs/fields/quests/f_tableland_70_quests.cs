//--- Melia Script ----------------------------------------------------------
// Ibre Plateau - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_70 map. First night's halt on the
// road out of Roxona, and a stockade broken from the inside.
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

public class FTableland70QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Four Panels and a Herd
		// =====================================================================
		// Drove-Master Kazys - rebuilding the halt
		//---------------------------------------------------------------------
		AddNpc(20156, L("[Drove-Master] Kazys"), "f_tableland_70", 3531, -2613, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1001);

			dialog.SetTitle(L("Kazys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A drove-master sights down a split rail with one eye, hunting for a length he can still use*{/}"));
				await dialog.Msg(L("Don't stand in my light there — hah, look at that, a new face. Well, timing's good, son, I could use a pair of hands that aren't already worn to the bone."));
				await dialog.Msg(L("Ibre's the first night's halt out of Roxona. Walked sixty-one convoys up this shelf in nine years, every last one slept inside that stockade — and now it's four panels down, with Blue Cronewts grazing fat and happy right where it used to stand. Kill 25 of them, bring me 8 lengths of usable timber, and let's fix that."));

				var response = await dialog.Select(L("Will you clear the ground and get me timber?"),
					Option(L("I'll clear them and cut timber"), "help"),
					Option(L("What kind of convoys?"), "info"),
					Option(L("Sleep in the open"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Timber off the Cronewts, mind, not off the ground — they drag rails into their beds, and their beds are the only dry wood left on this whole shelf, wouldn't you know it."));
						await dialog.Msg(L("They graze in a long line and turn together, all at once, like they've got one mind between the sixty of them. Don't get caught inside that turn, son. Trust me on that one."));
						break;

					case "info":
						await dialog.Msg(L("Exiles, most of 'em. Forty at a time, Roxona to Sventimas, four days on foot — Kingdom contracts it out because a drover comes cheaper than a soldier, and cheaper always wins."));
						await dialog.Msg(L("Don't choose who's on the list. Never once asked to. What I do — what I'm GOOD at — is get forty people to Sventimas with forty people still walking at the end of it."));
						break;

					case "leave":
						await dialog.Msg(L("In the open? On Ibre, with Hohen Manes prowling the ridge? Buried two folks on this shelf in nine years, son, and both of 'em slept outside a panel. I'd rather you didn't join that count."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCronewts", out var killObj)) return;
				if (!quest.TryGetProgress("collectTimber", out var woodObj)) return;

				if (killObj.Done && woodObj.Done)
				{
					await dialog.Msg(L("{#666666}*He stands the 8 lengths against each other and kicks the base of the stack to check it settles right*{/}"));
					await dialog.Msg(L("Eight good rails! That's two panels, two panels makes a corner, and a corner's somewhere forty tired people can finally put their backs against something solid."));
					await dialog.Msg(L("Here, the drove purse. Kingdom pays me by the head delivered and I haven't walked a convoy in five weeks, so this money's been doing nothing but sit in a bag reminding me of that. Might as well earn its keep."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Grazing's off the ground now, good. Eight lengths out of the Cronewt beds and I can finally start standing panels again."));
				}
				else
				{
					await dialog.Msg(L("Twenty-five Cronewts first, and mind the turn, son. They graze in a line and that whole line moves as one thing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two panels up, a third half-framed. Nobody's given me a convoy to walk and I'm building this anyway — Egle tells me that's the most Ibre thing she's ever seen a man do. I'll take that as a compliment."));
			}
		});

		// =====================================================================
		// QUEST 1002: Cold Shoes for a Cold Road
		// =====================================================================
		// Farrier Egle - black stone off the Hohen Mages
		//---------------------------------------------------------------------
		AddNpc(152065, L("[Farrier] Egle"), "f_tableland_70", 4424, -2033, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1002);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A farrier works a shoe cold on a stump anvil, because there is no fire anywhere in this camp*{/}"));
				await dialog.Msg(L("Careful of the sparks — ha, there aren't any, that's rather the whole problem. Cold iron doesn't throw sparks, it just throws a fit. You look like you can still walk far, which is more than I can say for these shoes."));
				await dialog.Msg(L("No charcoal on Ibre. No cart up from Roxona in five weeks. So I'm beating iron cold and ruining one shoe in three for the privilege. Blue Hohen Mages carry black stone that burns hotter than coal ever did. Bring me 6 pieces and let's stop this nonsense."));

				var response = await dialog.Select(L("Will you get me the black stone?"),
					Option(L("I'll bring 6 pieces"), "help"),
					Option(L("Why no cart in 5 weeks?"), "info"),
					Option(L("Shoe them in Roxona"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They hold it in the off hand and won't drop it even when they go down — clenched right through death, charming creatures. You'll have to open the hand yourself. Unpleasant. My apologies in advance."));
						await dialog.Msg(L("Don't put more than 2 in a pocket together. They warm each other up, and I'd much rather get six separate deliveries than one very interesting fire."));
						break;

					case "info":
						await dialog.Msg(L("Because the convoy schedule stopped, and the supply cart runs on the convoy schedule, obviously. No convoy, no cart. Nobody in Roxona spared a thought for the six of us who actually live here between convoys."));
						await dialog.Msg(L("Flour for three weeks and no fuel to bake it with. That's the real state of the Ibre halt, and you won't find it written in anybody's tidy little report."));
						break;

					case "leave":
						await dialog.Msg(L("Four days down and back to Roxona, with unshod animals the whole way, on a stone shelf. I'll take my chances prying open a dead Hohen's fist, thanks all the same."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectStone", out var stoneObj)) return;

				if (stoneObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays 2 of the stones in the stump hollow, strikes them, and the hollow lights with no smoke at all — she actually smiles at it*{/}"));
					await dialog.Msg(L("Hot, clean, holds a heat for hours. Look at that. I can shoe every animal in this camp AND bake, for the first time in five weeks. Miracles do happen, apparently, they just come wrapped in dead sorcerers."));
					await dialog.Msg(L("Take the farrier's fee. And go tell Kazys there's bread tonight — he'll pretend he doesn't care, and then he'll eat four of them, watch."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Six pieces, off the Blue Hohen Mages, out of the off hand. No more than two in a pocket, I mean it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Forge's stayed lit six days running on four stones. Shod everything here twice over and started making stockade nails, which isn't remotely farrier's work — but it's the most useful thing my two hands can do right now, so."));
			}
		});

		// =====================================================================
		// QUEST 1003: The Ridge Shamans
		// =====================================================================
		// Trailhand Domas - the Lapasape Shamans on the high line
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Trailhand] Domas"), "f_tableland_70", 2880, -3742, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1003);

			dialog.SetTitle(L("Domas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young trailhand grips a tally stick too tight and keeps snapping his eyes back up to the ridge line mid-sentence*{/}"));
				await dialog.Msg(L("Sorry! Sorry, I— you'll have to forgive me not looking right at you, it's a habit, eyes on the ridge or eyes closed, that's genuinely the only choice up here and I've picked the ridge."));
				await dialog.Msg(L("I walk the flank of a convoy — means I'm out on the edge, right where the Blue Lapasape Shamans are, and there's twenty of them now where there were four in spring, TWENTY, and nobody but me seems to think that's strange! Kill 20 and the flank's walkable again."));

				var response = await dialog.Select(L("Will you clear the ridge?"),
					Option(L("I'll clear the flank"), "help"),
					Option(L("Four to twenty in a season?"), "info"),
					Option(L("Walk the middle instead"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They call the Manes down on you — if you hear the call, MOVE, immediately, don't think about it. The call takes four seconds, the Manes take six, and those two seconds in between are the whole fight, that's it, that's everything."));
						await dialog.Msg(L("Kill the Shaman that called before you touch anything else. That's the only rule I've got and it's kept me alive two years, so, please, just — trust me on this one."));
						break;

					case "info":
						await dialog.Msg(L("They came up off the Mandara side! I've watched them come, in ones and twos, walking, for four months now, and every single one went past me toward Roxona and then just — turned around. And stayed."));
						await dialog.Msg(L("Something's moving off Mandara onto Ibre, I know it is, I've said so three times and been told I'm nineteen. Like that's an answer to anything!"));
						break;

					case "leave":
						await dialog.Msg(L("Then nobody's on the flank at all, and forty people walk this shelf with a ridge looming right over them and not one soul watching it. That's — no, that's a terrible idea, please reconsider."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killShamans", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the ridge line all the way out and all the way back before he'll say a single word*{/}"));
					await dialog.Msg(L("Flank's clear! I walked eighteen hundred paces of it and didn't hear one call — didn't even realize how loud my own breathing was until just now, honestly."));
					await dialog.Msg(L("Take my season's pay, please, I've got nothing to spend it on up here and I'd genuinely like to have paid somebody for something, just once, before I go grey up on this ridge."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Ridge line, north and west. Twenty of them. Kill the one that called first, before anything else, please remember that part!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Still counting them coming up off Mandara — nine more this week, all walking, all turning and staying, just like before. Kazys has finally stopped telling me I'm nineteen and started writing the number down instead. Small victories!"));
			}
		});

		// =====================================================================
		// QUEST 1004: Which Way the Rails Fell
		// =====================================================================
		// Pen-Warden Milda - four panels and the direction of the break
		//---------------------------------------------------------------------
		AddNpc(155035, L("[Pen-Warden] Rugile"), "f_tableland_70", 2702, -2586, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1004);

			dialog.SetTitle(L("Rugile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A pen-warden crouches at the foot of a fallen stockade panel, hand flat on the ground beside the post hole, not moving it a hair*{/}"));
				await dialog.Msg(L("Stay back from the hole. Please. ...There, thank you. I don't get many people crouching down next to me who aren't already halt staff, and I'd like to keep it that way until I'm finished here."));
				await dialog.Msg(L("I keep the halt. Four panels went down five weeks ago, the night of the 19th convoy. Kazys wants to rebuild them — I want someone to look at them first, properly, before a single post gets touched. Go to all 4 and tell me which way the rails are lying. Exactly which way."));

				var response = await dialog.Select(L("Will you look at the 4 panels?"),
					Option(L("I'll look at all 4"), "help"),
					Option(L("Does the direction matter?"), "info"),
					Option(L("Rebuild them and forget it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Look at the post holes as well as the rails — both, every time. A post pushed from outside splinters on the inside face. A post pushed from inside splinters on the outside. Do not confuse the two."));
						await dialog.Msg(L("And do not move anything. Anything. I have kept this ground exactly as it was for five weeks and had three separate people tell me I'm being strange about it. I don't care. It stays exactly as it fell."));
						break;

					case "info":
						await dialog.Msg(L("It is the ONLY thing that matters. If a herd came through the fence, that's simply Ibre, and I write it in the halt book, and we build a stronger fence, and we move on."));
						await dialog.Msg(L("But if it did not — then forty people were inside that pen the night it came down, and the halt book already says that convoy was delivered safely to Sventimas. Do you see why I need this exact?"));
						break;

					case "leave":
						await dialog.Msg(L("No. Absolutely not. I signed those forty through this halt with my own hand. I am the last person who wrote their number down, and I will not have anyone build over the place I did it. Not until I know."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var looked = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_70.Quest1004.Looked", 0);

				if (looked >= 4)
				{
					await dialog.Msg(L("{#666666}*She goes to each of the 4 post holes herself, fingers into the splintering, and stays at the fourth one for a long, long time*{/}"));
					await dialog.Msg(L("Outward. All four panels. Every single post splintered on the outside face — I checked each one three times to be certain. Nothing came in through that fence. Nothing."));
					await dialog.Msg(L("Something inside the pen went out through it, through four panels at once, and forty people were inside when it happened. Take this and find Vaidotas at the north camp. He was the drover on the 19th, and he has not spoken to a single person in five weeks."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Four panels. Look at the post holes AND the rails, both, every time. {0} of 4 looked at so far. Do not move anything.", looked));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written OUTWARD in the halt book under the 19th convoy, in my own hand, and signed it. If someone in Roxona wishes to argue with that, they are welcome to come up here and put their own fingers in the wood. I checked three times."));
			}
		});

		// =====================================================================
		// STOCKADE PANELS
		// =====================================================================
		// For Quest 1004 - Which Way the Rails Fell
		// =====================================================================

		void AddStockadePanel(int panelNumber, string panelName, string finding, int x, int z, int direction)
		{
			AddNpc(153158, L(panelName), "f_tableland_70", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_70", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A stockade panel lying flat with its posts still in the ground and snapped off at the collar*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_70.Quest1004.Panel{panelNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have read this panel. Nothing on it has been moved*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the break..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Reading interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var looked = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_70.Quest1004.Looked", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_70.Quest1004.Looked", looked);

				character.ServerMessage(L(finding));
				character.ServerMessage(LF("Panels read: {0}/4", looked));

				if (looked >= 4)
					character.ServerMessage(L("{#FFD700}All 4 panels read. Return to Pen-Warden Rugile.{/}"));
			});
		}

		AddStockadePanel(1, "North Panel", "Rails lying away from the pen. Posts splintered on the outside face.", 2697, -2653, 231);
		AddStockadePanel(2, "West Panel", "Same. Away from the pen, every post, the whole panel flat in one piece.", 2585, -2554, 211);
		AddStockadePanel(3, "South Panel", "Away. The collar pins are still seated - it was not levered, it was pushed.", 2583, -2617, 234);
		AddStockadePanel(4, "East Panel", "Away, like the other 3. Four panels went out at once and nothing came in.", 2616, -2640, 237);

		// =====================================================================
		// QUEST 1005: The Nineteenth Convoy
		// =====================================================================
		// Drover Vaidotas - the man who signed 40 out and 40 in
		//---------------------------------------------------------------------
		AddNpc(147486, L("[Drover] Vaidotas"), "f_tableland_70", 3179, -2332, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_70", 1005);

			dialog.SetTitle(L("Vaidotas"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_70", 1004)))
				{
					await dialog.Msg(L("{#666666}*He does not look up*{/}"));
					await dialog.Msg(L("Whoever you are. Save it. Rugile sent you — go read her panels first. I don't say this twice. Won't say it to someone who doesn't already know which way they fell."));
					return;
				}

				await dialog.Msg(L("{#666666}*He finally looks up. For a long moment he just studies your face, flat, like he's checking whether you already know*{/}"));
				await dialog.Msg(L("Outward. Yes."));
				await dialog.Msg(L("I've known that five weeks. Haven't found a way to make it mean anything I can say out loud. Until now, I suppose."));
				await dialog.Msg(L("Signed forty into that pen. Morning came, forty sets of tracks going north, no bodies, no blood, fence lying flat. Walked the empty pen to Sventimas myself. Signed forty in at the other end. Nobody there counted them either. Kill 25 Blue Hohen Mages. Take the pair on the ridge that's watched this halt ever since. That's all I have left to ask anyone."));

				var response = await dialog.Select(L("Will you take the pair on the ridge?"),
					Option(L("I'll take the pair"), "help"),
					Option(L("You signed 40 in at Sventimas?"), "info"),
					Option(L("Report it in Roxona"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663121, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the charge. Egle made it from the black stone, breaks a ridge line open — throw it low, behind them. A Mane runs toward noise before it thinks. Doesn't think much after, either."));
						await dialog.Msg(L("Mages first. The pair are Manes. Won't come off that ridge while anything's still standing up there to warn them."));
						break;

					case "info":
						await dialog.Msg(L("Yes. Receiving clerk at Sventimas has never counted a convoy in the nine years I've walked them. Signs the number on the sheet. I wrote forty. He signed forty."));
						await dialog.Msg(L("Thought about that every night for five weeks now. Nobody counts at either end — a convoy's forty people, or it's a piece of paper. This road's been running on the paper the whole time."));
						break;

					case "leave":
						await dialog.Msg(L("With what. I've got a signed sheet says I delivered them. Only evidence I didn't is four panels of broken fence and a warden nobody in Roxona's ever heard of."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearRidge", out var ridgeObj)) return;
				if (!quest.TryGetProgress("takeThePair", out var pairObj)) return;

				if (ridgeObj.Done && pairObj.Done)
				{
					await dialog.Msg(L("{#666666}*He climbs the ridge himself for the first time in five weeks and comes back down carrying something small in both hands*{/}"));
					await dialog.Msg(L("Boots. Six pairs. Set side by side under an overhang, laces tied, all facing the same way. Nobody walks north off this shelf out of their own boots."));
					await dialog.Msg(L("{#666666}*His voice catches, just once, before he steadies it*{/}"));
					await dialog.Msg(L("Take this — drove-master's badge, from before Kazys. Been in my pack nine years. I'm walking to Sventimas. I'm going to count somebody's convoy in myself, and if that clerk signs a number he hasn't counted, I will stand there until he does."));

					character.Quests.Complete(questId);
				}
				else if (ridgeObj.Done)
				{
					await dialog.Msg(L("Ridge's thin. The pair's still up there. Haven't moved off this halt in five weeks. That's the part I keep coming back to."));
				}
				else
				{
					await dialog.Msg(L("Twenty-five Mages first. Nothing comes off that ridge while something's still up there to call."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Six pairs of boots on a shelf in the halt house, a card under each saying where they were found. It's not forty people. It's not nothing, either."));
				await dialog.Msg(L("Rugile's got the halt book open at the 19th, boots written in. Whoever walks this shelf next is going to have to read it. Good."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Four Panels and a Herd
//-----------------------------------------------------------------------------

public class FourPanelsAndAHerdQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1001);
		SetName(L("Four Panels and a Herd"));
		SetType(QuestType.Sub);
		SetDescription(L("Ibre is the first night's halt on the road out of Roxona and its stockade is 4 panels down. The Blue Cronewts are grazing the ground it stood on and dragging the rails into their beds."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Drove-Master] Kazys"), "f_tableland_70");

		AddObjective("killCronewts", L("Kill Blue Cronewts grazing the halt ground"),
			new KillObjective(25, new[] { MonsterId.Cronewt_Blue }));

		AddObjective("collectTimber", L("Recover usable timber from the Cronewt beds"),
			new CollectItemObjective(663119, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(663119, 0.35f, MonsterId.Cronewt_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663119, character.Inventory.CountItem(663119), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663119, character.Inventory.CountItem(663119), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Cold Shoes for a Cold Road
//-----------------------------------------------------------------------------

public class ColdShoesForAColdRoadQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1002);
		SetName(L("Cold Shoes for a Cold Road"));
		SetType(QuestType.Sub);
		SetDescription(L("There has been no fuel cart up to Ibre in 5 weeks and Egle is shaping iron cold. The Blue Hohen Mages carry black stone that burns hotter and cleaner than charcoal."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Farrier] Egle"), "f_tableland_70");

		AddObjective("collectStone", L("Take black stone from Blue Hohen Mages"),
			new CollectItemObjective(663120, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(663120, 0.35f, MonsterId.Hohen_Mage_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663120, character.Inventory.CountItem(663120), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663120, character.Inventory.CountItem(663120), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Ridge Shamans
//-----------------------------------------------------------------------------

public class TheRidgeShamansQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1003);
		SetName(L("The Ridge Shamans"));
		SetType(QuestType.Sub);
		SetDescription(L("There were 4 Blue Lapasape Shamans on the Ibre ridge in the spring and there are 20 now, all of them walked up off the Mandara side. Domas cannot walk a convoy flank until they are gone."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trailhand] Domas"), "f_tableland_70");

		AddObjective("killShamans", L("Kill Blue Lapasape Shamans on the ridge"),
			new KillObjective(20, new[] { MonsterId.Lapasape_Bow_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Which Way the Rails Fell
//-----------------------------------------------------------------------------

public class WhichWayTheRailsFellQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1004);
		SetName(L("Which Way the Rails Fell"));
		SetType(QuestType.Sub);
		SetDescription(L("Four stockade panels went down on the night the 19th convoy slept inside them. Rugile has kept the ground untouched for 5 weeks and wants the post holes read before anybody rebuilds."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pen-Warden] Rugile"), "f_tableland_70");

		AddObjective("readPanels", L("Read the 4 fallen stockade panels"),
			new VariableCheckObjective("Laima.Quests.f_tableland_70.Quest1004.Looked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_70.Quest1004.Looked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_70.Quest1004.Panel{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_70.Quest1004.Looked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_70.Quest1004.Panel{i}");
	}
}

// Quest 1005 CLASS: The Nineteenth Convoy
//-----------------------------------------------------------------------------

public class TheNineteenthConvoyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_70", 1005);
		SetName(L("The Nineteenth Convoy"));
		SetType(QuestType.Sub);
		SetDescription(L("Vaidotas signed 40 people into the Ibre pen and walked an empty pen to Sventimas, where a clerk signed 40 in without counting. Two Hohen Manes have watched the halt from the ridge every day since."));
		SetLocation("f_tableland_70");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Drover] Vaidotas"), "f_tableland_70");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_70", 1004));

		AddObjective("clearRidge", L("Kill Blue Hohen Mages holding the ridge"),
			new KillObjective(25, new[] { MonsterId.Hohen_Mage_Blue }));

		AddObjective("takeThePair", L("Take the pair that have watched the halt"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Hohen_Mane_Purple, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Lapasape_Bow_Blue, 3),
				},
				resetIdent: "clearRidge",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603129, 1)); // Heretic's Bracelet
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663121, character.Inventory.CountItem(663121), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663121, character.Inventory.CountItem(663121), InventoryItemRemoveMsg.Destroyed);
	}
}
