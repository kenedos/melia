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
using Melia.Zone.Skills.Handlers.Archers.Falconer;
using Melia.Zone.Skills.Helpers;
using Yggdrasil.Logging;
using Yggdrasil.Util;

/// <summary>
/// AI for the Falconer's hawk companion.
/// Handles flying behavior, idle wandering, shoulder landing,
/// skill interactions, and combat.
/// </summary>
[Ai("PC_Pet_Hawk")]
public class PcPetHawkAiScript : AiScript
{
	// Flying constants
	protected const float DefaultFlyHeight = 80f;

	// Follow distances
	protected const float FollowDistanceOwnerMoving = 80f;
	protected const float FollowDistanceOwnerStationary = 150f;
	protected new const float TeleportDistance = 400f;

	// Idle wandering timing
	protected const float WanderInterval = 5f;
	protected const int WanderMinRange = 40;
	protected const int WanderMaxRange = 60;

	// State tracking
	private DateTime _lastActionTime;
	private DateTime _lastWanderTime;

	private Companion Companion => this.Entity as Companion;

	protected override void Setup()
	{
		this.MaxChaseDistance = 350;
		this.MaxMasterDistance = 300;
		this.MaxRoamDistance = 400;
		this.EnableReturnHome = false;

		this.SetViewRange(300);

		During("Idle", CheckFirstStrike);
		During("Idle", CheckFear);
		During("Idle", CheckAggressiveMode);
		During("Idle", TeleportToOwnerIfTooFar);
		During("Attack", TeleportToOwnerIfTooFar);
	}

	protected virtual void TeleportToOwnerIfTooFar()
	{
		if (!this.TryGetMaster(out var owner))
			return;

		var distance = this.Entity.Position.Get2DDistance(owner.Position);
		if (distance <= TeleportDistance)
			return;

		var teleportPos = GetRandomFlyPosition(owner, 40);
		this.Entity.Position = teleportPos;
		Send.ZC_SET_POS(this.Entity);
		this.RemoveAllHate();
		_target = null;
	}

	protected override void Root()
	{
		_lastActionTime = DateTime.UtcNow;
		_lastWanderTime = DateTime.UtcNow;

		if (this.Companion != null)
		{
			this.Companion.SetFlyHeight(DefaultFlyHeight);
			this.Companion.SetFlyOption();
		}

		StartRoutine("Idle", HawkIdle());
	}

	#region State Properties (via Vars for external access)

	private bool IsUsingSkillFlag
	{
		get => this.Companion?.Vars.TryGet<bool>("Hawk.UsingSkill", out var val) == true && val;
		set => this.Companion?.Vars.Set("Hawk.UsingSkill", value);
	}

