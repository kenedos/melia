//--- Melia Script ----------------------------------------------------------
// Fedimian Public House (c_request_1)
//--- Description -----------------------------------------------------------
// NPCs and shops in the Fedimian Mercenary Post public house.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class CRequest1NpcScript : GeneralScript
{
	// Account property that stores the mercenary badge balance. Mirrors the
	// existing SCR_USE_MISC_PVP_MINE2 flow: using misc_pvp_mine2 items adds
	// to this property.
	private const string MercenaryCurrencyProperty = "MISC_PVP_MINE2";

	// Client-side shop-point tag read by GET_PVP_POINT / propertyshop UI.
	private const string MercenaryShopPointName = "uphill_defense_shoppoint";
	private const string MercenaryPointScript = "GET_PVP_POINT";

	// Permanent character flag set once the player has spoken to the guild
	// leader. Without this flag, shopkeepers redirect the player to him.
	public const string MetCaptainVariable = "Laima.MercenaryGuild.MetCaptain";

	// Permanent character variable tracking mythic kills that have not yet
	// been redeemed for mercenary badges.
	public const string MythicKillsVariable = "Laima.Quests.c_request_1.Quest1001.MythicKills";

	public const int MercenaryBadgeItemId = 646076;
	public const int MythicKillsPerReward = 1;
	public const int BadgesPerReward = 50;

	protected override void Load()
	{
		CreateMercenaryArtefactShops();

		AddNpc(147515, L("[Captain] Davros Renn"), "CaptainDavrosRenn",
			"c_request_1", 64, -38, 0, this.CaptainDavrosDialog);

		AddMercenaryShopkeeper(147443, "Blade Mercenary", "BladeMercenary",
			"c_request_1", -33, -154, 90,
			"MercenaryBladeShop",
			L("Steel sings sweetest in a tavern's hush. Which edge calls to you?"),
			L("Names before blades, recruit. Davros vouches for everyone who buys from this rack — go find the captain in the middle of the hall, then come back when your name's on his ledger."));

		AddMercenaryShopkeeper(147343, "Archer Mercenary", "ArcherMercenary",
			"c_request_1", -40, -99, 90,
			"MercenaryArcherShop",
			L("A steady hand and a sharper eye. Bring badges; I'll bring the bowstring."),
			L("{#666666}*She doesn't look up from stringing her bow*{/} Stranger's strings, stranger's hands. Talk to Davros first — he sits up the middle of the room. I don't sell to anyone the captain hasn't read."));

		AddMercenaryShopkeeper(147442, "Wizard Mercenary", "WizardMercenary",
			"c_request_1", -41, -45, 90,
			"MercenaryWizardShop",
			L("Rods hum, staves whisper. Pay the toll and one shall answer for you."),
			L("Mm. The aether around you is unbound — no oath, no thread tying you to this hall. Seek Captain Davros in the heart of the house. Once your name is woven into ours, the rods will deign to answer you."));

		AddMercenaryShopkeeper(57229, "Cleric Mercenary", "ClericMercenary",
			"c_request_1", 27, -145, 180,
			"MercenaryClericShop",
			L("Faith guides the strike, and the shield guides the faithful. Choose wisely."),
			L("Peace, traveler. The order asks one rite of every newcomer: present yourself to Captain Davros first. He keeps the roll. I cannot bless arms for a name not written there."));

		AddMercenaryShopkeeper(151068, "Dragoon Mercenary", "DragoonMercenary",
			"c_request_1", 99, -170, 180,
			"MercenaryDragoonShop",
			L("A spear outreaches every regret. Step closer and I'll show you which."),
			L("Hah! Eager hands, but no introduction. A dragoon's spear answers only to a name on the captain's roll. Davros is at the center of the hall — present yourself to him, and we'll talk steel after."));

		AddMercenaryShopkeeper(57238, "Rogue Mercenary", "RogueMercenary",
			"c_request_1", 165, -58, 270,
			"MercenaryRogueShop",
			L("Quiet work takes a quiet blade. Keep your badges quieter still."),
			L("{#666666}*He doesn't quite look at you*{/} Mm. New face buying sharp things — that's how guards get nervous and contracts get cancelled. Go say hello to Davros first. If you're still here after that, we'll pretend we've never met and start over."));

		AddMercenaryShopkeeper(151071, "Gunner Mercenary", "GunnerMercenary",
			"c_request_1", 154, 16, 0,
			"MercenaryGunnerShop",
			L("Powder, shot, and patience. One of three I can sell you today."),
			L("Hold up. I don't hand out powder to a name I can't find on the captain's slate. Walk yourself over to Davros, get registered, then come back. Last thing this guild needs is an unaccounted muzzle flash."));

		AddMercenaryShopkeeper(160144, "Fashion Mercenary", "CostumeMaleMercenary",
			"c_request_1", 52, 124, 0,
			"MercenaryMaleCostumeShop",
			L("Off-duty hunters need off-duty clothes. Gentlemen's cuts only on this rack — three thousand badges each, and every piece has been shaken out of a mythic's lair."),
			L("A tailor sells to names, not strangers. Get yourself on Captain Davros' ledger first, then we'll talk about what suits you."));

		AddMercenaryShopkeeper(160194, "Fashion Mercenary", "CostumeFemaleMercenary",
			"c_request_1", 108, 129, 0,
			"MercenaryFemaleCostumeShop",
			L("Ladies' cuts, recovered from the scholars' vault in exchange for hunter's work. Three thousand badges a piece — pick what flatters."),
			L("Mmn. No name on the roll, no fitting. Go see Davros in the middle of the hall, then come back and we'll find something that suits you."));

		AddMercenaryShopkeeper(160198, "Wingmaker Mercenary", "WingMercenary",
			"c_request_1", -17, 78, 0,
			"MercenaryWingShop",
			L("Feathers, fabric, or fantasy — every hunter deserves a pair. Five thousand badges each. Scholars don't ask where they came from; neither do I."),
			L("Wings aren't for tourists. Present yourself to Captain Davros at the center of the hall. When your name's on his slate, you can come back and pick a pair."));
	}

	private void AddMercenaryShopkeeper(int modelId, string title, string uniqueName, string mapName, float x, float z, int direction, string shopName, string greeting, string needsCaptainLine)
	{
		Dialog.RegisterPropertyShopForMap(mapName, shopName, MercenaryPointScript);

		AddNpc(modelId, L("[" + title + "]"), uniqueName, mapName, x, z, direction, async dialog =>
		{
			var character = dialog.Player;
			dialog.SetTitle(L(title));

			if (!character.Variables.Perm.GetBool(MetCaptainVariable, false))
			{
				await dialog.Msg(needsCaptainLine);
				return;
			}

			await dialog.Msg(greeting);

			dialog.OpenPropertyShop(shopName, MercenaryCurrencyProperty, MercenaryShopPointName);
		});
	}

	private async Task CaptainDavrosDialog(Dialog dialog)
	{
		var character = dialog.Player;
		var questId = new QuestId("c_request_1", 1001);

		dialog.SetTitle(L("Davros Renn"));

		var firstMeeting = !character.Variables.Perm.GetBool(MetCaptainVariable, false);
		if (firstMeeting)
		{
			character.Variables.Perm.Set(MetCaptainVariable, true);

			await dialog.Msg(L("New face. Good. Sit, drink, listen. I'm Davros Renn, captain of this chapter of the Mercenary Guild. This post in Fedimian exists for one reason: mythic beasts."));
			await dialog.Msg(L("After the war, the worst of what crawled out of the corruption didn't die with the demon lords. A few crept off into the wildest corners of the map, fed on the leftovers, and grew into something stronger. Smarter. Worse."));
			await dialog.Msg(L("Scholars here in Fedimian call them {#996300}mythics{/}. Rare. Lethal. You'll know one when you see it: they don't move like ordinary beasts, and they wear a halo of unnatural power you can feel from a distance."));
			await dialog.Msg(L("They only haunt the truly dangerous maps. Never the roads. Never near the towns. If it's convenient, it's not a mythic. We sited the guild here because Fedimian's scholars pay well for every scrap of data we bring back, and our hunters need a place to drink and swap stories between contracts."));
		}

		var hasQuest = character.Quests.IsActive(questId) || character.Quests.HasCompleted(questId);
		if (!hasQuest)
		{
			await this.OfferHuntContract(dialog, character, questId);
			return;
		}

		var redeemed = this.RedeemMythicKills(character);
		if (redeemed > 0)
		{
			var badges = redeemed * BadgesPerReward;
			await dialog.Msg(LF("That's {0} mythics brought down. Fedimian's scholars will be circling me for reports. Here, {1} badges — spend them at the specialist booths along the walls.", redeemed * MythicKillsPerReward, badges));
		}

		var choice = await dialog.Select(L("Ledger's open. Fifty badges per mythic, paid the moment you report in. What'll it be?"),
			Option(L("Tell me a mythic story"), "story"),
			Option(L("I'll get back to the hunt"), "leave")
		);

		if (choice == "story")
			await this.TellMythicStory(dialog, character);
	}

	private async Task OfferHuntContract(Dialog dialog, Character character, QuestId questId)
	{
		var choice = await dialog.Select(L("So. Want contract work? The standing bounty is simple: one mythic, fifty badges. Bring me one more, fifty more. As long as you keep hunting, I keep paying — the ledger never closes."),
			Option(L("Sign me up"), "accept"),
			Option(L("Tell me a mythic story"), "story"),
			Option(L("Another time"), "leave")
		);

		switch (choice)
		{
			case "accept":
				await character.Quests.Start(questId);
				await dialog.Msg(L("Welcome to the ledger. Hunt anywhere on the dangerous maps — if a beast wears the mythic glow, it counts. Come find me whenever you want to collect. Remainders stay on the books."));
				break;

			case "story":
				await this.TellMythicStory(dialog, character);
				break;
		}
	}

	private async Task TellMythicStory(Dialog dialog, Character character)
	{
		var index = character.Variables.Perm.GetInt("Laima.MercenaryGuild.StoryIndex", 0);

		switch (index % 4)
		{
			case 0:
				await dialog.Msg(L("{#666666}*Davros lowers his cup*{/}"));
				await dialog.Msg(L("A year back, a hunter named Ilsa cornered something the scholars still argue over — a wolf-thing wreathed in blood-fog. Every cut she made filled the air with more of the stuff, until her lantern went out and she was fighting blind."));
				await dialog.Msg(L("She came back with half a cloak and a jar of the fog, sealed tight. Fedimian paid her triple for the sample. She retired on that one purse. Never hunted again."));
				break;

			case 1:
				await dialog.Msg(L("There was a mythic out past the ruins that never touched the ground. Floated. Whispered in languages nobody recognized. Scholars thought it was a construct left over from the war — until it shrugged off a pike through the chest and kept whispering."));
				await dialog.Msg(L("Took four of us a full day to bring it down. We only got paid for one hunter's worth, but the sketches are pinned on the board over there. Go look if you've got a strong stomach."));
				break;

			case 2:
				await dialog.Msg(L("Rarest mythic I've ever seen was a lucky one. Coat shimmered like coin in sunlight. Fedimian's scholars called it a blessing variant — something warped by residual goddess-light instead of demon-rot."));
				await dialog.Msg(L("Killing it felt wrong. Left us all quiet for a week. But the payout fed the guild for two months. Mythics aren't a job. They're a contract you sign with your own conscience."));
				break;

			case 3:
				await dialog.Msg(L("Worst one? An infected mythic that passed its sickness to anything it bled on. Killed a party of six before they realized what was happening. The survivors had to burn their own gear."));
				await dialog.Msg(L("Rule of the guild: if a mythic's bleeding, don't touch the blood. Don't walk through the puddle. Don't breathe it. The scholars will buy the sample — but only if you let them extract it, not you."));
				break;
		}

		character.Variables.Perm.Set("Laima.MercenaryGuild.StoryIndex", index + 1);
	}

	// Consumes whole groups of mythic kills from the player's ledger and
	// returns how many full reward packets were redeemed. Any remainder is
	// kept so the player can finish it out on the next hunt.
	private int RedeemMythicKills(Character character)
	{
		var count = character.Variables.Perm.GetInt(MythicKillsVariable, 0);
		if (count < MythicKillsPerReward)
			return 0;

		var rewards = count / MythicKillsPerReward;
		var remainder = count % MythicKillsPerReward;
		var consumed = rewards * MythicKillsPerReward;
		var badges = rewards * BadgesPerReward;

		character.Inventory.Add(MercenaryBadgeItemId, badges, InventoryAddType.PickUp);
		character.Variables.Perm.Set(MythicKillsVariable, remainder);

		var questId = new QuestId("c_request_1", 1001);
		if (character.Quests.TryGetById(questId, out var quest)
			&& quest.TryGetProgress("killMythics", out var progress))
		{
			progress.Count = System.Math.Max(0, progress.Count - consumed);
			character.Quests.UpdateClient_UpdateQuest(quest);
		}

		return rewards;
	}

	private void CreateMercenaryArtefactShops()
	{
		PropertyShops.Create("MercenaryBladeShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Swords
				shop.AddItem("Artefact_634004", 634004, 1, 1500); // Bishop Sword
				shop.AddItem("Artefact_634005", 634005, 1, 1500); // Knight Sword
				shop.AddItem("Artefact_634024", 634024, 1, 1500); // Scimitar of Glory
				shop.AddItem("Artefact_634030", 634030, 1, 1500); // Triple Jack-O-Lantern Sword
				shop.AddItem("Artefact_634040", 634040, 1, 1500); // Christmas Tree Sword
				shop.AddItem("Artefact_634053", 634053, 1, 1500); // Fluffy Kitty Sword
				shop.AddItem("Artefact_634066", 634066, 1, 1500); // Crunchy Choco Sword
				shop.AddItem("Artefact_634088", 634088, 1, 1500); // Twilight Star Sword
				shop.AddItem("Artefact_634092", 634092, 1, 1500); // Red Pincer Sword
				shop.AddItem("Artefact_634134", 634134, 1, 1500); // TOS Ranger Sword
				shop.AddItem("Artefact_634150", 634150, 1, 1500); // The Bat Count Sword
				shop.AddItem("Artefact_634180", 634180, 1, 1500); // Crane Sword
				shop.AddItem("Artefact_634187", 634187, 1, 1500); // Wave Scimitar
				shop.AddItem("Artefact_634207", 634207, 1, 1500); // Popo Pop Sword
				shop.AddItem("EP12_Artefact_001", 11007001, 1, 1500); // Red Fox Sword
				shop.AddItem("EP12_Artefact_017", 11007017, 1, 1500); // Mayflower Sword
				shop.AddItem("EP12_Artefact_033", 11007033, 1, 1500); // MusCATeer Sword
				shop.AddItem("EP12_Artefact_046", 11007046, 1, 1500); // TOS Task Force Sword
				shop.AddItem("EP12_Artefact_062", 11007062, 1, 1500); // Summer Wave Sword
				shop.AddItem("EP12_Artefact_078", 11007078, 1, 1500); // Honored Rose Sword
				shop.AddItem("EP13_Artefact_000", 11007103, 1, 1500); // Task Unit Red Beam Sword
				shop.AddItem("EP13_Artefact_001", 11007104, 1, 1500); // Task Unit Green Beam Sword
				shop.AddItem("EP13_Artefact_015", 11007118, 1, 1500); // Criminal Explosive Sword
				shop.AddItem("EP13_Artefact_019", 11007122, 1, 1500); // Giltine Sword
				shop.AddItem("EP13_Artefact_035", 11007138, 1, 1500); // STEM Smartphone Sword
				shop.AddItem("EP13_Artefact_141", 11007244, 1, 1500); // Athleisure Tennis Racket Sword
				shop.AddItem("EP12_Re_Artefact_001", 11007325, 1, 1500); // [Re] Red Fox Sword
				shop.AddItem("EP12_Re_Artefact_634150", 11007341, 1, 1500); // [Re] The Bat Count Sword
				shop.AddItem("EP14_Contents_Artefact_001", 11043001, 1, 1500); // Baubas Sword
				shop.AddItem("EP15_Artefact_001", 11104007, 1, 1500); // Lofty Snow Sword
				// Two-Handed Swords
				shop.AddItem("Artefact_634007", 634007, 1, 1500); // Ivory Queen Two-handed Sword
				shop.AddItem("Artefact_634008", 634008, 1, 1500); // Ebony King Two-handed Sword
				shop.AddItem("Artefact_634031", 634031, 1, 1500); // Chainsaw Two-handed Sword
				shop.AddItem("Artefact_634041", 634041, 1, 1500); // Christmas Tree Two-handed Sword
				shop.AddItem("Artefact_634054", 634054, 1, 1500); // Fluffy Kitty Two-handed Sword
				shop.AddItem("Artefact_634067", 634067, 1, 1500); // Crunchy Choco Two-handed Sword
				shop.AddItem("Artefact_634089", 634089, 1, 1500); // Twilight Star Two-handed Sword
				shop.AddItem("Artefact_634093", 634093, 1, 1500); // Sawfish Two-handed Sword
				shop.AddItem("Artefact_634108", 634108, 1, 1500); // BBQ Skewer Two-handed Sword
				shop.AddItem("Artefact_634135", 634135, 1, 1500); // TOS Ranger Two-handed Sword
				shop.AddItem("Artefact_634151", 634151, 1, 1500); // The Bat Count Two-handed Sword
				shop.AddItem("Artefact_634166", 634166, 1, 1500); // Aurora Dafne Two-handed Sword
				shop.AddItem("Artefact_634182", 634182, 1, 1500); // Wave Greatsword
				shop.AddItem("Artefact_634193", 634193, 1, 1500); // Magical Savior Two-handed Sword
				shop.AddItem("Artefact_634206", 634206, 1, 1500); // Popo Pop Two-handed Sword
				shop.AddItem("EP12_Artefact_002", 11007002, 1, 1500); // Red Fox Two-handed Sword
				shop.AddItem("EP12_Artefact_018", 11007018, 1, 1500); // Mayflower Two-handed Sword
				shop.AddItem("EP12_Artefact_034", 11007034, 1, 1500); // MusCATeer Two-handed Sword
				shop.AddItem("EP12_Artefact_047", 11007047, 1, 1500); // TOS Task Force Two-handed Sword
				shop.AddItem("EP12_Artefact_063", 11007063, 1, 1500); // Summer Wave Two-handed Sword
				shop.AddItem("EP12_Artefact_079", 11007079, 1, 1500); // Honored Rose Two-handed Sword
				shop.AddItem("EP12_Artefact_100", 11007100, 1, 1500); // Evening Star Two-handed Sword
				shop.AddItem("EP13_Artefact_011", 11007114, 1, 1500); // Rosy Floret Two-handed Sword
				shop.AddItem("EP13_Artefact_016", 11007119, 1, 1500); // Criminal Keen Edge Two-handed Sword
				shop.AddItem("EP13_Artefact_020", 11007123, 1, 1500); // Giltine Two-handed Sword
				shop.AddItem("EP13_Artefact_135", 11007238, 1, 1500); // Armonia Two-handed Sword of Otherworld
				shop.AddItem("EP13_Artefact_218", 11007321, 1, 1500); // Animal Kindergarten Bubble Two-handed Sword
				shop.AddItem("EP12_Re_Artefact_002", 11007326, 1, 1500); // [Re] Red Fox Two-handed Sword
				shop.AddItem("EP12_Re_Artefact_634151", 11007342, 1, 1500); // [Re] The Bat Count Two-handed Sword
				shop.AddItem("EP14_Artefact_006", 11007375, 1, 1500); // Knight of Zemyna Two-handed Sword
				shop.AddItem("EP14_Artefact_017", 11007386, 1, 1500); // Major Arcana The Tower Two-handed Sword
				shop.AddItem("EP14_Contents_Artefact_002", 11043002, 1, 1500); // Baubas Two-handed Sword
				// Rapiers
				shop.AddItem("Artefact_634003", 634003, 1, 1500); // Bishop Rapier
				shop.AddItem("Artefact_634006", 634006, 1, 1500); // Knight Rapier
				shop.AddItem("Artefact_634029", 634029, 1, 1500); // Pumpkin Rapier
				shop.AddItem("Artefact_634058", 634058, 1, 1500); // Fluffy Kitty Rapier
				shop.AddItem("Artefact_634068", 634068, 1, 1500); // Crunchy Choco Rapier
				shop.AddItem("Artefact_634081", 634081, 1, 1500); // Impassioned Rose Rapier
				shop.AddItem("Artefact_634094", 634094, 1, 1500); // Sawfish Rapier
				shop.AddItem("Artefact_634109", 634109, 1, 1500); // Roasted Marshmallow Rapier
				shop.AddItem("Artefact_634122", 634122, 1, 1500); // Fish-Carver Rapier
				shop.AddItem("Artefact_634138", 634138, 1, 1500); // TOS Ranger Rapier
				shop.AddItem("Artefact_634154", 634154, 1, 1500); // Surgical Scissors Rapier
				shop.AddItem("Artefact_634169", 634169, 1, 1500); // Santa Rapier
				shop.AddItem("Artefact_634188", 634188, 1, 1500); // Guardian Rapier
				shop.AddItem("Artefact_634199", 634199, 1, 1500); // Magical Savior Rapier
				shop.AddItem("Artefact_634205", 634205, 1, 1500); // Popo Pop Rapier
				shop.AddItem("EP12_Artefact_010", 11007010, 1, 1500); // Simmering Starlight Rapier
				shop.AddItem("EP12_Artefact_026", 11007026, 1, 1500); // Mayflower Rapier
				shop.AddItem("EP12_Artefact_042", 11007042, 1, 1500); // MusCATeer Rapier
				shop.AddItem("EP12_Artefact_055", 11007055, 1, 1500); // TOS Task Force Rapier
				shop.AddItem("EP12_Artefact_071", 11007071, 1, 1500); // Summer Wave Rapier
				shop.AddItem("EP12_Artefact_087", 11007087, 1, 1500); // Honored Rose Rapier
				shop.AddItem("EP12_Artefact_095", 11007095, 1, 1500); // Masquerade Blue Blossom Rapier
				shop.AddItem("EP13_Artefact_028", 11007131, 1, 1500); // Giltine Rapier
				shop.AddItem("EP13_Artefact_036", 11007139, 1, 1500); // STEM Spoid Rapier
				shop.AddItem("EP13_Artefact_132", 11007235, 1, 1500); // Good ol'days Pintail Comb Rapier
				shop.AddItem("EP12_Re_Artefact_010", 11007334, 1, 1500); // [Re] Simmering Starlight Rapier
				shop.AddItem("EP12_Re_Artefact_634154", 11007345, 1, 1500); // [Re] Surgical Scissors Rapier
				shop.AddItem("EP12_Re_Artefact_634122", 11007358, 1, 1500); // [Re] Fish-Carver Raiper
				shop.AddItem("EP14_Artefact_009", 11007378, 1, 1500); // Knight of Zemyna Rapier
		});

		PropertyShops.Create("MercenaryArcherShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Bows
				shop.AddItem("Artefact_634016", 634016, 1, 1500); // Bishop Bow
				shop.AddItem("Artefact_634017", 634017, 1, 1500); // Knight Bow
				shop.AddItem("Artefact_634034", 634034, 1, 1500); // Bat Bow
				shop.AddItem("Artefact_634050", 634050, 1, 1500); // Fluffy Kitty Bow
				shop.AddItem("Artefact_634076", 634076, 1, 1500); // Crunchy Choco Bow
				shop.AddItem("Artefact_634106", 634106, 1, 1500); // Hibiscus Bow
				shop.AddItem("Artefact_634119", 634119, 1, 1500); // Calcot Bow
				shop.AddItem("Artefact_634141", 634141, 1, 1500); // TOS Ranger Bow
				shop.AddItem("Artefact_634157", 634157, 1, 1500); // Haunted Oak Bow
				shop.AddItem("Artefact_634172", 634172, 1, 1500); // Rudolf Bow
				shop.AddItem("Artefact_634185", 634185, 1, 1500); // Dark Cloud Bow
				shop.AddItem("Artefact_634186", 634186, 1, 1500); // White Cloud Bow
				shop.AddItem("Artefact_634196", 634196, 1, 1500); // Magical Savior Bow
				shop.AddItem("EP12_Artefact_007", 11007007, 1, 1500); // Prickly Rose Bow
				shop.AddItem("EP12_Artefact_023", 11007023, 1, 1500); // Mayflower Bow
				shop.AddItem("EP12_Artefact_039", 11007039, 1, 1500); // MusCATeer Bow
				shop.AddItem("EP12_Artefact_052", 11007052, 1, 1500); // TOS Task Force Bow
				shop.AddItem("EP12_Artefact_068", 11007068, 1, 1500); // Summer Wave Bow
				shop.AddItem("EP12_Artefact_084", 11007084, 1, 1500); // Honored Rose Bow
				shop.AddItem("EP13_Artefact_008", 11007111, 1, 1500); // Rosy Floret Bow
				shop.AddItem("EP13_Artefact_025", 11007128, 1, 1500); // Giltine Bow
				shop.AddItem("EP13_Artefact_038", 11007141, 1, 1500); // STEM Organic Chemistry Bow
				shop.AddItem("EP12_Re_Artefact_007", 11007331, 1, 1500); // [Re] Prickly Rose Bow
				shop.AddItem("EP12_Re_Artefact_634157", 11007348, 1, 1500); // [Re] Haunted Oak Bow
				shop.AddItem("EP14_Artefact_008", 11007377, 1, 1500); // Knight of Zemyna Bow
				shop.AddItem("EP14_Artefact_013", 11007382, 1, 1500); // TOS Metal Flaming Torch Bow
				shop.AddItem("EP14_Artefact_022", 11007391, 1, 1500); // Furry Golden Fox Bow
				// Crossbows
				shop.AddItem("Artefact_634026", 634026, 1, 1500); // Apricot Blossom Crossbow
				shop.AddItem("Artefact_634042", 634042, 1, 1500); // Snowman Crossbow
				shop.AddItem("Artefact_634077", 634077, 1, 1500); // Crunchy Choco Crossbow
				shop.AddItem("Artefact_634105", 634105, 1, 1500); // Rubber Ducky Crossbow
				shop.AddItem("Artefact_634118", 634118, 1, 1500); // Sausage Crossbow
				shop.AddItem("Artefact_634142", 634142, 1, 1500); // TOS Ranger Crossbow
				shop.AddItem("Artefact_634158", 634158, 1, 1500); // Clown Crossbow
				shop.AddItem("Artefact_634173", 634173, 1, 1500); // Evergreen Crossbow
				shop.AddItem("Artefact_634191", 634191, 1, 1500); // Swallow Crossbow
				shop.AddItem("Artefact_634202", 634202, 1, 1500); // Magical Savior Crossbow
				shop.AddItem("EP12_Artefact_016", 11007016, 1, 1500); // East Biplane Crossbow
				shop.AddItem("EP12_Artefact_032", 11007032, 1, 1500); // Mayflower Crossbow
				shop.AddItem("EP12_Artefact_061", 11007061, 1, 1500); // TOS Task Force Crossbow
				shop.AddItem("EP12_Artefact_077", 11007077, 1, 1500); // Summer Wave Crossbow
				shop.AddItem("EP12_Artefact_093", 11007093, 1, 1500); // Honored Rose Crossbow
				shop.AddItem("EP12_Artefact_102", 11007102, 1, 1500); // Evening Star Crossbow
				shop.AddItem("EP13_Artefact_034", 11007137, 1, 1500); // Giltine Crossbow
				shop.AddItem("EP13_Artefact_037", 11007140, 1, 1500); // STEM Acute Angle Crossbow
				shop.AddItem("EP13_Artefact_129", 11007232, 1, 1500); // Drip Drop Frog Crossbow
				shop.AddItem("EP13_Artefact_134", 11007237, 1, 1500); // Good ol'days LP Crossbow
				shop.AddItem("EP12_Re_Artefact_016", 11007340, 1, 1500); // [Re] East Biplane Crossbow
				shop.AddItem("EP12_Re_Artefact_634158", 11007349, 1, 1500); // [Re] Clown Crossbow
				shop.AddItem("EP14_Artefact_003", 11007372, 1, 1500); // Tos Hero Crossbow
		});

		PropertyShops.Create("MercenaryWizardShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Rods
				shop.AddItem("Artefact_630046", 630047, 1, 1500); // Marine Rod
				shop.AddItem("Artefact_634011", 634011, 1, 1500); // Ebony King Rod
				shop.AddItem("Artefact_634012", 634012, 1, 1500); // Ivory Queen Rod
				shop.AddItem("Artefact_634022", 634022, 1, 1500); // Dumpling Rod
				shop.AddItem("Artefact_634045", 634045, 1, 1500); // Candlelight Rod
				shop.AddItem("Artefact_634060", 634060, 1, 1500); // Fluffy Kitty Rod
				shop.AddItem("Artefact_634062", 634062, 1, 1500); // Mystic Savior Rod
				shop.AddItem("Artefact_634075", 634075, 1, 1500); // Crunchy Choco Rod
				shop.AddItem("Artefact_634098", 634098, 1, 1500); // Butter-roasted Squid Rod
				shop.AddItem("Artefact_634113", 634113, 1, 1500); // Steel Frying Pan Rod
				shop.AddItem("Artefact_634123", 634123, 1, 1500); // Wine Glass Rod
				shop.AddItem("Artefact_634148", 634148, 1, 1500); // TOS Ranger Rod
				shop.AddItem("Artefact_634164", 634164, 1, 1500); // Hallowventer Hand Rod
				shop.AddItem("Artefact_634178", 634178, 1, 1500); // Candy Cane Rod
				shop.AddItem("Artefact_634195", 634195, 1, 1500); // Magical Savior Rod
				shop.AddItem("EP12_Artefact_013", 11007013, 1, 1500); // Soaring Bluebird Rod
				shop.AddItem("EP12_Artefact_029", 11007029, 1, 1500); // Mayflower Rod
				shop.AddItem("EP12_Artefact_044", 11007044, 1, 1500); // MusCATeer Rod
				shop.AddItem("EP12_Artefact_058", 11007058, 1, 1500); // TOS Task Force Rod
				shop.AddItem("EP12_Artefact_074", 11007074, 1, 1500); // Summer Wave Rod
				shop.AddItem("EP12_Artefact_090", 11007090, 1, 1500); // Honored Rose Rod
				shop.AddItem("EP13_Artefact_009", 11007112, 1, 1500); // Rosy Floret Rod
				shop.AddItem("EP13_Artefact_012", 11007115, 1, 1500); // Sweet Ice Cream Spoon Rod
				shop.AddItem("EP13_Artefact_031", 11007134, 1, 1500); // Giltine Rod
				shop.AddItem("EP13_Artefact_122", 11007225, 1, 1500); // TOSummer Rod
				shop.AddItem("EP13_Artefact_146", 11007249, 1, 1500); // Asgard Mimir Rod
				shop.AddItem("EP13_Artefact_221", 11007324, 1, 1500); // Animal Kindergarten Candy Rod
				shop.AddItem("EP12_Re_Artefact_013", 11007337, 1, 1500); // [Re] Soaring Bluebird Rod
				shop.AddItem("EP12_Re_Artefact_634164", 11007355, 1, 1500); // [Re] Hallowventer Hand Rod
				shop.AddItem("EP12_Re_Artefact_634123", 11007359, 1, 1500); // [Re] Wine Glass Rod
				shop.AddItem("EP13_Artefact_122_NoTrade", 11007411, 1, 1500); // TOSummer Rod
				// Staves
				shop.AddItem("Artefact_630047", 630048, 1, 1500); // Marine Staff
				shop.AddItem("Artefact_634013", 634013, 1, 1500); // Ebony King Staff
				shop.AddItem("Artefact_634014", 634014, 1, 1500); // Ivory Queen Staff
				shop.AddItem("Artefact_634021", 634021, 1, 1500); // Moon Rabbit Staff
				shop.AddItem("Artefact_634033", 634033, 1, 1500); // Broomstick Staff
				shop.AddItem("Artefact_634046", 634046, 1, 1500); // Candlelight Staff
				shop.AddItem("Artefact_634051", 634051, 1, 1500); // Fluffy Kitty Staff
				shop.AddItem("Artefact_634061", 634061, 1, 1500); // Mystic Savior Staff
				shop.AddItem("Artefact_634074", 634074, 1, 1500); // Crunchy Choco Staff
				shop.AddItem("Artefact_634099", 634099, 1, 1500); // Crab-hunter Staff
				shop.AddItem("Artefact_634114", 634114, 1, 1500); // Grilled Mackerel Staff
				shop.AddItem("Artefact_634128", 634128, 1, 1500); // Spatula Staff
				shop.AddItem("Artefact_634147", 634147, 1, 1500); // TOS Ranger Staff
				shop.AddItem("Artefact_634163", 634163, 1, 1500); // Haunted Skull Staff
				shop.AddItem("Artefact_634177", 634177, 1, 1500); // Ice Crystal Staff
				shop.AddItem("Artefact_634183", 634183, 1, 1500); // Lotus Staff
				shop.AddItem("Artefact_634184", 634184, 1, 1500); // Red Lotus Staff
				shop.AddItem("Artefact_634194", 634194, 1, 1500); // Magical Savior Staff
				shop.AddItem("EP12_Artefact_014", 11007014, 1, 1500); // Soaring Bluebird Staff
				shop.AddItem("EP12_Artefact_030", 11007030, 1, 1500); // Mayflower Staff
				shop.AddItem("EP12_Artefact_045", 11007045, 1, 1500); // MusCATeer Staff
				shop.AddItem("EP12_Artefact_059", 11007059, 1, 1500); // TOS Task Force Staff
				shop.AddItem("EP12_Artefact_075", 11007075, 1, 1500); // Summer Wave Staff
				shop.AddItem("EP12_Artefact_091", 11007091, 1, 1500); // Honored Rose Staff
				shop.AddItem("EP12_Artefact_097", 11007097, 1, 1500); // Maru Magic Staff
				shop.AddItem("EP13_Artefact_005", 11007108, 1, 1500); // Ice Cold Mini Heater Staff
				shop.AddItem("EP13_Artefact_032", 11007135, 1, 1500); // Giltine Staff
				shop.AddItem("EP13_Artefact_123", 11007226, 1, 1500); // TOSummer Staff
				shop.AddItem("EP13_Artefact_125", 11007228, 1, 1500); // Drip Drop Dewy Staff
				shop.AddItem("EP13_Artefact_147", 11007250, 1, 1500); // Asgard Mimir Staff
				shop.AddItem("EP13_Artefact_152", 11007255, 1, 1500); // Servant Candlestick Staff
				shop.AddItem("EP12_Re_Artefact_014", 11007338, 1, 1500); // [Re] Soaring Bluebird Staff
				shop.AddItem("EP12_Re_Artefact_634163", 11007354, 1, 1500); // [Re] Haunted Skull Staff
				shop.AddItem("EP12_Re_Artefact_634128", 11007364, 1, 1500); // [Re] Spatula Staff
				shop.AddItem("EP14_Artefact_001", 11007370, 1, 1500); // Eternal Savior Lumen Staff
				shop.AddItem("EP14_Artefact_021", 11007390, 1, 1500); // Furry Noble Fox Staff
				shop.AddItem("EP13_Artefact_123_NoTrade", 11007412, 1, 1500); // TOSummer Staff
		});

		PropertyShops.Create("MercenaryClericShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Maces
				shop.AddItem("Artefact_634009", 634009, 1, 1500); // Knight Mace
				shop.AddItem("Artefact_634010", 634010, 1, 1500); // Bishop Mace
				shop.AddItem("Artefact_634025", 634025, 1, 1500); // Wooden Stick Mace
				shop.AddItem("Artefact_634032", 634032, 1, 1500); // Jack-O-Lantern Mace
				shop.AddItem("Artefact_634049", 634049, 1, 1500); // Fluffy Kitty Mace
				shop.AddItem("Artefact_634063", 634063, 1, 1500); // Mystic Savior Mace
				shop.AddItem("Artefact_634073", 634073, 1, 1500); // Crunchy Choco Mace
				shop.AddItem("Artefact_634086", 634086, 1, 1500); // Twilight Star Mace
				shop.AddItem("Artefact_634100", 634100, 1, 1500); // Ukulele Mace
				shop.AddItem("Artefact_634115", 634115, 1, 1500); // Lumberjack Axe Mace
				shop.AddItem("Artefact_634124", 634124, 1, 1500); // Saltshaker Mace
				shop.AddItem("Artefact_634139", 634139, 1, 1500); // TOS Ranger Mace
				shop.AddItem("Artefact_634155", 634155, 1, 1500); // Ripped Teddy Mace
				shop.AddItem("Artefact_634170", 634170, 1, 1500); // Jingle Bell Mace
				shop.AddItem("Artefact_634181", 634181, 1, 1500); // Taegeuk Small Drum
				shop.AddItem("Artefact_634189", 634189, 1, 1500); // Dokkabi Mace
				shop.AddItem("Artefact_634200", 634200, 1, 1500); // Magical Savior Mace
				shop.AddItem("Artefact_634208", 634208, 1, 1500); // Popo Pop Mace
				shop.AddItem("EP12_Artefact_005", 11007005, 1, 1500); // Precious Small Rose Mace
				shop.AddItem("EP12_Artefact_021", 11007021, 1, 1500); // Mayflower Mace
				shop.AddItem("EP12_Artefact_037", 11007037, 1, 1500); // MusCATeer Mace
				shop.AddItem("EP12_Artefact_050", 11007050, 1, 1500); // TOS Task Force Mace
				shop.AddItem("EP12_Artefact_066", 11007066, 1, 1500); // Summer Wave Mace
				shop.AddItem("EP12_Artefact_082", 11007082, 1, 1500); // Honored Rose Mace
				shop.AddItem("EP13_Artefact_013", 11007116, 1, 1500); // Sweet Ice Cream Scoop Mace
				shop.AddItem("EP13_Artefact_023", 11007126, 1, 1500); // Giltine Mace
				shop.AddItem("EP13_Artefact_120", 11007223, 1, 1500); // TOSummer Mace
				shop.AddItem("EP13_Artefact_127", 11007230, 1, 1500); // Drip Drop Lightning Cloud Mace
				shop.AddItem("EP13_Artefact_130", 11007233, 1, 1500); // Good ol'days Electric Guitar Mace
				shop.AddItem("EP13_Artefact_144", 11007247, 1, 1500); // Asgard Mjolnir Mace
				shop.AddItem("EP13_Artefact_151", 11007254, 1, 1500); // Servant Dusty Mace
				shop.AddItem("EP13_Artefact_219", 11007322, 1, 1500); // Animal Kindergarten Cotton Candy Mace
				shop.AddItem("EP12_Re_Artefact_005", 11007329, 1, 1500); // [Re] Precious Small Rose Mace
				shop.AddItem("EP12_Re_Artefact_634155", 11007346, 1, 1500); // [Re] Ripped Teddy Mace
				shop.AddItem("EP12_Re_Artefact_634124", 11007360, 1, 1500); // [Re] Saltshaker Mace
				shop.AddItem("EP14_Artefact_004", 11007373, 1, 1500); // Tos Hero Mace
				shop.AddItem("EP14_Artefact_012", 11007381, 1, 1500); // TOS Metal Spiky Bat Mace
				shop.AddItem("EP14_Artefact_020", 11007389, 1, 1500); // Furry Sky Doggy Mace
				shop.AddItem("EP13_Artefact_120_NoTrade", 11007409, 1, 1500); // TOSummer Mace
				shop.AddItem("EP15_Artefact_003", 11104009, 1, 1500); // Lofty Snow Tuna Mace
				// Two-Handed Maces
				shop.AddItem("Artefact_634001", 634001, 1, 1500); // Ivory Pawn Two-handed Mace
				shop.AddItem("Artefact_634002", 634002, 1, 1500); // Ivory Rook Two-handed Mace
				shop.AddItem("Artefact_634028", 634028, 1, 1500); // Jack-O-Lantern Two-handed Mace
				shop.AddItem("Artefact_634059", 634059, 1, 1500); // Fluffy Kitty Two-handed Mace
				shop.AddItem("Artefact_634064", 634064, 1, 1500); // Mystic Savior Two-handed Mace
				shop.AddItem("Artefact_634072", 634072, 1, 1500); // Crunchy Choco Two-handed Mace
				shop.AddItem("Artefact_634087", 634087, 1, 1500); // Twilight Star Two-handed Mace
				shop.AddItem("Artefact_634101", 634101, 1, 1500); // Guitar Two-handed Mace
				shop.AddItem("Artefact_634116", 634116, 1, 1500); // Buttered Corn Two-handed Mace
				shop.AddItem("Artefact_634127", 634127, 1, 1500); // Pepper Shaker Two-handed Mace
				shop.AddItem("Artefact_634140", 634140, 1, 1500); // TOS Ranger Two-handed Mace
				shop.AddItem("Artefact_634156", 634156, 1, 1500); // Rag Doll Two-handed Mace
				shop.AddItem("Artefact_634171", 634171, 1, 1500); // Crystal Dafne Two-handed Mace
				shop.AddItem("Artefact_634190", 634190, 1, 1500); // Dokkabi Maul
				shop.AddItem("Artefact_634201", 634201, 1, 1500); // Magical Savior Two-handed Mace
				shop.AddItem("Artefact_634209", 634209, 1, 1500); // Popo Pop Two-handed Mace
				shop.AddItem("EP12_Artefact_006", 11007006, 1, 1500); // Precious Rose Two-handed Mace
				shop.AddItem("EP12_Artefact_022", 11007022, 1, 1500); // Mayflower Two-handed Mace
				shop.AddItem("EP12_Artefact_038", 11007038, 1, 1500); // MusCATeer Two-handed Mace
				shop.AddItem("EP12_Artefact_051", 11007051, 1, 1500); // TOS Task Force Two-handed Mace
				shop.AddItem("EP12_Artefact_067", 11007067, 1, 1500); // Summer Wave Two-handed Mace
				shop.AddItem("EP12_Artefact_083", 11007083, 1, 1500); // Honored Rose Two-handed Mace
				shop.AddItem("EP12_Artefact_101", 11007101, 1, 1500); // Evening Star Two-handed Mace
				shop.AddItem("EP13_Artefact_024", 11007127, 1, 1500); // Giltine Two-handed Mace
				shop.AddItem("EP13_Artefact_119", 11007222, 1, 1500); // TOSummer Two-handed Mace
				shop.AddItem("EP13_Artefact_133", 11007236, 1, 1500); // Good ol'days Neon Sign Two-handed Mace
				shop.AddItem("EP13_Artefact_140", 11007243, 1, 1500); // Athleisure Barbell Two-handed Mace
				shop.AddItem("EP13_Artefact_145", 11007248, 1, 1500); // Asgard Mjolnir Two-handed Mace
				shop.AddItem("EP13_Artefact_220", 11007323, 1, 1500); // Animal Kindergarten Duckling Two-handed Mace
				shop.AddItem("EP12_Re_Artefact_006", 11007330, 1, 1500); // [Re] Precious Rose Two-handed Mace
				shop.AddItem("EP12_Re_Artefact_634156", 11007347, 1, 1500); // [Re] Rag Doll Two-handed Mace
				shop.AddItem("EP12_Re_Artefact_634127", 11007363, 1, 1500); // [Re] Pepper Shaker Two-handed Mace
				shop.AddItem("EP14_Artefact_002", 11007371, 1, 1500); // Eternal Savior Nox Two-handed Mace
				shop.AddItem("EP13_Artefact_119_NoTrade", 11007408, 1, 1500); // TOSummer Two-handed Mace
				// Shields
				shop.AddItem("Artefact_634015", 634015, 1, 1500); // Chessboard Shield
				shop.AddItem("Artefact_634018", 634018, 1, 1500); // Wooden Pane Shield
				shop.AddItem("Artefact_634019", 634019, 1, 1500); // Horse Medal Shield
				shop.AddItem("Artefact_634043", 634043, 1, 1500); // Gingerbread Shield
				shop.AddItem("Artefact_634055", 634055, 1, 1500); // Fluffy Kitty Shield
				shop.AddItem("Artefact_634069", 634069, 1, 1500); // Crunchy Choco Shield
				shop.AddItem("Artefact_634085", 634085, 1, 1500); // Twilight Star Shield
				shop.AddItem("Artefact_634107", 634107, 1, 1500); // Steering Wheel Shield
				shop.AddItem("Artefact_634120", 634120, 1, 1500); // Grilled Platter Shield
				shop.AddItem("Artefact_634125", 634125, 1, 1500); // Pot-lid Shield
				shop.AddItem("Artefact_634149", 634149, 1, 1500); // TOS Ranger Shield
				shop.AddItem("Artefact_634165", 634165, 1, 1500); // Occult Paper Charm Shield
				shop.AddItem("Artefact_634179", 634179, 1, 1500); // Twinkle Shield
				shop.AddItem("Artefact_634203", 634203, 1, 1500); // Magical Savior Shield
				shop.AddItem("Artefact_634212", 634212, 1, 1500); // Popo Pop Shield
				shop.AddItem("EP12_Artefact_009", 11007009, 1, 1500); // Asteroid Shield
				shop.AddItem("EP12_Artefact_025", 11007025, 1, 1500); // Mayflower Shield
				shop.AddItem("EP12_Artefact_041", 11007041, 1, 1500); // MusCATeer Shield
				shop.AddItem("EP12_Artefact_054", 11007054, 1, 1500); // TOS Task Force Shield
				shop.AddItem("EP12_Artefact_070", 11007070, 1, 1500); // Summer Wave Shield
				shop.AddItem("EP12_Artefact_086", 11007086, 1, 1500); // Honored Rose Shield
				shop.AddItem("EP12_Artefact_094", 11007094, 1, 1500); // Masquerade Broken Mirror Shield
				shop.AddItem("EP12_Artefact_099", 11007099, 1, 1500); // Littleberk Claw Shield
				shop.AddItem("EP13_Artefact_006", 11007109, 1, 1500); // Ice Cold Handwarmer Shield
				shop.AddItem("EP13_Artefact_027", 11007130, 1, 1500); // Giltine Shield
				shop.AddItem("EP13_Artefact_143", 11007246, 1, 1500); // Asgard Sleipnir Shield
				shop.AddItem("EP12_Re_Artefact_009", 11007333, 1, 1500); // [Re] Asteroid Shield
				shop.AddItem("EP12_Re_Artefact_634165", 11007356, 1, 1500); // [Re] Occult Paper Charm Shield
				shop.AddItem("EP12_Re_Artefact_634125", 11007361, 1, 1500); // [Re] Pot-lid Shield
				shop.AddItem("EP14_Artefact_007", 11007376, 1, 1500); // Knight of Zemyna Shield
				shop.AddItem("EP14_Artefact_018", 11007387, 1, 1500); // Major Arcana The Sun & Moon Shield
				shop.AddItem("EP15_Artefact_004", 11104010, 1, 1500); // Lofty Snow Shield
		});

		PropertyShops.Create("MercenaryDragoonShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Spears
				shop.AddItem("Artefact_634023", 634023, 1, 1500); // Brush Stroke Spear
				shop.AddItem("Artefact_634035", 634035, 1, 1500); // Jack-O-Lantern Spear
				shop.AddItem("Artefact_634056", 634056, 1, 1500); // Fluffy Kitty Spear
				shop.AddItem("Artefact_634071", 634071, 1, 1500); // Crunchy Choco Spear
				shop.AddItem("Artefact_634090", 634090, 1, 1500); // Twilight Star Spear
				shop.AddItem("Artefact_634096", 634096, 1, 1500); // Pink Parasol Spear
				shop.AddItem("Artefact_634111", 634111, 1, 1500); // Roasted Kepa Spear
				shop.AddItem("Artefact_634121", 634121, 1, 1500); // Whisk Spear
				shop.AddItem("Artefact_634136", 634136, 1, 1500); // TOS Ranger Spear
				shop.AddItem("Artefact_634152", 634152, 1, 1500); // Pumpkin Lantern Spear
				shop.AddItem("Artefact_634167", 634167, 1, 1500); // Ice Crystal Spear
				shop.AddItem("Artefact_634197", 634197, 1, 1500); // Magical Savior Spear
				shop.AddItem("Artefact_634210", 634210, 1, 1500); // Popo Pop Spear
				shop.AddItem("EP12_Artefact_003", 11007003, 1, 1500); // Blooming Rose Spear
				shop.AddItem("EP12_Artefact_019", 11007019, 1, 1500); // Mayflower Spear
				shop.AddItem("EP12_Artefact_035", 11007035, 1, 1500); // MusCATeer Spear
				shop.AddItem("EP12_Artefact_048", 11007048, 1, 1500); // TOS Task Force Spear
				shop.AddItem("EP12_Artefact_064", 11007064, 1, 1500); // Summer Wave Spear
				shop.AddItem("EP12_Artefact_080", 11007080, 1, 1500); // Honored Rose Spear
				shop.AddItem("EP12_Artefact_096", 11007096, 1, 1500); // Masquerade Injector Spear
				shop.AddItem("EP13_Artefact_021", 11007124, 1, 1500); // Giltine Spear
				shop.AddItem("EP13_Artefact_126", 11007229, 1, 1500); // Drip Drop Rainbow Umbrella Spear
				shop.AddItem("EP13_Artefact_148", 11007251, 1, 1500); // Asgard Gungnir Spear
				shop.AddItem("EP12_Re_Artefact_003", 11007327, 1, 1500); // [Re] Blooming Rose Spear
				shop.AddItem("EP12_Re_Artefact_634152", 11007343, 1, 1500); // [Re] Pumpkin Lantern Spear
				shop.AddItem("EP12_Re_Artefact_634121", 11007357, 1, 1500); // [Re] Whisk Spear
				shop.AddItem("EP14_Artefact_016", 11007385, 1, 1500); // Heliopolis Horus Spear
				// Pikes
				shop.AddItem("Artefact_634036", 634036, 1, 1500); // Jack-O-Lantern Pike
				shop.AddItem("Artefact_634057", 634057, 1, 1500); // Fluffy Kitty Pike
				shop.AddItem("Artefact_634070", 634070, 1, 1500); // Crunchy Choco Pike
				shop.AddItem("Artefact_634091", 634091, 1, 1500); // Twilight Star Pike
				shop.AddItem("Artefact_634097", 634097, 1, 1500); // Blue Parasol Pike
				shop.AddItem("Artefact_634112", 634112, 1, 1500); // Red Kepa Roast Spike
				shop.AddItem("Artefact_634126", 634126, 1, 1500); // Whisk Pike
				shop.AddItem("Artefact_634137", 634137, 1, 1500); // TOS Ranger Pike
				shop.AddItem("Artefact_634153", 634153, 1, 1500); // Pumpkin Streetlamp Pike
				shop.AddItem("Artefact_634168", 634168, 1, 1500); // Ice Crystal Pike
				shop.AddItem("Artefact_634198", 634198, 1, 1500); // Magical Savior Pike
				shop.AddItem("Artefact_634211", 634211, 1, 1500); // Popo Pop Pike
				shop.AddItem("EP12_Artefact_004", 11007004, 1, 1500); // Fully Blooming Rose Pike
				shop.AddItem("EP12_Artefact_020", 11007020, 1, 1500); // Mayflower Pike
				shop.AddItem("EP12_Artefact_036", 11007036, 1, 1500); // MusCATeer Pike
				shop.AddItem("EP12_Artefact_049", 11007049, 1, 1500); // TOS Task Force Pike
				shop.AddItem("EP12_Artefact_065", 11007065, 1, 1500); // Summer Wave Pike
				shop.AddItem("EP12_Artefact_081", 11007081, 1, 1500); // Honored Rose Pike
				shop.AddItem("EP13_Artefact_010", 11007113, 1, 1500); // Rosy Floret Pike
				shop.AddItem("EP13_Artefact_022", 11007125, 1, 1500); // Giltine Pike
				shop.AddItem("EP13_Artefact_124", 11007227, 1, 1500); // TOSummer Pike
				shop.AddItem("EP13_Artefact_128", 11007231, 1, 1500); // Drip Drop Weather Doll Pike
				shop.AddItem("EP13_Artefact_136", 11007239, 1, 1500); // Armonia Pike of Otherworld
				shop.AddItem("EP13_Artefact_149", 11007252, 1, 1500); // Asgard Gungnir Pike
				shop.AddItem("EP12_Re_Artefact_004", 11007328, 1, 1500); // [Re] Fully Blooming Rose Pike
				shop.AddItem("EP12_Re_Artefact_634153", 11007344, 1, 1500); // [Re] Pumpkin Streetlamp Pike
				shop.AddItem("EP12_Re_Artefact_634126", 11007362, 1, 1500); // [Re] Whisk Pike
				shop.AddItem("EP14_Artefact_010", 11007379, 1, 1500); // TOS Metal Electric Base Pike
				shop.AddItem("EP13_Artefact_124_NoTrade", 11007413, 1, 1500); // TOSummer Pike
		});

		PropertyShops.Create("MercenaryRogueShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Daggers
				shop.AddItem("Artefact_634027", 634027, 1, 1500); // Roof Tile Dagger
				shop.AddItem("Artefact_634037", 634037, 1, 1500); // Pumpkin Dagger
				shop.AddItem("Artefact_634052", 634052, 1, 1500); // Fluffy Kitty Dagger
				shop.AddItem("Artefact_634065", 634065, 1, 1500); // Crunchy Choco Dagger
				shop.AddItem("Artefact_634095", 634095, 1, 1500); // Conch Shell Horn Dagger
				shop.AddItem("Artefact_634110", 634110, 1, 1500); // Survival Knife
				shop.AddItem("Artefact_634129", 634129, 1, 1500); // Cheese Knife
				shop.AddItem("Artefact_634130", 634130, 1, 1500); // Two-handed Butcher Knife
				shop.AddItem("Artefact_634133", 634133, 1, 1500); // Table Knife Sword
				shop.AddItem("Artefact_634145", 634145, 1, 1500); // TOS Ranger Dagger
				shop.AddItem("Artefact_634161", 634161, 1, 1500); // Surgical Scalpel Dagger
				shop.AddItem("Artefact_634176", 634176, 1, 1500); // Twinkle Dagger
				shop.AddItem("Artefact_634215", 634215, 1, 1500); // Popo Pop Dagger
				shop.AddItem("EP12_Artefact_011", 11007011, 1, 1500); // Simmering Starlight Dagger
				shop.AddItem("EP12_Artefact_027", 11007027, 1, 1500); // Mayflower Dagger
				shop.AddItem("EP12_Artefact_043", 11007043, 1, 1500); // MusCATeer Dagger
				shop.AddItem("EP12_Artefact_056", 11007056, 1, 1500); // TOS Task Force Dagger
				shop.AddItem("EP12_Artefact_072", 11007072, 1, 1500); // Summer Wave Dagger
				shop.AddItem("EP12_Artefact_088", 11007088, 1, 1500); // Honored Rose Dagger
				shop.AddItem("EP13_Artefact_014", 11007117, 1, 1500); // Sweet Ice Cream Cone Dagger
				shop.AddItem("EP13_Artefact_029", 11007132, 1, 1500); // Giltine Dagger
				shop.AddItem("EP13_Artefact_121", 11007224, 1, 1500); // TOSummer Dagger
				shop.AddItem("EP13_Artefact_131", 11007234, 1, 1500); // Good ol'days Fine-tooth Brush Dagger
				shop.AddItem("EP13_Artefact_139", 11007242, 1, 1500); // Athleisure Dumbbell Dagger
				shop.AddItem("EP13_Artefact_153", 11007256, 1, 1500); // Servant Butterknife Sword
				shop.AddItem("EP12_Re_Artefact_011", 11007335, 1, 1500); // [Re] Simmering Starlight Dagger
				shop.AddItem("EP12_Re_Artefact_634161", 11007352, 1, 1500); // [Re] Surgical Scalpel Dagger
				shop.AddItem("EP12_Re_Artefact_634129", 11007365, 1, 1500); // [Re] Cheese Knife
				shop.AddItem("EP12_Re_Artefact_634130", 11007366, 1, 1500); // [Re] Two-handed Butcher Knife
				shop.AddItem("EP12_Re_Artefact_634133", 11007369, 1, 1500); // [Re] Table Knife Sword
				shop.AddItem("EP14_Artefact_015", 11007384, 1, 1500); // Heliopolis Pharaoh Dagger
				shop.AddItem("EP13_Artefact_121_NoTrade", 11007410, 1, 1500); // TOSummer Dagger
				shop.AddItem("EP15_Artefact_002", 11104008, 1, 1500); // Lofty Snow Dagger
		});

		PropertyShops.Create("MercenaryGunnerShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				// Pistols
				shop.AddItem("Artefact_630044", 630045, 1, 1500); // Marine Pistol
				shop.AddItem("Artefact_630052", 630052, 1, 1500); // Little Whale Pistol
				shop.AddItem("Artefact_634038", 634038, 1, 1500); // Cat Pistol
				shop.AddItem("Artefact_634048", 634048, 1, 1500); // Candy Cane Pistol
				shop.AddItem("Artefact_634080", 634080, 1, 1500); // Crunchy Choco Pistol
				shop.AddItem("Artefact_634084", 634084, 1, 1500); // Impassioned Rose Pistol
				shop.AddItem("Artefact_634104", 634104, 1, 1500); // Mackerel Pistol
				shop.AddItem("Artefact_634132", 634132, 1, 1500); // Hand Mixer Pistol
				shop.AddItem("Artefact_634146", 634146, 1, 1500); // TOS Ranger Pistol
				shop.AddItem("Artefact_634162", 634162, 1, 1500); // Vital Sign Pistol
				shop.AddItem("Artefact_634204", 634204, 1, 1500); // Magical Savior Pistol
				shop.AddItem("Artefact_634214", 634214, 1, 1500); // Popop Pop Pistol
				shop.AddItem("EP12_Artefact_015", 11007015, 1, 1500); // West Biplane Pistol
				shop.AddItem("EP12_Artefact_031", 11007031, 1, 1500); // Mayflower Pistol
				shop.AddItem("EP12_Artefact_060", 11007060, 1, 1500); // TOS Task Force Pistol
				shop.AddItem("EP12_Artefact_076", 11007076, 1, 1500); // Summer Wave Pistol
				shop.AddItem("EP12_Artefact_092", 11007092, 1, 1500); // Honored Rose Pistol
				shop.AddItem("EP13_Artefact_003", 11007106, 1, 1500); // Task Unit Green Railgun Pistol
				shop.AddItem("EP13_Artefact_007", 11007110, 1, 1500); // Ice Cold Fish-shaped Bun Pistol
				shop.AddItem("EP13_Artefact_033", 11007136, 1, 1500); // Giltine Pistol
				shop.AddItem("EP13_Artefact_138", 11007241, 1, 1500); // Armonia Pistol of Otherworld
				shop.AddItem("EP13_Artefact_142", 11007245, 1, 1500); // Athleisure Massage Gun Pistol
				shop.AddItem("EP12_Re_Artefact_015", 11007339, 1, 1500); // [Re] West Biplane Pistol
				shop.AddItem("EP12_Re_Artefact_634162", 11007353, 1, 1500); // [Re] Vital Sign Pistol
				shop.AddItem("EP12_Re_Artefact_634132", 11007368, 1, 1500); // [Re] Hand Mixer Pistol
				shop.AddItem("EP14_Artefact_005", 11007374, 1, 1500); // Tos Hero Pistol
				shop.AddItem("EP14_Artefact_011", 11007380, 1, 1500); // TOS Metal Pump Pistol
				// Muskets
				shop.AddItem("Artefact_630045", 630046, 1, 1500); // Marine Musket
				shop.AddItem("Artefact_634039", 634039, 1, 1500); // Red Syringe Musket
				shop.AddItem("Artefact_634044", 634044, 1, 1500); // Candy Cane Musket
				shop.AddItem("Artefact_634079", 634079, 1, 1500); // Crunchy Choco Musket
				shop.AddItem("Artefact_634083", 634083, 1, 1500); // Impassioned Rose Musket
				shop.AddItem("Artefact_634102", 634102, 1, 1500); // Toucan Toy Musket
				shop.AddItem("Artefact_634143", 634143, 1, 1500); // TOS Ranger Musket
				shop.AddItem("Artefact_634159", 634159, 1, 1500); // Pumpkin Mage Musket
				shop.AddItem("Artefact_634174", 634174, 1, 1500); // Christmas Sock Musket
				shop.AddItem("EP12_Artefact_012", 11007012, 1, 1500); // Boa Constrictor Musket
				shop.AddItem("EP12_Artefact_028", 11007028, 1, 1500); // Mayflower Musket
				shop.AddItem("EP12_Artefact_057", 11007057, 1, 1500); // TOS Task Force Musket
				shop.AddItem("EP12_Artefact_073", 11007073, 1, 1500); // Summer Wave Musket
				shop.AddItem("EP12_Artefact_089", 11007089, 1, 1500); // Honored Rose Musket
				shop.AddItem("EP13_Artefact_002", 11007105, 1, 1500); // Task Unit Red Railgun Musket
				shop.AddItem("EP13_Artefact_018", 11007121, 1, 1500); // Criminal Submachine Gun Musket
				shop.AddItem("EP13_Artefact_030", 11007133, 1, 1500); // Giltine Musket
				shop.AddItem("EP13_Artefact_137", 11007240, 1, 1500); // Armonia Musket of Otherworld
				shop.AddItem("EP13_Artefact_150", 11007253, 1, 1500); // Servant Ceremonial Musket
				shop.AddItem("EP12_Re_Artefact_012", 11007336, 1, 1500); // [Re] Boa Constrictor Musket
				shop.AddItem("EP12_Re_Artefact_634159", 11007350, 1, 1500); // [Re] Pumpkin Mage Musket
				// Cannons
				shop.AddItem("Artefact_634020", 634020, 1, 1500); // Moon Rabbit Cannon
				shop.AddItem("Artefact_634047", 634047, 1, 1500); // Gift Pouch Cannon
				shop.AddItem("Artefact_634078", 634078, 1, 1500); // Crunchy Choco Cannon
				shop.AddItem("Artefact_634082", 634082, 1, 1500); // Impassioned Rose Cannon
				shop.AddItem("Artefact_634103", 634103, 1, 1500); // Octo-cannon
				shop.AddItem("Artefact_634117", 634117, 1, 1500); // Canned Kepa Cannon
				shop.AddItem("Artefact_634131", 634131, 1, 1500); // Pepper Grinder Cannon
				shop.AddItem("Artefact_634144", 634144, 1, 1500); // TOS Ranger Cannon
				shop.AddItem("Artefact_634160", 634160, 1, 1500); // Heavy Coffin Cannon
				shop.AddItem("Artefact_634175", 634175, 1, 1500); // Rudolf Cannon
				shop.AddItem("Artefact_634192", 634192, 1, 1500); // Giant Bell Cannon
				shop.AddItem("Artefact_634213", 634213, 1, 1500); // Popo Pop Cannon
				shop.AddItem("EP12_Artefact_008", 11007008, 1, 1500); // Rose and the Fox Cannon
				shop.AddItem("EP12_Artefact_024", 11007024, 1, 1500); // Mayflower Cannon
				shop.AddItem("EP12_Artefact_040", 11007040, 1, 1500); // MusCATeer Cannon
				shop.AddItem("EP12_Artefact_053", 11007053, 1, 1500); // TOS Task Force Cannon
				shop.AddItem("EP12_Artefact_069", 11007069, 1, 1500); // Summer Wave Cannon
				shop.AddItem("EP12_Artefact_085", 11007085, 1, 1500); // Honored Rose Cannon
				shop.AddItem("EP12_Artefact_098", 11007098, 1, 1500); // Littleberk Pillow Cannon
				shop.AddItem("EP13_Artefact_004", 11007107, 1, 1500); // Ice Cold Sweet Potato Drum Cannon
				shop.AddItem("EP13_Artefact_017", 11007120, 1, 1500); // Criminal Dynamite Cannon
				shop.AddItem("EP13_Artefact_026", 11007129, 1, 1500); // Giltine Cannon
				shop.AddItem("EP12_Re_Artefact_008", 11007332, 1, 1500); // [Re] Rose and the Fox Cannon
				shop.AddItem("EP12_Re_Artefact_634160", 11007351, 1, 1500); // [Re] Heavy Coffin Cannon
				shop.AddItem("EP12_Re_Artefact_634131", 11007367, 1, 1500); // [Re] Pepper Grinder Cannon
				shop.AddItem("EP14_Artefact_014", 11007383, 1, 1500); // Heliopolis Anubis Cannon
				shop.AddItem("EP14_Artefact_019", 11007388, 1, 1500); // Major Arcana The Star Cannon
		});

		PropertyShops.Create("MercenaryMaleCostumeShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
			shop.AddItem("costume_Com_253", 633253, 1, 3000); // Pyrmas Tantalizer Costume
			shop.AddItem("costume_Com_254", 633254, 1, 3000); // Pyrmas Moringponia Costume
			shop.AddItem("costume_Com_255", 633255, 1, 3000); // Black Rabbit Costume
			shop.AddItem("costume_Com_256", 633256, 1, 3000); // Candy Killer Rascal Costume (Male)
			shop.AddItem("costume_Com_258", 633258, 1, 3000); // Haunted Hospital Nurse Costume (Male)
			shop.AddItem("costume_Com_260", 633260, 1, 3000); // Skull Party Skeleton Costume (Male)
			shop.AddItem("costume_Com_264", 633264, 1, 3000); // Antras Tantalizer Costume
			shop.AddItem("costume_Com_265", 633265, 1, 3000); // Antras Moringponia Costume
			shop.AddItem("costume_Com_269", 633269, 1, 3000); // Gabija Guardian Costume (Male)
			shop.AddItem("costume_Com_271", 633271, 1, 3000); // Gabija Priest Costume (Male)
			shop.AddItem("costume_Com_273", 633273, 1, 3000); // Sparking Tree Trunk Costume
			shop.AddItem("costume_Com_274", 633274, 1, 3000); // Platina Dafne Costume (Male)
			shop.AddItem("costume_Com_276", 633276, 1, 3000); // Crimson Noel Costume (Male)
			shop.AddItem("costume_Com_278", 633278, 1, 3000); // Midnight Snow Costume (Male)
			shop.AddItem("costume_Com_282", 633282, 1, 3000); // Zaura Costume
			shop.AddItem("costume_Com_284", 633284, 1, 3000); // Heaven Guardian Armor (Male)
			shop.AddItem("costume_Com_286", 633286, 1, 3000); // Earth Guardian Armor (Male)
			shop.AddItem("costume_Com_290", 633290, 1, 3000); // Magical Savior - Clover Costume (Male)
			shop.AddItem("costume_Com_292", 633292, 1, 3000); // Magical Savior - Professor Costume (Male)
			shop.AddItem("costume_Com_302", 633302, 1, 3000); // Popo Pop Neon Activity Costume (Male)
			shop.AddItem("costume_Com_306", 633306, 1, 3000); // Popo Pop Funky Graffiti Costume (Male)
			shop.AddItem("costume_Com_308", 633308, 1, 3000); // Popo Pop Main Stage Costume (Male)
			shop.AddItem("costume_Com_310", 633310, 1, 3000); // Bramble Costume
			shop.AddItem("costume_Com_311", 633311, 1, 3000); // Kimono Costume (Male)
			shop.AddItem("costume_Com_313", 633313, 1, 3000); // Flower Deer Costume
			shop.AddItem("costume_Com_314", 633314, 1, 3000); // Solcomm Costume
			shop.AddItem("costume_Com_315", 633315, 1, 3000); // Naktis Costume
			shop.AddItem("costume_Com_316", 633318, 1, 3000); // Panda Costume
			shop.AddItem("costume_Com_317", 633333, 1, 3000); // Marina Summer Costume (Male)
			shop.AddItem("costume_Com_319", 633335, 1, 3000); // Scarlet Summer Costume (Male)
			shop.AddItem("EP12_costume_Com_001", 11004001, 1, 3000); // Starlight Prince Costume (Male)
			shop.AddItem("EP12_costume_Com_003", 11004003, 1, 3000); // Romantic Pilot Costume (Male)
			shop.AddItem("EP12_costume_Com_005", 11004005, 1, 3000); // Red Fox Costume
			shop.AddItem("EP12_costume_Com_006", 11004006, 1, 3000); // Mayflower Noble Plate Costume (Male)
			shop.AddItem("EP12_costume_Com_008", 11004008, 1, 3000); // Mayflower Wedding Tuxedo Costume (Male)
			shop.AddItem("EP12_costume_Com_010", 11004010, 1, 3000); // Mayflower Innocent Armor Costume (Male)
			shop.AddItem("EP12_costume_Com_012", 11004012, 1, 3000); // MusCATeer Baron Tabby Costume (Male)
			shop.AddItem("EP12_costume_Com_014", 11004014, 1, 3000); // MusCATeer Merchant Black Costume (Male)
			shop.AddItem("EP12_costume_Com_016", 11004016, 1, 3000); // MusCATeer Soldier Cookie Costume (Male)
			shop.AddItem("EP12_costume_Com_030", 11004030, 1, 3000); // Summer Wave Sailor Shirt (Male)
			shop.AddItem("EP12_costume_Com_032", 11004032, 1, 3000); // Summer Waver Swim Brief (Male)
			shop.AddItem("EP12_costume_Com_034", 11004034, 1, 3000); // Summer Wave Hawaiian Shirt (Male)
			shop.AddItem("EP12_costume_Com_036", 11004036, 1, 3000); // Honored Rose Royal Black Suit (Male)
			shop.AddItem("EP12_costume_Com_037", 11004037, 1, 3000); // Honored Rose Crimson Suit (Male)
			shop.AddItem("EP12_costume_Com_038", 11004038, 1, 3000); // Honored Rose Violet Suit (Male)
			shop.AddItem("EP12_costume_Com_042", 11004042, 1, 3000); // Honored Rose Arrogant Red Suit (Male)
			shop.AddItem("EP12_costume_Com_043", 11004043, 1, 3000); // Honored Rose Purple Suit (Male)
			shop.AddItem("EP12_costume_Com_044", 11004044, 1, 3000); // Honored Rose Green Suit (Male)
			shop.AddItem("EP12_costume_Com_048", 11004048, 1, 3000); // Honored Rose Superb Pink Suit (Male)
			shop.AddItem("EP12_costume_Com_049", 11004049, 1, 3000); // Honored Rose Blue Suit (Male)
			shop.AddItem("EP12_costume_Com_050", 11004050, 1, 3000); // Honored Rose Yellow Suit (Male)
			shop.AddItem("EP12_costume_Com_054", 11004054, 1, 3000); // Masquerade Mysterious Doctor Costume (Male)
			shop.AddItem("EP12_costume_Com_058", 11004058, 1, 3000); // Masquerade Horror Frankenstein Costume (Male)
			shop.AddItem("EP12_costume_Com_072", 11004072, 1, 3000); // Masquerade Phantom of Music Costume (Male)
			shop.AddItem("EP12_costume_Com_076", 11004076, 1, 3000); // Masquerade Simple Tux Costume (Male)
			shop.AddItem("EP12_costume_Com_078", 11004086, 1, 3000); // Littleberk Character Pajama Costume (Male)
			shop.AddItem("EP12_costume_Com_080", 11004088, 1, 3000); // Littleberk Custom Battle Uniform (Male)
			shop.AddItem("EP12_costume_Com_082", 11004090, 1, 3000); // Littleberk Fluffy Pajama Costume (Male)
			shop.AddItem("EP12_costume_Com_084", 11004092, 1, 3000); // Littleberk Sweet Dream Pajama Costume (Male)
			shop.AddItem("EP12_costume_Com_086", 11004094, 1, 3000); // Littleberk Classy Battle Costume (Male)
			shop.AddItem("EP12_costume_Com_088", 11004096, 1, 3000); // Littleberk Finest Daily Wear Costume (Male)
			shop.AddItem("EP12_costume_Com_096", 11004112, 1, 3000); // Evening Star Prestige Costume (Male)
			shop.AddItem("EP12_costume_Com_098", 11004114, 1, 3000); // Evening Star Bright Costume (Male)
			shop.AddItem("EP12_costume_Com_102", 11004118, 1, 3000); // Evening Star Aurora Costume (Male)
			shop.AddItem("EP12_costume_Com_104", 11004120, 1, 3000); // Evening Star Nebula Costume (Male)
			shop.AddItem("EP13_costume_Com_000", 11004130, 1, 3000); // Task Unit Alpha Patrol Costume (Male)
			shop.AddItem("EP13_costume_Com_002", 11004132, 1, 3000); // Task Unit Alpha Troop Costume (Male)
			shop.AddItem("EP13_costume_Com_018", 11004148, 1, 3000); // Ice Cold Hoodie Bubble Jacket Costume (Male)
			shop.AddItem("EP13_costume_Com_036", 11004166, 1, 3000); // Rosy Floret Scarlet Blossom Costume (Male)
			shop.AddItem("EP13_costume_Com_040", 11004170, 1, 3000); // Rosy Floret Powder Pink Costume (Male)
			shop.AddItem("EP13_costume_Com_054", 11004184, 1, 3000); // Sweet Mint Chocolate Costume (Male)
			shop.AddItem("EP13_costume_Com_056", 11004186, 1, 3000); // Sweet Cream Cookie Costume (Male)
			shop.AddItem("EP13_costume_Com_072", 11004212, 1, 3000); // Criminal Mafia Godfather Costume (Male)
			shop.AddItem("EP13_costume_Com_074", 11004214, 1, 3000); // Criminal Young Boss Costume (Male)
			shop.AddItem("EP13_costume_Com_080", 11004220, 1, 3000); // Criminal Best Detective Costume (Male)
			shop.AddItem("EP13_costume_Com_090", 11004230, 1, 3000); // STEM Speciality Scientist Costume (Male)
			shop.AddItem("EP13_costume_Com_092", 11004232, 1, 3000); // STEM Laboratory Scientist Costume (Male)
			shop.AddItem("EP13_costume_Com_108", 11004250, 1, 3000); // TOSummer Passion Costume (Male)
			shop.AddItem("EP13_costume_Com_126", 11004286, 1, 3000); // Drip Drop Giraffe Raincoat Costume (Male)
			shop.AddItem("EP13_costume_Com_144", 11004304, 1, 3000); // Good ol'days Legendary Star Costume (Male)
			shop.AddItem("EP13_costume_Com_158", 11004318, 1, 3000); // Blue Serpent of Otherworld Costume (Male)
			shop.AddItem("EP13_costume_Com_160", 11004320, 1, 3000); // Le Blanc of Otherworld Costume (Male)
			shop.AddItem("EP13_costume_Com_162", 11004322, 1, 3000); // Magician of Otherworld Costume (Male)
			shop.AddItem("EP13_costume_Com_164", 11004324, 1, 3000); // Gunman of Otherworld Costume (Male)
			shop.AddItem("EP13_costume_Com_166", 11004326, 1, 3000); // Warrior of Otherworld Costume (Male)
			shop.AddItem("EP13_costume_Com_170", 11004333, 1, 3000); // Athleisure Strength Costume (Male)
			shop.AddItem("EP13_costume_Com_182", 11004345, 1, 3000); // Asgard Thunder God Thor Costume (Male)
			shop.AddItem("EP13_costume_Com_186", 11004349, 1, 3000); // Asgard Trickster Loki Costume (Male)
			shop.AddItem("EP13_costume_Com_190", 11004353, 1, 3000); // Asgard Sea God Njord Costume (Male)
			shop.AddItem("EP13_costume_Com_194", 11004358, 1, 3000); // Servant Royal Butler Costume (Male)
			shop.AddItem("EP13_costume_Com_196", 11004360, 1, 3000); // Servant Second Butler Costume (Male)
			shop.AddItem("EP13_costume_Com_198", 11004362, 1, 3000); // Servant Butler Knight Costume (Male)
			shop.AddItem("EP13_costume_Com_200", 11004364, 1, 3000); // Servant Butler Guard Costume (Male)
			shop.AddItem("EP13_costume_Com_202", 11004366, 1, 3000); // Servant Butler Sergeant Costume (Male)
			shop.AddItem("EP13_costume_Com_206", 11004370, 1, 3000); // Animal Kindergarten Pink Bunny Costume (Male)
			shop.AddItem("EP13_costume_Com_208", 11004372, 1, 3000); // Animal Kindergarten White Duck Costume (Male)
			shop.AddItem("EP14_costume_Com_001", 11004386, 1, 3000); // Eternal Savior Lumen Costume (Male)
			shop.AddItem("EP14_costume_Com_003", 11004388, 1, 3000); // Eternal Savior Nox Costume (Male)
			shop.AddItem("EP14_costume_Com_015", 11004410, 1, 3000); // Knight of Zemyna Defender Costume (Male)
			shop.AddItem("EP14_costume_Com_019", 11004414, 1, 3000); // TOS Metal Leopard Costume (Male)
			shop.AddItem("EP14_costume_Com_023", 11004420, 1, 3000); // Heliopolis Horus Costume (Male)
			shop.AddItem("EP14_costume_Com_027", 11004427, 1, 3000); // Major Arcana The Sun Costume (Male)
			shop.AddItem("EP14_costume_Com_031", 11004432, 1, 3000); // Furry Sky Doggy Costume (Male)
			shop.AddItem("EP15_costume_Com_001", 11101001, 1, 3000); // Lofty Snow Squire Costume (Male)
			shop.AddItem("EP15_costume_Com_003", 11101003, 1, 3000); // Lofty Snow Knight Costume (Male)		
		});

			PropertyShops.Create("MercenaryFemaleCostumeShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				shop.AddItem("costume_Com_257", 633257, 1, 3000); // Candy Killer Clown Costume (Female)
				shop.AddItem("costume_Com_259", 633259, 1, 3000); // Haunted Hospital Nurse Costume (Female)
				shop.AddItem("costume_Com_261", 633261, 1, 3000); // Skull Party Skeleton Costume (Female)
				shop.AddItem("costume_Com_270", 633270, 1, 3000); // Gabija Guardian Costume (Female)
				shop.AddItem("costume_Com_272", 633272, 1, 3000); // Gabija Priestess Costume (Female)
				shop.AddItem("costume_Com_275", 633275, 1, 3000); // Platina Dafne Costume (Female)
				shop.AddItem("costume_Com_277", 633277, 1, 3000); // Crimson Noel Costume (Female)
				shop.AddItem("costume_Com_279", 633279, 1, 3000); // Midnight Snow Costume (Female)
				shop.AddItem("costume_Com_283", 633283, 1, 3000); // Nuaele Costume
				shop.AddItem("costume_Com_285", 633285, 1, 3000); // Heaven Guardian Armor (Female)
				shop.AddItem("costume_Com_287", 633287, 1, 3000); // Earth Guardian Armor (Female)
				shop.AddItem("costume_Com_291", 633291, 1, 3000); // Magical Savior - Clover Costume (Female)
				shop.AddItem("costume_Com_293", 633293, 1, 3000); // Magical Savior - Professor Costume (Female)
				shop.AddItem("costume_Com_303", 633303, 1, 3000); // Popo Pop Neon Activity Costume (Female)
				shop.AddItem("costume_Com_307", 633307, 1, 3000); // Popo Pop Funky Graffiti Costume (Female)
				shop.AddItem("costume_Com_309", 633309, 1, 3000); // Popo Pop Main Stage Costume (Female)
				shop.AddItem("costume_Com_312", 633312, 1, 3000); // Kimono Costume (Female)
				shop.AddItem("costume_Com_318", 633334, 1, 3000); // Marina Summer Costume (Female)
				shop.AddItem("costume_Com_320", 633336, 1, 3000); // Scarlet Summer Costume (Female)
				shop.AddItem("EP12_costume_Com_002", 11004002, 1, 3000); // Starlight Princess Costume (Female)
				shop.AddItem("EP12_costume_Com_004", 11004004, 1, 3000); // Romantic Pilot Costume (Female)
				shop.AddItem("EP12_costume_Com_007", 11004007, 1, 3000); // Mayflower Noble Plate Costume (Female)
				shop.AddItem("EP12_costume_Com_009", 11004009, 1, 3000); // Mayflower Wedding Dress Costume (Female)
				shop.AddItem("EP12_costume_Com_011", 11004011, 1, 3000); // Mayflower Innocent Armor Costume (Female)
				shop.AddItem("EP12_costume_Com_013", 11004013, 1, 3000); // MusCATeer Baron Cheese Costume (Female)
				shop.AddItem("EP12_costume_Com_015", 11004015, 1, 3000); // MusCATeer Merchant Ragdoll Costume (Female)
				shop.AddItem("EP12_costume_Com_017", 11004017, 1, 3000); // MusCATeer Soldier Shorthair Costume (Female)
				shop.AddItem("EP12_costume_Com_031", 11004031, 1, 3000); // Summer Wave Sky Blue Off-shoulder Bikini (Female)
				shop.AddItem("EP12_costume_Com_033", 11004033, 1, 3000); // Summer Wave Red Black Bikini (Female)
				shop.AddItem("EP12_costume_Com_035", 11004035, 1, 3000); // Summer Wave White Frill Bikini (Female)
				shop.AddItem("EP12_costume_Com_039", 11004039, 1, 3000); // Honored Rose Royal Black Dress (Female)
				shop.AddItem("EP12_costume_Com_040", 11004040, 1, 3000); // Honored Rose Crimson Dress (Female)
				shop.AddItem("EP12_costume_Com_041", 11004041, 1, 3000); // Honored Rose Violet Dress (Female)
				shop.AddItem("EP12_costume_Com_045", 11004045, 1, 3000); // Honored Rose Arrogant Red Dress (Female)
				shop.AddItem("EP12_costume_Com_046", 11004046, 1, 3000); // Honored Rose Purple Dress (Female)
				shop.AddItem("EP12_costume_Com_047", 11004047, 1, 3000); // Honored Rose Green Dress (Female)
				shop.AddItem("EP12_costume_Com_051", 11004051, 1, 3000); // Honored Rose Superb Pink Dress (Female)
				shop.AddItem("EP12_costume_Com_052", 11004052, 1, 3000); // Honored Rose Blue Dress (Female)
				shop.AddItem("EP12_costume_Com_053", 11004053, 1, 3000); // Honored Rose Yellow Dress (Female)
				shop.AddItem("EP12_costume_Com_055", 11004055, 1, 3000); // Masquerade Mysterious Witch Costume (Female)
				shop.AddItem("EP12_costume_Com_059", 11004059, 1, 3000); // Masquerade Horror Ghost Costume (Female)
				shop.AddItem("EP12_costume_Com_073", 11004073, 1, 3000); // Masquerade White Swan Costume (Female)
				shop.AddItem("EP12_costume_Com_077", 11004077, 1, 3000); // Masquerade Simple Tutu Costume (Female)
				shop.AddItem("EP12_costume_Com_079", 11004087, 1, 3000); // Maru Character Pajama Costume (Female)
				shop.AddItem("EP12_costume_Com_081", 11004089, 1, 3000); // Maru Custom Battle Uniform (Female)
				shop.AddItem("EP12_costume_Com_083", 11004091, 1, 3000); // Maru Fluffy Pajama Costume (Female)
				shop.AddItem("EP12_costume_Com_085", 11004093, 1, 3000); // Maru Sweet Dream Pajama Costume (Female)
				shop.AddItem("EP12_costume_Com_087", 11004095, 1, 3000); // Maru Classy Battle Costume (Female)
				shop.AddItem("EP12_costume_Com_089", 11004097, 1, 3000); // Maru Finest Daily Wear Costume (Female)
				shop.AddItem("EP12_costume_Com_097", 11004113, 1, 3000); // Evening Star Prestige Costume (Female)
				shop.AddItem("EP12_costume_Com_099", 11004115, 1, 3000); // Evening Star Bright Costume (Female)
				shop.AddItem("EP12_costume_Com_103", 11004119, 1, 3000); // Evening Star Aurora Costume (Female)
				shop.AddItem("EP12_costume_Com_105", 11004121, 1, 3000); // Evening Star Nebula Costume (Female)
				shop.AddItem("EP13_costume_Com_001", 11004131, 1, 3000); // Task Unit Alpha Patrol Costume (Female)
				shop.AddItem("EP13_costume_Com_003", 11004133, 1, 3000); // Task Unit Alpha Troop Costume (Female)
				shop.AddItem("EP13_costume_Com_019", 11004149, 1, 3000); // Ice Cold Hoodie Bubble Jacket Costume (Female)
				shop.AddItem("EP13_costume_Com_037", 11004167, 1, 3000); // Rosy Floret Scarlet Blossom Costume (Female)
				shop.AddItem("EP13_costume_Com_041", 11004171, 1, 3000); // Rosy Floret Powder Pink Costume (Female)
				shop.AddItem("EP13_costume_Com_055", 11004185, 1, 3000); // Sweet Mint Chocolate Costume (Female)
				shop.AddItem("EP13_costume_Com_057", 11004187, 1, 3000); // Sweet Cream Cookie Costume (Female)
				shop.AddItem("EP13_costume_Com_073", 11004213, 1, 3000); // Mafia Godmother Costume
				shop.AddItem("EP13_costume_Com_075", 11004215, 1, 3000); // Criminal Young Boss Costume (Female)
				shop.AddItem("EP13_costume_Com_081", 11004221, 1, 3000); // Criminal Best Detective Costume (Female)
				shop.AddItem("EP13_costume_Com_091", 11004231, 1, 3000); // STEM Speciality Scientist Costume (Female)
				shop.AddItem("EP13_costume_Com_093", 11004233, 1, 3000); // STEM Laboratory Scientist Costume (Female)
				shop.AddItem("EP13_costume_Com_109", 11004251, 1, 3000); // TOSummer Passion Costume (Female)
				shop.AddItem("EP13_costume_Com_127", 11004287, 1, 3000); // Drip Drop Giraffe Raincoat Costume (Female)
				shop.AddItem("EP13_costume_Com_145", 11004305, 1, 3000); // Good ol'days Legendary Star Costume (Female)
				shop.AddItem("EP13_costume_Com_159", 11004319, 1, 3000); // Blue Serpent of Otherworld Costume (Female)
				shop.AddItem("EP13_costume_Com_161", 11004321, 1, 3000); // Le Blanc of Otherworld Costume (Female)
				shop.AddItem("EP13_costume_Com_163", 11004323, 1, 3000); // Magician of Otherworld Costume (Female)
				shop.AddItem("EP13_costume_Com_165", 11004325, 1, 3000); // Gunman of Otherworld Costume (Female)
				shop.AddItem("EP13_costume_Com_167", 11004327, 1, 3000); // Warrior of Otherworld Costume (Female)
				shop.AddItem("EP13_costume_Com_171", 11004334, 1, 3000); // Athleisure Strength Costume (Female)
				shop.AddItem("EP13_costume_Com_183", 11004346, 1, 3000); // Asgard Serpant Jormungand Costume (Female)
				shop.AddItem("EP13_costume_Com_187", 11004350, 1, 3000); // Asgard Wolf Beast Fenrir Costume (Female)
				shop.AddItem("EP13_costume_Com_191", 11004354, 1, 3000); // Asgard Winter Goddess Skathi Costume (Female)
				shop.AddItem("EP13_costume_Com_195", 11004359, 1, 3000); // Servant Royal Maid Costume (Female)
				shop.AddItem("EP13_costume_Com_197", 11004361, 1, 3000); // Servant Second Maid Costume (Female)
				shop.AddItem("EP13_costume_Com_199", 11004363, 1, 3000); // Servant Maid Knight Costume (Female)
				shop.AddItem("EP13_costume_Com_201", 11004365, 1, 3000); // Servant Maid Guard Costume (Female)
				shop.AddItem("EP13_costume_Com_203", 11004367, 1, 3000); // Servant Maid Sergeant Costume (Female)
				shop.AddItem("EP13_costume_Com_207", 11004371, 1, 3000); // Animal Kindergarten Strawberry Moo Costume (Female)
				shop.AddItem("EP13_costume_Com_209", 11004373, 1, 3000); // Animal Kindergarten Red Fox Costume (Female)
				shop.AddItem("EP14_costume_Com_002", 11004387, 1, 3000); // Eternal Savior Lumen Costume (Female)
				shop.AddItem("EP14_costume_Com_004", 11004389, 1, 3000); // Eternal Savior Nox Costume (Female)
				shop.AddItem("EP14_costume_Com_016", 11004411, 1, 3000); // Knight of Zemyna Defender Costume (Female)
				shop.AddItem("EP14_costume_Com_020", 11004415, 1, 3000); // TOS Metal Leopard Costume (Female)
				shop.AddItem("EP14_costume_Com_024", 11004421, 1, 3000); // Heliopolis Bastet Costume (Female)
				shop.AddItem("EP14_costume_Com_028", 11004428, 1, 3000); // Major Arcana The Star Costume (Female)
				shop.AddItem("EP14_costume_Com_032", 11004433, 1, 3000); // Furry Golden Fox Costume (Female)
				shop.AddItem("EP15_costume_Com_002", 11101002, 1, 3000); // Lofty Snow Squire Costume (Female)
				shop.AddItem("EP15_costume_Com_004", 11101004, 1, 3000); // Lofty Snow Knight Costume (Female)
		});

		PropertyShops.Create("MercenaryWingShop", MercenaryShopPointName, MercenaryCurrencyProperty, shop =>
		{
				shop.AddItem("Wing_Heart", 637001, 1, 5000); // Broken Pink Heart Wings
				shop.AddItem("Wing_Falcon", 637002, 1, 5000); // Small Steam Punk Wings
				shop.AddItem("Wing_Angel", 637003, 1, 5000); // Small Mint Angel Wings
				shop.AddItem("Wing_Bat", 637004, 1, 5000); // Bat Wings
				shop.AddItem("Wing_Net", 637005, 1, 5000); // Spider Web Wings
				shop.AddItem("Wing_Guilty", 637006, 1, 5000); // Giltine's Wings
				shop.AddItem("Wing_Ice", 637007, 1, 5000); // Ice Crystal Wings
				shop.AddItem("Wing_Flower01", 637010, 1, 5000); // Golden Fairy Wings
				shop.AddItem("Wing_Flower02", 637011, 1, 5000); // Dawn Fairy Wings
				shop.AddItem("Wing_Flower03", 637012, 1, 5000); // Teeny Fairy Wings
				shop.AddItem("wing_Guilty_Little", 637025, 1, 5000); // Small Giltine Wings
				shop.AddItem("wing_winter_star", 637027, 1, 5000); // Shining Star Wings
				shop.AddItem("wing_winter_bell", 637028, 1, 5000); // Jingle Bell Wings
				shop.AddItem("wing_sea_wave", 637030, 1, 5000); // Emerald Beach Wings
				shop.AddItem("wing_tantaliser1", 637034, 1, 5000); // Pyrmas Tantalizer Wings
				shop.AddItem("wing_moth1", 637035, 1, 5000); // Pyrmas Moringponia Wings
				shop.AddItem("wing_tantaliser2", 637037, 1, 5000); // Antras Tantalizer Wings
				shop.AddItem("wing_moth2", 637038, 1, 5000); // Antras Moringponia Wings
				shop.AddItem("wing_gabia", 637039, 1, 5000); // Gabija Wings
				shop.AddItem("wing_12animal01", 637044, 1, 5000); // Pink Strand Wing
				shop.AddItem("wing_12animal02", 637045, 1, 5000); // Red Strand Wing
				shop.AddItem("wing_12animal03", 637046, 1, 5000); // Green Strand Wing
				shop.AddItem("wing_20magical01", 637047, 1, 5000); // Magical Savior Wing - Spade
				shop.AddItem("wing_20magical02", 637048, 1, 5000); // Magical Savior Wing - Clover
				shop.AddItem("wing_lecifer", 637049, 1, 5000); // Rexipher Wing
				shop.AddItem("wing_froster_lord", 637051, 1, 5000); // Froster Lord Wings
				shop.AddItem("Wing_Guilty_Raid", 637058, 1, 5000); // Giltine's Eroded Wings
				shop.AddItem("wing_asiomage", 637060, 1, 5000); // Asiomage's Wing
				shop.AddItem("wing_quipmage", 637061, 1, 5000); // Quipmage's Wing
				shop.AddItem("wing_diena", 637062, 1, 5000); // Diena Wing
				shop.AddItem("wing_ruby_sea_wave", 637064, 1, 5000); // Ruby Wave Wings
				shop.AddItem("wing_sapphire_sea_wave", 637065, 1, 5000); // Sapphire Wave Wings
				shop.AddItem("wing_summer_wave", 11008003, 1, 5000); // Sandy Beach Wave Wing
				shop.AddItem("wing_ep12honoredrose01", 11008004, 1, 5000); // Royal Rose Great Wing
				shop.AddItem("wing_ep12musical", 11008006, 1, 5000); // Masquerade Crimson Demon Wing
				shop.AddItem("wing_goddessLaima", 11008007, 1, 5000); // Laima's Wing
				shop.AddItem("wing_ep13cybersoldier", 11008011, 1, 5000); // Task Unit Plazma Wing
				shop.AddItem("wing_ep13toswinter", 11008012, 1, 5000); // Ice Cold Snow Glacial Wing
				shop.AddItem("wing_botruta", 11008014, 1, 5000); // Boruta Wings
				shop.AddItem("wing_ep13mafia", 11008016, 1, 5000); // Criminal Supreme Hitman Wing
				shop.AddItem("wing_ep13ge", 11008025, 1, 5000); // Rose Wing of Otherworld
				shop.AddItem("wing_ep14magical01", 11008029, 1, 5000); // Eternal Savior Lumen Wing
				shop.AddItem("wing_ep14magical02", 11008030, 1, 5000); // Eternal Savior Nox Wing
				shop.AddItem("wing_vakarine", 11008031, 1, 5000); // Goddess Vakarine's Twilight Star Wing
				shop.AddItem("wing_ep14egypt", 11008037, 1, 5000); // Wing of Ra
				shop.AddItem("wing_poporion", 11008043, 1, 5000); // Popolion Wing
				shop.AddItem("wing_ep15unicorn", 11105006, 1, 5000); // Unicorn PJs Wing
				shop.AddItem("wing_7th_helena", 11105007, 1, 5000); // Helena Morpho Butterfly Wing
				shop.AddItem("wing_ep15elf", 11105009, 1, 5000); // Faerie Queene Wing
				shop.AddItem("wing_upinis", 11105011, 1, 5000); // Upinis Wing
				shop.AddItem("wing_slogutis", 11105012, 1, 5000); // Slogutis Wing
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: Mythic Hunters' Ledger
//-----------------------------------------------------------------------------

public class MythicHuntersLedgerQuest : QuestScript
{
	protected override void Load()
	{
		this.SetId("c_request_1", 1001);
		this.SetName(L("Mythic Hunters' Ledger"));
		this.SetType(QuestType.Repeat);
		this.SetDescription(L("Captain Davros Renn of the Fedimian Mercenary Guild pays fifty badges for every mythic beast you bring down. Mythics prowl only the most dangerous maps, and the ledger never closes — unredeemed kills stay on the books until you return to collect."));
		this.SetLocation("c_request_1");
		this.SetAutoTracked(true);

		this.SetReceive(QuestReceiveType.Manual);
		this.SetCancelable(true);
		this.SetUnlock(QuestUnlockType.AllAtOnce);
		this.AddQuestGiver(L("[Captain] Davros Renn"), "c_request_1");

		this.AddObjective("killMythics", L("Hunt Mythic Monsters"),
			new UnlimitedKillObjective((mob, ch) =>
			{
				if (!mob.IsMythicMonster())
					return false;

				ch.Variables.Perm.Set(CRequest1NpcScript.MythicKillsVariable,
					ch.Variables.Perm.GetInt(CRequest1NpcScript.MythicKillsVariable, 0) + 1);

				return true;
			}));
	}
}
