using System.Collections.Generic;
using Yggdrasil.Util;

namespace Melia.Zone.Util
{
	public class WeightedRandom<T>
	{
		private readonly List<(T item, int weight)> _items = new List<(T, int)>();
		private int _totalWeight = 0;

		public void Add(T item, int weight)
		{
			if (weight <= 0) return;
			_items.Add((item, weight));
			_totalWeight += weight;
		}

		public T GetRandomItem()
		{
			if (_totalWeight == 0) return default(T);

			int randomNumber = RandomProvider.Get().Next(1, _totalWeight + 1);
			int cumulativeWeight = 0;

			foreach (var (item, weight) in _items)
			{
				cumulativeWeight += weight;
				if (randomNumber <= cumulativeWeight)
				{
					return item;
				}
			}
			return default(T); // Should not be reached
		}
	}
}
