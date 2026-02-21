using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
// using Melia.Zone.World.GameEvents; // Removed: GameEvents namespace deleted
using Yggdrasil.Logging;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Script for in-game events, like Golden Fishing.
	/// </summary>
	public abstract class GameEventScript : GeneralScript
	{
		private Timer _recurringTimer;
		private readonly List<ActivationSpan> _activationSpans = new List<ActivationSpan>();

		/// <summary>
		/// The event's unique id.
		/// </summary>
		/// <remarks>
		/// Sent to client, some ids activate special client behavior.
		/// </remarks>
		public string Id { get; private set; }

		/// <summary>
		/// The event's name, which is used in notices and broadcasts.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// The event's in progress message, 
		/// which is used for the broadcast notice.
		/// </summary>
		/// <remarks>
		/// Uses a default value if not set.
		/// </remarks>
		public string InProgressNotice { get; private set; }

		/// <summary>
		/// The event's ending message, 
		/// which is used for the broadcast notice.
		/// </summary>
		/// <remarks>
		/// Uses a default value if not set.
		/// </remarks>
		public string EndingNotice { get; private set; }

		/// <summary>
		/// Returns the current state of the event.
		/// </summary>
		public bool IsActive { get; private set; }

		/// <summary>
		/// Returns if the event shows notices upon starting or stopping.
		/// </summary>
		public bool UseEventNotices { get; private set; }

		/// <summary>
		/// Returns list of all activation spans.
		/// </summary>
		/// <returns></returns>
		public ActivationSpan[] GetActivationSpans()
		{
			lock (_activationSpans)
				return _activationSpans.ToArray();
		}

		/// <summary>
		/// Loads and sets up event.
		/// </summary>
		/// <returns></returns>
		public override bool Init()
		{
			base.Init();

			if (string.IsNullOrWhiteSpace(this.Id) || string.IsNullOrWhiteSpace(this.Name))
			{
				Log.Error("Id or name not set for event script '{0}'.", this.GetType().Name);
				return false;
			}

			if (string.IsNullOrWhiteSpace(this.InProgressNotice))
				this.InProgressNotice = L("The {0} Event is now in progress.");

			if (string.IsNullOrWhiteSpace(this.EndingNotice))
				this.EndingNotice = L("The {0} Event has ended.");

			this.UseEventNotices = true;

			ZoneServer.Instance.GameEvents.Register(this);

			this.AfterLoad();

			return true;
		}

		public override void Dispose()
		{
			_recurringTimer?.Stop();
			_recurringTimer?.Dispose();
			_recurringTimer = null;

			ZoneServer.Instance.GameEvents.Unregister(this.Id);
			this.End();

			base.Dispose();
		}

		/// <summary>
		/// Called after script was registered, so it can schedule itself.
		/// </summary>
		public virtual void AfterLoad()
		{
		}

		/// <summary>
		/// Sets event's id.
		/// </summary>
		/// <param name="id"></param>
		public void SetId(string id)
		{
			this.Id = id;
		}

		/// <summary>
		/// Sets event's name, which is used for notices and broadcasts.
		/// </summary>
		/// <param name="name"></param>
		public void SetName(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Sets event's notice message, which is used for the broadcast notice.
		/// </summary>
		/// <param name="inProgressNotice"></param>
		public void SetInProgressNotice(string inProgressNotice)
		{
			this.InProgressNotice = inProgressNotice;
		}

		/// <summary>
		/// Sets event's notice message, which is used for the broadcast notice.
		/// </summary>
		/// <param name="endingNotice"></param>
		public void SetEndingNotice(string endingNotice)
		{
			this.EndingNotice = endingNotice;
		}

		/// <summary>
		/// Starts event if it's not active yet.
		/// </summary>
		public void Start()
		{
			if (this.IsActive)
				return;

			this.IsActive = true;
			this.OnStart();

			if (this.UseEventNotices)
				Send.ZC_TEXT(NoticeTextType.Gold, $"{this.Name}: {this.InProgressNotice}");
			//Send.GameEventStateUpdate(this.Id, this.IsActive);
		}

		/// <summary>
		/// Stops event if it's active.
		/// </summary>
		public void End()
		{
			if (!this.IsActive)
				return;

			this.IsActive = false;
			this.OnEnd();

			if (this.UseEventNotices)
				Send.ZC_TEXT(NoticeTextType.Gold, $"{this.Name}: {this.EndingNotice}");
			//Send.GameEventStateUpdate(this.Id, this.IsActive);
		}

		/// <summary>
		/// Called when the event is activated.
		/// </summary>
		protected virtual void OnStart()
		{
		}

		/// <summary>
		/// Called when the event is deactivated.
		/// </summary>
		protected virtual void OnEnd()
		{
		}

		/// <summary>
		/// Adds the given activation span to the event, in which it's
		/// supposed to be active.
		/// </summary>
		/// <param name="span"></param>
		public void AddActivationSpan(ActivationSpan span)
		{
			lock (_activationSpans)
				_activationSpans.Add(span);

			var now = DateTime.Now;

			// Active time
			if (now >= span.Start && now < span.End)
			{
				this.Start();
			}
			// Inactive time
			else
			{
				this.End();
			}
		}

		/// <summary>
		/// Returns true if the event is supposed to be active at the given
		/// time, based on its activation spans.
		/// </summary>
		/// <param name="time"></param>
		/// <returns></returns>
		public bool IsActiveTime(DateTime time)
		{
			lock (_activationSpans)
				return _activationSpans.Any(a => time >= a.Start && time < a.End);
		}

		// Removed: GlobalBonusStat/GlobalBonusManager were in deleted GameEvents namespace
		// protected void AddGlobalBonus(GlobalBonusStat stat, float multiplier) { }
		// protected void RemoveGlobalBonuses() { }
		// protected void AddGlobalBuff(BuffId buffId, float numArg1 = 0, float numArg2 = 0) { }
		// protected void RemoveGlobalBuffs() { }

		/// <summary>
		/// Schedules event to be active during the given time span.
		/// </summary>
		/// <param name="gameEventId"></param>
		/// <param name="from"></param>
		/// <param name="till"></param>
		protected void ScheduleEvent(string gameEventId, DateTime from, DateTime till)
		{
			if (till < from)
				Log.Warning("{0}: ScheduleEvent: Till date is earlier than from date.", this.GetType().Name);

			ZoneServer.Instance.GameEvents.AddActivationSpan(gameEventId, from, till);
		}

		/// <summary>
		/// Schedules event to be active during the given time span.
		/// </summary>
		/// <param name="gameEventId"></param>
		/// <param name="from"></param>
		/// <param name="timeSpan"></param>
		protected void ScheduleEvent(string gameEventId, DateTime from, TimeSpan timeSpan)
		{
			var till = from.Add(timeSpan);
			this.ScheduleEvent(gameEventId, from, till);
		}

		/// <summary>
		/// Schedules this event to be active during the given time span.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="till"></param>
		protected void ScheduleEvent(DateTime from, DateTime till)
		{
			var gameEventId = this.Id;
			this.ScheduleEvent(gameEventId, from, till);
		}

		/// <summary>
		/// Schedules this event to be active during the given time span.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="timeSpan"></param>
		protected void ScheduleEvent(DateTime from, TimeSpan timeSpan)
		{
			var gameEventId = this.Id;
			this.ScheduleEvent(gameEventId, from, timeSpan);
		}

		/// <summary>
		/// Schedules the event to run repeatedly with a delay.
		/// </summary>
		/// <param name="delay">The delay between event runs (in milliseconds)</param>
		/// <param name="startTime">The time to start the event (if it's in the past, it will start immediately)</param>
		/// <param name="duration">The duration of each event run (in milliseconds)</param>
		protected void ScheduleRecurringEvent(TimeSpan delay, DateTime startTime, TimeSpan duration)
		{
			_recurringTimer = new System.Timers.Timer(delay.TotalMilliseconds);

			_recurringTimer.Elapsed += (sender, e) =>
			{
				var now = DateTime.Now;
				var eventStart = new DateTime(now.Year, now.Month, now.Day, startTime.Hour, startTime.Minute, startTime.Second);
				var eventEnd = eventStart.Add(duration);

				if (now >= eventStart && now < eventEnd)
				{
					Start();
					_recurringTimer.Interval = delay.TotalMilliseconds;
				}
				else
				{
					// Wait until next day's start time
					var nextDay = eventStart.AddDays(1);
					_recurringTimer.Interval = (nextDay - now).TotalMilliseconds;
				}
			};

			_recurringTimer.AutoReset = true;

			if (startTime < DateTime.Now)
			{
				_recurringTimer.Start();
			}
			else
			{
				var startDelay = (startTime - DateTime.Now).TotalMilliseconds;
				_recurringTimer.Interval = startDelay;
				_recurringTimer.Start();
			}
		}

		/// <summary>
		/// Adds the the item to the given shop.
		/// </summary>
		/// <param name="shopName"></param>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		/// <param name="price"></param>
		/// <param name="stock"></param>
		protected void AddEventItemToShop(string shopName, int itemId, int amount = 1, int price = -1, int stock = -1)
		{
			// We do not have this in Melia but it's not a bad idea.
			//var shop = ZoneServer.Instance.ScriptManager.NpcShopScripts.Get(shopName);
			//if (shop == null)
			{
				Log.Error("{0}.AddEventItemToShop: Shop '{1}' not found.", this.GetType().Name, shopName);
				return;
			}

			//shop.Add(Localization.Get("Event"), itemId, amount, price, stock);
		}

		/// <summary>
		/// Removes all event items from the given shop.
		/// </summary>
		/// <param name="shopName"></param>
		protected void RemoveEventItemsFromShop(string shopName)
		{
			//var shop = ZoneServer.Instance.ScriptManager.NpcShopScripts.Get(shopName);
			//if (shop == null)
			{
				Log.Error("{0}.RemoveEventItemsFromShop: Shop '{1}' not found.", this.GetType().Name, shopName);
				return;
			}

			//shop.ClearTab(Localization.Get("Event"));
		}

		/// <summary>
		/// Sets if the event shows notices upon starting or stopping.
		/// </summary>
		/// <param name="useEventNotices"></param>
		protected void HideEventNotices(bool useEventNotices)
		{
			this.UseEventNotices = !useEventNotices;
		}
	}

	public class ActivationSpan
	{
		public string Id { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
