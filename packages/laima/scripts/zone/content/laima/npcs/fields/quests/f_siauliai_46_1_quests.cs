//--- Melia Script ----------------------------------------------------------
// Spring Light Woods - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_siauliai_46_1. A too-bright magical grove
// where Shardstatues stand motionless by day and move at dusk, and Infro
// Blood drift at the waterline like red ink in clear water.
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

public class FSiauliai461QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Statues That Walk at Dusk
		// =====================================================================
		AddNpc(20109, L("[Grove-Warden] Rokas"), "f_siauliai_46_1", 770, -50, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1001);

			dialog.SetTitle(L("Rokas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A warden in polished brass scale squints against the grove's unnatural brightness. Shardstatues stand motionless around him - all facing slightly different directions than they should*{/}"));
				await dialog.Msg(L("Shardstatues hold still by day. At dusk they walk - slow, deliberate, always toward the grove-spring. I shatter them before they reach the water. Keeps the spring clean."));

				var response = await dialog.Select(L("Twenty Shardstatues put down while the light holds. Break them at the knee-seam; the upper body is slow to realize. Will you?"),
					Option(L("I'll break twenty Shardstatues"), "help"),
					Option(L("Why toward the spring?"), "info"),
					Option(L("Statues aren't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("If you hear a grinding sound behind you - a statue has started walking early. Turn and strike before it reaches you."));
						break;

					case "info":
						await dialog.Msg(L("The spring wakes them. Or something at the bottom of the spring wakes them. Nobody's looked, and nobody plans to. I just break them before they reach it."));
						break;

					case "leave":
						await dialog.Msg(L("Walk out of the grove before dusk, then. The statues don't follow past the treeline."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killStatue", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He counts the still-standing statues against a mental tally, then nods*{/}"));
					await dialog.Msg(L("Twenty fewer tonight. The spring stays clean until next week's batch wakes."));
					await dialog.Msg(L("Warden's purse. And a pair of shard-gloves - statue-dust burns bare skin. These won't."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Knee-seam first. Don't shatter near the spring. Twenty."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The spring ran clear eight full days. Then a dusk came warm, and three statues started walking before I was in position. Back to the count."));
			}
		});

		// =====================================================================
		// QUEST 1002: The Spring-Water Flask
		// =====================================================================
		AddNpc(20107, L("[Herbalist] Aldona"), "f_siauliai_46_1", -250, 100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1002);

			dialog.SetTitle(L("Aldona"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A herbalist in a sun-bleached apron seals a glass flask of pale water and wraps it in herb-muslin*{/}"));
				await dialog.Msg(L("Spring-water from this grove holds its light for two days. Grove-Tender Saule at the south plot brews cordials from it - but she can't leave the plot, her stock rots if she does."));

				var response = await dialog.Select(L("Carry this flask to Saule. Bring back whatever she presses into the muslin in return. Will you?"),
					Option(L("I'll carry the flask"), "help"),
					Option(L("Why the muslin wrap?"), "info"),
					Option(L("Carry it yourself"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Saule wears a yellow headband. Anyone else at the plot, walk on - she's alone most days."));
						break;

					case "info":
						await dialog.Msg(L("The muslin filters the grove's overlight. Without it, the flask glares bright enough to blind a careless walker. Healer's first rule - don't blind your couriers."));
						break;

					case "leave":
						await dialog.Msg(L("Then the cordials don't brew this week. Saule won't mind. But the patients who need them will."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverFlask", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She unwraps the returned muslin and lets out a small happy sound*{/}"));
					await dialog.Msg(L("Two vials of cordial, still warm. She brewed them the moment the flask arrived. Take one for yourself - traveler's due."));
					await dialog.Msg(L("Herbalist's coin. And the cordial keeps a month in a dark pocket. Cures what ails a long walk."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("South plot. Yellow headband. Don't unwrap the flask."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Saule brewed fourteen cordials this week. Patients up-village are recovering faster. Good grove, good water, good work."));
			}
		});

		// Quest 1002 recipient
		AddNpc(20114, L("[Grove-Tender] Saule"), "f_siauliai_46_1", 350, -900, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1002);

			dialog.SetTitle(L("Saule"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_46_1.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*A tender in a yellow headband unwraps the flask, brews with practiced speed, and fills two small cordial-vials within a minute*{/}"));
					await dialog.Msg(L("Aldona's flask. Warm, still holding. Good batch."));
					await dialog.Msg(L("{#666666}*She presses both vials into the same muslin and ties it with a sprig of yellow thread*{/}"));
					await dialog.Msg(L("Two vials back. Tell her the light held through the brew - strongest batch I've made all month."));

					character.Variables.Perm.Set("Laima.Quests.f_siauliai_46_1.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Saule's cordials received. Return to Herbalist Aldona.{/}"));
				}
				else
				{
					await dialog.Msg(L("Carry the cordials to Aldona. The yellow thread is her batch-mark."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Good brewing week. The plot's clover is blooming off-season - that means the grove-light is still strong here. Good sign."));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The tender grinds herb-seed in a small mortar, humming*{/}"));
				await dialog.Msg(L("Plot-visits only. If you've no flask, mind the clover-rows on your way."));
			}
		});

		// =====================================================================
		// QUEST 1003: Infro Blood-Orbs
		// =====================================================================
		AddNpc(20017, L("[Ink-Maker] Milda"), "f_siauliai_46_1", -600, -900, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1003);

			dialog.SetTitle(L("Milda"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*An ink-maker holds a small clear orb up to the grove-light. Red ink swirls inside, unprompted*{/}"));
				await dialog.Msg(L("Infro Blood-orbs make the finest red ink in three regions. The ink never fades - letters written in it still read true after a hundred years, even when the paper yellows."));

				var response = await dialog.Select(L("Eight Blood-orbs. Gather from Infro pools where the water runs still - moving water breaks the orb before it forms. Will you?"),
					Option(L("I'll gather eight Blood-orbs"), "help"),
					Option(L("Why never fade?"), "info"),
					Option(L("Ink-making isn't my craft"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("If an orb bursts in your palm, wipe the red before it dries. Dried Infro-ink doesn't come off skin for a week."));
						break;

					case "info":
						await dialog.Msg(L("Grove-magic in the water, grove-magic in the ink. Don't ask me the theory - I only know the recipe, and the recipe only works here."));
						break;

					case "leave":
						await dialog.Msg(L("Then the ink-stock runs out and the scribes up-village complain. It's fine. I'll manage a week on the old batch."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var orbCount = character.Inventory.CountItem(650855);
				if (orbCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She inspects each orb against lamplight, separating clear ones from cloudy*{/}"));
					await dialog.Msg(L("Six crystal-clear. Two cloudy - those make a duller ink, but still usable for ledgers. All eight earn their weight."));
					await dialog.Msg(L("Ink-maker's coin. And a small vial of fresh Infro-ink - write a letter you want to last a century."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still pools. Cup your hand. Eight orbs."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Seven letters went out to the magistrate this week, all in fresh Infro-ink. The magistrate will still read them when his great-grandchildren take office."));
			}
		});

		void AddOrbSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Infro Still-Pool"), "f_siauliai_46_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_46_1", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A still pool of red-veined water. Without an ink-maker's order, there's no reason to cup a hand under an orb*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);
				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This pool's orb has been taken. A small ripple remains where it drifted*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);
					if (SpawnTempMonsters(character, MonsterId.Infro_Blud, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}An Infro Blood rises from the pool - the red swirls toward you!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Cupping the orb..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(4));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650855, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					var currentCount = character.Inventory.CountItem(650855);
					character.ServerMessage(LF("Blood-orbs gathered: {0}/8", currentCount));
					if (currentCount >= 8)
						character.ServerMessage(L("{#FFD700}All orbs gathered! Return to Ink-Maker Milda.{/}"));
				}
				else
				{
					character.ServerMessage(L("Gathering interrupted. The orb sinks back."));
				}
			});
		}

		AddOrbSpot(1, -618, -1133, 0);
		AddOrbSpot(2, -441, -448, 90);
		AddOrbSpot(3, -744, -525, 180);
		AddOrbSpot(4, -199, 88, 270);
		AddOrbSpot(5, -845, 518, 0);
		AddOrbSpot(6, 138, 613, 90);
		AddOrbSpot(7, -579, -1342, 180);
		AddOrbSpot(8, -76, -914, 270);

		// =====================================================================
		// QUEST 1004: The Sun-Dials
		// =====================================================================
		AddNpc(20151, L("[Chronicler] Audrius"), "f_siauliai_46_1", 900, 500, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_46_1", 1004);

			dialog.SetTitle(L("Audrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A chronicler in a wide-brimmed hat reads a small brass sun-dial, then glances up at the noonlight. The noon is wrong - the sun is setting*{/}"));
				await dialog.Msg(L("The grove's four sun-dials should move with the sun. Three years ago, two of them stopped moving. This week, the count may have grown."));

				var response = await dialog.Select(L("Walk the four sun-dials. Read the shadow-position on each and compare to the actual sun. Will you?"),
					Option(L("I'll walk the four sun-dials"), "help"),
					Option(L("What does a stopped sun-dial mean?"), "info"),
					Option(L("Sun-dials are dull reading"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("A stopped dial means the grove-light at that dial has replaced the real sun. The dial sees noon forever. So do you, while you stand there - which is not as pleasant as it sounds."));
						break;

					case "info":
						await dialog.Msg(L("Permanent noon sounds lovely. It isn't. Nothing ages inside a stopped-dial zone - which sounds lovely too, and also isn't. A stopped zone catches time in amber. You step out younger than you stepped in, and the missing time goes somewhere."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself over a season. The season won't pass inside a stopped zone, though - so I'll have to walk them in real time, which is slow."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var dialsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked", 0);
				if (dialsChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*He reads each entry with a sinking expression*{/}"));
					await dialog.Msg(L("Dial One: shadow matches true sun. Moving. Dial Two: matches. Moving. Dial Three: stuck at noon. Dial Four: stuck at noon."));
					await dialog.Msg(L("Two stopped dials held steady for three years. Now there's still only two - I'd feared a third. Good news, measured in grove-terms."));
					await dialog.Msg(L("Chronicler's stipend. And a small brass travel-dial - carry it into a stopped zone and you'll see the fixed-noon shadow, which is the only honest way to tell you're inside one."));
					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four sun-dials. Shadow against true sun."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two dials stopped, two moving. The grove holds its strange balance. I file the reading and wait for next year's walk."));
			}
		});

		void AddSunDial(int dialNumber, string dialName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(dialName), "f_siauliai_46_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_46_1", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A small brass sun-dial set into a stone plinth. Without a chronicler's order, there's no reason to read the shadow*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_siauliai_46_1.Quest1004.Dial{dialNumber}";
				var checkedDial = character.Variables.Perm.GetBool(variableKey, false);
				if (checkedDial)
				{
					await dialog.Msg(L("{#666666}*You have already read this dial's shadow*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the shadow..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var dialsChecked = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked", dialsChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Dials read: {0}/4", dialsChecked + 1));
					if (dialsChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All dials read! Return to Chronicler Audrius.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddSunDial(1, "North Sun-Dial", "North Dial: shadow matches true sun. Moving normally.", 220, 305, 0);
		AddSunDial(2, "East Sun-Dial", "East Dial: shadow matches true sun. Moving normally.", 484, -261, 90);
		AddSunDial(3, "South Sun-Dial", "South Dial: shadow fixed at noon. Stopped. The air here feels warmer than the real hour.", 346, -813, 180);
		AddSunDial(4, "West Sun-Dial", "West Dial: shadow fixed at noon. Stopped. Your own shadow momentarily vanishes while you read.", -200, -994, 270);
	}
}

// Quest 1001
public class StatuesThatWalkAtDuskQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1001);
		SetName("Statues That Walk at Dusk");
		SetType(QuestType.Sub);
		SetDescription("Grove-Warden Rokas needs twenty Shardstatues broken before dusk so they cannot walk to the grove-spring tonight.");
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Grove-Warden] Rokas", "f_siauliai_46_1");
		AddObjective("killStatue", "Break Shardstatues in the grove",
			new KillObjective(20, new[] { MonsterId.Shardstatue }));
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}
}

// Quest 1002
public class TheSpringWaterFlaskQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1002);
		SetName("The Spring-Water Flask");
		SetType(QuestType.Sub);
		SetDescription("Herbalist Aldona's grove-spring flask must reach Grove-Tender Saule at the south plot and return with the brewed cordials.");
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Herbalist] Aldona", "f_siauliai_46_1");
		AddObjective("deliverFlask", "Deliver the flask and return with cordials",
			new VariableCheckObjective("Laima.Quests.f_siauliai_46_1.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1002.Delivered");
	}
}

// Quest 1003
public class InfroBloodOrbsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1003);
		SetName("Infro Blood-Orbs");
		SetType(QuestType.Sub);
		SetDescription("Ink-Maker Milda needs eight Infro Blood-orbs gathered from still pools for her century-lasting red ink.");
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Ink-Maker] Milda", "f_siauliai_46_1");
		AddObjective("collectOrbs", "Gather Infro Blood-orbs from still pools",
			new CollectItemObjective(650855, 8));
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
		AddReward(new ItemReward(640011, 1));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650855, character.Inventory.CountItem(650855), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650855, character.Inventory.CountItem(650855), InventoryItemRemoveMsg.Destroyed);
		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004
public class TheSunDialsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_46_1", 1004);
		SetName("The Sun-Dials");
		SetType(QuestType.Sub);
		SetDescription("Chronicler Audrius has asked you to read the four grove sun-dials and determine which still move with the real sun and which are frozen at noon.");
		SetLocation("f_siauliai_46_1");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Chronicler] Audrius", "f_siauliai_46_1");
		AddObjective("readDials", "Read the four grove sun-dials",
			new VariableCheckObjective("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked", 4, true));
		AddReward(new ExpReward(1900, 1430));
		AddReward(new SilverReward(3200));
		AddReward(new ItemReward(640082, 1));
		AddReward(new ItemReward(640003, 3));
		AddReward(new ItemReward(640006, 3));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1004.Dial{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_46_1.Quest1004.DialsChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_46_1.Quest1004.Dial{i}");
	}
}
