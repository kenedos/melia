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
using Yggdrasil.Extensions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_moa_Skill_1)]
	public class Mon_boss_moa_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1600;
			var damageDelay = 1800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1500;
			damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_moa_Skill_2)]
	public class Mon_boss_moa_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 40, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_moa_Skill_3)]
	public class Mon_boss_moa_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.CasterForward, 150, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_moa_Skill_4)]
	public class Mon_boss_moa_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(900));

			// Create 9 dust effects in 360 degrees around caster
			for (var i = 0; i < 9; i++)
			{
				var angle = i * 40f; // 360 / 9 = 40 degrees apart
				var direction = new Direction(angle);
				var position = originPos.GetRelative(direction, 40f);
				skill.Run(EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("None", 3f),
					PositionDelay = 1000,
					Effect = new EffectConfig("I_smoke017_spread_out", 1.5f),
					Range = 45f,
					KnockdownPower = 100f,
					Delay = 0f,
					HitCount = 1,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				}));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_moa_Skill_5)]
	public class Mon_boss_moa_Skill_5 : ITargetSkillHandler
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
			// Fan attack with knockdown like skill 3
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();

			// Start attack and wait for hit delay
			await skill.Wait(TimeSpan.FromMilliseconds(hitDelay));

			// Fire explosions right after hit delay with UC_blind
			var explosionHits = new List<SkillHitInfo>();
			for (var i = 0; i < 9; i++)
			{
				var angle = i * 40f;
				var direction = new Direction(angle);
				var position = originPos.GetRelative(direction, 40f);
				skill.Run(EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("None", 3f),
					PositionDelay = 0,
					Effect = new EffectConfig("F_explosion001_dark##0.5", 1.5f),
					Range = 45f,
					KnockdownPower = 100f,
					Delay = 0f,
					HitCount = 1,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				}, explosionHits));
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 5000f, 1, 100, -1, explosionHits);

			// Continue with the fan attack
			await SkillAttack(caster, skill, splashArea, 0, damageDelay - hitDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.CasterForward, 150, 30, 10, 1, 5, hits);
		}
	}
}
