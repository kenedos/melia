using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Geometry;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
namespace Melia.Zone.Skills.Helpers
{
	public static class SkillDamageHelper
	{
		public enum PosType
		{
			Self = 0,
			Target = 1,
			TargetDirection = 2,
			TargetFront = 3,
			TargetBack = 4,
			TargetRandom = 5,
			TargetFrontRandom = 6,
			TargetBackRandom = 7,
			TargetDistance = 8,
			TargetHeight = 9,
			TargetRandomDistance = 10,
		}

		public static Position GetNearestPositionWithinDistance(this Position currentPosition, Position targetPosition, float maxDistance)
		{
			// Calculate the distance between the current and target positions
			var distance = currentPosition.Get2DDistance(targetPosition);

			// If the target is within the max distance, return the target position
			if (distance <= maxDistance)
				return targetPosition;

			// Calculate the direction vector from the current position to the target
			var directionVector = targetPosition - currentPosition;

			// Normalize the direction vector to get a unit vector
			var normalizedDirection = directionVector.Normalized();

			// Scale the unit vector by the max distance to get the nearest position
			var nearestPosition = currentPosition + (normalizedDirection * maxDistance);

			// Recalculate the distance to ensure it's within the maximum distance
			var nearestDistance = (float)currentPosition.Get2DDistance(nearestPosition);
			if (nearestDistance > maxDistance)
			{
				// Adjust the position to be exactly maxDistance away if necessary
				nearestPosition = currentPosition + (normalizedDirection * (maxDistance / nearestDistance) * maxDistance);
			}

			return nearestPosition;
		}


		public static Position GetRelativePosition(PosType posType, ICombatEntity caster, double distance = 0, double angle = 0, int rand = 0, int height = 0)
		=> GetRelativePosition(posType, caster, caster, distance, angle, rand, height);

		public static Position GetRelativePosition(PosType posType, ICombatEntity caster, ICombatEntity target, double distance = 0, double angle = 0, int rand = 0, int height = 0)
		{
			var distanceF = (float)distance;
			var angleF = (float)angle;

			if (target == null)
			{
				Log.Error("GetRelativePosition: Target is null, returning Caster position");
				return caster.Position;
			}

			switch (posType)
			{
				case PosType.Self:
					return caster.Position.GetRelative(target.Direction.AddDegreeAngle(angleF), distanceF) + new Position(0, height, 0);

				case PosType.Target:
					return target.Position;

				case PosType.TargetDirection:
					return caster.Position.GetRelative(target.Position, distanceF);

				case PosType.TargetFront:
					return target.Position.GetRelative(target.Direction.Backwards.AddDegreeAngle(angleF), distanceF) + new Position(0, height, 0);

				case PosType.TargetBack:
					return target.Position.GetRelative(target.Direction.AddDegreeAngle(angleF), distanceF) + new Position(0, height, 0);

				case PosType.TargetRandom:
					var randomPos = target.Position.GetRandomInRange2D((int)rand, RandomProvider.Get());
					randomPos.Y += height;
					return randomPos;

				case PosType.TargetFrontRandom:
					var randomAngle = RandomProvider.Get().Next(-45, 46) + angleF;
					var randomDirection = target.Direction.AddDegreeAngle(randomAngle);
					var randomFrontPos = target.Position.GetRelative(randomDirection, distanceF);
					randomFrontPos.Y += height;
					return randomFrontPos;

				case PosType.TargetDistance:
					var relativePos = caster.Position.GetRelative(target.Position, distanceF + RandomProvider.Get().Next(rand));
					relativePos.Y += height;
					return relativePos;

				case PosType.TargetHeight:
					return target.Position + new Position(0, height, 0);

				case PosType.TargetRandomDistance:
					var minDist = (int)distance / 2;
					var maxDist = (int)distance;
					var randomDistPos = target.Position.GetRandomInRange2D(minDist, maxDist, RandomProvider.Get());
					randomDistPos.Y += height;
					return randomDistPos;

				default:
					return target.Position;
			}
		}

