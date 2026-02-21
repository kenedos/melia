using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Logging;

namespace Melia.Shared.Data.Database
{
	[Serializable]
	public class ItemGradeData
	{
		public int Id { get; set; }
		public string ClassName { get; set; }
		public int Grade { get; set; }
		public int BasicRatio { get; set; }
		public int ReinforceRatio { get; set; }
		public int ReinforceCostRatio { get; set; }
		public int TranscendCostRatio { get; set; }
		public float Cloth { get; set; }
		public float Leather { get; set; }
		public float Iron { get; set; }
		public float Shield { get; set; }
		public float Ghost { get; set; }
		public float Shirt { get; set; }
		public float Pants { get; set; }
		public float Boots { get; set; }
		public float Gloves { get; set; }
		public float None { get; set; }
		public float Sword { get; set; }
		public float THSword { get; set; }
		public float Staff { get; set; }
		public float THBow { get; set; }
		public float Bow { get; set; }
		public float Mace { get; set; }
		public float THMace { get; set; }
		public float Spear { get; set; }
		public float THSpear { get; set; }
		public float Dagger { get; set; }
		public float THStaff { get; set; }
		public float Pistol { get; set; }
		public float Rapier { get; set; }
		public float Cannon { get; set; }
		public float Musket { get; set; }
		public float Trinket { get; set; }
		public float Neck { get; set; }
		public float Ring { get; set; }
		public float Arcane { get; set; }
		public float Sword_Reinforce_1 { get; set; }
		public float Sword_Reinforce_2 { get; set; }
		public float Sword_Reinforce_3 { get; set; }
		public float THSword_Reinforce_1 { get; set; }
		public float THSword_Reinforce_2 { get; set; }
		public float THSword_Reinforce_3 { get; set; }
		public float Staff_Reinforce_1 { get; set; }
		public float Staff_Reinforce_2 { get; set; }
		public float Staff_Reinforce_3 { get; set; }
		public float THBow_Reinforce_1 { get; set; }
		public float THBow_Reinforce_2 { get; set; }
		public float THBow_Reinforce_3 { get; set; }
		public float Bow_Reinforce_1 { get; set; }
		public float Bow_Reinforce_2 { get; set; }
		public float Bow_Reinforce_3 { get; set; }
		public float Mace_Reinforce_1 { get; set; }
		public float Mace_Reinforce_2 { get; set; }
		public float Mace_Reinforce_3 { get; set; }
		public float THMace_Reinforce_1 { get; set; }
		public float THMace_Reinforce_2 { get; set; }
		public float THMace_Reinforce_3 { get; set; }
		public float Spear_Reinforce_1 { get; set; }
		public float Spear_Reinforce_2 { get; set; }
		public float Spear_Reinforce_3 { get; set; }
		public float THSpear_Reinforce_1 { get; set; }
		public float THSpear_Reinforce_2 { get; set; }
		public float THSpear_Reinforce_3 { get; set; }
		public float Dagger_Reinforce_1 { get; set; }
		public float Dagger_Reinforce_2 { get; set; }
		public float Dagger_Reinforce_3 { get; set; }
		public float THStaff_Reinforce_1 { get; set; }
		public float THStaff_Reinforce_2 { get; set; }
		public float THStaff_Reinforce_3 { get; set; }
		public float Pistol_Reinforce_1 { get; set; }
		public float Pistol_Reinforce_2 { get; set; }
		public float Pistol_Reinforce_3 { get; set; }
		public float Rapier_Reinforce_1 { get; set; }
		public float Rapier_Reinforce_2 { get; set; }
		public float Rapier_Reinforce_3 { get; set; }
		public float Cannon_Reinforce_1 { get; set; }
		public float Cannon_Reinforce_2 { get; set; }
		public float Cannon_Reinforce_3 { get; set; }
		public float Musket_Reinforce_1 { get; set; }
		public float Musket_Reinforce_2 { get; set; }
		public float Musket_Reinforce_3 { get; set; }
		public float Shield_Reinforce_1 { get; set; }
		public float Shield_Reinforce_2 { get; set; }
		public float Shield_Reinforce_3 { get; set; }
		public float Trinket_Reinforce_1 { get; set; }
		public float Trinket_Reinforce_2 { get; set; }
		public float Trinket_Reinforce_3 { get; set; }

		public Dictionary<string, float> EquipClassValues { get; set; } = new Dictionary<string, float>();
		public Dictionary<string, float> EquipClassReinforceValues { get; set; } = new Dictionary<string, float>();
		public Dictionary<string, float> MaterialTypeValues { get; set; } = new Dictionary<string, float>();
	}

	/// <summary>
	/// Recipe database, indexed by their id.
	/// </summary>
	public class ItemGradeDb : DatabaseJsonIndexed<int, ItemGradeData>
	{

