//--- Melia Script ----------------------------------------------------------
// Vieta Gorge - Quest NPCs
//--- Description -----------------------------------------------------------
// Quest NPCs and content for f_huevillage_58_2. Steep gorge country where
// trappers work the Ultanun warrens and naturalists study the Red Loxodon
// herds drifting through the maize-fields.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class FHuevillage582QuestNpcsScript : GeneralScript
{
	protected override void Load()
	{
		// =====================================================================
		// QUEST 1001: The Black Maize Blight
		// =====================================================================
		// Gorge-Trapper Adele - Zibu Maize overrunning the trap-lines
		//---------------------------------------------------------------------
		AddNpc(20109, L("[Gorge-Trapper] Adele"), "f_huevillage_58_2", -280, 200, 90, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1001);

			dialog.SetTitle(L("Adele"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A wiry trapper in a patched gorge-coat checks the tension on a snare-wire, chewing on a maize-stalk*{/}"));
				await dialog.Msg(L("Black Maize stalks snap my snares faster than I can reset them. Ten snares, three trapped kits this week. My livelihood is losing to weeds that walk."));

				var response = await dialog.Select(L("Twenty Black Maize cleared. Hit the central maize-field first - that's where the mature stalks cluster. The younger ones scatter back to seed once the elders fall."),
					Option(L("I'll clear twenty Black Maize"), "help"),
					Option(L("Why not move your trap-lines?"), "info"),
					Option(L("Weeds aren't my trade"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She sets the snare-wire down and pulls a folded map from her coat*{/}"));

						if (await dialog.YesNo(L("Twenty Black Maize. Central field, western slope too if you run short. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Strike at the stalk-joint, not the tassel. Tassels spray spore when cut - you'll choke if you swing high."));
						}
						break;

					case "info":
						await dialog.Msg(L("Three generations of Adele-women have trapped this gorge. We don't move for weeds - we clear them. That's family pride, inconvenient as it is."));
						break;

					case "leave":
						await dialog.Msg(L("Fair enough. Walk the west slope, then - less maize, more open ground."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("killMaize", out var killObj)) return;

				if (killObj.Done)
				{
					await dialog.Msg(L("{#666666}*She grins, the first genuine one since you met her, and snaps a maize-stalk between her fingers*{/}"));
					await dialog.Msg(L("Twenty stalks fewer, three snares re-set, tomorrow's trap-line in business again."));
					await dialog.Msg(L("Trapper's pay, coin and a spare snare-wire. Useful if you ever try the trade."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Central maize-field. Strike at the joint, not the tassel."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The field's thinned enough that my snares held through a full week. First time in a year. I'll toast your name when the kits cure."));
			}
		});

		// =====================================================================
		// QUEST 1002: Huntress's Hide Order
		// =====================================================================
		// Huntress Silvia - Ultanun hide bundle delivery to Tanner Juris
		//---------------------------------------------------------------------
		AddNpc(20107, L("[Huntress] Silvia"), "f_huevillage_58_2", 640, 20, 180, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1002);

			dialog.SetTitle(L("Silvia"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A lean huntress kneels beside a folded bundle of Ultanun hides, cleaning spore-dust from the edges with a bone-scraper*{/}"));
				await dialog.Msg(L("Fourteen Ultanun hides, cured three days, salted twice. I can't carry the bundle down-gorge myself - busted a tendon last week. Juris needs them by tonight or the order goes to Fedimian's tanner."));

				var response = await dialog.Select(L("Carry this bundle to Tanner Juris - his workshop sits at the southern gorge-bend. Bring back his mark-slip so I can settle accounts. Will you?"),
					Option(L("I'll carry the bundle to Juris"), "help"),
					Option(L("Can't he come to you?"), "info"),
					Option(L("Hide-hauling is dirty work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She rolls the bundle tight, binds it with a leather thong, and passes it over with a grunt*{/}"));

						if (await dialog.YesNo(L("Southern gorge-bend. Juris. Don't unroll the bundle - salt will spill. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("His mark-slip is stamped with a tanning-iron, not a seal. You'll know it when you see it - it smells like his shop."));
						}
						break;

					case "info":
						await dialog.Msg(L("Juris is older than me and his knees are worse than my ankle. He'd come if I asked, but he'd be useless for a day after. Better I ask a traveler."));
						break;

					case "leave":
						await dialog.Msg(L("Then the bundle goes rancid and the order goes to Fedimian. Your call."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				if (!character.Quests.TryGetById(questId, out var quest)) return;
				if (!quest.TryGetProgress("deliverHide", out var deliverObj)) return;

				if (deliverObj.Done)
				{
					await dialog.Msg(L("{#666666}*She takes the mark-slip, sniffs it, and laughs*{/}"));
					await dialog.Msg(L("Smells right. Tanning-iron mark, his price still fair. He'll have gloves out of that bundle by next market-day."));
					await dialog.Msg(L("Huntress's share. And a set of snare-wires Juris threw in as thanks - I'll pass them to you instead. More use in a traveler's pack than my cupboard."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Southern gorge-bend. Don't unroll the bundle."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Juris's gloves sold out by noon at market. He owes me an extra percent on the next order. Ankle's healing. Good month all around."));
			}
		});

		// =====================================================================
		// TANNER JURIS (Quest 1002 recipient)
		// =====================================================================
		AddNpc(20117, L("[Tanner] Juris"), "f_huevillage_58_2", 820, 100, 270, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1002);

			dialog.SetTitle(L("Juris"));

			if (character.Quests.IsActive(questId))
			{
				var delivered = character.Variables.Perm.GetInt("Laima.Quests.f_huevillage_58_2.Quest1002.Delivered", 0) >= 1;
				if (!delivered)
				{
					await dialog.Msg(L("{#666666}*An older tanner with forearms like tree-roots unrolls the bundle on a stone worktable and runs a thumb along the top hide's grain*{/}"));
					await dialog.Msg(L("Silvia's work. Salt-cured proper, scrape-lines even. She's the best huntress in the gorge and she knows it."));
					await dialog.Msg(L("{#666666}*He pulls a tanning-iron from a brazier, stamps a leather slip, and blows on the impression to cool it*{/}"));
					await dialog.Msg(L("Mark-slip. Tell her the gloves'll be ready by market-day - fourteen hides makes seven pairs. And give her these snare-wires - payment for the premium cure."));

					character.Variables.Perm.Set("Laima.Quests.f_huevillage_58_2.Quest1002.Delivered", 1);
					character.ServerMessage(L("{#FFD700}Juris's mark-slip received. Return to Huntress Silvia.{/}"));
				}
				else
				{
					await dialog.Msg(L("Back to Silvia with the mark-slip. She'll know it by the iron-mark."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Sold all seven pairs of Ultanun-hide gloves at market. Best batch in years. Silvia's tendon better?"));
			}
			else
			{
				await dialog.Msg(L("{#666666}*The tanner stirs a tanning-vat with a long wooden paddle, eyeing its color*{/}"));
				await dialog.Msg(L("Shop's open if you have hide to cure. Otherwise, mind your boots - the vat-splash stains."));
			}
		});

		// =====================================================================
		// QUEST 1003: Maize Husks for the Forager
		// =====================================================================
		// Forager Gedas - Husks for gorge-cure poultices
		//---------------------------------------------------------------------
		AddNpc(20017, L("[Forager] Gedas"), "f_huevillage_58_2", 414, -1100, 0, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1003);

			dialog.SetTitle(L("Gedas"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A wind-burned forager with a foraging-basket tucked under one arm crushes a dried maize-husk between her fingers, testing its flex*{/}"));
				await dialog.Msg(L("Black Maize husks. The dried outer-layer, not the stalk. Poultice-makers up-village pay three coppers per husk, and I've got a standing order for eight."));

				var response = await dialog.Select(L("Black Maize spore is harmless on the husks once they dry. Pick from the ground where stalks have already shed - never peel a live plant. Will you gather eight dry husks?"),
					Option(L("I'll gather eight dry husks"), "help"),
					Option(L("Why not pick them yourself?"), "info"),
					Option(L("Foraging is foragers' work"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She pulls a second basket from her pack and tosses it up*{/}"));

						if (await dialog.YesNo(L("Only dry husks. Check the ground at stalk-bases, not the tassels. Eight is the order. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("If a husk crumbles when you pick it up, it's too old - skip it. Poultice-makers refuse crumble-stock."));
						}
						break;

					case "info":
						await dialog.Msg(L("My back. Twenty years of bending for husks and my spine is done. I still walk the fields to scout; I don't bend for pickups anymore."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. Eight husks is a long bend-list for my back. I'll find someone else, eventually."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var huskCount = character.Inventory.CountItem(650850);

				if (huskCount >= 8)
				{
					await dialog.Msg(L("{#666666}*She inspects each husk, flexing them one at a time to check for cure*{/}"));
					await dialog.Msg(L("All dry, none crumbling. Poultice-makers will pay full rate on all eight."));
					await dialog.Msg(L("Forager's share, on the spot. And a small pouch of dried maize-flour - good for travel-biscuits. Keeps a month."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Ground pickups. Dry, flexible, no crumble. Eight husks."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("Poultice-makers paid the full rate and ordered twelve more. I'll take that walk myself - small batch, my back can handle twelve."));
			}
		});

		// =====================================================================
		// MAIZE HUSK SPOTS
		// =====================================================================
		// For Quest 1003 - Maize Husks for the Forager
		// =====================================================================

		void AddHuskSpot(int spotNumber, int x, int z, int direction)
		{
			AddNpc(47247, L("Dry Maize Husk"), "f_huevillage_58_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_2", 1003);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*A dry Black Maize husk lies in the leaf-litter. Without a forager's order, there's no reason to pick it up*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{spotNumber}";
				var gathered = character.Variables.Perm.GetBool(variableKey, false);

				if (gathered)
				{
					await dialog.Msg(L("{#666666}*This husk has already been taken. The ground-patch is cleared*{/}"));
					return;
				}

				var spawnedKey = $"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{spotNumber}.Spawned";
				var hasSpawned = character.Variables.Perm.GetBool(spawnedKey, false);
				if (!hasSpawned && RandomProvider.Get().Next(100) < 15)
				{
					character.Variables.Perm.Set(spawnedKey, true);

					if (SpawnTempMonsters(character, MonsterId.Zibu_Maize, 1, 70, TimeSpan.FromMinutes(1)))
					{
						character.ServerMessage(L("{#FF6666}A Black Maize stalk uproots itself beside the husk!{/}"));
					}
				}

				var result = await character.TimeActions.StartAsync(L("Picking the husk..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Inventory.Add(650850, 1, InventoryAddType.PickUp);
					character.Variables.Perm.Set(variableKey, true);

					var currentCount = character.Inventory.CountItem(650850);
					character.ServerMessage(LF("Maize husks gathered: {0}/8", currentCount));

					if (currentCount >= 8)
					{
						character.ServerMessage(L("{#FFD700}All husks gathered! Return to Forager Gedas.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Gathering interrupted."));
				}
			});
		}

		AddHuskSpot(1, 414, -1103, 0);
		AddHuskSpot(2, 598, -964, 90);
		AddHuskSpot(3, 409, -791, 180);
		AddHuskSpot(4, 250, -639, 270);
		AddHuskSpot(5, 505, -536, 0);
		AddHuskSpot(6, 286, -944, 90);
		AddHuskSpot(7, -820, -122, 180);
		AddHuskSpot(8, -540, -294, 270);

		// =====================================================================
		// QUEST 1004: The Loxodon Herd
		// =====================================================================
		// Naturalist Audra - Studying Red Loxodon movement patterns
		//---------------------------------------------------------------------
		AddNpc(20151, L("[Naturalist] Audra"), "f_huevillage_58_2", -60, 80, 45, async dialog =>
		{
			var character = dialog.Player;
			var questId = new QuestId("f_huevillage_58_2", 1004);

			dialog.SetTitle(L("Audra"));

			if (!character.Quests.Has(questId))
			{
				await dialog.Msg(L("{#666666}*A young naturalist in travel-stained field-clothes sketches a loxodon's hoof-print into her journal, brow furrowed*{/}"));
				await dialog.Msg(L("Three Red Loxodons sighted in this gorge last week. Three. The full Fedimian census counted fourteen left alive across the whole region last spring - if three of those fourteen are here, this gorge is a refuge, and I need to confirm it before the census closes."));

				var response = await dialog.Select(L("Four track-sites scatter across the gorge. Read each one - dung-age, hoof-depth, bark-rub height. The signs tell me whether the three are lingering, whether any are a calf, whether a cow is present. Will you walk them for me?"),
					Option(L("I'll walk the four track-sites"), "help"),
					Option(L("Fourteen left? Only fourteen?"), "info"),
					Option(L("Tracks are naturalists' business"), "leave")
				);

				switch (response)
				{
					case "help":
						await dialog.Msg(L("{#666666}*She tears a sketch-page loose and marks four points with a charcoal nub*{/}"));

						if (await dialog.YesNo(L("Four sites. Dung fresh or old, hoof-print deep or shallow, bark-rub high on the tree or low. Describe each. Will you?")))
						{
							character.Quests.Start(questId);
							await dialog.Msg(L("Fresh dung and deep prints mean they're staying - which is what we want. Old or shallow signs mean they're already drifting on to wherever, and the refuge-claim fails."));
							await dialog.Msg(L("High bark-rubs mean a bull. Low bark-rubs mean a calf. A calf is the whole argument - it means they're still breeding."));
						}
						break;

					case "info":
						await dialog.Msg(L("Hunted for the hide, fifty years. By the time the ban passed, the herds were already collapsing. Fourteen counted last spring - could be ten this spring, could be six. Depends on calves."));
						await dialog.Msg(L("No one hunts them now - Silvia wouldn't, even if the coin tripled. The work is counting them before the last ones drift off somewhere we can't find them."));
						break;

					case "leave":
						await dialog.Msg(L("Fair. I'll walk them myself over the next month. Slower, but my legs still work."));
						break;
				}
			}
			else if (character.Quests.IsActive(questId))
			{
				var sitesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked", 0);

				if (sitesChecked >= 4)
				{
					await dialog.Msg(L("{#666666}*She listens, sketching each detail, and her charcoal slows at the last entry*{/}"));
					await dialog.Msg(L("Site One: fresh dung, deep prints, bark-rub shoulder-high. Site Two: fresh, deep, shoulder-high. Site Three: old dung, deep prints, low bark-rub. Site Four: fresh, deep, calf-height bark-rub."));
					await dialog.Msg(L("{#666666}*She closes the journal and lets out a breath she'd been holding for the whole survey*{/}"));
					await dialog.Msg(L("Bull, cow, calf. All three staying. The gorge is a confirmed refuge - and they're still breeding. Three of fourteen, alive, here, with a next-generation calf."));
					await dialog.Msg(L("Naturalist's stipend, full. The Fedimian census amendment gets filed tonight; this gorge goes on the protected-refuge list by month's end."));

					character.Quests.Complete(questId);
				}
				else
				{
					await dialog.Msg(L("Four sites. Dung-age, hoof-depth, bark-rub height. Walk, read, report."));
				}
			}
			else if (character.Quests.HasCompleted(questId))
			{
				await dialog.Msg(L("The refuge-filing went through. This gorge is now one of four protected Loxodon refuges in the region. If the calf survives her first winter, we have four next spring."));
			}
		});

		// =====================================================================
		// LOXODON TRACK-SITES
		// =====================================================================
		// For Quest 1004 - The Loxodon Herd
		// =====================================================================

		void AddTrackSite(int siteNumber, string siteName, string observation, int x, int z, int direction)
		{
			AddNpc(47251, L(siteName), "f_huevillage_58_2", x, z, direction, async dialog =>
			{
				var character = dialog.Player;
				var questId = new QuestId("f_huevillage_58_2", 1004);

				if (!character.Quests.IsActive(questId))
				{
					await dialog.Msg(L("{#666666}*Loxodon tracks cross the gorge-path. Without a naturalist's order, there's no reason to read them*{/}"));
					return;
				}

				var variableKey = $"Laima.Quests.f_huevillage_58_2.Quest1004.Site{siteNumber}";
				var checkedSite = character.Variables.Perm.GetBool(variableKey, false);

				if (checkedSite)
				{
					await dialog.Msg(L("{#666666}*You have already read this track-site*{/}"));
					return;
				}

				var result = await character.TimeActions.StartAsync(L("Reading the tracks..."), "Cancel", "SITGROPE", TimeSpan.FromSeconds(3));

				if (result == TimeActionResult.Completed)
				{
					character.Variables.Perm.Set(variableKey, true);
					var sitesChecked = character.Variables.Perm.GetInt("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked", 0);
					character.Variables.Perm.Set("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked", sitesChecked + 1);

					character.ServerMessage(L(observation));
					character.ServerMessage(LF("Track-sites read: {0}/4", sitesChecked + 1));

					if (sitesChecked + 1 >= 4)
					{
						character.ServerMessage(L("{#FFD700}All sites read! Return to Naturalist Audra.{/}"));
					}
				}
				else
				{
					character.ServerMessage(L("Reading interrupted."));
				}
			});
		}

		AddTrackSite(1, "North Crossing", "North: fresh dung, deep prints. Bark-rub at shoulder-height - mature bull territory-marker.", -60, 400, 0);
		AddTrackSite(2, "Maize-Field Edge", "Maize-Field Edge: fresh dung, deep prints, shoulder-high bark-rubs. Second bull using this corridor.", 250, -640, 90);
		AddTrackSite(3, "South Wash", "South Wash: old dung, deep prints. Bark-rub low to the ground - a calf nudged here days ago, hasn't returned.", -348, -1390, 180);
		AddTrackSite(4, "Gorge-Bottom Spring", "Gorge-Bottom Spring: fresh dung, deep prints. Calf-height bark-rub beside a cow's flank-print. Nursing pair drinks here daily.", -634, -1128, 270);
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// Quest 1001 CLASS: The Black Maize Blight
//-----------------------------------------------------------------------------

public class TheBlackMaizeBlightQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1001);
		SetName("The Black Maize Blight");
		SetType(QuestType.Sub);
		SetDescription("Gorge-Trapper Adele needs twenty Black Maize cleared so her snare-lines can hold through the next week.");
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Gorge-Trapper] Adele", "f_huevillage_58_2");

		AddObjective("killMaize", "Defeat Black Maize in the gorge-fields",
			new KillObjective(20, new[] { MonsterId.Zibu_Maize }));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 12)); // Large HP Potion
		AddReward(new ItemReward(640007, 8)); // Large SP Potion
		AddReward(new ItemReward(640012, 4));  // Recovery Potion
	}
}

// Quest 1002 CLASS: Huntress's Hide Order
//-----------------------------------------------------------------------------

public class HuntresssHideOrderQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1002);
		SetName("Huntress's Hide Order");
		SetType(QuestType.Sub);
		SetDescription("Huntress Silvia's bundle of fourteen Ultanun hides must reach Tanner Juris at the southern gorge-bend, and his mark-slip must return to her.");
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Huntress] Silvia", "f_huevillage_58_2");

		AddObjective("deliverHide", "Deliver the hide and return with the mark-slip",
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_2.Quest1002.Delivered", 1, true));

		AddReward(new ExpReward(15600, 10800));
		AddReward(new SilverReward(32000));
		AddReward(new ItemReward(640085, 1));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 12));  // Large HP Potion
		AddReward(new ItemReward(640007, 8));  // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1002.Delivered");
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1002.Delivered");
	}
}

// Quest 1003 CLASS: Maize Husks for the Forager
//-----------------------------------------------------------------------------

public class MaizeHusksForTheForagerQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1003);
		SetName("Maize Husks for the Forager");
		SetType(QuestType.Sub);
		SetDescription("Forager Gedas needs eight dry Black Maize husks gathered for the up-village poultice-makers.");
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Forager] Gedas", "f_huevillage_58_2");

		AddObjective("collectHusks", "Gather dry Black Maize husks",
			new CollectItemObjective(650850, 8));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 13));  // Large HP Potion
		AddReward(new ItemReward(640007, 9));  // Large SP Potion
		AddReward(new ItemReward(640012, 3));  // Recovery Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Inventory.Remove(650850,
			character.Inventory.CountItem(650850),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{i}.Spawned");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Inventory.Remove(650850,
			character.Inventory.CountItem(650850),
			InventoryItemRemoveMsg.Destroyed);

		for (int i = 1; i <= 8; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{i}");
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1003.Spot{i}.Spawned");
		}
	}
}

// Quest 1004 CLASS: The Loxodon Herd
//-----------------------------------------------------------------------------

public class TheLoxodonHerdQuest : QuestScript
{
	protected override void Load()
	{
		SetId("f_huevillage_58_2", 1004);
		SetName("The Loxodon Herd");
		SetType(QuestType.Sub);
		SetDescription("Naturalist Audra has asked you to read four Red Loxodon track-sites and confirm whether this gorge qualifies as a protected refuge for one of the few surviving herds.");
		SetLocation("f_huevillage_58_2");
		SetAutoTracked(true);

		SetReceive(QuestReceiveType.Manual);
		SetCancelable(true);
		SetUnlock(QuestUnlockType.AllAtOnce);
		AddQuestGiver("[Naturalist] Audra", "f_huevillage_58_2");

		AddObjective("readSites", "Read the four Loxodon track-sites",
			new VariableCheckObjective("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked", 4, true));

		AddReward(new ExpReward(11000, 7500));
		AddReward(new SilverReward(45000));
		AddReward(new ItemReward(640085, 2));  // Lv5 EXP Card
		AddReward(new ItemReward(640004, 13)); // Large HP Potion
		AddReward(new ItemReward(640007, 9)); // Large SP Potion
	}

	public override void OnComplete(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1004.Site{i}");
		}
	}

	public override void OnCancel(Character character, Quest quest)
	{
		character.Variables.Perm.Remove("Laima.Quests.f_huevillage_58_2.Quest1004.SitesChecked");
		for (int i = 1; i <= 4; i++)
		{
			character.Variables.Perm.Remove($"Laima.Quests.f_huevillage_58_2.Quest1004.Site{i}");
		}
	}
}
