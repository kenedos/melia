//--- Melia Script ----------------------------------------------------------
// Stat by Level
//--- Description -----------------------------------------------------------
// The game removed the StatByLevel property from the stat point
// calculation when they switched to auto-statting. This script
// adds it again.
//---------------------------------------------------------------------------

using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class StatByLevelClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
	}

	protected override void Ready(Character character)
	{
		// The scripts are only sent if the feature to disable stats by
		// level isn't enabled.
		if (!Feature.IsEnabled("NoStatByLevel"))
			this.SendAllScripts(character);
	}
}
