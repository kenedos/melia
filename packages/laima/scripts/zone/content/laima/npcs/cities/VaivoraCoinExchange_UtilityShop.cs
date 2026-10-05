//--- Melia Script ----------------------------------------------------------
// Wings of Vaivora Coin Utility Exchange
//--- Description -----------------------------------------------------------
// NPC that exchanges Wings of Vaivora Coins for utility items.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class VaivoraCoinExchangeUtilityShopScript : GeneralScript
{
	private const int WingsOfVaivoraCoinItemId = 647016;

	private static readonly ExchangeEntry[] ExchangeItems =
	{
		new ExchangeEntry("Premium_indunReset", "Instanced Dungeon Reset Voucher", 490030, 1, 2),
		new ExchangeEntry( "161215Event_Seed", "Miracle Seeds", 641926, 1, 5),
		new ExchangeEntry( "Event_Goddess_Statue_DLC", "Goddess Sculpture", 641945, 1, 5),
		//new ExchangeEntry( "Premium_AddSkillPoint", "Skill Point Potion", 494155, 1, 20),
		new ExchangeEntry( "misc_ore15", "Practonium", 649014, 1, 175),

		new ExchangeEntry( "R_NECK03_121", "Recipe - Manosierdi Necklace", 943075, 1, 100),
		new ExchangeEntry( "R_NECK03_120", "Recipe - Atikha Necklace", 943074, 1, 100),
		new ExchangeEntry( "R_NECK03_119", "Recipe - Svijes Necklace", 943073, 1, 100),
		new ExchangeEntry( "R_NECK03_118", "Recipe - Mejstra Necklace", 943072, 1, 100),

		new ExchangeEntry( "R_BRC03_118", "Recipe - Mejstra Bracelet", 943076, 1, 50),
		new ExchangeEntry( "R_BRC03_119", "Recipe - Svijes Bracelet", 943077, 1, 50),
		new ExchangeEntry( "R_BRC03_121", "Recipe - Atikha Bracelet", 943078, 1, 50),
		new ExchangeEntry( "R_BRC03_122", "Recipe - Manosierdi Bracelet", 943079, 1, 50),
	};

	protected override void Load()
	{
		PropertyShops.Create( "VaivoraCoinUtilityShop", "vaivora_coin_utility", WingsOfVaivoraCoinItemId, shop =>
			{
				foreach (var entry in ExchangeItems)
				{
					shop.AddItem( entry.ClassName, entry.ItemId, entry.Amount, entry.CoinCost);
				}
			});

		Dialog.RegisterPropertyShopForMap( "c_Klaipe", "VaivoraCoinUtilityShop", "GET_PVP_POINT");

		AddNpc( 150257, L("[Vaivora Coin Utility Exchange] Artur"), "Artur", "c_Klaipe", -540, 895, 0, async dialog =>
			{
				dialog.SetTitle(L("Vaivora Coin Utility Exchange"));

				await dialog.Msg(L("Exchange your Wings of Vaivora Coins for useful items."));

				dialog.OpenPropertyShop( "VaivoraCoinUtilityShop", string.Empty, "uphill_defense_shoppoint", "MercenaryWingShop");
			});
	}

	private sealed class ExchangeEntry
	{
		public string ClassName { get; }

		public string Name { get; }

		public int ItemId { get; }

		public int Amount { get; }

		public int CoinCost { get; }

		public ExchangeEntry(
			string className,
			string name,
			int itemId,
			int amount,
			int coinCost)
		{
			ClassName = className;
			Name = name;
			ItemId = itemId;
			Amount = amount;
			CoinCost = coinCost;
		}
	}
}
