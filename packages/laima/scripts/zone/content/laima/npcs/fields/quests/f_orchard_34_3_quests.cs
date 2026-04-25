//--- Melia Script ----------------------------------------------------------
// Barha Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Cheerful woodland quests for Barha Forest, continuing the orchard tone.
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

public class FOrchard343QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: The Frog Chorus
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Villager] Lida"), "f_orchard_34_3", -1500, 500, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1001);

			dialog.SetTitle(L("Lida"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Have you ever tried to sleep through a Flying Flog chorus? It's like being in a bathtub with twenty opera singers."));
				await dialog.Msg(L("The whole west marsh is thick with them. Four nights running - nobody in my cottage has slept more than an hour."));
				await dialog.Msg(L("We don't want them gone for good. Just... thinned. Dramatically."));

				var response = await dialog.Select(L("How thinned?"),
					Option(L("I'll silence twenty-two frogs"), "help"),
					Option(L("Earplugs don't work?"), "info"),
					Option(L("Move houses"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Silence twenty-two Gray Winged Frogs from the west marsh?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Bless you! The marsh sits right past the treeline - you'll hear them long before you see them."));
							await dialog.Msg(L("Twenty-two should restore some semblance of silence. Please. I'm begging."));
						}
						break;

					case "info":
						await dialog.Msg(L("Wax earplugs? Tried them. Cotton? Tried it. Prayer? Oh, I've tried prayer."));
						await dialog.Msg(L("These frogs croak through walls. I swear they aim."));
						break;

					case "leave":
						await dialog.Msg(L("Move? My grandmother built this cottage. I'm not letting the frogs win."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("silenceFrogs", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Oh, the silence. The sweet, sweet silence."));
					await dialog.Msg(L("Take this with my deepest thanks. And a pillow - you deserve a really good nap."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hearing the croak-opera out there. Keep silencing them!"));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Slept twelve hours straight last night. Twelve! I'd forgotten what dreams looked like."));
			}
		});

		// Quest 2: Pollen Run
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Perfumer] Inara"), "f_orchard_34_3", 0, 400, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1002);

			dialog.SetTitle(L("Inara"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Green Rafflesia pollen. Ever smelled it? Heaven in a sneeze."));
				await dialog.Msg(L("It's the base note for my entire festival-edition line. I'd harvest it myself but Rafflesia don't exactly hold still for it."));
				await dialog.Msg(L("Five pollen sacs, five intact flowers. That's all I need."));

				var response = await dialog.Select(L("Will you gather five?"),
					Option(L("I'll collect the pollen"), "help"),
					Option(L("Do Rafflesia bite?"), "info"),
					Option(L("Use another flower"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Gather five Green Rafflesia pollen sacs from intact flowers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wonderful! Twist the sac off at the base - the pollen stays dry that way."));
							await dialog.Msg(L("And do keep your distance. Some of them are lungers."));
						}
						break;

					case "info":
						await dialog.Msg(L("Bite? Technically no. Snap shut around your arm like a bear trap? Absolutely."));
						await dialog.Msg(L("I've seen pickers walk away with a flower stuck to their elbow for three days."));
						break;

					case "leave":
						await dialog.Msg(L("There is no other flower. That's rather the point."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var pollenCount = character.Inventory.CountItem(650581);

				if (pollenCount >= 5)
				{
					await dialog.Msg(L("Five pristine sacs! The scent is already filling my shop - I can retire on this batch alone."));
					await dialog.Msg(L("Take this. A little silver, and a few sample bottles from the last batch."));

					character.Inventory.Remove(650581, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Still gathering? You have {0} of five.", pollenCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The festival perfume launched! The judges called it 'improbably wearable.' High praise."));
			}
		});

		// Rafflesia Pollen Points
		//-------------------------------------------------------------------------
		void AddRafflesiaBloom(int nodeNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Intact Green Rafflesia"), "f_orchard_34_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_3", 1002);
				var variableKey = $"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A Rafflesia bloom hangs heavy with pollen - motionless, for now*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already harvested this bloom's pollen sac*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 35)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Rafflesia_Green, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#99CC66}A pair of Rafflesia lunge from the undergrowth!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Twisting off pollen sac..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650581, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Harvested: Rafflesia Pollen Sac"));

					var currentCount = character.Inventory.CountItem(650581);
					character.ServerMessage(LF("Pollen sacs gathered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All five sacs gathered! Return to Inara.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You stepped back just in time."));
				}
			});
		}

		AddRafflesiaBloom(1, 1100, -280, 0);
		AddRafflesiaBloom(2, -365, -145, 0);
		AddRafflesiaBloom(3, 900, -150, 0);
		AddRafflesiaBloom(4, -200, 50, 0);
		AddRafflesiaBloom(5, 1200, 100, 0);

		// Quest 3: Siaulav Eviction
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Ranger] Kaspar"), "f_orchard_34_3", -1300, 600, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1003);

			dialog.SetTitle(L("Kaspar"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Siaulav Archers have squatted the northwest ridge for three months. Not legally, mind you - that's crown land."));
				await dialog.Msg(L("They've built four nest-roosts up there. Little woven platforms with feathers and arrow-fletchings hanging everywhere. Charming, if you ignore that they keep shooting at my patrols."));

				var response = await dialog.Select(L("Want them cleared out?"),
					Option(L("I'll evict them and dismantle the roosts"), "help"),
					Option(L("Can't you just negotiate?"), "info"),
					Option(L("Let them stay"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill fifteen Siaulav Archers and dismantle four nest-roosts?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Good. The roosts are platforms at the tree joints - you'll see the hanging fletchings from a good distance."));
							await dialog.Msg(L("Wear something that doesn't rustle. Siaulav hear everything."));
						}
						break;

					case "info":
						await dialog.Msg(L("They don't negotiate. They trill. And then they shoot. It's a short conversation."));
						break;

					case "leave":
						await dialog.Msg(L("Tell that to the crown. I'd love to 'let them stay' but the paperwork says otherwise."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var killObj)) return;
				if (!quest.TryGetProgress("dismantleRoosts", out var roostObj)) return;

				if (killObj.Done && roostObj.Done)
				{
					await dialog.Msg(L("Ridge is clear. Paperwork satisfied. Crown happy."));
					await dialog.Msg(L("Your share is in this pouch. Honest crown pay, no tricks."));

					character.Inventory.Remove(650750, character.Inventory.CountItem(650750), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Kill more Siaulav Archers. ");
					if (!roostObj.Done)
						status += L("Dismantle more nest-roosts. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Ridge has stayed quiet. Patrols can walk the whole length without ducking."));
			}
		});

		// Nest-Roost Dismantle Points
		//-------------------------------------------------------------------------
		void AddNestRoost(int roostNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Siaulav Nest-Roost"), "f_orchard_34_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_3", 1003);
				var variableKey = $"Laima.Quests.f_orchard_34_3.Quest1003.Roost{roostNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_34_3.Quest1003.Roost{roostNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A woven platform hangs between branches, fletchings swaying*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This roost has already been dismantled*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 40)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Siaulav_Bow_Orange, 2, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF9966}Furious Siaulav Archers drop from the canopy!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Dismantling roost..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650750, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Dismantled: Siaulav Nest-Roost"));

					var currentCount = character.Inventory.CountItem(650750);
					character.ServerMessage(LF("Roosts dismantled: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All roosts down! Return to Kaspar.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You let the roost hang for now."));
				}
			});
		}

		AddNestRoost(1, -1420, 650, 0);
		AddNestRoost(2, -1300, 800, 0);
		AddNestRoost(3, -1500, 400, 0);
		AddNestRoost(4, -1250, 550, 0);

		// Quest 4: The Missing Apprentice
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Forager] Master Vondel"), "f_orchard_34_3", 200, 600, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1004);

			dialog.SetTitle(L("Vondel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("My apprentice Tomas went out three days ago with a satchel of marking-tags. Supposed to count and tag every Big Red Griba in Barha."));
				await dialog.Msg(L("He hasn't come back. The Rafflesia are thick in the center of the forest - he must have gotten overwhelmed."));
				await dialog.Msg(L("I keep hoping he just fell asleep under a tree. But the tags should still be out there either way."));

				var response = await dialog.Select(L("What do you need?"),
					Option(L("I'll recover the tags and clear the Rafflesia"), "help"),
					Option(L("What's a marking-tag?"), "info"),
					Option(L("Sorry for your loss"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Recover five marking-tags and kill twelve Green Rafflesia blocking the survey route?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you. The tags are yellow cloth wrapped around little wooden posts - he'd have placed them near the Gribas, or dropped them along the way."));
							await dialog.Msg(L("Five tags is enough to know where he made it to. Please, tell me it's not as far as I fear."));
						}
						break;

					case "info":
						await dialog.Msg(L("A wooden post wrapped in yellow cloth, numbered. You tie it near a specimen so surveyors can find it later."));
						await dialog.Msg(L("Tomas made thirty of them himself. Each one's got his initials burned in."));
						break;

					case "leave":
						await dialog.Msg(L("He isn't lost - he's just late. He's always just late."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRafflesia", out var killObj)) return;
				var tagCount = character.Inventory.CountItem(650672);

				if (killObj.Done && tagCount >= 5)
				{
					await dialog.Msg(L("Five tags. His initials on all of them."));
					await dialog.Msg(L("The last tag was at the center. He made it farther than I dared hope - but he didn't make it back."));
					await dialog.Msg(L("Thank you. I'll retire the Griba survey. It was never worth this. Take this - he would have wanted you to have it."));

					character.Inventory.Remove(650672, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Clear more Rafflesia from the route. ");
					if (tagCount < 5)
						status += L("Recover more marking-tags. ");

					await dialog.Msg(LF("Keep searching. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I buried the tags in the cedar grove. No body to bury, but the tags were his writing. It'll have to do."));
			}
		});

		// Marking Tag Recovery Points
		//-------------------------------------------------------------------------
		void AddMarkingTag(int tagNum, int x, int z, int direction)
		{
			AddNpc(47190, L("Apprentice's Marking-Tag"), "f_orchard_34_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_3", 1004);
				var variableKey = $"Laima.Quests.f_orchard_34_3.Quest1004.Tag{tagNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A yellow-wrapped wooden post lies in the leaf litter*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already recovered this marking-tag*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Recovering tag..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650672, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Marking-Tag"));

					var currentCount = character.Inventory.CountItem(650672);
					character.ServerMessage(LF("Tags recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All five tags recovered! Return to Vondel.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You left the tag where it lay."));
				}
			});
		}

		AddMarkingTag(1, 58, 566, 0);
		AddMarkingTag(2, -100, 450, 0);
		AddMarkingTag(3, 250, 650, 0);
		AddMarkingTag(4, 0, 700, 0);
		AddMarkingTag(5, 150, 520, 0);

		// Quest 5: The Griba Baron
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Bounty Hunter] Lark"), "f_orchard_34_3", 500, -200, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1005);
			var baronSpawnedKey = "Laima.Quests.f_orchard_34_3.Quest1005.BaronSpawned";

			dialog.SetTitle(L("Lark"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("There's a rumour circling the foragers' guild. The Big Red Gribas in Barha have a... baron."));
				await dialog.Msg(L("Twice as tall as the regulars, stalks like saplings, cap big enough to shade a picnic. Nobody's seen him for more than thirty seconds before he lumbers back into the brush."));
				await dialog.Msg(L("The Rafflesia around him act as his bodyguards. Thin them - say, ten - and his pride brings him out."));

				var response = await dialog.Select(L("I'll hunt the Baron."),
					Option(L("Count me in"), "help"),
					Option(L("Why do you want him?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Thin ten Green Rafflesia to draw out the Griba Baron, then bring him down?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Ten Rafflesia first. Come back when the count's done - I'll sense when he's near."));
							await dialog.Msg(L("When he emerges, don't dawdle. A Baron-sized Ent hits like a falling tree because it basically is one."));
						}
						break;

					case "info":
						await dialog.Msg(L("Mounted cap for the festival stage. The organizers want it as a centerpiece - they'll pay a barony's ransom for one."));
						await dialog.Msg(L("No pun intended. Actually, yes, pun intended."));
						break;

					case "leave":
						await dialog.Msg(L("Bounty's posted. Come back if you change your mind."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killGuards", out var guardObj)) return;
				if (!quest.TryGetProgress("killBaron", out var baronObj)) return;

				if (guardObj.Done && baronObj.Done)
				{
					await dialog.Msg(L("He's down? You actually dropped the Baron?"));
					await dialog.Msg(L("The festival committee's going to faint. Cap will be on the main stage by sundown. Your bounty, paid in full."));

					character.Variables.Perm.Remove(baronSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (guardObj.Done && !baronObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(baronSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(baronSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Mushroom_Ent_Red, 1, 130, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("The Rafflesia are thinned enough. I can feel the ground shifting - he's coming out."));
							await dialog.Msg(L("{#FF6666}Go, go! He won't stay in the open if the Rafflesia regrow!{/}"));
							character.ServerMessage(L("{#FF6666}The Griba Baron lumbers into view!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's out there, tall as a church steeple. Don't let him retreat."));
					}
				}
				else
				{
					await dialog.Msg(L("The guards are still thick. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cap's on the festival stage. Kids are taking turns standing under it like it's a parasol."));
			}
		});

		// Quest 6: Barha Crossroads
		//-------------------------------------------------------------------------
		AddNpc(155145, L("[Pathfinder] Bram"), "f_orchard_34_3", -700, 400, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_3", 1006);

			dialog.SetTitle(L("Bram"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The Barha Crossroads run from Alemeth through here to the pilgrim road. Right now it's a shooting gallery."));
				await dialog.Msg(L("Siaulav Archers snipe from the ridge. Flying Flogs dive-bomb the carts. The caravans won't risk it anymore."));

				var response = await dialog.Select(L("What'll clear it?"),
					Option(L("I'll clear both groups"), "help"),
					Option(L("Which is worse?"), "info"),
					Option(L("Use another route"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Siaulav Archers and twelve Flying Flogs along the Crossroads?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Twelve of each. The Crossroads are the main stretch - you'll run into both species on every bend."));
							await dialog.Msg(L("When it's done, the carts roll again, and I'll sleep without flinching at every creak."));
						}
						break;

					case "info":
						await dialog.Msg(L("Siaulav shoot at your head. Flying Flogs land on your head. Pick your poison."));
						break;

					case "leave":
						await dialog.Msg(L("Not really an option. This is the only route between Alemeth and the pilgrim road."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killArchers", out var archObj)) return;
				if (!quest.TryGetProgress("killFrogs", out var frogObj)) return;

				if (archObj.Done && frogObj.Done)
				{
					await dialog.Msg(L("Crossroads is clear. First caravan rolled through an hour ago - the drivers looked giddy."));
					await dialog.Msg(L("Take your pay. You've opened the only road between two forests."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!archObj.Done)
						status += L("Kill more Siaulav Archers. ");
					if (!frogObj.Done)
						status += L("Kill more Flying Flogs. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The Crossroads are running on schedule. Three caravans a day, not one arrow loosed."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Frog Chorus
//-----------------------------------------------------------------------------

public class TheFrogChorusQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1001);
		SetName(L("The Frog Chorus"));
		SetType(QuestType.Sub);
		SetDescription(L("Silence the Gray Winged Frogs in the west marsh before Lida loses what remains of her sanity."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Villager] Lida"), "f_orchard_34_3");

		AddObjective("silenceFrogs", L("Silence Gray Winged Frogs"),
			new KillObjective(22, new[] { MonsterId.Flying_Flog_White }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: Pollen Run
//-----------------------------------------------------------------------------

public class PollenRunQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1002);
		SetName(L("Pollen Run"));
		SetType(QuestType.Sub);
		SetDescription(L("Gather Green Rafflesia pollen sacs for the perfumer Inara's festival-edition fragrance."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Perfumer] Inara"), "f_orchard_34_3");

		AddObjective("gatherPollen", L("Gather Rafflesia pollen sacs"),
			new CollectItemObjective(650581, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650581, character.Inventory.CountItem(650581), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650581, character.Inventory.CountItem(650581), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1002.Pollen{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: Siaulav Eviction
//-----------------------------------------------------------------------------

public class SiaulavEvictionQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1003);
		SetName(L("Siaulav Eviction"));
		SetType(QuestType.Sub);
		SetDescription(L("Evict the squatting Siaulav Archers from the northwest ridge and dismantle their nest-roosts."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ranger] Kaspar"), "f_orchard_34_3");

		AddObjective("killArchers", L("Kill Siaulav Archers"),
			new KillObjective(15, new[] { MonsterId.Siaulav_Bow_Orange }));

		AddObjective("dismantleRoosts", L("Dismantle nest-roosts"),
			new CollectItemObjective(650750, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
		AddReward(new ItemReward(640013, 14)); // Recovery Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650750, character.Inventory.CountItem(650750), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1003.Roost{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1003.Roost{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650750, character.Inventory.CountItem(650750), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1003.Roost{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1003.Roost{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Missing Apprentice
//-----------------------------------------------------------------------------

public class TheMissingApprenticeQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1004);
		SetName(L("The Missing Apprentice"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover the marking-tags Tomas left behind and kill the Rafflesia that blocked his survey route."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forager] Master Vondel"), "f_orchard_34_3");

		AddObjective("killRafflesia", L("Kill Rafflesia blocking the route"),
			new KillObjective(12, new[] { MonsterId.Rafflesia_Green }));

		AddObjective("recoverTags", L("Recover apprentice's marking-tags"),
			new CollectItemObjective(650672, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
		AddReward(new ItemReward(941035, 1)); // Recipe - Ferret Marauder Shield
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650672, character.Inventory.CountItem(650672), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1004.Tag{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650672, character.Inventory.CountItem(650672), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_3.Quest1004.Tag{i}");
		}
	}
}

// Quest 1005 CLASS: The Griba Baron
//-----------------------------------------------------------------------------

public class TheGribaBaronQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1005);
		SetName(L("The Griba Baron"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the Rafflesia guarding the legendary Griba Baron, then bring him down for the festival centerpiece."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Lark"), "f_orchard_34_3");

		AddObjective("killGuards", L("Kill Rafflesia guarding the Baron"),
			new KillObjective(10, new[] { MonsterId.Rafflesia_Green }));

		AddObjective("killBaron", L("Defeat the Griba Baron"),
			new KillObjective(1, new[] { MonsterId.Mushroom_Ent_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
		AddReward(new ItemReward(640013, 14)); // Recovery Potion
		AddReward(new ItemReward(531122, 1)); // Plate Armor
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_3.Quest1005.BaronSpawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_3.Quest1005.BaronSpawned");
	}
}

// Quest 1006 CLASS: Barha Crossroads
//-----------------------------------------------------------------------------

public class BarhaCrossroadsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_3", 1006);
		SetName(L("Barha Crossroads"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Siaulav Archers and Flying Flogs from the Barha Crossroads so the caravans between forests can roll again."));
		SetLocation("f_orchard_34_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Pathfinder] Bram"), "f_orchard_34_3");

		AddObjective("killArchers", L("Kill Siaulav Archers from the Crossroads"),
			new KillObjective(12, new[] { MonsterId.Siaulav_Bow_Orange }));

		AddObjective("killFrogs", L("Kill Flying Flogs from the Crossroads"),
			new KillObjective(12, new[] { MonsterId.Flying_Flog_White }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(512207, 1)); // Hasta Plate Boots
	}
}
