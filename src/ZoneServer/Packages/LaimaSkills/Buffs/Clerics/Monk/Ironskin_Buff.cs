using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for Iron Skin buff. Provides high physical defense and
	/// knockdown/knockback prevention at the cost of SP per prevented hit.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Ironskin_Buff)]
	public class Ironskin_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const int KnockPreventSpCost = 80;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPairedPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, PropertyName.DEF_RATE_BM, GetCaptionRatio(buff, 1));

			buff.SetUpdateTime(1000);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is Character character)
			{
				if (!character.TryGetSkill(SkillId.Monk_IronSkin, out var skill))
				{
					character.StopBuff(BuffId.Ironskin_Buff);
					return;
				}

				if (!character.TrySpendSp(skill.SpendSp))
				{
					character.StopBuff(BuffId.Ironskin_Buff);
					return;
				}

				if (!character.IsWearingArmorOfType(ArmorMaterialType.Cloth))
				{
					character.StopBuff(BuffId.Ironskin_Buff);
					return;
				}
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePairedPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, PropertyName.DEF_RATE_BM);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return this.TryPreventKnock(buff);
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return this.TryPreventKnock(buff);
		}

		private KnockResult TryPreventKnock(Buff buff)
		{
			if (buff.Target is not Character character)
				return KnockResult.Allow;

			if (!character.TrySpendSp(KnockPreventSpCost))
			{
				character.StopBuff(BuffId.Ironskin_Buff);
				return KnockResult.Allow;
			}

			return KnockResult.Prevent;
		}
	}
}
