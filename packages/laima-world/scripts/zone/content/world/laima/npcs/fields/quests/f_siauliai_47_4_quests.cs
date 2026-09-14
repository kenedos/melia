//--- Melia Script ----------------------------------------------------------
// Baron Allerno - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_siauliai_47_4 map. The grain estate that
// feeds the farm cluster, and the hollow that is eating its stores.
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

public class FSiauliai474QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: Forty-One Hollow Sacks
		// =====================================================================
		// Granary Warden Stepas - the estate cannot fill the shelter requisition
		//---------------------------------------------------------------------
		AddNpc(20143, L("[Granary Warden] Stepas"), "f_siauliai_47_4", 323, -684, 334, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_47_4", 1001);

			dialog.SetTitle(L("Stepas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A warden with a split sack open across his knees, one hand buried in the grain to the wrist*{/}"));
				await dialog.Msg(L("You'll have to wait a breath — I want to finish this count before I lose my place. Tenants' Farm wants oats for 300. This estate has 5 stores and it has filled that order every autumn since before I was born."));
				await dialog.Msg(L("I opened 41 sacks yesterday and 41 of them were hollow in the middle. Kill 25 of the Orange Hammings working the rows and bring me 6 of the seed bags they have dragged off, and I will at least know what the birds are taking."));

				var response = await dialog.Select(L("Will you walk the rows for me?"),
					Option(L("I'll clear the rows"), "help"),
					Option(L("Hollow how?"), "info"),
					Option(L("Buy your oats elsewhere"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The birds work the south rows in the morning and the west rows after noon. Take them off the ground, not off the ricks - they drop everything they are carrying when they come down."));
						await dialog.Msg(L("And bring the bags to me sealed. If one is open I cannot tell you whether it went bad in the field or in my store."));
						break;

					case "info":
						await dialog.Msg(L("Sound sack. Sound seal. Sound grain for two hands deep. Then your fingers go into something with the texture of wet ash and it comes out grey to the elbow."));
						await dialog.Msg(L("Not damp. Damp I know. Damp spreads from the bottom because that is where the water is. This starts in the middle of a sack sitting on a dry board, and it works outward."));
						break;

					case "leave":
						await dialog.Msg(L("From where? Klaipeda triples the price the moment they hear the word shelter. That is why the cluster keeps a granary in the first place."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killHammings", out var killObj)) return;
				if (!quest.TryGetProgress("collectSeedBags", out var bagObj)) return;

				if (killObj.Done && bagObj.Done)
				{
					await dialog.Msg(L("{#666666}*He slits all 6 bags open in a row and stands looking down at them*{/}"));
					await dialog.Msg(L("Every one hollow. These never saw the inside of my store. The birds took them out of the field and they were already wrong in the field."));
					await dialog.Msg(L("So it is not my boards, and it is not my seals, and it is not damp. It is the ground. Take this - the requisition purse, and it is going to sit unspent a while longer."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Rows are quieter. I still need 6 sealed bags off them, and sealed means sealed."));
				}
				else
				{
					await dialog.Msg(L("South rows in the morning, west rows after noon. 25 birds and 6 bags, and I will stop guessing."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have written to Tenants' Farm that the oats are late and not why. Mykolas has 7 barns to fill and I would rather send him a short letter than a frightening one."));
			}
		});

		// =====================================================================
		// QUEST 1002: Three Lengths Left
		// =====================================================================
		// Carter Marius - the Myrkiti relief cart has nothing to lash a load with
		//---------------------------------------------------------------------
		AddNpc(20118, L("[Carter] Marius"), "f_siauliai_47_4", -1135, -883, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_47_4", 1002);

			dialog.SetTitle(L("Marius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A carter is coiling a frayed rope over his elbow, counting the coils under his breath*{/}"));
				await dialog.Msg(L("Ah — you'll do nicely! Strong back on you, and Goddess knows I need one that isn't mine for once. Nine years I've run the Myrkiti road out of this gate, would you believe. This estate owned 14 lengths of good rope when I started. It owns 3 now. Three! I've counted them personally, more than once, hoping I'd miscounted."));
				await dialog.Msg(L("Orange Popolions, if you can believe it — pull the rick-covers clean apart and drag the cordage off into the west scrub to bed down in, of all the uses for good rope. Bring me back 6 lengths and Myrkiti finally gets its cart."));

				var response = await dialog.Select(L("Can you get out there and pull my rope back?"),
					Option(L("I'll fetch the rope"), "help"),
					Option(L("Why not buy more?"), "info"),
					Option(L("Lash it with something else"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(662018, 2, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the hardtack. I am sorry - it is twice-baked and it tastes like a roof tile, but it is the only thing in this estate I can hand a stranger and be certain of."));
						await dialog.Msg(L("They bed thickest west and north of the gate. Cut the coil out from under them, do not pull it - a wet rope you have dragged through a nest is a rope I cannot use."));
						break;

					case "info":
						await dialog.Msg(L("With what? The steward's post has been empty since spring and nobody has signed a purchase order in this house for 5 months. Stepas signs the food orders because somebody has to and he will not sign anything else."));
						await dialog.Msg(L("The Baron has been at the war 3 years. I do not think anybody here has admitted out loud yet that we are running his estate by habit."));
						break;

					case "leave":
						await dialog.Msg(L("Then the load shifts on the Myrkiti hill and I spend a day picking sacks out of a ditch. I have done that. Once was enough."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectRope", out var ropeObj)) return;

				if (ropeObj.Done)
				{
					await dialog.Msg(L("{#666666}*He runs each length through his hands from end to end before he will accept it*{/}"));
					await dialog.Msg(L("6 good, 3 mine, that is 9. That is a load lashed twice over and a spare on the tail-board."));
					await dialog.Msg(L("Myrkiti has demon-pollen in the air and half its hands working in stitched masks. They are not going to care what the cart is tied with. But I will."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("West and north of the gate, in under the scrub. 6 lengths. Cut them out, do not drag them."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cart went out at dawn and came back empty, which is the whole of what I want from a road. Goda sent word she can stitch 40 more masks with the cloth on it."));
			}
		});

		// =====================================================================
		// QUEST 1003: Six Carts in a Month
		// =====================================================================
		// Roadwarden Norkus - the Gytis road post
		//---------------------------------------------------------------------
		AddNpc(20144, L("[Roadwarden] Norkus"), "f_siauliai_47_4", 1409, -166, 302, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_47_4", 1003);

			dialog.SetTitle(L("Norkus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A road officer stands over two pedlar packs laid out in the grass, both still buckled*{/}"));
				await dialog.Msg(L("Stop right there and state your— oh. Sorry. Force of habit, that — you're the first traveler I've had all week and I nearly forgot how the line goes. This is the Gytis road. Used to carry 6 carts a day, this road."));
				await dialog.Msg(L("Now it carries 6 in a month, and these 2 packs have lain right there for 11 days without a soul coming back for them. Kill 18 of the Spion Mages and maybe, maybe, I'll have a road again instead of a very long, very quiet walk."));

				var response = await dialog.Select(L("Will you clear the road for me?"),
					Option(L("I'll clear them off the road"), "help"),
					Option(L("What are they after?"), "info"),
					Option(L("Post more guards"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They stand off and throw. Close on them and they are nothing - it is the 40 paces before you get there that kills carters."));
						await dialog.Msg(L("Work the cut east of here first. That is where they thin out, and thin is where you want to start a fight, not finish one."));
						break;

					case "info":
						await dialog.Msg(L("Robbery, is what I put in the book. But I have watched them let a loaded cart go past and keep walking west, and I have no line in the book for that."));
						await dialog.Msg(L("They are going somewhere. Every one of them is going the same somewhere. I would very much like to be wrong about that."));
						break;

					case "leave":
						await dialog.Msg(L("There is one guard on the Gytis road and you are looking at him. The Baron's men marched 3 years ago and the post was never filled."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMages", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks 200 paces up the road and back before he says anything*{/}"));
					await dialog.Msg(L("Clear. First clear hour on this road since spring. Take the post's month-purse, it has nothing else to be spent on."));
					await dialog.Msg(L("I will tell you what I did not put in the book. They were all walking northwest. Not toward the carts. Past them."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Start in the cut east of the post. 18 of them. Close the distance or they will keep you at 40 paces all day."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("11 carts through this week. And a pedlar came back down for his packs and could not tell me where he had been for a fortnight."));
			}
		});

		// =====================================================================
		// Lost Pedlar Packs - atmosphere at the Gytis road post
		//---------------------------------------------------------------------
		AddNpc(47160, L("Unclaimed Pedlar Packs"), "f_siauliai_47_4", 1411, -208, 0, async dialog =>
		{
			await dialog.Msg(L("{#666666}*Two packs set down side by side in the grass, straps still buckled, nothing spilled and nothing taken*{/}"));
			await dialog.Msg(L("{#666666}*Whoever put them down here meant to pick them up again in a moment, and that was 11 days ago*{/}"));
		});

		// =====================================================================
		// QUEST 1004: The Channels That Stand Water
		// =====================================================================
		// Ditcher Laterus - four channels that never held water and do now
		//---------------------------------------------------------------------
		AddNpc(20139, L("[Ditcher] Laterus"), "f_siauliai_47_4", 582, 54, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_47_4", 1004);

			dialog.SetTitle(L("Laterus"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A ditcher scrapes the blade of his spade clean against a stone and looks at what comes off it*{/}"));
				await dialog.Msg(L("Careful where you step, this ground's gone strange lately — but you're a fresh pair of eyes, and I could use one. 22 years I have cut this estate's 19 channels in the same order every spring."));
				await dialog.Msg(L("4 of them stand water now. They have never stood water. A channel that stands water is a channel with something under it. Sample the 4 for me."));

				var response = await dialog.Select(L("Will you go and look at them?"),
					Option(L("I'll sample the 4 channels"), "help"),
					Option(L("What am I looking for?"), "info"),
					Option(L("Just dig them deeper"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They are all in the east cut, between the grass and the road. Kneel at each one and fill a bottle off the top of the water, not out of the bottom."));
						await dialog.Msg(L("The reeds are dead round the far one. Reeds do not die in standing water. That is the one I want you to look at longest."));
						break;

					case "info":
						await dialog.Msg(L("Colour. Smell. Whether the water moves when nothing is touching it. I cannot tell you more than that because I have never had a channel do this and neither had my father."));
						await dialog.Msg(L("The estate drinks off these. So does everything in the field, and the field is what goes into Stepas's sacks."));
						break;

					case "leave":
						await dialog.Msg(L("Deeper only means more of it. I would like to know what it is before I make room for it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sampled = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled", 0);

				if (sampled >= 4)
				{
					await dialog.Msg(L("{#666666}*He holds the 4 bottles up against the sky, one after another, and puts them down very carefully*{/}"));
					await dialog.Msg(L("Oil on the top of all 4 and it is not oil. And the far one has more of it than the other 3 put together."));
					await dialog.Msg(L("So it comes from up there and it runs down. Northeast, in the reeds where nobody cuts because there is nothing up there worth cutting. Take the season's tool-money. I would rather you had it than the tools."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(LF("East cut, between the grass and the road. {0} of 4 sampled. Off the top of the water.", sampled));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have staked all 4 channels shut and put a board over the far one. 19 channels and I am running the estate on 15, which I can do, and which I will do until somebody tells me what is up there."));
			}
		});

		// =====================================================================
		// STANDING CHANNELS
		// =====================================================================
		// For Quest 1004 - The Channels That Stand Water
		// =====================================================================

		void AddStandingChannel(int channelNumber, string channelName, string observation, int x, int z, int direction)
		{
			AddNpc(47202, L(channelName), "f_siauliai_47_4", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_siauliai_47_4", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A cut channel holding still water it was never dug to hold*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_siauliai_47_4.Quest1004.Channel{channelNumber}";

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Your bottle from this channel is already stoppered and in your pack*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Filling a bottle..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result != TimeActionResult.Completed)
				{
					character.ServerMessage(L("Sampling interrupted."));
					return;
				}

				character.Variables.Perm.Set(variableKey, true);
				character.Inventory.Add(662019, 1, InventoryAddType.PickUp);

				var sampled = character.Variables.Perm.GetInt("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled", 0) + 1;
				character.Variables.Perm.Set("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled", sampled);

				character.ServerMessage(L(observation));
				character.ServerMessage(LF("Channels sampled: {0}/4", sampled));

				if (sampled >= 4)
					character.ServerMessage(L("{#FFD700}All 4 channels sampled. Return to Ditcher Laterus.{/}"));
			});
		}

		AddStandingChannel(1, "Fourth Channel", "Water clear to the bottom. A film on the surface that closes again behind the bottle.", 1116, -410, 0);
		AddStandingChannel(2, "Seventh Channel", "The film is thicker here and it holds the shape of a fingerprint.", 944, -581, 0);
		AddStandingChannel(3, "Eleventh Channel", "Nothing living in the water. No skaters, no larvae, no weed on the sides.", 1324, -699, 0);
		AddStandingChannel(4, "Nineteenth Channel", "Reeds dead standing, all of them leaning the same way. The film here is heavy enough to pour.", 1468, -800, 0);

		// =====================================================================
		// QUEST 1005: What Is Lying in the Reeds
		// =====================================================================
		// Baker Dalius - 40 years of bread and 8 wrong bakes in a month
		//---------------------------------------------------------------------
		AddNpc(20138, L("[Baker] Dalius"), "f_siauliai_47_4", -598, 806, 14, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_siauliai_47_4", 1005);

			dialog.SetTitle(L("Dalius"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_siauliai_47_4", 1004)))
				{
					await dialog.Msg(L("{#666666}*He's turning a misshapen loaf over in his hands, sniffing at the crust like it lied to him*{/}"));
					await dialog.Msg(L("Laterus has 4 channels standing water and he is going to tell me it is nothing. Go and let him prove it to himself first. I have been wrong about this estate for a month and I would like company."));
					return;
				}

				await dialog.Msg(L("{#666666}*He sets the loaf down and wipes his hands slowly on his apron, in no hurry to start talking*{/}"));
				await dialog.Msg(L("40 years I have baked this estate's bread and I can put a year to a loaf by the taste of the crust. 8 bakes this month and 8 of them wrong, and not one of them wrong in a way I have a word for."));
				await dialog.Msg(L("Laterus says northeast and Norkus says his hedge-wizards all walked northwest, and those two lines cross in the reeds. Kill 20 Orange Popolions on the way in and then take whatever is holding that ground."));

				var response = await dialog.Select(L("Will you go up into the reeds?"),
					Option(L("I'll go into the reeds"), "help"),
					Option(L("Wizards and Popolions both?"), "info"),
					Option(L("Burn the stores and start over"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Inventory.Add(662021, 1, InventoryAddType.PickUp);
						character.Quests.Start(questId);
						await dialog.Msg(L("Drink the tonic before you go in and not after. If the water is doing what I think it is doing then the air over it is doing a quieter version of the same thing."));
						await dialog.Msg(L("The Popolions bed down in the rows every night. Whatever is in that water, they are the ones carrying it into my flour."));
						break;

					case "info":
						await dialog.Msg(L("The Popolions drink there because it is water. The wizards walk there because it is worth walking to. Those are not the same reason and they are the same place."));
						await dialog.Msg(L("A man came through in the spring and told me there is a Scorpio asleep up in that reed bed and has been for years. I laughed at him. I have not laughed at him since Tuesday."));
						break;

					case "leave":
						await dialog.Msg(L("Burn 5 stores in the month the cluster asks us for oats for 300? No. I would rather hand Mykolas bad bread and my own name on it than hand him nothing."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("openReeds", out var reedObj)) return;
				if (!quest.TryGetProgress("holdTheSeep", out var seepObj)) return;

				if (reedObj.Done && seepObj.Done)
				{
					await dialog.Msg(L("{#666666}*He listens the whole way through with his hands flat on the table*{/}"));
					await dialog.Msg(L("Asleep. Years asleep, in the seep, and everything above it drinking off it. The Popolions carry it into the rows on their bellies and bed down on my sacks, and that is why every hollow is the same size and the same shape."));
					await dialog.Msg(L("The wizards were not robbing carts. They were queueing. Take these - the Baron's own, and out of a cupboard, and it took a stranger to make anybody in this house open a cupboard."));

					character.Quests.Complete(questId);
				}
				else if (reedObj.Done)
				{
					await dialog.Msg(L("Reeds are open. Whatever is standing on that seep is still standing on it and it has not moved once."));
				}
				else
				{
					await dialog.Msg(L("20 Popolions on the way in first, or you will have the whole bedding-ground behind you when you get there."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Laterus has the seep boarded and staked and the 4 channels cut away from it. First clean bake in 6 weeks came out this morning and I sat down in the flour to eat it."));
				await dialog.Msg(L("Stepas signed the Tenants' Farm order the same hour. Oats for 300, late by a fortnight, and every sack of it solid to the middle."));
			}
		});

		// =====================================================================
		// The Sleeping Scorpio - the thing under the seep
		//---------------------------------------------------------------------
		AddNpc(152021, L("Sleeping Scorpio"), "f_siauliai_47_4", 1456, 935, 179, async dialog =>
		{
			await dialog.Msg(L("{#666666}*Something enormous lies half-sunk in the reed bed, breathing slowly enough that the water only moves twice a minute*{/}"));
			await dialog.Msg(L("{#666666}*The reeds have grown up through it and died there. Whatever is coming off it runs downhill into the estate's channels, and has been doing it for years*{/}"));
		});

		// =====================================================================
		// Loader Jaunius - atmosphere on the burst sack
		//---------------------------------------------------------------------
		AddNpc(20117, L("[Loader] Jaunius"), "f_siauliai_47_4", 579, -1443, 90, async dialog =>
		{
			dialog.SetTitle(L("Jaunius"));

			await dialog.Msg(L("{#666666}*A loader sits on his heels beside a split sack, sifting the grain through his fingers and letting it fall back*{/}"));
			await dialog.Msg(L("I load. That is the whole of what I do here and I have done it since I was 12. I know a sound sack by the way it takes the hook."));
			await dialog.Msg(L("41 of them came off the boards wrong this week and I will tell you the thing nobody upstairs has noticed. Every hollow is the same size. Every hollow is the same shape."));
			await dialog.Msg(L("Grain does not rot in a shape. Something lay in each one of those sacks."));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Forty-One Hollow Sacks
//-----------------------------------------------------------------------------

public class FortyOneHollowSacksQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_47_4", 1001);
		SetName(L("Forty-One Hollow Sacks"));
		SetType(QuestType.Sub);
		SetDescription(L("Baron Allerno's estate owes Tenants' Farm oats for 300 and cannot fill the order. 41 sacks opened and 41 hollow in the middle. Clear the Orange Hammings off the rows and bring back 6 sealed seed bags."));
		SetLocation("f_siauliai_47_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Granary Warden] Stepas"), "f_siauliai_47_4");

		AddObjective("killHammings", L("Kill Orange Hammings working the grain rows"),
			new KillObjective(25, new[] { MonsterId.Haming_Orange }));

		AddObjective("collectSeedBags", L("Recover sealed seed bags"),
			new CollectItemObjective(662063, 6));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(662063, 0.30f, MonsterId.Haming_Orange);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662063, character.Inventory.CountItem(662063), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662063, character.Inventory.CountItem(662063), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Three Lengths Left
//-----------------------------------------------------------------------------

public class ThreeLengthsLeftQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_47_4", 1002);
		SetName(L("Three Lengths Left"));
		SetType(QuestType.Sub);
		SetDescription(L("The estate owned 14 lengths of rope 9 years ago and owns 3. The Orange Popolions have dragged the rest into the west scrub to bed in, and the Myrkiti relief cart cannot go out unlashed."));
		SetLocation("f_siauliai_47_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Carter] Marius"), "f_siauliai_47_4");

		AddObjective("collectRope", L("Cut lengths of strong rope out of the Popolion bedding"),
			new CollectItemObjective(662017, 6));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion

		AddDrop(662017, 0.30f, MonsterId.Popolion_Orange);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662017, character.Inventory.CountItem(662017), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662018, character.Inventory.CountItem(662018), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662017, character.Inventory.CountItem(662017), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(662018, character.Inventory.CountItem(662018), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: Six Carts in a Month
//-----------------------------------------------------------------------------

public class SixCartsInAMonthQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_47_4", 1003);
		SetName(L("Six Carts in a Month"));
		SetType(QuestType.Sub);
		SetDescription(L("The Gytis road carried 6 carts a day and now carries 6 a month. Roadwarden Norkus needs the Spion Mages taken off it, and cannot explain why they let loaded carts go past."));
		SetLocation("f_siauliai_47_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Roadwarden] Norkus"), "f_siauliai_47_4");

		AddObjective("killMages", L("Kill Spion Mages on the Gytis road"),
			new KillObjective(18, new[] { MonsterId.Spion_Mage }));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: The Channels That Stand Water
//-----------------------------------------------------------------------------

public class TheChannelsThatStandWaterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_47_4", 1004);
		SetName(L("The Channels That Stand Water"));
		SetType(QuestType.Sub);
		SetDescription(L("Laterus has cut the estate's 19 channels for 22 years and 4 of them stand water that has never stood there. Sample all 4 in the east cut and bring the bottles back."));
		SetLocation("f_siauliai_47_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ditcher] Laterus"), "f_siauliai_47_4");

		AddObjective("sampleChannels", L("Sample the 4 channels standing water"),
			new VariableCheckObjective("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled", 4, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662019, character.Inventory.CountItem(662019), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_47_4.Quest1004.Channel{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662019, character.Inventory.CountItem(662019), InventoryItemRemoveMsg.Destroyed);
		character.Variables.Perm.Remove("Laima.Quests.f_siauliai_47_4.Quest1004.Sampled");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_siauliai_47_4.Quest1004.Channel{i}");
	}
}

// Quest 1005 CLASS: What Is Lying in the Reeds
//-----------------------------------------------------------------------------

public class WhatIsLyingInTheReedsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_siauliai_47_4", 1005);
		SetName(L("What Is Lying in the Reeds"));
		SetType(QuestType.Sub);
		SetDescription(L("Laterus's channels run down from the northeast and Norkus's hedge-wizards were all walking northwest. The lines cross in a reed bed nobody cuts. Open the reeds and take whatever is holding the seep."));
		SetLocation("f_siauliai_47_4");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Baker] Dalius"), "f_siauliai_47_4");

		AddPrerequisite(new CompletedPrerequisite("f_siauliai_47_4", 1004));

		AddObjective("openReeds", L("Kill Orange Popolions on the way into the reeds"),
			new KillObjective(20, new[] { MonsterId.Popolion_Orange }));

		AddObjective("holdTheSeep", L("Take what is standing on the seep"),
			new LayeredKillObjective(
				spawnList: new[]
				{
					new KillSpec(MonsterId.Popolion_Orange, 2, BuffId.EliteMonsterBuff),
					new KillSpec(MonsterId.Spion_Mage, 3),
				},
				resetIdent: "openReeds",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(503102, 1)); // Shield Crasher
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(662021, character.Inventory.CountItem(662021), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(662021, character.Inventory.CountItem(662021), InventoryItemRemoveMsg.Destroyed);
	}
}
