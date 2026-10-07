using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;

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

		/// <summary>
		/// Id of the inventory item spent instead of the account property,
		/// or 0.
		/// </summary>
		public int CurrencyItemId { get; }

		/// <summary>
		/// Name of the permanent account variable spent instead of the
		/// account property, or null.
		/// </summary>
		public string CurrencyVariable { get; }

		public List<PropertyShopItem> Items { get; } = new();

		public PropertyShop(string name, string pointName, string currencyProperty, int currencyItemId = 0, string currencyVariable = null)
		{
			this.Name = name;
			this.PointName = pointName;
			this.CurrencyProperty = currencyProperty;
			this.CurrencyItemId = currencyItemId;
			this.CurrencyVariable = currencyVariable;
		}

		/// <summary>
		/// Returns the character's balance of this shop's currency.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public int GetBalance(Character character)
		{
			if (this.CurrencyItemId != 0)
				return character.Inventory.CountItem(this.CurrencyItemId);

			var account = character.Connection.Account;

			if (this.CurrencyVariable != null)
				return account.Variables.Perm.GetInt(this.CurrencyVariable, 0);

			return (int)account.Properties.GetFloat(this.CurrencyProperty);
		}

		/// <summary>
		/// Spends the given amount of this shop's currency if the character
		/// has enough of it. Returns false if they don't.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public bool TrySpend(Character character, int amount)
		{
			if (amount < 0 || this.GetBalance(character) < amount)
				return false;

			if (this.CurrencyItemId != 0)
				return character.Inventory.Remove(this.CurrencyItemId, amount, InventoryItemRemoveMsg.Used) == amount;

			var account = character.Connection.Account;

			if (this.CurrencyVariable != null)
				account.Variables.Perm.SetInt(this.CurrencyVariable, this.GetBalance(character) - amount);
			else
				character.ModifyAccountProperty(this.CurrencyProperty, -amount);

			return true;
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
		/// <summary>
		/// Prefix of the temporary character variables that map a UI shop
		/// alias to the shop it serves.
		/// </summary>
		public const string AliasVariablePrefix = "Melia.PropertyShop.Alias.";

		private static readonly Dictionary<string, PropertyShop> _shops = new(StringComparer.OrdinalIgnoreCase);

		public static PropertyShop Create(string name, string pointName, string currencyProperty, Action<PropertyShop> configure)
			=> Register(new PropertyShop(name, pointName, currencyProperty), configure);

		/// <summary>
		/// Creates a shop that charges an inventory item as its currency.
		/// </summary>
		public static PropertyShop CreateWithItem(string name, string pointName, int currencyItemId, Action<PropertyShop> configure)
		{
			if (currencyItemId <= 0)
				throw new ArgumentOutOfRangeException(nameof(currencyItemId), "Currency item id must be greater than zero.");

			return Register(new PropertyShop(name, pointName, null, currencyItemId: currencyItemId), configure);
		}

		/// <summary>
		/// Creates a shop that charges a permanent account variable as its
		/// currency.
		/// </summary>
		public static PropertyShop CreateWithVariable(string name, string pointName, string currencyVariable, Action<PropertyShop> configure)
		{
			if (string.IsNullOrWhiteSpace(currencyVariable))
				throw new ArgumentException("Currency variable name required.", nameof(currencyVariable));

			return Register(new PropertyShop(name, pointName, null, currencyVariable: currencyVariable), configure);
		}

		private static PropertyShop Register(PropertyShop shop, Action<PropertyShop> configure)
		{
			configure(shop);
			_shops[shop.Name] = shop;
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

		/// <summary>
		/// Returns the shop the character opened under the given name,
		/// resolving a UI alias first.
		/// </summary>
		public static bool TryGet(Character character, string name, out PropertyShop shop)
		{
			var realName = character.Variables.Temp.GetString(AliasVariablePrefix + name, null);
			return _shops.TryGetValue(realName ?? name, out shop);
		}
	}
}
