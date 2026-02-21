using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters
{
	[SkillHandler(SkillId.Mon_boss_dionia_Skill_2)]
	public class MonBossDionaSkill2 : SimpleMonsterAttackSkill
	{
		/// <summary>
		/// Executes the actual attack after a potential delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		protected override async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity designatedTarget)
		{
			if (!caster.TrySpendSp(skill))
				return;

			skill.IncreaseOverheat();

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, designatedTarget.Position);

			caster.PlayAnimation("SKL2", false, 0);
			caster.PlayAnimation("SKL2", false, 4000);

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_blood001_green#Dummy_skl2_shot", 0.8f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.5f),
				Range = 20,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 900,
				Speed = 0.5f,
				HitTime = 1000,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var targets = SkillSelectEnemiesInCircle(caster, designatedTarget.Position, 200, 50);
			foreach (var target in targets)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(1800));

				// Volley 1: 3 missiles (Height, Distance, Distance)
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 2: 3 missiles (Height, Distance, Height)
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 3: 3 missiles (Distance, Distance, Height)
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 4: 3 missiles (Distance, Distance, Distance)
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150, height: 2);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(3400));

				// Volley 5: 3 missiles (Distance x3), rand=170, height=1
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 6: 3 missiles (Height x3), rand=170, height=1
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 7: 3 missiles (Distance, Height, Height), rand=170, height=1
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);

				await skill.Wait(TimeSpan.FromMilliseconds(200));

				// Volley 8: 3 missiles (Height, Distance, Distance), rand=170, height=1
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}

		private async void ThrowProjectile(ICombatEntity caster, ICombatEntity target, Skill skill, int delay, int randDist, int count)
		{
			var splashArea = this.GetSplashArea(skill, caster, target);
			var damageDelay = this.GetDamageDelay(skill) + TimeSpan.FromMilliseconds(delay);
			var hitDelay = this.GetHitDelay(skill);
			var skillHitDelay = skill.Properties.HitDelay;

			await skill.Wait(TimeSpan.FromMilliseconds(delay)).ContinueWith(_ =>
			{
				for (var i = 0; i < count; i++)
				{
					var pos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 150);
					Send.ZC_NORMAL.SkillProjectile(caster, pos, "I_blood001_green#Dummy_skl2_shot", 0.8f, "F_explosion062_blood", 0.5f, 900, TimeSpan.FromSeconds(1));

					// Select targets in a circle
					var circle = new CircleF(pos, 50);
					var targets = caster.Map.GetAttackableEnemiesIn(caster, circle);

					Send.ZC_START_RANGE_PREVIEW(caster, skill.Data.ClassName, TimeSpan.FromSeconds(3), splashArea);
					var hits = new List<SkillHitInfo>();

					foreach (var currentTarget in targets.LimitBySDR(caster, skill))
					{
						var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);
						currentTarget.TakeDamage(skillHitResult.Damage, caster);

						var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, damageDelay, skillHitDelay);
						hits.Add(skillHit);
					}

					Send.ZC_SKILL_HIT_INFO(caster, hits);
				}
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_dionia_Skill_3)]
	public class MonBossDioniaSkill3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1600);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position);

			skill.Run(this.HandleSkill(caster, target, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var spawnPos = GetRelativePosition(PosType.Self, caster, target, distance: 83.414459f, angle: 56f, height: 1);
			MonsterSkillCreateMob(skill, caster, "dionia_mini", spawnPos, 0f, "", "BasicMonster", -5, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.Self, caster, target, distance: 77.647041f, angle: -57f, height: 1);
			MonsterSkillCreateMob(skill, caster, "dionia_mini", spawnPos, 0f, "", "BasicMonster", -5, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.Self, caster, target, distance: 86.10994f, angle: -16f, height: 1);
			MonsterSkillCreateMob(skill, caster, "dionia_mini", spawnPos, 0f, "", "BasicMonster", -5, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.Self, caster, target, distance: 92.408195f, angle: 20f, height: 1);
			MonsterSkillCreateMob(skill, caster, "dionia_mini", spawnPos, 0f, "", "BasicMonster", -5, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_dionia_Skill_4)]
	public class MonBossDionaSkill4 : SimpleMonsterAttackSkill
	{
		protected override async Task Attack(Skill skill, ICombatEntity caster, ICombatEntity designatedTarget)
		{
			if (!caster.TrySpendSp(skill))
				return;

			skill.IncreaseOverheat();

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, designatedTarget.Position);

			// Play animation
			caster.PlayAnimation("SKL", false);

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#dionia_head_001", 1.5f),
				EndEffect = new EffectConfig("F_explosion001_yellow", 2f),
				Range = 30,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 600,
				Speed = 1f,
				HitTime = 1000,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var targets = SkillSelectEnemiesInCircle(caster, designatedTarget.Position, 200, 30);
			var target = targets.Random();
			if (target != null)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(1500));
				var position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await MissileThrow(skill, caster, position, config);

				for (var i = 0; i < 5; i++)
				{
					position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
					await MissileThrow(skill, caster, position, config);
				}
			}

			// Select targets in a circle , 30, 2, 0, 1
			//var targets = caster.Map.GetAttackableEntitiesIn(caster, new CircleF(caster.Position, 200));

			// Throw projectiles
			//this.ThrowProjectile(caster, designatedTarget, skill, 1500, 0, 1); // Center
			//this.ThrowProjectile(caster, designatedTarget, skill, 1500, 140, 6); // Around
		}

		private async void ThrowProjectile(ICombatEntity caster, ICombatEntity target, Skill skill, int delay, int randDist, int count)
		{
			var splashArea = this.GetSplashArea(skill, caster, target);
			var damageDelay = this.GetDamageDelay(skill);
			var hitDelay = this.GetHitDelay(skill);
			var skillHitDelay = skill.Properties.HitDelay;

			await skill.Wait(TimeSpan.FromMilliseconds(delay));
			for (var i = 0; i < count; i++)
			{
				Position pos;
				if (randDist == 0)
					pos = target.Position; // Center projectile
				else
					pos = caster.Position.GetRandomInRange2D(randDist, RandomProvider.Get()); // Random projectile

				Send.ZC_NORMAL.SkillProjectile(caster, pos, "I_force045_green#dionia_head_001", 1.5f, "F_explosion001_yellow", 2f, 600, TimeSpan.FromMilliseconds(1000), TimeSpan.Zero, 3);

				// Select targets in a circle
				var circle = new CircleF(pos, 50);
				var targets = caster.Map.GetAttackableEnemiesIn(caster, circle);
				var hits = new List<SkillHitInfo>();

				foreach (var currentTarget in targets.LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);
					currentTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, damageDelay, skillHitDelay);
					hits.Add(skillHit);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_dionia_Skill_5)]
	public class MonBossDioniaSkill5 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, forceId, null);
			caster.PlayAnimation("SKL", false, 0);
			caster.PlayAnimation("SKL", false, 2000);
			var targets = SkillSelectEnemiesInCircle(caster, target.Position, 250, 30);

			if (targets == null || targets.Count == 0)
				return;
			target = targets.Random();

			skill.Run(this.HandleSkill(caster, target, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var configA = new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#dionia_head_001", 1.5f),
				EndEffect = new EffectConfig("F_explosion001_yellow", 2f),
				Range = 30,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 600,
				Speed = 1f,
				HitTime = 1000,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var configB = new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#dionia_head_001", 1.5f),
				EndEffect = new EffectConfig("F_explosion001_yellow", 2f),
				Range = 30,
				FlyTime = 1.8f,
				DelayTime = 0f,
				Gravity = 600,
				Speed = 1f,
				HitTime = 1000,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			for (var i = 0; i < 6; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 230, height: 2);
				await MissileThrow(skill, caster, position, configA);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1800));

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 230, height: 2);
				await MissileThrow(skill, caster, position, configB);
				if (i < 9)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_dionia_Skill_6)]
	public class MonBossDioniaSkill6 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(10199);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, forceId, null);
			//caster.PlayAnimation("SKL3", false, 0, 0.7, 0.2);
			//caster.PlayAnimation("SKL3", false, 4000, 0.8, 0.2);
			caster.PlayAnimation("SKL3", false, 0);
			caster.PlayAnimation("SKL3", false, 4000);


			skill.Run(this.HandleSkill(caster, target, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var innerConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 600,
				Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.5f),
				Range = 25,
				KnockdownPower = 150,
				Delay = 0,
				HitCount = 1,
				HitDuration = 1000,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0,
				InnerRange = 0,
			};

			var outerConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 600,
				Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
				Range = 35,
				KnockdownPower = 150,
				Delay = 0,
				HitCount = 1,
				HitDuration = 1000,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0,
				InnerRange = 0,
			};

			var ringAngles = new[] { 0f, -0.69813168f, -1.5707964f, -2.4434609f, -3.1415927f, 2.4434609f, 1.5707964f, 0.69813168f };

			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			// Inner ring: distance=50
			foreach (var angle in ringAngles)
			{
				var position = GetRelativePosition(PosType.Self, caster, target, distance: 50, angle: angle);
				await EffectAndHit(skill, caster, position, innerConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(4000));

			// Outer ring: distance=150, first pass (same 8 angles)
			foreach (var angle in ringAngles)
			{
				var position = GetRelativePosition(PosType.Self, caster, target, distance: 150, angle: angle);
				await EffectAndHit(skill, caster, position, outerConfig);
			}

			// Outer ring: distance=150, second pass (offset angles)
			var offsetAngles = new[] { 0.69813168f, -0.34906584f, -1.134464f, -2.0071287f, 2.0071287f, 1.134464f, 0.34906584f, -2.7925267f, 2.7925267f };
			foreach (var angle in offsetAngles)
			{
				var position = GetRelativePosition(PosType.Self, caster, target, distance: 150, angle: angle);
				await EffectAndHit(skill, caster, position, outerConfig);
			}
		}
	}
}
