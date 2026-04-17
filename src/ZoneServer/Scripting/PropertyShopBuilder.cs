using System;
using System.Collections.Generic;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Server-side definition of a property/point shop (e.g. Mercenary Badge Shop).
	///
	/// Property shops use a per-shop currency item (e.g. Mercenary Badge) instead
	/// of silver. The client UI is the `propertyshop` frame. The server owns the
	/// item list and prices; the client XML in LaimaClient must expose a matching
	/// shop entry so the UI has something to render.
	/// </summary>
	public class PropertyShop
	{
		public string Name { get; }
		public string PointName { get; }

		/// <summary>
		/// Account property name that holds the player's point balance for this
		/// shop (e.g. "MISC_PVP_MINE2"). Server reads/writes this to charge
		/// purchases and report the balance to the client.
		/// </summary>
		public string CurrencyProperty { get; }
		public List<PropertyShopItem> Items { get; } = new();

		public PropertyShop(string name, string pointName, string currencyProperty)
		{
			this.Name = name;
			this.PointName = pointName;
			this.CurrencyProperty = currencyProperty;
		}

		public void AddItem(string className, int itemId, int amount, int price)
		{
			this.Items.Add(new PropertyShopItem(className, itemId, amount, price));
		}
	}

	public class PropertyShopItem
	{
		public string ClassName { get; }
		public int ItemId { get; }
		public int Amount { get; }
		public int Price { get; }

		public PropertyShopItem(string className, int itemId, int amount, int price)
		{
			this.ClassName = className;
			this.ItemId = itemId;
			this.Amount = amount;
			this.Price = price;
		}
	}

	/// <summary>
	/// Registry of all server-defined property shops, looked up by shop name
	/// in propertyshop packet handlers.
	/// </summary>
	public static class PropertyShops
	{
		private static readonly Dictionary<string, PropertyShop> _shops = new(StringComparer.OrdinalIgnoreCase);

		public static PropertyShop Create(string name, string pointName, string currencyProperty, Action<PropertyShop> configure)
		{
			var shop = new PropertyShop(name, pointName, currencyProperty);
			configure(shop);
			_shops[name] = shop;
			return shop;
		}

		/// <summary>
		/// Returns the shop whose <see cref="PropertyShop.PointName"/> matches
		/// the given name, or null.
		/// </summary>
		public static PropertyShop FindByPointName(string pointName)
		{
			foreach (var shop in _shops.Values)
			{
				if (string.Equals(shop.PointName, pointName, StringComparison.OrdinalIgnoreCase))
					return shop;
			}
			return null;
		}

		public static bool TryGet(string name, out PropertyShop shop)
			=> _shops.TryGetValue(name, out shop);
	}
}
