//--- Melia Script ----------------------------------------------------------
// Collection Merchant
//--- Description -----------------------------------------------------------
// Custom NPC in Klaipeda that sells the new collections for 100,000 Silver.
//---------------------------------------------------------------------------

using Melia.Shared.Data.Database;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class CollectionMerchantNpcScript : GeneralScript
{
	private const int CollectionPrice = 100000;
	private const string ShopName = "CollectionMerchantShop";

	protected override void Load()
	{
		CreateCollectionShop();

		var collectionMerchant = AddNpc(147510, L("[Collection Merchant] Collector"), "Collection Merchant", "c_Klaipe", -190, 390, 0, async dialog =>
		{
			dialog.SetTitle(L("Collection Merchant"));
			dialog.SetPortrait("Dlg_port_TOOL_DEALER");

			await dialog.Msg(L("Welcome! I sell special collections obtained from maps and dungeons. Each one costs 100,000 Silver."));
			await dialog.OpenShop(ShopName);
		});

		collectionMerchant.AssociatedShopName = ShopName;
		collectionMerchant.ShopType = ShopType.Potion;
	}

	private void CreateCollectionShop()
	{
		CreateShop(ShopName, shop =>
		{
			// Dungeon collections
			shop.AddItem(11220112, amount: 1, price: CollectionPrice); // COLLECT_453 - Archmage Tower Dungeon
			shop.AddItem(11220113, amount: 1, price: CollectionPrice); // COLLECT_454 - Catacombs Underground Dungeon
			shop.AddItem(11220114, amount: 1, price: CollectionPrice); // COLLECT_455 - Historic Site Ruins
			shop.AddItem(11220115, amount: 1, price: CollectionPrice); // COLLECT_456 - Monument of Desire Dungeon
			shop.AddItem(11220116, amount: 1, price: CollectionPrice); // COLLECT_457 - Hollow Thorn Forest I
			shop.AddItem(11220117, amount: 1, price: CollectionPrice); // COLLECT_458 - Hollow Thorn Forest II
			shop.AddItem(11220118, amount: 1, price: CollectionPrice); // COLLECT_459 - Blue Fortress Dungeon I
			shop.AddItem(11220119, amount: 1, price: CollectionPrice); // COLLECT_460 - Blue Fortress Dungeon II
			shop.AddItem(11220120, amount: 1, price: CollectionPrice); // COLLECT_461 - Castle Dungeon I
			shop.AddItem(11220121, amount: 1, price: CollectionPrice); // COLLECT_462 - Castle Dungeon II
			shop.AddItem(11220122, amount: 1, price: CollectionPrice); // COLLECT_463 - Earth Tower - Ausura
			shop.AddItem(11220123, amount: 1, price: CollectionPrice); // COLLECT_464 - Earth Tower - Fietas
			shop.AddItem(11220124, amount: 1, price: CollectionPrice); // COLLECT_465 - Nevellet Quarry Dungeon I
			shop.AddItem(11220125, amount: 1, price: CollectionPrice); // COLLECT_466 - Nevellet Quarry Dungeon II
			shop.AddItem(11220126, amount: 1, price: CollectionPrice); // COLLECT_467 - Lanko Lake Dungeon I
			shop.AddItem(11220127, amount: 1, price: CollectionPrice); // COLLECT_468 - Lanko Lake Dungeon II
			shop.AddItem(11220128, amount: 1, price: CollectionPrice); // COLLECT_486 - Earth Tower - Laitas

			// Map collections
			shop.AddItem(11220095, amount: 1, price: CollectionPrice); // COLLECT_469 - Sausis Room 9 I
			shop.AddItem(11220096, amount: 1, price: CollectionPrice); // COLLECT_470 - Sausis Room 9 II
			shop.AddItem(11220097, amount: 1, price: CollectionPrice); // COLLECT_471 - Sausis Room 10 I
			shop.AddItem(11220098, amount: 1, price: CollectionPrice); // COLLECT_472 - Sausis Room 10 II
			shop.AddItem(11220099, amount: 1, price: CollectionPrice); // COLLECT_473 - Valandis Room 2 I
			shop.AddItem(11220100, amount: 1, price: CollectionPrice); // COLLECT_474 - Valandis Room 2 II
			shop.AddItem(11220101, amount: 1, price: CollectionPrice); // COLLECT_475 - Valandis Room 3 I
			shop.AddItem(11220102, amount: 1, price: CollectionPrice); // COLLECT_476 - Valandis Room 3 II
			shop.AddItem(11220103, amount: 1, price: CollectionPrice); // COLLECT_477 - Valandis Room 91
			shop.AddItem(11220104, amount: 1, price: CollectionPrice); // COLLECT_478 - Tavorh Cave
			shop.AddItem(11220105, amount: 1, price: CollectionPrice); // COLLECT_479 - Narcon Prison
			shop.AddItem(11220106, amount: 1, price: CollectionPrice); // COLLECT_480 - Natarh Watchtower
			shop.AddItem(11220107, amount: 1, price: CollectionPrice); // COLLECT_481 - Nazarene Tower
			shop.AddItem(11220108, amount: 1, price: CollectionPrice); // COLLECT_482 - Tatenye Prison
			shop.AddItem(11220109, amount: 1, price: CollectionPrice); // COLLECT_483 - Neighport Church East Building
			shop.AddItem(11220110, amount: 1, price: CollectionPrice); // COLLECT_484 - Sjarejo Chamber
			shop.AddItem(11220111, amount: 1, price: CollectionPrice); // COLLECT_485 - Netanmalek Mausoleum
		});
	}
}
