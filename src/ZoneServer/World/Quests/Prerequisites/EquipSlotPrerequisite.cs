using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Prerequisites
{
	public class EquipSlotPrerequisite : QuestPrerequisite
	{
		/// <summary>
		/// Returns the equipped slots required, to meet this prerequisite.
		/// </summary>
		public EquipSlot[] EquipSlots { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="equipSlots"></param>
		public EquipSlotPrerequisite(params EquipSlot[] equipSlots)
		{
			this.EquipSlots = equipSlots;
		}

		/// <summary>
		/// Returns true if the character's item amount is equal or higher than
		/// this prerequisite's minimum item amount required.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public override bool Met(Character character)
		{
			foreach (var equipSlot in this.EquipSlots)
			{
				if (character.Inventory.GetEquip(equipSlot) == null)
					return false;
			}
			return true;
		}
	}
}
