using Newtonsoft.Json;

namespace Melia.Shared.Game.Const.Web
{
	public class HousingInfo
	{
		[JsonProperty("team_name")]
		public string TeamName { get; set; }

		[JsonProperty("filter_type")]
		public string FilterType { get; set; }

		[JsonProperty("my_favorite_page")]
		public string MyFavoritePage { get; set; }

		[JsonProperty("socialInfo")]
		public SocialInfo SocialInfo { get; set; }

		[JsonProperty("pointInfo")]
		public PointInfo PointInfo { get; set; }

		[JsonProperty("thumbnail_id")]
		public string ThumbnailId { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("desc")]
		public string Desc { get; set; }

		[JsonProperty("page_id")]
		public string PageId { get; set; }

		[JsonProperty("channel_id")]
		public string ChannelId { get; set; }

		[JsonProperty("reg_time")]
		public string RegTime { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("world_id")]
		public int WorldId { get; set; }
	}

	public class SocialInfo
	{
		[JsonProperty("total_visitors")]
		public int TotalVisitors { get; set; }

		[JsonProperty("today_visitors")]
		public int TodayVisitors { get; set; }

		[JsonProperty("good_point")]
		public int GoodPoint { get; set; }
	}

	public class PointInfo
	{
		[JsonProperty("personalHousing_PlaceLV")]
		public int PersonalHousingPlaceLV { get; set; }

		[JsonProperty("personalHousing_Point1")]
		public int PersonalHousingPoint1 { get; set; }

		[JsonProperty("personalHousing_PlacePoint")]
		public int PersonalHousingPlacePoint { get; set; }
	}
}
