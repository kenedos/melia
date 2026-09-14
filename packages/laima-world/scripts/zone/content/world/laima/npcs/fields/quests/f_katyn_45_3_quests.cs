//--- Melia Script ----------------------------------------------------------
// Grynas Hills Quest NPCs
//--- Description -----------------------------------------------------------
// The hills the blackening comes down from, the three suppressing sculptures
// that were supposed to hold it, and what the Dievdirbys buried up here.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
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
		// Quest 1001: Blue Sakmoli Leaves
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_3", -1813, -178, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1001);

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's out of breath from the climb, satchel still swinging against his hip*{/}"));
				await dialog.Msg(L("You made better time up here than I did. I came up the ridge without waiting for Fedimian - Esol told me not to and then packed my satchel while he was telling me, which is the sort of man he is."));
				await dialog.Msg(L("The Blue Sakmoli up here are the only thing carrying the blackening clean, without any rot in it. Kill 20 of them and bring me 8 leaves - I need to see it undiluted."));

				var response = await dialog.Select(L("Well? Will you work the slope, or just admire how out of breath I am?"),
					Option(L("I'll hunt them and bring the leaves"), "help"),
					Option(L("Undiluted?"), "info"),
					Option(L("Wait for the order"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They will come at you the moment you are on the slope - there is no stalking them. Fight with the hill above you so they have to climb."));
						break;

					case "info":
						await dialog.Msg(L("Everything down on the training field has been sick for weeks and the sample is muddied by the sickness. Up here it is fresh. Whatever it is, it starts on this hill and it starts clean."));
						await dialog.Msg(L("A carver reads wood the way a physician reads a pulse. I want the pulse at the heart, not at the wrist."));
						break;

					case "leave":
						await dialog.Msg(L("The order will send 4 carvers and a written procedure in about 5 weeks. I have looked at the ridge and I do not think it has 5 weeks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killSakmoli", out var killObj)) return;
				if (!quest.TryGetProgress("collectLeaves", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He splits a leaf along the vein and holds both halves up together*{/}"));
					await dialog.Msg(L("It is not a blight and it is not a rot. It is a pattern. Something up here is cutting a shape into everything that grows, and the shape repeats."));
					await dialog.Msg(L("Take what is in my satchel. I did not pack it for coin and I have no use for it out here."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("The slope's thinner. I still need leaves - take them off the plant on the body, not off the ground."));
				}
				else
				{
					await dialog.Msg(L("Still Sakmoli holding the slope. They will not let you past them, so there is no way round it."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have drawn the pattern out on 3 sheets and it matches the cut on the broken obelisk. 400 years and nobody could read that carving, and a leaf just read it for me."));
			}
		});

		// Quest 1002: Yellow Griba Pollen
		//---------------------------------------------------------------------
		AddNpc(20116, L("[Griba-Picker] Zita"), "f_katyn_45_3", -389, 1672, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1002);

			dialog.SetTitle(L("Zita"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's crouched with a half-empty sack, watching a mushroom that shouldn't be moving*{/}"));
				await dialog.Msg(L("Careful where you step, dear — and since you're already being careful, you might as well lend a hand. Thirty years I've picked these hills. Yellow Griba, dried and sold down to the dye houses at Fedimian, two sacks a season, never once a bad one. Not one, in thirty years."));
				await dialog.Msg(L("This year, they walk. Not all of them — enough. Enough that an old woman alone can't fill a sack anymore. Bring me 10 lots of pollen and I'll still make the autumn cart, God and mushrooms willing."));

				var response = await dialog.Select(L("So? Will you pick with an old woman, or leave her talking to mushrooms?"),
					Option(L("I'll gather 10 lots of pollen"), "help"),
					Option(L("They walk?"), "info"),
					Option(L("Skip the season"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Take the pollen off them once they're down, mind. Shake a live one and the pollen goes up — you lose the lot, and you get a face full for your trouble."));
						break;

					case "info":
						await dialog.Msg(L("A Griba is a mushroom, dear. A mushroom has no business having a direction — and this year every last one of them has the same direction, and it's uphill."));
						await dialog.Msg(L("My grandmother picked this hill, and hers before that. Three generations of family talk about these mushrooms, and not one word in any of it about them deciding to go somewhere."));
						break;

					case "leave":
						await dialog.Msg(L("Skip one season, dear, and the dye houses find another supplier quick enough — you never get them back after that. I've watched it happen to three other pickers. Good ones, too, better than me some of them."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectPollen", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She rubs a pinch between her fingers and holds it up against the sky*{/}"));
					await dialog.Msg(L("Colour's still true. Whatever's got into them hasn't got into the pollen yet, and the dye houses will never know the difference."));
					await dialog.Msg(L("Your share of the cart money, paid now rather than in the spring. I've learned not to promise anybody spring."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short of a sack. They're thickest in the northern fields, where the ground's wet."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Cart went down the ridge full. 31st season running. If it's the last one, at least it wasn't the short one."));
			}
		});

		// Quest 1003: The Three Suppressing Sculptures
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_3", -486, 99, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1003);

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's crouched by a leaning stone figure, one hand testing how loose it sits*{/}"));
				await dialog.Msg(L("Good, another set of shoulders. There are 3 suppressing sculptures on this hill and no record of who carved them. They are not road statues - a road statue protects a road. These were cut to hold something down."));
				await dialog.Msg(L("All 3 have shifted off their seats. Go to each one and set it true again, and do it in one afternoon - a sculpture reseated alone does nothing."));

				var response = await dialog.Select(L("Well? Will you set all three true, or leave an old man to do it on his knees?"),
					Option(L("I'll set the 3 sculptures true"), "help"),
					Option(L("Nobody carved them?"), "info"),
					Option(L("Leave them where they've fallen"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Face them inward, toward each other. Whatever they hold sits in the middle of the triangle they make, and the triangle is what matters, not the carvings."));
						break;

					case "info":
						await dialog.Msg(L("The order keeps a record of every piece cut since the school opened. 12,000 entries. These 3 are not among them and they are older than the ledger."));
						await dialog.Msg(L("Which means somebody was carving suppressing work on this hill before there was an order to do it, and stopped without telling anyone why."));
						break;

					case "leave":
						await dialog.Msg(L("They have been falling for 400 years and the blackening arrived this summer. I do not think those two facts are strangers."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("seatSculptures", out var seatObj)) return;

				if (seatObj.Done)
				{
					await dialog.Msg(L("{#666666}*He walks the line between the three and stops in the middle of it*{/}"));
					await dialog.Msg(L("The ground here is warm. Not sun-warm. There is something 6 feet under my boots and the sculptures have been pointing at it since before the order existed."));
					await dialog.Msg(L("Take the rest of what I brought up. I am about to start digging and a satchel is only in the way."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still sculptures off their seats. All 3, facing inward - 2 out of 3 is the same as none."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("I have been digging since dawn and I am 4 feet down. Every carver instinct I have is telling me to fill it back in."));
			}
		});

		// Quest 1003 interaction points - the suppressing sculptures
		//---------------------------------------------------------------------
		void AddSuppressingSculpture(int sculptureNumber, string observation, int x, int z, int direction)
		{
			AddNpc(157008, L("Suppressing Sculpture"), "f_katyn_45_3", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_3", 1003);
				var variableKey = $"Laima.Quests.f_katyn_45_3.Quest1003.Sculpture{sculptureNumber}";
				var counterKey = "Laima.Quests.f_katyn_45_3.Quest1003.SculpturesSeated";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A squat carved figure, leaning off its stone seat*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This one stands true now*{/}"));
					return;
				}

				var luredCount = LureNearbyEnemies(character, 500, 400);
				if (luredCount > 0)
					character.ServerMessage(LF("{{#FF6666}}The hill answers the setting - {0} drawn in!{{/}}", luredCount));

				var result = await character.TimeActions.StartAsync(
					L("Setting the sculpture true..."), L("Cancel"), "PRAY", TimeSpan.FromSeconds(5)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var seated = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, seated);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Sculptures set true: {0}/3", seated));

					if (seated >= 3)
						character.ServerMessage(L("{#FFD700}All 3 stand true. Return to Dievdirbys Ajel.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the sculpture leaning."));
				}
			});
		}

		AddSuppressingSculpture(1,
			L("The first sculpture settles onto its seat and turns to face the other two."), 16, 391, 225);
		AddSuppressingSculpture(2,
			L("The second sculpture seats with a sound like a door closing underground."), -25, 430, 180);
		AddSuppressingSculpture(3,
			L("The third sculpture seats, and the grass between all three lies flat."), -473, 118, 45);

		// Quest 1004: The Broken Amulet
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Sanctuary Warden] Barvydas"), "f_katyn_45_3", 1147, 358, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1004);

			dialog.SetTitle(L("Barvydas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's standing at what's left of a doorway with no door in it, watching the water below*{/}"));
				await dialog.Msg(L("Visitors, this far up the hill — now that's a rare thing indeed. I keep the old sanctuary: no congregation, no priest, no roof to speak of. Just a warden. That's me. And my grandfather before me, warding the same empty air."));
				await dialog.Msg(L("The sanctuary's amulet went into the water 11 years back and the Blue Fishermen have been carrying the pieces around ever since. Kill 15 of them and bring me 6 fragments."));

				var response = await dialog.Select(L("Will you fetch them, for a warden with nothing left to guard but his own pride?"),
					Option(L("I'll hunt them and bring the fragments"), "help"),
					Option(L("What did the amulet do?"), "info"),
					Option(L("It's just an amulet"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They wear the pieces. Look at the ones with something bright on the chest and take those first."));
						break;

					case "info":
						await dialog.Msg(L("It hung over the door — the last thing anyone hung up here before the sanctuary was abandoned to me. Nobody told me what it did. My grandfather didn't know either, and he was considerably better at this job than I am at mine."));
						await dialog.Msg(L("What I know is that the summer it went in the water was the summer the hill started warming, and I have had 11 years to notice that."));
						break;

					case "leave":
						await dialog.Msg(L("It is a broken amulet in a roofless sanctuary and I am a warden with nothing to ward. I am aware of how the whole arrangement looks."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killFishermen", out var killObj)) return;
				if (!quest.TryGetProgress("collectFragments", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He lays the fragments out on the altar stone and pushes them together with one finger*{/}"));
					await dialog.Msg(L("6 pieces and they make a ring. There is a carving on the inside of it that I have never seen, because the inside of a ring is not a place you look."));
					await dialog.Msg(L("Take the offering box. Nobody's put a coin in it since my grandfather, and nobody's about to start with me standing here looking this useless."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("Plenty of them down and not enough pieces. Not every one of them is wearing a fragment."));
				}
				else
				{
					await dialog.Msg(L("Still Blue Fishermen in the shallows. They stay near the water, so that is where to look."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The carving on the inside of the ring is the same mark that is on the 3 hill sculptures. I showed the carver. He went very quiet and then went to get a spade."));
			}
		});

		// Quest 1005: What the Sculptures Were Holding
		//---------------------------------------------------------------------
		AddNpc(157004, L("[Dievdirbys] Ajel"), "f_katyn_45_3", -407, -424, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_3", 1005);

			dialog.SetTitle(L("Ajel"));

			if (!character.Quests.Has(questId))
			{
				if (!character.Quests.HasCompleted(new QuestId("f_katyn_45_3", 1003)))
				{
					await dialog.Msg(L("The 3 sculptures have to stand true before anything else happens on this hill. Set them and come back."));
					return;
				}

				await dialog.Msg(L("{#666666}*He's standing waist-deep in a pit, dirt on both hands, not looking away from something at his feet*{/}"));
				await dialog.Msg(L("Get down here - now, before I lose my nerve and cover it back up myself. 6 feet down and I found a dagger. Not buried - pinned, under a flat stone, with the 3 sculptures aimed at it from every side for 400 years."));
				await dialog.Msg(L("It is awake now that I have moved the stone, and it is calling the Sakmoli in. Kill 25 Yellow Griba off the dig so nothing walks up behind me, then hold the pit while I get the pinning stone back on."));

				var response = await dialog.Select(L("Well? Will you hold the pit, or should I dig myself a grave to match while I'm down here?"),
					Option(L("I'll clear the dig and hold the pit"), "help"),
					Option(L("You should not have moved the stone"), "info"),
					Option(L("Cover it and walk away"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("What comes will be Sakmoli and they will not be the size you are used to. Keep your back to the pit wall - if they get behind you they will put you in the hole with it."));
						break;

					case "info":
						await dialog.Msg(L("No. I should not have. I have spent 46 years telling trainees that a carver's first duty is to leave standing work alone, and then I dug up standing work because I wanted to know."));
						await dialog.Msg(L("Esol will not say a word about it, which will be considerably worse than if he did."));
						break;

					case "leave":
						await dialog.Msg(L("It is calling with the stone off. Covering it now buries the sound, not the source, and the whole valley has been listening to that sound since June."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("clearDig", out var digObj)) return;
				if (!quest.TryGetProgress("holdThePit", out var pitObj)) return;

				if (digObj.Done && pitObj.Done)
				{
					await dialog.Msg(L("Stone's back on and the 3 sculptures are aimed at it again. I have cut a fourth to stand over the pit and I will cut a fifth before winter."));
					await dialog.Msg(L("The dagger goes down to the order under seal and nobody digs on this hill again. Take this - it came out of the pit with the dagger and it is clean."));

					character.Quests.Complete(questId);
				}
				else if (digObj.Done)
				{
					await dialog.Msg(L("The dig's clear. They are coming up the slope now and they are coming for the pit, not for us."));
				}
				else
				{
					await dialog.Msg(L("Too many Griba still crowding the dig. They are only mushrooms until 40 of them are behind you."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stumps at the school went cool 2 days after we pinned it. Esol wrote me one line about it and I have read it about 30 times."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Blue Sakmoli Leaves
//-----------------------------------------------------------------------------

public class BlueSakmoliLeavesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1001);
		SetName(L("Blue Sakmoli Leaves"));
		SetType(QuestType.Sub);
		SetDescription(L("Everything on the training field below is too sick to read. The Blue Sakmoli on the hill carry the blackening clean, with no rot muddying it. Kill them on the slope and bring back leaves."));
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_3");

		AddObjective("killSakmoli", L("Kill Blue Sakmoli on the hill slope"),
			new KillObjective(20, new[] { MonsterId.Sakmoli_Purple }));

		AddObjective("collectLeaves", L("Collect Blue Sakmoli Leaves"),
			new CollectItemObjective(668042, 8));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668042, 0.45f, MonsterId.Sakmoli_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668042, character.Inventory.CountItem(668042), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668042, character.Inventory.CountItem(668042), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Yellow Griba Pollen
//-----------------------------------------------------------------------------

public class YellowGribaPollenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1002);
		SetName(L("Yellow Griba Pollen"));
		SetType(QuestType.Sub);
		SetDescription(L("A picker has worked these hills for 30 seasons and this year her Yellow Griba have started walking uphill. She still has a dye-house cart to fill. Gather pollen from the northern fields."));
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Griba-Picker] Zita"), "f_katyn_45_3");

		AddObjective("collectPollen", L("Collect Yellow Griba Pollen in the northern fields"),
			new CollectItemObjective(668024, 10));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668024, 0.50f, MonsterId.Mushroom_Boy_Yellow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668024, character.Inventory.CountItem(668024), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668024, character.Inventory.CountItem(668024), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1003 CLASS: The Three Suppressing Sculptures
//-----------------------------------------------------------------------------

public class TheThreeSuppressingSculpturesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1003);
		SetName(L("The Three Suppressing Sculptures"));
		SetType(QuestType.Sub);
		SetDescription(L("Three carved figures stand on the hill with no entry in the order's 12,000-piece ledger, and all 3 have shifted off their seats. They were cut to hold something down. Set all 3 true again."));
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_3");

		AddObjective("seatSculptures", L("Set the 3 suppressing sculptures true"),
			new VariableCheckObjective("Laima.Quests.f_katyn_45_3.Quest1003.SculpturesSeated", 3, true));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1003.SculpturesSeated");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Sculpture{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_3.Quest1003.SculpturesSeated");

		for (var i = 1; i <= 3; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_3.Quest1003.Sculpture{i}");
	}
}

// Quest 1004 CLASS: The Broken Amulet
//-----------------------------------------------------------------------------

public class TheBrokenAmuletQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1004);
		SetName(L("The Broken Amulet"));
		SetType(QuestType.Sub);
		SetDescription(L("The old sanctuary's amulet went into the water 11 years ago, the same summer the hill started warming, and the Blue Fishermen have worn the pieces ever since. Kill them and bring the warden 6 fragments."));
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Sanctuary Warden] Barvydas"), "f_katyn_45_3");

		AddObjective("killFishermen", L("Kill Blue Fishermen in the shallows"),
			new KillObjective(15, new[] { MonsterId.Fisherman_Blue }));

		AddObjective("collectFragments", L("Collect Broken Amulet fragments"),
			new CollectItemObjective(668038, 6));

		AddReward(new ExpReward(6100, 4200));
		AddReward(new SilverReward(7200));
		AddReward(new ItemReward(640084, 2)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 2)); // Large HP Potion
		AddReward(new ItemReward(640007, 2)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion

		AddDrop(668038, 0.45f, MonsterId.Fisherman_Blue);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(668038, character.Inventory.CountItem(668038), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(668038, character.Inventory.CountItem(668038), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: What the Sculptures Were Holding
//-----------------------------------------------------------------------------

public class WhatTheSculpturesWereHoldingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_3", 1005);
		SetName(L("What the Sculptures Were Holding"));
		SetType(QuestType.Sub);
		SetDescription(L("Six feet under the middle of the three sculptures lay a dagger, pinned under a flat stone for 400 years. It is awake now, and it is calling the Sakmoli in. Clear the dig and hold the pit until the stone is back on."));
		SetLocation("f_katyn_45_3");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Dievdirbys] Ajel"), "f_katyn_45_3");

		AddPrerequisite(new CompletedPrerequisite("f_katyn_45_3", 1003));

		AddObjective("clearDig", L("Kill Yellow Griba crowding the dig"),
			new KillObjective(25, new[] { MonsterId.Mushroom_Boy_Yellow }));

		AddObjective("holdThePit", L("Hold the pit against the Sakmoli"),
			new LayeredKillObjective(
				spawnList: new[] {
					new KillSpec(MonsterId.Sakmoli_Purple, 3, BuffId.EliteMonsterBuff),
				},
				resetIdent: "clearDig",
				spawnDistance: 100,
				lifetime: TimeSpan.FromMinutes(5)));

		AddReward(new ExpReward(16000, 11000));
		AddReward(new SilverReward(20000));
		AddReward(new ItemReward(603107, 1)); // Elements Dance
		AddReward(new ItemReward(640084, 3)); // Lv4 EXP Card
		AddReward(new ItemReward(640004, 3)); // Large HP Potion
		AddReward(new ItemReward(640007, 3)); // Large SP Potion
		AddReward(new ItemReward(640012, 1)); // Recovery Potion
	}
}
