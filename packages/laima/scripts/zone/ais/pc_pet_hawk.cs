using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using Yggdrasil.Util;

/// <summary>
/// AI for the Falconer's hawk companion.
/// Handles flying behavior, resting mechanics, skill interactions, and combat.
/// </summary>
[Ai("PC_Pet_Hawk")]
public class PcPetHawkAiScript : AiScript
{
	// Flying constants
	protected const float DefaultHawkHeight = 80f;
	protected const float FlyAwayHeight = 280f;
	protected const float FlyAwayDistance = 300f;

	// Follow distances
	protected const float FollowDistanceOwnerMoving = 50f;
	protected const float FollowDistanceOwnerStationary = 150f;
	protected const float TeleportDistance = 300f;

	// Resting timing
	protected const float TimeBeforeRest = 10f; // seconds of inactivity before resting
	protected const float RestDuration = 15f; // seconds to rest before getting up

	// State tracking via Vars (shared with skill handlers)
	private DateTime _lastActionTime;
	private DateTime _lastSitTime;

	private Mob Mob => this.Entity as Mob;

	protected override void Setup()
	{
		this.MaxChaseDistance = 350;
		this.MaxMasterDistance = 300;
		this.MaxRoamDistance = 1000;
		this.EnableReturnHome = false;

		// Set view range for hawk
		this.SetViewRange(300);

		During("Idle", CheckEnemies);
		During("Idle", CheckFirstStrike);
		During("Idle", CheckFear);
		During("Attack", CheckTarget);
		During("Attack", CheckMaster);
		During("Attack", CheckFear);
	}

	protected override void Root()
	{
		// Initialize hawk state
		_lastActionTime = DateTime.UtcNow;
		_lastSitTime = DateTime.UtcNow;

		// Set initial flying height
		NotifyFlying(true, DefaultHawkHeight);

		StartRoutine("Idle", HawkIdle());
	}

	#region State Properties (via Vars for external access)

	private bool IsHidden
	{
		get => this.Mob.Vars.TryGet<bool>("Hawk.IsHidden", out var val) && val;
		set => this.Mob.Vars.Set("Hawk.IsHidden", value);
	}

	private bool IsResting
	{
		get => this.Mob.Vars.TryGet<bool>("Hawk.IsResting", out var val) && val;
		set => this.Mob.Vars.Set("Hawk.IsResting", value);
	}

	private bool IsOnRoost
	{
		get => this.Mob.Vars.TryGet<bool>("Hawk.IsOnRoost", out var val) && val;
		set => this.Mob.Vars.Set("Hawk.IsOnRoost", value);
	}

	private bool IsFlyingAway
	{
		get => this.Mob.Vars.TryGet<bool>("Hawk.IsFlyingAway", out var val) && val;
		set => this.Mob.Vars.Set("Hawk.IsFlyingAway", value);
	}

	private bool IsUsingSkillFlag
	{
		get => this.Mob.Vars.TryGet<bool>("Hawk.UsingSkill", out var val) && val;
		set => this.Mob.Vars.Set("Hawk.UsingSkill", value);
	}

	private string CurrentSkillScript
	{
		get => this.Mob.Vars.TryGet<string>("Hawk.CurrentSkill", out var val) ? val : "None";
		set => this.Mob.Vars.Set("Hawk.CurrentSkill", value);
	}

	#endregion

	#region Main Routines

	/// <summary>
	/// Main idle routine for the hawk.
	/// </summary>
	protected virtual IEnumerable HawkIdle()
	{
		this.ResetMoveSpeed();

		while (true)
		{
			if (this.Entity.IsDead)
				yield break;

			// Check if hawk is hidden (flew away)
			if (IsHidden)
			{
				yield return this.Wait(500);
				continue;
			}

			// Check if using a skill (let skill handler control)
			if (IsUsingSkillFlag)
			{
				yield return this.Wait(200);
				continue;
			}

			// Check if owner exists
			if (!this.TryGetMaster(out var owner))
			{
				yield return this.Wait(500);
				continue;
			}

			// Check if we should sit on roost
			if (CheckAndSitOnRoost(owner))
			{
				SetLastActionTime();
				yield return this.Wait(500);
				continue;
			}

			// Check if we should follow owner
			if (ShouldFollowOwner(owner))
			{
				yield return FollowOwner(owner);
				SetLastActionTime();
				continue;
			}

			// Process resting behavior
			yield return ProcessRestBehavior(owner);

			yield return this.Wait(200);
		}
	}

