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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_1)]
	public class Mon_boss_Rajapearl_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1500);
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
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rajapearl_Skill_2)]
	public class Mon_boss_Rajapearl_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2100);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1900;
			var damageDelay = 2100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 2600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2900;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 3000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 3100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 6000f, 1, 100, -1, hits);
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

			var rnd = new Random();
			for (var i = 0; i < 12; i++)
			{
				var hits = new List<SkillHitInfo>();
				var angle = rnd.NextDouble() * Math.PI * 2;
				var distance = rnd.NextDouble() * 30;
				var missilePos = new Position(
					target.Position.X + (float)(Math.Cos(angle) * distance),
					target.Position.Y,
					target.Position.Z + (float)(Math.Sin(angle) * distance)
				);
				await MissileFall(caster, skill, missilePos, new MissileConfig
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
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 5, -1, hits);
				if (i < 11)
					await skill.Wait(TimeSpan.FromMilliseconds(250));
			}
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

			for (var i = 0; i < 12; i++)
			{
				var hits = new List<SkillHitInfo>();
				await MissileFall(caster, skill, target.Position, new MissileConfig
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
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 5, -1, hits);
				if (i < 11)
					await skill.Wait(TimeSpan.FromMilliseconds(250));
			}
		}
	}
}
