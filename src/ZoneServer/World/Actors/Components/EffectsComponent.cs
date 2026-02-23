using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Scheduling;

namespace Melia.Zone.World.Actors.Components
{
	public class EffectsComponent : ActorComponent, IUpdateable
	{
		private readonly Dictionary<string, Effect> _effects = new Dictionary<string, Effect>();

		public int Count
		{
			get
			{
				lock (_effects)
					return _effects.Count;
			}
		}

		public EffectsComponent(IActor owner) : base(owner)
		{
		}

		public void AddEffect(Effect effect)
		{
			lock (_effects)
			{
				var effectName = $"{_effects.Count}";
				_effects.Add(effectName, effect);
			}

			this.BroadcastEffectAddition(effect);
		}

		public void AddEffect(string effectName, Effect effect)
		{
			lock (_effects)
				_effects.Add(effectName, effect);

			this.BroadcastEffectAddition(effect);
		}

		/// <summary>
		/// Sends an effect to all players that can currently see the owner.
		/// </summary>
		/// <param name="effect">The effect to show.</param>
		private void BroadcastEffectAddition(Effect effect)
		{
			if (this.Actor?.Map == null)
				return;

			// Get all characters within a generous visibility range of the owner.
			var charactersInRange = this.Actor.Map.GetActorsIn<Character>(new CircleF(this.Actor.Position, 1000f));

			foreach (var character in charactersInRange)
			{
				// Check if the character is connected and can actually see the owner actor.
				if (character.Connection != null && character.CanSee(this.Actor))
				{
					effect.ShowEffect(character.Connection, this.Actor);
				}
			}
		}

		public void ShowEffects(IZoneConnection connection)
		{
			Effect[] snapshot;

			lock (_effects)
				snapshot = _effects.Values.ToArray();

			foreach (var effect in snapshot)
				effect.ShowEffect(connection, this.Actor);
		}

		/// <summary>
		/// Remove an effect with a given name.
		/// </summary>
		/// <param name="effectName"></param>
		public void RemoveEffect(string effectName)
		{
			if (string.IsNullOrEmpty(effectName))
				return;

			Effect removed = null;

			lock (_effects)
			{
				if (_effects.TryGetValue(effectName, out var effect))
				{
					removed = effect;
					_effects.Remove(effectName);
				}
			}

			removed?.OnRemove(this.Actor);
		}

		/// <summary>
		/// Updates the component, triggering events.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			Effect[] snapshot;

			lock (_effects)
				snapshot = _effects.Values.ToArray();

			foreach (var effect in snapshot)
			{
				if (effect is IUpdateable updateable)
					updateable.Update(elapsed);
			}
		}
	}
}
