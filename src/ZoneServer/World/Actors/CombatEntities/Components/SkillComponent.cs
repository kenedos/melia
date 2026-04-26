using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Skill manager, keeping references to the entity's skills.
	/// </summary>
	public class SkillComponent : BaseSkillComponent, IUpdateable
	{
		public Character Character { get; }

		/// <summary>
		/// Creates new instance for entity.
		/// </summary>
		/// <param name="entity"></param>
		public SkillComponent(ICombatEntity entity) : base(entity)
		{
			this.Character = entity as Character;
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

			if (this.Entity is Character character)
			{
				Send.ZC_SKILL_REMOVE(character, skillId);
				RemoveFromQuickSlots(character, skillId);
			}

			return true;
		}

		/// <summary>
		/// Scrubs any saved quickslot entries that reference the given skill
		/// id, so a skill removed server-side (e.g. an item-granted costume
		/// transform skill on unequip) doesn't linger as a ghost entry the
		/// next time the client requests the saved hotkeys.
		/// </summary>
		private static void RemoveFromQuickSlots(Character character, SkillId skillId)
		{
			var serialized = character.Variables.Perm.Get<string>("Melia.QuickSlotList", null);
			if (string.IsNullOrEmpty(serialized))
				return;

			var skillToken = "," + (int)skillId + ",";
			var skillPrefix = QuickSlotType.Skill.ToString();
			var entries = serialized.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
			var changed = false;

			for (var i = 0; i < entries.Length; i++)
			{
				var entry = entries[i];
				if (entry.StartsWith(skillPrefix + ",", StringComparison.Ordinal)
					&& entry.Contains(skillToken, StringComparison.Ordinal))
				{
					entries[i] = "None,0,0";
					changed = true;
				}
			}

			if (!changed)
				return;

			character.Variables.Perm.SetString("Melia.QuickSlotList", "#" + string.Join("#", entries) + "#");
			Send.ZC_QUICK_SLOT_LIST(character);
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
		/// Returns the max level the entity can currently reach on the
		/// given skill.
		/// </summary>
		/// <remarks>
		/// Due to skill max levels being tied to jobs and job levels,
		/// this method can only return a correct value if the entity has
		/// jobs. For others, such as monster, it will always return 0.
		/// </remarks>
		/// <param name="skillId"></param>
		/// <returns></returns>
		public int GetMaxLevel(SkillId skillId)
		{
			var maxLevel = 0;

			if (this.Entity.Components.TryGet<JobComponent>(out var jobComponent))
			{
				// I don't like this, but I don't have a better idea for
				// handling it right now. A skill's max level could
				// technically be different for each job if a skill can be
				// obtained on more than one, so we have to go through all
				// jobs to find the "max max".

				foreach (var job in jobComponent.GetList())
				{
					var skillTreeDataList = ZoneServer.Instance.Data.SkillTreeDb.FindSkills(job.Id, job.Level).Where(a => a.SkillId == skillId);
					foreach (var data in skillTreeDataList)
					{
						var jobsMaxLevel = data.MaxLevel;
						if (jobsMaxLevel > maxLevel)
							maxLevel = jobsMaxLevel;
					}
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
				var conn = this.Character?.Connection;
			if (conn != null)
				Send.ZC_OBJECT_PROPERTY(conn, skill);
			}
		}
	}
}
