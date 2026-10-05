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
	[BuffHandler(BuffId.Marschierendeslied_Buff)]
	public class Marschierendeslied_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler, IBuffOnHitInfoCreatedHandler
	{
		private const string RemainingBlockCountVariable = "Marschierendeslied.RemainingBlockCount";

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

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Target != buff.Target)
				return;

			var remainingBlockCount = buff.Vars.GetInt(RemainingBlockCountVariable);

			if (remainingBlockCount <= 0)
				return;

			remainingBlockCount--;

			buff.Vars.SetInt(RemainingBlockCountVariable, remainingBlockCount);
			buff.OverbuffCounter = Math.Max(remainingBlockCount, 1);
			buff.NotifyUpdate();

			if (remainingBlockCount <= 0)
				buff.Target.RemoveBuff(BuffId.Marschierendeslied_Buff);
		}
	}
}
