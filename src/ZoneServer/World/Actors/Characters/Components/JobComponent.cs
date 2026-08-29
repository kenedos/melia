using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Versioning;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Job collection.
	/// </summary>
	public class JobComponent : CharacterComponent
	{
		private static readonly Regex JobClassName = new(@"^Char(?<class>[1-4])_(?<index>[0-9]{1,2})$", RegexOptions.Compiled);

		private readonly Dictionary<JobId, Job> _jobs = new();

		// Cache for job ranks, invalidated when jobs are added/removed
		private Dictionary<JobId, int> _jobRanks = null;

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public JobComponent(Character character) : base(character)
		{
		}

		/// <summary>
		/// Clears all jobs to release references for GC.
		/// </summary>
		public void Clear()
		{
			lock (_jobs)
			{
				_jobs.Clear();
				_jobRanks = null;
			}
		}

		/// <summary>
		/// Returns the amount of jobs in the collection.
		/// </summary>
		public int Count { get { lock (_jobs) return _jobs.Count; } }

		/// <summary>
		/// Adds given without updating the client. Replaces existing
		/// jobs.
		/// </summary>
		/// <param name="job"></param>
		public void AddSilent(Job job)
		{
			// Setting the rank based on the order jobs are added was the
			// simplest solution to add the Rank property after the fact,
			// but this works out well, because we don't have to worry
			// about getting and setting the correct rank manually this
			// way.

			lock (_jobs)
			{
				// A job loaded from the database brings its own rank, which
				// records when its current circle was taken. Only jobs
				// without one get placed at the end of the ladder.
				var rank = job.Rank;

				if (rank <= 0)
				{
					// Every circle the job already carries occupies a rank of
					// its own, so a job loaded at C3 sits three ranks up.
					var circles = ZoneServer.Instance.Conf.World.ClassCircleSystem ? Math.Max(1, (int)job.Circle) : 1;

					rank = circles;

					if (_jobs.Count > 0)
						rank = this.GetCurrentRank() + circles;

					if (_jobs.TryGetValue(job.Id, out var existing))
						rank = existing.Rank;
				}

				_jobs[job.Id] = job;
				job.Rank = rank;
				_jobRanks = null; // Invalidate rank cache
			}
		}

		/// <summary>
		/// Adds given job and updates the client. Replaces existing
		/// jobs.
		/// </summary>
		/// <param name="job"></param>
		public void Add(Job job)
		{
			this.AddSilent(job);

			// The client rebuilds its skill tree on the job change, so the
			// circles have to be in place before it hears about one.
			Send.ZC_NORMAL.JobCircles(this.Character);
			this.Character.Connection?.Party?.UpdateMemberJobs(this.Character);
			Send.ZC_PC(this.Character, PcUpdateType.Job, (int)job.Id, 0);
			Send.ZC_NORMAL.UpdateSkillUI(this.Character);
			this.Character.Properties.SetFloat(PropertyName.Job, (int)job.Id);
			Send.ZC_OBJECT_PROPERTY(this.Character, PropertyName.JobName);
			this.Character.AddonMessage(AddonMessage.JOB_UPDATE);
			this.Character.InvalidateProperties();
			//this.Character.AddonMessage(AddonMessage.START_JOB_CHANGE);
		}

		/// <summary>
		/// Removes job with given id, returns false if it
		/// didn't exist. Doesn't update the client.
		/// </summary>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public bool RemoveSilent(JobId jobId)
		{
			lock (_jobs)
			{
				var removed = _jobs.Remove(jobId);
				if (!removed)
					return false;

				_jobRanks = null; // Invalidate rank cache

				// Switch character's job to another one if the active
				// one was removed. We'll use the last one in the list
				// for now, assuming that it's going to be the other
				// most recent job selected.
				if (this.Character.JobId == jobId)
					this.Character.JobId = _jobs.Last().Value.Id;

				return true;
			}
		}

		/// <summary>
		/// Removes job with given id, returns false if it
		/// didn't exist. Updates the client on success.
		/// </summary>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public bool Remove(JobId jobId)
		{
			if (!this.RemoveSilent(jobId))
				return false;

			// XXX: Seems like this is not enough to get rid of the jobs at
			//   run-time. Is there a way for us to refresh the UI?
			Send.ZC_PC(this.Character, PcUpdateType.Job, (int)this.Character.JobId, _jobs.Last().Value.SkillPoints);
			Send.ZC_NORMAL.UpdateSkillUI(this.Character);
			this.Character.AddonMessage(AddonMessage.JOB_UPDATE);
			this.Character.InvalidateProperties();

			return true;
		}

		/// <summary>
		/// Returns job with given id, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public Job Get(JobId jobId)
		{
			lock (_jobs)
			{
				_jobs.TryGetValue(jobId, out var job);
				return job;
			}
		}

		/// <summary>
		/// Returns the job with the given class name, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="jobClassName"></param>
		/// <returns></returns>
		public Job Get(string jobClassName)
		{
			lock (_jobs)
				return _jobs.Values.FirstOrDefault(job => job.Data.ClassName == jobClassName);
		}

		/// <summary>
		/// Returns job with the given id via out. Returns false if it
		/// wasn't found.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="job"></param>
		/// <returns></returns>
		public bool TryGet(JobId jobId, out Job job)
		{
			lock (_jobs)
				return _jobs.TryGetValue(jobId, out job);
		}

		/// <summary>
		/// Returns a list with all jobs.
		/// </summary>
		/// <returns></returns>
		public Job[] GetList()
		{
			lock (_jobs)
				return _jobs.Values.OrderBy(a => a.SelectionDate).ToArray();
		}

		/// <summary>
		/// Returns true if the job exists, and whether it's at least at the
		/// given circle.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="circle"></param>
		/// <returns></returns>
		public bool Has(JobId jobId, JobCircle circle = JobCircle.First)
		{
			lock (_jobs)
			{
				if (!_jobs.TryGetValue(jobId, out var job))
					return false;

				return (job.Circle >= circle);
			}
		}

		/// <summary>
		/// Changes job's circle if it exists, returns false if not.
		/// Updates client on success.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="circle"></param>
		/// <remarks>
		/// Each circle is its own rank with its own 1~15 level band, so
		/// the job's EXP restarts. Skill points earned on earlier circles
		/// are kept.
		/// </remarks>
		public bool ChangeCircle(JobId jobId, JobCircle circle)
		{
			var job = this.Get(jobId);
			if (job == null)
				return false;

			job.Circle = circle;

			if (ZoneServer.Instance.Conf.World.ClassCircleSystem)
			{
				// The new circle is the character's latest advancement, so it
				// takes the next rank. Jobs taken earlier keep theirs, which
				// is why the rank is stored rather than derived from the
				// order jobs were selected in.
				job.Rank = this.GetCurrentRank();
				job.AdvancementDate = DateTime.Now;

				job.TotalExp = 0;

				// Level 1 of a circle is reached without EXP, so it grants
				// no level up. Its point comes with the circle instead, the
				// same way a freshly picked job is granted one.
				job.ModifySkillPoints(1);
			}

			this.Character.Inventory.RefreshGemSkills();

			// The client rebuilds its skill tree on the job change, so the
			// circles have to be in place before it hears about one.
			Send.ZC_NORMAL.JobCircles(this.Character);
			this.Character.Connection?.Party?.UpdateMemberJobs(this.Character);
			Send.ZC_PC(this.Character, PcUpdateType.Job, (int)job.Id, 0);
			Send.ZC_NORMAL.UpdateSkillUI(this.Character);
			Send.ZC_SKILL_LIST(this.Character);

			this.Character.AddonMessage(AddonMessage.JOB_UPDATE);
			this.Character.AddonMessage("NOTICE_Dm_levelup_skill", "!@#$Auto_KeulLeSeu_LeBeli_SangSeungHayeossSeupNiDa#@!", 3);
			this.Character.PlayEffect("F_pc_joblevel_up", 3);

			return true;
		}

		/// <summary>
		/// Modifies given job's skill points if it exists and updates
		/// the client. Returns false if job wasn't found.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		public bool ModifySkillPoints(JobId jobId, int modifier)
		{
			var job = this.Get(jobId);
			if (job == null)
				return false;

			job.ModifySkillPoints(modifier);

			return true;
		}

		/// <summary>
		/// Returns the circle the character is on on the given job.
		/// </summary>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public JobCircle GetCircle(JobId jobId)
		{
			var job = this.Get(jobId);
			if (job == null)
				return JobCircle.None;

			return job.Circle;
		}

		/// <summary>
		/// Returns the circle the character is on on the given job.
		/// </summary>
		/// <param name="jobClassName"></param>
		/// <returns></returns>
		public JobCircle GetCircle(string jobClassName)
		{
			var jobId = GetJobIdFromClassName(jobClassName);
			return this.GetCircle(jobId);
		}

		/// <summary>
		/// Converts job class name to job id and returns it.
		/// </summary>
		/// <param name="jobClassName"></param>
		/// <returns></returns>
		private static JobId GetJobIdFromClassName(string jobClassName)
		{
			var match = JobClassName.Match(jobClassName);
			if (!match.Success)
				throw new ArgumentException($"Invalid job class name format '{jobClassName}'.");

			var jobClass = int.Parse(match.Groups["class"].Value);
			var index = int.Parse(match.Groups["index"].Value);

			return (JobId)((jobClass * 1000) + index);
		}

		/// <summary>
		/// Returns the character's job rank.
		/// </summary>
		/// <remarks>
		/// I currently don't know how *exactly* this is supposed to work.
		/// It's the equivilant of the client's "GetTotalJobCount" function,
		/// which, I assume returns the rank the character is on with its
		/// jobs, which is kind of based on the amount of jobs, or rather
		/// the circle sum.
		///
		/// For example:
		/// - Swordman Circle 1 = 1
		/// - Swordman Circle 2 = 2
		/// - Swordman Circle 2 + Highlander Circle 1 = 3
		/// - Swordman Circle 2 + Highlander Circle 2 = 4
		/// - etc.
		/// </remarks>
		/// <returns></returns>
		public int GetCurrentRank()
		{
			var rank = 0;
			lock (_jobs)
				rank = _jobs.Values.Sum(a => (int)a.Circle);

			// Even if _jobs is empty right now for some reason, rank 1
			// is always the minimum.
			return Math.Max(1, rank);
		}

		/// <summary>
		/// Drops the cached job ranks, so they're rebuilt on next access.
		/// </summary>
		public void InvalidateRankCache()
		{
			lock (_jobs)
				_jobRanks = null;
		}

		/// <summary>
		/// Returns the EXP table rank for a specific job, based on its
		/// position in the character's job progression (ordered by
		/// selection date).
		/// </summary>
		/// <remarks>
		/// Each job has its own rank for EXP table lookups:
		/// - Rank 1: Base Job (e.g., Cleric)
		/// - Rank 2: First Job Advancement (e.g., Priest)
		/// - Rank 3: Second Job Advancement (e.g., Paladin)
		/// With the class circle system enabled every circle takes the next
		/// rank of the ladder instead, and a job sits on the rank its
		/// current circle was taken on. Since jobs and circles interleave,
		/// the rank is stored rather than derived from the selection order,
		/// which would move a job onto a different EXP curve.
		/// </remarks>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public int GetJobRank(JobId jobId)
		{
			lock (_jobs)
			{
				// Build cache if needed
				if (_jobRanks == null)
				{
					_jobRanks = new Dictionary<JobId, int>();
					var orderedJobs = _jobs.Values.OrderBy(j => j.SelectionDate).ThenBy(j => j.Rank).ToList();
					var circleSystem = ZoneServer.Instance.Conf.World.ClassCircleSystem;
					var nextRank = 1;

					for (var i = 0; i < orderedJobs.Count; i++)
					{
						var job = orderedJobs[i];

						if (!circleSystem)
						{
							_jobRanks[job.Id] = i + 1;
							continue;
						}

						// Every circle takes the next rank of the ladder, and a
						// job sits on the rank its current circle was taken on.
						var circles = Math.Max(1, (int)job.Circle);

						if (job.Rank <= 0)
							job.Rank = nextRank + circles - 1;

						nextRank += circles;

						_jobRanks[job.Id] = job.Rank;
					}
				}

				if (_jobRanks.TryGetValue(jobId, out var rank))
					return rank;

				// Job not found, return max rank as fallback
				return Math.Max(1, _jobRanks.Count);
			}
		}

		/// <summary>
		/// Returns the character's jobs in the order of the ranks they
		/// currently sit on.
		/// </summary>
		/// <remarks>
		/// A job holds the rank its current circle was taken on, and the
		/// ranks its earlier circles sat on are not kept, so this shows the
		/// ladder as it stands rather than every rank ever spent.
		/// </remarks>
		/// <returns></returns>
		public List<JobHistoryEntry> GetHistory()
		{
			var entries = new List<JobHistoryEntry>();

			foreach (var job in this.GetList())
				entries.Add(new JobHistoryEntry(job, this.GetJobRank(job.Id), job.Level, job.TotalExp, job.SkillPoints));

			entries.Sort((a, b) => a.Rank.CompareTo(b.Rank));

			return entries;
		}
	}

	/// <summary>
	/// Represents one rank a character's job occupies.
	/// </summary>
	public readonly struct JobHistoryEntry
	{
		/// <summary>
		/// Returns the job holding this rank.
		/// </summary>
		public Job Job { get; }

		/// <summary>
		/// Returns the rank this entry sits on.
		/// </summary>
		public int Rank { get; }

		/// <summary>
		/// Returns the job level reached on this rank.
		/// </summary>
		public int Level { get; }

		/// <summary>
		/// Returns the EXP collected on this rank.
		/// </summary>
		public long TotalExp { get; }

		/// <summary>
		/// Returns the skill points still unspent on this rank.
		/// </summary>
		public int SkillPoints { get; }

		/// <summary>
		/// Returns the total EXP this rank's level was reached at.
		/// </summary>
		public long LevelStartExp { get; }

		/// <summary>
		/// Returns the total EXP this rank's level ends at.
		/// </summary>
		public long LevelEndExp { get; }

		/// <summary>
		/// Creates new entry.
		/// </summary>
		/// <param name="job"></param>
		/// <param name="rank"></param>
		/// <param name="level"></param>
		/// <param name="totalExp"></param>
		/// <param name="skillPoints"></param>
		public JobHistoryEntry(Job job, int rank, int level, long totalExp, int skillPoints)
		{
			this.Job = job;
			this.Rank = rank;
			this.Level = level;
			this.TotalExp = totalExp;
			this.SkillPoints = skillPoints;
			this.LevelStartExp = level > 1 ? ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(rank, level - 1) : 0;
			this.LevelEndExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(rank, level);
		}
	}

	/// <summary>
	/// Represents one of a character's jobs.
	/// </summary>
	public class Job
	{
		/// <summary>
		/// The owner of the job.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Returns this job's id.
		/// </summary>
		public JobId Id { get; }

		/// <summary>
		/// Gets or sets the circle this job is on.
		/// </summary>
		public JobCircle Circle
		{
			get { return _circle; }
			set
			{
				_circle = value;
				this.Character?.Jobs?.InvalidateRankCache();
			}
		}
		private JobCircle _circle;

		public DateTime AdvancementDate { get; set; }

		/// <summary>
		/// Gets or sets skill points available for this job.
		/// </summary>
		public int SkillPoints
		{
			get { return _skillPoints; }
			set { _skillPoints = Math2.Clamp(0, short.MaxValue, value); }
		}
		private int _skillPoints;

		/// <summary>
		/// Reference to the job's data from the job database.
		/// </summary>
		public JobData Data { get; }

		/// <summary>
		/// Gets or sets the total EXP collected for this job.
		/// </summary>
		/// <remarks>
		/// Every job has its own EXP count, which the client uses in
		/// combination with the job and its rank to determine the level.
		/// There doesn't seem to be a way to change the max job EXP from
		/// the server, as it is with the base EXP.
		/// </remarks>
		private long _totalExp;
		private int _cachedLevel = -1;
		private long _cachedLevelExp = -1;
		private int _cachedLevelRank = -1;

		public long TotalExp
		{
			get => _totalExp;
			set
			{
				if (_totalExp != value)
				{
					_totalExp = value;
					_cachedLevel = -1;
				}
			}
		}

		/// <summary>
		/// Returns the total maximum EXP that can be collected on this job.
		/// </summary>
		/// <remarks>
		/// The last level of a rank is a cap to advance out of rather than a
		/// band to fill, and its EXP is the step onto the next rank. Holding
		/// one level short of it keeps the job off the total the next rank
		/// begins on, which the client reads as a level of its own.
		/// </remarks>
		public long TotalMaxExp
		{
			get
			{
				var rank = this.Character.Jobs.GetJobRank(this.Id);
				return ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(rank, Math.Max(1, this.MaxLevel - 1));
			}
		}

		/// <summary>
		/// Returns the EXP collected on the job's current level.
		/// </summary>
		public long Exp
		{
			get
			{
				if (this.Level == 1 || this.Level == this.MaxLevel)
					return this.TotalExp;

				return this.TotalExp - ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(this.Character.Jobs.GetJobRank(this.Id), Math.Max(1, this.Level - 1));
			}
		}

		/// <summary>
		/// Returns the EXP necessary for leveling up at the current
		/// level.
		/// </summary>
		public long MaxExp
		{
			get
			{
				if (this.Level == this.MaxLevel)
					return this.TotalMaxExp;

				var curLevelExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(this.Character.Jobs.GetJobRank(this.Id), Math.Min(this.MaxLevel, this.Level));

				if (this.Level == 1)
					return curLevelExp;

				var lastLevelExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(this.Character.Jobs.GetJobRank(this.Id), Math.Max(1, this.Level - 1));

				return curLevelExp - lastLevelExp;
			}
		}

		/// <summary>
		/// Returns the level reached on this job based on the
		/// job's individual rank and total EXP.
		/// </summary>
		public int Level
		{
			get
			{
				if (this.Rank == 0)
					throw new InvalidOperationException("The job needs to be added to a character before the level can be determined.");

				var totalExp = this.TotalExp;
				var rank = this.Character.Jobs.GetJobRank(this.Id);

				if (_cachedLevel > 0 && _cachedLevelExp == totalExp && _cachedLevelRank == rank)
					return _cachedLevel;

				var max = this.MaxLevel;

				// Search for the first level which's requirement we can't
				// fulfill, as that will be the level we're on.
				for (var i = 1; i < max; ++i)
				{
					var needed = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(rank, i);
					if (totalExp < needed)
					{
						_cachedLevel = i;
						_cachedLevelExp = totalExp;
						_cachedLevelRank = rank;
						return i;
					}
				}

				// Found none? It's the max then.
				_cachedLevel = max;
				_cachedLevelExp = totalExp;
				_cachedLevelRank = rank;
				return max;
			}
		}

		/// <summary>
		/// Returns this job's level under the flat job model, folding the
		/// circle back into the level. Skill tree unlock levels are still
		/// banded 1/16/31, so they're checked against this instead of the
		/// raw level, which only ever runs 1~15 per circle.
		/// </summary>
		public int EffectiveLevel
		{
			get
			{
				if (!ZoneServer.Instance.Conf.World.ClassCircleSystem)
					return this.Level;

				return JobCircleHelper.GetEffectiveJobLevel(this.Circle, this.Level, this.MaxLevel);
			}
		}

		/// <summary>
		/// Returns this job's EXP as the client has to receive it to show
		/// the right level and bar.
		/// </summary>
		/// <remarks>
		/// The client works the level out from the EXP against its own copy
		/// of the table, on a rank that counts the character's jobs rather
		/// than the ranks they've spent. So the job's level and progress are
		/// mapped onto the rank the client will read, which lands it on the
		/// same level the server holds.
		/// </remarks>
		public long DisplayExp
		{
			get
			{
				var expDb = ZoneServer.Instance.Data.ExpDb;
				var clientRank = Math.Max(1, this.Character.Jobs.Count);
				var maxLevel = this.MaxLevel;
				var level = Math2.Clamp(1, maxLevel, this.Level);

				// The last level has to stay under the row above it, or the
				// client counts a level past the one it's on.
				if (level >= maxLevel)
					return expDb.GetNextTotalJobExp(clientRank, maxLevel) - 1;

				var levelStart = level > 1 ? expDb.GetNextTotalJobExp(clientRank, level - 1) : 0;
				var levelEnd = expDb.GetNextTotalJobExp(clientRank, level);

				var maxExp = this.MaxExp;
				var progress = maxExp > 0 ? (double)this.Exp / maxExp : 0;
				progress = Math.Max(0, Math.Min(1, progress));

				var into = (long)(progress * (levelEnd - levelStart));

				return levelStart + Math.Min(levelEnd - levelStart - 1, into);
			}
		}

		/// <summary>
		/// Returns the highest effective level currently reachable on this
		/// job, which is the cap of its current circle. Skills banded above
		/// it aren't available yet.
		/// </summary>
		public int EffectiveMaxLevel
		{
			get
			{
				if (!ZoneServer.Instance.Conf.World.ClassCircleSystem)
					return this.MaxLevel;

				return JobCircleHelper.GetEffectiveJobLevel(this.Circle, this.MaxLevel, this.MaxLevel);
			}
		}

		/// <summary>
		/// Returns the max level the given skill tree entry can be raised
		/// to on this job, which its circle caps under the class circle
		/// system.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int GetSkillMaxLevel(SkillTreeData data)
		{
			if (!ZoneServer.Instance.Conf.World.ClassCircleSystem)
				return data.MaxLevel;

			return JobCircleHelper.GetSkillMaxLevel(this.Circle, data.UnlockLevel, data.MaxLevel, this.MaxLevel);
		}

		/// <summary>
		/// Returns the max level for this job.
		/// </summary>
		public int MaxLevel
		{
			get
			{
				// Use job-specific rank (position in job progression) to
				// determine max level. Base job (rank 1) caps at the base
				// job level, advanced jobs (rank 2+) at the advanced one.
				// Under the class circle system that advanced cap applies
				// per circle, since each circle is its own rank.
				if (Versions.Client <= KnownVersions.PreReBuild)
					return 15;

				var rank = this.Character.Jobs.GetJobRank(this.Id);
				if (rank > 1)
					return ZoneServer.Instance.Conf.World.MaxAdvanceJobLevel;

				return ZoneServer.Instance.Conf.World.MaxBaseJobLevel;
			}
		}

		/// <summary>
		/// Gets or sets the date the character chose this job.
		/// </summary>
		public DateTime SelectionDate { get; set; } = DateTime.Now;

		/// <summary>
		/// Gets or sets the rank the job was added on.
		/// </summary>
		/// <remarks>
		/// This value is used to determine the job's (max) level and is
		/// assigned automatically based on the order the jobs were added.
		/// </remarks>
		public int Rank { get; set; }

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="jobId"></param>
		/// <param name="circle"></param>
		/// <param name="skillPoints"></param>
		public Job(Character character, JobId jobId, JobCircle circle = JobCircle.First, int skillPoints = 0)
			: this(character, jobId, 0, circle, skillPoints)
		{
		}

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="jobId"></param>
		/// <param name="totalExp"></param>
		/// <param name="circle"></param>
		/// <param name="skillPoints"></param>
		public Job(Character character, JobId jobId, long totalExp, JobCircle circle = JobCircle.First, int skillPoints = 0)
		{
			this.Character = character;
			this.Id = jobId;
			this.Circle = circle;
			this.SkillPoints = skillPoints;
			this.TotalExp = totalExp;
			this.AdvancementDate = DateTime.Now;
			this.Data = ZoneServer.Instance.Data.JobDb.Find(jobId) ?? throw new ArgumentException($"Unknown job '{jobId}'.");
		}

		/// <summary>
		/// Modifies job's skill points updates the client.
		/// </summary>
		/// <param name="modifier"></param>
		/// <returns></returns>
		public void ModifySkillPoints(int modifier)
			=> this.SetSkillPoints(this.SkillPoints + modifier);

		/// <summary>
		/// Sets job's skill points updates the client.
		/// </summary>
		/// <param name="skillPoints"></param>
		/// <returns></returns>
		public void SetSkillPoints(int skillPoints)
		{
			this.SkillPoints = skillPoints;
			Send.ZC_JOB_PTS(this.Character, this);
		}
	}
}
