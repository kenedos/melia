//--- Melia Script ----------------------------------------------------------
// Custom Property Shops Enabler
//--- Description -----------------------------------------------------------
// Installs a client-side shim so the server can inject propertyshop (point
// shop) contents at runtime via M_SET_CUSTOM_PROPERTY_SHOP, bypassing the
// need for a matching <Shop> entry in propertyshop.xml.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class CustomPropertyShopClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadLuaScript("001.lua");
	}

	protected override void Ready(Character character)
	{
		this.SendLuaScript(character, "001.lua");
	}
}
