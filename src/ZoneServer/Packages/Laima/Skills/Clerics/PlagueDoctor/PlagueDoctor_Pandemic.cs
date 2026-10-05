using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Pandemic.
	/// Spreads debuffs among nearby enemies and applies Panic to affected targets.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_Pandemic)]
	public class PlagueDoctor_Pandemic : IGroundSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int TargetsAtLevelOne = 5;
		private const int AdditionalTargetsPerLevel = 2;
		private const int MaximumRangeAbilityLevel = 5;
		private const float BaseRange = 100f;
		private const float AdditionalRangePerAbilityLevel = 5f;
		private const float SpreadIncinerationChancePerLevel = 5f;
		private static readonly TimeSpan PanicDuration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var maximumTargets = TargetsAtLevelOne + (skillLevel - MinimumSkillLevel) * AdditionalTargetsPerLevel;
			var range = this.GetPandemicRange(character);
			var enemies = caster.Map.GetAttackableEnemiesIn(caster, new Circle(farPos, range)).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).ToList();
			var sourceDebuffs = this.GetSourceDebuffs(enemies);
			var incinerationSource = sourceDebuffs.FirstOrDefault(buff => buff.Id == BuffId.Incineration_Debuff);

			if (sourceDebuffs.Count == 0)
			{
				caster.SetAttackState(false);
				return;
			}

			var affectedTargets = enemies.Where(enemy => this.CanReceiveAnyDebuff(enemy, sourceDebuffs, character, incinerationSource)).Take(maximumTargets).ToList();

			foreach (var enemy in affectedTargets)
			{
				this.SpreadNormalDebuffs(enemy, sourceDebuffs);
				this.TrySpreadIncineration(enemy, incinerationSource, character);
				enemy.StartBuff(BuffId.Panic_Pandemic_Debuff, skillLevel, 0f, PanicDuration, character, skill.Id);
			}

			caster.SetAttackState(false);
		}

		private List<Buff> GetSourceDebuffs(IEnumerable<ICombatEntity> enemies)
		{
			var result = new List<Buff>();
			var registeredIds = new HashSet<BuffId>();

			foreach (var enemy in enemies)
			{
				var buffComponent = enemy.Components.Get<BuffComponent>();

				if (buffComponent == null)
					continue;

				foreach (var debuff in buffComponent.GetList())
				{
					if (!this.CanSpreadDebuff(debuff))
						continue;

					if (registeredIds.Add(debuff.Id))
						result.Add(debuff);
				}
			}

			return result;
		}

		private bool CanSpreadDebuff(Buff debuff)
		{
			if (debuff.Data.Type != BuffType.Debuff)
				return false;

			if (!debuff.Data.RemoveBySkill)
				return false;

			if (debuff.Id == BuffId.Panic_Pandemic_Debuff)
				return false;

			return true;
		}

		private bool CanReceiveAnyDebuff(ICombatEntity target, IEnumerable<Buff> sourceDebuffs, Character caster, Buff incinerationSource)
		{
			if (sourceDebuffs.Any(debuff => debuff.Id != BuffId.Incineration_Debuff && !target.IsBuffActive(debuff.Id)))
				return true;

			if (incinerationSource == null || target.IsBuffActive(BuffId.Incineration_Debuff))
				return false;

			return caster.IsAbilityActive(AbilityId.PlagueDoctor14);
		}

		private void SpreadNormalDebuffs(ICombatEntity target, IEnumerable<Buff> sourceDebuffs)
		{
			foreach (var sourceDebuff in sourceDebuffs)
			{
				if (sourceDebuff.Id == BuffId.Incineration_Debuff)
					continue;

				if (target.IsBuffActive(sourceDebuff.Id))
					continue;

				if (sourceDebuff.RemainingDuration <= TimeSpan.Zero)
					continue;

				target.StartBuff(sourceDebuff.Id, sourceDebuff.NumArg1, sourceDebuff.NumArg2, sourceDebuff.RemainingDuration, sourceDebuff.Caster, sourceDebuff.SkillId, copiedBuff =>
				{
					copiedBuff.NumArg3 = sourceDebuff.NumArg3;
					copiedBuff.NumArg4 = sourceDebuff.NumArg4;
					copiedBuff.NumArg5 = sourceDebuff.NumArg5;

					foreach (var variable in sourceDebuff.Vars.GetList())
						copiedBuff.Vars.Set(variable.Key, variable.Value);
				});
			}
		}

		private void TrySpreadIncineration(ICombatEntity target, Buff incinerationSource, Character caster)
		{
			if (incinerationSource == null)
				return;

			if (target.IsBuffActive(BuffId.Incineration_Debuff))
				return;

			if (!caster.IsAbilityActive(AbilityId.PlagueDoctor14))
				return;

			var abilityLevel = Math.Clamp(caster.Abilities.GetLevel(AbilityId.PlagueDoctor14), 1, 10);
			var spreadChance = abilityLevel * SpreadIncinerationChancePerLevel;

			if (RandomProvider.Get().Next(100) >= spreadChance)
				return;

			if (incinerationSource.RemainingDuration <= TimeSpan.Zero)
				return;

			target.StartBuff(BuffId.Incineration_Debuff, incinerationSource.NumArg1, incinerationSource.NumArg2, incinerationSource.RemainingDuration, incinerationSource.Caster, incinerationSource.SkillId, copiedBuff =>
			{
				copiedBuff.NumArg3 = incinerationSource.NumArg3;
				copiedBuff.NumArg4 = incinerationSource.NumArg4;
				copiedBuff.NumArg5 = incinerationSource.NumArg5;

				foreach (var variable in incinerationSource.Vars.GetList())
					copiedBuff.Vars.Set(variable.Key, variable.Value);
			});
		}

		private float GetPandemicRange(Character caster)
		{
			var abilityLevel = Math.Clamp(caster.Abilities.GetLevel(AbilityId.PlagueDoctor10), 0, MaximumRangeAbilityLevel);
			return BaseRange + abilityLevel * AdditionalRangePerAbilityLevel;
		}
	}
}
