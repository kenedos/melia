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
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var aniTime = TimeSpan.FromMilliseconds(150);

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

					var skillHit = new SkillHitInfo(caster, currentTarget, skill, skillHitResult, aniTime, TimeSpan.Zero);
					skillHit.ForceId = forceId;
					hits.Add(skillHit);
				}
			}

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, hits);

		}
	}
}
