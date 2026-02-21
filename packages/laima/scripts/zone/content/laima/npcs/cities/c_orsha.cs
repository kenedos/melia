//--- Melia Script ----------------------------------------------------------
// Orsha
//--- Description -----------------------------------------------------------
// NPCs found in and around Orsha.
//---------------------------------------------------------------------------

using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class COrshaNpcScript : GeneralScript
{
	protected override void Load()
	{
		CreateWeaponShop();
		CreateArmorShop();
		CreateAccessoryShop();
		CreateMiscItemShop();
		CreateCompanionFoodShop();
		CreateTorasCompanionShop();

		// [Equipment Merchant] Jura
		//-------------------------------------------------------------------------
		AddNpc(20056, "[Equipment Merchant] Jura", "Jura", "c_orsha", 21, 154, 123.0, async dialog =>
		{
			dialog.SetTitle("Jura");
			dialog.SetPortrait("Dlg_port_Julla");

			var response = await dialog.Select("Welcome! What kind of equipment are you looking for?",
				Option("Weapons", "weapon"),
				Option("Armor", "armor"),
				Option("Cancel", "cancel")
			);

			if (response == "weapon")
				await dialog.OpenShop("OrshaWeapons");
			else if (response == "armor")
				await dialog.OpenShop("OrshaArmors");
		});

		// [Item Merchant] Alf
		//-------------------------------------------------------------------------
		AddNpc(20055, "[Item Merchant] Alf", "Alf", "c_orsha", 231, 166, 120.0, async dialog =>
		{
			dialog.SetTitle("Alf");
			dialog.SetPortrait("Dlg_port_Alf");

			if (RandomProvider.Get().NextDouble() >= 0.5)
				await dialog.Msg("Looking for potions or other useful items? You've come to the right place!");
			else
				await dialog.Msg("Welcome to Alf's shop. What can I get for you today?");

			await dialog.OpenShop("OrshaMiscItems");
		});

		// [Accessory Merchant] Jurus
		//-------------------------------------------------------------------------
		AddNpc(20057, "[Accessory Merchant] Jurus", "Jurus", "c_orsha", 462.1917, -29.93526, -11.0, async dialog =>
		{
			dialog.SetTitle("Jurus");
			dialog.SetPortrait("Dlg_port_Yurrs");

			if (await dialog.Hooks("BeforeDialog"))
				await dialog.Msg("Welcome to my accessory shop. I have some rare and unique items you might be interested in.");
			else
				await dialog.Msg("Greetings! Looking for something to enhance your style and power?");

			await dialog.OpenShop("OrshaAccessories");
		});

		// [Storage Keeper] Aisa
		//-------------------------------------------------------------------------
		AddNpc(20067, "[Storage Keeper] Aisah", "Aisah", "c_orsha", 308, 64, 90.0, async dialog =>
		{
			dialog.SetTitle("Aisah");
			dialog.SetPortrait("Dlg_port_aisah");

			var response = await dialog.Select("Welcome to Orsha's storage. How may I assist you?",
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

		// [Blacksmith] Ilanai
		//-------------------------------------------------------------------------
		AddNpc(2, 20066, "[Blacksmith]{nl}Ilanai", "c_orsha", -133.44, 175.98, -285.69, 73, "ORSHA_BLACKSMITH", "TUTO_REPAIR_NPC", "");

		// [Companion Trader] Toras
		//-------------------------------------------------------------------------
		var toras = AddNpc(20058, "[Companion Trader] Toras", "Toras", "c_orsha", -109.365, 362.765, 104.0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Toras");
			dialog.SetPortrait("Dlg_port_npc_tonus");

			var options = dialog.CreateOptions(
				Option("Adopt Companion", "adopt"),
				Option("Buy Pet Food", "food"),
				Option(ScpArgMsg("shop_companion_learnabil"), "learnabil", () => character.HasCompanions),
				Option(ScpArgMsg("shop_companion_info"), "info"),
				Option("Leave", "leave")
			);

			var selectedOption = await dialog.Select("PETSHOP_ORSHA_basic1", options);

			switch (selectedOption)
			{
				case "adopt":
					await dialog.OpenCustomCompanionShop("TorasCompanions");
					break;
				case "learnabil":
					dialog.OpenAddon(AddonMessage.COMPANION_UI_OPEN);
					break;
				case "info":
					await dialog.Msg("PETSHOP_ORSHA_basic2");
					break;
				case "food":
					await dialog.OpenShop("TorasCompanionFood");
					break;
			}
		});

		toras.AssociatedShopName = "TorasCompanions";
		toras.ShopType = ShopType.Potion;

		// Statue of Goddess Ausrine
		//-------------------------------------------------------------------------
		AddNpc(115, 154063, "Statue of Goddess Ausrine", "c_orsha", 103.14, 176.14, 322.95, -46, "WARP_C_ORSHA", "STOUP_CAMP", "STOUP_CAMP");

		// Emoticon Chest
		//-------------------------------------------------------------------------
		AddPlatformNpc("c_orsha", -423, 489, 92, 0, "green");
		AddPlatformNpc("c_orsha", -423, 529, 92, 0, "blue");
		AddPlatformNpc("c_orsha", -423, 569, 92, 0, "white");
		AddFloatingTreasureChestSpawner("Laima.Treasures.Orsha.Chest1", "c_orsha", -423, 569, 92, 0, ItemId.EmoticonItem_55_58, monsterId: 147393);
	}

	/// <summary>
	/// Creates the misc item shop
	/// </summary>
	private void CreateMiscItemShop()
	{
		CreateShop("OrshaMiscItems", shop =>
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
		CreateShop("OrshaAccessories", shop =>
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
		CreateShop("OrshaWeapons", shop =>
		{
			// One-Handed Swords
			shop.AddItem(101126, amount: 1, price: 400);
			shop.AddItem(101127, amount: 1, price: 3600);
			shop.AddItem(101128, amount: 1, price: 4320);
			shop.AddItem(101124, amount: 1, price: 6370);
			shop.AddItem(101129, amount: 1, price: 7280);
			shop.AddItem(101125, amount: 1, price: 19311);

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
		CreateShop("OrshaArmors", shop =>
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
		CreateShop("TorasCompanionFood", shop =>
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
	/// Creates Toras's companion shop with custom prices
	/// Orsha military city theme: war mounts, battle companions, disciplined animals
	/// </summary>
	private void CreateTorasCompanionShop()
	{
		CreateCompanionShop("TorasCompanions", shop =>
		{
			shop.AddCompanion("Velhider", price: 40000);
			shop.AddCompanion("hoglan_Pet", price: 40000);
			shop.AddCompanion("pet_hawk", price: 40000);

			shop.AddCompanion("parrotbill", price: 110000);
			shop.AddCompanion("Pet_Rocksodon", price: 110000);
			shop.AddCompanion("Penguin", price: 110000);
			shop.AddCompanion("Armadillo", price: 110000);
			shop.AddCompanion("penguin_marine", price: 110000);
			shop.AddCompanion("Lesser_panda_gray", price: 110000);
		});
	}
}
