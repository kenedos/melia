using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	public class BaseSkillComponent : CombatEntityComponent, IUpdateable
	{
		protected readonly Dictionary<SkillId, Skill> _skills = new();
		public SkillId CurrentSkill { get; private set; } = SkillId.None;
		public Skill CurrentSkillRef { get; private set; }
		public DateTime SkillUseTime { get; private set; }

		public BaseSkillComponent(ICombatEntity entity) : base(entity)
		{
		}

		/// <summary>
		/// Returns the amount of skills in the collection.
		/// </summary>
		public int Count { get { lock (_skills) return _skills.Count; } }

		/// <summary>
		/// Clears the current skills
		/// </summary>
		public void Clear()
		{
			lock (_skills)
				_skills.Clear();
		}

		/// <summary>
		/// Adds given without updating the client. Replaces existing
		/// skills.
		/// </summary>
		/// <param name="skill"></param>
		public void AddSilent(Skill skill)
		{
			lock (_skills)
				_skills[skill.Id] = skill;
		}

		/// <summary>
		/// Removes skill with given id, returns false if it
		/// didn't exist. Doesn't update the client.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public bool RemoveSilent(SkillId skillId)
		{
			lock (_skills)
				return _skills.Remove(skillId);
		}

		/// <summary>
		/// Returns skill with given id, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public Skill Get(SkillId skillId)
		{
			lock (_skills)
			{
				_skills.TryGetValue(skillId, out var result);
				return result;
			}
		}

		/// <summary>
		/// Returns skill with given class name, or null if it didn't
		/// exist.
		/// </summary>
		/// <param name="skillClassName"></param>
		/// <returns></returns>
		public Skill Get(string skillClassName)
		{
			lock (_skills)
				return _skills.Values.FirstOrDefault(a => a.Data.ClassName == skillClassName);
		}

		/// <summary>
		/// Returns skill with given id via out, returns false if the
		/// skill wasn't found.
		/// </summary>
		/// <param name="skillId"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		public bool TryGet(SkillId skillId, out Skill skill)
		{
			skill = this.Get(skillId);
			return skill != null;
		}

		/// <summary>
		/// Returns a list with all skills.
		/// </summary>
		/// <returns></returns>
		public Skill[] GetList()
		{
			lock (_skills)
				return _skills.Values.ToArray();
		}

		/// <summary>
		/// Returns a list of all skills that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Skill[] GetList(Func<Skill, bool> predicate)
		{
			lock (_skills)
				return _skills.Values.Where(predicate).ToArray();
		}

		/// <summary>
		/// Returns true if the skill exists.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public bool Has(SkillId skillId)
		{
			lock (_skills)
				return _skills.ContainsKey(skillId);
		}

		/// <summary>
		/// Updates the skills and their overheats.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (this.CurrentSkill != SkillId.None
				&& this.SkillUseTime < DateTime.Now)
			{
				this.CurrentSkill = SkillId.None;
				this.CurrentSkillRef = null;
			}
			lock (_skills)
			{
				foreach (var skill in _skills.Values)
					skill.Update(elapsed);
			}
		}

		/// <summary>
		/// Sets the current skill being used.
		/// </summary>
		/// <param name="skillId"></param>
		public void UseSkill(SkillId skillId)
		{
			if (this.TryGet(skillId, out var skill))
			{
				this.CurrentSkill = skillId;
				this.CurrentSkillRef = skill;
				this.SkillUseTime = DateTime.Now.Add(skill.Properties.ShootTime);
				skill.IncreaseOverheat();
			}
		}

		/// <summary>
		/// Sets the current skill reference directly for cancellation tracking.
		/// </summary>
		/// <param name="skill"></param>
		public void SetCurrentSkill(Skill skill)
		{
			this.CurrentSkill = skill.Id;
			this.CurrentSkillRef = skill;
			this.SkillUseTime = DateTime.Now.Add(skill.Properties.ShootTime);
		}

		/// <summary>
		/// Cancels the currently running skill if any, stopping any async
		/// tasks associated with it.
		/// </summary>
		public void CancelCurrentSkill()
		{
			if (this.CurrentSkillRef != null)
			{
				this.CurrentSkillRef.Cancel();
				this.CurrentSkillRef = null;
			}
			this.CurrentSkill = SkillId.None;
		}

		/// <summary>
		/// Cancels all skills that have running async tasks, not just
		/// the current one. Used during disconnect to stop lingering
		/// async loops (e.g. Dievdirbys statue buff loops).
		/// </summary>
		public void CancelAllRunningSkills()
		{
			lock (_skills)
			{
				foreach (var skill in _skills.Values)
				{
					if (skill.IsRunning)
						skill.Cancel();
				}
			}

			this.CurrentSkillRef = null;
			this.CurrentSkill = SkillId.None;
		}
	}
}
