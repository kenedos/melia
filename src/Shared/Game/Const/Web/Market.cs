using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.ObjectProperties;
using Newtonsoft.Json;

namespace Melia.Shared.Game.Const.Web
{
	public class MarketSearch
	{
		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("find_text")]
		public string FindText { get; set; }

		[JsonProperty("price_order")]
		public int PriceOrder { get; set; }

		/// <summary>
		/// Key: Filter Key (e.g., "CT_ItemGrade", "STR", "Reinforce_2")
		/// Value: Filter Value (e.g., "1;5" for range, or single value)
		/// </summary>
		[JsonProperty("search_option")]
		public Dictionary<string, string> SearchOption { get; set; }
	}

	public class MarketItemProperty
	{
		[JsonProperty("iesID")]
		public long IesId { get; set; } // Usually 0 or database ID

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }
	}

	/// <summary>
	/// Represents a single item listing in the market.
	/// Matches the JSON structure expected by the client (Zone_MarketInfoModel).
	/// </summary>
	public class MarketItem
	{
		[JsonProperty("marketGuid")]
		public long MarketGuid { get; set; }

		[JsonProperty("sellerAID")]
		public long SellerAccountObjectId => this.SellerAccountId | ObjectIdRanges.Accounts;

		[JsonProperty("sellerCID")]
		public long SellerCharacterObjectId => this.SellerCharacterId | ObjectIdRanges.Characters;

		[JsonProperty("seller")]
		public string SellerName { get; set; }

		[JsonProperty("itemGuid")]
		public long ItemGuid { get; set; }

		[JsonProperty("itemType")]
		public int ItemType { get; set; }

		[JsonProperty("sellPrice")]
		public long SellPrice { get; set; }

		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("regTime")]
		public DateTime RegTime { get; set; }

		[JsonProperty("endTime")]
		public DateTime EndTime { get; set; }

		[JsonProperty("showDelay")]
		public int ShowDelay { get; set; } = 0;

		[JsonProperty("isPrivate")]
		public bool IsPrivate { get; set; } = false;

		[JsonProperty("premiumState")]
		public int PremiumState { get; set; } = 0;

		[JsonProperty("is_mine")]
		public bool IsMine { get; set; }

		[JsonProperty("end_time")]
		public string EndTimeStr => IsMine ? EndTime.ToString("yyyy-MM-dd HH:mm:ss") : "null";

		[JsonProperty("properties")]
		public List<MarketItemProperty> Properties { get; set; } = new List<MarketItemProperty>();

		// --- Server Logic Fields (Not sent to Web JSON) ---

		[JsonIgnore]
		public MarketItemStatus Status { get; set; }

		[JsonIgnore]
		public long BuyerId { get; set; }

		[JsonIgnore]
		public long SellerAccountId { get; set; }

		[JsonIgnore]
		public long SellerCharacterId { get; set; }

		[JsonIgnore]
		public bool IsSold => this.Status.HasFlag(MarketItemStatus.Sold);

		[JsonIgnore]
		public bool IsCancelled => this.Status.HasFlag(MarketItemStatus.Cancelled);

		[JsonIgnore]
		public bool IsExpiredCheck => this.Status.HasFlag(MarketItemStatus.Expired) || (DateTime.MinValue != this.EndTime && DateTime.Now > this.EndTime);

		[JsonIgnore]
		public bool HasReceivedSilver => this.Status.HasFlag(MarketItemStatus.SilverReceived);

		[JsonIgnore]
		public bool HasReceivedItem => this.Status.HasFlag(MarketItemStatus.ItemReceived);

		[JsonIgnore]
		public ItemData Data { get; set; }

		[JsonIgnore]
		public Dictionary<string, string> PropertyMap { get; private set; }

		public void BuildPropertyMap()
		{
			PropertyMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (var prop in Properties)
			{
				if (!PropertyMap.ContainsKey(prop.Name))
					PropertyMap[prop.Name] = prop.Value;
			}
		}

		public void LoadData(ItemDb itemDb)
		{
			if (this.ItemType == 0) return;
			this.Data = itemDb.Find(this.ItemType);
		}
	}

	public class MarketSearchResult
	{
		public MarketSearchResult() { }
		public MarketSearchResult(List<MarketItem> items)
		{
			this.ItemList = items;
			this.TotalCount = items.Count;
		}

		[JsonProperty("total_cnt")]
		public int TotalCount { get; set; } = 0;

		[JsonProperty("item_list")]
		public List<MarketItem> ItemList { get; set; } = new List<MarketItem>();
	}

	public class MarketSearchRecipe
	{
		[JsonProperty("find_recipe_list")]
		public string FindRecipeList { get; set; }
	}

	public class CancelMarketItemResponse
	{
		[JsonProperty("market_idx")] public long MarketIdx { get; set; }
		[JsonProperty("seller_cid")] public long SellerCid { get; set; }
		[JsonProperty("item_guid")] public long ItemGuid { get; set; }
		[JsonProperty("item_type")] public long ItemType { get; set; }
		[JsonProperty("item_class_name")] public string ItemClassName { get; set; }
		[JsonProperty("belonging_count")] public int BelongingCount { get; set; } = 0;
		[JsonProperty("sell_price")] public long SellPrice { get; set; }
		[JsonProperty("count")] public int Count { get; set; }
		[JsonProperty("reg_time_str")] public string RegTimeStr { get; set; }
	}

	public static class MarketExtensions
	{
		public static int GetItemId(this MarketItem item, long characterId)
		{
			// If I am the seller, and it's sold, I see Silver icon (to retrieve money)
			if (item.SellerCharacterObjectId == characterId && item.IsSold)
				return ItemId.Silver;

			return item.ItemType;
		}

		public static string GetMarketStatus(this MarketItem item, long characterId)
		{
			if (item.BuyerId == characterId)
				return "market_buy";

			if (item.SellerCharacterObjectId == characterId)
			{
				if (item.IsSold)
					return "market_sell";
				if (item.IsCancelled)
					return "market_cancel";
				if (item.IsExpiredCheck)
					return "market_expire";
			}

			return "market_unknown";
		}
	}
}
