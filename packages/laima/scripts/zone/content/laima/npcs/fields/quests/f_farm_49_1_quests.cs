//--- Melia Script ----------------------------------------------------------
// Greene Manor - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_farm_49_1 map. Abandoned noble estate at
// the dead-end of the Klaipeda farm cluster.
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

public class FFarm491QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Blue Spion Archers
		// =====================================================================
		// Groundskeeper Saulius - Archers picking off messenger-lads
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Groundskeeper] Saulius"), "f_farm_49_1", 1623, 920, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_1", 1001);

			dialog.SetTitle(L("Saulius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A weathered groundskeeper leans on a splintered rake, eyes on the manor's dark windows*{/}"));
				await dialog.Msg(L("Three messenger-lads in two months. Blue Spion arrows, every one. They nest in the old orchards and the rose-garden has become their hunting ground."));

				var response = await dialog.Select(L("Lady Greene was good to us. We stayed after the manor fell - someone has to close the gates, someone has to bury the dead. But we can't send word to Aqueduct without the archers stopping whoever we send. Fifteen kills should break their watch."),
					Option(L("I'll kill the Archers"), "help"),
					Option(L("What happened here?"), "info"),
					Option(L("This place is too far gone"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He nods once, grateful but tired*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("Take this - a recipe for a Wreech Bow. Lady Greene's armory had a dozen; we lost them all. This recipe is the last of the house."));
						await dialog.Msg(L("Close with them fast. Their arrows are poison-tipped - a clean cut heals, a grazing scratch festers."));
						break;

					case "info":
						await dialog.Msg(L("Demon-pollen crept south across the aqueduct. Lady Greene refused to evacuate - 'the manor is four hundred years old, it outlasts one season's rot.' She was wrong."));
						await dialog.Msg(L("One night the Blue Spions came through the wards. We servants made the kitchen barricade. The family... didn't make it in time. Or maybe they fled. We're still not sure."));
						break;

					case "leave":
						await dialog.Msg(L("It is. Doesn't mean we abandon it while people still breathe on the grounds."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArcher", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He looks toward the orchard, letting out a slow breath*{/}"));
					await dialog.Msg(L("Fifteen. The orchard paths are quiet. I can send a lad to Aqueduct tomorrow and expect him back."));
					await dialog.Msg(L("Keep the recipe. And take this - what's left in the groundskeeper's purse. Lady Greene would approve."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Eastern orchards, southern rose-garden. Close distance fast - their poison is slow but sure."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Vidas's letter went through to Aqueduct. The staff are formally released. We stay by choice now, not by oath."));
			}

			// Also serves as recipient for Quest 1002 ("The Manor is Lost")
			var q1002 = new QuestId("f_farm_49_1", 1002);
			if (character.Quests.IsActive(q1002))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_1.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*You hold out Vidas's final letter. He reads it standing, then folds it very carefully*{/}"));
					await dialog.Msg(L("So. The house is released. The staff are free to go."));
					await dialog.Msg(L("{#666666}*He tucks the letter into his coat, over his heart*{/}"));
					await dialog.Msg(L("Tell Vidas I'll pass it through to Audrone at Aqueduct by tonight's messenger. The Klaipeda magistrates will have a record by the new moon."));
					await dialog.Msg(L("And tell her I'm staying. Someone has to close the gate when the last of us leaves."));

					character.Variables.Perm.Set("Laima.Quests.f_farm_49_1.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Letter delivered. Return to Estate Cook Vidas.{/}"));
				}
			}
		});

		// =====================================================================
		// QUEST 1002: The Manor is Lost
		// =====================================================================
		// Estate Cook Vidas - Final letter releasing the staff
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Estate Cook] Vidas"), "f_farm_49_1", 655, 926, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_1", 1002);

			dialog.SetTitle(L("Vidas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A man in a flour-dusted apron stirs a pot over a makeshift hearth, eyes red but hands steady*{/}"));
				await dialog.Msg(L("Kitchen's open for anyone still on the grounds. Soup tonight, bread if the oven holds. Won't starve while I'm cooking."));

				var response = await dialog.Select(L("I've drafted the letter that ends our service. The Greene line is broken or fled - we can no longer serve a house that doesn't answer us. Carry it to Saulius at the Aqueduct warp? He'll send it through to Audrone for the magistrate's record."),
					Option(L("I'll carry the letter"), "help"),
					Option(L("Why not go yourself?"), "info"),
					Option(L("That's estate business"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He wipes his hands on his apron and unfolds a letter that's been rewritten four times*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The letter's blotted in one place. I couldn't write 'Lady Greene is gone' without breaking. Tell him it's the same to me if he wants to rewrite that line."));
						break;

					case "info":
						await dialog.Msg(L("I can't leave the kitchen. Rasa comes for broth every evening - she'd be alone if I stopped. Lukas eats here when the stable gets too cold."));
						await dialog.Msg(L("Someone stays cooking while the others mourn. That's my part."));
						break;

					case "leave":
						await dialog.Msg(L("Then have soup on your way out. I'll keep ladling whether you carry letters or not."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverLetter", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens to Saulius's reply and nods, blinking fast*{/}"));
					await dialog.Msg(L("He's staying. Of course he's staying. Stubborn old man."));
					await dialog.Msg(L("{#666666}*He pulls a small pouch from the apron pocket*{/}"));
					await dialog.Msg(L("Here - honest pay, from what the staff still had pooled. Take the soup on your way out too. It's the last batch made with Greene barley."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Saulius is by the Aqueduct warp. Tell him the letter's from the kitchen."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The magistrates formally registered the house as fallen. The Greene line is closed. The kitchen still serves soup. Some things you don't stop."));
			}
		});

		// =====================================================================
		// QUEST 1003: Dandel Wingpieces
		// =====================================================================
		// Widow-Gardener Rasa - Mourning pillows for empty children's beds
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Widow-Gardener] Rasa"), "f_farm_49_1", -687, 868, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_1", 1003);

			dialog.SetTitle(L("Rasa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A grey-haired widow sews a small pillowcase by the fallen rose-hedge, her hands gentle with each stitch*{/}"));
				await dialog.Msg(L("The Greene children had three beds in the nursery. Three beds, three pillowcases, no children left to use them. I'm stuffing them anyway."));

				var response = await dialog.Select(L("Orange Dandels shed soft wingpieces when they drift through the orchard - peaceful creatures, they'll let you gather if you're gentle. Five wingpieces would fill each pillow. Three pillowcases. I'd be grateful."),
					Option(L("I'll gather four wingpieces"), "help"),
					Option(L("Why stuff pillows for empty beds?"), "info"),
					Option(L("That's somebody else's grief"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She looks up with a small, grateful smile*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("If a Dandel startles, just kneel and wait. They settle in a count of ten."));
						await dialog.Msg(L("Lady Greene taught me that trick. She was gentle with every animal on the estate."));
						break;

					case "info":
						await dialog.Msg(L("Because if the children fled - and maybe they did - they deserve soft pillows when they come home. And if they didn't flee, the pillows are for their memory, which deserves just as soft a bed."));
						await dialog.Msg(L("I've stuffed worse pillows for worse reasons. My husband's. My sister's. Grief needs somewhere to rest its head."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. Not everyone can carry another's grief. Be well, traveler."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var featherCount = character.Inventory.CountItem(662067);

				if (featherCount >= 5)
				{
					await dialog.Msg(L("{#666666}*She weighs each wingpiece, nodding as it catches the light*{/}"));
					await dialog.Msg(L("Soft. Clean. No blood, no panic on the wingpieces. You gathered kindly."));
					await dialog.Msg(L("{#666666}*She tucks them into a pillowcase, one at a time*{/}"));
					await dialog.Msg(L("Here - my thanks. Small coin, quiet thanks. The pillows will rest on the beds by nightfall."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four wingpieces. Gentle hands. The Dandels know the difference."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The pillows are on the beds. I close the nursery door softly when I leave. Whatever comes, they sleep ready."));
			}
		});

		// =====================================================================
		// TAMA DOWN SPOTS
		// =====================================================================
		// For Quest 1003 - Tama Down Feathers
		// =====================================================================

		void AddTamaDownSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Dandel Wing-Patch"), "f_farm_49_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_49_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Orange wingpieces drift in a grass hollow where a Dandel has been grooming*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_49_1.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*The wing-patch is gathered clean. A fresh wingpiece will drift in tomorrow*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_farm_49_1.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Dandel_Orange, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}An Orange Dandel drifts over, curious about the gathered wingpieces!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Gathering down..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(662067, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(662067);
					character.ServerMessage(LF("Wingpieces gathered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All wingpieces gathered! Return to Widow-Gardener Rasa.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering interrupted."));
				}
			});
		}

		AddTamaDownSpot(1, 595, 510, 0);
		AddTamaDownSpot(2, 173, 933, 90);
		AddTamaDownSpot(3, -687, 895, 180);
		AddTamaDownSpot(4, 599, -581, 270);

		// =====================================================================
		// QUEST 1004: The Last Greene
		// =====================================================================
		// Former Stableboy Lukas - Seeking the truth of Lady Greene's end
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Former Stableboy] Lukas"), "f_farm_49_1", -125, -477, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_farm_49_1", 1004);

			dialog.SetTitle(L("Lukas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young man in a half-buttoned stablecoat cleans a bridle that will never be worn again*{/}"));
				await dialog.Msg(L("I was in the stables when the Spions came through. By the time I got to the farmhouse, it was already quiet. I've never looked past the gate since."));

				var response = await dialog.Select(L("I need to know. Did Lady Greene die on the grounds, or did she flee? Saulius won't walk it. Vidas can't. Rasa is older than these fences. But you - you're a traveler. Walk the four places where she'd have left a sign and tell me what you find. The granary, the old well, the field-shrine, and the root-cellar hatch."),
					Option(L("I'll walk the four places"), "help"),
					Option(L("Why do you need to know?"), "info"),
					Option(L("Let the dead rest"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*He sets the bridle down very carefully*{/}"));

						character.Quests.Start(questId);
						await dialog.Msg(L("The granary stands north of the barn-ruin. The old well is in the east paddock. The field-shrine's out by the orchard row. The root-cellar hatch is just behind the farmhouse kitchen door."));
						await dialog.Msg(L("Don't take anything. Just look."));
						break;

					case "info":
						await dialog.Msg(L("Because I was her stableboy for nine years. Because she taught me to ride when no one else would. Because if she fled, I want to believe she made it."));
						await dialog.Msg(L("And because closure - proper closure - is harder to come by than a horse in a burning barn."));
						break;

					case "leave":
						await dialog.Msg(L("Rest. Yes. But I don't sleep until I know."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var roomsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited", 0);

				if (roomsVisited >= 4)
				{
					await dialog.Msg(L("{#666666}*He listens to your report in silence, jaw working*{/}"));
					await dialog.Msg(L("Granary stripped. Well-bucket dry on the rim. Field-shrine offerings cleared. The root-cellar hatch unbarred from the inside."));
					await dialog.Msg(L("{#666666}*His eyes close in relief*{/}"));
					await dialog.Msg(L("She fled. She took the children and the grain through the cellar passage. She made it out."));
					await dialog.Msg(L("Thank you. More than you know. Take this - a stableboy's savings, offered whole. You gave me back a night of sleep."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four places. Granary, old well, field-shrine, root-cellar hatch."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Rasa confirmed the cellar passage leads to a Fedimian safehouse. Lady Greene kept its key in the field-shrine offering box. She knew. She prepared."));
			}
		});

		// =====================================================================
		// GREENE ESTATE LANDMARKS
		// =====================================================================
		// For Quest 1004 - The Last Greene
		// =====================================================================

		void AddManorRoom(int roomNumber, string roomName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(roomName), "f_farm_49_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_farm_49_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A silent landmark on the abandoned Greene estate*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_farm_49_1.Quest1004.Room{roomNumber}";
				var checkedRoom = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedRoom)
				{
					await dialog.Msg(L("{#666666}*You've already checked this place*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Looking around..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var roomsVisited = character.Variables.Perm.GetInt("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited", 0);
					character.Variables.Perm.Set("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited", roomsVisited + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Places checked: {0}/4", roomsVisited + 1));

					if (roomsVisited + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All places checked! Return to Lukas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Examination interrupted."));
				}
			});
		}

		AddManorRoom(1, "Greene Granary", "Sacks of grain stripped clean. Footprints lead toward the root-cellar, not the road.", 100, 400, 0);
		AddManorRoom(2, "Old East Well", "Bucket dry on the rim, rope coiled tight - last drawn in a hurry, not abandoned mid-use.", 300, 600, 90);
		AddManorRoom(3, "Field-Shrine", "Offering bowl emptied. A scrap of paper tucked under the shrine-stone - a Fedimian safehouse address.", 450, 750, 180);
		AddManorRoom(4, "Root-Cellar Hatch", "Hatch unbarred from the inside. The passage beyond goes deeper than any root-cellar needs to.", 200, 800, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Blue Spion Archers
//-----------------------------------------------------------------------------

public class BlueSpionArchersQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_1", 1001);
		SetName("Blue Spion Archers");
		SetType(QuestType.Sub);
		SetDescription("Groundskeeper Saulius needs the Blue Spion Archers killed so Greene Manor's surviving staff can send messengers to Aqueduct Bridge.");
		SetLocation("f_farm_49_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Groundskeeper] Saulius", "f_farm_49_1");

		AddObjective("killArcher", "Defeat Blue Spion Archers",
			new KillObjective(15, new[] { MonsterId.Spion_Bow_Blue }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
		AddReward(new ItemReward(924069, 1));  // Recipe - Wreech Bow
	}
}

// Quest 1002 CLASS: The Manor is Lost
//-----------------------------------------------------------------------------

public class TheManorIsLostQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_1", 1002);
		SetName("The Manor is Lost");
		SetType(QuestType.Sub);
		SetDescription("Estate Cook Vidas's final letter releasing Greene Manor's staff must reach Groundskeeper Saulius at the Aqueduct warp.");
		SetLocation("f_farm_49_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Estate Cook] Vidas", "f_farm_49_1");

		AddObjective("deliverLetter", "Deliver Vidas's letter to Saulius",
			new VariableCheckObjective("Laima.Quests.f_farm_49_1.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_1.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Dandel Wingpieces
//-----------------------------------------------------------------------------

public class DandelWingpiecesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_1", 1003);
		SetName("Dandel Wingpieces");
		SetType(QuestType.Sub);
		SetDescription("Widow-Gardener Rasa needs four Orange Dandel wingpieces to stuff mourning-pillows for the empty beds in the Greene children's nursery.");
		SetLocation("f_farm_49_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Widow-Gardener] Rasa", "f_farm_49_1");

		AddObjective("collectFeathers", "Gather Orange Dandel wingpieces from wing-patches",
			new CollectItemObjective(662067, 4));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662067,
			character.Inventory.CountItem(662067),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662067,
			character.Inventory.CountItem(662067),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Last Greene
//-----------------------------------------------------------------------------

public class TheLastGreeneQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_farm_49_1", 1004);
		SetName("The Last Greene");
		SetType(QuestType.Sub);
		SetDescription("Former Stableboy Lukas has asked you to walk the four main rooms of Greene Manor and determine whether Lady Greene died in the attack or fled through the root-cellar passage.");
		SetLocation("f_farm_49_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Former Stableboy] Lukas", "f_farm_49_1");

		AddObjective("checkRooms", "Check the manor's four main rooms",
			new VariableCheckObjective("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited", 4, true));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1004.Room{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_farm_49_1.Quest1004.RoomsVisited");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_farm_49_1.Quest1004.Room{i}");
		}
	}
}
