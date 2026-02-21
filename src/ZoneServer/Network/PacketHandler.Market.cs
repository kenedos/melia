using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Network.Inter.Messages;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

namespace Melia.Zone.Network
{
	public partial class PacketHandler : PacketHandler<IZoneConnection>
	{
		[PacketHandler(Op.CZ_REQ_MARKET_REGISTER)]
		public void CZ_REQ_MARKET_REGISTER(IZoneConnection conn, Packet packet)
		{
			var itemWorldId = packet.GetLong();
			var itemAmount = packet.GetInt();
			var itemId = packet.GetInt();
			var itemPrice = packet.GetLong();
			var s1 = packet.GetShort();
			var s2 = packet.GetShort();
			var i1 = packet.GetInt();
			var saleLength = packet.GetInt();

			var character = conn.SelectedCharacter;
			var fee = 0;
			var item = character.Inventory.GetItem(itemWorldId);

			if (item == null)
			{
				Log.Warning("CZ_REQ_MARKET_REGISTER: User '{0}' tried to sell a non-existent item.", conn.Account.Name);
				return;
			}

			// TODO: Figure out why sale length is bugged in client.
			if (saleLength == 0)
				saleLength = 1;

			var commissionFee = saleLength switch
			{
				1 => 0.005f,
				3 => 0.0075f,
				5 => 0.01f,
				_ => 0.0125f,
			};
			fee += (int)Math.Truncate(itemPrice * commissionFee);

			if (!character.HasItem(ItemId.Silver, fee))
			{
				Log.Warning("CZ_REQ_MARKET_REGISTER: User '{0}' failed to remove registration fee.", conn.Account.Name);
				return;
			}

			Send.ZC_OBJECT_PROPERTY(character.Connection, item);
			Send.ZC_ADDON_MSG(conn.SelectedCharacter, AddonMessage.MARKET_REGISTER, 0, "None");

			//Remove Item and Cost from Inventory
			if (character.Inventory.Remove(itemWorldId, itemAmount, InventoryItemRemoveMsg.Given) != InventoryResult.Success)
			{
				Log.Warning("CZ_REQ_MARKET_REGISTER: User '{0}' failed to remove item.", conn.Account.Name);
				return;
			}

			character.RemoveItem(ItemId.Silver, fee);
			// To Force the item to be saved again.
			item.DbId = 0;
			ZoneServer.Instance.Database.SaveMarketItem(character, item, itemPrice, saleLength);
			Send.ZC_RELOAD_SELL_LIST(character);

			// Notify WebServer to reload market data
			var marketUpdateMsg = new MarketUpdateMessage(MarketUpdateType.ItemListed);
			ZoneServer.Instance.Communicator.Broadcast("AllServers", marketUpdateMsg);
		}

