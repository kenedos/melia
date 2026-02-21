using Melia.Shared.Game.Const;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// A function that handles the usage of an item.
	/// </summary>
	/// <param name="character">The character who used the item.</param>
	/// <param name="item">The item that is being used.</param>
	/// <param name="strArg">String argument, as defined in the item data.</param>
	/// <param name="numArg1">First number argument, as defined in the item data.</param>
	/// <param name="numArg2">Second number argument, as defined in the item data.</param>
	/// <returns></returns>
	public delegate ItemUseResult ItemScriptFunc(Character character, Item item, string strArg, float numArg1, float numArg2);

	/// <summary>
	/// Specifies the result of using an item.
	/// </summary>
	public enum ItemUseResult
	{
		/// <summary>
		/// The item was used successfully and should be decremented.
		/// </summary>
		Okay,

		/// <summary>
		/// The item was used successfully but should not be decremented.
		/// </summary>
		OkayNotConsumed,

		/// <summary>
		/// The usage failed and the item should not be decremented.
		/// </summary>
		Fail,
	}

	/// <summary>
	/// A function that handles the equipping of an item.
	/// </summary>
	/// <param name="character">The character who equipped the item.</param>
	/// <param name="item">The item that is being equipped.</param>
	/// <param name="slot">The slot in which the item is being equipped.</param>
	/// <returns></returns>
	public delegate ItemEquipResult ItemEquipScriptFunc(Character character, Item item, EquipSlot slot);

	public enum ItemEquipResult
	{
		/// <summary>
		/// The item was equipped successfully.
		/// </summary>
		Okay,

		/// <summary>
		/// The item equip failed.
		/// </summary>
		Fail,
	}

	/// <summary>
	/// A function that handles the unequipping of an item.
	/// </summary>
	/// <param name="character">The character who used the item.</param>
	/// <param name="item">The item that is being unequipped.</param>
	/// <param name="slot">The slot in which the item is being equipped.</param>
	/// <returns></returns>
	public delegate ItemUnequipResult ItemUnequipScriptFunc(Character character, Item item, EquipSlot slot);

	public enum ItemUnequipResult
	{
		/// <summary>
		/// The item was unequipped successfully.
		/// </summary>
		Okay,

		/// <summary>
		/// The item unequip failed.
		/// </summary>
		Fail,
	}


	/// <summary>
	/// A function that handles the usage of an item.
	/// </summary>
	/// <param name="character">The character who used the item.</param>
	/// <param name="itemUsed">The item that is being used.</param>
	/// <param name="itemUsedOn">The item that is being used on.</param>
	/// <param name="strArg">String argument, as defined in the item data.</param>
	/// <param name="numArg1">First number argument, as defined in the item data.</param>
	/// <param name="numArg2">Second number argument, as defined in the item data.</param>
	/// <returns></returns>
	public delegate ItemUseResult ItemUseOnItemScriptFunc(Character character, Item itemUsed, Item itemUsedOn, string strArg, float numArg1, float numArg2);
}
