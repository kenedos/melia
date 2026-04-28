//--- Melia Script ----------------------------------------------------------
// Grynas Training Camp - Quest NPCs
//--- Description -----------------------------------------------------------
// A mix of the living and the dead haunt this dark-forest tract.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.Scripting.Dialogues;

public class FKatyn452QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		Npc AddGhostNpc(int model, string name, string map, double x, double z, double direction, DialogFunc dialog)
		{
			var npc = AddNpc(model, name, map, x, z, direction, dialog);
			npc.AddEffect(new ColorEffect(255, 150, 50, 150, 0.01f));
			return npc;
		}

		// Quest 1001: Marius (Woodwarden) — alive, normal NPC position
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Woodwarden] Marius"), "f_katyn_45_2", 977, 645, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1001);
			dialog.SetTitle(L("Marius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A hollow-eyed warden leans against a trunk whose bark has puckered into something like a face*{/}"));
				await dialog.Msg(L("Ridimed are circling lone walkers in slow inward spirals. The trees hear them. The knots turn. Something is being rehearsed."));
				var response = await dialog.Select(L("Twenty Ridimed put down. Will you?"),
					Option(L("I'll put down twenty"), "help"),
					Option(L("Rehearsing what?"), "info"),
					Option(L("Not my problem"), "leave")
				);
				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't run. Don't answer your own name if the wood calls it."));
						break;
					case "info":
						await dialog.Msg(L("I don't know. Old foresters say the Ridimed copy what the wood teaches them. I don't want to be here when the lesson ends."));
						break;
					case "leave":
						await dialog.Msg(L("Walk the edge-paths, then. The middle is where the spirals form."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killRidimed", out var killObj)) return;
				if (killObj.Done)
				{
					await dialog.Msg(L("The wood's quieter. Not quiet. Quieter. Take your purse and a salt-pouch."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Twenty. Don't stop in the center of a spiral."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The knots stopped turning for three nights. Then started again, slower. We count that a win in this wood."));
			}
		});

		// Quest 1002 giver: Nijole (Shrine-Keeper) — alive, shrine position
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Shrine-Keeper] Nijole"), "f_katyn_45_2", 268, 136, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1002);
			dialog.SetTitle(L("Nijole"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A quiet keeper braids red thread through a small bone-and-wax charm*{/}"));
				await dialog.Msg(L("The Deep Shrine's ward-thread snapped on the new moon. My sister kept that shrine. She is gone now, but her ghost still tends the stone. The charm I'm finishing replaces the thread."));
				var response = await dialog.Select(L("Carry this charm to my sister Dalia at the cliff-shrine. Bring back her ward-token so I know the replacement took."),
					Option(L("I'll carry the charm"), "help"),
					Option(L("Her ghost?"), "info"),
					Option(L("Shrine business stays with shrines"), "leave")
				);
				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Don't ring the bells on the path. The cliff-shrine answers only its own bells."));
						break;
					case "info":
						await dialog.Msg(L("She jumped. The wood was speaking through her, and she would not let it. Her ghost still tends the stone where she fell."));
						break;
					case "leave":
						await dialog.Msg(L("Then her stone keeps swallowing words. I'll find another carrier."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverCharm", out var deliverObj)) return;
				if (deliverObj.Done)
				{
					await dialog.Msg(L("Her token. Warm. The ward took. That's all I needed to know."));
					await dialog.Msg(L("Keeper's coin. And a strand of charm-thread - keeps small things from speaking to you at night."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("South-east. The cliff. Don't ring the bells."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("My sister's stone speaks again. She and I exchange words across death. Strange comfort, but a comfort still."));
			}
		});

		// Quest 1002 recipient: Dalia (Deep-Shrine) — GHOST, staring down cliff
		//---------------------------------------------------------------------
		AddGhostNpc(155131, L("[Restless Soul] Cliff-Sister"), "f_katyn_45_2", 2105, 436, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1002);
			dialog.SetTitle(L("Cliff-Sister"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("The cliff... I stare down the cliff... I jumped from this cliff..."));
					await dialog.Msg(L("The wood... The wood was speaking through me... I would not let it... So I jumped... I jumped..."));
					await dialog.Msg(L("The bells... My sister's bells... You bring the charm... At last..."));
					await dialog.Msg(L("{#666666}*She binds the charm to a small shrine-stone at the cliff edge. The stone settles*{/}"));
					await dialog.Msg(L("Carry her my token... Tell her... Tell her I no longer hear the wood..."));
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Cliff-Sister's ward-token received. Return to Shrine-Keeper Nijole.{/}"));
				}
				else await dialog.Msg(L("Back to her... Back to my sister... Tell her I am quiet at last..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The stone speaks clean again... I hear my sister... Twice a day..."));
			}
			else
			{
				await dialog.Msg(L("The cliff... The cliff... I cannot look away..."));
			}
		});

		// Quest 1003: Egle (Dream-Herbalist) — GHOST, looking at pond (drowned)
		//---------------------------------------------------------------------
		AddGhostNpc(154017, L("[Restless Soul] Pond-Herbalist"), "f_katyn_45_2", -519, 1476, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1003);
			dialog.SetTitle(L("Pond-Herbalist"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("The pond... The pond... I see my own face in the pond... It is not my face anymore..."));
				var response = await dialog.Select(L("..."),
					Option(L("What happened?"), "info"),
					Option(L("I will help you rest"), "help"),
					Option(L("Leave"), "leave")
				);
				if (response == "info")
				{
					await dialog.Msg(L("Bark-strips... I cut bark from a watching-tree... A knot opened... I should have walked away..."));
					await dialog.Msg(L("It chased me to the pond... It pushed me under... I drank the pond... I drank the pond..."));
					var r2 = await dialog.Select(L("..."),
						Option(L("I will finish your dream-draught"), "help"),
						Option(L("Rest..."), "leave")
					);
					if (r2 == "help") { character.Quests.Start(questId); await dialog.Msg(L("Eight bark-strips... Closed knots only... Never an open knot... Never... Never...")); }
				}
				else if (response == "help")
				{
					character.Quests.Start(questId);
					await dialog.Msg(L("Eight bark-strips... Closed knots only... Never an open knot... Never... Never..."));
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var barkCount = character.Inventory.CountItem(650851);
				if (barkCount >= 8)
				{
					await dialog.Msg(L("Eight strips... My draught will steep at last... I see my face again... My own face..."));
					await dialog.Msg(L("Take this herbalist's purse... Salt-water vial... For when something follows you..."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Closed knots... Don't hum... The bark remembers melodies..."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The pond shows my face... My own face at last... I will leave the pond..."));
			}
		});

		// Quest 1004: Audrius (Forest-Scholar) — alive, staring at creepy statues
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Forest-Scholar] Audrius"), "f_katyn_45_2", -842, -815, 225, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_katyn_45_2", 1004);
			dialog.SetTitle(L("Audrius"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young scholar studies a circle of weathered statues. He does not turn his back on them*{/}"));
				await dialog.Msg(L("These statues watch the wood, and the wood watches them. Four trees nearby keep their knot-eyes open through the day. My paper called this folklore. My eyes call it otherwise."));
				var response = await dialog.Select(L("Walk the four marked trees. Tell me whether each knot is closed, weeping sap, or - the worst case - actively tracking your hand. Will you?"),
					Option(L("I'll walk the four trees"), "help"),
					Option(L("Tracking?"), "info"),
					Option(L("Not for trees"), "leave")
				);
				switch (response)
				{
					case "help":
						character.Quests.Start(questId);
						await dialog.Msg(L("Closed knot: still grain. Weeping: clear sap. Tracking: grain follows your hand."));
						await dialog.Msg(L("If one tracks - leave. Don't return alone. Don't return."));
						break;
					case "info":
						await dialog.Msg(L("Tracking means the tree has enough awareness to follow gestures. We don't want this wood to be active."));
						break;
					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself. Slower, but my eyes still work. For now."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var treesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 0);
				if (treesChecked >= 4)
				{
					await dialog.Msg(L("One tree fully active. Above the threshold my paper called folklore. I'll forward this to Fedimian tonight."));
					await dialog.Msg(L("Scholar's stipend. Thank you. The fourth tree gets a keep-clear circle."));
					character.Quests.Complete(questId);
				}
				else await dialog.Msg(L("Four trees. Wave past each knot. Don't stare back."));
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Wardmage set a circle around the active tree. It closed for a week, then re-opened. We watch."));
			}
		});

		// Watching-tree inspection points (kept as inspection pattern)
		//---------------------------------------------------------------------
		void AddKnotTree(int treeNumber, string treeName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(treeName), "f_katyn_45_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_katyn_45_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A marked tree*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_katyn_45_2.Quest1004.Tree{treeNumber}";
				if (character.Variables.Perm.GetBool(variableKey, false))
				{
					await dialog.Msg(L("{#666666}*Already read*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Waving past the knot..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var treesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", treesChecked + 1);
					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Trees read: {0}/4", treesChecked + 1));
					if (treesChecked + 1 >= 4)
						character.ServerMessage(L("{#FFD700}All trees read! Return to Forest-Scholar Audrius.{/}"));
				}
				else
				{
					character.ServerMessage(L("Reading interrupted. You step back a pace."));
				}
			});
		}

		AddKnotTree(1, "West Watching-Tree", "West Tree: knot closed tight, grain still. Dormant.", -299, 1111, 90);
		AddKnotTree(2, "North Watching-Tree", "North Tree: ringed by fresh sap. Grain still.", 502, 790, 180);
		AddKnotTree(3, "East Watching-Tree", "East Tree: sap beading and grain shifted half an inch sideways as your hand passed.", 1120, 10, 270);
		AddKnotTree(4, "Deep Watching-Tree", "Deep Tree: grain followed your hand across a full arc. Knot remained open.", 595, -1781, 0);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

public class TheCirclingRidimedQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1001);
		SetName("The Circling Ridimed");
		SetType(QuestType.Sub);
		SetDescription("Woodwarden Marius needs twenty Blue Ridimed put down before their inward spirals complete whatever the wood is rehearsing.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Woodwarden] Marius", "f_katyn_45_2");
		AddObjective("killRidimed", "Defeat Blue Ridimed",
			new KillObjective(20, new[] { MonsterId.Ridimed_Blue }));
		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));
	}
}

