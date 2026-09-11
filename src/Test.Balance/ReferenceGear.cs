using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Test.Balance
{
	/// <summary>
	/// What a reference character is wearing, so a measurement can be
	/// reported against the gear that produced it.
	/// </summary>
	public class GearSet
	{
		public ItemGrade Grade { get; init; }
		public int ItemLevel { get; init; }
		public Dictionary<EquipSlot, Item> Items { get; } = new();

		/// <summary>
		/// The weapon, or null for a deliberately naked run.
		/// </summary>
		public Item Weapon => this.Items.TryGetValue(EquipSlot.RightHand, out var item) ? item : null;

		public override string ToString()
		{
			var weapon = this.Weapon == null ? "none" : $"{this.Weapon.Data.ClassName} lv{this.Weapon.UseLevel}";

			return $"{this.Grade} lv{this.ItemLevel} set, weapon {weapon}, {this.Items.Count} piece(s)";
		}
	}

	/// <summary>
	/// One layer stack a character can be dressed in.
	/// </summary>
	/// <remarks>
	/// Separate from GearTier so a diagnostic can build the layers one at a
	/// time and report what each is worth, which is the only way to answer
	/// whether gems and cards outweigh the equipment they sit in.
	/// </remarks>
	/// <param name="Grade">Stamped on every piece; drives how many random options identification rolls.</param>
	/// <param name="Refine">Reinforce level, worth 3.5% of the piece's base value each.</param>
	/// <param name="RandomOptions">Whether pieces are identified, rolling the bonus stats a player would get.</param>
	/// <param name="GemLevel">Level of the gem in every socket, or zero to leave the sockets shut. A colored gem grants three times its level of one stat.</param>
	/// <param name="Cards">Whether one stat card per card group is equipped.</param>
	/// <param name="SetNames">Named set to draw from, or null to take the level-appropriate pool.</param>
	public sealed record GearLoadout(ItemGrade Grade, int Refine, bool RandomOptions, int GemLevel, bool Cards, string[] SetNames = null);

	/// <summary>
	/// How well equipped a measured character is.
	/// </summary>
	/// <remarks>
	/// A single gear point is not enough to price a defensive buff. Damage is
	/// attack * (r^1.2 / (r^1.2 + 1)) on r = attack/defense, so a buff that
	/// grants a percentage of damage reduction is worth the same wherever the
	/// character sits on that curve and a buff that grants a percentage of
	/// defense is not - and measuring both at one r sets their relative price
	/// by whichever r happened to be picked. The tiers are the ends of the
	/// range a player really passes through, so the two price to the same
	/// average mitigation rather than to the same mitigation at one gear
	/// level.
	/// </remarks>
	public enum GearTier
	{
		/// <summary>
		/// Level-appropriate Normal-grade gear, unrefined, with no sockets,
		/// gems or cards. What every measurement used before there were tiers.
		/// </summary>
		/// <remarks>
		/// The low end used to be nothing equipped at all, and that priced a
		/// whole family of buffs wrong in the other direction: a shield block
		/// buff is worth exactly nothing with no shield and a weapon damage
		/// buff nothing with no weapon, so a third of the blend read 1.000 for
		/// a structural reason and the solver inflated the magnitude to make
		/// up for it. A fresh character holds something, and holding something
		/// is what lets every buff be read.
		/// </remarks>
		Starter,

		/// <summary>
		/// Level-appropriate gear at Rare grade and +7, identified, with a gem
		/// in every socket.
		/// </summary>
		/// <remarks>
		/// The rung between the two ends, and it exists because they were too
		/// far apart without it: Normal at level 50 against Legend +15 at 99
		/// was 250 attack against 1,278, so the grid sampled the two ends of
		/// the mitigation curve and almost nothing in the middle.
		/// </remarks>
		Mid,

		/// <summary>
		/// The endgame set, matching what /equipset hands out: Legend grade at
		/// +15, identified, with a gem in every socket and stat cards equipped.
		/// </summary>
		Endgame,
	}

	/// <summary>
	/// Builds and equips the level-appropriate reference gear a scenario is
	/// measured with, so absolute damage can be checked against R1/R2 rather
	/// than only its shape.
	/// </summary>
	public static class ReferenceGear
	{
		/// <summary>
		/// Item levels stop at 75 by design; past that, progression runs
		/// through grade and reinforce.
		/// </summary>
		public const int MaxItemLevel = 75;

		/// <summary>
		/// Armor slots the reference set fills. Accessories are left empty
		/// so their rolled properties do not muddy the base curve.
		/// </summary>
		private static readonly (EquipSlot Slot, EquipType Type)[] ArmorSlots =
		[
			(EquipSlot.Top, EquipType.Shirt),
			(EquipSlot.Pants, EquipType.Pants),
			(EquipSlot.Gloves, EquipType.Gloves),
			(EquipSlot.Shoes, EquipType.Boots),
		];

		/// <summary>
		/// The named endgame set, matching /equipset's own defaults so the tier
		/// is reproducible in game.
		/// </summary>
		/// <remarks>
		/// Raffye is this server's lv75 weapon and shield line and Blint its
		/// lv70 armour, which is what the command hands out with no set named.
		/// </remarks>
		private static readonly string[] EndgameSets = ["Raffye", "Blint"];

		/// <summary>
		/// Level every card is held at; a card's effect is scaled by its level
		/// directly. Gem level is per loadout, since it is most of what
		/// separates one tier from the next.
		/// </summary>
		private const int CardLevel = 10;

		/// <summary>
		/// What each tier is made of.
		/// </summary>
		/// <remarks>
		/// Grade does not raise an item's own attack or defense - the only
		/// stat it drives is how many random options identification rolls
		/// (Magic one or two, Rare two, Legend three or four), so a tier's
		/// power is really its item level, its refine and what is socketed
		/// into it. Written as data rather than as three builders so the
		/// diagnostic can take a tier apart layer by layer.
		/// </remarks>
		private static readonly Dictionary<GearTier, GearLoadout> Loadouts = new()
		{
			[GearTier.Starter] = new(ItemGrade.Normal, 0, false, 0, false),
			[GearTier.Mid] = new(ItemGrade.Rare, 7, true, 5, false),
			[GearTier.Endgame] = new(ItemGrade.Legend, 15, true, 10, true, EndgameSets),
		};

		/// <summary>
		/// Three cards of each colour, which is what a character can hold.
		/// </summary>
		/// <remarks>
		/// Named rather than searched, for the reason the gems are: a rule like
		/// "the lowest id in the group whose effect is a stat" is deterministic
		/// and picks the wrong card every time. It gave a STR class the
		/// Netherbovine card (+50 DEX at level 10) and, for defense, the Gorgon
		/// card - a Dark property resistance, which mitigates nothing the probe
		/// swings.
		///
		/// Only stat effects are eligible. Every other card function starts a
		/// buff, and a window measuring what one buff is worth must not have
		/// others appearing in it - a card granting its own damage multiplier
		/// would be read as part of the subject's value.
		///
		/// The LEG group is deliberately absent. Legend_card_Zawra is
		/// DEF_RATE at coefficient 10, which resolves to +100% physical defense
		/// at card level 10 and nothing at all on the magical side, so holding
		/// it pushed the endgame tier's incoming damage back to 31% physical
		/// against 69% magical and undid the attack-type balance the probe
		/// depends on.
		///
		/// See doc/packages/laima/monster_cards_rebalanced.md for what each
		/// group carries.
		/// </remarks>
		private static readonly (CardGroup Group, string[] Physical, string[] Magical)[] Cards =
		[
			// Red: attack. Archon feeds both sides, the other two follow the
			// class's own attack stat.
			(CardGroup.ATK,
				["card_archon", "card_plokste", "card_Genmagnus"],
				["card_archon", "card_helgasercle", "card_plokste"]),

			// Blue: defense, split evenly across the two sides so a card set
			// cannot tilt the physical-magical balance on its own.
			(CardGroup.DEF,
				["card_Sequoia_blue", "card_Golem_gray", "card_Nuaele"],
				["card_Sequoia_blue", "card_Golem_gray", "card_Nuaele"]),

			// Purple: the utility cards that touch a roll the probe makes.
			// The rest of the group is resistances and recovery, which no
			// window here can read.
			(CardGroup.UTIL,
				["card_molich", "card_Velorchard", "card_Naktis"],
				["card_molich", "card_Velorchard", "card_Naktis"]),
		];

		/// <summary>
		/// Green: the single-stat card matching each primary stat, worth five
		/// times its level - so at level 10 it is +50, five times what a gem of
		/// the same level grants. The two all-stat cards fill the colour.
		/// </summary>
		private static readonly Dictionary<string, string> StatCards = new(StringComparer.OrdinalIgnoreCase)
		{
			["STR"] = "card_Minotaurs",
			["INT"] = "card_Kubas",
			["CON"] = "card_Blud",
			["DEX"] = "card_NetherBovine",
			["MNA"] = "card_Unknocker",
			["SPR"] = "card_Unknocker",
		};

		private static readonly string[] AllStatCards = ["card_Lapene", "card_payawoota"];

		/// <summary>
		/// Gems that grant each primary stat, so a character is gemmed for the
		/// stat its own class builds into.
		/// </summary>
		private static readonly Dictionary<string, int> StatGems = new(StringComparer.OrdinalIgnoreCase)
		{
			["STR"] = 643501,
			["INT"] = 643502,
			["CON"] = 643503,
			["DEX"] = 643504,
			["MNA"] = 643817,
			["SPR"] = 643817,
		};

		/// <summary>
		/// Equips the gear of the given tier and returns what was equipped.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="job"></param>
		/// <param name="tier"></param>
		public static GearSet Equip(Character character, JobEntry job, GearTier tier)
			=> Equip(character, job, Loadouts[tier]);

		/// <summary>
		/// Equips one loadout and returns what was equipped.
		/// </summary>
		/// <remarks>
		/// Every layer is a flag rather than a separate builder, so the tiers
		/// and the diagnostic that takes one apart read the same code path.
		///
		/// Random options are rolled through GameRandom, which the probe seeds,
		/// so a control window and its treatment window are handed the same
		/// rolls and the pair still compares like with like.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="job"></param>
		/// <param name="loadout"></param>
		public static GearSet Equip(Character character, JobEntry job, GearLoadout loadout)
		{
			var itemLevel = Math.Min(MaxItemLevel, (int)character.Properties.GetFloat(PropertyName.Lv));
			var set = new GearSet { Grade = loadout.Grade, ItemLevel = itemLevel };

			var weaponData = loadout.SetNames == null
				? FindWeapon(job, ItemGrade.Normal, itemLevel)
				: job.Weapons.Select(type => FindSetItem(loadout.SetNames, type, null)).FirstOrDefault(data => data != null);

			if (weaponData != null)
				set.Items[EquipSlot.RightHand] = Forge(weaponData, job, loadout);

			if (job.UsesShield)
			{
				var shieldData = Find(loadout, EquipType.Shield, itemLevel, null);

				if (shieldData != null)
					set.Items[EquipSlot.LeftHand] = Forge(shieldData, job, loadout);
			}

			foreach (var (slot, type) in ArmorSlots)
			{
				var armorData = Find(loadout, type, itemLevel, ArmorMaterialType.Leather);

				if (armorData != null)
					set.Items[slot] = Forge(armorData, job, loadout);
			}

			foreach (var pair in set.Items)
				character.Inventory.SetEquipSilent(pair.Key, pair.Value);

			if (loadout.Cards)
				EquipStatCards(character, job);

			character.Properties.InvalidateAll();
			character.Properties.SetFloat(PropertyName.HP, character.Properties.GetFloat(PropertyName.MHP));
			character.Properties.SetFloat(PropertyName.SP, character.Properties.GetFloat(PropertyName.MSP));

			return set;
		}

		/// <summary>
		/// Returns the item a loadout puts in one slot, from its named set when
		/// it has one and from the level-appropriate pool otherwise.
		/// </summary>
		/// <remarks>
		/// The pool is searched at Normal grade whatever the loadout asks for,
		/// and the grade is stamped on afterwards - which is what /equipset
		/// does, and what keeps a tier from depending on whether the item
		/// database happens to carry a Rare-grade shirt at that level.
		/// </remarks>
		/// <param name="loadout"></param>
		/// <param name="type"></param>
		/// <param name="itemLevel"></param>
		/// <param name="material"></param>
		private static ItemData Find(GearLoadout loadout, EquipType type, int itemLevel, ArmorMaterialType? material)
			=> loadout.SetNames == null
				? FindItem(type, ItemGrade.Normal, itemLevel, material)
				: FindSetItem(loadout.SetNames, type, material);

		/// <summary>
		/// Builds one piece: graded, identified, refined, and with a gem in
		/// every socket, to whatever depth the loadout asks for.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="job"></param>
		/// <param name="loadout"></param>
		private static Item Forge(ItemData data, JobEntry job, GearLoadout loadout)
		{
			var item = new Item(data.Id);

			item.Properties.SetFloat(PropertyName.ItemGrade, (int)loadout.Grade);

			if (loadout.RandomOptions)
			{
				item.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
				item.GenerateGradeBasedRandomOptions();
				item.Appraisal();
			}

			if (loadout.Refine > 0 && item.IsRefinable)
				item.Properties.SetFloat(PropertyName.Reinforce_2, loadout.Refine);

			if (loadout.GemLevel <= 0 || !StatGems.TryGetValue(JobCatalog.GetPrimaryStat(job), out var gemId))
				return item;

			for (var i = 0; i < item.MaxSockets; ++i)
			{
				item.CreateSocket(i);

				var gem = new Item(gemId);
				gem.SetLevel(loadout.GemLevel);

				item.SocketGem(gem);
			}

			return item;
		}

		/// <summary>
		/// Puts three cards of each colour into the character's card slots.
		/// </summary>
		/// <remarks>
		/// A card the database does not carry is skipped rather than failing
		/// the window, so a rename degrades the measurement instead of stopping
		/// it.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="job"></param>
		private static void EquipStatCards(Character character, JobEntry job)
		{
			var stat = JobCatalog.GetPrimaryStat(job);
			var magical = stat.Equals("INT", StringComparison.OrdinalIgnoreCase) || stat.Equals("MNA", StringComparison.OrdinalIgnoreCase);

			var picks = Cards.SelectMany(c => magical ? c.Magical : c.Physical).ToList();

			if (StatCards.TryGetValue(stat, out var statCard))
				picks.Add(statCard);

			picks.AddRange(AllStatCards);

			var slot = 1;

			foreach (var className in picks)
			{
				var data = ZoneServer.Instance.Data.ItemDb.FindAll(i => i.ClassName == className).FirstOrDefault();

				if (data == null)
					continue;

				var card = new Item(data.Id);
				card.SetLevel(CardLevel);

				character.Inventory.Add(card, InventoryAddType.PickUp);
				character.Inventory.EquipCard(slot++, card.ObjectId);
			}
		}

		/// <summary>
		/// Returns the endgame set's piece for a slot, matching /equipset's own
		/// name search.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="material"></param>
		private static ItemData FindSetItem(string[] setNames, EquipType type, ArmorMaterialType? material)
		{
			foreach (var setName in setNames)
			{
				var candidates = ZoneServer.Instance.Data.ItemDb.Entries.Values
					.Where(i => i.EquipType1 == type)
					.Where(i => !string.IsNullOrEmpty(i.EquipSlot))
					.Where(i => i.Name != null && i.Name.StartsWith(setName + " ", StringComparison.OrdinalIgnoreCase))
					.Where(i => !i.Name.Contains("Realization", StringComparison.OrdinalIgnoreCase))
					.Where(i => material == null || i.Material == material.Value)
					.Where(i => !IsExcluded(i))
					.OrderByDescending(i => i.MinLevel)
					.ThenBy(i => i.Id)
					.ToArray();

				if (candidates.Length > 0)
					return candidates[0];
			}

			return null;
		}

		/// <summary>
		/// Equips the class's reference set at the given character level and
		/// returns what was equipped.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="job"></param>
		/// <param name="grade"></param>
		/// <param name="armorMaterial"></param>
		public static GearSet Equip(Character character, JobEntry job, ItemGrade grade = ItemGrade.Normal, ArmorMaterialType armorMaterial = ArmorMaterialType.Leather)
		{
			var itemLevel = Math.Min(MaxItemLevel, (int)character.Properties.GetFloat(PropertyName.Lv));
			var set = new GearSet { Grade = grade, ItemLevel = itemLevel };

			var weaponData = FindWeapon(job, grade, itemLevel);
			if (weaponData != null)
				Add(set, EquipSlot.RightHand, weaponData);

			if (job.UsesShield)
			{
				var shieldData = FindItem(EquipType.Shield, grade, itemLevel, null);
				if (shieldData != null)
					Add(set, EquipSlot.LeftHand, shieldData);
			}

			foreach (var (slot, type) in ArmorSlots)
			{
				var armorData = FindItem(type, grade, itemLevel, armorMaterial);
				if (armorData != null)
					Add(set, slot, armorData);
			}

			foreach (var pair in set.Items)
				character.Inventory.SetEquipSilent(pair.Key, pair.Value);

			character.Properties.InvalidateAll();
			character.Properties.SetFloat(PropertyName.HP, character.Properties.GetFloat(PropertyName.MHP));
			character.Properties.SetFloat(PropertyName.SP, character.Properties.GetFloat(PropertyName.MSP));

			return set;
		}

		/// <summary>
		/// Returns the class's weapon at the given level, walking its
		/// preference list and falling back to any weapon it can hold, so a
		/// gap in the item pool degrades the measurement instead of failing
		/// it silently.
		/// </summary>
		/// <param name="job"></param>
		/// <param name="grade"></param>
		/// <param name="itemLevel"></param>
		public static ItemData FindWeapon(JobEntry job, ItemGrade grade, int itemLevel)
		{
			foreach (var type in job.Weapons)
			{
				var data = FindItem(type, grade, itemLevel, null);

				if (data != null)
					return data;
			}

			foreach (var type in job.Weapons)
			{
				var data = FindItem(type, ItemGrade.Normal, itemLevel, null);

				if (data != null)
					return data;
			}

			return null;
		}

		/// <summary>
		/// Returns the highest-level item of the given type at or below the
		/// requested level, or null if the pool has none.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="grade"></param>
		/// <param name="itemLevel"></param>
		/// <param name="material"></param>
		public static ItemData FindItem(EquipType type, ItemGrade grade, int itemLevel, ArmorMaterialType? material)
		{
			var candidates = ZoneServer.Instance.Data.ItemDb.Entries.Values
				.Where(i => i.EquipType1 == type)
				.Where(i => !string.IsNullOrEmpty(i.EquipSlot))
				.Where(i => i.Grade == grade)
				.Where(i => i.MinLevel > 0 && i.MinLevel <= itemLevel)
				.Where(i => material == null || i.Material == material.Value)
				.Where(i => !IsExcluded(i))
				.OrderByDescending(i => i.MinLevel)
				.ThenBy(i => i.Id)
				.ToArray();

			return candidates.FirstOrDefault();
		}

		/// <summary>
		/// Returns true for equipment a player would not fight in. Pet gear
		/// shares the weapon equip types, and at lv75 it was outranking every
		/// real sword.
		/// </summary>
		/// <param name="data"></param>
		private static bool IsExcluded(ItemData data)
		{
			if (data.ClassName == null)
				return false;

			return data.ClassName.StartsWith("PET", StringComparison.OrdinalIgnoreCase);
		}

		private static void Add(GearSet set, EquipSlot slot, ItemData data)
			=> set.Items[slot] = new Item(data.Id);
	}
}
