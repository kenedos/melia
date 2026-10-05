//--- Melia Script ----------------------------------------------------------
// Gem
//--- Description -----------------------------------------------------------
// Gems is a socket system for equipment in ToS.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class GemsClientScript : ClientScript
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
