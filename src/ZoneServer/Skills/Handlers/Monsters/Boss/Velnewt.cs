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
	[SkillHandler(SkillId.Mon_boss_velnewt_Skill_1)]
	public class Mon_boss_velnewt_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1100);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 40f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 1000;
			damageDelay = 2100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			var position = originPos.GetRelative(farPos, distance: 40);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 45f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_velnewt_Skill_2)]
	public class Mon_boss_velnewt_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 65);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("I_explosion002_green", 1.5f),
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
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_velnewt_Skill_3)]
	public class Mon_boss_velnewt_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2550));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion004_mint", 0.3f),
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
			};

			var position = originPos.GetRelative(farPos, distance: 55.198566f, angle: -26f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 55.075836f, angle: 26f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_velnewt_Skill_4)]
	public class Mon_boss_velnewt_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green2#B_light 01", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 2f),
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

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 40, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green2#B_light 02", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 2f),
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

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var missileConfig3 = new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green2#B_light 03", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 2f),
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

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 40, height: 1);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var missileConfig4 = new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green2#B_light 04", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 2f),
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

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, missileConfig4);
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig4);
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 50, height: 2);
			await MissileThrow(skill, caster, position, missileConfig4);
		}
	}
}
