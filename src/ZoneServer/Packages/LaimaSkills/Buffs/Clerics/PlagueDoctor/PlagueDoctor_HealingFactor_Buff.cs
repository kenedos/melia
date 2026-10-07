using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Healing Factor, which heals the target every 5 seconds
	/// while their HP is below what they had when they received it.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: HP the target had when the buff started
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HealingFactor_Buff)]
	public class PlagueDoctor_HealingFactor_BuffOverride : BuffHandler
	{
		private const int PartyHealInterval = 2500;
		private const float PartyHealRate = 0.6f;
		private const float CloseHealingRange = 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is ICombatEntity caster && caster.IsAbilityActive(AbilityId.PlagueDoctor18))
				buff.SetUpdateTime(PartyHealInterval);
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster || caster.Map != target.Map)
				return;

			var closeHealing = caster.IsAbilityActive(AbilityId.PlagueDoctor19);
			if (closeHealing && !caster.Position.InRange2D(target.Position, CloseHealingRange))
				return;

			var healAmount = caster.Properties.GetFloat(PropertyName.HEAL_PWR) * GetCaptionRatio(buff, 1) / 100f;

			if (caster.IsAbilityActive(AbilityId.PlagueDoctor18))
				healAmount *= PartyHealRate;

			if (!closeHealing)
				healAmount = Math.Min(healAmount, buff.NumArg2 - target.Hp);

			if (healAmount <= 0)
				return;

			target.Heal(healAmount, 0);
		}
	}
}