		public bool TryFind(string className, out ItemGradeData itemGradeData)
		{
			itemGradeData = this.Entries.Values.Where(a => a.ClassName == className).FirstOrDefault();

			return itemGradeData != null;
		}

		public bool TryFindByGrade(int grade, out ItemGradeData itemGradeData)
		{
			itemGradeData = this.Entries.Values.Where(a => a.Grade == grade).FirstOrDefault();

			return itemGradeData != null;
		}

		public float FindValue(string className, EquipType equipType)
		{
			var itemGradeData = this.Entries.Values.Where(a => a.ClassName == className).FirstOrDefault();

			if (itemGradeData != null && itemGradeData.EquipClassValues.ContainsKey(equipType.ToString()))
				return itemGradeData.EquipClassValues[equipType.ToString()];
			return 0;
		}

		public float FindValue(string className, ArmorMaterialType materialType)
		{
			var itemGradeData = this.Entries.Values.Where(a => a.ClassName == className).FirstOrDefault();

			if (itemGradeData != null && itemGradeData.MaterialTypeValues.ContainsKey(materialType.ToString()))
				return itemGradeData.MaterialTypeValues[materialType.ToString()];
			return 0;
		}

		/// <summary>
		/// Reads given entry and adds it to the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "className");

			var data = new ItemGradeData();

			data.Id = entry.ReadInt("id");
			data.ClassName = entry.ReadString("className");
			data.Grade = entry.ReadInt("grade");
			data.BasicRatio = entry.ReadInt("basicRatio");
			data.ReinforceRatio = entry.ReadInt("reinforceRatio");
			data.ReinforceCostRatio = entry.ReadInt("reinforceCostRatio");
			data.TranscendCostRatio = entry.ReadInt("transcendCostRatio");
			data.Cloth = entry.ReadFloat("cloth");
			data.Leather = entry.ReadFloat("leather");
			data.Iron = entry.ReadFloat("iron");
			data.Shield = entry.ReadFloat("shield");
			data.Ghost = entry.ReadFloat("ghost");
			data.Shirt = entry.ReadFloat("shirt");
			data.Pants = entry.ReadFloat("pants");
			data.Boots = entry.ReadFloat("boots");
			data.Gloves = entry.ReadFloat("gloves");
			foreach (var equipType in Enum.GetValues(typeof(EquipType)).Cast<EquipType>())
			{
				var value = entry.ReadFloat(equipType.ToString().ToLower());
				if (value != 0)
					data.EquipClassValues.Add(equipType.ToString(), value);
			}
			foreach (var equipType in Enum.GetValues(typeof(EquipType)).Cast<EquipType>())
			{
				for (var i = 1; i <= 3; i++)
				{
					var value = entry.ReadFloat(equipType.ToString().ToLower() + "Reinforce" + i);
					if (value != 0)
						data.EquipClassReinforceValues.Add(equipType.ToString() + "Reinforce" + i, value);
				}
			}

