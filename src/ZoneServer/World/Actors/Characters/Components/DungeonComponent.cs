using Melia.Shared.Data.Database;
using Melia.Shared.Game.Properties;
using Melia.Zone.Network;
using Melia.Zone.World.Dungeons;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Dungeon Component
	/// </summary>
	public class DungeonComponent : CharacterComponent
	{
		public InstanceDungeon InstanceDungeon { get; set; }

		public DungeonComponent(Character character) : base(character)
		{
		}

		/// <summary>
		/// Get max entry count for instance dungeon by a given dungeon id.
		/// </summary>
		/// <remarks>
		/// This function was created based on GET_INDUN_MAX_ENTERANCE_COUNT from the client.
		/// </remarks>
		/// <param name="dungeonId">Dungeon Id (ClassId)</param>
		/// <returns></returns>
		public int GetMaxEntryCount(int dungeonId)
		{
			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryFind(dungeonId, out var instancedDungeonData))
			{
				Log.Error("The character '{0}' has attempt to get remaining entry count but the given dungeon id ({1}) is invalid.", this.Character.TeamName, dungeonId);
				return 0;
			}

			if (instancedDungeonData.EnableInfiniteEnter || instancedDungeonData.AdmissionItem != null)
			{
				if (instancedDungeonData.DungeonType == InstanceDungeonType.Raid
					|| instancedDungeonData.DungeonType == InstanceDungeonType.GTower)
				{
					return instancedDungeonData.WeeklyEnterableCount;
				}
				return int.MaxValue;
			}

			if (instancedDungeonData.WeeklyEnterableCount != 0)
				return instancedDungeonData.WeeklyEnterableCount;
			else
				return instancedDungeonData.PlayPerReset;
		}

		/// <summary>
		/// Get remaining entry count for a instance dungeon by given dungeon id.
		/// </summary>
		/// <remarks>
		/// This function was created based on GET_CURRENT_ENTERANCE_COUNT from the client.
		/// </remarks>
		/// <param name="dungeonId">Dungeon Id (ClassId)</param>
		/// <returns></returns>
		public int GetCurrentEntryCount(int dungeonId)
		{
			// Requires valid connection for account property access
			// If no connection, return max value to block entry (fail-safe)
			if (this.Character.Connection?.Account == null)
			{
				Log.Warning("DungeonComponent.GetCurrentEntryCount: Character '{0}' has no active connection. Returning max value to block entry.", this.Character.Name);
				return int.MaxValue;
			}

			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryFind(dungeonId, out var instancedDungeonData))
			{
				Log.Error("The character '{0}' has attempt to get remaining entry count but the given dungeon id ({1}) is invalid.", this.Character.TeamName, dungeonId);
				return 0;
			}

			var accountProperties = this.Character.Connection.Account.Properties;

			// Challenge Mode Entry Count
			if (instancedDungeonData.ClassName.Contains("Challenge_")
				&& instancedDungeonData.TicketingType == InstanceDungeonTicketingType.Entrance_Ticket
			)
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.ACCOUNT)
				{
					if (PropertyTable.Exists(accountProperties.Namespace, instancedDungeonData.CheckCountName)
						&& accountProperties.Has(instancedDungeonData.CheckCountName))
					{
						if (accountProperties.TryGetFloat(instancedDungeonData.CheckCountName, out var propertyValue))
							return (int)propertyValue;
					}
				}
				else
				{
					if (PropertyTable.Exists("PCEtc", instancedDungeonData.CheckCountName)
						&& this.Character.Etc.Properties.Has(instancedDungeonData.CheckCountName))
					{
						if (this.Character.Etc.Properties.TryGetFloat(instancedDungeonData.CheckCountName, out var propertyValue))
							return (int)propertyValue;
					}
				}
			}

			// Weekly Based instances
			if (instancedDungeonData.WeeklyEnterableCount != 0)
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.ACCOUNT)
				{
					var propertyName = "IndunWeeklyEnteredCount_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists(accountProperties.Namespace, propertyName) && accountProperties.Has(propertyName))
					{
						if (accountProperties.TryGetFloat(propertyName, out var propertyValue))
							return (int)propertyValue;
					}
				}
				else
				{
					if (instancedDungeonData.DungeonType == InstanceDungeonType.EarringRaid && instancedDungeonData.ClassName != "EarringRaid_Extreme"
						|| instancedDungeonData.DungeonType == InstanceDungeonType.SeasonEarringRaid
						|| instancedDungeonData.StartNPCDialog == "Goddess_Raid_Ex")
					{
						if (PropertyTable.Exists(accountProperties.Namespace, instancedDungeonData.CheckCountName)
							&& accountProperties.Has(instancedDungeonData.CheckCountName))
						{
							if (accountProperties.TryGetFloat(instancedDungeonData.CheckCountName, out var propertyValue))
								return (int)propertyValue;
						}
					}
					var propertyName = "IndunWeeklyEnteredCount_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists(accountProperties.Namespace, propertyName) && accountProperties.Has(propertyName))
					{
						if (accountProperties.TryGetFloat(propertyName, out var propertyValue))
							return (int)propertyValue;
					}
				}
			}
			else
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.PC)
				{
					var propertyName = "InDunCountType_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists("PCEtc", propertyName) && this.Character.Etc.Properties.Has(propertyName))
					{
						if (this.Character.Etc.Properties.TryGetFloat(propertyName, out var propertyValue))
							return (int)propertyValue;
					}
				}
				else
				{
					if (instancedDungeonData.CheckCountName != "None")
					{
						var propertyName = instancedDungeonData.CheckCountName;
						if (PropertyTable.Exists(accountProperties.Namespace, propertyName)
							&& accountProperties.Has(propertyName))
						{
							if (accountProperties.TryGetFloat(propertyName, out var propertyValue))
								return (int)propertyValue;
						}
					}

					if (instancedDungeonData.DungeonType == InstanceDungeonType.Challenge_Auto
						|| instancedDungeonData.DungeonType == InstanceDungeonType.Challenge_Solo)
					{
						var propertyName = "ChallengeModeCompleteCount";
						if (PropertyTable.Exists("PCEtc", propertyName) && this.Character.Etc.Properties.Has(propertyName))
						{
							if (this.Character.Etc.Properties.TryGetFloat(propertyName, out var propertyValue))
								return (int)propertyValue;
						}
					}
					else
					{
						var propertyName = "InDunCountType_" + instancedDungeonData.PlayPerResetType;
						if (PropertyTable.Exists(accountProperties.Namespace, propertyName) && accountProperties.Has(propertyName))
						{
							if (accountProperties.TryGetFloat(propertyName, out var propertyValue))
								return (int)propertyValue;
						}
					}
				}
			}

			return 0;
		}

		/// <summary>
		/// Updates the entry count for a dungeon instance for a given dungeon id.
		/// </summary>
		/// <param name="dungeonId">Dungeon Id (ClassId)</param>
		/// <param name="value"></param>
		/// <returns></returns>
		public void IncreaseEntryCount(int dungeonId, int value)
		{
			// Requires valid connection for account property access
			// If no connection, this indicates an error - player should have been blocked from entering
			if (this.Character.Connection?.Account == null)
			{
				Log.Error("DungeonComponent.IncreaseEntryCount: Character '{0}' has no active connection. Entry count not incremented.", this.Character.Name);
				return;
			}

			if (!ZoneServer.Instance.Data.InstanceDungeonDb.TryFind(dungeonId, out var instancedDungeonData))
			{
				Log.Error("The character '{0}' has attempt to get remaining entry count but the given dungeon id ({1}) is invalid.", this.Character.TeamName, dungeonId);
				return;
			}

			var accountProperties = this.Character.Connection.Account.Properties;

			// Challenge Mode Entry Count
			if (instancedDungeonData.ClassName.Contains("Challenge_")
				&& instancedDungeonData.TicketingType == InstanceDungeonTicketingType.Entrance_Ticket
			)
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.ACCOUNT)
				{
					if (PropertyTable.Exists(accountProperties.Namespace, instancedDungeonData.CheckCountName))
						this.Character.ModifyAccountProperty(instancedDungeonData.CheckCountName, value);
				}
				else
				{
					if (PropertyTable.Exists("PCEtc", instancedDungeonData.CheckCountName))
						this.Character.ModifyEtcProperty(instancedDungeonData.CheckCountName, value);
				}
			}

			// Weekly Based instances
			if (instancedDungeonData.WeeklyEnterableCount != 0)
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.ACCOUNT)
				{
					var propertyName = "IndunWeeklyEnteredCount_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists(accountProperties.Namespace, propertyName))
						this.Character.ModifyAccountProperty(propertyName, value);
				}
				else
				{
					if (instancedDungeonData.DungeonType == InstanceDungeonType.EarringRaid && instancedDungeonData.ClassName != "EarringRaid_Extreme"
						|| instancedDungeonData.DungeonType == InstanceDungeonType.SeasonEarringRaid
						|| instancedDungeonData.StartNPCDialog == "Goddess_Raid_Ex")
					{
						if (PropertyTable.Exists(accountProperties.Namespace, instancedDungeonData.CheckCountName))
							this.Character.ModifyAccountProperty(instancedDungeonData.CheckCountName, value);
					}
					var propertyName = "IndunWeeklyEnteredCount_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists(accountProperties.Namespace, propertyName))
						this.Character.ModifyAccountProperty(propertyName, value);
				}
			}
			else
			{
				if (instancedDungeonData.ResetType == InstanceDungeonResetType.PC)
				{
					var propertyName = "InDunCountType_" + instancedDungeonData.PlayPerResetType;
					if (PropertyTable.Exists("PCEtc", propertyName))
						this.Character.ModifyEtcProperty(propertyName, value);
				}
				else
				{
					if (instancedDungeonData.CheckCountName != "None")
					{
						var propertyName = instancedDungeonData.CheckCountName;
						if (PropertyTable.Exists(accountProperties.Namespace, propertyName))
							this.Character.ModifyAccountProperty(propertyName, value);
					}

					if (instancedDungeonData.DungeonType == InstanceDungeonType.Challenge_Auto
						|| instancedDungeonData.DungeonType == InstanceDungeonType.Challenge_Solo)
					{
						var propertyName = "ChallengeModeCompleteCount";
						if (PropertyTable.Exists("PCEtc", propertyName))
							this.Character.ModifyEtcProperty(propertyName, value);
					}
					else
					{
						var propertyName = "InDunCountType_" + instancedDungeonData.PlayPerResetType;
						if (PropertyTable.Exists(accountProperties.Namespace, propertyName))
							this.Character.ModifyAccountProperty(propertyName, value);
					}
				}
			}
		}
	}
}
