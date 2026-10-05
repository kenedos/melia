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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Fallen_Statue_Skill_1)]
	public class Mon_boss_Fallen_Statue_Skill_1 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			var spawns = new[]
			{
				(distance: 60.25832f, angle: 142f),
				(distance: 40.030876f, angle: 47f),
				(distance: 43.123322f, angle: -141f),
				(distance: 41.76033f, angle: -51f),
			};

			foreach (var (distance, angle) in spawns)
			{
				var spawnPos = originPos.GetRelative(farPos, distance: distance, angle: angle, rand: 40);
				MonsterSkillCreateMob(skill, caster, "Shardstatue_summon", spawnPos, 0f, "", "BasicMonster_ATK", -2, 300f, "None", "");
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Fallen_Statue_Skill_2)]
	public class Mon_boss_Fallen_Statue_Skill_2 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 400);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_smoke119##0.5", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("I_fallen_statue_atk001_mash", 0.35f),
				Range = 30f,
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

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 130, height: 1);
				await EffectAndHit(skill, caster, position, hitConfig, hits);
				if (i < 7)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 10000f, 1, 20, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Petrification, 1, 0f, 1000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Fallen_Statue_Skill_3)]
	public class Mon_boss_Fallen_Statue_Skill_3 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetNearestPositionWithinDistance(target.Position, 400);
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 10f),
				PositionDelay = 2000,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 100, 70, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 10000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Fallen_Statue_Skill_4)]
	public class Mon_boss_Fallen_Statue_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 400);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var hits = new List<SkillHitInfo>();

			var hitConfigSmall = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.8f),
				PositionDelay = 2000,
				Effect = new EffectConfig("I_fallen_statue_atk001_mash", 0.35f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var hitConfigLarge = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.9f),
				PositionDelay = 2000,
				Effect = new EffectConfig("I_fallen_statue_atk001_mash", 0.35f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var delays = new[] { 400, 600, 300, 600, 600, 500, 1500, 500, 300, 700, 500, 500 };

			for (var i = 0; i < 13; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
				var config = i < 4 ? hitConfigSmall : hitConfigLarge;
				await EffectAndHit(skill, caster, position, config, hits);
				if (i < 12)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 10000f, 1, 20, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Petrification, 1, 0f, 1000f, 1, 5, -1, hits);
		}
	}
}
