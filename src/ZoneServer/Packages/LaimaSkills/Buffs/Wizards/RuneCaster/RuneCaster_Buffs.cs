using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Wizards.RuneCaster
{
	/// <summary>
	/// Handler for Rune of Protection, which lowers the damage taken while
	/// casting by the skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Algiz_Buff)]
	public class RuneCaster_Algiz_BuffOverride : BuffHandler
	{
		private const float MaxReduction = 0.9f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Algiz_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Algiz_Buff, out var buff) || !target.IsCasting())
				return;

			skillHitResult.Damage *= Math.Max(1 - MaxReduction, 1 - GetCaptionRatio(buff, 1) / 100f);
		}
	}

	/// <summary>
	/// Handler for Rune of Protection: Giant, which raises defense and CON by
	/// 20% and movement speed by 20.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Thurisaz_Buff)]
	public class RuneCaster_Thurisaz_BuffOverride : BuffHandler
	{
		private const float DefenseRate = 0.20f;
		private const float ConRate = 0.20f;
		private const float SpeedBonus = 20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.CON_BM);

			UpdatePropertyModifier(buff, target, PropertyName.DEF_RATE_BM, DefenseRate);
			UpdatePropertyModifier(buff, target, PropertyName.CON_BM, (float)Math.Floor(target.Properties.GetFloat(PropertyName.CON) * ConRate));
			UpdatePropertyModifier(buff, target, PropertyName.MSPD_BM, SpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.CON_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
		}
	}
}
