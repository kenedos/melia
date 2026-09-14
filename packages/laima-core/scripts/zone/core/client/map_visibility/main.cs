//--- Melia Script ----------------------------------------------------------
// Map Visibility
//--- Description -----------------------------------------------------------
// Makes warps/NPCs visible in map regardless of range to player
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class MapVisibilityClientScript : ClientScript
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
