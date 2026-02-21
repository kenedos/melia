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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_spector_gh Attack.
	/// Single hit attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_spector_gh_Attack)]
	public class Mon_boss_spector_gh_Attack : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 30.858559f, angle: -1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 600,
				Effect = EffectConfig.None,
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	/// <summary>
	/// Handler for boss_spector_gh Skill 1.
	/// Circle AoE with silence debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_spector_gh_Skill_1)]
	public class Mon_boss_spector_gh_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_spector_gh Skill 2.
	/// Self-buff speed boost.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_spector_gh_Skill_2)]
	public class Mon_boss_spector_gh_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			caster.StartBuff(BuffId.Rage_Rockto_spd, 1f, 0f, TimeSpan.FromMilliseconds(18000f), caster);
		}
	}

	/// <summary>
	/// Handler for boss_spector_gh Skill 3.
	/// Two missile buck attacks with silence debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_spector_gh_Skill_3)]
	public class Mon_boss_spector_gh_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			position = originPos.GetRelative(farPos);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_spector_gh Skill 4.
	/// Circle AoE with explosion effect that pulls targets towards the caster.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_spector_gh_Skill_4)]
	public class Mon_boss_spector_gh_Skill_4 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 150);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsCaster, 200, 10, 0, 0, 0, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion005_violet", 0.8f),
				Range = 300f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsCaster, 200, 10, 0, 0, 0, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}
}
