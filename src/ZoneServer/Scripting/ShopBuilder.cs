using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// A builder for creating shop data.
	/// </summary>
	public class ShopBuilder
	{
		private readonly ShopData _shopData;
		private int _productClassId = 100_001;

		/// <summary>
		/// Creates new instance for creating a shop with the given name.
		/// </summary>
		/// <param name="shopName"></param>
		public ShopBuilder(string shopName)
		{
			_shopData = new ShopData();
			_shopData.Name = shopName;
			_shopData.IsCustom = true;
		}

		/// <summary>
		/// Returns the built shop data.
		/// </summary>
		/// <returns></returns>
		public ShopData Build()
		{
			return _shopData;
		}

		/// <summary>
		/// Adds item to the shop.
		/// </summary>
		/// <param name="itemId">Id of the item for sale.</param>
		/// <param name="amount">Number of items that are sold at once.</param>
		/// <param name="price">The price per 1 unit.</param>
		public void AddItem(
			int itemId,
			int amount = 1,
			int price = 1,
			string requiredFactionId = null)
		{
			amount = Math.Max(1, amount);
			price = Math.Max(1, price);

			var productData = new ProductData();

			productData.Id = _productClassId++;
			productData.ItemId = itemId;
			productData.Amount = amount;
			productData.Price = price;
			productData.RequiredFactionId = requiredFactionId;

			_shopData.Products.Add(productData.Id, productData);
		}

		/// <summary>
		/// Adds item to the shop with a faction and reputation tier requirement.
		/// </summary>
		public void AddItem(int itemId, int amount, int price, string requiredFactionId, ReputationTier requiredReputationTier)
		{
			var tierValue = requiredReputationTier switch
			{
				ReputationTier.Hated => -1000,
				ReputationTier.Disliked => -750,
				ReputationTier.Neutral => -250,
				ReputationTier.Liked => 250,
				ReputationTier.Honored => 750,
				_ => -1000,
			};

			this.AddItem(itemId, amount, price, requiredFactionId, tierValue);
		}

		/// <summary>
		/// Adds item to the shop with a faction and minimum reputation value requirement.
		/// </summary>
		public void AddItem(int itemId, int amount, int price, string requiredFactionId, int requiredTierValue)
		{
			amount = Math.Max(1, amount);
			price = Math.Max(1, price);

			var productData = new ProductData();

			productData.Id = _productClassId++;
			productData.ItemId = itemId;
			productData.Amount = amount;
			productData.Price = price;
			productData.RequiredFactionId = requiredFactionId;
			productData.RequiredTierValue = requiredTierValue;

			_shopData.Products.Add(productData.Id, productData);
		}


		/// <summary>
		/// Adds item to the shop with specific item world IDs.
		/// </summary>
		/// <param name="itemId">Id of the item for sale.</param>
		/// <param name="amount">Number of items that are sold at once.</param>
		/// <param name="price">The price per 1 unit.</param>
		/// <param name="worldIds">World IDs of the specific item instances being sold.</param>
		public void AddSellItem(
			int itemId,
			int amount,
			int price,
			long[] worldIds,
			string requiredFactionId = null)
		{
			amount = Math.Max(1, amount);
			price = Math.Max(1, price);

			var productData = new ProductData();

			productData.Id = _productClassId++;
			productData.ItemId = itemId;
			productData.Amount = amount;
			productData.RequiredAmount = amount;
			productData.Price = price;
			productData.RequiredFactionId = requiredFactionId;

			if (worldIds != null)
				productData.ItemWorldIds.AddRange(worldIds);

			_shopData.Products.Add(productData.Id, productData);
		}

		/// <summary>
		/// Handle visitor BUYING items FROM a sell shop.
		/// Visitor gives silver, receives items.
		/// Shop owner gives items, receives silver.
		/// </summary>
		public static void HandleBuyFromSellShop(IZoneConnection conn, Character character, Character shopOwner, ShopData shop, int index, long itemId, int itemAmount)
		{
			var product = shop.GetProduct(index);
			if (product == null)
			{
				Log.Warning("HandleBuyFromSellShop: Invalid product index {0}", index);
				return;
			}

			if (product.ItemId != (int)itemId)
			{
				Log.Warning("HandleBuyFromSellShop: Item ID mismatch. Expected {0}, got {1}", product.ItemId, itemId);
				return;
			}

			lock (product)
			{
				if (itemAmount <= 0)
				{
					Log.Warning("HandleBuyFromSellShop: Invalid item amount {0}", itemAmount);
					return;
				}

				if (product.RequiredAmount < itemAmount)
				{
					character.SystemMessage("NotEnoughStock");
					return;
				}

				if (product.ItemWorldIds.Count == 0)
				{
					character.SystemMessage("NotEnoughStock");
					product.RequiredAmount = 0;
					return;
				}

				var totalCost = product.Price * itemAmount;

				var buyerSilver = character.Inventory.CountItem(ItemId.Silver);
				if (buyerSilver < totalCost)
				{
					character.SystemMessage("NotEnoughSilver");
					return;
				}

				var itemsToRemove = new Dictionary<long, int>();
				var amountToVerify = itemAmount;
				var validWorldIds = new List<long>();

				foreach (var worldId in product.ItemWorldIds)
				{
					if (amountToVerify <= 0)
					{
						validWorldIds.Add(worldId);
						continue;
					}

					if (shopOwner.Inventory.TryGetItem(worldId, out var item))
					{
						var takeAmount = Math.Min(item.Amount, amountToVerify);
						itemsToRemove[worldId] = takeAmount;
						amountToVerify -= takeAmount;

						if (takeAmount < item.Amount)
							validWorldIds.Add(worldId);
					}
				}

				if (amountToVerify > 0)
				{
					character.SystemMessage("SellerOutOfStock");
					product.RequiredAmount = itemAmount - amountToVerify;
					product.ItemWorldIds.Clear();
					product.ItemWorldIds.AddRange(validWorldIds);
					foreach (var kvp in itemsToRemove)
						if (!validWorldIds.Contains(kvp.Key))
							product.ItemWorldIds.Add(kvp.Key);
					Send.ZC_AUTOSELLER_LIST(shopOwner);
					return;
				}

				if (character.RemoveItem(ItemId.Silver, totalCost) == totalCost)
				{
					var removeSuccess = true;
					var totalRemoved = 0;
					var itemsToTransfer = new List<Item>();

					foreach (var kvp in itemsToRemove)
					{
						var worldId = kvp.Key;
						var amountToRemove = kvp.Value;

						// Get item and create copy BEFORE removing
						if (shopOwner.Inventory.TryGetItem(worldId, out var item))
						{
							var newItem = new Item(item, amountToRemove);

							if (shopOwner.Inventory.Remove(worldId, amountToRemove, InventoryItemRemoveMsg.Sold) != InventoryResult.Success)
							{
								removeSuccess = false;
								break;
							}
							totalRemoved += amountToRemove;
							itemsToTransfer.Add(newItem);
						}
						else
						{
							removeSuccess = false;
							break;
						}
					}

					if (removeSuccess && totalRemoved == itemAmount)
					{
						product.RequiredAmount -= itemAmount;
						product.ItemWorldIds.Clear();
						product.ItemWorldIds.AddRange(validWorldIds);

						// Transfer items with properties preserved
						foreach (var transferItem in itemsToTransfer)
							character.Inventory.Add(transferItem, InventoryAddType.Buy);

						shopOwner.AddItem(ItemId.Silver, totalCost);

						Log.Debug("HandleBuyFromSellShop: {0} bought {1}x {2} from {3} for {4} silver",
							character.Name, itemAmount, product.ItemId, shopOwner.Name, totalCost);

						if (product.RequiredAmount <= 0)
						{
							shop.Products.Remove(index);
							Log.Debug("HandleBuyFromSellShop: Product {0} sold out, removed from shop", product.ItemId);
						}

						Send.ZC_AUTOSELLER_LIST(shopOwner);
					}
					else
					{
						character.AddItem(ItemId.Silver, totalCost);
						character.SystemMessage("TransactionFailed");
					}
				}
				else
				{
					character.SystemMessage("TransactionFailed");
				}
			}
		}

		/// <summary>
		/// Handle visitor SELLING items TO a buy shop.
		/// Visitor gives items, receives silver.
		/// Shop owner gives silver, receives items.
		/// </summary>
		public static void HandleSellToBuyShop(IZoneConnection conn, Character character, Character shopOwner, ShopData shop, int index, long itemId, int itemAmount)
		{
			var product = shop.GetProduct(index);

			if (character.Inventory.TryGetItem(itemId, out var item) && product.RequiredAmount >= itemAmount)
			{
				var sellerSilver = shopOwner.Inventory.CountItem(ItemId.Silver);
				var totalCost = product.Price * itemAmount;

				if (sellerSilver >= totalCost && character.Inventory.Remove(itemId, itemAmount) == InventoryResult.Success
						&& shopOwner.RemoveItem(ItemId.Silver, totalCost) == totalCost)
				{
					product.RequiredAmount -= itemAmount;

					if (product.RequiredAmount <= 0)
					{
						shop.Products.Remove(index);
					}

					character.AddItem(ItemId.Silver, totalCost);
					shopOwner.AddItem(item.Id, itemAmount);
					Send.ZC_AUTOSELLER_LIST(shopOwner);
				}
				else
				{
					character.SystemMessage("FarFromFoodTable");
				}
			}
		}

		/// <summary>
		/// Closes a shop if it's empty and notifies all parties.
		/// </summary>
		public static void CloseShopIfEmpty(IZoneConnection conn, Character shopOwner, ShopData shop)
		{
			if (shop.Products.Count == 0)
			{
				shop.IsClosed = true;
				shopOwner.Connection.ShopCreated = null;

				Send.ZC_AUTOSELLER_TITLE(shopOwner);
				Send.ZC_NORMAL.ShopAnimation(shopOwner.Connection, shopOwner, "Squire_Repair", 1, 0);

				shopOwner.SystemMessage("ShopClosedAllItemsSold");
				Send.ZC_EXEC_CLIENT_SCP(conn, "ui.CloseFrame('personal_shop_target')");

				Log.Debug("CloseShopIfEmpty: Shop empty, closing shop for {0}", shopOwner.Name);
			}
			else
			{
				Send.ZC_AUTOSELLER_LIST(shopOwner);
				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
			}
		}
	}
}
