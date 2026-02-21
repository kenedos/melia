using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class SimonyData
	{
		/// <summary>
		/// The skill type ID that can be crafted.
		/// </summary>
		public int SkillType { get; set; }

		/// <summary>
		/// The skill ID enum value.
		/// </summary>
		public SkillId SkillId { get; set; }

		/// <summary>
		/// The skill class name.
		/// </summary>
		public string ClassName { get; set; }

		/// <summary>
		/// Display name of the skill.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Whether this skill can be made into a scroll.
		/// </summary>
		public bool CanMake { get; set; } = true;

		/// <summary>
		/// Minimum Simony skill level required to craft this scroll.
		/// </summary>
		public int RequiredLevel { get; set; } = 1;

		/// <summary>
		/// Base silver cost for crafting.
		/// </summary>
		public long BaseCost { get; set; } = 1000;

		/// <summary>
		/// Additional silver cost per skill level.
		/// </summary>
		public long CostPerLevel { get; set; } = 500;

		/// <summary>
		/// Material item class name required for crafting.
		/// </summary>
		public string MaterialClassName { get; set; } = "misc_parchment";

		/// <summary>
		/// Base number of material items required per scroll.
		/// </summary>
		public int MaterialCount { get; set; } = 1;

		/// <summary>
		/// Additional materials required per skill level.
		/// </summary>
		public int MaterialPerLevel { get; set; } = 0;

		/// <summary>
		/// Time in seconds to craft one scroll.
		/// </summary>
		public float CraftTime { get; set; } = 3.0f;

		/// <summary>
		/// The buff ID applied when using this scroll (if applicable).
		/// </summary>
		public BuffId BuffId { get; set; } = BuffId.None;

		/// <summary>
		/// Duration of the buff in milliseconds when used from scroll.
		/// </summary>
		public int BuffDuration { get; set; } = 1800000; // 30 minutes default
	}

	/// <summary>
	/// Database for Simony skill scroll crafting data.
	/// Defines which skills can be crafted into scrolls.
	/// </summary>
	public class SimonyDb : DatabaseJsonIndexed<int, SimonyData>
	{
		/// <summary>
		/// Returns first simony data entry with given class name, or null
		/// if it wasn't found.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public SimonyData Find(string className)
			=> this.Find(a => a.ClassName == className);

		/// <summary>
		/// Returns the simony data entry if found by class name,
		/// otherwise returns false.
		/// </summary>
		/// <param name="className"></param>
		/// <param name="simonyData"></param>
		/// <returns></returns>
		public bool TryFind(string className, out SimonyData simonyData)
		{
			simonyData = this.Find(className);
			return simonyData != null;
		}

		/// <summary>
		/// Returns first simony data entry with given skill ID, or null
		/// if it wasn't found.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public SimonyData FindBySkillId(SkillId skillId)
			=> this.Find(a => a.SkillId == skillId);

		/// <summary>
		/// Returns the simony data entry if found by skill ID,
		/// otherwise returns false.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="simonyData"></param>
		/// <returns></returns>
		public bool TryFindBySkillId(SkillId skillId, out SimonyData simonyData)
		{
			simonyData = this.FindBySkillId(skillId);
			return simonyData != null;
		}

		/// <summary>
		/// Returns all simony entries that can be crafted.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<SimonyData> GetCraftable()
			=> this.Entries.Values.Where(a => a.CanMake);

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("skillType", "className");

			var data = new SimonyData();

			data.SkillType = entry.ReadInt("skillType");
			data.SkillId = (SkillId)entry.ReadInt("skillType");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name", data.ClassName);

			data.CanMake = entry.ReadBool("canMake", true);
			data.RequiredLevel = entry.ReadInt("requiredLevel", 1);

			data.BaseCost = entry.ReadLong("baseCost", 1000);
			data.CostPerLevel = entry.ReadLong("costPerLevel", 500);

			data.MaterialClassName = entry.ReadString("materialClassName", "misc_scrollpaper");
			data.MaterialCount = entry.ReadInt("materialCount", 1);
			data.MaterialPerLevel = entry.ReadInt("materialPerLevel", 0);

			data.CraftTime = entry.ReadFloat("craftTime", 3.0f);

			data.BuffId = entry.ReadEnum("buffId", BuffId.None);
			data.BuffDuration = entry.ReadInt("buffDuration", 1800000);

			this.AddOrReplace(data.SkillType, data);
		}
	}
}
