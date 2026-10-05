using System;
using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Handlers.Common;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

[Ai("PC_Summon_Necromancer")]
public class PCSummonNecromancerAiScript : AiScript
{
	private const float EnemyDetectionRange = 180f;
	private const float MaximumEnemyDistanceFromMaster = 250f;
	private const float FollowMasterDistance = 45f;
	private const float TeleportToMasterDistance = 300f;
	private const int SearchIntervalMilliseconds = 100;
	private const int AttackRetryIntervalMilliseconds = 100;
	private const int MinimumAttackDelayMilliseconds = 100;

	protected override void Setup()
	{
		MaxChaseDistance = 250;
		MaxMasterDistance = 250;
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
		SetRunning(true);

		while (!Entity.IsDead)
		{
			var master = GetMaster();

			if (master == null || EntityGone(master) || master.Map != Entity.Map)
			{
				yield return Wait(SearchIntervalMilliseconds);
				continue;
			}

			var distanceToMaster = Entity.Position.Get2DDistance(master.Position);

			if (distanceToMaster > TeleportToMasterDistance)
			{
				Entity.SetPosition(master.Position);
				yield return Wait(SearchIntervalMilliseconds);
				continue;
			}

			if (!IsMasterAttacking(master))
			{
				yield return FollowMaster(master);
				continue;
			}

			var enemy = FindNearestEnemy(master);

			if (enemy != null)
			{
				yield return AttackEnemy(master, enemy);
				continue;
			}

			yield return FollowMaster(master);
		}
	}

	private IEnumerable AttackEnemy(ICombatEntity master, ICombatEntity enemy)
	{
		while (!Entity.IsDead && !EntityGone(master) && IsMasterAttacking(master) && IsValidEnemy(master, enemy))
		{
			if (!TryGetRandomSkill(out var skill))
			{
				yield return Wait(SearchIntervalMilliseconds);
				yield break;
			}

			var attackRange = GetAttackRange(skill);

			if (attackRange <= 0f)
				attackRange = 35f;

			var distanceToEnemy = Entity.Position.Get2DDistance(enemy.Position);

			if (distanceToEnemy > attackRange)
			{
				SetRunning(true);
				yield return MoveTo(enemy.Position, wait: false);
				yield return Wait(50);
				continue;
			}

			yield return StopMove();

			if (!IsValidEnemy(master, enemy))
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

		yield return StopMove();
	}

	private IEnumerable FollowMaster(ICombatEntity master)
	{
		var distanceToMaster = Entity.Position.Get2DDistance(master.Position);

		if (distanceToMaster > FollowMasterDistance)
		{
			SetRunning(true);
			yield return MoveTo(master.Position, wait: false);
			yield return Wait(50);
			yield break;
		}

		yield return Wait(SearchIntervalMilliseconds);
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

		var enemyDistanceFromMaster = master.Position.Get2DDistance(enemy.Position);

		return enemyDistanceFromMaster <= MaximumEnemyDistanceFromMaster;
	}

	private bool IsMasterAttacking(ICombatEntity master)
	{
		if (master == null || master.IsDead)
			return false;

		if (!master.Components.TryGet<CombatComponent>(out var combatComponent))
			return false;

		return combatComponent.AttackState;
	}
}
