using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Zealot
{
	[Package("laima")]
	[SkillHandler(SkillId.Zealot_BlindFaith)]
	public class Zealot_BlindFaith : ISelfSkillHandler
	{
		private const float MaximumSpConsumptionRate = 0.05f;
		private const float SkillRange = 60f;
		private const int HitCount = 10;
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction direction)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var maximumSp = Math.Max(0f, character.Properties.GetFloat(PropertyName.MSP));
			var spCost = Math.Max(1f, maximumSp * MaximumSpConsumptionRate);

			if (!character.TrySpendSp(spCost))
			{
				character.SetAttackState(false);
				return;
			}

			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, character.Position, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var area = new CircleF(character.Position, SkillRange);
			var targets = character.Map.GetAttackableEnemiesIn(character, area).Where(target => target != null && !target.IsDead).ToList();
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
				{
					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= this.GetEnhanceMultiplier(character) / HitCount;

					var result = SCR_SkillHit(character, target, skill, modifier);
					target.TakeDamage(result.Damage, character);

					if (character.IsAbilityActive(AbilityId.Zealot7) && result.Result == HitResultType.Crit)
						target.StartBuff(BuffId.BlindFaith_Debuff, 1, 0.10f, TimeSpan.FromSeconds(5), character, skill.Id);

					var hitDelay = TimeSpan.FromMilliseconds(hitIndex * 80);
					hits.Add(new SkillHitInfo(character, target, skill, result, HitAnimationTime, hitDelay));
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(character, hits.ToArray());

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private float GetEnhanceMultiplier(Character character)
		{
			var abilityLevel = Math.Min(character.Abilities.GetLevel(AbilityId.Zealot17), MaximumEnhanceLevel);

			if (abilityLevel <= 0)
				return 1f;

			var enhancePercent = abilityLevel * 0.5f;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhancePercent += 10f;

			return 1f + enhancePercent / 100f;
		}
	}
}
