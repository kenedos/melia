//--- Melia Script ----------------------------------------------------------
// Henry's Beauty Shop - Wig Crafter
//--- Description -----------------------------------------------------------
// Henry is a professional wig crafter who creates custom wigs from materials.
// Each wig has its own quest requiring specific materials that drop from monsters.
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
using static Melia.Zone.Scripting.Shortcuts;
using System.Threading.Tasks;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone;

//-----------------------------------------------------------------------------
// MATERIAL CONFIGURATION - CHANGE ITEM IDS HERE
//-----------------------------------------------------------------------------
// This section contains all material requirements for wig quests.
// To change a material item ID, edit it here in ONE place.
//-----------------------------------------------------------------------------

public static class WigMaterialConfig
{
	// Male Wig Materials
	public static readonly int[] MaleWig1Materials = { 645180, 645450, 645210 };  //  Two-block Ponytail - Vesper Wing, Karas Shell, Echad Shell
	public static readonly int[] MaleWig2Materials = { 645431, 645177, 645437 };  //  Side Swept - Armori Metal Piece, Hogma Leather, Ellomago Wing
	public static readonly int[] MaleWig3Materials = { 645218, 645256, 645373 };  //  Volume Wave - Groll Backbone, Wendigo Fang, Kowak Leather
	public static readonly int[] MaleWig4Materials = { 645390, 645196, 645197 };  //  Slight Wavy - Lemur Tail, Phyracon Wing, Shaman Doll Core
	public static readonly int[] MaleWig5Materials = { 645386, 645153, 645164 };  //  Wavy Ponytail - Rambear Fur, Ultanun Wing, Merog Horn
	public static readonly int[] MaleWig6Materials = { 645220, 645180, 645450 };  //  Iron Perm - Vekarabe Shell, Vesper Wing, Karas Shell
	public static readonly int[] MaleWig7Materials = { 645210, 645431, 645177 };  //  Long Moisty Wave - Echad Shell, Armori Metal Piece, Hogma Leather
	public static readonly int[] MaleWig8Materials = { 645437, 645218, 645256 };  //  Retro Long Quiff - Ellomago Wing, Groll Backbone, Wendigo Fang
	public static readonly int[] MaleWig9Materials = { 645460, 645565, 645592 };  //  Sky Doggy Hair (Male) - Chromadog Bones, Chupaluka Fur, Rabbee Fur
	public static readonly int[] MaleWig10Materials = { 645297, 645165, 645160 }; //  Faerie Dawn Hair (Male) - Hummingbird Wing, Chafperor Wing, Operor Thorn
	public static readonly int[] MaleWig11Materials = { 645682, 645702, 645594 }; //  Faerie Midnight Hair (Male) - Black Kepari Leaves, Darkness Crystal Fragment, Fan Leaf
	public static readonly int[] MaleWig12Materials = { 645373, 645390, 645196 }; //  Troubadour Hair (Male) - Kowak Leather, Lemur Tail, Phyracon Wing

	// Female Wig Materials
	public static readonly int[] FemaleWig1Materials = { 645197, 645386, 645153 };  // Twintail Chignon - Shaman Doll Core, Rambear Fur, Ultanun Wing
	public static readonly int[] FemaleWig2Materials = { 645164, 645220, 645180 };  // French Bob - Merog Horn, Vekarabe Shell, Vesper Wing
	public static readonly int[] FemaleWig3Materials = { 645450, 645210, 645431 };  // Braid Long - Karas Shell, Echad Shell, Armori Metal Piece
	public static readonly int[] FemaleWig4Materials = { 645177, 645437, 645218 };  // Classic Bob - Hogma Leather, Ellomago Wing, Groll Backbone
	public static readonly int[] FemaleWig5Materials = { 645256, 645373, 645390 };  // Twin Dumpling Long Wave - Wendigo Fang, Kowak Leather, Lemur Tail
	public static readonly int[] FemaleWig6Materials = { 645196, 645197, 645386 };  // Snapback Bridge Long - Phyracon Wing, Shaman Doll Core, Rambear Fur
	public static readonly int[] FemaleWig7Materials = { 645153, 645164, 645220 };  // Bridge Long - Ultanun Wing, Merog Horn, Vekarabe Shell
	public static readonly int[] FemaleWig8Materials = { 645180, 645450, 645210 };  // Tangle Up - Vesper Wing, Karas Shell, Echad Shell
	public static readonly int[] FemaleWig9Materials = { 645431, 645177, 645437 };  // Semi Wave Long Perm - Armori Metal Piece, Hogma Leather, Ellomago Wing
	public static readonly int[] FemaleWig10Materials = { 645593, 645595, 645200 }; // Golden Fox Hair (Female) - Honeybean Stinger, Honeymeli Stinger, Red Infrorocktor Shell
	public static readonly int[] FemaleWig11Materials = { 645662, 645677, 645709 }; // Lofty Snow Hair (Female) - Blue Wendigo Fur, White Spion Fur, Blue Lepusbunny Bones
	public static readonly int[] FemaleWig12Materials = { 645208, 645432, 645586 }; // Faerie Dawn Hair (Female) - Rocktor Skin, Ammon Backbone, Cyst Needle
	public static readonly int[] FemaleWig13Materials = { 645479, 645672, 645380 }; // Faerie Midnight Hair (Female) - High Vubbe Token, Red Lepusbunny Tail, Doyor Claw

