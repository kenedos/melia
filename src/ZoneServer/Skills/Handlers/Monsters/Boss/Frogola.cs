using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	// Q1 Variant Skills
	[SkillHandler(SkillId.Mon_boss_Frogola_Q1_Skill_1)]
	public class Mon_boss_Frogola_Q1_Skill_1 : ITargetSkillHandler
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

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var configL = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_eye L2", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var configR = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_eye R2", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			// Converging pairs fired at decreasing angles, then reversing back out
			var anglePairs = new[]
			{
				(39f, -39f),
				(30f, -30f),
				(19f, -19f),
				(9f, -9f),
				(-9f, 9f),
				(-19f, 19f),
				(-30f, 30f),
				(39f, -39f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1800));

			// First pair uses R2 effect for the negative angle side
			var position = originPos.GetRelative(farPos, distance: 120, angle: anglePairs[0].Item1);
			await MissileThrow(skill, caster, position, configL);
			position = originPos.GetRelative(farPos, distance: 120, angle: anglePairs[0].Item2);
			await MissileThrow(skill, caster, position, configR);

			for (var i = 1; i < anglePairs.Length; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(200));
				position = originPos.GetRelative(farPos, distance: 120, angle: anglePairs[i].Item1);
				await MissileThrow(skill, caster, position, configL);
				position = originPos.GetRelative(farPos, distance: 120, angle: anglePairs[i].Item2);
				await MissileThrow(skill, caster, position, configL);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Q1_Skill_2)]
	public class Mon_boss_Frogola_Q1_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2200);
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

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 400;
			damageDelay = 2600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 2800;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 10000f, 1, 2, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Q1_Skill_3)]
	public class Mon_boss_Frogola_Q1_Skill_3 : ITargetSkillHandler
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

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);

			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_bubble003_green", 1f),
				PositionDelay = 500,
				Effect = new EffectConfig("F_burstup011_violet", 1f),
				Range = 30f,
				KnockdownPower = 180f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var hits = new List<SkillHitInfo>();

			var position = originPos.GetRelative(farPos, distance: 90, angle: -159f);
			await EffectAndHit(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 100, angle: 39f);
			await EffectAndHit(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110);
			await EffectAndHit(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110);
			await EffectAndHit(skill, caster, position, config, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await EffectAndHit(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, config, hits);
			position = originPos.GetRelative(farPos, distance: 105, angle: -169f);
			await EffectAndHit(skill, caster, position, config, hits);
			position = originPos.GetRelative(farPos, distance: 80, angle: 60f);
			await EffectAndHit(skill, caster, position, config, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120);
			await EffectAndHit(skill, caster, position, config, hits);
			position = originPos.GetRelative(farPos, distance: 80, angle: 180f);
			await EffectAndHit(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await EffectAndHit(skill, caster, position, config, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, 5000f, 1, 3, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Q1_Skill_4)]
	public class Mon_boss_Frogola_Q1_Skill_4 : ITargetSkillHandler
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

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			// First wave: forward arc
			var firstWave = new[]
			{
				(95.182526f, -71f),
				(79.850021f, -53f),
				(114.86465f, -55f),
				(82.437714f, -26f),
				(32.468433f, -2f),
				(81.43351f, 6f),
				(68.218147f, 54f),
				(111.06789f, 49f),
				(113.39835f, -26f),
				(105.28146f, 11f),
				(144.14546f, -3f),
				(127.03156f, -36f),
				(143.13211f, 19f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(2500));

			foreach (var (dist, angle) in firstWave)
			{
				var position = originPos.GetRelative(farPos, distance: dist, angle: angle, rand: 20, height: 1);
				await MissileThrow(skill, caster, position, config);
			}

			// Second wave: rear arc
			var secondWave = new[]
			{
				(132.23003f, 143f),
				(132.39554f, 159f),
				(133.3774f, 174f),
				(138.41817f, -172f),
				(128.25478f, -158f),
				(162.28583f, -176f),
				(173.64447f, 158f),
				(96.333656f, -174f),
				(104.32298f, 148f),
				(156.95863f, 163f),
				(160.77681f, -170f),
				(174.3107f, 139f),
				(149.3418f, 164f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(800));

			foreach (var (dist, angle) in secondWave)
			{
				var position = originPos.GetRelative(farPos, distance: dist, angle: angle, rand: 20, height: 1);
				await MissileThrow(skill, caster, position, config);
			}

			// Third wave: forward arc repeat
			var thirdWave = new[]
			{
				(105.28146f, 11f),
				(113.39835f, -26f),
				(111.06789f, 49f),
				(68.218147f, 54f),
				(81.43351f, 6f),
				(32.468433f, -2f),
				(82.437714f, -26f),
				(114.86465f, -55f),
				(79.850021f, -53f),
				(95.182526f, -71f),
				(143.13211f, 19f),
				(127.03156f, -36f),
				(144.14546f, -3f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(700));

			foreach (var (dist, angle) in thirdWave)
			{
				var position = originPos.GetRelative(farPos, distance: dist, angle: angle, rand: 20, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	// Regular Frogola Skills
	[SkillHandler(SkillId.Mon_boss_Frogola_Skill_1)]
	public class Mon_boss_Frogola_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#Bip001 Spine", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1600));

			for (var i = 0; i < 10; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 55, rand: 50);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Skill_2)]
	public class Mon_boss_Frogola_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2150);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 25);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1950;
			var damageDelay = 2150;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 25);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 350;
			damageDelay = 2500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 10000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Skill_3)]
	public class Mon_boss_Frogola_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup011_violet", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var targetPos = originPos.GetRelative(farPos);

			var delays = new[] { 300, 0, 400, 0, 300, 0, 500, 0 };

			for (var i = 0; i < 8; i++)
			{
				if (delays[i] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));

				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Frogola_Skill_4)]
	public class Mon_boss_Frogola_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			var hits = new List<SkillHitInfo>();

			var lobConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 2.4f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var directConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			// Lobbed missiles with increasing fly times
			var flyTimes = new[] { 2.4f, 2.4f, 2.5f, 2.5f, 2.6f, 2.6f, 2.7f, 2.7f };

			await skill.Wait(TimeSpan.FromMilliseconds(400));

			for (var i = 0; i < flyTimes.Length; i++)
			{
				lobConfig.FlyTime = flyTimes[i];
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
				await MissileThrow(skill, caster, position, lobConfig);
			}

			// Direct missiles in bursts of 2
			await skill.Wait(TimeSpan.FromMilliseconds(2100));

			for (var i = 0; i < 3; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 55, rand: 50);
				await MissileThrow(skill, caster, position, directConfig);
				position = originPos.GetRelative(farPos, distance: 55, rand: 50);
				await MissileThrow(skill, caster, position, directConfig);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 8000f, 1, 45, -1, hits);
		}
	}
}
