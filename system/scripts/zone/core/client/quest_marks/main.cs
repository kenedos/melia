//--- Melia Script ----------------------------------------------------------
// Quest Marks
//--- Description -----------------------------------------------------------
// Displays a marker above the heads of NPCs that have quests to give,
// quests in progress, or quests ready to be handed in.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class QuestMarksClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
	}

	protected override void Ready(Character character)
	{
		this.SendAllScripts(character);
		character.Quests.UpdateClient_QuestMarks();
	}
}