	// Material quantities (adjust as needed)
	public const int Material1Count = 50;
	public const int Material2Count = 50;
	public const int Material3Count = 25;

	public static string GetObjectiveDescription(int itemId, int count)
	{
		var itemData = ZoneServer.Instance.Data.ItemDb.Find(itemId);
		if (itemData != null)
			return $"Collect {count} {itemData.Name}";
		else
			return $"Collect {count} items (ID: {itemId})";
	}
}

public class BeautyShopNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddNpc(161005, "[Wig Crafter] Henry", "c_barber_dress", 2, 29, 0, HenryDialog);
	}

	private async Task HenryDialog(Dialog dialog)
	{
		var character = dialog.Player;
		dialog.SetTitle("Henry");

		await dialog.Msg("Welcome! I'm Henry, master wig crafter. I've spent years perfecting my craft, and I take great pride in creating wigs that are second to none.");

		var selection = await dialog.Select("What can I help you with today?",
			Option("I'd like to commission a wig", "commission"),
			Option("Tell me about your wigs", "info"),
			Option("Show me your collection", "browse"),
			Option("Nothing for now", "leave")
		);

		switch (selection)
		{
			case "info":
				await dialog.Msg("Every wig I create is handcrafted using only the finest materials gathered from the wild.");
				await dialog.Msg("I don't cut corners. Bring me what I need, and I'll craft you something you can be proud to wear.");
				await dialog.Msg("I have designs for both men and women. Each one is a testament to my years of experience.");
				break;

			case "commission":
				await ShowWigCommissionMenu(dialog, character);
				break;

			case "browse":
				await dialog.Msg("Certainly. I'm confident you'll find something that catches your eye.");
				await ShowWigBrowseMenu(dialog, character);
				break;

			case "leave":
				await dialog.Msg("Very well. When you're ready for quality craftsmanship, you know where to find me.");
				break;
		}
	}

	private async Task ShowWigCommissionMenu(Dialog dialog, Character character)
	{
		var gender = character.Gender;

		await dialog.Msg("Alright, which style are you interested in?");

		var selection = await dialog.Select("Choose a category:",
			Option("Male Wigs", "male"),
			Option("Female Wigs", "female"),
			Option("Never mind", "back")
		);

		if (selection == "back")
			return;

		if (selection == "male")
		{
			await ShowMaleWigList(dialog, character);
		}
		else if (selection == "female")
		{
			await ShowFemaleWigList(dialog, character);
		}
	}

	private async Task ShowMaleWigList(Dialog dialog, Character character)
	{
		var selection = await dialog.Select("Which male wig design interests you?",
			Option("Two-block Ponytail", "m138"),
			Option("Side Swept", "m139"),
			Option("Volume Wave", "m140"),
			Option("Slight Wavy", "m141"),
			Option("Wavy Ponytail", "m142"),
			Option("Iron Perm", "m144"),
			Option("Long Moisty Wave", "m145"),
			Option("Retro Long Quiff", "m146"),
			Option("Sky Doggy Hair (Male)", "m147"),
			Option("Faerie Dawn Hair (Male)", "m153"),
			Option("Faerie Midnight Hair (Male)", "m154"),
			Option("Troubadour Hair (Male)", "m159"),
			Option("Go back", "back")
		);

		if (selection == "back")
			return;

		var wigNumber = int.Parse(selection.Substring(1));
		await StartWigQuest(dialog, character, "Male", wigNumber);
	}

	private async Task ShowFemaleWigList(Dialog dialog, Character character)
	{
		var selection = await dialog.Select("Which female wig design interests you?",
			Option("Twintail Chignon", "f139"),
			Option("French Bob", "f140"),
			Option("Braid Long", "f141"),
			Option("Classic Bob", "f142"),
			Option("Twin Dumpling Long Wave", "f143"),
			Option("Snapback Bridge Long", "f147"),
			Option("Bridge Long", "f148"),
			Option("Tangle Up", "f149"),
			Option("Semi Wave Long Perm", "f150"),
			Option("Golden Fox Hair (Female)", "f151"),
			Option("Lofty Snow Hair (Female)", "f152"),
			Option("Faerie Dawn Hair (Female)", "f153"),
			Option("Faerie Midnight Hair (Female)", "f154"),
			Option("Go back", "back")
		);

		if (selection == "back")
			return;

		var wigNumber = int.Parse(selection.Substring(1));
		await StartWigQuest(dialog, character, "Female", wigNumber);
	}

	private async Task StartWigQuest(Dialog dialog, Character character, string gender, int wigNumber)
	{
		var questId = new QuestId("BeautyShop", GetQuestIdForWig(gender, wigNumber));

		if (character.Quests.HasCompleted(questId))
		{
			await dialog.Msg("I've already crafted this one for you, and I must say, it turned out beautifully. Perhaps another style?");
			return;
		}

		if (character.Quests.Has(questId))
		{
			await CheckWigQuestProgress(dialog, character, gender, wigNumber);
			return;
		}

		await dialog.Msg($"The {GetWigName(gender, wigNumber)}. You have good taste.");
		await dialog.Msg("To craft this properly, I'll need specific materials from the wilderness. I only use the best.");

		if (await dialog.YesNo("Would you like to gather the materials for me?"))
		{
			character.Quests.Start(questId);
			await dialog.Msg("Excellent. Bring me the materials, and I'll create something worthy of my reputation.");
		}
		else
		{
			await dialog.Msg("I understand. Quality takes commitment. Come back when you're ready.");
		}
	}

	private async Task CheckWigQuestProgress(Dialog dialog, Character character, string gender, int wigNumber)
	{
		var questId = new QuestId("BeautyShop", GetQuestIdForWig(gender, wigNumber));
		var materials = GetMaterialsForWig(gender, wigNumber);

		var hasAll = true;
		foreach (var materialId in materials)
		{
			var count = character.Inventory.CountItem(materialId);
			var required = GetRequiredAmount(Array.IndexOf(materials, materialId));
			if (count < required)
			{
				hasAll = false;
				break;
			}
		}

		if (hasAll)
		{
			await dialog.Msg("Excellent. You've gathered everything I need. These materials are of good quality.");
			await dialog.Msg("Now watch closely. This is the result of years of dedication to my craft.");

			// Remove materials
			for (int i = 0; i < materials.Length; i++)
			{
				var required = GetRequiredAmount(i);
				character.Inventory.Remove(materials[i], required, InventoryItemRemoveMsg.Given);
			}

			await dialog.Msg("There. Another piece I can be proud of. Wear it well.");

			character.Quests.Complete(questId);
		}
		else
		{
			await dialog.Msg("Still gathering materials? Good work takes time. I'll be here when you're ready.");
		}
	}

	private async Task ShowWigBrowseMenu(Dialog dialog, Character character)
	{
		await dialog.Msg("My collection includes 12 styles for men and 13 for women. Each one represents countless hours of refining my technique.");
		await dialog.Msg("They all require specific materials from the wild. Commission one, and I'll show you what a master craftsman can do.");
	}

	private int GetQuestIdForWig(string gender, int wigNumber)
	{
		if (gender == "Male")
		{
			return wigNumber switch
			{
				138 => 1001,
				139 => 1002,
				140 => 1003,
				141 => 1004,
				142 => 1005,
				144 => 1006,
				145 => 1007,
				146 => 1008,
				147 => 1009,
				153 => 1010,
				154 => 1011,
				159 => 1012,
				_ => 1001
			};
		}
		else // Female
		{
			return wigNumber switch
			{
				139 => 2001,
				140 => 2002,
				141 => 2003,
				142 => 2004,
				143 => 2005,
				147 => 2006,
				148 => 2007,
				149 => 2008,
				150 => 2009,
				151 => 2010,
				152 => 2011,
				153 => 2012,
				154 => 2013,
				_ => 2001
			};
		}
	}

	private int[] GetMaterialsForWig(string gender, int wigNumber)
	{
		if (gender == "Male")
		{
			return wigNumber switch
			{
				138 => WigMaterialConfig.MaleWig1Materials,
				139 => WigMaterialConfig.MaleWig2Materials,
				140 => WigMaterialConfig.MaleWig3Materials,
				141 => WigMaterialConfig.MaleWig4Materials,
				142 => WigMaterialConfig.MaleWig5Materials,
				144 => WigMaterialConfig.MaleWig6Materials,
				145 => WigMaterialConfig.MaleWig7Materials,
				146 => WigMaterialConfig.MaleWig8Materials,
				147 => WigMaterialConfig.MaleWig9Materials,
				153 => WigMaterialConfig.MaleWig10Materials,
				154 => WigMaterialConfig.MaleWig11Materials,
				159 => WigMaterialConfig.MaleWig12Materials,
				_ => WigMaterialConfig.MaleWig1Materials
			};
		}
		else // Female
		{
			return wigNumber switch
			{
				139 => WigMaterialConfig.FemaleWig1Materials,
				140 => WigMaterialConfig.FemaleWig2Materials,
				141 => WigMaterialConfig.FemaleWig3Materials,
				142 => WigMaterialConfig.FemaleWig4Materials,
				143 => WigMaterialConfig.FemaleWig5Materials,
				147 => WigMaterialConfig.FemaleWig6Materials,
				148 => WigMaterialConfig.FemaleWig7Materials,
				149 => WigMaterialConfig.FemaleWig8Materials,
				150 => WigMaterialConfig.FemaleWig9Materials,
				151 => WigMaterialConfig.FemaleWig10Materials,
				152 => WigMaterialConfig.FemaleWig11Materials,
				153 => WigMaterialConfig.FemaleWig12Materials,
				154 => WigMaterialConfig.FemaleWig13Materials,
				_ => WigMaterialConfig.FemaleWig1Materials
			};
		}
	}

	private string GetWigName(string gender, int wigNumber)
	{
		if (gender == "Male")
		{
			return wigNumber switch
			{
				138 => "Two-block Ponytail",
				139 => "Side Swept",
				140 => "Volume Wave",
				141 => "Slight Wavy",
				142 => "Wavy Ponytail",
				144 => "Iron Perm",
				145 => "Long Moisty Wave",
				146 => "Retro Long Quiff",
				147 => "Sky Doggy Hair",
				153 => "Faerie Dawn Hair",
				154 => "Faerie Midnight Hair",
				159 => "Troubadour Hair",
				_ => "Custom Wig"
			};
		}
		else // Female
		{
			return wigNumber switch
			{
				139 => "Twintail Chignon",
				140 => "French Bob",
				141 => "Braid Long",
				142 => "Classic Bob",
				143 => "Twin Dumpling Long Wave",
				147 => "Snapback Bridge Long",
				148 => "Bridge Long",
				149 => "Tangle Up",
				150 => "Semi Wave Long Perm",
				151 => "Golden Fox Hair",
				152 => "Lofty Snow Hair",
				153 => "Faerie Dawn Hair",
				154 => "Faerie Midnight Hair",
				_ => "Custom Wig"
			};
		}
	}

	private int GetRequiredAmount(int materialIndex)
	{
		return materialIndex switch
		{
			0 => WigMaterialConfig.Material1Count,
			1 => WigMaterialConfig.Material2Count,
			2 => WigMaterialConfig.Material3Count,
			_ => 10
		};
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS - MALE WIGS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Wig Style M-138 (Two-block Ponytail)
//-----------------------------------------------------------------------------

public class MaleWig138Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1001);
		SetName("Two-block Ponytail Wig");
		SetDescription("Gather materials for Henry to craft a Two-block Ponytail wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig1Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig1Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig1Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig1Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig1Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig1Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12147, 1));
	}
}

