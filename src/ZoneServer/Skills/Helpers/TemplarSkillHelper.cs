using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Templar's Uplift, flags and orders.
	/// </summary>
	public static class TemplarSkillHelper
	{
		/// <summary>
		/// Highest Uplift stage a Templar can hold.
		/// </summary>
		public const int MaxUplift = 3;

		private static readonly TimeSpan UpliftInterval = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan UpliftTickInterval = TimeSpan.FromMilliseconds(900);
		private const string UpliftProgressVar = "Melia.Templar.UpliftProgress";
		private const string UpliftLastTickVar = "Melia.Templar.UpliftLastTick";
		private const string MoraleMinCritVar = "Melia.Templar.MoraleMinCrit";

		private static readonly string[] FlagPads = [PadName.Templer_MoraleBanner, PadName.Templer_RevengeBanner, PadName.Templer_VitalityBanner];

		/// <summary>
		/// Returns the entity's current Uplift stage.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static int GetUplift(ICombatEntity entity)
		{
			if (!entity.TryGetBuff(BuffId.Templar_Enhancement_Buff, out var buff))
				return 0;

			return Math.Min(MaxUplift, buff.OverbuffCounter);
		}

		/// <summary>
		/// Removes the entity's Uplift and returns the stage it had.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static int ConsumeUplift(ICombatEntity entity)
		{
			var uplift = GetUplift(entity);
			if (uplift > 0)
				entity.StopBuff(BuffId.Templar_Enhancement_Buff);

			return uplift;
		}

		/// <summary>
		/// Advances the Templar's Uplift by one second, granting a stage
		/// every five seconds. Several sources ticking in the same second
		/// count once.
		/// </summary>
		/// <param name="templar"></param>
		public static void ProgressUplift(ICombatEntity templar)
		{
			if (templar is not Character character || character.IsDead)
				return;

			var now = GameClock.LocalNow;
			var lastTick = character.Variables.Temp.Get<DateTime>(UpliftLastTickVar, DateTime.MinValue);
			if (now - lastTick < UpliftTickInterval)
				return;

			character.Variables.Temp.Set(UpliftLastTickVar, now);

			if (GetUplift(character) >= MaxUplift)
				return;

			var progress = character.Variables.Temp.GetInt(UpliftProgressVar) + 1;
			if (progress < UpliftInterval.TotalSeconds)
			{
				character.Variables.Temp.SetInt(UpliftProgressVar, progress);
				return;
			}

			character.Variables.Temp.SetInt(UpliftProgressVar, 0);
			character.StartBuff(BuffId.Templar_Enhancement_Buff, 1, 0, TimeSpan.Zero, character);
		}

		/// <summary>
		/// Destroys the caster's flags, as only one can stand at a time.
		/// </summary>
		/// <param name="caster"></param>
		public static void RemoveFlags(ICombatEntity caster)
		{
			foreach (var pad in caster.Map.GetPads(p => p.Creator == caster && FlagPads.Contains(p.Name)))
				pad.Destroy();
		}

		/// <summary>
		/// Raises the entity's minimum critical chance while it stands
		/// inside a Flag of Morale.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="minCritChance"></param>
		public static void SetMoraleMinCrit(ICombatEntity entity, float minCritChance)
			=> entity.SetTempVar(MoraleMinCritVar, minCritChance);

		/// <summary>
		/// Removes the Flag of Morale bonus from the entity.
		/// </summary>
		/// <param name="entity"></param>
		public static void ClearMoraleMinCrit(ICombatEntity entity)
			=> entity.RemoveTempVar(MoraleMinCritVar);

		/// <summary>
		/// Returns the minimum critical chance granted to the entity by a
		/// Flag of Morale, in percent.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public static float GetMoraleMinCrit(ICombatEntity entity)
			=> entity.GetTempVar(MoraleMinCritVar);
	}
}
