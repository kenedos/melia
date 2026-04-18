//--- Melia Script ----------------------------------------------------------
// Demon Prison District 2 - Quest NPCs
//--- Description -----------------------------------------------------------
// Demon hunter NPC with powder shop and repeatable collection quest.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DVelniasprison512QuestNpcsScript : GeneralScript
{
	private const int HardenedBlackCrystalId = 650554;

	protected override void Load()
	{
		this.CreateDemonHunterShop();

		// [Demon Hunter] Varkis
		//-------------------------------------------------------------------------
		AddNpc(57236, L("[Demon Hunter] Varkis"), "d_velniasprison_51_2", 1029.3206, 1644.7653, 90, this.DemonHunterDialog);
	}

	private void CreateDemonHunterShop()
	{
		CreateShop("DemonPrisonPowders", shop =>
		{
			shop.AddItem(649025, amount: 1, price: 100000);  // Nucle Powder
			shop.AddItem(649026, amount: 1, price: 1000000); // Sierra Powder
		});
	}

	private async Task DemonHunterDialog(Dialog dialog)
	{
		var character = dialog.Player;
		var questId = new QuestId("d_velniasprison_51_2", 1001);

		dialog.SetTitle(L("Varkis"));
		dialog.SetPortrait("Dlg_port_UosisKrieg2");

		var hasActiveQuest = character.Quests.IsActive(questId);

		if (hasActiveQuest)
		{
			if (!character.Quests.TryGetById(questId, out var quest))
				return;
			if (!quest.TryGetProgress("collectCrystals", out var collectObj))
				return;

			if (collectObj.Done)
			{
				var crystals = character.Inventory.CountItem(HardenedBlackCrystalId);
				var groups = crystals / 100;

				// OnComplete removes 100 crystals and ItemReward gives 1 Nucle
				// Powder, so the extras need to be settled manually here.
				var extraToRemove = (groups - 1) * 100;
				if (extraToRemove > 0)
					character.Inventory.Remove(HardenedBlackCrystalId, extraToRemove, InventoryItemRemoveMsg.Given);

				var extraPowders = groups - 1;
				if (extraPowders > 0)
					character.Inventory.Add(649025, extraPowders, InventoryAddType.PickUp);

				await dialog.Msg(LF("That's {0} crystals off the pile. Dense with demonic energy... refines into {1} jar(s) of Nucle Powder. The remainder stays on the books.", groups * 100, groups));
				character.Quests.Complete(questId);
				await this.OfferNewQuest(dialog, character, questId);
				return;
			}

			var response = await dialog.Select(L("Still gathering? The demons on deeper floors leave behind more of it."),
				Option(L("Open shop"), "shop"),
				Option(L("I'll keep at it"), "leave")
			);

			if (response == "shop")
				await dialog.OpenShop("DemonPrisonPowders");

			return;
		}

		await dialog.Msg(L("You look like you can handle yourself. Name's Varkis. I hunt demons in this prison and grind down what they leave behind into powder."));
		await dialog.Msg(L("When you kill these fiends, they leave behind a black powder. Hardened demonic residue. I refine that into Nucle and Sierra powders. I sell what I make, if you're interested."));

		var mainResponse = await dialog.Select(L("What'll it be?"),
			Option(L("Open shop"), "shop"),
			Option(L("I want work"), "quest"),
			Option(L("Just passing through"), "leave")
		);

		switch (mainResponse)
		{
			case "shop":
				await dialog.OpenShop("DemonPrisonPowders");
				break;

			case "quest":
				await this.OfferNewQuest(dialog, character, questId);
				break;
		}
	}

	private async Task OfferNewQuest(Dialog dialog, Character character, QuestId questId)
	{
		var response = await dialog.Select(L("I need that black powder for my next batch. Kill demons across any floor of this prison and collect what they leave behind. The deeper you go, the more you'll find."),
			Option(L("Consider it done"), "accept"),
			Option(L("Not right now"), "decline")
		);

		if (response == "accept")
		{
			await character.Quests.Start(questId);
			await dialog.Msg(L("Good. Any demon in this prison leaves it behind, but the ones on the lower floors drop it far more often. Bring me back 100."));
		}
	}
}

