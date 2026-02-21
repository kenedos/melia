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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Clymen_Skill_1)]
	public class Mon_boss_Clymen_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 30, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1100,
				Effect = EffectConfig.None,
				Range = 35f,
				KnockdownPower = 200f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 30000f, 1, 100, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Clymen_Skill_2)]
	public class Mon_boss_Clymen_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1450));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Bip001 R Hand", 1f),
				EndEffect = new EffectConfig("F_explosion009_green", 1f),
				Range = 15f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var hits = new List<SkillHitInfo>();
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 90, rand: 80, height: 1);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 90, rand: 70, height: 2);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, missileConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 90, rand: 70, height: 2);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, missileConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 90, rand: 70, height: 2);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, missileConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 90, rand: 70, height: 2);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, missileConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Clymen_Skill_3)]
	public class Mon_boss_Clymen_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2250));

			var hands = new[] { "L Hand", "R Hand", "R Hand", "L Hand", "L Hand", "L Hand", "R Hand", "R Hand" };
			for (var i = 0; i < hands.Length; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(target.Position, 150f);
				var hits = new List<SkillHitInfo>();
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig($"I_force011_green#Bip001 {hands[i]}", 1f),
					EndEffect = new EffectConfig("F_explosion009_green", 1f),
					Range = 10f,
					FlyTime = 0.7f,
					DelayTime = 0f,
					Gravity = 0f,
					Speed = 1f,
					HitTime = 100f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 1.3f),
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Clymen_Skill_4)]
	public class Mon_boss_Clymen_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));

			var angles = new float[] { -89f, -109f, -130f, -150f, -170f, -190f, -209f, -229f, -250f, -270f, -289f, -310f, -329f, -349f, -369f, -390f, -409f, -430f };
			var dummyNames = new[] { "Dummy001", "Dummy002", "Dummy003", "Dummy004", "Dummy005", "Dummy006", "Dummy007", "Dummy008", "Dummy009", "Dummy010", "Dummy011", "Dummy012", "Dummy013", "Dummy014", "Dummy015", "Dummy016", "Dummy017", "Dummy018" };

			for (var i = 0; i < angles.Length; i++)
			{
				var posOrigin = i == 0 ? originPos : farPos;
				var position = originPos.GetRelative(posOrigin, distance: 20, angle: angles[i], height: i == 0 ? 2 : 0);
				var hits = new List<SkillHitInfo>();
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig($"I_force011_green#{dummyNames[i]}", 1f),
					EndEffect = new EffectConfig("F_explosion009_green", 1f),
					Range = 20f,
					FlyTime = 0.3f,
					DelayTime = 0f,
					Gravity = 0f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.7f),
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 15000f, 1, 15, -1, hits);
				if (i < angles.Length - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}
}
