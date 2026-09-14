//--- Melia Script ----------------------------------------------------------
// Verkti Square Quest NPCs
//--- Description -----------------------------------------------------------
// Petrification-cursed plateau quests for the Verkti Square map.
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

public class FFlash59QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1: Greying Jukopus
		//-------------------------------------------------------------------------
		AddNpc(150183, L("[Ward-Keeper] Halya"), "f_flash_59", 275, 602, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1001);

			dialog.SetTitle(L("Halya"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You're new. Good. We need new."));
				await dialog.Msg(L("This whole plateau's been turning to stone for thirty years. Slow, but it doesn't stop. The wards I tend slow it further, that's all."));
				await dialog.Msg(L("The Jukopus out here drink the bad water. They've gone grey, and what they touch goes greyer. The more of them around, the faster the soil dies."));

				var response = await dialog.Select(L("Will you kill the Jukopus for me?"),
					Option(L("Tell me how many to kill"), "help"),
					Option(L("Why does this place exist?"), "info"),
					Option(L("I'll come back later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twenty-two should buy us a season. Don't let one grab you for long. If your skin starts feeling cold, get out and find me."));
						await dialog.Msg(L("And if you've a mind to do more after, talk to Dania at the boundary stones. She can use the help too."));
						break;

					case "info":
						await dialog.Msg(L("Old story. A mage tried to cure a plague by petrifying it. The plague didn't last. Neither did he. The curse stayed."));
						await dialog.Msg(L("Now it just sits here, eating the ground."));
						break;

					case "leave":
						await dialog.Msg(L("Take your time. The curse already has."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJukopus", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("Quieter out there. I can hear birds again, which I haven't in weeks."));
					await dialog.Msg(L("Take your pay. And these salves - keep one in a pocket, you'll thank me eventually."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Not enough yet. Keep going - and don't fight tired, that's how people end up part of the scenery."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Green's coming back near the inner ward. I haven't seen that in a long time."));
			}
		});

		// Quest 2: The Statued Garrison
		//-------------------------------------------------------------------------
		AddNpc(20114, L("[Statue-Cataloguer] Dania"), "f_flash_59", -75, 119, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1003);

			dialog.SetTitle(L("Dania"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Halya sent you? She does that. She's trying to keep me busy."));
				await dialog.Msg(L("Every grey boulder here used to be a soldier. The garrison didn't lose a battle - they just stopped moving, one at a time, over a couple of months."));
				await dialog.Msg(L("Most of them buried something before the stone reached their hands. Letters. Tags. A wife's name on a folded scrap. The Rambears bed down on the boulders now and dig up everything underneath."));

				var response = await dialog.Select(L("Will you bring me what they buried?"),
					Option(L("I'll bring you what I find"), "help"),
					Option(L("Why are you doing this?"), "info"),
					Option(L("Maybe later"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Fifteen bears, four caches. The bears keep digging things up and chewing them. Get there before they do."));
						await dialog.Msg(L("If a stone has a face, leave it. Some still look like they're listening."));
						break;

					case "info":
						await dialog.Msg(L("My husband's out there somewhere. He was the sergeant. I've been comparing his handwriting to every letter I find for two years."));
						await dialog.Msg(L("Haven't matched one yet. But I will."));
						break;

					case "leave":
						await dialog.Msg(L("They've been waiting a hundred years. They can wait a little more."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRambears", out var killObj)) return;
				if (!quest.TryGetProgress("recoverCaches", out var cacheObj)) return;

				if (killObj.Done && cacheObj.Done)
				{
					await dialog.Msg(L("Four. Open them carefully - the oilcloth's brittle by now."));
					await dialog.Msg(L("...One of them's signed by a Captain Velkas. I knew that name. Take your pay. I need a moment."));

					character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Rambears need clearing. ");
					if (!cacheObj.Done)
						status += L("More caches still buried. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("A great-granddaughter came to see Velkas's stone yesterday. She brought him flowers. He'd have liked that, I think."));
			}
		});

		// Quest 3: Stolen Pages
		//-------------------------------------------------------------------------
		AddNpc(20121, L("[Curse-Scholar] Ivor"), "f_flash_59", 99, -298, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1004);

			dialog.SetTitle(L("Ivor"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("Don't tell Halya I'm here. She thinks I'm meddling. She's not entirely wrong."));
				await dialog.Msg(L("The goblins on the ridges aren't local. They came after the curse, the same way I did - except they want to sell it, and I want to understand it."));
				await dialog.Msg(L("They're carrying pages they shouldn't have. Stolen from a Fedimian archive. If they ever finish translating them, this stops being a quiet tragedy and starts being someone's weapon."));

				var response = await dialog.Select(L("Can I count on you for this?"),
					Option(L("I'll bring back the pages"), "help"),
					Option(L("Who'd buy a curse?"), "info"),
					Option(L("Not interested"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve goblins. Five pages. Don't read them - some of the sigils on the corners are still active and you don't want them on your eyes."));
						await dialog.Msg(L("Burn anything you can't carry. I'll trust your judgment on what that is."));
						break;

					case "info":
						await dialog.Msg(L("Every petty warlord in the south. A curse you can chant is a city you can take. The buyers exist - the sellers, until now, did not."));
						await dialog.Msg(L("I'd prefer to keep it that way."));
						break;

					case "leave":
						await dialog.Msg(L("Reconsider before someone less careful takes the contract."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killWandGoblins", out var killObj)) return;
				if (!quest.TryGetProgress("gatherPages", out var pageObj)) return;

				if (killObj.Done && pageObj.Done)
				{
					await dialog.Msg(L("All five. Good - and two of them are originals, not copies. That tells me where the leak is."));
					await dialog.Msg(L("Your pay. The pages go in a sealed case tonight. The leak I'll handle when I'm back in Fedimian."));

					character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Given);

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!killObj.Done)
						status += L("More Wand-Goblins to silence. ");
					if (!pageObj.Done)
						status += L("More pages still missing. ");

					await dialog.Msg(LF("Keep at it. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Pages are sealed. The buyer never got their merchandise. That's the kind of result I like."));
			}
		});

		// Quest 4: The Stone-Scarred Alpha
		//-------------------------------------------------------------------------
		AddNpc(47245, L("[Bounty Hunter] Stryker"), "f_flash_59", 747, -476, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1005);
			var alphaSpawnedKey = "Laima.Quests.f_flash_59.Quest1005.AlphaSpawned";

			dialog.SetTitle(L("Stryker"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("You look like someone who takes contracts. I have one nobody local will touch."));
				await dialog.Msg(L("There's an old bear out east. The curse caught him halfway and stopped - half his body's hide, half's stone plate. He's slower than he used to be. He's also about twice as hard to kill."));
				await dialog.Msg(L("He won't come out while his pack's around him. Drop ten Rambears and he'll show himself. After that it's just you and a bear with rocks for ribs."));

				var response = await dialog.Select(L("So? Want the contract?"),
					Option(L("I'll take it"), "help"),
					Option(L("Why's the hide worth anything?"), "info"),
					Option(L("Pass"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Ten first. Don't engage him tired - he hits with the stone shoulder, and that side does not give."));
						await dialog.Msg(L("Keep to his left. His left's still a bear. His right's a wall."));
						break;

					case "info":
						await dialog.Msg(L("Halya's wards use cursed material as anchors. Cursed bear-plate, cursed stone, all of it. Hide like his keeps a season's worth of wards lit."));
						await dialog.Msg(L("She won't ask me to bring it in. So I bring it in and don't tell her where it came from."));
						break;

					case "leave":
						await dialog.Msg(L("Bounty's open till someone takes it. I'm not in a hurry."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killPack", out var packObj)) return;
				if (!quest.TryGetProgress("killAlpha", out var alphaObj)) return;

				if (packObj.Done && alphaObj.Done)
				{
					await dialog.Msg(L("Plate's intact. That's the one. You earned every coin of this."));
					await dialog.Msg(L("Pay's yours, plus a cut of mine - I wasn't going to take him myself anytime soon."));

					character.Variables.Perm.Remove(alphaSpawnedKey);

					character.Quests.Complete(questId);
				}
				else if (packObj.Done && !alphaObj.Done)
				{
					var hasSpawned = character.Variables.Perm.GetBool(alphaSpawnedKey, false);
					if (!hasSpawned)
					{
						character.Variables.Perm.Set(alphaSpawnedKey, true);

						if (SpawnTempMonsters(character, MonsterId.Rambear, 1, 120, TimeSpan.FromMinutes(5)))
						{
							await dialog.Msg(L("Hear that? That's him. Pack's gone, he doesn't have a reason to hide anymore."));
							await dialog.Msg(L("{#FF9966}Move - and stay off his right.{/}"));
							character.ServerMessage(L("{#FF9966}The Stone-Scarred Alpha charges out of the copse!{/}"));
						}
					}
					else
					{
						await dialog.Msg(L("He's loose. Don't lose him - he'll heal up if he gets back into the trees."));
					}
				}
				else
				{
					await dialog.Msg(L("Pack's still thick. He won't budge. Keep thinning."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Hide's gone to the wards. Halya hasn't asked where it came from. I'm not going to volunteer it."));
			}
		});

		// Quest 5: The Cursed Perimeter
		//-------------------------------------------------------------------------
		AddNpc(147410, L("[Trail Master] Odessa"), "f_flash_59", -712, -309, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_flash_59", 1006);

			dialog.SetTitle(L("Odessa"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("I used to scout this trail for the caravan guild. Walked every yard of it before the curse spread this far west. Now I sit at the waypost and turn drivers around."));
				await dialog.Msg(L("The trail itself is fine. It's the flanks. Jukopus on the low ground, goblins on the ridges, and neither side stops trying to get at the carts."));
				await dialog.Msg(L("Thin a dozen of each and I can wave traffic through again. There's three caravans waiting in town that won't move until I do."));

				var response = await dialog.Select(L("Need a hand?"),
					Option(L("I'll handle both sides"), "help"),
					Option(L("Why not the deep route?"), "info"),
					Option(L("I'll think about it"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Twelve and twelve. Watch your footing on the low ground - the slime's slick and the goblins shoot down at anyone slipping."));
						await dialog.Msg(L("Get a salve from Halya before you go. She'll know what for."));
						break;

					case "info":
						await dialog.Msg(L("The deep route crosses the inner ground. Lost a driver and two horses to it last spring. The horses we found again. Sort of."));
						await dialog.Msg(L("Perimeter or nothing. That's the only honest answer."));
						break;

					case "leave":
						await dialog.Msg(L("I'll be here. The carts will too."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killJukopus", out var jukObj)) return;
				if (!quest.TryGetProgress("killGoblins", out var gobObj)) return;

				if (jukObj.Done && gobObj.Done)
				{
					await dialog.Msg(L("Both flanks light enough to move through. I'll send the first cart at first light."));
					await dialog.Msg(L("Your pay. Drivers will know your name by next month."));

					character.Quests.Complete(questId);
				}
				else
				{
					var status = "";
					if (!jukObj.Done)
						status += L("More Grey Jukopus on the low ground. ");
					if (!gobObj.Done)
						status += L("More Wand-Goblins on the ridges. ");

					await dialog.Msg(LF("Keep pushing. {0}", status));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Four caravans through this week. One driver left a flask on the bench for you. I'm holding it for whenever you next pass."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Greying Jukopus
//-----------------------------------------------------------------------------

public class StoneWetJukopusQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1001);
		SetName(L("Greying Jukopus"));
		SetType(QuestType.Sub);
		SetDescription(L("Halya's wards are losing ground to the cursed Jukopus drinking the plateau's bad water. Thin them out before the soil greys further."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Ward-Keeper] Halya"), "f_flash_59");

		AddObjective("killJukopus", L("Kill cursed Grey Jukopus"),
			new KillObjective(22, new[] { MonsterId.Jukopus_Gray }));

		AddReward(new ExpReward(11900, 8100));
		AddReward(new SilverReward(15000));
		AddReward(new ItemReward(640086, 1));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
	}
}

// Quest 1003 CLASS: The Statued Garrison
//-----------------------------------------------------------------------------

public class TheStatuedGarrisonQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1003);
		SetName(L("The Statued Garrison"));
		SetType(QuestType.Sub);
		SetDescription(L("Dania catalogues the petrified soldiers of Verkti. Clear the Rambears bedding on the boulders and recover the caches the garrison buried before the curse took their hands."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Statue-Cataloguer] Dania"), "f_flash_59");

		AddObjective("killRambears", L("Kill stone-streaked Rambears"),
			new KillObjective(15, new[] { MonsterId.Rambear }));

		AddObjective("recoverCaches", L("Recover buried garrison caches"),
			new CollectItemObjective(650675, 4));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650675, 0.40f, MonsterId.Rambear);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650675, character.Inventory.CountItem(650675), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1004 CLASS: Stolen Pages
//-----------------------------------------------------------------------------

public class PetrifierCantQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1004);
		SetName(L("Stolen Pages"));
		SetType(QuestType.Sub);
		SetDescription(L("Wand-Goblins on the ridges are carrying pages stolen from Fedimian's archive. Kill them and recover the pages before the curse becomes someone's product."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Curse-Scholar] Ivor"), "f_flash_59");

		AddObjective("killWandGoblins", L("Kill Wand-Goblins"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand1 }));

		AddObjective("gatherPages", L("Recover stolen pages from Wand-Goblins"),
			new CollectItemObjective(650783, 5));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));

		AddDrop(650783, 0.50f, MonsterId.Goblin2_Wand1);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650783, character.Inventory.CountItem(650783), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Stone-Scarred Alpha
//-----------------------------------------------------------------------------

public class TheStoneScarredAlphaQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1005);
		SetName(L("The Stone-Scarred Alpha"));
		SetType(QuestType.Sub);
		SetDescription(L("Stryker has a contract on a half-petrified Alpha Rambear. Thin the pack first; the Alpha won't show himself until they scatter."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Bounty Hunter] Stryker"), "f_flash_59");

		AddObjective("killPack", L("Thin the Rambear pack"),
			new KillObjective(10, new[] { MonsterId.Rambear }));

		AddObjective("killAlpha", L("Defeat the Stone-Scarred Alpha"),
			new KillObjective(1, new[] { MonsterId.Rambear }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}

// Quest 1006 CLASS: The Cursed Perimeter
//-----------------------------------------------------------------------------

public class TheCursedPerimeterQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_flash_59", 1006);
		SetName(L("The Cursed Perimeter"));
		SetType(QuestType.Sub);
		SetDescription(L("Odessa can't wave caravans through until both flanks of the perimeter trail are thinned. Clear the Jukopus on the low ground and the Wand-Goblins on the ridges."));
		SetLocation("f_flash_59");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Trail Master] Odessa"), "f_flash_59");

		AddObjective("killJukopus", L("Kill Grey Jukopus on the low ground"),
			new KillObjective(12, new[] { MonsterId.Jukopus_Gray }));

		AddObjective("killGoblins", L("Kill Wand-Goblins on the ridges"),
			new KillObjective(12, new[] { MonsterId.Goblin2_Wand1 }));

		AddReward(new ExpReward(23800, 16200));
		AddReward(new SilverReward(17000));
		AddReward(new ItemReward(640086, 2));
		AddReward(new ItemReward(640004, 3));
		AddReward(new ItemReward(640007, 3));
		AddReward(new ItemReward(640013, 1));
	}
}