// Quest 1002: Wig Style M-139 (Side Swept)
//-----------------------------------------------------------------------------

public class MaleWig139Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1002);
		SetName("Side Swept Wig");
		SetDescription("Gather materials for Henry to craft a Side Swept wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig2Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig2Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig2Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig2Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig2Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig2Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12148, 1));
	}
}

// Quest 1003: Wig Style M-140 (Volume Wave)
//-----------------------------------------------------------------------------

public class MaleWig140Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1003);
		SetName("Volume Wave Wig");
		SetDescription("Gather materials for Henry to craft a Volume Wave wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig3Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig3Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig3Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig3Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig3Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig3Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12149, 1));
	}
}

// Quest 1004: Wig Style M-141 (Slight Wavy)
//-----------------------------------------------------------------------------

public class MaleWig141Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1004);
		SetName("Slight Wavy Wig");
		SetDescription("Gather materials for Henry to craft a Slight Wavy wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig4Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig4Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig4Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig4Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig4Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig4Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12150, 1));
	}
}

// Quest 1005: Wig Style M-142 (Wavy Ponytail)
//-----------------------------------------------------------------------------

public class MaleWig142Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1005);
		SetName("Wavy Ponytail Wig");
		SetDescription("Gather materials for Henry to craft a Wavy Ponytail wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig5Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig5Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig5Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig5Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig5Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig5Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12151, 1));
	}
}

