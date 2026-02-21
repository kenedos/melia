using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Cataphract
{
	/// <summary>
	/// Handler for the Cataphract skill Trot.
	/// Toggle skill that increases movement speed while mounted.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_Trot)]
	public class Cataphract_TrotOverride : ISelfSkillHandler
	{
		/// <summary>
		/// Handles the Trot skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character || !character.IsRiding)
			{
				caster.ServerMessage(Localization.Get("You must be mounted to use this skill."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			caster.StartBuff(BuffId.Trot_Buff, skill.Level, 0f, TimeSpan.FromMinutes(5), caster);
		}
	}
}
