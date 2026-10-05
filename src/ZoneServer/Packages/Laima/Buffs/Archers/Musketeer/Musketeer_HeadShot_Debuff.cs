using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Musketeer
{
	[Package("laima")]
	[BuffHandler(BuffId.HeadShot_Debuff)]
	public class Musketeer_HeadShot_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const int StatPenaltyPerLevel = 5;
		private const float PvpBonusRateFromHr = 0.1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			if (activationType != ActivationType.Start)
				return;

			var target = buff.Target;
			if (buff.Caster is not ICombatEntity caster)
				return;

			Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");

			var skillLevel = buff.NumArg1;
			var statPenalty = StatPenaltyPerLevel * skillLevel;

			if (caster.Map.IsPVP)
			{
				var hitRate = caster.Properties.GetFloat(PropertyName.HR);
				statPenalty += hitRate * PvpBonusRateFromHr;
			}

			var finalPenalty = (float)Math.Floor(statPenalty);

			AddPropertyModifier(buff, target, PropertyName.INT_BM, -finalPenalty);
			AddPropertyModifier(buff, target, PropertyName.MSP_BM, -finalPenalty);
		}

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Normal;
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.INT_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSP_BM);
		}
	}
}
