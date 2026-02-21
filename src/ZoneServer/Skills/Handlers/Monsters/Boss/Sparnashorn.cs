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
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Sparnashorn_Skill_1)]
	public class Mon_boss_Sparnashorn_Skill_1 : ITargetSkillHandler
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

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var targetHandle = target?.Handle ?? 0;
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2600));
			var position = originPos.GetRelative(farPos, distance: 30);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground084_ice", 3.5f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnashorn_Skill_2)]
	public class Mon_boss_Sparnashorn_Skill_2 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos, target));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 100);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 100, rand: 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force003_blue#Bone_Jaw", 2f),
				EndEffect = new EffectConfig("F_smoke010_blue", 2f),
				Range = 50f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnashorn_Skill_3)]
	public class Mon_boss_Sparnashorn_Skill_3 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke044", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 30, angle: 15f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 30, angle: -15f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 30, angle: 15f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Sparnashorn_Skill_4)]
	public class Mon_boss_Sparnashorn_Skill_4 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos, target));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var hits = new List<SkillHitInfo>();
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("F_spin004", 0.6f),
				EndEffect = new EffectConfig("F_spin016_blue", 1.4f),
				Range = 30f,
				FlyTime = 0.6f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 1000f, 1, 100, -1, hits);
		}
	}
}
