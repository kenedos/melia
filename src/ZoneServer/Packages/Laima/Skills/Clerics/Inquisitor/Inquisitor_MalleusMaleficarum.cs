using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Inquisitor;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Inquisitor
{
	[Package("laima")]
	[SkillHandler(SkillId.Inquisitor_MalleusMaleficarum)]
	public class Inquisitor_MalleusMaleficarum : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumTargets = 7;
		private const int TotalHitCount = 7;
		private const float AttackRange = 140f;
		private const float ConeHalfAngle = 30f;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos, target);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos, targets?.FirstOrDefault());
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
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

			character.SetAttackState(true);
			try
			{
				character.TurnTowards(aimPosition);
				skill.IncreaseOverheat();
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
				var forceId = ForceId.GetNew();
				Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, aimPosition);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, originPos, character.Direction, aimPosition);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, aimPosition, forceId, null);
				var enhanceMultiplier = Inquisitor_MalleusMaleficarumEnhanceAbility.GetDamageMultiplier(character);
				var targets = this.GetTargets(character, originPos, directionX, directionZ);
				var hits = new List<SkillHitInfo>();
				var baseHitsPerTarget = targets.Count > 0 ? TotalHitCount / targets.Count : 0;
				var remainingHits = targets.Count > 0 ? TotalHitCount % targets.Count : 0;
				for (var targetIndex = 0; targetIndex < targets.Count; targetIndex++)
				{
					var currentTarget = targets[targetIndex];
					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= enhanceMultiplier;
					modifier.HitCount = baseHitsPerTarget + (targetIndex < remainingHits ? 1 : 0);
					var result = SCR_SkillHit(character, currentTarget, skill, modifier);
					if (result.Result != HitResultType.Dodge && result.Damage > 0)
					{
						currentTarget.TakeDamage(result.Damage, character);
						currentTarget.StartBuff(BuffId.MalleusMaleficarum_Debuff, skill.Level, 0, DebuffDuration, character, skill.Id);
						Inquisitor_MalleusMaleficarumManaBurnAbility.TryApplySilence(character, currentTarget, skill);
					}
					var hit = new SkillHitInfo(character, currentTarget, skill, result, HitAnimationTime, TimeSpan.Zero);
					hit.ForceId = forceId;
					hits.Add(hit);
				}
				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(character, hits);
			}
			finally
			{
				character.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(character);
			}
		}

		private IList<ICombatEntity> GetTargets(ICombatEntity caster, Position originPosition, float directionX, float directionZ)
		{
			var area = new Circle(originPosition, AttackRange);

			return caster.Map
				.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideCone(target.Position, originPosition, directionX, directionZ))
				.OrderBy(target => originPosition.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();
		}

		private bool IsInsideCone(Position targetPosition, Position originPosition, float directionX, float directionZ)
		{
			var offsetX = targetPosition.X - originPosition.X;
			var offsetZ = targetPosition.Z - originPosition.Z;
			var distance = MathF.Sqrt(offsetX * offsetX + offsetZ * offsetZ);

			if (distance <= 0.001f)
				return true;

			var normalizedX = offsetX / distance;
			var normalizedZ = offsetZ / distance;
			var dot = Math.Clamp(normalizedX * directionX + normalizedZ * directionZ, -1f, 1f);
			var angle = MathF.Acos(dot) * 180f / MathF.PI;

			return angle <= ConeHalfAngle;
		}
	}
}
