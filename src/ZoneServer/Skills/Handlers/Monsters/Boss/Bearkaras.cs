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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_bearkaras_Attack1)]
	public class Mon_boss_bearkaras_Attack1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, farPos, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 6000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bearkaras_Attack2)]
	public class Mon_boss_bearkaras_Attack2 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, height: 1);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 2000,
				Effect = EffectConfig.None,
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_burstup020_smoke", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup045", 0.8f),
				Range = 30f,
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

			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, config, hits);
			}
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.5f),
				PositionDelay = 1300,
				Effect = new EffectConfig("None", 0.8f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, config, hits);
			}
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("None", 0.8f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, config, hits);
			}
			position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.5f),
				PositionDelay = 1200,
				Effect = new EffectConfig("None", 0.8f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, config, hits);
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bearkaras_Skill)]
	public class Mon_boss_bearkaras_Skill : ITargetSkillHandler
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
			var farPos = originPos;
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, farPos, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 7f),
				PositionDelay = 2700,
				Effect = EffectConfig.None,
				Range = 90f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_bearkaras_Skill_1)]
	public class Mon_boss_bearkaras_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2500;
			var damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 30f,
				ArrowSpacingTime = 0.05f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_spin018", 0.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 4; i++)
			{
				var startingPosition = originPos.GetRelative(farPos);
				var endingPosition = originPos.GetRelative(farPos, distance: 180f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);

				if (i < 3)
					await skill.Wait(TimeSpan.FromMilliseconds(250));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_bearkaras_Skill_2)]
	public class Mon_boss_bearkaras_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 70);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 100f,
				HitEffect = new EffectConfig("F_spin018", 0.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_bearkaras_Skill_3)]
	public class Mon_boss_bearkaras_Skill_3 : ITargetSkillHandler
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
			//await skill.Wait(TimeSpan.FromMilliseconds(4500));
			//var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			//await skill.Wait(TimeSpan.FromMilliseconds(2000));
			//position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			//await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 70);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 4500;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			//await skill.Wait(TimeSpan.FromMilliseconds(1000));
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 70);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 2000;
			damageDelay = 2000;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}
}
