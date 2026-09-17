using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Tracks
{
	/// <summary>
	/// The state a party shares while one of its track plays: the private
	/// layer, the cutscene cast, and the members watching it.
	/// </summary>
	public class TrackGroup
	{
		internal readonly static object SyncLock = new object();

		/// <summary>
		/// Serialises party track starts. Held across the cast spawn, which
		/// takes the starters' quest locks, so it must not be SyncLock.
		/// </summary>
		internal readonly static object StartLock = new object();

		private readonly static Dictionary<string, TrackGroup> Groups = new Dictionary<string, TrackGroup>();

		private readonly List<Character> _members = new List<Character>();
		private readonly string _key;

		/// <summary>
		/// Returns the object id of the party the group belongs to.
		/// </summary>
		public long PartyId { get; }

		/// <summary>
		/// Returns the id of the quest the track is bound to.
		/// </summary>
		public int QuestId { get; }

		/// <summary>
		/// Returns the track's id.
		/// </summary>
		public string TrackId { get; }

		/// <summary>
		/// Returns the property the track records itself under.
		/// </summary>
		public string PropertyId { get; }

		/// <summary>
		/// Returns the id of the map the group runs on.
		/// </summary>
		public int MapId { get; }

		/// <summary>
		/// Returns the layer the group's cast lives on.
		/// </summary>
		public int Layer { get; }

		/// <summary>
		/// Returns the character that currently owns the group and drives
		/// its shared commands.
		/// </summary>
		public Character Owner { get; private set; }

		/// <summary>
		/// Returns the group's cutscene cast.
		/// </summary>
		public IActor[] Actors { get; }

		/// <summary>
		/// Gets or sets whether the track created a battle box, which keeps
		/// the group running into the fight for every member.
		/// </summary>
		public bool HasBattleBoxInLayer { get; set; }

		/// <summary>
		/// Returns whether every member has left the group.
		/// </summary>
		public bool Ended { get; private set; }

		/// <summary>
		/// Returns a snapshot of the characters currently in the group.
		/// </summary>
		public IReadOnlyList<Character> Members
		{
			get { lock (SyncLock) return _members.ToArray(); }
		}

		private TrackGroup(long partyId, int questId, string trackId, string propertyId, int mapId, int layer, IActor[] actors, Character owner)
		{
			this.PartyId = partyId;
			this.QuestId = questId;
			this.TrackId = trackId;
			this.PropertyId = propertyId;
			this.MapId = mapId;
			this.Layer = layer;
			this.Actors = actors ?? Array.Empty<IActor>();
			this.Owner = owner;
			_key = MakeKey(partyId, questId, trackId, mapId);
		}

		private static string MakeKey(long partyId, int questId, string trackId, int mapId)
			=> partyId + ":" + questId + ":" + trackId + ":" + mapId;

		/// <summary>
		/// Returns the running group for the party's quest track, or null.
		/// Caller holds SyncLock.
		/// </summary>
		/// <param name="partyId"></param>
		/// <param name="questId"></param>
		/// <param name="trackId"></param>
		/// <param name="mapId"></param>
		/// <returns></returns>
		public static TrackGroup FindLocked(long partyId, int questId, string trackId, int mapId)
		{
			if (!Groups.TryGetValue(MakeKey(partyId, questId, trackId, mapId), out var group))
				return null;

			if (group.Ended || group.MapId != mapId)
				return null;

			return group;
		}

		/// <summary>
		/// Returns the running group for the party's quest track, or null.
		/// </summary>
		/// <param name="partyId"></param>
		/// <param name="questId"></param>
		/// <param name="trackId"></param>
		/// <param name="mapId"></param>
		/// <returns></returns>
		public static TrackGroup Find(long partyId, int questId, string trackId, int mapId)
		{
			lock (SyncLock)
				return FindLocked(partyId, questId, trackId, mapId);
		}

		/// <summary>
		/// Registers a new group and adds its owner. Caller holds SyncLock.
		/// </summary>
		/// <param name="partyId"></param>
		/// <param name="questId"></param>
		/// <param name="trackId"></param>
		/// <param name="propertyId"></param>
		/// <param name="mapId"></param>
		/// <param name="layer"></param>
		/// <param name="actors"></param>
		/// <param name="owner"></param>
		/// <returns></returns>
		public static TrackGroup CreateLocked(long partyId, int questId, string trackId, string propertyId, int mapId, int layer, IActor[] actors, Character owner)
		{
			var group = new TrackGroup(partyId, questId, trackId, propertyId, mapId, layer, actors, owner);
			group._members.Add(owner);
			Groups[group._key] = group;

			return group;
		}

		/// <summary>
		/// Adds a character to the group, returning false if the group
		/// already ended.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool TryJoin(Character character)
		{
			lock (SyncLock)
			{
				if (this.Ended)
					return false;

				if (!_members.Contains(character))
					_members.Add(character);

				return true;
			}
		}

		/// <summary>
		/// Removes a character from the group, returning true when it was
		/// the last member and the group has been unregistered.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool Leave(Character character)
		{
			lock (SyncLock)
			{
				_members.Remove(character);

				if (this.Owner == character)
					this.ReassignOwnerLocked();

				if (_members.Count == 0)
				{
					this.Ended = true;

					if (Groups.TryGetValue(_key, out var current) && current == this)
						Groups.Remove(_key);

					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// Hands the group to a remaining member, so its cast and layer
		/// keep a driver when the owner leaves. Caller holds SyncLock.
		/// </summary>
		private void ReassignOwnerLocked()
		{
			var newOwner = _members.FirstOrDefault(a => a != this.Owner && a.IsOnline)
				?? _members.FirstOrDefault(a => a != this.Owner);

			if (newOwner == null)
				return;

			var oldOwner = this.Owner;
			this.Owner = newOwner;

			// The cast carries the owner's character in the player slot, so
			// later members still stage themselves after a hand-over.
			var actors = this.Actors;
			for (var i = 0; i < actors.Length; i++)
			{
				if (actors[i] == oldOwner)
					actors[i] = newOwner;
			}

			foreach (var member in _members)
			{
				var track = member.Tracks?.ActiveTrack;
				if (track != null && track.Group == this)
					track.Owner = newOwner;
			}
		}

		/// <summary>
		/// Returns the cast with the owner's character slot swapped for the
		/// given member, so each client stages itself as the player.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public IActor[] BuildActorsFor(Character member)
		{
			var actors = this.Actors;
			var result = new IActor[actors.Length];

			for (var i = 0; i < actors.Length; i++)
				result[i] = actors[i] == this.Owner ? member : actors[i];

			return result;
		}
	}
}
