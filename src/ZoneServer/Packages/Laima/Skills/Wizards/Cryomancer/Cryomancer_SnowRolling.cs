using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Cryomancer
{
	/// <summary>
	/// Creates a snowball mob, attaches character on top of it,
	/// and applies damage to nearby targets while rotating them around
	/// the snowball.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cryomancer_SnowRolling)]
	public class Cryomancer_SnowRollingOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float SnowballScale = 0.65f;
		private const float SnowballHeight = 10.4f;
		private const int SnowballMonsterId = 47312;
		private const int SnowRollingDuration = 10000;
		private const float DamageRadius = 20f;
		private const int DamageInterval = 200;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			this.AddStateClearSkill(caster);
			var snowball = this.CreateSnowball(caster, originPos);
			this.ApplySnowRollingBuff(caster, skillHandle);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, snowball));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Mob snowball)
		{
			var damageTask = this.ApplyDamageAndPullEffect(caster, skill, snowball);

			await Task.WhenAll(skill.Wait(TimeSpan.FromMilliseconds(SnowRollingDuration)), damageTask);

			this.CleanupSnowRolling(caster as Character, snowball);
		}

		private async Task ApplyDamageAndPullEffect(ICombatEntity caster, Skill skill, Mob snowball)
		{
			var startTime = DateTime.Now;
			var random = RandomProvider.Get();
			while ((DateTime.Now - startTime).TotalMilliseconds < SnowRollingDuration)
			{
				if (snowball.IsDead)
					break;
				snowball.Position = new Position(caster.Position.X, caster.Position.Y - SnowballHeight, caster.Position.Z);
				snowball.Direction = caster.Direction;
				snowball.Vars.Set("NextAvailableAttachPoint", 1);
				var splashArea = new Circle(snowball.Position, DamageRadius);
				var hitDelay = 0;
				var damageDelay = 0;
				var hits = new List<SkillHitInfo>();
				var targets = caster.Map.GetAttackableEnemiesIn(snowball, splashArea);

				foreach (var target in targets)
				{
					// Apply damage
					var splashHitResult = SCR_SkillHit(caster, target, skill);
					target.TakeDamage(splashHitResult.Damage, caster);
					var splashHit = new SkillHitInfo(caster, target, skill, splashHitResult, TimeSpan.FromMilliseconds(damageDelay), TimeSpan.FromMilliseconds(hitDelay));
					hits.Add(splashHit);

					// Apply freeze
					var freezeChance = 100f;
					var freezeDurationMilli = 3000f;
					if ((random.Next(100) < freezeChance) && splashHitResult.Damage > 0)
					{
						target.StartBuff(BuffId.Cryomancer_Freeze, TimeSpan.FromMilliseconds(freezeDurationMilli), caster);
					}

					if (target.MoveType != MoveType.Holding
						&& target.EffectiveSize > SizeType.Hidden
						&& target.EffectiveSize < SizeType.L)
					{
						target.Position = snowball.Position;
						// Attaches target to snowball
						var availableAttachPoint = (int)snowball.Vars.Get("NextAvailableAttachPoint");
						var attachPoint = "Dummy_snow" + availableAttachPoint.ToString();
						Send.ZC_ATTACH_TO_OBJ(target, snowball, attachPoint, "", 0, 0.001f, attachAnimation: AnimationName.DOWN);

						// Increment attach points
						snowball.Vars.Set("NextAvailableAttachPoint", availableAttachPoint + 1);
						if (availableAttachPoint > 20)
							snowball.Vars.Set("NextAvailableAttachPoint", 1);
					}
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);

				await skill.Wait(TimeSpan.FromMilliseconds(DamageInterval));
			}
		}

		private void AddStateClearSkill(ICombatEntity caster)
		{
			if (caster is Character character)
				character.Skills.AddSilent(new Skill(character, SkillId.Common_StateClear, 1));
			Send.ZC_NORMAL.ApplyBuff(caster, "SnowRolling_Buff", SkillId.Common_StateClear, true);
		}

		private Mob CreateSnowball(ICombatEntity caster, Position originPos)
		{
			var snowball = new Mob(SnowballMonsterId, RelationType.Friendly)
			{
				Faction = FactionType.Law,
				Position = (originPos + new Position(0, -0.10f, 0)).Floor,
				OwnerHandle = caster.Handle,
				AssociatedHandle = caster.Handle,
				Layer = caster.Layer
			};
			snowball.Components.Add(new LifeTimeComponent(snowball, TimeSpan.FromMilliseconds(SnowRollingDuration)));
			snowball.Died += this.Snowball_Died;
			caster.Map.AddMonster(snowball);
			snowball.StartBuff(BuffId.Invincible);

			this.SetupSnowball(caster, snowball);

			return snowball;
		}

		private void SetupSnowball(ICombatEntity caster, Mob snowball)
		{
			Send.ZC_NORMAL.AttachEffect(snowball, "", 1, EffectLocation.Bottom, 0, 10, 0);
			Send.ZC_NORMAL.ControlObject(caster, snowball, ControlLookType.SameDirection, true, true, "SnowRolling_Buff", false);
			Send.ZC_NORMAL.Skill_CallLuaFunc(snowball, AnimationName.MissileDead, 2, 4, 0, 3, 1);
			Send.ZC_MOVE_ANIM(snowball, FixedAnimation.WLK, 0);
			Send.ZC_STD_ANIM(snowball, FixedAnimation.WLK);
			Send.ZC_FACTION(snowball);
			Send.ZC_NORMAL.SetScale(snowball, AnimationName.SnowRolling, SnowballScale);
			Send.ZC_NORMAL.DelayEnterWorld(snowball);
			Send.ZC_NORMAL.EnterDelayedActor(snowball);
			Send.ZC_NORMAL.SetHeight(snowball, SnowballHeight);
			caster.AttachToObject(snowball, AnimationName.DummyTop, "None", 0, 0.001f);
			//Send.ZC_ATTACH_TO_OBJ(caster, snowball, AnimationName.DummyTop, "", 0.001f);
			Send.ZC_NORMAL.PlayEffect(caster, 0, EffectLocation.Bottom, 0, 0, AnimationName.Empty);
			Send.ZC_GROUND_EFFECT(caster, snowball.Position, AnimationName.Empty);
		}

		private void ApplySnowRollingBuff(ICombatEntity caster, int skillHandle)
		{
			Send.ZC_SYNC_START(caster, skillHandle, 1);
			caster.StartBuff(BuffId.SnowRolling_Buff, TimeSpan.FromMilliseconds(SnowRollingDuration), caster);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(300));
		}

		private void Snowball_Died(ICombatEntity killer, ICombatEntity killed)
		{
			if (killed is Mob snowball && snowball.Map.TryGetCharacter(snowball.OwnerHandle, out var owner))
				this.CleanupSnowRolling(owner, snowball);
		}

		private void CleanupSnowRolling(Character owner, Mob snowball)
		{
			owner.Skills.RemoveSilent(SkillId.Common_StateClear);
			Send.ZC_NORMAL.RemoveBuff(owner, "SnowRolling_Buff");
			Send.ZC_SKILL_DISABLE(owner);
			Send.ZC_NORMAL.PlayEffect(snowball, 0, EffectLocation.Bottom, 0, 0, AnimationName.Empty, 0, owner.Handle);
			//Send.ZC_ATTACH_TO_OBJ(owner, null, "", "", 0, 1);
			owner.AttachToObject(null, "None", "None", 0, 1);
			owner.Buffs.Remove(BuffId.SnowRolling_Buff);
			Send.ZC_NORMAL.SkillCancelCancel(owner, SkillId.Cryomancer_SnowRolling);
			Send.ZC_NORMAL.ClearEffects(snowball);
			Send.ZC_EXEC_CLIENT_SCP(owner.Connection, ClientScripts.UPDATE_PC_FOLLOWER_LIST);
		}
	}
}