	/// <summary>
	/// Attack routine for the hawk.
	/// </summary>
	protected override IEnumerable Attack()
	{
		this.SetRunning(true);
		UnrestIfNeeded();

		while (!this.EntityGone(_target) && this.IsHating(_target))
		{
			// Check master distance
			if (this.TryGetMaster(out var master))
			{
				if (this.EntityGone(master) || !this.InRangeOf(master, MaxMasterDistance))
				{
					_target = null;
					this.RemoveAllHate();
					this.StartRoutine("Idle", HawkIdle());
					yield break;
				}
			}

			// Check if we can attack
			if (this.Entity.IsLocked(LockType.Attack))
			{
				yield return this.Wait(100, 200);
				continue;
			}

			// Try to get a skill
			if (!this.TryGetRandomSkill(out var skill))
			{
				yield return this.Wait(250);
				continue;
			}

			var attackRange = this.GetAttackRange(skill);

			// Move into range
			if (!this.InRangeOf(_target, attackRange))
			{
				yield return this.MoveToAttack(_target, attackRange);

				if (this.EntityGone(_target) || !this.IsHating(_target))
				{
					_target = null;
					this.StartRoutine("Idle", HawkIdle());
					yield break;
				}
			}

			// Attack
			if (this.InRangeOf(_target, attackRange) && this.CanUseSkill(skill, _target))
			{
				yield return this.TurnTowards(_target);
				yield return this.UseSkill(skill, _target);
				SetLastActionTime();
			}

			yield return true;
		}

		_target = null;
		this.CheckEnemies();

		if (_target != null)
			yield break;

		this.StartRoutine("Idle", HawkIdle());
	}

	#endregion

	#region Flying Behavior

	/// <summary>
	/// Notifies the movement component about flying state.
	/// </summary>
	private void NotifyFlying(bool flying, float height = DefaultHawkHeight)
	{
		if (this.Entity.Components.TryGet<MovementComponent>(out var movement))
		{
			movement.NotifyFlying(flying, height, 1, 1.5f);
		}
	}

	/// <summary>
	/// Makes the hawk fly away and become hidden.
	/// </summary>
	public IEnumerable FlyAway()
	{
		if (IsFlyingAway || IsHidden)
			yield break;

		// Check for roost
		if (this.TryGetMaster(out var owner))
		{
			var roostHandle = (int)owner.GetTempVar("HAWK_ROOST");
			if (roostHandle != 0)
			{
				LockHawkAction(false);
				yield break;
			}

			// Falconer20: Hawk Hunt
			if (owner.IsAbilityActive(AbilityId.Falconer20))
			{
				LockHawkAction(false);
				yield break;
			}
		}

		IsFlyingAway = true;

		var flyPos = GetRandomFlyPosition(owner, FlyAwayDistance);
		NotifyFlying(true, FlyAwayHeight);

		yield return this.MoveTo(flyPos, wait: false);
		yield return this.Wait(5000);

		if (!IsFlyingAway)
			yield break;

		IsHidden = true;
		IsFlyingAway = false;
		LockHawkAction(false);
	}

	/// <summary>
	/// Makes the hawk unhide and return to its owner.
	/// </summary>
	public IEnumerable Unhide(ICombatEntity target)
	{
		if (!IsHidden && !IsFlyingAway)
			yield break;

		NotifyFlying(true, DefaultHawkHeight);

		if (IsFlyingAway)
		{
			IsFlyingAway = false;
		}
		else
		{
			var pos = GetRandomFlyPosition(target, 250);
			this.Entity.Position = pos;
			Send.ZC_SET_POS(this.Entity);
		}

		IsHidden = false;
		LockHawkAction(false);

		yield return this.MoveTo(target.Position, wait: false);
	}

