using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	[Package("laima")]
	[SkillHandler(SkillId.Sage_DimensionCompression)]
	public class Sage_DimensionCompressionOverride : IGroundSkillHandler
	{
		private const float AreaRadius = 180f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var targetPosition = character.Position;

			if (!character.InSkillUseRange(skill, targetPosition))
				return;

			if (!character.TrySpendSp(skill))
				return;

			var duration = GetDuration(skill);
			var pad = new Pad(PadName.Sage_DimensionCompression_Abil, character, skill, new CircleF(targetPosition, AreaRadius));
			pad.Position = targetPosition;
			pad.Direction = character.Direction;
			pad.Trigger.LifeTime = duration;
			pad.Trigger.MaxActorCount = 15;
			pad.Trigger.MaxConcurrentUseCount = 15;

			character.SetAttackState(true);
			character.TurnTowards(targetPosition);

			var origin = character.Position;
			Send.ZC_SKILL_READY(character, skill, origin, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(character, 0, origin, origin.GetDirection(targetPosition), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition);

			character.Map.AddPad(pad);
			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private static TimeSpan GetDuration(Skill skill)
		{
			var level = Math.Clamp(skill.Level, 1, 10);
			return TimeSpan.FromSeconds(4.0 + 0.4 * level);
		}
	}
}