// Quest 1006: Wig Style M-144 (Iron Perm)
//-----------------------------------------------------------------------------

public class MaleWig144Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1006);
		SetName("Iron Perm Wig");
		SetDescription("Gather materials for Henry to craft an Iron Perm wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig6Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig6Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig6Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig6Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig6Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig6Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12153, 1));
	}
}

// Quest 1007: Wig Style M-145 (Long Moisty Wave)
//-----------------------------------------------------------------------------

public class MaleWig145Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1007);
		SetName("Long Moisty Wave Wig");
		SetDescription("Gather materials for Henry to craft a Long Moisty Wave wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig7Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig7Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig7Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig7Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig7Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig7Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12154, 1));
	}
}

// Quest 1008: Wig Style M-146 (Retro Long Quiff)
//-----------------------------------------------------------------------------

public class MaleWig146Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1008);
		SetName("Retro Long Quiff Wig");
		SetDescription("Gather materials for Henry to craft a Retro Long Quiff wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig8Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig8Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig8Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig8Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig8Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig8Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12155, 1));
	}
}

// Quest 1009: Wig Style M-147 (Sky Doggy Hair)
//-----------------------------------------------------------------------------

public class MaleWig147Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1009);
		SetName("Sky Doggy Hair (Male) Wig");
		SetDescription("Gather materials for Henry to craft a Sky Doggy Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig9Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig9Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig9Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig9Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig9Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig9Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12156, 1));
	}
}

