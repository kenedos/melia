using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
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
	protected override void Setup()
	{
		base.Setup();

		During("Idle", StayWithMaster);
		During("Attack", StayWithMaster);
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
		// The fox's fireball needs Onmyoji18, which the ability tree does not offer.
		if (base.TryGetRandomSkill(out skill) && skill.Id == SkillId.Mon_pcskill_FireFoxShikigami_Skill_1)
			return true;

		skill = null;
		return false;
	}
}
