using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner Sturzhau skill.
	/// Ignores a flat amount of DEF or MDEF based on the caster's maximum HP.
	/// </summary>
	[SkillHandler(SkillId.Doppelsoeldner_Sturzhau)]
	public class Doppelsoeldner_Sturzhau : IGroundSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumMaxHpRate = 0.05f;
		private const float MaximumMaxHpRate = 0.10f;

		/// <summary>
		/// Handles the skill cast.
		/// </summary>
		public void Handle(
			Skill skill,
			ICombatEntity caster,
			Position originPos,
			Position farPos,
			ICombatEntity target
		)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParameters = skill.GetSplashParameters(
				caster,
				originPos,
				farPos,
				length: 45,
				width: 30,
				angle: 0
			);

			var splashArea = skill.GetSplashArea(
				SplashType.Square,
				splashParameters
			);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(
				caster,
				skill,
				farPos,
				ForceId.GetNew(),
				null
			);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes both attacks of Sturzhau.
		/// </summary>
		private async Task Attack(
			Skill skill,
			ICombatEntity caster,
			ISplashArea splashArea
		)
		{
			var hitDelay = TimeSpan.FromMilliseconds(200);
			var animationTime = TimeSpan.FromMilliseconds(50);
			var delayBetweenAttacks = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(hitDelay);

			this.AttackTargets(
				skill,
				caster,
				splashArea,
				animationTime,
				skillHitDelay
			);

			await skill.Wait(delayBetweenAttacks);

			this.AttackTargets(
				skill,
				caster,
				splashArea,
				animationTime,
				skillHitDelay
			);
		}

		/// <summary>
		/// Attacks every valid target currently inside the skill area.
		/// </summary>
		private void AttackTargets(
			Skill skill,
			ICombatEntity caster,
			ISplashArea splashArea,
			TimeSpan animationTime,
			TimeSpan skillHitDelay
		)
		{
			var targets = caster.Map.GetAttackableEnemiesIn(
				caster,
				splashArea
			);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);

				modifier.DefensePenetrationRate =
					this.GetDefensePenetrationRate(
						skill,
						caster,
						target
					);

				var skillHitResult = SCR_SkillHit(
					caster,
					target,
					skill,
					modifier
				);

				target.TakeDamage(
					skillHitResult.Damage,
					caster
				);

				var skillHit = new SkillHitInfo(
					caster,
					target,
					skill,
					skillHitResult,
					animationTime,
					skillHitDelay
				);

				skillHit.HitEffect = HitEffect.Impact;
				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Converts the flat defense penetration based on maximum HP into
		/// the equivalent percentage of the target's relevant defense.
		/// </summary>
		private float GetDefensePenetrationRate(
			Skill skill,
			ICombatEntity caster,
			ICombatEntity target
		)
		{
			var skillLevel = Math.Clamp(
				skill.Level,
				MinimumSkillLevel,
				MaximumSkillLevel
			);

			var levelProgress =
				(skillLevel - MinimumSkillLevel) /
				(float)(MaximumSkillLevel - MinimumSkillLevel);

			var maximumHpRate =
				MinimumMaxHpRate +
				(MaximumMaxHpRate - MinimumMaxHpRate) *
				levelProgress;

			var maximumHp = caster.Properties.GetFloat(
				PropertyName.MHP
			);

			var ignoredDefenseAmount =
				maximumHp * maximumHpRate;

			var targetDefense =
				skill.Data.AttackType == SkillAttackType.Magic
					? target.Properties.GetFloat(PropertyName.MDEF)
					: target.Properties.GetFloat(PropertyName.DEF);

			if (targetDefense <= 0)
				return 0f;

			return Math.Clamp(
				ignoredDefenseAmount / targetDefense,
				0f,
				1f
			);
		}
	}
}
