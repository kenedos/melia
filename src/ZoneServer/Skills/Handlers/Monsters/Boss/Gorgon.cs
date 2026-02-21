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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Gorgon_Skill_1)]
	public class Mon_boss_Gorgon_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1150);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 80f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 950;
			var damageDelay = 1150;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gorgon_Skill_2)]
	public class Mon_boss_Gorgon_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 0, angle: 70f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 0, angle: 70f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 2600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 0, angle: 70f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 2700;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 0, angle: 70f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 2800;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gorgon_Skill_3)]
	public class Mon_boss_Gorgon_Skill_3 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 20);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8.5f),
				PositionDelay = 1400,
				Effect = new EffectConfig("F_spread_out004_dark", 1.2f),
				Range = 120f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
			position = originPos.GetRelative(farPos, distance: 20);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8.5f),
				PositionDelay = 1400,
				Effect = new EffectConfig("F_spread_out004_dark", 1.2f),
				Range = 120f,
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
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gorgon_Skill_4)]
	public class Mon_boss_Gorgon_Skill_4 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2600;
			var damageDelay = 2600;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 2000f, 1, 100, -1, hits);

			var startingPosition = originPos;
			var endingPosition = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.3f,
				PositionDelay = 2500f,
				HitEffect = new EffectConfig("F_ground122_dark##1", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 18f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}
}
