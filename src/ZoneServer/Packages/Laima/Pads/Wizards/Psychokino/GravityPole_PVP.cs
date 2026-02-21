using System;
using System.Collections.Generic;
using System.Linq;
using g4;
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
	public class GravityPole_PVPOverride : ICreatePadHandler, IUpdatePadHandler
	{
		private const float GravitationalAxisRange = 110f;
		private const int KnockdownVelocity = 80;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(10000);

			var value = 6;
			pad.Trigger.MaxActorCount = value;
			pad.Trigger.MaxConcurrentUseCount = value;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || creator == null || skill == null)
				return;

			if (!creator.IsCasting(skill))
			{
				pad.Destroy();
				return;
			}

			var spPerSecond = skill.Data.BasicSp * 0.2f;
			var spPerTick = spPerSecond * 0.5f;
			if (!creator.TrySpendSp(spPerTick))
			{
				pad.Destroy();
				return;
			}

			var axisStart = creator.Position;
			var axisEnd = creator.Position.GetRelative(creator.Direction, GravitationalAxisRange);

			var hits = new List<SkillHitInfo>();
			var enemies = pad.Map.GetAttackableEnemiesIn(creator, pad.Area);
			var enemyList = enemies.Take(13);

			foreach (var enemy in enemyList)
			{
				if (enemy == null || enemy.IsDead || !creator.IsEnemy(enemy)) 
					continue;

				var modifier = new SkillModifier();
				var skillHitResult = SCR_SkillHit(creator, enemy, skill, modifier);
				enemy.TakeDamage(skillHitResult.Damage, creator);

				var skillHit = new SkillHitInfo(creator, enemy, skill, skillHitResult);

				if (enemy.IsKnockdownable())
				{
					var closestPoint = this.GetClosestPointOnLineSegment(axisStart, axisEnd, enemy.Position);
					var pullDirection = enemy.Position.GetDirection(closestPoint);
					var pullFromPos = enemy.Position.GetRelative(pullDirection.Backwards, 10);

					var velocity = Math.Min(150,(int)enemy.Position.Get2DDistance(closestPoint) * 5 + 50);
					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPos, enemy, HitType.KnockDown, velocity, 60);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.Type = HitType.KnockDown;

					enemy.ApplyKnockdown(creator, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);
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
