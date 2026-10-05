using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Priest
{
	[Package("laima")]
	[BuffHandlerAttribute(BuffId.SpellShop_Sacrament_Buff)]
	public class SpellShop_Sacrament_BuffOverride : BuffHandler
	{
		private const float BaseHolyDamage = 360f;
		private const float HolyDamagePerLevel = 88f;
		private const int ShopSkillLevel = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var holyDamage = MathF.Floor(BaseHolyDamage + HolyDamagePerLevel * ShopSkillLevel);
			AddPropertyModifier(buff, buff.Target, PropertyName.Holy_Atk_BM, holyDamage);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.Holy_Atk_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.SpellShop_Sacrament_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.SpellShop_Sacrament_Buff, out _))
				return;

			if (skill.Data.Attribute == AttributeType.None || skill.Data.Attribute == AttributeType.Melee || skill.Data.Attribute == AttributeType.Magic)
				modifier.AttackAttribute = AttributeType.Holy;
		}
	}
}
