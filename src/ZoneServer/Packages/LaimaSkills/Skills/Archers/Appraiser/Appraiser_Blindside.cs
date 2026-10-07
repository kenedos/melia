using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Appraiser skill Expose Weakness, which marks the
	/// target so attacks against it gain a minimum critical chance.
	/// </summary>
	/// <remarks>
	/// With Expose Weakness: Dispersion the mark spreads to up to 8
	/// enemies around the target.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Appraiser_Blindside)]
	public class Appraiser_BlindsideOverride : IGroundSkillHandler
	{
		private const float DispersionRange = 50f;
		private const int DispersionTargets = 8;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (target == null || target.IsDead)
			{
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
			caster.TurnTowards(target);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			var targets = new List<ICombatEntity> { target };

			if (caster.IsAbilityActive(AbilityId.Appraiser7))
			{
				var nearby = caster.Map.GetAttackableEnemiesInPosition(caster, target.Position, DispersionRange).Where(e => e != target);
				targets.AddRange(nearby.Take(DispersionTargets - 1));
			}

			var duration = TimeSpan.FromSeconds(skill.Properties.GetFloat(PropertyName.CaptionRatio));

			foreach (var markedTarget in targets)
				markedTarget.StartBuff(BuffId.Blindside_Debuff, skill.Level, 0, duration, caster, skill.Id);
		}
	}
}
