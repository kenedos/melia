using System;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.Scripting.AI;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Warcry
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Warcry)]
	public class Barbarian_WarcryOverride : ISelfSkillHandler
	{
		/// <summary>
		/// Handles skill, debuffing nearby enemies
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			var splashParam = skill.GetSplashParameters(caster, caster.Position, caster.Position, length: 0, width: 250, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			var maxTargets = 4 + skill.Level;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Barbarian1, out var barbarian1Level))
				maxTargets += barbarian1Level;

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea, maxTargets);

			var debuffDuration = 20;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Barbarian2, out var barbarian2Level))
				debuffDuration += barbarian2Level * 2;

			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Warcry_Debuff, skill.Level, 0, TimeSpan.FromSeconds(debuffDuration), caster);

				// Pull enemy hate/aggro
				if (target.Components.TryGet<AiComponent>(out var aiComponent))
				{
					// Reset hate and simulate a hit to build threat on the caster
					aiComponent.Script.QueueEventAlert(new HateResetAlert(caster));
					aiComponent.Script.QueueEventAlert(new HitEventAlert(target, caster, 0));
				}
			}
		}
	}
}
