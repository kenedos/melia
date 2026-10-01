//--- Melia Script ----------------------------------------------------------
// Custom Property Shops Enabler
//--- Description -----------------------------------------------------------
// Installs a client-side shim so the server can inject propertyshop (point
// shop) contents at runtime via M_SET_CUSTOM_PROPERTY_SHOP, bypassing the
// need for a matching <Shop> entry in propertyshop.xml.
//---------------------------------------------------------------------------

using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;

public class CustomPropertyShopClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();
	}

	private const string LuaReadyFlag = "Melia.PropertyShop.LuaReady";

	protected override void Ready(Character character)
	{
		this.SendAllScripts(character);

		// The helper lua is now resident client-side, so the deferred
		// PlayerEnteredMap path may safely stream shops from here on.
		character.Variables.Temp.Set(LuaReadyFlag, true);

		// Stream any shops registered for the map the player just loaded
		// into. The initial PlayerEnteredMap fires before this point and
		// is skipped because the lua wasn't installed yet.
		Dialog.SendMapPropertyShops(character);
	}

	/// <summary>
	/// Streams any property shops registered for the player's map. Scripts
	/// place shopkeepers via Dialog.RegisterPropertyShopForMap, so the lua
	/// injection fires only on maps that actually host those shops.
	/// </summary>
	[On("PlayerEnteredMap")]
	private void OnPlayerEnteredMap(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		// PlayerEnteredMap fires in Map.AddCharacter, which runs before the
		// client reports ready and before Ready() ships the helper lua.
		// Skip until Ready() has flagged the lua as available; Ready()
		// handles the initial map's stream itself.
		if (!character.Variables.Temp.GetBool(LuaReadyFlag, false))
			return;

		Dialog.SendMapPropertyShops(character);
	}
}
