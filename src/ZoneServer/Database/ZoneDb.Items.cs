using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Melia.Shared.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Storages;
using MySqlConnector;
using Yggdrasil.Logging;

namespace Melia.Zone.Database
{
	/// <summary>
	/// Contains methods related to Item and Storage persistence.
	/// </summary>
	public partial class ZoneDb
	{
		private void LoadCharacterItems(Character character)
		{
			using (Debug.Profile($"LoadItems.Total: {character.Name}", 500))
			{
				var items = new List<Item>();
				var equip = new Dictionary<EquipSlot, Item>();
				var cards = new Dictionary<int, Item>();

				using (Debug.Profile($"LoadItems.1_QueryDB: {character.Name}", 200))
				{
					using (var conn = this.GetConnection())
					using (var mc = new MySqlCommand("SELECT `i`.*, `inv`.`sort`, `inv`.`equipSlot` FROM `inventory` AS `inv` INNER JOIN `items` AS `i` ON `inv`.`itemId` = `i`.`itemUniqueId` WHERE `characterId` = @characterId ORDER BY `sort` ASC", conn))
					{
						mc.Parameters.AddWithValue("@characterId", character.DbId);
						using (var reader = mc.ExecuteReader(CommandBehavior.SequentialAccess))
						{
							while (reader.Read())
							{
								var itemUniqueId = reader.GetInt64("itemUniqueId");
								var itemId = reader.GetInt32("itemId");
								var amount = reader.GetInt32("amount");
								var equipSlotValue = reader.GetByte("equipSlot");

								if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
								{
									Log.Warning("ZoneDb.LoadCharacterItems: Item '{0}' not found in database, skipping from inventory.", itemId);
									continue;
								}

								var item = new Item(itemUniqueId, itemId, amount);

								// Check if it's a card slot (100-115 maps to card slots 1-15)
								if (equipSlotValue >= 100 && equipSlotValue <= 115)
								{
									// Convert to card slot 1-15
									var cardSlot = equipSlotValue - 100 + 1;
									cards[cardSlot] = item;
								}
								else
								{
									var equipSlot = (EquipSlot)equipSlotValue;
									if (!Enum.IsDefined(typeof(EquipSlot), equipSlot) || equipSlot == (EquipSlot)0x7F)
										items.Add(item);
									else
										equip[equipSlot] = item;
								}
							}
						}
					}
				}

				var allLoadedItems = items.Concat(equip.Values).Concat(cards.Values).ToList();

				using (Debug.Profile($"LoadItems.2_BatchLoadProps: {character.Name}", 500))
				{
					if (allLoadedItems.Any())
					{
						var allItemIds = allLoadedItems.Select(i => i.DbId).ToList();
						var itemPropsMap = allLoadedItems.ToDictionary(i => i.DbId, i => i.Properties);
						this.LoadPropertiesInBatch("item_properties", "itemId", allItemIds, itemPropsMap);
					}
				}

				using (Debug.Profile($"LoadItems.3_Hydrate: {character.Name}", 100))
				{
					foreach (var item in allLoadedItems)
					{
						// Fix incorrectly set GemLevel properties on cards/gems
						item.MigrateProperties();

						if (item.Data.Type == ItemType.Equip)
						{
							for (var i = 0; i < item.MaxSockets; i++)
							{
								if (!item.Properties.TryGetFloat($"Socket_{i}", out var socketType) || socketType == 0)
								continue;

							if (item.Properties.TryGetFloat($"Socket_Equip_{i}", out var gemId) && gemId != 0)
							{
								var gem = new Item((int)gemId);
								var exp = (int)item.Properties.GetFloat($"SocketItemExp_{i}");
								gem.Properties.SetFloat(PropertyName.ItemExp, item.Properties.GetFloat($"SocketItemExp_{i}"));
								gem.Properties.SetFloat(PropertyName.GemRoastingLv, item.Properties.GetFloat($"Socket_JamLv_{i}"));
								gem.Properties.SetFloat(PropertyName.BelongingCount, item.Properties.GetFloat($"Socket_GemBelongingCount_{i}"));
								gem.GetItemLevelExp(exp, out var totalLevel, out var cur, out var max);
								gem.Properties.SetFloat(PropertyName.Level, totalLevel);
								gem.UpdateGemStatOptions();
								item.SocketGemAt(i, gem);
							}
							else
								item.CreateSocket(i);
							}
						}
					}
				}

				using (Debug.Profile($"LoadItems.4_AddToComponent: {character.Name}", 100))
				{
					foreach (var item in items) character.Inventory.AddSilent(item);
					foreach (var kvp in equip) character.Inventory.SetEquipSilent(kvp.Key, kvp.Value);
					foreach (var kvp in cards) character.Inventory.AddSilentCard(kvp.Key, kvp.Value);
				}
			}
		}

		internal void LoadStorage(Storage storage, string tableName, string idFieldName, long id)
		{
			using (var conn = this.GetConnection())
			using (var mc = new MySqlCommand($"SELECT `i`.*, `stg`.`itemId`, `stg`.`position` FROM `{tableName}` AS `stg` INNER JOIN `items` AS `i` ON `stg`.`itemId` = `i`.`itemUniqueId` WHERE `{idFieldName}` = @id", conn))
			{
				mc.Parameters.AddWithValue("@id", id);
				using (var reader = mc.ExecuteReader())
				{
					while (reader.Read())
					{
						var itemUniqueId = reader.GetInt64("itemUniqueId");
						var itemId = reader.GetInt32("itemId");
						var amount = reader.GetInt32("amount");
						var position = reader.GetInt32("position");
						if (!ZoneServer.Instance.Data.ItemDb.Contains(itemId))
						{
							Log.Warning("ZoneDb.LoadStorage: Item '{0}' not found, removing it from storage.", itemId);
							continue;
						}
						var item = new Item(itemUniqueId, itemId, amount);
						if (!this.LoadProperties("item_properties", "itemId", item.DbId, item.Properties))
						{
							if (item.Properties.GetFloat(PropertyName.MaxDur) > 0)
								item.Durability = (int)item.Properties.GetFloat(PropertyName.MaxDur);
						}

						// Fix incorrectly set GemLevel properties on cards/gems
						item.MigrateProperties();

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
						if (itemId == ItemId.Silver && storage is TeamStorage teamStorage)
						{
							teamStorage.SetSilver(amount);
						}
						else
							storage.AddAtPosition(item, position, out var addedAmount);
					}
				}
			}
		}

		public void SaveStorage(Storage storage, string tableName, string idFieldName, long id)
		{
			using (var conn = this.GetConnection())
			using (var trans = conn.BeginTransaction())
			{
				try
				{
					this.InternalSaveStorage(storage, tableName, idFieldName, id, conn, trans);
					trans.Commit();
				}
				catch (Exception)
				{
					trans.Rollback();
					throw;
				}
			}
		}

		public bool ModifyItemAmount(long itemUniqueId, int amount)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `items` SET {0} WHERE `itemUniqueId` = @itemUniqueId", conn))
			{
				cmd.AddParameter("@itemUniqueId", itemUniqueId);
				cmd.Set("amount", amount);
				return cmd.Execute() == 1;
			}
		}
	}
}
