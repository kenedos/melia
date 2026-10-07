using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Wizards.Sage
{
	/// <summary>
	/// Handler for the Sage skill Missile Hole, which shields the Sage and
	/// the party around them from missile and magic bullet attacks.
	/// </summary>
	/// <remarks>
	/// Missile Hole: Escape sets the Sage's movement speed to 60 for 3
	/// seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Sage_MissileHole)]
	public class Sage_MissileHoleOverride : IGroundSkillHandler
	{
		private const float Range = 150f;
		private static readonly TimeSpan EscapeDuration = TimeSpan.FromSeconds(3);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, Range))
				ally.StartBuff(BuffId.MissileHole_Buff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);

			if (caster.IsAbilityActive(AbilityId.Sage13))
				caster.StartBuff(BuffId.MissileHole_MSPD_Buff, skill.Level, 0, EscapeDuration, caster, skill.Id);
		}
	}
}
