using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.BlossomBlader
{
	/// <summary>
	/// StartUp.
	/// Toggles the one-handed sword specialization stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.BlossomBlader_StartUp)]
	public class BlossomBlader_StartUp : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (character.IsBuffActive(BuffId.StartUp_Buff))
			{
				character.RemoveBuff(BuffId.StartUp_Buff);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);
				return;
			}

			// Bloqueio 1: Off-hand (mão esquerda) precisa estar desequipada
			if (!BlossomBladerStartUpHelper.IsSubWeaponEmpty(character))
			{
				character.ServerMessage(Localization.Get("Your off-hand weapon slot must be empty to use StartUp."));
				return;
			}

			// Bloqueio 2: Precisa ter Espada de Uma Mão equipada
			if (!BlossomBladerStartUpHelper.IsUsingOneHandSword(character))
			{
				character.ServerMessage(Localization.Get("A one-handed sword must be equipped to activate StartUp."));
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			Send.ZC_SKILL_READY(character, skill, 1, originPos, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			character.StartBuff(BuffId.StartUp_Buff, 1f, 1f, TimeSpan.Zero, character, skill.Id);
			skill.IncreaseOverheat();
		}
	}
}
