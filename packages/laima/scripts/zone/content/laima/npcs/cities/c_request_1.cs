//--- Melia Script ----------------------------------------------------------
// Fedimian Public House (c_request_1)
//--- Description -----------------------------------------------------------
// NPCs and shops in the Fedimian Mercenary Post public house.
//---------------------------------------------------------------------------

using System.Text;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

public class CRequest1NpcScript : GeneralScript
{
	// Account property that stores the mercenary badge balance. Mirrors the
	// existing SCR_USE_MISC_PVP_MINE2 flow: using misc_pvp_mine2 items adds
	// to this property.
	private const string MercenaryCurrencyProperty = "MISC_PVP_MINE2";

	// Client-side shop-point tag read by GET_PVP_POINT / propertyshop UI.
	private const string MercenaryShopPointName = "uphill_defense_shoppoint";
	private const string MercenaryShopName = "MercenaryShop";
	private const string MercenaryPointScript = "GET_PVP_POINT";

	protected override void Load()
	{
		CreateMercenaryArtefactShop();

		AddNpc(20055, L("[Mercenary Archivist]"), "MercenaryArchivist", "c_request_1", -35, -87, 90, async dialog =>
		{
			dialog.SetTitle(L("Mercenary Archivist"));

			await dialog.Msg(L("The Mercenary Guild keeps records of every weapon ever wielded within these walls. Bring me badges, and I shall share their likeness."));

			OpenMercenaryShop(dialog.Player);
		});
	}

	private static void OpenMercenaryShop(Character character)
	{
		if (!PropertyShops.TryGet(MercenaryShopName, out var shop))
			return;

		var conn = character.Connection;

		// Push the current badge balance so the UI header shows it
		var balance = (int)conn.Account.Properties.GetFloat(MercenaryCurrencyProperty);
		Send.ZC_SHOP_POINT_UPDATE(conn, MercenaryShopPointName, balance);

		// Stream the shop's items to the client. The client lua addon
		// (custom_propertyshop.lua) will install them so gePropertyShop.Get
		// returns a matching shopInfo when the UI opens.
		var sb = new StringBuilder();
		foreach (var item in shop.Items)
		{
			if (sb.Length > 0) sb.Append(',');
			sb.AppendFormat("{{name=\"{0}\",count={1},price={2}}}", item.ClassName, item.Amount, item.Price);
		}

		var pointScript = MercenaryPointScript;
		var script = $"M_SET_CUSTOM_PROPERTY_SHOP({{{sb}}}, '{MercenaryShopName}', '{pointScript}')";
		Send.ZC_EXEC_CLIENT_SCP(conn, script);

		// Open the property-shop UI
		Send.ZC_EXEC_CLIENT_SCP(conn, $"TOGGLE_PROPERTY_SHOP('{MercenaryShopName}', 1)");
	}

	private void CreateMercenaryArtefactShop()
	{
		PropertyShops.Create(MercenaryShopName, MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
			// Product order here MUST stay aligned with how the client sends
			// product indices (0-based, in order of iteration below).
			// One-Handed Swords
			shop.AddItem("Artefact_634003", 634003, 1, 30); // Bishop Rapier
			shop.AddItem("Artefact_634004", 634004, 1, 30); // Bishop Sword
			shop.AddItem("Artefact_634005", 634005, 1, 30); // Knight Sword
			shop.AddItem("Artefact_634006", 634006, 1, 30); // Knight Rapier
			// Two-Handed Swords
			shop.AddItem("Artefact_634007", 634007, 1, 60); // Ivory Queen Two-handed Sword
			shop.AddItem("Artefact_634008", 634008, 1, 60); // Ebony King Two-handed Sword
			// Two-Handed Maces
			shop.AddItem("Artefact_634001", 634001, 1, 50); // Ivory Pawn Two-handed Mace
			shop.AddItem("Artefact_634002", 634002, 1, 50); // Ivory Rook Two-handed Mace
			// Ranged / Magic (note: className is itemId - 1 for these)
			shop.AddItem("Artefact_630045", 630046, 1, 40); // Marine Musket
			shop.AddItem("Artefact_630046", 630047, 1, 40); // Marine Rod
			shop.AddItem("Artefact_630047", 630048, 1, 60); // Marine Staff
			// Shields
			shop.AddItem("Artefact_634015", 634015, 1, 25); // Chessboard Shield
			shop.AddItem("Artefact_634018", 634018, 1, 25); // Wooden Pane Shield
			shop.AddItem("Artefact_634043", 634043, 1, 25); // Gingerbread Shield
			shop.AddItem("Artefact_634055", 634055, 1, 25); // Fluffy Kitty Shield
			shop.AddItem("Artefact_634069", 634069, 1, 25); // Crunchy Choco Shield
			shop.AddItem("Artefact_634125", 634125, 1, 25); // Pot-lid Shield
			shop.AddItem("Artefact_634149", 634149, 1, 25); // TOS Ranger Shield
		});
	}
}
