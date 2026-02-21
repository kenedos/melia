//--- Melia Script ----------------------------------------------------------
// Fedimian
//--- Description -----------------------------------------------------------
// NPCs found in and around Fedimian.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class CFedimianNpcScript : GeneralScript
{
	protected override void Load()
	{
		CreateWeaponShop();
		CreateArmorShop();
		CreateAccessoryShop();
		CreateMiscItemShop();

		// Statue of Goddess Vakarine
		//-------------------------------------------------------------------------
		AddNpc(10, 40120, "Statue of Goddess Vakarine", "c_fedimian", -280, 162, -239, 7, "WARP_C_FEDIMIAN", "STOUP_CAMP", "STOUP_CAMP");

		// [Item Merchant] Muras
		//-------------------------------------------------------------------------
		AddNpc(20055, "[Item Merchant] Muras", "Muras", "c_fedimian", -631.32, -174.9, 0, async dialog =>
		{
			dialog.SetTitle("Muras");
			dialog.SetPortrait("Dlg_port_Muras");

			if (RandomProvider.Get().NextDouble() >= 0.5)
				await dialog.Msg("H-Hello there! C-Can I help you with some good consumables?");
			else
				await dialog.Msg("O-Oh, hi. W-Welcome to my humble shop.");

			await dialog.OpenShop("FedimianMiscItems");
		});

		// [Storage Keeper] Zadan
		//-------------------------------------------------------------------------
		AddNpc(156027, "[Storage Keeper] Zadan", "Zadan", "c_fedimian", -170, -218, 0, async dialog =>
		{
			dialog.SetTitle("Zadan");
			dialog.SetPortrait("Dlg_port_Zadan");

			var response = await dialog.Select("Looking for a place to store your items safely? I'm just the guy.",
				Option("Personal Storage", "personal"),
				Option("Team Storage", "team"),
				Option("Save Spawn Location", "savelocation"),
				Option("Cancel", "cancel")
			);

			if (response == "personal")
				await dialog.OpenPersonalStorage();
			else if (response == "team")
				await dialog.OpenTeamStorage();
			else if (response == "savelocation")
			{
				await dialog.SaveLocation();
				await dialog.Msg("Your location has been saved!");
			}
		});

		// [Equipment Merchant] Yorgis
		//-------------------------------------------------------------------------
		AddNpc(151035, "[Equipment Merchant] Yorgis", "Yorgis", "c_fedimian", -219.15, -558.35, 90, async dialog =>
		{
			dialog.SetTitle("Dunkel");
			dialog.SetPortrait("Dlg_port_Yorgis");

			var response = await dialog.Select("Welcome! Only the best quality equipments around here!",
				Option("Weapons", "weapon"),
				Option("Armor", "armor"),
				Option("Cancel", "cancel")
			);

			if (response == "weapon")
				await dialog.OpenShop("FedimianWeapons");
			else if (response == "armor")
				await dialog.OpenShop("FedimianArmors");
		});

		// [Blacksmith] Anna
		//-------------------------------------------------------------------------
		AddNpc(126, 151036, "[Blacksmith]{nl}Anna", "c_fedimian", 120, 160, -504, 75, "FEDIMIAN_BLACKSMITH", "TUTO_REPAIR_NPC", "");

		// [Accessory Merchant] Joana
		//-------------------------------------------------------------------------
		AddNpc(151038, "[Accessory Merchant] Joana", "Joana", "c_fedimian", -130.2, -496.14, 0, async dialog =>
		{
			dialog.SetTitle("Joana");
			dialog.SetPortrait("Dlg_port_Yoana");

			if (await dialog.Hooks("BeforeDialog"))
				await dialog.Msg("Accessories~! Everyone loves good looking jewelry!");
			else
				await dialog.Msg("Accessories~! Everyone loves good looking jewelry!");

			await dialog.OpenShop("FedimianAccessories");
		});

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("c_fedimian", 139, 945, 867, 0, "blue");
		AddPlatformNpc("c_fedimian", 99, 985, 867, 0, "yellow");
		AddPlatformNpc("c_fedimian", 59, 1025, 867, 0, "red");
		AddPlatformNpc("c_fedimian", 19, 1065, 867, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.c_fedimian.Chest1", "c_fedimian", 19, 1065, 867, 0, ItemId.EmoticonItem_59_63, monsterId: 147393);
		AddPlatformNpc("c_fedimian", -21, 1025, 867, 0, "red");
		AddPlatformNpc("c_fedimian", -61, 985, 867, 0, "yellow");
		AddPlatformNpc("c_fedimian", -101, 945, 867, 0, "blue");
		AddPlatformNpc("c_fedimian", -101, 905, 827, 0, "green");
		AddPlatformNpc("c_fedimian", -101, 865, 787, 0, "red");
	}

	/// <summary>
	/// Creates the misc item shop
	/// </summary>
	private void CreateMiscItemShop()
	{
		CreateShop("FedimianMiscItems", shop =>
		{
			shop.AddItem(640002, amount: 1, price: 80);
			shop.AddItem(640003, amount: 1, price: 160);
			shop.AddItem(640005, amount: 1, price: 120);
			shop.AddItem(640006, amount: 1, price: 480);
			shop.AddItem(640008, amount: 1, price: 80);
			shop.AddItem(640009, amount: 1, price: 320);
			shop.AddItem(640073, amount: 1, price: 500);
			shop.AddItem(640156, amount: 1, price: 500);
			shop.AddItem(640182, amount: 1, price: 500);
			shop.AddItem(643002, amount: 1, price: 1500);
			shop.AddItem(645337, amount: 1, price: 20);
		});
	}

	/// <summary>
	/// Creates the accessory shop
	/// </summary>
	private void CreateAccessoryShop()
	{
		CreateShop("FedimianAccessories", shop =>
		{
			shop.AddItem(601111, amount: 1, price: 180);
			shop.AddItem(601112, amount: 1, price: 1296);
			shop.AddItem(601113, amount: 1, price: 1296);
			shop.AddItem(601114, amount: 1, price: 2592);
			shop.AddItem(601121, amount: 1, price: 5184);
			shop.AddItem(581112, amount: 1, price: 270);
			shop.AddItem(581113, amount: 1, price: 1944);
			shop.AddItem(581114, amount: 1, price: 1944);
			shop.AddItem(581115, amount: 1, price: 3888);
			shop.AddItem(581116, amount: 1, price: 7776);
		});
	}

	/// <summary>
	/// Creates the weapon shop with all available weapons
	/// </summary>
	private void CreateWeaponShop()
	{
		CreateShop("FedimianWeapons", shop =>
		{
			// One-Handed Swords
			shop.AddItem(101126, amount: 1, price: 400);
			shop.AddItem(101127, amount: 1, price: 3600);
			shop.AddItem(101128, amount: 1, price: 4320);
			shop.AddItem(101124, amount: 1, price: 6370);
			shop.AddItem(101129, amount: 1, price: 7280);
			shop.AddItem(101125, amount: 1, price: 19311);
			shop.AddItem(101130, amount: 1, price: 19311);

			// Two-Handed Swords
			shop.AddItem(121102, amount: 1, price: 5760);
			shop.AddItem(121120, amount: 1, price: 6912);
			shop.AddItem(121118, amount: 1, price: 10192);
			shop.AddItem(121121, amount: 1, price: 11648);
			shop.AddItem(121119, amount: 1, price: 30987);

			// Rods
			shop.AddItem(141126, amount: 1, price: 400);
			shop.AddItem(141127, amount: 1, price: 3600);
			shop.AddItem(141128, amount: 1, price: 4320);
			shop.AddItem(141124, amount: 1, price: 6370);
			shop.AddItem(141129, amount: 1, price: 7280);
			shop.AddItem(141125, amount: 1, price: 19311);

			// Staves
			shop.AddItem(271118, amount: 1, price: 640);
			shop.AddItem(271119, amount: 1, price: 5760);
			shop.AddItem(271120, amount: 1, price: 6912);
			shop.AddItem(271116, amount: 1, price: 10192);
			shop.AddItem(271121, amount: 1, price: 11648);
			shop.AddItem(271117, amount: 1, price: 30897);

			// Bows
			shop.AddItem(161126, amount: 1, price: 640);
			shop.AddItem(161127, amount: 1, price: 5760);
			shop.AddItem(161128, amount: 1, price: 6912);
			shop.AddItem(161124, amount: 1, price: 10192);
			shop.AddItem(161129, amount: 1, price: 11648);
			shop.AddItem(161125, amount: 1, price: 30897);

			// Crossbows
			shop.AddItem(181120, amount: 1, price: 3600);
			shop.AddItem(181121, amount: 1, price: 4320);
			shop.AddItem(181118, amount: 1, price: 6370);
			shop.AddItem(181122, amount: 1, price: 7280);
			shop.AddItem(181119, amount: 1, price: 19311);

			// Clubs
			shop.AddItem(201103, amount: 1, price: 400);
			shop.AddItem(201127, amount: 1, price: 3600);
			shop.AddItem(201126, amount: 1, price: 4320);
			shop.AddItem(201128, amount: 1, price: 6370);
			shop.AddItem(201124, amount: 1, price: 7280);
			shop.AddItem(201125, amount: 1, price: 19311);

			// Two-Handed Clubs
			shop.AddItem(210100, amount: 1, price: 30897);

			// Spears
			shop.AddItem(241109, amount: 1, price: 6370);
			shop.AddItem(241110, amount: 1, price: 7280);
			shop.AddItem(241111, amount: 1, price: 19311);

			// Two-Handed Spears
			shop.AddItem(251107, amount: 1, price: 30897);

			// Rapiers
			shop.AddItem(311111, amount: 1, price: 3600);
			shop.AddItem(311112, amount: 1, price: 7280);
			shop.AddItem(311113, amount: 1, price: 19311);

			// Pistols
			shop.AddItem(301112, amount: 1, price: 3600);
			shop.AddItem(301113, amount: 1, price: 7280);
			shop.AddItem(301114, amount: 1, price: 19311);

			// Daggers
			shop.AddItem(111001, amount: 1, price: 3600);
			shop.AddItem(111003, amount: 1, price: 7280);
			shop.AddItem(111005, amount: 1, price: 19311);

			// Trinkets
			shop.AddItem(692003, amount: 1, price: 7280);
		});
	}

	/// <summary>
	/// Creates the armor shop with all available armor pieces
	/// </summary>
	private void CreateArmorShop()
	{
		CreateShop("FedimianArmors", shop =>
		{
			// Dunkel Quilted Armor Set
			shop.AddItem(531129, amount: 1, price: 360);
			shop.AddItem(521129, amount: 1, price: 360);
			shop.AddItem(511129, amount: 1, price: 180);
			shop.AddItem(501129, amount: 1, price: 180);

			// Dunkel Cotton Armor Set
			shop.AddItem(531130, amount: 1, price: 2592);
			shop.AddItem(521130, amount: 1, price: 2592);
			shop.AddItem(511130, amount: 1, price: 1296);
			shop.AddItem(501130, amount: 1, price: 1296);

			// Dunkel Hard Leather Set
			shop.AddItem(531131, amount: 1, price: 2592);
			shop.AddItem(521131, amount: 1, price: 2592);
			shop.AddItem(511131, amount: 1, price: 1296);
			shop.AddItem(501131, amount: 1, price: 1296);

			// Dunkel Ring Mail Set
			shop.AddItem(531132, amount: 1, price: 2592);
			shop.AddItem(521132, amount: 1, price: 2592);
			shop.AddItem(511132, amount: 1, price: 1296);
			shop.AddItem(501132, amount: 1, price: 1296);

			// Acolyte Set
			shop.AddItem(531126, amount: 1, price: 5184);
			shop.AddItem(521126, amount: 1, price: 5184);
			shop.AddItem(511126, amount: 1, price: 2592);
			shop.AddItem(501126, amount: 1, price: 2592);

			// Mark Set
			shop.AddItem(531106, amount: 1, price: 5184);
			shop.AddItem(521106, amount: 1, price: 5184);
			shop.AddItem(511106, amount: 1, price: 2592);
			shop.AddItem(501106, amount: 1, price: 2592);

			// Scale Set
			shop.AddItem(531110, amount: 1, price: 5184);
			shop.AddItem(521110, amount: 1, price: 5184);
			shop.AddItem(511110, amount: 1, price: 2592);
			shop.AddItem(501110, amount: 1, price: 2592);

			// Superior Grima Set
			shop.AddItem(531153, amount: 1, price: 10368);
			shop.AddItem(521153, amount: 1, price: 10368);
			shop.AddItem(511153, amount: 1, price: 5184);
			shop.AddItem(501153, amount: 1, price: 5184);

			// Hard Veris Set
			shop.AddItem(531113, amount: 1, price: 10368);
			shop.AddItem(521113, amount: 1, price: 10368);
			shop.AddItem(511113, amount: 1, price: 5184);
			shop.AddItem(501113, amount: 1, price: 5184);

			// Full Plate Set
			shop.AddItem(531137, amount: 1, price: 10368);
			shop.AddItem(521137, amount: 1, price: 10368);
			shop.AddItem(511137, amount: 1, price: 5184);
			shop.AddItem(501137, amount: 1, price: 5184);

			// Shields
			shop.AddItem(221111, amount: 1, price: 400);
			shop.AddItem(221112, amount: 1, price: 3600);
			shop.AddItem(221105, amount: 1, price: 7280);
			shop.AddItem(221113, amount: 1, price: 19311);
		});
	}
}
