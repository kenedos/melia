using System;
using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Handlers.Common;
using Melia.Zone.World.Actors;

[Ai("PC_Summon_Holding")]
public class PCSummonHoldingAiScript : AiScript
{
	private const float EnemyDetectionRange = 150f;
	private const int SearchIntervalMilliseconds = 150;
	private const int AttackRetryIntervalMilliseconds = 100;
	private const int MinimumAttackDelayMilliseconds = 100;

	protected override void Setup()
	{
		MaxChaseDistance = 150;
		EnableReturnHome = false;
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	protected override void CheckEnemies()
	{
	}

	protected override IEnumerable Idle()
	{
		while (!Entity.IsDead)
		{
			var master = GetMaster();

			if (master == null || EntityGone(master) || master.Map != Entity.Map)
			{
				yield return Wait(SearchIntervalMilliseconds);
				continue;
			}

			var enemy = FindNearestEnemy(master);

			if (enemy == null)
			{
				yield return Wait(SearchIntervalMilliseconds);
				continue;
			}

			yield return AttackEnemy(master, enemy);
		}
	}

	private IEnumerable AttackEnemy(ICombatEntity master, ICombatEntity enemy)
	{
		while (!Entity.IsDead && !EntityGone(master) && IsValidEnemy(master, enemy))
		{
			if (!TryGetRandomSkill(out var skill))
			{
				yield return Wait(SearchIntervalMilliseconds);
				yield break;
			}

			if (!InRangeOf(enemy, GetAttackRange(skill)))
				yield break;

			Entity.TurnTowards(enemy.Position);

			if (!CanUseSkill(skill, enemy))
			{
				yield return Wait(AttackRetryIntervalMilliseconds);
				continue;
			}

			var originPosition = Entity.Position;
			var targetPosition = enemy.Position;

			new MeleeGroundSkillHandler().Handle(skill, Entity, originPosition, targetPosition, enemy);

			var skillSpeedRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);

			if (skillSpeedRate <= 0f)
				skillSpeedRate = 1f;

			var attackDelay = Math.Max(MinimumAttackDelayMilliseconds, (int)(650f / skillSpeedRate));

			yield return Wait(attackDelay);
		}
	}

	private ICombatEntity FindNearestEnemy(ICombatEntity master)
	{
		if (Entity.Map == null)
			return null;

		return Entity.Map
			.GetAttackableEnemiesInPosition(Entity, Entity.Position, EnemyDetectionRange)
			.Where(enemy => IsValidEnemy(master, enemy))
			.OrderBy(enemy => Entity.Position.Get2DDistance(enemy.Position))
			.FirstOrDefault();
	}

	private bool IsValidEnemy(ICombatEntity master, ICombatEntity enemy)
	{
		if (enemy == null || enemy.IsDead || EntityGone(enemy))
			return false;

		if (enemy.Map != Entity.Map || master.Map != Entity.Map)
			return false;

		return Entity.Position.Get2DDistance(enemy.Position) <= EnemyDetectionRange;
	}
}
