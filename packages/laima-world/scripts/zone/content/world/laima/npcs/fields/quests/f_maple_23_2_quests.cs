//--- Melia Script ----------------------------------------------------------
// Pistis Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The refugee camp at the Vakarine statue, and the two lords who have both
// been claiming this forest for eleven years without either owning it.
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

public class FMaple232QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// Quest 1001: Leafnut Tailfruit
		//---------------------------------------------------------------------
		AddNpc(147489, L("[Refugee] Milia"), "f_maple_23_2", 1279, 120, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1001);

			dialog.SetTitle(L("Milia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She's scraping the bottom of a grain sack with a wooden cup, counting under her breath*{/}"));
				await dialog.Msg(L("You're not from either lord's house, are you? Good — then maybe you can actually help, for once. Forty of us camped round this statue, eating out of that one sack since spring. Four days left in it, maybe less if the little ones get hungry before I can stretch it."));
				await dialog.Msg(L("The Yellow Leafnuts carry a tailfruit that's good eating once it's boiled. Bring me 10 of them and I can put something hot in front of the children tonight."));

				var response = await dialog.Select(L("Will you gather the tailfruit?"),
					Option(L("I'll bring you 10 tailfruit"), "help"),
					Option(L("Why is nobody feeding you?"), "info"),
					Option(L("Go and ask a lord"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("The tailfruit's on the back of the thing, not in it. It comes away easy once the Leafnut's down, but it bruises, so don't stack them."));
						break;

					case "info":
						await dialog.Msg(L("Because two lords both say this forest is theirs, and a man who's arguing about who owns a wood won't hand out grain from it. That would settle the argument the wrong way."));
						await dialog.Msg(L("Liudas sends us word he's sympathetic. Jokubas sends us word he's sympathetic. Neither of them sends us grain."));
						break;

					case "leave":
						await dialog.Msg(L("Greg walked to Liudas's holding twice. Second time they gave him a cup of beer and a letter for the other one."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("collectTailfruit", out var itemObj)) return;

				if (itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*She counts them into her apron and starts a pot before she says anything else*{/}"));
					await dialog.Msg(L("10, and 3 of them big enough to split. That's a meal for everyone and seconds for the small ones."));
					await dialog.Msg(L("Take what's in the camp tin. It's not much and it's honestly ours to give, which is more than most of what's in this forest."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still short. They're thickest along the eastern edge where the light gets in."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Two of the boys went out and got tailfruit themselves this morning. They came back filthy and pleased with themselves and I've decided not to be frightened about it."));
			}
		});

		// Quest 1002: Bracing the Lean-Tos
		//---------------------------------------------------------------------
		AddNpc(147484, L("[Refugee] Greg"), "f_maple_23_2", 1061, 130, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1002);

			dialog.SetTitle(L("Greg"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's holding both hands out in front of him, watching them shake, and quickly puts them behind his back when he notices you*{/}"));
				await dialog.Msg(L("Don't mind that. I built 4 lean-tos out of cart boards in one afternoon and I'm no carpenter, so all 4 of them lean the wrong way. One came down on a family last week."));
				await dialog.Msg(L("There are sturdy sticks all over this forest floor. Go to each of the 4 shelters and brace it properly - I'd do it myself but my hands shake since the road."));

				var response = await dialog.Select(L("Will you brace them?"),
					Option(L("I'll brace all 4 shelters"), "help"),
					Option(L("What happened on the road?"), "info"),
					Option(L("Build them again from scratch"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Cross-brace the back corner first, that's the one that folds. There's a stick pile at each shelter, I did manage that much."));
						break;

					case "info":
						await dialog.Msg(L("We walked 9 days with Colimen in the trees the whole way. Nothing came down on us. Nine days of nothing coming down on us."));
						await dialog.Msg(L("My hands were fine when we arrived. They started this a fortnight after, when it was over, which is apparently how it works."));
						break;

					case "leave":
						await dialog.Msg(L("With what? Every board we had went into those 4. There is no fifth cart."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("braceShelters", out var braceObj)) return;

				if (braceObj.Done)
				{
					await dialog.Msg(L("{#666666}*He puts his weight against the corner post of the nearest one and it does not move*{/}"));
					await dialog.Msg(L("That's the first thing in this camp I've been able to lean on. I mean that as literally as it sounds."));
					await dialog.Msg(L("Take my tool roll. There's a plane in it my father made and I'd rather it went to somebody with steady hands."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still shelters unbraced. All 4 - a camp with 3 good roofs and 1 bad one just means everybody argues about the bad one."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("It rained hard on Tuesday and nobody got wet. I sat in mine and listened to it and didn't do anything else all evening."));
			}
		});

		// Quest 1002 interaction points - the refugee lean-tos
		//---------------------------------------------------------------------
		void AddLeanTo(int shelterNumber, string observation, int x, int z, int direction)
		{
			AddNpc(147375, L("Refugee Lean-To"), "f_maple_23_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_maple_23_2", 1002);
				var variableKey = $"Laima.Quests.f_maple_23_2.Quest1002.Shelter{shelterNumber}";
				var counterKey = "Laima.Quests.f_maple_23_2.Quest1002.SheltersBraced";

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A lean-to of cart boards with a pile of sticks beside it*{/}"));
					return;
				}

				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*This one stands square now*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(
					L("Bracing the shelter..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(4)
				);

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);

					var braced = character.Variables.Perm.GetInt(counterKey, 0) + 1;
					character.Variables.Perm.Set(counterKey, braced);

					character.ServerMessage(observation);
					character.ServerMessage(LF("Shelters braced: {0}/4", braced));

					if (braced >= 4)
						character.ServerMessage(L("{#FFD700}All 4 shelters stand square. Return to Greg.{/}"));
				}
				else
				{
					character.ServerMessage(L("You leave the shelter as it is."));
				}
			});
		}

		AddLeanTo(1, L("First shelter braced. The back corner no longer folds under a push."), 1180, 220, 0);
		AddLeanTo(2, L("Second shelter braced. Somebody has chalked four names on the inside board."), 1330, 60, 90);
		AddLeanTo(3, L("Third shelter braced. This is the one that came down last week."), 1120, 30, 180);
		AddLeanTo(4, L("Fourth shelter braced. The roof boards sit flat for the first time."), 1279, 120, 305);

		// Quest 1003: Colimen in the North Holding
		//---------------------------------------------------------------------
		AddNpc(155022, L("[Lord] Liudas"), "f_maple_23_2", 87, 1639, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1003);

			dialog.SetTitle(L("Liudas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He looks you over from horseback before he bothers dismounting, deciding you're worth the trouble*{/}"));
				await dialog.Msg(L("You'll do, if you're not one of Jokubas's men. I hold the northern half of this forest - 11 years, whatever he's told you - and the Red Colimen have been in it since June, eating my timber stand."));
				await dialog.Msg(L("Kill 25 of them in the northern holding. I would send my own men but every man I have is watching the boundary in case Jokubas moves a fence post."));

				var response = await dialog.Select(L("Will you clear the northern holding?"),
					Option(L("I'll kill the Red Colimen"), "help"),
					Option(L("Why not just settle with Jokubas?"), "info"),
					Option(L("Move your men off the fence"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They cluster on the north face where the timber's thickest. Go at them from the open ground - inside the stand they'll come at you from above."));
						break;

					case "info":
						await dialog.Msg(L("Because settling means one of us is wrong, and I have spent 11 years and a great deal of money on not being wrong."));
						await dialog.Msg(L("His grandfather and my grandfather split this forest with a handshake and no paper. Every problem I have descends from that handshake."));
						break;

					case "leave":
						await dialog.Msg(L("The day I move my men off the boundary is the day the boundary moves. I did not invent this arrangement, I inherited it."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killColimen", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("My forester walked the north face this morning and came back without a scratch on him. He was so surprised he told me twice."));
					await dialog.Msg(L("Take this from the estate purse. And do not tell Jokubas what I paid, he will only match it out of spite."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Still Colimen in the stand. My forester will not go in and I have stopped pretending I blame him."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The timber stand is cutting again. First income off the northern half in 3 years and I intend to spend all of it on lawyers."));
			}
		});

		// Quest 1004: The Joquvas Family Shield
		//---------------------------------------------------------------------
		AddNpc(153176, L("[Lord] Jokubas"), "f_maple_23_2", -337, -768, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1004);

			dialog.SetTitle(L("Jokubas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*He's pinching the bridge of his nose, muttering something about Liudas before he even looks up*{/}"));
				await dialog.Msg(L("Oh, wonderful, a witness. My family shield hung in the southern lodge for 90 years. Last month a Yellow Caro walked in through a window that should not have been open and took it."));
				await dialog.Msg(L("It is a painted board. It is worth nothing. Kill 20 of the Caro and bring it back, because Liudas has already heard about it and I would like the story to end."));

				var response = await dialog.Select(L("Will you get the shield back?"),
					Option(L("I'll hunt the Caro and recover the shield"), "help"),
					Option(L("A Caro took a shield?"), "info"),
					Option(L("Let it go"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("They nest west of the lodge in the low scrub. Look for the mound with something painted sticking out of it - subtlety is not their strength."));
						break;

					case "info":
						await dialog.Msg(L("They take anything bright. The shield is bright. I am told this is well known to everyone in the district except me, which is a sentence I have heard a great deal lately."));
						await dialog.Msg(L("Roberta thinks it is funny. Roberta is paid by me and she thinks it is funny to my face, which I have decided to take as a good sign."));
						break;

					case "leave":
						await dialog.Msg(L("I would love to let it go. Liudas has been letting it go loudly at every dinner table between here and Klaipeda for a fortnight."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killCaro", out var killObj)) return;
				if (!quest.TryGetProgress("findShield", out var itemObj)) return;

				if (killObj.Done && itemObj.Done)
				{
					await dialog.Msg(L("{#666666}*He turns it over, finds a chew mark on the rim and laughs before he can stop himself*{/}"));
					await dialog.Msg(L("90 years on a wall and a fortnight in a scrub mound. It has more character now than it ever had."));
					await dialog.Msg(L("Take the reward and take it publicly, so the story people tell is the one where it came back."));

					character.Quests.Complete(questId);
				}
				else if (killObj.Done)
				{
					await dialog.Msg(L("You have thinned them and no shield. Then it is in a mound you have not opened yet - keep going west."));
				}
				else
				{
					await dialog.Msg(L("Still Caro in the western scrub. The one that has it will not be far from the nest it dragged it to."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("It is back on the wall with the chew mark facing out. Roberta suggested that and she was right, which is becoming a pattern."));
			}
		});

		// Quest 1005: The Chest of Deeds
		//---------------------------------------------------------------------
		AddNpc(155144, L("[Schoolmistress] Roberta"), "f_maple_23_2", -345, -732, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1005);
			var deliveredKey = "Laima.Quests.f_maple_23_2.Quest1005.Delivered";

			dialog.SetTitle(L("Roberta"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*She glances at the lodge door before speaking, then decides it doesn't matter and pulls a small chest out from under her shawl*{/}"));
				await dialog.Msg(L("You're not sworn to either household - that's exactly what I need. I teach Jokubas's letters and I've spent 3 years reading his family papers because nobody else in the lodge can. Last winter I found this, unopened since his grandfather."));
				await dialog.Msg(L("The deeds inside are water-ruined and the ink has run. Red Colimen fluid lifts run ink - bring me 8 of it, and then carry the chest down to Milia at the refugee camp, because I already know what the deeds say."));

				var response = await dialog.Select(L("Will you fetch the fluid and carry the chest?"),
					Option(L("I'll bring the fluid and take the chest to Milia"), "help"),
					Option(L("What do you think they say?"), "info"),
					Option(L("Show them to Jokubas first"), "leave")
				);

				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						character.Inventory.Add(666141, 1, InventoryAddType.PickUp);
						await dialog.Msg(L("The fluid comes out of a Red Colimen's back segment and it keeps about a day, so gather it in one go. Carry the chest level - the sheets inside are half paste."));
						break;

					case "info":
						await dialog.Msg(L("That neither grandfather ever owned this forest. It was held in common by the villages, and the two families were named as its wardens, which is a duty and not a title."));
						await dialog.Msg(L("Eleven years of boundary men and lawyers over a job description. Milia's people have more right to camp here than either lord has to argue about it."));
						break;

					case "leave":
						await dialog.Msg(L("If I show it to Jokubas first it goes in a drawer and I go back to teaching letters. I have thought about this for a whole winter."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (character.Variables.Perm.GetBool(deliveredKey, false))
				{
					await dialog.Msg(L("She read it. Good. Then it exists outside this lodge and there is nothing either of them can do about it now."));
					await dialog.Msg(L("Take my whole year's fee. I am fairly certain I am about to stop being paid it."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Fluid first, then the camp. Milia is the one at the statue with the cooking pot - she'll know what she's looking at."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Both lords have written to me. Liudas's letter is 4 pages and furious. Jokubas's is one line and it says 'come back to work on Monday'."));
			}
		});

		// Quest 1005 recipient - the chest goes to the camp
		//---------------------------------------------------------------------
		AddNpc(147489, L("[Refugee] Milia"), "f_maple_23_2", 1220, 90, 315, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_maple_23_2", 1005);
			var deliveredKey = "Laima.Quests.f_maple_23_2.Quest1005.Delivered";

			dialog.SetTitle(L("Milia"));

			if (character.Quests.IsActive(questId) && character.Inventory.HasItem(666141))
			{
				if (!character.Variables.Perm.GetBool(deliveredKey, false))
				{
					if (!character.Quests.TryGetById(questId, out var quest)) return;
					if (!quest.TryGetProgress("collectFluid", out var fluidObj)) return;

					if (!fluidObj.Done)
					{
						await dialog.Msg(L("{#666666}*The sheets in the chest are still a grey paste. Nothing can be read from them yet*{/}"));
						return;
					}

					await dialog.Msg(L("{#666666}*She works the fluid over the top sheet with a rag and the ink comes up brown and readable*{/}"));
					await dialog.Msg(L("Held in common by the villages. Wardens, not owners. I can read that much and I only ever learned to read prices."));
					await dialog.Msg(L("Forty of us have been apologising for standing here since spring. Tell the schoolmistress we are done apologising."));

					character.Variables.Perm.Set(deliveredKey, true);
					character.Quests.CompleteObjective(questId, "deliverChest");
					character.ServerMessage(L("{#FFD700}Milia has read the deeds. Return to Roberta.{/}"));
				}
				else
				{
					await dialog.Msg(L("Go on back to her. She's owed the news more than I am."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Greg nailed a copy of the top sheet to the statue post. Liudas's man read it, went white, and rode off without saying a word."));
			}
			else
			{
				await dialog.Msg(L("Forty of us, one grain sack and two lords writing us sympathetic letters. That's the whole situation."));
			}
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Leafnut Tailfruit
//-----------------------------------------------------------------------------

public class LeafnutTailfruitQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1001);
		SetName(L("Leafnut Tailfruit"));
		SetType(QuestType.Sub);
		SetDescription(L("Forty refugees camped at the Vakarine statue have 4 days of grain left and no lord willing to feed them. Yellow Leafnuts carry a tailfruit that boils up into a meal."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Refugee] Milia"), "f_maple_23_2");

		AddObjective("collectTailfruit", L("Collect Leafnut Tailfruit from Yellow Leafnut"),
			new CollectItemObjective(666136, 10));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion

		AddDrop(666136, 0.50f, MonsterId.Leafnut_Yellow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666136, character.Inventory.CountItem(666136), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666136, character.Inventory.CountItem(666136), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1002 CLASS: Bracing the Lean-Tos
//-----------------------------------------------------------------------------

public class BracingTheLeanTosQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1002);
		SetName(L("Bracing the Lean-Tos"));
		SetType(QuestType.Sub);
		SetDescription(L("A refugee built 4 shelters out of cart boards in one afternoon and all 4 lean the wrong way. One already came down on a family. Brace every one of them."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Refugee] Greg"), "f_maple_23_2");

		AddObjective("braceShelters", L("Brace the 4 refugee shelters"),
			new VariableCheckObjective("Laima.Quests.f_maple_23_2.Quest1002.SheltersBraced", 4, true));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_23_2.Quest1002.SheltersBraced");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_23_2.Quest1002.Shelter{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_maple_23_2.Quest1002.SheltersBraced");

		for (var i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_maple_23_2.Quest1002.Shelter{i}");
	}
}

// Quest 1003 CLASS: Colimen in the North Holding
//-----------------------------------------------------------------------------

public class ColimenInTheNorthHoldingQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1003);
		SetName(L("Colimen in the North Holding"));
		SetType(QuestType.Sub);
		SetDescription(L("Red Colimen have been eating the northern timber stand since June, and every man Lord Liudas has is watching a boundary instead. Clear them off the north face."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lord] Liudas"), "f_maple_23_2");

		AddObjective("killColimen", L("Kill Red Colimen in the northern timber stand"),
			new KillObjective(25, new[] { MonsterId.Colimen_Red }));

		AddReward(new ExpReward(1000, 700));
		AddReward(new SilverReward(2200));
		AddReward(new ItemReward(640081, 2)); // Lv2 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
	}
}

// Quest 1004 CLASS: The Joquvas Family Shield
//-----------------------------------------------------------------------------

public class TheJoquvasFamilyShieldQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1004);
		SetName(L("The Joquvas Family Shield"));
		SetType(QuestType.Sub);
		SetDescription(L("A Yellow Caro walked into the southern lodge and took a painted board that had hung there for 90 years. Lord Jokubas wants it back before the story reaches another dinner table."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver(L("[Lord] Jokubas"), "f_maple_23_2");

		AddObjective("killCaro", L("Kill Yellow Caro in the western scrub"),
			new KillObjective(20, new[] { MonsterId.Caro_Yellow }));

		AddObjective("findShield", L("Recover the Joquvas Family Shield"),
			new CollectItemObjective(666142, 1));

		AddReward(new ExpReward(1550, 1090));
		AddReward(new SilverReward(2900));
		AddReward(new ItemReward(640082, 1)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 2)); // Normal HP Potion
		AddReward(new ItemReward(640006, 2)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(666142, 0.20f, MonsterId.Caro_Yellow);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666142, character.Inventory.CountItem(666142), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666142, character.Inventory.CountItem(666142), InventoryItemRemoveMsg.Destroyed);
	}
}

// Quest 1005 CLASS: The Chest of Deeds
//-----------------------------------------------------------------------------

public class TheChestOfDeedsQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_maple_23_2", 1005);
		SetName(L("The Chest of Deeds"));
		SetType(QuestType.Sub);
		SetDescription(L("A schoolmistress found a jeweled chest of water-ruined deeds that prove neither lord ever owned Pistis Forest. Lift the run ink with Red Colimen fluid and put the chest in the refugees' hands."));
		SetLocation("f_maple_23_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.Sequential);
		AddQuestGiver(L("[Schoolmistress] Roberta"), "f_maple_23_2");

		AddObjective("collectFluid", L("Collect Red Colimen Fluid"),
			new CollectItemObjective(666140, 8));

		AddObjective("deliverChest", L("Take the Small Jeweled Chest to Milia at the refugee camp"),
			new ManualObjective());

		AddReward(new ExpReward(3100, 2200));
		AddReward(new SilverReward(5000));
		AddReward(new ItemReward(640082, 2)); // Lv3 EXP Card
		AddReward(new ItemReward(640003, 3)); // Normal HP Potion
		AddReward(new ItemReward(640006, 3)); // Normal SP Potion
		AddReward(new ItemReward(640009, 1)); // Stamina Potion

		AddDrop(666140, 0.50f, MonsterId.Colimen_Red);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(666140, character.Inventory.CountItem(666140), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666141, character.Inventory.CountItem(666141), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_maple_23_2.Quest1005.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(666140, character.Inventory.CountItem(666140), InventoryItemRemoveMsg.Destroyed);
		character.Inventory.Remove(666141, character.Inventory.CountItem(666141), InventoryItemRemoveMsg.Destroyed);

		character.Variables.Perm.Remove("Laima.Quests.f_maple_23_2.Quest1005.Delivered");
	}
}