		[PacketHandler(Op.CZ_REQ_MARKET_MINMAX_INFO)]
		public void CZ_REQ_MARKET_MINMAX_INFO(IZoneConnection conn, Packet packet)
		{
			var itemWorldId = packet.GetLong();
			var character = conn.SelectedCharacter;

			if (!character.Inventory.TryGetItem(itemWorldId, out var item))
			{
				Log.Warning("CZ_REQ_MARKET_MINMAX_INFO: User '{0}' tried to check price for non-existent item.", conn.Account.Name);
				return;
			}

			// Get stats from DB
			var stats = ZoneServer.Instance.Database.GetMarketPriceStats(item.Id);

			long basePrice;

			if (stats.Count == 0)
			{
				// Case A: No Market History
				// Use the Item's Vendor Sell Price as the baseline.
				// If the item has no vendor price (0), default to 100 Silver to allow listing.
				basePrice = item.Data.SellPrice > 0 ? item.Data.SellPrice : 100;
			}
			else
			{
				// Case B: Market History Exists
				// The game uses the Average Sold Price as the anchor for all limits.
				basePrice = stats.Avg;
			}

			// Ensure baseline is never zero to prevent math errors or free listings
			if (basePrice <= 0) basePrice = 1;

			// 4. Calculate Limits based on Reverse Engineered Multipliers
			// Derived from analysis:
			// Min Price     = 50% (0.5x) of Average
			// Warning Price = 400% (4.0x) of Average (Soft Cap)
			// Max Price     = 2000% (20.0x) of Average (Hard Cap)

			var minLimit = (long)(basePrice * 0.5);
			var warningLimit = basePrice * 4;
			var maxLimit = basePrice * 20;

			// Sanity check: Min limit must be at least 1 silver
			if (minLimit < 1) minLimit = 1;

			// 5. Send Response
			// Send.ZC_NORMAL.MarketMinMaxInfo(Character, IsTradable, AvgPrice, MinLimit, WarningLimit, MaxLimit, UnitPrice)

			// We pass 'basePrice' as the 'unitPrice' (last argument) so the UI autofills the box with the average.
			Send.ZC_NORMAL.MarketMinMaxInfo(character, true, basePrice, minLimit, warningLimit, maxLimit, basePrice);
		}


		[PacketHandler(Op.CZ_REQ_MARKET_BUY)]
		public void CZ_REQ_MARKET_BUY(IZoneConnection conn, Packet packet)
		{
			var size = packet.GetShort();
			var count = packet.GetInt();
			var marketWorldId = packet.GetLong();
			var itemAmount = packet.GetInt();

			var character = conn.SelectedCharacter;
			if (!ZoneServer.Instance.Database.GetMarketItem(marketWorldId, out var item))
			{
				Log.Warning("CZ_REQ_MARKET_BUY: User '{0}' failed to find market item {1}.", conn.Account.Name, marketWorldId);
				return;
			}

			if (itemAmount > item.Count || itemAmount <= 0)
			{
				Log.Warning("CZ_REQ_MARKET_BUY: User '{0}' not enough quantity of the market item {1} - {2} > {3}.", conn.Account.Name, marketWorldId, itemAmount, item.Count);
				return;
			}

			var totalPrice = (int)item.SellPrice * itemAmount;
			var remainAmount = item.Count - itemAmount;
			if (!character.HasSilver(totalPrice))
				return;
			Send.ZC_NORMAL.MarketBuyItem(character, marketWorldId, remainAmount, 0);
			character.RemoveItem(ItemId.Silver, totalPrice);
			ZoneServer.Instance.Database.ModifyItemAmount(item.ItemGuid, -itemAmount);
			ZoneServer.Instance.Database.UpdateMarketItemBuyer(marketWorldId, character.DbId);
			Send.ZC_NORMAL.MarketRetrieveItem(character, item.ItemGuid);

			// Notify WebServer to reload market data
			var marketUpdateMsg = new MarketUpdateMessage(MarketUpdateType.ItemPurchased, marketWorldId);
			ZoneServer.Instance.Communicator.Broadcast("AllServers", marketUpdateMsg);
		}

		[PacketHandler(Op.CZ_REQ_CABINET_LIST)]
		public void CZ_REQ_CABINET_LIST(IZoneConnection conn, Packet packet)
		{
			var character = conn.SelectedCharacter;
			var items = ZoneServer.Instance.Database.GetMarketItems(character);

			Send.ZC_NORMAL.MarketRetrievalItems(character, items);
		}

