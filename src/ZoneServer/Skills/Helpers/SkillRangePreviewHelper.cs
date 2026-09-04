using System;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Helpers
{
	public static class SkillRangePreviewHelper
	{
		/// <summary>
		/// Draws a debug outline of a skill's damage area. Characters need
		/// the "Melia.RangePreview" temp flag; non-Companion mobs always show.
		/// Pass an explicit duration when the skill's ShootTime covers the
		/// entire skill rather than a single projectile.
		/// </summary>
		public static void ShowRangePreview(ICombatEntity caster, Skill skill, IShapeF area, TimeSpan? duration = null)
		{
			if (!IsPreviewEnabled(caster))
				return;

			var effectiveDuration = duration ?? (skill.Data.ShootTime < SkillConstants.MaxShootTimeForPreview
				? skill.Data.ShootTime
				: SkillConstants.DefaultDebugShapeDuration);

			Debug.ShowShape(caster.Map, area, effectiveDuration);
		}

		/// <summary>
		/// Draws a debug outline of an area a skill searched, for the default
		/// duration. For areas resolved without a skill's timings to size the
		/// preview by.
		/// </summary>
		public static void ShowRangePreview(ICombatEntity caster, IShapeF area)
		{
			if (!IsPreviewEnabled(caster))
				return;

			Debug.ShowShape(caster.Map, area, SkillConstants.DefaultDebugShapeDuration);
		}

		/// <summary>
		/// Returns whether the caster should be shown range previews.
		/// </summary>
		/// <param name="caster"></param>
		private static bool IsPreviewEnabled(ICombatEntity caster)
		{
			if (caster is not Character character)
				return caster is Mob && caster is not Companion;

			return character.Variables.Temp.GetBool("Melia.RangePreview");
		}

		/// <summary>
		/// Returns a Donut if innerRange > 0, otherwise a CircleF.
		/// </summary>
		public static IShapeF GetPreviewArea(Position position, float range, float innerRange = 0)
		{
			if (innerRange > 0)
				return new Donut(position, range, innerRange);
			return new CircleF(position, range);
		}

		/// <summary>
		/// Same as GetPreviewArea but floors the range to the caster's body
		/// radius, matching SplashDamage's effective hit area.
		/// </summary>
		public static IShapeF GetPreviewArea(ICombatEntity caster, Position position, float range, float innerRange = 0)
		{
			range = Math.Max(range, SizeTypeRadius.GetRadius(caster.EffectiveSize));
			return GetPreviewArea(position, range, innerRange);
		}
	}
}
