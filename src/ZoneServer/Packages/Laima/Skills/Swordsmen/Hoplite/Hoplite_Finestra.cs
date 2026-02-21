using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handler override for the Hoplite skill Finestra.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hoplite_Finestra)]
	public class Hoplite_FinestraOverride : ISelfSkillHandler
	{
		/// <summary>
		/// Handles skill, applying a buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			// Check if caster is wielding a spear or two-handed spear
			if (caster is Character character)
			{
				var weapon = character.Inventory.GetItem(EquipSlot.RightHand);
				if (weapon == null || (weapon.Data.EquipType1 != EquipType.Spear && weapon.Data.EquipType1 != EquipType.THSpear))
				{
					caster.ServerMessage(Localization.Get("Skill requires a spear or two-handed spear."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			// Buff lasts 5 minutes (300 seconds)
			caster.StartBuff(BuffId.Finestra_Buff, skill.Level, 0, TimeSpan.FromMinutes(5), caster);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster, null);
		}
	}
}