	private Position GetRandomFlyPosition(ICombatEntity reference, float range)
	{
		if (reference == null)
			return this.Entity.Position;

		var rnd = RandomProvider.Get();
		var angle = rnd.Next(360) * Math.PI / 180.0;

		var x = reference.Position.X + (float)(Math.Cos(angle) * range);
		var z = reference.Position.Z + (float)(Math.Sin(angle) * range);
		var y = reference.Position.Y + DefaultHawkHeight;

		return new Position(x, y, z);
	}

	#endregion

	#region Following Behavior

	private bool ShouldFollowOwner(ICombatEntity owner)
	{
		var isOwnerMoving = IsOwnerMoving(owner);
		var parentRange = isOwnerMoving ? FollowDistanceOwnerMoving : FollowDistanceOwnerStationary;
		var distance = this.Entity.Position.Get2DDistance(owner.Position);
		return distance > parentRange;
	}

	private IEnumerable FollowOwner(ICombatEntity owner)
	{
		var distance = this.Entity.Position.Get2DDistance(owner.Position);

		if (distance >= TeleportDistance)
		{
			var teleportPos = GetRandomFlyPosition(owner, 250);
			this.Entity.Position = teleportPos;
			Send.ZC_SET_POS(this.Entity);
		}

		UnrestIfNeeded();
		this.Entity.Interrupt();
		this.RemoveAllHate();
		this.SetRunning(true);

		for (var i = 0; i < 20; i++)
		{
			var dist = this.Entity.Position.Get2DDistance(owner.Position);
			if (dist < 20f)
				yield break;

			yield return this.MoveTo(owner.Position, wait: false);
			SetLastActionTime();
			yield return this.Wait(200);
		}
	}

	private bool IsOwnerMoving(ICombatEntity owner)
	{
		if (owner.Components.TryGet<MovementComponent>(out var movement) && movement.IsMoving)
			return true;
		return owner.IsCasting();
	}

	#endregion

	#region Resting Behavior

	private IEnumerable ProcessRestBehavior(ICombatEntity owner)
	{
		var timeSinceLastAction = (DateTime.UtcNow - _lastActionTime).TotalSeconds;

		if (IsResting)
		{
			if (IsOwnerMoving(owner) || timeSinceLastAction > RestDuration)
			{
				yield return Unrest();
			}
		}
		else
		{
			if (timeSinceLastAction > TimeBeforeRest)
			{
				SetLastActionTime();

				if (!IsOwnerMoving(owner))
				{
					var timeSinceLastSit = (DateTime.UtcNow - _lastSitTime).TotalSeconds;
					if (timeSinceLastSit < 40)
					{
						var wanderPos = owner.Position.GetRandomInRange2D(30, 50);
						yield return this.MoveTo(wanderPos, wait: false);
					}
					else
					{
						yield return RestWithOwner(owner);
					}
				}
			}
		}
	}

	private IEnumerable RestWithOwner(ICombatEntity owner)
	{
		LockHawkAction(true, "RestWithOwner");

		for (var i = 0; i < 20; i++)
		{
			var dist = this.Entity.Position.Get2DDistance(owner.Position);
			if (dist < 10)
				break;
			yield return this.MoveTo(owner.Position, wait: false);
			yield return this.Wait(500);
		}

		NotifyFlying(true, 1);
		yield return this.Wait(1000);

		Send.ZC_PLAY_ANI(this.Entity, "ASTD_TO_SIT");

		if (IsOwnerMoving(owner))
		{
			LockHawkAction(false);
			yield return Unrest();
			yield break;
		}

		this.Entity.Direction = owner.Direction;
		yield return this.Wait(1000);

		owner.PlayEffect("F_smoke109_2", scale: 1.5f);

		IsResting = true;
		_lastSitTime = DateTime.UtcNow;
		_lastActionTime = DateTime.UtcNow + TimeSpan.FromSeconds(RandomProvider.Get().Next(15, 25));

		LockHawkAction(false);
	}

