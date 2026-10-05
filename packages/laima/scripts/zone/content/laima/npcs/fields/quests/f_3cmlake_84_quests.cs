//--- Melia Script ----------------------------------------------------------
// Absenta Reservoir Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs in Absenta Reservoir — brackish shrine-water that overflows
// from the Pelke shrine pond, fouled by pearlite swarms and ailing crystals.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

public class F3Cmlake84QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Shrine Marker Vigil — Mindaugas
		//-------------------------------------------------------------------------
		AddNpc(20108, L("[Reservoir-Warden] Mindaugas"), "f_3cmlake_84", -65, -1337, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1001);

			dialog.SetTitle(L("Mindaugas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grizzled warden in an oilskin coat keeps an eye on the water.*{/}"));
				await dialog.Msg(L("There are 4 Pelke shrine markers around the reservoir. I need someone to activate each one so the priests can read the water and know where to perform their cleansing rites."));
				await dialog.Msg(L("Be warned — when you activate a marker, the sound it makes draws the Pearlites in the water. They'll attack you while the marker reads. You'll have to fight them off until it finishes."));

				var response = await dialog.Select(L("Will you activate the markers?"),
					Option(L("I'll do it."), "help"),
					Option(L("Why does it attract them?"), "info"),
					Option(L("I've no time for it."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. Activate each marker, fight off whatever it draws, and let the reading finish. Four markers, four fights."));
						break;

					case "info":
						await dialog.Msg(L("The markers pick up whatever's in the water. When you activate one, the sound spreads through the reservoir, and the Pearlites go for it because the water's so fouled right now."));
						break;

					case "leave":
						await dialog.Msg(L("Come back if you change your mind. I can't approach the markers myself."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("touchMarkers", out var vObj)) return;

				var awakened = character.Variables.Perm.GetInt("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened", 0);

				if (vObj.Done || awakened >= 4)
				{
					await dialog.Msg(L("All four markers done. Three came up deep purple, one was barely gold. The priests will know where to start. My wardens couldn't have managed that in a week."));
					character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker1");
					character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker2");
					character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker3");
					character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker4");
					character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened");
					quest.CompleteObjectives();
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Markers activated: {0} of 4. Save the ones near deep water for last — those will draw the worst attacks.", awakened));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The priests came in right after you finished. They started with the worst stretches. Half the shore reads clean again now."));
			}
		});

		// Shrine Marker helper (Q1001)
		//-------------------------------------------------------------------------
		void AddShrineMarker(int markerNumber, int x, int z, int direction, string flavorReading)
		{
			AddNpc(152008, L("Pelke Shrine Marker"), "f_3cmlake_84", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_3cmlake_84", 1001);
				var variableKey = $"Laima.Quests.f_3cmlake_84.Quest1001.Marker{markerNumber}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A weather-worn Pelke shrine marker. It is silent and does not respond.*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This marker has already been activated.*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 600, 500);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The marker's resonance has drawn enemies in!{{/}}"));

				var result = await character.TimeActions.StartAsync(L("Activating the marker..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					await dialog.Msg(flavorReading);

					character.Variables.Perm.Set(variableKey, true);
					var newCount = character.Variables.Perm.GetInt("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened", 0) + 1;
					character.Variables.Perm.Set("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened", newCount);

					character.ServerMessage(LF("Markers activated: {0}/4", newCount));
					if (newCount >= 4)
						character.ServerMessage(L("{#FFD700}All four markers activated. Return to Mindaugas at the south shore.{/}"));
				}
				else
				{
					character.ServerMessage(L("Activation interrupted."));
				}
			});
		}

		AddShrineMarker(1, -413, -946, 0,
			L("{#666666}*The marker glows deep purple — the western shelf is heavily fouled.*{/}"));
		AddShrineMarker(2, -59, -940, 315,
			L("{#666666}*The marker flickers between gold and purple — the eastern bay is partly spoiled.*{/}"));
		AddShrineMarker(3, -345, -1298, 30,
			L("{#666666}*The marker glows faintly gold — the southern reach is the cleanest stretch left.*{/}"));
		AddShrineMarker(4, -369, -1088, 0,
			L("{#666666}*The marker glows almost black — the central channel is the worst affected.*{/}"));

		// Quest 2: Bile Phials — Ruta
		//-------------------------------------------------------------------------
		AddNpc(20107, L("[Shrine-Attendant] Ruta"), "f_3cmlake_84", -207, 211, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1002);

			dialog.SetTitle(L("Ruta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young attendant in salt-stained robes is scrubbing an offering bowl. The water in it has turned dark.*{/}"));
				await dialog.Msg(L("Third bowl this morning. The Blue Slimes are leaking into the spring water and ruining it before it reaches us."));
				await dialog.Msg(L("There's an old purification rite that uses the slimes' own bile to neutralize the contamination. I need 6 fresh phials to perform it. Please kill 25 Blue Slimes and collect the phials from them."));

				var response = await dialog.Select(L("Will you bring me the phials?"),
					Option(L("I'll bring them."), "help"),
					Option(L("Why use bile?"), "info"),
					Option(L("Find someone else."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. The bile must be fresh — it eats through leather quickly, so don't delay returning."));
						break;

					case "info":
						await dialog.Msg(L("It's an old shrine technique — using a substance to neutralize itself. Sealed and blessed, drawn bile cancels out the contamination the loose bile leaves behind. It hasn't failed us yet."));
						break;

					case "leave":
						await dialog.Msg(L("The water will keep souring either way. Come back when you have time."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinSlimes", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPhials", out var pObj)) return;

				if (killObj.Done && pObj.Done)
				{
					await dialog.Msg(L("Six sealed phials. I'll perform the rite at dusk and the offering bowls will be safe by morning. Thank you."));
					character.Inventory.Remove(650095, character.Inventory.CountItem(650095), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting Blue Slimes. The phials only stay good if drawn from a fresh kill."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The purification rite has held for three nights. Pilgrims are drinking from the bowls again without worry."));
			}
		});

		// Quest 3: Sawpent Scales — Gintare
		//-------------------------------------------------------------------------
		AddNpc(20146, L("[Herbalist] Gintare"), "f_3cmlake_84", -310, 1665, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1003);

			dialog.SetTitle(L("Gintare"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An herbalist sorts roots and salt-jars on a folding table. A row of empty antivenom flasks waits beside her, each one already labeled.*{/}"));
				await dialog.Msg(L("Three flasks short again. The Sawpents are everywhere along the shrine paths and they always strike twice — and every patrol that comes through here leaves with fewer flasks than they need."));
				await dialog.Msg(L("I can make antivenom for the supply carts, but I need fresh scales — the venom comes from a film between scale and skin, and it dries up fast. Please kill 20 Sawpents and bring me 7 damp scales."));

				var response = await dialog.Select(L("Will you bring the scales?"),
					Option(L("I'll bring them."), "help"),
					Option(L("Why fresh scales?"), "info"),
					Option(L("Not today."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Thank you. Strip the scales right after killing — and watch out for the second strike. Sawpents always lash twice."));
						break;

					case "info":
						await dialog.Msg(L("The venom is in a thin film under the scales, and that film dries up almost immediately. Fresh scales make strong antivenom. Dry ones are useless."));
						break;

					case "leave":
						await dialog.Msg(L("Then the flasks stay empty and the next patrol goes out short. Come back when you can."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinSawpents", out var killObj)) return;
				if (!quest.TryGetProgress("gatherScales", out var sObj)) return;

				if (killObj.Done && sObj.Done)
				{
					await dialog.Msg(L("Seven scales, all still damp. I'll prepare the antivenom tonight and have it on the supply carts by sunrise. Thank you."));
					character.Inventory.Remove(666028, character.Inventory.CountItem(666028), InventoryItemRemoveMsg.Given);
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep hunting Sawpents. Remember — fresh, damp scales only."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Every supply cart has antivenom now. Two children made it home this week who would have lost a leg otherwise."));
			}
		});

		// Quest 4: Crystal Slivers — Saule
		//-------------------------------------------------------------------------
		AddNpc(20116, L("[Crystal-Warder] Saule"), "f_3cmlake_84", -227, 372, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1004);

			dialog.SetTitle(L("Saule"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A warder is reading a half-buried Tree Root Crystal. The thing is humming wrong.*{/}"));
				await dialog.Msg(L("All the crystals around the reservoir have gone bad. They've absorbed the contaminated water and trapped the mana inside, where Pelke can't reclaim it."));
				await dialog.Msg(L("The only way to fix it is to break the crystals open and release the trapped mana. Please break 8 of the worst ones on the deep shelf."));

				var response = await dialog.Select(L("Will you break the bad crystals?"),
					Option(L("I'll break them."), "help"),
					Option(L("Why can't you do it?"), "info"),
					Option(L("Pass."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Wear gloves — touching a bad crystal with bare skin causes nightmares for weeks. Break the deep-shelf ones first; they hold the most contamination."));
						break;

					case "info":
						await dialog.Msg(L("Pearlites attack anyone in warder's robes on sight. I have to read the crystals from the shore now, and the readings are usually too old to act on."));
						break;

					case "leave":
						await dialog.Msg(L("The crystals will keep souring. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("breakCrystals", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Eight crystals broken — that's eight pockets of mana returned to Pelke. I'll perform the shore rite tonight. The lake should be clearer by dawn."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep going. Focus on the crystals on the deep shelf — those hold the worst contamination."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The water is reading clean for the first time in a season. The shrine bowls are full again."));
			}
		});

		// Quest 5: The Brood-Pearl — Kazys
		//-------------------------------------------------------------------------
		AddNpc(20129, L("[Bounty Hunter] Kazys"), "f_3cmlake_84", 302, -908, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1005);

			dialog.SetTitle(L("Kazys"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A weather-worn hunter is sharpening a heavy gaff hook, his eyes never leaving the water.*{/}"));
				await dialog.Msg(L("Every Pearlite swarm has a brood-mother. Ours is called Rajapearl — twice the size of the others, with a dark purple shell. She sleeps under the deep shelf."));
				await dialog.Msg(L("She'll only come out if her brood is in danger. Kill 10 Black Rajapearlites along the shore and she'll surface to fight."));
				await dialog.Msg(L("Once she's dead, the swarm will die out by the end of the season."));

				var response = await dialog.Select(L("Will you take the bounty?"),
					Option(L("I'll face her."), "help"),
					Option(L("How big is she?"), "info"),
					Option(L("Not my hunt."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Kill exactly 10 Rajapearlites and she'll surface. Stay near the shore so you can fight on solid ground."));
						break;

					case "info":
						await dialog.Msg(L("Twice the size of her brood, dark purple shell. As long as she lives the swarm keeps replenishing. Kill her and the rest are just stragglers."));
						break;

					case "leave":
						await dialog.Msg(L("The contract stays open. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinSwarm", out var pObj)) return;
				if (!quest.TryGetProgress("slayBroodPearl", out var bObj)) return;

				if (bObj.Done)
				{
					await dialog.Msg(L("Rajapearl is dead. The deep shelf is quiet for the first time in years. Your bounty is ready. Good work."));
					character.Quests.Complete(questId);
				}
				else if (pObj.Done)
				{
					await dialog.Msg(L("She's surfaced. Find her and kill her before she dives back down."));
				}
				else
				{
					await dialog.Msg(L("Kill 10 Rajapearlites first. She won't surface for less."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The swarm shrinks every day. Come spring the shelf will be safe to dredge."));
			}
		});

		// Quest 6: Reservoir Sweep — Vaidas
		//-------------------------------------------------------------------------
		AddNpc(20106, L("[Militia-Captain] Vaidas"), "f_3cmlake_84", -1220, 1449, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_3cmlake_84", 1006);

			dialog.SetTitle(L("Vaidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A militia captain is watching the gate of Sienakal Graveyard. Hand on her saber.*{/}"));
				await dialog.Msg(L("My post is here. I watch that gate. Something's been moving around in there and if it comes out, I'm what's between it and the shrine."));
				await dialog.Msg(L("Problem is, the swarm on the reservoir keeps chewing through my patrols. I can't leave the gate, but I can't let the shore go either."));
				await dialog.Msg(L("Help me thin them out — 12 Black Rajapearlites, 12 Blue Slimes, and 12 Sawpents. If the shore's quiet I can keep my eyes here."));

				var response = await dialog.Select(L("Will you take the contract?"),
					Option(L("I'll do the sweep."), "help"),
					Option(L("What are you watching for?"), "info"),
					Option(L("Not interested."), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Work the perimeter first and save the deep shelf for last — the swarm is thickest there. Come back when all three groups are done."));
						break;

					case "info":
						await dialog.Msg(L("Sienakal hasn't been quiet. Lights moving where they shouldn't be, sounds I'd rather not describe. If something does come out of that gate, I'd rather not have a panic on the shore behind me at the same time."));
						break;

					case "leave":
						await dialog.Msg(L("Then the swarm keeps growing and my patrols keep shrinking. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("thinPearlites", out var pObj)) return;
				if (!quest.TryGetProgress("thinSlimes", out var sObj)) return;
				if (!quest.TryGetProgress("thinSawpents", out var wObj)) return;

				if (pObj.Done && sObj.Done && wObj.Done)
				{
					await dialog.Msg(L("All three groups thinned out. Shore's quiet from here for the first time in weeks. Now I can watch this gate without splitting my attention. Pay's yours."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Keep at it. I need all three groups thinned before I can call the shore safe."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Shore's still quiet. I can keep my watch on the gate without worrying about my back. Worth more than the silver, honestly."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Shrine Marker Vigil
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1001 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1001);
		SetName(L("Shrine Marker Vigil"));
		SetType(QuestType.Sub);
		SetDescription(L("Walk the four Pelke shrine markers ringing the reservoir and pray each one awake so the priests can chart the rot."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Reservoir-Warden] Mindaugas"), "f_3cmlake_84");

		AddObjective("touchMarkers", L("Read the Pelke shrine markers"),
			new VariableCheckObjective("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker1");
		character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker2");
		character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker3");
		character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.Marker4");
		character.Variables.Perm.Remove("Laima.Quests.f_3cmlake_84.Quest1001.MarkersAwakened");
	}
}

// Quest 1002 CLASS: Bile Phials
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1002 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1002);
		SetName(L("Bile Phials"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Blue Slimes and bring six fresh-drawn bile phials to Ruta for the purging rite."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shrine-Attendant] Ruta"), "f_3cmlake_84");

		AddDrop(650095, 0.35f, MonsterId.Slime_Dark_Blue);

		AddObjective("thinSlimes", L("Thin the Blue Slimes"),
			new KillObjective(25, new[] { MonsterId.Slime_Dark_Blue }));

		AddObjective("gatherPhials", L("Gather bile phials"),
			new CollectItemObjective(650095, 6));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650095, character.Inventory.CountItem(650095), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650095, character.Inventory.CountItem(650095), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Sawpent Scales
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1003 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1003);
		SetName(L("Sawpent Scales"));
		SetType(QuestType.Sub);
		SetDescription(L("Hunt Sawpents and bring damp scales to Gintare for the antivenom draught."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Herbalist] Gintare"), "f_3cmlake_84");

		AddDrop(666028, 0.40f, MonsterId.Sowpent);

		AddObjective("thinSawpents", L("Hunt Sawpents"),
			new KillObjective(20, new[] { MonsterId.Sowpent }));

		AddObjective("gatherScales", L("Gather damp Sawpent scales"),
			new CollectItemObjective(666028, 7));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666028, character.Inventory.CountItem(666028), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666028, character.Inventory.CountItem(666028), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Reservoir Slivers
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1004 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1004);
		SetName(L("Water-Mana Retuning"));
		SetType(QuestType.Sub);
		SetDescription(L("Break the sour Tree Root Crystals on the deep shelf so Pelke can pull the trapped mana home before it spoils again."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Crystal-Warder] Saule"), "f_3cmlake_84");

		AddObjective("breakCrystals", L("Break sour Tree Root Crystals"),
			new KillObjective(8, new[] { MonsterId.Rootcrystal_01 }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

// Quest 1005 CLASS: The Brood-Pearl
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1005 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1005);
		SetName(L("The Brood-Pearl"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Black Rajapearlite swarm to draw the brood-mother Rajapearl from the deep shelf, then bring her down."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Bounty Hunter] Kazys"), "f_3cmlake_84");

		AddObjective("thinSwarm", L("Thin the Black Rajapearlite swarm"),
			new KillObjective(10, new[] { MonsterId.Rajapearlite_Purple }));

		AddObjective("slayBroodPearl", L("Slay the brood-mother Rajapearl"),
			new LayeredKillObjective(
				spawnList: new[] { new KillSpec(MonsterId.Boss_Rajapearl, 1) },
				resetIdent: "thinSwarm",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(60000, 40000));
		AddReward(new SilverReward(50000));
		AddReward(new ItemReward(694009, 1));
		AddReward(new ItemReward(640086, 5));
		AddReward(new ItemReward(640004, 6));
		AddReward(new ItemReward(640007, 6));
		AddReward(new ItemReward(640013, 3));
	}
}

// Quest 1006 CLASS: Reservoir Sweep
//-----------------------------------------------------------------------------
public class F3Cmlake84Quest1006 : QuestScript
{
	protected override void Load()
	{
		SetId("f_3cmlake_84", 1006);
		SetName(L("Reservoir Sweep"));
		SetType(QuestType.Sub);
		SetDescription(L("Standard sweep of Black Rajapearlites, Blue Slimes, and Sawpents to keep the militia's reservoir road clear."));
		SetLocation("f_3cmlake_84");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Militia-Captain] Vaidas"), "f_3cmlake_84");

		AddObjective("thinPearlites", L("Thin Black Rajapearlites"),
			new KillObjective(12, new[] { MonsterId.Rajapearlite_Purple }));

		AddObjective("thinSlimes", L("Thin Blue Slimes"),
			new KillObjective(12, new[] { MonsterId.Slime_Dark_Blue }));

		AddObjective("thinSawpents", L("Thin Sawpents"),
			new KillObjective(12, new[] { MonsterId.Sowpent }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