// Quest 1001 CLASS: Demon Prison Purge
//-----------------------------------------------------------------------------

public class DemonPrisonPurgeQuest : QuestScript
{
	private const int HardenedBlackCrystalId = 650554;

	protected override void Load()
	{
		this.SetId("d_velniasprison_51_2", 1001);
		this.SetName("Demon Prison Purge");
		this.SetType(QuestType.Repeat);
		this.SetDescription("Varkis needs 100 Hardened Black Crystals collected from demons across the Demon Prison. Deeper floors yield them more frequently.");
		this.SetLocation("d_velniasprison_51_2");
		this.SetAutoTracked(true);

		this.SetReceive(QuestReceiveType.Manual);
		this.SetCancelable(true);
		this.SetUnlock(QuestUnlockType.AllAtOnce);
		this.AddQuestGiver("[Demon Hunter] Varkis", "d_velniasprison_51_2");

		// District 1 - 0.50%
		this.AddDrop(HardenedBlackCrystalId, 0.005f, MonsterId.Yognome_Yellow);
		this.AddDrop(HardenedBlackCrystalId, 0.005f, MonsterId.Egnome_Yellow);
		this.AddDrop(HardenedBlackCrystalId, 0.005f, MonsterId.Gazing_Golem_Yellow);
		this.AddDrop(HardenedBlackCrystalId, 0.005f, MonsterId.Moya_Yellow);

		// District 2 - 0.75%
		this.AddDrop(HardenedBlackCrystalId, 0.0075f, MonsterId.Defender_Spider);
		this.AddDrop(HardenedBlackCrystalId, 0.0075f, MonsterId.Nuka);
		this.AddDrop(HardenedBlackCrystalId, 0.0075f, MonsterId.Harugal);
		this.AddDrop(HardenedBlackCrystalId, 0.0075f, MonsterId.Elet);

		// District 3 - 1.12%
		this.AddDrop(HardenedBlackCrystalId, 0.0112f, MonsterId.Hohen_Ritter);
		this.AddDrop(HardenedBlackCrystalId, 0.0112f, MonsterId.Hohen_Barkle);
		this.AddDrop(HardenedBlackCrystalId, 0.0112f, MonsterId.Hohen_Orben);
		this.AddDrop(HardenedBlackCrystalId, 0.0112f, MonsterId.Hohen_Mane);

		// District 4 - 1.67%
		this.AddDrop(HardenedBlackCrystalId, 0.0167f, MonsterId.Elma);
		this.AddDrop(HardenedBlackCrystalId, 0.0167f, MonsterId.Nuo);
		this.AddDrop(HardenedBlackCrystalId, 0.0167f, MonsterId.Harugal);
		this.AddDrop(HardenedBlackCrystalId, 0.0167f, MonsterId.Mushroom_Ent_Green);

		// District 5 - 2.50%
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Hohen_Mage);
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Hohen_Barkle);
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Hohen_Ritter);
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Hohen_Gulak);
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Hohen_Orben);
		this.AddDrop(HardenedBlackCrystalId, 0.025f, MonsterId.Harugal);

		this.AddObjective("collectCrystals", "Collect Hardened Black Crystals from Demon Prison",
			new CollectItemObjective(HardenedBlackCrystalId, 100));

		this.AddReward(new ExpReward(23800, 16200));
		this.AddReward(new ItemReward(640085, 2)); // Lv6 EXP Card
		this.AddReward(new ItemReward(649025, 1)); // Nucle Powder
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.RemoveItem(HardenedBlackCrystalId, 100);
	}

	public override void OnCancel(Character character, Quest quest)
	{
		var count = character.Inventory.CountItem(HardenedBlackCrystalId);
		if (count > 0)
			character.Inventory.Remove(HardenedBlackCrystalId, count, InventoryItemRemoveMsg.Destroyed);
	}
}
