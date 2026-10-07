using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for Judgment, which raises critical chance by the skill's
	/// ratio in percent, and with [Arts] Judgment: Summary Decision marks
	/// the enemies the Inquisitor attacks as devils for 10 seconds.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Judgment_Buff)]
	public class Inquisitor_Judgment_BuffOverride : BuffHandler
	{
		private static readonly TimeSpan MarkDuration = TimeSpan.FromSeconds(10);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM, GetCaptionRatio(buff, 1) / 100f);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Judgment_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Damage <= 0 || !attacker.IsBuffActive(BuffId.Judgment_Buff))
				return;

			if (!attacker.IsAbilityActive(AbilityId.Inquisitor29) || attacker.IsAbilityActive(AbilityId.Inquisitor15))
				return;

			target.StartBuff(BuffId.Judgment_Abil_Debuff, 1, 0, MarkDuration, attacker, SkillId.Inquisitor_Judgment);
		}
	}

	/// <summary>
	/// Handler for Iron Maiden, which holds the target in place and lowers
	/// its max HP by 3%, and strikes it with Iron Maiden every tick if it's
	/// a devil or the Inquisitor is under Judgment.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.IronMaiden_Debuff)]
	public class Inquisitor_IronMaiden_DebuffOverride : BuffHandler
	{
		private const float MaxHpReduction = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			target.AddState(StateType.Held, buff.Duration);
			UpdatePropertyModifier(buff, target, PropertyName.MHP_BM, -(float)Math.Floor(target.Properties.GetFloat(PropertyName.MHP) * MaxHpReduction));
		}

		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead || target.IsDead)
				return;

			if (!InquisitorSkillHelper.IsPunishing(caster, target) || !caster.TryGetSkill(SkillId.Inquisitor_IronMaiden, out var skill))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MHP_BM);
		}
	}

	/// <summary>
	/// Handler for Malleus Maleficarum, which halves the target's INT and
	/// SPR and doubles the SP its magic skills cost.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.MalleusMaleficarum_Debuff)]
	public class Inquisitor_MalleusMaleficarum_DebuffOverride : BuffHandler
	{
		private const float StatReduction = 0.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target.Properties.Has(PropertyName.INT))
			{
				RemovePropertyModifier(buff, target, PropertyName.INT_BM);
				RemovePropertyModifier(buff, target, PropertyName.MNA_BM);

				UpdatePropertyModifier(buff, target, PropertyName.INT_BM, -(float)Math.Floor(target.Properties.GetFloat(PropertyName.INT) * StatReduction));
				UpdatePropertyModifier(buff, target, PropertyName.MNA_BM, -(float)Math.Floor(target.Properties.GetFloat(PropertyName.MNA) * StatReduction));
			}

			if (target.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.INT_BM);
			RemovePropertyModifier(buff, target, PropertyName.MNA_BM);

			if (target.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}
	}
}
