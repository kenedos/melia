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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1900;
			var damageDelay = 2100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var hits = new List<SkillHitInfo>();

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

			var knockbackHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var smokeKnockdownConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, smokeHitConfig, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 68, angle: -38f);
			await EffectAndHit(skill, caster, position, knockbackHitConfig, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 68, angle: 34f);
			await EffectAndHit(skill, caster, position, knockbackHitConfig, hits);

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
				await EffectAndHit(skill, caster, position, smokeHitConfig, hits);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var finalDelays = new[] { 0, 0, 0, 0, 100 };
			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
				await EffectAndHit(skill, caster, position, smokeKnockdownConfig, hits);
				if (finalDelays[i] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(finalDelays[i]));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 74, angle: -34f);
			await EffectAndHit(skill, caster, position, knockbackHitConfig, hits);

			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mummyghast_Skill_3)]
	public class Mon_boss_Mummyghast_Skill_3 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 150, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
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

				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100);
				await EffectAndHit(skill, caster, position, hitConfig, hits);
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}
}
