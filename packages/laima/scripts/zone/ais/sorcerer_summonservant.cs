using System.Collections;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

[Ai("Sorcerer_SummonServant")]
public class SummonServantAiScript : AiScript
{
	private const int MaxChaseDistance = 300;
	private const int MaxMasterDistance = 200;

	protected override void Setup()
	{
		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	protected IEnumerable Idle()
	{
		ResetMoveSpeed();

		var master = GetMaster();
		if (master != null)
		{
			yield return Animation("IDLE");
			yield return Follow(master);
			yield break;
		}

		yield return Wait(4000, 8000);

		SwitchRandom();
		if (Case(80))
		{
			yield return MoveRandom();
		}
		else
		{
			yield return Animation("IDLE");
		}
	}

	protected IEnumerable Attack()
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

	protected IEnumerable StopAndIdle()
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

	private void CheckEnemies()
	{
		var mostHated = GetMostHated();
		if (mostHated != null)
		{
			this._target = mostHated;
			StartRoutine("StopAndAttack", StopAndAttack());
		}
	}

	private void CheckTarget()
	{
		// Transition to idle if the target has vanished or is out of range
		if (EntityGone(_target) || !InRangeOf(_target, MaxChaseDistance) || !IsHating(_target))
		{
			this._target = null;
			StartRoutine("StopAndIdle", StopAndIdle());
		}
	}

	private void CheckMaster()
	{
		if (_target == null)
			return;

		if (!TryGetMaster(out var master))
			return;

		// Reset aggro if the master left
		if (EntityGone(master) || !InRangeOf(master, MaxMasterDistance))
		{
			this._target = null;
			RemoveAllHate();
			StartRoutine("StopAndIdle", StopAndIdle());
		}
	}
}
