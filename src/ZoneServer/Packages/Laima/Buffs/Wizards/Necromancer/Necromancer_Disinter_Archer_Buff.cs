using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Disinter_Archer_Buff)]
	public class Necromancer_Disinter_Archer_BuffOverride : BuffHandler
	{
		private const string CriticalRateVarName = "Melia.Disinter.CriticalRateBonus";
		private const float Bonus = 1.00f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var criticalRate = target.Properties.GetFloat(PropertyName.CRTHR);
			var criticalRateBonus = criticalRate * Bonus;

			buff.Vars.SetFloat(CriticalRateVarName, criticalRateBonus);

			target.Properties.Modify(PropertyName.CRTHR_BM, criticalRateBonus);
			target.Properties.Invalidate(PropertyName.CRTHR);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (!buff.Vars.TryGetFloat(CriticalRateVarName, out var criticalRateBonus))
				return;

			target.Properties.Modify(PropertyName.CRTHR_BM, -criticalRateBonus);
			target.Properties.Invalidate(PropertyName.CRTHR);
		}
	}
}
