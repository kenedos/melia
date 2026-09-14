//--- Melia Script ----------------------------------------------------------
// Tenet Garden - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_gele_57_4. The Tenet Church's garden, where
// Brown Rodelin come up out of the Guards Graveyard and rot from below is
// working through the flower beds.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FGele574QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: The Garden's Dead Walk
		//---------------------------------------------------------------------
		AddNpc(147390, L("[Chapel Follower] Kazys"), "f_gele_57_4", 947, 1189, 338, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1001);

			dialog.SetTitle(L("Kazys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got his back to a headstone, flinching at every sound*{/}"));
				await dialog.Msg(L("—Oh! Gods, you scared half the life out of me, didn't hear you come up at all! Sorry, sorry — I've been jumping at shadows since the Guards Graveyard broke open last month, and every little sound sets me off these days. Those Brown Rodelin were people once, would you believe that?"));
				await dialog.Msg(L("They look like monsters now, but they've stayed docile, near enough. Please, would you help me put them to rest? Kill 18 of them, and I'll do the praying from here."));

				var response = await dialog.Select(L("Will you do it?"),
					Option(L("I'll do it"), "help"),
					Option(L("What happened to them?"), "info"),
					Option(L("Undead aren't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("You'll find plenty of them out there. Say something over each one, would you, whatever you'd want said over you. It doesn't help them, not really, but it helps me sleep at night."));
						break;

					case "info":
						await dialog.Msg(L("The graveyard gave way underneath us, that's all it takes. Closing it again needs a priest, and Fedimian stopped answering our letters three weeks ago — three weeks! I check for word every single day."));
						await dialog.Msg(L("So until somebody remembers we're even out here, we put them back one at a time, and I try not to think too hard about the math of it."));
						break;

					case "leave":
						await dialog.Msg(L("Oh — of course, of course, no trouble at all! Just keep your distance if you're passing through, would you? They don't much care who they follow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRodelin", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("All 18! Back where they belong, oh thank the goddess. Tenet Garden will be walkable again, for a few days at least."));
					await dialog.Msg(L("Here, take this. It'll help your travels."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still more of them out there, I'm afraid. Keep going, and — do be careful."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("They still come up, now and then, but fewer. Whatever's left of the ground down there is holding the rest. For now, anyway — I try not to think about later."));
			}
		});

		// Quest 1002: The Steward's List
		//---------------------------------------------------------------------
		AddNpc(147400, L("[Chapel Steward] Vincas"), "f_gele_57_4", 1305, 2033, 99, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);

			dialog.SetTitle(L("Vincas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got a ledger open on a crate, crossing out lines faster than he's writing new ones*{/}"));
				await dialog.Msg(L("You'll forgive me if I don't stop writing — I'm behind on everything, and every hour I stop is an hour I don't get back. Seventeen boxes of church silver went out of that storeroom the night the graveyard broke. Some are lying in the grass out there. The rest walked off on things that used to be people."));
				await dialog.Msg(L("Emilis watches the road up to Akmens Ridge and writes down everything that passes him. Take him this list, bring back whatever he's already got, and let me get on with the other forty things on this crate."));

				var response = await dialog.Select(L("Can you carry it?"),
					Option(L("I'll carry the list to Emilis"), "help"),
					Option(L("Can't one of your own take it?"), "info"),
					Option(L("That's a long walk for a piece of paper"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Here. Don't open it on the way — Emilis reads it first or he won't believe a word of it, and I haven't the time to write it twice."));
						await dialog.Msg(L("He keeps his post out on the Akmens Ridge road. Go on, before I think of six more things I need done."));
						break;

					case "info":
						await dialog.Msg(L("The only people I have left are children and old women, and the Panto Archers out there shoot at anything that moves. I won't send a child to die over paperwork."));
						await dialog.Msg(L("You're a traveler. Travelers take their own risks, and I'm not too proud to pay one when my own people can't get through."));
						break;

					case "leave":
						await dialog.Msg(L("Paper's all I have left to send, and nobody left to send it with. Suit yourself — I'll find someone, eventually, the way I find everyone eventually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverList", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("Four matched at his post. Three more already on their way to Fedimian. Ten still unaccounted for, but four back is four more than my ledger had yesterday, and I'll take it."));
					await dialog.Msg(L("Your pay, and something for the road besides. Won't fix the graveyard. Will keep you upright the next time something comes at you out there."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Emilis keeps his post out on the Akmens Ridge road. Hand him the list unopened."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four boxes back on the shelf, finally. I've written to the Fedimian magistrates about the other ten — one more letter into the void, but it's their problem to ignore now, not mine."));
			}
		});

		// Quest 1002 recipient - Emilis on the Akmens Ridge road
		//---------------------------------------------------------------------
		AddNpc(147403, L("[Ridge Watchman] Emilis"), "f_gele_57_4", -998, 2041, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);
			var deliveredKey = "Laima.Quests.f_gele_57_4.Quest1002.Delivered";

			dialog.SetTitle(L("Emilis"));

			if (character.Quests.IsActive(questId))
			{
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("{#666666}*He unrolls the list, holds it out at arm's length, and reads it twice*{/}"));
					await dialog.Msg(L("Vincas still writes like a clerk. Good. I can actually read it, which is more than I can say for half of what crosses this post."));
					await dialog.Msg(L("Four of these are on my shelf already. Three more went past last week, bound for Fedimian — I'll send a rider after them myself. The other ten are out in the wind, wherever the wind's taken them."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(questId, "deliverList");

					await dialog.Msg(L("Here's my answer, written on the back of his own list. Vincas knows my hand well enough by now."));
					character.ServerMessage(L("{#FFD700}Emilis's reply received. Return to Chapel Steward Vincas.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry my answer back down to Vincas. He'll know the hand on sight."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Fedimian rider went through right on schedule — my schedule, not the Kingdom's, which is the only kind that's ever mattered up here. Three more boxes should surface down there inside a fortnight, roads willing."));
			}
			else
			{
				await dialog.Msg(L("Travelers come through. I log what they're carrying. That's the whole of my job, and I'd wager it's saved more lives than half the garrison in Klaipeda."));
			}
		});

		// Quest 1003: Transplanting the Saplings
		//---------------------------------------------------------------------
		AddNpc(147473, L("[Garden Caretaker] Egle"), "f_gele_57_4", -1020, 1107, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1003);

			dialog.SetTitle(L("Egle"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched at the edge of a garden bed, fingers pressed into the soil like she's checking a pulse*{/}"));
				await dialog.Msg(L("Careful where you step — that bed's all root, and I won't have you crushing what's left of it. Count the saplings in these beds now and you'll get eight. Count them last spring, you'd have gotten thirty. The rest are still standing right where they were, and you are not to touch them, understand?"));
				await dialog.Msg(L("The rot runs root to root underground, one bed into the next, and a Seedmia can't just step out of its way — that's the whole trouble with being a plant. Lift 6 of the young ones and pot them, quick, before the rot reaches their row too."));

				var response = await dialog.Select(L("Will you help me move them?"),
					Option(L("I'll pot 6 saplings"), "help"),
					Option(L("What happens to the ones it reaches?"), "info"),
					Option(L("Plants can reseed themselves"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Lift from the base. Never the crown, you'll kill it outright. Take the whole root ball, and leave the bad soil right there in the hole where it belongs."));
						await dialog.Msg(L("And keep your guard up while you're at it. The grown ones in that bed drank the rot a year back, and they'll strike at anything fool enough to come in reach now."));
						break;

					case "info":
						await dialog.Msg(L("Petals grey out, stem sets hard as a fencepost. They don't go anywhere after that — they just stop being what they were, and they'll open you up if you stand too close admiring the view."));
						await dialog.Msg(L("That's why I'm not sending you after the grown ones. Roots that deep don't come up whole, and there'd be nothing worth saving if they did. The young ones still lift clean, so that's where you're going."));
						break;

					case "leave":
						await dialog.Msg(L("They'd only seed themselves straight back into the same poisoned row. That's the whole problem with leaving well enough alone."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("potSaplings", out var potObj)) return;

				if (potObj.Done)
				{
					await dialog.Msg(L("Root balls whole, no black in the fibers — six came up clean, and most people I send out there can't manage two without snapping a root."));
					await dialog.Msg(L("Here, take this, and a twist of tea leaves off my own bench. Steep them slow, and don't you dare rush it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More of them still in the beds. Lift from the base and take the whole root ball with you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All six took to the new plot, still green, roots sunk in clean soil. Whatever's working through the old beds can't reach that far, and it won't, not while I've got eyes on it."));
			}
		});

		// Quest 1003 interaction points - Seedmia saplings in the garden beds
		//---------------------------------------------------------------------
		void AddSeedmiaSapling(int saplingNum, int x, int z, int direction)
		{
			AddNpc(160049, L("Seedmia Sapling"), "f_gele_57_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_gele_57_4", 1003);
				var variableKey = $"Laima.Quests.f_gele_57_4.Quest1003.Sapling{saplingNum}";
				var counterKey = "Laima.Quests.f_gele_57_4.Quest1003.SaplingsPotted";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A young Seedmia rooted in the garden bed, petals folded in tight. The soil around it smells of open graves*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This bed is empty. You already potted the sapling that grew here*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, new[] { MonsterId.Seedmia }, 300);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The roots tear free and the grown ones in the bed turn on you!{{/}}"));

				var result = await character.TimeActions.StartAsync(
					L("Lifting the sapling..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var potted = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, potted);
					character.ServerMessage(LF("Saplings potted: {0}/6", potted));

					if (potted >= 6)
						character.ServerMessage(L("{#FFD700}That's six. Return to Garden Caretaker Egle.{/}"));
				}
				else
				{
					character.ServerMessage(L("You set the sapling back into the loam."));
				}
			});
		}

		AddSeedmiaSapling(1, -560, 1160, 0);
		AddSeedmiaSapling(2, -530, 1195, 90);
		AddSeedmiaSapling(3, -570, 1290, 180);
		AddSeedmiaSapling(4, -567, 1330, 270);
		AddSeedmiaSapling(5, -620, 1275, 45);
		AddSeedmiaSapling(6, -595, 1245, 135);
		AddSeedmiaSapling(7, -960, 1290, 225);
		AddSeedmiaSapling(8, -1000, 1150, 315);

		// Quest 1004: Which Graves Were Touched
		//---------------------------------------------------------------------
		AddNpc(147416, L("[Memorial Keeper] Ieva"), "f_gele_57_4", -489, 1995, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1004);

			dialog.SetTitle(L("Ieva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched over a memorial stone with a rubbing-cloth, frowning at the fracture lines*{/}"));
				await dialog.Msg(L("Mind the loose stones on your way over, I haven't finished with this one yet. Four memorial stones cracked open the night the graveyard gave way, and not one record tells us which went first. The cracks will, if they're read properly — cracks tell the truth, unlike people."));
				await dialog.Msg(L("Go to all 4 memorial stones out in the garden and read them. Cracks running inward mean something pushed in from outside. Running outward means something pushed out from under."));

				var response = await dialog.Select(L("Will you walk them for me?"),
					Option(L("I'll read the 4 memorial stones"), "help"),
					Option(L("Why does the direction matter?"), "info"),
					Option(L("Grave-business isn't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't move them — just read them, precisely, the way I taught myself to. Note whether the cracks are fresh or weathered; that alone gives me the order they broke in."));
						await dialog.Msg(L("Outward and old. That's the one answer I don't want, and I've been dreading it for weeks now."));
						break;

					case "info":
						await dialog.Msg(L("Inward cracks mean grave-robbers, or the ground settling, or a beam giving out below. Ordinary things."));
						await dialog.Msg(L("If three cracked inward and one outward, then one Brown Rodelin came up and woke the rest, and that's manageable. If all four cracked outward, there are four things walking that nobody counted."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. I'll walk the memorial stones myself next week, then, if the ground holds that long."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readPlots", out var plotObj)) return;

				if (plotObj.Done)
				{
					await dialog.Msg(L("First stone inward and fresh. Second inward and old. Third outward and fresh. The fourth outward — older than any of the rest, and there, in the dust, a child's footprint facing the church door. I've read that stone six times now and it says the same thing every time."));
					await dialog.Msg(L("Two outward. That means the fourth grave opened first, maybe a season ago, and not one of us noticed. We thought we were dealing with a single waking. There were two, this whole time."));
					await dialog.Msg(L("Take this straight to Milda — tonight, not tomorrow. She watches the Septyni Glen road, and she deserves to hear it from me before anyone else."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four memorial stones out in the garden. Inward or outward, fresh or weathered. Read them all and come back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The church posted a second watchman at the fourth grave. The footprint hasn't returned. I still check for it every morning regardless — old habits, and this one I intend to keep."));
			}
		});

		// Quest 1004 interaction points - the four cracked memorial stones
		//---------------------------------------------------------------------
		void AddMemorialStone(int stoneNumber, string stoneName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, stoneName, "f_gele_57_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_gele_57_4", 1004);
				var variableKey = $"Laima.Quests.f_gele_57_4.Quest1004.Plot{stoneNumber}";
				var counterKey = "Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered memorial stone. The ground beneath it looks undisturbed*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have already read this stone's cracks*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Reading the crack-pattern..."), L("Cancel"), "SITREAD", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var read = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, read);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Memorial stones read: {0}/4", read));

					if (read >= 4)
						character.ServerMessage(L("{#FFD700}All four memorial stones read. Return to Memorial Keeper Ieva.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddMemorialStone(1, L("Memorial Stone"),
			L("First stone: the cracks run inward toward the center. Fresh - the stone-dust is still pale at the broken edges."), -629, 2110, 0);
		AddMemorialStone(2, L("Memorial Stone"),
			L("Second stone: the cracks run inward. Older - moss has begun closing them at the rim."), -710, 1532, 0);
		AddMemorialStone(3, L("Memorial Stone"),
			L("Third stone: the cracks run outward. Fresh. The capstone leans away from the grave."), 274, 899, 0);
		AddMemorialStone(4, L("Memorial Stone"),
			L("Fourth stone: the cracks run outward and they are older than the rest. A small footprint in the dust, a child's size, faces the church door."), 718, 1213, 0);

		// Quest 1005: What Came Out of the Fourth Grave
		//---------------------------------------------------------------------
		AddNpc(147422, L("[Road Watcher] Milda"), "f_gele_57_4", -1350, 389, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1005);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_gele_57_4", 1004)))
				{
					await dialog.Msg(L("{#666666}*She doesn't turn from the tree line, but her hand drifts off her weapon when she clocks who it is*{/}"));
					await dialog.Msg(L("You walk quiet, for someone I didn't hear coming. I don't like that. I watch the Septyni Glen road, and lately I've had the feeling this garden's started watching back."));
					await dialog.Msg(L("Ieva's reading the cracked stones for me. Until she tells me which grave went first, I'm only guessing — and I don't send people out on a guess. Not into that."));
					return;
				}

				await dialog.Msg(L("Ieva's rubbings came in last night, and I haven't slept since. The fourth grave opened outward, a full season before the graveyard gave way — something's been standing in this garden that whole time, watching us dig."));
				await dialog.Msg(L("A Chapparition. It raised every Brown Rodelin walking this garden to keep itself hidden. Kill 12 of them and it'll have nothing left to hide behind. Then, and only then, it comes for you."));

				var response = await dialog.Select(L("Will you take it?"),
					Option(L("I'll face the Chapparition"), "help"),
					Option(L("What is a Chapparition?"), "info"),
					Option(L("Not for a watchman's purse"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It doesn't charge. It arrives — somewhere you've already walked past, somewhere you'd swear was empty a moment ago. Keep your back to open ground. Never to the stones."));
						break;

					case "info":
						await dialog.Msg(L("Grief that outlasted whoever first felt it. A church holds a great deal of that, and when the ground under it opens, it finally has somewhere to go."));
						await dialog.Msg(L("The child's footprint Ieva found — it's its. That part I haven't written down anywhere, and I'd rather it stayed that way."));
						break;

					case "leave":
						await dialog.Msg(L("It isn't just my purse. The church has been putting coin aside since the first stone cracked."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("strip", out var stripObj)) return;
				if (!quest.TryGetProgress("killChapparition", out var bossObj)) return;

				if (stripObj.Done && bossObj.Done)
				{
					await dialog.Msg(L("It's gone. Not banished. Not sealed. Gone — the way a thing goes when there's nothing left in this world still holding it here. First full breath I've taken in weeks."));
					await dialog.Msg(L("Every coin the church set aside, and the sword the thing was carrying. Neither one belongs to me. They belong to whoever finished it, and that's you."));

					character.Quests.Complete(questId);
				}
				else if (stripObj.Done)
				{
					await dialog.Msg(L("The garden's bare and it has nowhere left to stand. Go back out - it will find you."));
				}
				else
				{
					await dialog.Msg(L("Too many Brown Rodelin still standing. It won't show itself while it has cover."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The fourth grave's been closed up, and named properly this time. The chaplain in it had a name after all — Ieva found it on the list Vincas got back from the ridge. Small mercy, but I'll take it."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Garden's Dead Walk
//-----------------------------------------------------------------------------

public class TheGardensDeadWalkQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1001);
		SetName(L("The Garden's Dead Walk"));
		SetType(QuestType.Sub);
		SetDescription(L("Brown Rodelin keep coming up out of the Guards Graveyard and into Tenet Garden. Follower Kazys wants them given a quiet end."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chapel Follower] Kazys"), "f_gele_57_4");

		AddObjective("killRodelin", L("Kill Brown Rodelin in Tenet Garden"),
			new KillObjective(18, new[] { MonsterId.Zombiegirl2_Brown }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}
}

// Quest 1002 CLASS: The Steward's List
//-----------------------------------------------------------------------------

public class TheStewardsListQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1002);
		SetName(L("The Steward's List"));
		SetType(QuestType.Sub);
		SetDescription(L("Seventeen boxes of church silver went missing the night the graveyard broke. Carry Steward Vincas's list to Emilis on the Akmens Ridge road and bring back whatever he can match against his own log."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chapel Steward] Vincas"), "f_gele_57_4");

		AddObjective("deliverList", L("Take Vincas's list to Emilis on the Akmens Ridge road"),
			new ManualObjective());

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Transplanting the Saplings
//-----------------------------------------------------------------------------

public class TransplantingTheSaplingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1003);
		SetName(L("Transplanting the Saplings"));
		SetType(QuestType.Sub);
		SetDescription(L("Rot from the graveyard is spreading root to root through the garden beds, and a Seedmia that drinks it is lost where it stands. Lift six young saplings out and pot them for Caretaker Egle before it reaches their row."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Garden Caretaker] Egle"), "f_gele_57_4");

		AddObjective("potSaplings", L("Pot Seedmia saplings from the garden beds"),
			new VariableCheckObjective("Laima.Quests.f_gele_57_4.Quest1003.SaplingsPotted", 6, true));

		AddReward(new ExpReward(3800, 2700));
		AddReward(new SilverReward(4000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1003.SaplingsPotted");

		for (var i = 1; i <= 8; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1003.Sapling{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1003.SaplingsPotted");

		for (var i = 1; i <= 8; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1003.Sapling{i}");
	}
}

// Quest 1004 CLASS: Which Graves Were Touched
//-----------------------------------------------------------------------------

public class WhichGravesWereTouchedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1004);
		SetName(L("Which Graves Were Touched"));
		SetType(QuestType.Sub);
		SetDescription(L("Find the four cracked memorial stones in Tenet Garden, read the crack-patterns on all of them, and report back to Memorial Keeper Ieva."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Memorial Keeper] Ieva"), "f_gele_57_4");

		AddObjective("readPlots", L("Read the four memorial stones"),
			new VariableCheckObjective("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked", 4, true));

		AddReward(new ExpReward(4200, 3200));
		AddReward(new SilverReward(6000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1004.Plot{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1004.Plot{i}");
	}
}

// Quest 1005 CLASS: What Came Out of the Fourth Grave
//-----------------------------------------------------------------------------

public class WhatCameOutOfTheFourthGraveQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1005);
		SetName(L("What Came Out of the Fourth Grave"));
		SetType(QuestType.Sub);
		SetDescription(L("Ieva's rubbings place the first opening at the fourth grave, a season before the graveyard broke. Clear the Brown Rodelin out of Tenet Garden so the Chapparition has nothing left to hide behind, then put it down."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Road Watcher] Milda"), "f_gele_57_4");

		AddPrerequisite(new CompletedPrerequisite("f_gele_57_4", 1004));

		AddObjective("strip", L("Kill Brown Rodelin sheltering the Chapparition"),
			new KillObjective(12, new[] { MonsterId.Zombiegirl2_Brown }));

		AddObjective("killChapparition", L("Defeat the Chapparition"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Chapparition, 1) },
				resetIdent: "strip",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(10000, 7000));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(103107, 1)); // Chapparition Sword
		AddReward(new ItemReward(640082, 3)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}
}
