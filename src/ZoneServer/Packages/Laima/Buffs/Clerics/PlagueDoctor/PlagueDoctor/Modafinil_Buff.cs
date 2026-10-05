using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Applies the Modafinil movement speed bonus.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Modafinil_Buff)]
	public class Modafinil_BuffOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float MovementSpeedAtLevelOne = 3f;
		private const float MovementSpeedAtLevelTen = 12f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var movementSpeedBonus = this.GetMovementSpeedBonus(skillLevel);

			if (buff.Caster is Character caster)
				movementSpeedBonus *= this.GetEnhanceMultiplier(caster);

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, movementSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		private float GetMovementSpeedBonus(int skillLevel)
		{
			var progress = (skillLevel - MinimumSkillLevel) / (float)(MaximumSkillLevel - MinimumSkillLevel);
			return MovementSpeedAtLevelOne + (MovementSpeedAtLevelTen - MovementSpeedAtLevelOne) * progress;
		}

		private float GetEnhanceMultiplier(Character caster)
		{
			var enhanceLevel = Math.Clamp(caster.Abilities.GetLevel(AbilityId.PlagueDoctor17), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
