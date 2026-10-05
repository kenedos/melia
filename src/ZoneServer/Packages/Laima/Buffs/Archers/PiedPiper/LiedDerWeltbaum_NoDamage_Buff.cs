using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.LiedDerWeltbaum_NoDamage_Buff)]
	public class LiedDerWeltbaum_NoDamage_BuffOverride : BuffHandler, IBuffOnHitInfoCreatedHandler
	{
		private const string RemainingBlockCountVariable = "LiedDerWeltbaum.RemainingBlockCount";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var blockCount = Math.Max((int)buff.NumArg1, 1);

			buff.Vars.SetInt(RemainingBlockCountVariable, blockCount);
			buff.OverbuffCounter = blockCount;
			buff.NotifyUpdate();
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			var remainingBlockCount = buff.Vars.GetInt(RemainingBlockCountVariable);

			if (remainingBlockCount <= 0)
				return;

			skillHitInfo.HitInfo.Damage = 0;
			skillHitInfo.HitInfo.Type = HitType.Endure;

			remainingBlockCount--;

			buff.Vars.SetInt(RemainingBlockCountVariable, remainingBlockCount);

			if (remainingBlockCount > 0)
			{
				buff.OverbuffCounter = remainingBlockCount;
				buff.NotifyUpdate();
				return;
			}

			buff.Target.RemoveBuff(BuffId.LiedDerWeltbaum_NoDamage_Buff);
		}
	}
}
