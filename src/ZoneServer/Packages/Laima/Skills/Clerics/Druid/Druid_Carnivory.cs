using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_Carnivory)]
	public class Druid_Carnivory : IGroundSkillHandler, IDynamicCasted
	{
		private const float AttackRange = 100f;
		private const int MaximumTargets = 6;
		private const int BaseDurationSeconds = 10;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
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

			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var area = new Melia.Zone.Skills.SplashAreas.Circle(character.Position, AttackRange);
			var targets = character.Map.GetAttackableEnemiesIn(character, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => character.Position.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();

			var durationAbilityLevel = Druid_CarnivoryDurationTimeAbility.GetLevel(character);
			var duration = TimeSpan.FromSeconds(BaseDurationSeconds + durationAbilityLevel);

			foreach (var target in targets)
				target.StartBuff(BuffId.Carnivory_Debuff, skill.Level, 0, duration, character, skill.Id);

			character.SetAttackState(false);
		}
	}
}
