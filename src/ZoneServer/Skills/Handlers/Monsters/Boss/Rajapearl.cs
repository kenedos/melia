using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Shared.Util;
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
using System.Linq;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_1)]
	public class Mon_boss_Rajapearl_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 80f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1300;
			var aniTime = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_2)]
	public class Mon_boss_Rajapearl_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2100);
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
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 57f, angle: 2f), 35f);
			var hitDelay = 1900;
			var aniTime = 2100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 59f, angle: 14f), 35f);
			hitDelay = 500;
			aniTime = 500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 59f, angle: 14f), 35f);
			hitDelay = 300;
			aniTime = 300;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 59f, angle: 14f), 35f);
			hitDelay = 100;
			aniTime = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 59f, angle: 14f), 35f);
			hitDelay = 100;
			aniTime = 100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 6000f, 1, 40, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_3)]
	public class Mon_boss_Rajapearl_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle006_violet", 1.5f),
				EndEffect = new EffectConfig("F_explosion021", 1f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 0.2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
			};

			for (var i = 0; i < 12; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				var position = originPos.GetNearestPositionWithinDistance(GetLeadPositionScatter(target, 800, 60, caster), 250f);
				_ = this.Fall(caster, skill, position, config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}

		private async Task Fall(ICombatEntity caster, Skill skill, Position position, MissileConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_4)]
	public class Mon_boss_Rajapearl_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle006_violet", 1.5f),
				EndEffect = new EffectConfig("F_explosion021", 1f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 0.2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
			};

			for (var i = 0; i < 12; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				var position = originPos.GetNearestPositionWithinDistance(GetLeadPositionScatter(target, 800, 60, caster), 250f);
				_ = this.Fall(caster, skill, position, config);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}

		private async Task Fall(ICombatEntity caster, Skill skill, Position position, MissileConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await MissileFall(caster, skill, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 5, -1, hits);
		}
	}
}
