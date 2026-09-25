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
	[SkillHandler(SkillId.Mon_boss_Mummyghast_Skill_1)]
	public class Mon_boss_Mummyghast_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1900;
			var aniTime = 2100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mummyghast_Skill_2)]
	public class Mon_boss_Mummyghast_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var smokeHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var knockbackHitConfig = smokeHitConfig;
			knockbackHitConfig.GroundEffect = EffectConfig.None;
			knockbackHitConfig.KnockdownPower = 150f;
			knockbackHitConfig.KnockType = 4;

			var smokeKnockdownConfig = smokeHitConfig;
			smokeKnockdownConfig.KnockdownPower = 100f;

			var baseDir = originPos.GetDirection(farPos);
			var tasks = new List<Task>();

			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 700, caster), 2, 120, 45))
				tasks.Add(this.Blast(caster, skill, position, smokeHitConfig));

			await skill.Wait(TimeSpan.FromMilliseconds(200));
			tasks.Add(this.Blast(caster, skill, originPos.GetRelative(baseDir.AddDegreeAngle(-38f), 68f), knockbackHitConfig));

			await skill.Wait(TimeSpan.FromMilliseconds(200));
			tasks.Add(this.Blast(caster, skill, originPos.GetRelative(baseDir.AddDegreeAngle(34f), 68f), knockbackHitConfig));
			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 700, caster), 5, 140, 45))
				tasks.Add(this.Blast(caster, skill, position, smokeHitConfig));

			await skill.Wait(TimeSpan.FromMilliseconds(300));
			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 700, caster), 4, 140, 45))
				tasks.Add(this.Blast(caster, skill, position, smokeKnockdownConfig));

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			tasks.Add(this.Blast(caster, skill, originPos.GetRelative(baseDir.AddDegreeAngle(-34f), 74f), knockbackHitConfig));

			await Task.WhenAll(tasks);
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 5000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mummyghast_Skill_3)]
	public class Mon_boss_Mummyghast_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 150, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var aniTime = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 9f),
				PositionDelay = 3500,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 6000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mummyghast_Skill_4)]
	public class Mon_boss_Mummyghast_Skill_4 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var hits = new List<SkillHitInfo>();

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup003", 0.7f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var delays = new[] { 0, 100, 400, 350, 150, 300, 100, 2100, 500, 500, 500, 500, 500, 500, 400, 400, 400, 400, 400 };
			for (var i = 0; i < 20; i++)
			{
				if (i > 0 && delays[i - 1] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i - 1]));

				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				var position = GetLeadPositionScatter(target, 1500, 50, caster);
				_ = this.Blast(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), hitConfig);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1500));
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 6000f, 1, 5, -1, hits);
		}
	}
}
