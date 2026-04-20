using System;
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
			if (caster is not Character character)
			{
				if (caster is not Mob || caster is Companion)
					return;
			}
			else if (!character.Variables.Temp.GetBool("Melia.RangePreview"))
			{
				return;
			}

			var effectiveDuration = duration ?? (skill.Data.ShootTime < SkillConstants.MaxShootTimeForPreview
				? skill.Data.ShootTime
				: SkillConstants.DefaultDebugShapeDuration);

			Debug.ShowShape(caster.Map, area, effectiveDuration);
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
	}
}
