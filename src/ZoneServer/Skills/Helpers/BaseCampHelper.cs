using System;
using System.Collections.Generic;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// One Squire's Base Camp, standing on its map until it expires.
	/// </summary>
	public class BaseCamp
	{
		/// <summary>
		/// Returns the object the camp stands on in the world.
		/// </summary>
		public Npc Npc { get; }

		/// <summary>
		/// Returns the object id of the character that built the camp.
		/// </summary>
		public long OwnerObjectId { get; }

		/// <summary>
		/// Returns the name of the character that built the camp.
		/// </summary>
		public string OwnerName { get; }

		/// <summary>
		/// Returns the account the camp is counted against.
		/// </summary>
		public long OwnerAccountId { get; }

		/// <summary>
		/// Returns the level of the skill the camp was built with.
		/// </summary>
		public int SkillLevel { get; }

		/// <summary>
		/// Returns the id of the map the camp stands on.
		/// </summary>
		public int MapId { get; }

		/// <summary>
		/// Gets or sets when the camp comes back down.
		/// </summary>
		public DateTime ExpirationTime
		{
			get => this.Npc.DisappearTime;
			set => this.Npc.DisappearTime = value;
		}

		/// <summary>
		/// Creates a camp standing on the given object.
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="owner"></param>
		/// <param name="skillLevel"></param>
		public BaseCamp(Npc npc, Character owner, int skillLevel)
		{
			this.Npc = npc;
			this.OwnerObjectId = owner.ObjectId;
			this.OwnerName = owner.Name;
			this.OwnerAccountId = owner.Connection?.Account?.Id ?? 0;
			this.SkillLevel = skillLevel;
			this.MapId = owner.MapId;
		}
	}

	/// <summary>
	/// Keeps track of the Base Camps standing in the world.
	/// </summary>
	/// <remarks>
	/// A camp outlives its owner's session, so it is registered here rather
	/// than on their connection, and party members reach it through this
	/// registry while its owner is offline.
	/// </remarks>
	public static class BaseCampHelper
	{
		private static readonly Dictionary<long, BaseCamp> Camps = new();

		/// <summary>
		/// How far in front of the camp a traveller arrives.
		/// </summary>
		private const int ArrivalDistance = 40;

		/// <summary>
		/// Registers the given camp as its owner's one camp, taking down
		/// the one they had standing.
		/// </summary>
		/// <param name="camp"></param>
		public static void Register(BaseCamp camp)
		{
			BaseCamp previous;

			lock (Camps)
			{
				Camps.TryGetValue(camp.OwnerObjectId, out previous);
				Camps[camp.OwnerObjectId] = camp;
			}

			if (previous != null)
				TakeDown(previous);
		}

		/// <summary>
		/// Returns the camp the character with the given object id has
		/// standing, if any.
		/// </summary>
		/// <param name="ownerObjectId"></param>
		/// <param name="camp"></param>
		public static bool TryGet(long ownerObjectId, out BaseCamp camp)
		{
			lock (Camps)
			{
				if (!Camps.TryGetValue(ownerObjectId, out camp))
					return false;

				if (IsStanding(camp))
					return true;

				Camps.Remove(ownerObjectId);
			}

			camp = null;
			return false;
		}

		/// <summary>
		/// Returns the camp the given character has standing, if any.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="camp"></param>
		public static bool TryGet(Character owner, out BaseCamp camp)
			=> TryGet(owner.ObjectId, out camp);

		/// <summary>
		/// Returns the camp the object with the given handle stands on, if
		/// it's a camp that's still up.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="camp"></param>
		public static bool TryGetByHandle(int handle, out BaseCamp camp)
		{
			var owners = new List<long>();

			lock (Camps)
			{
				foreach (var registered in Camps.Values)
				{
					if (registered.Npc.Handle == handle)
						owners.Add(registered.OwnerObjectId);
				}
			}

			foreach (var ownerObjectId in owners)
			{
				if (TryGet(ownerObjectId, out camp))
					return true;
			}

			camp = null;
			return false;
		}

		/// <summary>
		/// Returns the camp with the given handle out of the ones the
		/// character can reach, if it's one of them.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="handle"></param>
		/// <param name="camp"></param>
		public static bool TryGetReachable(Character character, int handle, out BaseCamp camp)
		{
			foreach (var reachable in GetReachableCamps(character))
			{
				if (reachable.Npc.Handle != handle)
					continue;

				camp = reachable;
				return true;
			}

			camp = null;
			return false;
		}

		/// <summary>
		/// Takes the given camp out of the world and forgets it.
		/// </summary>
		/// <param name="camp"></param>
		public static void Remove(BaseCamp camp)
		{
			lock (Camps)
			{
				if (Camps.TryGetValue(camp.OwnerObjectId, out var registered) && registered == camp)
					Camps.Remove(camp.OwnerObjectId);
			}

			TakeDown(camp);
		}

		/// <summary>
		/// Returns every camp the given character may travel to, their own
		/// first and their party's after it.
		/// </summary>
		/// <param name="character"></param>
		public static List<BaseCamp> GetReachableCamps(Character character)
		{
			var camps = new List<BaseCamp>();

			if (TryGet(character, out var ownCamp))
				camps.Add(ownCamp);

			var party = character.Connection?.Party;
			if (party == null)
				return camps;

			foreach (var member in party.GetMembers())
			{
				if (member.ObjectId == character.ObjectId)
					continue;

				if (TryGet(member.ObjectId, out var memberCamp))
					camps.Add(memberCamp);
			}

			return camps;
		}

		/// <summary>
		/// Sends the character to the camp with the given handle, and
		/// returns whether they were sent.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="handle"></param>
		public static bool TryWarpTo(Character character, int handle)
		{
			if (!TryGetReachable(character, handle, out var camp))
				return false;

			var arrival = camp.Npc.Position.GetRelative(camp.Npc.Direction, ArrivalDistance);

			character.SetDirection(-camp.Npc.Direction);
			character.Warp(camp.MapId, arrival);
			character.ServerMessage(Localization.Get("Travelled to {0}'s Base Camp in {1}."), camp.OwnerName, GetMapName(camp));

			return true;
		}

		/// <summary>
		/// Returns the name of the map the camp stands on.
		/// </summary>
		/// <param name="camp"></param>
		public static string GetMapName(BaseCamp camp)
		{
			if (ZoneServer.Instance.Data.MapDb.TryFind(camp.MapId, out var mapData))
				return mapData.Name;

			return Localization.Get("Unknown");
		}

		/// <summary>
		/// Returns how long the camp still stands for.
		/// </summary>
		/// <param name="camp"></param>
		public static TimeSpan GetRemainingTime(BaseCamp camp)
		{
			var remaining = camp.ExpirationTime - DateTime.Now;

			return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
		}

		/// <summary>
		/// Returns whether the given camp is still up.
		/// </summary>
		/// <param name="camp"></param>
		private static bool IsStanding(BaseCamp camp)
			=> camp.Npc.Map != null && camp.Npc.Map != Map.Limbo && DateTime.Now < camp.ExpirationTime;

		/// <summary>
		/// Takes the camp out of the world and tells its owner it's gone.
		/// </summary>
		/// <param name="camp"></param>
		private static void TakeDown(BaseCamp camp)
		{
			camp.Npc.Map?.RemoveMonster(camp.Npc);

			var owner = ZoneServer.Instance.World.GetCharacter(camp.OwnerObjectId);
			if (owner?.Connection != null)
				Send.ZC_CAMPINFO(owner.Connection, camp.OwnerAccountId);
		}
	}
}
