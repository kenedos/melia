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
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Appraiser skill High Scale Magnifying Glass, which
	/// raises the Appraiser's accuracy and block penetration.
	/// </summary>
	/// <remarks>
	/// With High Scale Magnifying Glass: Focus Trim it instead raises the
	/// final damage of the Appraiser's party for 30 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Appraiser_HighMagnifyingGlass)]
	public class Appraiser_HighMagnifyingGlassOverride : IGroundSkillHandler
	{
		private const float PartyRange = 150f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(5);
		private static readonly TimeSpan FocusTrimDuration = TimeSpan.FromSeconds(30);

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

			if (!caster.IsAbilityActive(AbilityId.Appraiser16))
			{
				caster.StartBuff(BuffId.HighMagnifyingGlass_Buff, skill.Level, 0, BuffDuration, caster, skill.Id);
				return;
			}

			var allies = new List<ICombatEntity> { caster };
			if (caster is Character character)
				allies.AddRange(caster.Map.GetPartyMembersInRange(character, PartyRange).Where(m => m != caster));

			foreach (var ally in allies)
				ally.StartBuff(BuffId.HighMagnifyingGlass_Abil_Buff, skill.Level, 0, FocusTrimDuration, caster, skill.Id);
		}
	}
}
