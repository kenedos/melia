using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Character skills.
	/// </summary>
	public class SkillComponent : BaseSkillComponent
	{
		public Character Character { get; }

		/// <summary>
		/// Creates new instance for character.
		/// </summary>
		/// <param name="character"></param>
		public SkillComponent(Character character) : base(character)
		{
			this.Character = character;
		}

		/// <summary>
		/// Adds given skill and updates the client. Replaces existing
		/// skills.
		/// </summary>
		/// <param name="skill"></param>
		public void Add(Skill skill)
		{
			this.AddSilent(skill);

			Send.ZC_SKILL_ADD(this.Character, skill);
			Send.ZC_UPDATE_SKL_SPDRATE_LIST(this.Character, skill);
		}

		/// <summary>
		/// Removes skill with given id, returns false if it
		/// didn't exist. Updates the client on success.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public bool Remove(SkillId skillId)
		{
			if (!this.RemoveSilent(skillId))
				return false;

			Send.ZC_SKILL_REMOVE(this.Character, skillId);

			return true;
		}

		/// <summary>
		/// Returns current level of given skill, returns 0 if skill
		/// doesn't exist.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public int GetLevel(SkillId skillId)
		{
			var skill = this.Get(skillId);
			if (skill == null)
				return 0;

			return skill.Level;
		}

		/// <summary>
		/// Returns the max level the character can currently reach on the
		/// given skill.
		/// </summary>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public int GetMaxLevel(SkillId skillId)
		{
			// I don't like this, but I don't have a better idea for
			// handling it right now. A skill's max level could technically
			// be different for each job if a skill can be obtained on
			// more than one, so we have to go through all jobs to find the
			// "max max".

			var maxLevel = 0;

			foreach (var job in this.Character.Jobs.GetList())
			{
				var skillTreeDataList = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(job.Id, job.Level).Where(a => a.SkillId == skillId);
				foreach (var data in skillTreeDataList)
				{
					var jobsMaxLevel = data.MaxLevel;
					if (jobsMaxLevel > maxLevel)
						maxLevel = jobsMaxLevel;
				}
			}

			return maxLevel;
		}

		/// <summary>
		/// Invalidates all calculated properties, to update them when
		/// they're accessed next.
		/// </summary>
		public void InvalidateAll()
		{
			foreach (var skill in this.GetList())
			{
				var properties = skill.Properties.GetAll();

				foreach (var property in properties)
				{
					if (property is CFloatProperty calcProperty)
						calcProperty.Invalidate();
				}
				Send.ZC_OBJECT_PROPERTY(this.Character.Connection, skill);
			}
		}
	}
}
