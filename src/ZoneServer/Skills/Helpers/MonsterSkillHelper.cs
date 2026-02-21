using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Pads.Helpers;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Helpers
{
	public static class MonsterSkillHelper
	{
		/// <summary>
		/// Creates a monster given an owner.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="className"></param>
		/// <param name="position"></param>
		/// <param name="angle"></param>
		/// <param name="range"></param>
		/// <param name="option"></param>
		public static Mob CreateMonster(ICombatEntity owner, string className,
			Position position, float angle, int level, int range = 1,
			RelationType relationType = RelationType.Enemy)
		{
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(className, out var data))
			{
				Log.Debug("CreateMonster: Failed to create monster because {0} wasn't found in monster db.", className);
				return null;
			}

			var monster = new Mob(data.Id, relationType);

			monster.Level = level;

			// Validate spawn position is within map boundaries
			if (!owner.Map.Ground.TryGetNearestValidPosition(position, out var validPosition))
				validPosition = owner.Position;

			monster.Position = validPosition;
			monster.Direction = owner.Direction.AddDegreeAngle(angle);
			monster.Properties[PropertyName.Range] = range;
			monster.Layer = owner.Layer;

			if (owner.Map.TryGetPropertyOverrides(monster.Id, out var propertyOverrides))
				monster.ApplyOverrides(propertyOverrides);

			return monster;
		}

		public static async Task MonsterMissileFall(ICombatEntity caster, Skill skill, Position position, MissileConfig config)
		{
			if (caster.IsDead)
				return;

			skill.Vars.Set("Melia.Skill.vAngle", config.VerticalAngle);

			caster.MissileFall(skill.Data.ClassName, config.Effect.Name, config.Effect.Scale, position, config.Range, config.DelayTime, config.FlyTime, config.Height, config.Easing, config.EndEffect.Name, config.EndEffect.Scale, config.StartEasing, config.GroundEffect.Name, config.GroundEffect.Scale);

			if (config.DelayTime > 0)
				await caster.PlayEffectToGround(config.GroundEffect.Name, position, config.GroundEffect.Scale, 0, config.DelayTime);

			caster.SetTempVar("CHECK_SKL_KD_PROP", 1);
			await DoDamageOverTime(skill, caster, position, (config.FlyTime + config.DelayTime + config.HitStartFix), config.Range, config.HitTime, config.HitCount, config.DotEffect.Name, config.DotEffect.Scale, config.KnockdownPower, (int)config.KnockType);
		}

		public static void MonsterSkillSetCollisionDamage(ICombatEntity caster, Skill skill, bool onFlag, float dmgRate)
		{
			if (caster is not Mob mob)
				return;

			if (!onFlag)
			{
				if (mob.Trigger != null)
					mob.Components.Remove<TriggerComponent>();
				mob.Vars.SetInt("Melia.CollisionSkillId", (int)SkillId.None);
				return;
			}

			var skillId = caster.GetCurrentSkill();
			var direction = caster.Direction;
			var originPos = caster.Position;
			var square = new SplashAreas.Square(originPos, direction, 80f, 50f);

			if (mob.Trigger != null)
				mob.Components.Remove<TriggerComponent>();

			var trigger = new TriggerComponent(mob, square);
			trigger.Filter = TriggerActorFilter.Characters;
			trigger.UpdateInterval = TimeSpan.FromMilliseconds(100);
			trigger.Entered += (sender, args) =>
			{
				if (args.Initiator is not ICombatEntity target)
					return;
				if (!caster.CanAttack(target))
					return;

				var modifier = new SkillModifier();
				modifier.DamageMultiplier = dmgRate;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				Send.ZC_SKILL_HIT_INFO(caster, new List<SkillHitInfo> { skillHit });

				SkillResultHelper.SkillResultKnockTarget(caster, target, skill, skillHit, KnockType.KnockDown, KnockDirection.TowardsTarget, 130f, 30, 10, 1, 5);
			};

			mob.Components.Add(mob.Trigger = trigger);
			mob.Vars.SetInt("Melia.CollisionSkillId", (int)skillId);
			mob.Vars.SetFloat("Melia.CollisionDamageRate", dmgRate);
		}

		public static Pad MonsterSkillCreatePad(ICombatEntity caster, Skill skill, Position position, float angle, string padName)
			=> SkillCreatePad(caster, skill, position, angle, padName);

		public static async Task MonsterSkillPadFrontMissile(ICombatEntity caster, Skill skill, Position position, string padName, float moveRange, int shootCount, float speed, float acceleration, float shootDelay, int padDirSet)
		{
			float padAngle = 0;

			if (padDirSet != 0)
			{
				padAngle = caster.Direction.DegreeAngle;
				if (padDirSet == 2)
					padAngle += 180;
			}

			var padList = new List<Pad>();
			for (var i = 0; i < shootCount; i++)
			{
				var pad = SkillCreatePad(caster, skill, position, padAngle, padName);
				if (pad == null)
					continue;
				padList.Add(pad);
			}

			float accumDelay = 0;
			foreach (var pad in padList)
			{
				if (pad != null)
				{
					var destPos = caster.Position.GetRelative(caster.Direction, moveRange);
					await pad.SetDestPosWithDelay(destPos, speed, acceleration, true, accumDelay);
					accumDelay += shootDelay;
				}
			}
			await Task.Yield();
		}

		public static void MonsterSkillPadDirMissile(ICombatEntity caster, Skill skill,
			Position position, string padName, float moveRange, int shootCount, float speed,
			float acceleration, float shootDelay, float addAngle,
			int padDirSet = 0)
		{
			var padAngle = 0f;

			if (padDirSet != 0)
			{
				padAngle = caster.Direction.DegreeAngle;
				if (padDirSet == 2)
				{
					padAngle += 180;
				}
			}

			var accumDelay = 0f;
			for (var i = 0; i < shootCount; i++)
			{
				var pad = SkillCreatePad(caster, skill, position, padAngle, padName);
				if (pad != null)
				{
					var newPosition = caster.Position.GetRelative(caster.Direction.AddDegreeAngle(addAngle), moveRange);
					pad.SetDestPosWithDelay(newPosition, speed, acceleration, true, accumDelay);
					accumDelay += shootDelay;
				}
			}
		}

		public static void MonsterSkillPadLookDirMissile(
			ICombatEntity caster, Skill skill, Position position, string padName, float moveRange, int shootCount, float speed, float acceleration, float shootDelay, float addAngle)
		{
			var padAngle = caster.Direction.DegreeAngle;

			var accumDelay = 0f;
			for (var i = 0; i < shootCount; i++)
			{
				var pad = SkillCreatePad(caster, skill, position, padAngle, padName);
				if (pad != null)
				{
					var newPosition = caster.Position.GetRelative(caster.Direction.AddDegreeAngle(addAngle), moveRange);
					pad.SetDestPosWithDelay(newPosition, speed, acceleration, true, accumDelay);
					accumDelay += shootDelay;
				}
			}
		}

		public static void MonsterSkillPadMissileBuck(
			ICombatEntity caster, Skill skill, Position position, string padName, float startAngle, float moveRange, int shootCount, float shootAngle, float speed, float acceleration, float shootDelay = 0f)
		{
			var accumDelay = 0f;
			for (var i = 0; i < shootCount; i++)
			{
				var angle = startAngle + (i * shootAngle);
				if (angle < 0)
					angle += 360;
				var pad = SkillCreatePad(caster, skill, position, 0, padName);
				if (pad != null)
				{
					var newPosition = caster.Position.GetRelative(pad.Direction.AddDegreeAngle(angle), moveRange);
					pad.SetDestPosWithDelay(newPosition, speed, acceleration, true, accumDelay);
					accumDelay += shootDelay;
				}
			}
		}

		public static bool UseMonsterSkillToDir(ICombatEntity caster, SkillId skillId, Direction direction)
		{
			if (caster.IsDead)
				return false;

			var skill = new Skill(caster, skillId);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, ForceId.GetNew(), null);
			return true;
		}
	}
}
