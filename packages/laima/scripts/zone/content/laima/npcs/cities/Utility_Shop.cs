//--- Melia Script ----------------------------------------------------------
// Utility Shop
//--- Description -----------------------------------------------------------
// Custom utility shop NPC in Klaipeda.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Yggrasil.Ai.BehaviorTree.Leafs;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.Network;

public class UtilityShopNpcScript : GeneralScript
{
	protected override void Load()
	{
		CreateUtilityShop();

		// [Utility Shop]
		//-------------------------------------------------------------------------
		var utilityShop = AddNpc(20115, L("[Utility Shop]"), "Utility Shop", "c_Klaipe", 440, 80, 270, async dialog =>
		{
			dialog.SetTitle(L("Utility Shop"));
			dialog.SetPortrait("Dlg_port_TOOL_DEALER");

			await dialog.Msg(L("Welcome. I sell useful utility items."));

			await dialog.OpenShop("UtilityShop");
		});

		utilityShop.AssociatedShopName = "UtilityShop";
		utilityShop.ShopType = ShopType.Potion;
}

	/// <summary>
	/// Creates the Utility Shop.
	/// Replace the item IDs with the exact IDs from items.txt/items.ies.
	/// </summary>
	private void CreateUtilityShop()
	{
		CreateShop("UtilityShop", shop =>
		{
			/*// Mystic Tomes
			shop.AddItem(2020002, amount: 1, price: 1000000); // Unidentified Mystic Tome Swordsman
			shop.AddItem(2020003, amount: 1, price: 1000000); // Unidentified Mystic Tome Wizard
			shop.AddItem(2020004, amount: 1, price: 1000000); // Unidentified Mystic Tome Archer
			shop.AddItem(2020005, amount: 1, price: 1000000); // Unidentified Mystic Tome Cleric
			shop.AddItem(2020006, amount: 1, price: 1000000); // Unidentified Mystic Tome Scout*/

			//Transcedence itens
			shop.AddItem(645783, amount: 1, price: 1000000); //blessed shard
			shop.AddItem(646045, amount: 1, price: 1000000); //blessed gem
			shop.AddItem(919018, amount: 1, price: 1000000); //Recipe - Goddesses' Blessed Gem

			// Hidden class unlock voucher
			shop.AddItem(642539, amount: 1, price: 10000000); // Unlock Voucher Selection

			//Class Unlock Vouchers
			shop.AddItem(490273, amount: 1, price: 10000000); // Swordsman Unlock Voucher
			shop.AddItem(490181, amount: 1, price: 10000000); // Cleric Unlock Voucher 
			shop.AddItem(490182, amount: 1, price: 10000000); // Archer Unlock Voucher
			shop.AddItem(490183, amount: 1, price: 10000000); // Wizard Unlock Voucher
			shop.AddItem(490184, amount: 1, price: 10000000); // Scout Unlock Voucher
		});
	}
}
