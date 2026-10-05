using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Abilities.Handlers.Archers.Cannoneer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Cannoneer
{
	/// <summary>

	/// Sweeping Cannon.

	/// Posiciona a turret à frente do personagem e aplica quatro ataques

	/// em até 23 inimigos dentro da área.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.Cannoneer_SweepingCannon)]
	public class Cannoneer_SweepingCannonOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int HitCount = 4;
		private const int MaximumTargets = 23;
		private const float EffectRadius = 120f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.Zero;
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets?.FirstOrDefault());
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position turretPosition, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead || caster.Map == null)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(turretPosition);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, turretPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, caster.Direction, turretPosition);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, turretPosition, ForceId.GetNew(), null);

			var damageMultiplier = Cannoneer_SweepingCannonEnhanceAbility.GetDamageMultiplier(character);
			skill.Run(this.Attack(skill, caster, turretPosition, damageMultiplier));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position turretPosition, float damageMultiplier)
		{
			try
			{
				await skill.Wait(FirstHitDelay);

				for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
				{
					if (caster.IsDead || caster.Map == null)
						break;

					var targets = this.GetTargets(caster, turretPosition);
					var hits = new List<SkillHitInfo>();

					foreach (var target in targets)
					{
						var skillHitResult = SCR_SkillHit(caster, target, skill);
						skillHitResult.Damage = Math.Max(1, (int)(skillHitResult.Damage * damageMultiplier));

						target.TakeDamage(skillHitResult.Damage, caster);

						var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
						skillHit.ForceId = ForceId.GetNew();
						hits.Add(skillHit);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);

					if (hitIndex + 1 < HitCount)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				caster.SetAttackState(false);
				Send.ZC_NORMAL.Skill_45(caster);
				Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
				Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			}
		}

		private IList<ICombatEntity> GetTargets(ICombatEntity caster, Position turretPosition)
		{
			var splashArea = new Circle(turretPosition, EffectRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target => target != null && !target.IsDead)
				.Distinct()
				.OrderBy(target => turretPosition.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();
		}
	}
}