// Quest 1010: Wig Style M-153 (Faerie Dawn Hair)
//-----------------------------------------------------------------------------

public class MaleWig153Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1010);
		SetName("Faerie Dawn Hair (Male) Wig");
		SetDescription("Gather materials for Henry to craft a Faerie Dawn Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig10Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig10Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig10Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig10Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig10Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig10Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12157, 1));
	}
}

// Quest 1011: Wig Style M-154 (Faerie Midnight Hair)
//-----------------------------------------------------------------------------

public class MaleWig154Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1011);
		SetName("Faerie Midnight Hair (Male) Wig");
		SetDescription("Gather materials for Henry to craft a Faerie Midnight Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig11Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig11Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig11Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig11Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig11Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig11Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12158, 1));
	}
}

// Quest 1012: Wig Style M-159 (Troubadour Hair)
//-----------------------------------------------------------------------------

public class MaleWig159Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 1012);
		SetName("Troubadour Hair (Male) Wig");
		SetDescription("Gather materials for Henry to craft a Troubadour Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig12Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig12Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig12Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig12Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.MaleWig12Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.MaleWig12Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12159, 1));
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS - FEMALE WIGS
//-----------------------------------------------------------------------------

// Quest 2001 CLASS: Wig Style F-139 (Twintail Chignon)
//-----------------------------------------------------------------------------

public class FemaleWig139Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2001);
		SetName("Twintail Chignon Wig");
		SetDescription("Gather materials for Henry to craft a Twintail Chignon wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig1Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig1Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig1Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig1Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig1Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig1Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12539, 1));
	}
}

// Quest 2002: Wig Style F-140 (French Bob)
//-----------------------------------------------------------------------------

public class FemaleWig140Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2002);
		SetName("French Bob Wig");
		SetDescription("Gather materials for Henry to craft a French Bob wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig2Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig2Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig2Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig2Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig2Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig2Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12540, 1));
	}
}

// Quest 2003: Wig Style F-141 (Braid Long)
//-----------------------------------------------------------------------------

public class FemaleWig141Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2003);
		SetName("Braid Long Wig");
		SetDescription("Gather materials for Henry to craft a Braid Long wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig3Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig3Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig3Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig3Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig3Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig3Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12541, 1));
	}
}