			foreach (var materialType in Enum.GetValues(typeof(ArmorMaterialType)).Cast<ArmorMaterialType>())
			{
				var value = entry.ReadFloat(materialType.ToString().ToLower());
				if (value != 0)
					data.MaterialTypeValues.Add(materialType.ToString(), value);
			}
			data.None = entry.ReadFloat("none");
			data.Sword = entry.ReadFloat("sword");
			data.THSword = entry.ReadFloat("THSword");
			data.Staff = entry.ReadFloat("Staff");
			data.THBow = entry.ReadFloat("THBow");
			data.Bow = entry.ReadFloat("Bow");
			data.Mace = entry.ReadFloat("Mace");
			data.THMace = entry.ReadFloat("THMace");
			data.Spear = entry.ReadFloat("Spear");
			data.THSpear = entry.ReadFloat("THSpear");
			data.Dagger = entry.ReadFloat("Dagger");
			data.THStaff = entry.ReadFloat("THStaff");
			data.Pistol = entry.ReadFloat("Pistol");
			data.Rapier = entry.ReadFloat("Rapier");
			data.Cannon = entry.ReadFloat("Cannon");
			data.Musket = entry.ReadFloat("Musket");
			data.Trinket = entry.ReadFloat("Trinket");
			data.Neck = entry.ReadFloat("Neck");
			data.Ring = entry.ReadFloat("Ring");
			data.Arcane = entry.ReadFloat("Arcane");
			data.Sword_Reinforce_1 = entry.ReadFloat("Sword_Reinforce_1");
			data.Sword_Reinforce_2 = entry.ReadFloat("Sword_Reinforce_2");
			data.Sword_Reinforce_3 = entry.ReadFloat("Sword_Reinforce_3");
			data.THSword_Reinforce_1 = entry.ReadFloat("THSword_Reinforce_1");
			data.THSword_Reinforce_2 = entry.ReadFloat("THSword_Reinforce_2");
			data.THSword_Reinforce_3 = entry.ReadFloat("THSword_Reinforce_3");
			data.Staff_Reinforce_1 = entry.ReadFloat("Staff_Reinforce_1");
			data.Staff_Reinforce_2 = entry.ReadFloat("Staff_Reinforce_2");
			data.Staff_Reinforce_3 = entry.ReadFloat("Staff_Reinforce_3");
			data.THBow_Reinforce_1 = entry.ReadFloat("THBow_Reinforce_1");
			data.THBow_Reinforce_2 = entry.ReadFloat("THBow_Reinforce_2");
			data.THBow_Reinforce_3 = entry.ReadFloat("THBow_Reinforce_3");
			data.Bow_Reinforce_1 = entry.ReadFloat("Bow_Reinforce_1");
			data.Bow_Reinforce_2 = entry.ReadFloat("Bow_Reinforce_2");
			data.Bow_Reinforce_3 = entry.ReadFloat("Bow_Reinforce_3");
			data.Mace_Reinforce_1 = entry.ReadFloat("Mace_Reinforce_1");
			data.Mace_Reinforce_2 = entry.ReadFloat("Mace_Reinforce_2");
			data.Mace_Reinforce_3 = entry.ReadFloat("Mace_Reinforce_3");
			data.THMace_Reinforce_1 = entry.ReadFloat("THMace_Reinforce_1");
			data.THMace_Reinforce_2 = entry.ReadFloat("THMace_Reinforce_2");
			data.THMace_Reinforce_3 = entry.ReadFloat("THMace_Reinforce_3");
			data.Spear_Reinforce_1 = entry.ReadFloat("Spear_Reinforce_1");
			data.Spear_Reinforce_2 = entry.ReadFloat("Spear_Reinforce_2");
			data.Spear_Reinforce_3 = entry.ReadFloat("Spear_Reinforce_3");
			data.THSpear_Reinforce_1 = entry.ReadFloat("THSpear_Reinforce_1");
			data.THSpear_Reinforce_2 = entry.ReadFloat("THSpear_Reinforce_2");
			data.THSpear_Reinforce_3 = entry.ReadFloat("THSpear_Reinforce_3");
			data.Dagger_Reinforce_1 = entry.ReadFloat("Dagger_Reinforce_1");
			data.Dagger_Reinforce_2 = entry.ReadFloat("Dagger_Reinforce_2");
			data.Dagger_Reinforce_3 = entry.ReadFloat("Dagger_Reinforce_3");
			data.THStaff_Reinforce_1 = entry.ReadFloat("THStaff_Reinforce_1");
			data.THStaff_Reinforce_2 = entry.ReadFloat("THStaff_Reinforce_2");
			data.THStaff_Reinforce_3 = entry.ReadFloat("THStaff_Reinforce_3");
			data.Pistol_Reinforce_1 = entry.ReadFloat("Pistol_Reinforce_1");
			data.Pistol_Reinforce_2 = entry.ReadFloat("Pistol_Reinforce_2");
			data.Pistol_Reinforce_3 = entry.ReadFloat("Pistol_Reinforce_3");
			data.Rapier_Reinforce_1 = entry.ReadFloat("Rapier_Reinforce_1");
			data.Rapier_Reinforce_2 = entry.ReadFloat("Rapier_Reinforce_2");
			data.Rapier_Reinforce_3 = entry.ReadFloat("Rapier_Reinforce_3");
			data.Cannon_Reinforce_1 = entry.ReadFloat("Cannon_Reinforce_1");
			data.Cannon_Reinforce_2 = entry.ReadFloat("Cannon_Reinforce_2");
			data.Cannon_Reinforce_3 = entry.ReadFloat("Cannon_Reinforce_3");
			data.Musket_Reinforce_1 = entry.ReadFloat("Musket_Reinforce_1");
			data.Musket_Reinforce_2 = entry.ReadFloat("Musket_Reinforce_2");
			data.Musket_Reinforce_3 = entry.ReadFloat("Musket_Reinforce_3");
			data.Shield_Reinforce_1 = entry.ReadFloat("Shield_Reinforce_1");
			data.Shield_Reinforce_2 = entry.ReadFloat("Shield_Reinforce_2");
			data.Shield_Reinforce_3 = entry.ReadFloat("Shield_Reinforce_3");
			data.Trinket_Reinforce_1 = entry.ReadFloat("Trinket_Reinforce_1");
			data.Trinket_Reinforce_2 = entry.ReadFloat("Trinket_Reinforce_2");
			data.Trinket_Reinforce_3 = entry.ReadFloat("Trinket_Reinforce_3");

			this.AddOrReplace(data.Id, data);
		}
	}
}
