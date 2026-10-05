using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Periodically restores the target's HP up to the amount it had when Healing Factor was applied.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HealingFactor_Buff)]
	public class HealingFactor_BuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float HealFactorAtLevelOne = 1020f;
		private const float HealFactorAtLevelTen = 2257f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.UpdateTime = TimeSpan.FromSeconds(5);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character target || target.IsDead)
				return;

			if (buff.Caster is not Character caster)
				return;

			var currentHp = target.Properties.GetFloat(PropertyName.HP);
			var maximumHp = target.Properties.GetFloat(PropertyName.MHP);
			var referenceHp = Math.Min(buff.NumArg2, maximumHp);

			if (currentHp >= referenceHp)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var healFactor = this.GetHealFactor(skillLevel);
			var healingPower = caster.Properties.GetFloat(PropertyName.HEAL_PWR);
			var healingAmount = healingPower * healFactor / 100f;
			var missingReferenceHp = referenceHp - currentHp;
			var appliedHealing = Math.Min(healingAmount, missingReferenceHp);

			if (appliedHealing <= 0)
				return;

			target.ModifyHp(appliedHealing);
		}

		private float GetHealFactor(int skillLevel)
		{
			var progress = (skillLevel - MinimumSkillLevel) / (float)(MaximumSkillLevel - MinimumSkillLevel);
			return HealFactorAtLevelOne + (HealFactorAtLevelTen - HealFactorAtLevelOne) * progress;
		}
	}
}
