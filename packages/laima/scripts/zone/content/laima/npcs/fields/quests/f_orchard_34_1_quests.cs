//--- Melia Script ----------------------------------------------------------
// Alemeth Forest Quest NPCs
//--- Description -----------------------------------------------------------
// Fruit-themed, cheerful quests for the lively orchards of Alemeth Forest.
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

public class FOrchard341QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Apple Avalanche
		//-------------------------------------------------------------------------
		AddNpc(20059, L("[Orchardist] Bertie"), "f_orchard_34_1", 1000, -900, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1001);

			dialog.SetTitle(L("Bertie"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Oh thank goodness - a traveler! And a strong-looking one, too."));
				await dialog.Msg(L("The Red Truffles have been bouncing through my apple rows all week. Every time one thumps a tree, a dozen apples come down bruised."));
				await dialog.Msg(L("At this rate I won't have any clean fruit left for the harvest festival!"));

				var response = await dialog.Select(L("Can you help thin them out?"),
					Option(L("I'll chase off the truffles"), "help"),
					Option(L("Why are they bouncing?"), "info"),
					Option(L("Good luck with that"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Chase off twenty Red Truffles from the apple rows?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Wonderful! They're all through the eastern rows - you can't miss them, they're round and bright red."));
							await dialog.Msg(L("Don't hurt them too badly - a solid scare should do. Twenty ought to clear the orchard for a while."));
						}
						break;

					case "info":
						await dialog.Msg(L("It's the season! Red Truffles always bounce when they're happy, and the apple ground is springy."));
						await dialog.Msg(L("Normally it's charming. This year there's just too many of them."));
						break;

					case "leave":
						await dialog.Msg(L("Oh well. I'll try shaking a broom at them myself, I suppose."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("chaseTruffles", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Oh splendid! I can already hear the orchard settling down."));
					await dialog.Msg(L("Here, take these - some of our best apples, and a little coin besides. You've saved the festival!"));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still hearing them out there! Keep at it, dear."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The orchard is peaceful again. Come back in three weeks - you'll get the first bite of the festival cider!"));
			}
		});

		// Quest 2: The Giant Peach Hunt
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Fruit Picker] Annika"), "f_orchard_34_1", 0, 520, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1002);

			dialog.SetTitle(L("Annika"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Have you seen the peaches in Alemeth? They grow as big as your head here - I'm not exaggerating!"));
				await dialog.Msg(L("Festival judging is in two weeks and I need five perfectly ripe giants for my entry. The trees that produce them are scattered all over the forest."));
				await dialog.Msg(L("Sometimes when you shake a tree a squirrel jumps down, but that's half the fun."));

				var response = await dialog.Select(L("Will you help me find five?"),
					Option(L("I'll pick five giant peaches"), "help"),
					Option(L("Why are yours so big?"), "info"),
					Option(L("Shake your own trees"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Pick five giant peaches from the special trees for the festival entry?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Oh hooray! Look for the trees that curve down from the weight - those are the ones with giants ripe enough to pick."));
							await dialog.Msg(L("Be gentle! A bruised giant peach is just a sad giant peach."));
						}
						break;

					case "info":
						await dialog.Msg(L("Alemeth soil, Alemeth sun, and a little song from the pickers while we tend them - that's the whole secret."));
						await dialog.Msg(L("Or so my granny told me, anyway."));
						break;

					case "leave":
						await dialog.Msg(L("Oh come now, don't be grumpy. There's pie at the end of this, I promise!"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var peachCount = character.Inventory.CountItem(661093);

				if (peachCount >= 5)
				{
					await dialog.Msg(L("Five giant peaches, each the size of a melon! Oh, the judges are going to faint."));
					await dialog.Msg(L("Thank you, thank you! You've just about guaranteed me the blue ribbon."));

					character.Inventory.Remove(661093, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("Still shy a few. You've got {0} of five so far - keep going!", peachCount));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Blue ribbon! I won the blue ribbon! You're getting a whole peach pie, I don't care what Annika-from-last-week says about portions."));
			}
		});

		// Giant Peach Tree Points
		//-------------------------------------------------------------------------
		void AddPeachTree(int nodeNum, int x, int z, int direction)
		{
			AddNpc(46218, L("Bent Peach Tree"), "f_orchard_34_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_1", 1002);
				var variableKey = $"Laima.Quests.f_orchard_34_1.Quest1002.Peach{nodeNum}";
				var spawnedKey = $"Laima.Quests.f_orchard_34_1.Quest1002.Peach{nodeNum}.Spawned";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A peach tree bows under the weight of enormous fruit*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already picked this tree's giant peach*{/}"));
					return;
				}

				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 25)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Truffle_Red, 1, 80, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FFCC66}A startled Red Truffle tumbles out of the branches!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(
					L("Picking giant peach..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(661093, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Picked: Giant Peach"));

					var currentCount = character.Inventory.CountItem(661093);
					character.ServerMessage(LF("Giant peaches picked: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All five giants picked! Return to Annika.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You stepped back from the tree."));
				}
			});
		}

		AddPeachTree(1, 100, 620, 0);
		AddPeachTree(2, 300, 900, 0);
		AddPeachTree(3, -200, 730, 0);
		AddPeachTree(4, 500, 1050, 0);
		AddPeachTree(5, -320, 1150, 0);

		// Quest 3: Pie Contest Provisions
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Baker] Marja"), "f_orchard_34_1", -1000, 320, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1003);

			dialog.SetTitle(L("Marja"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Flour? Flour I've got. Butter? Mountains of it. Wildberries? Not nearly enough."));
				await dialog.Msg(L("And I can't get them myself because the Green Eldigos have taken over my berry patch. Those overgrown pups flop all over the bushes and squash every berry in sight."));

				var response = await dialog.Select(L("What do you need?"),
					Option(L("I'll clear the Eldigos and pick your berries"), "help"),
					Option(L("Eldigos are friendly, aren't they?"), "info"),
					Option(L("I don't do baking"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Clear fifteen Green Eldigos from the berry patch and gather four baskets of wildberries?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Oh bless you! Shoo the Eldigos - fifteen of them, at least - and then the bushes are yours."));
							await dialog.Msg(L("Four baskets is all I need. Any more and I'll run out of crust before I run out of filling!"));
						}
						break;

					case "info":
						await dialog.Msg(L("Oh, they're friendly enough. Too friendly! They're just big and clumsy and smell like wet dog."));
						await dialog.Msg(L("I've got nothing against the breed. I just need them somewhere other than my berry patch."));
						break;

					case "leave":
						await dialog.Msg(L("Your loss. Those wildberry pies practically fly off the festival tables."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearEldigos", out var killObj)) return;
				if (!quest.TryGetProgress("gatherBerries", out var berryObj)) return;

				if (killObj.Done && berryObj.Done)
				{
					await dialog.Msg(L("Four baskets! Oh, they're gorgeous - plump and dark as ink."));
					await dialog.Msg(L("Come back tomorrow and there'll be a pie with your name on it. Fair trade, I'd say!"));

					character.Inventory.Remove(650807, character.Inventory.CountItem(650807), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Clear more Eldigos from the patch. ");
					if (!berryObj.Done)
						status += L("Gather more wildberry baskets. ");

					await dialog.Msg(LF("Keep at it, dear. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("That pie went to the judges' table. Took second place! Not bad for a festival side-entry."));
			}
		});

		// Wildberry Basket Points
		//-------------------------------------------------------------------------
		void AddBerryBasket(int basketNum, int x, int z, int direction)
		{
			AddNpc(12080, L("Ripe Wildberry Bush"), "f_orchard_34_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_1", 1003);
				var variableKey = $"Laima.Quests.f_orchard_34_1.Quest1003.Basket{basketNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A wildberry bush heavy with fruit*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This bush has already been harvested*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Picking wildberries..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650807, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Gathered: Basket of Wildberries"));

					var currentCount = character.Inventory.CountItem(650807);
					character.ServerMessage(LF("Baskets gathered: {0}/4", currentCount));

					if (currentCount >= 4)
					{
						character.ServerMessage(L("{#FFD700}All baskets full! Return to Marja.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Picking paused."));
				}
			});
		}

		AddBerryBasket(1, -1100, 500, 0);
		AddBerryBasket(2, -1300, 200, 0);
		AddBerryBasket(3, -900, 100, 0);
		AddBerryBasket(4, -1200, 600, 0);

		// Quest 4: The Great Picnic Rescue
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Teacher] Elza"), "f_orchard_34_1", -800, -900, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1004);

			dialog.SetTitle(L("Elza"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Oh dear, oh dear. I was taking the children on their picnic and the Corpse Flowers flared up - you know how they do when they're startled."));
				await dialog.Msg(L("The children are safe, bless them, but all five picnic baskets got dropped and scattered. The sandwiches, the jam jars, everything!"));
				await dialog.Msg(L("And those Corpse Flowers are still huffing out spores around the southern clearing. The kids are devastated."));

				var response = await dialog.Select(L("Can you help me recover the baskets?"),
					Option(L("I'll find the baskets and shoo the flowers"), "help"),
					Option(L("Are the flowers really dangerous?"), "info"),
					Option(L("Reschedule the picnic"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Recover five scattered picnic baskets and kill twelve Green Corpse Flowers?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! The baskets are all through the southern clearing - bright checkered cloth, you can't miss them."));
							await dialog.Msg(L("And please, do shoo those flowers. Twelve of them should quiet the whole meadow down."));
						}
						break;

					case "info":
						await dialog.Msg(L("Not dangerous, exactly. But a lungful of their pollen will make you sneeze for an hour."));
						await dialog.Msg(L("Try sneezing while holding a jam sandwich! The children found out the hard way."));
						break;

					case "leave":
						await dialog.Msg(L("Oh, I couldn't - they've been talking about this picnic for two whole weeks!"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFlowers", out var killObj)) return;
				var basketCount = character.Inventory.CountItem(662098);

				if (killObj.Done && basketCount >= 5)
				{
					await dialog.Msg(L("All five baskets, and the meadow is quiet at last! Oh, the children are going to squeal when I tell them."));
					await dialog.Msg(L("Please, take this - I saved back some of the best cookies, just for you."));

					character.Inventory.Remove(662098, 5, InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("Shoo more Green Corpse Flowers. ");
					if (basketCount < 5)
						status += L("Find more scattered picnic baskets. ");

					await dialog.Msg(LF("Keep looking, dear. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("We had the picnic! The children said it was the best one ever - scattered-basket adventure and all."));
			}
		});

		// Picnic Basket Points
		//-------------------------------------------------------------------------
		void AddPicnicBasket(int basketNum, int x, int z, int direction)
		{
			AddNpc(47190, L("Dropped Picnic Basket"), "f_orchard_34_1", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_orchard_34_1", 1004);
				var variableKey = $"Laima.Quests.f_orchard_34_1.Quest1004.Basket{basketNum}";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A checkered picnic basket lies on its side in the grass*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*You've already picked up this basket*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Gathering picnic basket..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(662098, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);
					character.ServerMessage(L("Recovered: Picnic Basket"));

					var currentCount = character.Inventory.CountItem(662098);
					character.ServerMessage(LF("Baskets recovered: {0}/5", currentCount));

					if (currentCount >= 5)
					{
						character.ServerMessage(L("{#FFD700}All baskets recovered! Return to Elza.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("You let the basket sit for now."));
				}
			});
		}

		AddPicnicBasket(1, -700, -800, 0);
		AddPicnicBasket(2, -900, -1100, 0);
		AddPicnicBasket(3, -500, -1000, 0);
		AddPicnicBasket(4, -1000, -700, 0);
		AddPicnicBasket(5, -600, -1200, 0);

		// Quest 5: The Queen Truffle
		//-------------------------------------------------------------------------
		AddNpc(20109, L("[Forager] Pip"), "f_orchard_34_1", -1400, 0, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1005);
			var queenSpawnedKey = "Laima.Quests.f_orchard_34_1.Quest1005.QueenSpawned";

			dialog.SetTitle(L("Pip"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Friend, friend - listen close. There's a legend in these woods. They call her the Queen Truffle."));
				await dialog.Msg(L("Twice the size of a regular. Pink as dawn. Bounces higher than the lowest branch. Every festival, someone claims to have seen her, but nobody ever catches her."));
				await dialog.Msg(L("The truth is, she only comes out when the regulars thin out enough. If we kill ten, she'll waddle right into the clearing looking for leftover bounce-room."));

				var response = await dialog.Select(L("A Queen Truffle? Really?"),
					Option(L("I'll draw her out and catch her"), "help"),
					Option(L("Why do you want her?"), "info"),
					Option(L("Sounds like a tall tale"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill ten regular Red Truffles to draw out the Queen, then bring her down?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Yes! I've been waiting six festivals for someone brave enough. Kill the ten first - I'll wait here."));
							await dialog.Msg(L("When it's done, come see me and I'll point you to where she'll bounce out."));
						}
						break;

					case "info":
						await dialog.Msg(L("Festival committee pays a year's wages to whoever brings the Queen to the stew pot."));
						await dialog.Msg(L("Not that I'd eat her myself - I just want to look her in the mushroom-eyes once before she hops off again."));
						break;

					case "leave":
						await dialog.Msg(L("Skeptic! Fine. When someone else brings her in, don't say I didn't give you first offer."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killTruffles", out var killObj)) return;
				if (!quest.TryGetProgress("catchQueen", out var queenObj)) return;

				if (killObj.Done && queenObj.Done)
				{
					await dialog.Msg(L("She's down? She's really down? Oh, I've got to see the hat on her!"));
					await dialog.Msg(L("The committee will lose their minds. Take this reward with my compliments - and a slice of Queen stew, on the house!"));

					character.Variables.Perm.Remove(queenSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (killObj.Done && !queenObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(queenSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(queenSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Truffle_Big, 1, 150, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("The clearing is empty enough - she'll come. She always comes when there's space to bounce."));
							await dialog.Msg(L("{#FFCC66}Go, go! I can hear the branches already - she's on her way in!{/}"));
							character.ServerMessage(L("{#FFCC66}The Queen Truffle bounces out into the clearing!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("She's out there bouncing around - don't let her get away!"));
					}
				}
				else
				{
					await dialog.Msg(L("The clearing's still too full of regulars. Thin them first."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("They stuffed her and mounted her above the festival stage. Every year she'll be smiling down on the pie contest. Perfect."));
			}
		});

		// Quest 6: Festival Grounds
		//-------------------------------------------------------------------------
		AddNpc(155145, L("[Festival Organizer] Mira"), "f_orchard_34_1", 500, 330, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1006);

			dialog.SetTitle(L("Mira"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Two weeks until the Alemeth Harvest Festival and the fairgrounds are not ready. Not even close."));
				await dialog.Msg(L("Eldigos keep bedding down right on the dancing green, and Red Truffles have claimed the whole pie-judging pavilion for their afternoon bounces."));
				await dialog.Msg(L("I need both cleared - properly cleared, not just shooed - so the setup crews can start raising the tents."));

				var response = await dialog.Select(L("Can you clear the grounds?"),
					Option(L("I'll clear the fairgrounds"), "help"),
					Option(L("Can't the crews just work around them?"), "info"),
					Option(L("Move the festival"), "leave")
				);

				switch (response)
				{
					case "help":
						if (await dialog.YesNo(L("Kill twelve Green Eldigos and twelve Red Truffles from the festival grounds?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Thank you! Both species, twelve apiece. The grounds cover a fair stretch - you'll find them wandering the whole perimeter."));
							await dialog.Msg(L("Clear that, and I can finally sleep tonight."));
						}
						break;

					case "info":
						await dialog.Msg(L("Have you tried raising a maypole around a sleeping Eldigo? I have. Don't."));
						await dialog.Msg(L("The crews went on strike after last year. This year we get them cleared first. Non-negotiable."));
						break;

					case "leave":
						await dialog.Msg(L("Move the Alemeth Harvest Festival? It's literally named after the forest!"));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killEldigos", out var dogObj)) return;
				if (!quest.TryGetProgress("killTruffles", out var trufObj)) return;

				if (dogObj.Done && trufObj.Done)
				{
					await dialog.Msg(L("Grounds are clear! The setup crew is already staking out the main tent as we speak."));
					await dialog.Msg(L("You've saved this festival. I mean it. Take this, and reserve a seat for the cider tasting - on the house."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!dogObj.Done)
						status += L("Kill more Green Eldigos. ");
					if (!trufObj.Done)
						status += L("Kill more Red Truffles. ");

					await dialog.Msg(LF("Keep at it! {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Best festival in three years. The maypole didn't fall down once! Not once!"));
			}
		});

		// Quest 7 (Repeatable): Shepherd's Standing Order
		//-------------------------------------------------------------------------
		AddNpc(155018, L("[Shepherd] Pieter"), "f_orchard_34_1", -200, -500, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_orchard_34_1", 1007);
			const string truffleKillsKey = "Laima.Quests.f_orchard_34_1.Quest1007.TruffleKills";
			const int killsPerBatch = 25;
			const int expPerBatch = 5800;
			const int jobExpPerBatch = 4100;
			const int silverPerBatch = 7500;
			const int redOrbId = 685415;

			dialog.SetTitle(L("Pieter"));

			if (character.Quests.IsActive(questId))
			{
				var killCount = character.Variables.Perm.GetInt(truffleKillsKey, 0);
				var batches = killCount / killsPerBatch;
				var remainder = killCount % killsPerBatch;

				if (batches > 0)
				{
					character.Variables.Perm.Set(truffleKillsKey, remainder);

					character.GiveExp(expPerBatch * batches, jobExpPerBatch * batches, null);
					character.AddItem(ItemId.Vis, silverPerBatch * batches);
					character.Inventory.Add(redOrbId, batches, InventoryAddType.PickUp);

					await dialog.Msg(LF("{0} batch(es) delivered! The flock can feel the forest settling. Here's your family-recipe reward - keep those bouncers coming.", batches));
				}
				else
				{
					await dialog.Msg(LF("Still hearing wild ones bouncing out there - {0} of {1} thinned. Keep at it!", remainder, killsPerBatch));
				}
				return;
			}

			await dialog.Msg(L("Oh, hello! Don't mind the little ones - they're my flock. Red Truffles make splendid companions, you know. Fluffy as a cloud, and they bounce when they're happy."));
			await dialog.Msg(L("My troubles? The wild Truffles. They come bouncing in from the forest and frighten my flock silly. If you could thin them - and keep thinning them - I'd make it worth your while."));
			await dialog.Msg(L("Every twenty-five you bring down, I'll pay out in one go. Come back as often as you like - this order never closes."));

			var response = await dialog.Select(L("A standing order, then?"),
				Option(L("I'll take the contract"), "accept"),
				Option(L("Tell me about your flock"), "info"),
				Option(L("Maybe another time"), "leave")
			);

			switch (response)
			{
				case "accept":
					character.Quests.Start(questId);
					await dialog.Msg(L("Wonderful! Every twenty-five wild Red Truffles, one packet of payment. Come back any time you've got the count - I keep the pot on the stove."));
					await dialog.Msg(L("Oh, and the reward - it's a family specialty. You'll see."));
					break;

				case "info":
					await dialog.Msg(L("My grandmother kept the first fluffy Truffles, back before anyone thought they could be tame. She said they're loyal as dogs if you feed them right."));
					await dialog.Msg(L("She was absolutely correct. My flock of thirty would take a Gosaru down for me if I asked - though I never would, of course."));
					break;

				case "leave":
					await dialog.Msg(L("The contract stays open. Come back whenever you like!"));
					break;
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Apple Avalanche
//-----------------------------------------------------------------------------

public class AppleAvalancheQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1001);
		SetName(L("Apple Avalanche"));
		SetType(QuestType.Sub);
		SetDescription(L("Chase Red Truffles out of the orchardist's apple rows before the harvest festival."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Orchardist] Bertie"), "f_orchard_34_1");

		AddObjective("chaseTruffles", L("Chase off Red Truffles from the apple rows"),
			new KillObjective(20, new[] { MonsterId.Truffle_Red }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 15)); // Normal HP Potion
		AddReward(new ItemReward(640007, 15)); // Normal SP Potion
	}
}

// Quest 1002 CLASS: The Giant Peach Hunt
//-----------------------------------------------------------------------------

public class TheGiantPeachHuntQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1002);
		SetName(L("The Giant Peach Hunt"));
		SetType(QuestType.Sub);
		SetDescription(L("Pick five giant peaches from the bowed trees of Alemeth for Annika's festival entry."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Fruit Picker] Annika"), "f_orchard_34_1");

		AddObjective("pickPeaches", L("Pick giant peaches"),
			new CollectItemObjective(661093, 5));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(60000));
		AddReward(new ItemReward(640086, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(661093, character.Inventory.CountItem(661093), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1002.Peach{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1002.Peach{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(661093, character.Inventory.CountItem(661093), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1002.Peach{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1002.Peach{i}.Spawned");
		}
	}
}

// Quest 1003 CLASS: Pie Contest Provisions
//-----------------------------------------------------------------------------

public class PieContestProvisionsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1003);
		SetName(L("Pie Contest Provisions"));
		SetType(QuestType.Sub);
		SetDescription(L("Clear Eldigos from Marja's berry patch and gather wildberry baskets for her festival pies."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Baker] Marja"), "f_orchard_34_1");

		AddObjective("clearEldigos", L("Shoo Green Eldigos from the berry patch"),
			new KillObjective(15, new[] { MonsterId.Eldigo_Green }));

		AddObjective("gatherBerries", L("Gather wildberry baskets"),
			new CollectItemObjective(650807, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(640013, 13)); // Recovery Potion
		AddReward(new ItemReward(926012, 1)); // Recipe - Shield Breaker
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650807, character.Inventory.CountItem(650807), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1003.Basket{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650807, character.Inventory.CountItem(650807), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1003.Basket{i}");
		}
	}
}

// Quest 1004 CLASS: The Great Picnic Rescue
//-----------------------------------------------------------------------------

public class TheGreatPicnicRescueQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1004);
		SetName(L("The Great Picnic Rescue"));
		SetType(QuestType.Sub);
		SetDescription(L("Recover the school picnic baskets scattered across the southern meadow and shoo the sneeze-inducing Corpse Flowers."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Teacher] Elza"), "f_orchard_34_1");

		AddObjective("killFlowers", L("Shoo Green Corpse Flowers from the meadow"),
			new KillObjective(12, new[] { MonsterId.Corpse_Flower_Green }));

		AddObjective("recoverBaskets", L("Recover scattered picnic baskets"),
			new CollectItemObjective(662098, 5));

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
		character.Inventory.Remove(662098, character.Inventory.CountItem(662098), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1004.Basket{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662098, character.Inventory.CountItem(662098), InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 5; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_orchard_34_1.Quest1004.Basket{i}");
		}
	}
}

// Quest 1005 CLASS: The Queen Truffle
//-----------------------------------------------------------------------------

public class TheQueenTruffleQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1005);
		SetName(L("The Queen Truffle"));
		SetType(QuestType.Sub);
		SetDescription(L("Thin the regular Red Truffles enough to draw out the legendary Queen Truffle for the forager's festival trophy."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Forager] Pip"), "f_orchard_34_1");

		AddObjective("killTruffles", L("Kill Red Truffles to empty the clearing"),
			new KillObjective(10, new[] { MonsterId.Truffle_Red }));

		AddObjective("catchQueen", L("Catch the Queen Truffle"),
			new KillObjective(1, new[] { MonsterId.Truffle_Big }));

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
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_1.Quest1005.QueenSpawned");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_orchard_34_1.Quest1005.QueenSpawned");
	}
}

// Quest 1006 CLASS: Festival Grounds
//-----------------------------------------------------------------------------

public class FestivalGroundsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1006);
		SetName(L("Festival Grounds"));
		SetType(QuestType.Sub);
		SetDescription(L("Kill Green Eldigos and Red Truffles from the Alemeth festival fairgrounds so the setup crews can raise the tents."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Festival Organizer] Mira"), "f_orchard_34_1");

		AddObjective("killEldigos", L("Kill Green Eldigos from the fairgrounds"),
			new KillObjective(12, new[] { MonsterId.Eldigo_Green }));

		AddObjective("killTruffles", L("Kill Red Truffles from the fairgrounds"),
			new KillObjective(12, new[] { MonsterId.Truffle_Red }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(68000));
		AddReward(new ItemReward(640086, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640004, 14)); // Normal HP Potion
		AddReward(new ItemReward(640007, 14)); // Normal SP Potion
		AddReward(new ItemReward(502202, 1)); // Valtas Plate Gauntlents
	}
}

// Quest 1007 CLASS: Shepherd's Standing Order (Repeatable)
//-----------------------------------------------------------------------------

public class ShepherdsStandingOrderQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_1", 1007);
		SetName(L("Shepherd's Standing Order"));
		SetType(QuestType.Repeat);
		SetDescription(L("Pieter the Truffle shepherd keeps an open standing order on wild Red Truffles. Every twenty-five you thin earns one packet of payment - come back as often as you like."));
		SetLocation("f_orchard_34_1");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Shepherd] Pieter"), "f_orchard_34_1");

		AddObjective("thinTruffles", L("Thin wild Red Truffles (rewarded per 25)"),
			new UnlimitedKillObjective((mob, ch) =>
			{
				if (mob.Id != MonsterId.Truffle_Red)
					return false;

				ch.Variables.Perm.Set(
					"Laima.Quests.f_orchard_34_1.Quest1007.TruffleKills",
					ch.Variables.Perm.GetInt("Laima.Quests.f_orchard_34_1.Quest1007.TruffleKills", 0) + 1
				);

				return true;
			}));
	}
}
