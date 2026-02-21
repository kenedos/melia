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
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_Reaverpede Skill 1.
	/// AoE attack with knockback.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Reaverpede_Skill_1)]
	public class Mon_boss_Reaverpede_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup007_smoke1", 1f),
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 150, 30, 0, 3, 5, hits);
		}
	}

	/// <summary>
	/// Handler for boss_Reaverpede Skill 2.
	/// Missile throws that spawn maggots.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Reaverpede_Skill_2)]
	public class Mon_boss_Reaverpede_Skill_2 : ITargetSkillHandler
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

			var positions = new Position[6];
			positions[0] = originPos.GetRelative(farPos, distance: 120, angle: -24f);
			positions[1] = originPos.GetRelative(farPos, distance: 90, angle: -130f);
			positions[2] = originPos.GetRelative(farPos, distance: 100, angle: 89f);
			positions[3] = originPos.GetRelative(farPos, distance: 100, angle: -89f);
			positions[4] = originPos.GetRelative(farPos, distance: 120, angle: 24f);
			positions[5] = originPos.GetRelative(farPos, distance: 90, angle: 130f);

			_ = MissileThrow(skill, caster, positions[0], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney R1 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});
			_ = MissileThrow(skill, caster, positions[1], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney R3 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});
			_ = MissileThrow(skill, caster, positions[2], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney L2 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});
			_ = MissileThrow(skill, caster, positions[3], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney R2 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});
			_ = MissileThrow(skill, caster, positions[4], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney L1 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});
			_ = MissileThrow(skill, caster, positions[5], new MissileConfig
			{
				Effect = new EffectConfig("I_maggotegg_atk_mash#B_chimney L3 02", 0.25f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.3f),
				Range = 18f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			});

			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			for (var i = 0; i < 6; i++)
				MonsterSkillCreateMob(skill, caster, "maggot", positions[i], 0f, "", "BasicMonster_ATK", 0, 40f, "None", "");
		}
	}

	/// <summary>
	/// Handler for boss_Reaverpede Skill 3.
	/// Multiple poison missile throws.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Reaverpede_Skill_3)]
	public class Mon_boss_Reaverpede_Skill_3 : ITargetSkillHandler
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

			var bones = new[] { "L1", "R2", "L3", "R1", "L2", "R3" };

			for (var i = 0; i < 5; i++)
			{
				var hits = new List<SkillHitInfo>();
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 2);
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig($"I_force045_green#B_chimney {bones[i]} 02", 1f),
					EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
					Range = 20f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 500f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 0.8f),
					GroundDelay = 0,
					EffectMoveDelay = 0,
					InnerRange = 0,
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 25, -1, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 25, -1, hits);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			{
				var hits = new List<SkillHitInfo>();
				var pos = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 2);
				await MissileThrow(skill, caster, pos, new MissileConfig
				{
					Effect = new EffectConfig($"I_force045_green#B_chimney {bones[5]} 02", 1f),
					EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
					Range = 20f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 500f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 0.8f),
					GroundDelay = 0,
					EffectMoveDelay = 0,
					InnerRange = 0,
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 25, -1, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 25, -1, hits);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(400));

			for (var i = 0; i < 6; i++)
			{
				var hits = new List<SkillHitInfo>();
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 2);
				await MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig($"I_force045_green#B_chimney {bones[i]} 02", 1f),
					EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
					Range = 20f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 500f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 0.8f),
					GroundDelay = 0,
					EffectMoveDelay = 0,
					InnerRange = 0,
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 25, -1, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 25, -1, hits);
			}
		}
	}

	/// <summary>
	/// Handler for boss_Reaverpede Skill 4.
	/// Dual AoE attacks with pull effect.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Reaverpede_Skill_4)]
	public class Mon_boss_Reaverpede_Skill_4 : ITargetSkillHandler
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
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 2400,
				Effect = new EffectConfig("F_smoke006", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 1500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50, angle: -30f);
			_ = EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 50, angle: 30f);
			_ = EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(3200));
			var hits = new List<SkillHitInfo>();
			position = originPos.GetRelative(farPos, distance: 80.33934f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsCaster, 80, 10, 0, 1, 5, hits);
		}
	}
}
