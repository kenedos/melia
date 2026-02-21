using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// A monster that represents a warp to another map.
	/// </summary>
	public class WarpMonster : Npc
	{
		/// <summary>
		/// The monster class id of a green arrow representing a warp.
		/// </summary>
		public const int WarpMonsterClassId = 40001;

		private short _useCount;

		/// <summary>
		/// Returns the warp's name that can be used to identify it.
		/// </summary>
		public string WarpName { get; }

		/// <summary>
		/// Returns the location of the warp.
		/// </summary>
		public Location SourceLocation { get; }

		/// <summary>
		/// Returns the destination of the warp.
		/// </summary>
		public Location WarpLocation { get; }

		/// <summary>
		/// Returns the max use count for a warp.
		/// </summary>
		public short MaxUseCount { get; set; } = short.MaxValue;

		/// <summary>
		/// Automatically warp players when near the warp.
		/// </summary>
		public bool WarpWhenNearby { get; set; } = true;


		public int DestinationMapId => this.WarpLocation.MapId;

		/// <summary>
		/// Destination map name.
		/// </summary>
		public string DestinationMapName => GetMapClassName(this.WarpLocation.MapId);

		/// <summary>
		/// Creates new warp monster.
		/// </summary>
		/// <param name="warpName"></param>
		/// <param name="targetLocationName"></param>
		/// <param name="sourceLocation"></param>
		/// <param name="targetLocation"></param>
		/// <param name="direction"></param>
		public WarpMonster(string warpName, string targetLocationName, Location sourceLocation, Location targetLocation, Direction direction)
			: base(WarpMonsterClassId, warpName, sourceLocation, direction)
		{
			this.WarpName = warpName;
			this.Name = targetLocationName;
			this.Position = sourceLocation.Position;
			this.Direction = direction;
			this.SourceLocation = sourceLocation;
			this.WarpLocation = targetLocation;

			this.SetupWarpTrigger();
		}

		public WarpMonster(int genType, string warpName, string targetLocationName, Location sourceLocation, Location targetLocation, Direction direction)
			: base(WarpMonsterClassId, warpName, sourceLocation, direction, genType)
		{
			this.WarpName = warpName;
			this.Name = targetLocationName;
			this.Position = sourceLocation.Position;
			this.Direction = direction;
			this.SourceLocation = sourceLocation;
			this.WarpLocation = targetLocation;
			this.State = NpcState.Highlighted;

			this.SetupWarpTrigger();
		}

		public WarpMonster(int monsterId, Location sourceLocation, Location targetLocation, Direction direction)
			: base(monsterId, "", sourceLocation, direction)
		{
			this.Position = sourceLocation.Position;
			this.Direction = direction;
			this.SourceLocation = sourceLocation;
			this.WarpLocation = targetLocation;

			this.SetupWarpTrigger();
		}

		public WarpMonster(int monsterId, Location sourceLocation, Location targetLocation, Direction direction, bool warpWhenNearby)
			: base(monsterId, "", sourceLocation, direction)
		{
			this.Position = sourceLocation.Position;
			this.Direction = direction;
			this.SourceLocation = sourceLocation;
			this.WarpLocation = targetLocation;
			this.WarpWhenNearby = warpWhenNearby;

			this.SetupWarpTrigger();
		}

		/// <summary>
		/// Sets up the click trigger handler for warping.
		/// </summary>
		private void SetupWarpTrigger()
		{
			this.SetClickTrigger("DYNAMIC_DIALOG", async dialog =>
			{
				// Skip if player is dead
				if (dialog.Player.IsDead)
					return;

				if (dialog.Player.IsOutOfBody())
					return;

				// Not making Player.CanMove() check, so we're
				// allowing players to run away from map when they're
				// frozen, stunned, knocked down, etc.

				if (dialog.Npc is WarpMonster warpMonster)
				{
					dialog.Player.Warp(warpMonster.WarpLocation);
				}

				await Task.Yield();
			});
		}

		/// <summary>
		/// Since the warps don't use the trigger system, we'll replicate 
		/// this behavior. Returns true if the max use count was reached.
		/// If no max use count (default <see cref="short.MaxValue"/>)
		/// is defined we return false.
		/// </summary>
		public bool IncreaseUseCount()
		{
			if (this.MaxUseCount == short.MaxValue)
				return false;
			_useCount++;

			var usedUp = _useCount >= this.MaxUseCount;
			if (usedUp)
				this.Map.RemoveMonster(this);

			return usedUp;
		}
	}
}