	private IEnumerable Unrest()
	{
		if (!IsResting && !IsOnRoost)
			yield break;

		NotifyFlying(true, DefaultHawkHeight);
		Send.ZC_PLAY_ANI(this.Entity, "SIT_TO_ASTD");

		IsResting = false;
		IsOnRoost = false;

		yield return this.Wait(1000);
	}

	private void UnrestIfNeeded()
	{
		if (IsResting || IsOnRoost)
		{
			this.ExecuteOnce(Unrest());
		}
	}

	#endregion

	#region Roost Behavior

	private bool CheckAndSitOnRoost(ICombatEntity owner)
	{
		var roostHandle = (int)owner.GetTempVar("HAWK_ROOST");
		if (roostHandle == 0)
		{
			if (IsOnRoost)
				this.ExecuteOnce(Unrest());
			return false;
		}

		var roost = this.Entity.Map.GetCombatEntity(roostHandle);
		if (roost == null || roost.IsDead)
		{
			if (IsOnRoost)
				this.ExecuteOnce(Unrest());
			return false;
		}

		if (IsFlyingAway || IsHidden)
		{
			this.ExecuteOnce(Unhide(roost));
			return true;
		}

		var roostSitDist = 80f;
		if (owner.IsAbilityActive(AbilityId.Falconer1))
			roostSitDist += 100f;

		var distFromRoost = owner.Position.Get2DDistance(roost.Position);
		if (distFromRoost >= roostSitDist)
		{
			if (IsOnRoost)
				this.ExecuteOnce(Unrest());
			return false;
		}

		if (!IsOnRoost)
			this.ExecuteOnce(SitOnRoost(roost));

		return true;
	}

	private IEnumerable SitOnRoost(ICombatEntity roost)
	{
		LockHawkAction(true, "SitOnRoost");

		for (var i = 0; i < 20; i++)
		{
			var dist = this.Entity.Position.Get2DDistance(roost.Position);
			if (dist < 10)
				break;
			yield return this.MoveTo(roost.Position, wait: false);
			yield return this.Wait(500);
		}

		NotifyFlying(true, 1);
		yield return this.Wait(1000);

		Send.ZC_PLAY_ANI(this.Entity, "ASTD_TO_SIT");
		yield return this.Wait(1000);

		roost.PlayEffect("F_smoke109_2", scale: 1.5f);

		IsOnRoost = true;
		_lastSitTime = DateTime.UtcNow;

		LockHawkAction(false);
	}

	#endregion

	#region First Strike Behavior

	protected virtual void CheckFirstStrike()
	{
		if (!this.TryGetMaster(out var owner))
			return;

		if (!owner.IsBuffActive(BuffId.FirstStrike_Buff))
			return;

		if (IsUsingSkillFlag)
			return;

		if (owner.Components.TryGet<CombatComponent>(out var combat) && !combat.AttackState)
			return;

		var target = GetOwnerTarget(owner);
		if (target == null || this.EntityGone(target))
			return;

		// Falconer14: Remove Sonic Strike
		if (!owner.IsAbilityActive(AbilityId.Falconer14)
			&& owner.TryGetSkill(SkillId.Falconer_BlisteringThrash, out var skill) && !skill.IsOnCooldown)
		{
			TriggerHawkSkill("BlisteringThrash", owner, target, skill, true);
			return;
		}

		// Falconer13: Remove Hovering
		if (!owner.IsAbilityActive(AbilityId.Falconer13))
		{
			var approachingEnemy = FindEnemyApproachingOwner(owner);
			if (approachingEnemy != null
				&& owner.TryGetSkill(SkillId.Falconer_Hovering, out var hovering) && !hovering.IsOnCooldown)
			{
				TriggerHawkSkill("Hovering", owner, approachingEnemy, hovering, true);
			}
		}
	}

