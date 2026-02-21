using System;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters
{
	// ===================================================================
	// CharacterStats.cs - Character statistics and progression
	// ===================================================================
	public partial class Character
	{
		#region Stats Properties
		/// <summary>
		/// Gets or sets the character's current job id.
		/// </summary>
		public JobId JobId { get; set; }

		/// <summary>
		/// Returns the class of the character's current job.
		/// </summary>
		public JobClass JobClass => this.JobId.ToClass();

		/// <summary>
		/// Returns the character's current job.
		/// </summary>
		public Job Job => this.Jobs.Get(this.JobId);

		/// <summary>
		/// Current experience points.
		/// </summary>
		public long Exp { get; set; }

		/// <summary>
		/// Current maximum experience points.
		/// </summary>
		public long MaxExp { get; set; }

		/// <summary>
		/// Total number of accumulated experience points.
		/// </summary>
		public long TotalExp { get; set; }

		/// <summary>
		/// Returns the character's current job level.
		/// </summary>
		public int JobLevel
		{
			get
			{
				var job = this.Jobs.Get(this.JobId);
				return job.Level;
			}
		}

		/// <summary>
		/// Returns the character's current level.
		/// </summary>
		public int Level => (int)this.Properties.GetFloat(PropertyName.Lv);

		/// <summary>
		/// Returns the character's current MSPD.
		/// </summary>
		public float MSPD => this.Properties.GetFloat(PropertyName.MSPD);

		/// <summary>
		/// Returns the character's current Moving Shot.
		/// </summary>
		public float MovingShot => this.Properties.GetFloat(PropertyName.MovingShot);

		/// <summary>
		/// Returns the character's current HP.
		/// </summary>
		public int Hp => (int)this.Properties.GetFloat(PropertyName.HP);

		/// <summary>
		/// Returns the character's max HP.
		/// </summary>
		public int MaxHp => (int)this.Properties.GetFloat(PropertyName.MHP);

		/// <summary>
		/// Returns the character's current SP.
		/// </summary>
		public int Sp => (int)this.Properties.GetFloat(PropertyName.SP);

		/// <summary>
		/// Returns the character's max SP.
		/// </summary>
		public int MaxSp => (int)this.Properties.GetFloat(PropertyName.MSP);

		/// <summary>
		/// Returns the character's current stamina.
		/// </summary>
		public int Stamina => this.Properties.Stamina;

		/// <summary>
		/// Returns the character's max stamina.
		/// </summary>
		public int MaxStamina => this.Properties.MaxStamina;

		/// <summary>
		/// Returns the character's current shield.
		/// </summary>
		public int Shield => (int)this.Properties.GetFloat(PropertyName.Shield);

		/// <summary>
		/// Returns the character's max shield.
		/// </summary>
		public int MaxShield => (int)this.Properties.GetFloat(PropertyName.MShield);

		/// <summary>
		/// Holds the order of successive changes in character HP.
		/// </summary>
		public int HpChangeCounter { get; private set; }
		#endregion

		#region Status Properties
		/// <summary>
		/// Returns true if the character has run out of HP and died.
		/// </summary>
		public bool IsDead => (this.Hp == 0);

		/// <summary>
		/// Check if character is resurrecting or not.
		/// </summary>
		public bool IsResurrecting { get; private set; }

		/// <summary>
		/// Returns true if the character has a party.
		/// </summary>
		public bool HasParty => this.Connection?.Party != null;

		/// <summary>
		/// Returns true if the character has a guild.
		/// </summary>
		// Removed: Guild type deleted during Laima merge
		public bool HasGuild => false;

		/// <summary>
		/// Has Companion(s)
		/// </summary>
		public bool HasCompanions => this.Companions.HasCompanions;

		/// <summary>
		/// Active Companion
		/// </summary>
		public Companion ActiveCompanion => this.Companions.ActiveCompanion;

		/// <summary>
		/// Character's current duel
		/// </summary>
		public bool IsDueling => ZoneServer.Instance.World.Duels.IsInDuel(this);

		/// <summary>
		/// Check if character is trading.
		/// </summary>
		public bool IsTrading => ZoneServer.Instance.World.Trades.IsTrading(this.ObjectId);

		/// <summary>
		/// Gets or sets whether the character is participating in a
		/// colony war on the current map.
		/// </summary>
		public bool IsJoinColonyWarMap { get; set; }

		/// <summary>
		/// Gets or sets whether the character is in an instanced dungeon.
		/// </summary>
		public bool IsIndun { get; set; }

		/// <summary>
		/// Gets or sets whether the character is in a mission instance.
		/// </summary>
		public bool IsMissionInst { get; set; }

		/// <summary>
		/// Returns the character's effective size (always PC for players).
		/// </summary>
		public SizeType EffectiveSize => SizeType.PC;

		/// <summary>
		/// Returns the character's monster rank (always Normal for players).
		/// </summary>
		public MonsterRank Rank => MonsterRank.Normal;
		#endregion

		#region Stats Management
		/// <summary>
		/// Gives character its initial properties if they're missing.
		/// </summary>
		public virtual void InitProperties()
		{
			if (this.Job == null)
				throw new InvalidOperationException("Character's jobs need to be loaded before initializing the properties.");

			if (!this.Variables.Perm.Has("Melia.PropertiesInitialized"))
			{
				this.Exp = 0;
				this.TotalExp = 0;
				this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(1);

				this.Properties.Invalidate(PropertyName.MHP, PropertyName.MSP, PropertyName.MaxSta);

				this.Properties.SetFloat(PropertyName.HP, this.Properties.GetFloat(PropertyName.MHP));
				this.Properties.SetFloat(PropertyName.SP, this.Properties.GetFloat(PropertyName.MSP));
				this.Properties.Stamina = (int)this.Properties.GetFloat(PropertyName.MaxSta);

				this.Variables.Perm.SetBool("Melia.PropertiesInitialized", true);
			}
			else
			{
				var maxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(this.Level);
				if (this.MaxExp != maxExp)
					this.MaxExp = maxExp;
			}

			this.Properties.InvalidateAll();
			this.Properties.InitAutoUpdates();
		}

		/// <summary>
		/// Grants exp to character and handles level ups.
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="jobExp"></param>
		/// <param name="monster"></param>
		public virtual void GiveExp(long exp, long jobExp, IMonster monster)
		{
			// Decide whether to keep this as is or delay exp till resurrection or something.
			if (this.IsDead)
				return;

			// Base EXP
			this.Exp += exp;
			this.TotalExp += exp;

			if (monster != null)
				Send.ZC_EXP_UP_BY_MONSTER(this, exp, jobExp, monster);

			Send.ZC_EXP_UP(this, exp, jobExp); // Not always sent? Might be quest related?

			var level = this.Level;
			var levelUps = 0;
			var maxExp = this.MaxExp;
			var maxLevel = ZoneServer.Instance.Conf.World.MaxLevel;
			//var maxLevel = ZoneServer.Instance.Data.ExpDb.GetMaxLevel();

			// Consume EXP as many times as possible to reach new levels
			while (this.Exp >= maxExp && level < maxLevel)
			{
				this.Exp -= maxExp;

				level++;
				levelUps++;
				maxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp(level);
			}

			// Execute level up only once to avoid client lag on multiple
			// level ups. Leveling up a thousand times in a loop is not
			// fun for the client =D"
			if (levelUps > 0)
				this.LevelUp(levelUps);

			// Job EXP
			// Increase the total EXP and check whether the job level,
			// which is calculcated from that value, has changed.
			var jobLevel = this.JobLevel;
			var rank = this.Jobs.GetCurrentRank();
			var job = this.Job;

			// Limit EXP to the total max, otherwise the client will
			// display level 1 with 0%.
			job.TotalExp = Math.Min(job.TotalMaxExp, (job.TotalExp + jobExp));

			var newJobLevel = this.JobLevel;
			var jobLevelsGained = (newJobLevel - jobLevel);

			Send.ZC_JOB_EXP_UP(this, jobExp);

			if (jobLevelsGained > 0)
				this.FinishJobLevelChange(jobLevelsGained);

			if (this.HasCompanions)
			{
				// Pretty sure companions get reduced exp, but don't remember the exact value.
				exp = (long)(exp * .25f);
				foreach (var companion in this.Companions.GetList())
					companion.GiveExp(exp, monster);
			}
		}

		/// <summary>
		/// Increases character's level by the given amount.
		/// </summary>
		public void LevelUp(int amount = 1)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be lower than 1.");

			this.ShowHelp("TUTO_STATPOINT");

			var statsPerLevel = ZoneServer.Instance.Conf.World.StatsPerLevel;
			var extraStatsLevels = ZoneServer.Instance.Conf.World.ExtraStatsLevels;

			var level = this.Level;
			var additionalStats = 0;
			for (var i = 0; i < amount; i++)
			{
				level++;
				var extraStats = 0;
				for (var j = 0; j < extraStatsLevels.Count; j++)
				{
					if (level >= extraStatsLevels[j])
					{
						extraStats = j + 1;
					}
					else
					{
						break;
					}
				}
				additionalStats += extraStats;
			}

			var newLevel = this.Properties.Modify(PropertyName.Lv, amount);
			if (newLevel >= ZoneServer.Instance.Conf.World.MaxLevel && !this.Variables.Perm.Has("Melia.MaxLevel.AchievedTime"))
			{
				this.Variables.Perm.Set("Melia.MaxLevel.AchievedTime", DateTime.UtcNow.Ticks.ToString());
				Log.Info("Max Level Reached: {0} {1} {2} ", this.DbId, this.Name, this.TeamName);
				Send.ZC_TEXT(NoticeTextType.Gold, $"Congratulations to {this.Name} for reaching max level.");
			}

			this.Properties.Modify(PropertyName.StatByLevel, (amount * statsPerLevel) + additionalStats);
			this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp((int)newLevel);
			this.FullHeal();

			Send.ZC_MAX_EXP_CHANGED(this, 0);
			Send.ZC_PC_LEVELUP(this);
			Send.ZC_OBJECT_PROPERTY(this);

			this.AddonMessage("NOTICE_Dm_levelup_base", "!@#$Auto_KaeLigTeo_LeBeli_SangSeungHayeossSeupNiDa#@!", 3);
			this.PlayEffect("F_pc_level_up", 3);
			this.Connection.Party?.UpdateMemberInfo(this);
			// this.Connection.Guild?.UpdateMemberInfo(this); // Removed: Guild type deleted
		}

		/// <summary>
		/// Decreases character's level by the given amount.
		/// </summary>
		public void LevelDown(int amount = 1)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be lower than 1.");

			var statsPerLevel = ZoneServer.Instance.Conf.World.StatsPerLevel;
			var extraStatsLevels = ZoneServer.Instance.Conf.World.ExtraStatsLevels;

			var level = this.Level;
			var statsToRemove = 0;
			for (var i = 0; i < amount; i++)
			{
				var extraStats = 0;
				for (var j = 0; j < extraStatsLevels.Count; j++)
				{
					if (level >= extraStatsLevels[j])
					{
						extraStats = j + 1;
					}
					else
					{
						break;
					}
				}
				statsToRemove += extraStats;
				level--;
			}

			var newLevel = this.Properties.Modify(PropertyName.Lv, -amount);
			this.Properties.Modify(PropertyName.StatByLevel, -((amount * statsPerLevel) + statsToRemove));
			this.MaxExp = ZoneServer.Instance.Data.ExpDb.GetNextExp((int)newLevel);

			Send.ZC_MAX_EXP_CHANGED(this, 0);
			Send.ZC_PC_LEVELUP(this);
			Send.ZC_OBJECT_PROPERTY(this);

			this.AddonMessage("NOTICE_Dm_levelup_base", "!@#$Auto_KaeLigTeo_LeBeli_SangSeungHayeossSeupNiDa#@!", 3);
			this.PlayEffect("F_pc_level_up", 3);

			this.Connection.Party?.UpdateMemberInfo(this);
			// this.Connection.Guild?.UpdateMemberInfo(this); // Removed: Guild type deleted
		}

		/// <summary>
		/// Adds amount to character's stat points and updates the client.
		/// </summary>
		public void AddStatPoints(int amount)
		{
			if (amount < 1)
				throw new ArgumentException("Amount can't be negative.");

			this.Properties.Modify(PropertyName.StatByBonus, amount);
			Send.ZC_OBJECT_PROPERTY(this, PropertyName.StatByBonus);
		}

		/// <summary>
		/// Resets the character's stats.
		/// </summary>
		public void ResetStats()
		{
			this.Properties.SetFloat(PropertyName.UsedStat, 0);
			this.Properties.SetFloat(PropertyName.STR_STAT, 0);
			this.Properties.SetFloat(PropertyName.CON_STAT, 0);
			this.Properties.SetFloat(PropertyName.INT_STAT, 0);
			this.Properties.SetFloat(PropertyName.MNA_STAT, 0);
			this.Properties.SetFloat(PropertyName.DEX_STAT, 0);

			this.Properties.InvalidateAll();
			Send.ZC_OBJECT_PROPERTY(this);
			this.AddonMessage(Shared.Game.Const.AddonMessage.RESET_STAT_UP);
		}
		#endregion
	}
}
