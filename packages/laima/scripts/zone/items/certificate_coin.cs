//--- Melia Script ----------------------------------------------------------
// Add Certificate Coin Items
//--- Description -----------------------------------------------------------
// Item scripts that add coin (Gabija/Vakrine/Lada(Rada)) to an account.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class CertificateCoinItemScript : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_CERTIFICATE_COIN(Character character, Item item, string propertyName, float coinAmount, float numArg2)
	{
		if (!character.ModifyAccountProperty(propertyName, coinAmount))
			return ItemUseResult.Fail;

		// Don't know the exact values for playing this effect (Seen value: 75000 ->
		if (coinAmount < 10001)
			character.PlayEffect("F_sys_TPBOX_normal_30", 2.5f, 1, EffectLocation.Bottom, 1);
		else if (coinAmount < 50001)
			character.PlayEffect("F_sys_TPBOX_normal_100", 2.5f, 1, EffectLocation.Bottom, 1);
		else if (coinAmount < 75001)
			character.PlayEffect("F_sys_TPBOX_great_200", 2.5f, 1, EffectLocation.Bottom, 1);
		else
			character.PlayEffect("F_sys_TPBOX_great_300", 2.5f, 1, EffectLocation.Bottom, 1);

		// Obtained Points message
		character.SystemMessage("PointGet{name}{count}", new MsgParameter("name", item.Data.Name), new MsgParameter("count", coinAmount.ToString()));

		return ItemUseResult.Okay;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_USE_MISC_PVP_MINE2(Character character, Item item, string propertyName, float numArg1, float numArg2)
	{
		var properties = character.Connection.Account.Properties;
		var previousValue = properties.GetFloat(PropertyName.MISC_PVP_MINE2);

		if (!character.ModifyAccountProperty(PropertyName.MISC_PVP_MINE2, item.Amount))
			return ItemUseResult.Fail;

		var newValue = properties.GetFloat(PropertyName.MISC_PVP_MINE2);
		character.SystemMessage("GET_MISC_PVP_MINE2{count}", new MsgParameter("count", item.Amount.ToString()));
		character.AddonMessage(AddonMessage.EARTHTOWERSHOP_BUY_ITEM_RESULT, $"{previousValue}/{newValue}");

		return ItemUseResult.Okay;
	}
}
