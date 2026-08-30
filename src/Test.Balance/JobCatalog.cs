using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;

namespace Melia.Test.Balance
{
	/// <summary>
	/// One in-scope job class, with everything a scenario needs to build a
	/// representative character of it.
	/// </summary>
	public class JobEntry
	{
		/// <summary>
		/// Prefix its skills' class names carry, and the name
		/// skill_gem_classes.cs uses. Not always the JobId's name.
		/// </summary>
		public string SkillPrefix { get; init; }

		public JobId JobId { get; init; }

		/// <summary>
		/// Weapon types this class fights with, most representative first.
		/// </summary>
		public EquipType[] Weapons { get; init; }

		/// <summary>
		/// Whether the class's reference build carries a shield, which is
		/// what puts block on the board for it.
		/// </summary>
		public bool UsesShield { get; init; }

		public JobClass BaseJob => this.JobId.ToClass();

		public override string ToString() => $"{this.SkillPrefix} ({this.JobId})";
	}

	/// <summary>
	/// What a skill is for, which decides whether a zero reading is a
	/// finding or the expected answer.
	/// </summary>
	public enum SkillRole
	{
		/// <summary>
		/// Deals damage through its own factor, so the direct-hit model
		/// prices it.
		/// </summary>
		Direct,

		/// <summary>
		/// Carries a factor but no attack type, so whatever damage it does
		/// comes from a pet, a pad or handler logic. Only the encounter probe
		/// can price it.
		/// </summary>
		Indirect,

		/// <summary>
		/// A passive, a toggle or a self-cast buff. It has no damage of its
		/// own and no target to aim at.
		/// </summary>
		Utility,
	}

	/// <summary>
	/// One skill of a class, with what it is for and how far its tree
	/// allows it to be levelled.
	/// </summary>
	public class SkillEntry
	{
		public SkillData Data { get; init; }
		public SkillRole Role { get; init; }
		public int MaxLevel { get; init; }
		public int UnlockLevel { get; init; }

		public SkillId Id => this.Data.Id;
		public string ClassName => this.Data.ClassName;

		public override string ToString() => $"{this.ClassName} ({this.Role}, max sk{this.MaxLevel})";
	}

