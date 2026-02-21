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
	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_1)]
	public class Mon_boss_stone_whale_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.8", 2.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_rize008##1", 0.5f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 5000f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_2)]
	public class Mon_boss_stone_whale_Skill_2 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 10);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1.3f),
				PositionDelay = 2500,
				Effect = EffectConfig.None,
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_3)]
	public class Mon_boss_stone_whale_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3600);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20, angle: 50f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 3400;
			var damageDelay = 300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20, angle: 50f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 300;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20, angle: 50f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 300;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 3000f, 1, 100, -1, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20, angle: 50f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 300;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_petrify, 1, 0f, 3000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_4)]
	public class Mon_boss_stone_whale_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss", 7f),
				PositionDelay = 3300,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 7f),
				PositionDelay = 100,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 7; i++)
			{
				position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_5)]
	public class Mon_boss_stone_whale_Skill_5 : ITargetSkillHandler
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
			var targetPos = GetRelativePosition(PosType.TargetDistance, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			var position = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.8", 2.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_rize008##1", 0.5f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 500f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_stone_whale_Skill_6)]
	public class Mon_boss_stone_whale_Skill_6 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, distance: 89.927567f);
			MonsterSkillCreateMob(skill, caster, "StonOrca_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 78.795387f);
			MonsterSkillCreateMob(skill, caster, "StonOrca_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 78.033653f);
			MonsterSkillCreateMob(skill, caster, "StonOrca_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 41.998669f);
			MonsterSkillCreateMob(skill, caster, "StonOrca_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 68.544388f);
			MonsterSkillCreateMob(skill, caster, "StonOrca_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
