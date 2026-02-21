//--- Melia Script ----------------------------------------------------------
// Give Item Scripts
//--- Description -----------------------------------------------------------
// Item scripts that give specific items when used (box items).
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class GiveItemScripts : GeneralScript
{
	/// <summary>
	/// Gives a specified item when used.
	/// Used by Ability Point boxes.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="itemToGive">The item class name to give (strArg)</param>
	/// <param name="count">The amount to give (numArg1)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_Give_Item(Character character, Item item, string itemToGive, float count, float numArg2)
	{
		var amount = (int)count;
		if (amount <= 0)
			amount = 1;

		character.AddItem(itemToGive, amount);
		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Gives Enchant Scrolls (Premium_Enchantchip) when used.
	/// Used by Magic Scroll boxes.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="count">The amount to give (numArg1)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_Give_MagicScroll(Character character, Item item, string strArg, float count, float numArg2)
	{
		var amount = (int)count;
		if (amount <= 0)
			amount = 1;

		character.AddItem("Premium_Enchantchip", amount);
		return ItemUseResult.Okay;
	}

	/// <summary>
	/// Gives EXP Tomes (Premium_boostToken) when used.
	/// Used by Boost Token boxes.
	/// </summary>
	/// <remarks>
	/// Note: The original function name has a typo "Bosst" which is preserved
	/// for compatibility with existing item data.
	/// </remarks>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="count">The amount to give (numArg1)</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_Give_BosstToken(Character character, Item item, string strArg, float count, float numArg2)
	{
		var amount = (int)count;
		if (amount <= 0)
			amount = 1;

		character.AddItem("Premium_boostToken", amount);
		return ItemUseResult.Okay;
	}
}
