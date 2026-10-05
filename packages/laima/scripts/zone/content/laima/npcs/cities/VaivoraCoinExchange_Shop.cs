//--- Melia Script ----------------------------------------------------------
// Wings of Vaivora Coin Exchange
//--- Description -----------------------------------------------------------
// NPC that exchanges Wings of Vaivora Coins for cosmetic items.
//---------------------------------------------------------------------------

using System.Linq;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class VaivoraCoinExchangeNpcScript : GeneralScript
{
	private const int WingsOfVaivoraCoinItemId = 647016;
	
	private static readonly ExchangeEntry[] ExchangeItems =
	{
		new ExchangeEntry("Effect_Stamp_Good", "Good Student Stamp", 639101, 1, 500),
		new ExchangeEntry("Effect_Special_Marin", "Lapping Waves", 639102, 1, 500),
		new ExchangeEntry("Effect_Cherry_Blossom", "Blossoms in the Wind", 639105, 1, 500),
		new ExchangeEntry("Effect_Ghost", "Creepy Ghost Party", 639107, 1, 500),
		new ExchangeEntry("Effect_AURORA", "Mysterious Aurora", 639109, 1, 500),
		new ExchangeEntry("Effect_SNOW", "Heavy Snow", 639110, 1, 500),
		new ExchangeEntry("Effect_flutting_rose", "Rose Petal Shower", 639115, 1, 500),
		new ExchangeEntry("Effect_GabiaFire", "Gabija's Fire", 639118, 1, 500),
		new ExchangeEntry("Effect_2019xmas", "Midnight Starlight", 639120, 1, 500),
		new ExchangeEntry("Effect_12animal_halo", "Guardian Halo", 639121, 1, 500),
		new ExchangeEntry("Effect_Stamp_Good_A", "Good Student (A++) Stamp", 639129, 1, 500),
		new ExchangeEntry("Effect_SnowFlower", "White Snowflake Crystal", 640000, 1, 500),
		new ExchangeEntry("Effect_littleprince", "Pilot's Dream", 11009001, 1, 500),
		new ExchangeEntry("Effect_ep12tactical01", "Police Line", 11009004, 1, 500),
		new ExchangeEntry("Effect_ep12tactical02", "Emergency Siren", 11009005, 1, 500),
		new ExchangeEntry("Effect_ep12summer01", "Lurking Shark", 11009006, 1, 500),
		new ExchangeEntry("Effect_ep12summer02", "Honking Seagulls", 11009007, 1, 500),
		new ExchangeEntry("Effect_ep13cybersoldier", "Hologram Screen", 11009012, 1, 500),
		new ExchangeEntry("Effect_ep13toswinter", "Dancing Leaves on Breeze", 11009013, 1, 500),
		new ExchangeEntry("Effect_ep13mafia", "Wanted Poster", 11009015, 1, 500),
		new ExchangeEntry("Effect_ep13stem", "STEM Planetary Orbits", 11009016, 1, 500),
		new ExchangeEntry("Effect_ep13raincoat", "Toddle Waddle Ducklings", 11009018, 1, 500),
		new ExchangeEntry("Effect_ep12demonlord_violet", "Violet Twinkling Steps", 11009026, 1, 500),
		new ExchangeEntry("Effect_ep15snowknight_aurora", "Lofty Snow Aurora", 11106001, 1, 500),
		new ExchangeEntry("Effect_ep15snowknight_snowflake", "Lofty Snowflake Sprinkle", 11106002, 1, 500),
		new ExchangeEntry("Effect_ep15spring01", "Spring Fluff Bouncy Bunny", 11106004, 1, 500),
		new ExchangeEntry("Effect_ep15unicorn", "Sparkle Unicorn Heart", 11106005, 1, 500),
	};

	protected override void Load()
	{
		PropertyShops.Create(
			"VaivoraCoinCostumeShop",
			"vaivora_coin_costume",
			WingsOfVaivoraCoinItemId,
			shop =>
			{
				foreach (var entry in ExchangeItems)
				{
					shop.AddItem(
						entry.ClassName,
						entry.ItemId,
						entry.Amount,
						entry.CoinCost);
				}
			});

		Dialog.RegisterPropertyShopForMap("c_Klaipe","VaivoraCoinCostumeShop","GET_PVP_POINT");

		AddNpc(150257, L("[Vaivora Coin Costume Exchange] Renan"), "Renan", "c_Klaipe", -500, 895, 0, async dialog =>
			{
				dialog.SetTitle(L("Vaivora Coin Costume Exchange"));

				await dialog.Msg(
					L("Exchange your Wings of Vaivora Coins for exclusive cosmetics."));

				dialog.OpenPropertyShop("VaivoraCoinCostumeShop", string.Empty, "uphill_defense_shoppoint", "MercenaryWingShop");
			});
	}

	private sealed class ExchangeEntry
	{
		public string ClassName { get; }

		public string Name { get; }

		public int ItemId { get; }

		public int Amount { get; }

		public int CoinCost { get; }

		public ExchangeEntry(
			string className,
			string name,
			int itemId,
			int amount,
			int coinCost)
		{
			ClassName = className;
			Name = name;
			ItemId = itemId;
			Amount = amount;
			CoinCost = coinCost;
		}
	}
}
