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
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_Skill_1)]
	public class Mon_boss_Goblin_Warrior_Skill_1 : ITargetSkillHandler
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
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 73f, angle: 13f), 50f);
			var hitDelay = 2200;
			var aniTime = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 15, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_Skill_2)]
	public class Mon_boss_Goblin_Warrior_Skill_2 : ITargetSkillHandler
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
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 80f, angle: 15f), 50f);
			var hitDelay = 2900;
			var aniTime = 3200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_Skill_3)]
	public class Mon_boss_Goblin_Warrior_Skill_3 : ITargetSkillHandler
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
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 80f, angle: 15f), 30f);
			var burstConfig = new EffectHitConfig
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
			};

			var schedule = new (int Time, int PositionDelay)[]
			{
				(4100, -1), (5000, 1000), (5300, 1000), (5600, 100),
				(7100, -1), (8000, 900), (8300, 900), (8600, 900),
				(10100, -1), (11000, 900), (11300, 90), (11600, 90),
			};

			var attacks = new List<Task>();
			var elapsed = 0;
			foreach (var (time, positionDelay) in schedule)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(time - elapsed));
				elapsed = time;

				if (positionDelay < 0)
				{
					attacks.Add(this.Slam(caster, skill, splashArea));
					continue;
				}

				var config = burstConfig;
				config.PositionDelay = positionDelay;
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
				attacks.Add(EffectAndHit(skill, caster, position, config));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(13000 - elapsed));
			caster.StartBuff(BuffId.Stun, 1f, 0f, TimeSpan.FromMilliseconds(5000f), caster);
			await Task.WhenAll(attacks);
		}

		private async Task Slam(ICombatEntity caster, Skill skill, CircleF splashArea)
		{
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, 1000, 1000, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Goblin_Warrior_Skill_4)]
	public class Mon_boss_Goblin_Warrior_Skill_4 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, distance: 67f, angle: 69f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 89f, angle: -48f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50.318855f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 45f, angle: -90f);
			MonsterSkillCreateMob(skill, caster, "Goblin_Spear_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
