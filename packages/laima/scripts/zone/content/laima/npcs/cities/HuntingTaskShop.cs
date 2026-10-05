//--- Melia Script ----------------------------------------------------------
// Hunting Task Shop
//--- Description -----------------------------------------------------------
// Exchanges Hunting Points for Hunting Task rewards.
//---------------------------------------------------------------------------

using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;

namespace ScriptsZoneLaima.content.laima.npcs.cities
{
	public class HuntingTaskShop : GeneralScript
	{
		public const string ShopName = "HuntingTaskShop";

		protected override void Load()
		{
			PropertyShops.Create(ShopName, "hunting_task_shop", string.Empty, shop =>
			{
				shop.UsesHuntingPoints = true;

				shop.AddItem("Premium_indunReset", 490030, 1, 10);
				shop.AddItem("161215Event_Seed", 641926, 1, 10);
				shop.AddItem("Event_Goddess_Statue_DLC", 641945, 1, 10);
				shop.AddItem("misc_ore15", 649014, 1, 75);
			});

			Dialog.RegisterPropertyShopForMap(
				"c_Klaipe",
				ShopName,
				"GET_PVP_POINT");
		}
	}
}
