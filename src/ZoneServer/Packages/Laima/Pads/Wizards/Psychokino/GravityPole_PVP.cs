using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Psychokino
{
	[Package("laima")]
	[PadHandler(PadName.GravityPole_PVP)]
	public class GravityPole_PVPOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float GravitationalAxisRange = 110f;
		private const int MaximumTargets = 20;
		private const int UpdateIntervalMilliseconds = 500;
		private const float ConeAngleDegrees = 90f;
		private const int PullVelocity = 60;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetUpdateInterval(UpdateIntervalMilliseconds);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);

			var value = MaximumTargets;
			pad.Trigger.MaxActorCount = value;
			pad.Trigger.MaxConcurrentUseCount = value;
			if (creator != null && !creator.IsDead && creator.IsCasting(pad.Skill))
				creator.StartBuff(BuffId.Wizard_SklCasting_Avoid, pad.Skill.Level, 0f, TimeSpan.FromSeconds(5), creator, pad.Skill.Id);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var caster = args.Creator;
			var skill = args.Trigger.Skill;
			if (caster == null || skill == null)
				return;

			if (caster.TryGetBuff(BuffId.Wizard_SklCasting_Avoid, out var protection)
				&& protection.SkillId == skill.Id && protection.Caster == caster)
				caster.RemoveBuff(BuffId.Wizard_SklCasting_Avoid);

			if (skill.Vars.TryGet<float>("Psychokino3_EvasionBonus", out var evasionBonus))
			{
				caster.Properties.Modify(PropertyName.DR_BM, -evasionBonus);
				skill.Vars.Remove("Psychokino3_EvasionBonus");
			}
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || creator == null || skill == null)
				return;

			if (creator.IsDead || pad.Map == null || creator.Map != pad.Map || !creator.IsCasting(skill))
			{
				pad.Destroy();
				return;
			}

			var spPerSecond = skill.Data.BasicSp * 0.2f;
			var spPerTick = spPerSecond * (UpdateIntervalMilliseconds / 1000f);
			if (!creator.TrySpendSp(spPerTick))
			{
				pad.Destroy();
				return;
			}

			var axisStart = creator.Position;
			var axisEnd = creator.Position.GetRelative(creator.Direction, GravitationalAxisRange);

			var hits = new List<SkillHitInfo>();
			// Query the full radius, then filter the cone independently of the original rectangular pad.
			var enemies = pad.Map.GetAttackableEnemiesInPosition(creator, axisStart, GravitationalAxisRange);
			var enemyList = enemies
				.Where(enemy => enemy != null && !enemy.IsDead && creator.IsEnemy(enemy))
				.Where(enemy => this.IsInsideCone(axisStart, axisEnd, enemy.Position))
				.OrderBy(enemy => enemy.Position.Get2DDistance(axisStart))
				.Take(MaximumTargets)
				.ToList();

			foreach (var enemy in enemyList)
			{
				if (enemy == null || enemy.IsDead || !creator.IsEnemy(enemy))
					continue;

				var modifier = new SkillModifier();
				var skillHitResult = SCR_SkillHit(creator, enemy, skill, modifier);
				enemy.TakeDamage(skillHitResult.Damage, creator);

				var skillHit = new SkillHitInfo(creator, enemy, skill, skillHitResult);

				if (enemy is Mob && !enemy.IsDead && skillHitResult.Result != HitResultType.Dodge && enemy.IsKnockdownable())
				{
					var closestPoint = this.GetClosestPointOnLineSegment(axisStart, axisEnd, enemy.Position);
					if (enemy.Position.Get2DDistance(closestPoint) > 1f)
					{
						var pullDirection = enemy.Position.GetDirection(closestPoint);
						var pullFromPos = enemy.Position.GetRelative(pullDirection.Backwards, 80);
						skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, enemy, KnockBackType.KnockBack, PullVelocity, 10);
						skillHit.KnockBackInfo.Speed = 1;
						skillHit.KnockBackInfo.VPow = 1;
						skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
						enemy.ApplyKnockback(creator, skill, skillHit);
					}
				}

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);
		}

		private bool IsInsideCone(Position origin, Position forwardPoint, Position point)
		{
			var dx = point.X - origin.X;
			var dz = point.Z - origin.Z;
			var distanceSquared = dx * dx + dz * dz;

			if (distanceSquared > GravitationalAxisRange * GravitationalAxisRange)
				return false;
			if (distanceSquared <= 0.0001f)
				return true;

			// Vetor direção do conjurador
			var fx = forwardPoint.X - origin.X;
			var fz = forwardPoint.Z - origin.Z;

			// Ângulo em relação à frente (em graus)
			var angleToTarget = Math.Abs(Math.Atan2(dz, dx) - Math.Atan2(fz, fx)) * (180.0 / Math.PI);
			if (angleToTarget > 180.0)
				angleToTarget = 360.0 - angleToTarget;

			// Half-angle do cone de 90° = 45° para cada lado
			return angleToTarget <= (ConeAngleDegrees / 2.0);
		}

		private Position GetClosestPointOnLineSegment(Position lineStart, Position lineEnd, Position point)
		{
			var ABx = lineEnd.X - lineStart.X;
			var ABz = lineEnd.Z - lineStart.Z;

			var APx = point.X - lineStart.X;
			var APz = point.Z - lineStart.Z;

			var dotABAB = ABx * ABx + ABz * ABz;

			if (dotABAB == 0)
				return lineStart;

			var dotAPAB = APx * ABx + APz * ABz;
			var t = dotAPAB / dotABAB;

			t = Math.Max(0, Math.Min(1, t));

			var closestX = lineStart.X + t * ABx;
			var closestZ = lineStart.Z + t * ABz;

			return new Position(closestX, lineStart.Y, closestZ);
		}
	}
}