		/// <summary>
		/// Performs the effect of a force attack.
		/// Does not actually deal damage.
		/// </summary>
		public static async Task ForceAttackEffect(
			ICombatEntity caster,
			ICombatEntity target,
			Skill skill,
			float hitDelay = 0
			)
		{
			if (caster == null || caster.IsDead)
				return;

			if (target == null || target.IsDead)
				return;

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);

			await Task.Yield();
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="hitDelay">Delay before the hit occurs</param>
		/// <param name="damageDelay">Delay before damage is applied</param>
		/// <param name="hits">List of SkillHitInfo objects</param>
		/// <param name="modifySkillHitResult">Optional callback to modify 
		/// skillHitResult before applying damage</param>
		public static async Task SkillAttack(
			ICombatEntity caster,
			Skill skill,
			IShapeF splashArea,
			float hitDelay = 0,
			float damageDelay = 0,
			List<SkillHitInfo> hits = null,
			Func<Skill, ICombatEntity, ICombatEntity, SkillHitResult, SkillHitResult> modifySkillHitResult = null,
			SkillModifier skillModifier = null)
		{
			if (caster is Character character && (character.Variables.Temp.GetBool("Melia.RangePreview")))
			{
				if (skill.Data.ShootTime < SkillConstants.MaxShootTimeForPreview)
					Debug.ShowShape(caster.Map, splashArea, skill.Data.ShootTime);
				else
					Debug.ShowShape(caster.Map, splashArea, SkillConstants.DefaultDebugShapeDuration);
			}
			if (caster is Mob)
			{
				if (skill.Data.ShootTime < SkillConstants.MaxShootTimeForPreview)
					Debug.ShowShape(caster.Map, splashArea, skill.Data.ShootTime);
				else
					Debug.ShowShape(caster.Map, splashArea, SkillConstants.DefaultDebugShapeDuration);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(hitDelay));

			if (caster.IsDead) return;

			if (skillModifier == null)
			{
				skillModifier = SkillModifier.Default;
			}

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			hits ??= new List<SkillHitInfo>();


			var skillHitDelay = skill.Properties.HitDelay;
			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				if (target == null || target.IsDead || !caster.CanAttack(target))
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill, skillModifier);

				// Apply the modification callback if provided
				if (modifySkillHitResult != null)
					skillHitResult = modifySkillHitResult(skill, caster, target, skillHitResult);

				target.TakeDamage(skillHitResult.Damage, caster);
				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(damageDelay), skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;
				if (skillModifier == SkillModifier.Default)
					skillHit.VarInfoCount = 0;
				hits.Add(skillHit);
			}
			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Creates a pad at the given position.
		/// </summary>
		public static Pad SkillCreatePad(ICombatEntity caster, Skill skill, Position position, float angle, string padName, bool isActive = true, float range = 0)
		{
			if (caster.IsDead)
				return null;

			if (caster.Map.Ground.TryGetHeightAt(position, out var height))
				position.Y = height;

			var pad = new Pad(caster, skill, padName, position, new Direction(angle), range);

			if (isActive)
				pad.Activate();

			if (caster is Character character && (character.Variables.Temp.GetBool("Melia.RangePreview")))
			{
				if (skill.Data.ShootTime < SkillConstants.MaxPadShootTimeForPreview)
					Debug.ShowShape(caster.Map, pad.Area, skill.Data.ShootTime);
				else
					Debug.ShowShape(caster.Map, pad.Area, SkillConstants.ShortDebugShapeDuration);
			}
			if (caster is Mob)
			{
				if (skill.Data.ShootTime < SkillConstants.MaxPadShootTimeForPreview)
					Debug.ShowShape(caster.Map, pad.Area, skill.Data.ShootTime);
				else
					Debug.ShowShape(caster.Map, pad.Area, SkillConstants.ShortDebugShapeDuration);
			}

			return pad;
		}

		public static void SkillRemovePad(ICombatEntity caster, Skill skill)
		{
			var padHandle = skill.Vars.GetInt($"Melia.{skill.Id}.PadHandle");
			if (caster.Map.TryGetPad(padHandle, out var pad))
			{
				pad.Destroy();
				skill.Vars.Remove($"Melia.{skill.Id}.PadHandle");
			}
		}

