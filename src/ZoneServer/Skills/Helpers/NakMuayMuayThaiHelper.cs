using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Applies Muay Thai's final damage bonus to Nak Muay attacks.
	///
	/// Level 1: +14%
	/// Level 2: +16%
	/// Level 3: +18%
	/// Level 4: +20%
	/// Level 5: +22%
	/// </summary>
	public static class NakMuayMuayThaiHelper
	{
		public static void Apply(
			ICombatEntity caster,
			SkillHitResult skillHitResult)
		{
			if (!caster.TryGetBuff(BuffId.MuayThai_Buff, out var buff))
				return;

			var skillLevel = (int)buff.NumArg1;

			if (skillLevel < 1)
				return;

			skillLevel = System.Math.Min(skillLevel, 5);

			// Lv1 = 14%, aumentando 2% por nível.
			var finalDamageRate = 0.12f + (0.02f * skillLevel);

			skillHitResult.Damage *= 1f + finalDamageRate;
		}
	}
}
