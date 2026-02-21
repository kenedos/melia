using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Skill_boss_Spector_m_Dummy_Skill_1)]
	public class Mon_Skill_boss_Spector_m_Dummy_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_rize006", 0.2f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 16; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 70);
				await EffectAndHit(skill, caster, position, config);

				if (i < 15)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
			}
		}
	}

	[SkillHandler(SkillId.Mon_Skill_boss_Strongholder_Dummy_Skill_1)]
	public class Mon_Skill_boss_Strongholder_Dummy_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 12.135043f, angle: -50f);
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red", 0.80000001f),
				EndEffect = new EffectConfig("I_force032_red", 1.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 1f,
				FlyTime = 1.5f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.80000001f),
			};

			for (var i = 0; i < 6; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target);
				await MissileFall(caster, skill, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_Skill_F_boss_Chapparition_Dummy_Skill_1)]
	public class Mon_Skill_F_boss_Chapparition_Dummy_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 250f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_explosion050_fire_violet", 0.2f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_rize006", 0.5f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 200, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_explosion050_fire_violet", 0.2f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_rize006", 0.5f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 200, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 200, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 200, height: 1);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 200, height: 1);
				await EffectAndHit(skill, caster, position, effectHitConfig2);

				if (i < 3)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 200);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
				await EffectAndHit(skill, caster, position, effectHitConfig);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}
		}
	}

	[SkillHandler(SkillId.Mon_Skill_Glassmole_Dummy_Skill_1)]
	public class Mon_Skill_Glassmole_Dummy_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1850));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_glassmole_skl1_mash_down", 1f),
				EndEffect = new EffectConfig("E_glassmole_skl1_obj", 1f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 2.5f,
				FlyTime = 0.3f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.40000001f),
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var delays = new[] { 250, 150, 250, 250, 2800, 100, 100, 300, 300, 300 };
			for (var i = 0; i < 10; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 1);
				await MissileFall(caster, skill, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
			var delays2 = new[] { 300, 300, 300, 300, 350, 350, 350, 350 };
			for (var i = 0; i < 9; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80, height: 1);
				await MissileFall(caster, skill, position, config);

				if (i < delays2.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays2[i]));
			}
		}
	}
}
