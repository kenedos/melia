//--- Melia Script ----------------------------------------------------------
// Achievement Item Scripts
//--- Description -----------------------------------------------------------
// Item scripts that unlock achievements by adding achievement points.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class AchievementItemScripts : GeneralScript
{
	/// <summary>
	/// Unlocks an achievement by adding 1 achievement point for the specified achievement.
	/// Used by weekly rank titles and special achievement items.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="achievementPointName">The achievement point class name (e.g., "WeeklyRank_1", "moringponia_first_kill_hard")</param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_ACHIEVE_WEEKLY_RANK_STRING(Character character, Item item, string achievementPointName, float numArg1, float numArg2)
	{
		character.Achievements.AddAchievementPoints(achievementPointName, 1);

		return ItemUseResult.Okay;
	}
}
