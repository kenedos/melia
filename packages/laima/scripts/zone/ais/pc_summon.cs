using System.Collections;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;

[Ai("PC_Summon")]
public class PCSummonAiScript : AiScript
{
	protected override void Setup()
	{
		MaxChaseDistance = 200;

		During("Idle", ResetDistantHateDuringIdle);
		During("Idle", CheckEnemiesEnhanced);
		During("Idle", TeleportToMasterIfTooFar);

		During("Attack", TeleportToMasterIfTooFar);
		During("Attack", CheckTargetSummon);
		During("Attack", CheckMasterSummon);
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
			yield return DispersedIdle();
			yield break;
		}

		yield return Wait(400, 800);

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

		while (!EntityGone(_target) && IsHating(_target))
		{
			if (!TryGetRandomSkill(out var skill))
			{
				yield return Wait(2000);
				continue;
			}

			yield return MoveToAttack(_target, skill.GetAttackRange());
			yield return UseSkill(skill, _target);

			yield return Wait(skill.Properties.Delay);
		}

		_target = null;
		StartRoutine("Idle", Idle());
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
}
