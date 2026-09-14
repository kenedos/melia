using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handle for the Swashbuckling Debuff
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SwashBuckling_Debuff)]
	public class SwashBuckling_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// The skill's description states that the block and dodge only applies
			// to attacks from the caster, but our current implementation applies
			// to all incoming damage.
			// 
			// It also says that this grants a movement speed buff, but testing
			// has shown none.

			var target = buff.Target;

			var blockLoss = this.GetBlockPenalty(buff);
			var dodgeLoss = this.GetDodgePenalty(buff);

			AddPropertyModifier(buff, target, PropertyName.BLK_BM, -blockLoss);
			AddPropertyModifier(buff, target, PropertyName.DR_BM, -dodgeLoss);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, target, PropertyName.DR_BM);
		}

		private float GetBlockPenalty(Buff buff)
		{
			var curVal = buff.Target.Properties.GetFloat(PropertyName.BLK);

			var reductionRate = GetCaptionRatio(buff, 2) / 100f;

			var reduction = curVal * reductionRate;
			return Math.Min(0.75f, reduction);
		}

		private float GetDodgePenalty(Buff buff)
		{
			var curVal = buff.Target.Properties.GetFloat(PropertyName.DR);

			var reductionRate = GetCaptionRatio(buff, 3) / 100f;

			var reduction = curVal * reductionRate;
			return Math.Min(0.75f, reduction);
		}
	}
}
