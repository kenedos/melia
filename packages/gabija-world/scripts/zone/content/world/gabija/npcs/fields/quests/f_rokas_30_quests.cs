//--- Melia Script ----------------------------------------------------------
// King's Plateau Quest NPCs
//--- Description -----------------------------------------------------------
// The historians who have never heard of Rexipher, the four altars of the
// Royal Mausoleum, and the stonemason's treasure map.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class FRokas30QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq1 = new QuestId(20189);
	private readonly static QuestId Mq1Bridge = new QuestId(20150);
	private readonly static QuestId Mq2 = new QuestId(20190);
	private readonly static QuestId Mq2Sub = new QuestId(19350);
	private readonly static QuestId Mq3 = new QuestId(20191);
	private readonly static QuestId Mq5 = new QuestId(20193);
	private readonly static QuestId Mq6 = new QuestId(20194);
	private readonly static QuestId Mq6Sub = new QuestId(19360);
	private readonly static QuestId Mq7 = new QuestId(20195);
	private readonly static QuestId Mq8 = new QuestId(20196);
	private readonly static QuestId Hq01 = new QuestId(9110);

	private readonly static QuestId Pipoti1 = new QuestId(1053);
	private readonly static QuestId Pipoti2 = new QuestId(1054);
	private readonly static QuestId Pipoti3 = new QuestId(1055);
	private readonly static QuestId Pipoti4 = new QuestId(1056);
	private readonly static QuestId Pipoti5 = new QuestId(1057);

	protected override void Load()
	{
		// Liaison Officer Bale
		//-------------------------------------------------------------------------
		AddNpc(20108, L("Liaison Officer Bale"), "ROKAS30_BAIL", "f_rokas_30", 950.13, -782.25, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Liaison Officer Bale"));

			if (character.Quests.IsActive(Mq1) && character.Quests.IsCompletable(Mq1))
			{
				await dialog.Msg(L("I searched the list and asked other people about it, however... there's no one with the name Rexipher."));
				await dialog.Msg(L("I am sorry that I couldn't be of help. Historian Colin keeps the older rolls - try him."));
				await dialog.CompleteQuest(Mq1);
				return;
			}

			if (!character.Quests.Has(Mq1) && character.Quests.MeetsPrerequisites(Mq1))
			{
				var answer = await dialog.SelectQuestOffer(Mq1, L("Are you looking for someone? Can you tell me the name of the person you are looking for?"),
					Option(L("I'll ask another historian"), "accept"),
					Option(L("Stop looking for Rexipher"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1);
					character.Quests.CompleteObjective(Mq1, "askBale");
					await dialog.Msg(L("Rexipher, was it? Give me a moment with the roll."));
				}
				return;
			}

			await dialog.Msg(L("Everyone coming through King's Plateau is written down here. Almost everyone."));
		});

		// Historian Colin
		//-------------------------------------------------------------------------
		AddNpc(147421, L("Historian Colin"), "ROKAS30_COLLIN", "f_rokas_30", 1140, -476, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Colin"));

			if (character.Quests.IsActive(Mq1Bridge) && character.Quests.IsCompletable(Mq1Bridge))
			{
				await dialog.Msg(L("Rexipher? I've never heard this name before."));
				await dialog.Msg(L("Cyrenia Odell reads further back than I do. Ask her."));
				await dialog.CompleteQuest(Mq1Bridge);
				return;
			}

			if (character.Quests.IsActive(Hq01) && character.Quests.IsCompletable(Hq01))
			{
				await dialog.Msg(L("The one who fell while researching... Return to the goddess by becoming a part of the canyon."));
				await dialog.Msg(L("The saddened mind breaks the gold stones of the canyon, and the poured drink soaks into the ground on behalf of the tears."));
				await dialog.CompleteQuest(Hq01);
				return;
			}

			if (!character.Quests.Has(Mq1Bridge) && character.Quests.MeetsPrerequisites(Mq1Bridge))
			{
				var answer = await dialog.SelectQuestOffer(Mq1Bridge, L("Rexipher? I've never heard this name before. Shall I point you at the others?"),
					Option(L("Find the other historian"), "accept"),
					Option(L("Stop looking for Rexipher"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq1Bridge);
					character.Quests.CompleteObjective(Mq1Bridge, "askColin");
					await dialog.Msg(L("Let me think on the name a moment."));
				}
				return;
			}

			if (!character.Quests.Has(Hq01) && character.Quests.MeetsPrerequisites(Hq01))
			{
				await dialog.Msg(L("This place is like a grave for many explorers and historians."));
				await dialog.Msg(L("We are studying solid historical truths on top of their graves."));

				var answer = await dialog.SelectQuestOffer(Hq01, L("Can you burn this oration in front of the epitaph at Nepatogus Field in Rukas Plateau? You have walked more of this valley than I have."),
					Option(L("I will burn the oration and comfort the souls"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Hq01);
					character.Inventory.Add(ItemId.ROKAS_30_HQ01_ITEM, 1, InventoryAddType.PickUp);
					await dialog.Msg(L("Nepatogus Field is at the far north-west of Rukas Plateau. The epitaph is easy to miss."));
				}
				return;
			}

			if (character.Quests.IsActive(Hq01))
			{
				await dialog.Msg(L("The epitaph is at Nepatogus Field, in Rukas Plateau. Burn it where they can see the smoke."));
				return;
			}

			await dialog.Msg(L("Every name that ever passed the crossroads is written down somewhere. Finding which somewhere is the work."));
		});

		// Historian Cyrenia Odell, at the camp
		//-------------------------------------------------------------------------
		AddConditionalNpc(147345, L("Historian Cyrenia Odell"), "ROKAS30_ODEL", "f_rokas_30", 1551, 410, 90, c => !c.Quests.HasCompleted(Mq5), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Cyrenia Odell"));
			dialog.SetPortrait("Dlg_port_Cyrenia_Odell");

			if (!character.Quests.Has(Mq2) && character.Quests.MeetsPrerequisites(Mq2))
			{
				var answer = await dialog.SelectQuestOffer(Mq2, L("Rexipher? I am not sure. Are you sure that's the correct name?"),
					Option(L("I'm sure that is the name"), "accept"),
					Option(L("Stop looking for Rexipher"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq2);
					await dialog.Msg(L("Hold on for a minute. I've seen this before from a book."));
					await dialog.Msg(L("Wasn't Rexipher a Demon Lord who is part of the army of Vaiga, the Demon King?"));
					await dialog.Msg(L("He must have a lot of courage if he runs around with the name Rexipher."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq2))
			{
				await dialog.Msg(L("Whoever he is, he walked out of Rukas Plateau with your symbols. Keep tracing him."));
				return;
			}

			await dialog.Msg(L("The Royal Mausoleum was not built to be opened. That is the only thing every record agrees on."));
		});

		// Historian Cyrenia Odell, at the Gedulah Altar
		//-------------------------------------------------------------------------
		AddConditionalNpc(147345, L("Historian Cyrenia Odell"), "ROKAS_ODEL2", "f_rokas_30", 145, 385, 90, c => c.Quests.HasCompleted(Mq5) && !c.Quests.HasCompleted(Mq8), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Cyrenia Odell"));
			dialog.SetPortrait("Dlg_port_Cyrenia_Odell");

			if (character.Quests.IsActive(Mq6) && character.Quests.IsCompletable(Mq6))
			{
				await dialog.Msg(L("So the Tzedej Altar has been attacked too?"));
				await dialog.Msg(L("Regardless, we will still have a chance if we protect the other altars."));
				await dialog.CompleteQuest(Mq6);
				return;
			}

			if (character.Quests.IsActive(Mq5) && character.Quests.IsCompletable(Mq5))
			{
				await dialog.Msg(L("The Gedulah Altar is holding. That is one more than I expected."));
				await dialog.CompleteQuest(Mq5);
				return;
			}

			if (!character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6))
			{
				var answer = await dialog.SelectQuestOffer(Mq6, L("As Laulas' injuries are severe, I'm here in his stead. The seal is released - what exactly happened here?"),
					Option(L("Explain about Rexipher and the symbols"), "accept"),
					Option(L("About the Altar here"), "explain"),
					Option(L("Better keep quiet"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("The altars here are one of the Royal Mausoleum's various defenses built to ward off intruders."));
					await dialog.Msg(L("They should've been reactivated, but since they were built such a long time ago, no one knows how."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq6);
					await dialog.Msg(L("I had no idea it came from Rukas Plateau."));
					await dialog.Msg(L("What is he trying to do by releasing the seal?"));
					await dialog.Msg(L("Go to the Tzedej Altar nearby and activate it before he reaches it."));
				}
				return;
			}

			if (!character.Quests.Has(Mq7) && character.Quests.MeetsPrerequisites(Mq7))
			{
				var answer = await dialog.SelectQuestOffer(Mq7, L("Please go to the Sviesa Altar. I will follow you."),
					Option(L("I will go"), "accept"),
					Option(L("Ask her to wait a bit"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq7);
					await dialog.Msg(L("Either Rexipher is going to destroy the altar or we're going to activate it."));
					await dialog.Msg(L("Get to the Sviesa Altar and don't look back!"));
				}
				return;
			}

			if (character.Quests.IsActive(Mq6))
			{
				await dialog.Msg(L("What will Rexipher do?"));
				return;
			}

			if (character.Quests.IsActive(Mq7))
			{
				await dialog.Msg(L("The Sviesa Altar is west of here, past the ridge."));
				return;
			}

			await dialog.Msg(L("Four altars, and we are one short already."));
		});

		// Historian Cyrenia Odell, at the Viesha Altar
		//-------------------------------------------------------------------------
		AddConditionalNpc(147345, L("Historian Cyrenia Odell"), "ROKAS30_ODEL_KIDNAP", "f_rokas_30", -1397, -493, 90, c => c.Quests.IsActive(Mq8), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Historian Cyrenia Odell"));
			dialog.SetPortrait("Dlg_port_Cyrenia_Odell");

			if (character.Quests.IsActive(Mq8) && !character.Quests.IsCompletable(Mq8))
			{
				await dialog.Msg(L("The Sviesa Altar is awake. Three of four - that is better than I dared hope."));
				character.Quests.StartQuestTrack(Mq8);
				return;
			}

			await dialog.Msg(L("Three of the four altars are awake."));
		});

		// Wounded Historian Laulas
		//-------------------------------------------------------------------------
		AddNpc(152002, L("Wounded Historian Laulas"), "ROKAS30_HURT", "f_rokas_30", 752.55, 475, 17, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Wounded Historian Laulas"));

			if (!character.Quests.Has(Mq3) && character.Quests.MeetsPrerequisites(Mq3))
			{
				await dialog.Msg(L("He is a crazy person who can control monsters."));
				await dialog.Msg(L("He's the reason I'm wounded like this, and he also destroyed the altar which I used for research."));

				var answer = await dialog.SelectQuestOffer(Mq3, L("A man who looks like your historian is taking the Chesed and Gedula Altars one by one."),
					Option(L("Go to the Chesed Altar"), "accept"),
					Option(L("Stop looking for Rexipher"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq3);
					await dialog.Msg(L("A human controlling monsters... That's the first time I've seen such a thing."));
					await dialog.Msg(L("Also, I don't understand why he even broke the altar."));
				}
				return;
			}

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("The Chesed Altar is north of the camp. Go before he finishes with it."));
				return;
			}

			await dialog.Msg(L("Leave me here. My legs will not carry me to another altar."));
		});

		// The Chesed Altar
		//-------------------------------------------------------------------------
		AddConditionalNpc(47102, L("Chesed Altar"), "ROKAS30_SEALDESTROY1", "f_rokas_30", 542, 1065, 90, c => !c.Quests.HasCompleted(Mq3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Chesed Altar"));

			if (character.Quests.IsActive(Mq3))
			{
				await dialog.Msg(L("The altar's stone is already split. Rexipher's Hogma hold the ground around it."));
				character.Quests.ClearQuestTrack(Mq3);
				return;
			}

			await dialog.Msg(L("One of the four altars that keep the Royal Mausoleum shut."));
		});

		// The Gedulah Altar
		//-------------------------------------------------------------------------
		AddNpc(47102, L("Gedulah Altar"), "ROKAS30_SAELDEVICE1", "f_rokas_30", -205, 484, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Gedulah Altar"));

			if (character.Quests.IsActive(Mq5) && !character.Quests.IsCompletable(Mq5))
			{
				var sealed5 = await character.TimeActions.StartAsync(L("Releasing the seal..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (sealed5 != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq5, "wakeGedulah");

				character.ServerMessage(L("The Gedulah Altar is awake. Cyrenia Odell has come up to the altar."));
				character.LookAround();
				return;
			}

			await dialog.Msg(L("One of the four altars that keep the Royal Mausoleum shut. Its light is steady."));
		});

		// The Tzedej Altar
		//-------------------------------------------------------------------------
		AddNpc(47102, L("Tzedej Altar"), "ROKAS30_SEALDESTROY2", "f_rokas_30", -310, -250, 91, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Tzedej Altar"));

			if (character.Quests.IsActive(Mq6Sub) && character.Quests.IsCompletable(Mq6Sub))
			{
				await dialog.Msg(L("The statues are rubble and the altar is awake."));
				await dialog.CompleteQuest(Mq6Sub);
				return;
			}

			if (character.Quests.IsActive(Mq6) && !character.Quests.IsCompletable(Mq6))
			{
				var opened = await character.TimeActions.StartAsync(L("Releasing the seal..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.StartQuestTrack(Mq6);
				return;
			}

			if (character.Quests.IsActive(Mq6Sub))
			{
				character.Quests.StartQuestTrack(Mq6Sub);
				return;
			}

			if (!character.Quests.Has(Mq6Sub) && !character.Quests.Has(Mq6) && character.Quests.MeetsPrerequisites(Mq6Sub))
			{
				var answer = await dialog.SelectQuestOffer(Mq6Sub, L("The Tzedej Altar in King's Plateau is not yet activated."),
					Option(L("Activate the altar"), "accept"),
					Option(L("Leave it sealed"), "leave")
				);

				if (answer == "accept")
				{
					var opened = await character.TimeActions.StartAsync(L("Releasing the seal..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

					if (opened != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq6Sub);
					character.Quests.StartQuestTrack(Mq6Sub);
				}
				return;
			}

			await dialog.Msg(L("One of the four altars that keep the Royal Mausoleum shut."));
		});

		// The Sviesa Altar
		//-------------------------------------------------------------------------
		AddNpc(47102, L("Sviesa Altar"), "ROKAS30_SAELDEVICE2", "f_rokas_30", -1351, -873, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Sviesa Altar"));

			if (character.Quests.IsActive(Mq7) && !character.Quests.IsCompletable(Mq7))
			{
				var opened = await character.TimeActions.StartAsync(L("Releasing the seal..."), L("Cancel"), "MAKING", TimeSpan.FromSeconds(3));

				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq7, "wakeSviesa");

				character.ServerMessage(L("The Sviesa Altar is awake. Rexipher did not reach this one."));
				return;
			}

			await dialog.Msg(L("One of the four altars that keep the Royal Mausoleum shut. Its light is steady."));
		});

		// Body of a Soldier
		//-------------------------------------------------------------------------
		AddConditionalNpc(10023, L("Body of a Soldier"), "ROKAS30_MQ2_1_SCAR", "f_rokas_30", 741, 623, 0, c => !c.Quests.HasCompleted(Mq2Sub), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Body of a Soldier"));

			if (character.Quests.IsActive(Mq2Sub) && character.Quests.IsCompletable(Mq2Sub))
			{
				var prayed = await character.TimeActions.StartAsync(L("Praying for the dead..."), L("Cancel"), "WORSHIP", TimeSpan.FromSeconds(4));

				if (prayed != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The ground is quiet again. You say what there is to say over him."));
				await dialog.CompleteQuest(Mq2Sub);
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq2Sub))
			{
				character.Quests.StartQuestTrack(Mq2Sub);
				return;
			}

			if (!character.Quests.Has(Mq2Sub) && character.Quests.MeetsPrerequisites(Mq2Sub))
			{
				var answer = await dialog.SelectQuestOffer(Mq2Sub, L("There are many dead people around here, left where the raid caught them."),
					Option(L("Pray for the dead"), "accept"),
					Option(L("Walk on"), "leave")
				);

				if (answer == "accept")
				{
					var prayed = await character.TimeActions.StartAsync(L("Praying for the dead..."), L("Cancel"), "WORSHIP", TimeSpan.FromSeconds(2));

					if (prayed != TimeActionResult.Completed)
						return;

					character.Quests.Start(Mq2Sub);
					character.Quests.StartQuestTrack(Mq2Sub);
				}
				return;
			}

			await dialog.Msg(L("A mercenary, face down where the Hogma left him."));
		});

		// Stonemason Pipoti
		//-------------------------------------------------------------------------
		AddConditionalNpc(20109, L("Stonemason Pipoti"), "ROKAS30_PIPOTI", "f_rokas_30", 1406, 662, 0, c => !c.Quests.HasCompleted(Pipoti1), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Stonemason Pipoti"));

			if (character.Quests.IsActive(Pipoti1) && character.Quests.IsCompletable(Pipoti1))
			{
				await dialog.Msg(L("My colleague is dead? That can't be."));
				await dialog.Msg(L("If only I had not tempted him..."));
				await dialog.Msg(L("Take the family map. I have no use for it now. Right click it and it will show you where to dig."));
				await dialog.CompleteQuest(Pipoti1);
				character.LookAround();
				return;
			}

			if (!character.Quests.Has(Pipoti1) && character.Quests.MeetsPrerequisites(Pipoti1))
			{
				var answer = await dialog.SelectQuestOffer(Pipoti1, L("The colleague who I used to work with has disappeared. He was definitely with me at the Forest of Fireflies, but I won't go back there. It's scary."),
					Option(L("I will find your colleague"), "accept"),
					Option(L("I will come back soon"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Pipoti1);
					await dialog.Msg(L("Where is he? I am scared."));
				}
				return;
			}

			if (character.Quests.IsActive(Pipoti1))
			{
				await dialog.Msg(L("The Forest of Fireflies is south of the camp. Please hurry."));
				return;
			}

			await dialog.Msg(L("Stone is honest work. Everything else in this valley is not."));
		});

		// Marked spots on the treasure map
		//-------------------------------------------------------------------------
		AddNpc(20025, L("Marked Spot"), "ROKAS30_PIPOTI02_TRIGGER", "f_rokas_30", 359, 1078, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Marked Spot"));

			if (character.Quests.IsActive(Pipoti2))
			{
				await dialog.Msg(L("The chest is here, and so is Yonazolem."));
				character.Quests.ClearQuestTrack(Pipoti2);
				return;
			}

			if (!character.Quests.Has(Pipoti2) && character.Quests.MeetsPrerequisites(Pipoti2))
			{
				var answer = await dialog.SelectQuestOffer(Pipoti2, L("This is the first mark on the stonemason's map."),
					Option(L("Check the marked spot"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					var checked2 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2.5));

					if (checked2 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Pipoti2);
					character.Quests.StartQuestTrack(Pipoti2);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Ground that matches a mark on somebody's map."));
		});

		AddNpc(20025, L("Marked Spot"), "ROKAS30_PIPOTI03_TRIGGER", "f_rokas_30", -220.82, 716.85, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Marked Spot"));

			if (character.Quests.IsActive(Pipoti3))
			{
				await dialog.Msg(L("The Hogma are still standing over the chest."));
				character.Quests.ClearQuestTrack(Pipoti3);
				return;
			}

			if (!character.Quests.Has(Pipoti3) && character.Quests.MeetsPrerequisites(Pipoti3))
			{
				var answer = await dialog.SelectQuestOffer(Pipoti3, L("This is the second mark on the stonemason's map."),
					Option(L("Check the marked spot"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					var checked3 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

					if (checked3 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Pipoti3);
					character.Quests.StartQuestTrack(Pipoti3);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Ground that matches a mark on somebody's map."));
		});

		AddNpc(20025, L("Marked Spot"), "ROKAS30_PIPOTI04_TRIGGER", "f_rokas_30", 34.87, -311.34, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Marked Spot"));

			if (character.Quests.IsActive(Pipoti4))
			{
				await dialog.Msg(L("The monsters are still between you and the chest."));
				character.Quests.ClearQuestTrack(Pipoti4);
				return;
			}

			if (!character.Quests.Has(Pipoti4) && character.Quests.MeetsPrerequisites(Pipoti4))
			{
				var answer = await dialog.SelectQuestOffer(Pipoti4, L("This is the third mark on the stonemason's map."),
					Option(L("Check the marked spot"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					var checked4 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

					if (checked4 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Pipoti4);
					character.Quests.StartQuestTrack(Pipoti4);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Ground that matches a mark on somebody's map."));
		});

		AddNpc(20025, L("Marked Spot"), "ROKAS30_PIPOTI05_TRIGGER", "f_rokas_30", -1491.98, -416.77, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Marked Spot"));

			if (character.Quests.IsActive(Pipoti5))
			{
				await dialog.Msg(L("The Werewolf is still on the ridge above the chest."));
				character.Quests.ClearQuestTrack(Pipoti5);
				return;
			}

			if (!character.Quests.Has(Pipoti5) && character.Quests.MeetsPrerequisites(Pipoti5))
			{
				var answer = await dialog.SelectQuestOffer(Pipoti5, L("This is the last mark on the stonemason's map."),
					Option(L("Check the marked spot"), "accept"),
					Option(L("Leave it"), "leave")
				);

				if (answer == "accept")
				{
					var checked5 = await character.TimeActions.StartAsync(L("Checking it..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

					if (checked5 != TimeActionResult.Completed)
						return;

					character.Quests.Start(Pipoti5);
					character.Quests.StartQuestTrack(Pipoti5);
					character.LookAround();
				}
				return;
			}

			await dialog.Msg(L("Ground that matches a mark on somebody's map."));
		});

		// The chests the marks lead to
		//-------------------------------------------------------------------------
		AddConditionalNpc(147392, L("Treasure Chest"), "ROKAS30_PIPOTI02_TREASUREBOX", "f_rokas_30", 359.90, 1077.96, 91, c => c.Quests.IsActive(Pipoti2), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Chest"));

			if (character.Quests.IsActive(Pipoti2) && character.Quests.IsCompletable(Pipoti2))
			{
				await dialog.Msg(L("The key turns, the lid comes up - and the chest fades out of your hands before you can reach in."));
				await dialog.Msg(L("Right click the map again and find the next mark."));
				await dialog.CompleteQuest(Pipoti2);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A locked chest. Yonazolem has the key."));
		});

		AddConditionalNpc(147392, L("Treasure Chest"), "ROKAS30_PIPOTI03_TREASUREBOX", "f_rokas_30", -239, 727, 90, c => c.Quests.IsActive(Pipoti3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Chest"));

			if (character.Quests.IsActive(Pipoti3) && character.Quests.IsCompletable(Pipoti3))
			{
				await dialog.Msg(L("This treasure chest is empty."));
				await dialog.Msg(L("Right click the map again and find the next mark."));
				await dialog.CompleteQuest(Pipoti3);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A chest, with Hogma standing over it."));
		});

		AddConditionalNpc(147392, L("Treasure Chest"), "ROKAS30_PIPOTI04_TREASUREBOX", "f_rokas_30", 34.87, -311.34, -85, c => c.Quests.IsActive(Pipoti4), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Chest"));

			if (character.Quests.IsActive(Pipoti4) && character.Quests.IsCompletable(Pipoti4))
			{
				await dialog.Msg(L("Empty again."));
				await dialog.Msg(L("Right click the map again and find the next mark."));
				await dialog.CompleteQuest(Pipoti4);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A chest, with monsters standing over it."));
		});

		AddConditionalNpc(147392, L("Treasure Chest"), "ROKAS30_PIPOTI05_TREASUREBOX", "f_rokas_30", -1510.02, -414.29, 90, c => c.Quests.IsActive(Pipoti5), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Treasure Chest"));

			if (character.Quests.IsActive(Pipoti5) && character.Quests.IsCompletable(Pipoti5))
			{
				await dialog.Msg(L("The stonemason family's treasure is not here either."));
				await dialog.Msg(L("Pipoti is gone from the camp, but a man like that turns up again."));
				await dialog.CompleteQuest(Pipoti5);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("A chest, with a Werewolf on the ridge above it."));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		// Where the road out of the camp is ambushed.
		AddQuestTrigger("ROKAS30_SUDDEN_ATTACK", "f_rokas_30", 1018, 389, 150, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq2) && !character.Quests.IsCompletable(Mq2))
			{
				character.Quests.CompleteObjective(Mq2, "traceRexipher");
				character.ServerMessage(L("Rexipher is a demon's name, and the trail out of the camp is fresh."));
			}

			await Task.CompletedTask;
		});

		// The approach to the Chesed Altar.
		AddQuestTrigger("ROKAS30_SEALPRODECT_1", "f_rokas_30", 539, 992, 50, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq3) && !character.Quests.IsCompletable(Mq3))
				character.Quests.StartQuestTrack(Mq3);

			await Task.CompletedTask;
		});

		// The way into the Forest of Fireflies.
		AddQuestTrigger("ROKAS30_PIPOTI01_TRIGGER", "f_rokas_30", 1240, -221, 100, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Pipoti1) && !character.Quests.IsCompletable(Pipoti1))
				character.Quests.StartQuestTrack(Pipoti1);

			await Task.CompletedTask;
		});

		// The epitaph at Nepatogus Field, over in Rukas Plateau.
		AddQuestTrigger("ROKAS_30_HQ01_EPITAPH", "f_rokas_29", -485, -2083, 50, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Hq01) && !character.Quests.IsCompletable(Hq01))
			{
				character.Quests.CompleteObjective(Hq01, "burnOration");
				character.Inventory.RemoveItem(ItemId.ROKAS_30_HQ01_ITEM, 1);
				character.ServerMessage(L("The oration burns down to nothing in front of the epitaph. Report back to Historian Colin."));
			}

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20189: Rexipher, the Missing Historian (1)
//-----------------------------------------------------------------------------
public class Rokas30Mq1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20189);
		SetName(L("Rexipher, the Missing Historian (1)"));
		SetDescription(L("Rexipher walked off with the symbols. Ask Liaison Officer Bale at King's Plateau who he was."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_BAIL", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Historian Rexipher disappeared with the symbols of the ordeal. Talk to Liaison Officer Bale at King's Plateau to ask about his whereabouts."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_BAIL", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Liaison Officer Bale says he doesn't know Historian Rexipher. Ask the other historians for his whereabouts."));
		SetPhase(QuestStatus.Success, "ROKAS30_BAIL", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Liaison Officer Bale says he doesn't know Historian Rexipher. Ask the other historians for his whereabouts."));

		AddPrerequisite(new QuestStatusPrerequisite(20188, QuestStatus.Completed));

		AddObjective("askBale", L("Ask Liaison Officer Bale about Rexipher"), new ManualObjective());
	}
}

// 20150: Rexipher, the Missing Historian (2)
//-----------------------------------------------------------------------------
public class Rokas30Mq1BridgeQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20150);
		SetName(L("Rexipher, the Missing Historian (2)"));
		SetDescription(L("Bale has never heard the name. Ask the historians instead."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_COLLIN", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Liaison Officer Bale says he doesn't know Historian Rexipher. Ask the other historians for his whereabouts."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_COLLIN", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Historian Colin says he doesn't know Rexipher, either. Ask other historians for Rexipher's whereabouts."));
		SetPhase(QuestStatus.Success, "ROKAS30_COLLIN", "f_rokas_30", L("Find out Historian Rexipher's whereabouts"), L("Historian Colin says he doesn't know Rexipher, either. Ask other historians for Rexipher's whereabouts."));

		AddPrerequisite(new QuestStatusPrerequisite(20189, QuestStatus.Completed));

		AddObjective("askColin", L("Ask Historian Colin about Rexipher"), new ManualObjective());
	}
}

