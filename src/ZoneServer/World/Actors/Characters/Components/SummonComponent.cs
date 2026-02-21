using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.World.Actors.Characters.Components
{
	/// <summary>
	/// A component to handle Summoned Monsters
	/// </summary>
	public class SummonComponent : CharacterComponent
	{
		private readonly List<Summon> _summons = new();
		public SummonComponent(Character character) : base(character)
		{
		}

		public List<Summon> GetSummons()
		{
			lock (_summons)
				return _summons.ToList();
		}

		/// <summary>
		/// Gets all the summons that match given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		public List<Summon> GetSummons(Func<Summon, bool> predicate)
		{
			lock (_summons)
				return _summons.Where(predicate).ToList();
		}

		public List<Summon> GetSummons(params int[] monsterIds)
		{
			lock (_summons)
				return _summons.Where(monster => monsterIds.Contains(monster.Id)).ToList();
		}

		public bool TryGetSummon(int handle, out Summon summon)
		{
			lock (_summons)
				summon = _summons.First(s => s.Handle == handle);
			return summon != null;
		}

		/// <summary>
		/// Add a summoned monster and update client summoned monster list.
		/// </summary>
		/// <param name="summon"></param>
		public void AddSummon(Summon summon)
		{
			lock (_summons)
			{
				summon.Died += this.Summon_Died;
				_summons.Add(summon);
			}
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, string.Format("UPDATE_PC_FOLLOWER_LIST(\"{0}\")", this.ToString()));
		}

		/// <summary>
		/// Remove a summoned monster and update client summoned monster list.
		/// </summary>
		/// <param name="summon"></param>
		public void RemoveSummon(Summon summon)
		{
			lock (_summons)
			{
				summon.Died -= this.Summon_Died;
				_summons.Remove(summon);
			}
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, string.Format("UPDATE_PC_FOLLOWER_LIST(\"{0}\")", this.ToString()));
		}

		/// <summary>
		/// Remove a summoned monster by id and update client summoned monster list.
		/// </summary>
		/// <param name="monsterIds"></param>
		public void RemoveSummon(params int[] monsterIds)
		{
			lock (_summons)
			{
				var toRemove = _summons.Where(monster => monsterIds.Contains(monster.Id)).ToList();
				foreach (var summon in toRemove)
					summon.Died -= this.Summon_Died;
				_summons.RemoveAll(monster => monsterIds.Contains(monster.Id));
			}
			Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, string.Format("UPDATE_PC_FOLLOWER_LIST(\"{0}\")", this.ToString()));
		}

		/// <summary>
		/// Returns a total summon count.
		/// </summary>
		/// <returns></returns>
		public int Count { get { lock (_summons) return _summons.Count; } }

		/// <summary>
		/// Returns a count of a specific monster.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <returns></returns>
		public int CountSummonById(int monsterId)
		{
			lock (_summons)
				return _summons.Count(monster => monster.Id == monsterId);
		}

		/// <summary>
		/// Returns if a position is valid by comparing positions of nearby summoned monsters.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public bool IsValidPosition(Position position, float range = 10)
		{
			lock (_summons)
				return !_summons.Exists(monster => monster.Position.InRange2D(position, range));
		}

		/// <summary>
		/// Removes all summons by killing them and removing them from the map.
		/// This should be called when the player logs out to prevent orphaned summons.
		/// </summary>
		public void RemoveAllSummons()
		{
			List<Summon> summonsToRemove;
			lock (_summons)
				summonsToRemove = _summons.ToList();

			foreach (var summon in summonsToRemove)
			{
				summon.Map?.RemoveMonster(summon);
			}

			lock (_summons)
				_summons.Clear();

			if (this.Character.Connection != null)
				Send.ZC_EXEC_CLIENT_SCP(this.Character.Connection, string.Format("UPDATE_PC_FOLLOWER_LIST(\"{0}\")",this.ToString()));
		}

		/// <summary>
		/// Handle behavior for monster dying.
		/// </summary>
		/// <param name="mob"></param>
		/// <param name="entity"></param>
		private void Summon_Died(Mob mob, ICombatEntity entity)
		{
			if (mob is Summon summon)
			{
				summon.Components.Remove<AiComponent>();
				summon.Components.Remove<MovementComponent>();
				if (summon.HasBuffs())
				{
					summon.Buffs.RemoveAll();
					summon.Components.Remove<BuffComponent>();
				}
				this.RemoveSummon(summon);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var resultString = new StringBuilder();
			lock (_summons)
			{
				foreach (var monsterId in _summons.GroupBy(m => m.Id).Select(x => x.Key))
				{
					var count = this.CountSummonById(monsterId);
					resultString.Append($"{monsterId}:{count}#");
				}
			}
			return resultString.ToString();
		}
	}
}
