using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	[BuffHandler(BuffId.Poison_Mon)]
	public class Poison_Mon : DamageOverTimeBuffHandler
	{
		private const float AttackReductionPerLevel = -2f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (activationType == ActivationType.Start)
			{
				Send.ZC_SHOW_EMOTICON(buff.Target, "I_emo_poison", buff.Duration);

				if (buff.NumArg1 > 0)
					AddPropertyModifier(buff, buff.Target, PropertyName.ATK_BM, buff.NumArg1 * AttackReductionPerLevel);
			}
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.NumArg1 > 0)
				RemovePropertyModifier(buff, buff.Target, PropertyName.ATK_BM);
		}
	}
}
