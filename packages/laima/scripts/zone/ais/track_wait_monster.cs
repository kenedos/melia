using System.Collections;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

[Ai("TrackWaitMonster")]
public class TrackWaitMonsterAiScript : AiScript
{

	protected override void Setup()
	{
		this.MaxChaseDistance = 1500;
		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	protected override IEnumerable Idle()
	{
		yield return Animation("IDLE");
	}

	protected override IEnumerable Attack()
	{
		SetRunning(true);

		while (!_target.IsDead)
		{
			if (!TryGetRandomSkill(out var skill))
			{
				continue;
			}

			yield return MoveToAttack(_target, GetAttackRange(skill));

			if (InRangeOf(_target, GetAttackRange(skill)))
				yield return UseSkill(skill, _target);
		}

		yield break;
	}

	protected override IEnumerable StopAndIdle()
	{
		yield return StopMove();
		StartRoutine("Idle", Idle());
	}

	protected IEnumerable StopAndAttack()
	{
		ExecuteOnce(Emoticon("I_emo_exclamation"));
		ExecuteOnce(TurnTowards(_target));

		yield return StopMove();
		StartRoutine("Attack", Attack());
	}
}
