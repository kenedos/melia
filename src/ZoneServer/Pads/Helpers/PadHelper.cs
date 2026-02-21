using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Handlers.Common;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Shared.Util.TaskHelper;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Helpers
{
	public static class PadHelper
	{
		public static bool PadActivate(Pad pad, ICombatEntity target, RelationType relationType)
		{
			var caster = (ICombatEntity)pad.Creator;

			if (!caster.CheckRelation(target, relationType))
				return false;

			if (pad.Trigger.AtCapacity)
				return false;

			pad.Trigger.ActivateCount++;
			return true;
		}

		public static bool PadDeactivate(Pad pad, ICombatEntity target, RelationType relationType)
		{
			var caster = (ICombatEntity)pad.Creator;

			if (!caster.CheckRelation(target, relationType))
				return false;

			pad.Trigger.ActivateCount--;
			return true;
		}

		public static IMonster PadAttachMonster(Pad pad, string className, Position pos, float angle,
			int lvFix, float lifeTime, float altitude,
			string monProp, string effect = "None", float eftScale = 1f, bool own = true, string aiName = "",
			string initScript = "None", bool isPadSelect = false, string scpName = "")
		{
			var monster = PadCreateMonster(pad, className, pos, angle, lvFix, lifeTime, monProp, effect, eftScale, own, aiName, initScript, isPadSelect, scpName);
			Send.ZC_NORMAL.PadSetMonsterAltitude(pad, monster, altitude);

			return monster;
		}

		public static IMonster PadCreateMonster(Pad pad, string className, Position pos, float angle, float lifeTime,
			string effect, float eftScale, int range = 0, bool own = true)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (caster == null || skill == null)
				return null;

			var monster = CreateMonster(caster, className, pos, angle, caster.Level, range, 0);
			if (monster == null)
				return monster;

			if (own)
			{
				monster.OwnerHandle = caster.Handle;
				monster.AssociatedHandle = caster.Handle;
				monster.Faction = caster.Faction;
			}
			else
			{
				monster.OwnerHandle = caster.Handle;
			}

			monster.Vars.SetInt("Melia.Summon.SkillLevel", skill.Level);

			if (lifeTime > 0)
				monster.DisappearTime = DateTime.Now.AddMilliseconds(lifeTime);

			if (effect == "Invisible")
				monster.AddEffect(new ScriptInvisibleEffect());
			else if (effect != "None")
				monster.AttachEffect(effect, eftScale);

			monster.Vars.SetInt("Melia.Pad.Handle", pad.Handle);
			monster.Vars.Set("Melia.Summoner.Handle", caster.Handle);
			pad.Monster = monster;
			monster.OwnerHandle = caster.Handle;
			caster.Map.AddMonster(monster);

			return monster;
		}

		public static IMonster PadCreateMonster(Pad pad, string className, Position pos, float angle, int lvFix, float lifeTime,
			string monProp, string effect, float eftScale, bool own = true, string aiName = "",
			string initScript = "None", bool isPadSelect = false, string scpName = "")
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (caster == null || skill == null)
				return null;

			var copyMonsterHandle = 0; //GetExProp(caster, "COPY_MON_HANDLE_" + skill.Data.ClassName);
			if (copyMonsterHandle > 0 && caster.Map.GetMonster(copyMonsterHandle) is Mob copyMob)
			{
				caster = copyMob;
			}

			var mob = CreateMonster(caster, className, pos, angle, caster.Level + lvFix, 1, 0);
			if (mob == null)
			{
				Log.Debug("PadCreateMonster: Failed to create monster {0}.", className);
				return mob;
			}

			if (own)
			{
				mob.OwnerHandle = caster.Handle;
				mob.AssociatedHandle = caster.Handle;
				mob.Faction = caster.Faction;
			}
			else
			{
				//monster.OwnerHandle = caster.Handle;
				mob.AssociatedHandle = caster.Handle;
			}

			mob.Vars.SetInt("Melia.Summon.SkillLevel", skill.Level);

			if (!string.IsNullOrEmpty(aiName) && aiName != "None" && AiScript.Exists(aiName))
			{
				mob.Components.Add(new MovementComponent(mob));
				mob.Components.Add(new AiComponent(mob, aiName, caster));
			}

			if (lifeTime > 0 && lifeTime <= 300)
				mob.DisappearTime = DateTime.Now.AddSeconds(lifeTime);
			else if (lifeTime > 300)
				mob.DisappearTime = DateTime.Now.AddMilliseconds(lifeTime);

			if (effect != "None")
				mob.AttachEffect(effect, eftScale);

			mob.Vars.SetInt("Melia.Pad.Handle", pad.Handle);
			mob.Vars.Set("Melia.Summoner.Handle", caster.Handle);
			mob.OwnerHandle = caster.Handle;
			pad.Monster = mob;

			if (skill.Id == SkillId.Corsair_JollyRoger)
			{
				pad.Variables.Set("Melia.Pad.Monster.Handle", mob.Handle);
			}

			return mob;
		}

		public static void PadCreateObstacle(Pad pad, float sizeX, float sizeY)
			=> Send.ZC_NORMAL.PadCreateObstacle(pad, sizeX, sizeY);

		public static bool CheckConcurrentUseCount(Pad pad, string userValue)
		{
			var concurrentCount = pad.Trigger.MaxConcurrentUseCount;
			if (concurrentCount > 0)
			{
				var currentCount = pad.Trigger.ActivateCount;
				if (currentCount >= concurrentCount)
					return false;
				pad.Trigger.ActivateCount++;
			}

			return true;
		}

		/// <summary>
		/// Applies buff/debuff to entities in 
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="targetRelation"></param>
		/// <param name="consumeLife"></param>
		/// <param name="consumeUse"></param>
		/// <param name="buffId"></param>
		/// <param name="lv"></param>
		/// <param name="arg2"></param>
		/// <param name="applyTime"></param>
		/// <param name="over"></param>
		/// <param name="rate"></param>
		/// <param name="buffCondition">A callback to check if the buff should be applied to the target</param>
		public static void PadBuff(Pad pad, RelationType targetRelation, int consumeLife, int consumeUse, BuffId buffId, int lv, int arg2, float applyTime, int over, int rate, Func<ICombatEntity, bool> buffCondition = null)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;

			if (caster == null || skill == null)
				return;

			foreach (var actor in pad.Trigger.GetActors())
			{
				if (!caster.CheckRelation(actor, targetRelation) || actor is not ICombatEntity target)
					continue;

				// Check the buff condition if provided
				if (buffCondition != null && !buffCondition(target))
					continue;

				if (buffId == BuffId.SadhuBind_Debuff)
				{
					var immune = target.IsBuffActive(BuffId.SadhuPossessionTemporaryImmune);
					var isAppliedBuff = target.IsBuffActive(buffId);
					if (isAppliedBuff && immune)
					{
						var elapsedTime = (float)(DateTime.UtcNow - skill.Vars.Get<DateTime>("Sadhu_Bind_StartTime")).TotalMilliseconds / 1000f;
						applyTime = 6500f - elapsedTime;
						AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);
					}
					if (immune)
						target.PlayTextEffect("I_SYS_Text_Effect_Skill", ScpArgMsg("SHOW_GUNGHO"));
				}
				else
				{
					AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);
				}

				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;
				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
				if (pad.IsDead)
					return;
			}
		}

		public static void PadBuffEnemyMonster(Pad pad, RelationType targetRelation, int consumeLife, int consumeUse, BuffId buffId, int lv, int arg2, float applyTime, int over, int rate)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;
			if (caster == null || skill == null)
				return;

			foreach (var actor in pad.Trigger.GetActors())
			{
				if (!caster.CheckRelation(actor, targetRelation) || actor is not ICombatEntity target || target.IsDead)
					continue;
				AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);
				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;
				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
				if (pad.IsDead)
					return;
			}
		}

		public static void PadBuffCheckBuffEnemy(Pad pad, RelationType targetRelation, int consumeLife,
			int consumeUse, BuffId checkBuff, BuffId buffId, int lv, int arg2, float applyTime, int over, int rate)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;
			if (caster == null || skill == null)
				return;

			foreach (var actor in pad.Trigger.GetActors())
			{
				if (!caster.CheckRelation(actor, targetRelation) || actor is not ICombatEntity target)
					continue;
				if (target.IsBuffActive(checkBuff))
					continue;
				AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);
				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;
				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
				if (pad.IsDead)
					return;
			}
		}

		/// <summary>
		/// Damages enemies inside the pad up to pad's use count or
		/// pad's maximum number of actors.
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="damageMultiplier"></param>
		/// <param name="consumeLife"></param>
		/// <param name="consumeUse"></param>
		/// <param name="effectName"></param>
		/// <param name="effectScale"></param>
		/// <param name="effectDuration"></param>
		/// <param name="delay"></param>
		/// <param name="enemy"></param>
		/// <param name="multiHits"></param>
		/// <param name="damageCondition">A callback to check if damage should be applied to the target</param>
		public static void PadDamageEnemy(Pad pad, float damageMultiplier = 1, int consumeLife = 0, int consumeUse = 0, string effectName = "None", float effectScale = 1, float effectDuration = 0, float delay = 0, bool enemy = true, int multiHits = 1, Func<ICombatEntity, bool> damageCondition = null)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;
			if (caster == null)
				return;
			if (caster.IsDead)
				return;

			// Official code searches for normal skill if it's null.
			if (skill == null)
				return;

			var targetCount = pad.Trigger.MaxActorCount;
			var targets = pad.Map.GetAttackableEnemiesIn(caster, pad.Area);
			foreach (var actor in targets)
			{
				if (actor is not ICombatEntity target || target.IsDead)
					continue;

				if (targetCount <= 0)
					break;

				if (!caster.IsEnemy(target))
					return;

				var targetOwner = target;

				// Check the damage condition if provided
				if (damageCondition != null && !damageCondition(target))
					continue;

				var modifier = SkillModifier.MultiHit(multiHits);

				var skillHitResult = SCR_SkillHit(caster, targetOwner, skill, modifier);
				var damage = skillHitResult.Damage;

				target.TakeDamage(damage * damageMultiplier, caster);

				var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
				Send.ZC_HIT_INFO(caster, target, hitInfo);

				if (effectName != null && effectName != "None")
				{
					var pos = target.Position;
					caster.PlayEffectToGround(effectName, pos, effectScale, effectDuration, delay, 0);
				}

				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;

				targetCount--;

				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
				if (pad.IsDead)
					break;
			}
		}

		public static void SetDestPos(this Pad pad, Position destination, float speed, float accel, bool destroyOnArrival)
		{
			pad.Movement.Speed = speed;
			var timeTaken = pad.Movement.MoveTo(destination);
			if (destroyOnArrival)
			{
				Task.Delay(timeTaken).ContinueWith(_ =>
				{
					pad.Destroy();
				});
			}
		}

		public static async Task SetDestPosWithDelay(this Pad pad, Position destination, float speed, float accel, bool destroyOnArrival, float accumDelay = 0)
		{
			pad.Movement.Speed = speed;
			var timeTaken = pad.Movement.MoveTo(destination);
			if (destroyOnArrival)
			{
				await Task.Delay(timeTaken).ContinueWith(_ =>
				{
					pad.Destroy();
				});
			}
			if (accumDelay > 0)
				await Task.Delay((int)accumDelay);
		}

		public static void PadRemoveBuff(Pad pad, RelationType targetRelation, float consumeLife, float consumeUse, BuffId buffId)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null)
				return;

			var otherPadsFromSameSkill = pad.Map.GetPads(p => p != pad && p.Skill?.Id == skill?.Id && !p.IsDead);
			var actors = pad.Trigger.GetActors();

			foreach (var actor in actors)
			{
				if (caster.CheckRelation(actor, targetRelation) && actor is ICombatEntity target)
				{
					var isInOtherPad = false;
					foreach (var otherPad in otherPadsFromSameSkill)
					{
						if (otherPad.Trigger.GetActors().Contains(actor))
						{
							isInOtherPad = true;
							break;
						}
					}

					if (!isInOtherPad)
						target.RemoveBuff(buffId);

					if (consumeUse != 0)
						pad.Trigger.IncreaseUseCount();
					if (pad.IsDead)
						return;
				}
			}
		}

		public static void PadSelectPadKill(Pad pad, string padName, float searchRange)
		{
			var targetPads = pad.Map.GetPadsAt(pad.Position, searchRange);

			foreach (var targetPad in targetPads)
			{
				if (targetPad.Name == padName)
					targetPad.Destroy();
			}
		}

		/// <summary>
		/// Sets up a jump rope effect on a skill pad with specified parameters.
		/// </summary>
		/// <param name="pad">The pad where the jump rope effect takes place.</param>
		/// <param name="effect">The effect name.</param>
		/// <param name="effectScale">The scale of the effect.</param>
		/// <param name="radius">The radius of the jump rope area.</param>
		/// <param name="width">The width of the jump rope effect.</param>
		/// <param name="ropeCount">The number of ropes.</param>
		/// <param name="readySec">The preparation time before the rope starts.</param>
		/// <param name="loopCount">The number of loops.</param>
		/// <param name="loopSec">The duration of each loop.</param>
		/// <param name="height">The height at which the rope moves.</param>
		public static void PadSetJumpRope(
			Pad pad,
			string effect,
			float effectScale,
			float radius,
			float width,
			int ropeCount,
			float readySec,
			int loopCount,
			float loopSec,
			float height)
		{
			if (pad.IsDead)
				return;

			// Execute the jump rope effect
			var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();
			pad.Variables.Set("Pad.JumpRope.EffectHandle", effectHandle);
			Send.ZC_NORMAL.RunJumpRope(pad.Creator, effectHandle, effect, effectScale, pad.Position, radius, width, ropeCount, readySec, loopCount, loopSec, height);
			pad.Trigger.Destroyed += (sender, e) => Send.ZC_NORMAL.RunJumpRope(pad, effectHandle);
		}

		public static void PadTargetBuff(Pad pad, ICombatEntity target, RelationType targetRelation, float consumeLife, float consumeUse, BuffId buffId, int lv, int arg2, int applyTime, int over, int rate = 0, bool saveHandle = false)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (target == null)
				return;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;

			Buff buff = null;
			if (skill.Vars.Has("FromBuffSeller"))
			{
				var addTime = 0;
				if (caster.TryGetSkill(SkillId.Pardoner_SpellShop, out var spellshopSkill))
				{
					addTime += 420000 * spellshopSkill.Level;
				}

				if (caster.TryGetActiveAbility(AbilityId.Pardoner4, out var abil))
				{
					addTime += 300000 * abil.Level;
				}

				skill.Vars.Remove("FromBuffSeller");
				applyTime += addTime;

				buff = AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate, (int)BuffOrigin.FromAutoSeller);
				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;
				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
			}
			else if (caster.CheckRelation(target, targetRelation))
			{
				if (over == 0 && target.IsBuffActive(buffId))
					return;

				if (skill.Id == SkillId.Sapper_Cover && target is Character)
					return;

				if (saveHandle)
				{
					//SetExArgObject(caster, "SaveOwner", GetOwner(caster));
				}

				if (!CheckConcurrentUseCount(pad, buffId.ToString()))
					return;

				if (target is Character)
				{
					var remainingTime = pad.Trigger.LifeTime;
					if (skill.Id == SkillId.Psychokino_Raise)
					{
						//var now = Convert.ToInt32(DateTime.Now);
						//target.SetTempVar("Psychokino_Raise_remainingTime", (float)remainingTime.TotalSeconds);
						//target.SetTempVar("Psychokino_Raise_startTime", now);
					}
				}

				buff = AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);

				//pad.Life += consumeLife;
				//pad.UseLimit += consumeUse;
				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
			}
		}

		public static void PadTargetBuffCheckAbility(Pad pad, ICombatEntity target, RelationType targetRelation,
			AbilityId abilityId, int consumeLife, int consumeUse, BuffId buffId, int arg2, int applyTime, int over,
			int rate = 0, bool hasAbility = true, bool checkConcurrentUse = false)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;

			PadTargetBuffCheckAbility(pad, target, targetRelation, abilityId, consumeLife, consumeUse, buffId, skill.Level, arg2, applyTime, over, rate, hasAbility, checkConcurrentUse);
		}

		public static void PadTargetBuffCheckAbility(Pad pad, ICombatEntity target, RelationType targetRelation,
			AbilityId abilityId, int consumeLife, int consumeUse, BuffId buffId, int arg1, int arg2, int applyTime,
			int over, int rate, bool hasAbility = true, bool checkConcurrentUse = false)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;

			var applyBuff = hasAbility ? caster.IsAbilityActive(abilityId) : !caster.IsAbilityActive(abilityId);

			if (applyBuff)
			{
				if (checkConcurrentUse && !CheckConcurrentUseCount(pad, buffId.ToString()))
					return;

				PadTargetBuff(pad, target, targetRelation, consumeLife, consumeUse, buffId, arg1, arg2, applyTime, over, rate);
			}
		}

		public static void PadTargetBuffAfterBuffCheck(Pad pad, ICombatEntity target, RelationType targetRelation, int consumeLife, int useCount, BuffId checkbuff, BuffId buffId, int lv, int arg2, int applyTime, int over, int rate = 0, bool saveHandle = false)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;
			if (target.IsBuffActive(checkbuff))
				return;

			if (caster.CheckRelation(target, targetRelation))
				PadTargetBuff(pad, target, targetRelation, consumeLife, useCount, buffId, lv, arg2, applyTime, over, rate, saveHandle);
		}

		public static void PadTargetBuffMon(Pad pad, ICombatEntity target, RelationType targetRelation, int consumeLife,
			int consumeUse, BuffId buffId, int lv, int arg2, float applyTime, int over, int rate = 0)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;
			if (!caster.CheckRelation(target, targetRelation))
				return;
			if (over == 0 && target.IsBuffActive(buffId))
				return;

			AddPadBuff(caster, target, pad, buffId, lv, arg2, applyTime, over, rate);
			//pad.Life += consumeLife;
			//pad.UseLimit += consumeUse;
			if (consumeUse != 0)
				pad.Trigger.IncreaseUseCount();
		}

		public static void PadTargetBuffRemove(Pad pad, ICombatEntity target, RelationType targetRelation, int consumeLife, int consumeUse, BuffId buffId, bool delOwnerBuff = false)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;

			if (caster.CheckRelation(target, targetRelation))
			{
				var otherPadsFromSameSkill = pad.Map.GetPads(p => p != pad && p.Skill?.Id == skill?.Id && !p.IsDead);
				var isInOtherPad = false;
				foreach (var otherPad in otherPadsFromSameSkill)
				{
					if (otherPad.Trigger.GetActors().Contains(target))
					{
						isInOtherPad = true;
						break;
					}
				}

				if (!isInOtherPad)
				{
					if (!delOwnerBuff)
						target.RemoveBuff(buffId);
					else
						target.RemoveBuffByCaster(caster, buffId);
				}

				if (consumeUse != 0)
					pad.Trigger.IncreaseUseCount();
			}

			target.RemoveBuff(BuffId.SadhuPossessionTemporaryImmune);
		}

		public static void PadTargetBuffRemoveMonster(Pad pad, ICombatEntity target, RelationType targetRelation, int consumeLife, int consumeUse, BuffId buffId)
		{
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return;

			if (!caster.CheckRelation(target, targetRelation))
				return;
			target.RemoveBuff(buffId);
			//pad.Life += consumeLife;
			//pad.UseLimit += consumeUse;
			if (consumeUse != 0)
				pad.Trigger.IncreaseUseCount();
		}

		[Obsolete("Use PadTargetDamage(Pad pad, ICombatEntity target, out SkillHitInfo skillHit, RelationType targetRelation = RelationType.Enemy, float atkRate = 1f, float consumeLife = 0, float consumeUse = 0)")]
		public static bool PadTargetDamage(Pad pad, ICombatEntity target, RelationType targetRelation = RelationType.Enemy, float atkRate = 1f, float consumeLife = 0, float consumeUse = 0)
			=> PadTargetDamage(pad, target, out _, targetRelation, atkRate, consumeLife, consumeUse);

		public static bool PadTargetDamage(Pad pad, ICombatEntity target, out SkillHitInfo skillHit, RelationType targetRelation = RelationType.Enemy, float atkRate = 1f, float consumeLife = 0, float consumeUse = 0)
		{
			skillHit = default;
			var caster = (ICombatEntity)pad.Creator;
			var skill = pad.Skill;

			if (pad.IsDead || caster == null || skill == null || target.IsDead)
				return false;

			if (!caster.CheckRelation(target, targetRelation))
				return false;

			if (!CheckConcurrentUseCount(pad, "TargetDamageCount"))
				return false;

			skill.Properties.TryGetFloat(PropertyName.SkillAtkAdd, out var divineAtkAdd);
			var addValue = 0;

			if (pad.NumArg1 > 0)
				addValue = (int)pad.NumArg1;

			divineAtkAdd = addValue - divineAtkAdd;

			if (divineAtkAdd < 0)
				divineAtkAdd = 0;

			var modifier = new SkillModifier();
			modifier.BonusPAtk += divineAtkAdd;

			var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
			target.TakeDamage(skillHitResult.Damage, caster);
			skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, skill.GetDamageDelay(), skill.GetHitDelay());

			if (skill.Id == SkillId.Peltasta_ShieldLob || skill.Id == SkillId.Peltasta_ShieldLob_2)
				skillHit.HitCount = 4;

			Send.ZC_HIT_INFO(caster, skillHit.Target, skillHit.HitInfo);

			//pad.Life += consumeLife;
			//pad.UseLimit += consumeUse;
			if (consumeUse != 0)
				pad.Trigger.IncreaseUseCount();

			return true;
		}

		public static Buff AddPadBuff(ICombatEntity caster, ICombatEntity target, Pad pad, BuffId buffId, int arg1, int arg2, float time, int over, int rate = 0, int? fromWho = null)
		{
			if (pad.IsDead || caster == null)
				return null;
			if (target.IsBuffActive(BuffId.Skill_NoDamage_Buff)
				&& ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData)
				&& buffData.Type == BuffType.Debuff && buffData.Level < 99)
				return null;

			if (!fromWho.HasValue)
			{
				fromWho = caster == target ? 0 : 1;
			}

			Buff buff = null;
			if (rate == 0 || rate >= RandomProvider.Next(1, 100))
			{
				buff = target.StartBuff(buffId, arg1, arg2, TimeSpan.FromMilliseconds(time), caster, pad.Skill?.Id ?? SkillId.None);
			}

			return buff;
		}
	}
}
