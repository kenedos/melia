//--- Melia Script ----------------------------------------------------------
// Hidden Quest Gates
//--- Description -----------------------------------------------------------
// Conditions that decide when a hidden quest is offered.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;

public static class HiddenQuestGates
{
	/// <summary>
	/// Returns whether the character completed every one of the quests
	/// that exist in this world.
	/// </summary>
	public static bool CompletedAll(Character character, params int[] questIds)
	{
		return questIds
			.Select(id => new QuestId(id))
			.Where(QuestScript.Exists)
			.All(character.Quests.HasCompleted);
	}

	/// <summary>
	/// Returns whether the character's account has fully explored every
	/// one of the maps.
	/// </summary>
	public static bool ExploredAll(Character character, params string[] mapClassNames)
	{
		return mapClassNames.All(map => character.GetMapExplorationPercentage(map) >= 100);
	}
}
