using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Items
{
	/// <summary>
	/// Shows a character items they don't own, as real items rather than
	/// as item classes.
	/// </summary>
	/// <remarks>
	/// A window that lists someone else's items - an offering box, a sell
	/// shop - can only build its slots from the item class, and the client
	/// reads sockets and gems off an item instance, never off properties.
	/// Sending the items as one of the client's own item lists gives its
	/// windows the instance they need.
	/// </remarks>
	public static class ItemPreview
	{
		private const string ListTypeVar = "Melia.ItemPreview.ListType";
		private const string ItemsVar = "Melia.ItemPreview.Items";

		/// <summary>
		/// Remembers which of the client's item lists the given character's
		/// windows read previewed items from.
		/// </summary>
		/// <remarks>
		/// The list is identified by a client-side constant with no server
		/// equivalent, so the window reports it instead of the server
		/// assuming a value.
		/// </remarks>
		/// <param name="viewer"></param>
		/// <param name="listType"></param>
		public static void SetListType(Character viewer, int listType)
		{
			if (listType > 0)
				viewer.Variables.Temp.SetInt(ListTypeVar, listType);
		}

		/// <summary>
		/// Returns whether the given list is the one the character's
		/// windows preview items from.
		/// </summary>
		/// <param name="viewer"></param>
		/// <param name="listType"></param>
		/// <returns></returns>
		public static bool IsPreviewList(Character viewer, int listType)
			=> listType > 0 && viewer.Variables.Temp.GetInt(ListTypeVar, 0) == listType;

		/// <summary>
		/// Sends the given items to the character's preview list, doing
		/// nothing if no window has reported one yet.
		/// </summary>
		/// <param name="viewer"></param>
		/// <param name="items"></param>
		public static void Show(Character viewer, IEnumerable<Item> items)
		{
			var previewItems = items.ToList();

			// The client asks for a list before it keeps one, so what a
			// window previews has to outlive the send that announced it.
			viewer.Variables.Temp.Set(ItemsVar, previewItems);

			SendList(viewer, previewItems);
		}

		/// <summary>
		/// Sends the character the items they were last shown, in answer
		/// to the client asking for the list.
		/// </summary>
		/// <param name="viewer"></param>
		public static void Resend(Character viewer)
			=> SendList(viewer, viewer.Variables.Temp.Get<List<Item>>(ItemsVar) ?? new List<Item>());

		/// <summary>
		/// Sends the given items as the character's preview list, doing
		/// nothing if no window has reported one yet.
		/// </summary>
		/// <param name="viewer"></param>
		/// <param name="items"></param>
		private static void SendList(Character viewer, List<Item> items)
		{
			var listType = viewer.Variables.Temp.GetInt(ListTypeVar, 0);
			if (listType <= 0)
				return;

			var indexedItems = new Dictionary<int, Item>();
			foreach (var item in items)
				indexedItems[indexedItems.Count] = item;

			Send.ZC_SOLD_ITEM_DIVISION_LIST(viewer, (InventoryType)listType, indexedItems);

			foreach (var item in indexedItems.Values.Where(a => a.HasSockets))
				Send.ZC_EQUIP_GEM_INFO(viewer, item);
		}
	}
}
