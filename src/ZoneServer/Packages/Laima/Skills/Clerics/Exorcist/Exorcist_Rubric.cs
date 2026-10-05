using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_Rubric)]
	public class Exorcist_Rubric : IGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const float AttackRange = 100f;
		private const int BaseMaximumTargets = 5;
		private const int TotalDamageCycles = 8;
		private const int NormalCycleMilliseconds = 500;
		private const int SpeedReadingCycleMilliseconds = 250;
		private const int SlowGraceMilliseconds = 250;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			this.StopChanneling(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			this.StopChanneling(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			skill.Run(this.ExecuteRubric(skill, character));
		}

		private async Task ExecuteRubric(Skill skill, Character caster)
		{
			try
			{
				var speedReading = caster.IsAbilityActive(AbilityId.Exorcist3);
				var cycleMilliseconds = speedReading ? SpeedReadingCycleMilliseconds : NormalCycleMilliseconds;

				for (var cycle = 0; cycle < TotalDamageCycles; cycle++)
				{
					if (caster.IsDead || caster.Map == null)
						break;

					this.ExecuteDamageCycle(skill, caster, cycleMilliseconds);

					if (cycle + 1 < TotalDamageCycles)
						await skill.Wait(TimeSpan.FromMilliseconds(cycleMilliseconds));
				}
			}
			finally
			{
				this.StopChanneling(caster);
			}
		}

		private void ExecuteDamageCycle(Skill skill, Character caster, int cycleMilliseconds)
		{
			var maximumTargets = this.GetMaximumTargets(caster);
			var area = new Melia.Zone.Skills.SplashAreas.Circle(caster.Position, AttackRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
					.Where(target => target != null && !target.IsDead)
					.OrderBy(target => caster.Position.Get2DDistance(target.Position))
					.Take(maximumTargets)
					.ToList();

			if (targets.Count == 0)
				return;

			var grandCrossInRange = this.HasGrandCrossInRange(caster);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= Exorcist_RubricEnhanceAbility.GetDamageMultiplier(caster);

				if (grandCrossInRange || this.IsDemon(target))
					modifier.HitCount = 3;

				var result = SCR_SkillHit(caster, target, skill, modifier);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
				{
					target.TakeDamage(result.Damage, caster);
					target.StartBuff(BuffId.Rubric_DeBuff, skill.Level, 0, TimeSpan.FromMilliseconds(cycleMilliseconds + SlowGraceMilliseconds), caster, skill.Id);
				}

				hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private int GetMaximumTargets(Character caster)
		{
			var maximumTargets = BaseMaximumTargets;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Exorcist2, out var abilityLevel))
				maximumTargets += Math.Clamp(abilityLevel, 1, 5);

			return maximumTargets;
		}

		private bool HasGrandCrossInRange(Character caster)
		{
			return caster.Map.GetActorsInRange<Pad>(caster.Position, AttackRange, pad =>
					pad != null &&
					!pad.IsDead &&
					pad.Skill != null &&
					pad.Skill.Id == SkillId.Exorcist_Koinonia
			).Any();
		}

		private bool IsDemon(ICombatEntity target)
		{
			if (!Enum.TryParse<RaceType>(target.Properties.GetString(PropertyName.RaceType), true, out var race))
				return false;

			return race == RaceType.Velnias;
		}

		private void StopChanneling(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}
	}
}
