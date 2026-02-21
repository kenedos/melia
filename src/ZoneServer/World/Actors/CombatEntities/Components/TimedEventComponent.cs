using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Composition;
using Yggdrasil.Logging;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.Components
{
	/// <summary>
	/// An event that occurs or reccurs 
	/// at a specific time interval
	/// </summary>
	public class TimedEventComponent : CombatEntityComponent, IUpdateable
	{
		private readonly Dictionary<string, TimedEvent> _events;

		public TimedEventComponent(ICombatEntity entity) : base(entity)
		{
			_events = new Dictionary<string, TimedEvent>();
		}

		/// <summary>
		/// Timed Event, An Event which will run after a delay
		/// </summary>
		public class TimedEvent
		{
			public TimedEvent(TimeSpan startDelay, TimeSpan repeatDelay, int repeatCount, Action<ICombatEntity> function)
			{
				this.StartDelay = startDelay;
				this.RepeatDelay = repeatDelay;
				this.RepeatCount = repeatCount;
				this.Function = function;
				this.NextRunTime = DateTime.Now.Add(this.StartDelay);
			}

			public Action<ICombatEntity> Function { get; }
			public DateTime NextRunTime { get; set; }
			public TimeSpan StartDelay { get; }
			public TimeSpan RepeatDelay { get; }
			public int RepeatCount { get; }
			public int Count { get; set; }
			public TimedEventState State { get; set; } = TimedEventState.Started;
			public bool IsInfinite => this.RepeatCount < 0;

			public enum TimedEventState
			{
				Started,
				Completed,
				Canceled,
			}

			public void Cancel()
			{
				if (this.State != TimedEventState.Completed)
					this.State = TimedEventState.Canceled;
			}
		}

		/// <summary>
		/// Cancelable Timed Event, An Event which will run after a delay but is cancelable
		/// </summary>
		public class CancelableTimedEvent : TimedEvent
		{
			public Action<ICombatEntity> CancelFunction { get; }
			public CancelableTimedEvent(TimeSpan startDelay, TimeSpan repeatDelay, int repeatCount, Action<ICombatEntity> onCompletion, Action<ICombatEntity> onCanceled) : base(startDelay, repeatDelay, repeatCount, onCompletion)
			{
				this.CancelFunction = onCanceled;
			}
		}

		/// <summary>
		/// Add a lambda function to run repeatedly after a delay
		/// </summary>
		/// <param name="startDelay"></param>
		/// <param name="repeatDelay"></param>
		/// <param name="repeatCount"></param>
		/// <param name="eventId"></param>
		/// <param name="function"></param>
		public void Add(TimeSpan startDelay, TimeSpan repeatDelay, int repeatCount, string eventId, Action<ICombatEntity> function)
		{
			lock (_events)
			{
				if (!_events.ContainsKey(eventId))
				{
					_events.Add(eventId, new TimedEvent(startDelay, repeatDelay, repeatCount, function));
				}
			}
		}


		/// <summary>
		/// Add a lambda function to run repeatedly after a delay or on cancelling the event
		/// </summary>
		/// <param name="startDelay"></param>
		/// <param name="repeatDelay"></param>
		/// <param name="repeatCount"></param>
		/// <param name="eventId"></param>
		/// <param name="onCompleted"></param>
		/// <param name="onCanceled"></param>
		public void Add(TimeSpan startDelay, TimeSpan repeatDelay, int repeatCount, string eventId, Action<ICombatEntity> onCompleted, Action<ICombatEntity> onCanceled)
		{
			lock (_events)
			{
				if (!_events.ContainsKey(eventId))
				{
					_events.Add(eventId, new CancelableTimedEvent(startDelay, repeatDelay, repeatCount, onCompleted, onCanceled));
				}
			}
		}

		/// <summary>
		/// Return the TimedEvent or null
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public TimedEvent Get(string id)
		{
			lock (_events)
				return _events.TryGetValue(id, out TimedEvent result) ? result : null;
		}

		/// <summary>
		/// Update Timed Events
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			lock (_events)
			{
				if (_events.Count == 0)
					return;

				var eventsToRemoved = new List<string>();

				foreach (var timedEventKey in _events.Keys)
				{
					var timedEvent = _events[timedEventKey];

					if (timedEvent.State == TimedEvent.TimedEventState.Canceled)
					{
						if (timedEvent is CancelableTimedEvent cancelableTimedEvent)
							cancelableTimedEvent.CancelFunction?.Invoke(this.Entity);

						eventsToRemoved.Add(timedEventKey);
					}
					else if (DateTime.Now > timedEvent.NextRunTime && (timedEvent.IsInfinite || timedEvent.Count <= timedEvent.RepeatCount))
					{
						timedEvent.Function?.Invoke(this.Entity);
						timedEvent.NextRunTime = DateTime.Now.Add(timedEvent.RepeatDelay);
						timedEvent.Count++;

						if (!timedEvent.IsInfinite && timedEvent.Count >= timedEvent.RepeatCount)
						{
							timedEvent.State = TimedEvent.TimedEventState.Completed;
							eventsToRemoved.Add(timedEventKey);
						}
					}
				}
				foreach (var timedEventKey in eventsToRemoved)
				{
					_events.Remove(timedEventKey);
				}
			}
		}

		public bool Remove(string eventId)
		{
			lock (_events)
			{
				return _events.Remove(eventId);
			}
		}
	}
}
