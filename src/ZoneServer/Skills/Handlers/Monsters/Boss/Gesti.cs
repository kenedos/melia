using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_gesti_Q1_Skill_1)]
	public class Mon_boss_gesti_Q1_Skill_1 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var skillTargets = SkillSelectEnemiesInCircle(caster, target.Position, 100f, 20);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 L Finger0", 1f),
				EndEffect = new EffectConfig("F_ground131_dark_red", 1.5f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
			};

			foreach (var skillTarget in skillTargets)
			{
				for (var i = 0; i < 3; i++)
				{
					var position = GetRelativePosition(PosType.TargetDistance, caster, skillTarget, distance: 100, rand: 70, height: 1);
					_ = MissileThrow(skill, caster, position, missileConfig);
				}
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Q1_Skill_2)]
	public class Mon_boss_gesti_Q1_Skill_2 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var fallConfig = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("F_rize006_red", 0.3f),
				DotEffect = new EffectConfig("I_light004_red2##0.5", 1f),
				GroundEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0.2f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
			};

			var fallTimes = new[] { 1500, 1500, 1700, 1700, 1900, 1900, 2100, 2100, 2300, 2300, 2500, 2500 };
			var fallTypes = new[] { PosType.TargetHeight, PosType.TargetHeight, PosType.TargetHeight, PosType.TargetRandomDistance, PosType.TargetHeight, PosType.TargetRandomDistance, PosType.TargetHeight, PosType.TargetRandomDistance, PosType.TargetHeight, PosType.TargetRandomDistance, PosType.TargetRandomDistance, PosType.TargetHeight };
			var padTimes = new[] { 2000, 2500, 3000 };
			var padNames = new[] { PadName.gesti_spread1, PadName.gesti_spread2, PadName.gesti_spread3 };

			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			await EffectAndHit(skill, caster, caster.Position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion078_dark", 0.5f),
				Range = 100f,
				KnockdownPower = 180f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});

			var elapsed = 1100;
			var nextPad = 0;
			for (var i = 0; i < fallTimes.Length; i++)
			{
				while (nextPad < padTimes.Length && padTimes[nextPad] <= fallTimes[i])
				{
					await skill.Wait(TimeSpan.FromMilliseconds(padTimes[nextPad] - elapsed));
					elapsed = padTimes[nextPad];
					MonsterSkillCreatePad(caster, skill, caster.Position, 0f, padNames[nextPad]);
					nextPad++;
				}

				await skill.Wait(TimeSpan.FromMilliseconds(fallTimes[i] - elapsed));
				elapsed = fallTimes[i];

				var position = GetRelativePosition(fallTypes[i], caster, target, rand: 120, height: 1);
				_ = MissileFall(caster, skill, position, fallConfig);
			}

			while (nextPad < padTimes.Length)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(padTimes[nextPad] - elapsed));
				elapsed = padTimes[nextPad];
				MonsterSkillCreatePad(caster, skill, caster.Position, 0f, padNames[nextPad]);
				nextPad++;
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Q1_Skill_3)]
	public class Mon_boss_gesti_Q1_Skill_3 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var fallConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red", 0.8f),
				EndEffect = new EffectConfig("F_ground131_dark_red", 1.5f),
				DotEffect = EffectConfig.None,
				GroundEffect = new EffectConfig("I_force029_red", 1f),
				Range = 30f,
				DelayTime = 0.5f,
				FlyTime = 0.2f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
			};

			var angles = new[] { 0f, 90f, 180f, 270f };

			await skill.Wait(TimeSpan.FromMilliseconds(700));
			foreach (var angle in angles)
				_ = MissileFall(caster, skill, originPos.GetRelative(farPos, distance: 100f, angle: angle), fallConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			foreach (var angle in angles)
			{
				_ = EffectAndHit(skill, caster, originPos.GetRelative(farPos, distance: 100f, angle: angle), new EffectHitConfig
				{
					GroundEffect = EffectConfig.None,
					PositionDelay = 0,
					Effect = new EffectConfig("F_explosion016_red", 1f),
					Range = 80f,
					KnockdownPower = 180f,
					Delay = 0f,
					HitCount = 1,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 4,
					VerticalAngle = 60f,
					InnerRange = 0f,
				});
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Q1_Skill_4)]
	public class Mon_boss_gesti_Q1_Skill_4 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var fallConfig = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("F_rize006_red", 0.3f),
				DotEffect = new EffectConfig("I_light004_red2##0.5", 1f),
				GroundEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0.2f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
			};

			var waves = new[] { 1600, 1800, 2000, 2200, 2400, 2600 };
			var elapsed = 0;
			foreach (var wave in waves)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(wave - elapsed));
				elapsed = wave;

				for (var i = 0; i < 2; i++)
				{
					var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 90, height: 2);
					_ = MissileFall(caster, skill, position, fallConfig);
				}
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Q1_Skill_5)]
	public class Mon_boss_gesti_Q1_Skill_5 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup029_smoke_red", 1f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 18f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(700));
			foreach (var angle in new[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f })
				_ = EffectHitArrow(skill, caster, originPos, originPos.GetRelative(farPos, distance: 100f, angle: angle), arrowConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			await EffectAndHit(skill, caster, caster.Position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out028_dark_fire", 1.25f),
				Range = 120f,
				KnockdownPower = 180f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Skill_1)]
	public class Mon_boss_gesti_Skill_1 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var skillTargets = SkillSelectEnemiesInCircle(caster, target.Position, 100f, 20);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 L Finger0", 1f),
				EndEffect = new EffectConfig("F_ground131_dark_red", 1.5f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
			};

			foreach (var skillTarget in skillTargets)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, skillTarget, distance: 100, rand: 70, height: 1);
				_ = MissileThrow(skill, caster, position, missileConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Skill_2)]
	public class Mon_boss_gesti_Skill_2 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			MonsterSkillCreatePad(caster, skill, caster.Position, 0f, PadName.gesti_spread1);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillCreatePad(caster, skill, caster.Position, 0f, PadName.gesti_spread2);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillCreatePad(caster, skill, caster.Position, 0f, PadName.gesti_spread3);
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Skill_3)]
	public class Mon_boss_gesti_Skill_3 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var skillTargets = SkillSelectEnemiesInCircle(caster, originPos.GetRelative(farPos, distance: 80f), 100f, 20);
			if (!skillTargets.Contains(target))
				skillTargets.Insert(0, target);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 R Finger0", 1f),
				EndEffect = new EffectConfig("F_ground133_red", 1f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
			};

			foreach (var skillTarget in skillTargets)
			{
				var position = GetRelativePosition(PosType.Target, caster, skillTarget, rand: 40, height: 1);
				_ = caster.PlayEffectToGround("F_sys_target_monster", position, 1f, 1300f);
				_ = MissilePadThrow(skill, caster, position, missileConfig, 0f, PadName.gesti_Slow);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_gesti_Skill_4)]
	public class Mon_boss_gesti_Skill_4 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 L Finger02", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red", 1f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			_ = MissileThrow(skill, caster, originPos.GetRelative(farPos, distance: 40f, angle: 40f), missileConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			_ = MissileThrow(skill, caster, originPos.GetRelative(farPos, distance: 40f), missileConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			_ = MissileThrow(skill, caster, originPos.GetRelative(farPos, distance: 40f, angle: -40f), missileConfig);
		}
	}

	[SkillHandler(SkillId.Mon_BW_boss_gesti_Skill_1)]
	public class Mon_BW_boss_gesti_Skill_1 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var skillTargets = SkillSelectEnemiesInCircle(caster, target.Position, 150f, 20);

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 L Finger0", 2f),
				EndEffect = new EffectConfig("F_ground131_dark_red", 2.5f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 40f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			for (var i = 0; i < 5; i++)
			{
				foreach (var skillTarget in skillTargets)
				{
					var position = GetRelativePosition(PosType.TargetDistance, caster, skillTarget, distance: 100, rand: 70, height: 1);
					_ = MissileThrow(skill, caster, position, missileConfig);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_BW_boss_gesti_Skill_2)]
	public class Mon_BW_boss_gesti_Skill_2 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			MonsterSkillCreatePad(caster, skill, caster.Position, 0f, PadName.gesti_spread1);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillCreatePad(caster, skill, caster.Position, 0f, PadName.gesti_spread2);
		}
	}

	[SkillHandler(SkillId.Mon_BW_boss_gesti_Skill_3)]
	public class Mon_BW_boss_gesti_Skill_3 : ITargetSkillHandler
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
			caster.TurnTowards(target);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var skillTargets = SkillSelectEnemiesInCircle(caster, originPos.GetRelative(farPos, distance: 80f), 150f, 20);
			if (!skillTargets.Contains(target))
				skillTargets.Insert(0, target);

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force029_red#Bip01 R Finger0", 1f),
				EndEffect = new EffectConfig("F_ground133_red", 1f),
				DotEffect = EffectConfig.None,
				GroundEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			foreach (var skillTarget in skillTargets)
			{
				for (var i = 0; i < 2; i++)
				{
					var position = GetRelativePosition(PosType.Target, caster, skillTarget, rand: 40, height: 1);
					_ = caster.PlayEffectToGround("F_sys_target_monster", position, 1f, 1300f);
					_ = MissilePadThrow(skill, caster, position, missileConfig, 0f, PadName.BW_gesti_slow);
				}
			}
		}
	}
}
