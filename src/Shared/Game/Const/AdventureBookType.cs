namespace Melia.Shared.Game.Const
{
	public enum AdventureBookType : byte
	{
		MonsterKilled = 0,
		MonsterDrop = 1,
		ItemObtained = 2,
		ItemPermanent = 3,
		Dungeon = 4,
		ItemCrafted = 5,
		Fishing = 6,
		PersonalShop = 7,
		Achievement = 8,
		Character = 9,
	}

	/// <summary>
	/// Legacy Adventure Journal
	/// </summary>
	public enum WikiType : byte
	{
		Unknown1 = 1,
		MonsterKilled = 2,
		Unknown3 = 3,
		Unknown4 = 4,
		Monster = 5,
	}
}
