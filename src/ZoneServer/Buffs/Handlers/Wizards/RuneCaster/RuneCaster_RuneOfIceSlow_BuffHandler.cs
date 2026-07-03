using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Wizards.RuneCaster
{
	[Package("laima")]
	[BuffHandler(BuffId.RuneOfEarth_Slow_Debuff)]
	public class RuneCaster_RuneOfEarthSlow_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var slow = this.GetSlowRate(buff);

			// Guarda o valor aplicado para remover exatamente o mesmo no OnEnd.
			buff.NumArg3 = slow;

			buff.Target.Properties.Modify(PropertyName.MSPD_BM, slow);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.Properties.Modify(PropertyName.MSPD_BM, -buff.NumArg3);
		}

		private float GetSlowRate(Buff buff)
		{
			var abilityLevel = buff.NumArg2;

			// Slow base: -15%
			// +1% por nível da ability.
			// Ex.: Lv1 = -16, Lv5 = -20.
			return -(15f + abilityLevel);
		}
	}
}