	private ICombatEntity GetOwnerTarget(ICombatEntity owner)
	{
		if (owner.Components.TryGet<CombatComponent>(out var combat))
			return combat.GetTopAttackerByDamage();
		return null;
	}

	private ICombatEntity FindEnemyApproachingOwner(ICombatEntity owner)
	{
		var enemies = this.Entity.Map.GetAttackableEnemiesInPosition(owner, owner.Position, 100);

		foreach (var enemy in enemies)
		{
			if (enemy.IsDead)
				continue;

			if (enemy is Mob mob && mob.Components.TryGet<AiComponent>(out var ai))
			{
				var enemyTarget = ai.Script.Target;
				if (enemyTarget != null && enemyTarget.Handle == owner.Handle)
					return enemy;
			}
		}

		return null;
	}

	#endregion

	#region Skill Helpers

	public void LockHawkAction(bool locked, string scriptName = "None")
	{
		IsUsingSkillFlag = locked;
		CurrentSkillScript = scriptName;
	}

	public bool IsHawkActionLocked() => IsUsingSkillFlag;

	public string GetCurrentSkillScript() => CurrentSkillScript;

	private void SetLastActionTime()
	{
		_lastActionTime = DateTime.UtcNow;
	}

	public void TriggerHawkSkill(string skillName, ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike = false)
	{
		var currentScript = GetCurrentSkillScript();
		if (currentScript != "None" && IsUsingSkillFlag)
		{
			StopCurrentSkillScript(currentScript);
		}

		switch (skillName)
		{
			case "BlisteringThrash":
				this.ExecuteOnce(HawkBlisteringThrash(owner, target, skill, isFirstStrike));
				break;
			case "Hovering":
				this.ExecuteOnce(HawkHovering(owner, target, skill, isFirstStrike));
				break;
			case "Combination":
				this.ExecuteOnce(HawkCombination(owner, target, skill));
				break;
		}
	}

	private void StopCurrentSkillScript(string scriptName)
	{
		LockHawkAction(false);
		NotifyFlying(true, DefaultHawkHeight);
	}

	#endregion

	#region Hawk Skill Routines

	private IEnumerable HawkBlisteringThrash(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		LockHawkAction(true, "BlisteringThrash");
		UnrestIfNeeded();

		var targetPos = target?.Position ?? owner.Position;
		var syncKey = this.Entity.GenerateSyncKey();

		var goTime = 0.7f;
		var backTime = 0.5f;

		var dx = 2 * targetPos.X - owner.Position.X;
		var dy = owner.Position.Y + DefaultHawkHeight;
		var dz = 2 * targetPos.Z - owner.Position.Z;

		if (target != null)
			Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		this.Entity.PlayEffect("F_archer_blisteringthrash_slash", scale: 1f);

		yield return this.Wait((int)((goTime + backTime) * 1000));

		SetLastActionTime();
		yield return FlyAway();
	}

	private IEnumerable HawkHovering(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		LockHawkAction(true, "Hovering");
		yield return this.Wait(1000);
		UnrestIfNeeded();

		// Hovering is primarily managed by the skill handler's pad
		// This just handles the hawk's position and animation

		SetLastActionTime();
		LockHawkAction(false);
	}

	private IEnumerable HawkCombination(ICombatEntity owner, ICombatEntity target, Skill skill)
	{
		LockHawkAction(true, "Combination");
		UnrestIfNeeded();

		var targetPos = target.Position;
		var syncKey = this.Entity.GenerateSyncKey();

		var goTime = 0.5f;
		var backTime = 0.5f;

		Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		yield return this.Wait((int)((goTime + backTime) * 1500));

		syncKey = this.Entity.GenerateSyncKey();
		Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		yield return this.Wait((int)((goTime + backTime) * 1000));

		LockHawkAction(false);
		SetLastActionTime();
	}

	#endregion
}
