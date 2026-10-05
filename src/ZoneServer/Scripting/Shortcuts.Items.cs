using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.Util;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Scripting
{
	public static partial class Shortcuts
	{
		// A helper to create and use the weighted random selector
		public static void GiveWeightedRandomItem(Character character, List<(string itemName, int weight)> itemData, string reason)
		{
			var selector = new WeightedRandom<string>();
			foreach (var (itemName, weight) in itemData)
			{
				selector.Add(itemName, weight);
			}
			var selectedItem = selector.GetRandomItem();
			if (!string.IsNullOrEmpty(selectedItem))
			{
				character.AddItem(selectedItem, 1, reason);
			}
		}

		// A helper for unweighted (uniform) random selection
		public static void GiveUniformRandomItem(Character character, List<string> items, string reason)
		{
			if (items == null || items.Count == 0) return;
			var random = new Random();
			var selectedItem = items[random.Next(items.Count)];
			character.AddItem(selectedItem, 1, reason);
		}
	}
}
