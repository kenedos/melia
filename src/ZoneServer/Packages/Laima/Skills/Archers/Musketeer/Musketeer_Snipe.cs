using System;
using System.Collections.Generic;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Snipe.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_Snipe)]
	public class Musketeer_SnipeOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int InitialExposedStacks = 3;
		private static readonly TimeSpan HitAniTime = TimeSpan.FromMilliseconds(150);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("sys_snipe_target", "sys_snipe_target");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out _))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var hits = new List<SkillHitInfo>();
			var forceId = ForceId.GetNew();

			if (target != null)
			{
				var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 22);
				var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var currentTarget in targets.LimitBySDR(caster, skill))
				{
					var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);
					currentTarget.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, HitAniTime, TimeSpan.Zero);
					skillHit.ForceId = forceId;
					hits.Add(skillHit);
				}
			}

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, hits);

			this.AddExposedStack(caster, skill);
		}

		/// <summary>
		/// Adds a Sniper Exposed stack, starting the buff at its initial
		/// count when the caster is not exposed yet.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		private void AddExposedStack(ICombatEntity caster, Skill skill)
		{
			if (caster.IsBuffActive(BuffId.Musketeer_Snipe_UseStack_Buff))
			{
				caster.StartBuff(BuffId.Musketeer_Snipe_UseStack_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
				return;
			}

			var buff = caster.StartBuff(BuffId.Musketeer_Snipe_UseStack_Buff, skill.Level, 0, TimeSpan.Zero, caster, skill.Id);
			if (buff == null)
				return;

			buff.OverbuffCounter = InitialExposedStacks;
			buff.NotifyUpdate();
		}
	}
}
