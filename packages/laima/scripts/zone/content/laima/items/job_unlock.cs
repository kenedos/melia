//--- Melia Script ----------------------------------------------------------
// Job Unlock Items
//--- Description -----------------------------------------------------------
// Item scripts that unlock hidden job classes.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class JobUnlockItemScripts : GeneralScript
{
	protected override void Load()
	{
		Log.Info("JobUnlockItemScripts loaded.");
	}

/// <summary>
/// Unlocks a hidden job class for the character.
/// </summary>
/// <param name="character">The character using the item.</param>
/// <param name="item">The consumed unlock voucher.</param>
/// <param name="strArg">The hidden job class name, such as Char4_18.</param>
/// <param name="numArg1">Unused by this implementation. Kept for compatibility.</param>
/// <param name="numArg2">Unused by this implementation. Kept for compatibility.</param>
/// <returns>The item use result.</returns>
[ScriptableFunction]
public ItemUseResult SCR_USE_CustomHiddenJobUnlock(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		// Verify that the hidden job exists.
		if (!ZoneServer.Instance.Data.JobDb.TryFind(strArg, out var jobData))
		{
			character.ServerMessage($"Invalid job class: {strArg}");
			return ItemUseResult.Fail;
		}

		// Verify that this job is configured as a hidden job.
		//if (!jobData.IsHidden)
		//{
		//	character.ServerMessage("This is not a hidden job.");
		//	return ItemUseResult.Fail;
		//}

		// Resolve the ETC property used by the advancement system.
		if (!TryGetHiddenJobProperty(strArg, out var propertyName))
		{
			character.ServerMessage($"Unsupported hidden job: {strArg}");
			return ItemUseResult.Fail;
		}

		// Check if the job is already unlocked.
		var currentValue = character.Etc.Properties.GetFloat(propertyName, 0);

		if (currentValue >= 300)
		{
			character.ServerMessage("This hidden class is already unlocked.");
			return ItemUseResult.OkayNotConsumed;
		}

		// The original ToS unlock value is 300.
		character.Etc.Properties.SetFloat(propertyName, 300);

		// Update the client.
		Send.ZC_OBJECT_PROPERTY(character, character.Etc, propertyName);

		// Notify the player.
		character.ServerMessage($"The hidden class [{jobData.Name}] has been unlocked.");

		return ItemUseResult.Okay;
	}


	/// <summary>
	/// Resolves the hidden job ETC property used by the advancement system. 
	/// </summary> 
	private bool TryGetHiddenJobProperty(string jobClassName, out string propertyName) 
	{
		switch (jobClassName) 
		{ 
			case "Char1_20": propertyName = PropertyName.HiddenJob_Char1_20; 
				return true; 
			case "Char2_17": propertyName = PropertyName.HiddenJob_Char2_17; 
				return true; 
			case "Char3_13": propertyName = PropertyName.HiddenJob_Char3_13; 
				return true; 
			case "Char4_18": propertyName = PropertyName.HiddenJob_Char4_18; 
				return true; 
			case "Char5_6": propertyName = PropertyName.HiddenJob_Char5_6; 
				return true; 
			default: propertyName = null; 
				return false; 
		}
	}
}
