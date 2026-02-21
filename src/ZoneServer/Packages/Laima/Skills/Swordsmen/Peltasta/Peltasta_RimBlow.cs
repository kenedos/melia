using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Swordsmen.Peltasta;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Peltasta
{
	/// <summary>
	/// Handler for the Peltasta skill Rim Blow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Peltasta_RimBlow)]
	public class Peltasta_RimBlowOverride : IMeleeGroundSkillHandler
	{
		private readonly static TimeSpan StunDuration = TimeSpan.FromSeconds(3);
		private const float StunChancePerLevel = 5f;

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 55, width: 25, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var hitDelay = TimeSpan.FromMilliseconds(50);
			var damageDelay = TimeSpan.FromMilliseconds(330);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			var bonusPAtk = Peltasta38.GetBonusPAtk(caster);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				// Get target's state BEFORE dealing damage
				var targetIsMoving = target.Components.TryGet<MovementComponent>(out var targetMovement) && targetMovement.IsMoving;

				var modifier = SkillModifier.MultiHit(4);
				modifier.BonusPAtk = bonusPAtk;

				// Increase damage by 10% if target is under the effect of
				// Swashbuckling from the caster
				if (target.TryGetBuff(BuffId.SwashBuckling_Debuff, out var swashBuckingDebuff)
					&& swashBuckingDebuff.Caster == caster)
					modifier.DamageMultiplier += 0.10f;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0 && !caster.TryGetActiveAbility(AbilityId.Peltasta35, out var abilityLevel) && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.Normal, 115, 10, formulaType: KnockBackFormulaType.Extended);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);

				if (skillHitResult.Damage > 0
					&& caster.TryGetActiveAbilityLevel(AbilityId.Impact, out int stunLevel) && RandomProvider.Get().Next(100) < stunLevel * StunChancePerLevel)
					target.StartBuff(BuffId.Stun, stunLevel, 0, StunDuration, caster);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
