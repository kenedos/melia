using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.Mon_FirePilla)]
	public class Mon_FirePilla : DamageOverTimeBuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (activationType == ActivationType.Start)
				Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_flame", buff.Duration);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Fire;
		}
	}
}
