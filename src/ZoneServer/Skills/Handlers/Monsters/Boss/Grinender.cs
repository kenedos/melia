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
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;
namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Grinender_Skill_1)]
	public class Mon_boss_Grinender_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 0, angle: 90);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 8000f, 1, 100, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			var targetPos = originPos.GetRelative(caster.Direction.Left, 30f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetRelative(caster.Direction.Right, 30f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Grinender_Skill_2)]
	public class Mon_boss_Grinender_Skill_2 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 230f);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.2f,
				PositionDelay = 2400f,
				HitEffect = new EffectConfig("E_grinender_skl1", 0.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});

		}
	}

	[SkillHandler(SkillId.Mon_boss_Grinender_Skill_3)]
	public class Mon_boss_Grinender_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2010));
		}
	}

	[SkillHandler(SkillId.Mon_boss_Grinender_Skill_4)]
	public class Mon_boss_Grinender_Skill_4 : ITargetSkillHandler
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200f);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
		}
	}
}
