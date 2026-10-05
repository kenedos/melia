//--- Melia Script ----------------------------------------------------------
// Skill Gem Class Constants
//--- Description -----------------------------------------------------------
// Shared list of job classes that have skill gems available.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;

public static class SkillGemConst
{
	public static readonly HashSet<string> AllowedClasses = new(StringComparer.OrdinalIgnoreCase)
	{
		"Swordman",
		"Highlander",
		"Peltasta",
		"Barbarian",
		"Hoplite",
		"Cataphract",
		"Rodelero",
		"Archer",
		"Ranger",
		"Sapper",
		"QuarrelShooter",
		"Wugushi",
		"Fletcher",
		"Hunter",
		"Wizard",
		"Pyromancer",
		"Cryomancer",
		"Psychokino",
		"Bokor",
		"Chronomancer",
		"Elementalist",
		"Cleric",
		"Priest",
		"Kriwi",
		"Paladin",
		"Dievdirbys",
		"Sadhu",
		"Monk",
		"Scout",
		"Linker",
		"Assassin",
		"OutLaw",
		"Corsair",
		"Thaumaturge",
		"Rogue",
		"Enchanter",
		"Schwarzereiter",
		"RuneCaster",
		"NakMuay",
		"BulletMarker",
		"Shinobi",
		"Miko",
		"Appraiser",
		"Mergen",
		"PlagueDoctor",
		"BlossomBlader",
		"Musketeer",
		"Fencer",
		"Sorcerer",
		"Necromancer",
		"Falconer",
		"PiedPiper",
		"Templer",
		"Zealot",
		"Druid",
		"Exorcist",
		"Inquisitor",
		"Cannoneer",
		"Onmyoji",
		"Sage",
	};
}
