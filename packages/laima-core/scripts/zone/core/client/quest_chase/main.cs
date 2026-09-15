//--- Melia Script ----------------------------------------------------------
// Quest Chase Window
//--- Description -----------------------------------------------------------
// Replaces the client's quest tracker with the one driven by the custom
// quest system.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class LaimaQuestChaseClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
	}

	protected override void Ready(Character character)
	{
		this.SendAllScripts(character);
	}
}
