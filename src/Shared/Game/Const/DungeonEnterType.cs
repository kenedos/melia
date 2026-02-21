namespace Melia.Shared.Game.Const
{
	public enum DungeonEnterType
	{
		None = 0,
		EnterNow = 1,
		AutoMatch = 2,
		AutoMatchWithParty = 3,
		Reenter = 4
	}

	/// <summary>
	/// A structure to hold the configuration options for the Instance Dungeon Matchmaking UI.
	/// </summary>
	public class DungeonOptions
	{
		public bool AllowAutoMatchReenter { get; set; } = true;
		public bool AllowAutoMatch { get; set; } = true;
		public bool AllowEnterNow { get; set; } = true;
		public bool AllowPartyMatch { get; set; } = true;
		public bool IsGrowthSupportGuildParty { get; set; } = false;
	}
}
