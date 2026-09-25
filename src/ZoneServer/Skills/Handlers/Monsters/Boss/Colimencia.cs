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
	[SkillHandler(SkillId.Mon_boss_Colimencia_Skill_1)]
	public class Mon_boss_Colimencia_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1700;
			var aniTime = 1900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Colimencia_Skill_2)]
	public class Mon_boss_Colimencia_Skill_2 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force080_green_blue#Ball1", 1f),
				EndEffect = new EffectConfig("F_burstup007_blue", 0.5f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.6f),
			};

			var throws = new List<Task>();
			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(500));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1000, caster), 6, 120, 40))
					throws.Add(this.Throw(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), config));
			}
			await Task.WhenAll(throws);
		}

		private async Task Throw(ICombatEntity caster, Skill skill, Position position, MissileConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 4000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Colimencia_Skill_3)]
	public class Mon_boss_Colimencia_Skill_3 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2600));
			var targetPos = originPos.GetRelative(farPos, distance: 35, angle: 30f);
			SkillCreatePad(caster, skill, targetPos, 0.52359879f, PadName.Colimencia_circlewave);
			targetPos = originPos.GetRelative(farPos, distance: 35, angle: -30f);
			SkillCreatePad(caster, skill, targetPos, -0.52359879f, PadName.Colimencia_circlewave);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			targetPos = originPos.GetRelative(farPos, distance: 35, angle: 30f);
			SkillCreatePad(caster, skill, targetPos, 0.52359879f, PadName.Colimencia_circlewave);
			targetPos = originPos.GetRelative(farPos, distance: 35, angle: -30f);
			SkillCreatePad(caster, skill, targetPos, -0.52359879f, PadName.Colimencia_circlewave);

			// TODO: Move this to pad logic
			// SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Colimencia_Skill_4)]
	public class Mon_boss_Colimencia_Skill_4 : ITargetSkillHandler
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

			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			var hits = new List<SkillHitInfo>();
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force080_green_blue#Ball1", 1.5f),
				EndEffect = new EffectConfig("F_burstup007_blue", 0.6f),
				Range = 25f,
				FlyTime = 1.5f,
				DelayTime = 0.1f,
				Gravity = 800f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.6f),
			};

			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 15, -1, hits);
		}
	}
}
