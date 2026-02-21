using System.Collections.Generic;
using Newtonsoft.Json;

namespace Melia.Shared.Game.Const.Web
{
	public class GearScore
	{
		[JsonProperty("my_rank")]
		public int MyRank { get; set; }

		[JsonProperty("my_score")]
		public int MyScore { get; set; }

		[JsonProperty("size")]
		public int Size { get; set; }

		[JsonProperty("list")]
		public List<GearScoreRanking> GearScoreRanking { get; set; }
	}

	public class GearScoreRanking
	{
		[JsonProperty("guild_name")]
		public string GuildName { get; set; }

		[JsonProperty("team_name")]
		public string TeamName { get; set; }

		[JsonProperty("char_name")]
		public string CharName { get; set; }

		[JsonProperty("guild_idx")]
		public string GuildIdx { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("value")]
		public int Value { get; set; }

		[JsonProperty("rank")]
		public int Rank { get; set; }
	}
}
