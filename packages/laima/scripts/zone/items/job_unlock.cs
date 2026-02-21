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

public class JobUnlockItemScripts : GeneralScript
{
	/// <summary>
	/// Unlocks a hidden job class for the character.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="jobClassName">The job class name (e.g., Char4_18 for Miko)</param>
	/// <param name="numArg1">Job rank</param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_HiddenJobUnlock(Character character, Item item, string jobClassName, float numArg1, float numArg2)
	{
		// Verify the job exists and is a hidden job
		if (!ZoneServer.Instance.Data.JobDb.TryFind(jobClassName, out var jobData))
		{
			character.SystemMessage("Invalid job class.");
			return ItemUseResult.Fail;
		}

		if (!jobData.IsHidden)
		{
			character.SystemMessage("This is not a hidden job.");
			return ItemUseResult.Fail;
		}

		// Check if already unlocked (value of 300 means unlocked)
		var propertyName = "HiddenJob_" + jobClassName;
		var currentValue = character.Etc.Properties.GetFloat(propertyName, 0);
		if (currentValue == 300)
		{
			// Already unlocked - don't consume item
			character.SystemMessage("This job is already unlocked.");
			return ItemUseResult.OkayNotConsumed;
		}

		// Unlock the job by setting the ETC property to 300
		character.Etc.Properties.SetFloat(propertyName, 300);

		// Send property update to client
		Send.ZC_OBJECT_PROPERTY(character, character.Etc, propertyName);

		// Show success message
		var jobName = jobData.Name;
		character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, $"The hidden class [{jobName}] has been unlocked!");

		return ItemUseResult.Okay;
	}
}
