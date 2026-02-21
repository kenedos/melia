using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.Versioning;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class SkillTreeData
	{
		public JobId JobId { get; set; }
		public SkillId SkillId { get; set; }
		public int UnlockLevel { get; set; }
		public int MaxLevel { get; set; }
		public int UnlockCircle { get; set; }
		public int LevelPerCircle { get; set; }
	}

	/// <summary>
	/// Skill tree database.
	/// </summary>
	public class SkillTreeDb : DatabaseJson<SkillTreeData>
	{
		/// <summary>
		/// Returns all skills the given job can learn at a certain job
		/// level.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="jobLevel"></param>
		/// <returns></returns>
		public SkillTreeData[] FindSkills(JobId jobId, int jobLevel)
		{
			return this.Entries.Where(a => a.JobId == jobId && a.UnlockLevel <= jobLevel).ToArray();
		}

		/// <summary>
		/// Returns all skills the given job can learn at a certain class
		/// level.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="circle"></param>
		/// <returns></returns>
		public SkillTreeData[] FindSkills(JobId jobId, JobCircle circle)
		{
			return this.Entries.Where(a => a.JobId == jobId && a.UnlockCircle <= (int)circle).ToArray();
		}

		/// <summary>
		/// Returns all jobs that contain the given skill at a certain job
		/// level.
		/// </summary>
		/// <returns></returns>
		public SkillTreeData[] FindJobs(SkillId skillId, int jobLevel)
		{
			return this.Entries.Where(a => a.SkillId == skillId && a.UnlockLevel <= jobLevel).ToArray();
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			if (Versions.Protocol > 500)
				entry.AssertNotMissing("jobId", "skillId", "unlockLevel", "maxLevel");
			else
				entry.AssertNotMissing("jobId", "skillId", "unlockCircle", "levelsPerCircle", "maxLevel");

			var data = new SkillTreeData();

			data.JobId = (JobId)entry.ReadInt("jobId");
			data.SkillId = (SkillId)entry.ReadInt("skillId");
			if (Versions.Protocol > 500)
				data.UnlockLevel = entry.ReadInt("unlockLevel");
			else
			{
				data.UnlockCircle = entry.ReadInt("unlockCircle");
				data.LevelPerCircle = entry.ReadInt("levelsPerCircle");
			}
			data.MaxLevel = entry.ReadInt("maxLevel");

			this.Add(data);
		}
	}
}
