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
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_NetherBovine_Skill_1)]
	public class Mon_boss_NetherBovine_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 67f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.8", 4f),
				PositionDelay = 1500,
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
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 3000f, 1, 15, -1, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits, 20);
		}
	}

	[SkillHandler(SkillId.Mon_boss_NetherBovine_Skill_2)]
	public class Mon_boss_NetherBovine_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(3400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 3200;
			var aniTime = 3400;
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 17f, angle: -22f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.8", 6.5f),
				PositionDelay = 2500,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 1000f,
				HitCount = 4,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_NetherBovine_Skill_3)]
	public class Mon_boss_NetherBovine_Skill_3 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var positionDelays = new[] { 1500, 2000 };
			for (var i = 0; i < 2; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 20f);
				await EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("None", 7f),
					PositionDelay = positionDelays[i],
					Effect = new EffectConfig("F_spread_out008_smoke", 2f),
					Range = 80f,
					KnockdownPower = 0f,
					Delay = 0f,
					HitCount = 1,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 0f,
					InnerRange = 0,
				}, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_confuse, 1, 0f, 9000f, 1, 15, -1, hits);
				hits.Clear();
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_NetherBovine_Skill_4)]
	public class Mon_boss_NetherBovine_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2000);
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
			var slams = this.Slams(caster, skill, originPos, farPos);

			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_burstup020_smoke", 0.6f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup008_smoke", 1.5f),
				Range = 30f,
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
			var effectHitConfig2 = effectHitConfig;
			effectHitConfig2.KnockdownPower = 100f;

			var schedule = new (int Time, PosType PosType, int Rand, bool Knock)[]
			{
				(1500, PosType.TargetDistance, 120, false),
				(1900, PosType.TargetRandomDistance, 230, false),
				(2700, PosType.TargetRandomDistance, 230, false),
				(3100, PosType.TargetHeight, 150, true),
				(6000, PosType.TargetRandomDistance, 140, false),
				(7000, PosType.TargetRandomDistance, 160, false),
				(7500, PosType.TargetHeight, 160, true),
				(8600, PosType.TargetRandomDistance, 160, false),
				(9000, PosType.TargetRandomDistance, 160, false),
				(10000, PosType.TargetHeight, 160, false),
				(10500, PosType.TargetHeight, 180, true),
			};

			var blasts = new List<Task> { slams };
			var elapsed = 0;
			foreach (var entry in schedule)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(entry.Time - elapsed));
				elapsed = entry.Time;

				var position = GetRelativePosition(entry.PosType, caster, target, rand: entry.Rand);
				position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
				blasts.Add(this.Blast(caster, skill, position, entry.Knock ? effectHitConfig2 : effectHitConfig));
			}

			await Task.WhenAll(blasts);
		}

		private async Task Slams(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			await SkillAttack(caster, skill, splashArea, 1800, 2000, new List<SkillHitInfo>());
			await SkillAttack(caster, skill, splashArea, 3800, 4000, new List<SkillHitInfo>());
			await SkillAttack(caster, skill, splashArea, 3300, 3500, new List<SkillHitInfo>());
		}

		private async Task Blast(ICombatEntity caster, Skill skill, Position position, EffectHitConfig config)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 15, -1, hits);
		}
	}
}
