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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_1)]
	public class Mon_boss_BiteRegina_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2800;
			var damageDelay = 3000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_2)]
	public class Mon_boss_BiteRegina_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1700;
			var damageDelay = 1800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_3)]
	public class Mon_boss_BiteRegina_Skill_3 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_4)]
	public class Mon_boss_BiteRegina_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 60);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force003_green#Bone005", 1.5f),
				EndEffect = new EffectConfig("I_explosion013_green", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.2f),
			};

			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await MissileThrow(skill, caster, position, config);
			//SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_5)]
	public class Mon_boss_BiteRegina_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 60);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Bone005", 1f),
				EndEffect = new EffectConfig("I_explosion013_green", 2f),
				Range = 30f,
				FlyTime = 0.9f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2),
			};

			var delays = new[] { 150, 150, 150, 50, 250, 150, 1600, 150, 150, 150, 150, 150, 150, 150 };
			for (var i = 0; i < 15; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await MissileThrow(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
			//SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_BiteRegina_Skill_6)]
	public class Mon_boss_BiteRegina_Skill_6 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 50, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 100, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var position = originPos.GetRelative(farPos, distance: 30);
		}
	}
}
