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
	[Package("laima")]
	[BuffHandler(BuffId.Ironskin_Buff)]
	public class Ironskin_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const float BaseDef = 100f;
		private const float DefPerLevel = 10f;
		private const float BaseDefRate = 0.5f;
		private const float DefRatePerLevel = 0.05f;
		private const int KnockPreventSpCost = 80;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;

			var flatDef = BaseDef + DefPerLevel * skillLevel;
			var defRate = BaseDefRate + DefRatePerLevel * skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, flatDef);
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM, defRate);

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
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
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
