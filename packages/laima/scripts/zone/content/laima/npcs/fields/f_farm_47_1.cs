//--- Melia Script ----------------------------------------------------------
// Tenants' Farm
//--- Description -----------------------------------------------------------
// NPCs found in and around Tenants' Farm.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class FFarm471NpcScript : GeneralScript
{
	protected override void Load()
	{
		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(4, 40120, "Statue of Goddess Vakarine", "f_farm_47_1", -1250.313, -41.2164, -270.3558, 90, "WARP_F_FARM_47_1", "STOUP_CAMP", "STOUP_CAMP");
		
		// Lv1 Treasure Chest
		//-------------------------------------------------------------------------
		AddNpc(64, 147392, "Lv1 Treasure Chest", "f_farm_47_1", -1173.09, 40.77, -1323.89, 90, "TREASUREBOX_LV_F_FARM_47_164", "", "");

		CreateShop("IzoldeFruits", shop =>
		{
			shop.AddItem(640053, amount: 1, price: 200);  // Strawberry (HP DoT)
			shop.AddItem(640025, amount: 1, price: 240);  // Varnalesa (SP)
			shop.AddItem(640027, amount: 1, price: 180);  // Ciobrelis (Stamina)
			shop.AddItem(640044, amount: 1, price: 360);  // Pine Mushroom (HP+SP)
			shop.AddItem(640072, amount: 1, price: 240);  // Popolion Meat (HP)
			shop.AddItem(640123, amount: 1, price: 160);  // White Bread (HP)
			shop.AddItem(640124, amount: 1, price: 220);  // Lettuce (SP)
			shop.AddItem(640125, amount: 1, price: 180);  // Milk (Stamina)
		});

		CreateShop("AgneArmbandRecipes", shop =>
		{
			shop.AddItem(946002, amount: 1, price: 80000);  // Recipe - Knotted Armband
			shop.AddItem(946004, amount: 1, price: 80000);  // Recipe - Feather Armband
			shop.AddItem(946005, amount: 1, price: 80000);  // Recipe - Tasseled Armband
		});
	}
}
