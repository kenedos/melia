using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Nak Muay skill Sok Chiang.
	/// Requires Ram Muay stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.NakMuay_SokChiang)]
	public class NakMuay_SokChiangOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MultiHitCount = 4;
		private const int BleedingDurationSeconds = 10;
		private const int BleedingTickCount = 10;
		private static readonly TimeSpan InitialHitDelay = TimeSpan.FromMilliseconds(400);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(120);
		private const float SkillRange = 100f;

		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			this.Cast(skill, caster, caster.Position, target?.Position ?? caster.Position, new[] { target });
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, new[] { target });
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IEnumerable<ICombatEntity> targets)
		{
			if (!caster.TryGetBuff(BuffId.RamMuay_Buff, out _))
			{
				caster.ServerMessage(Localization.Get("Ram Muay is required."));
				this.RejectCast(skill, caster, originPos, farPos);
				return;
			}

			var validTargets = targets.Where(target => target != null && !target.IsDead && caster.GetDistance(target) <= SkillRange).Take(1).ToList();

			if (validTargets.Count == 0)
			{
				this.RejectCast(skill, caster, originPos, farPos);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.RejectCast(skill, caster, originPos, farPos);
				return;
			}

			var primaryTarget = validTargets[0];

			skill.IncreaseOverheat();
			caster.TurnTowards(primaryTarget);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, primaryTarget.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, primaryTarget);

			skill.Run(this.HandleSkill(caster, skill, primaryTarget));
		}

		private void RejectCast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos);
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, ICombatEntity target)
		{
			try
			{
				await skill.Wait(InitialHitDelay);

				var totalDamage = 0f;

				for (var hitIndex = 0; hitIndex < MultiHitCount; hitIndex++)
				{
					if (target.IsDead)
						break;

					totalDamage += this.ApplyHit(caster, target, skill);

					if (hitIndex < MultiHitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}

				if (!target.IsDead && totalDamage > 0)
				{
					var bleedingDamagePerTick = totalDamage / BleedingTickCount;
					target.StartBuff(BuffId.HeavyBleeding, skill.Level, bleedingDamagePerTick, TimeSpan.FromSeconds(BleedingDurationSeconds), caster);
				}
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}

		private float ApplyHit(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);

			NakMuayMuayThaiHelper.Apply(caster, skillHitResult);
			this.ApplyEnhanceAttribute(caster, skillHitResult);

			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(20), TimeSpan.Zero);

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);

			return skillHitResult.Damage;
		}

		private void ApplyEnhanceAttribute(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			if (!character.Abilities.TryGet(AbilityId.NakMuay2, out var ability))
				return;

			if (!ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
