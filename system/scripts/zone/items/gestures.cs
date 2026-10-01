//--- Melia Script ----------------------------------------------------------
// Add Gesture Items
//--- Description -----------------------------------------------------------
// Item scripts that add a gesture to an account.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class GestureItemScript : GeneralScript
{
	[ScriptableFunction("SCR_USE_ITEM_ADD_GESTURE")]
	public ItemUseResult SCR_USE_ITEM_ADD_GESTURE(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (!GestureItems.TryGetValue(item.Id, out var propertyName))
			return ItemUseResult.Fail;
		character.SetAccountProperty(propertyName, 1);
		return ItemUseResult.Okay;
	}

	private readonly static Dictionary<int, string> GestureItems = new Dictionary<int, string>()
	{
		//[ItemId.Gesture_MAGICAL] = PropertyName.COLLECT_100_REWARD,
		[ItemId.Gesture_Cheerup] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_2,
		[ItemId.Gesture_Wave] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_1,
		[ItemId.Gesture_Unbelievable] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_3,
		[ItemId.Gesture_Dabdance] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_4,
		[ItemId.Gesture_Flex] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_5,
		[ItemId.Gesture_Homerun] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_6,
		[ItemId.Gesture_Nyang] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_7,
		[ItemId.Gesture_Wipe] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_8,
		[ItemId.Gesture_PULLUP] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_9,
		[ItemId.Gesture_BASEBALL] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_10,
		[ItemId.Gesture_SQUAT] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_11,
		[ItemId.Gesture_SPOTLIGHT] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_12,
		[ItemId.Gesture_WIGGLE] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_13,
		[ItemId.Gesture_SHOOTDANCE] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_14,
		[ItemId.Gesture_CHEER] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_15,
		[ItemId.Gesture_RUSTLE] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_16,
		[ItemId.Gesture_CLOCK] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_17,
		[ItemId.Gesture_LUCHADOR] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_18,
		[ItemId.Gesture_FLOWERSCENT] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_19,
		[ItemId.Gesture_QUEENCARD] = PropertyName.CONTENTS_TOTAL_SHOP_POSE_20,
	};
}
