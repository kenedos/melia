//--- Melia Script ----------------------------------------------------------
// Transcending Items
//--- Description -----------------------------------------------------------
// Item-related scripts that transcend equipment.
//---------------------------------------------------------------------------

using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class TranscendingItemScripts : GeneralScript
{
	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_TRANSCEND(
		Character character,
		DialogTxArgs args)
	{
		if (args.TxItems == null || args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;

		if (!TranscendenceHelper.CanBeTranscended(item))
		{
			character.ServerMessage(
				"This item cannot be transcended.");

			return DialogTxResult.Fail;
		}

		var success = TranscendenceHelper.TryTranscendItem(
			character,
			item,
			out var message);

		character.ServerMessage(message);

		return success
			? DialogTxResult.Okay
			: DialogTxResult.Fail;
	}

	[ScriptableFunction("SCR_ITEM_TRANSCEND_TX")]
	public DialogTxResult SCR_ITEM_TRANSCEND_TX(
		Character character,
		DialogTxArgs args)
	{
		if (args.TxItems == null || args.TxItems.Length < 1)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;

		if (!TranscendenceHelper.CanBeTranscended(item))
		{
			character.ServerMessage(
				"This item cannot be transcended.");

			return DialogTxResult.Fail;
		}

		var success = TranscendenceHelper.TryTranscendItem(
			character,
			item,
			out var message);

		character.ServerMessage(message);

		if (!success)
			return DialogTxResult.Fail;

		character.ExecuteClientScript(
			"ui.CloseFrame('itemtranscend');");

		character.ExecuteClientScript(
			"ui.CloseFrame('inventory');");

		character.ExecuteClientScript(
			"control.EnableControl(1);");

		Send.ZC_DIALOG_CLOSE(character.Connection);
		Send.ZC_LEAVE_TRIGGER(character.Connection);

		return DialogTxResult.Okay;
	}

	[ScriptableFunction("SCR_REQ_LEGEND_ITEM_DIALOG")]
	public CustomCommandResult SCR_REQ_LEGEND_ITEM_DIALOG(
		Character character,
		int numArg1,
		int numArg2,
		int numArg3)
	{
		character.ServerMessage(
			$"SCR_REQ_LEGEND_ITEM_DIALOG args: " +
			$"{numArg1}, {numArg2}, {numArg3}");

		return CustomCommandResult.Okay;
	}
}
