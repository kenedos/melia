//--- Melia Script ----------------------------------------------------------
// Mandara Plateau - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_tableland_71 map. A mine that has dug around
// the same pillar for 40 years without drawing it on a plan.
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

public class FTableland71QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Third Gallery
		// =====================================================================
		// Pit-Boss Alvinas - the Ritters are in the workings
		//---------------------------------------------------------------------
		AddNpc(20158, L("[Pit-Boss] Alvinas"), "f_tableland_71", -1291, 546, 314, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1001);

			dialog.SetTitle(L("Alvinas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A pit-boss stands at the head of a shaft with an unlit lamp, head cocked, listening down into the dark*{/}"));
				await dialog.Msg(L("Quiet — right. Nothing down there but the usual, which these days is a low bar. You're not one of my five, so you're lost or you're looking for work. Doesn't much matter which. Walk with me either way."));
				await dialog.Msg(L("Forty years this mine's run, five of us keep it going. Blue Hohen Ritters came up out of the low ground into the workings, and I will not put a man underground with those things loose at his back on the surface. Kill 25, bring me 8 magic stones off the Barkles — I want light I don't have to carry a flame near, not with what's down in that third gallery now."));

				var response = await dialog.Select(L("Will you clear the workings?"),
					Option(L("I'll clear them and get the stones"), "help"),
					Option(L("Why not carry a flame?"), "info"),
					Option(L("Close the mine"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ritters come at you in a line abreast, never vary it. Break the line at one end — the rest hold formation into a wall you're no longer standing in front of. Simple as that, if you keep your head."));
						await dialog.Msg(L("Barkles are slower, further out. Take the stones off them last, when you've got nothing left at your back to worry about."));
						break;

					case "info":
						await dialog.Msg(L("Because the third gallery's got gas in it now that wasn't there last year. Same rock, same depth, same forty years of workings — and now, gas. From nowhere. I don't like things that come from nowhere."));
						await dialog.Msg(L("Asked Tulis about it three times now. Man keeps looking at that blasted pillar instead of answering me straight, which — in my experience — is an answer all its own."));
						break;

					case "leave":
						await dialog.Msg(L("Then five men with forty years of one trade between them go stand in a queue in Roxona. I'd sooner take my chances against the Ritters, thanks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRitters", out var killObj)) return;
				if (!quest.TryGetProgress("collectStones", out var stoneObj)) return;

				if (killObj.Done && stoneObj.Done)
				{
					await dialog.Msg(L("{#666666}*He sets a stone in the lamp housing, and the shaft head lights up cold and steady, no flame at all*{/}"));
					await dialog.Msg(L("Surface is clear, and I've got 8 cold lamps to show for it. Third gallery's workable again — gas and all, which I'll admit still gives me pause."));
					await dialog.Msg(L("Take the pit purse. Forty years, and this mine's never once been robbed — I say that with more pride than the sum probably deserves, but it's mine to be proud of."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Workings are clear now. Eight stones off the Barkles and I can finally light that gallery properly."));
				}
				else
				{
					await dialog.Msg(L("Twenty-five Ritters first. Break their line at an end — never, ever the middle."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Third gallery's open and lit, pulling more ore in a week than we managed in a month. And every single shift ends the exact same way — somebody comes up top and says the pillar's warm again. I wish they'd stop saying that."));
			}
		});

		// =====================================================================
		// QUEST 1002: Crystals for the Cut Face
		// =====================================================================
		// Sorter Grasilda - needler crystals to grade the ore
		//---------------------------------------------------------------------
		AddNpc(147481, L("[Sorter] Grasilda"), "f_tableland_71", -1246, 546, 222, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1002);

			dialog.SetTitle(L("Grasilda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A sorter divides a barrow of rock into three heaps by feel alone, barely glancing at any of it*{/}"));
				await dialog.Msg(L("Don't mind the hands, love, they know these heaps better than my eyes do these days. New face. Good — an outside pair of hands is exactly what I've been short of."));
				await dialog.Msg(L("I grade what comes up out of that hole. Three heaps, by hand, eleven tons a week, done it since I was fourteen years old. What I've lost is my scratch set — the crystals I test hardness against. Bring me 6 off the Blue Cronewt Needlers, would you."));

				var response = await dialog.Select(L("Will you get me a scratch set?"),
					Option(L("I'll bring 6 crystals"), "help"),
					Option(L("Lost how?"), "info"),
					Option(L("Grade it by eye"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take them whole, mind. A chipped crystal scratches soft, and I'll grade a whole week's ore wrong off just one bad stone — my hands aren't that forgiving of mistakes."));
						await dialog.Msg(L("Needlers are down in the wet ground, south and west. They throw before they close, and dear, they do not miss."));
						break;

					case "info":
						await dialog.Msg(L("Third gallery. In the dark. When the gas came in and every last one of us came up a good deal faster than we went down."));
						await dialog.Msg(L("Alvinas has been down twice looking for them, bless him. Told him twice not to bother. Nothing in that gallery's worth a man, and that includes my scratch set."));
						break;

					case "leave":
						await dialog.Msg(L("By eye's how a mine ships eleven tons of second-grade to Roxona marked as first and loses its contract inside a season. Seen it happen to a finer pit than this one, love."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectCrystals", out var crysObj)) return;

				if (crysObj.Done)
				{
					await dialog.Msg(L("{#666666}*She runs each crystal down the same test block in turn, lining them up in order of bite, nodding to herself as each one lands*{/}"));
					await dialog.Msg(L("Six, and they step evenly — better set than the one I lost, if I'm honest, and I've been sorting from memory for three weeks getting away with murder."));
					await dialog.Msg(L("Take the sorter's share, love. And don't you tell Alvinas this set's better than the old one. He'll decide the gallery owes us something and go right back down after it, and I won't have that."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Six whole crystals, off the Needlers in the wet ground south and west. Whole, not chipped, remember."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Eleven tons graded clean this week, and the Roxona assay came back matching my heaps to the pound, thank you very much. Forty years, this pit's never had a bad assay — I'd like that carved somewhere, honestly."));
			}
		});

		// =====================================================================
		// QUEST 1003: The East Ground Is Full
		// =====================================================================
		// Powderman Juozas - the Tini between here and Ibre
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Powderman] Juozas"), "f_tableland_71", -1299, 477, 341, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1003);

			dialog.SetTitle(L("Juozas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A powderman counts cartridges into a satchel with barely-contained excitement, restarting the count every time he's interrupted, not that he minds*{/}"));
				await dialog.Msg(L("Ooh — a person! Don't mind me, lost count again, I always do, doesn't matter, you're WAY more interesting than counting sticks of powder! Well. Almost."));
				await dialog.Msg(L("Our powder comes up the east road from Ibre, hasn't for five weeks, and you want to know why? Blue Tini — hundreds of them, MORE than I've ever seen on this whole plateau — just sitting on the road like they own it. Kill 30 and I get my supply line back, and then, oh, then we can really start blasting."));

				var response = await dialog.Select(L("Will you open the east road?"),
					Option(L("I'll clear the east road"), "help"),
					Option(L("Still arriving from where?"), "info"),
					Option(L("Make your own powder"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Individually they're nothing! Nothing at all! But there's a HUNDRED of them, so don't stand still and don't let any one fight drag on — anything that takes you twenty seconds picks up six more Tini by the end of it."));
						await dialog.Msg(L("Work the road itself, not the ground either side. I need one line open, not a tidy little cleared plateau — I've got things to blow up, priorities, you understand."));
						break;

					case "info":
						await dialog.Msg(L("Not FROM, TOWARD! Isn't that marvelous and horrible! They're not coming onto the road from somewhere — they're coming from the west of this whole map and just... stopping there."));
						await dialog.Msg(L("Everything on Mandara's moving east and northeast, every last creature. The Tini got as far as the road and stopped, because that's where the Ibre folk start shoving back. Fascinating, really, in the way that keeps me up at night!"));
						break;

					case "leave":
						await dialog.Msg(L("With saltpetre I'd have to dig for, in a gallery that's got GAS in it, using an open FLAME. Oh, no, absolutely not, I quite like having both my eyebrows."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTini", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks a hundred paces of the east road and comes back holding a cartridge aloft like a trophy*{/}"));
					await dialog.Msg(L("ROAD'S OPEN! Ibre can get a cart to us in two days, and I can finally shot-fire the third gallery properly instead of picking it out by hand like some kind of animal!"));
					await dialog.Msg(L("Take the powder allowance. Drawn it five weeks running and blown up NOTHING — for a powderman, my friend, that is a genuine form of suffering."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("The east road, just the road! Thirty of them! Don't let any single fight run long, keep moving!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cart came through, and a trailhand off Ibre rode along with it — lad called Domas, counting everything, very serious about it. Asked me which way things were moving, I said east, and he went dead quiet and wrote it down. Odd fellow. I liked him."));
			}
		});

		// =====================================================================
		// QUEST 1004: Four Bearings on One Thing
		// =====================================================================
		// Wizard Tulis - sighting the pillar from a distance
		//---------------------------------------------------------------------
		AddNpc(153148, L("[Wizard] Tulis"), "f_tableland_71", -1143, 513, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1004);

			dialog.SetTitle(L("Tulis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A wizard draws a plan of the mine, leaving a blank oval in the middle with nothing written inside it — he does not look up when you approach*{/}"));
				await dialog.Msg(L("Ah. Legs that still work, and evidently no particular reason to avoid this stretch of ground. You'll do. Come and look at this plan with me, if you would."));
				await dialog.Msg(L("This mine has three galleries. All three bend. They bend around the same object, and no plan drawn in forty years has recorded it. I have four sighting orbs placed across the plateau, and I require bearings from each — I cannot approach the pillar myself, and I would strongly advise that you not attempt it either."));

				var response = await dialog.Select(L("Will you take the 4 bearings?"),
					Option(L("I'll take all 4 bearings"), "help"),
					Option(L("Why can't you go near it?"), "info"),
					Option(L("Just dig it out"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663122, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the detector. Set it on the orb, allow it to settle, and read it without touching it. Touch it while it settles and you will record my hand's number, not the pillar's — an elementary but common error."));
						await dialog.Msg(L("The four orbs are placed far apart, deliberately. Two bearings give you a line. Four bearings give you an argument, and an argument is precisely what I require."));
						break;

					case "info":
						await dialog.Msg(L("I have approached to within forty paces of it, twice. On both occasions I walked away and could not, afterward, account for the walk. I do not recall deciding to leave. I do not recall leaving."));
						await dialog.Msg(L("A man who cannot remember leaving a place should not, by any reasonable standard, return to it. So I remain here, at nine hundred paces, and I do arithmetic instead. It is safer arithmetic."));
						break;

					case "leave":
						await dialog.Msg(L("Forty years of miners have worked within six feet of that object, and every one of them turned the gallery rather than continue through it. Not one recorded why. I invite you to consider what manner of object gets quietly routed around for forty years without comment."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var bearings = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_71.Quest1004.Bearings", 0);

				if (bearings >= 4)
				{
					await dialog.Msg(L("{#666666}*He plots the 4 bearings on the plan; the lines do not converge at the oval. They converge well past it*{/}"));
					await dialog.Msg(L("The pillar is not the object of interest. The pillar is pointed AT the object of interest. Four bearings, four separate stations, and every single one passes through the pillar and continues on the identical heading beyond it."));
					await dialog.Msg(L("North by northwest. Entirely off this plateau, over Ibre, toward Roxona. Take this to Zamelan. He has waited four months for someone to bring him precisely this sentence."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Four orbs, well separated. Set the detector, let it settle, read it without touching it. {0} of 4 recorded so far.", bearings));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have redrawn the plan with the heading marked and the oval still blank, since I remain ignorant of what the pillar actually IS. I now know what it is aimed at, which is, I assure you, a substantially worse category of knowledge to possess."));
			}
		});

		// =====================================================================
		// SIGHTING ORBS
		// =====================================================================
		// For Quest 1004 - Four Bearings on One Thing
		// =====================================================================

		void AddSightingOrb(int orbNumber, string orbName, string bearing, int x, int z, int direction)
		{
			AddNpc(151022, L(orbName), "f_tableland_71", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_tableland_71", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A sighting orb on a low tripod, turned very slightly away from where it was set*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_tableland_71.Quest1004.Orb{orbNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This bearing is already written on the detector's slate*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Letting the detector settle..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Bearing interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);

				var bearings = character.Variables.Perm.GetInt("Laima.Quests.f_tableland_71.Quest1004.Bearings", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_tableland_71.Quest1004.Bearings", bearings);

				character.ServerMessage(L(bearing));
				character.ServerMessage(LF("Bearings taken: {0}/4", bearings));

				if (bearings >= 4)
					character.ServerMessage(L("{#FFD700}All 4 bearings taken. Return to Wizard Tulis.{/}"));
			});
		}

		AddSightingOrb(1, "Camp Sighting Orb", "The needle settles across the pillar and keeps going. It does not stop at the pillar.", -1257, 486, 0);
		AddSightingOrb(2, "Ridge Sighting Orb", "Same heading. The pillar is on the line but the line does not end there.", -979, 398, 0);
		AddSightingOrb(3, "South Sighting Orb", "Same heading again, from 1,600 paces away and 90 degrees round.", -270, -1157, 0);
		AddSightingOrb(4, "East Road Sighting Orb", "North by northwest, through the pillar, out over Ibre. Four stations, one line.", 1703, 619, 0);

		// =====================================================================
		// The Unidentified Pillar - atmosphere in the low ground
		//---------------------------------------------------------------------
		AddNpc(155051, L("Unidentified Pillar"), "f_tableland_71", -876, -834, 287, async dialog =>
		{
			await dialog.Msg(L("{#666666}*A pillar of worked stone standing in the low ground, warm to within a pace of it, with no seam and no base you can find*{/}"));
			await dialog.Msg(L("{#666666}*Three mine galleries pass within six feet of it underground and all three of them bend. Forty years of workings and not one plan has it drawn*{/}"));
			await dialog.Msg(L("{#666666}*You realise you have started walking away from it and cannot say when you decided to*{/}"));
		});

		// =====================================================================
		// QUEST 1005: Everything Is Walking East
		// =====================================================================
		// Zamelan - four months of counting the wrong direction
		//---------------------------------------------------------------------
		AddNpc(155034, L("[Pilgrim] Zamelan"), "f_tableland_71", -1137, 704, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_tableland_71", 1005);

			dialog.SetTitle(L("Zamelan"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_tableland_71", 1004)))
				{
					await dialog.Msg(L("{#666666}*He glances at you, then back at the notch he's counting off in his walking stick*{/}"));
					await dialog.Msg(L("You're not one of the miners. Interesting, but it'll have to wait — Tulis has 4 orbs out and no bearings on any of them. Go and take them for him. I have waited 4 months and I can wait an afternoon."));
					return;
				}

				await dialog.Msg(L("{#666666}*He stands up slowly from the notch-marked stick, like the number he's about to hear is the one he's been counting toward*{/}"));
				await dialog.Msg(L("North by northwest. Through the pillar and out the far side. Yes."));
				await dialog.Msg(L("I have sat on this plateau 4 months counting what walks across it, and everything walks east - Tini, Shamans, Barkles, all of it, away from that pillar. Everything except the Hohen Ritters. Kill 25 Ritters and take the 2 holding the ground closest to it."));

				var response = await dialog.Select(L("Will you take the ground by the pillar?"),
					Option(L("I'll take the pair"), "help"),
					Option(L("Why not the Ritters?"), "info"),
					Option(L("Leave the pillar alone"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(663126, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take Tulis's scroll and break it before you close. It will not protect you. It will make you remember leaving, which is the only thing he has ever managed to build against that pillar."));
						await dialog.Msg(L("The line first, then the pair. And when you have finished, walk away deliberately. Count your own steps out loud."));
						break;

					case "info":
						await dialog.Msg(L("Because they are the only things on Mandara moving toward it. 4 months of everything alive going one way and one kind of thing going the other way."));
						await dialog.Msg(L("I am a pilgrim. I have walked to 60 holy places and I have never once seen a creature choose a direction the way those do. They are not drawn to it. They are posted on it."));
						break;

					case "leave":
						await dialog.Msg(L("The pillar is aimed at Roxona and there is a road under it that walks 40 people at a time out of the city and up this shelf. I have stopped believing those are 2 separate facts."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakTheLine", out var lineObj)) return;
				if (!quest.TryGetProgress("takeThePosted", out var pairObj)) return;

				if (lineObj.Done && pairObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens, and then asks you to say the last part again, and then writes it into a book with 4 months of tally marks in it*{/}"));
					await dialog.Msg(L("Posted. You said they were standing in a line facing outward with their backs to it. That is a guard, and a guard is somebody's."));
					await dialog.Msg(L("Take this - I have carried it to 60 holy places and it has never once been the useful thing in my pack. Go on to Sventimas. The priests there keep a figurine they call cursed and a device they call a monitor, and after today I would very much like somebody to go and look at what those two things are aimed at."));

					character.Quests.Complete(questId);
				}
				else if (lineObj.Done)
				{
					await dialog.Msg(L("Line's broken. The 2 nearest the pillar have not moved and they are not going to. They have been stood there 4 months."));
				}
				else
				{
					await dialog.Msg(L("25 Ritters first. Do not go near the posted pair with a line of them still on the ground behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Nothing has replaced them. 9 days and the ground by the pillar is empty, which after 4 months of counting is the loudest thing this plateau has ever done."));
				await dialog.Msg(L("Tulis has stopped doing arithmetic and started writing letters. He has sent 3 and had no answer, which is exactly what everybody on this shelf has had."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Third Gallery
//-----------------------------------------------------------------------------

public class TheThirdGalleryQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1001);
		SetName(L("The Third Gallery"));
		SetType(QuestType.Sub);
		SetDescription(L("Five men work a mine that has run 40 years, and the Blue Hohen Ritters have come up into the workings. Alvinas also needs 8 magic stones for cold lamps, because the third gallery has gas in it that was not there last year."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pit-Boss] Alvinas"), "f_tableland_71");

		AddObjective("killRitters", L("Kill Blue Hohen Ritters in the workings"),
			new KillObjective(25, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("collectStones", L("Take magic stones from Blue Hohen Barkles"),
			new CollectItemObjective(663124, 8));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion

		AddDrop(663124, 0.35f, MonsterId.Hohen_Barkle_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663124, character.Inventory.CountItem(663124), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663124, character.Inventory.CountItem(663124), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Crystals for the Cut Face
//-----------------------------------------------------------------------------

public class CrystalsForTheCutFaceQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1002);
		SetName(L("Crystals for the Cut Face"));
		SetType(QuestType.Sub);
		SetDescription(L("Grasilda grades 11 tons a week by hand and lost her scratch set in the third gallery when the gas came in. The Blue Cronewt Needlers carry crystals that will make a better one."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sorter] Grasilda"), "f_tableland_71");

		AddObjective("collectCrystals", L("Take whole crystals from Blue Cronewt Needlers"),
			new CollectItemObjective(663123, 6));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion

		AddDrop(663123, 0.35f, MonsterId.Cronewt_Bow_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663123, character.Inventory.CountItem(663123), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The East Ground Is Full
//-----------------------------------------------------------------------------

public class TheEastGroundIsFullQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1003);
		SetName(L("The East Ground Is Full"));
		SetType(QuestType.Sub);
		SetDescription(L("Mandara's powder comes up the east road from Ibre and has not come for 5 weeks. Blue Tini are standing on the road in numbers Juozas has never seen, and they are still arriving from the west."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Powderman] Juozas"), "f_tableland_71");

		AddObjective("killTini", L("Kill Blue Tini standing on the east road"),
			new KillObjective(30, new[] { MonsterId.Tiny_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
	}
}

// Quest 1004 CLASS: Four Bearings on One Thing
//-----------------------------------------------------------------------------

public class FourBearingsOnOneThingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1004);
		SetName(L("Four Bearings on One Thing"));
		SetType(QuestType.Sub);
		SetDescription(L("All 3 mine galleries bend around the same object and no plan in 40 years has drawn it. Tulis cannot go near it himself. Take a bearing at each of his 4 sighting orbs."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Wizard] Tulis"), "f_tableland_71");

		AddObjective("takeBearings", L("Take a bearing at each of the 4 sighting orbs"),
			new VariableCheckObjective("Laima.Quests.f_tableland_71.Quest1004.Bearings", 4, true));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663122, character.Inventory.CountItem(663122), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1004.Bearings");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1004.Orb{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663122, character.Inventory.CountItem(663122), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_tableland_71.Quest1004.Bearings");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_tableland_71.Quest1004.Orb{i}");
	}
}

// Quest 1005 CLASS: Everything Is Walking East
//-----------------------------------------------------------------------------

public class EverythingIsWalkingEastQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_tableland_71", 1005);
		SetName(L("Everything Is Walking East"));
		SetType(QuestType.Sub);
		SetDescription(L("Four months of counting says everything alive on Mandara walks away from the pillar, except the Blue Hohen Ritters, who stand facing outward with their backs to it. Take the two closest to it."));
		SetLocation("f_tableland_71");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Pilgrim] Zamelan"), "f_tableland_71");

		AddPrerequisite(new CompletedPrerequisite("f_tableland_71", 1004));

		AddObjective("breakTheLine", L("Kill Blue Hohen Ritters holding the low ground"),
			new KillObjective(25, new[] { MonsterId.Hohen_Ritter_Purple }));

		AddObjective("takeThePosted", L("Take the pair posted closest to the pillar"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Hohen_Ritter_Purple, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Hohen_Barkle_Blue, 3),
				},
				resetIdent: "breakTheLine",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(583124, 1)); // Graduation Gift
		AddReward(new ItemReward(640086, 2)); // Lv6 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640013, 1)); // Large Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(663126, character.Inventory.CountItem(663126), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(663126, character.Inventory.CountItem(663126), InventoryItemRemoveMsg.Destroyed);
	}
}
