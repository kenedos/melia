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
	[SkillHandler(SkillId.Mon_boss_Ravinepede_Skill_1)]
	public class Mon_boss_Ravinepede_Skill_1 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 15));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Ravinepede_Skill_2)]
	public class Mon_boss_Ravinepede_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 10, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			var startingPosition = originPos;
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1500f,
				HitEffect = new EffectConfig("F_explosion006_orange", 0.5f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.25f,
				HitCount = 1,
				HitDuration = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Ravinepede_Skill_3)]
	public class Mon_boss_Ravinepede_Skill_3 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, rand: 110);
			MonsterSkillCreateMobPC(skill, caster, "RavineLerva_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "RavineLerva_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 48, angle: -6f);
			MonsterSkillCreateMobPC(skill, caster, "RavineLerva_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Ravinepede_Skill_4)]
	public class Mon_boss_Ravinepede_Skill_4 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 10, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.03f,
				ArrowLifeTime = 0.03f,
				PositionDelay = 1700f,
				HitEffect = new EffectConfig("F_explosion006_orange##0.5", 0.5f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.2f,
				HitCount = 1,
				HitDuration = 0f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 37.061462f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 10000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Ravinepede_Skill_5)]
	public class Mon_boss_Ravinepede_Skill_5 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 15));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 210);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 70);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 90);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoisonPilla);
		}
	}
}
