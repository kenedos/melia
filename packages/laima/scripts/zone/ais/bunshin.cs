using System;
using System.Collections;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Handlers.Common;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

[Ai("Bunshin")]
public class BunshinAiScript : AiScript
{
	private const float SideOffset = 30f;
	private const float BackOffset = 20f;
	private const float MoveThreshold = 8f;
	private const float RepositionDistance = 90f;
	private const float EnemyDetectionRange = 120f;
	private const int MaximumEnemyDistanceFromMaster = 160;
	private const float BasicAttackRange = 35f;
	private const float RunSpeedMultiplier = 2f;

	protected override void Setup()
	{
		MaxChaseDistance = MaximumEnemyDistanceFromMaster;
		MaxMasterDistance = MaximumEnemyDistanceFromMaster;
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

			if (master == null || EntityGone(master))
			{
				yield return Wait(100);
				continue;
			}

			UpdateRunSpeed(master);

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
		var basicAttack = GetOrCreateSkill(SkillId.Normal_Attack);

		if (basicAttack == null)
		{
			yield return Wait(250);
			yield break;
		}

		while (!Entity.IsDead && !EntityGone(master) && IsValidEnemy(master, enemy))
		{
			UpdateRunSpeed(master);

			var distanceToEnemy = Entity.Position.Get2DDistance(enemy.Position);

			if (distanceToEnemy > BasicAttackRange)
			{
				SetRunning(true);
				yield return MoveTo(enemy.Position, wait: false);
				yield return Wait(50);
				continue;
			}

			yield return StopMove();

			Entity.TurnTowards(enemy.Position);

			if (!CanUseSkill(basicAttack, enemy))
			{
				yield return Wait(100);
				continue;
			}

			var originPosition = Entity.Position;
			var targetPosition = enemy.Position;

			new MeleeGroundSkillHandler().Handle(
				basicAttack,
				Entity,
				originPosition,
				targetPosition,
				enemy
			);

			var skillSpeedRate = basicAttack.Properties.GetFloat(PropertyName.SklSpdRate);

			if (skillSpeedRate <= 0)
				skillSpeedRate = 1f;

			var attackDelay = Math.Max(100, (int)(330f / skillSpeedRate));

			yield return Wait(attackDelay);
		}
	}

	private IEnumerable FollowMaster(ICombatEntity master)
	{
		var cloneIndex = 1;

		if (Entity is DummyCharacter dummy)
			cloneIndex = dummy.Variables.Temp.GetInt("Bunshin.Index");

		var sideOffset = cloneIndex == 1 ? -SideOffset : SideOffset;

		var targetPosition = new Position(
			master.Position.X + sideOffset,
			master.Position.Y,
			master.Position.Z + BackOffset
		);

		var distanceToMaster = Entity.Position.Get2DDistance(master.Position);
		var distanceToTarget = Entity.Position.Get2DDistance(targetPosition);

		if (distanceToMaster > RepositionDistance)
		{
			Entity.SetPosition(targetPosition);
			yield return Wait(50);
			yield break;
		}

		if (distanceToTarget > MoveThreshold)
		{
			SetRunning(true);
			yield return MoveTo(targetPosition, wait: false);
		}

		yield return Wait(50);
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

	private void UpdateRunSpeed(ICombatEntity master)
	{
		var masterMoveSpeed = master.Properties.GetFloat(PropertyName.MSPD);
		var cloneRunSpeed = Math.Max(1f, masterMoveSpeed * RunSpeedMultiplier);

		SetRunning(true);
		SetFixedMoveSpeed(cloneRunSpeed);
	}
}
