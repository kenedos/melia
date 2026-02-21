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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_simorph_Skill_2)]
	public class Mon_boss_simorph_Skill_2 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200, height: 1);
			var hits = new List<SkillHitInfo>();
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_simorph_skl_3Deffect_mash", 0.55000001f),
				EndEffect = new EffectConfig("E_simorph_skl", 1.3f),
				DotEffect = EffectConfig.None,
				Range = 40f,
				DelayTime = 0.3f,
				FlyTime = 0.7f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
			};

			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 6000f, 1, 50, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200, height: 1);
			hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 6000f, 1, 50, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200, height: 1);
			hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 6000f, 1, 50, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200, height: 1);
			hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 6000f, 1, 50, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200, height: 1);
			hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_simorph_Skill_3)]
	public class Mon_boss_simorph_Skill_3 : ITargetSkillHandler
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
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 3f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup008_smoke2", 2f),
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_simorph_Skill_4)]
	public class Mon_boss_simorph_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 120, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Monster_Sleep);
		}
	}

	[SkillHandler(SkillId.Mon_boss_simorph_Skill_7)]
	public class Mon_boss_simorph_Skill_7 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var startingPosition = originPos.GetRelative(farPos, distance: 0);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 15f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.2f,
				PositionDelay = 100f,
				HitEffect = new EffectConfig("F_burstup032_smoke", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 10f,
				HitEffectSpacing = 14f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 50f,
			});
		}
	}
}
