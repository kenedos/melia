using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class TxItemScripts : GeneralScript
{
	/// <summary>
	/// Register's an item in "Goddess" Cabinet
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_OPEN_CABINET_GODDESS")]
	public DialogTxResult SCR_OPEN_CABINET_GODDESS(Character character, DialogTxArgs args)
	{
		var txItem = args.TxItems[0];
		var item = txItem.Item;
		var cabinetId = args.NumArgs[0];

		if (item.Id < ItemId.Open_Ticket_Cabinet_Goddess_Lv1 || item.Id > ItemId.Open_Ticket_Cabinet_Goddess_Lv3)
			return DialogTxResult.Fail;

		if (!ZoneServer.Instance.Data.CabinetDb.TryFind(CabinetType.Weapon, cabinetId, out var data))
			return DialogTxResult.Fail;

		var hasUnlockedCabinet = character.Connection.Account.Properties[data.AccountProperty] > 1;
		var prevCabinetUpgradeValue = (int)character.Connection.Account.Properties[data.UpgradeAccountProperty];

		if (!string.IsNullOrEmpty(data.AccountProperty))
			character.SetAccountProperty(data.AccountProperty, 1);

		if (!string.IsNullOrEmpty(data.UpgradeAccountProperty))
			character.SetAccountProperty(data.UpgradeAccountProperty, (int)item.Data.Script.NumArg1);

		character.Inventory.Remove(txItem.Item.ObjectId, txItem.Amount, msg: InventoryItemRemoveMsg.Given);

		if (hasUnlockedCabinet && item.Data.Script.NumArg1 > prevCabinetUpgradeValue)
			character.AddItem(ItemId.Open_Ticket_Cabinet_Goddess_Lv1 + (prevCabinetUpgradeValue - 1), 1);

		return DialogTxResult.Okay;
	}


	/// <summary>
	/// Register's an item in "Vaivora" Cabinet
	/// </summary>
	/// <param name="character"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_OPEN_CABINET_VIBORA")]
	public DialogTxResult SCR_OPEN_CABINET_VIBORA(Character character, DialogTxArgs args)
	{
		var txItem = args.TxItems[0];
		var item = txItem.Item;
		var cabinetId = args.NumArgs[0];

		if (item.Id < ItemId.Open_Ticket_Cabinet_Vibora_Lv1 || item.Id > ItemId.Open_Ticket_Cabinet_Vibora_Lv4)
			return DialogTxResult.Fail;

		if (!ZoneServer.Instance.Data.CabinetDb.TryFind(CabinetType.Weapon, cabinetId, out var data))
			return DialogTxResult.Fail;

		var hasUnlockedCabinet = character.Connection.Account.Properties[data.AccountProperty] > 1;
		var prevCabinetUpgradeValue = (int)character.Connection.Account.Properties[data.UpgradeAccountProperty];

		if (!string.IsNullOrEmpty(data.AccountProperty))
			character.SetAccountProperty(data.AccountProperty, 1);

		if (!string.IsNullOrEmpty(data.UpgradeAccountProperty))
			character.SetAccountProperty(data.UpgradeAccountProperty, (int)item.Data.Script.NumArg1);

		character.Inventory.Remove(item.ObjectId, txItem.Amount, msg: InventoryItemRemoveMsg.Given);

		if (hasUnlockedCabinet && item.Data.Script.NumArg1 > prevCabinetUpgradeValue)
			character.AddItem(ItemId.Open_Ticket_Cabinet_Vibora_Lv1 + (prevCabinetUpgradeValue - 1), 1);

		return DialogTxResult.Okay;
	}
}
