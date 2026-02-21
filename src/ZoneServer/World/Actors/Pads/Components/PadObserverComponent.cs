using System.Collections.Concurrent;
using System.Collections.Generic;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.World.Actors.Pads.Components
{
	/// <summary>
	/// Tracks characters who have seen this pad, ensuring they receive
	/// the destroy packet when the pad is removed.
	/// </summary>
	public class PadObserverComponent : ActorComponent
	{
		private readonly ConcurrentDictionary<int, Character> _observers = new();

		/// <summary>
		/// Returns the pad that owns this component.
		/// </summary>
		public Pad Pad { get; }

		/// <summary>
		/// Creates a new observer component for a pad.
		/// </summary>
		/// <param name="pad"></param>
		public PadObserverComponent(Pad pad) : base(pad)
		{
			this.Pad = pad;
		}

		/// <summary>
		/// Registers a character as an observer of this pad.
		/// </summary>
		/// <param name="character"></param>
		public void AddObserver(Character character)
		{
			_observers.TryAdd(character.Handle, character);
		}

		/// <summary>
		/// Removes a character from the observer list.
		/// </summary>
		/// <param name="character"></param>
		public void RemoveObserver(Character character)
		{
			_observers.TryRemove(character.Handle, out _);
		}

		/// <summary>
		/// Returns all valid observers (connected, same map, same layer).
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Character> GetObservers()
		{
			foreach (var kvp in _observers)
			{
				var character = kvp.Value;
				if (character.Connection != null &&
					character.Map == this.Pad.Map &&
					character.Layer == this.Pad.Layer)
				{
					yield return character;
				}
			}
		}

		/// <summary>
		/// Clears all observers.
		/// </summary>
		public void Clear()
		{
			_observers.Clear();
		}
	}
}
