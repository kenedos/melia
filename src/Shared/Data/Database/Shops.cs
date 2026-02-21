using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	public enum PersonalShopType
	{
		// 4 (0 = Buff Shop, 2 = Repair Shop,3 = Gem Roasting, 5 = Food Table, 9 = Portal Shop, 10 = Item Awakening)
		PersonalSell = -3,
		NPC = -2,
		Personal = -1,
		SpellShop = 0, // Not sure about this value
		Buff = 1,
		Repair = 2,
		GemRoasting = 3,
		Oblation = 4,
		FoodTable = 5,
		Portal = 9,
		ItemAwakening = 10,
	}

	/// <summary>
	/// Custom Shop Types
	/// </summary>
	public enum ShopType
	{
		None, Weapon, Armor, Potion, General, Material, Accessory
	}

	[Serializable]
	public class ShopData
	{
		public string Name { get; set; }
		public bool IsCustom { get; set; }
		public PersonalShopType Type { get; set; } = PersonalShopType.NPC;
		public Dictionary<int, ProductData> Products { get; set; } = new Dictionary<int, ProductData>();
		public int Level { get; set; }
		public int EffectId { get; set; }
		public string ShopAnimation
		{
			get
			{
				return this.Type switch
				{
					PersonalShopType.SpellShop => "Pardoner_SpellShop",
					PersonalShopType.Repair => "Squire_Repair",
					PersonalShopType.Portal => "Sage_PortalShop",
					_ => "Squire_Repair",
				};
			}
		}

		public bool IsClosed { get; set; }
		public int OwnerHandle { get; set; }

		public int SkillIcon
		{
			get
			{
				return this.Type switch
				{
					PersonalShopType.ItemAwakening => 21007,
					PersonalShopType.SpellShop => 40805,
					PersonalShopType.Oblation => 40804,
					PersonalShopType.Repair => 50301,
					PersonalShopType.FoodTable => 50304,
					_ => 0,
				};
			}
		}

		public void AddProduct(ProductData product)
		{
			this.Products.Add(this.Products.Count, product);
		}

		public ProductData GetProduct(int id)
		{
			this.Products.TryGetValue(id, out var product);
			return product;
		}
	}

	[Serializable]
	public class ProductData
	{
		public string ShopName { get; set; }
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int Amount { get; set; } = 1;
		public int Price { get; set; }
		public float PriceMultiplier { get; set; } = 1f;
		public int RequiredAmount { get; set; }
		public string RequiredFactionId { get; set; } = null; // Faction Identifier String
		public int RequiredTierValue { get; set; } = -1000; // Store the minimum *integer value* required (e.g., 250 for Liked, -1000 for Hated/no requirement)

		/// <summary>
		/// World IDs of the specific item instances being sold in a personal sell shop.
		/// Used to track and verify that the seller still has the exact items listed.
		/// </summary>
		public List<long> ItemWorldIds { get; set; } = new List<long>();
	}

	/// <summary>
	/// Shop database, indexed by shop name.
	/// </summary>
	public class ShopDb : DatabaseJsonIndexed<string, ShopData>
	{
		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("shopName", "productId", "itemId", "amount", "priceMultiplier");

			var data = new ProductData();

			data.ShopName = entry.ReadString("shopName");
			data.Id = entry.ReadInt("productId");
			data.ItemId = entry.ReadInt("itemId");
			data.Amount = entry.ReadInt("amount");
			data.PriceMultiplier = entry.ReadFloat("priceMultiplier", 1f);

			if (!this.Entries.TryGetValue(data.ShopName, out var shopData))
			{
				shopData = new ShopData();
				shopData.Name = data.ShopName;

				this.AddOrReplace(data.ShopName, shopData);
			}

			shopData.Products[data.Id] = data;
		}
	}
}
