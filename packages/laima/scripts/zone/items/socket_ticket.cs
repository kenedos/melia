//--- Melia Script ----------------------------------------------------------
// Socket Ticket Scripts
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class SocketTicketScripts : GeneralScript
{
	private const int AncientGoldenSocketId = 643032;

	private static DialogTxResult Fail(Character character, string code, string details)
	{
		var message = $"Socket ticket failed [{code}]: {details}";
		Log.Warning("SCR_ADD_SOCKET_BY_TICKET: {0}", message);
		character?.ServerMessage(message);
		return DialogTxResult.Fail;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ADD_SOCKET_BY_TICKET(Character character, DialogTxArgs args)
	{
		if (character == null)
			return DialogTxResult.Fail;
		if (args?.TxItems == null)
			return Fail(character, "ST01", "transaction item list is null");
		if (args.TxItems.Length != 2)
			return Fail(character, "ST02", $"expected 2 items, received {args.TxItems.Length}");

		var firstTxItem = args.TxItems[0];
		var secondTxItem = args.TxItems[1];
		var firstItem = firstTxItem.Item;
		var secondItem = secondTxItem.Item;

		if (firstItem == null || secondItem == null)
			return Fail(character, "ST03", "one or more transaction items are null");

		DialogTxItem ticketTxItem;
		DialogTxItem targetTxItem;

		if (firstItem.Id == AncientGoldenSocketId)
		{
			ticketTxItem = firstTxItem;
			targetTxItem = secondTxItem;
		}
		else if (secondItem.Id == AncientGoldenSocketId)
		{
			ticketTxItem = secondTxItem;
			targetTxItem = firstTxItem;
		}
		else
		{
			return Fail(character, "ST04", $"ticket 643032 not found; ids={firstItem.Id},{secondItem.Id}");
		}

		var targetItem = targetTxItem.Item;
		var ticketItem = ticketTxItem.Item;

		if (targetItem.ObjectId == ticketItem.ObjectId)
			return Fail(character, "ST05", $"target and ticket reference the same object={targetItem.ObjectId}");
		if (ticketItem.Data?.ClassName != "Old_Socket_Gold_Team")
			return Fail(character, "ST06", $"invalid ticket class={ticketItem.Data?.ClassName}");
		if (ticketItem.IsLocked || ticketItem.IsExpired)
			return Fail(character, "ST07", $"ticket locked={ticketItem.IsLocked}, expired={ticketItem.IsExpired}");
		if (targetItem.IsLocked)
			return Fail(character, "ST08", "target item is locked");
		if (targetItem.Data == null || targetItem.Data.Type != ItemType.Equip)
			return Fail(character, "ST09", $"target {targetItem.Id} is not equipment");
		if (targetItem.NeedsAppraisal || targetItem.NeedRandomOptions)
			return Fail(character, "ST10", $"appraisal={targetItem.NeedsAppraisal}, randomOptions={targetItem.NeedRandomOptions}");
		if (targetItem.Potential != 0)
			return Fail(character, "ST11", $"potential={targetItem.Potential}, expected=0");

		var maxSockets = targetItem.MaxSockets;
		var socketsUsed = targetItem.GetUsedSockets();
		var nextFreeSocket = targetItem.GetNextFreeSocket();

		if (maxSockets <= 0 || socketsUsed >= maxSockets || nextFreeSocket < 0 || nextFreeSocket >= maxSockets)
			return Fail(character, "ST12", $"max={maxSockets}, opened={socketsUsed}, next={nextFreeSocket}");

		var price = 100;
		if (ScriptableFunctions.ItemCalc.TryGet("SCR_Get_Item_SocketPrice", out var socketPriceFunc))
			price = (int)socketPriceFunc(targetItem);

		if (price < 0)
			return Fail(character, "ST13", $"invalid price={price}");
		if (!character.HasSilver(price))
			return Fail(character, "ST14", $"not enough silver; price={price}");
		if (price > 0 && character.RemoveItem(ItemId.Vis, price) != price)
			return Fail(character, "ST15", $"failed to remove silver; price={price}, Vis={ItemId.Vis}");

		var ticketRemoveResult = character.Inventory.Remove(ticketItem, 1, InventoryItemRemoveMsg.Used);
		if ((int)ticketRemoveResult != 0)
		{
			if (price > 0)
				character.AddItem(ItemId.Vis, price, "SocketTicketRefund");
			return Fail(character, "ST16", $"failed to consume ticket object={ticketItem.ObjectId}");
		}

		targetItem.CreateSocket(nextFreeSocket);
		Send.ZC_OBJECT_PROPERTY(character, targetItem);
		Send.ZC_ITEM_INVENTORY_DIVISION_LIST(character);
		Send.ZC_EQUIP_GEM_INFO(character);
		character.AddonMessage(AddonMessage.MSG_MAKE_ITEM_SOCKET);
		character.InvalidateProperties();

		return DialogTxResult.Okay;
	}
}
