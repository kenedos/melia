using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Common
{
	[BuffHandler(BuffId.HeavyBleeding, BuffId.UC_bleed)]
	public class Bleeding : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (activationType == ActivationType.Start)
				Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_bleed", buff.Duration);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Bleeding;
		}
	}
}
