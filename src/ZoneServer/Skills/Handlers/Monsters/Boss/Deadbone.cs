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
	/// Handler for boss_deadbone Skill 1.
	/// Circle AoE attack with debrave debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_deadbone_Skill_1)]
	public class Mon_boss_deadbone_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 300;
			var damageDelay = 500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_deadbone Skill 2.
	/// Two delayed hits at offset positions.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_deadbone_Skill_2)]
	public class Mon_boss_deadbone_Skill_2 : ITargetSkillHandler
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
			var hits1 = new List<SkillHitInfo>();
			var splashParam1 = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 120f);
			var splashArea1 = skill.GetSplashArea(SplashType.Fan, splashParam1);
			await SkillAttack(caster, skill, splashArea1, hitDelay: 1500, damageDelay: 500, hits1);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits1);

			var hits2 = new List<SkillHitInfo>();
			var splashParam2 = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 120f);
			var splashArea2 = skill.GetSplashArea(SplashType.Fan, splashParam2);
			await SkillAttack(caster, skill, splashArea2, hitDelay: 400, damageDelay: 500, hits2);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits2);
		}
	}

	/// <summary>
	/// Handler for boss_deadbone Skill 3.
	/// Arrow projectile attack with debrave debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_deadbone_Skill_3)]
	public class Mon_boss_deadbone_Skill_3 : ITargetSkillHandler
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
			var farPos = originPos.GetRelative(target.Position, distance: 120f);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 120f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 900f,
				HitEffect = new EffectConfig("F_spin016_blue", 1.2f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 18f,
				HitTimeSpacing = 0.17f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 4000f, 1, 10, -1, hits);

			var position = caster.Map.Ground.GetLastValidPosition(originPos, farPos);
			caster.Position = position;
			Send.ZC_SET_POS(caster, position);
		}
	}

	/// <summary>
	/// Handler for M_boss_deadbone Skill 1.
	/// Circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_M_boss_deadbone_Skill_1)]
	public class Mon_M_boss_deadbone_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 120f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 300;
			var damageDelay = 500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits);
		}
	}

	/// <summary>
	/// Handler for M_boss_deadbone Skill 2.
	/// Two delayed hits at offset positions.
	/// </summary>
	[SkillHandler(SkillId.Mon_M_boss_deadbone_Skill_2)]
	public class Mon_M_boss_deadbone_Skill_2 : ITargetSkillHandler
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
			var hits1 = new List<SkillHitInfo>();
			var splashParam1 = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 120f);
			var splashArea1 = skill.GetSplashArea(SplashType.Fan, splashParam1);
			await SkillAttack(caster, skill, splashArea1, hitDelay: 1500, damageDelay: 500, hits1);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits1);

			var hits2 = new List<SkillHitInfo>();
			var splashParam2 = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 120f);
			var splashArea2 = skill.GetSplashArea(SplashType.Fan, splashParam2);
			await SkillAttack(caster, skill, splashArea2, hitDelay: 400, damageDelay: 500, hits2);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 12000f, 1, 50, -1, hits2);
		}
	}

	/// <summary>
	/// Handler for M_boss_deadbone Skill 3.
	/// Arrow projectile attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_M_boss_deadbone_Skill_3)]
	public class Mon_M_boss_deadbone_Skill_3 : ITargetSkillHandler
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
			var farPos = originPos.GetRelative(target.Position, distance: 120f);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 120f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 900f,
				HitEffect = new EffectConfig("F_spin016_blue", 1.2f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 18f,
				HitTimeSpacing = 0.17f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 4000f, 1, 10, -1, hits);

			var position = caster.Map.Ground.GetLastValidPosition(originPos, farPos);
			caster.Position = position;
			Send.ZC_SET_POS(caster, position);
		}
	}

	/// <summary>
	/// Handler for M_boss_deadbone Skill 4.
	/// Three delayed hits with stun debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_M_boss_deadbone_Skill_4)]
	public class Mon_M_boss_deadbone_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 32.278568f, angle: -14f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 36.069061f, angle: 25f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 30f,
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
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = originPos.GetRelative(farPos, distance: 40f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 35f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}
}
