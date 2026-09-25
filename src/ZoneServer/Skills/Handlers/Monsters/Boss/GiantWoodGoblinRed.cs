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
using Melia.Zone.Skills.Helpers;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{

	/// <summary>
	/// Handler for boss_GiantWoodGoblin_red Skill 1.
	/// Multiple fire AoE attacks.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_GiantWoodGoblin_red_Skill_1)]
	public class Mon_boss_GiantWoodGoblin_red_Skill_1 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{

			await skill.Wait(TimeSpan.FromMilliseconds(1100));

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var positions = GetScatteredPositions(GetLeadPosition(target, 2200, caster), 6, 130, 50);
			for (var i = 0; i < positions.Count; i++)
			{
				var position = originPos.GetNearestPositionWithinDistance(positions[i], 250f);
				var effectName = i == 0 ? "F_burstup005_fire" : "F_burstup005_fire##0.8";
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2.5f),
					PositionDelay = 2000,
					Effect = new EffectConfig(effectName, 1.1f),
					Range = 25f,
					KnockdownPower = 0f,
					Delay = 200f,
					HitCount = 3,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				});
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	/// <summary>
	/// Handler for boss_GiantWoodGoblin_red Skill 2.
	/// Delayed targeted AoE.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_GiantWoodGoblin_red_Skill_2)]
	public class Mon_boss_GiantWoodGoblin_red_Skill_2 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashArea = new CircleF(originPos.GetRelative(farPos, distance: 81f), 45f);
			var hitDelay = 2000;
			var aniTime = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	/// <summary>
	/// Handler for boss_GiantWoodGoblin_red Skill 3.
	/// Multiple explosion AoE attacks.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_GiantWoodGoblin_red_Skill_3)]
	public class Mon_boss_GiantWoodGoblin_red_Skill_3 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion041_smoke", 1.25f),
				Range = 110f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			_ = EffectAndHit(skill, caster, originPos, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			_ = EffectAndHit(skill, caster, originPos, config);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			_ = EffectAndHit(skill, caster, originPos, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion041_smoke", 1.25f),
				Range = 110f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});

		}
	}

	/// <summary>
	/// Handler for boss_GiantWoodGoblin_red Skill 4.
	/// Large circle AoE with multiple hits.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_GiantWoodGoblin_red_Skill_4)]
	public class Mon_boss_GiantWoodGoblin_red_Skill_4 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			// First breath
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2300;
			var aniTime = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);

			// Next breaths
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			aniTime = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 0;
			aniTime = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 0;
			aniTime = 100;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 0;
			aniTime = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}
}
