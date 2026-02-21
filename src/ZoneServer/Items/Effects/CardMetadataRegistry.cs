using System.Collections.Generic;

namespace Melia.Zone.Items.Effects
{
	/// <summary>
	/// Stores metadata for card items that doesn't fit in the item's Properties.
	/// Maps item ObjectId to its metadata.
	/// </summary>
	public class CardMetadataRegistry
	{
		private static CardMetadataRegistry _instance;
		public static CardMetadataRegistry Instance => _instance ?? (_instance = new CardMetadataRegistry());

		private readonly Dictionary<long, CardMetadata> _metadata = new();

		public void Set(long itemObjectId, CardMetadata metadata)
		{
			lock (_metadata)
				_metadata[itemObjectId] = metadata;
		}

		public bool TryGet(long itemObjectId, out CardMetadata metadata)
		{
			lock (_metadata)
				return _metadata.TryGetValue(itemObjectId, out metadata);
		}

		public void Remove(long itemObjectId)
		{
			lock (_metadata)
				_metadata.Remove(itemObjectId);
		}
	}

	/// <summary>
	/// Metadata for a card item.
	/// </summary>
	public class CardMetadata
	{
		public string ConditionFunction { get; set; }
		public string ConditionArg { get; set; }
		public string ConditionArg2 { get; set; }
		public float ConditionNumArg1 { get; set; }
		public float ConditionNumArg2 { get; set; }
		public int CardUseType { get; set; }
		public float AspdModifier { get; set; }
	}
}
