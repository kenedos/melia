using System;
using System.Collections.Generic;
using System.Data;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.ObjectProperties;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using MySqlConnector;
using Yggdrasil.Logging;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods related to the Market persistence.
	/// </summary>
	public partial class ZoneDb
	{
		/// <summary>
		/// Persists an item in the database.
		/// </summary>
		/// <param name="item"></param>
		private long SaveItem(Item item)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `items` {0}", conn))
			{
				cmd.Set("itemId", item.Id);
				cmd.Set("amount", item.Amount);
				cmd.Execute();
				return cmd.LastId;
			}
		}

		/// <summary>
		/// Save an item listed on the market.
		/// </summary>
		public void SaveMarketItem(Character character, Item item, long itemPrice, int saleLength)
		{
			if (character == null || item == null)
				return;

			// Ensure item is saved and has a DbId before listing it.
			if (item.DbId <= 0)
				item.DbId = this.SaveItem(item);

			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				using (var cmd = new InsertCommand("INSERT INTO `market_items` {0}", conn, trans))
				{
					var registerDate = DateTime.Now;
					var expireDate = registerDate.AddDays(saleLength);
					cmd.Set("itemUniqueId", item.DbId);
					cmd.Set("sellerId", character.DbId);
					cmd.Set("price", itemPrice);
					cmd.Set("dateRegistered", registerDate);
					cmd.Set("dateExpired", expireDate);
					cmd.Set("status", MarketItemStatus.Listed);
					cmd.Execute();
				}
				trans.Commit();
			}
		}

		/// <summary>
		/// Cancel an item listed on the market.
		/// </summary>
		public bool CancelMarketItem(Character character, long marketWorldId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `market_items` SET {0} WHERE `sellerId` = @characterId AND `marketItemUniqueId` = @marketId", conn))
			{
				var date = DateTime.Now;
				cmd.AddParameter("@characterId", character.DbId);
				cmd.AddParameter("@marketId", marketWorldId);
				cmd.Set("dateExpired", date);
				cmd.Set("status", MarketItemStatus.Cancelled);
				return cmd.Execute() > 0;
			}
		}

		/// <summary>
		/// Get market items for a specific character (Cabinet / My Sell List).
		/// Maps to the new MarketItem DTO and uses internal helpers for status checks.
		/// </summary>
		public List<MarketItem> GetMarketItems(Character character, bool isExpiredOrCancelledOrBought = true)
		{
			var result = new List<MarketItem>();
			if (character == null)
				return result;

			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT `market_items`.*, `items`.`itemId`, `items`.`amount` FROM `market_items` INNER JOIN `items` ON `market_items`.`itemUniqueId` = `items`.`itemUniqueId` WHERE `sellerId` = @characterId OR `buyerId` = @characterId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						// 1. Hydrate the MarketItem object completely first
						var item = new MarketItem
						{
							MarketGuid = reader.GetInt64("marketItemUniqueId"),
							ItemGuid = reader.GetInt64("itemUniqueId"),
							ItemType = reader.GetInt32("itemId"),
							Count = reader.GetInt32("amount"), // Mapped from items table
							SellerCharacterId = reader.GetInt64("sellerId"),
							BuyerId = !reader.IsDBNull("buyerId") ? reader.GetInt64("buyerId") : 0,
							SellPrice = reader.GetInt32("price"),
							RegTime = reader.GetDateTimeSafe("dateRegistered"),
							EndTime = reader.GetDateTimeSafe("dateExpired"),
							Status = (MarketItemStatus)reader.GetByte("status"), // Essential for helper properties
							IsPrivate = true,
							PremiumState = 0
						};

						// Set derived property
						item.IsMine = (item.SellerCharacterObjectId == character.DbId);

						// 2. Filter based on flags/state
						var include = true;

						if (isExpiredOrCancelledOrBought)
						{
							// Cabinet Logic: Items needing action (Sold, Cancelled, Expired) OR Items I bought
							var isBoughtByMe = item.BuyerId == character.DbId;

							// Note: IsExpiredCheck helper handles both Status flag and Date check
							if (!(item.IsSold || item.IsCancelled || item.IsExpiredCheck || isBoughtByMe))
								include = false;
						}
						else
						{
							// My Sell List Logic: Only active listings
							if (item.Status != MarketItemStatus.Listed)
								include = false;
						}

						if (include)
							result.Add(item);
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Get a specific Market Item DTO from Database by Market ID.
		/// </summary>
		/// <param name="marketWorldId"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool GetMarketItem(long marketWorldId, out MarketItem item)
		{
			item = null;
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT `market_items`.*, `items`.`itemId`, `items`.`amount` FROM `market_items` INNER JOIN `items` ON `market_items`.`itemUniqueId` = `items`.`itemUniqueId` WHERE `marketitemUniqueId` = @marketitemUniqueId", conn))
			{
				cmd.Parameters.AddWithValue("@marketitemUniqueId", marketWorldId);
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return false;

					item = new MarketItem
					{
						MarketGuid = marketWorldId,
						ItemGuid = reader.GetInt64("itemUniqueId"),
						ItemType = reader.GetInt32("itemId"),
						Count = reader.GetInt32("amount"),
						RegTime = reader.GetDateTimeSafe("dateRegistered"),
						EndTime = reader.GetDateTimeSafe("dateExpired"),
						SellPrice = reader.GetInt32("price"),
						SellerCharacterId = reader.GetInt64("sellerId"),
						BuyerId = !reader.IsDBNull("buyerId") ? reader.GetInt64("buyerId") : 0,
						Status = (MarketItemStatus)reader.GetByte("status"),
						IsMine = false,
						IsPrivate = false
					};
				}
			}
			return item != null;
		}

		/// <summary>
		/// Load a full Game Item object from the market (used for retrieving items to inventory).
		/// This returns Melia.Zone.World.Items.Item, NOT the DTO.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public Item GetMarketItem(Character character, long marketWorldId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT `market_items`.*, `items`.`itemId`, `items`.`amount` FROM `market_items` INNER JOIN `items` ON `market_items`.`itemUniqueId` = `items`.`itemUniqueId` WHERE `sellerId` = @characterId OR `buyerId` = @characterId AND `marketItemUniqueId` = @marketId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", character.DbId);
				cmd.AddParameter("@marketId", marketWorldId);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var itemUniqueId = reader.GetInt64("itemUniqueId");
						var itemId = reader.GetInt32("itemId");
						var amount = reader.GetInt32("amount");

						// Check item, in case its data was removed
						if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
						{
							Log.Warning("ZoneDb.GetMarketItem: Item '{0}' not found in Game Data.", itemId);
							continue;
						}

						var item = new Item(itemUniqueId, itemId, amount);
						if (!this.LoadProperties("item_properties", "itemId", item.DbId, item.Properties))
						{
							// Fallback for durability if properties missing
							if (item.Properties.GetFloat(PropertyName.MaxDur) > 0)
								item.Durability = (int)item.Properties.GetFloat(PropertyName.MaxDur);
						}

						if (item.Data.Type == ItemType.Equip)
						{
							for (var i = 0; i < item.MaxSockets; i++)
							{
								if (!item.Properties.TryGetFloat($"Socket_{i}", out var socketType) || socketType == 0)
									continue;

								if (item.Properties.TryGetFloat($"Socket_Equip_{i}", out var gemId) && gemId != 0)
								{
									var gem = new Item((int)gemId);
									gem.Properties.SetFloat(PropertyName.ItemExp, item.Properties.GetFloat($"SocketItemExp_{i}"));
									gem.Properties.SetFloat(PropertyName.GemRoastingLv, item.Properties.GetFloat($"Socket_JamLv_{i}"));
									gem.Properties.SetFloat(PropertyName.BelongingCount, item.Properties.GetFloat($"Socket_GemBelongingCount_{i}"));
									item.SocketGemAt(i, gem);
								}
								else
									item.CreateSocket(i);
							}
						}

						return item;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Get Market Item DTO from Database for a specific character and Item GUID.
		/// Used for validation during retrieval (Cabinet).
		/// </summary>
		/// <param name="characterId">The character ID trying to access the item (Buyer or Seller)</param>
		/// <param name="itemWorldId">This is treated as ItemUniqueId in the query</param>
		/// <param name="item">The resulting DTO</param>
		/// <returns>True if found and authorized</returns>
		public bool GetMarketItem(long characterId, long itemWorldId, out MarketItem item)
		{
			if (characterId > ObjectIdRanges.Characters)
				characterId -= ObjectIdRanges.Characters;
			if (itemWorldId > ObjectIdRanges.Items)
				itemWorldId -= ObjectIdRanges.Items;

			item = null;
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT `market_items`.*, `items`.`itemId`, `items`.`amount` FROM `market_items` INNER JOIN `items` ON `market_items`.`itemUniqueId` = `items`.`itemUniqueId` WHERE (`sellerId` = @characterId OR `buyerId` = @characterId) AND `market_items`.`itemUniqueId` = @itemUniqueId", conn))
			{
				cmd.Parameters.AddWithValue("@characterId", characterId);
				cmd.Parameters.AddWithValue("@itemUniqueId", itemWorldId);
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return false;

					var sellerId = reader.GetInt64("sellerId");

					item = new MarketItem
					{
						MarketGuid = reader.GetInt64("marketItemUniqueId"),
						ItemGuid = itemWorldId,
						ItemType = reader.GetInt32("itemId"),
						Count = reader.GetInt32("amount"),
						RegTime = reader.GetDateTimeSafe("dateRegistered"),
						EndTime = reader.GetDateTimeSafe("dateExpired"),
						SellPrice = reader.GetInt32("price"),
						SellerCharacterId = sellerId,
						BuyerId = !reader.IsDBNull("buyerId") ? reader.GetInt64("buyerId") : 0,
						Status = (MarketItemStatus)reader.GetByte("status"),
						IsMine = (sellerId == characterId),
						IsPrivate = true
					};
				}
			}
			return item != null;
		}

		/// <summary>
		/// Loads the full Item object (with properties, gems, etc.) associated with a Market entry.
		/// Use this when retrieving an item back to inventory.
		/// </summary>
		public Item LoadFullItemFromMarket(long itemUniqueId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `items` WHERE `itemUniqueId` = @uid", conn))
			{
				cmd.Parameters.AddWithValue("@uid", itemUniqueId);
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
					{
						Log.Error("LoadFullItemFromMarket: Failed to find item {0} in items table.", itemUniqueId);
						return null;
					}

					var itemId = reader.GetInt32("itemId");
					var amount = reader.GetInt32("amount");

					// 1. Base Item
					var item = new Item(itemUniqueId, itemId, amount);

					// 2. Load Properties (Enhancements, Durability, Potential, etc.)
					if (!this.LoadProperties("item_properties", "itemId", item.DbId, item.Properties))
					{
						// Fallback for durability if properties missing logic
						if (item.Properties.GetFloat(PropertyName.MaxDur) > 0)
							item.Durability = (int)item.Properties.GetFloat(PropertyName.MaxDur);
					}

					// 3. Load Gems / Sockets
					if (item.Data.Type == ItemType.Equip)
					{
						for (var i = 0; i < item.MaxSockets; i++)
						{
							if (!item.Properties.Has($"Socket_{i}"))
								continue;

							if (item.Properties.TryGetFloat($"Socket_{i}", out var socketType) && socketType != 0)
							{
								// Check if a gem is equipped
								if (item.Properties.TryGetFloat($"Socket_Equip_{i}", out var gemId) && gemId != 0)
								{
									var gem = new Item((int)gemId);
									gem.Properties.SetFloat(PropertyName.ItemExp, item.Properties.GetFloat($"SocketItemExp_{i}"));
									gem.Properties.SetFloat(PropertyName.GemRoastingLv, item.Properties.GetFloat($"Socket_JamLv_{i}"));
									gem.Properties.SetFloat(PropertyName.BelongingCount, item.Properties.GetFloat($"Socket_GemBelongingCount_{i}"));
									item.SocketGemAt(i, gem);
								}
								else
								{
									// Empty socket
									item.CreateSocket(i);
								}
							}
						}
					}

					return item;
				}
			}
		}

		/// <summary>
		/// Delete market item
		/// </summary>
		public void DeleteMarketItem(long characterId, long marketWorldId)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				// Note: Cascade delete trigger on `inventory` -> `items` exists in schema,
				// but here we are deleting from `market_items`.

				using (var cmd = new MySqlCommand("DELETE FROM `market_items` WHERE `sellerId` = @characterId AND `marketItemUniqueId` = @marketId", conn, trans))
				{
					cmd.Parameters.AddWithValue("@characterId", characterId);
					cmd.Parameters.AddWithValue("@marketId", marketWorldId);
					cmd.ExecuteNonQuery();
				}
				trans.Commit();
			}
		}

		/// <summary>
		/// Update a market item buyer after purchase
		/// </summary>
		public bool UpdateMarketItemBuyer(long marketWorldId, long characterId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `market_items` SET {0} WHERE `marketItemUniqueId` = @marketItemUniqueId", conn))
			{
				cmd.AddParameter("@marketItemUniqueId", marketWorldId);
				cmd.Set("buyerId", characterId);
				cmd.Set("status", MarketItemStatus.Sold);
				return cmd.Execute() == 1;
			}
		}

		/// <summary>
		/// Update a market item status
		/// </summary>
		public bool UpdateMarketItemStatus(long marketWorldId, MarketItemStatus status)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `market_items` SET {0} WHERE `marketItemUniqueId` = @marketItemUniqueId", conn))
			{
				cmd.AddParameter("@marketItemUniqueId", marketWorldId);
				cmd.Set("status", (byte)status);
				return cmd.Execute() == 1;
			}
		}

		/// <summary>
		/// Calculates the Min, Max, and Average price for a specific Item Type (ClassID).
		/// Returns tuple: (Min, Max, Avg, Count).
		/// </summary>
		public (long Min, long Max, long Avg, int Count) GetMarketPriceStats(int itemClassId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand(@"
                SELECT 
                    MIN(m.price) as minPrice, 
                    MAX(m.price) as maxPrice, 
                    AVG(m.price) as avgPrice, 
                    COUNT(*) as totalCount
                FROM market_items m
                JOIN items i ON m.itemUniqueId = i.itemUniqueId
                WHERE i.itemId = @cid AND m.status = @status", conn))
			{
				cmd.Parameters.AddWithValue("@cid", itemClassId);
				cmd.Parameters.AddWithValue("@status", (byte)MarketItemStatus.Listed);

				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read() && !reader.IsDBNull(0))
					{
						long min = reader.GetInt64("minPrice");
						long max = reader.GetInt64("maxPrice");
						long avg = (long)reader.GetDouble("avgPrice");
						int count = Convert.ToInt32(reader.GetInt64("totalCount"));
						return (min, max, avg, count);
					}
				}
			}
			return (0, 0, 0, 0);
		}
	}
}
