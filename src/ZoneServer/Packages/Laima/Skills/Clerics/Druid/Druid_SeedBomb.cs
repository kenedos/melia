using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Druid
{
	[Package("laima")]
	[SkillHandler(SkillId.Druid_Seedbomb)]
	public class Druid_SeedBomb : IGroundSkillHandler, IDynamicCasted
	{
		private const int MaximumDebuffTargets = 10;
		private const float ApplicationRange = 120f;
		private const float ApplicationHalfAngle = 35f;
		private static readonly TimeSpan SeedDuration = TimeSpan.FromSeconds(2);

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

			var area = new Melia.Zone.Skills.SplashAreas.Circle(originPos, ApplicationRange);
			var targets = character.Map.GetAttackableEnemiesIn(character, area)
				.Where(target => target != null && !target.IsDead)
				.Where(target => this.IsInsideCone(target.Position, originPos, directionX, directionZ))
				.OrderBy(target => originPos.Get2DDistance(target.Position))
				.Take(MaximumDebuffTargets)
				.ToList();

			foreach (var target in targets)
			{
				if (target.TryGetBuff(BuffId.Seedbomb_Buff, out var existingSeed))
					Druid_SeedBombHelper.Trigger(existingSeed, character, skill, 2);
				else
					target.StartBuff(BuffId.Seedbomb_Buff, skill.Level, 0, SeedDuration, character, skill.Id);
			}

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
			return angle <= ApplicationHalfAngle;
		}
	}

	public static class Druid_SeedBombHelper
	{
		public const string DetonatedVariable = "Druid.SeedBomb.Detonated";
		private const float ExplosionRange = 100f;
		private const int MaximumExplosionTargets = 10;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public static void Trigger(Melia.Zone.Buffs.Buff seedBuff, Character caster, Skill skill, int hitCount)
		{
			if (seedBuff == null || seedBuff.Target == null || seedBuff.Target.IsDead || caster == null || caster.IsDead)
				return;

			if (seedBuff.Vars.GetInt(DetonatedVariable) != 0)
				return;

			seedBuff.Vars.SetInt(DetonatedVariable, 1);
			var explosionPosition = seedBuff.Target.Position;
			seedBuff.Target.RemoveBuff(seedBuff.Id);
			Detonate(caster, skill, explosionPosition, hitCount);
		}

		public static void Detonate(Character caster, Skill skill, Position explosionPosition, int hitCount)
		{
			if (caster == null || caster.IsDead || caster.Map == null || skill == null)
				return;

			var area = new Melia.Zone.Skills.SplashAreas.Circle(explosionPosition, ExplosionRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => explosionPosition.Get2DDistance(target.Position))
				.Take(MaximumExplosionTargets)
				.ToList();

			var enhanceMultiplier = Druid_SeedBombEnhanceAbility.GetDamageMultiplier(caster);
			var hits = new List<SkillHitInfo>();

			for (var hitIndex = 0; hitIndex < Math.Max(1, hitCount); hitIndex++)
			{
				foreach (var target in targets)
				{
					var modifier = SkillModifier.Default;
					modifier.DamageMultiplier *= enhanceMultiplier;

					var result = SCR_SkillHit(caster, target, skill, modifier);

					if (result.Result != HitResultType.Dodge && result.Damage > 0)
						target.TakeDamage(result.Damage, caster);

					var hitDelay = TimeSpan.FromMilliseconds(hitIndex * 100);
					hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, hitDelay));
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
