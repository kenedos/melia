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
	[SkillHandler(SkillId.Mon_boss_Zawra_Skill_1)]
	public class Mon_boss_Zawra_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 90f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Zawra_Skill_2)]
	public class Mon_boss_Zawra_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 30);
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_red", 1.3f),
				Range = 75f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 8000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Zawra_Skill_3)]
	public class Mon_boss_Zawra_Skill_3 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_spin038_orange", 2.5f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 80f,
				HitTimeSpacing = 0.2f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_smoke117", 1f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 80f,
				HitTimeSpacing = 0.2f,
				HitCount = 0,
				HitDuration = 1000f,
			});
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_light013_spark_orange2", 3f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 80f,
				HitTimeSpacing = 0.2f,
				HitCount = 0,
				HitDuration = 1000f,
			});


			caster.Position = farPos;
			Send.ZC_SET_POS(caster, farPos);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Zawra_Skill_4)]
	public class Mon_boss_Zawra_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var spawnPos = originPos.GetRelative(farPos, distance: 79, angle: -68f);
			MonsterSkillCreateMobPC(skill, caster, "PagNurse_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 60, angle: 49f);
			MonsterSkillCreateMobPC(skill, caster, "PagNurse_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 71, angle: -154f);
			MonsterSkillCreateMobPC(skill, caster, "PagEmitter_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 59, angle: 146f);
			MonsterSkillCreateMobPC(skill, caster, "PagEmitter_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 78, angle: -1f);
			MonsterSkillCreateMobPC(skill, caster, "PagEmitter_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			caster.StartBuff(BuffId.Mon_Rage, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Zawra_Skill_5)]
	public class Mon_boss_Zawra_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var position = originPos.GetRelative(farPos, distance: 200, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup022_smoke_fire", 2f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 200);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup001_dark", 3f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});
			position = originPos.GetRelative(farPos, distance: 200);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_cleric_handknife_shot_stone_mash1", 1f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 0,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});
			position = originPos.GetRelative(farPos, distance: 200);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_stone015_mash", 1f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 0,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});

			caster.Position = farPos;
			Send.ZC_SET_POS(caster, farPos);
		}
	}
}
