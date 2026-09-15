//--- Melia Script ----------------------------------------------------------
// Quest Chase Window
//--- Description -----------------------------------------------------------
// Replaces the client's quest tracker with the one driven by the custom
// quest system. Worlds that leave this out keep the client's own tracker.
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