		public static void SkillHitCircle(ICombatEntity caster, Skill skill, Position position, float range, List<SkillHitInfo> hits = null)
		{
			if (caster == null || caster.IsDead)
				return;

			var targets = caster.SelectObjects(position, range);

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;

				var skillHitResult = SCR_SkillHit(caster, target, skill);
				if (skillHitResult.Result == HitResultType.Dodge)
					continue;
				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
				Send.ZC_HIT_INFO(caster, target, hitInfo);
				hits?.Add(new SkillHitInfo(caster, target, skill, skillHitResult, hitInfo));
			}
		}

		public static void SkillHitSquare(ICombatEntity caster, Skill skill, Position startPos, Position endPos, float width, bool setTargets = false)
		{
			if (!setTargets)
			{
				var targets = caster.SelectObjectBySquareCoor(RelationType.Enemy, startPos, endPos, width);

				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(skillHitResult.Damage, caster);

					var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
					Send.ZC_HIT_INFO(caster, target, hitInfo);
				}
			}
			else
			{
				var targets = caster.GetTargets();

				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(skillHitResult.Damage, caster);

					var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
					Send.ZC_HIT_INFO(caster, target, hitInfo);
				}
			}
		}

		public static async Task TeleportToPosition(ICombatEntity caster, Position position, string groundEffect = "None", float groundScale = 1f)
		{
			if (caster == null || caster.IsDead)
				return;

			if (!caster.Map.Ground.TryGetNearestValidPosition(position, out var validPosition))
				validPosition = caster.Position;

			if (groundEffect != null && groundEffect != "None")
				await caster.PlayEffectToGround(groundEffect, validPosition, groundScale);

			caster.Position = validPosition;
			Send.ZC_SET_POS(caster, validPosition);
		}

		public static List<ICombatEntity> SkillSelectEnemiesInCircle(ICombatEntity caster, Position position, float radius, int maxTargets = 0)
		{
			var circle = new CircleF(position, radius);
			return caster.Map.GetAttackableEnemiesIn(caster, circle, maxTargets);
		}

		public static List<ICombatEntity> SkillSelectAlliesInCircle(ICombatEntity caster, Position position, float radius, int maxTargets = 0)
		{
			var circle = new CircleF(position, radius);
			return caster.Map.GetAliveAlliedEntitiesIn(caster, circle, maxTargets);
		}

		public static List<ICombatEntity> SkillSelectEnemiesInSquare(ICombatEntity caster, Position originPos, float angle, float distance, float width, int maxTargets = 0)
		{
			var direction = caster.Direction.AddDegreeAngle(angle);
			var square = new Square(originPos, direction, distance, width);
			return caster.Map.GetAttackableEnemiesIn(caster, square, maxTargets);
		}

		public static List<ICombatEntity> SkillSelectEnemiesInFan(ICombatEntity caster, Position position, float angle, float distance, int maxTargets = 0)
		{
			var direction = caster.Direction;
			var fan = new Fan(position, direction, distance, angle);
			return caster.Map.GetAttackableEnemiesIn(caster, fan, maxTargets);
		}

		/// <summary>
		/// Resets skill cooldown
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public static void SkillResetCooldown(Skill skill, ICombatEntity caster)
		{
			caster.RemoveCooldown(skill.CooldownData.Id);
		}

		public static void MonsterSkillCreateMobPC(Skill skill, ICombatEntity caster, string className,
			Position position, float angle, string name, string behaviorTree, int levelOffset, float lifeTime = 0f,
			string simpleAiName = "None", string monsterProperties = "", int count = 0)
		{
			if (caster == null || caster.IsDead)
				return;

			var characterCount = caster.Map.GetCharacters(a => caster.Layer == a.Layer)?.Length ?? 0;

			if (characterCount < 3)
			{
				MonsterSkillCreateMob(skill, caster, className, position, angle, name, behaviorTree, levelOffset, lifeTime, simpleAiName, monsterProperties);
			}
			else
			{
				var bossPos = caster.Position;
				var targetOffset = bossPos - position;

				var maxCount = (characterCount - 2) + (count - 1);
				for (var i = 0; i <= maxCount; i++)
					MonsterSkillCreateMob(skill, caster, className, position + targetOffset * i, angle, name, behaviorTree, levelOffset, lifeTime, simpleAiName, monsterProperties);
			}
		}

		public static Mob MonsterSkillCreateMob(Skill skill, ICombatEntity caster,
			string className, Position position, float angle, string name,
			string behaviorTree, int levelOffset, float lifeTime, string simpleAiName, string monsterProperties)
		{
			if (caster.IsDead)
				return null;

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var monsterData))
				return null;

			Mob mob;
			if (caster is Character)
			{
				mob = new Summon(caster, monsterData.Id, RelationType.Friendly);
			}
			else
			{
				mob = new Mob(monsterData.Id, RelationType.Enemy);
				mob.Tendency = TendencyType.Aggressive;
			}

			mob.HasDrops = false;
			mob.HasExp = false;

			if (!string.IsNullOrEmpty(name) && name != mob.Name)
				mob.Name = name;

			if (!string.IsNullOrEmpty(behaviorTree))
				mob.Properties.SetString(PropertyName.BTree, behaviorTree);

			mob.Properties[PropertyName.EXP_Rate] = 0;
			mob.Properties[PropertyName.JEXP_Rate] = 0;

			var lv = 0;

			if (caster is Character)
				lv = caster.Level + levelOffset;
			else
				lv = (int)caster.Properties[PropertyName.Lv] + levelOffset;

			if (lv <= 0)
				mob.Properties[PropertyName.Lv] = 1;
			else
				mob.Properties[PropertyName.Lv] = lv;

			if (!string.IsNullOrEmpty(monsterProperties))
				mob.ApplyPropList(monsterProperties, caster, skill);

			var range = 1;

			// Validate spawn position is within map boundaries
			if (!caster.Map.Ground.TryGetNearestValidPosition(position, out var validPosition))
				validPosition = caster.Position;

			mob.Position = validPosition;
			mob.Direction = caster.Direction.AddDegreeAngle(angle);
			mob.Layer = caster.Layer;

			mob.Vars.SetInt("Melia.Summon.SkillLevel", skill.Level);

			mob.OwnerHandle = caster.Handle;
			mob.AssociatedHandle = caster.Handle;
			mob.Faction = caster.Faction;
			if (lifeTime > 0)
				mob.Components.Add(new LifeTimeComponent(mob, TimeSpan.FromSeconds(lifeTime)));

			if (!string.IsNullOrEmpty(simpleAiName) && simpleAiName != "None")
			{
				if (!AiScript.Exists(simpleAiName))
				{
					Log.Debug("MonsterSkillCreateMob: Missing Simple AI: {0}", simpleAiName);
				}
				else
				{
					mob.Components.Add(new MovementComponent(mob));
					mob.Components.Add(new AiComponent(mob, simpleAiName, caster));
				}
			}
			else if (!string.IsNullOrEmpty(behaviorTree) && behaviorTree != "None")
			{
				if (!AiScript.Exists(behaviorTree))
				{
					Log.Debug("MonsterSkillCreateMob: Missing AI Script: {0}", behaviorTree);
					behaviorTree = "BasicMonster";
				}
				mob.Components.Add(new MovementComponent(mob));
				mob.Components.Add(new AiComponent(mob, behaviorTree, caster));
			}

			if (caster is Mob masterMob)
			{
				mob.Components.Add(new LifeTimeComponent(mob, TimeSpan.FromMinutes(3)));
				var target = masterMob.CombatState.GetTopAttackerByDamage();
				if (target != null)
					mob.InsertHate(target);
				Action<Mob, ICombatEntity> onMasterDied = null;
				onMasterDied = (Mob arg1, ICombatEntity arg2) =>
				{
					masterMob.Died -= onMasterDied;
					mob?.Kill(null);
				};
				masterMob.Died += onMasterDied;
			}
			else if (caster is Character character && mob is Summon summon)
			{
				character.Summons.AddSummon(summon);
			}

			mob.Vars.SetInt("Melia.Summon.Skill", (int)skill.Id);
			mob.Vars.Set("Melia.Summoner.Owner", caster);
			caster.Map.AddMonster(mob);
			mob.FromGround = true;
			mob.DelayEnterWorld();
			mob.EnterDelayedActor();

			return mob;
		}


		public static async Task MissileFall(ICombatEntity caster, Skill skill, Position position, MissileConfig config, List<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			skill.Vars.Set("Melia.Skill.vAngle", config.VerticalAngle);

			caster.MissileFall(skill.Data.ClassName, config.Effect.Name, config.Effect.Scale, position, config.Range, config.DelayTime, config.FlyTime, config.Height, config.Easing, config.EndEffect.Name, config.EndEffect.Scale, config.StartEasing, config.GroundEffect.Name, config.GroundEffect.Scale);

			if (config.DelayTime > 0)
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale, 0, config.DelayTime);

			caster.SetTempVar("CHECK_SKL_KD_PROP", 1);
			await DoDamageOverTime(skill, caster, position, (config.FlyTime + config.DelayTime + config.HitStartFix), config.Range, config.HitTime, config.HitCount, config.DotEffect.Name, config.DotEffect.Scale, config.KnockdownPower, (int)config.KnockType, hits: hits);
		}

		public static async Task MissilePadThrow(Skill skill, ICombatEntity caster, Position position, MissileConfig config, float padAngle, string padName)
		{
			if (caster.IsDead)
				return;

			if (!string.IsNullOrEmpty(config.GroundEffect.Name) && config.GroundEffect.Name != "None")
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale, 0, config.GroundDelay);

			Send.ZC_NORMAL.SkillProjectile(caster, position,
				config.Effect.Name, config.Effect.Scale,
				config.EndEffect.Name, config.EndEffect.Scale, config.Range,
				TimeSpan.FromSeconds(config.FlyTime), TimeSpan.FromSeconds(config.DelayTime),
				config.Gravity, config.Speed, TimeSpan.FromSeconds(config.GroundDelay), config.GroundEffect.Scale, config.GroundEffect.Name);

			if (config.EffectMoveDelay > 0)
			{
				var delayMs = config.EffectMoveDelay * 1000;
				await skill.Wait(TimeSpan.FromMilliseconds(delayMs));
			}

			if (config.HitCount > 0)
			{
				caster.SetTempVar("CHECK_SKL_KD_PROP", 1);
				await DoDamageOverTime(skill, caster, position, config.FlyTime + config.DelayTime, config.Range, config.HitTime, config.HitCount, config.DotEffect.Name, config.DotEffect.Scale, 0);
			}
			else
			{
				var sleepMS = (int)((config.FlyTime + config.DelayTime) * 1000);
				if (sleepMS > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(sleepMS));
			}

			var pad = new Pad(caster, skill, padName, position, caster.Direction, config.Range);
			pad.Activate();
		}

		public static async Task MissileThrow(Skill skill, ICombatEntity caster, Position position, MissileConfig config, List<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			if (!string.IsNullOrEmpty(config.TargetEffect.Name) && config.TargetEffect.Name != "None")
				caster.PlayEffectToGround(config.TargetEffect.Name, position, config.TargetEffect.Scale, 0, config.TargetEffectDuration);

			if (config.GroundDelay > 0)
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale, 0, config.GroundDelay);

			Send.ZC_NORMAL.SkillProjectile(caster, position,
				config.Effect.Name, config.Effect.Scale,
				config.EndEffect.Name, config.EndEffect.Scale, config.Range,
				TimeSpan.FromSeconds(config.FlyTime), TimeSpan.FromSeconds(config.DelayTime),
				config.Gravity, config.Speed, TimeSpan.FromSeconds(config.GroundDelay), config.GroundEffect.Scale, config.GroundEffect.Name);

			if (config.EffectMoveDelay > 0)
			{
				var delayMs = config.EffectMoveDelay * 1000;
				await skill.Wait(TimeSpan.FromMilliseconds(delayMs));
			}

			await DoDamageOverTime(skill, caster, position, config.FlyTime + config.DelayTime, config.Range, config.HitTime, config.HitCount, config.DotEffect.Name, config.DotEffect.Scale, 0, 0, config.InnerRange, hits);
		}

		public static async Task EffectAndHit(Skill skill, ICombatEntity caster, Position position, EffectHitConfig config, List<SkillHitInfo> hitResults = null)
		{
			if (caster.IsDead)
				return;

			skill.Vars.Set("Melia.Skill.vAngle", config.VerticalAngle);

			if (config.GroundEffect.Name != "None")
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale);

			if (config.PositionDelay > 0)
				await skill.Wait(TimeSpan.FromMilliseconds(config.PositionDelay));

			if (config.Effect.Name != "None")
				await caster.PlayEffectToGround(config.Effect.Name, position, config.Effect.Scale);

			if (config.CasterEffect.Name != null && config.CasterEffect.Name != "None")
			{
				if (config.CasterNodeName != null && config.CasterNodeName != "None")
					caster.PlayEffectNode(config.CasterEffect.Name, config.CasterEffect.Scale, config.CasterNodeName);
				else
					caster.PlayEffect(config.CasterEffect.Name, config.CasterEffect.Scale);
			}
			await DoDamageOverTime(skill, caster, position, config.Delay * 0.001f, config.Range, config.HitDuration, config.HitCount, "None", 1.0f, config.KnockdownPower, config.KnockType, config.InnerRange, hits: hitResults);
		}

		public static async Task EffectAndHitRangePreview(Skill skill, ICombatEntity caster, Position position, EffectHitConfig config, List<SkillHitInfo> hitResults = null)
		{
			if (caster.IsDead)
				return;

			if (config.GroundEffect.Name != "None")
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale);

			Send.ZC_START_RANGE_PREVIEW(caster, skill.Data.ClassName, TimeSpan.FromMilliseconds(config.PositionDelay), Shared.Data.Database.SplashType.Circle, caster.Position, caster.Direction, 0, config.Range);

			if (config.PositionDelay > 0)
				await skill.Wait(TimeSpan.FromMilliseconds(config.PositionDelay));

			await DoDamageOverTime(skill, caster, position, config.Delay * 0.001f, config.Range, config.HitDuration, config.HitCount, "None", 1.0f, config.KnockdownPower, hits: hitResults);
		}

		public static async Task PadDestruction(Skill skill, ICombatEntity caster, Position position, int padCount, float range, string padStyle, string effect, float effectScale, float hitRange, float knockdownPower, int relationBit)
		{
			if (caster == null || caster.IsDead)
				return;

			var pads = caster.Map.GetPadsAt(position, range);
			if (padStyle == "MINE")
				pads = pads.Where(a => a.Creator.Handle == caster.Handle).ToArray();
			var loopCnt = MathF.Min(pads.Length, padCount);

			var hitResults = new List<SkillHitInfo>();
			for (var i = 0; i < loopCnt; i++)
			{
				var pad = pads[i];
				if (pad == null)
					continue;

				var padPos = pad.Position;
				pad.Destroy();
				await caster.PlayEffectToGround(effect, padPos, effectScale, 0.0f);
				await DoDamageOverTime(skill, caster, padPos, 0, hitRange, 0, 1, "None", 1.0f, knockdownPower, hits: hitResults);
			}
		}

		public static async Task EffectHitArrow(Skill skill, ICombatEntity caster, Position startingPosition, Position endingPosition, ArrowConfig config, List<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			Send.ZC_NORMAL.PlayArrowEffect(caster, startingPosition, endingPosition,
				config.ArrowEffect.Name, config.ArrowEffect.Scale, config.ArrowSpacing, config.ArrowSpacingTime, config.ArrowLifeTime);

			if (config.PositionDelay > 0)
				await skill.Wait(TimeSpan.FromMilliseconds((int)config.PositionDelay));

			var dist = startingPosition.Get2DDistance(endingPosition);
			var hitPointCount = 1;

			if (config.HitEffectSpacing != 0)
				hitPointCount = (int)Math.Floor(dist / config.HitEffectSpacing);

			skill.Vars.Set("Melia.Skill.vAngle", config.VerticalAngle);
			for (var i = 0; i < hitPointCount; i++)
			{
				var di = (float)i / hitPointCount;
				var dx = startingPosition.X + (endingPosition.X - startingPosition.X) * di;
				var dy = startingPosition.Y + (endingPosition.Y - startingPosition.Y) * di;
				var dz = startingPosition.Z + (endingPosition.Z - startingPosition.Z) * di;

				await caster.PlayEffectToGround(config.HitEffect.Name, new Position(dx, dy, dz), config.HitEffect.Scale, config.HitTimeSpacing * i);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(config.Delay));
			for (var i = 0; i < hitPointCount; i++)
			{
				var di = (float)i / hitPointCount;
				var dx = startingPosition.X + (endingPosition.X - startingPosition.X) * di;
				var dy = startingPosition.Y + (endingPosition.Y - startingPosition.Y) * di;
				var dz = startingPosition.Z + (endingPosition.Z - startingPosition.Z) * di;

				await DoDamageOverTime(skill, caster, new Position(dx, dy, dz), config.HitTimeSpacing, config.Range, config.HitDuration, config.HitCount, "None", 1.0f, config.KnockdownPower, (int)config.KnockType, 0, hits);
			}
		}

		/// <summary>
		/// Performs multiple hits over time at a position, commonly used for
		/// channeled skills, ground effects, and delayed multi-hit abilities.
		/// </summary>
		/// <param name="skill">The skill being used.</param>
		/// <param name="caster">The entity performing the attack.</param>
		/// <param name="position">The center position of the damage area.</param>
		/// <param name="delayTime">Initial delay in seconds before damage starts.</param>
		/// <param name="range">The radius of the damage area.</param>
		/// <param name="hitTime">Total duration of all hits in milliseconds.</param>
		/// <param name="hitCount">Number of damage ticks.</param>
		/// <param name="effectName">Effect name for each tick.</param>
		/// <param name="effectScale">Effect scale.</param>
		/// <param name="knockdownPower">Knockback power.</param>
		/// <param name="knockType">Knockback type (0 = none).</param>
		/// <param name="innerRange">Inner range for donut areas.</param>
		/// <param name="hits">Optional list to collect hit results.</param>
		public static async Task DoDamageOverTime(Skill skill, ICombatEntity caster, Position position, float delayTime, float range,
			float hitTime, int hitCount, string effectName, float effectScale, float knockdownPower, int knockType = 0, float innerRange = 0, List<SkillHitInfo> hits = null)
		{
			if (caster.IsDead)
				return;

			var sleepMS = (int)(delayTime * 1000);
			if (sleepMS > 0)
				await skill.Wait(TimeSpan.FromMilliseconds(sleepMS));

			var sleepDelay = hitTime / hitCount;

			for (var i = 1; i <= hitCount; i++)
			{
				if (caster.IsDead)
					break;
				SplashDamage(skill, caster, position, range, knockdownPower, false, knockType, innerRange, hits);
				if (i < hitCount)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(sleepDelay));
				}
			}
		}

		/// <summary>
		/// Performs splash damage at a position, hitting all valid targets within range.
		/// </summary>
		/// <param name="skill">The skill being used.</param>
		/// <param name="caster">The entity performing the attack.</param>
		/// <param name="position">The center position of the splash.</param>
		/// <param name="range">The radius of the splash area.</param>
		/// <param name="knockdownPower">Knockback/knockdown power.</param>
		/// <param name="ignorePlayers">Whether to ignore player characters as targets.</param>
		/// <param name="knockType">The type of knockback effect (0 = none, non-zero applies skill's knockdown type).</param>
		/// <param name="innerRange">Inner range for donut-shaped areas (0 for circular area).</param>
		/// <param name="hitResults">Optional list to collect hit results for further processing.</param>
		public static void SplashDamage(Skill skill, ICombatEntity caster, Position position,
			float range, float knockdownPower, bool ignorePlayers, int knockType, float innerRange, List<SkillHitInfo> hitResults)
		{
			if (caster.IsDead)
				return;

			if (!caster.Map.Ground.IsValidPosition(position))
				return;

			range = Math.Max(range, SizeTypeRadius.GetRadius(caster.EffectiveSize));

			var damageDelay = skill.GetDamageDelay();
			var hitDelay = skill.GetHitDelay();

			List<ICombatEntity> targets;
			if (innerRange > 0)
				targets = caster.Map.GetAttackableEnemiesIn(caster, new Donut(position, range, innerRange));
			else
			{
				if (caster is Character)
				{
					targets = caster.Map.GetAttackableEnemiesIn(caster, new CircleF(position, range));
				}
				else
				{
					targets = caster.Map.GetAttackableEnemiesIn(caster, new CircleF(position, range));
					if (targets.Count == 0)
						targets = caster.Map.GetAttackableEnemiesInPosition(caster, position, range);
				}
			}

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				if (target == null || target.IsDead)
					continue;

				if ((!target.IsJumping()) && (!ignorePlayers || target is not Character))
				{
					var hits = new List<SkillHitInfo>();
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, hitDelay);
					var hasKnock = skillHit.KnockBackInfo != null && skillHitResult.Damage > 0 && target.IsKnockdownable();

					if (hasKnock)
					{
						if (skill.Data.KnockDownHitType == HitType.KnockBack || skill.Data.KnockDownHitType == HitType.Motion)
							target.ApplyKnockback(caster, skill, skillHit);
						else if (skill.Data.KnockDownHitType == HitType.KnockDown)
							target.ApplyKnockdown(caster, skill, skillHit);
					}

					hits.Add(skillHit);
					hitResults?.Add(skillHit);

					Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);
					if (hasKnock)
					{
						if (skill.Data.KnockDownHitType == HitType.KnockBack || skill.Data.KnockDownHitType == HitType.Motion)
							Send.ZC_KNOCKBACK_INFO(target, skillHit.KnockBackInfo);
						else if (skill.Data.KnockDownHitType == HitType.KnockDown)
							Send.ZC_KNOCKDOWN_INFO(target, skillHit.KnockBackInfo);
					}
				}
			}
		}

		/// <summary>
		/// Handles monster skill casting with balloon UI, cast time, and interruption checking.
		/// Returns true if the cast completed successfully, false if interrupted.
		/// The mob will continuously face the target during the cast if provided.
		/// </summary>
		/// <param name="skill">The skill being cast.</param>
		/// <param name="caster">The caster entity (should be a Mob).</param>
		/// <param name="skillName">The name to display in the casting balloon.</param>
		/// <param name="castTimeMs">The cast time in milliseconds.</param>
		/// <param name="target">Optional target to face during casting.</param>
		/// <param name="showCastingBar">Whether to show the casting bar UI.</param>
		/// <param name="changeColor">Whether to change the balloon color.</param>
		/// <returns>True if cast completed, false if interrupted.</returns>
		public static async Task<bool> MonsterCastTime(Skill skill, ICombatEntity caster, string skillName, int castTime, ICombatEntity target = null, bool showCastingBar = true, bool changeColor = false)
		{
			if (caster == null || caster.IsDead)
				return false;

			var mob = caster as Mob;
			if (mob == null)
				return false;

			skill.Vars.SetBool("Melia.MonsterCastInterrupted", false);
			mob.StartCasting(skill, skillName, castTime, showCastingBar, changeColor);

			const int tickInterval = 100;
			var elapsed = 0;

			while (elapsed < castTime)
			{
				if (!caster.IsCasting(skill))
				{
					skill.Vars.SetBool("Melia.MonsterCastInterrupted", true);
					return false;
				}

				if (target != null && !target.IsDead)
					caster.TurnTowards(target);

				await skill.Wait(TimeSpan.FromMilliseconds(tickInterval));
				elapsed += tickInterval;
			}

			if (!caster.IsCasting(skill))
			{
				skill.Vars.SetBool("Melia.MonsterCastInterrupted", true);
				return false;
			}

			mob.EndCasting(skill);
			return true;
		}

	}
}
