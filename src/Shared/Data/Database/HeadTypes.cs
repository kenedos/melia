using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class HeadTypeData
	{
		public int Id { get; set; }
		public int Index { get; set; }
		public Gender Gender { get; set; }
		public string Name { get; set; }
		public string ClassName { get; set; }
		public string Color { get; set; }
	}

	/// <summary>
	/// Head type database.
	/// </summary>
	public class HeadTypeDb : DatabaseJson<HeadTypeData>
	{
		/// <summary>
		/// Returns the head with the given class name or null if there was no
		/// matching head.
		/// </summary>
		/// <param name="gender"></param>
		/// <param name="name"></param>
		/// <param name="headData"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public bool TryFind(Gender gender, string name, out HeadTypeData headData, string color = "default")
		{
			headData = this.Entries.Find(a => a.Gender == gender && a.ClassName.Equals(name, StringComparison.CurrentCultureIgnoreCase) && a.Color == color);

			return headData != null;
		}

		/// <summary>
		/// Returns the head with the given index or null if there was no
		/// matching head.
		/// </summary>
		/// <param name="gender"></param>
		/// <param name="index"></param>
		/// <param name="headData"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public bool TryFind(Gender gender, int index, out HeadTypeData headData, string color = "default")
		{
			headData = this.Entries.Find(a => a.Gender == gender && a.Index == index && a.Color == color);

			return headData != null;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("index", "gender", "name", "className", "color");

			var data = new HeadTypeData();

			data.Index = entry.ReadInt("index");
			data.Gender = entry.ReadEnum<Gender>("gender");
			data.Name = entry.ReadString("name");
			data.ClassName = entry.ReadString("className");
			data.Color = entry.ReadString("color");

			this.Add(data);
		}
	}
}
