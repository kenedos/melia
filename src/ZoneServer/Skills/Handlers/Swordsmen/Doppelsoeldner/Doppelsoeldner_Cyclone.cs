using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner Cyclone skill.
	/// </summary>
	[SkillHandler(SkillId.Doppelsoeldner_Cyclone)]
	public class Doppelsoeldner_Cyclone : IGroundSkillHandler, IDynamicCasted
	{
		private const float HpCostRate = 0.025f;
		private const float FinalDamageBonusPerHpCost = 0.025f;

		private static readonly TimeSpan HpCostInterval = TimeSpan.FromSeconds(1);

		/// <summary>
		/// Enables movement while Cyclone is being channeled.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StartBuff(BuffId.Cyclone_EnableMovingShot_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);

			if (caster is Character character)
			{
				Send.ZC_OBJECT_PROPERTY(character);
				Send.ZC_MOVE_SPEED(character);
			}

			if (caster.TryGetActiveAbilityLevel(AbilityId.Doppelsoeldner17, out var abilityLevel))
			{
				caster.StartBuff(BuffId.Cyclone_Buff_ImmuneAbil, abilityLevel, 0, TimeSpan.Zero, caster);
			}
		}

		/// <summary>
		/// Removes Cyclone's movement and immunity buffs when casting ends.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.Cyclone_EnableMovingShot_Buff);

			caster.StopBuff(BuffId.Cyclone_Buff_ImmuneAbil);

			if (caster is Character character)
			{
				Send.ZC_OBJECT_PROPERTY(character);
				Send.ZC_MOVE_SPEED(character);
			}
		}

		/// <summary>
		/// Starts Cyclone.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || caster.IsDead || caster.Map == null)
			{
				this.StopCyclone(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));

				this.StopCyclone(caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);

			skill.Run(this.Attack(skill, character));
		}

		/// <summary>
		/// Continuously attacks while Cyclone is being channeled.
		/// Every second, consumes 2.5% of maximum HP and adds 2.5% final damage.
		/// </summary>
		private async Task Attack(Skill skill, Character caster)
		{
			var hits = new List<SkillHitInfo>();
			var firstHitDelay = TimeSpan.FromMilliseconds(30);
			var animationTime = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var attackWidth = 50f;
			var delayBetweenAttacks = TimeSpan.FromMilliseconds(200);

			// Cyclone: Ravage
			// Increases the attack area, but reduces attack frequency.
			if (caster.IsAbilityActive(AbilityId.Doppelsoeldner25))
			{
				attackWidth = 75f;
				delayBetweenAttacks = TimeSpan.FromMilliseconds(330);
			}

			var maximumHp = caster.Properties.GetFloat(PropertyName.MHP);

			var hpCost = maximumHp * HpCostRate;
			var consumedHpStacks = 0;

			var hpCostTimer = Stopwatch.StartNew();
			var nextHpCostTime = HpCostInterval;

			try
			{
				await skill.Wait(firstHitDelay);

				while (caster.IsCasting() && !caster.IsDead && caster.Map != null)
				{
					while (hpCostTimer.Elapsed >= nextHpCostTime)
					{
						var currentHp = caster.Properties.GetFloat(PropertyName.HP);

						// Cyclone must not kill its caster.
						if (currentHp - hpCost < 1f)
						{
							return;
						}

						caster.ModifyHp(-hpCost);

						consumedHpStacks++;
						nextHpCostTime += HpCostInterval;
					}

					var splashParameters = skill.GetSplashParameters(caster, caster.Position, caster.Position, length: 0, width: attackWidth, angle: 0);

					var splashArea = skill.GetSplashArea(SplashType.Circle, splashParameters);

					var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

					var cycloneDamageMultiplier = 1f + consumedHpStacks * FinalDamageBonusPerHpCost;

					foreach (var target in targets.LimitBySDR(caster, skill))
					{
						var modifier = SkillModifier.Default;

						modifier.FinalDamageMultiplier = cycloneDamageMultiplier;

						var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

						target.TakeDamage(skillHitResult.Damage, caster);

						var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, animationTime, skillHitDelay);

						skillHit.HitEffect = HitEffect.Impact;
						hits.Add(skillHit);
					}

					if (hits.Count > 0)
						Send.ZC_SKILL_HIT_INFO(caster, hits);

					hits.Clear();

					await skill.Wait(delayBetweenAttacks);
				}
			}
			finally
			{
				this.StopCyclone(caster);
			}
		}

		private void StopCyclone(ICombatEntity caster)
		{
			if (caster == null)
				return;
			caster.StopBuff(BuffId.Cyclone_EnableMovingShot_Buff);
			caster.StopBuff(BuffId.Cyclone_Buff_ImmuneAbil);
			caster.SetAttackState(false);
			if (caster is Character character)
			{
				Send.ZC_OBJECT_PROPERTY(character);
				Send.ZC_MOVE_SPEED(character);
			}
			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
