using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	[Package("laima")]
	[SkillHandler(SkillId.Mergen_FocusFire)]
	public class Mergen_TargetedArrow : IGroundSkillHandler, IDynamicCasted
	{
		private const int MaximumTargets = 10;
		private const int BaseHitCount = 5;
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan ShackleDuration = TimeSpan.FromSeconds(2);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, skill.Data.SplashRange).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).Take(MaximumTargets).ToList();

			if (selectedTarget != null && !selectedTarget.IsDead && !targets.Contains(selectedTarget))
			{
				targets.Insert(0, selectedTarget);

				if (targets.Count > MaximumTargets)
					targets.RemoveAt(targets.Count - 1);
			}

			if (targets.Count == 0)
			{
				caster.ServerMessage(Localization.Get("No valid target was found."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			var primaryTarget = targets[0];
			var targetPosition = primaryTarget.Position;

			caster.TurnTowards(targetPosition);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, primaryTarget.Handle, originPos, originPos.GetDirection(targetPosition), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var targetWasHit = false;

				for (var hitIndex = 0; hitIndex < BaseHitCount; hitIndex++)
				{
					if (target.IsDead)
						break;

					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= damageMultiplier;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

					if (skillHitResult.Result == HitResultType.Dodge)
						continue;

					target.TakeDamage(skillHitResult.Damage, caster);
					targetWasHit = true;

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.Data.DefaultHitDelay, skill.Data.DefaultHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);
				}

				if (targetWasHit)
					this.TryApplyShackle(caster, target, skill);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			skill.IncreaseOverheat();
			caster.SetAttackState(false);
		}

		private void TryApplyShackle(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			if (caster is not Character character || !character.IsAbilityActive(AbilityId.Mergen29))
				return;

			if (target is not Mob monster || monster.Rank == MonsterRank.Boss)
				return;

			target.StartBuff(BuffId.Bind_Debuff, 1, 0, ShackleDuration, caster, skill.Id);
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character || !character.TryGetAbility(AbilityId.Mergen3, out var ability))
				return 1f;

			var level = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var bonusPercent = level * 0.5f;

			if (level >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + (bonusPercent / 100f);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}
	}
}