// Quest 2004: Wig Style F-142 (Classic Bob)
//-----------------------------------------------------------------------------

public class FemaleWig142Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2004);
		SetName("Classic Bob Wig");
		SetDescription("Gather materials for Henry to craft a Classic Bob wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig4Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig4Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig4Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig4Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig4Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig4Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12542, 1));
	}
}

// Quest 2005: Wig Style F-143 (Twin Dumpling Long Wave)
//-----------------------------------------------------------------------------

public class FemaleWig143Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2005);
		SetName("Twin Dumpling Long Wave Wig");
		SetDescription("Gather materials for Henry to craft a Twin Dumpling Long Wave wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig5Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig5Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig5Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig5Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig5Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig5Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12543, 1));
	}
}

// Quest 2006: Wig Style F-147 (Snapback Bridge Long)
//-----------------------------------------------------------------------------

public class FemaleWig147Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2006);
		SetName("Snapback Bridge Long Wig");
		SetDescription("Gather materials for Henry to craft a Snapback Bridge Long wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig6Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig6Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig6Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig6Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig6Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig6Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12547, 1));
	}
}

// Quest 2007: Wig Style F-148 (Bridge Long)
//-----------------------------------------------------------------------------

public class FemaleWig148Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2007);
		SetName("Bridge Long Wig");
		SetDescription("Gather materials for Henry to craft a Bridge Long wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig7Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig7Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig7Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig7Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig7Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig7Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12548, 1));
	}
}

// Quest 2008: Wig Style F-149 (Tangle Up)
//-----------------------------------------------------------------------------

public class FemaleWig149Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2008);
		SetName("Tangle Up Wig");
		SetDescription("Gather materials for Henry to craft a Tangle Up wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig8Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig8Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig8Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig8Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig8Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig8Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12549, 1));
	}
}

// Quest 2009: Wig Style F-150 (Semi Wave Long Perm)
//-----------------------------------------------------------------------------

public class FemaleWig150Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2009);
		SetName("Semi Wave Long Perm Wig");
		SetDescription("Gather materials for Henry to craft a Semi Wave Long Perm wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig9Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig9Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig9Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig9Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig9Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig9Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12550, 1));
	}
}

// Quest 2010: Wig Style F-151 (Golden Fox Hair)
//-----------------------------------------------------------------------------

public class FemaleWig151Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2010);
		SetName("Golden Fox Hair (Female) Wig");
		SetDescription("Gather materials for Henry to craft a Golden Fox Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig10Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig10Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig10Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig10Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig10Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig10Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12551, 1));
	}
}

// Quest 2011: Wig Style F-152 (Lofty Snow Hair)
//-----------------------------------------------------------------------------

public class FemaleWig152Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2011);
		SetName("Lofty Snow Hair (Female) Wig");
		SetDescription("Gather materials for Henry to craft a Lofty Snow Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig11Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig11Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig11Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig11Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig11Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig11Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12552, 1));
	}
}

// Quest 2012: Wig Style F-153 (Faerie Dawn Hair)
//-----------------------------------------------------------------------------

public class FemaleWig153Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2012);
		SetName("Faerie Dawn Hair (Female) Wig");
		SetDescription("Gather materials for Henry to craft a Faerie Dawn Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig12Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig12Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig12Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig12Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig12Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig12Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12553, 1));
	}
}

// Quest 2013: Wig Style F-154 (Faerie Midnight Hair)
//-----------------------------------------------------------------------------

public class FemaleWig154Quest : QuestScript
{
	protected override void Load()
	{
		SetId("BeautyShop", 2013);
		SetName("Faerie Midnight Hair (Female) Wig");
		SetDescription("Gather materials for Henry to craft a Faerie Midnight Hair wig.");
		SetLocation("c_barber_dress");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Wig Crafter] Henry", "c_barber_dress");

		AddObjective("collectMaterials1", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig13Materials[0], WigMaterialConfig.Material1Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig13Materials[0], WigMaterialConfig.Material1Count));
		AddObjective("collectMaterials2", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig13Materials[1], WigMaterialConfig.Material2Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig13Materials[1], WigMaterialConfig.Material2Count));
		AddObjective("collectMaterials3", WigMaterialConfig.GetObjectiveDescription(WigMaterialConfig.FemaleWig13Materials[2], WigMaterialConfig.Material3Count),
			new CollectItemObjective(WigMaterialConfig.FemaleWig13Materials[2], WigMaterialConfig.Material3Count));

		AddReward(new ItemReward(12554, 1));
	}
}
