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
	/// Handler for boss_honeypin Skill 1.
	/// AoE attack with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_honeypin_Skill_1)]
	public class Mon_boss_honeypin_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 48.987946f, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1300,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 4000f, 1, 30, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 12000f, 1, 100, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_honeypin Skill 2.
	/// Multiple blood missile throws with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_honeypin_Skill_2)]
	public class Mon_boss_honeypin_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_blood001_green#Dummy_bufficon", 1.5f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.4f),
				Range = 12f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var waveDelays = new[] { 2200, 2800 };

			for (var wave = 0; wave < 3; wave++)
			{
				for (var i = 0; i < 9; i++)
				{
					var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 60, height: 1);
					_ = MissileThrow(skill, caster, position, config);
				}

				if (wave < waveDelays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(waveDelays[wave]));
			}
		}
	}

	/// <summary>
	/// Handler for boss_honeypin Skill 3.
	/// Targeted AoE attacks with knockback.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_honeypin_Skill_3)]
	public class Mon_boss_honeypin_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1050));

			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out006", 0.9f),
				Range = 90f,
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

			var distances = new double[] { 40, 0, 40 };
			var hits = new List<SkillHitInfo>();

			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: distances[i]);
				TeleportToPosition(caster, position, "F_sys_target_boss##0.3", 5f);
				await skill.Wait(TimeSpan.FromMilliseconds(1350));
				hits.Clear();
				await EffectAndHit(skill, caster, position, config, hits);
				SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 100, 10, 0, 1, 5, hits);

				var validPos = caster.Map.Ground.GetLastValidPosition(caster.Position, position);
				caster.Position = validPos;
				Send.ZC_SET_POS(caster, validPos);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(2650));
			}
		}
	}

	/// <summary>
	/// Handler for boss_honeypin Skill 4.
	/// Missile pad throws creating slow zones.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_honeypin_Skill_4)]
	public class Mon_boss_honeypin_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2800));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_white#Dummy_atk2_effect", 1f),
				EndEffect = new EffectConfig("I_explosion014_white", 1f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var delays = new[] { 1500, 1300 };

			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, 30f, rand: 40);
				await MissilePadThrow(skill, caster, position, config, 0f, "Boss_slow");

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}
}