public class TheWardingCharmQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1002);
		SetName("The Warding Charm");
		SetType(QuestType.Sub);
		SetDescription("Shrine-Keeper Nijole's warding charm must reach her sister's ghost at the cliff-shrine, and her ward-token must return.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Shrine-Keeper] Nijole", "f_katyn_45_2");
		AddObjective("deliverCharm", "Deliver the charm to the Cliff-Sister and return with the token",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_2.Quest1002.Delivered", 1, true));
		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(8000));
		AddReward(new ItemReward(640085, 1));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1002.Delivered");
	}
}

public class BarkFromTheWatchingTreesQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1003);
		SetName("The Pond-Herbalist's Bark");
		SetType(QuestType.Sub);
		SetDescription("A drowned herbalist's ghost begs for eight bark-strips from closed-knot watching-trees so her unfinished dream-draught may steep.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Restless Soul] Pond-Herbalist", "f_katyn_45_2");
		AddObjective("collectBark", "Gather twisted bark-strips",
			new CollectItemObjective(650851, 8));
		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
		AddReward(new ItemReward(640012, 1));

		AddDrop(650851, 0.40f, MonsterId.Pappus_Kepa_Purple);
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650851, character.Inventory.CountItem(650851), InventoryItemRemoveMsg.Destroyed);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650851, character.Inventory.CountItem(650851), InventoryItemRemoveMsg.Destroyed);
	}
}

public class WhichKnotEyesAreOpenQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_katyn_45_2", 1004);
		SetName("Which Knot-Eyes Are Open");
		SetType(QuestType.Sub);
		SetDescription("Forest-Scholar Audrius has asked you to read the four watching-trees and determine which still sleep, which weep sap, and which actively track movement.");
		SetLocation("f_katyn_45_2");
		SetAutoTracked(true);
		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Forest-Scholar] Audrius", "f_katyn_45_2");
		AddObjective("readTrees", "Read the four watching-trees",
			new VariableCheckObjective("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked", 4, true));
		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(11200));
		AddReward(new ItemReward(640085, 2));
		AddReward(new ItemReward(640004, 2));
		AddReward(new ItemReward(640007, 2));
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1004.Tree{i}");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_katyn_45_2.Quest1004.TreesChecked");
		for (int i = 1; i <= 4; i++)
			character.Variables.Perm.Remove($"Laima.Quests.f_katyn_45_2.Quest1004.Tree{i}");
	}
}
