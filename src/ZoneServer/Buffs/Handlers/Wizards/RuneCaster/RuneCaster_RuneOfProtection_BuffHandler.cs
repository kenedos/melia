using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Wizards.RuneCaster
{
	[Package("laima")]
	[BuffHandler(BuffId.Algiz_Buff)]
	public class RuneCaster_RuneOfProtection_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler, IBuffBeforeKnockdownHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			buff.NumArg2 = this.GetDamageReductionRate(buff, character);
		}

		public override void OnEnd(Buff buff)
		{
			buff.NumArg2 = 0;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			var reductionRate = buff.NumArg2;

			if (reductionRate <= 0)
				return;

			skillHitInfo.HitInfo.Damage *= 1f - reductionRate;
		}

		private float GetDamageReductionRate(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			// Base Rune of Protection reduction.
			var reductionRate = 0.05f + (0.01f * skillLevel);

			if (character.Abilities.TryGet(AbilityId.RuneCaster10, out var ability) && ability.Active)
			{
				var enhanceRate = ability.Level * 0.005f;

				if (ability.Level >= 100)
					enhanceRate += 0.10f;

				reductionRate *= 1f + enhanceRate;
			}

			return Math.Clamp(reductionRate, 0f, 0.95f);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			if (buff.Target.TryGetAbility(AbilityId.RuneCaster11, out _))
				return KnockResult.Prevent;

			return KnockResult.Allow;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			if (buff.Target.TryGetAbility(AbilityId.RuneCaster11, out _))
				return KnockResult.Prevent;

			return KnockResult.Allow;
		}
	}
}
