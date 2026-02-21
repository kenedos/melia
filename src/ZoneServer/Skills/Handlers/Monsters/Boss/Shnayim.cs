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
	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_1)]
	public class Mon_boss_Shnayim_Skill_1 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1600;
			var damageDelay = 1800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_2)]
	public class Mon_boss_Shnayim_Skill_2 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 10, angle: 1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup022_smoke", 0.6f),
				Range = 40f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 10, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_3)]
	public class Mon_boss_Shnayim_Skill_3 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 32.419632f);
			var endingPosition = originPos.GetRelative(farPos, distance: 300f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster##0.2", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = EffectConfig.None,
				Range = 15f,
				KnockdownPower = 100f,
				Delay = 1000f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.04f,
				HitCount = 1,
				HitDuration = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_4)]
	public class Mon_boss_Shnayim_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var spawnPos = originPos.GetRelative(farPos, distance: 66, angle: -1f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 80, angle: 123f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 76, angle: -148f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 94, angle: 56f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			spawnPos = originPos.GetRelative(farPos, distance: 88, angle: -69f);
			MonsterSkillCreateMob(skill, caster, "shtayim_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_boss_Shnayim_Skill_5)]
	public class Mon_boss_Shnayim_Skill_5 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 59, angle: 1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_ground058_smoke", 1f),
				Range = 40f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize001", 0.5f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
				await EffectAndHit(skill, caster, position, effectHitConfig, hits);

				if (i < 4)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos, distance: 59, angle: 1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 4f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_ground058_smoke", 1.6f),
				Range = 60f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize001##0.3", 0.55f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			position = originPos.GetRelative(farPos, distance: 59, angle: 1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 6f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_ground058_smoke", 2.5f),
				Range = 80f,
				KnockdownPower = 180f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var effectHitConfig3 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 3.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize001##0.3", 0.8f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 140);
				await EffectAndHit(skill, caster, position, effectHitConfig3, hits);

				if (i < 4)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 6000f, 1, 20, -1, hits);
		}
	}
}