	private string CurrentSkillScript
	{
		get => this.Companion?.Vars.TryGet<string>("Hawk.CurrentSkill", out var val) == true ? val : "None";
		set => this.Companion?.Vars.Set("Hawk.CurrentSkill", value);
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

			if (IsUsingSkillFlag)
			{
				yield return this.Wait(200);
				continue;
			}

			if (!this.TryGetMaster(out var owner))
			{
				yield return this.Wait(500);
				continue;
			}

			// Skip AI movement while perched (shoulder or roost)
			if (this.Companion?.IsPerched == true)
			{
				yield return this.Wait(500);
				continue;
			}

			// Fly to roost and land when one is active
			if (this.Companion?.ActiveRoost != null && !this.Companion.ActiveRoost.IsDead && !this.Companion.IsOnRoost)
			{
				// After a skill, fly toward the owner first before
				// heading back to the roost
				var lastSkillEnd = this.Companion?.Vars.TryGet<DateTime>("Hawk.LastSkillEndTime", out var t) == true ? t : DateTime.MinValue;
					var timeSinceSkill = (DateTime.UtcNow - lastSkillEnd).TotalSeconds;
				if (timeSinceSkill < 3)
				{
					this.SetRunning(true);
					yield return this.MoveTo(owner.Position, wait: false);
					yield return this.Wait(500);
					continue;
				}

				var roost = this.Companion.ActiveRoost;
				yield return FlyToAndLand(roost,
					() => this.Companion.LandOnRoost(roost),
					() => roost.IsDead || this.Companion.ActiveRoost != roost);
				continue;
			}

			// Fly to owner and land on shoulder when requested
			if (this.Companion?.WantsToLand == true)
			{
				yield return FlyToAndLand(owner,
					() => this.Companion.LandOnOwnerShoulder(),
					() => this.Companion.WantsToLand == false);
				continue;
			}

			// Follow owner if too far away
			if (ShouldFollowOwner(owner))
			{
				yield return FollowOwner(owner);
				SetLastActionTime();
				continue;
			}

			// Idle behavior: wander
			yield return ProcessIdleBehavior(owner);

			yield return this.Wait(200);
		}
	}

	/// <summary>
	/// Attack routine for the hawk.
	/// </summary>
	protected override IEnumerable Attack()
	{
		this.SetRunning(true);
		TakeOffIfLanded();

		while (!this.EntityGone(_target) && this.IsHating(_target))
		{
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

			if (this.Entity.IsLocked(LockType.Attack))
			{
				yield return this.Wait(100, 200);
				continue;
			}

			if (!this.TryGetRandomSkill(out var skill))
			{
				_target = null;
				this.RemoveAllHate();
				this.StartRoutine("Idle", HawkIdle());
				yield break;
			}

			var attackRange = this.GetAttackRange(skill);

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

	#region Idle Behavior

	/// <summary>
	/// Processes idle behavior: wander randomly every few seconds.
	/// Shoulder landing/takeoff is handled by Companion.UpdateBirdBehavior.
	/// </summary>
	private IEnumerable ProcessIdleBehavior(ICombatEntity owner)
	{
		// Don't wander while landed on shoulder
		if (this.Companion?.IsLandedOnShoulder == true)
			yield break;

		var timeSinceWander = (DateTime.UtcNow - _lastWanderTime).TotalSeconds;
		if (timeSinceWander >= WanderInterval && !IsOwnerMoving(owner))
		{
			_lastWanderTime = DateTime.UtcNow;

			var hawkPos = this.Entity.Position;
			var rnd = RandomProvider.Get();

			// Get angle from owner to hawk, then pick the opposite side
			// with some randomness (+-70 degrees) so it flies across
			var angleToHawk = Math.Atan2(hawkPos.Z - owner.Position.Z, hawkPos.X - owner.Position.X);
			var oppositeAngle = angleToHawk + Math.PI;
			var spreadRad = 70.0 * Math.PI / 180.0;
			var angle = oppositeAngle + (rnd.NextDouble() * 2 - 1) * spreadRad;
			var dist = rnd.Next(WanderMinRange, WanderMaxRange);

			var candidate = new Position(
				owner.Position.X + (float)(Math.Cos(angle) * dist),
				owner.Position.Y,
				owner.Position.Z + (float)(Math.Sin(angle) * dist)
			);

			if (this.Entity.Map.Ground.TryGetNearestValidPosition(candidate, out var wanderPos))
				yield return this.MoveTo(wanderPos, wait: false);
		}
	}

	/// <summary>
	/// Flies the hawk to a roost and lands on it.
	/// </summary>
	/// <summary>
	/// Flies the hawk to a target and lands. Used for both shoulder
	/// and roost landing - the Companion handles the actual attach.
	/// </summary>
	/// <param name="target">Entity to fly to (owner or roost mob).</param>
	/// <param name="onArrival">Called when the hawk reaches the target. Should call the appropriate land method.</param>
	/// <param name="shouldAbort">Called each tick to check if landing should be aborted.</param>
	private IEnumerable FlyToAndLand(ICombatEntity target, Action onArrival, Func<bool> shouldAbort)
	{
		if (this.Companion == null || target == null)
			yield break;

		this.SetRunning(true);

		for (var i = 0; i < 30; i++)
		{
			if (shouldAbort())
				yield break;

			var dist = this.Entity.Position.Get2DDistance(target.Position);
			if (dist < 15f)
				break;

			yield return this.MoveTo(target.Position, wait: false);
			yield return this.Wait(200);
		}

		if (!shouldAbort())
			onArrival();
	}

	/// <summary>
	/// Takes the hawk off shoulder or roost if currently perched.
	/// </summary>
	private void TakeOffIfLanded()
	{
		if (this.Companion == null)
			return;

		if (this.Companion.IsLandedOnShoulder)
			this.Companion.TakeOff(DefaultFlyHeight);
		else if (this.Companion.IsOnRoost)
			this.Companion.LeaveRoost(DefaultFlyHeight);
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

		// Don't hard-teleport while FirstStrike is active — the hawk
		// may be far from the owner after an auto-cast and should fly
		// back smoothly instead of visibly jumping.
		if (distance >= TeleportDistance && !owner.IsBuffActive(BuffId.FirstStrike_Buff))
		{
			var teleportPos = GetRandomFlyPosition(owner, 250);
			this.Entity.Position = teleportPos;
			Send.ZC_SET_POS(this.Entity);
		}

		TakeOffIfLanded();
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

	#region Flying Behavior

	private Position GetRandomFlyPosition(ICombatEntity reference, float range)
	{
		if (reference == null)
			return this.Entity.Position;

		var angle = RandomProvider.Get().NextDouble() * Math.PI * 2;
		var dist = RandomProvider.Get().Next(0, (int)Math.Max(1, range));

		var candidate = new Position(
			reference.Position.X + (float)(Math.Cos(angle) * dist),
			reference.Position.Y,
			reference.Position.Z + (float)(Math.Sin(angle) * dist)
		);

		if (this.Entity.Map.Ground.TryGetNearestValidPosition(candidate, out var validPos))
			return new Position(validPos.X, validPos.Y + DefaultFlyHeight, validPos.Z);

		return new Position(candidate.X, candidate.Y + DefaultFlyHeight, candidate.Z);
	}

	#endregion

	#region Aggressive Mode (from PC_Pet)

	/// <summary>
	/// Checks for nearby enemies when in aggressive mode and engages them.
	/// Prioritizes enemies closer to the master.
	/// </summary>
	protected virtual void CheckAggressiveMode()
	{
		if (this.Companion == null || !this.Companion.IsAggressiveMode)
			return;

		if (this.Entity.IsLocked(LockType.Attack))
			return;

		if (!this.TryGetMaster(out var master))
			return;

		if (!this.InRangeOf(master, MaxMasterDistance))
			return;

		var nearbyEnemies = this.Entity.Map.GetAttackableEnemiesInPosition(this.Entity, this.Entity.Position, _viewRange)
			.Where(e => !e.IsDead && this.IsHostileTowards(e))
			.OrderBy(e => e.Position.Get2DDistance(master.Position))
			.ToList();

		if (nearbyEnemies.Any())
		{
			var closestToMaster = nearbyEnemies.First();
			this.IncreaseHate(closestToMaster, 150);
		}
	}

	/// <summary>
	/// Checks if aggressive mode was disabled mid-combat and stops attacking.
	/// </summary>
	protected virtual void CheckAggressiveDisabled()
	{
		if (this.Companion == null)
			return;

		if (!this.Companion.IsAggressiveMode && _target != null)
		{
			_target = null;
			this.RemoveAllHate();
			this.StartRoutine("Idle", HawkIdle());
		}
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

		if (FalconerHawkHelper.IsOnGlobalCooldown(this.Companion))
			return;

		// Pick the most hated target first (hate is added by the
		// FirstStrike buff when the owner attacks). Fall back to
		// the owner's current combat target if no hate exists.
		var target = this.GetMostHated();

		if (target == null || this.EntityGone(target))
			target = GetOwnerTarget(owner);

		if (target == null || this.EntityGone(target))
			return;

		// Try skills in priority order — each handler manages its own
		// cooldowns, damage, and visual animation

		// Blistering Thrash / Sonic Strike (if not disabled by Falconer14)
		if (!owner.IsAbilityActive(AbilityId.Falconer14)
			&& owner.TryGetSkill(SkillId.Falconer_BlisteringThrash, out var bt) && !bt.IsOnCooldown)
		{
			TakeOffIfLanded();
			Falconer_BlisteringThrashOverride.TryActivateSonicStrike(owner, target);
			return;
		}

		// Pheasant
		if (owner.TryGetSkill(SkillId.Falconer_Pheasant, out var ph) && !ph.IsOnCooldown)
		{
			TakeOffIfLanded();
			Falconer_PheasantOverride.TryAutoActivate(owner, target);
			return;
		}

		// Tomahawk
		if (owner.TryGetSkill(SkillId.Falconer_Tomahawk, out var tm) && !tm.IsOnCooldown)
		{
			TakeOffIfLanded();
			Falconer_TomahawkOverride.TryAutoActivate(owner, target);
			return;
		}

		// Hovering (if not disabled by Falconer13)
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
		this.Companion?.SetFlyHeight(DefaultFlyHeight);
	}

	#endregion

	#region Hawk Skill Routines

	private IEnumerable HawkBlisteringThrash(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		LockHawkAction(true, "BlisteringThrash");
		TakeOffIfLanded();

		var targetPos = target?.Position ?? owner.Position;
		var syncKey = this.Entity.GenerateSyncKey();

		var goTime = 0.7f;
		var backTime = 0.5f;

		if (target != null)
			Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		this.Entity.PlayEffect("F_archer_blisteringthrash_slash", scale: 1f);

		yield return this.Wait((int)((goTime + backTime) * 1000));

		SetLastActionTime();
	}

	private IEnumerable HawkHovering(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		LockHawkAction(true, "Hovering");
		yield return this.Wait(1000);
		TakeOffIfLanded();

		// Hovering is primarily managed by the skill handler's pad
		SetLastActionTime();
		LockHawkAction(false);
	}

	private IEnumerable HawkCombination(ICombatEntity owner, ICombatEntity target, Skill skill)
	{
		LockHawkAction(true, "Combination");
		TakeOffIfLanded();

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
