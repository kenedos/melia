using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.Network;

public class ItemQuestScripts : GeneralScript
{
	/// <summary>
	/// Attaches a balloon or removes it.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="strArg"></param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_USE_E_Balloon")]
	public ItemUseResult SCR_USE_E_Balloon(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var oldBalloonStr = character.Variables.Perm.GetString("Melia.BalloonId", "Dummy_balloon");
		var newBalloonStr = "artefact_balloon_" + strArg;

		if (oldBalloonStr == newBalloonStr)
		{
			oldBalloonStr = null;
			newBalloonStr = null;
		}

		character.Variables.Perm.SetString("Melia.BalloonId", newBalloonStr);

		Send.ZC_ATTACH_TO_SLOT(character, 5, newBalloonStr, oldBalloonStr);

		return ItemUseResult.OkayNotConsumed;
	}

	[ScriptableFunction]
	public ItemUseResult SCR_UES_ITEM_BOOK(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		Send.ZC_NORMAL.ShowBook(character, strArg);
		return ItemUseResult.OkayNotConsumed;
	}
}
