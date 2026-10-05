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
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Zealot
{
	[Package("laima")]
	[SkillHandler(SkillId.Zealot_EmphasisTrust)]
	public class Zealot_EmphasisTrust : IGroundSkillHandler
	{
		private const float SkillRange = 100f;
		private const int MaximumTargets = 10;
		private const int MaximumTriggers = 10;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				return;
			}

			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var skillLevel = Math.Clamp(skill.Level, 1, 10);
			var duration = TimeSpan.FromSeconds(17 + (skillLevel - 1) * 2);
			var area = new CircleF(character.Position, SkillRange);
			var targets = character.Map.GetAttackableEnemiesIn(character, area).Where(target => target != null && !target.IsDead).OrderBy(target => character.Position.Get2DDistance(target.Position)).Take(MaximumTargets).ToList();

			foreach (var target in targets)
				target.StartBuff(BuffId.EmphasisTrust_Debuff, skillLevel, MaximumTriggers, duration, character, skill.Id);

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}
	}
}
