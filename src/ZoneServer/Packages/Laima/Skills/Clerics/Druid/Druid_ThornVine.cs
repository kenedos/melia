using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_ThornVine)]
	public class Druid_ThornVine : IGroundSkillHandler, IDynamicCasted
	{
		private const float AttackRange = 140f;
		private const float AttackHalfAngle = 35f;
		private const int MaximumTargets = 10;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(3);

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

			var aimPosition = selectedTarget != null && !selectedTarget.IsDead ? selectedTarget.Position : farPos;
			var directionX = aimPosition.X - originPos.X;
			var directionZ = aimPosition.Z - originPos.Z;
			var directionLength = MathF.Sqrt(directionX * directionX + directionZ * directionZ);

			if (directionLength <= 0.001f)
			{
				directionX = character.Direction.Cos;
				directionZ = character.Direction.Sin;
			}
			else
			{
				directionX /= directionLength;
				directionZ /= directionLength;
			}

			character.TurnTowards(aimPosition);
			character.SetAttackState(true);
			skill.IncreaseOverheat();

			Send.ZC_SKILL_READY(character, skill, 1, originPos, aimPosition);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, aimPosition);

			var area = new Melia.Zone.Skills.SplashAreas.Circle(originPos, AttackRange);
			var targets = character.Map.GetAttackableEnemiesIn(character, area)
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideCone(target.Position, originPos, directionX, directionZ))
				.OrderBy(target => originPos.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();

			foreach (var target in targets)
				target.StartBuff(BuffId.ThornVine_Debuff, skill.Level, 0, DebuffDuration, character, skill.Id);

			character.SetAttackState(false);
		}

		private bool IsInsideCone(Position targetPosition, Position originPosition, float directionX, float directionZ)
		{
			var targetX = targetPosition.X - originPosition.X;
			var targetZ = targetPosition.Z - originPosition.Z;
			var targetLength = MathF.Sqrt(targetX * targetX + targetZ * targetZ);

			if (targetLength <= 0.001f)
				return true;

			targetX /= targetLength;
			targetZ /= targetLength;

			var dot = Math.Clamp(directionX * targetX + directionZ * targetZ, -1f, 1f);
			var angle = MathF.Acos(dot) * 180f / MathF.PI;
			return angle <= AttackHalfAngle;
		}
	}
}
