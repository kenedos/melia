using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_mushcaria_Skill_1)]
	public class Mon_boss_mushcaria_Skill_1 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke018_spread_in#Dummy_head_effect", 0.5f),
				EndEffect = new EffectConfig("I_smoke017_spread_out", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 3f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.5", 0.5f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var positions = GetScatteredPositions(GetLeadPosition(target, 1000, caster), 5, 80, 45);
			var waits = new[] { 0, 0, 200, 0, 200 };
			for (var i = 0; i < positions.Count; i++)
			{
				if (waits[i] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(waits[i]));
				_ = MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(positions[i], 200f), config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushcaria_Skill_3)]
	public class Mon_boss_mushcaria_Skill_3 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke037_yellow#Dummy_head_effect", 1.5f),
				EndEffect = new EffectConfig("F_explosion044", 0.5f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 3f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster##0.4", 0.5f),
			};

			var waits = new[] { 1400, 1000, 0, 1200, 0, 800, 1100, 0, 900, 0, 900 };
			for (var i = 0; i < waits.Length; i++)
			{
				if (waits[i] > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(waits[i]));
				if (!caster.Position.InRange2D(target.Position, 300))
					return;

				var scatter = waits[i] > 0 ? 0 : 60;
				var position = GetLeadPositionScatter(target, 1000, scatter, caster);
				_ = MissileThrow(skill, caster, caster.Position.GetNearestPositionWithinDistance(position, 200f), config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushcaria_Skill_5)]
	public class Mon_boss_mushcaria_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var spawnPos = originPos.GetRelative(caster.Direction.AddDegreeAngle(-133f), distance: 82.192505f);
			MonsterSkillCreateMobPC(skill, caster, "Mushcarfung_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(caster.Direction.AddDegreeAngle(47f), distance: 83.836403f);
			MonsterSkillCreateMobPC(skill, caster, "Mushcarfung_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(caster.Direction.AddDegreeAngle(-48f), distance: 82.457108f);
			MonsterSkillCreateMobPC(skill, caster, "Mushcarfung_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(caster.Direction.AddDegreeAngle(141f), distance: 100.10666f);
			MonsterSkillCreateMobPC(skill, caster, "Mushcarfung_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}
}
