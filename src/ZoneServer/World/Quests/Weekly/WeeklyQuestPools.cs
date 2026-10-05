using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Quests.Daily;

namespace Melia.Zone.World.Quests.Weekly
{
	public static class WeeklyQuestPools
	{
		public static IReadOnlyList<DailyQuestPools.MapTarget> MapTargets => DailyQuestPools.MapTargets;

		public static IReadOnlyList<RaceType> RaceTargets { get; } = new[]
		{
			RaceType.Klaida,
			RaceType.Paramune,
			RaceType.Forester,
			RaceType.Velnias,
			RaceType.Widling,
		};

		public static IReadOnlyList<AttributeType> AttributeTargets { get; } = new[]
		{
			AttributeType.Fire,
			AttributeType.Ice,
			AttributeType.Poison,
			AttributeType.Earth,
			AttributeType.Soul,
			AttributeType.Lightning,
			AttributeType.Holy,
			AttributeType.Dark,
		};

		public static IReadOnlyList<SizeType> SizeTargets { get; } = new[]
		{
			SizeType.S,
			SizeType.M,
			SizeType.L,
		};
	}
}
