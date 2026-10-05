//--- Melia Script ----------------------------------------------------------
// Talt Utility Shop
//--- Description -----------------------------------------------------------
// NPC that exchanges Talt for materials, recipes and utility items.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class TaltUtilityShop : GeneralScript
	{
		private const int TaltItemId = 645268;
		private static readonly ExchangeEntry[] ExchangeItems =
		{
			new ExchangeEntry("Premium_indunReset", "Instanced Dungeon Reset Voucher", 490030, 1, 10),
			new ExchangeEntry("161215Event_Seed", "Miracle Seeds", 641926, 1, 10),
			new ExchangeEntry("Event_Goddess_Statue_DLC", "Goddess Sculpture", 641945, 1, 10),
			new ExchangeEntry("misc_ore15", "Practonium", 649014, 1, 250),
			new ExchangeEntry("R_TOP04_133", "Recipe - Laitas Robe", 948022, 1, 250),
			new ExchangeEntry("R_TOP04_134", "Recipe - Fietas Leather Armor", 948023, 1, 250),
			new ExchangeEntry("R_TOP04_135", "Recipe - Ausura Plate Armor", 948024, 1, 250),
			new ExchangeEntry("R_LEG04_133", "Recipe - Laitas Pants", 948025, 1, 250),
			new ExchangeEntry("R_LEG04_134", "Recipe - Fietas Leather Pants", 948026, 1, 250),
			new ExchangeEntry("R_LEG04_135", "Recipe - Ausura Plate Pants", 948027, 1, 250),
			new ExchangeEntry("R_FOOT04_136", "Recipe - Laitas Boots", 948028, 1, 250),
			new ExchangeEntry("R_FOOT04_137", "Recipe - Fietas Leather Boots", 948029, 1, 250),
			new ExchangeEntry("R_FOOT04_138", "Recipe - Ausura Greaves", 948030, 1, 250),
			new ExchangeEntry("R_HAND04_134", "Recipe - Laitas Gloves", 948031, 1, 250),
			new ExchangeEntry("R_HAND04_135", "Recipe - Fietas Leather Gloves", 948032, 1, 250),
			new ExchangeEntry("R_HAND04_136", "Recipe - Ausura Gauntlets", 948033, 1, 250),
			new ExchangeEntry("NECK02_130", "Fyrmes Necklace", 582130, 1, 20),
			new ExchangeEntry("NECK02_131", "Predji Necklace", 582131, 1, 20),
			new ExchangeEntry("BRC02_124", "Fyrmes Necklace", 602124, 1, 10),
			new ExchangeEntry("BRC02_125", "Predji Necklace", 602125, 1, 10),
			new ExchangeEntry("R_TBW03_120", "Recipe - Aufgowle Bow", 924090, 1, 125),
			new ExchangeEntry("R_BOW03_202", "Recipe - Silver Hawk", 925085, 1, 125),
			new ExchangeEntry("R_MAC03_204", "Recipe - Vienarazis Mace", 926090, 1, 125),
			new ExchangeEntry("R_PST03_101", "Recipe - Double Stack", 930011, 1, 125),
			new ExchangeEntry("R_CAN03_101", "Recipe - Lionhead Cannon", 947011, 1, 125),
			new ExchangeEntry("R_MUS03_104", "Recipe - Dragoon Piper", 931007, 1, 125),
			new ExchangeEntry("R_TMAC03_106", "Recipe - Vienarazis Two-handed Mace", 942030, 1, 125),
			new ExchangeEntry("R_SWD03_120", "Recipe - Pierene Sword", 920096, 1, 125),
			new ExchangeEntry("R_TSW03_120", "Recipe - Gale Slasher", 921085, 1, 125),
			new ExchangeEntry("R_STF03_120", "Recipe - Windia Rod", 922088, 1, 125),
			new ExchangeEntry("R_SHD03_110", "Recipe - Lionhead Shield", 941057, 1, 125),
			new ExchangeEntry("R_SPR03_115", "Recipe - Pygry Spear", 927070, 1, 125),
			new ExchangeEntry("R_TSP03_115", "Recipe - Sacmet", 928060, 1, 125),
			new ExchangeEntry("R_DAG03_105", "Recipe - Lionhead Dagger", 942014, 1, 125),
			new ExchangeEntry("R_TSF03_120", "Recipe - Vienarazis Staff", 923089, 1, 125),
			new ExchangeEntry("R_RAP03_305", "Recipe - Elga Rapier", 929022, 1, 125),
			new ExchangeEntry("R_TBW04_109", "Recipe - Astra Bow", 924091, 1, 250),
			new ExchangeEntry("R_BOW04_109", "Recipe - Regard Horn Crossbow", 925086, 1, 250),
			new ExchangeEntry("R_TMAC04_101", "Recipe - Skull Breaker", 942031, 1, 250),
			new ExchangeEntry("R_MAC04_111", "Recipe - Skull Smasher", 926091, 1, 250),
			new ExchangeEntry("R_TSF04_109", "Recipe - Regard Horn Staff", 923090, 1, 250),
			new ExchangeEntry("R_PST04_104", "Recipe - Aspana Revolver", 930012, 1, 250),
			new ExchangeEntry("R_CAN04_103", "Recipe - Emengard Cannon", 947012, 1, 250),
			new ExchangeEntry("R_MUS04_103", "Recipe - Emengard Musket", 931008, 1, 250),
			new ExchangeEntry("R_TSW04_109", "Recipe - Sarkmis", 942018, 1, 250),
			new ExchangeEntry("R_SWD04_109", "Recipe - Abdochar", 920097, 1, 250),
			new ExchangeEntry("R_STF04_110", "Recipe - Heart of Glory", 922089, 1, 250),
			new ExchangeEntry("R_SHD04_105", "Recipe - Emengard Shield", 941058, 1, 250),
			new ExchangeEntry("R_SPR04_110", "Recipe - Wingshard Spear", 927071, 1, 250),
			new ExchangeEntry("R_TSP04_111", "Recipe - Regard Horn Pike", 928061, 1, 250),
			new ExchangeEntry("R_DAG04_104", "Recipe - Emengard Dagger", 942015, 1, 250),
			new ExchangeEntry("R_RAP04_106", "Recipe - Black Horn", 929023, 1, 250),

		};

		protected override void Load()
		{
			PropertyShops.Create("TaltUtilityShop", "talt_utility_shop", TaltItemId, shop =>
			{
				foreach (var entry in ExchangeItems)
					shop.AddItem(entry.ClassName, entry.ItemId, entry.Amount, entry.TaltCost);
			});

			Dialog.RegisterPropertyShopForMap("c_Klaipe", "TaltUtilityShop", "GET_PVP_POINT");

			AddNpc(57225, L("[Talt Utility Shop] Thiago"), "Thiago", "c_Klaipe", -720, 610, 0, async dialog =>
			{
				dialog.SetTitle(L("Talt Utility Shop"));
				await dialog.Msg(L("Exchange your Talt for materials, recipes and utility items."));
				dialog.OpenPropertyShop("TaltUtilityShop", string.Empty, "uphill_defense_shoppoint", "MercenaryWingShop");
			});
		}

		private sealed class ExchangeEntry
		{
			public string ClassName { get; }
			public string Name { get; }
			public int ItemId { get; }
			public int Amount { get; }
			public int TaltCost { get; }

			public ExchangeEntry(string className, string name, int itemId, int amount, int taltCost)
			{
				ClassName = className;
				Name = name;
				ItemId = itemId;
				Amount = amount;
				TaltCost = taltCost;
			}
		}
	}
}
