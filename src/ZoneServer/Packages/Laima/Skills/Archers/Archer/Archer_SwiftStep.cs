using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handler for the Archer skill Concentration.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_SwiftStep)]
	public class Archer_SwiftStepOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, applying buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (caster is Character character)
			{
				if (character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var duration = TimeSpan.FromSeconds(300);
			caster.StartBuff(BuffId.SwiftStep_Buff, skill.Level, 0, duration, caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}
	}
}
