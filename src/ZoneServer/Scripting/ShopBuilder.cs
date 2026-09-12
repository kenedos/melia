using System;
using System.Collections.Generic;
using System.Text;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Skills.Helpers;
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
		/// <summary>
		/// How far away a character may ask for a shop to be opened from.
		/// </summary>
		/// <remarks>
		/// The client walks the character up to a shop it was told to open
		/// and asks once it's happy with the distance, which leaves this as
		/// a cap on requests rather than a distance anyone stands at.
		/// </remarks>
		public const int ShopRequestRange = 80;

		/// <summary>
		/// How far a character may get from a shop's owner before the shop
		/// is taken off their screen.
		/// </summary>
		public const int ShopCloseRange = 80;

		private const string OwnerFundsWarnedVar = "Melia.Oblation.OwnerFundsWarned";

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
			if (!character.Inventory.TryGetItem(itemId, out var item))
				return;

			if (shop.Type == PersonalShopType.Oblation)
			{
				HandleOfferToOblationBox(character, shopOwner, shop, item, itemAmount);
				return;
			}

			// Find the matching product by item class ID, since the
			// client may not send a reliable product index for buyshops.
			var matchedKey = -1;
			ProductData product = null;
			foreach (var kvp in shop.Products)
			{
				if (kvp.Value.ItemId == item.Id)
				{
					matchedKey = kvp.Key;
					product = kvp.Value;
					break;
				}
			}

			if (product == null || product.RequiredAmount < itemAmount)
				return;

			var sellerSilver = shopOwner.Inventory.CountItem(ItemId.Silver);
			var totalCost = product.Price * itemAmount;

			if (sellerSilver >= totalCost && character.Inventory.Remove(itemId, itemAmount) == InventoryResult.Success
					&& shopOwner.RemoveItem(ItemId.Silver, totalCost) == totalCost)
			{
				product.RequiredAmount -= itemAmount;

				if (product.RequiredAmount <= 0)
				{
					shop.Products.Remove(matchedKey);
				}

				character.AddItem(ItemId.Silver, totalCost);
				shopOwner.AddItem(item.Id, itemAmount);
			}
			else
			{
				character.SystemMessage("FarFromFoodTable");
			}
		}

		/// <summary>
		/// Handle visitor offering items to a Pardoner's Oblation box.
		/// The box takes any item the game buys back, at a rate the skill
		/// sets, so it has no product list to match against.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="shopOwner"></param>
		/// <param name="shop"></param>
		/// <param name="item"></param>
		/// <param name="itemAmount"></param>
		private static void HandleOfferToOblationBox(Character character, Character shopOwner, ShopData shop, Item item, int itemAmount)
		{
			if (itemAmount < 1 || item.Amount < itemAmount)
				return;

			// Offering to your own box would hand the owner the church's
			// premium without anyone having been paid for the item
			if (character == shopOwner)
				return;

			var pricePerUnit = PardonerSkillHelper.GetOblationPrice(shopOwner, item);
			if (pricePerUnit <= 0)
			{
				character.SystemMessage("Auto_SangJeom_PanMae_BulKaNeung");
				return;
			}

			var box = shopOwner.OblationBox;

			if (box.GetItemCount() >= PardonerSkillHelper.GetOblationCapacity(shopOwner))
			{
				character.SystemMessage("ExceedItemGetLimit");
				return;
			}

			var totalCost = pricePerUnit * itemAmount;

			if (shopOwner.Inventory.CountItem(ItemId.Silver) < totalCost)
			{
				// The box owner is the one paying, and one offer failing
				// this way means every later one in the same batch will too.
				if (!character.Variables.Temp.GetBool(OwnerFundsWarnedVar, false))
				{
					character.Variables.Temp.SetBool(OwnerFundsWarnedVar, true);
					character.ServerMessage(Localization.Get("The Offering Box owner doesn't have enough silver to buy that."));
				}

				return;
			}

			var offeredItem = new Item(item, itemAmount);

			if (character.Inventory.Remove(item, itemAmount, InventoryItemRemoveMsg.Given) != InventoryResult.Success)
				return;

			if (box.Offer(offeredItem, pricePerUnit) != StorageResult.Success)
			{
				character.Inventory.Add(offeredItem, InventoryAddType.New);
				return;
			}

			if (shopOwner.RemoveItem(ItemId.Silver, totalCost) != totalCost)
			{
				box.Consume(offeredItem.ObjectId);
				character.Inventory.Add(offeredItem, InventoryAddType.New);
				return;
			}

			character.AddItem(ItemId.Silver, totalCost);
		}

		/// <summary>
		/// Closes the given character's personal shop and takes it off
		/// every screen that has it open.
		/// </summary>
		/// <remarks>
		/// An empty shop title is what tells a browsing client to close the
		/// window it has open on this owner, so this has to run while the
		/// owner is still on the map and there is a map to broadcast it to.
		/// </remarks>
		/// <param name="shopOwner"></param>
		public static void ClosePersonalShop(Character shopOwner)
		{
			var conn = shopOwner.Connection;
			var shop = conn?.ShopCreated;

			if (shop == null)
				return;

			shop.IsClosed = true;

			Send.ZC_AUTOSELLER_LIST(conn, shopOwner);

			// Everyone browsing it gets the closed list while the shop is
			// still there to send, and forgets it afterwards, so a handle
			// that gets reused isn't taken for a shop they had open.
			foreach (var viewer in GetShopViewers(shopOwner))
			{
				Send.ZC_AUTOSELLER_LIST(viewer.Connection, shopOwner);

				viewer.Connection.ActiveShop = null;
				viewer.Connection.ActiveShopOwnerHandle = 0;
			}

			Send.ZC_AUTOSELLER_TITLE(shopOwner);
			Send.ZC_NORMAL.ShopAnimation(shopOwner, shop.ShopAnimation, 1, 0);

			conn.ShopCreated = null;
		}

		/// <summary>
		/// Walks the given character to the owner of the shop they asked to
		/// open as necessary, and opens it for them once they're there.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="shopOwner"></param>
		/// <param name="optionSelected"></param>
		public static void RequestShopOpen(IZoneConnection conn, Character character, Character shopOwner, int optionSelected)
		{
			// Nothing but a client that was made to send it asks to shop
			// from farther away than one can be clicked from.
			if (!character.Position.InRange2D(shopOwner.Position, ShopRequestRange))
			{
				character.SystemMessage("FarFromFoodTable");
				return;
			}

			OpenShopView(conn, character, shopOwner, optionSelected);
		}

		/// <summary>
		/// Opens the given owner's shop on the given character's client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="shopOwner"></param>
		/// <param name="optionSelected"></param>
		private static void OpenShopView(IZoneConnection conn, Character character, Character shopOwner, int optionSelected)
		{
			if (shopOwner.Map != character.Map)
				return;

			if (shopOwner.Connection.ShopCreated == null)
			{
				Log.Warning("OpenShopView: {0} has no shop open.", shopOwner.Name);
				return;
			}

			var alreadyOpen = conn.ActiveShopOwnerHandle == shopOwner.Handle;

			var shop = conn.ActiveShop = shopOwner.Connection.ShopCreated;
			conn.ActiveShopOwnerHandle = shopOwner.Handle;

			// A Spell Shop is bought from through this same request, which
			// carries the buff's list index. Merely opening the shop sends
			// -1 there, and leaves the amount beside it uninitialized.
			if (shop.Type == PersonalShopType.SpellShop)
			{
				if (alreadyOpen && optionSelected >= 0)
					PardonerSkillHelper.SellSpellShopBuff(character, shopOwner, shop, optionSelected);

				// The owner's materials are what's for sale, and they can
				// spend them anywhere between two visitors.
				PardonerSkillHelper.RefreshSpellShopStock(shopOwner, shop);

				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
				Send.ZC_AUTOSELLER_LIST(shopOwner.Connection, shopOwner);
				return;
			}

			// ============================================================
			// BUYSHOP (IsCustom=false) - Visitor wants to SELL items TO shop owner
			// Uses ZC_AUTOSELLER_LIST packet only (Laima3 style)
			// ============================================================
			if (!shop.IsCustom)
			{
				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
				return;
			}

			// ============================================================
			// SELLSHOP (IsCustom=true) - Visitor wants to BUY items FROM shop owner
			// Uses MeliaCustomShop dialog system
			// ============================================================

			// Owner clicking their own sellshop - show management UI
			if (character.Handle == shopOwner.Handle)
			{
				Send.ZC_EXEC_CLIENT_SCP(conn, string.Format(
					"MY_AUTOSELL_LIST('PersonalShop', {0})",
					(int)PersonalShopType.PersonalSell));
				return;
			}

			// Visitor opening a sellshop - send custom shop data via Melia.Comm and open dialog
			// The items themselves go over as a preview list, so their
			// sockets and gems reach the client's tooltip.
			var shopItems = new List<Item>();
			foreach (var productData in shop.Products.Values)
			{
				if (productData.ItemWorldIds.Count == 0)
					continue;

				if (shopOwner.Inventory.TryGetItem(productData.ItemWorldIds[0], out var shopItem))
					shopItems.Add(shopItem);
			}

			ItemPreview.Show(character, shopItems);

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.BeginRecv('CustomShop')");

			var sb = new StringBuilder();
			foreach (var productData in shop.Products.Values)
			{
				// Get item properties for tooltip display
				var propsStr = "nil";
				var worldId = 0L;
				if (productData.ItemWorldIds.Count > 0)
				{
					worldId = productData.ItemWorldIds[0];
					if (shopOwner.Inventory.TryGetItem(worldId, out var item))
					{
						try
						{
							propsStr = item.SerializePropertiesToLua();
						}
						catch (Exception ex)
						{
							Log.Warning("Failed to serialize item properties for shop: {0}", ex.Message);
							propsStr = "nil";
						}
					}
				}

				// Format: { productId, itemId, amount, price, properties, worldId }
				var entry = string.Format("{{ {0},{1},{2},{3},{4},'{5}' }},", productData.Id, productData.ItemId, productData.Amount, productData.Price, propsStr, worldId);

				// Flushed before the entry rather than after it: a socketed
				// item's properties alone can outgrow what the client takes.
				if (sb.Length > 0 && sb.Length + entry.Length > ClientScript.ScriptMaxLength - 64)
				{
					Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
					sb.Clear();
				}

				sb.Append(entry);
			}

			if (sb.Length > 0)
			{
				Send.ZC_EXEC_CLIENT_SCP(conn, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
				sb.Clear();
			}

			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.ExecData('CustomShop', M_SET_CUSTOM_SHOP)");
			Send.ZC_EXEC_CLIENT_SCP(conn, "Melia.Comm.EndRecv('CustomShop')");
			Send.ZC_DIALOG_TRADE(conn, "MeliaCustomShop");
		}


		/// <summary>
		/// Closes the shop the given character is browsing if they walked
		/// out of reach of its owner or the shop is no longer there.
		/// </summary>
		/// <param name="character"></param>
		public static void UpdateShopDistance(Character character)
		{
			var conn = character.Connection;
			var shop = conn?.ActiveShop;

			if (shop == null || conn.ActiveShopOwnerHandle == 0)
				return;

			if (character.Map != null && character.Map.TryGetCharacter(conn.ActiveShopOwnerHandle, out var shopOwner))
			{
				if (shopOwner.Connection?.ShopCreated == shop && !shop.IsClosed && shopOwner.Position.InRange2D(character.Position, ShopCloseRange))
					return;
			}

			CloseShopView(character);
		}

		/// <summary>
		/// Takes the shop the given character is browsing off their screen,
		/// leaving it open for everyone else.
		/// </summary>
		/// <param name="viewer"></param>
		public static void CloseShopView(Character viewer)
		{
			var conn = viewer.Connection;
			var shop = conn?.ActiveShop;

			if (shop == null)
				return;

			var shopOwnerHandle = conn.ActiveShopOwnerHandle;

			conn.ActiveShop = null;
			conn.ActiveShopOwnerHandle = 0;

			Send.ZC_AUTOSELLER_LIST_CLOSED(conn, shopOwnerHandle, shop);

			switch (shop.Type)
			{
				case PersonalShopType.SpellShop:
					Send.ZC_EXEC_CLIENT_SCP(conn, "ui.CloseFrame('buffseller_target')");
					break;

				case PersonalShopType.Oblation:
					Send.ZC_EXEC_CLIENT_SCP(conn, "ui.CloseFrame('oblation_sell')");
					break;

				default:
					Send.ZC_EXEC_CLIENT_SCP(conn, "ui.CloseFrame('personal_shop_target')");

					if (shop.IsCustom)
					{
						Send.ZC_DIALOG_CLOSE(conn);
						Send.ZC_LEAVE_TRIGGER(conn);
					}
					break;
			}

			Send.ZC_ENABLE_CONTROL(conn, "AUTOSELLER", true);
			Send.ZC_LOCK_KEY(viewer, "AUTOSELLER", false);
		}

		/// <summary>
		/// Returns every character on the owner's map with the owner's shop
		/// open.
		/// </summary>
		/// <param name="shopOwner"></param>
		/// <returns></returns>
		private static Character[] GetShopViewers(Character shopOwner)
		{
			if (shopOwner.Map == null)
				return Array.Empty<Character>();

			return shopOwner.Map.GetCharacters(a => a != shopOwner && a.Connection != null && a.Connection.ActiveShopOwnerHandle == shopOwner.Handle);
		}

		/// <summary>
		/// Closes a shop if it's empty and notifies all parties.
		/// </summary>
		public static void CloseShopIfEmpty(IZoneConnection conn, Character shopOwner, ShopData shop)
		{
			// An Oblation box starts empty and stays open until its owner
			// closes it, so it never runs out of anything to sell. This runs
			// once per purchase packet, after every item in it was offered.
			if (shop.Type == PersonalShopType.Oblation)
			{
				conn.SelectedCharacter?.Variables.Temp.SetBool(OwnerFundsWarnedVar, false);

				Send.ZC_AUTOSELLER_LIST(shopOwner.Connection, shopOwner);
				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);

				// Refreshing the box also reaches everyone browsing it,
				// the buyer included.
				PardonerSkillHelper.RefreshOblationBox(shopOwner);

				return;
			}

			if (shop.Products.Count == 0)
			{
				ClosePersonalShop(shopOwner);

				shopOwner.ServerMessage("All items purchased. Shop closed.");
				Send.ZC_EXEC_CLIENT_SCP(conn, "ui.CloseFrame('personal_shop_target')");

				Log.Debug("CloseShopIfEmpty: Shop empty, closing shop for {0}", shopOwner.Name);
			}
			else
			{
				Send.ZC_AUTOSELLER_LIST(shopOwner.Connection, shopOwner);
				Send.ZC_AUTOSELLER_LIST(conn, shopOwner);
			}
		}
	}
}
