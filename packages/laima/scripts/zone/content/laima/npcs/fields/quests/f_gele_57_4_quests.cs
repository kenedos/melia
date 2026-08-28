//--- Melia Script ----------------------------------------------------------
// Tenet Garden - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_gele_57_4. Corrupted chapel garden where
// Rodelin undead wander from the crypts and Seedmia plant-sisters root in
// the grave-mounds.
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
		AddNpc(147390, L("[Chapel Follower] Algis"), "f_gele_57_4", 947, 1189, 338, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1001);

			dialog.SetTitle(L("Algis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got his back to a headstone, watching the east path like it might move*{/}"));
				await dialog.Msg(L("—Oh! Gods, you scared half the life out of me, didn't hear you come up at all! Sorry, sorry — I've been half-watching that east path since the crypt doors broke last month, and every little sound makes me jump these days. They were chapel-wards in life, if you can believe it — girls who swept these very paths."));
				await dialog.Msg(L("They don't mean any harm by it, truly — they just don't remember where to stop anymore. Please, would you kill 18 of them? Give them a quiet end. I couldn't bear to ask anyone from the chapel to do it."));

				var response = await dialog.Select(L("Will you do it?"),
					Option(L("I'll put 18 to rest"), "help"),
					Option(L("Why are they walking now?"), "info"),
					Option(L("Undead aren't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They cluster on the memorial paths east of here — o-oh, and say a chapel word over each one, would you? Rest, return, release. It doesn't help them, not really, but it helps me sleep at night."));
						break;

					case "info":
						await dialog.Msg(L("A crypt-seal failed, that's all it takes, just one. Re-blessing it needs a proper chapel wardmage, and the one in Fedimian stopped answering our letters three weeks ago — three weeks! I check the post every single day."));
						await dialog.Msg(L("So until somebody remembers we're even out here, we put them back one at a time, and I try not to think too hard about the math of it."));
						break;

					case "leave":
						await dialog.Msg(L("Oh — of course, of course, no trouble at all! Just, take the west path if you're passing through, would you? They're thickest to the east, and they don't much care who they follow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRodelin", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("All 18! Back where they belong, oh thank the goddess. The memorial paths will be walkable again, for a few days at least."));
					await dialog.Msg(L("Here — chapel coin, out of the warden's share, don't ask how. Just don't tell the steward, please, the accounts don't cover mercy-strokes and he counts everything."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still more of them on the east paths, I'm afraid. Keep going, and — do be careful."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("They still drift up, now and then, but fewer. Whatever's left of the seal is holding the rest down. For now, anyway — I try not to think about later."));
			}
		});

		// Quest 1002: The Reliquary-List
		//---------------------------------------------------------------------
		AddNpc(147400, L("[Chapel Steward] Vincas"), "f_gele_57_4", 1305, 2033, 99, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);

			dialog.SetTitle(L("Vincas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's got a ledger open on a crate, crossing out lines faster than he's writing new ones*{/}"));
				await dialog.Msg(L("You'll forgive me if I don't stop writing — I am behind on everything, and every hour I stop is an hour I don't get back. Seventeen reliquaries went missing when the crypt broke. Some sit scattered on the memorial paths. The rest are being carted around by things that used to be chapel-wards, if you'll believe the reports."));
				await dialog.Msg(L("Emilis logs everything that passes his ridge post. Take this list to him, bring back whatever he can match against it, and let me get back to the other forty things on my desk."));

				var response = await dialog.Select(L("Can you carry it?"),
					Option(L("I'll carry the list to Emilis"), "help"),
					Option(L("Why not send a chapel runner?"), "info"),
					Option(L("That's a long road for paper"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Sealed with chapel wax. Do not crack it before he does — the seal is half the authority of the thing, and I haven't time to reseal a broken one."));
						await dialog.Msg(L("His post is up past the garden's north edge, near the forest road. Go on, before I think of six more things I need done."));
						break;

					case "info":
						await dialog.Msg(L("Our runners are children, and the Panto Archers on that ridge shoot at anything shorter than their own bowstave. I won't send a child to die over paperwork."));
						await dialog.Msg(L("You're a traveler. Travelers take their own risks, and I'm not too proud to pay one when my own people can't get through."));
						break;

					case "leave":
						await dialog.Msg(L("Paper's all the chapel has left to send, and I haven't the staff to spare or the time to wait. Suit yourself — I'll find someone, eventually, the way I find everyone eventually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverList", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("Four matched at the ridge. Three more already on their way to Fedimian. Ten still unaccounted for, but four back is four more than my ledger had yesterday, and I'll take it."));
					await dialog.Msg(L("Your pay, and a chapel-charm besides. Won't fix the crypt. Will keep Rodelin hands off your cloak, which is more than most days offer around here."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Emilis keeps the ridge watchpost past the garden's north edge. The chapel wax has to be unbroken when you hand it over."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four reliquaries back on the shelf, finally. I've written to the Fedimian magistrates about the other ten — one more letter into the void, but it's their problem to ignore now, not mine."));
			}
		});

		// Quest 1002 recipient - Ridge-Watch Emilis
		//---------------------------------------------------------------------
		AddNpc(147403, L("[Ridge-Watch] Emilis"), "f_gele_57_4", -998, 2041, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);
			var deliveredKey = "Laima.Quests.f_gele_57_4.Quest1002.Delivered";

			dialog.SetTitle(L("Emilis"));

			if (character.Quests.IsActive(questId))
			{
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("{#666666}*He breaks the chapel wax with a thumbnail and unrolls the list*{/}"));
					await dialog.Msg(L("Vincas still writes like a clerk. Good. I can actually read it, which is more than I can say for half the reports that cross this post."));
					await dialog.Msg(L("Four of these are on my shelf already. Three more went past last week, bound for Fedimian — I'll send a rider after them myself. The other ten are out in the wind, wherever the wind's taken them."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(questId, "deliverList");

					await dialog.Msg(L("Sealed the reply with ridge-pitch, not chapel wax — wax cracks in this cold, and Vincas knows my mark well enough by now."));
					character.ServerMessage(L("{#FFD700}Emilis's reply received. Return to Chapel Steward Vincas.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the pitch-sealed reply back down to Vincas. He'll know the mark on sight."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Fedimian rider went through right on schedule — my schedule, not the Kingdom's, which is the only kind that's ever mattered up here. Three more reliquaries should surface in the capital inside a fortnight, passes willing."));
			}
			else
			{
				await dialog.Msg(L("Travelers come through. I log what they're carrying. That's the whole of my job, and I'd wager it's saved more lives up here than half the garrison south of the ridge."));
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
				await dialog.Msg(L("Careful where you step — that bed's all root, and I won't have you crushing what's left of it. Count the saplings in the east beds now and you'll get eight. Count them last spring, you'd have gotten thirty. The rest are still standing right where they were, and you are not to touch them, understand?"));
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
					await dialog.Msg(L("Here — caretaker's coin, and a twist of tea leaves off my own bench. Steep them slow, and don't you dare rush it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("More of them still in the beds. Lift from the base and take the whole root ball with you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("All six took to the new plot, still green, roots sunk in clean soil. Whatever's working through the east beds can't reach that far, and it won't, not while I've got eyes on it."));
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
					await dialog.Msg(L("{#666666}*A young Seedmia rooted in the garden bed, petals folded in tight. The soil around it smells of the crypts*{/}"));
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
				await dialog.Msg(L("Mind the loose stones on your way over, I haven't finished cataloguing this one yet. The crypt-break opened four garden plots atop the main seal, and not one record tells us which went first. The crack patterns will, if read correctly — cracks always tell the truth, unlike people."));
				await dialog.Msg(L("Go to all 4 memorial stones and read the cracks. Inward means something pushed in from outside. Outward means something pushed out from within."));

				var response = await dialog.Select(L("Will you walk them for me?"),
					Option(L("I'll read the 4 memorial stones"), "help"),
					Option(L("Why does the direction matter?"), "info"),
					Option(L("Grave-business isn't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't move the stones — just read them, precisely, the way I taught myself to. Note whether the fractures are fresh or weathered; that alone gives me the sequence."));
						await dialog.Msg(L("Outward and old. That's the one combination that ruins my sleep, and has for weeks now."));
						break;

					case "info":
						await dialog.Msg(L("Inward cracks mean robbers, a landslide, a crypt-beam settling. Ordinary things."));
						await dialog.Msg(L("If three cracked inward and one outward, then one Rodelin came up and woke the rest, and that's manageable. If all four cracked outward, there are four things walking that nobody counted."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. I'll walk the memorial stones myself next week, then, if the crypt holds that long."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("readPlots", out var plotObj)) return;

				if (plotObj.Done)
				{
					await dialog.Msg(L("One inward and fresh. Two inward and old. Three outward and fresh. Four outward — older than any of the rest, and there, in the dust, a child's footprint facing the chapel door. I've read that stone six times now and it says the same thing every time."));
					await dialog.Msg(L("Two outward. That means Plot Four went first, maybe a season ago, and not one of us noticed. We thought we were dealing with a single waking. There were two, this whole time."));
					await dialog.Msg(L("Take this straight to Watcher Reike — tonight, not tomorrow. She keeps the west shrine, and she deserves to hear it from me before anyone else."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four plots on the east paths. Inward or outward, fresh or weathered. Read them all and come back."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The chapel-watch posted a second warden on Plot Four. The footprint hasn't returned. I still check for it every morning regardless — old habits, and this one I intend to keep."));
			}
		});

		// Quest 1004 interaction points - the four opened memorial plots
		//---------------------------------------------------------------------
		void AddMemorialPlot(int plotNumber, string plotName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, plotName, "f_gele_57_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_gele_57_4", 1004);
				var variableKey = $"Laima.Quests.f_gele_57_4.Quest1004.Plot{plotNumber}";
				var counterKey = "Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered memorial stone. The crypt beneath it looks undisturbed*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You have already read this plot's cracks*{/}"));
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
					character.ServerMessage(LF("Plots read: {0}/4", read));

					if (read >= 4)
						character.ServerMessage(L("{#FFD700}All four plots read. Return to Memorial Keeper Ieva.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddMemorialPlot(1, L("Memorial Stone"),
			L("Plot One: the cracks run inward toward the center. Fresh - the stone-dust is still pale at the fracture edges."), -629, 2110, 0);
		AddMemorialPlot(2, L("Memorial Stone"),
			L("Plot Two: the cracks run inward. Older - moss has begun closing the fractures at the rim."), -710, 1532, 0);
		AddMemorialPlot(3, L("Memorial Stone"),
			L("Plot Three: the cracks run outward. Fresh. The capstone leans away from the plot's heart."), 274, 899, 0);
		AddMemorialPlot(4, L("Memorial Stone"),
			L("Plot Four: the cracks run outward and they are older than the rest. A small footprint in the dust, a child's size, faces the chapel door."), 718, 1213, 0);

		// Quest 1005: What Came Out of Plot Four
		//---------------------------------------------------------------------
		AddNpc(147422, L("[Watcher] Reike"), "f_gele_57_4", -1350, 389, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1005);

			dialog.SetTitle(L("Reike"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_gele_57_4", 1004)))
				{
					await dialog.Msg(L("{#666666}*She doesn't turn from the tree line, but her hand drifts off her weapon when she clocks who it is*{/}"));
					await dialog.Msg(L("You walk quiet, for someone I didn't hear coming. I don't like that. I keep the west shrine and watch this garden, and lately I've had the feeling the garden's started watching back."));
					await dialog.Msg(L("Ieva's reading the opened plots for me. Until she tells me which one went first, I'm only guessing — and I don't send people out on a guess. Not into that."));
					return;
				}

				await dialog.Msg(L("Ieva's rubbings came in last night, and I haven't slept since. Plot Four opened outward, a full season before the crypt broke — something's been standing in this garden that whole time, watching us dig."));
				await dialog.Msg(L("A Chapparition. Wears the chapel like a coat, and it raised every Rodelin walking the east paths to keep itself hidden. Kill 12 of them and it'll have nothing left to hide behind. Then, and only then, it comes for you."));

				var response = await dialog.Select(L("Will you take it?"),
					Option(L("I'll face the Chapparition"), "help"),
					Option(L("What is a Chapparition?"), "info"),
					Option(L("Not for a shrine-keeper's purse"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("It doesn't charge. It arrives — somewhere you've already walked past, somewhere you'd swear was empty a moment ago. Keep your back to open ground. Never to the stones."));
						break;

					case "info":
						await dialog.Msg(L("Grief that outlasted whoever first felt it. A chapel holds a great deal of that, and when a seal fails, it finally has somewhere to go."));
						await dialog.Msg(L("The child's footprint Ieva found — it's its. That part I haven't written down anywhere, and I'd rather it stayed that way."));
						break;

					case "leave":
						await dialog.Msg(L("It's not just a shrine-keeper's purse. The order's been saving toward this since the first plot opened."));
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
					await dialog.Msg(L("The order's whole purse, and the blade with it. Pulled from Plot Four before the chapel sealed the crypt back up. It belongs with whoever finished this, and that's you."));

					character.Quests.Complete(questId);
				}
				else if (stripObj.Done)
				{
					await dialog.Msg(L("The paths are bare and it has nowhere to stand. Go back out - it will find you."));
				}
				else
				{
					await dialog.Msg(L("Too many Rodelin still on the paths. It won't show itself while it has cover."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Plot Four's been re-sealed, and re-named properly this time. The chaplain in it had a name after all — Ieva found it buried in the reliquary list Vincas got back. Small mercy, but I'll take it."));
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
		SetDescription(L("Brown Rodelin keep drifting up out of the broken crypt and onto the chapel's memorial paths. Follower Algis wants them given a quiet end."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chapel Follower] Algis"), "f_gele_57_4");

		AddObjective("killRodelin", L("Kill Brown Rodelin on the memorial paths"),
			new KillObjective(18, new[] { MonsterId.Zombiegirl2_Brown }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640011, 1)); // Recovery Potion
	}
}

// Quest 1002 CLASS: The Reliquary-List
//-----------------------------------------------------------------------------

public class TheReliquaryListQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1002);
		SetName(L("The Reliquary-List"));
		SetType(QuestType.Sub);
		SetDescription(L("Seventeen reliquaries went missing when the crypt broke. Carry Steward Vincas's sealed list to the ridge watchpost and bring back whatever Emilis can match against his log."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Chapel Steward] Vincas"), "f_gele_57_4");

		AddObjective("deliverList", L("Take the sealed list to Ridge-Watch Emilis"),
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
		SetDescription(L("Rot from the crypts is spreading root to root through the memorial beds, and a Seedmia that drinks it is lost where it stands. Lift six young saplings out and pot them for Caretaker Egle before it reaches their row."));
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
		SetDescription(L("Four garden plots opened alongside the main crypt seal and nobody knows which went first. Read the crack-patterns on all four and report them to Memorial Keeper Ieva."));
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

// Quest 1005 CLASS: What Came Out of Plot Four
//-----------------------------------------------------------------------------

public class WhatCameOutOfPlotFourQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1005);
		SetName(L("What Came Out of Plot Four"));
		SetType(QuestType.Sub);
		SetDescription(L("Ieva's rubbings place the first opening at Plot Four, a season before the crypt broke. Strip the Rodelin off the memorial paths so the Chapparition has nothing left to hide behind, then put it down."));
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Watcher] Reike"), "f_gele_57_4");

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
