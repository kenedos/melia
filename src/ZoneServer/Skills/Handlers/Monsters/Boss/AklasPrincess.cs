using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters
{
	[SkillHandler(SkillId.Mon_boss_aklasprincess_Skill_1)]
	public class MonBossAklasprincessSkill1 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = GetRelativePosition(PosType.Self, caster);
			var skillTargets = SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20);
			await skill.Wait(TimeSpan.FromMilliseconds(1800));

			var missileConfig092 = new MissileConfig
			{
				Effect = new EffectConfig("I_force092_trail_pink#Bip001 R Finger3Nub", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				Range = 15f,
				FlyTime = 0.75f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var missileConfig017 = new MissileConfig
			{
				Effect = new EffectConfig("I_force017_trail4_addtive2#Bip001 R Finger3Nub", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				Range = 15f,
				FlyTime = 0.75f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var missileConfig093 = new MissileConfig
			{
				Effect = new EffectConfig("I_force093_trail_pink#Bip001 R Finger3Nub", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				Range = 15f,
				FlyTime = 0.75f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var missileConfig094 = new MissileConfig
			{
				Effect = new EffectConfig("I_force094_trail_pink#Bip001 R Finger3Nub", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				Range = 15f,
				FlyTime = 0.75f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileThrow(skill, caster, position, missileConfig092);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileThrow(skill, caster, position, missileConfig017);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileThrow(skill, caster, position, missileConfig017);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileThrow(skill, caster, position, missileConfig092);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileThrow(skill, caster, position, missileConfig092);

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 170);
				await MissileThrow(skill, caster, position, missileConfig093);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 170);
				await MissileThrow(skill, caster, position, missileConfig094);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_aklasprincess_Skill_2)]
	public class MonBossAklasprincessSkill2 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 5f),
				Range = 90f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var hitConfigNoneEffect = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 90f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var fallConfig05 = new MissileConfig
			{
				Effect = new EffectConfig("I_force093_trail_pink_loop#Bip001 R Finger3Nub", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				DelayTime = 0f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 0f,
				KnockType = (KnockType)1,
				VerticalAngle = 0f,
			};

			var fallConfig07 = new MissileConfig
			{
				Effect = new EffectConfig("I_force093_trail_pink_loop#Bip001 R Finger3Nub", 0.69999999f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				DelayTime = 0f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 0f,
				KnockType = (KnockType)1,
				VerticalAngle = 0f,
			};

			var fallConfig09 = new MissileConfig
			{
				Effect = new EffectConfig("I_force093_trail_pink_loop#Bip001 R Finger3Nub", 0.89999998f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				DelayTime = 0f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 0f,
				KnockType = (KnockType)1,
				VerticalAngle = 0f,
			};

			var fallConfig084 = new MissileConfig
			{
				Effect = new EffectConfig("I_force084_pink", 0.5f),
				EndEffect = new EffectConfig("F_blood007_pink", 0.75f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				DelayTime = 0f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 0f,
				KnockType = (KnockType)1,
				VerticalAngle = 0f,
			};

			var position = GetRelativePosition(PosType.Self, caster);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 6f),
				Range = 90f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});

			var delays = new[] { 600, 250, 250, 250, 250, 250, 250 };
			for (var i = 0; i < 7; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
				position = GetRelativePosition(PosType.Self, caster);
				await EffectAndHit(skill, caster, position, hitConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig05);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			position = GetRelativePosition(PosType.Self, caster);
			await EffectAndHit(skill, caster, position, hitConfig);

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig07);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig07);
			}

			position = GetRelativePosition(PosType.Self, caster);
			await EffectAndHit(skill, caster, position, hitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig05);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig05);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(50));

			position = GetRelativePosition(PosType.Self, caster);
			await EffectAndHit(skill, caster, position, hitConfigNoneEffect);

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig09);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, rand: 140, height: 1);
				await MissileFall(caster, skill, position, fallConfig084);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_aklasprincess_Skill_3)]
	public class MonBossAklasprincessSkill3 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var startingPosition = GetRelativePosition(PosType.Self, caster, target, distance: 40);
			var endingPosition = GetRelativePosition(PosType.Self, caster, target, distance: 280);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_ground160_dark_poison", 2f),
				Range = 50f,
				KnockdownPower = 1f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.15f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_aklasprincess_Skill_4)]
	public class MonBossAklasprincessSkill4 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			var summonNames = new[]
			{
				"aklaspetal_summon",
				"aklasbishop_summon",
				"aklaschurl_summon",
				"aklascountess_summon",
				"aklasia_summon",
				"aklaspetal_summon",
				"aklaspetal_summon",
				"aklaspetal_summon",
				"aklasbishop_summon",
				"aklaschurl_summon",
				"aklaschurl_summon",
				"aklascountess_summon",
			};

			foreach (var summonName in summonNames)
			{
				var spawnPos = GetRelativePosition(PosType.Self, caster, rand: 110, height: 1);
				MonsterSkillCreateMobPC(skill, caster, summonName, spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var matk = caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK) / 2;
			caster.StartBuff(BuffId.Mon_Heal_Buff, skill.Level, matk, TimeSpan.FromMilliseconds(3000), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_aklasprincess_Skill_5)]
	public class MonBossAklasprincessSkill5 : ITargetSkillHandler
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
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_dark", 0.5f),
				Range = 40f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(3200));
			var targetPos = GetRelativePosition(PosType.Self, caster, target, distance: 141, angle: 0f);
			SkillCreatePad(caster, skill, targetPos, -0.10175269f, PadName.Mon_PoleofAgony);
			var position = GetRelativePosition(PosType.Self, caster, distance: 141.0013, angle: 0f);
			await EffectAndHit(skill, caster, position, hitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = GetRelativePosition(PosType.Self, caster, distance: 261.54001, angle: 6f);
			SkillCreatePad(caster, skill, targetPos, 0.12147491f, PadName.Mon_PoleofAgony);
			position = GetRelativePosition(PosType.Self, caster, distance: 261.53659, angle: 6f);
			await EffectAndHit(skill, caster, position, hitConfig);
		}
	}
}
