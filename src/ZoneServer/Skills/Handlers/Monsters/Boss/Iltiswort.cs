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
	[SkillHandler(SkillId.Mon_boss_Iltiswort_Skill_1)]
	public class Mon_boss_Iltiswort_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, distance: 73.936501f, angle: 13f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 900,
				Effect = new EffectConfig("F_burstup008_smoke1", 1.5f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Iltiswort_Skill_2)]
	public class Mon_boss_Iltiswort_Skill_2 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var targetPos = originPos.GetRelative(farPos, distance: 150);
			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force055_violet#Dummy_lip_M_effect", 1.3f),
				EndEffect = new EffectConfig("F_burstup012_blue", 0.6f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			};

			for (var i = 0; i < 12; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 150, rand: 110, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);
				if (i < 11)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Iltiswort_Skill_3)]
	public class Mon_boss_Iltiswort_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force055_violet#Dummy_lip_M_effect", 2f),
				EndEffect = new EffectConfig("F_ground004_violet", 1.5f),
				DotEffect = EffectConfig.None,
				Range = 80f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 7f),
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force055_violet#Dummy_lip_M_effect", 2.5f),
				EndEffect = new EffectConfig("F_ground004_violet", 1.5f),
				DotEffect = EffectConfig.None,
				Range = 80f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 7f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Iltiswort_Skill_4)]
	public class Mon_boss_Iltiswort_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var hits = new List<SkillHitInfo>();

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_burstup023_smoke##1", 0.8f),
				PositionDelay = 1900,
				Effect = new EffectConfig("F_burstup008_smoke1", 1.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var delays = new[] { 400, 400, 1200, 450, 250, 300, 300, 700, 200, 300, 300 };
			for (var i = 0; i < 12; i++)
			{
				Position position;
				if (i < 4)
					position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190, height: 2);
				else if (i < 8)
					position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 190, height: 1);
				else
					position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 180, height: 1);

				await EffectAndHit(skill, caster, position, hitConfig, hits);
				if (i < 11)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 5000f, 1, 30, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Iltiswort_Skill_5)]
	public class Mon_boss_Iltiswort_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var smallMissile = new MissileConfig
			{
				Effect = new EffectConfig("I_force055_violet#Dummy_lip_M_effect", 2f),
				EndEffect = new EffectConfig("F_ground004_violet", 1.2f),
				DotEffect = EffectConfig.None,
				Range = 60f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 7f),
			};

			var largeMissile = new MissileConfig
			{
				Effect = new EffectConfig("I_force055_violet#Dummy_lip_M_effect", 2.5f),
				EndEffect = new EffectConfig("F_ground004_violet", 1.2f),
				DotEffect = EffectConfig.None,
				Range = 60f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 7f),
			};

			var position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, smallMissile);
			position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, smallMissile);

			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, largeMissile);
			position = originPos.GetRelative(farPos, rand: 140);
			await MissileThrow(skill, caster, position, largeMissile);

			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetRelative(farPos, rand: 140);
				await MissileThrow(skill, caster, position, largeMissile);
			}
		}
	}
}
