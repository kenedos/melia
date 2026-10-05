//--- Melia Script ----------------------------------------------------------
// World Boss Exchange Utility Shop
//--- Description -----------------------------------------------------------
// NPC that exchanges Golden Coins for utility items.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class WorldBossExchangeUtilityShop : GeneralScript
	{
		private const int GoldenCoinItemId = 902064;

		private static readonly ExchangeEntry[] ExchangeItems =
		{
			new ExchangeEntry("misc_ore15", "Practonium", 649014, 1, 15),
			new ExchangeEntry("Old_Socket_Gold_Team", "Ancient Golden Socket", 643032, 1, 15),
		};

		protected override void Load()
		{
			PropertyShops.Create("WorldBossExchangeUtilityShop", "world_boss_golden_coin_utility", GoldenCoinItemId, shop =>
			{
				foreach (var entry in ExchangeItems)
				{
					shop.AddItem(entry.ClassName, entry.ItemId, entry.Amount, entry.CoinCost);
				}
			});

			Dialog.RegisterPropertyShopForMap("c_Klaipe", "WorldBossExchangeUtilityShop", "GET_PVP_POINT");

			AddNpc(151072, L("[World Boss Exchange Utility Shop] Felipe"), "Felipe", "c_Klaipe", -650, 690, 0, async dialog =>
			{
				dialog.SetTitle(L("World Boss Exchange Utility Shop"));

				await dialog.Msg(L("Exchange your Golden Coins for useful materials and utility items."));

				dialog.OpenPropertyShop("WorldBossExchangeUtilityShop", string.Empty, "uphill_defense_shoppoint", "MercenaryWingShop");
			});
		}

		private sealed class ExchangeEntry
		{
			public string ClassName { get; }
			public string Name { get; }
			public int ItemId { get; }
			public int Amount { get; }
			public int CoinCost { get; }

			public ExchangeEntry(string className, string name, int itemId, int amount, int coinCost)
			{
				ClassName = className;
				Name = name;
				ItemId = itemId;
				Amount = amount;
				CoinCost = coinCost;
			}
		}
	}
}