		[PacketHandler(Op.CZ_REQ_GET_CABINET_ITEM)]
		public void CZ_REQ_GET_CABINET_ITEM(IZoneConnection conn, Packet packet)
		{
			var itemWorldId = packet.GetLong(); // itemUniqueId
			var itemId = packet.GetInt();       // ClassID
			var itemAmount = packet.GetInt();   // Count or Silver Amount

			var character = conn.SelectedCharacter;

			// 1. Fetch Market DTO to validate ownership and status
			if (!ZoneServer.Instance.Database.GetMarketItem(character.DbId, itemWorldId, out var item))
			{
				Log.Warning("CZ_REQ_GET_CABINET_ITEM: User '{0}' failed to find market item {1}.", conn.Account.Name, itemWorldId);
				return;
			}

			// 2. Handle Silver Retrieval (Sold Item)
			if (itemId == ItemId.Silver)
			{
				// Validate amount against sell price
				// Note: itemAmount from packet for silver retrieval is usually the price
				if (item.SellPrice != itemAmount)
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Silver mismatch. Packet: {0}, DB: {1}", itemAmount, item.SellPrice);
					return;
				}

				if (!item.IsSold)
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Attempted to retrieve silver for unsold item.");
					return;
				}

				if (item.HasReceivedSilver)
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Silver already received.");
					return;
				}

				var retrievedSilver = new Item(ItemId.Silver, (int)item.SellPrice);
				if (!character.Inventory.Add(retrievedSilver))
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Inventory full (Silver).");
					return;
				}

				item.Status |= MarketItemStatus.SilverReceived; // Use Flag OR to keep other flags if any
				ZoneServer.Instance.Database.UpdateMarketItemStatus(item.MarketGuid, item.Status);
				Send.ZC_NORMAL.MarketRetrieveItem(character, item.ItemGuid);
			}
			// 3. Handle Item Retrieval (Expired/Cancelled/Bought)
			else
			{
				// Validation
				if (item.ItemType != itemId || item.Count != itemAmount)
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Item mismatch. Packet: {0}:{1}, DB: {2}:{3}", itemId, itemAmount, item.ItemType, item.Count);
					return;
				}

				if (item.HasReceivedItem)
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Item already received.");
					return;
				}

				// Load Full Item Object
				var retrievedItem = ZoneServer.Instance.Database.LoadFullItemFromMarket(item.ItemGuid);
				if (retrievedItem == null)
				{
					Log.Error("CZ_REQ_GET_CABINET_ITEM: Critical Error - Item Data missing in DB for {0}", item.ItemGuid);
					return;
				}

				if (!character.Inventory.Add(retrievedItem))
				{
					Log.Warning("CZ_REQ_GET_CABINET_ITEM: Inventory full.");
					return;
				}

				item.Status |= MarketItemStatus.ItemReceived;
				ZoneServer.Instance.Database.UpdateMarketItemStatus(item.MarketGuid, item.Status);
				Send.ZC_NORMAL.MarketRetrieveItem(character, item.ItemGuid);
			}
		}

		[PacketHandler(Op.CZ_REQ_CANCEL_MARKET_ITEM)]
		public void CZ_REQ_CANCEL_MARKET_ITEM(IZoneConnection conn, Packet packet)
		{
			var marketWorldId = packet.GetLong();
			var character = conn.SelectedCharacter;

			var item = ZoneServer.Instance.Database.GetMarketItem(character, marketWorldId);
			if (item == null)
			{
				Log.Warning("CZ_REQ_CANCEL_MARKET_ITEM: User '{0}' failed to get market item {1}.", conn.Account.Name, marketWorldId);
				return;
			}

			if (!ZoneServer.Instance.Database.CancelMarketItem(character, marketWorldId))
			{
				Log.Warning("CZ_REQ_CANCEL_MARKET_ITEM: User '{0}' failed to cancel market item {1}.", conn.Account.Name, marketWorldId);
				return;
			}

			Send.ZC_NORMAL.MarketCancelItem(character, marketWorldId);
			Send.ZC_RELOAD_SELL_LIST(character);

			// Notify WebServer to reload market data
			var marketUpdateMsg = new MarketUpdateMessage(MarketUpdateType.ItemCancelled, marketWorldId);
			ZoneServer.Instance.Communicator.Broadcast("AllServers", marketUpdateMsg);
		}
	}
}