	/// <summary>
	/// The 45 job classes in scope, taken from skill_gem_classes.cs, and the
	/// skills each of them owns.
	/// </summary>
	public static class JobCatalog
	{
		/// <summary>
		/// The in-scope classes. Kept in the same order as
		/// skill_gem_classes.cs so the two lists can be diffed by eye.
		/// </summary>
		public static readonly JobEntry[] Entries =
		[
			Job("Swordman", JobId.Swordsman, true, EquipType.Sword, EquipType.THSword),
			Job("Highlander", JobId.Highlander, false, EquipType.THSword, EquipType.Sword),
			Job("Peltasta", JobId.Peltasta, true, EquipType.Sword),
			Job("Barbarian", JobId.Barbarian, false, EquipType.THSword, EquipType.Sword),
			Job("Hoplite", JobId.Hoplite, true, EquipType.Spear, EquipType.THSpear),
			Job("Cataphract", JobId.Cataphract, false, EquipType.THSpear, EquipType.Spear),
			Job("Rodelero", JobId.Rodelero, true, EquipType.Sword),
			Job("Doppelsoeldner", JobId.Doppelsoeldner, false, EquipType.THSword, EquipType.Sword),
			Job("Fencer", JobId.Fencer, false, EquipType.Rapier, EquipType.Sword),

			Job("Archer", JobId.Archer, false, EquipType.Bow, EquipType.THBow),
			Job("Ranger", JobId.Ranger, false, EquipType.Bow, EquipType.THBow),
			Job("Sapper", JobId.Sapper, false, EquipType.Bow, EquipType.THBow),
			Job("QuarrelShooter", JobId.QuarrelShooter, true, EquipType.Bow),
			Job("Wugushi", JobId.Wugushi, false, EquipType.Bow, EquipType.THBow),
			Job("Fletcher", JobId.Fletcher, false, EquipType.Bow, EquipType.THBow),
			Job("Hunter", JobId.Hunter, false, EquipType.Bow, EquipType.THBow),
			Job("Falconer", JobId.Falconer, false, EquipType.Bow, EquipType.THBow),
			Job("Musketeer", JobId.Musketeer, false, EquipType.Musket, EquipType.THBow),

			Job("Wizard", JobId.Wizard, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Pyromancer", JobId.Pyromancer, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Cryomancer", JobId.Cryomancer, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Psychokino", JobId.Psychokino, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Bokor", JobId.Bokor, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Chronomancer", JobId.Chronomancer, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Elementalist", JobId.Elementalist, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Sorcerer", JobId.Sorcerer, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),
			Job("Necromancer", JobId.Necromancer, false, EquipType.Staff, EquipType.THStaff, EquipType.Wand),

			Job("Cleric", JobId.Cleric, true, EquipType.Mace, EquipType.THMace),
			Job("Priest", JobId.Priest, true, EquipType.Mace, EquipType.THMace),
			Job("Kriwi", JobId.Krivis, true, EquipType.Mace, EquipType.THMace),
			Job("Paladin", JobId.Paladin, true, EquipType.Mace, EquipType.THMace),
			Job("Dievdirbys", JobId.Dievdirbys, true, EquipType.Mace, EquipType.THMace),
			Job("Sadhu", JobId.Sadhu, false, EquipType.Mace, EquipType.THMace),
			Job("Monk", JobId.Monk, false, EquipType.Mace, EquipType.THMace),
			Job("Pardoner", JobId.Pardoner, true, EquipType.Mace, EquipType.THMace),
			Job("Oracle", JobId.Oracle, true, EquipType.Mace, EquipType.THMace),

			Job("Scout", JobId.Scout, false, EquipType.Dagger, EquipType.Pistol),
			Job("Linker", JobId.Linker, false, EquipType.Dagger),
			Job("Assassin", JobId.Assassin, false, EquipType.Dagger),
			Job("OutLaw", JobId.Outlaw, false, EquipType.Dagger),
			Job("Corsair", JobId.Corsair, false, EquipType.Pistol, EquipType.Dagger),
			Job("Thaumaturge", JobId.Thaumaturge, false, EquipType.Dagger),
			Job("Rogue", JobId.Rogue, false, EquipType.Dagger),
			Job("Squire", JobId.Squire, false, EquipType.Sword, EquipType.Dagger),
			Job("Schwarzereiter", JobId.SchwarzerReiter, false, EquipType.Pistol, EquipType.Dagger),
		];

		/// <summary>
		/// Returns the in-scope classes of the given base job, in catalog
		/// order, which is the order Phase 4 works through them.
		/// </summary>
		/// <param name="baseJob"></param>
		public static JobEntry[] ByBaseJob(JobClass baseJob)
			=> Entries.Where(e => e.BaseJob == baseJob).ToArray();

		/// <summary>
		/// Returns the entry with the given skill prefix.
		/// </summary>
		/// <param name="skillPrefix"></param>
		/// <param name="entry"></param>
		public static bool TryGet(string skillPrefix, out JobEntry entry)
		{
			entry = Entries.FirstOrDefault(e => string.Equals(e.SkillPrefix, skillPrefix, StringComparison.OrdinalIgnoreCase));

			return entry != null;
		}

		/// <summary>
		/// Returns every skill in the class's tree, with the max level the
		/// tree allows for each.
		/// </summary>
		/// <param name="entry"></param>
		public static SkillTreeData[] GetSkills(JobEntry entry)
		{
			return ZoneServer.Instance.Data.SkillTreeDb.Entries
				.Where(s => s.JobId == entry.JobId)
				.ToArray();
		}

		/// <summary>
		/// Returns every castable skill of the class, classified by role and
		/// carrying the tree's level cap.
		/// </summary>
		/// <param name="entry"></param>
		public static SkillEntry[] GetProfiledSkills(JobEntry entry)
		{
			var skills = new List<SkillEntry>();

			foreach (var tree in GetSkills(entry))
			{
				if (!ZoneServer.Instance.Data.SkillDb.TryFind(tree.SkillId, out var data))
					continue;

				skills.Add(new SkillEntry
				{
					Data = data,
					Role = Classify(data),
					MaxLevel = Math.Max(1, GetTreeMaxLevel(tree)),
					UnlockLevel = tree.UnlockLevel,
				});
			}

			return skills.ToArray();
		}

		/// <summary>
		/// Returns a skill's level cap on a fully advanced class, which the
		/// class circle system derives from the skill's own circle.
		/// </summary>
		/// <param name="tree"></param>
		private static int GetTreeMaxLevel(SkillTreeData tree)
		{
			if (!ZoneServer.Instance.Conf.World.ClassCircleSystem)
				return tree.MaxLevel;

			var levelsPerCircle = ZoneServer.Instance.Conf.World.MaxAdvanceJobLevel;
			return JobCircleHelper.GetSkillMaxLevel(JobCircle.Third, tree.UnlockLevel, tree.MaxLevel, levelsPerCircle);
		}

		/// <summary>
		/// Returns the class's damage-dealing skills, which are the ones
		/// Phase 4 assigns a budget to.
		/// </summary>
		/// <param name="entry"></param>
		public static SkillEntry[] GetDamageSkills(JobEntry entry)
			=> GetProfiledSkills(entry).Where(s => s.Role != SkillRole.Utility).ToArray();

		/// <summary>
		/// Decides what a skill is for from its own data.
		/// </summary>
		/// <remarks>
		/// A passive cannot be cast, a self-cast skill has no target to aim
		/// at, and a skill with no attack type does not damage anything
		/// through the direct-hit path even when it carries a factor - the
		/// Hunter's pet skills and the Corsair's hook are the examples.
		/// </remarks>
		/// <param name="data"></param>
		private static SkillRole Classify(SkillData data)
		{
			if (data.ActivationType == SkillActivationType.PassiveSkill)
				return SkillRole.Utility;

			if (data.UseType == SkillUseType.Self)
				return SkillRole.Utility;

			if (data.Factor <= 0 && data.AtkAdd <= 0)
				return SkillRole.Utility;

			if (data.AttackType == SkillAttackType.None)
				return SkillRole.Indirect;

			return SkillRole.Direct;
		}

		/// <summary>
		/// Returns the stat the class's damage scales on, decided by whether
		/// its damage skills are mostly magic. Derived rather than declared,
		/// so it cannot drift from the skill data.
		/// </summary>
		/// <param name="entry"></param>
		public static string GetPrimaryStat(JobEntry entry)
		{
			var damageSkills = GetDamageSkills(entry);

			if (damageSkills.Length == 0)
				return "STR";

			var magic = damageSkills.Count(s => s.Data.AttackType == SkillAttackType.Magic);

			return magic * 2 > damageSkills.Length ? "INT" : "STR";
		}

		/// <summary>
		/// Returns the level the class's own skills unlock at, which is the
		/// earliest character level a scenario for it makes sense at.
		/// </summary>
		/// <param name="entry"></param>
		public static int GetMinLevel(JobEntry entry)
		{
			if (!ZoneServer.Instance.Data.JobDb.TryFind(entry.JobId, out var data))
				return 1;

			// Rank 1 starts at 1; every rank after it needs 15 levels per
			// preceding rank to be reachable at all.
			return Math.Max(1, (data.Rank - 1) * 15);
		}

		private static JobEntry Job(string skillPrefix, JobId jobId, bool usesShield, params EquipType[] weapons)
		{
			return new JobEntry
			{
				SkillPrefix = skillPrefix,
				JobId = jobId,
				UsesShield = usesShield,
				Weapons = weapons,
			};
		}
	}
}
