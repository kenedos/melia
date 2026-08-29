using System;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Items
{
	/// <summary>
	/// Decides whether a character is allowed to pick up a dropped item,
	/// based on their explicit loot filter or, without one, the automatic
	/// trash loot rules.
	/// </summary>
	public static class LootFilter
	{
		private const string FilterGradeVarName = "Laima.LootFilterGrade";
		private const string GraceStepVarName = "Laima.LootFilterGraceStep";
		private const string GraceUntilVarName = "Laima.LootFilterGraceUntil";
		private const string LastProofVarName = "Laima.LootFilterLastProof";
		private const int MaxGraceStep = 16;

		/// <summary>
		/// Returns true if the item is considered trash loot for the
		/// given character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <param name="grade"></param>
		/// <returns></returns>
		public static bool IsTrashLoot(Character character, Item item, ItemGrade grade)
		{
			if (character == null || item == null)
				return false;

			var conf = ZoneServer.Instance.Conf.World;

			if (!conf.TrashLootEnabled)
				return false;

			if (grade > (ItemGrade)conf.TrashLootMaxGrade)
				return false;

			return item.UseLevel <= character.Level - conf.TrashLootLevelGap;
		}

		/// <summary>
		/// Returns true if the character is currently barred from picking
		/// up the given item.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool BlocksPickUp(Character character, Item item)
		{
			if (character == null || item == null)
				return false;

			var filterGrade = GetFilterGrade(character);

			if (filterGrade != ItemGrade.None)
			{
				if (item.Data.Type != ItemType.Equip)
					return false;

				return GetItemGrade(item) < filterGrade;
			}

			if (!item.IsTrashLoot)
				return false;

			if (IsGraceActive(character))
				return false;

			return DateTime.Now < item.TrashPickUpTime;
		}

		/// <summary>
		/// Records that the character waited out the pickup delay for a
		/// trash item, granting and extending their grace period.
		/// </summary>
		/// <param name="character"></param>
		public static void RegisterTrashPickUp(Character character)
		{
			if (character == null)
				return;

			var conf = ZoneServer.Instance.Conf.World;
			var now = DateTime.Now;

			var step = Math.Min(GetGraceStep(character) + 1, MaxGraceStep);
			var seconds = Math.Min(conf.TrashLootGraceSeconds * Math.Pow(2, step - 1), conf.TrashLootGraceMaxSeconds);

			character.Variables.Perm.SetInt(GraceStepVarName, step);
			character.Variables.Perm.SetLong(LastProofVarName, now.Ticks);
			character.Variables.Perm.SetLong(GraceUntilVarName, now.AddSeconds(seconds).Ticks);
		}

		/// <summary>
		/// Returns true if the character's trash loot grace period is
		/// currently active.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static bool IsGraceActive(Character character)
		{
			var graceUntil = character.Variables.Perm.GetLong(GraceUntilVarName, 0);
			if (graceUntil == 0)
				return false;

			return DateTime.Now.Ticks < graceUntil;
		}

		/// <summary>
		/// Returns the character's grace step, reduced by the time that
		/// passed since they last picked up a trash item.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static int GetGraceStep(Character character)
		{
			var step = character.Variables.Perm.GetInt(GraceStepVarName, 0);
			if (step <= 0)
				return 0;

			var lastProof = character.Variables.Perm.GetLong(LastProofVarName, 0);
			if (lastProof == 0)
				return step;

			var decaySeconds = ZoneServer.Instance.Conf.World.TrashLootGraceDecaySeconds;
			if (decaySeconds <= 0)
				return step;

			var elapsed = DateTime.Now - new DateTime(lastProof);
			if (elapsed <= TimeSpan.Zero)
				return step;

			return Math.Max(0, step - (int)(elapsed.TotalSeconds / decaySeconds));
		}

		/// <summary>
		/// Returns the grade a character's explicit loot filter is set to,
		/// or None if they don't have one.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static ItemGrade GetFilterGrade(Character character)
		{
			return (ItemGrade)character.Variables.Perm.GetInt(FilterGradeVarName, (int)ItemGrade.None);
		}

		/// <summary>
		/// Sets the character's explicit loot filter, disabling it if the
		/// grade is None.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="grade"></param>
		public static void SetFilterGrade(Character character, ItemGrade grade)
		{
			character.Variables.Perm.SetInt(FilterGradeVarName, (int)grade);
		}

		/// <summary>
		/// Returns the loot filter grade matching the given name via out.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="grade"></param>
		/// <returns></returns>
		public static bool TryParseFilterGrade(string name, out ItemGrade grade)
		{
			grade = ItemGrade.None;

			if (string.IsNullOrWhiteSpace(name))
				return false;

			if (name.Equals("off", StringComparison.OrdinalIgnoreCase))
				return true;

			return Enum.TryParse(name, true, out grade) && Enum.IsDefined(typeof(ItemGrade), grade);
		}

		/// <summary>
		/// Returns the grade the item was rolled with.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private static ItemGrade GetItemGrade(Item item)
		{
			return (ItemGrade)(int)item.Properties.GetFloat(PropertyName.ItemGrade, (int)ItemGrade.Normal);
		}
	}
}
