//--- Melia Script ----------------------------------------------------------
// DOT Buff Items
//--- Description -----------------------------------------------------------
// Item scripts that heal or damage over time.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class DotBuffItemScript : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_DotBuff(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		character.Buffs.Start(buffName, numArg1, numArg2, TimeSpan.FromSeconds(15), character);
		character.AddAchievementPoint("Potion", 1);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	[ScriptableFunction("SCR_USE_ITEM_DotBuff_Time")]
	public ItemUseResult SCR_USE_ITEM_AddBuff(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		character.Buffs.Start(buffName, numArg1, 0, TimeSpan.FromMilliseconds(numArg2), character);
		character.AddAchievementPoint("Potion", 1);

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_DETOXIFIY(Character character, Item item, string buffName, float numArg1, float numArg2)
	{
		var buffs = character.Buffs.GetAll(buff => buff.Data.Tags.Has(BuffTag.Poison));

		foreach (var buff in buffs)
			character.RemoveBuff(buff.Id);

		return ItemUseResult.Okay;
	}
}
