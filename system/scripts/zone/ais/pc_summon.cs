using System;
using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

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

	protected override IEnumerable Attack()
	{
		SetRunning(true);

		while (!EntityGone(_target) && IsHating(_target))
		{
			if (!TryGetRandomSkill(out var skill))
			{
				yield return Wait(250);
				continue;
			}

			yield return MoveToAttack(_target, GetAttackRange(skill));

			if (EntityGone(_target) || !IsHating(_target))
				break;

			if (InRangeOf(_target, GetAttackRange(skill)) && CanUseSkill(skill, _target))
				yield return UseSkill(skill, _target);
			else
				yield return Wait(100);
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

[Ai("PC_Summon_FireFox")]
public class PCSummonFireFoxAiScript : PCSummonAiScript
{
	private const string MoveAnimation = "run";
	private const string StandAnimation = "astd";
	private static readonly TimeSpan AnimationInterval = TimeSpan.FromMilliseconds(500);
	private static readonly TimeSpan AttackAnimationPause = TimeSpan.FromMilliseconds(2050);
	private static readonly TimeSpan SpawnDelay = TimeSpan.FromMilliseconds(1400);

	private DateTime _nextAnimation = DateTime.MinValue;
	private DateTime _readyTime = DateTime.MinValue;

	protected override void Setup()
	{
		base.Setup();

		_readyTime = GameClock.Now + SpawnDelay;
		_nextAnimation = _readyTime;

		During("Idle", StayWithMaster);
		During("Idle", MatchMasterMovement);
		During("Attack", StayWithMaster);
		During("Attack", MatchMasterMovement);
	}

	/// <summary>
	/// Plays the fox's run animation while its master moves and its stand
	/// animation otherwise.
	/// </summary>
	protected void MatchMasterMovement()
	{
		if (GameClock.Now < _nextAnimation)
			return;

		this.PlayMovementAnimation();
		_nextAnimation = GameClock.Now + AnimationInterval;
	}

	protected override IEnumerable UseSkill(Skill skill, ICombatEntity target, TimeSpan delay = default)
	{
		this.PlayMovementAnimation();
		_nextAnimation = GameClock.Now + AttackAnimationPause;

		yield return base.UseSkill(skill, target, delay);
	}

	/// <summary>
	/// Sends the animation matching the master's movement.
	/// </summary>
	private void PlayMovementAnimation()
	{
		if (!this.TryGetMaster(out var master))
			return;

		var isMoving = master is Character character && character.Movement.IsMoving && character.Movement.MoveTarget == MoveTargetType.Direction;

		Send.ZC_PLAY_ANI(this.Entity, isMoving ? MoveAnimation : StandAnimation, true, 0, 1, 1, 1);
	}

	/// <summary>
	/// Keeps the fox on its master, whom the client makes it hover beside.
	/// </summary>
	protected void StayWithMaster()
	{
		if (this.TryGetMaster(out var master) && master.Map == this.Entity.Map)
			this.Entity.Position = master.Position;
	}

	protected override bool TryGetRandomSkill(out Skill skill)
	{
		if (GameClock.Now < _readyTime)
		{
			skill = null;
			return false;
		}

		// The fox's fireball needs Onmyoji18, which the ability tree does not offer.
		if (base.TryGetRandomSkill(out skill) && skill.Id == SkillId.Mon_pcskill_FireFoxShikigami_Skill_1)
			return true;

		skill = null;
		return false;
	}
}
