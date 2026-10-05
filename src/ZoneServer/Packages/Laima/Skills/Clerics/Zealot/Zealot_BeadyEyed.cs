using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Zealot
{
	[Package("laima")]
	[SkillHandler(SkillId.Zealot_BeadyEyed)]
	public class Zealot_BeadyEyed : IGroundSkillHandler
	{
		private const float DistanceBehindTarget = 15f;
		private static readonly TimeSpan AbilityDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			var target = this.FindTarget(character, skill, packetTarget);

			if (target == null)
			{
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			var destination = target.Position.GetRelative(target.Direction.Backwards, DistanceBehindTarget);

			character.TurnTowards(target.Position);
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, target.Position);

			character.SetPosition(destination);
			character.TurnTowards(target.Position);

			if (character.IsAbilityActive(AbilityId.Zealot5))
				character.StartBuff(BuffId.BeadyEyed_Buff, 20, 0, AbilityDuration, character, skill.Id);

			if (character.IsAbilityActive(AbilityId.Zealot8))
				character.StartBuff(BuffId.BeadyEyed_Debuff, 0.15f, 0, AbilityDuration, character, skill.Id);

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private ICombatEntity FindTarget(Character caster, Skill skill, ICombatEntity packetTarget)
		{
			var targets = caster.Map
				.GetAttackableEnemiesInPosition(caster, caster.Position, skill.Data.MaxRange)
				.Where(target => target != null && !target.IsDead)
				.ToList();

			if (packetTarget != null && targets.Contains(packetTarget))
				return packetTarget;

			return targets
				.OrderBy(target => caster.Position.Get2DDistance(target.Position))
				.FirstOrDefault();
		}
	}
}
