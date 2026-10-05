using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	/// <summary>
	/// Gohei.
	/// Deals Holy magic damage, removes removable enemy buffs and
	/// removable allied debuffs, and applies the mental effects.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Miko_Gohei)]
	public class Miko_Gohei : IGroundSkillHandler
	{
		private const int HitCount = 2;
		private const float MaximumCastRange = 100f;
		private const float EnemyEffectRadius = 10f;
		private const float AllyEffectRadius = 100f;
		private const float EnhancePerLevel = 0.005f;
		private static readonly TimeSpan MentalEffectDuration = TimeSpan.FromMinutes(1);
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(450);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(50);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(50);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (originPos.Get2DDistance(farPos) > MaximumCastRange)
			{
				character.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Execute(skill, character, farPos));
		}

		private async Task Execute(Skill skill, Character caster, Position farPos)
		{
			await skill.Wait(FirstHitDelay);

			var enemyArea = new Circle(farPos, EnemyEffectRadius);

			var enemies = caster.Map
				.GetAttackableEnemiesIn(caster, enemyArea)
				.Where(enemy => enemy != null && !enemy.IsDead)
				.ToList();

			foreach (var enemy in enemies)
			{
				this.RemoveRemovableBuffs(enemy);

				enemy.StartBuff(
					BuffId.MentalCollapse_Debuff,
					skill.Level,
					caster.Handle,
					MentalEffectDuration,
					caster,
					skill.Id);

				if (enemy.TryGetBuff(BuffId.MentalCollapse_Debuff, out var mentalCollapse))
					mentalCollapse.Vars.Set("Gohei.SkillLevel", skill.Level);
			}

			this.ApplyMentalRecovery(caster, caster, skill);

			var allies = caster.Map
				.GetCharacters(character =>
					character != null &&
					!character.IsDead &&
					character.Handle != caster.Handle &&
					character.Layer == caster.Layer &&
					!caster.IsEnemy(character) &&
					caster.Position.Get2DDistance(character.Position) <= AllyEffectRadius)
				.ToList();

			foreach (var ally in allies)
				this.ApplyMentalRecovery(ally, caster, skill);

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				foreach (var enemy in enemies)
				{
					if (enemy == null || enemy.IsDead)
						continue;

					var skillHitResult = SCR_SkillHit(caster, enemy, skill);

					this.ApplyEnhanceAbility(caster, skillHitResult);

					enemy.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(
						caster,
						enemy,
						skill,
						skillHitResult,
						HitAnimationTime,
						TimeSpan.Zero);

					Send.ZC_SKILL_FORCE_TARGET(caster, enemy, skill, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			caster.SetAttackState(false);
		}

		private void ApplyMentalRecovery(Character target, Character caster, Skill skill)
		{
			this.RemoveRemovableDebuffs(target);

			target.StartBuff(
				BuffId.MentalRecovery_Buff,
				skill.Level,
				caster.Handle,
				MentalEffectDuration,
				caster,
				skill.Id);

			if (target.TryGetBuff(BuffId.MentalRecovery_Buff, out var mentalRecovery))
				mentalRecovery.Vars.Set("Gohei.SkillLevel", skill.Level);
		}

		private void RemoveRemovableBuffs(ICombatEntity target)
		{
			target.Components.Get<BuffComponent>()?.RemoveAll(buff => buff.Data.Type == BuffType.Buff && buff.Data.RemoveBySkill);
		}

		private void RemoveRemovableDebuffs(ICombatEntity target)
		{
			target.Components.Get<BuffComponent>()?.RemoveAll(buff => buff.Data.Type == BuffType.Debuff && buff.Data.RemoveBySkill);
		}

		private void ApplyEnhanceAbility(Character character, SkillHitResult skillHitResult)
		{
			if (!character.Abilities.TryGet(AbilityId.Miko1, out var ability) || !ability.Active)
				return;

			var abilityLevel = Math.Min(ability.Level, 100);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
