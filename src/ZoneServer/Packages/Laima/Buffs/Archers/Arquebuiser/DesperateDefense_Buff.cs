using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Desperate Defense Buff, which reduces the damage
	/// received and increases the target's defensive stats.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DesperateDefense_Buff)]
	public class DesperateDefense_Buff : BuffHandler
	{
		private const float ResistanceRateBase = 0.05f;
		private const float ResistanceRatePerLevel = 0.015f;
		private const float AoeDefenseRatioBonus = 4;
		private const float DamageReductionBase = 0.10f;
		private const float DamageReductionPerLevel = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var resistanceRate = ResistanceRateBase + ResistanceRatePerLevel * this.GetSkillLevel(buff);

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM, resistanceRate);
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM, resistanceRate);
			AddPropertyModifier(buff, buff.Target, PropertyName.SDR_BM, AoeDefenseRatioBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.SDR_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.DesperateDefense_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.DesperateDefense_Buff, out var buff))
				return;

			var damageReduction = DamageReductionBase + DamageReductionPerLevel * (this.GetSkillLevel(buff) - 1);
			skillHitResult.Damage *= Math.Max(0f, 1f - damageReduction);
		}

		// The buff's args are all 0 on the client, the level is taken
		// from the caster's skill instead
		private int GetSkillLevel(Buff buff)
		{
			if (buff.Caster is ICombatEntity caster && caster.TryGetSkill(SkillId.Arquebusier_DesperateDefense, out var skill))
				return skill.Level;

			return 1;
		}
	}
}
