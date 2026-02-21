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
	[SkillHandler(SkillId.Mon_boss_Marionette_Skill_1)]
	public class Mon_boss_Marionette_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_violet", 1f),
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 60);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var darkBurstConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_dark", 1f),
				Range = 30f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 210, angle: 22f);
			await EffectAndHit(skill, caster, position, darkBurstConfig, hits);
			position = originPos.GetRelative(farPos, distance: 210);
			await EffectAndHit(skill, caster, position, darkBurstConfig, hits);
			position = originPos.GetRelative(farPos, distance: 210, angle: -22f);
			await EffectAndHit(skill, caster, position, darkBurstConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 210, angle: 22f);
			position = originPos.GetRelative(farPos, distance: 210);
			position = originPos.GetRelative(farPos, distance: 210, angle: -22f);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 5000f, 1, 3, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Marionette_Skill_2)]
	public class Mon_boss_Marionette_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup002_dark", 3f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 20f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_violet", 1.5f),
				Range = 90f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 50f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 5000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Marionette_Skill_3)]
	public class Mon_boss_Marionette_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(4400));

			var smokeConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 0.8f),
				Range = 75f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			};

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark2_loop#Bip001 R Hand#2", 5f),
				EndEffect = new EffectConfig("F_burstup004_dark##1", 1f),
				DotEffect = EffectConfig.None,
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 5; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(50));

				var position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, smokeConfig);

				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);

				if (i < 4)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(50));

					position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
					await MissileThrow(skill, caster, position, missileConfig);
					position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
					await MissileThrow(skill, caster, position, missileConfig);
				}
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var finalPos = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, finalPos, smokeConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			finalPos = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, finalPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 0.8f),
				Range = 75f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Marionette_Skill_4)]
	public class Mon_boss_Marionette_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3550));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground077_smoke", 1f),
				Range = 120f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});

			var darkBurstConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_dark", 1f),
				Range = 45f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var delaysAndDistances = new[] { (50, 90), (600, 220), (800, 330), (800, 380), (1000, 430) };
			foreach (var (delay, distance) in delaysAndDistances)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = originPos.GetRelative(farPos, distance: distance);
				await EffectAndHit(skill, caster, position, darkBurstConfig);
			}
		}
	}
}
