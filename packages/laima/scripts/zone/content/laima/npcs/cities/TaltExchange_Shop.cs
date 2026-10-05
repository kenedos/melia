//--- Melia Script ----------------------------------------------------------
// Talt Exchange Shop
//--- Description -----------------------------------------------------------
// NPC that exchanges Talt for contact lenses.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class TaltExchangeShop : GeneralScript
	{
		private const int TaltItemId = 645268;
		private static readonly ExchangeEntry[] ExchangeItems =
		{
			new ExchangeEntry("LENS01_001", "Yellow Contact Lenses", 18001, 1, 200),
			new ExchangeEntry("LENS01_002", "Violet Contact Lenses", 18002, 1, 200),
			new ExchangeEntry("LENS01_003", "Crimson Contact Lenses", 18003, 1, 200),
			new ExchangeEntry("LENS01_004", "Black Contact Lenses", 18004, 1, 200),
			new ExchangeEntry("LENS01_005", "Pink Heart Contact Lenses", 18005, 1, 200),
			new ExchangeEntry("LENS01_006", "Shiny Contact Lenses", 18006, 1, 200),
			new ExchangeEntry("LENS01_007", "Grey Contact Lenses", 18007, 1, 200),
			new ExchangeEntry("LENS01_008", "Orange Contact Lenses", 18008, 1, 200),
			new ExchangeEntry("LENS01_009", "Brown Contact Lenses", 18009, 1, 200),
			new ExchangeEntry("LENS01_011", "Twinkle Purple Lenses", 18011, 1, 200),
			new ExchangeEntry("LENS01_012", "Twinkle Forest Lenses", 18012, 1, 200),
			new ExchangeEntry("LENS01_013", "Amber Cross Lenses", 18013, 1, 200),
			new ExchangeEntry("LENS01_014", "Blue Drop Lenses", 18014, 1, 200),
			new ExchangeEntry("LENS01_015", "Scarlet Lens", 18015, 1, 200),
			new ExchangeEntry("LENS01_016", "Emerald Lens", 18016, 1, 200),
			new ExchangeEntry("LENS01_017", "Cat Eye Blue Lense", 18017, 1, 200),
			new ExchangeEntry("LENS01_018", "Cat Eye Brown Lense", 18018, 1, 200),
			new ExchangeEntry("LENS01_019", "Fenrir Lense", 18023, 1, 200),
			new ExchangeEntry("LENS01_010_KOR", "Blue Lense", 18024, 1, 200),
			new ExchangeEntry("LENS01_020", "Holy Pink Lense", 18025, 1, 200),
		};

		protected override void Load()
		{
			PropertyShops.Create("TaltExchangeShop", "talt_exchange_shop", TaltItemId, shop =>
			{
				foreach (var entry in ExchangeItems)
					shop.AddItem(entry.ClassName, entry.ItemId, entry.Amount, entry.TaltCost);
			});

			Dialog.RegisterPropertyShopForMap("c_Klaipe", "TaltExchangeShop", "GET_PVP_POINT");

			AddNpc(57224, L("[Talt Exchange Shop] Ana"), "Ana", "c_Klaipe", -680, 650, 0, async dialog =>
			{
				dialog.SetTitle(L("Talt Exchange Shop"));
				await dialog.Msg(L("Exchange your Talt for contact lenses."));
				dialog.OpenPropertyShop("TaltExchangeShop", string.Empty, "uphill_defense_shoppoint", "MercenaryWingShop");
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
