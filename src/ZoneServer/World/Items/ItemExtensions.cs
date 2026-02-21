using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;

namespace Melia.Zone.World.Items
{
	public static class ItemExtensions
	{
		public static bool IsKeyMaterial(this Item item)
		{
			var reinforceType = item.Data.ReinforceType;
			var equipXpGroup = item.Data.EquipExpGroup;
			return reinforceType == ReinforceType.Hethran && equipXpGroup == EquipExpGroup.hethran_material;
		}

		public static void GetItemLevelExp(this Item item, out int lv, out int curExp, out int maxExp)
		{
			var itemExp = (int)item.Properties[PropertyName.ItemExp];

			lv = ZoneServer.Instance.Data.ItemExpDb.GetLevel(item.Data.EquipExpGroup, itemExp);
			curExp = ZoneServer.Instance.Data.ItemExpDb.GetNextExp(item.Data.EquipExpGroup, lv) - itemExp;
			maxExp = ZoneServer.Instance.Data.ItemExpDb.GetTotalExp(item.Data.EquipExpGroup, lv);
		}

		public static void GetItemLevelExp(this Item item, int itemExp, out int lv, out int curExp, out int maxExp)
		{
			lv = ZoneServer.Instance.Data.ItemExpDb.GetLevel(item.Data.EquipExpGroup, itemExp);
			curExp = ZoneServer.Instance.Data.ItemExpDb.GetNextExp(item.Data.EquipExpGroup, lv) - itemExp;
			maxExp = ZoneServer.Instance.Data.ItemExpDb.GetTotalExp(item.Data.EquipExpGroup, lv);
		}

		public static int GetMaterialPrice(this Item item)
		{
			var itemExp = (int)item.Properties[PropertyName.ItemExp];
			var lv = ZoneServer.Instance.Data.ItemExpDb.GetLevel(item.Data.EquipExpGroup, itemExp);
			var priceMultiple = ZoneServer.Instance.Data.ItemExpDb.GetPriceMultiplier(item.Data.EquipExpGroup, lv);

			return (int)(priceMultiple * item.Properties[PropertyName.MaterialPrice]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int GetMixMaterialExp(this Item item)
		{
			if (item.Data.EquipExpGroup == EquipExpGroup.None)
				return 0;

			int level;
			if (item.Data.Group == ItemGroup.Card)
				level = Math.Max(item.CardLevel, 1);
			else
				level = Math.Max(item.GemLevel, 1);
			var itemExp = ZoneServer.Instance.Data.ItemExpDb.GetGainExp(item.Data.EquipExpGroup, level);

			if (itemExp != 0)
			{
				if (item.Data.Type == ItemType.Equip)
				{
					return (int)item.Properties[PropertyName.UseLv];
				}
				else if (item.Data.EquipExpGroup == EquipExpGroup.hethran_material)
				{
					return itemExp;
				}
				// Material Exp doesn't exist?
				//return item.Properties[PropertyName.MaterialExp];
				return itemExp;
			}

			return 0;
		}

		/// <summary>
		/// Serializes an item's properties to a Lua table string for sending to the client.
		/// </summary>
		/// <param name="item">The item to serialize</param>
		/// <returns>A Lua table string like {["PropName"]=value, ...}</returns>
		public static string SerializePropertiesToLua(this Item item)
		{
			var sb = new StringBuilder();
			sb.Append("{");

			var first = true;
			foreach (var property in item.Properties.GetAll())
			{
				// Escape property name
				var escapedIdent = property.Ident
					.Replace("\\", "\\\\")
					.Replace("\"", "\\\"");

				if (property is FloatProperty floatProp)
				{
					// Skip NaN and Infinity values
					if (float.IsNaN(floatProp.Value) || float.IsInfinity(floatProp.Value))
						continue;

					if (!first)
						sb.Append(",");
					first = false;

					// Format as integer if it's a whole number (avoids "645097.0" format)
					var value = floatProp.Value;
					if (value == Math.Truncate(value))
						sb.AppendFormat("[\"{0}\"]={1}", escapedIdent, (long)value);
					else
						sb.AppendFormat("[\"{0}\"]={1}", escapedIdent, value.ToString(CultureInfo.InvariantCulture));
				}
				else if (property is StringProperty stringProp)
				{
					if (!first)
						sb.Append(",");
					first = false;

					// Escape string values properly
					var escapedValue = stringProp.Value
						.Replace("\\", "\\\\")
						.Replace("\"", "\\\"")
						.Replace("\n", "\\n")
						.Replace("\r", "\\r");
					sb.AppendFormat("[\"{0}\"]=\"{1}\"", escapedIdent, escapedValue);
				}
			}

			sb.Append("}");
			return sb.ToString();
		}
	}
}
