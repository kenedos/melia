using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Snipe.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_Snipe)]
	public class Musketeer_SnipeOverride : IGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(150);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("sys_snipe_target", "sys_snipe_target");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.InSkillUseRange(skill, targetPos))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(targetPos);

			var aniTime = TimeSpan.FromMilliseconds(150);
			var forceId = ForceId.GetNew();
			var hits = new List<SkillHitInfo>();
			var dealtDamage = false;
			var splashArea = new Circle(targetPos, 22);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var currentTarget in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, currentTarget, skill);
				currentTarget.TakeDamage(skillHitResult.Damage, caster);

				if (skillHitResult.Damage > 0)
					dealtDamage = true;

				var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, aniTime, TimeSpan.Zero);
				skillHit.ForceId = forceId;
				hits.Add(skillHit);
			}

			if (dealtDamage)
				caster.StartBuff(BuffId.Musketeer_Snipe_UseStack_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(targetPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, forceId, hits);
		}
	}
}
