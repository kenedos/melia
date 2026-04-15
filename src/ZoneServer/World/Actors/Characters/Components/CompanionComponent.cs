using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.World.Actors.Characters.Components
{
	public class CompanionComponent : CharacterComponent
	{
		private readonly List<Companion> _companions = new();

		public CompanionComponent(Character character) : base(character)
		{
		}

		public bool HasCompanions
		{
			get
			{
				lock (_companions)
					return _companions.Count > 0;
			}
		}

		public Companion ActiveCompanion
		{
			get
			{
				lock (_companions)
					return _companions?.Find(companion => companion.IsActivated);
			}
		}

		/// <summary>
		/// Returns the active ground companion (non-bird), or null.
		/// </summary>
		public Companion ActiveGroundCompanion
		{
			get
			{
				lock (_companions)
					return _companions?.Find(companion => companion.IsActivated && !companion.IsBird);
			}
		}

		/// <summary>
		/// Returns the active bird companion (hawk), or null.
		/// </summary>
		public Companion ActiveBirdCompanion
		{
			get
			{
				lock (_companions)
					return _companions?.Find(companion => companion.IsActivated && companion.IsBird);
			}
		}

		/// <summary>
		/// Returns all currently activated companions.
		/// </summary>
		public IList<Companion> GetActiveCompanions()
		{
			lock (_companions)
			{
				var result = new List<Companion>();
				foreach (var companion in _companions)
				{
					if (companion.IsActivated)
						result.Add(companion);
				}
				return result;
			}
		}

		/// Add companion
		/// </summary>
		/// <param name="companion"></param>
		/// <param name="silently"></param>
		public void AddCompanion(Companion companion, bool silently = false)
		{
			lock (_companions)
				this._companions.Add(companion);
			if (!silently)
				Send.ZC_NORMAL.PetInfo(this.Character);
		}

		/// <summary>
		/// Adds companion to character and the database.
		/// </summary>
		/// <param name="companion"></param>
		public void CreateCompanion(Companion companion)
		{
			ZoneServer.Instance.Database.CreateCompanion(this.Character.AccountDbId, this.Character.DbId, companion);
			this.AddCompanion(companion);
		}

		/// <summary>
		/// Get Companions
		/// </summary>
		/// <returns></returns>
		public IList<Companion> GetList()
		{
			lock (_companions)
				return _companions;
		}

		/// <summary>
		/// Get Companions
		/// </summary>
		/// <returns></returns>
		public IList<Companion> GetList(Func<Companion, bool> predicate)
		{
			lock (_companions)
				return _companions.Where(predicate).ToList();
		}

		/// <summary>
		/// Returns companion or null with a given id.
		/// </summary>
		/// <param name="companionId"></param>
		/// <returns></returns>
		public Companion GetCompanion(long companionId)
		{
			lock (_companions)
				return _companions.Find(c => c.ObjectId == companionId);
		}

		/// <summary>
		/// Returns companion or null with a given id.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="companion"></param>
		/// <returns></returns>
		public bool TryGetCompanion(Predicate<Companion> predicate, out Companion companion)
		{
			lock (_companions)
				companion = _companions.Find(predicate);
			return companion != null;
		}
	}
}
