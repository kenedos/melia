using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Prerequisites;
using Melia.Zone.World.Maps;
using Yggdrasil.Composition;

namespace Melia.Zone.World.Actors
{
	/// <summary>
	/// An object that can be placed on a map.
	/// </summary>
	public interface IActor
	{
		/// <summary>
		/// Returns the actor's unique handle.
		/// </summary>
		int Handle { get; }

		/// <summary>
		/// Returns the actor's display name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Returns the map the actor is currently on.
		/// </summary>
		Map Map { get; set; }

		/// <summary>
		/// Returns the actor's position on its current map.
		/// </summary>
		Position Position { get; set; }

		/// <summary>
		/// Returns the direction the actor is facing.
		/// </summary>
		Direction Direction { get; set; }

		/// <summary>
		/// Returns the actor's faction.
		/// </summary>
		FactionType Faction { get; }

		/// <summary>
		/// Returns the actor's layer.
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// Returns the actor's visibility.
		/// </summary>
		public ActorVisibility Visibility { get; set; }

		/// <summary>
		/// Returns the components the actor has.
		/// </summary>
		public ComponentCollection Components { get; }

		/// <summary>
		/// Returns true if an actor is visible.
		/// </summary>
		/// <param name="actor"></param>
		/// <returns></returns>
		bool IsVisible(IActor actor);

		/// <summary>
		/// Gets or sets the time the actor will be removed from the
		/// map it's on.
		/// </summary>
		DateTime DisappearTime { get; }

		/// <summary>
		/// Raised when the actor is removed from the map it's on.
		/// </summary>
		Action OnDisappear { get; }
	}

	/// <summary>
	/// An actor that can be owned or associated with.
	/// </summary>
	public interface ISubActor : IActor
	{
		int OwnerHandle { get; }
		int AssociatedHandle { get; }
	}

	// Removed: IGuildMember interface referenced Guild type deleted during Laima merge
	// public interface IGuildMember : IActor { Guild Guild { get; } }
	public interface IGuildMember : IActor
	{
	}

	/// <summary>
	/// Represents a spawnable
	/// </summary>
	public interface ISpawn
	{
		/// <summary>
		/// Spawn position
		/// </summary>
		/// <remarks>
		/// Changed from Location to Position - 7/21/24
		/// </remarks>
		Position SpawnPosition { get; set; }
	}

	/// <summary>
	/// An object that can be placed on a map.
	/// </summary>
	public abstract class Actor : ISubActor
	{
		/// <summary>
		/// Returns the actor's unique handle.
		/// </summary>
		public virtual int Handle { get; } = ZoneServer.Instance.World.CreateHandle();

		/// <summary>
		/// Returns the actor's display name.
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		/// Returns the map the actor is currently on.
		/// </summary>
		public Map Map
		{
			get => _map;
			set => _map = value ?? Map.Limbo;
		}
		private Map _map = Map.Limbo;

		/// <summary>
		/// Returns the actor's position on its current map.
		/// </summary>
		public virtual Position Position { get; set; }

		/// <summary>
		/// Returns the direction the actor is facing.
		/// </summary>
		public Direction Direction { get; set; }

		/// <summary>
		/// Returns the components the actor has.
		/// </summary>
		public ComponentCollection Components { get; set; } = new ComponentCollection();

		/// <summary>
		/// Returns the actor's visibility.
		/// </summary>
		public ActorVisibility Visibility { get; set; } = ActorVisibility.Everyone;

		/// <summary>
		/// Returns a specific visibility id (Character, Party)
		/// </summary>
		public long VisibilityId { get; set; }

		/// <summary>
		/// Returns a list of the visibility prerequisites, that need to be
		/// met to for the entity to be visible.
		/// </summary>
		public List<Prerequisite> VisibilityPrerequisites { get; } = new List<Prerequisite>();

		/// <summary>
		/// Returns the layer on which this actor exists.
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// Returns the time when the actor is removed from the map.
		/// </summary>
		public DateTime DisappearTime { get; set; } = DateTime.MaxValue;

		/// <summary>
		/// Raised when the actor is removed from the map it's on.
		/// </summary>
		public Action OnDisappear { get; set; }

		/// <summary>
		/// Actor's Faction
		/// </summary>
		public FactionType Faction { get; set; } = FactionType.Peaceful;

		/// <summary>
		/// Actor's trigger component
		/// </summary>
		public TriggerComponent Trigger { get; set; }

		/// <summary>
		/// Actor's owner handle
		/// </summary>
		public int OwnerHandle { get; set; }

		/// <summary>
		/// Actor's associated handle
		/// </summary>
		public int AssociatedHandle { get; set; }

		/// <summary>
		/// Set a specific visibilty limiter.
		/// </summary>
		/// <param name="visibility"></param>
		/// <param name="id"></param>
		public void SetVisibilty(ActorVisibility visibility, long id)
		{
			this.Visibility = visibility;
			this.VisibilityId = id;
		}

