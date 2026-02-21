using System;
using System.Collections.Generic;

namespace Melia.Zone.Util
{
	public class WeightedRandom<T>
	{
		private readonly List<(T item, int weight)> _items = new List<(T, int)>();
		private int _totalWeight = 0;
		private static readonly Random _random = new Random();

		public void Add(T item, int weight)
		{
			if (weight <= 0) return;
			_items.Add((item, weight));
			_totalWeight += weight;
		}

		public T GetRandomItem()
		{
			if (_totalWeight == 0) return default(T);

			int randomNumber = _random.Next(1, _totalWeight + 1);
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
