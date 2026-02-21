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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{

			await skill.Wait(TimeSpan.FromMilliseconds(1100));

			for (var i = 0; i < 11; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 50, angle: 100f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			// First breath
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 500;
			var damageDelay = 600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);

			// Next breaths
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 1700;
			damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 150, angle: 100f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 200;
			damageDelay = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}
}
