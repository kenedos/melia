using Melia.Shared.World;

namespace Melia.Zone.World.Patrols
{
	/// <summary>
	/// An ordered list of positions a monster walks back and forth
	/// between.
	/// </summary>
	public class PatrolRoute
	{
		private readonly Position[] _nodes;

		/// <summary>
		/// Returns the amount of nodes on the route.
		/// </summary>
		public int Count => _nodes.Length;

		/// <summary>
		/// Creates a new patrol route.
		/// </summary>
		/// <param name="nodes"></param>
		public PatrolRoute(Position[] nodes)
		{
			_nodes = nodes;
		}

		/// <summary>
		/// Returns the position of the node with the given index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Position GetNode(int index)
		{
			if (index < 0)
				index = 0;
			else if (index >= _nodes.Length)
				index = _nodes.Length - 1;

			return _nodes[index];
		}

		/// <summary>
		/// Returns the index of the node closest to the given position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public int GetNearestNodeIndex(Position position)
		{
			var nearestIndex = 0;
			var nearestDistance = double.MaxValue;

			for (var i = 0; i < _nodes.Length; ++i)
			{
				var distance = _nodes[i].Get2DDistance(position);
				if (distance < nearestDistance)
				{
					nearestDistance = distance;
					nearestIndex = i;
				}
			}

			return nearestIndex;
		}

		/// <summary>
		/// Returns the position of the node closest to the given position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Position GetNearestNode(Position position)
			=> _nodes[this.GetNearestNodeIndex(position)];
	}
}
