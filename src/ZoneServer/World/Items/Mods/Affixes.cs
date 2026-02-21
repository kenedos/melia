using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Items.Mods
{
	public class AffixGenerator
	{
		private static readonly Dictionary<string, (int Min, int Max, string Name)[]> StatAffixes = new()
		{
			{ "DEX", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Swift"),
					(5, 8, "Velocity"),
					(9, 10, "Time-Bender")
				}
			},
			{ "STR", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Buff"),
					(5, 8, "Powerful"),
					(9, 10, "World-Breaker")
				}
			},
			{ "CON", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Tough"),
					(5, 8, "Resilient"),
					(9, 10, "Eon-Lasting")
				}
			},
			{ "INT", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Keen-Mind"),
					(5, 8, "Brilliant"),
					(9, 10, "Mind-Bender")
				}
			},
			{ "MNA", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Wise"),
					(5, 8, "Contemplative"),
					(9, 10, "Cosmos-Sage")
				}
			},
			{ "RSP", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Lively"),
					(5, 8, "Energetic"),
					(9, 10, "Life-Fountain")
				}
			},
			{ "LootingChance", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Lucky"),
					(5, 8, "Fortunate"),
					(9, 10, "Fate-Bender")
				}
			},
			{ "RHP", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Healing"),
					(5, 8, "Restorative"),
					(9, 10, "Life-Forger")
				}
			},
			{ "CRTHR", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Sharp"),
					(5, 8, "Piercing"),
					(9, 10, "Truth-Seeker")
				}
			},
			{ "CRTDR", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Steadfast"),
					(5, 8, "Resolute"),
					(9, 10, "Fate-Defier")
				}
			},
			{ "CRTATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Keen-Edge"),
					(5, 8, "Devastating"),
					(9, 10, "World-Ender")
				}
			},
			{ "ADD_HR", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Precise"),
					(5, 8, "Unerring"),
					(9, 10, "Omni-Sight")
				}
			},
			{ "ADD_DR", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Agile"),
					(5, 8, "Elusive"),
					(9, 10, "Void-Walker")
				}
			},
			{ "BLK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Solid"),
					(5, 8, "Immovable"),
					(9, 10, "Omni-Shield")
				}
			},
			{ "BLK_BREAK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Piercing"),
					(5, 8, "Shattering"),
					(9, 10, "Wall-Breaker")
				}
			},
			{ "PATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Bold"),
					(5, 8, "Mighty"),
					(9, 10, "Force-Wielder")
				}
			},
			{ "ADD_MATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Fey"),
					(5, 8, "Esoteric"),
					(9, 10, "Spell-Weaver")
				}
			},
			{ "ADD_DEF", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Sturdy"),
					(5, 8, "Impenetrable"),
					(9, 10, "Force-Field")
				}
			},
			{ "ADD_MDEF", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Warded"),
					(5, 8, "Mystical"),
					(9, 10, "Magic-Null")
				}
			},
			{ "MHP", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Robust"),
					(5, 8, "Tenacious"),
					(9, 10, "Life-Eternal")
				}
			},
			{ "MSP", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Potent"),
					(5, 8, "Limitless"),
					(9, 10, "Mana-Infinite")
				}
			},
			{ "MSTA", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Fit"),
					(5, 8, "Enduring"),
					(9, 10, "Force-Eternal")
				}
			},
			{ "MINATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Stable"),
					(5, 8, "Steadfast"),
					(9, 10, "Base-Master")
				}
			},
			{ "MAXATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Peak"),
					(5, 8, "Paramount"),
					(9, 10, "Apex-Ruler")
				}
			},
			{ "ADD_MINATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Sure"),
					(5, 8, "Constant"),
					(9, 10, "Floor-Raiser")
				}
			},
			{ "ADD_MAXATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "High"),
					(5, 8, "Dominant"),
					(9, 10, "Peak-Breaker")
				}
			},
			{ "Slash", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Cut"),
					(5, 8, "Lacerate"),
					(9, 10, "Blade-Master")
				}
			},
			{ "SlashDEF", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Thick"),
					(5, 8, "Reinforced"),
					(9, 10, "Blade-Ward")
				}
			},
			{ "Aries", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Fierce"),
					(5, 8, "Lunging"),
					(9, 10, "Pierce-Master")
				}
			},
			{ "AriesDEF", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Rooted"),
					(5, 8, "Unmovable"),
					(9, 10, "Pierce-Ward")
				}
			},
			{ "Strike", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Slam"),
					(5, 8, "Crushing"),
					(9, 10, "Strike-Master")
				}
			},
			{ "StrikeDEF", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Hard"),
					(5, 8, "Fortified"),
					(9, 10, "Strike-Ward")
				}
			},
			{ "ADD_SMALLSIZE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Quick"),
					(5, 8, "Diminutive"),
					(9, 10, "Small-Slayer")
				}
			},
			{ "ADD_MIDDLESIZE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Balanced"),
					(5, 8, "Moderate"),
					(9, 10, "Mid-Slayer")
				}
			},
			{ "ADD_LARGESIZE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Big"),
					(5, 8, "Colossal"),
					(9, 10, "Large-Slayer")
				}
			},
			{ "ADD_BOSS_ATK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Killer"),
					(5, 8, "Destroyer"),
					(9, 10, "Boss-Slayer")
				}
			},
			{ "ADD_VELIAS", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Pure"),
					(5, 8, "Exorcist"),
					(9, 10, "Demon-Slayer")
				}
			},
			{ "ADD_WIDLING", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Wild"),
					(5, 8, "Untamed"),
					(9, 10, "Beast-Slayer")
				}
			},
			{ "ADD_PARAMUNE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Strange"),
					(5, 8, "Ubiquitous"),
					(9, 10, "Mutant-Slayer")
				}
			},
			{ "ADD_FORESTER", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Green"),
					(5, 8, "Woodland"),
					(9, 10, "Plant-Slayer")
				}
			},
			{ "ADD_KLAIDA", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Bug"),
					(5, 8, "Pesticide"),
					(9, 10, "Insect-Slayer")
				}
			},
			{ "ADD_FIRE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Hot"),
					(5, 8, "Burning"),
					(9, 10, "Flame-Lord")
				}
			},
			{ "ADD_ICE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Cold"),
					(5, 8, "Freezing"),
					(9, 10, "Frost-Lord")
				}
			},
			{ "ADD_LIGHTNING", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Fast"),
					(5, 8, "Lightning"),
					(9, 10, "Storm-Lord")
				}
			},
			{ "ADD_POISON", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Toxic"),
					(5, 8, "Venomous"),
					(9, 10, "Plague-Lord")
				}
			},
			{ "ADD_EARTH", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Stout"),
					(5, 8, "Earthen"),
					(9, 10, "Earth-Lord")
				}
			},
			{ "ADD_HOLY", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Sacred"),
					(5, 8, "Blessed"),
					(9, 10, "Holy-Lord")
				}
			},
			{ "ADD_DARK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Dark"),
					(5, 8, "Shadowy"),
					(9, 10, "Dark-Lord")
				}
			},
			{ "ADD_SOUL", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Soul"),
					(5, 8, "Spiritual"),
					(9, 10, "Spirit-Lord")
				}
			},
			{ "RES_POISON", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Cleansed"),
					(5, 8, "Antitoxin"),
					(9, 10, "Plague-Ward")
				}
			},
			{ "RES_EARTH", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Light"),
					(5, 8, "Floating"),
					(9, 10, "Earth-Ward")
				}
			},
			{ "RES_HOLY", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Obscured"),
					(5, 8, "Veiled"),
					(9, 10, "Holy-Ward")
				}
			},
			{ "RES_DARK", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Bright"),
					(5, 8, "Luminous"),
					(9, 10, "Dark-Ward")
				}
			},
			{ "RES_FIRE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Cool"),
					(5, 8, "Frostened"),
					(9, 10, "Fire-Ward")
				}
			},
			{ "RES_ICE", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Shock"),
					(5, 8, "Electric"),
					(9, 10, "Ice-Ward")
				}
			},
			{ "RES_LIGHTNING", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Dense"),
					(5, 8, "Grounded"),
					(9, 10, "Lightning-Ward")
				}
			},
			{ "RES_SOUL", new (int Min, int Max, string Name)[]
				{
					(1, 4, "Bound"),
					(5, 8, "Soulbound"),
					(9, 10, "Spirit-Ward")
				}
			}
		};

		/// <summary>
		/// Gets the affix of the item given a stat name, an item grade, and
		/// how many stats the item has.
		/// </summary>
		/// <param name="stat"></param>
		/// <param name="itemGrade"></param>
		/// <returns></returns>
		public static string GetAffixName(string stat, ItemGrade itemGrade, int statsCount)
		{
			if (StatAffixes.TryGetValue(stat, out var ranges))
			{
				foreach (var (Min, Max, Name) in ranges)
				{
					var itemQuality = (int)itemGrade + statsCount;
					if (itemQuality >= Min && itemQuality <= Max)
					{
						return Name;
					}
				}
			}

			return "Mysterious";
		}
	}
}
