using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Melia.Shared.Game.Const.Web
{
	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PartyRequestLimitation
	{
		public int GearScoreLimit { get; set; }
		public int AbilityPoint { get; set; }
		public int RelicLevel { get; set; }
	}

	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PartyRequestInfo
	{
		public string Type { get; set; }
		public string Msg { get; set; }
		public DateTime RegTime { get; set; }
		public DateTime ExpireTime { get; set; }
		public string OwnerAidx { get; set; }
		public PartyRequestLimitation Limitation { get; set; }
		public List<string> MemberList { get; set; }

		public void AddMember(string name, int jobId)
		{
			MemberList.Add($"{name}/{jobId}/0/0/0");
		}
	}

	[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public class PartyRequests
	{
		public int TotalCnt { get; set; }
		public List<PartyRequestInfo> PartyList { get; set; }
	}
}
