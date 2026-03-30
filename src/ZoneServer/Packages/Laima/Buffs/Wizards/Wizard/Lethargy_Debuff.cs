using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using System;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Lethargy main debuff that reduces a target's
	/// physical and magical attack by a percentage, and movement speed
	/// by a flat amount.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: Wizard27 ability level (enhance)
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Lethargy_Debuff)]
	public class Lethargy_DebuffOverride : BuffHandler
	{
		private const float AtkReductionRatePerLevel = 3f;
		private const float MaxAtkReductionRate = 80f;
		private const float MovementReductionPerLevel = 4f;
		private const float MaxMovementReduction = 30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var byAbility = 1f + (buff.NumArg2 * 0.005f);
			var skillLevel = buff.NumArg1;

			var atkRate = skillLevel * AtkReductionRatePerLevel * byAbility;
			atkRate = Math.Min(MaxAtkReductionRate, atkRate) / 100f;

			var minPAtk = buff.Target.Properties.GetFloat(PropertyName.MINPATK);
			var maxPAtk = buff.Target.Properties.GetFloat(PropertyName.MAXPATK);
			var patkDebuff = (minPAtk + maxPAtk) / 2f * atkRate;

			var minMAtk = buff.Target.Properties.GetFloat(PropertyName.MINMATK);
			var maxMAtk = buff.Target.Properties.GetFloat(PropertyName.MAXMATK);
			var matkDebuff = (minMAtk + maxMAtk) / 2f * atkRate;

			var movementReduction = skillLevel * MovementReductionPerLevel * byAbility;
			movementReduction = Math.Min(MaxMovementReduction, movementReduction);

			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_BM, -patkDebuff);
			AddPropertyModifier(buff, buff.Target, PropertyName.MATK_BM, -matkDebuff);
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -movementReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
