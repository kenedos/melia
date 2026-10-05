using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.Packages.Laima.Abilities.Wizards.Sage;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	[Package("laima")]
	[SkillHandler(SkillId.Sage_UltimateDimension)]
	public class Sage_UltimateDimensionOverride : IGroundSkillHandler
	{
		private const float AreaRadius = 120f;
		private const int BaseHitCount = 5;
		private const int MaximumTargets = 15;
		private const int HitIntervalMilliseconds = 300;
		private const int AfterEffectsDurationMilliseconds = 5000;
		private const int ConfusionDurationMilliseconds = 6000;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var targetPosition = GetTargetPosition(skill, farPos);

			if (!character.InSkillUseRange(skill, targetPosition))
				return;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetAttackState(true);
			character.TurnTowards(targetPosition);
			skill.IncreaseOverheat();

			if (character.TryGetActiveAbility(AbilityId.Sage9, out _))
				Melia.Zone.Pads.HandlersOverride.Wizards.Sage.Sage_HoleOfDarknessOverride.EnlargeNearby(character, targetPosition, AreaRadius);

			var origin = character.Position;

			Send.ZC_SKILL_READY(character, skill, origin, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(character, 0, origin, origin.GetDirection(targetPosition), Position.Zero);

			if (Versions.Protocol > 500)
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition, new List<SkillHitInfo>());
			else
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition);

			skill.Run(AttackArea(character, skill, targetPosition));

			character.SetAttackState(false);
		}

		private static Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
		}

		private static async Task AttackArea(Character character, Skill skill, Position center)
		{
			var damageMultiplier = Sage_UltimateDimensionEnhanceAbility.GetDamageMultiplier(character);
			var hasAfterEffects = character.TryGetActiveAbility(AbilityId.Sage12, out _);

			if (character.IsDead || character.Map == null)
				return;

			var map = character.Map;

			var targets = map
				.GetAttackableEnemiesIn(character, new CircleF(center, AreaRadius))
				.Where(target => !target.IsDead && target.Map == map)
				.Distinct()
				.OrderBy(target => target.Position.Get2DDistance(center))
				.Take(MaximumTargets)
				.ToList();

			var afterEffectsDamage = new Dictionary<ICombatEntity, float>();

			for (var hitIndex = 0; hitIndex < BaseHitCount; hitIndex++)
			{
				if (character.IsDead || character.Map != map)
					return;

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					if (target.IsDead || target.Map != map)
						continue;

					var result = SCR_SkillHit(character, target, skill, SkillModifier.MultiHit(1));
					result.Damage *= damageMultiplier;

					if (result.Result != HitResultType.Dodge && result.Damage > 0f)
					{
						target.TakeDamage(result.Damage, character);

						if (!afterEffectsDamage.ContainsKey(target))
							afterEffectsDamage[target] = result.Damage;

						if (!target.IsDead)
						{
							target.AddState(StateType.Stunned, TimeSpan.FromSeconds(3));
							target.StartBuff(BuffId.Confuse, skill.Level, 0, TimeSpan.FromMilliseconds(ConfusionDurationMilliseconds), character, skill.Id);
						}
					}

					hits.Add(new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(character, hits);

				if (hitIndex < BaseHitCount - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(HitIntervalMilliseconds));
			}

			if (!hasAfterEffects)
				return;

			foreach (var entry in afterEffectsDamage)
			{
				var target = entry.Key;

				if (target.IsDead || target.Map != map)
					continue;

				target.StartBuff(BuffId.UltimateDimension_Debuff, skill.Level, entry.Value, TimeSpan.FromMilliseconds(AfterEffectsDurationMilliseconds), character, skill.Id);
			}
		}
	}
}
