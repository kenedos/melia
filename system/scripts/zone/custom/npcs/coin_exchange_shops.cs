//--- Melia Script ----------------------------------------------------------
// Coin Exchange Shops
//--- Description -----------------------------------------------------------
// Klaipeda shops that trade Talt, Wings of Vaivora Coins and Golden Coins
// for costumes, recipes and utility items, plus a silver utility shop.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomCoinExchangeShopsScript : GeneralScript
{
	private const string PointScript = "GET_PVP_POINT";

	protected override void Load()
	{
		if (!Feature.IsEnabled("CustomNpcs"))
			return;

		this.CreateUtilityShop();
		this.AddExchangeShop("TaltExchangeShop", "talt_exchange_shop", ItemId.Misc_Talt, 57224, "Talt Exchange", "Ana", -680, 650, 0, L("Exchange your Talt for contact lenses."), shop =>
		{
			shop.AddItem("LENS01_001", 18001, 1, 200); // Yellow Contact Lenses
			shop.AddItem("LENS01_002", 18002, 1, 200); // Violet Contact Lenses
			shop.AddItem("LENS01_003", 18003, 1, 200); // Crimson Contact Lenses
			shop.AddItem("LENS01_004", 18004, 1, 200); // Black Contact Lenses
			shop.AddItem("LENS01_005", 18005, 1, 200); // Pink Heart Contact Lenses
			shop.AddItem("LENS01_006", 18006, 1, 200); // Shiny Contact Lenses
			shop.AddItem("LENS01_007", 18007, 1, 200); // Grey Contact Lenses
			shop.AddItem("LENS01_008", 18008, 1, 200); // Orange Contact Lenses
			shop.AddItem("LENS01_009", 18009, 1, 200); // Brown Contact Lenses
			shop.AddItem("LENS01_011", 18011, 1, 200); // Twinkle Purple Lenses
			shop.AddItem("LENS01_012", 18012, 1, 200); // Twinkle Forest Lenses
			shop.AddItem("LENS01_013", 18013, 1, 200); // Amber Cross Lenses
			shop.AddItem("LENS01_014", 18014, 1, 200); // Blue Drop Lenses
			shop.AddItem("LENS01_015", 18015, 1, 200); // Scarlet Lens
			shop.AddItem("LENS01_016", 18016, 1, 200); // Emerald Lens
			shop.AddItem("LENS01_017", 18017, 1, 200); // Cat Eye Blue Lense
			shop.AddItem("LENS01_018", 18018, 1, 200); // Cat Eye Brown Lense
			shop.AddItem("LENS01_019", 18023, 1, 200); // Fenrir Lense
			shop.AddItem("LENS01_010_KOR", 18024, 1, 200); // Blue Lense
			shop.AddItem("LENS01_020", 18025, 1, 200); // Holy Pink Lense
		});

		this.AddExchangeShop("TaltUtilityShop", "talt_utility_shop", ItemId.Misc_Talt, 57225, "Talt Utility Exchange", "Thiago", -720, 610, 0, L("Exchange your Talt for materials, recipes and utility items."), shop =>
		{
			shop.AddItem("Premium_indunReset", 490030, 1, 10); // Instanced Dungeon Reset Voucher
			shop.AddItem("161215Event_Seed", 641926, 1, 10); // Miracle Seeds
			shop.AddItem("Event_Goddess_Statue_DLC", 641945, 1, 10); // Goddess Sculpture
			shop.AddItem("misc_ore15", 649014, 1, 250); // Practonium
			shop.AddItem("R_TOP04_133", 948022, 1, 250); // Recipe - Laitas Robe
			shop.AddItem("R_TOP04_134", 948023, 1, 250); // Recipe - Fietas Leather Armor
			shop.AddItem("R_TOP04_135", 948024, 1, 250); // Recipe - Ausura Plate Armor
			shop.AddItem("R_LEG04_133", 948025, 1, 250); // Recipe - Laitas Pants
			shop.AddItem("R_LEG04_134", 948026, 1, 250); // Recipe - Fietas Leather Pants
			shop.AddItem("R_LEG04_135", 948027, 1, 250); // Recipe - Ausura Plate Pants
			shop.AddItem("R_FOOT04_136", 948028, 1, 250); // Recipe - Laitas Boots
			shop.AddItem("R_FOOT04_137", 948029, 1, 250); // Recipe - Fietas Leather Boots
			shop.AddItem("R_FOOT04_138", 948030, 1, 250); // Recipe - Ausura Greaves
			shop.AddItem("R_HAND04_134", 948031, 1, 250); // Recipe - Laitas Gloves
			shop.AddItem("R_HAND04_135", 948032, 1, 250); // Recipe - Fietas Leather Gloves
			shop.AddItem("R_HAND04_136", 948033, 1, 250); // Recipe - Ausura Gauntlets
			shop.AddItem("NECK02_130", 582130, 1, 20); // Fyrmes Necklace
			shop.AddItem("NECK02_131", 582131, 1, 20); // Predji Necklace
			shop.AddItem("BRC02_124", 602124, 1, 10); // Fyrmes Bracelet
			shop.AddItem("BRC02_125", 602125, 1, 10); // Predji Bracelet
			shop.AddItem("R_TBW03_120", 924090, 1, 125); // Recipe - Aufgowle Bow
			shop.AddItem("R_BOW03_202", 925085, 1, 125); // Recipe - Silver Hawk
			shop.AddItem("R_MAC03_204", 926090, 1, 125); // Recipe - Vienarazis Mace
			shop.AddItem("R_PST03_101", 930011, 1, 125); // Recipe - Double Stack
			shop.AddItem("R_CAN03_101", 947011, 1, 125); // Recipe - Lionhead Cannon
			shop.AddItem("R_MUS03_104", 931007, 1, 125); // Recipe - Dragoon Piper
			shop.AddItem("R_TMAC03_106", 942030, 1, 125); // Recipe - Vienarazis Two-handed Mace
			shop.AddItem("R_SWD03_120", 920096, 1, 125); // Recipe - Pierene Sword
			shop.AddItem("R_TSW03_120", 921085, 1, 125); // Recipe - Gale Slasher
			shop.AddItem("R_STF03_120", 922088, 1, 125); // Recipe - Windia Rod
			shop.AddItem("R_SHD03_110", 941057, 1, 125); // Recipe - Lionhead Shield
			shop.AddItem("R_SPR03_115", 927070, 1, 125); // Recipe - Pygry Spear
			shop.AddItem("R_TSP03_115", 928060, 1, 125); // Recipe - Sacmet
			shop.AddItem("R_DAG03_105", 942014, 1, 125); // Recipe - Lionhead Dagger
			shop.AddItem("R_TSF03_120", 923089, 1, 125); // Recipe - Vienarazis Staff
			shop.AddItem("R_RAP03_305", 929022, 1, 125); // Recipe - Elga Rapier
			shop.AddItem("R_TBW04_109", 924091, 1, 250); // Recipe - Astra Bow
			shop.AddItem("R_BOW04_109", 925086, 1, 250); // Recipe - Regard Horn Crossbow
			shop.AddItem("R_TMAC04_101", 942031, 1, 250); // Recipe - Skull Breaker
			shop.AddItem("R_MAC04_111", 926091, 1, 250); // Recipe - Skull Smasher
			shop.AddItem("R_TSF04_109", 923090, 1, 250); // Recipe - Regard Horn Staff
			shop.AddItem("R_PST04_104", 930012, 1, 250); // Recipe - Aspana Revolver
			shop.AddItem("R_CAN04_103", 947012, 1, 250); // Recipe - Emengard Cannon
			shop.AddItem("R_MUS04_103", 931008, 1, 250); // Recipe - Emengard Musket
			shop.AddItem("R_TSW04_109_Event", 942018, 1, 250); // Artisan Recipe - Sarkmis
			shop.AddItem("R_SWD04_109", 920097, 1, 250); // Recipe - Abdochar
			shop.AddItem("R_STF04_110", 922089, 1, 250); // Recipe - Heart of Glory
			shop.AddItem("R_SHD04_105", 941058, 1, 250); // Recipe - Emengard Shield
			shop.AddItem("R_SPR04_110", 927071, 1, 250); // Recipe - Wingshard Spear
			shop.AddItem("R_TSP04_111", 928061, 1, 250); // Recipe - Regard Horn Pike
			shop.AddItem("R_DAG04_104", 942015, 1, 250); // Recipe - Emengard Dagger
			shop.AddItem("R_RAP04_106", 929023, 1, 250); // Recipe - Black Horn
		});

		this.AddExchangeShop("VaivoraCoinCostumeShop", "vaivora_coin_costume", ItemId.Misc_0533, 150257, "Vaivora Coin Costume Exchange", "Renan", -500, 895, 0, L("Exchange your Wings of Vaivora Coins for exclusive cosmetics."), shop =>
		{
			shop.AddItem("Effect_Stamp_Good", 639101, 1, 500); // Good Student Stamp
			shop.AddItem("Effect_Special_Marin", 639102, 1, 500); // Lapping Waves
			shop.AddItem("Effect_Cherry_Blossom", 639105, 1, 500); // Blossoms in the Wind
			shop.AddItem("Effect_Ghost", 639107, 1, 500); // Creepy Ghost Party
			shop.AddItem("Effect_AURORA", 639109, 1, 500); // Mysterious Aurora
			shop.AddItem("Effect_SNOW", 639110, 1, 500); // Heavy Snow
			shop.AddItem("Effect_flutting_rose", 639115, 1, 500); // Rose Petal Shower
			shop.AddItem("Effect_GabiaFire", 639118, 1, 500); // Gabija's Fire
			shop.AddItem("Effect_2019xmas", 639120, 1, 500); // Midnight Starlight
			shop.AddItem("Effect_12animal_halo", 639121, 1, 500); // Guardian Halo
			shop.AddItem("Effect_Stamp_Good_A", 639129, 1, 500); // Good Student (A++) Stamp
			shop.AddItem("Effect_SnowFlower", 640000, 1, 500); // White Snowflake Crystal
			shop.AddItem("Effect_littleprince", 11009001, 1, 500); // Pilot's Dream
			shop.AddItem("Effect_ep12tactical01", 11009004, 1, 500); // Police Line
			shop.AddItem("Effect_ep12tactical02", 11009005, 1, 500); // Emergency Siren
			shop.AddItem("Effect_ep12summer01", 11009006, 1, 500); // Lurking Shark
			shop.AddItem("Effect_ep12summer02", 11009007, 1, 500); // Honking Seagulls
			shop.AddItem("Effect_ep13cybersoldier", 11009012, 1, 500); // Hologram Screen
			shop.AddItem("Effect_ep13toswinter", 11009013, 1, 500); // Dancing Leaves on Breeze
			shop.AddItem("Effect_ep13mafia", 11009015, 1, 500); // Wanted Poster
			shop.AddItem("Effect_ep13stem", 11009016, 1, 500); // STEM Planetary Orbits
			shop.AddItem("Effect_ep13raincoat", 11009018, 1, 500); // Toddle Waddle Ducklings
			shop.AddItem("Effect_ep12demonlord_violet", 11009026, 1, 500); // Violet Twinkling Steps
			shop.AddItem("Effect_ep15snowknight_aurora", 11106001, 1, 500); // Lofty Snow Aurora
			shop.AddItem("Effect_ep15snowknight_snowflake", 11106002, 1, 500); // Lofty Snowflake Sprinkle
			shop.AddItem("Effect_ep15spring01", 11106004, 1, 500); // Spring Fluff Bouncy Bunny
			shop.AddItem("Effect_ep15unicorn", 11106005, 1, 500); // Sparkle Unicorn Heart
		});

		this.AddExchangeShop("VaivoraCoinUtilityShop", "vaivora_coin_utility", ItemId.Misc_0533, 150257, "Vaivora Coin Utility Exchange", "Artur", -540, 895, 0, L("Exchange your Wings of Vaivora Coins for useful items."), shop =>
		{
			shop.AddItem("Premium_indunReset", 490030, 1, 2); // Instanced Dungeon Reset Voucher
			shop.AddItem("161215Event_Seed", 641926, 1, 5); // Miracle Seeds
			shop.AddItem("Event_Goddess_Statue_DLC", 641945, 1, 5); // Goddess Sculpture
			shop.AddItem("Premium_AddSkillPoint", 494155, 1, 20); // Skill Point Potion
			shop.AddItem("misc_ore15", 649014, 1, 175); // Practonium
			shop.AddItem("R_NECK03_121", 943075, 1, 100); // Recipe - Manosierdi Necklace
			shop.AddItem("R_NECK03_120", 943074, 1, 100); // Recipe - Atikha Necklace
			shop.AddItem("R_NECK03_119", 943073, 1, 100); // Recipe - Svijes Necklace
			shop.AddItem("R_NECK03_118", 943072, 1, 100); // Recipe - Mejstra Necklace
			shop.AddItem("R_BRC03_118", 943076, 1, 50); // Recipe - Mejstra Bracelet
			shop.AddItem("R_BRC03_119", 943077, 1, 50); // Recipe - Svijes Bracelet
			shop.AddItem("R_BRC03_121", 943078, 1, 50); // Recipe - Atikha Bracelet
			shop.AddItem("R_BRC03_122", 943079, 1, 50); // Recipe - Manosierdi Bracelet
		});

		this.AddExchangeShop("WorldBossExchangeShop", "world_boss_golden_coin", ItemId.Event_Steam_Night_Market_Gold, 151072, "World Boss Exchange", "Igor", -650, 750, 0, L("Exchange your Golden Coins for headgear."), shop =>
		{
			shop.AddItem("Hat_628001", 628001, 1, 10); // Cat Ears
			shop.AddItem("Hat_628010", 628010, 1, 10); // Goggles
			shop.AddItem("Hat_628012", 628012, 1, 10); // Lepusbunny Headband
			shop.AddItem("Hat_628013", 628013, 1, 10); // Ginkgo Leaf
			shop.AddItem("Hat_628014", 628014, 1, 10); // Tiger Swallowtail
			shop.AddItem("Hat_628015", 628015, 1, 10); // Eagle Feather
			shop.AddItem("Hat_628016", 628016, 1, 10); // Muscharia Hat
			shop.AddItem("Hat_628017", 628017, 1, 10); // Crown
			shop.AddItem("Hat_628018", 628018, 1, 10); // White Flower Hairpin
			shop.AddItem("Hat_628019", 628019, 1, 10); // Tiger Mask
			shop.AddItem("Hat_628020", 628020, 1, 10); // Tini Doll
			shop.AddItem("Hat_628021", 628021, 1, 10); // Wing Helmet
			shop.AddItem("Hat_628022", 628022, 1, 10); // Wing Decoration
			shop.AddItem("Hat_628023", 628023, 1, 10); // Nesting Egg
			shop.AddItem("Hat_628024", 628024, 1, 10); // Nesting Chick
			shop.AddItem("Hat_628025", 628025, 1, 10); // Nesting Chicken
			shop.AddItem("Hat_628026", 628026, 1, 10); // Campaign Hat
			shop.AddItem("Hat_628027", 628027, 1, 10); // Gentleman's Dignity
			shop.AddItem("Hat_628028", 628028, 1, 10); // Stylish Glasses
			shop.AddItem("Hat_628029", 628029, 1, 10); // Ice Bag
			shop.AddItem("Hat_628030", 628030, 1, 10); // Rosa Rugosa
			shop.AddItem("Hat_628031", 628031, 1, 10); // Feather Hat
			shop.AddItem("Hat_628032", 628032, 1, 10); // Maple Leaf
			shop.AddItem("Hat_628033", 628033, 1, 10); // Red Tassel Hat
			shop.AddItem("Hat_628034", 628034, 1, 10); // Nurse Headband
			shop.AddItem("Hat_628035", 628035, 1, 10); // Kateen Flower Decoration
			shop.AddItem("Hat_628036", 628036, 1, 10); // Dog Ears
			shop.AddItem("Hat_628037", 628037, 1, 10); // Cow Headband
			shop.AddItem("Hat_628038", 628038, 1, 10); // Small Lion Mask
			shop.AddItem("Hat_628039", 628039, 1, 10); // Big Lion Mask
			shop.AddItem("Hat_628040", 628040, 1, 10); // Tengu Mask
			shop.AddItem("Hat_628041", 628041, 1, 10); // Fox Mask
			shop.AddItem("Hat_628042", 628042, 1, 10); // Glasses from the Otherworld
			shop.AddItem("Hat_628043", 628043, 1, 10); // Goat Horns
			shop.AddItem("Hat_628044", 628044, 1, 10); // Demon Wings
			shop.AddItem("Hat_628045", 628045, 1, 10); // Silver Tiara
			shop.AddItem("Hat_628046", 628046, 1, 10); // Bear Eyemask
			shop.AddItem("Hat_628047", 628047, 1, 10); // Symbol of Wealth
			shop.AddItem("Hat_628048", 628048, 1, 10); // Golden Rabbit Hairpin
			shop.AddItem("Hat_628049", 628049, 1, 10); // Antler Horns
			shop.AddItem("Hat_628050", 628050, 1, 10); // Officer Helmet
			shop.AddItem("Hat_628051", 628051, 1, 10); // Gilt Helmet
			shop.AddItem("Hat_628052", 628052, 1, 10); // Steel Helmet
			shop.AddItem("Hat_628053", 628053, 1, 10); // Black Rabbit Ears
			shop.AddItem("Hat_628054", 628054, 1, 10); // Kepa Doll
			shop.AddItem("Hat_628055", 628055, 1, 10); // Pink Bunny
			shop.AddItem("Hat_628056", 628056, 1, 10); // Stake
			shop.AddItem("Hat_628057", 628057, 1, 10); // White Tiger Mask
			shop.AddItem("Hat_628058", 628058, 1, 10); // Glasses
			shop.AddItem("Hat_628059", 628059, 1, 10); // Unicorn Horn
			shop.AddItem("Hat_628060", 628060, 1, 10); // Red Horns
			shop.AddItem("Hat_628061", 628061, 1, 10); // Desert Chupacabra Ears
			shop.AddItem("Hat_628062", 628062, 1, 10); // Mirtis Helmet
			shop.AddItem("Hat_628063", 628063, 1, 10); // Red Devil Wings
			shop.AddItem("Hat_628064", 628064, 1, 10); // Angel Wings
			shop.AddItem("Hat_628065", 628065, 1, 10); // Fallen Angel Wings
			shop.AddItem("Hat_628066", 628066, 1, 10); // Maid Headband
			shop.AddItem("Hat_628067", 628067, 1, 10); // Miao Hair Ornament
			shop.AddItem("Hat_628069", 628069, 1, 10); // Luxury Hairpin
			shop.AddItem("Hat_628070", 628070, 1, 10); // Feather Helmet
			shop.AddItem("Hat_628071", 628071, 1, 10); // Shaman Mask
			shop.AddItem("Hat_628072", 628072, 1, 10); // Sprout
			shop.AddItem("Hat_628073", 628073, 1, 10); // Fried Egg
			shop.AddItem("Hat_628074", 628074, 1, 10); // Tribal Hair Ornament
			shop.AddItem("Hat_628075", 628075, 1, 10); // Party Cone
			shop.AddItem("Hat_628076", 628076, 1, 10); // Cat Hairpin
			shop.AddItem("Hat_628077", 628077, 1, 10); // Imperial Helm
			shop.AddItem("Hat_628078", 628078, 1, 10); // Ball from Another World
			shop.AddItem("Hat_628079", 628079, 1, 10); // Burdensome Ribbon
			shop.AddItem("Hat_628080", 628080, 1, 10); // Paper Crane
			shop.AddItem("Hat_628081", 628081, 1, 10); // Ghost Headband
			shop.AddItem("Hat_628082", 628082, 1, 10); // Square Black Hat
			shop.AddItem("Hat_628083", 628083, 1, 10); // Carnival Mask
			shop.AddItem("Hat_628084", 628084, 1, 10); // Peacock Feather
			shop.AddItem("Hat_628086", 628086, 1, 10); // Pineapple
			shop.AddItem("Hat_628087", 628087, 1, 10); // Desert Outlaw
			shop.AddItem("Hat_628088", 628088, 1, 10); // Clown Cap
			shop.AddItem("Hat_628089", 628089, 1, 10); // Owl
			shop.AddItem("Hat_628090", 628090, 1, 10); // Crown Headband
			shop.AddItem("Hat_628091", 628091, 1, 10); // Pudding
			shop.AddItem("Hat_628092", 628092, 1, 10); // Bride Coronet
			shop.AddItem("Hat_628093", 628093, 1, 10); // Flower Branch
			shop.AddItem("Hat_628094", 628094, 1, 10); // Polka Dot Ribbon
			shop.AddItem("Hat_628095", 628095, 1, 10); // Blue Ribbon
			shop.AddItem("Hat_628096", 628096, 1, 10); // Mint Chocolate Cupcake
			shop.AddItem("Hat_628097", 628097, 1, 10); // Strawberry Cupcake
			shop.AddItem("Hat_628098", 628098, 1, 10); // Lama
			shop.AddItem("Hat_628099", 628099, 1, 10); // Mergen Hat
			shop.AddItem("Hat_628101", 628101, 1, 10); // Decorative Ribbon Hat
			shop.AddItem("Hat_628102", 628102, 1, 10); // White Ribbon Decoration Boater
			shop.AddItem("Hat_628103", 628103, 1, 10); // Red Ribbon Decoration Boater
			shop.AddItem("Hat_628104", 628104, 1, 10); // Winter Hat
			shop.AddItem("Hat_628106", 628106, 1, 10); // Small Brim Feather Hat
			shop.AddItem("Hat_628107", 628107, 1, 10); // Boater
			shop.AddItem("Hat_628108", 628108, 1, 10); // Luxury Hat and Rose
			shop.AddItem("Hat_628109", 628109, 1, 10); // Noble Hat
			shop.AddItem("Hat_628110", 628110, 1, 10); // Dual Color Boater
			shop.AddItem("Hat_628111", 628111, 1, 10); // Outdoors Hat
			shop.AddItem("Hat_628112", 628112, 1, 10); // Beret (Red Artist Hat)
			shop.AddItem("Hat_628113", 628113, 1, 10); // Sailor Cap
			shop.AddItem("Hat_628114", 628114, 1, 10); // Blue Sailor Cap
			shop.AddItem("Hat_628115", 628115, 1, 10); // Noble Flower Bonnet
			shop.AddItem("Hat_628116", 628116, 1, 10); // Dignity Miter
			shop.AddItem("Hat_628117", 628117, 1, 10); // Prestige Miter
			shop.AddItem("Hat_628118", 628118, 1, 10); // Imposing Miter
			shop.AddItem("Hat_628119", 628119, 1, 10); // Maid Hair Accessory
			shop.AddItem("Hat_628120", 628120, 1, 10); // Ceremonial Officer Cap
			shop.AddItem("Hat_628123", 628123, 1, 10); // Tribal Chief Head Decoration
			shop.AddItem("Hat_628124", 628124, 1, 10); // Canine Head Decoration
			shop.AddItem("Hat_628125", 628125, 1, 10); // Ancient Crown
			shop.AddItem("Hat_628127", 628127, 1, 10); // Ceremonial Veil
			shop.AddItem("Hat_628128", 628128, 1, 10); // Ritual Veil
			shop.AddItem("Hat_628129", 628129, 1, 10); // Rosy Bowler
			shop.AddItem("Hat_628131", 628131, 1, 10); // Ritual Headdress
			shop.AddItem("Hat_628132", 628132, 1, 10); // White Flower Headgear
			shop.AddItem("Hat_628133", 628133, 1, 10); // Red Flower Headgear
			shop.AddItem("Hat_628134", 628134, 1, 10); // Event Balloon
			shop.AddItem("Hat_628135", 628135, 1, 10); // Cat's Feet Handband
			shop.AddItem("Hat_628136", 628136, 1, 10); // Major Spade Hat
			shop.AddItem("Hat_628137", 628137, 1, 10); // Diamond Magician Hat
			shop.AddItem("Hat_628138", 628138, 1, 10); // Queen Diamond's Tiara
			shop.AddItem("Hat_628139", 628139, 1, 10); // Sailor Heart Hat
			shop.AddItem("Hat_628140", 628140, 1, 10); // Clover Feather Hat
			shop.AddItem("Hat_628141", 628141, 1, 10); // White Bunny Hairband
			shop.AddItem("Hat_628142", 628142, 1, 10); // Red-feathered Corolla
			shop.AddItem("Hat_628143", 628143, 1, 10); // Pink-feathered Corolla
			shop.AddItem("Hat_628144", 628144, 1, 10); // Beach Fedora
			shop.AddItem("Hat_628145", 628145, 1, 10); // Huckleberry Hair Accessory
			shop.AddItem("Hat_628146", 628146, 1, 10); // Sailor Hat
			shop.AddItem("Hat_628147", 628147, 1, 10); // Polka Dot Lime Ribbon
			shop.AddItem("Hat_628148", 628148, 1, 10); // Snorkeling Mask
			shop.AddItem("Hat_628149", 628149, 1, 10); // Swimming Goggles
			shop.AddItem("Hat_628150", 628150, 1, 10); // Beach Straw Hat
			shop.AddItem("Hat_628151", 628151, 1, 10); // Aloha Flower Crown
			shop.AddItem("Hat_628152", 628152, 1, 10); // Butterfly Hat
			shop.AddItem("Hat_628153", 628153, 1, 10); // Lotus Cup Hair Accessory
			shop.AddItem("Hat_628154", 628154, 1, 10); // Hatter's Cake Hat
			shop.AddItem("Hat_628155", 628155, 1, 10); // Parfait and Mouse Ear Headband
			shop.AddItem("Hat_628156", 628156, 1, 10); // Dodo Hair Accessory
			shop.AddItem("Hat_628157", 628157, 1, 10); // Chesirecat Hair Accessory
			shop.AddItem("Hat_628158", 628158, 1, 10); // White Rabbit Hair Accessory
			shop.AddItem("Hat_628159", 628159, 1, 10); // Alice's Key Hair Accessory
			shop.AddItem("Hat_628173", 628173, 1, 10); // Blue Guarding Cap
			shop.AddItem("Hat_628174", 628174, 1, 10); // Maid Ribbon
			shop.AddItem("Hat_628180", 628180, 1, 10); // Flarestone Horn
			shop.AddItem("Hat_628181", 628181, 1, 10); // Flamingo
			shop.AddItem("Hat_628182", 628182, 1, 10); // Hamster
			shop.AddItem("Hat_628183", 628183, 1, 10); // TestHairAccessory24
			shop.AddItem("Hat_628185", 628185, 1, 10); // Black Feather Ornament
			shop.AddItem("Hat_628187", 628187, 1, 10); // Female Traditional Hair Ornament
			shop.AddItem("Hat_628188", 628188, 1, 10); // TestHairAccessory29
			shop.AddItem("Hat_628189", 628189, 1, 10); // Heart Hairpin
			shop.AddItem("Hat_628190", 628190, 1, 10); // Queen Hat
			shop.AddItem("Hat_628191", 628191, 1, 10); // Lightning Cloud
			shop.AddItem("Hat_628192", 628192, 1, 10); // Menu
			shop.AddItem("Hat_628193", 628193, 1, 10); // Paper Ticket
			shop.AddItem("Hat_628194", 628194, 1, 10); // Rain Cloud
			shop.AddItem("Hat_628195", 628195, 1, 10); // Pancake
			shop.AddItem("Hat_628196", 628196, 1, 10); // Penguin
			shop.AddItem("Hat_628197", 628197, 1, 10); // Peach-colored Rose Horn
			shop.AddItem("Hat_628198", 628198, 1, 10); // Pirate Hat
			shop.AddItem("Hat_628199", 628199, 1, 10); // Purple Flower Headband
			shop.AddItem("Hat_628200", 628200, 1, 10); // Queenly Crown
			shop.AddItem("Hat_628201", 628201, 1, 10); // Radishu
			shop.AddItem("Hat_628202", 628202, 1, 10); // Red Flower Headband
			shop.AddItem("Hat_628203", 628203, 1, 10); // Sky Blue Ribbon
			shop.AddItem("Hat_628204", 628204, 1, 10); // Winding Key
			shop.AddItem("Hat_628205", 628205, 1, 10); // Slid-off Glasses
			shop.AddItem("Hat_628206", 628206, 1, 10); // Sun
			shop.AddItem("Hat_628207", 628207, 1, 10); // Sunflower Seed
			shop.AddItem("Hat_628208", 628208, 1, 10); // Salmon Egg Sushi
			shop.AddItem("Hat_628209", 628209, 1, 10); // Egg Sushi
			shop.AddItem("Hat_628210", 628210, 1, 10); // Shrimp Sushi
			shop.AddItem("Hat_628211", 628211, 1, 10); // Tuna Sushi
			shop.AddItem("Hat_628212", 628212, 1, 10); // Salmon Sushi
			shop.AddItem("Hat_628213", 628213, 1, 10); // Roll
			shop.AddItem("Hat_628214", 628214, 1, 10); // Fried Shrimp
			shop.AddItem("Hat_628215", 628215, 1, 10); // Salmon Egg Roll
			shop.AddItem("Hat_628216", 628216, 1, 10); // Salmon Roll
			shop.AddItem("Hat_628217", 628217, 1, 10); // Avocado Roll
			shop.AddItem("Hat_628218", 628218, 1, 10); // Octopus Roll
			shop.AddItem("Hat_628219", 628219, 1, 10); // Huge Sweat
			shop.AddItem("Hat_628220", 628220, 1, 10); // Toaster
			shop.AddItem("Hat_628221", 628221, 1, 10); // Good Idea!
			shop.AddItem("Hat_628222", 628222, 1, 10); // Rage!
			shop.AddItem("Hat_628223", 628223, 1, 10); // Wi-Fi
			shop.AddItem("Hat_628224", 628224, 1, 10); // Autumn-colored Rose Horn
			shop.AddItem("Hat_628226", 628226, 1, 10); // Banana Peel
			shop.AddItem("Hat_628227", 628227, 1, 10); // Hipster Glasses
			shop.AddItem("Hat_628228", 628228, 1, 10); // Blue Laced Hat
			shop.AddItem("Hat_628229", 628229, 1, 10); // Watery Rose Horn
			shop.AddItem("Hat_628230", 628230, 1, 10); // Buried Carrot
			shop.AddItem("Hat_628231", 628231, 1, 10); // Cloud
			shop.AddItem("Hat_628232", 628232, 1, 10); // Pink Pudding
			shop.AddItem("Hat_628233", 628233, 1, 10); // Black Checker
			shop.AddItem("Hat_628234", 628234, 1, 10); // Black Gold-laced Butterfly Wings
			shop.AddItem("Hat_628235", 628235, 1, 10); // Boss Bear Hat
			shop.AddItem("Hat_628236", 628236, 1, 10); // Red Checker
			shop.AddItem("Hat_628237", 628237, 1, 10); // Golden Rose Horn
			shop.AddItem("Hat_628238", 628238, 1, 10); // Spartan Helmet
			shop.AddItem("Hat_628239", 628239, 1, 10); // Key-Hat
			shop.AddItem("Hat_628240", 628240, 1, 10); // Sky Blue Striped Crown
			shop.AddItem("Hat_628241", 628241, 1, 10); // Navy Blue Featherhat
			shop.AddItem("Hat_628242", 628242, 1, 10); // Sea-themed Headband
			shop.AddItem("Hat_628243", 628243, 1, 10); // Laced Black Hat
			shop.AddItem("Hat_628244", 628244, 1, 10); // Bison Helmet
			shop.AddItem("Hat_628245", 628245, 1, 10); // Ribboned Marne Hat
			shop.AddItem("Hat_628246", 628246, 1, 10); // Angelic Halo
			shop.AddItem("Hat_628247", 628247, 1, 10); // White Key-Hat
			shop.AddItem("Hat_628248", 628248, 1, 10); // Sleeping Leopard
			shop.AddItem("Hat_628249", 628249, 1, 10); // Watery Shell Ornament
			shop.AddItem("Hat_628250", 628250, 1, 10); // Reindeer Crown
			shop.AddItem("Hat_628251", 628251, 1, 10); // Red Pudding Crown
			shop.AddItem("Hat_628252", 628252, 1, 10); // Slacking Sloth
			shop.AddItem("Hat_628253", 628253, 1, 10); // Grey Crown
			shop.AddItem("Hat_628254", 628254, 1, 10); // Puppy Balloon
			shop.AddItem("Hat_628255", 628255, 1, 10); // Horned Hat
			shop.AddItem("Hat_628256", 628256, 1, 10); // Umbrella
			shop.AddItem("Hat_628257", 628257, 1, 10); // Lotus Leaf
			shop.AddItem("Hat_628258", 628258, 1, 10); // Nabe
			shop.AddItem("Hat_628259", 628259, 1, 10); // Bowl
			shop.AddItem("Hat_628260", 628260, 1, 10); // Feather Helmet
			shop.AddItem("Hat_628261", 628261, 1, 10); // Crocus
			shop.AddItem("Hat_628262", 628262, 1, 10); // Carve World Tree
			shop.AddItem("Hat_628263", 628263, 1, 10); // Valorous Ceremonial Hat
			shop.AddItem("Hat_628264", 628264, 1, 10); // Decorated Ceremonial Hat
			shop.AddItem("Hat_628265", 628265, 1, 10); // Tactical Ceremonial Hat
			shop.AddItem("Hat_628266", 628266, 1, 10); // Swift Ceremonial Hat
			shop.AddItem("Hat_628267", 628267, 1, 10); // Serving Ceremonial Hat
			shop.AddItem("Hat_628268", 628268, 1, 10); // Jack-O-Lantern Hair Ornament
			shop.AddItem("Hat_628269", 628269, 1, 10); // Star Hairpin
			shop.AddItem("Hat_628270", 628270, 1, 10); // White Leather Brimmed Hat
			shop.AddItem("Hat_628271", 628271, 1, 10); // Black Leather Brimmed Hat
			shop.AddItem("Hat_628272", 628272, 1, 10); // High Brim Teal Hat
			shop.AddItem("Hat_628273", 628273, 1, 10); // High Brim Orange Hat
			shop.AddItem("Hat_628274", 628274, 1, 10); // Black Western Hat
			shop.AddItem("Hat_628275", 628275, 1, 10); // Pink Western Hat
			shop.AddItem("Hat_628276", 628276, 1, 10); // Smart Short Brim Hat
			shop.AddItem("Hat_628277", 628277, 1, 10); // Smart Outdoors Hat
			shop.AddItem("Hat_628278", 628278, 1, 10); // Drum Hat
			shop.AddItem("Hat_628279", 628279, 1, 10); // Soldier Hat
			shop.AddItem("Hat_628280", 628280, 1, 10); // Strawberry Moon Pie
			shop.AddItem("Hat_628281", 628281, 1, 10); // Bunny Cupcake
			shop.AddItem("Hat_628282", 628282, 1, 10); // Black Crown
			shop.AddItem("Hat_628283", 628283, 1, 10); // White Crown
			shop.AddItem("Hat_628284", 628284, 1, 10); // Rudolph Antler Hat
			shop.AddItem("Hat_628285", 628285, 1, 10); // Bell Ornament Winter Hat
			shop.AddItem("Hat_628286", 628286, 1, 10); // Rudolph Headband
			shop.AddItem("Hat_628287", 628287, 1, 10); // Christmas Tree Snowglobe
			shop.AddItem("Hat_628288", 628288, 1, 10); // Rudolph Hat
			shop.AddItem("Hat_628289", 628289, 1, 10); // Christmas Cone Hat
			shop.AddItem("Hat_628290", 628290, 1, 10); // Pink Rooster Headband
			shop.AddItem("Hat_628291", 628291, 1, 10); // Velcoffer Horns
			shop.AddItem("Hat_628292", 628292, 1, 10); // Velcoffer Wings Ornament
			shop.AddItem("Hat_628293", 628293, 1, 10); // Rooster Hair Accessory
			shop.AddItem("Hat_628294", 628294, 1, 10); // Hen Hair Accessory
			shop.AddItem("Hat_628295", 628295, 1, 10); // Grey Cat Ears
			shop.AddItem("Hat_628296", 628296, 1, 10); // Orange Cat Ears
			shop.AddItem("Hat_628297", 628297, 1, 10); // White Cat Ears
			shop.AddItem("Hat_628298", 628298, 1, 10); // Grey Rabbit Ears
			shop.AddItem("Hat_628299", 628299, 1, 10); // White Rabbit Ears
			shop.AddItem("Hat_628300", 628300, 1, 10); // Orange Rabbit Ears
			shop.AddItem("Hat_628301", 628301, 1, 10); // Picnic Hat
			shop.AddItem("Hat_628302", 628302, 1, 10); // Orange Candy
			shop.AddItem("Hat_628303", 628303, 1, 10); // Heart Crown Cake
			shop.AddItem("Hat_628304", 628304, 1, 10); // Pink Heart Hair Accessory
			shop.AddItem("Hat_628305", 628305, 1, 10); // Pink Candy
			shop.AddItem("Hat_628306", 628306, 1, 10); // Sweet Biscuit
			shop.AddItem("Hat_628307", 628307, 1, 10); // Strawberry Cake
			shop.AddItem("Hat_628308", 628308, 1, 10); // Heart Candy
			shop.AddItem("Hat_628309", 628309, 1, 10); // Toy Paper Ticket
			shop.AddItem("Hat_628310", 628310, 1, 10); // Paper Boat
			shop.AddItem("Hat_628311", 628311, 1, 10); // Golden Pup Doll
			shop.AddItem("Hat_628312", 628312, 1, 10); // Golden Pup Paw Print
			shop.AddItem("Hat_628313", 628313, 1, 10); // Smiley Orange
			shop.AddItem("Hat_628314", 628314, 1, 10); // Sardine Coffee
			shop.AddItem("Hat_628315", 628315, 1, 10); // Spiral Glasses
			shop.AddItem("Hat_628316", 628316, 1, 10); // Cherry Blossom Hairpin
			shop.AddItem("Hat_628317", 628317, 1, 10); // Tricolor Treat Hairpin
			shop.AddItem("Hat_628318", 628318, 1, 10); // Soccer Fan Headband
			shop.AddItem("Hat_628319", 628319, 1, 10); // Marine Beret
			shop.AddItem("Hat_628320", 628320, 1, 10); // Sailor Beret
			shop.AddItem("Hat_628321", 628321, 1, 10); // Blue Anchor
			shop.AddItem("Hat_628322", 628322, 1, 10); // Marine Stripe Glasses
			shop.AddItem("Hat_628323", 628323, 1, 10); // Romantic Marine Scarf
			shop.AddItem("Hat_628324", 628324, 1, 10); // Lighthouse
			shop.AddItem("Hat_628325", 628325, 1, 10); // [PP] Orange Rabbit Ears
			shop.AddItem("Hat_628326", 628326, 1, 10); // Ebony King Crown
			shop.AddItem("Hat_628327", 628327, 1, 10); // Ivory Queen Crown
			shop.AddItem("Hat_628328", 628328, 1, 10); // Bishop Fedora
			shop.AddItem("Hat_628329", 628329, 1, 10); // Ebony Knight
			shop.AddItem("Hat_628330", 628330, 1, 10); // Knight Galea
			shop.AddItem("Hat_628331", 628331, 1, 10); // Chessboard
			shop.AddItem("Hat_628332", 628332, 1, 10); // Ebony Rook
			shop.AddItem("Hat_628333", 628333, 1, 10); // Red Hair Bow
			shop.AddItem("Hat_628334", 628334, 1, 10); // Big Red Hair Bow
			shop.AddItem("Hat_628335", 628335, 1, 10); // Latte Art: Bunny
			shop.AddItem("Hat_628336", 628336, 1, 10); // Latte Art: Kitty
			shop.AddItem("Hat_628337", 628337, 1, 10); // Latte Art: Popolion
			shop.AddItem("Hat_628338", 628338, 1, 10); // Horse Medal
			shop.AddItem("Hat_628339", 628339, 1, 10); // Satgat Hat
			shop.AddItem("Hat_628340", 628340, 1, 10); // Black Gat Hat
			shop.AddItem("Hat_628341", 628341, 1, 10); // Wolf Mask
			shop.AddItem("Hat_628342", 628342, 1, 10); // Black Cat Ears
			shop.AddItem("Hat_628343", 628343, 1, 10); // Pumpkin Hair Accessory
			shop.AddItem("Hat_628344", 628344, 1, 10); // Purple Ribbon Witch Hat
			shop.AddItem("Hat_628345", 628345, 1, 10); // Red Ribbon Witch Hat
			shop.AddItem("Hat_628346", 628346, 1, 10); // Latte Art: Bunny
			shop.AddItem("Hat_628347", 628347, 1, 10); // Nicopolis Guard Hat
			shop.AddItem("Hat_628348", 628348, 1, 10); // Cozy Polar Bear Ears
			shop.AddItem("Hat_628349", 628349, 1, 10); // Elegant Snow Fox Ears
			shop.AddItem("Hat_628350", 628350, 1, 10); // Small Present Hairpin
			shop.AddItem("Hat_628351", 628351, 1, 10); // Small Star Hairpin
			shop.AddItem("Hat_628352", 628352, 1, 10); // Groucho Glasses
			shop.AddItem("Hat_628353", 628353, 1, 10); // Grandpa Groucho Glasses
			shop.AddItem("Hat_628354", 628354, 1, 10); // Neon Popolion
			shop.AddItem("Hat_628355", 628355, 1, 10); // Tantalizer Feelers
			shop.AddItem("Hat_628356", 628356, 1, 10); // Moringponia Hat
			shop.AddItem("Hat_628357", 628357, 1, 10); // Firebird Hair Brooch
			shop.AddItem("Hat_628358", 628358, 1, 10); // Red Hair Knot
			shop.AddItem("Hat_628359", 628359, 1, 10); // Gabija Feather Hairband
			shop.AddItem("Hat_628360", 628360, 1, 10); // Messenger of Fire
			shop.AddItem("Hat_628361", 628361, 1, 10); // Snowy Reindeer Antlers
			shop.AddItem("Hat_628362", 628362, 1, 10); // Snowy Deer Antlers
			shop.AddItem("Hat_628363", 628363, 1, 10); // Jingle Bell Earmuffs
			shop.AddItem("Hat_628364", 628364, 1, 10); // Present Wrapper Ribbon
			shop.AddItem("Hat_628365", 628365, 1, 10); // Christmas Party Hat
			shop.AddItem("Hat_628366", 628366, 1, 10); // Snowman Nose
			shop.AddItem("Hat_628367", 628367, 1, 10); // Santa Beard
			shop.AddItem("Hat_628368", 628368, 1, 10); // Magical Savior Double Ribbon
			shop.AddItem("Hat_628369", 628369, 1, 10); // Magical Savior Pure Ribbon
			shop.AddItem("Hat_628370", 628370, 1, 10); // Magical Savior Brooch Ribbon
			shop.AddItem("Hat_628371", 628371, 1, 10); // Magical Savior Clover Ribbon
			shop.AddItem("Hat_628372", 628372, 1, 10); // Magical Savior Neon Heart
			shop.AddItem("Hat_628373", 628373, 1, 10); // Magical Savior Wing Decoration
			shop.AddItem("Hat_628374", 628374, 1, 10); // Magical Savior Red Frill Ribbon
			shop.AddItem("Hat_628375", 628375, 1, 10); // Magical Savior Spade Ribbon
			shop.AddItem("Hat_628376", 628376, 1, 10); // Popo Pop Tint Sunglasses
			shop.AddItem("Hat_628377", 628377, 1, 10); // Popo Pop Neon Sunglasses
			shop.AddItem("Hat_628378", 628378, 1, 10); // Popo Pop Black Mask
			shop.AddItem("Hat_628379", 628379, 1, 10); // Popo Pop Stage Microphone
			shop.AddItem("Hat_628380", 628380, 1, 10); // Popo Pop Fandom Hairband
			shop.AddItem("Hat_628381", 628381, 1, 10); // Flower Deer's Horn Headband
			shop.AddItem("Hat_628382", 628382, 1, 10); // Blue Magpie Headband
			shop.AddItem("Hat_628383", 628383, 1, 10); // Panda Sleeping Mask
			shop.AddItem("Hat_628384", 628384, 1, 10); // Panda Hairpin
			shop.AddItem("Hat_628343_Trade", 628385, 1, 10); // Pumpkin Hair Accessory
			shop.AddItem("Hat_628386", 628386, 1, 10); // Dragoon Helmet
			shop.AddItem("Hat_628182_NoTrade", 628387, 1, 10); // [Event] Hamster
			shop.AddItem("Hat_628194_NoTrade", 628388, 1, 10); // [Event] Rain Cloud 
			shop.AddItem("Hat_628223_NoTrade", 628389, 1, 10); // [Event] Wi-Fi
			shop.AddItem("Hat_628205_NoTrade", 628390, 1, 10); // [Event] Slid-off Glasses
			shop.AddItem("Hat_628253_NoTrade", 628391, 1, 10); // [Event] Grey Crown
			shop.AddItem("Hat_628079_NoTrade", 628392, 1, 10); // [Event] Burdensome Ribbon
			shop.AddItem("Hat_628295_NoTrade", 628393, 1, 10); // [Event] Grey Cat Ears
			shop.AddItem("Hat_628207_NoTrade", 628394, 1, 10); // [Event] Sunflower Seed
			shop.AddItem("Hat_628193_NoTrade", 628395, 1, 10); // [Event] Paper Ticket
			shop.AddItem("Hat_628094_NoTrade", 628396, 1, 10); // [Event] Polka Dot Ribbon
			shop.AddItem("Hat_628300_NoTrade", 628397, 1, 10); // [Event] Orange Rabbit Ears
			shop.AddItem("Hat_628354_NoTrade", 628398, 1, 10); // [Event] Neon Popolion
			shop.AddItem("Hat_628399", 628399, 1, 10); // [Event] Neon Kepa Gone Bad
			shop.AddItem("Hat_628400", 628400, 1, 10); // [Event] Neon Hanaming
			shop.AddItem("Hat_628401", 628401, 1, 10); // Machiner's Monocle
			shop.AddItem("Hat_628017_1", 628402, 1, 10); // Crown
			shop.AddItem("Hat_628403", 628403, 1, 10); // Mask of Illusionist
			shop.AddItem("Hat_629001", 629001, 1, 10); // War Bonnet
			shop.AddItem("Hat_629002", 629002, 1, 10); // Cornus Helmet
			shop.AddItem("Hat_629003", 629003, 1, 10); // Hanaming Doll
			shop.AddItem("Hat_629004", 629004, 1, 10); // Popolion Doll
			shop.AddItem("steam_Hat_629003", 629005, 1, 10); // Hanaming Doll
			shop.AddItem("steam_Hat_629004", 629006, 1, 10); // Popolion Doll
			shop.AddItem("Hat_629008", 629008, 1, 10); // Premium Snowflake Crown
			shop.AddItem("Hat_629009", 629009, 1, 10); // White Snowflake Unicorn Horn
			shop.AddItem("Hat_629010", 629010, 1, 10); // White Snowflake Glasses
			shop.AddItem("Hat_629011", 629011, 1, 10); // Genealogical Standards Manual
			shop.AddItem("Hat_629012", 629012, 1, 10); // Nerd Glasses
			shop.AddItem("Hat_629013", 629013, 1, 10); // Potted Flower
			shop.AddItem("Hat_629014", 629014, 1, 10); // Fruit Snack Basket
			shop.AddItem("Hat_629015", 629015, 1, 10); // Little Grain of Rice
			shop.AddItem("Hat_629016", 629016, 1, 10); // Golden Fairy Laurel
			shop.AddItem("Hat_629017", 629017, 1, 10); // Golden Fairy Flower Tiara
			shop.AddItem("Hat_629018", 629018, 1, 10); // Dawn Fairy Flower Bud
			shop.AddItem("Hat_629019", 629019, 1, 10); // Pink Fairy Flower Bud
			shop.AddItem("Hat_629020", 629020, 1, 10); // Teal Teeny Hat
			shop.AddItem("Hat_629021", 629021, 1, 10); // Pink Teeny Hat
			shop.AddItem("Hat_629022", 629022, 1, 10); // Pixel Sunglasses
			shop.AddItem("Hat_629023", 629023, 1, 10); // Boeing Sunglasses
			shop.AddItem("Hat_629024", 629024, 1, 10); // Gehong's Hat
			shop.AddItem("Hat_629025", 629025, 1, 10); // Hitomiko's Wreath
			shop.AddItem("Hat_629026", 629026, 1, 10); // Colorful Boeing Sunglasses
			shop.AddItem("Hat_629027", 629027, 1, 10); // Mystic Savior - Heart Ribbon
			shop.AddItem("Hat_629028", 629028, 1, 10); // Mystic Savior - Diamond Ribbon
			shop.AddItem("Hat_629029", 629029, 1, 10); // Baby Chick
			shop.AddItem("Hat_629030", 629030, 1, 10); // Laima Kindergarten Hat
			shop.AddItem("Hat_629031", 629031, 1, 10); // Little Budding Sprout
			shop.AddItem("Hat_629032", 629032, 1, 10); // Blue Twin Ribbons
			shop.AddItem("Hat_629033", 629033, 1, 10); // Honorable Medal Hat
			shop.AddItem("Hat_629034", 629034, 1, 10); // Rose Knight Hat
			shop.AddItem("Hat_629035", 629035, 1, 10); // Elegant Noble Capeline
			shop.AddItem("Hat_629036", 629036, 1, 10); // Twilight Star Head Ornament
			shop.AddItem("Hat_629037", 629037, 1, 10); // Twilight Star Winged Helm
			shop.AddItem("Hat_629038", 629038, 1, 10); // Bubble Tea Accessory
			shop.AddItem("accessary_beachwear01", 629039, 1, 10); // Hibiscus Straw Hat
			shop.AddItem("accessary_beachwear02", 629040, 1, 10); // Lobelia Straw Hat
			shop.AddItem("accessary_beachwear03", 629041, 1, 10); // Ribbon Straw Hat
			shop.AddItem("accessary_beachwear04", 629042, 1, 10); // Cool Shades
			shop.AddItem("accessary_beachwear05", 629043, 1, 10); // Manly Man Beard
			shop.AddItem("accessory_gaviya", 629044, 1, 10); // Gabija Doll Accessory
			shop.AddItem("accessory_travelHat", 629045, 1, 10); // Explorer's Hat
			shop.AddItem("accessory_blackbear_ear", 629046, 1, 10); // Black Bear Ears
			shop.AddItem("Hat_629047", 629047, 1, 10); // Champa Hairpin
			shop.AddItem("Hat_629048", 629048, 1, 10); // Chef Hat
			shop.AddItem("Hat_629049", 629049, 1, 10); // Culinary Hat
			shop.AddItem("Hat_629050", 629050, 1, 10); // Toque Blanche
			shop.AddItem("Hat_629051", 629051, 1, 10); // Head Chef Hat
			shop.AddItem("Hat_629052", 629052, 1, 10); // Fox Spirit Mask
			shop.AddItem("Hat_629053", 629053, 1, 10); // Yellow Beanie
			shop.AddItem("Hat_629054", 629054, 1, 10); // Checkered Hat
			shop.AddItem("Hat_629055", 629055, 1, 10); // TOS Ranger Hairpin
			shop.AddItem("Hat_629056", 629056, 1, 10); // TOS Ranger Glasses
			shop.AddItem("Hat_629057", 629057, 1, 10); // Clown Mask
			shop.AddItem("Hat_629058", 629058, 1, 10); // Butcher Knife Headband
			shop.AddItem("Hat_629059", 629059, 1, 10); // Skull Party Mask
			shop.AddItem("Hat_629060", 629061, 1, 10); // Solcomm Visor
			shop.AddItem("Hat_629062", 629062, 1, 10); // Asiomage's Horn Crown
			shop.AddItem("steam_Hat_629503_ev", 629063, 1, 10); // Leaf Ornament Helmet
			shop.AddItem("Hat_629501", 629501, 1, 10); // Red Beret
			shop.AddItem("Hat_629502", 629502, 1, 10); // Event Hat 2
			shop.AddItem("Hat_629503", 629503, 1, 10); // Leaf Ornament Helmet
			shop.AddItem("EP12_Hat_041", 11006041, 1, 10); // Honored Rose Intelligent Monocle
			shop.AddItem("EVENT_accessory_snowflower", 10300048, 1, 10); // Snowflake Hairpin
		});

		this.AddExchangeShop("WorldBossExchangeUtilityShop", "world_boss_golden_coin_utility", ItemId.Event_Steam_Night_Market_Gold, 151072, "World Boss Utility Exchange", "Felipe", -650, 690, 0, L("Exchange your Golden Coins for useful materials and utility items."), shop =>
		{
			shop.AddItem("misc_ore15", 649014, 1, 15); // Practonium
			shop.AddItem("Old_Socket_Gold_Team", 643032, 1, 15); // Ancient Golden Socket
		});

		AddNpc(147510, L("[Utility Shop] Thomas"), "c_Klaipe", 440, 80, 270, async dialog =>
		{
			dialog.SetTitle(L("Utility Shop"));
			dialog.SetPortrait("Dlg_port_TOOL_DEALER");
			await dialog.Msg(L("Welcome. I sell useful utility items."));
			await dialog.OpenShop("CustomUtilityShop");
		});
	}

	private void AddExchangeShop(string shopName, string pointName, int currencyItemId, int modelId, string title, string npcName, int x, int z, int direction, string greeting, System.Action<PropertyShop> configure)
	{
		PropertyShops.CreateWithItem(shopName, pointName, currencyItemId, configure);
		Dialog.RegisterPropertyShopForMap("c_Klaipe", shopName, PointScript);

		AddNpc(modelId, L($"[{title}] {npcName}"), "c_Klaipe", x, z, direction, async dialog =>
		{
			dialog.SetTitle(L(title));
			await dialog.Msg(greeting);
			dialog.OpenPropertyShop(shopName, null, pointName);
		});
	}

	private void CreateUtilityShop()
	{
		CreateShop("CustomUtilityShop", shop =>
		{
			shop.AddItem(919018, amount: 1, price: 10000); // Recipe - Goddesses' Blessed Gem
			shop.AddItem(646064, amount: 1, price: 1000); // Portal Stone
			shop.AddItem(490015, amount: 1, price: 1000000); // EXP Tome
			shop.AddItem(919013, amount: 1, price: 100000); // Recipe - x4 EXP Tome
			shop.AddItem(919014, amount: 1, price: 100000); // Recipe - x8 EXP Tome
			shop.AddItem(943087, amount: 1, price: 2000000); // Recipe - Frieno Necklace
			shop.AddItem(943086, amount: 1, price: 2000000); // Recipe - Pasiutes Necklace
			shop.AddItem(943084, amount: 1, price: 2000000); // Recipe - Lynnki Sit Necklace
			shop.AddItem(943085, amount: 1, price: 2000000); // Recipe - Kite Moor Necklace
			shop.AddItem(943083, amount: 1, price: 1000000); // Recipe - Frieno Bracelet
			shop.AddItem(943082, amount: 1, price: 1000000); // Recipe - Pasiutes Bracelet
			shop.AddItem(943080, amount: 1, price: 1000000); // Recipe - Lynnki Sit Bracelet
			shop.AddItem(943081, amount: 1, price: 1000000); // Recipe - Kite Moor Bracelet
			shop.AddItem(919022, amount: 1, price: 1000000); // Recipe - Ominous Spirit Crystal
		});
	}
}
