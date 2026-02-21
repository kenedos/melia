using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Zone.Scripting;

namespace Melia.Zone.World.Actors.Monsters
{
	/// <summary>
	/// Represents a companion's properties.
	/// </summary>
	public class CompanionProperties : MonsterProperties
	{
		private Companion Companion => (Companion)this.Monster;

		/// <summary>
		/// Creates new instance for companion.
		/// </summary>
		/// <param name="companion"></param>
		public CompanionProperties(Companion companion) : base(companion)
		{
		}

		/// <summary>
		/// Adds the companion's default properties, overriding monster defaults
		/// to use companion-specific calculation functions.
		/// </summary>
		public override void AddDefaultProperties()
		{
			base.AddDefaultProperties();

			this.Create(PropertyName.STR, "SCR_Get_Companion_STR");
			this.Create(PropertyName.DEX, "SCR_Get_Companion_DEX");
			this.Create(PropertyName.CON, "SCR_Get_Companion_CON");
			this.Create(PropertyName.INT, "SCR_Get_Companion_INT");
			this.Create(PropertyName.MNA, "SCR_Get_Companion_MNA");
			this.Create(PropertyName.DEF, "SCR_Get_Companion_DEF");
			this.Create(PropertyName.MDEF, "SCR_Get_Companion_MDEF");

			this.Create(PropertyName.MHP, "SCR_Get_Companion_MHP");
			this.Create(new FloatProperty(PropertyName.HP, min: 0));

			this.Create(PropertyName.MaxStamina, "SCR_Get_Companion_MaxStamina");
			this.Create(new FloatProperty(PropertyName.Stamina, 0, min: 0));

			this.Create(PropertyName.MAXPATK, "SCR_Get_Companion_MAXPATK");
			this.Create(PropertyName.MINPATK, "SCR_Get_Companion_MINPATK");
			this.Create(PropertyName.MAXMATK, "SCR_Get_Companion_MAXMATK");
			this.Create(PropertyName.MINMATK, "SCR_Get_Companion_MINMATK");
			this.Create(PropertyName.ATK, "SCR_Get_Companion_ATK");

			this.Create(PropertyName.MountDEF, "SCR_Get_Companion_MOUNTDEF");
			this.Create(PropertyName.MountDR, "SCR_Get_Companion_MOUNTDR");
			this.Create(PropertyName.MountMHP, "SCR_Get_Companion_MOUNTMHP");

			this.Create(PropertyName.HR, "SCR_Get_Companion_HR");
			this.Create(PropertyName.BLK_BREAK, "SCR_Get_Companion_BLK_BREAK");
			this.Create(PropertyName.CRTHR, "SCR_Get_Companion_CRTHR");
			this.Create(PropertyName.DR, "SCR_Get_Companion_DR");
			this.Create(PropertyName.SDR, "SCR_Get_Companion_SDR");

			this.Create(new FloatProperty(PropertyName.Stat_MHP, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_ATK, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_DEF, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_MDEF, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_HR, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_CRTHR, min: 1));
			this.Create(new FloatProperty(PropertyName.Stat_DR, min: 1));

			this.Create(PropertyName.RHP, "SCR_Get_Companion_RHP");
			this.Create(PropertyName.RHPTIME, "SCR_Get_Companion_RHPTIME");
		}

		/// <summary>
		/// Initializes the auto-updates for the companion's properties.
		/// </summary>
		public override void InitAutoUpdates()
		{
			this.AutoUpdate(PropertyName.MINPATK, [PropertyName.PATK_BM, PropertyName.ATK_BM]);
			this.AutoUpdate(PropertyName.MAXPATK, [PropertyName.PATK_BM, PropertyName.ATK_BM]);
			this.AutoUpdate(PropertyName.MINMATK, [PropertyName.MATK_BM, PropertyName.ATK_BM]);
			this.AutoUpdate(PropertyName.MAXMATK, [PropertyName.MATK_BM, PropertyName.ATK_BM]);
			this.AutoUpdate(PropertyName.CRTHR, [PropertyName.CRTHR_BM, PropertyName.Stat_CRTHR_BM]);
			this.AutoUpdate(PropertyName.HR, [PropertyName.Stat_HR_BM]);
			this.AutoUpdate(PropertyName.BLK_BREAK, [PropertyName.Stat_HR_BM]);
			this.AutoUpdate(PropertyName.SR, [PropertyName.SR_BM]);

			this.AutoUpdate(PropertyName.RHP, [PropertyName.Stamina]);

			this.AutoUpdateMax(PropertyName.HP, PropertyName.MHP);
			this.AutoUpdateMax(PropertyName.Stamina, PropertyName.MaxStamina);
		}

		/// <summary>
		/// Calculates property value using companion-specific scriptable functions
		/// for COMPANION functions, falling back to monster functions for others.
		/// </summary>
		/// <param name="calcFuncName"></param>
		/// <returns></returns>
		private new float CalculateProperty(string calcFuncName)
		{
			if (calcFuncName.StartsWith("SCR_Get_Companion_"))
			{
				if (ScriptableFunctions.Companion.TryGet(calcFuncName, out var companionFunc))
					return companionFunc(this.Companion);
			}

			// Fall back to monster functions
			if (ScriptableFunctions.Monster.TryGet(calcFuncName, out var monsterFunc))
				return monsterFunc(this.Monster);

			throw new ArgumentException($"Scriptable function '{calcFuncName}' not found.");
		}

		/// <summary>
		/// Creates a new calculated float property that uses the given
		/// function.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="calcFuncName"></param>
		private new void Create(string propertyName, string calcFuncName)
		{
			if (PropertyTable.Exists(this.Namespace, propertyName))
				this.Create(new CFloatProperty(propertyName, () => this.CalculateProperty(calcFuncName)));
		}
	}
}
