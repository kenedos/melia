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
	[SkillHandler(SkillId.Mon_boss_cactusvel_Skill_1)]
	public class Mon_boss_cactusvel_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 80, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2800;
			var damageDelay = 3000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 150;
			damageDelay = 150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 150;
			damageDelay = 150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 150;
			damageDelay = 150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 150;
			damageDelay = 150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 70, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 150;
			damageDelay = 150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 14000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_cactusvel_Skill_2)]
	public class Mon_boss_cactusvel_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3100);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2900;
			var damageDelay = 3100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 600;
			damageDelay = 600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1000;
			damageDelay = 1000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 45, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1000;
			damageDelay = 1000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 45, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_cactusvel_Skill_3)]
	public class Mon_boss_cactusvel_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force075_pink#Dummy_effect_L_force", 1f),
				EndEffect = new EffectConfig("I_explosion003_pink", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.8", 0.5f),
			};

			Position position;

			for (var i = 0; i < 5; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
				await MissileThrow(skill, caster, position, missileConfig);
			}
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force075_pink#Dummy_effect_R_force", 1f),
				EndEffect = new EffectConfig("I_explosion003_pink", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.8", 0.5f),
			};

			for (var i = 0; i < 5; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
				await MissileThrow(skill, caster, position, missileConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_cactusvel_Skill_4)]
	public class Mon_boss_cactusvel_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			position = originPos.GetRelative(farPos);
		}
	}
}
