//--- Melia Script ----------------------------------------------------------
// Card Album Boss Rewards
//--- Description -----------------------------------------------------------
// Handles card drops for Card Album bosses. Top 5 DPS players receive cards
// dropped on ground with loot protection. Visual ranking effects are shown.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;

/// <summary>
/// Handles special card drops for Card Album summoned bosses.
/// Awards cards to the top 5 DPS players with visual ranking effects.
/// </summary>
public class CardAlbumBossRewardsScript : GeneralScript
{
	private const int MAX_DPS_REWARDS = 5;
	private const float SUPPORT_RANGE = 400f;

	// Card data mapping: monster class name -> card item ID
	private static readonly Dictionary<string, int> CardAlbumDrops = new()
	{
		// Item Summon Boss Drops (Card Album)
		{ "item_summon_boss_Gaigalas", 644001 },
		{ "item_summon_boss_Golem", 644004 },
		{ "item_summon_boss_ginklas", 644007 },
		{ "item_summon_boss_Ravinepede", 644014 },
		{ "item_summon_boss_Rajatoad", 644015 },
		{ "item_summon_boss_spector_gh", 644019 },
		{ "item_summon_boss_Malletwyvern", 644023 },
		{ "item_summon_boss_MagBurk", 644024 },
		{ "item_summon_boss_Mushcaria", 644027 },
		{ "item_summon_boss_bearkaras", 644037 },
		{ "item_summon_boss_Goblin_Warrior", 644039 },
		{ "item_summon_boss_Goblin_Warrior_red", 644040 },
		{ "item_summon_boss_bramble", 644041 },
		{ "item_summon_boss_BiteRegina", 644042 },
		{ "item_summon_boss_Saltistter", 644044 },
		{ "item_summon_boss_Confinedion", 644049 },
		{ "item_summon_boss_yekub", 644058 },
		{ "item_summon_boss_Iltiswort", 644063 },
		{ "item_summon_boss_GiantWoodGoblin_red", 644064 },
		{ "item_summon_boss_chafer", 644068 },
		{ "item_summon_boss_onion_the_great", 644072 },
		{ "item_summon_boss_hydra", 644084 },
		{ "item_summon_boss_sparnas", 644089 },
		{ "item_summon_boss_Sparnanman", 644092 },
		{ "item_summon_boss_capria", 644094 },
		{ "item_summon_boss_Nepenthes", 644095 },
		{ "item_summon_boss_Naktis", 644099 },
		{ "item_summon_boss_Neop", 644104 },
		{ "item_summon_boss_velnewt", 644113 },
		{ "item_summon_boss_Pyroego", 644117 },
		{ "item_summon_boss_Frogola", 644123 },
		{ "item_summon_boss_Marionette", 644125 },
		{ "item_summon_boss_Crabil", 644127 },
		{ "item_summon_boss_Genmagnus", 644128 },
		{ "item_summon_boss_merregina", 644129 },
		{ "item_summon_boss_Grinender", 644005 },
		{ "item_summon_boss_GazingGolem", 644002 },
		{ "item_summon_boss_Gorgon", 644003 },
		{ "item_summon_boss_Glackuman", 644106 },
		{ "item_summon_boss_Glass_mole", 644006 },
		{ "item_summon_boss_NetherBovine", 644008 },
		{ "item_summon_boss_necrovanter", 644009 },
		{ "item_summon_boss_deadbone", 644010 },
		{ "item_summon_boss_Devilglove", 644011 },
		{ "item_summon_boss_Deathweaver", 644133 },
		{ "item_summon_boss_Denoptic", 644012 },
		{ "item_summon_boss_Rocktortuga", 644016 },
		{ "item_summon_boss_lepus", 644088 },
		{ "item_summon_boss_lecifer", 644017 },
		{ "item_summon_boss_Reaverpede", 644018 },
		{ "item_summon_boss_Riteris", 644102 },
		{ "item_summon_boss_RingCrawler", 644100 },
		{ "item_summon_boss_Marnoks", 644135 },
		{ "item_summon_boss_mineloader", 644022 },
		{ "item_summon_boss_Manticen", 644025 },
		{ "item_summon_boss_Mummyghast", 644026 },
		{ "item_summon_boss_Merge", 644028 },
		{ "item_summon_boss_Mothstem", 644029 },
		{ "item_summon_boss_moa", 644030 },
		{ "item_summon_boss_Moyabruka", 644031 },
		{ "item_summon_boss_Moldyhorn", 644032 },
		{ "item_summon_boss_molich", 644033 },
		{ "item_summon_boss_Minotaurs", 644034 },
		{ "item_summon_boss_mirtis", 644035 },
		{ "item_summon_boss_bebraspion", 644036 },
		{ "item_summon_boss_Velniamonkey", 644139 },
		{ "item_summon_boss_Velorchard", 644038 },
		{ "item_summon_boss_Blud", 644105 },
		{ "item_summon_boss_Strongholder", 644043 },
		{ "item_summon_boss_salamander", 644045 },
		{ "item_summon_boss_ShadowGaoler", 644046 },
		{ "item_summon_boss_stone_whale", 644047 },
		{ "item_summon_boss_Shnayim", 644048 },
		{ "item_summon_boss_Spector_m", 644050 },
		{ "item_summon_boss_simorph", 644096 },
		{ "item_summon_boss_Throneweaver", 644051 },
		{ "item_summon_boss_Ironbaum", 644052 },
		{ "item_summon_boss_archon", 644053 },
		{ "item_summon_boss_Abomination", 644054 },
		{ "item_summon_boss_Unknocker", 644055 },
		{ "item_summon_boss_Achat", 644056 },
		{ "item_summon_boss_ellaganos", 644057 },
		{ "item_summon_boss_Yeti", 644122 },
		{ "item_summon_boss_yonazolem", 644059 },
		{ "item_summon_boss_werewolf", 644061 },
		{ "item_summon_boss_unicorn", 644062 },
		{ "item_summon_boss_Zawra", 644136 },
		{ "item_summon_boss_GiantWoodGoblin", 644065 },
		{ "item_summon_boss_Chapparition", 644067 },
		{ "item_summon_boss_Carapace", 644070 },
		{ "item_summon_boss_Canceril", 644126 },
		{ "item_summon_boss_Centaurus", 644121 },
		{ "item_summon_boss_Kerberos", 644093 },
		{ "item_summon_boss_Colimencia", 644073 },
		{ "item_summon_boss_Kubas", 644132 },
		{ "item_summon_boss_Clymen", 644074 },
		{ "item_summon_boss_Kimeleech", 644075 },
		{ "item_summon_boss_Templeshooter", 644130 },
		{ "item_summon_boss_tutu", 644076 },
		{ "item_summon_boss_TombLord", 644077 },
		{ "item_summon_boss_Fireload", 644131 },
		{ "item_summon_boss_FerretMarauder", 644138 },
		{ "item_summon_boss_poata", 644078 },
		{ "item_summon_boss_Prisoncutter", 644137 },
		{ "item_summon_boss_Flammidus", 644134 },
		{ "item_summon_boss_plokste", 644111 },
		{ "item_summon_boss_Harpeia", 644080 },
		{ "item_summon_boss_honeypin", 644081 },
		{ "item_summon_boss_helgasercle", 644082 },
		{ "item_summon_boss_Spector_F", 644083 },
		{ "item_summon_boss_golem_Gray", 644098 },
		{ "item_summon_boss_SwordBallista", 644146 },
		{ "item_summon_boss_Rambandgad", 644145 },
		{ "item_summon_boss_Lapene", 644144 },
		{ "item_summon_boss_Armaox", 644141 },
		{ "item_summon_boss_Rajapearl", 644085 },
		{ "item_summon_boss_fallen_statue", 644087 },
		{ "item_summon_boss_Teraox", 644108 },
		{ "item_summon_boss_Basilisk", 644109 },
		{ "item_summon_boss_velpede", 644114 },
		{ "item_summon_boss_Lithorex", 644119 },
		{ "item_summon_boss_nuodai", 644120 },
		{ "item_summon_boss_Nuaelle", 644103 },
		{ "item_summon_boss_durahan", 644101 },
		{ "item_summon_boss_Mandara", 644118 },
		{ "item_summon_boss_Sequoia", 644079 },
		{ "item_summon_boss_woodhoungan", 644112 },
		{ "item_summon_boss_succubus", 644142 },
		{ "item_summon_boss_stonefroster", 644143 },
		{ "item_summon_boss_froster_lord", 644147 },
		{ "item_summon_boss_woodspirit", 644140 },

		// Item Summon Field Boss Drops (Card Album - Field variants)
		{ "item_summon_F_boss_Chapparition", 644067 },
		{ "item_summon_F_boss_Glackuman", 644106 },
		{ "item_summon_F_boss_Velniamonkey", 644139 },
		{ "item_summon_F_boss_Fireload", 644131 },
		{ "item_summon_F_boss_mineloader", 644022 },
		{ "item_summon_F_boss_Deathweaver", 644133 },
		{ "item_summon_F_boss_FerretMarauder", 644138 },
		{ "item_summon_F_boss_ellaganos", 644057 },
		{ "item_summon_F_boss_Prisoncutter", 644137 },
		{ "item_summon_F_boss_molich", 644033 },
		{ "item_summon_F_boss_mirtis", 644035 },
		{ "item_summon_F_boss_helgasercle", 644082 },
		{ "item_summon_F_boss_lecifer", 644017 },
		{ "item_summon_F_boss_Marnoks", 644135 },
		{ "item_summon_F_boss_succubus", 644142 },
		{ "item_summon_F_boss_Harpeia_orange", 644080 },
		{ "item_summon_F_boss_Nuaelle", 644103 },
		{ "item_summon_F_boss_Zawra", 644136 },
		{ "item_summon_F_boss_Blud", 644105 },
	};

