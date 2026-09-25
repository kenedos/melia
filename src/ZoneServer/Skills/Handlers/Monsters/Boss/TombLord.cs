using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
	[SkillHandler(SkillId.Mon_boss_TombLord_Skill_1)]
	public class Mon_boss_TombLord_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 100f);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark#Bip001 R Finger2Nub", 3f),
				EndEffect = new EffectConfig("F_explosion065_green", 0.6f),
				Range = 15f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.2f),
			};

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var throws = new List<Task>();
			foreach (var position in GetScatteredPositions(GetLeadPosition(target, 500, caster), 8, 80, 35))
				throws.Add(MissileThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 200f), config));
			await Task.WhenAll(throws);
		}
	}

	[SkillHandler(SkillId.Mon_boss_TombLord_Skill_2)]
	public class Mon_boss_TombLord_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 6f),
				PositionDelay = 1500,
				Effect = new EffectConfig("none", 1f),
				Range = 70f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 40f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var spawnPos = originPos.GetRelative(farPos, distance: 123f, angle: -141f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			spawnPos = originPos.GetRelative(farPos, distance: 97f, angle: -66f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			spawnPos = originPos.GetRelative(farPos, distance: 69f, angle: 10f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			spawnPos = originPos.GetRelative(farPos, distance: 102f, angle: 112f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			spawnPos = originPos.GetRelative(farPos, distance: 67f, angle: 164f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 10000f, 1, 5, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 10000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_TombLord_Skill_3)]
	public class Mon_boss_TombLord_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1f),
				PositionDelay = 1400,
				Effect = new EffectConfig("I_tomblord_obj_atk001_mash", 0.6f),
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

			Position position;

			var baseDir = originPos.GetDirection(farPos);
			for (var i = 0; i < 6; i++)
				_ = this.Blast(caster, skill, originPos.GetRelative(baseDir.AddDegreeAngle(i * 60f), 50f), effectHitConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.3f),
				PositionDelay = 1000,
				Effect = new EffectConfig("I_tomblord_obj_atk001_mash##0.8", 0.6f),
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

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var blasts = new List<Task>();
			foreach (var blastPos in GetScatteredPositions(GetLeadPosition(target, 1000, caster), 8, 140, 45))
				blasts.Add(this.Blast(caster, skill, originPos.GetNearestPositionWithinDistance(blastPos, 250f), effectHitConfig2));
			await Task.WhenAll(blasts);
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 10000f, 1, 5, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 10000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_TombLord_Skill_4)]
	public class Mon_boss_TombLord_Skill_4 : ITargetSkillHandler
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
			var position = GetRelativePosition(PosType.Target, caster, target, distance: 150, rand: 40, height: 1);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
				PositionDelay = 3300,
				Effect = new EffectConfig("F_explosion007_green", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var spawnPos = originPos.GetRelative(farPos, distance: 102f, angle: 112f);
			MonsterSkillCreateMob(skill, caster, "TombLord_obj", spawnPos, 0f, "툼싱커", "BasicMonster_ATK", -1, 23f, "Boss_TombLord", "");
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			spawnPos = originPos.GetRelative(farPos, distance: 150f);
			MonsterSkillCreateMob(skill, caster, "monster_TombLord_obj2", spawnPos, 0f, "", "", 0, 0f, "boss_TombLord2", "");
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_TombLord_Skill_5)]
	public class Mon_boss_TombLord_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			_ = this.Slam(caster, skill, originPos, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 1500,
				Effect = EffectConfig.None,
				Range = 70f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});

			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark", 3.5f),
				EndEffect = new EffectConfig("F_explosion065_green", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 1f,
				FlyTime = 1.7f,
				Height = 600f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.2f),
			};

			if (caster.Position.InRange2D(target.Position, 300))
			{
				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 2700, caster), 8, 150, 45))
					_ = MissileFall(caster, skill, originPos.GetNearestPositionWithinDistance(position, 250f), config);
			}

			await this.Slam(caster, skill, originPos, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_explosion052_mint", 4f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 60f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
		}

		private async Task Slam(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 10000f, 1, 30, -1, hits);
		}
	}
}
