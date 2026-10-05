using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	/// <summary>

	/// Applies the effects of Bunshin no Jutsu: Jin.

	/// </summary>

	public static class ShinobiBunshinJinHelper
	{
		private const int KatonBurnTickCount = 7;
		private const float KatonBurnDamageRate = 0.20f;
		private static readonly TimeSpan KatonBurnTickInterval = TimeSpan.FromSeconds(1);

		public static bool IsActive(ICombatEntity caster)
		{
			if (caster is DummyCharacter clone)
				return clone.Owner != null && clone.Owner.IsAbilityActive(AbilityId.Shinobi16);

			return caster is Character character && character.IsAbilityActive(AbilityId.Shinobi16);
		}

		public static void ApplyKatonBurn(ICombatEntity caster, ICombatEntity target, Skill skill, float initialDamage)
		{
			if (!IsActive(caster) || target == null || target.IsDead)
				return;

			var tickDamage = initialDamage * KatonBurnDamageRate;
			skill.Run(ApplyKatonBurnTicks(caster, target, skill, tickDamage));
		}

		private static async Task ApplyKatonBurnTicks(ICombatEntity caster, ICombatEntity target, Skill skill, float tickDamage)
		{
			for (var tick = 0; tick < KatonBurnTickCount; tick++)
			{
				await skill.Wait(KatonBurnTickInterval);

				if (caster.Map == null || target.Map != caster.Map || target.IsDead)
					return;

				target.TakeDamage(tickDamage, caster);
			}
		}
	}
}
