using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

[Ai("Bunshin")]
public class BunshinAiScript : AiScript
{
	private const float EnemyRange = 120f;
	private const float AttackRange = 35f;
	private const float FollowRange = 40f;
	private const int FollowSpread = 30;
	private const int AttackDelay = 500;

	private Skill _basicAttack;

	protected override void Setup()
	{
		EnableReturnHome = false;
	}

	protected override void CheckEnemies()
	{
	}

	protected override IEnumerable Idle()
	{
		var master = GetMaster();
		if (master == null || EntityGone(master))
		{
			yield return Wait(200);
			yield break;
		}

		SetRunning(true);

		var enemy = Entity.Map.GetAttackableEnemiesInPosition(Entity, master.Position, EnemyRange).FirstOrDefault();
		if (enemy == null)
		{
			if (!InRangeOf(master, FollowRange))
				yield return MoveTo(master.Position.GetRandomInRange2D(FollowSpread), wait: false);

			yield return Wait(300);
			yield break;
		}

		if (!InRangeOf(enemy, AttackRange))
		{
			yield return MoveTo(enemy.Position, wait: false);
			yield return Wait(200);
			yield break;
		}

		yield return StopMove();

		Entity.TurnTowards(enemy);
		this.Attack(enemy);

		yield return Wait(AttackDelay);
	}

	/// <summary>
	/// Strikes the enemy with the clone's basic attack.
	/// </summary>
	/// <param name="enemy"></param>
	private void Attack(ICombatEntity enemy)
	{
		if (!ZoneServer.Instance.SkillHandlers.TryGetHandler<IMeleeGroundSkillHandler>(SkillId.Normal_Attack, out var handler))
			return;

		_basicAttack ??= new Skill(Entity, SkillId.Normal_Attack);

		handler.Handle(_basicAttack, Entity, Entity.Position, enemy.Position, new List<ICombatEntity> { enemy });
	}
}
