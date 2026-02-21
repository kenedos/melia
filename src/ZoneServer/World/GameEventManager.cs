using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Zone.Events;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;

namespace Melia.Zone.World
{
	/// <summary>
	/// Holds all available game events, starts and stops them, and notifies
	/// players about them.
	/// </summary>
	public class GameEventManager
	{
		private readonly Dictionary<string, GameEventScript> _gameEvents = new();

		/// <summary>
		/// Returns the number of active events.
		/// </summary>
		public int CountActive { get { lock (_gameEvents) return _gameEvents.Values.Count(a => a.IsActive); } }

		/// <summary>
		/// Returns true if any events are active.
		/// </summary>
		public bool AnyActive { get { lock (_gameEvents) return _gameEvents.Values.Any(a => a.IsActive); } }

		/// <summary>
		/// Creates new instance of manager.
		/// </summary>
		public GameEventManager()
		{
		}

		/// <summary>
		/// Sets up necessarily subscriptions.
		/// </summary>
		public void Initialize()
		{
			ZoneServer.Instance.ServerEvents.MinuteTick.Subscribe(this.OnMinutesTimeTick);
			ZoneServer.Instance.ServerEvents.PlayerReady.Subscribe(this.OnPlayerReady);
		}

		/// <summary>
		/// Called once a minute to check which events need to be started
		/// or stopped.
		/// </summary>
		private void OnMinutesTimeTick(object sender, TimeEventArgs args)
		{
			var toStart = new List<GameEventScript>();
			var toEnd = new List<GameEventScript>();

			lock (_gameEvents)
			{
				if (_gameEvents.Count == 0)
					return;

				foreach (var gameEvent in _gameEvents.Values)
				{
					var isActive = (gameEvent.IsActive);
					var isActiveTime = gameEvent.IsActiveTime(args.Now);

					if (!isActive && isActiveTime)
						toStart.Add(gameEvent);
					else if (isActive && !isActiveTime)
						toEnd.Add(gameEvent);
				}
			}

			foreach (var gameEvent in toStart)
				gameEvent.Start();

			foreach (var gameEvent in toEnd)
				gameEvent.End();

			if (toStart.Count != 0 || toEnd.Count != 0)
			{
				var activeEvents = this.GetActiveEvents();
				var message = GetBroadcastMessage(activeEvents);
				Send.ZC_TEXT(NoticeTextType.Gold, message);
			}
		}

		/// <summary>
		/// Returns event script with the given name or null if it
		/// doesn't exist.
		/// </summary>
		public GameEventScript Get(string ident)
		{
			lock (_gameEvents)
			{
				_gameEvents.TryGetValue(ident, out var eventScript);
				return eventScript;
			}
		}

		/// <summary>
		/// Called when a player logged in, sends notice about active events.
		/// </summary>
		private void OnPlayerReady(object sender, PlayerEventArgs args)
		{
			var character = args.Character;
			if (character.Connection == null)
				return;

			var activeEvents = this.GetActiveEvents();
			var message = GetBroadcastMessage(activeEvents);

			if (!string.IsNullOrWhiteSpace(message))
				Send.ZC_TEXT(character, NoticeTextType.Gold, message);
		}

		/// <summary>
		/// Registers an event with the manager.
		/// </summary>
		public void Register(GameEventScript gameEvent)
		{
			lock (_gameEvents)
			{
				if (_gameEvents.ContainsKey(gameEvent.Id))
					throw new ArgumentException("An event with the id '" + gameEvent.Id + "' already exists.");

				_gameEvents[gameEvent.Id] = gameEvent;
			}
		}

		/// <summary>
		/// Unregisters an event from the manager.
		/// </summary>
		public void Unregister(string gameEventId)
		{
			lock (_gameEvents)
			{
				_gameEvents.Remove(gameEventId);
			}
		}

		/// <summary>
		/// Adds an activation span to the given event.
		/// </summary>
		public void AddActivationSpan(string gameEventId, DateTime start, DateTime end)
		{
			GameEventScript gameEvent;
			lock (_gameEvents)
			{
				if (!_gameEvents.TryGetValue(gameEventId, out gameEvent))
					throw new ArgumentException("Unknown event id '" + gameEventId + "'.");
			}

			var span = new ActivationSpan();
			span.Id = gameEventId;
			span.Start = start;
			span.End = end;

			gameEvent.AddActivationSpan(span);
		}

		/// <summary>
		/// Returns true if the given event is currently active.
		/// </summary>
		public bool IsActive(string gameEventId)
		{
			GameEventScript gameEvent;
			lock (_gameEvents)
				_gameEvents.TryGetValue(gameEventId, out gameEvent);

			if (gameEvent == null)
				return false;

			return gameEvent.IsActive;
		}

		/// <summary>
		/// Returns broadcast message that is used to inform players about
		/// active events.
		/// </summary>
		private static string GetBroadcastMessage(IEnumerable<GameEventScript> gameEvents)
		{
			var sb = new StringBuilder();
			var count = gameEvents.Count();

			var i = 0;
			foreach (var gameEvent in gameEvents)
			{
				if (gameEvent.UseEventNotices)
					sb.AppendFormat(gameEvent.InProgressNotice, gameEvent.Name);
				if (++i < count)
					sb.Append("     ");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Returns list of all events that are currently active.
		/// </summary>
		public GameEventScript[] GetActiveEvents()
		{
			lock (_gameEvents)
				return _gameEvents.Values.Where(a => a.IsActive).ToArray();
		}

		/// <summary>
		/// Returns list of all events that are currently active.
		/// </summary>
		public GameEventScript[] GetAllEvents()
		{
			lock (_gameEvents)
				return _gameEvents.Values.ToArray();
		}
	}
}
