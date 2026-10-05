using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Assault Fire / Marching Fire.
	/// SkillId: 51006
	/// ClassName: Schwarzereiter_AssaultFire
	///
	/// Continuously attacks every valid enemy inside the skill area.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_AssaultFire)]
	public class SchwarzerReiter_AssaultFireOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int SplashRadius = 120;
		private const int NormalHitCount = 25;
		private const int EnhancedHitCount = 35;

		private static readonly TimeSpan TotalDuration = TimeSpan.FromMilliseconds(5000);

		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(250);

		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(270);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var character = caster as Character;

			var hasTakingCover = character != null && character.IsAbilityActive(AbilityId.Schwarzereiter29);

			caster.StartBuff(
				BuffId.AssaultFire_Buff,
				1f,
				hasTakingCover ? 1f : 0f,
				TimeSpan.Zero,
				caster,
				skill.Id);

			// [Arts] Marching Fire: Taking Cover
			// Reduces the caster's accuracy while channeling.
			if (hasTakingCover)
			{
				character.Properties.Modify(PropertyName.HR_BM, -1000);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.HR,
					PropertyName.HR_BM);
			}
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			var character = caster as Character;

			// Restore the accuracy removed by Taking Cover.
			if (character != null &&
				caster.TryGetBuff(BuffId.AssaultFire_Buff, out var buff) &&
				buff.NumArg2 > 0)
			{
				character.Properties.Modify(PropertyName.HR_BM, 1000);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.HR,
					PropertyName.HR_BM);
			}

			caster.StopBuff(BuffId.AssaultFire_Buff);
			caster.SetAttackState(false);
		}

		public void Handle(
			Skill skill,
			ICombatEntity caster,
			Position originPos,
			Position farPos,
			ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(
				caster,
				skill,
				1,
				originPos,
				farPos);

			Send.ZC_NORMAL.UpdateSkillEffect(
				caster,
				targetHandle,
				originPos,
				originPos.GetDirection(farPos),
				Position.Zero);

			Send.ZC_SKILL_MELEE_GROUND(
				caster,
				skill,
				farPos);

			skill.Run(
				this.Attack(
					skill,
					caster,
					farPos));
		}

		private async Task Attack(
			Skill skill,
			ICombatEntity caster,
			Position farPos)
		{
			var character = caster as Character;

			// [Arts] Marching Fire: Enhanced Upgrade
			var hasEnhancedUpgrade =
				character != null &&
				character.IsAbilityActive(AbilityId.Schwarzereiter24);

			var hitCount = hasEnhancedUpgrade
				? EnhancedHitCount
				: NormalHitCount;

			var artsDamageMultiplier = hasEnhancedUpgrade
				? 1.3f
				: 1f;

			var intervalMilliseconds =
				(TotalDuration.TotalMilliseconds - FirstHitDelay.TotalMilliseconds) /
				hitCount;

			var delayBetweenHits =
				TimeSpan.FromMilliseconds(intervalMilliseconds);

			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(FirstHitDelay);

			for (var i = 0; i < hitCount; i++)
			{
				// Stop immediately if the channeling buff was removed.
				if (!caster.TryGetBuff(BuffId.AssaultFire_Buff, out _))
					break;

				// Search again every cycle so enemies entering or leaving
				// the area are handled correctly.
				var targets = this.GetTargets(caster, farPos);

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var skillHitResult = SCR_SkillHit(caster, target, skill);

					skillHitResult.Damage *= artsDamageMultiplier;

					target.TakeDamage( skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(
						caster,
						target,
						skill,
						skillHitResult,
						HitAnimationTime,
						skillHitDelay);

					hits.Add(skillHit);
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);

				if (i + 1 < hitCount)
					await skill.Wait(delayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		/// <summary>
		/// Returns every living attackable enemy inside the skill area.
		/// The list is recalculated for every channeling hit.
		/// </summary>
		private IList<ICombatEntity> GetTargets(
			ICombatEntity caster,
			Position farPos)
		{
			var splashArea = new Circle(caster.Position, SplashRadius);

			return caster.Map
				.GetAttackableEnemiesIn(caster, splashArea)
				.Where(target =>
					target != null &&
					!target.IsDead)
				.ToList();
		}
	}
}
