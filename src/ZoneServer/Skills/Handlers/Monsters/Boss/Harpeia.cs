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
	[SkillHandler(SkillId.Mon_boss_Harpeia_Skill_1)]
	public class Mon_boss_Harpeia_Skill_1 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 22f, angle: -4f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_spin005_mint", 4f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Harpeia_Skill_2)]
	public class Mon_boss_Harpeia_Skill_2 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = GetRelativePosition(PosType.Target, caster, target, distance: 200);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("F_spin005_mint", 2.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Harpeia_Skill_3)]
	public class Mon_boss_Harpeia_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Harpeia_Skill_4)]
	public class Mon_boss_Harpeia_Skill_4 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 3200,
				Effect = new EffectConfig("F_spin012_blue", 2.5f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 20f,
			}, hits);
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 70);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 200;
			var damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 85);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Harpeia_Skill_5)]
	public class Mon_boss_Harpeia_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(800));

			var hits = new List<SkillHitInfo>();
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.15f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_warrior_whirlwind_shot_smoke", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 300f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 300f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}
}
