//--- Melia Script ----------------------------------------------------------
// Grynas Hills - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_katyn_45_3. Hilly dark-forest tract where
// Griba mushrooms grow in closing rings, forest-jellyfish drift between
// trunks, and ghost-fishermen cast into ponds that went dry a generation ago.
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

public class FKatyn453QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Break the Closing Rings
		// =====================================================================
		// Ring-Breaker Eimantas - Griba mushrooms forming closing ring-circles
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Ring-Breaker] Eimantas"), "f_katyn_45_3", -540, 1300, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1001);

			dialog.SetTitle(L("Eimantas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A gaunt man in rune-scratched leathers kicks a small yellow mushroom out of a half-formed ring at his boot. It bleeds a thin pink sap where he touched it*{/}"));
				await dialog.Msg(L("Griba ring-circles are forming twice as fast this month. Step inside a closed one and your hour goes wrong - hands on the wrong way, bootlaces tied to themselves, a song stuck in your head you never learned."));

				var response = await dialog.Select(L("Twenty Griba cut before their rings close. Hit the ones already bending inward toward each other - those are the ring-starters. Will you?"),
					Option(L("I'll cut twenty ring-starters"), "help"),
					Option(L("What happens in a closed ring?"), "info"),
					Option(L("Leave the mushrooms alone"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He spits into the half-ring, dispersing it*{/}"));

						if (await dialog.YesNo(L("Twenty. Break the ring-starters first. If you find a closed ring, walk around it - never through. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If you hear giggling from inside a ring - that's the ring working on the last thing that crossed it. Keep moving. Don't turn."));
						}
						break;

					case "info":
						await dialog.Msg(L("Depends on the ring. Small ones steal an hour. Medium ones steal a week. Big ones - the ones we don't let close - steal something you didn't know was yours until it was gone."));
						break;

					case "leave":
						await dialog.Msg(L("Then the rings close. Then someone crosses one. Then they don't come back the same, if they come back. You know how this ends."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGriba", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He counts the broken rings on his mental tally, then nods, tight and relieved*{/}"));
					await dialog.Msg(L("Twenty rings broken before they closed. That's an afternoon's work well done, and eight hours the woods don't steal from anyone crossing them."));
					await dialog.Msg(L("Ring-Breaker's purse. And a kick-knife - the trick isn't cutting, it's kicking the ring-starter out of its compass. Pass it on."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Bending-inward mushrooms. Kick them out of the ring before they root in. Twenty."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The hill-paths stayed safe a full week. Then I found a closed ring with a boot inside it, and no boot-owner anywhere. Back to kicking."));
			}
		});

		// =====================================================================
		// QUEST 1002: A Rosary for the Drowned Pond
		// =====================================================================
		// Fisherwoman Onute - Rosary for her husband's pond-memorial
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Fisherwoman] Onute"), "f_katyn_45_3", -240, -680, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1002);

			dialog.SetTitle(L("Onute"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An old woman in a fisher-coat that has not touched water in years polishes a bead-rosary against her sleeve. Her breath ghosts despite the mild air*{/}"));
				await dialog.Msg(L("Pond Tarvydas went dry thirty years ago. My Aleksas drowned in it anyway - don't ask how. My sister Genute keeps his memorial at the old pond-rim. The rosary I'm finishing replaces the one that rotted off the marker."));

				var response = await dialog.Select(L("Carry this rosary to Genute at the drowned pond's eastern edge. The ghost-fishers gather there at dusk, but they do not harm those on a memorial-errand. Bring back whatever Genute hands you in return."),
					Option(L("I'll carry the rosary"), "help"),
					Option(L("Dry pond? Drowned anyway?"), "info"),
					Option(L("Grief isn't my errand"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She binds the rosary in waxed paper, ties it with a strand of her own grey hair, and presses it into your hand*{/}"));

						if (await dialog.YesNo(L("Eastern edge, past the mushroom hill. Do not speak to a fisher casting into the dry bowl. They will answer, and you do not want them to. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Genute wears a blue kerchief. If the woman at the memorial wears red, she is not Genute. Walk on. Come back. Try at dawn."));
						}
						break;

					case "info":
						await dialog.Msg(L("Some ponds drown people whether they hold water or not. Aleksas fished Tarvydas every dawn for forty years. The day it went dry, he went too - on the bowl's bottom, lungs full of pond-water that wasn't there."));
						await dialog.Msg(L("The hills remember what the ponds did. Aleksas is one memory among many. Memorials keep the memory polite."));
						break;

					case "leave":
						await dialog.Msg(L("Then the marker's bare another season. The fisher-ghosts notice when a rosary rots off. They notice, and they get curious. Your choice."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverRosary", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She receives the returned wax-bundle with both hands, slowly, not looking down until her fingers tell her what's inside*{/}"));
					await dialog.Msg(L("Genute sent a bead back. The one from Aleksas's wrist-cord - she keeps it by the marker, and it only leaves when the rosary gets renewed. It means the memorial accepted. It means he settles another season."));
					await dialog.Msg(L("Fisherwoman's coin, what I can spare. Keep the bead a night before you move on - the dream you have tonight is Aleksas's thanks. Quiet dream. He was a quiet man."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Eastern edge. Blue kerchief. Don't speak to the dry-bowl fishers."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Aleksas was quiet last night. Genute says she heard him whistle once - his work-whistle, never his alarm-whistle. That's the memorial working."));
			}
		});

		// =====================================================================
		// MEMORIAL-KEEPER GENUTE (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20114, L("[Memorial-Keeper] Genute"), "f_katyn_45_3", 1160, 1100, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1002);

			dialog.SetTitle(L("Genute"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A woman in a blue kerchief kneels beside a weathered pond-marker at the rim of a bone-dry bowl. The bowl's floor is cracked but damp, somehow. The air smells of deep water*{/}"));
					await dialog.Msg(L("{#666666}*You present the wax-bundle. She unwraps it, binds the rosary around the marker-post, and the bowl's damp edge recedes a finger's-width*{/}"));
					await dialog.Msg(L("The rosary holds. Tell Onute the memorial accepted."));
					await dialog.Msg(L("{#666666}*She unties a small bone-bead from her own wrist-cord - Aleksas's bead - and wraps it in the same wax-paper*{/}"));
					await dialog.Msg(L("Carry this back. The bead only travels when the memorial renews. Onute will understand."));
					await dialog.Msg(L("{#666666}*Behind her, a ghost-fisher casts a long phantom line into the dry bowl. It does not look up. Neither does she*{/}"));

					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Aleksas's bead received. Return to Fisherwoman Onute.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the bead back. Onute waits. The pond-edge recedes with each renewal."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Aleksas whistled twice last dusk. Work-whistle, both times. The memorial is calm. The bowl stays dry where it should be dry."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*She does not turn. Her hand rests on the marker-post. The dry bowl beyond her feels watchful*{/}"));
				await dialog.Msg(L("Memorial-errands only. Keep the path, traveler."));
			}
		});

		// =====================================================================
		// QUEST 1003: Jellyfish Tear-Fronds
		// =====================================================================
		// Dream-Diviner Rima - Tear-fronds from forest-jellyfish
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Dream-Diviner] Rima"), "f_katyn_45_3", 680, -1170, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1003);

			dialog.SetTitle(L("Rima"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A diviner in salt-crusted robes drops a translucent green frond into a copper bowl and stirs slowly. The frond does not dissolve - it weeps, steadily, a clear fluid that should not come from a dried thing*{/}"));
				await dialog.Msg(L("Forest-jellyfish don't belong in a forest. They belong to whatever ocean used to be here, or whatever ocean might still be here, viewed sideways. Their tear-fronds divine true."));

				var response = await dialog.Select(L("Eight tear-fronds. Approach a drifting jellyfish from below - never from above. Cut the trailing frond, not the bell. The bell remembers; the frond only weeps. Will you gather for me?"),
					Option(L("I'll gather eight tear-fronds"), "help"),
					Option(L("What do they divine?"), "info"),
					Option(L("Jellyfish belong in the sea"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She draws a long copper hook from her sleeve and passes it to you. It is warm, as if it had just been in water*{/}"));

						if (await dialog.YesNo(L("Below the bell. Trailing frond only. Eight. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If the bell pulses toward you after you cut - drop the frond and walk away. The bell remembers you now. Fronds can be replaced."));
						}
						break;

					case "info":
						await dialog.Msg(L("Dreams of water. True dreams of people who drowned, wherever and whenever. I use them to find kin for the fisher-widows - who died, how, where the body drifts. Honest work for hard grief."));
						break;

					case "leave":
						await dialog.Msg(L("Then the fisher-widows wait another moon for their dreams. The jellyfish drift either way - in the forest, out of it. They don't need us."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var frondCount = character.Inventory.CountItem(650852);

				if (frondCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She inspects each frond in lamplight, watching which ones weep faster, and sets two aside without comment*{/}"));
					await dialog.Msg(L("Six will steep clean. Two cried before you finished gathering - those fronds saw something first-hand. Those go in a locked drawer. Honest work either way."));
					await dialog.Msg(L("Diviner's share. And a small vial of tear-water for your own pack - rinse your eyes with it if a dream overstays its welcome."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Below the bell. Trailing frond. Eight."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Three widows received true answers this week. One fisher-son came home after seven years - the dream told his mother the exact harbor. The fronds earn their keep."));
			}
		});

		// =====================================================================
		// JELLYFISH FROND SPOTS
		// =====================================================================
		// For Quest 1003 - Jellyfish Tear-Fronds
		// =====================================================================

		void AddFrondSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Drifting Jellyfish"), "f_katyn_45_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_3", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A green jellyfish drifts in still air between two trunks. It should not exist here. Without a diviner's order, there's no reason to cut*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_3.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*A trailing frond has been cut. The bell drifts onward without apparent concern*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_katyn_45_3.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Jellyfish_Green, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A second jellyfish drifts out from between the trunks - the bell pulses toward you!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Cutting the trailing frond..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650852, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650852);
					character.ServerMessage(LF("Tear-fronds gathered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All fronds gathered! Return to Dream-Diviner Rima.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Cutting interrupted. The bell pulses once."));
				}
			});
		}

		AddFrondSpot(1, 683, -1167, 0);
		AddFrondSpot(2, 924, -1114, 90);
		AddFrondSpot(3, 1254, -606, 180);
		AddFrondSpot(4, 907, -481, 270);
		AddFrondSpot(5, 268, -1339, 0);
		AddFrondSpot(6, -468, -908, 90);
		AddFrondSpot(7, 1154, 470, 180);
		AddFrondSpot(8, 1178, 669, 270);

		// =====================================================================
		// QUEST 1004: Which Dry Bowls Still Drown
		// =====================================================================
		// Pond-Surveyor Vilma - Checking which drowned ponds are active
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Pond-Surveyor] Vilma"), "f_katyn_45_3", 1130, -820, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1004);

			dialog.SetTitle(L("Vilma"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young surveyor in waterproofed boots that have never seen rain checks the level on a palm-sized drift-gauge. The gauge's needle quivers despite stillness*{/}"));
				await dialog.Msg(L("Four dry pond-bowls on these hills. All went dry in the same drought, fifty years back. Three should be inert; one is listed as 'drowning' - still pulls water out of people who kneel too close."));

				var response = await dialog.Select(L("Walk the four bowls. Don't kneel - just stand at the rim and hold the drift-gauge out. Tell me which way the needle swings. Will you?"),
					Option(L("I'll walk the four bowls"), "help"),
					Option(L("What does 'drowning' mean?"), "info"),
					Option(L("Dry ponds stay dry"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She presses the drift-gauge into your hand. It is cool and slightly damp despite the dry air*{/}"));

						if (await dialog.YesNo(L("Four bowls. Needle still: inert. Needle toward the bowl: drowning. Needle away from the bowl: something worse than drowning. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If the needle swings away, step back three paces and then walk - don't turn to look at the bowl. I mean this. Don't look."));
						}
						break;

					case "info":
						await dialog.Msg(L("A drowning bowl pulls water out of a body that stands too close. Small amounts - a mouthful, maybe a cup. Enough to make you light-headed. Enough, over a careless afternoon, to kill."));
						await dialog.Msg(L("A bowl that pulls the needle the other way pulls something else out of you. I don't know what. I haven't lost it yet - because I don't kneel."));
						break;

					case "leave":
						await dialog.Msg(L("Then the drowning goes on unmeasured, and someone kneels, and someone doesn't come home. It's fine. I'll walk them myself over six months."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var bowlsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked", 0);

				if (bowlsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*She logs each reading, and her pencil stops at the final line*{/}"));
					await dialog.Msg(L("Bowl One: needle still. Inert. Bowl Two: needle still, slight tremor. Bowl Three: needle toward the bowl. Drowning, listed correctly. Bowl Four: needle away from the bowl."));
					await dialog.Msg(L("{#666666}*She closes the logbook. Her jaw is tight*{/}"));
					await dialog.Msg(L("Bowl Four was listed inert. It isn't. It's the other kind. I'll cordon it before nightfall and send word to Fedimian - this is beyond hill-survey work."));
					await dialog.Msg(L("Surveyor's stipend, every coin. Thank you for not kneeling. Most would have knelt."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four bowls. Drift-gauge at the rim. Don't kneel."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Bowl Four is cordoned. A wardmage is due next week. Until then, the ribbon keeps strangers from kneeling, and that is enough."));
			}
		});

		// =====================================================================
		// DRY POND-BOWLS
		// =====================================================================
		// For Quest 1004 - Which Dry Bowls Still Drown
		// =====================================================================

		void AddPondBowl(int bowlNumber, string bowlName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(bowlName), "f_katyn_45_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_3", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A dry pond-bowl, cracked-mud floor, rim marked with a surveyor's ribbon. Without an order, there's no reason to hold a gauge out*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_3.Quest1004.Bowl{bowlNumber}";
				var checkedBowl = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedBowl)
				{
					await dialog.Msg(L("{#666666}*You have already gauged this bowl. The reading remains in the logbook*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Holding the drift-gauge at the rim..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var bowlsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked", bowlsChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Bowls gauged: {0}/4", bowlsChecked + 1));

					if (bowlsChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All bowls gauged! Return to Pond-Surveyor Vilma.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gauging interrupted. The needle settles, slowly."));
				}
			});
		}

		AddPondBowl(1, "North Dry Bowl", "North Bowl: needle still. Inert. Floor bone-dry all the way across.", -527, -754, 0);
		AddPondBowl(2, "Mushroom-Rim Bowl", "Mushroom-Rim Bowl: needle still, faint tremor. Inert, but listening. Cracked floor damp at the center.", 725, -722, 90);
		AddPondBowl(3, "Fisher's Bowl", "Fisher's Bowl: needle swings toward the bowl. Drowning, as listed. You take a small step back without meaning to.", 308, -979, 180);
		AddPondBowl(4, "Hilltop Bowl", "Hilltop Bowl: needle swings AWAY from the bowl. You step back three paces and do not look behind you.", 1336, 1488, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Break the Closing Rings
//-----------------------------------------------------------------------------

public class BreakTheClosingRingsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1001);
		SetName("Break the Closing Rings");
		SetType(QuestType.Sub);
		SetDescription("Ring-Breaker Eimantas needs twenty Griba mushrooms cut before their ring-circles close and begin stealing from travelers.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Ring-Breaker] Eimantas", "f_katyn_45_3");

		AddObjective("killGriba", "Defeat ring-starter Griba mushrooms",
			new KillObjective(20, new[] { MonsterId.Mushroom_Boy_Yellow }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10)); // Large HP Potion
		AddReward(new ItemReward(640007, 10)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}
}

// Quest 1002 CLASS: A Rosary for the Drowned Pond
//-----------------------------------------------------------------------------

public class ARosaryForTheDrownedPondQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1002);
		SetName("A Rosary for the Drowned Pond");
		SetType(QuestType.Sub);
		SetDescription("Fisherwoman Onute's renewal-rosary must reach her sister Genute at Pond Tarvydas's memorial, and Aleksas's bead must return to confirm the memorial accepted.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Fisherwoman] Onute", "f_katyn_45_3");

		AddObjective("deliverRosary", "Deliver the rosary and return with Aleksas's bead",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_3.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 10));  // Large HP Potion
		AddReward(new ItemReward(640007, 10));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Jellyfish Tear-Fronds
//-----------------------------------------------------------------------------

public class JellyfishTearFrondsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1003);
		SetName("Jellyfish Tear-Fronds");
		SetType(QuestType.Sub);
		SetDescription("Dream-Diviner Rima needs eight tear-fronds cut from forest-jellyfish trailing-fronds for her grief-dream divinations.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Dream-Diviner] Rima", "f_katyn_45_3");

		AddObjective("collectFronds", "Gather tear-fronds from drifting jellyfish",
			new CollectItemObjective(650852, 8));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
		AddReward(new ItemReward(640012, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650852,
			character.Inventory.CountItem(650852),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650852,
			character.Inventory.CountItem(650852),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: Which Dry Bowls Still Drown
//-----------------------------------------------------------------------------

public class WhichDryBowlsStillDrownQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1004);
		SetName("Which Dry Bowls Still Drown");
		SetType(QuestType.Sub);
		SetDescription("Pond-Surveyor Vilma has asked you to gauge four dry pond-bowls and determine which are inert, which are still drowning, and which pull something worse.");
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Pond-Surveyor] Vilma", "f_katyn_45_3");

		AddObjective("gaugeBowls", "Gauge the four dry pond-bowls",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 11)); // Large HP Potion
		AddReward(new ItemReward(640007, 11)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1004.Bowl{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1004.BowlsChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1004.Bowl{i}");
		}
	}
}
