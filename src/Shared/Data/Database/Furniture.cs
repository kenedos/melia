using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	public enum FurnitureGroup
	{
		None = 0,
		CategoryCount = 1,
		Category_1 = 2,
		Category_2 = 3,
		Category_3 = 4,
		Interaction_Furniture = 1001,
		Statue = 1002,
		NPC = 1003,
		Etc_Furniture = 1004,
		Craft_Furniture = 1005,
		Interaction_Wall = 2001,
		Picture = 2002,
		Etc_Wall = 2003,
		Craft_Wall = 2004,
		Rug = 3001,
		Fur = 3002,
		Etc_Carpet = 3003,
		Craft_Carpet = 3004,
	}

	[Serializable]
	public class FurnitureData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public string Category { get; set; }
		public string Name { get; set; }
		public string ItemClassName { get; set; }
		public bool IsBackgroundModel { get; set; }
		public int Column { get; set; }
		public int Row { get; set; }
		public int Height { get; set; }
		public float DefaultAngle { get; set; }
		public int DemolitionPrice { get; set; }
		public FurnitureGroup Group { get; set; }
		public int Points { get; set; }
		public int Level { get; set; }

		public bool IsWall
		{
			get
			{
				switch (this.Group)
				{
					case FurnitureGroup.Interaction_Wall:
					case FurnitureGroup.Picture:
					case FurnitureGroup.Etc_Wall:
					case FurnitureGroup.Craft_Wall:
						return true;
					default:
						return false;
				}
			}
		}

		public bool IsCarpet
		{
			get
			{
				switch (this.Group)
				{
					case FurnitureGroup.Rug:
					case FurnitureGroup.Fur:
					case FurnitureGroup.Etc_Carpet:
					case FurnitureGroup.Craft_Carpet:
						return true;
					default:
						return false;
				}
			}
		}
	}

	/// <summary>
	/// Housing Furniture database.
	/// </summary>
	public class FurnitureDb : DatabaseJsonIndexed<int, FurnitureData>
	{
		/// <summary>
		/// Find a furniture or null with a given class name.
		/// </summary>
		/// <param name="className"></param>
		/// <returns></returns>
		public FurnitureData FindByClass(string className)
		{
			className = className.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.ClassName.ToLower() == className).Value;
		}

		/// <summary>
		/// Find a furniture or null with a given item class name.
		/// </summary>
		/// <param name="itemClassName"></param>
		/// <returns></returns>
		public FurnitureData FindByItemClass(string itemClassName)
		{
			itemClassName = itemClassName.ToLower();
			return this.Entries.FirstOrDefault(a => a.Value.ItemClassName.ToLower() == itemClassName).Value;
		}

		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("furnitureId", "className", "name", "itemClassName", "backgroundModel");

			var data = new FurnitureData();

			data.Id = entry.ReadInt("furnitureId");
			data.ClassName = entry.ReadString("className");
			data.Name = entry.ReadString("name");
			data.ItemClassName = entry.ReadString("itemClassName");
			data.IsBackgroundModel = entry.ReadBool("backgroundModel");
			if (!string.IsNullOrEmpty(entry.ReadString("group")))
				data.Group = entry.ReadEnum<FurnitureGroup>("group");
			else
				data.Group = FurnitureGroup.None;
			data.Row = entry.ReadInt("row");
			data.Column = entry.ReadInt("column");
			data.Height = entry.ReadInt("height");
			data.Points = entry.ReadInt("points");
			data.Level = entry.ReadInt("level");
			data.DefaultAngle = entry.ReadFloat("defaultAngle");
			data.DemolitionPrice = entry.ReadInt("demolitionPrice");

			this.Entries[data.Id] = data;
		}
	}
}
