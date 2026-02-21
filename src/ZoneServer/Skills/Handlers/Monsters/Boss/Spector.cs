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
	[SkillHandler(SkillId.Mon_boss_Spector_F_Skill_1)]
	public class Mon_boss_Spector_F_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(600);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 400;
			var damageDelay = 600;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_F_Skill_2)]
	public class Mon_boss_Spector_F_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			var noKnockConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var knockConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50, angle: 120f, rand: 70);
			await EffectAndHit(skill, caster, position, noKnockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -120f, rand: 70);
			await EffectAndHit(skill, caster, position, noKnockConfig);
			position = originPos.GetRelative(farPos, distance: 50, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -180f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: 60f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -60f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_F_Skill_3)]
	public class Mon_boss_Spector_F_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			var noKnockConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 5000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var knockConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 5000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50, angle: 120f, rand: 70);
			await EffectAndHit(skill, caster, position, noKnockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -120f, rand: 70);
			await EffectAndHit(skill, caster, position, noKnockConfig);
			position = originPos.GetRelative(farPos, distance: 50, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -180f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: 60f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
			position = originPos.GetRelative(farPos, distance: 50, angle: -60f, rand: 70);
			await EffectAndHit(skill, caster, position, knockConfig);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_F_Skill_4)]
	public class Mon_boss_Spector_F_Skill_4 : ITargetSkillHandler
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
			var targets = SkillSelectEnemiesInFan(caster, targetPos, 50f, 130f, 1);
			if (targets == null || targets.Count == 0)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force0082", 4f),
				EndEffect = new EffectConfig("I_force0082", 3f),
				Range = 30f,
				FlyTime = 1.2f,
				DelayTime = 0f,
				Gravity = 200f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileThrow(skill, caster, position, missileConfig);
			for (var i = 0; i < 4; i++)
			{
				position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await skill.Wait(TimeSpan.FromMilliseconds(300));
				await MissileThrow(skill, caster, position, missileConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_m_Skill_1)]
	public class Mon_boss_Spector_m_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1400));

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.8", 1.3f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_rize006", 0.2f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var hits = new List<SkillHitInfo>();
			for (var i = 0; i < 6; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, hitConfig, hits);
				if (i < 5)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 5000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_m_Skill_2)]
	public class Mon_boss_Spector_m_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 0);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 0.7f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.05f,
				ArrowLifeTime = 0.05f,
				PositionDelay = 10f,
				HitEffect = new EffectConfig("F_smoke047", 0.8f),
				Range = 12f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 12f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 5000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_m_Skill_3)]
	public class Mon_boss_Spector_m_Skill_3 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 160f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force035_red#B_Prop1", 1.5f),
				EndEffect = new EffectConfig("F_smoke029_violet", 2.2f),
				Range = 40f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 3f),
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Spector_m_Skill_4)]
	public class Mon_boss_Spector_m_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var spawnPos = originPos.GetRelative(farPos, distance: 60);
			MonsterSkillCreateMob(skill, caster, "Shredded_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