// 20190: Historian Rexipher's Identity
//-----------------------------------------------------------------------------
public class Rokas30Mq2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20190);
		SetName(L("Historian Rexipher's Identity"));
		SetDescription(L("Cyrenia Odell knows the name Rexipher, but not as a historian."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_ODEL", "f_rokas_30", L("Ask Historian Cyrenia Odell about Rexipher's whereabouts"), L("You must somehow find Rexipher since he has all the symbols of the ordeal. Ask her about him."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SUDDEN_ATTACK", "f_rokas_30", L("Trace Rexipher's whereabouts"), L("You were not able to find Rexipher's whereabouts, but you found out that Rexipher is a demon's name. Continue investigating Rexipher."));
		SetPhase(QuestStatus.Success, "ROKAS30_SUDDEN_ATTACK", "f_rokas_30", L("Trace Rexipher's whereabouts"), L("You were not able to find Rexipher's whereabouts, but you found out that Rexipher is a demon's name. Continue investigating Rexipher."));

		AddPrerequisite(new QuestStatusPrerequisite(20150, QuestStatus.Completed));

		AddObjective("traceRexipher", L("Trace Rexipher's whereabouts"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The trail is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}

// 19350: The dead body of the mercenary who has fallen
//-----------------------------------------------------------------------------
public class Rokas30Mq2SubQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19350);
		SetName(L("The dead body of the mercenary who has fallen"));
		SetDescription(L("The Hogma that killed the mercenary are still on the road he fell on."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_MQ2_1_SCAR", "f_rokas_30", L("Body of the collapsed mercenary"), L("There are many dead people around due to the monsters' raid. Approach and comfort them."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_MQ2_1_SCAR", "f_rokas_30", L("Ambush of Hogmas"), L("Hogmas hiding appeared when you touched the dead body. Defeat them."));
		SetPhase(QuestStatus.Success, "ROKAS30_MQ2_1_SCAR", "f_rokas_30", L("Comfort the body to rest in peace"), L("Defeated all the monsters nearby. Comfort the bodies to rest in peace."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_MQ2_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killAmbush", L("Defeat the Hogmas ambushing"), new KillObjective(5, "hogma_warrior", "hogma_archer", "hogma_sorcerer") { LayerOnly = true });
	}
}

// 20191: Rexipher's True Colors (1)
//-----------------------------------------------------------------------------
public class Rokas30Mq3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20191);
		SetName(L("Rexipher's True Colors (1)"));
		SetDescription(L("A man wearing Rexipher's face is taking the altars of the Royal Mausoleum one by one."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_HURT", "f_rokas_30", L("Trace Rexipher's whereabouts"), L("You were not able to find Rexipher's whereabouts, but you found out that Rexipher is a demon's name. Continue investigating Rexipher."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SEALPRODECT_1", "f_rokas_30", L("Go to the Chesed Altar where Rexipher is attacking"), L("A man who looks like Rexipher is attacking historians at the Chesed and Gedula Altars to take control of the area. First, go to the Chesed Altar."));
		SetPhase(QuestStatus.Success, "ROKAS30_SEALPRODECT_1", "f_rokas_30", L("Trace Rexipher's whereabouts"), L("A man who looks like Rexipher is attacking historians at the Chesed and Gedula Altars to take control of the area. First, go to the Chesed Altar."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_MQ4_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20190, QuestStatus.Completed));

		AddObjective("killHogma", L("Defeat the Hogmas"), new KillObjective(5, "hogma_warrior", "warleader_hogma", "Hogma_combat") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The kill is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("The Chesed Altar is lost. Reach the Gedulah Altar before Rexipher does!"));
		character.Quests.Complete(this.QuestId);

		if (!character.Quests.Has(new QuestId(20193)))
			character.Quests.Start(new QuestId(20193));
	}
}

// 20193: Rexipher's True Colors (2)
//-----------------------------------------------------------------------------
public class Rokas30Mq5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20193);
		SetName(L("Rexipher's True Colors (2)"));
		SetDescription(L("The Chesed Altar is gone. Wake the Gedulah Altar before Rexipher reaches it."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_SAELDEVICE1", "f_rokas_30", L("Find Rexipher"), L("Rexipher is trying to destroy all the altars at King's Plateau. Follow him and stop him."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SAELDEVICE1", "f_rokas_30", L("Activate the Gedulah Altar before Rexipher does"), L("The Chesed Altar has been destroyed! The remaining altars are in danger. Get to the Gedulah Altar before Rexipher and activate it."));
		SetPhase(QuestStatus.Success, "ROKAS_ODEL2", "f_rokas_30", L("Talk to Historian Cyrenia Odell"), L("Cyrenia Odell came to the altar. Talk to her."));

		AddPrerequisite(new QuestStatusPrerequisite(20191, QuestStatus.Completed));

		AddObjective("wakeGedulah", L("Activate the Gedulah Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 20194: Rexipher's True Colors (3)
//-----------------------------------------------------------------------------
public class Rokas30Mq6Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20194);
		SetName(L("Rexipher's True Colors (3)"));
		SetDescription(L("The Tzedej Altar is next, and the Hogma statues around it are not statues."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS_ODEL2", "f_rokas_30", L("Talk to Historian Cyrenia Odell"), L("Cyrenia Odell came to the altar. Talk to her."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SEALDESTROY2", "f_rokas_30", L("Activate the Tzedej Altar before Rexipher does"), L("Better do something before Rexipher does. First, go to the Tzedej Altar nearby and activate it."));
		SetPhase(QuestStatus.Success, "ROKAS_ODEL2", "f_rokas_30", L("Talk to Historian Cyrenia Odell"), L("Tzedej's seal has been activated. Tell Cyrenia Odell about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_MQ6_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20193, QuestStatus.Completed));

		AddObjective("killStatues", L("Defeat the Hogmas shown as stone statues"), new KillObjective(5, "Hogma_combat", "Hogma_guard", "warleader_hogma", "hogma_archer", "hogma_sorcerer") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 3));
	}
}

// 19360: Activate the Tzedej Altar
//-----------------------------------------------------------------------------
public class Rokas30Mq6SubQuest : QuestScript
{
	protected override void Load()
	{
		SetClientId(19360);
		SetName(L("Activate the Tzedej Altar"));
		SetDescription(L("The Tzedej Altar has never been woken, and the statues around it are waiting for whoever tries."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_SEALDESTROY2", "f_rokas_30", L("Tzedej Altar"), L("The Tzedej Altar in King's Plateau is not yet activated. Activate it."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SEALDESTROY2", "f_rokas_30", L("Monster Appeared"), L("The altar suddenly broke and monster statues showed up! Defeat the Hogmas from the statues!"));
		SetPhase(QuestStatus.Success, "ROKAS30_SEALDESTROY2", "f_rokas_30", L("Tzedej Altar"), L("The statues are rubble. Check the altar again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_MQ6_TRACK", 4000, autoStart: false, partyPlay: true);

		AddObjective("killStatues", L("Defeat the Hogmas shown as stone statues"), new KillObjective(5, "Hogma_combat", "Hogma_guard", "warleader_hogma", "hogma_archer", "hogma_sorcerer") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
	}
}

// 20195: Rexipher's True Colors (4)
//-----------------------------------------------------------------------------
public class Rokas30Mq7Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20195);
		SetName(L("Rexipher's True Colors (4)"));
		SetDescription(L("The Sviesa Altar is the last one still asleep. Wake it first."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS_ODEL2", "f_rokas_30", L("Talk to Historian Cyrenia Odell"), L("Talk to Cyrenia Odell and ask about the location of other altars."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_SAELDEVICE2", "f_rokas_30", L("Activate the Sviesa Altar before Rexipher does"), L("Go to the Sviesa Altar to activate it, preventing Rexipher's plan before he does it."));
		SetPhase(QuestStatus.Success, "ROKAS30_SAELDEVICE2", "f_rokas_30", L("Talk to Cyrenia Odell"), L("Fortunately, it seems that you protected the altar. Return to Cyrenia Odell."));

		AddPrerequisite(new QuestStatusPrerequisite(20194, QuestStatus.Completed));

		AddObjective("wakeSviesa", L("Activate the Sviesa Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard5", 2));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The altar is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);

		if (!character.Quests.Has(new QuestId(20196)))
			character.Quests.Start(new QuestId(20196));
	}
}

// 20196: Rexipher's True Colors (5)
//-----------------------------------------------------------------------------
public class Rokas30Mq8Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20196);
		SetName(L("Rexipher's True Colors (5)"));
		SetDescription(L("Cyrenia Odell came up to the Sviesa Altar, and so did Rexipher."));
		SetType(QuestType.Main);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_ODEL_KIDNAP", "f_rokas_30", L("Activate the Sviesa Altar"), L("The Tzedej Altar has been destroyed, but you successfully activated the Sviesa Altar. Talk to Historian Cyrenia Odell."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_ODEL_KIDNAP", "f_rokas_30", L("Go to Cyrenia Odell"), L("The Chesed Altar was destroyed, but you managed to release the seal of Sviesa Altar. Talk to Cyrenia Odell."));
		SetPhase(QuestStatus.Success, "ROKAS30_ODEL_KIDNAP", "f_rokas_30", L("Follow Rexipher"), L("Rexipher kidnapped Cyrenia Odell! Follow him to Zachariel Crossroads."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_MQ8_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20195, QuestStatus.Completed));

		AddObjective("seeTheKidnap", L("Go to Cyrenia Odell"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The cutscene is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
		character.LookAround();
	}
}

// 9110: Historian Colin's Favor
//-----------------------------------------------------------------------------
public class Rokas30Hq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(9110);
		SetName(L("Historian Colin's Favor"));
		SetDescription(L("Colin wants an oration burned for the explorers who died in this valley."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30", "f_rokas_29");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_COLLIN", "f_rokas_30", L("Talk to Historian Colin"), L("Colin looks lonely. Talk to Colin."));
		SetPhase(QuestStatus.InProgress, "ROKAS_30_HQ01_EPITAPH", "f_rokas_29", L("Burn the oration"), L("Historian Colin wants to honor the explorers who died while exploring this area. Burn the oration in their honor in front of the epitaph at Rukas Plateau."));
		SetPhase(QuestStatus.Success, "ROKAS30_COLLIN", "f_rokas_30", L("Report to Colin"), L("Burned the oration in front of the epitaph in Rukas Plateau. Report to Historian Colin."));

		// The client's only gate is a server-side script; the map's band is the
		// substitute.
		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("burnOration", L("Burn the oration at the epitaph in Rukas Plateau"), new ManualObjective());
	}
}

// 1053: Stonemason Pipoti's Friend
//-----------------------------------------------------------------------------
public class Rokas30Pipoti1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1053);
		SetName(L("Stonemason Pipoti's Friend"));
		SetDescription(L("Pipoti's colleague went into the Forest of Fireflies and never came back out."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_PIPOTI", "f_rokas_30", L("Talk to Stonemason Pipoti"), L("Stonemason Pipoti looks anxious. Try talking to him."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_PIPOTI01_TRIGGER", "f_rokas_30", L("Find Pipoti's colleague"), L("Stonemason Pipoti says his colleague suddenly disappeared. Find his colleague around the Forest of Fireflies."));
		SetPhase(QuestStatus.Success, "ROKAS30_PIPOTI", "f_rokas_30", L("Talk to Stonemason Pipoti"), L("Unfortunately, Pipoti's colleague did not make it alive. Return to Stonemason Pipoti."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_PIPOTI01_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(66));

		AddObjective("killScouts", L("Defeat the attacking monsters"), new KillObjective(10, "Hogma_guard") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
		AddReward(new ItemReward("ROKAS30_PIPOTI_MAP", 1));
	}
}

// 1054: Treasure Map of the Stonemason's Family (1)
//-----------------------------------------------------------------------------
public class Rokas30Pipoti2Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1054);
		SetName(L("Treasure Map of the Stonemason's Family (1)"));
		SetDescription(L("The first mark on the map is a locked chest with Yonazolem beside it."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_PIPOTI02_TRIGGER", "f_rokas_30", L("Move to the area marked on Stonemason Pipoti's map"), L("There is a mark on Stonemason Pipoti's map. Right-click on the map and move to the marked area."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_PIPOTI02_TRIGGER", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked on Stonemason Pipoti's map. Defeat Yonazolem and open the treasure box."));
		SetPhase(QuestStatus.Success, "ROKAS30_PIPOTI02_TREASUREBOX", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked on Stonemason Pipoti's map. Defeat Yonazolem and open the treasure box."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_PIPOTI02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1053, QuestStatus.Completed));

		AddPityDrop("ROKAS30_PIPOTI02_ITEM", 1.0f, 0, 1, "boss_yonazolem_Q2");

		AddObjective("takeKey", L("Obtain the Key from Yonazolem"), new CollectItemObjective("ROKAS30_PIPOTI02_ITEM", 1));

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("ROKAS30_PIPOTI02_ITEM"));
	}
}

// 1055: Treasure Map of the Stonemason's Family (2)
//-----------------------------------------------------------------------------
public class Rokas30Pipoti3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1055);
		SetName(L("Treasure Map of the Stonemason's Family (2)"));
		SetDescription(L("The second mark on the map, with Hogma Scouts already standing on it."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_PIPOTI03_TRIGGER", "f_rokas_30", L("Move to the marked area on Stonemason Pipoti's map"), L("There are other marked areas on Stonemason Pipoti's map. Right-click on the map and move to the marked area."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_PIPOTI03_TRIGGER", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked in Stonemason Pipoti's map. Defeat the monsters and open the treasure box."));
		SetPhase(QuestStatus.Success, "ROKAS30_PIPOTI03_TREASUREBOX", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked in Stonemason Pipoti's map. Defeat the monsters and open the treasure box."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_PIPOTI03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1054, QuestStatus.Completed));

		AddObjective("killScouts", L("Defeat the interfering Hogmas"), new KillObjective(6, "Hogma_guard") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 1056: Treasure Map of the Stonemason's Family (3)
//-----------------------------------------------------------------------------
public class Rokas30Pipoti4Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1056);
		SetName(L("Treasure Map of the Stonemason's Family (3)"));
		SetDescription(L("The third mark on the map, and another empty chest to reach."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_PIPOTI04_TRIGGER", "f_rokas_30", L("Move to the marked area on Stonemason Pipoti's map"), L("There are other marked areas on Stonemason Pipoti's map. Right-click on the map and move to the marked area."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_PIPOTI04_TRIGGER", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked in Stonemason Pipoti's map. Defeat the monsters and open the treasure box."));
		SetPhase(QuestStatus.Success, "ROKAS30_PIPOTI04_TREASUREBOX", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Something hidden is marked in Stonemason Pipoti's map. Defeat the monsters and open the treasure box."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_PIPOTI04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1055, QuestStatus.Completed));

		AddObjective("killInterference", L("Defeat the interfering monsters"), new KillObjective(5, "hogma_warrior", "hogma_sorcerer") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 1));
	}
}

// 1057: Treasure Map of the Stonemason's Family (4)
//-----------------------------------------------------------------------------
public class Rokas30Pipoti5Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(1057);
		SetName(L("Treasure Map of the Stonemason's Family (4)"));
		SetDescription(L("The last mark on the map, and a Werewolf on the ridge above it."));
		SetType(QuestType.Sub);
		SetLocation("f_rokas_30");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "ROKAS30_PIPOTI05_TRIGGER", "f_rokas_30", L("Move to the marked area on Stonemason Pipoti's map"), L("There are other marked areas on Stonemason Pipoti's map. Right-click on the map and move to the marked area."));
		SetPhase(QuestStatus.InProgress, "ROKAS30_PIPOTI05_TRIGGER", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("The map that Stonemason Pipoti gave you displayed where something was hidden. Open the box and defeat the interfering Werewolf."));
		SetPhase(QuestStatus.Success, "ROKAS30_PIPOTI05_TREASUREBOX", "f_rokas_30", L("Defeat the monsters and check the treasure chest"), L("Open the box."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "ROKAS30_PIPOTI05_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(1056, QuestStatus.Completed));

		AddObjective("killWerewolf", L("Werewolf extermination"), new KillObjective(1, "boss_werewolf_Q2") { LayerOnly = true });

		AddReward(new ItemReward("expCard5", 2));
		AddReward(new TakeItemReward("ROKAS30_PIPOTI_MAP"));
	}
}
