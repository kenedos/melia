using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Monsters
{
	public partial class Mob
	{
		// 100 = 100% Chance to drop that grade
		private static readonly Dictionary<MonsterRank, float[]> BaseDropChance = new Dictionary<MonsterRank, float[]>()
		{
			{ MonsterRank.Normal, new float[] { 100f, 25f, 5f, 1f, 0.2f, 0.04f } },
			{ MonsterRank.Elite, new float[] { 100f, 25f, 5f, 1f, 0.2f, 0.04f } },
			{ MonsterRank.Boss, new float[] { 100f, 25f, 5f, 1f, 0.2f, 0.04f } },
			{ MonsterRank.Special, new float[] { 100f, 25f, 5f, 1f, 0.2f, 0.04f } },
		};

		/// <summary>
		/// Calculates a chance for given grade to drop.
		/// Returns number representing the chance where 100 = 100%
		/// </summary>
		/// <remarks>
		/// May return values above 100, but never negative.
		/// </remarks>
		/// <param name="grade"></param>
		/// <param name="lootingChance"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		private float CalculateGradeDropChance(ItemGrade grade, float lootingChance, Item item)
		{
			var itemLevel = item.UseLevel;

			var monsterRank = this.Rank;
			var monsterLevel = this.Level;

			// Base Chance
			var baseDropChance = BaseDropChance[monsterRank][(int)grade - 1];

			// Monster level multiplier
			var level = Math.Max(0, monsterLevel - 40);
			var levelMultiplier = 1f + ((float)Math.Pow(level, 1.4f) / 200f);

			// Low level penalty (0.5x at level 1, 1.0x at level 40+)
			var cappedLevel = Math.Min(40, monsterLevel);
			var levelPenaltyMultiplier = 0.5f + (0.5f * (cappedLevel - 1) / 39.0f);

			var adjustedDropChance = baseDropChance * levelMultiplier * levelPenaltyMultiplier;

			// Looting Rate multiplier
			var lootingRate = 1f + lootingChance * 0.001f;
			adjustedDropChance *= lootingRate;

			var chance = baseDropChance * levelMultiplier;

			if (chance <= 0)
				return 0;

			// Log.Debug("Calculated chance for grade {0}: {1}", grade, chance);

			return chance;
		}

		private ItemGrade DetermineItemGrade(float lootingChance, Item item)
		{
			foreach (var grade in Enum.GetValues(typeof(ItemGrade)).Cast<ItemGrade>().Reverse())
			{
				if (grade == ItemGrade.Normal)
					return grade;

				var dropChance = this.CalculateGradeDropChance(grade, lootingChance, item);
				var randomChance = RandomProvider.Get().NextDouble() * 100;
				// Log.Debug("Drop Chance for '{0}' Grade: {1}%", grade.ToString(), dropChance);
				if (randomChance < dropChance)
					return grade;
			}

			return ItemGrade.Normal;
		}

		private void ApplyItemGrade(Item item, ItemGrade grade)
		{
			//Log.Debug("ApplyItemGrade: Applying {0} to item {1}", grade, item.Id);
			item.Properties.Modify(PropertyName.UseLv, (int)grade);
			item.Properties.SetFloat(PropertyName.ItemGrade, (int)grade); // Set the item grade property
			switch (grade)
			{
				case ItemGrade.Magic:
				case ItemGrade.Rare:
				case ItemGrade.Unique:
				case ItemGrade.Legend:
				case ItemGrade.Goddess:
					item.Properties.SetFloat(PropertyName.NeedAppraisal, 1);
					item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
					break;
				case ItemGrade.Normal:
					// No changes needed for normal items.
					break;
			}
		}
	}
}