	/// <summary>
	/// Ranking emoticon names for positions 1-5.
	/// </summary>
	private static readonly string[] RankEmoticons = new[]
	{
		"I_emo_damagerank1_crown",  // Rank 1 (with crown)
		"I_emo_damagerank2_crown",  // Rank 2 (with crown)
		"I_emo_damagerank3_crown",  // Rank 3 (with crown)
		"I_emo_damagerank4",        // Rank 4
		"I_emo_damagerank5",        // Rank 5
	};

	/// <summary>
	/// Duration to display rank emoticons.
	/// </summary>
	private static readonly TimeSpan RankEmoticonDuration = TimeSpan.FromSeconds(10);

	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob mob)
			return;

		// Only handle Card Album bosses
		if (mob.Vars.GetInt("CARDSUMMON_BOSS", 0) != 1)
			return;

		// Check if this boss has card data
		if (!CardAlbumDrops.TryGetValue(mob.ClassName, out var cardItemId))
		{
			Log.Warning("CardAlbumBossRewards: No card data found for '{0}'.", mob.ClassName);
			return;
		}

		// Get top damage dealers
		var combatComponent = mob.Components.Get<CombatComponent>();
		var topAttackers = combatComponent?.GetTopAttackersByDamage(MAX_DPS_REWARDS);

		// Resolve attackers to characters (summons -> master) and deduplicate
		var rankedPlayers = ResolveToCharacters(topAttackers, mob);

		// Fill remaining slots with nearby support players (within SUPPORT_RANGE)
		if (rankedPlayers.Count < MAX_DPS_REWARDS)
			AddNearbySupportPlayers(rankedPlayers, mob, MAX_DPS_REWARDS);

		if (rankedPlayers.Count == 0)
			return;

		// Award cards to each ranked player
		for (var rank = 0; rank < rankedPlayers.Count; rank++)
		{
			var character = rankedPlayers[rank];
			DropCardForPlayer(character, cardItemId, rank + 1, mob.Position);
			ShowRankingEffect(character, rank + 1);
		}
	}

	/// <summary>
	/// Resolves combat entities to their owning characters.
	/// Summons/pets credit their masters. Deduplicates by character ID.
	/// </summary>
	private List<Character> ResolveToCharacters(List<(ICombatEntity Attacker, float Damage)> attackers, Mob boss)
	{
		var result = new List<Character>();
		var seenIds = new HashSet<long>();

		if (attackers == null)
			return result;

		foreach (var (attacker, damage) in attackers)
		{
			var character = GetOwningCharacter(attacker);
			if (character == null)
				continue;

			if (seenIds.Contains(character.DbId))
				continue;

			// Verify character is still on the same map
			if (character.Map != boss.Map)
				continue;

			result.Add(character);
			seenIds.Add(character.DbId);
		}

		return result;
	}

	/// <summary>
	/// Gets the character that owns this combat entity.
	/// Returns the master for summons/pets, or the entity itself if it's a character.
	/// </summary>
	private Character GetOwningCharacter(ICombatEntity entity)
	{
		if (entity is Character character)
			return character;

		// Check if this is a summon with a master
		var aiComponent = entity.Components.Get<AiComponent>();
		if (aiComponent?.Script.GetMaster() is Character master)
			return master;

		return null;
	}

	/// <summary>
	/// Adds nearby players who haven't dealt damage but are within support range.
	/// </summary>
	private void AddNearbySupportPlayers(List<Character> rankedPlayers, Mob boss, int maxCount)
	{
		var existingIds = rankedPlayers.Select(c => c.DbId).ToHashSet();
		var nearbyCharacters = boss.Map.GetActorsInRange<Character>(boss.Position, SUPPORT_RANGE);

		foreach (var character in nearbyCharacters)
		{
			if (rankedPlayers.Count >= maxCount)
				break;

			if (existingIds.Contains(character.DbId))
				continue;

			if (character.IsDead)
				continue;

			rankedPlayers.Add(character);
			existingIds.Add(character.DbId);
		}
	}

	/// <summary>
	/// Drops a card for a player with loot protection.
	/// </summary>
	private void DropCardForPlayer(Character character, int cardItemId, int rank, Position bossPos)
	{
		var rnd = RandomProvider.Get();

		// Calculate drop position spread around the boss
		var dropPos = GetDropPosition(bossPos, rank);

		// Create and drop the card
		var card = new Item(cardItemId, 1);
		card.SetLootProtection(character, TimeSpan.FromSeconds(ZoneServer.Instance.Conf.World.LootPrectionSeconds));

		var direction = new Direction(rnd.Next(0, 360));
		var dropRadius = ZoneServer.Instance.Conf.World.DropRadius;
		var distance = rnd.Next(dropRadius / 2, dropRadius + 1);

		card.Drop(character.Map, dropPos, direction, distance, character.AccountObjectId, character.Layer);
	}

	/// <summary>
	/// Gets a slightly offset drop position for each rank to spread items.
	/// </summary>
	private Position GetDropPosition(Position center, int rank)
	{
		var angle = (rank - 1) * (360f / MAX_DPS_REWARDS) * (Math.PI / 180);
		var radius = 30f;

		return new Position(
			center.X + (float)(Math.Cos(angle) * radius),
			center.Y,
			center.Z + (float)(Math.Sin(angle) * radius)
		);
	}

	/// <summary>
	/// Shows a visual ranking emoticon above the player's head using the EffectsComponent.
	/// This ensures players who enter visibility range will also see the emoticon.
	/// </summary>
	private void ShowRankingEffect(Character character, int rank)
	{
		if (rank < 1 || rank > RankEmoticons.Length)
			return;

		var emoticonName = RankEmoticons[rank - 1];
		var effectKey = $"card_album_rank_{rank}";

		// Add to effects component so players entering visibility range see it
		var effectsComponent = character.Components.Get<EffectsComponent>();
		if (effectsComponent != null)
		{
			// Remove any existing effect from a previous boss kill
			effectsComponent.RemoveEffect(effectKey);

			var effect = new EmoticonEffect(emoticonName, RankEmoticonDuration);
			effectsComponent.AddEffect(effectKey, effect);

			// Schedule removal after duration
			_ = RemoveEffectAfterDelay(character, effectKey, RankEmoticonDuration);
		}
		else
		{
			// Fallback to direct send if no effects component
			Send.ZC_SHOW_EMOTICON(character, emoticonName, RankEmoticonDuration);
		}
	}

	/// <summary>
	/// Removes an effect after a delay.
	/// </summary>
	private async System.Threading.Tasks.Task RemoveEffectAfterDelay(Character character, string effectName, TimeSpan delay)
	{
		await System.Threading.Tasks.Task.Delay(delay);

		if (character?.Map != null)
		{
			var effectsComponent = character.Components.Get<EffectsComponent>();
			effectsComponent?.RemoveEffect(effectName);
		}
	}
}
