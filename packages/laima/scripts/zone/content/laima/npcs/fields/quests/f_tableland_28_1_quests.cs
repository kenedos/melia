//--- Melia Script ----------------------------------------------------------
// Mesafasla - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_28_1 map. A post with an
// establishment for 40 doing a company's work with 9.
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

public class FTableland281QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Rations for Forty
		// =====================================================================
		// Quartermaster Rimgaile - feeding 9 off an establishment for 40
		//---------------------------------------------------------------------
		AddNpc(20141, L("[Quartermaster] Rimgaile"), "f_tableland_28_1", 719, -18, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1001);

			dialog.SetTitle(L("Rimgaile"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A quartermaster is cutting a return sheet down the middle with a knife rather than crossing it out*{/}"));
				await dialog.Msg(L("Oh, look. A new face — and not one of mine, so you get to leave when I'm done with you, lucky thing. Sit down, don't sit down, I don't care, just don't touch the sheet."));
				await dialog.Msg(L("This post is on the books for forty. There are nine of us breathing. Roxona bills me for forty rations, I sign for forty rations, and the day I write '9' on this ledger is the day some clerk decides Mesafasla doesn't need to exist. So — kill 30 Green Lepusbunnies, and bring me 6 pelts off the Assassins. For the fiction, understand. Everything here is for the fiction."));

				var response = await dialog.Select(L("Care to keep the fiction alive with me?"),
					Option(L("I'll clear them and get the pelts"), "help"),
					Option(L("What do you want pelts for?"), "info"),
					Option(L("Write 9 on the sheet"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The plain ones are everywhere, breeding like the paperwork does. Not dangerous — just endless. Work outward from the post and don't let a knot of them box you in."));
						await dialog.Msg(L("The Assassins, west of here, are a different animal — six pelts, and I mean exactly six. Bring me seven and I'll assume you're skimming."));
						break;

					case "info":
						await dialog.Msg(L("Linings, obviously. Nine bodies, four greatcoats, and this shelf hits freezing by mid-month — do the arithmetic yourself."));
						await dialog.Msg(L("I *could* requisition coats properly. Eleven weeks of paperwork, and it'd arrive sized for forty men, thirty-one of which would sit in a Roxona warehouse looking smug. So. Pelts. Much faster, and nobody official has to know."));
						break;

					case "leave":
						await dialog.Msg(L("Mm-hm. And then some clerk in Roxona sees '9' and decides that's not a post, that's a detachment, and detachments get folded into whoever's nearest. There is no whoever's nearest. We *are* the nearest."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killBunnies", out var killObj)) return;
				if (!quest.TryGetProgress("collectPelts", out var peltObj)) return;

				if (killObj.Done && peltObj.Done)
				{
					await dialog.Msg(L("{#666666}*She lays the 6 pelts fur-side down and starts measuring them against a folded coat, muttering measurements to herself*{/}"));
					await dialog.Msg(L("Five linings out of six pelts — and the sixth goes to Petras, because that boy stands on a cairn for four hours at a stretch and has never once asked me for a single thing. Someone should reward that kind of foolishness."));
					await dialog.Msg(L("Here's your pay, out of the thirty-one rations I sign for and never draw. Cleanest coin on this post, and I'd rather it went to you than back to a Roxona warehouse."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Ground's thinner already, good. Six pelts off the Assassins in the west and I can start cutting linings."));
				}
				else
				{
					await dialog.Msg(L("Thirty of the plain ones first, working outward. They won't kill you. They will simply never, ever stop arriving, which is somehow worse."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Nine linings finished, two spare — and Gomen refused his until every last one of us had one. Man's going to be the last person on this shelf to be warm, and he looks positively delighted about it."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Firebreak Nobody Ordered
		// =====================================================================
		// Sapper Vaitkus - cutting a break alone
		//---------------------------------------------------------------------
		AddNpc(20156, L("[Sapper] Vaitkus"), "f_tableland_28_1", 1260, -609, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1002);

			dialog.SetTitle(L("Vaitkus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A sapper is dragging cut scrub into a windrow, humming tunelessly to himself, with a rake worn down to half a head*{/}"));
				await dialog.Msg(L("Oh, HAH! A person! An actual person, walking, with a face! Come here, come here, you have to see this — nobody's stopped to talk to me since the second week of autumn, and I've started narrating my own work out loud, which is a bad sign, I'm told."));
				await dialog.Msg(L("Firebreak. Across the whole east shelf, fourteen hundred paces of it, cut by yours truly, solo, over — well, since the second week of autumn, we've established that. What I don't have is anything to actually start the burn with! You'd think that'd be the easy part. Bring me 6 flints off the Red Saltisdaughter Magicians and let's finally set something on fire."));

				var response = await dialog.Select(L("So? Fancy helping a man commit some very controlled arson?"),
					Option(L("I'll bring 6 flints"), "help"),
					Option(L("Who ordered a firebreak?"), "info"),
					Option(L("Burn it with a torch"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They carry them in a hip pouch and strike them one-handed — I've watched them do it forty times now and I still can't manage it, it's honestly a little humiliating."));
						await dialog.Msg(L("Take them off the ones working alone! A Magician in a group will just set the grass alight around you and wait, and that's a much worse afternoon for everyone involved."));
						break;

					case "info":
						await dialog.Msg(L("Nobody! That's rather the beauty of it. If the east shelf goes up it runs downwind into Mesafasla in about forty minutes, and there are nine of us to fight it, so — somebody should probably do something, yes?"));
						await dialog.Msg(L("I asked for a work party. Got told the establishment's forty and to draw the party from it. There is no party! So there's me, a rake, and fourteen hundred paces, and I have made my peace with it, mostly."));
						break;

					case "leave":
						await dialog.Msg(L("Ah — no, no, a torch just burns what's in front of it. A struck flint in the wind burns the *line*, in the order I laid it. It's the difference between a firebreak and just... more fire. Bad fire. The kind I'm trying to prevent!"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectFlints", out var flintObj)) return;

				if (flintObj.Done)
				{
					await dialog.Msg(L("{#666666}*He strikes one against the back of the rake head, gets a spark on the second try, and grins about it for far too long*{/}"));
					await dialog.Msg(L("Six! Marvelous. I only need two — I'll keep the other four, because there's going to be a second autumn, and I'm under no illusion anyone else is cutting this break with me."));
					await dialog.Msg(L("Here, out of the works allowance — meant for forty sappers, drawn by one, so it's easily the least suspicious money on this whole shelf."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Six flints, off the Red Magicians — and only the ones working alone, remember!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("It's burned and it HELD! Fourteen hundred paces of bare ground, and I spent the whole night on the windward side with a wet sack just in case — but look at it! The shelf can catch now and Mesafasla's still standing come morning. I'm rather proud, honestly."));
			}
		});

		// =====================================================================
		// QUEST 1003: The West Is Not Patrolled
		// =====================================================================
		// Outrider Danguole - the assassins on the Stogas road
		//---------------------------------------------------------------------
		AddNpc(20143, L("[Outrider] Danguole"), "f_tableland_28_1", -89, 231, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1003);

			dialog.SetTitle(L("Danguole"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An outrider is scrubbing a saddle that has no horse anywhere within a mile of it*{/}"));
				await dialog.Msg(L("Ha — you walked in on two feet as well, I see. Welcome to the club. My patrol's the Stogas road, west, three times a day, official. I've managed four times in eight weeks. All on foot. Some outrider I am."));
				await dialog.Msg(L("Green Lepusbunny Assassins own the middle of that road, and they take the horse first — every single time, like it's a rule they wrote themselves. Kill 20 of them and I get a road back. Maybe even a horse someday, who knows."));

				var response = await dialog.Select(L("Fancy earning me back a road?"),
					Option(L("I'll clear the road"), "help"),
					Option(L("Take the horse first?"), "info"),
					Option(L("Ride around them"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They come from the sides, never straight on — cowards, the lot of them, and I mean that with real professional respect. Don't stop moving in the middle of the road. That's exactly where they want you, and they've had eight weeks to get good at it."));
						await dialog.Msg(L("They don't chase, at least. Get forty paces clear and you get to pick which three you fight instead of the other way around."));
						break;

					case "info":
						await dialog.Msg(L("Every time. Not the rider — the horse. It goes down, they're simply gone, and you're standing in the road holding an empty saddle with four miles to walk. Very dignified."));
						await dialog.Msg(L("I've lost three horses that way. Walked the last one back myself, leading her — she made two hundred paces before she went, and I wasn't leaving her out there for them to pick clean."));
						break;

					case "leave":
						await dialog.Msg(L("Around's the shelf edge on one side, Vedas ground on the other, and nobody on this whole post is riding onto Vedas ground. Not even me, and I'm reckless."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killAssassins", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*She has the saddle up on her shoulder before you've even finished the sentence*{/}"));
					await dialog.Msg(L("Road's open! Stogas is four hours west, and they haven't seen a rider from us since the action — bet they're wondering why."));
					await dialog.Msg(L("Take the mount allowance. I've drawn it every month for a horse I don't have, and it's been sitting in a tin making me feel exactly as pathetic as you'd expect. Might as well be useful now."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("West, on the road, and they come from the sides. Twenty of them. Keep moving and don't be a target."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three patrols a day, five days running! Ades over at Stogas sent a rider back with a note that just says GOOD in enormous letters — which, coming from Ades, is basically a love letter."));
			}
		});

		// =====================================================================
		// QUEST 1004: The Line to Vedas
		// =====================================================================
		// Signal-Clerk Petras - four cairns and no answer
		//---------------------------------------------------------------------
		AddNpc(155034, L("[Signal-Clerk] Petras"), "f_tableland_28_1", 1978, 599, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1004);

			dialog.SetTitle(L("Petras"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A signal clerk stands on a low cairn with a shuttered lamp, lips moving as he counts flashes under his breath*{/}"));
				await dialog.Msg(L("Give me — sorry, one second, I have to finish the count or I start over, I know that's — there. Sorry! You're the first person to climb up here in... longer than I'd like to say out loud, actually."));
				await dialog.Msg(L("There's, um, four cairns on the east shelf — together they're the signal line to Vedas. I've sent the evening call up that line every night for eight weeks and nothing's ever come back down it. Could you — I mean, would you walk the four and tell me what state they're in? Please?"));

				var response = await dialog.Select(L("Would that be all right? I don't want to be a bother."),
					Option(L("I'll walk the 4 cairns"), "help"),
					Option(L("Nothing at all in 8 weeks?"), "info"),
					Option(L("Stop sending"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Oh — thank you, really. Check the lamp housing, the shutter cord, and the step. Please don't relight anything, even if it looks cold — I need to know it was cold when you found it, that's, um, important for the report."));
						await dialog.Msg(L("They stand in a square on the east ground. You can actually see all four from any one of them — that was the whole point, I think, when somebody built them."));
						break;

					case "info":
						await dialog.Msg(L("Nothing. Fifty-six evenings, and — I'm nineteen, this is the only posting I've ever had, so I don't even really know if that's... normal? I keep meaning to ask someone and then not asking."));
						await dialog.Msg(L("Rimgaile says it isn't normal. Gomen doesn't say anything at all when I bring it up, which I've started finding worse than an actual answer, if I'm honest."));
						break;

					case "leave":
						await dialog.Msg(L("N-no, I can't stop — if I stop, and somebody up there finally lights one, there'd be nobody down here watching for it. I'd rather send into nothing for a year than miss it just once. Sorry. I know that sounds silly."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var walked = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_1.Quest1004.Walked", 0);

				if (walked >= 4)
				{
					await dialog.Msg(L("{#666666}*He listens all the way through without once looking away from the northeast*{/}"));
					await dialog.Msg(L("All four sound. All four oiled. All four with a full lamp. So the line isn't broken — it's never been broken — it's been carrying my call perfectly for fifty-six nights to a post that just... isn't there."));
					await dialog.Msg(L("Here, the lamp allowance, take it — and, um, please don't tell Rimgaile I sat down on the step when you told me that. I'd rather she kept thinking I took it standing up."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("They stand in a square on the east ground. {0} of 4 walked so far. Please don't relight anything!", walked));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I still send it, every evening. Gomen's come out and stood with me the last six nights, and he doesn't say anything either — so now there's two of us not saying anything at a cairn, which is, somehow, so much better than being alone up here."));
			}
		});

		// =====================================================================
		// SIGNAL CAIRNS
		// =====================================================================
		// For Quest 1004 - The Line to Vedas
		// =====================================================================

		void AddSignalCairn(int cairnNumber, string cairnName, string finding, int x, int z, int direction)
		{
			AddNpc(155051, L(cairnName), "f_tableland_28_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_28_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A low signal cairn with a shuttered lamp on top, the housing recently oiled*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_28_1.Quest1004.Cairn{cairnNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have already checked this one over. The shutter cord runs clean*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Checking the cairn..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Check interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var walked = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_28_1.Quest1004.Walked", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_28_1.Quest1004.Walked", walked);

				character.ServerMessage(L(finding));
				character.ServerMessage(LF("Cairns walked: {0}/4", walked));

				if (walked >= 4)
					character.ServerMessage(L("{#FFD700}All 4 cairns walked. Return to Signal-Clerk Petras.{/}"));
			});
		}

		AddSignalCairn(1, "First Signal Cairn", "Housing sound, shutter cord new, lamp full. Somebody has been up here this week.", 1659, 936, 315);
		AddSignalCairn(2, "Second Signal Cairn", "Sound. Step swept. Oil can stowed under the lip with the cap on.", 1659, 816, 315);
		AddSignalCairn(3, "Third Signal Cairn", "Sound. The cord has been replaced twice by the wear marks on the housing.", 1781, 936, 315);
		AddSignalCairn(4, "Fourth Signal Cairn", "Sound, oiled, full. Nothing on this line has failed. Nothing on this line ever did.", 1781, 816, 315);

		// =====================================================================
		// QUEST 1005: Sixty Forms a Day
		// =====================================================================
		// Assistant Commander Gomen - the mark on the cancellation
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Assistant Commander] Gomen"), "f_tableland_28_1", 522, -695, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_28_1", 1005);

			dialog.SetTitle(L("Gomen"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_28_1", 1004)))
				{
					await dialog.Msg(L("{#666666}*He does not look up from the stack in front of him*{/}"));
					await dialog.Msg(L("If you are looking for work, I have none to give you. Signal-Clerk Petras is on his cairn. Walk his line with him first. Whatever you intend to say to me will keep — it has kept eight weeks already, it can keep a little longer."));
					return;
				}

				await dialog.Msg(L("{#666666}*He looks up when your shadow crosses the desk, then returns his eyes to the slip in front of him*{/}"));
				await dialog.Msg(L("You again. Sit, if there is anywhere to sit."));
				await dialog.Msg(L("{#666666}*He reads the cancellation slip through once, sets it down precisely, and rests one finger on his own mark at the bottom*{/}"));
				await dialog.Msg(L("That is my mark. I did not write what is above it. I sign between sixty and ninety forms a day. I have done so for eleven years without fail. Somewhere in one of those stacks, this was placed in front of me, and I put my hand to it without reading a word."));
				await dialog.Msg(L("I cannot give you a name. I can give you the ground between here and Vedas, which no one has held since the action. Kill 20 Red Saltisdaughter Magicians. Take the pair who have made a station of the old company line. I ask this of you formally, and I will not pretend it is a small thing."));

				var response = await dialog.Select(L("Will you take the ground back?"),
					Option(L("I'll take the ground"), "help"),
					Option(L("You really never read it?"), "info"),
					Option(L("Then somebody used you"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(666042, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take my memo. It is written plainly, it is signed, and it states exactly what I have told you. It is the first document in eleven years that I have read four times before setting my hand to it."));
						await dialog.Msg(L("The Magicians, first. The pair on the old line are Assassins, and they have chosen their ground. Do not permit them to choose yours."));
						break;

					case "info":
						await dialog.Msg(L("Sixty a day. Requisitions. Drafts. Transfers. Condemnations. Ration returns. Three separate forms for a single broken cart. No man reads sixty forms. Every man signs sixty forms."));
						await dialog.Msg(L("I have known this for eleven years and called it simply the job. It transpires it is a door, and someone walked through it, and forty-one men now stand on Vedas ground because of a habit I permitted myself."));
						break;

					case "leave":
						await dialog.Msg(L("Yes. That is not a defence, and I offer it as none. A man who can be used as I was used should not hold my post. I have written that down as well, for the record."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearMagicians", out var magObj)) return;
				if (!quest.TryGetProgress("takeTheLine", out var lineObj)) return;

				if (magObj.Done && lineObj.Done)
				{
					await dialog.Msg(L("{#666666}*He receives the report standing, and does not sit for any part of it*{/}"));
					await dialog.Msg(L("The line is open. Danguole may ride it. Petras may walk it. A grave detail may reach Vedas from this side without the long road through Roxona. That is a full account of what your work has purchased."));
					await dialog.Msg(L("Take this. It is mine, not the Kingdom's — it has sat in a chest since I was given this post by a man who told me the entire duty was to read everything. Carry my memo to whoever will accept it. If none will, carry it to the necromancers. They remain the only people on this shelf still writing things down properly."));

					character.Quests.Complete(questId);
				}
				else if (magObj.Done)
				{
					await dialog.Msg(L("The Magicians are cleared from the ground. The two on the old company line remain. They have not moved in eight weeks, and I see no reason they intend to start."));
				}
				else
				{
					await dialog.Msg(L("The Magicians first. I will not send anyone onto the old line while the shelf behind them still stands full."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The memo went down to Roxona six days ago. Nothing has returned, and I did not expect it to. It is written, it is signed, and it exists somewhere other than in my own head. That is the whole of what I was able to do about it."));
				await dialog.Msg(L("I still sign sixty a day. I read every one now. It takes until midnight, and Rimgaile has stopped telling me to go to bed — I believe she has given up on that particular argument."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Rations for Forty
//-----------------------------------------------------------------------------

public class RationsForFortyQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1001);
		SetName(L("Rations for Forty"));
		SetType(QuestType.Sub);
		SetDescription(L("Mesafasla has an establishment for 40 and 9 people on it. Rimgaile needs the Green Lepusbunnies pushed off the post ground and 6 Assassin pelts to line winter coats she cannot requisition."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Quartermaster] Rimgaile"), "f_tableland_28_1");

		AddObjective("killBunnies", L("Kill Green Lepusbunnies around the post"),
			new KillObjective(30, new[] { MonsterId.Repusbunny_Green }));

		AddObjective("collectPelts", L("Recover Lepusbunny Assassin pelts"),
			new CollectItemObjective(666043, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(666043, 0.35f, MonsterId.Repusbunny_Bow_Green);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666043, character.Inventory.CountItem(666043), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666043, character.Inventory.CountItem(666043), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: The Firebreak Nobody Ordered
//-----------------------------------------------------------------------------

public class TheFirebreakNobodyOrderedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1002);
		SetName(L("The Firebreak Nobody Ordered"));
		SetType(QuestType.Sub);
		SetDescription(L("Vaitkus has cut 1,400 paces of firebreak across the east shelf on his own and has nothing to start the burn with. The Red Saltisdaughter Magicians carry flints at the hip."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sapper] Vaitkus"), "f_tableland_28_1");

		AddObjective("collectFlints", L("Recover flints from Red Saltisdaughter Magicians"),
			new CollectItemObjective(666044, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(666044, 0.35f, MonsterId.Saltisdaughter_Mage_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666044, character.Inventory.CountItem(666044), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666044, character.Inventory.CountItem(666044), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The West Is Not Patrolled
//-----------------------------------------------------------------------------

public class TheWestIsNotPatrolledQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1003);
		SetName(L("The West Is Not Patrolled"));
		SetType(QuestType.Sub);
		SetDescription(L("Danguole has made her Stogas patrol 4 times in 8 weeks, on foot, because the Green Lepusbunny Assassins hold the middle of the road and take the horse first."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Outrider] Danguole"), "f_tableland_28_1");

		AddObjective("killAssassins", L("Kill Green Lepusbunny Assassins on the Stogas road"),
			new KillObjective(20, new[] { MonsterId.Repusbunny_Bow_Green }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: The Line to Vedas
//-----------------------------------------------------------------------------

public class TheLineToVedasQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1004);
		SetName(L("The Line to Vedas"));
		SetType(QuestType.Sub);
		SetDescription(L("Petras has sent the evening call up the 4 east cairns for 56 nights and nothing has come back. Walk the line and find out whether the fault is in the cairns."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Signal-Clerk] Petras"), "f_tableland_28_1");

		AddObjective("walkCairns", L("Walk the 4 signal cairns"),
			new VariableCheckObjective("Laima.Quests.f_tableland_28_1.Quest1004.Walked", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1004.Walked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1004.Cairn{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_28_1.Quest1004.Walked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_28_1.Quest1004.Cairn{i}");
	}
}

// Quest 1005 CLASS: Sixty Forms a Day
//-----------------------------------------------------------------------------

public class SixtyFormsADayQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_28_1", 1005);
		SetName(L("Sixty Forms a Day"));
		SetType(QuestType.Sub);
		SetDescription(L("The mark on the cancelled requisition is Gomen's and he did not write it. He cannot give a name. He can give back the ground between Mesafasla and Vedas, which nobody has held since the action."));
		SetLocation("f_tableland_28_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Assistant Commander] Gomen"), "f_tableland_28_1");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_28_1", 1004));

		AddObjective("clearMagicians", L("Kill Red Saltisdaughter Magicians on the old company line"),
			new KillObjective(20, new[] { MonsterId.Saltisdaughter_Mage_Red }));

		AddObjective("takeTheLine", L("Take the pair holding the old company line"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Repusbunny_Bow_Green, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Saltisdaughter_Mage_Red, 3),
				},
				resetIdent: "clearMagicians",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(603127, 1)); // Himil Legacy
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666042, character.Inventory.CountItem(666042), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666042, character.Inventory.CountItem(666042), InventoryItemRemoveMsg.Destroyed);
	}
}
