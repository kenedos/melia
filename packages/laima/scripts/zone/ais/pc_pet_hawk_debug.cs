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
using static g4.RoundRectGenerator;

/// <summary>
/// DEBUG VERSION: AI for the Falconer's hawk companion.
/// Handles flying behavior, resting mechanics, skill interactions, and combat.
/// All behaviors are logged for debugging purposes.
/// </summary>
[Ai("PC_Pet_Hawk_Debug")]
public class PcPetHawkDebugAiScript : AiScript
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

	// Rest state tracking - stores owner position when resting started
	private Position _restOwnerPos;

	private Mob Mob => this.Entity as Mob;

	// ====== DEBUG HELPERS ======
	private void DebugLog(string message)
	{
		if (this.ShowDebug)
			Log.Debug($"[HAWK-AI] {this.Entity?.Name ?? "Unknown"}: {message}");
	}

	private void DebugLogState(string context)
	{
		if (!this.ShowDebug)
			return;

		var stateInfo = $"[STATE @ {context}] " +
			$"Hidden={IsHidden}, " +
			$"Resting={IsResting}, " +
			$"OnRoost={IsOnRoost}, " +
			$"FlyingAway={IsFlyingAway}, " +
			$"UsingSkill={IsUsingSkillFlag} ({CurrentSkillScript}), " +
			$"Target={(_target?.Name ?? "None")}, " +
			$"TimeSinceAction={(DateTime.UtcNow - _lastActionTime).TotalSeconds:F1}s";

		Log.Debug($"[HAWK-AI] {this.Entity?.Name ?? "Unknown"}: {stateInfo}");
	}

	protected override void Setup()
	{
		DebugLog("Setup() called - configuring hawk AI");

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

		DebugLog("Setup() complete - During actions registered: " +
			"Idle[CheckEnemies, CheckFirstStrike, CheckFear], " +
			"Attack[CheckTarget, CheckMaster, CheckFear]");
	}

	protected override void Root()
	{
		DebugLog("Root() called - initializing hawk");

		// Initialize hawk state
		_lastActionTime = DateTime.UtcNow;
		_lastSitTime = DateTime.UtcNow;

		// Set initial flying height
		NotifyFlying(true, DefaultHawkHeight);

		DebugLog($"Root() - Initial state set: flying at height {DefaultHawkHeight}, starting Idle routine");
		StartRoutine("Idle", HawkIdle());
	}

	#region State Properties (via Vars for external access)

	private bool IsHidden
	{
		get => this.Mob?.Vars.TryGet<bool>("Hawk.IsHidden", out var val) == true && val;
		set
		{
			this.Mob?.Vars.Set("Hawk.IsHidden", value);
			DebugLog($"State Change: IsHidden = {value}");
		}
	}

	private bool IsResting
	{
		get => this.Mob?.Vars.TryGet<bool>("Hawk.IsResting", out var val) == true && val;
		set
		{
			this.Mob?.Vars.Set("Hawk.IsResting", value);
			DebugLog($"State Change: IsResting = {value}");
		}
	}

	private bool IsOnRoost
	{
		get => this.Mob?.Vars.TryGet<bool>("Hawk.IsOnRoost", out var val) == true && val;
		set
		{
			this.Mob?.Vars.Set("Hawk.IsOnRoost", value);
			DebugLog($"State Change: IsOnRoost = {value}");
		}
	}

	private bool IsFlyingAway
	{
		get => this.Mob?.Vars.TryGet<bool>("Hawk.IsFlyingAway", out var val) == true && val;
		set
		{
			this.Mob?.Vars.Set("Hawk.IsFlyingAway", value);
			DebugLog($"State Change: IsFlyingAway = {value}");
		}
	}

	private bool IsUsingSkillFlag
	{
		get => this.Mob?.Vars.TryGet<bool>("Hawk.UsingSkill", out var val) == true && val;
		set
		{
			this.Mob?.Vars.Set("Hawk.UsingSkill", value);
			DebugLog($"State Change: IsUsingSkillFlag = {value}");
		}
	}

	private string CurrentSkillScript
	{
		get => this.Mob?.Vars.TryGet<string>("Hawk.CurrentSkill", out var val) == true ? val : "None";
		set
		{
			this.Mob?.Vars.Set("Hawk.CurrentSkill", value);
			DebugLog($"State Change: CurrentSkillScript = {value}");
		}
	}

	#endregion

	#region Main Routines

	/// <summary>
	/// Main idle routine for the hawk.
	/// </summary>
	protected virtual IEnumerable HawkIdle()
	{
		DebugLog("HawkIdle() - Starting idle routine");
		this.ResetMoveSpeed();

		var loopCount = 0;
		while (true)
		{
			loopCount++;

			if (this.Entity.IsDead)
			{
				DebugLog("HawkIdle() - Entity is dead, exiting");
				yield break;
			}

			// Periodic state logging every 50 loops (~10 seconds at 200ms wait)
			if (loopCount % 50 == 0)
			{
				DebugLogState($"HawkIdle loop #{loopCount}");
			}

			// Check if hawk is hidden (flew away)
			if (IsHidden)
			{
				DebugLog("HawkIdle() - Hawk is hidden, waiting");
				yield return this.Wait(500);
				continue;
			}

			// Check if using a skill (let skill handler control)
			if (IsUsingSkillFlag)
			{
				DebugLog($"HawkIdle() - Using skill '{CurrentSkillScript}', waiting");
				yield return this.Wait(200);
				continue;
			}

			// Check if owner exists
			if (!this.TryGetMaster(out var owner))
			{
				DebugLog("HawkIdle() - No master found, waiting");
				yield return this.Wait(500);
				continue;
			}

			// Check if we should sit on roost
			if (CheckAndSitOnRoost(owner))
			{
				DebugLog("HawkIdle() - Roost behavior triggered");
				SetLastActionTime();
				yield return this.Wait(500);
				continue;
			}

			// Check if we should follow owner
			if (ShouldFollowOwner(owner))
			{
				DebugLog($"HawkIdle() - Following owner (distance: {this.Entity.Position.Get2DDistance(owner.Position):F1})");
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
		DebugLog($"Attack() - Starting attack routine, target: {_target?.Name ?? "None"}");
		DebugLogState("Attack Start");

		this.SetRunning(true);
		UnrestIfNeeded();

		var attackLoopCount = 0;
		while (!this.EntityGone(_target) && this.IsHating(_target))
		{
			attackLoopCount++;

			// Periodic logging during attack
			if (attackLoopCount % 20 == 0)
			{
				DebugLogState($"Attack loop #{attackLoopCount}");
			}

			// Check master distance
			if (this.TryGetMaster(out var master))
			{
				if (this.EntityGone(master) || !this.InRangeOf(master, MaxMasterDistance))
				{
					DebugLog($"Attack() - Master out of range ({this.Entity.Position.Get2DDistance(master?.Position ?? Position.Zero):F1}), returning to idle");
					_target = null;
					this.RemoveAllHate();
					this.StartRoutine("Idle", HawkIdle());
					yield break;
				}
			}

			// Check if we can attack
			if (this.Entity.IsLocked(LockType.Attack))
			{
				DebugLog("Attack() - Attack locked, waiting");
				yield return this.Wait(100, 200);
				continue;
			}

			// Try to get a skill
			if (!this.TryGetRandomSkill(out var skill))
			{
				DebugLog("Attack() - No skill available");
				yield return this.Wait(250);
				continue;
			}

			var attackRange = this.GetAttackRange(skill);
			DebugLog($"Attack() - Using skill {skill.Id}, range: {attackRange:F1}");

			// Move into range
			if (!this.InRangeOf(_target, attackRange))
			{
				DebugLog($"Attack() - Moving to target (current distance: {this.Entity.Position.Get2DDistance(_target.Position):F1})");
				yield return this.MoveToAttack(_target, attackRange);

				if (this.EntityGone(_target) || !this.IsHating(_target))
				{
					DebugLog("Attack() - Target lost during movement");
					_target = null;
					this.StartRoutine("Idle", HawkIdle());
					yield break;
				}
			}

			// Attack
			if (this.InRangeOf(_target, attackRange) && this.CanUseSkill(skill, _target))
			{
				DebugLog($"Attack() - Executing skill {skill.Id} on {_target.Name}");
				yield return this.TurnTowards(_target);
				yield return this.UseSkill(skill, _target);
				SetLastActionTime();
			}
			else
			{
				DebugLog($"Attack() - Cannot use skill: InRange={this.InRangeOf(_target, attackRange)}, CanUse={this.CanUseSkill(skill, _target)}");
			}

			yield return true;
		}

		DebugLog("Attack() - Attack loop ended");
		_target = null;
		this.CheckEnemies();

		if (_target != null)
		{
			DebugLog($"Attack() - Found new target: {_target.Name}");
			yield break;
		}

		DebugLog("Attack() - No new targets, returning to idle");
		this.StartRoutine("Idle", HawkIdle());
	}

	#endregion

	#region Flying Behavior

	/// <summary>
	/// Notifies the movement component about flying state.
	/// </summary>
	private void NotifyFlying(bool flying, float height = DefaultHawkHeight)
	{
		DebugLog($"NotifyFlying() - flying={flying}, height={height}");
		if (this.Entity.Components.TryGet<MovementComponent>(out var movement))
		{
			Send.ZC_FLY_OPTION(this.Entity, true, true, true, false);
			movement.NotifyFlying(flying, height, 1, 1.5f);
		}
		else
		{
			DebugLog("NotifyFlying() - WARNING: No MovementComponent found!");
		}
	}

	/// <summary>
	/// Makes the hawk fly away and become hidden.
	/// </summary>
	public IEnumerable FlyAway()
	{
		DebugLog("FlyAway() - Starting fly away sequence");

		if (IsFlyingAway || IsHidden)
		{
			DebugLog($"FlyAway() - Already flying away or hidden (FlyingAway={IsFlyingAway}, Hidden={IsHidden})");
			yield break;
		}

		// Check for roost
		if (this.TryGetMaster(out var owner))
		{
			var roostHandle = (int)owner.GetTempVar("HAWK_ROOST");
			if (roostHandle != 0)
			{
				DebugLog("FlyAway() - Roost exists, canceling fly away");
				LockHawkAction(false);
				yield break;
			}

			// Falconer20: Hawk Hunt
			if (owner.IsAbilityActive(AbilityId.Falconer20))
			{
				DebugLog("FlyAway() - Falconer20 (Hawk Hunt) active, canceling fly away");
				LockHawkAction(false);
				yield break;
			}
		}

		IsFlyingAway = true;

		var flyPos = GetRandomFlyPosition(owner, FlyAwayDistance);
		DebugLog($"FlyAway() - Flying to position: {flyPos}");
		NotifyFlying(true, FlyAwayHeight);

		yield return this.MoveTo(flyPos, wait: false);
		yield return this.Wait(5000);

		if (!IsFlyingAway)
		{
			DebugLog("FlyAway() - Flying away was interrupted");
			yield break;
		}

		IsHidden = true;
		IsFlyingAway = false;
		LockHawkAction(false);
		DebugLog("FlyAway() - Hawk is now hidden");
	}

	/// <summary>
	/// Makes the hawk unhide and return to its owner.
	/// </summary>
	public IEnumerable Unhide(ICombatEntity target)
	{
		DebugLog($"Unhide() - Starting unhide sequence (target: {target?.Name ?? "None"})");

		if (!IsHidden && !IsFlyingAway)
		{
			DebugLog("Unhide() - Not hidden or flying away, nothing to do");
			yield break;
		}

		NotifyFlying(true, DefaultHawkHeight);

		if (IsFlyingAway)
		{
			DebugLog("Unhide() - Interrupting fly away");
			IsFlyingAway = false;
		}
		else
		{
			var pos = GetRandomFlyPosition(target, 250);
			DebugLog($"Unhide() - Teleporting to position: {pos}");
			this.Entity.Position = pos;
			Send.ZC_SET_POS(this.Entity);
		}

		IsHidden = false;
		LockHawkAction(false);

		yield return this.MoveTo(target.Position, wait: false);
		DebugLog("Unhide() - Hawk is now visible");
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
		var shouldFollow = distance > parentRange;

		if (shouldFollow)
		{
			DebugLog($"ShouldFollowOwner() - Yes (distance={distance:F1}, threshold={parentRange}, ownerMoving={isOwnerMoving})");
		}

		return shouldFollow;
	}

	private IEnumerable FollowOwner(ICombatEntity owner)
	{
		var distance = this.Entity.Position.Get2DDistance(owner.Position);
		DebugLog($"FollowOwner() - Starting follow (distance={distance:F1})");

		if (distance >= TeleportDistance)
		{
			var teleportPos = GetRandomFlyPosition(owner, 250);
			DebugLog($"FollowOwner() - Teleporting (distance={distance:F1} >= {TeleportDistance}) to {teleportPos}");
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
			{
				DebugLog($"FollowOwner() - Reached owner (distance={dist:F1})");
				yield break;
			}

			yield return this.MoveTo(owner.Position, wait: false);
			SetLastActionTime();
			yield return this.Wait(200);
		}

		DebugLog("FollowOwner() - Follow loop ended (max iterations reached)");
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
		var isOwnerMoving = IsOwnerMoving(owner);
		var timeSinceLastAction = (DateTime.UtcNow - _lastActionTime).TotalSeconds;

		if (IsResting)
		{
			// Check if owner moved or time expired
			var ownerMoved = _restOwnerPos != Position.Zero &&
							 owner.Position.Get2DDistance(_restOwnerPos) > 5;

			if (isOwnerMoving || ownerMoved || timeSinceLastAction > RestDuration)
			{
				DebugLog($"ProcessRestBehavior() - Ending rest (ownerMoving={isOwnerMoving}, ownerMoved={ownerMoved}, time={timeSinceLastAction:F1}s)");
				yield return Unrest();
			}
		}
		else
		{
			if (timeSinceLastAction > TimeBeforeRest)
			{
				DebugLog($"ProcessRestBehavior() - Inactive for {timeSinceLastAction:F1}s (threshold={TimeBeforeRest}s)");
				SetLastActionTime();

				if (!isOwnerMoving)
				{
					var timeSinceLastSit = (DateTime.UtcNow - _lastSitTime).TotalSeconds;
					if (timeSinceLastSit < 40)
					{
						// Wander randomly near owner
						var wanderPos = owner.Position.GetRandomInRange2D(30, 50);
						DebugLog($"ProcessRestBehavior() - Wandering to {wanderPos} (timeSinceLastSit={timeSinceLastSit:F1}s)");
						yield return this.MoveTo(wanderPos, wait: false);
						LockHawkAction(false);

						// Set next action time with random offset (Lua: imcTime.GetAppTime() - IMCRandom(2, 7))
						_lastActionTime = DateTime.UtcNow - TimeSpan.FromSeconds(RandomProvider.Get().Next(2, 7));
					}
					else
					{
						// Rest with owner
						DebugLog($"ProcessRestBehavior() - Starting rest (timeSinceLastSit={timeSinceLastSit:F1}s)");
						yield return RestWithOwner(owner, "Dummy_pet_hawk_R", "IS_RESTING");
					}
				}
			}
		}
	}

	/// <summary>
	/// Attach hawk to owner/roost and play sit animation
	/// </summary>
	private IEnumerable RestWithOwner(ICombatEntity owner, string sitNodeName, string successExPropName)
	{
		DebugLog("RestWithOwner() - Starting rest sequence");
		LockHawkAction(true, "RestWithOwner");

		for (var i = 0; i < 20; i++)
		{
			var dist = this.Entity.Position.Get2DDistance(owner.Position);
			if (dist < 10)
			{
				DebugLog($"RestWithOwner() - Reached owner (distance={dist:F1})");
				break;
			}
			yield return this.MoveTo(owner.Position, wait: false);
			yield return this.Wait(500);
		}

		DebugLog("RestWithOwner() - Lowering flight height");
		NotifyFlying(true, 1);
		yield return this.Wait(1000);

		DebugLog("RestWithOwner() - Playing sit animation");
		Send.ZC_PLAY_ANI(this.Entity, "ASTD_TO_SIT", true);

		if (IsOwnerMoving(owner))
		{
			DebugLog("RestWithOwner() - Owner started moving, canceling rest");
			LockHawkAction(false);
			yield return Unrest();
			yield break;
		}

		// Match owner's direction
		this.Entity.Direction = owner.Direction;
		Send.ZC_ROTATE(this.Entity);

		// AttachToObject(self, owner, sitNodeName, "None", 1, 1, 0, 0, 0, "SIT", playAttachAnimWhenCompletelyAttached)
		DebugLog($"RestWithOwner() - Attaching to {owner.Name} at node {sitNodeName}");
		this.Entity.AttachToObject(owner, sitNodeName, "None", attachSec: 1, attachAnim: "SIT");
		Send.ZC_NORMAL.SetHeight(this.Entity, 0);

		// AddAttachAnimList for idle animations while attached
		if (sitNodeName == "Dummy_hawk")
		{
			// Roost uses two idle animations
			//Send.ZC_ADD_ATTACH_ANIM_LIST(this.Entity, "SIT_IDLE", "SIT_IDLE2");
		}
		else
		{
			// Owner rest uses no additional animations
			//Send.ZC_ADD_ATTACH_ANIM_LIST(this.Entity);
		}

		yield return this.Wait(1000);

		Send.ZC_NORMAL.AutoDetachWhenTargetMove(this.Entity, true, "SIT");
		owner.PlayEffectNode("F_smoke109_2", 1, sitNodeName);
		owner.PlayEffectNode("F_archer_hawk_fether_sit", 1, sitNodeName);

		IsResting = true;
		_lastSitTime = DateTime.UtcNow;
		_lastActionTime = DateTime.UtcNow + TimeSpan.FromSeconds(RandomProvider.Get().Next(15, 25));

		DebugLog("RestWithOwner() - Now resting");
		LockHawkAction(false);
	}

	private IEnumerable Unrest()
	{
		if (!IsResting && !IsOnRoost)
		{
			DebugLog("Unrest() - Not resting or on roost, nothing to do");
			yield break;
		}

		DebugLog("Unrest() - Getting up");
		Send.ZC_NORMAL.AutoDetachWhenTargetMove(this.Entity, false, "SIT");
		this.Entity.AttachToObject(null, "None", "None", attachSec: 1);
		NotifyFlying(true, DefaultHawkHeight);
		Send.ZC_PLAY_ANI(this.Entity, "SIT_TO_ASTD");

		IsResting = false;
		IsOnRoost = false;
		_restOwnerPos = Position.Zero;

		yield return this.Wait(1000);

		// Detach again to ensure clean state
		this.Entity.AttachToObject(null, "None", "None", attachSec: 1);


		// Use for other "flying" things
		Send.ZC_FLY_OPTION(this.Entity, true, true, true, false);

		DebugLog("Unrest() - Complete");
	}

	private void UnrestIfNeeded()
	{
		if (IsResting || IsOnRoost)
		{
			DebugLog("UnrestIfNeeded() - Triggering unrest");
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
			{
				DebugLog("CheckAndSitOnRoost() - No roost found but was on roost, unresting");
				this.ExecuteOnce(Unrest());
			}
			return false;
		}

		var roost = this.Entity.Map.GetCombatEntity(roostHandle);
		if (roost == null || roost.IsDead)
		{
			DebugLog($"CheckAndSitOnRoost() - Roost entity invalid (handle={roostHandle})");
			if (IsOnRoost)
				this.ExecuteOnce(Unrest());
			return false;
		}

		if (IsFlyingAway || IsHidden)
		{
			DebugLog("CheckAndSitOnRoost() - Hawk flying/hidden, triggering unhide");
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
			{
				DebugLog($"CheckAndSitOnRoost() - Owner too far from roost ({distFromRoost:F1} >= {roostSitDist}), unresting");
				this.ExecuteOnce(Unrest());
			}
			return false;
		}

		if (!IsOnRoost)
		{
			DebugLog($"CheckAndSitOnRoost() - Sitting on roost (distance={distFromRoost:F1}, threshold={roostSitDist})");
			this.ExecuteOnce(SitOnRoost(roost));
		}

		return true;
	}

	private IEnumerable SitOnRoost(ICombatEntity roost)
	{
		DebugLog($"SitOnRoost() - Starting (roost: {roost?.Name ?? "Unknown"})");
		LockHawkAction(true, "SitOnRoost");

		for (var i = 0; i < 20; i++)
		{
			var dist = this.Entity.Position.Get2DDistance(roost.Position);
			if (dist < 10)
			{
				DebugLog($"SitOnRoost() - Reached roost (distance={dist:F1})");
				break;
			}
			yield return this.MoveTo(roost.Position, wait: false);
			yield return this.Wait(500);
		}

		DebugLog("SitOnRoost() - Lowering flight height");
		NotifyFlying(true, 1);
		yield return this.Wait(1000);

		DebugLog("SitOnRoost() - Playing sit animation");
		Send.ZC_PLAY_ANI(this.Entity, "ASTD_TO_SIT");
		yield return this.Wait(1000);

		roost.PlayEffect("F_smoke109_2", scale: 1.5f);

		IsOnRoost = true;
		_lastSitTime = DateTime.UtcNow;

		DebugLog("SitOnRoost() - Now on roost");
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
		{
			DebugLog("CheckFirstStrike() - Already using skill, skipping");
			return;
		}

		if (owner.Components.TryGet<CombatComponent>(out var combat) && !combat.AttackState)
			return;

		var target = GetOwnerTarget(owner);
		if (target == null || this.EntityGone(target))
			return;

		DebugLog($"CheckFirstStrike() - First Strike triggered, target: {target.Name}");

		// Falconer14: Remove Sonic Strike
		if (!owner.IsAbilityActive(AbilityId.Falconer14)
			&& owner.TryGetSkill(SkillId.Falconer_BlisteringThrash, out var skill) && !skill.IsOnCooldown)
		{
			DebugLog("CheckFirstStrike() - Triggering BlisteringThrash");
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
				DebugLog($"CheckFirstStrike() - Triggering Hovering on approaching enemy: {approachingEnemy.Name}");
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
				{
					DebugLog($"FindEnemyApproachingOwner() - Found approaching enemy: {enemy.Name}");
					return enemy;
				}
			}
		}

		return null;
	}

	#endregion

	#region Skill Helpers

	public void LockHawkAction(bool locked, string scriptName = "None")
	{
		DebugLog($"LockHawkAction() - locked={locked}, script={scriptName}");
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
		DebugLog($"TriggerHawkSkill() - skill={skillName}, target={target?.Name ?? "None"}, isFirstStrike={isFirstStrike}");

		var currentScript = GetCurrentSkillScript();
		if (currentScript != "None" && IsUsingSkillFlag)
		{
			DebugLog($"TriggerHawkSkill() - Stopping current script: {currentScript}");
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
			default:
				DebugLog($"TriggerHawkSkill() - Unknown skill: {skillName}");
				break;
		}
	}

	private void StopCurrentSkillScript(string scriptName)
	{
		DebugLog($"StopCurrentSkillScript() - Stopping: {scriptName}");
		LockHawkAction(false);
		NotifyFlying(true, DefaultHawkHeight);
	}

	#endregion

	#region Hawk Skill Routines

	private IEnumerable HawkBlisteringThrash(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		DebugLog($"HawkBlisteringThrash() - Starting (target={target?.Name ?? "None"}, firstStrike={isFirstStrike})");
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
		{
			DebugLog("HawkBlisteringThrash() - Sending CollisionAndBack");
			Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);
		}

		this.Entity.PlayEffect("F_archer_blisteringthrash_slash", scale: 1f);

		yield return this.Wait((int)((goTime + backTime) * 1000));

		SetLastActionTime();
		DebugLog("HawkBlisteringThrash() - Starting fly away");
		yield return FlyAway();
	}

	private IEnumerable HawkHovering(ICombatEntity owner, ICombatEntity target, Skill skill, bool isFirstStrike)
	{
		DebugLog($"HawkHovering() - Starting (target={target?.Name ?? "None"}, firstStrike={isFirstStrike})");
		LockHawkAction(true, "Hovering");
		yield return this.Wait(1000);
		UnrestIfNeeded();

		// Hovering is primarily managed by the skill handler's pad
		// This just handles the hawk's position and animation

		SetLastActionTime();
		LockHawkAction(false);
		DebugLog("HawkHovering() - Complete");
	}

	private IEnumerable HawkCombination(ICombatEntity owner, ICombatEntity target, Skill skill)
	{
		DebugLog($"HawkCombination() - Starting (target={target?.Name ?? "None"})");
		LockHawkAction(true, "Combination");
		UnrestIfNeeded();

		var targetPos = target.Position;
		var syncKey = this.Entity.GenerateSyncKey();

		var goTime = 0.5f;
		var backTime = 0.5f;

		DebugLog("HawkCombination() - First attack");
		Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		yield return this.Wait((int)((goTime + backTime) * 1500));

		DebugLog("HawkCombination() - Second attack");
		syncKey = this.Entity.GenerateSyncKey();
		Send.ZC_NORMAL.CollisionAndBack(this.Entity, target, syncKey, "HOVERING_SHOT", goTime, 7f, backTime, 0.7f, 30f, true);

		yield return this.Wait((int)((goTime + backTime) * 1000));

		LockHawkAction(false);
		SetLastActionTime();
		DebugLog("HawkCombination() - Complete");
	}

	#endregion

	#region Overridden Check Methods with Debug Logging

	protected override void CheckEnemies()
	{
		var mostHated = this.GetMostHated();
		if (mostHated != null && (_target != mostHated || _target == null))
		{
			DebugLog($"CheckEnemies() - New target found: {mostHated.Name}");
			if (_target != mostHated)
			{
				// Target acquired time would be set in base class
			}
			_target = mostHated;
			this.StartRoutine("StopAndAttack", this.StopAndAttack());
		}
	}

	protected override void CheckTarget()
	{
		if (this.Entity.IsLocked(LockType.Attack))
		{
			return;
		}

		if (this.EntityGone(_target) || !this.InRangeOf(_target, MaxChaseDistance))
		{
			DebugLog($"CheckTarget() - Target gone or out of range (MaxChaseDistance={MaxChaseDistance})");
			_target = null;
			this.StartRoutine("Idle", HawkIdle());
			return;
		}

		if (!this.IsHating(_target))
		{
			DebugLog($"CheckTarget() - No longer hating target");
			_target = null;
			this.StartRoutine("Idle", HawkIdle());
		}
	}

	protected override void CheckMaster()
	{
		if (_target == null)
			return;

		if (!this.TryGetMaster(out var master))
			return;

		if (this.EntityGone(master) || !this.InRangeOf(master, MaxMasterDistance))
		{
			DebugLog($"CheckMaster() - Master gone or out of range (MaxMasterDistance={MaxMasterDistance})");
			_target = null;
			this.RemoveAllHate();
			this.StartRoutine("Idle", HawkIdle());
		}
	}

	#endregion
}
