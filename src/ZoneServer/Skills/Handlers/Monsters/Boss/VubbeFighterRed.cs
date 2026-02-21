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
	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_red_Skill_1)]
	public class Mon_boss_Goblin_Warrior_red_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 0, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 100, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_red_Skill_2)]
	public class Mon_boss_Goblin_Warrior_red_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2600;
			var damageDelay = 2600;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_red_Skill_3)]
	public class Mon_boss_Goblin_Warrior_red_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(4100));
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 200;
			var damageDelay = 200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 100, -1, hits);
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
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

			var position = originPos.GetRelative(farPos, distance: 80f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##1.5", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup008_smoke", 1.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 0;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 100, -1, hits);
			position = originPos.GetRelative(farPos, distance: 80f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##1.5", 1f),
				PositionDelay = 900,
				Effect = new EffectConfig("F_burstup008_smoke", 1.5f),
				Range = 30f,
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

			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 50);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 0;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##1.5", 1f),
				PositionDelay = 90,
				Effect = new EffectConfig("F_burstup008_smoke", 1.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			caster.StartBuff(BuffId.Stun, 1f, 0f, TimeSpan.FromMilliseconds(5000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_red_Skill_4)]
	public class Mon_boss_Goblin_Warrior_red_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var spawnPos = originPos.GetRelative(farPos, distance: 67.255928f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 89.260567f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50.318855f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Archer_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 45.242489f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Archer_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
