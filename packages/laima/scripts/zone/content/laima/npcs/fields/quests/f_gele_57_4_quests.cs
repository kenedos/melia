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
		// =====================================================================
		// QUEST 1001: The Garden's Dead Walk
		// =====================================================================
		// Chapel-Warden Vincas - Rodelin undead wandering from the crypts
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Chapel-Warden] Vincas"), "f_gele_57_4", 317, 867, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1001);

			dialog.SetTitle(L("Vincas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A lean warden in a chapel surcoat braces a cracked bell-rope, eyes fixed on the memorial paths*{/}"));
				await dialog.Msg(L("The crypt doors broke last month. Rodelin - girls in grave-cloth - keep drifting up through the garden paths. They don't mean to. They just don't remember where to stop."));

				var response = await dialog.Select(L("Eighteen put back to rest. Don't strike them proud - strike them clean. They were chapel-wards in life; they deserve quiet in death. Will you help?"),
					Option(L("I'll put eighteen to rest"), "help"),
					Option(L("Why are they walking now?"), "info"),
					Option(L("Undead aren't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods, and the tension in his shoulders eases by a fraction*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Speak their name if you know it. If not, speak any chapel-word - 'rest,' 'return,' 'release.' The words matter more than the blade."));
						break;

					case "info":
						await dialog.Msg(L("A crypt-seal failed. Chapel wardmages are slow to re-bless - the Fedimian wardmage stopped answering letters three weeks ago."));
						await dialog.Msg(L("Until the seal is redrawn, the Rodelin will keep drifting. We put them back one at a time, like leaves in autumn."));
						break;

					case "leave":
						await dialog.Msg(L("Then walk the west path, traveler. The undead cluster east. West keeps you clear."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRodelin", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to the count, then bows his head a fraction*{/}"));
					await dialog.Msg(L("Eighteen returned to rest. The memorial paths are quiet for a day, perhaps two."));
					await dialog.Msg(L("Take this - chapel coin, a warden's share. And tell no one I paid you; the Fedimian accounts don't cover mercy-strokes."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("East memorial paths. Eighteen Rodelin. Speak a chapel-word with each strike."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Rodelin still drift, but slower. The seal will hold the rest - when Fedimian remembers us."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Reliquary-List
		// =====================================================================
		// Steward Algis - Ridge-Watch needs the list of lost relics
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Chapel-Steward] Algis"), "f_gele_57_4", 1210, 2016, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);

			dialog.SetTitle(L("Algis"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A thin steward counts entries on a long parchment by lamplight, smudging ink where his hand shakes*{/}"));
				await dialog.Msg(L("Seventeen reliquaries unaccounted for after the crypt-break. Half may have been scattered down the memorial paths; half may have walked off in Rodelin hands."));

				var response = await dialog.Select(L("Ridge-Watch Emilis keeps records on Akmens Ridge - pilgrims pass his watchpost carrying ridge-kept relics. If any of mine surface there, his list catches them first. Carry my copy to him. Bring back whatever he can match."),
					Option(L("I'll carry the list to Emilis"), "help"),
					Option(L("Why not send a chapel runner?"), "info"),
					Option(L("That's a long road for paper"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He rolls the parchment twice, slides it into a leather tube, seals the cap with chapel wax*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("He'll recognize the chapel wax. Don't crack it before he does - the seal's half the authority."));
						break;

					case "info":
						await dialog.Msg(L("Our chapel runners are children. The Panto archers on the ridge side pick off anyone under a head taller than their bowstave. I won't send children at bow-height."));
						await dialog.Msg(L("You're a traveler. Travelers carry their own risk. I'm not above buying a stranger's courage when my own runners can't reach."));
						break;

					case "leave":
						await dialog.Msg(L("Paper is what the chapel has. Stone takes too long; blood wastes too much. Paper walks further than both."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverList", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He reads Emilis's reply line by line, making quick marks with a charcoal stub*{/}"));
					await dialog.Msg(L("Four matched at the ridge. Three more passed through toward Fedimian - he'll forward those. The other ten are still out there, but four back is four more than yesterday."));
					await dialog.Msg(L("Take this. A steward's thanks, by weight and in coin. And a chapel-charm; it won't fix the crypt, but it keeps Rodelin hands off your cloak."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Akmens Ridge warp, north garden edge. Emilis keeps a ridge-watch shelter. The chapel wax must be unbroken when you hand it over."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four reliquaries recovered. I've sent word to the Fedimian magistrates; the rest of the list is now their ledger."));
			}
		});

		// =====================================================================
		// RIDGE-WATCH EMILIS (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20117, L("[Ridge-Watch] Emilis"), "f_gele_57_4", -949, 2207, 89, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1002);

			dialog.SetTitle(L("Emilis"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_gele_57_4.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A wind-weathered watchman breaks the chapel wax with his thumbnail and unrolls the list against his knee*{/}"));
					await dialog.Msg(L("Algis still writes like a clerk. Good. Let me see..."));
					await dialog.Msg(L("{#666666}*He scans the entries, marking four with a quick tick and three with a circle*{/}"));
					await dialog.Msg(L("Four matched on my shelf. Three passed through last week bound for Fedimian - I'll send a rider. The remaining ten are out in the wind."));
					await dialog.Msg(L("{#666666}*He re-rolls the list, adds a short note, seals it with ridge-pitch instead of wax*{/}"));
					await dialog.Msg(L("Pitch seal for the return. Chapel wax won't hold in the cold passes. Algis will know the mark."));

					character.Variables.Perm.Set("Laima.Quests.f_gele_57_4.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Emilis's reply received. Return to Chapel-Steward Algis.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the pitch-sealed reply back to Algis. He'll recognize the mark."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Fedimian rider went through on schedule. Three more reliquaries will surface in the capital within the fortnight, assuming the passes stay open."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The watchman tightens a strap on a weathered log-book, eyes on the ridge trail*{/}"));
				await dialog.Msg(L("Travelers pass; I log what they carry. Chapel business, if you have any."));
			}
		});

		// =====================================================================
		// QUEST 1003: Seedmia Saplings
		// =====================================================================
		// Caretaker Neringa - Saving Seedmia saplings before the rot reaches them
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Garden-Caretaker] Neringa"), "f_gele_57_4", -1102, 679, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1003);

			dialog.SetTitle(L("Neringa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A stout caretaker cradles a clay pot of damp loam, her apron pockets bulging with transplant-twine*{/}"));
				await dialog.Msg(L("Seedmia look like little girls when they root, but they're plants - not daughters. Still. I can't watch the crypt-rot reach them without moving them first."));

				var response = await dialog.Select(L("Eight saplings is all my blessed plot can hold. Gather them gently - Seedmia don't fight back unless you rip them up by the crown. Approach from the base, same as every rooting plant. Will you gather for me?"),
					Option(L("I'll gather eight saplings"), "help"),
					Option(L("They look like girls?"), "info"),
					Option(L("Plants can replant themselves"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She sets the pot down and tears a strip of linen from her apron*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The largest cluster is northwest, near the old arboretum stone. Smaller clusters scatter toward the memorial paths."));
						await dialog.Msg(L("If a sapling shivers, wait. They settle in a count of six. Then lift from the base."));
						break;

					case "info":
						await dialog.Msg(L("Petals arrange like faces. Stems like little skirts. The chapel taught they're a cousin-species to the Fragolin berry-girls down south, but quieter - and less sisterly."));
						await dialog.Msg(L("They don't cry when you move them. They just... hold still, and grow again where you put them. I find that restful, most days."));
						break;

					case "leave":
						await dialog.Msg(L("Not fast enough. The rot moves one memorial path per week. I move them one pot at a time."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var saplingCount = character.Inventory.CountItem(650720);

				if (saplingCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She unwraps each linen-bundle and inspects the roots with a gardener's patience*{/}"));
					await dialog.Msg(L("Roots whole. No tearing. No panic in the stem-lines. You gathered properly."));
					await dialog.Msg(L("{#666666}*She tucks the saplings into the clay pot, one by one, pressing damp loam around each*{/}"));
					await dialog.Msg(L("A caretaker's coin. Small, but honest. And a pot of tea-leaves from my own herb-bench; steep it slow."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Lift from the base. Wrap the roots in linen. Eight saplings."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The blessed plot holds all eight now. Their petals face east - the same direction every morning. I find that comforting."));
			}
		});

		// =====================================================================
		// QUEST 1004: Which Graves Were Touched
		// =====================================================================
		// Memorial-Keeper Ieva - Tracing which plots were disturbed first
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Memorial-Keeper] Ieva"), "f_gele_57_4", -489, 1995, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_gele_57_4", 1004);

			dialog.SetTitle(L("Ieva"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young keeper in a charcoal-smudged apron holds a graphite-stick over a blank rubbing-sheet*{/}"));
				await dialog.Msg(L("The crypt-break opened four plots in the garden, not just the main seal. But we don't know which opened first, or how. If one broke inward, the rest followed its pattern. If one broke outward, we're missing something else."));

				var response = await dialog.Select(L("Four memorial plots were touched. Check each. Don't disturb the stones - just describe what you see. Crack patterns tell the story. Will you walk them for me?"),
					Option(L("I'll walk the four plots"), "help"),
					Option(L("Why does the direction matter?"), "info"),
					Option(L("Grave-business isn't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She tears a fresh page from her rubbing-book and marks four points*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Inward cracks mean something pushed in from outside. Outward cracks mean something pushed out from within."));
						await dialog.Msg(L("Pushed out from within is the one that ruins my sleep."));
						break;

					case "info":
						await dialog.Msg(L("Inward cracks - robbers, a landslide, the crypt-beam settling. Outward cracks - whatever was inside got out."));
						await dialog.Msg(L("If three plots cracked inward and one outward, we have a simple answer: one Rodelin came up through that outward plot and woke the rest. That's manageable."));
						await dialog.Msg(L("If all four cracked outward, we have four things walking that we weren't counting on. That's not manageable."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself next week, if the crypt holds that long."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var plotsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked", 0);

				if (plotsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*She listens, marking each plot with a quick stroke, and her graphite slows at the last entry*{/}"));
					await dialog.Msg(L("Plot One: inward cracks, fresh. Plot Two: inward, older. Plot Three: outward, fresh. Plot Four: outward, older - and a child's footprint in the dust, facing the chapel door."));
					await dialog.Msg(L("{#666666}*She sets the graphite down*{/}"));
					await dialog.Msg(L("Two outward. Plot Four went first, maybe a season ago - nobody noticed. Plot Three followed recently. We've been hunting one waking; there were two."));
					await dialog.Msg(L("Take this - a memorial-keeper's stipend, and my thanks. I'll draft a warning for the chapel-watch tonight."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four plots. Inward or outward cracks. Fresh or weathered. Walk them, report what you see."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The chapel-watch posted a second warden by Plot Four. The child-footprint has not reappeared. But we watch for it."));
			}
		});

		// =====================================================================
		// MEMORIAL PLOTS
		// =====================================================================
		// For Quest 1004 - Which Graves Were Touched
		// =====================================================================

		void AddMemorialPlot(int plotNumber, string plotName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(plotName), "f_gele_57_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_gele_57_4", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weathered memorial stone in the chapel garden. The crypt beneath has not been opened recently*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_gele_57_4.Quest1004.Plot{plotNumber}";
				var checkedPlot = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedPlot)
				{
					await dialog.Msg(L("{#666666}*You have already read this plot's cracks*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the crack-pattern..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var plotsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked", plotsChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Plots read: {0}/4", plotsChecked + 1));

					if (plotsChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All plots read! Return to Memorial-Keeper Ieva.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddMemorialPlot(1, "Guard-Captain Vytas", "Plot One: cracks run inward toward the center. Fresh - stone-dust still pale at the fracture edges.", 341, 633, 0);
		AddMemorialPlot(2, "Deaconess Ona", "Plot Two: cracks run inward. Older - moss has begun closing the fractures at the rim.", 524, 1046, 90);
		AddMemorialPlot(3, "War-Deacon Balius", "Plot Three: cracks run outward. Fresh. The capstone leans away from the plot's heart.", 696, 1148, 180);
		AddMemorialPlot(4, "Unnamed Chaplain", "Plot Four: cracks run outward. Older than the others. A small footprint in the dust - a child's size - faces the chapel door.", 883, 1002, 270);
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
		SetName("The Garden's Dead Walk");
		SetType(QuestType.Sub);
		SetDescription("Chapel-Warden Vincas needs eighteen Rodelin undead put back to rest before the chapel memorial paths become unwalkable.");
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Chapel-Warden] Vincas", "f_gele_57_4");

		AddObjective("killRodelin", "Defeat Rodelin undead",
			new KillObjective(18, new[] { MonsterId.Zombiegirl2_Brown }));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3));  // Normal HP Potion
		AddReward(new ItemReward(640006, 2));  // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion
	}
}

// Quest 1002 CLASS: The Reliquary-List
//-----------------------------------------------------------------------------

public class TheReliquaryListQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1002);
		SetName("The Reliquary-List");
		SetType(QuestType.Sub);
		SetDescription("Chapel-Steward Algis's list of missing reliquaries must reach Ridge-Watch Emilis at the Akmens Ridge warp.");
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Chapel-Steward] Algis", "f_gele_57_4");

		AddObjective("deliverList", "Deliver the list to Emilis and return",
			new VariableCheckObjective("Laima.Quests.f_gele_57_4.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3));  // Normal HP Potion
		AddReward(new ItemReward(640006, 2));  // Normal SP Potion
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

// Quest 1003 CLASS: Seedmia Saplings
//-----------------------------------------------------------------------------

public class SeedmiaSaplingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1003);
		SetName("Seedmia Saplings");
		SetType(QuestType.Sub);
		SetDescription("Garden-Caretaker Neringa needs eight Seedmia saplings gently transplanted before the crypt-rot reaches them.");
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Garden-Caretaker] Neringa", "f_gele_57_4");

		AddObjective("collectSaplings", "Gather Seedmia saplings from the garden",
			new CollectItemObjective(650720, 8));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3));  // Normal HP Potion
		AddReward(new ItemReward(640006, 2));  // Normal SP Potion
		AddReward(new ItemReward(640011, 1));  // Recovery Potion

		AddDrop(650720, 0.50f, MonsterId.Seedmia);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650720,
			character.Inventory.CountItem(650720),
			InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650720,
			character.Inventory.CountItem(650720),
			InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Which Graves Were Touched
//-----------------------------------------------------------------------------

public class WhichGravesWereTouchedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_gele_57_4", 1004);
		SetName("Which Graves Were Touched");
		SetType(QuestType.Sub);
		SetDescription("Memorial-Keeper Ieva has asked you to read the crack-patterns on the four disturbed memorial plots and determine which broke outward from within.");
		SetLocation("f_gele_57_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Memorial-Keeper] Ieva", "f_gele_57_4");

		AddObjective("readPlots", "Read the four memorial plot crack-patterns",
			new VariableCheckObjective("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked", 4, true));

		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));  // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3));  // Normal HP Potion
		AddReward(new ItemReward(640006, 2));  // Normal SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1004.Plot{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_gele_57_4.Quest1004.PlotsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_gele_57_4.Quest1004.Plot{i}");
		}
	}
}