		/// <summary>
		/// Attaches an effect to the actor that is displayed alongside it.
		/// </summary>
		/// <param name="packetString"></param>
		/// <param name="scale"></param>
		public void AttachEffect(string packetString, float scale = 1, EffectLocation heightOffset = EffectLocation.Bottom, float offsetX = 0, float offsetY = 0, float offetsetZ = 0)
		{
			this.Components ??= new ComponentCollection();
			if (!this.Components.Has<EffectsComponent>())
				this.Components.Add(new EffectsComponent(this));
			var effect = new AttachEffect(packetString, scale, heightOffset);
			if (this.Components.TryGet<EffectsComponent>(out var effectsComponent))
				effectsComponent.AddEffect(packetString, effect);

			if (this.Map != Map.Limbo)
				Send.ZC_NORMAL.AttachEffect(this, effect.EffectName, effect.Scale, heightOffset, offsetX, offsetY, offetsetZ);
		}

		/// <summary>
		/// Attaches an effect to the actor that is displayed alongside it.
		/// </summary>
		/// <param name="packetString"></param>
		public void DetachEffect(string packetString)
		{
			if (this.Components.TryGet<EffectsComponent>(out var effectsComponent))
				effectsComponent.RemoveEffect(packetString);
			if (this.Map != Map.Limbo)
				Send.ZC_NORMAL.DetachEffect(this, packetString);
		}

		/// <summary>
		/// Add an effect to an actor.
		/// </summary>
		/// <param name="effect"></param>
		public void AddEffect(Effect effect)
		{
			this.Components ??= new ComponentCollection();
			if (!this.Components.Has<EffectsComponent>())
				this.Components.Add(new EffectsComponent(this));
			this.Components.Get<EffectsComponent>()?.AddEffect(effect);
		}

		public void SetFaction(FactionType factionType, bool silently = false)
		{
			this.Faction = factionType;
			if (!silently)
				Send.ZC_FACTION(this);
		}

		public virtual bool CanSee(IActor actor)
		{
			if (actor == null)
				return false;
			if (!this.Position.InRange2D(actor.Position, Map.VisibleRange))
				return false;
			if (!actor.IsVisible(this))
				return false;
			return true;
		}

		/// <summary>
		/// Adds prerequisite to the entity.
		/// </summary>
		/// <param name="prerequisites"></param>
		public void AddVisibilityPrerequisite(params Prerequisite[] prerequisites)
		{
			foreach (var prerequisite in prerequisites)
				this.VisibilityPrerequisites.Add(prerequisite);
		}

		/// <summary>
		/// Plays effect for the character.
		/// </summary>
		/// <param name="packetString"></param>
		public void PlayEffect(string packetString, float scale = 1, byte b1 = 1, EffectLocation heightOffset = EffectLocation.Bottom, byte b2 = 0, int associatedHandle = 0)
		{
			Send.ZC_NORMAL.PlayEffect(this, b1, heightOffset, b2, scale, packetString, 0, associatedHandle);
		}

		/// <summary>
		/// Plays effect for the character.
		/// </summary>
		/// <param name="packetString"></param>
		public void PlayEffectLocal(IZoneConnection conn, string packetString, float scale = 1, EffectLocation heightOffset = EffectLocation.Bottom, byte b1 = 0)
		{
			Send.ZC_NORMAL.PlayEffect(conn, this, b1, heightOffset, 1, scale, packetString, 0, 0);
		}

		public bool IsVisible(IActor actor)
		{
			for (var i = 0; i < this.VisibilityPrerequisites.Count; ++i)
			{
				var prerequisite = this.VisibilityPrerequisites[i];
				if (!prerequisite.Met(actor))
					return false;
			}

			if (this.Layer != actor.Layer)
				return false;

			if (actor is Character character)
			{
				switch (actor.Visibility)
				{
					case ActorVisibility.NoOne:
						return false;
					case ActorVisibility.Individual:
						if (this.VisibilityId == character.ObjectId)
							return true;
						break;
					case ActorVisibility.Party:
						if (character.Connection.Party != null)
							return this.VisibilityId == character.Connection.Party.ObjectId;
						break;
					case ActorVisibility.Track:
						if (character.Tracks != null && character.Tracks.ActiveTrack != null)
							return this.VisibilityId == character.ObjectId;
						break;
				}
			}
			return true;
		}


		public void Broadcast(Packet packet)
		{
			this.Map?.Broadcast(packet, this);
		}
	}

	/// <summary>
	/// Defines an actor's visibility.
	/// </summary>
	public enum ActorVisibility
	{
		NoOne,
		Individual,
		Party,
		Track,
		Everyone,
		Always,
	}
}
