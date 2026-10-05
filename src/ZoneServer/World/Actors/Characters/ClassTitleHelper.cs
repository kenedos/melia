using Melia.Shared.Game.Const;

namespace Melia.Zone.World.Actors.Characters
{
	public static class ClassTitleHelper
	{
		public static void Grant(Character character, JobId jobId)
		{
			var achievementPointName = jobId switch
			{
				// Swordsman
				JobId.Highlander => "WeeklyRankClass_1",
				JobId.Peltasta => "WeeklyRankClass_2",
				JobId.Hoplite => "WeeklyRankClass_3",
				JobId.Barbarian => "WeeklyRankClass_4",
				JobId.Cataphract => "WeeklyRankClass_5",
				JobId.Doppelsoeldner => "WeeklyRankClass_6",
				JobId.Rodelero => "WeeklyRankClass_7",
				JobId.Murmillo => "WeeklyRankClass_8",
				JobId.Fencer => "WeeklyRankClass_9",
				JobId.Dragoon => "WeeklyRankClass_10",
				JobId.Templar => "WeeklyRankClass_11",
				JobId.Lancer => "WeeklyRankClass_12",
				JobId.Matador => "WeeklyRankClass_13",
				JobId.NakMuay => "WeeklyRankClass_14",
				JobId.Retiarius => "WeeklyRankClass_15",
				JobId.Hackapell => "WeeklyRankClass_16",
				JobId.BlossomBlader => "WeeklyRankClass_17",
				JobId.Shenji => "WeeklyRankClass_95",
				JobId.WingedHussar => "WeeklyRankClass_99",
				JobId.Vanquisher => "WeeklyRankClass_102",
				JobId.SledgerS => "WeeklyRankClass_104",
				JobId.BonemancerS => "WeeklyRankClass_105",
				JobId.GrimmarkS => "WeeklyRankClass_109",
				JobId.Eskrimer => "WeeklyRankClass_111",

				// Wizard
				JobId.Pyromancer => "WeeklyRankClass_18",
				JobId.Cryomancer => "WeeklyRankClass_19",
				JobId.Psychokino => "WeeklyRankClass_20",
				JobId.Alchemist => "WeeklyRankClass_21",
				JobId.Sorcerer => "WeeklyRankClass_22",
				JobId.Chronomancer => "WeeklyRankClass_23",
				JobId.Necromancer => "WeeklyRankClass_24",
				JobId.Elementalist => "WeeklyRankClass_25",
				JobId.Sage => "WeeklyRankClass_26",
				JobId.Warlock => "WeeklyRankClass_27",
				JobId.Featherfoot => "WeeklyRankClass_28",
				JobId.RuneCaster => "WeeklyRankClass_29",
				JobId.Shadowmancer => "WeeklyRankClass_30",
				JobId.Onmyoji => "WeeklyRankClass_31",
				JobId.Taoist => "WeeklyRankClass_32",
				JobId.Bokor => "WeeklyRankClass_33",
				JobId.Terramancer => "WeeklyRankClass_34",
				JobId.Keraunos => "WeeklyRankClass_92",
				JobId.Illusionist => "WeeklyRankClass_98",
				JobId.VultureW => "WeeklyRankClass_103",
				JobId.BonemancerW => "WeeklyRankClass_105",
				JobId.AetherBladerW => "WeeklyRankClass_107",
				JobId.HermitW => "WeeklyRankClass_108",

				// Archer
				JobId.Hunter => "WeeklyRankClass_35",
				JobId.QuarrelShooter => "WeeklyRankClass_36",
				JobId.Ranger => "WeeklyRankClass_37",
				JobId.Sapper => "WeeklyRankClass_38",
				JobId.Wugushi => "WeeklyRankClass_39",
				JobId.Fletcher => "WeeklyRankClass_40",
				JobId.PiedPiper => "WeeklyRankClass_41",
				JobId.Appraiser => "WeeklyRankClass_42",
				JobId.Falconer => "WeeklyRankClass_43",
				JobId.Cannoneer => "WeeklyRankClass_44",
				JobId.Musketeer => "WeeklyRankClass_45",
				JobId.Mergen => "WeeklyRankClass_46",
				JobId.Matross => "WeeklyRankClass_47",
				JobId.TigerHunter => "WeeklyRankClass_48",
				JobId.Arbalester => "WeeklyRankClass_49",
				JobId.Arquebusier => "WeeklyRankClass_50",
				JobId.Hwarang => "WeeklyRankClass_90",
				JobId.Engineer => "WeeklyRankClass_96",
				JobId.Godeye => "WeeklyRankClass_100",
				JobId.VultureA => "WeeklyRankClass_103",
				JobId.BonemancerA => "WeeklyRankClass_105",
				JobId.BlitzHunterA => "WeeklyRankClass_106",
				JobId.HermitA => "WeeklyRankClass_108",

				// Cleric
				JobId.Priest => "WeeklyRankClass_51",
				JobId.Krivis => "WeeklyRankClass_52",
				JobId.Druid => "WeeklyRankClass_53",
				JobId.Sadhu => "WeeklyRankClass_54",
				JobId.Dievdirbys => "WeeklyRankClass_55",
				JobId.Oracle => "WeeklyRankClass_56",
				JobId.Monk => "WeeklyRankClass_57",
				JobId.Pardoner => "WeeklyRankClass_58",
				JobId.Paladin => "WeeklyRankClass_59",
				JobId.Chaplain => "WeeklyRankClass_60",
				JobId.PlagueDoctor => "WeeklyRankClass_61",
				JobId.Kabbalist => "WeeklyRankClass_62",
				JobId.Inquisitor => "WeeklyRankClass_63",
				JobId.Miko => "WeeklyRankClass_64",
				JobId.Zealot => "WeeklyRankClass_66",
				JobId.Exorcist => "WeeklyRankClass_67",
				JobId.Crusader => "WeeklyRankClass_68",
				JobId.Lama => "WeeklyRankClass_93",
				JobId.Pontifex => "WeeklyRankClass_97",
				JobId.SledgerC => "WeeklyRankClass_104",
				JobId.BonemancerC => "WeeklyRankClass_105",
				JobId.AetherBladerC => "WeeklyRankClass_107",
				JobId.HermitC => "WeeklyRankClass_108",

				// Scout
				JobId.Assassin => "WeeklyRankClass_70",
				JobId.Outlaw => "WeeklyRankClass_71",
				JobId.Squire => "WeeklyRankClass_72",
				JobId.Corsair => "WeeklyRankClass_73",
				JobId.Shinobi => "WeeklyRankClass_74",
				JobId.Thaumaturge => "WeeklyRankClass_75",
				JobId.Enchanter => "WeeklyRankClass_76",
				JobId.Linker => "WeeklyRankClass_77",
				JobId.Rogue => "WeeklyRankClass_78",
				JobId.SchwarzerReiter => "WeeklyRankClass_79",
				JobId.BulletMarker => "WeeklyRankClass_80",
				JobId.Ardito => "WeeklyRankClass_81",
				JobId.Sheriff => "WeeklyRankClass_82",
				JobId.Rangda => "WeeklyRankClass_83",
				JobId.Clown => "WeeklyRankClass_84",
				JobId.Hakkapeliter => "WeeklyRankClass_91",
				JobId.Jaguar => "WeeklyRankClass_94",
				JobId.VultureT => "WeeklyRankClass_103",
				JobId.BlitzHunterT => "WeeklyRankClass_106",
				JobId.AetherBladerT => "WeeklyRankClass_107",
				JobId.GrimmarkT => "WeeklyRankClass_109",
				JobId.KnellerT => "WeeklyRankClass_110",

				// Base classes
				JobId.Swordsman => "WeeklyRankClass_85",
				JobId.Archer => "WeeklyRankClass_86",
				JobId.Cleric => "WeeklyRankClass_87",
				JobId.Wizard => "WeeklyRankClass_88",

				_ => null
			};

			if (achievementPointName == null)
				return;

			var separatorIndex = achievementPointName.LastIndexOf('_');

			if (separatorIndex < 0 ||
					!int.TryParse(achievementPointName[(separatorIndex + 1)..], out var classTitleId))
				return;

			var achievementId = 160000 + classTitleId;

			if (character.Achievements.HasAchievement(achievementId))
				return;

			character.Achievements.AddAchievementPoints(achievementPointName, 1);
		}
	}
}
