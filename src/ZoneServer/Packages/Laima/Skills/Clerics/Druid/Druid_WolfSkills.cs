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
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	public abstract class Druid_WolfSkillHandler : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var range = this.GetRange(skill.Id);
			var maximumTargets = this.GetMaximumTargets(skill.Id);
			var area = new Melia.Zone.Skills.SplashAreas.Circle(character.Position, range);
			var targets = character.Map.GetAttackableEnemiesIn(character, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => character.Position.Get2DDistance(target.Position))
				.Take(maximumTargets)
				.ToList();

			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var result = SCR_SkillHit(character, target, skill, SkillModifier.Default);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
					target.TakeDamage(result.Damage, character);

				hits.Add(new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(character, hits);

			character.SetAttackState(false);
		}

		private float GetRange(SkillId skillId)
		{
			return skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_1 ? 45f : 100f;
		}

		private int GetMaximumTargets(SkillId skillId)
		{
			return skillId == SkillId.Mon_pcskill_boss_werewolf_Skill_4 ? 7 : 3;
		}
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_1)]
	public class Druid_WolfScratch : Druid_WolfSkillHandler
	{
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_3)]
	public class Druid_WolfSlash : Druid_WolfSkillHandler
	{
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_4)]
	public class Druid_WolfWildBreath : Druid_WolfSkillHandler
	{
	}

	[Package("laima")]
	[SkillHandler(SkillId.Mon_pcskill_boss_werewolf_Skill_5)]
	public class Druid_WolfWarcry : Druid_WolfSkillHandler
	{
	}

	[Package("laima")]
	[SkillHandler(SkillId.Lycan_Half_Attack)]
	public class Druid_LycanHalfAttack : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var area = new Melia.Zone.Skills.SplashAreas.Circle(character.Position, 100f);
			var target = character.Map.GetAttackableEnemiesIn(character, area)
				.Where(candidate => candidate != null && !candidate.IsDead)
				.OrderBy(candidate => character.Position.Get2DDistance(candidate.Position))
				.FirstOrDefault();

			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			if (target != null)
			{
				var result = SCR_SkillHit(character, target, skill, SkillModifier.Default);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
				{
					target.TakeDamage(result.Damage, character);

					if (Random.Shared.Next(1, 101) <= 5)
						target.StartBuff(BuffId.HeavyBleeding, 1, result.Damage, TimeSpan.FromSeconds(10), character, skill.Id);
				}

				var hit = new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero);
				Send.ZC_SKILL_HIT_INFO(character, new[] { hit });
			}

			character.SetAttackState(false);
		}
	}
}
