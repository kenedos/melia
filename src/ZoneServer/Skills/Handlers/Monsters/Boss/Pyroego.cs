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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Pyroego_pattern_Skill_1)]
	public class Mon_boss_Pyroego_pattern_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var position = GetRelativePosition(PosType.Target, caster, target, height: 1);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_wizard_fireball_cast_fire3#Bip001 Ponytail1Nub", 5f),
				EndEffect = new EffectConfig("F_spread_out022_fire", 5f),
				DotEffect = EffectConfig.None,
				Range = 150f,
				FlyTime = 2.5f,
				DelayTime = 0.3f,
				Gravity = 900f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var hits = new List<SkillHitInfo>();
			position = GetRelativePosition(PosType.Target, caster, target);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##1", 5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_fire051_ground", 4f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 0,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Mythic_stun, 99, 0f, 3000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_1)]
	public class Mon_boss_Pyroego_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 600;
			damageDelay = 1800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_2)]
	public class Mon_boss_Pyroego_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1600));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force006_fire#Bip001 L Finger1", 2f),
				EndEffect = new EffectConfig("F_burstup027_fire1", 2.5f),
				DotEffect = EffectConfig.None,
				Range = 23f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			var groupSizes = new[] { 3, 3, 3, 3, 4 };
			for (var g = 0; g < groupSizes.Length; g++)
			{
				for (var i = 0; i < groupSizes[g]; i++)
				{
					var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 160, height: 1);
					await MissileThrow(skill, caster, position, config);
				}

				if (g < groupSizes.Length - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_3)]
	public class Mon_boss_Pyroego_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(farPos, distance: 111.77953f, angle: -92f, height: 1);
			SkillCreatePad(caster, skill, targetPos, -1.6143024f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(farPos, distance: 102.93429f, angle: 85f);
			SkillCreatePad(caster, skill, targetPos, 1.4957336f, PadName.Grinender_FirePillar);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_4)]
	public class Mon_boss_Pyroego_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			var rightConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Bip001 R Finger1", 2.5f),
				EndEffect = new EffectConfig("F_burstup028_fire", 1f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var leftConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Bip001 L Finger1", 2.5f),
				EndEffect = new EffectConfig("F_burstup028_fire", 1f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				FlyTime = 2f,
				DelayTime = 0.05f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 180, height: 1);
				await MissileThrow(skill, caster, position, rightConfig);

				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150, height: 2);
				if (i < 6)
					await MissileThrow(skill, caster, position, leftConfig);
				else
					await MissileThrow(skill, caster, position, new MissileConfig
					{
						Effect = new EffectConfig("I_force054_fire#Bip001 L Finger1", 2.5f),
						EndEffect = new EffectConfig("I_explosion009_orange", 2f),
						DotEffect = EffectConfig.None,
						Range = 20f,
						FlyTime = 2f,
						DelayTime = 0.05f,
						Gravity = 600f,
						Speed = 1f,
						HitTime = 1000f,
						HitCount = 1,
						GroundEffect = EffectConfig.None,
					});

				if (i < 6)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_5)]
	public class Mon_boss_Pyroego_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			var forwardConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var reverseConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 7; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 20f);
				var endingPosition = originPos.GetRelative(farPos, distance: 250f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, forwardConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			for (var i = 0; i < 6; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 250f);
				var endingPosition = originPos.GetRelative(farPos, distance: 20f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, reverseConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Pyroego_Skill_6)]
	public class Mon_boss_Pyroego_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			var forwardConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var reverseConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 7; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 20f);
				var endingPosition = originPos.GetRelative(farPos, distance: 300f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, forwardConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			targetPos = originPos.GetRelative(farPos, distance: 88.103012f, angle: 125f);
			SkillCreatePad(caster, skill, targetPos, 2.1865909f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(farPos, distance: 128.49944f, angle: -49f, height: 1);
			SkillCreatePad(caster, skill, targetPos, -0.86019135f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(farPos, distance: 117.72496f, angle: -118f);
			SkillCreatePad(caster, skill, targetPos, -2.0700257f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(farPos, distance: 131.73189f, angle: 41f);
			SkillCreatePad(caster, skill, targetPos, 0.71808273f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			for (var i = 0; i < 6; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 300f);
				var endingPosition = originPos.GetRelative(farPos, distance: 20f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, reverseConfig);
			}
		}
	}
}
