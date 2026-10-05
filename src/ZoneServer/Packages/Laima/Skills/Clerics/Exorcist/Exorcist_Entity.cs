using System;
using System.Collections.Generic;
using System.Linq;
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
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_Entity)]
	public class Exorcist_Entity : ISelfSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const float AttackRange = 120f;
		private const int MaximumTargets = 10;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position position, Direction direction)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
			{
				StopSkill(caster);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(character);
				return;
			}

			try
			{
				skill.IncreaseOverheat();
				character.SetAttackState(true);
				character.Direction = direction;

				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, character.Position, character.Direction, Position.Zero);

				var area = new Melia.Zone.Skills.SplashAreas.Circle(character.Position, AttackRange);
				var targets = character.Map.GetAttackableEnemiesIn(character, area)
					.Where(target => target != null && !target.IsDead)
					.OrderByDescending(target => target.IsBuffActiveByKeyword(BuffTag.Cloaking))
					.ThenBy(target => character.Position.Get2DDistance(target.Position))
					.Take(MaximumTargets)
					.ToList();

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					var wasHidden = target.IsBuffActiveByKeyword(BuffTag.Cloaking);
					var hasGregorateMagic = target.IsBuffActive(BuffId.GregorateATK_Buff);

					if (wasHidden)
						target.StopBuffByTag(BuffTag.Cloaking);

					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= Exorcist_EntityEnhanceAbility.GetDamageMultiplier(character);

					if (!wasHidden && !hasGregorateMagic)
						modifier.FinalDamageMultiplier *= Exorcist_EntityDivineImpactAbility.GetNonHiddenDamageMultiplier(character);

					var result = SCR_SkillHit(character, target, skill, modifier);

					if (result.Result != HitResultType.Dodge && result.Damage > 0)
						target.TakeDamage(result.Damage, character);

					hits.Add(new SkillHitInfo(character, target, skill, result, HitAnimationTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(character, hits);
			}
			finally
			{
				StopSkill(character);
			}
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}
	}
}
