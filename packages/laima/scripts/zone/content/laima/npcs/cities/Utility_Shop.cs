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
		var utilityShop = AddNpc(147510, L("[Utility Shop] Thomas"), "Utility Shop", "c_Klaipe", 440, 80, 270, async dialog =>
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

			/*//Transcedence itens
			shop.AddItem(645783, amount: 1, price: 1000000); //blessed shard
			shop.AddItem(646045, amount: 1, price: 1000000); //blessed gem*/
			shop.AddItem(919018, amount: 1, price: 10000); //Recipe - Goddesses' Blessed Gem

			//Portal Stone
			shop.AddItem(646064, amount: 1, price: 1000); // Portal Stone

			// Hidden class unlock voucher
			//shop.AddItem(642539, amount: 1, price: 1000000); // Unlock Voucher Selection

			//Class Unlock Vouchers
			shop.AddItem(490273, amount: 1, price: 10000); // Swordsman Unlock Voucher
			shop.AddItem(490181, amount: 1, price: 10000); // Cleric Unlock Voucher 
			shop.AddItem(490182, amount: 1, price: 10000); // Archer Unlock Voucher
			shop.AddItem(490183, amount: 1, price: 10000); // Wizard Unlock Voucher
			shop.AddItem(490184, amount: 1, price: 10000); // Scout Unlock Voucher

			//Exp Tomes
			shop.AddItem(490015, amount: 1, price: 1000000); // Exp Tome
			shop.AddItem(919013, amount: 1, price: 100000); // Recipe - x4 Exp Tome
			shop.AddItem(919014, amount: 1, price: 100000); // Recipe - x8 Exp Tome

			//Recipes
			shop.AddItem(943087, amount: 1, price: 2000000); //Recipe - Frieno Necklace
			shop.AddItem(943086, amount: 1, price: 2000000); //Recipe - Pasiutes Necklace
			shop.AddItem(943084, amount: 1, price: 2000000); //Recipe - Lynnki Sit Necklace
			shop.AddItem(943085, amount: 1, price: 2000000); //Recipe - Kite Moor Necklace
			shop.AddItem(943083, amount: 1, price: 1000000); //Recipe - Frieno Bracelet
			shop.AddItem(943082, amount: 1, price: 1000000); //Recipe - Pasiutes Bracelet
			shop.AddItem(943080, amount: 1, price: 1000000); //Recipe - Lynnki Sit Bracelet
			shop.AddItem(943081, amount: 1, price: 1000000); //Recipe - Kite Moor Bracelet
			shop.AddItem(919022, amount: 1, price: 1000000); //Recipe - Ominous Spirit Crystal
		});
	}
}
