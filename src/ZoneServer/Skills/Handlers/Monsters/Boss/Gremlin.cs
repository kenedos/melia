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
	[SkillHandler(SkillId.Mon_boss_Gremlin_Skill_1)]
	public class Mon_boss_Gremlin_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1600;
			var damageDelay = 1800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 15000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gremlin_Skill_2)]
	public class Mon_boss_Gremlin_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 55, width: 45, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2400;
			var damageDelay = 2400;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 55, width: 45, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 400;
			damageDelay = 400;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 8000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 15000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gremlin_Skill_3)]
	public class Mon_boss_Gremlin_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var position = originPos.GetRelative(farPos, distance: 20);
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1.5f),
				Range = 50f,
				KnockdownPower = 150f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 260, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Gremlin_Skill_4)]
	public class Mon_boss_Gremlin_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var spawnPos = originPos.GetRelative(farPos, rand: 80, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Dumaro_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 80, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Dumaro_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 80, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Dumaro_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			caster.StartBuff(BuffId.Rage_Rockto_atk, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}
}
