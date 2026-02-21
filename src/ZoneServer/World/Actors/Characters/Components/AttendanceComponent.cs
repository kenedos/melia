using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Zone.Network;
using Yggdrasil.Logging;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// Attendance Events/Rewards
	/// </summary>
	/// <remarks>
	/// Author: TheReturn
	/// </remarks>
	public class AttendanceComponent : CharacterComponent
	{
		private readonly SortedList<int, AttendanceRewardEntry> _attendencesRewarded = new();

		public AttendanceComponent(Character character) : base(character)
		{
		}

		/// <summary>
		/// Returns the attendance rewards.
		/// </summary>
		/// <returns></returns>
		public SortedList<int, AttendanceRewardEntry> GetAttendanceRewards()
		{
			lock (_attendencesRewarded)
				return _attendencesRewarded;
		}

		/// <summary>
		/// Returns whatever the attendance has been rewarded or not.
		/// </summary>
		/// <param name="classId">Attendance Reward ClassId</param>
		/// <returns></returns>
		public bool HasBeenRewarded(int classId)
		{
			return _attendencesRewarded.ContainsKey(classId);
		}

		/// <summary>
		/// Returns whatever the attendance has an event within a reward to be retrieved.
		/// </summary>
		/// <returns></returns>
		public bool CanRetrieveAnyReward()
		{
			if (!Feature.IsEnabled(FeatureId.AttendanceRewardSystem))
				return false;

			lock (_attendencesRewarded)
			{
				var now = DateTime.Now;
				var eventAttendanceAvailable = new List<EventAttendanceData>();

				foreach (var keyValuePair in ZoneServer.Instance.Data.EventAttendanceDb.Entries)
				{
					// 1. Checks if the event is happening
					if (now >= keyValuePair.Value.StartTime && now <= keyValuePair.Value.EndTime)
					{
						// 2: Checks if today reward was retrieved
						// 2.1: Gets the full list of attendance rewards and order it by DayOffset
						var list = ZoneServer.Instance.Data.RewardAttendanceDb.FindByGroupName(keyValuePair.Value.ClassName)
							.OrderBy(a => a.DayOffset)
							.ToList();

						var firstElement = list.First();
						if (firstElement != null && firstElement.DayOffset == 1)
						{
							foreach (var element in list)
							{
								element.DayOffset -= 1;
							}
						}

						// 2.2: Creates a HashSet
						var rewardIds = list.Select(r => r.Id).ToHashSet();

						// 2.3: Selects rewards that has been received from the full list
						var matchingRewards = _attendencesRewarded.Values.Where(a => rewardIds.Contains(a.RewardClassId)).ToList();

						// 2.4: Processed if the list has elements
						if (matchingRewards.Count > 0)
						{
							// 2.5: Discovery which day the player is at
							var lastRewardRetrieved = matchingRewards.Last();

							// 2.6: Checks if the last reward was retrieved yesterday
							if (lastRewardRetrieved.RewardTime < DateTime.Today)
							{
								return true;
							}
						}
						else
						{
							if (list.Count > 0)
								return true;
						}
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Returns whatever the attendance has an event within a reward to be retrieved for a given eventClassId.
		/// </summary>
		/// <returns></returns>
		public bool CanRetrieveReward(int eventClassId, out EventAttendanceData eventAttendanceData)
		{
			eventAttendanceData = default;
			if (!Feature.IsEnabled(FeatureId.AttendanceRewardSystem))
				return false;

			lock (_attendencesRewarded)
			{
				var now = DateTime.Now;

				if (!ZoneServer.Instance.Data.EventAttendanceDb.TryFind(eventClassId, out EventAttendanceData data))
				{
					Log.Error("The Account '{0}' has attempt to get a attendance reward but no event data ({1}) was found.", this.Character.TeamName, eventClassId);
					return false;
				}

				eventAttendanceData = data;

				// 1. Checks if the event is happening
				if (now >= data.StartTime && now <= data.EndTime)
				{
					// 2: Checks if today reward was retrieved
					// 2.1: Gets the full list of attendance rewards and order it by DayOffset
					var list = ZoneServer.Instance.Data.RewardAttendanceDb.FindByGroupName(data.ClassName)
						.OrderBy(a => a.DayOffset)
						.ToList();

					var firstElement = list.First();
					if (firstElement != null && firstElement.DayOffset == 1)
					{
						foreach (var element in list)
						{
							element.DayOffset -= 1;
						}
					}

					// 2.2: Creates a HashSet
					var rewardIds = list.Select(r => r.Id).ToHashSet();

					// 2.3: Selects rewards that has been received from the full list
					var matchingRewards = _attendencesRewarded.Values.Where(a => rewardIds.Contains(a.RewardClassId)).ToList();

					// 2.4: Processed if the list has elements
					if (matchingRewards.Count > 0)
					{
						// 2.5: Discovery which day the player is at
						var lastRewardRetrieved = matchingRewards.Last();

						// 2.6: Checks if the last reward was retrieved yesterday
						if (lastRewardRetrieved.RewardTime < DateTime.Today)
						{
							return true;
						}
					}
					else
					{
						if (list.Count > 0)
							return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Gets the next reward day.
		/// </summary>
		/// <param name="groupName">Attendance Event ClassName</param>
		/// <returns></returns>
		public RewardAttendanceData GetNextRewardAttendanceData(string groupName)
		{
			lock (_attendencesRewarded)
			{
				if (!ZoneServer.Instance.Data.EventAttendanceDb.TryFind(groupName, out EventAttendanceData data))
				{
					Log.Error("The Account '{0}' has attempt to get the next attendance '{1}' but no attendance reward data was found.", this.Character.TeamName, groupName);
					return null;
				}

				var now = DateTime.Now;

				// 1. Checks if the event is happening
				if (now >= data.StartTime && now <= data.EndTime)
				{
					// 2: Checks if today reward was retrieved
					// 2.1: Gets the full list of attendance rewards and order it by DayOffset
					var list = ZoneServer.Instance.Data.RewardAttendanceDb.FindByGroupName(groupName)
						.OrderBy(a => a.DayOffset)
						.ToList();

					var firstElement = list.First();
					if (firstElement != null && firstElement.DayOffset == 1)
					{
						foreach (var element in list)
						{
							element.DayOffset -= 1;
						}
					}

					// 2.2: Creates a HashSet
					var rewardIds = list.Select(r => r.Id).ToHashSet();

					// 2.3: Selects rewards that has been received from the full list
					var matchingRewards = _attendencesRewarded.Values.Where(a => rewardIds.Contains(a.RewardClassId)).ToList();

					// 2.4: Processed if the list has elements
					if (matchingRewards.Count > 0)
					{
						// 2.5: Discovery which day the player is at
						var lastRewardRetrieved = matchingRewards.Last();

						// 2.6: Checks if the last reward was retrieved yesterday
						if (lastRewardRetrieved.RewardTime < DateTime.Today)
						{
							// 3.1: Returns the next reward data
							return list.Where(a => a.DayOffset == lastRewardRetrieved.DayOffSet + 1).FirstOrDefault();
						}
						else
						{
							// 3.2: Stills returns the next reward data even if we already retrieved it
							return list.Where(a => a.DayOffset == lastRewardRetrieved.DayOffSet).FirstOrDefault();
						}
					}
					else
					{
						// 3.3: returns the first element of the list based on the DayOffset
						return list.FirstOrDefault();
					}
				}

				Log.Error("The Account '{0}' has attempt to get the next attendance '{1}' but no attendance event is not in period time.", this.Character.TeamName, groupName);
				return null;
			}
		}

		/// <summary>
		/// Gives the daily attendance reward to the player.
		/// </summary>
		/// <param name="rewardClassId">Attendance Reward ClassId</param>
		/// <param name="silently">Attendance Reward ClassId</param>
		public void AddAttendanceReward(int rewardClassId, bool silently = false)
		{
			lock (_attendencesRewarded)
			{
				if (!ZoneServer.Instance.Data.RewardAttendanceDb.TryFind(rewardClassId, out RewardAttendanceData rewardAttendanceData))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance ({2}) but no attendance reward data was found.", this.Character.TeamName, this.Character.DbId, rewardClassId);
					return;
				}

				if (!ZoneServer.Instance.Data.EventAttendanceDb.TryFind(rewardAttendanceData.GroupName, out EventAttendanceData eventAttendanceData))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance reward ({2})  but no attendance event data was found", this.Character.TeamName, this.Character.DbId, rewardAttendanceData.Id);
					return;
				}

				if (!ZoneServer.Instance.Data.ItemDb.TryFind(rewardAttendanceData.RewardItem_1, out ItemData itemData))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance reward ({2})  but no item data was found for '{3}'", this.Character.TeamName, this.Character.DbId, rewardAttendanceData.Id, rewardAttendanceData.RewardItem_1);
					return;
				}

				if (_attendencesRewarded.TryGetValue(rewardAttendanceData.Id, out var value))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance reward ({2}) but an entry already existed.", this.Character.TeamName, this.Character.DbId, rewardAttendanceData.Id);
					return;
				}

				_attendencesRewarded.Add(rewardAttendanceData.Id, new AttendanceRewardEntry()
				{
					EventClassId = eventAttendanceData.Id,
					RewardClassId = rewardAttendanceData.Id,
					ItemId = itemData.Id,
					ItemCount = rewardAttendanceData.RewardItemCnt_1,
					RewardTime = DateTime.Now,
					DayOffSet = rewardAttendanceData.DayOffset
				});

				if (!silently)
				{
					this.Character.AddItem(itemData.Id, rewardAttendanceData.RewardItemCnt_1);
					this.UpdateClientReceiptReward(eventAttendanceData, true);
				}
			}
		}

		/// <summary>
		/// Gives the daily attendance reward to the player.
		/// </summary>
		/// <param name="rewardClassId">Attendance Reward ClassId</param>
		/// <param name="eventClassId">Attendance Reward ClassId</param>
		/// <param name="itemId">Attendance Reward ClassId</param>
		/// <param name="itemCount">Attendance Reward ClassId</param>
		/// <param name="dayOffSet">Attendance Reward ClassId</param>
		/// <param name="rewardTime">Attendance Reward ClassId</param>
		/// <param name="silently">Attendance Reward ClassId</param>
		public void AddAttendanceReward(int rewardClassId, int eventClassId, int itemId, int itemCount, int dayOffSet, DateTime rewardTime, bool silently = false)
		{
			lock (_attendencesRewarded)
			{
				if (!ZoneServer.Instance.Data.RewardAttendanceDb.TryFind(rewardClassId, out RewardAttendanceData rewardData))
				{
					Log.Error("The Account '{0}' has attempt to insert a attendance ({1}) but no attendance reward data was found.", this.Character.TeamName, rewardClassId);
					return;
				}

				if (!ZoneServer.Instance.Data.EventAttendanceDb.TryFind(eventClassId, out EventAttendanceData eventData))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance reward ({2})  but no attendance event data was found", this.Character.TeamName, this.Character.DbId, rewardData.Id);
					return;
				}

				if (_attendencesRewarded.TryGetValue(rewardData.Id, out var value))
				{
					Log.Error("The Account '{0}' ({1}) has attempt to insert a attendance reward ({2}) but an entry already existed.", this.Character.TeamName, this.Character.DbId, rewardData.Id);
					return;
				}

				if (!ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out ItemData itemData) || rewardTime > DateTime.Now || dayOffSet < 0 || itemCount <= 0)
				{
					Log.Error("The Account '{0}' has attempt to insert a attendance reward with an invalid values.", this.Character.TeamName);
					return;
				}

				_attendencesRewarded.Add(rewardData.Id, new AttendanceRewardEntry()
				{
					EventClassId = eventData.Id,
					RewardClassId = rewardData.Id,
					ItemId = itemData.Id,
					ItemCount = itemCount,
					RewardTime = rewardTime,
					DayOffSet = dayOffSet
				});

				if (!silently)
				{
					this.Character.AddItem(itemData.Id, itemCount);
					this.UpdateClientReceiptReward(eventData, true);
				}
			}
		}

		/// <summary>
		/// Updates the client with all attendance reward info.
		/// </summary>
		public void UpdateClient()
		{
			lock (_attendencesRewarded)
			{
				this.UpdateClientReceiptRewards();

				if (this.CanRetrieveAnyReward() && this.Character.Map.ClassName != "id_maple_01")
					Send.ZC_ATTENDANCE_REWARD_CHECK_UI_ON(this.Character.Connection);
			}
		}

		/// <summary>
		/// Updates the client with all on going attendance event within the rewards received.
		/// </summary>
		public void UpdateClientReceiptRewards()
		{
			foreach (var keyValuePair in ZoneServer.Instance.Data.EventAttendanceDb.Entries)
			{
				this.UpdateClientReceiptReward(keyValuePair.Value);
			}
		}

		/// <summary>
		/// Sends an packet containing the update info about the on going attendance event and received rewards.
		/// </summary>
		/// <param name="data">Event attendance data</param>
		private void UpdateClientReceiptReward(EventAttendanceData data, bool openUi = false)
		{
			var now = DateTime.Now;

			if (now >= data.StartTime && now <= data.EndTime)
			{
				// 2: Checks if today reward was retrieved
				// 2.1: Gets the full list of attendance rewards and order it by DayOffset
				var list = ZoneServer.Instance.Data.RewardAttendanceDb.FindByGroupName(data.ClassName)
					.OrderBy(a => a.DayOffset)
					.ToList();

				var firstElement = list.FirstOrDefault();
				if (firstElement != null && firstElement.DayOffset == 1)
				{
					foreach (var element in list)
					{
						element.DayOffset -= 1;
					}
				}

				// 2.2: Creates a HashSet
				var rewardIds = list.Select(r => r.Id).ToHashSet();

				// 2.3: Selects rewards that has been received from the full list
				var matchingRewards = _attendencesRewarded.Values.Where(a => rewardIds.Contains(a.RewardClassId)).ToList();

				// 2.4: Processed if the list has elements
				if (matchingRewards.Count > 0)
				{
					Send.ZC_ATTENDANCE_RECEIPT_REWARD(this.Character.Connection, data, matchingRewards, openUi);
				}
				else
				{
					if (list.Count > 0)
						Send.ZC_ATTENDANCE_RECEIPT_REWARD(this.Character.Connection, data, matchingRewards, openUi);
				}
			}
		}
	}

	public class AttendanceRewardEntry
	{
		public int EventClassId { get; set; }
		public int RewardClassId { get; set; }
		public int ItemId { get; set; }
		public int ItemCount { get; set; }
		public int DayOffSet { get; set; }
		public DateTime RewardTime { get; set; }
	}
}
