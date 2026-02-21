//--- Melia Script ----------------------------------------------------------
// Klaipeda
//--- Description -----------------------------------------------------------
// NPCs found in and around Klaipeda.
//---------------------------------------------------------------------------

using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class CKlaipeNpcScript : GeneralScript
{
	protected override void Load()
	{
		CreateWeaponShop();
		CreateArmorShop();
		CreateAccessoryShop();
		CreateMiscItemShop();
		CreateCompanionFoodShop();
		CreateChristinaCompanionShop();

		// Storage Keeper
		//-------------------------------------------------------------------------

		AddNpc(154018, "[Storage Keeper] Rita", "Rita", "c_Klaipe", 317, 279, 90.0, async dialog =>
		{
			dialog.SetTitle("Rita");
			dialog.SetPortrait("WAREHOUSE_DLG");

			var response = await dialog.Select("Hello! Can I help you store your items?",
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

		// [Item Merchant] Mirina
		//-------------------------------------------------------------------------
		var itemMerchant = AddNpc(20115, "[Item Merchant] Mirina", "Mirina", "c_Klaipe", 510.7029, -349.3194, 90.0, async dialog =>
		{
			dialog.SetTitle("Mirina");
			dialog.SetPortrait("Dlg_port_TOOL_DEALER");

			if (RandomProvider.Get().NextDouble() >= 0.5)
				await dialog.Msg("Care for some potions or helpful consumables?{nl}Buy at Mirina's shop today!");
			else
				await dialog.Msg("Mirina's shop at your service~");

			await dialog.OpenShop("KlaipedaMiscItems");
		});

		itemMerchant.AssociatedShopName = "KlaipedaMiscItems"; // Set the shop name for the NPC
		itemMerchant.ShopType = ShopType.Potion; // Set the shop type for the NPC

		// Equipment Merchant
		//-------------------------------------------------------------------------

		var equipmentMerchant = AddNpc(20111, "[Equipment Merchant] Dunkel", "Dunkel", "c_Klaipe", 394, -475, 90.0, async dialog =>
		{
			dialog.SetTitle("Dunkel");
			dialog.SetPortrait("Dlg_port_vickers");

			var response = await dialog.Select("Take a look around at your own pace without feeling anxious.",
				Option("Weapons", "weapon"),
				Option("Armor", "armor"),
				Option("Cancel", "cancel")
			);

			if (response == "weapon")
				await dialog.OpenShop("KlaipedaWeapons");
			else if (response == "armor")
				await dialog.OpenShop("KlaipedaArmors");
		});

		equipmentMerchant.AssociatedShopName = "KlaipedaWeapons"; // Set the shop name for the NPC
		equipmentMerchant.ShopType = ShopType.Weapon; // Set the shop type for the NPC

		// Accessory Merchant
		//-------------------------------------------------------------------------

		var ronesa = AddNpc(20104, "[Accessory Merchant] Ronesa", "Ronesa", "c_Klaipe", 269, -611, 90.0, async dialog =>
		{
			dialog.SetTitle("Ronesa");
			dialog.SetPortrait("Dlg_port_KLAPEDA_ACCESSORY");

			if (await dialog.Hooks("BeforeDialog"))
				await dialog.Msg("While you're here, do you need anything?{nl}I've got some hard-to-find stuff.");
			else
				await dialog.Msg("Welcome.{nl}Only hard-to-find stuff here.");

			await dialog.OpenShop("KlaipedaAccessories");
		});

		ronesa.AssociatedShopName = "KlaipedaAccessories"; // Set the shop name for the NPC
		ronesa.ShopType = ShopType.Accessory; // Set the shop type for the NPC

		// [Blacksmith] Zaras
		//-------------------------------------------------------------------------
		AddNpc(13, 20105, "[Blacksmith]{nl}Zaras", "c_Klaipe", 600, -1, -83, 90, "KLAPEDA_BLACKSMITH", "TUTO_REPAIR_NPC", "");

		// [Companion Trader] Christina
		//-------------------------------------------------------------------------
		var christina = AddNpc(153005, "[Companion Trader] Christina", "Christina", "c_Klaipe", -1, -760, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Christina");
			dialog.SetPortrait("Dlg_port_kristina");

			var options = dialog.CreateOptions(
				Option("Adopt Companion", "adopt"),
				Option("Buy Pet Food", "food"),
				Option(ScpArgMsg("shop_companion_learnabil"), "learnabil", () => character.HasCompanions),
				Option(ScpArgMsg("shop_companion_info"), "info"),
				Option("Leave", "leave")
			);

			var selectedOption = await dialog.Select("PETSHOP_KLAIPE_basic1", options);

			switch (selectedOption)
			{
				case "adopt":
					await dialog.OpenCustomCompanionShop("ChristinaCompanions");
					break;
				case "learnabil":
					dialog.OpenAddon(AddonMessage.COMPANION_UI_OPEN);
					break;
				case "info":
					await dialog.Msg("PETSHOP_KLAIPE_basic2");
					break;
				case "food":
					await dialog.OpenShop("ChristinaCompanionFood");
					break;
			}
		});

		christina.AssociatedShopName = "ChristinaCompanions";
		christina.ShopType = ShopType.Potion;

		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddNpc(10017, 154039, "Statue of Goddess Ausrine", "c_Klaipe", -206.574, 148.8251, 98.63973, 45, "WARP_C_KLAIPE", "STOUP_CAMP", "STOUP_CAMP");
	}

	/// <summary>
	/// Creates the misc item shop
	/// </summary>
	private void CreateMiscItemShop()
	{
		CreateShop("KlaipedaMiscItems", shop =>
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
		CreateShop("KlaipedaAccessories", shop =>
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
		CreateShop("KlaipedaWeapons", shop =>
		{
			// One-Handed Swords
			shop.AddItem(101126, amount: 1, price: 400);
			shop.AddItem(101127, amount: 1, price: 3600);
			shop.AddItem(101128, amount: 1, price: 4320);
			shop.AddItem(101124, amount: 1, price: 6370);
			shop.AddItem(101129, amount: 1, price: 7280);
			shop.AddItem(101125, amount: 1, price: 19311);
			shop.AddItem(101130, amount: 1, price: 100000, FactionId.Klaipeda, ReputationTier.Honored);

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
		CreateShop("KlaipedaArmors", shop =>
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

	/// <summary>
	/// Creates the companion food shop with custom prices
	/// </summary>
	private void CreateCompanionFoodShop()
	{
		CreateShop("ChristinaCompanionFood", shop =>
		{
			shop.AddItem(640152, amount: 1, price: 600);
			shop.AddItem(640231, amount: 1, price: 600);
			shop.AddItem(640236, amount: 1, price: 600);
			shop.AddItem(640249, amount: 1, price: 600);
			shop.AddItem(640189, amount: 1, price: 600);
			shop.AddItem(640188, amount: 1, price: 600);
		});
	}

	/// <summary>
	/// Creates Christina's companion shop with custom prices
	/// Klaipeda merchant city theme: trade animals, exotic pets, practical companions
	/// </summary>
	private void CreateChristinaCompanionShop()
	{
		CreateCompanionShop("ChristinaCompanions", shop =>
		{
			shop.AddCompanion("Velhider", price: 40000);
			shop.AddCompanion("hoglan_Pet", price: 40000);
			shop.AddCompanion("pet_hawk", price: 40000);

			shop.AddCompanion("Guineapig", price: 110000);
			shop.AddCompanion("barn_owl", price: 110000);
			shop.AddCompanion("Lesser_panda", price: 110000);
			shop.AddCompanion("Toucan", price: 110000);
			shop.AddCompanion("Piggy", price: 110000);
			shop.AddCompanion("penguin_green", price: 110000);
		});
	}
}
