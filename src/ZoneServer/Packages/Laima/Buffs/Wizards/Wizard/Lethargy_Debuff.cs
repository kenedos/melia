using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Network;
using System;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Lethargy main debuff that reduces a target's attack
	/// and evasion properties.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Lethargy_Debuff)]
	public class Lethargy_DebuffOverride : BuffHandler
	{
		private const float AtkReductionRatePerLevel = 3;
		private const float MovementReductionRatePerLevel = 4;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// We have to reduce PATK_BM and MATK_BM by a percentage of
			// their min/max values, but I'm not sure how the reference
			// values are supposed to be calculated, so we'll use the
			// average for now.

			var byAbility = 1 + (buff.NumArg2 * 0.005f);
			var skillLevel = buff.NumArg1;

			var minPAtk = buff.Target.Properties.GetFloat(PropertyName.MINPATK);
			var maxPAtk = buff.Target.Properties.GetFloat(PropertyName.MAXPATK);
			var patk = (minPAtk + maxPAtk) / 2;

			var minMAtk = buff.Target.Properties.GetFloat(PropertyName.MINMATK);
			var maxMAtk = buff.Target.Properties.GetFloat(PropertyName.MAXMATK);
			var matk = (minMAtk + maxMAtk) / 2;

			var dr = buff.Target.Properties.GetFloat(PropertyName.DR);

			var atkRate = (skillLevel * AtkReductionRatePerLevel * byAbility) / 100f;
			atkRate = Math.Min(0.8f, atkRate);

			var patkDebuff = patk * atkRate;
			var matkDebuff = matk * atkRate;

			var currentMovement = buff.Target.Properties.GetFloat(PropertyName.MSPD);
			var movementReduction = skillLevel * MovementReductionRatePerLevel * byAbility;
			movementReduction = Math.Min(30f, movementReduction);

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
