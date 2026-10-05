using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Doppelsoeldner skill Deeds of Valor.
	/// </summary>
	[SkillHandler(SkillId.Doppelsoeldner_DeedsOfValor)]
	public class Doppelsoeldner_DeedsOfValor : ISelfSkillHandler
	{
		/// <summary>
		/// Toggles Deeds of Valor. The buff only works while a two-handed sword
		/// is equipped; its damage bonus is calculated by the buff handler.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character)
				return;

			if (character.IsBuffActive(BuffId.DeedsOfValor))
			{
				character.RemoveBuff(BuffId.DeedsOfValor);
				Send.ZC_SKILL_MELEE_TARGET(character, skill, character, null);
				return;
			}

			if (!IsUsingTwoHandedSword(character))
			{
				character.ServerMessage(Localization.Get("A two-handed sword is required."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			var duration = TimeSpan.Zero;
			character.StartBuff(BuffId.DeedsOfValor, 1, 1f, duration, character, skill.Id);

			Send.ZC_SKILL_MELEE_TARGET(character, skill, character, null);
		}

		private static bool IsUsingTwoHandedSword(Character character)
		{
			return character.TryGetEquipItem(EquipSlot.RightHand, out var weapon)
				&& weapon.Data.EquipType1 == EquipType.THSword;
		}
	}
}
