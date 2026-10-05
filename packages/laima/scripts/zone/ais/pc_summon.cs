using System;
using System.Collections;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

[Ai("PC_Summon")]
public class PCSummonAiScript : AiScript
{
	private const float MaxTargetHeightDifference = 60f;
	private const float MaxTargetDistanceFromMaster = 180f;

	protected override void Setup()
	{
		MaxChaseDistance = 180;
		MaxMasterDistance = 180;

		During("Idle", ResetDistantHateDuringIdle);
		During("Idle", ValidateCurrentTarget);
		During("Idle", CheckEnemiesEnhanced);
		During("Idle", TeleportToMasterIfTooFar);

		During("Attack", TeleportToMasterIfTooFar);
		During("Attack", ValidateCurrentTarget);
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
			yield return MoveRandom();
		else
			yield return Animation("IDLE");
	}

	protected override IEnumerable Attack()
	{
		SetRunning(true);

		while (true)
		{
			var target = _target;

			if (!IsValidSummonTarget(target))
			{
				ClearInvalidTarget(target);
				break;
			}

			if (!IsHating(target))
			{
				ClearInvalidTarget(target);
				break;
			}

			if (!TryGetRandomSkill(out var skill))
			{
				yield return Wait(250);
				continue;
			}

			if (!IsValidSummonTarget(target))
			{
				ClearInvalidTarget(target);
				break;
			}

			yield return MoveToAttack(target, GetAttackRange(skill));

			if (!IsValidSummonTarget(target))
			{
				ClearInvalidTarget(target);
				break;
			}

			if (_target != target || !IsHating(target))
			{
				ClearInvalidTarget(target);
				break;
			}

			if (!InRangeOf(target, GetAttackRange(skill)))
			{
				yield return Wait(100);
				continue;
			}

			if (!CanUseSkill(skill, target))
			{
				if (!IsValidSummonTarget(target))
				{
					ClearInvalidTarget(target);
					break;
				}

				yield return Wait(100);
				continue;
			}

			yield return UseSkill(skill, target);

			if (!IsValidSummonTarget(target))
			{
				ClearInvalidTarget(target);
				break;
			}
		}

		_target = null;

		yield return StopMove();

		SetRunning(false);

		StartRoutine("Idle", Idle());
	}

	private void ValidateCurrentTarget()
	{
		if (_target == null)
			return;

		if (IsValidSummonTarget(_target))
			return;

		ClearInvalidTarget(_target);

		if (CurrentRoutine == "Attack")
			StartRoutine("StopAndIdle", StopAndIdle());
	}

	private bool IsValidSummonTarget(ICombatEntity target)
	{
		if (target == null)
			return false;

		if (Entity == null || Entity.Map == null)
			return false;

		if (EntityGone(target) || target.IsDead)
			return false;

		if (target.Map != Entity.Map)
			return false;

		var registeredTarget = Entity.Map.GetCombatEntity(target.Handle);

		if (registeredTarget == null || !ReferenceEquals(registeredTarget, target))
			return false;

		if (!CanBeHated(target))
			return false;

		if (!IsHostileTowards(target))
			return false;

		if (!InRangeOf(target, MaxChaseDistance))
			return false;

		var summonHeightDifference = Math.Abs(
			Entity.Position.Y - target.Position.Y
		);

		if (summonHeightDifference > MaxTargetHeightDifference)
			return false;

		if (Entity.Map.Ground.AnyObstacles(
			Entity.Position,
			target.Position
		))
		{
			return false;
		}

		if (Entity is Mob summon && !summon.CanSee(target))
			return false;

		var master = GetMaster();

		if (master == null || EntityGone(master))
			return false;

		if (IsSorcererSummon() && !IsMasterAttacking(master))
			return false;

		if (master.Map != target.Map)
			return false;

		var targetDistanceFromMaster = master.Position.Get2DDistance(
			target.Position
		);

		if (targetDistanceFromMaster > MaxTargetDistanceFromMaster)
			return false;

		var masterHeightDifference = Math.Abs(
			master.Position.Y - target.Position.Y
		);

		if (masterHeightDifference > MaxTargetHeightDifference)
			return false;

		if (Entity.Map.Ground.AnyObstacles(
			master.Position,
			target.Position
		))
		{
			return false;
		}

		return true;
	}

	private bool IsSorcererSummon()
	{
		return HasSummonVariable("SORCERER_SUMMONING") ||
			HasSummonVariable("SORCERER_MON") ||
			HasSummonVariable("SORCERER_SUMMONSALOON") ||
			HasSummonVariable("SORCERER_FAMILIAR");
	}

	private bool HasSummonVariable(string variableName)
	{
		return Entity is Summon summon && summon.Vars.TryGetInt(variableName, out var value) && value == 1;
	}

	private bool IsMasterAttacking(ICombatEntity master)
	{
		if (master == null || master.IsDead)
			return false;

		if (!master.Components.TryGet<CombatComponent>(out var combatComponent))
			return false;

		return combatComponent.AttackState;
	}

	private void ClearInvalidTarget(ICombatEntity target)
	{
		if (target != null)
			RemoveHate(target);

		if (_target == target)
			_target = null;
	}

	protected IEnumerable StopAndIdle()
	{
		yield return StopMove();
		StartRoutine("Idle", Idle());
	}

	protected IEnumerable StopAndAttack()
	{
		if (!IsValidSummonTarget(_target))
		{
			ClearInvalidTarget(_target);
			StartRoutine("Idle", Idle());
			yield break;
		}

		ExecuteOnce(Emoticon("I_emo_exclamation"));
		ExecuteOnce(TurnTowards(_target));

		yield return StopMove();

		if (!IsValidSummonTarget(_target))
		{
			ClearInvalidTarget(_target);
			StartRoutine("Idle", Idle());
			yield break;
		}

		StartRoutine("Attack", Attack());
	}
}
