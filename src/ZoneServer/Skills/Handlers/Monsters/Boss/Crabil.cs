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
	[SkillHandler(SkillId.Mon_boss_Crabil_Skill_1)]
	public class Mon_boss_Crabil_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1900;
			var damageDelay = 2100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 8000f, 1, 25, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Crabil_Skill_2)]
	public class Mon_boss_Crabil_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2700;
			var damageDelay = 2900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 40f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup041_water_blue", 0.8f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 100f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.5f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 7000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Crabil_Skill_3)]
	public class Mon_boss_Crabil_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2300);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 35);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 35);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 15000f, 1, 18, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Crabil_Skill_4)]
	public class Mon_boss_Crabil_Skill_4 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 1f, 0f, TimeSpan.FromMilliseconds(500f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 1f, 0f, TimeSpan.FromMilliseconds(500f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 1f, 0f, TimeSpan.FromMilliseconds(500f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 2f, 0f, TimeSpan.FromMilliseconds(500f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 2f, 0f, TimeSpan.FromMilliseconds(500f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 3f, 0f, TimeSpan.FromMilliseconds(500f), caster);
		}
	}
}
