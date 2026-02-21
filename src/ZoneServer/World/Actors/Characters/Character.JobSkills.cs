// ===================================================================
// CharacterJobSkills.cs - Job, skill, and ability management
// ===================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		#region Job & Skill Management
		/// <summary>
		/// Increases character's job level by the given amount. Returns the amount of levels actually gained.
		/// </summary>
		public int JobLevelUp(int amount)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be lower than 1.");

			if (this.Job.Level == this.Job.MaxLevel)
				return 0;

			if (this.Job.Level + amount > this.Job.MaxLevel)
				amount = this.Job.MaxLevel - this.Job.Level;

			var prevLevel = this.Job.Level;
			var prevExp = this.Job.TotalExp;

			this.Job.TotalExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(this.Jobs.GetCurrentRank(), prevLevel + amount - 1);

			var expGained = (this.Job.TotalExp - prevExp);
			var levelsGained = (this.Job.Level - prevLevel);

			Send.ZC_JOB_EXP_UP(this, expGained);

			if (levelsGained > 0)
				this.FinishJobLevelChange(levelsGained);

			return levelsGained;
		}

		/// <summary>
		/// Modifies skill points for job, heals character (if leveling up), and notifies client about the job level change.
		/// </summary>
		private void FinishJobLevelChange(int amount)
		{
			if (amount == 0)
				return;

			this.Jobs.ModifySkillPoints(this.JobId, amount);

			if (amount > 0)
				this.FullHeal();

			Send.ZC_OBJECT_PROPERTY(this);
			this.AddonMessage("NOTICE_Dm_levelup_skill", "!@#$Auto_KeulLeSeu_LeBeli_SangSeungHayeossSeupNiDa#@!", 3);
			this.PlayEffect("F_pc_joblevel_up", 3);
			Send.ZC_SKILL_LIST(this);
		}

		/// <summary>
		/// Decreases character's job level by the given amount. Returns the amount of levels actually lost.
		/// </summary>
		public int JobLevelDown(int amount)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be lower than 1.");

			if (this.Job.Level == 1)
				return 0;

			if (this.Job.Level - amount < 1)
				amount = this.Job.Level - 1;

			var prevLevel = this.Job.Level;

			var newLevel = prevLevel - amount;
			if (newLevel < 1)
				newLevel = 1;

			this.Job.TotalExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(this.Jobs.GetCurrentRank(), newLevel - 1);

			var levelsLost = prevLevel - this.Job.Level;

			if (levelsLost > 0)
				this.FinishJobLevelChange(-levelsLost);

			return levelsLost;
		}

		/// <summary>
		/// Modifies the character's ability points by the given amount and updates the client.
		/// </summary>
		public int ModifyAbilityPoints(int amount)
		{
			var abilityPoints = int.Parse(this.Properties.GetString(PropertyName.AbilityPoint));
			abilityPoints += amount;
			this.Properties.SetString(PropertyName.AbilityPoint, abilityPoints.ToString());

			Send.ZC_CUSTOM_COMMANDER_INFO(this, CommanderInfoType.AbilityPoints, abilityPoints);
			this.AddonMessage(Shared.Game.Const.AddonMessage.UPDATE_ABILITY_POINT);

			return abilityPoints;
		}

		/// <summary>
		/// Resets the character's skills, returning all spent points.
		/// </summary>
		public void ResetSkills()
		{
			foreach (var skill in this.Skills.GetList())
			{
				var skillTree = ZoneServer.Instance.Data.SkillTreeDb.Find(skillTree => skillTree.SkillId == skill.Id);
				if (skillTree == null)
					continue;

				if (skill.LevelByDB > 0)
					this.Skills.Remove(skill.Id);
			}

			foreach (var job in this.Jobs.GetList())
			{
				job.SetSkillPoints(job.Level);
			}
		}

		/// <summary>
		/// Resets the character's skills, returning all spent points.
		/// </summary>
		public void ResetSkills(JobId jobId)
		{
			if (!this.Jobs.TryGet(jobId, out var job))
				return;

			foreach (var skill in this.Skills.GetList())
			{
				var skillTree = ZoneServer.Instance.Data.SkillTreeDb.Find(skillTree => skillTree.JobId == jobId && skillTree.SkillId == skill.Id);
				if (skillTree == null)
					continue;

				if (skill.LevelByDB > 0)
					this.Skills.Remove(skill.Id);
			}

			job.SetSkillPoints(job.Level);
		}

		/// <summary>
		/// Resets the character's learned abilities, removing all job-specific
		/// abilities from the ability tree and refunding spent ability points.
		/// </summary>
		public void ResetAbilities()
		{
			var totalRefund = 0;
			var jobIds = this.Jobs.GetList().Select(j => j.Id).ToHashSet();

			foreach (var ability in this.Abilities.GetList())
			{
				AbilityTreeData treeData = null;
				foreach (var jobId in jobIds)
				{
					treeData = ZoneServer.Instance.Data.AbilityTreeDb.Find(jobId, ability.Id);
					if (treeData != null)
						break;
				}

				if (treeData == null)
					continue;

				var abilityData = ZoneServer.Instance.Data.AbilityDb.Find(ability.Id);
				if (abilityData == null)
					continue;

				if (treeData.HasPriceTimeScript)
				{
					if (ScriptableFunctions.AbilityPrice.TryGet(treeData.PriceTimeScript, out var priceTimeFunc))
					{
						for (var i = 1; i <= ability.Level; i++)
						{
							priceTimeFunc(this, abilityData, i, treeData.MaxLevel, out var price, out var time);
							totalRefund += price;
						}
					}
				}

				this.Abilities.Remove(ability.Id);
			}

			if (totalRefund > 0)
				this.ModifyAbilityPoints(totalRefund);
		}

		/// <summary>
		/// Sets and returns the currently correct stance, based on equipment. Does not update client.
		/// </summary>
		public int UpdateStance()
		{
			var jobId = this.JobId;
			var riding = false;
			var rightHand = this.Inventory.GetItem(EquipSlot.RightHand).Data.EquipType1;
			var leftHand = this.Inventory.GetItem(EquipSlot.LeftHand).Data.EquipType1;

			if (leftHand == EquipType.Trinket)
				leftHand = EquipType.None;

			this.Stance = ZoneServer.Instance.Data.StanceConditionDb.FindStanceId(jobId, riding, rightHand, leftHand);
			return this.Stance;
		}

		/// <summary>
		/// Changes the character's job with default circle and skill points for normal advancement.
		/// </summary>
		public void ChangeJob(JobId jobId)
		{
			this.ChangeJob(jobId, JobCircle.First, skillPoints: 1, playEffect: true);
		}

		/// <summary>
		/// Changes the character's job with specified circle and skill points.
		/// </summary>
		public void ChangeJob(JobId jobId, JobCircle circle, int skillPoints, bool playEffect = true)
		{
			// Store current levels of all existing jobs before adding the new one,
			// because adding a job changes the rank which affects how TotalExp
			// translates to Level (Level is calculated from TotalExp + rank)
			var existingJobLevels = new Dictionary<JobId, int>();
			foreach (var existingJob in this.Jobs.GetList())
				existingJobLevels[existingJob.Id] = existingJob.Level;

			var newJob = new Job(this, jobId, circle, skillPoints);
			newJob.AdvancementDate = DateTime.Now;

			if (playEffect)
				this.PlayEffect("F_pc_class_change");

			this.JobId = jobId;
			this.Jobs.Add(newJob);

			// After adding the job, the rank has increased. Recalculate TotalExp
			// for all existing jobs to maintain their levels at the new rank.
			var newRank = this.Jobs.GetCurrentRank();
			foreach (var existingJob in this.Jobs.GetList())
			{
				if (existingJob.Id == jobId)
					continue;

				var targetLevel = existingJobLevels[existingJob.Id];
				existingJob.TotalExp = ZoneServer.Instance.Data.ExpDb.GetNextTotalJobExp(newRank, targetLevel);
			}

			ZoneServer.Instance.ServerEvents.PlayerAdvancedJob.Raise(new PlayerEventArgs(this));

			this.Variables.Perm.Remove("JobAdvancement");
		}

		/// <summary>
		/// Updates character skills/abilities based on class.
		/// </summary>
		private void UpdateCharacter(Character character)
		{
			if (character.JobClass == JobClass.Cleric)
			{
				if (Feature.IsEnabled("GuardingClerics"))
					LearnSkill(character, SkillId.Warrior_Guard);
			}
		}
		#endregion

		#region Job Initializers
		/// <summary>
		/// Gives the character the skills and abilities shared by all
		/// job classes.
		/// </summary>
		protected static void InitCommon(Character character)
		{
			LearnSkill(character, SkillId.Default);
			LearnSkill(character, SkillId.Common_shovel);
			LearnSkill(character, SkillId.Common_otlflag);
			LearnSkill(character, SkillId.Common_dumbbell);
			LearnSkill(character, SkillId.Common_vuvuzela);
			LearnSkill(character, SkillId.Common_snowspray);
			LearnSkill(character, SkillId.Common_balloonpipe);

			LearnAbility(character, AbilityId.Cloth);
			LearnAbility(character, AbilityId.Leather);
			LearnAbility(character, AbilityId.Iron);
			LearnAbility(character, AbilityId.SwapWeapon);
		}

		/// <summary>
		/// Gives the character the default skills and abilities for the
		/// Swordsman class tree.
		/// </summary>
		protected static void InitSwordsman(Character character)
		{
			LearnSkill(character, SkillId.Normal_Attack);
			LearnSkill(character, SkillId.Normal_Attack_TH);
			LearnSkill(character, SkillId.Warrior_Guard);
			LearnSkill(character, SkillId.Pistol_Attack);
			LearnSkill(character, SkillId.Common_DaggerAries);

			LearnAbility(character, AbilityId.Sword);
			LearnAbility(character, AbilityId.Staff);
			LearnAbility(character, AbilityId.Mace);
		}

		/// <summary>
		/// Gives the character the default skills and abilities for the
		/// Wizard class tree.
		/// </summary>
		protected static void InitWizard(Character character)
		{
			LearnSkill(character, SkillId.Magic_Attack);
			LearnSkill(character, SkillId.Magic_Attack_TH);
			LearnSkill(character, SkillId.Common_DaggerAries);
			LearnSkill(character, SkillId.Common_StaffAttack);

			LearnAbility(character, AbilityId.Sword);
			LearnAbility(character, AbilityId.Staff);
			LearnAbility(character, AbilityId.Mace);
			LearnAbility(character, AbilityId.THStaff);
		}

		/// <summary>
		/// Gives the character the default skills and abilities for the
		/// Archer class tree.
		/// </summary>
		protected static void InitArcher(Character character)
		{
			LearnSkill(character, SkillId.Bow_Attack);
			LearnSkill(character, SkillId.CrossBow_Attack);
			LearnSkill(character, SkillId.Common_DaggerAries);
			LearnSkill(character, SkillId.Warrior_Guard);
			LearnSkill(character, SkillId.Pistol_Attack);
			LearnSkill(character, SkillId.Musket_Attack);
			LearnSkill(character, SkillId.Sword_Attack);
			LearnSkill(character, SkillId.Cannon_Normal_Attack);

			LearnAbility(character, AbilityId.THBow);
			LearnAbility(character, AbilityId.Bow);
		}

		/// <summary>
		/// Gives the character the default skills and abilities for the
		/// Cleric class tree.
		/// </summary>
		protected static void InitCleric(Character character)
		{
			LearnSkill(character, SkillId.Hammer_Attack);
			LearnSkill(character, SkillId.Hammer_Attack_TH);
			LearnSkill(character, SkillId.Common_DaggerAries);

			LearnAbility(character, AbilityId.Sword);
			LearnAbility(character, AbilityId.Staff);
			LearnAbility(character, AbilityId.Mace);
			LearnAbility(character, AbilityId.THMace);
			LearnAbility(character, AbilityId.Cleric36);
		}

		/// <summary>
		/// Gives the character the default skills and abilities for the
		/// Scout class tree.
		/// </summary>
		protected static void InitScout(Character character)
		{
			LearnSkill(character, SkillId.Normal_Attack);
			LearnSkill(character, SkillId.Normal_Attack_TH);
			LearnSkill(character, SkillId.Warrior_Guard);
			LearnSkill(character, SkillId.War_JustFrameAttack);
			LearnSkill(character, SkillId.War_JustFrameDagger);
			LearnSkill(character, SkillId.War_JustFramePistol);
			LearnSkill(character, SkillId.Pistol_Attack);
			LearnSkill(character, SkillId.Common_DaggerAries);

			LearnAbility(character, AbilityId.Sword);
		}

		/// <summary>
		/// Adds the skill to the character silently if they don't already
		/// have it and the skill exists in the database.
		/// </summary>
		protected static void LearnSkill(Character character, SkillId skillId)
		{
			if (character.Skills.Has(skillId) || !ZoneServer.Instance.Data.SkillDb.TryFind(skillId, out _))
				return;

			var skill = new Skill(character, skillId, 1);
			character.Skills.AddSilent(skill);
		}

		/// <summary>
		/// Adds the ability to the character silently if they don't already
		/// have it and the ability exists in the database.
		/// </summary>
		protected static void LearnAbility(Character character, AbilityId abilityId)
		{
			if (character.Abilities.Has(abilityId) || !ZoneServer.Instance.Data.AbilityDb.TryFind(abilityId, out _))
				return;

			var ability = new Ability(abilityId, 1);
			character.Abilities.AddSilent(ability);
		}
		#endregion
	}
}
