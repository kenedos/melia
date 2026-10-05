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

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_Judgment)]
	public class Inquisitor_Judgment : IGroundSkillHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int ProvokeHateAmount = 10;
		private const float ProvokeRange = 150f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);
		private static readonly TimeSpan ProvokeDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			character.StartBuff(BuffId.Judgment_Buff, skillLevel, 0, BuffDuration, character, skill.Id);

			if (character.IsAbilityActive(AbilityId.Inquisitor15))
				this.ProvokeNearbyEnemies(character, skill);

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private void ProvokeNearbyEnemies(Character character, Skill skill)
		{
			if (character.Map == null)
				return;

			var area = new CircleF(character.Position, ProvokeRange);
			var targets = character.Map
				.GetAttackableEnemiesIn(character, area)
				.Where(target => target != null && !target.IsDead)
				.ToList();

			foreach (var target in targets)
			{
				target.InsertHate(character, ProvokeHateAmount);
				target.StartBuff(BuffId.Judgment_Provoke_Debuff, 1, 0, ProvokeDuration, character, skill.Id);
			}
		}
	}
}
