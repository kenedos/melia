using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.ObjectProperties;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items.Mods;
using Melia.Zone.World.Maps;
using SixLabors.ImageSharp.Drawing;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;

namespace Melia.Zone.World.Items
{
	/// <summary>
	/// An item, that might be lying around in the world or is owned by
	/// some entity.
	/// </summary>
	public class Item : IPropertyObject
	{
		private readonly List<Item> _gemSockets = new();

		private static long ObjectIds = ObjectIdRanges.Items;

		/// <summary>
		/// Returns the item's class id.
		/// </summary>
		public int Id { get; private set; }

		/// <summary>
		/// Returns a reference to the item's data from the item data file.
		/// </summary>
		public ItemData Data { get; protected set; }

		/// <summary>
		/// Gets or sets the item's amount (clamped to 1~max),
		/// does not update the client.
		/// </summary>
		public int Amount
		{
			get { return _amount; }
			set
			{
				var max = (this.Data != null ? this.Data.MaxStack : 1);
				_amount = Math2.Clamp(1, max, value);
			}
		}
		private int _amount;

		/// <summary>
		/// Returns true if item's MaxStack is higher than 1, indicating
		/// that it can contain more than one item of its type.
		/// </summary>
		public bool IsStackable => this.Data.MaxStack > 1;

		/// <summary>
		/// Gets or sets item's globally unique db id.
		/// </summary>
		public long DbId { get; set; }

		/// <summary>
		/// Returns the globally unique object id.
		/// </summary>
		public virtual long ObjectId { get; } = Interlocked.Increment(ref ObjectIds);

		/// <summary>
		/// Returns item's buy price.
		/// </summary>
		public int Price { get; private set; }

		/// <summary>
		/// Returns the item's current durability.
		/// </summary>
		public int Durability
		{
			get => (int)this.Properties.GetFloat(PropertyName.Dur);
			set => this.Properties.SetFloat(PropertyName.Dur, (int)Math2.Clamp(0, this.Properties.GetFloat(PropertyName.MaxDur), value));
		}

		/// <summary>
		/// Returns the item's maximum durability.
		/// </summary>
		public int MaxDurability => (int)this.Properties.GetFloat(PropertyName.MaxDur);

		/// <summary>
		/// Gets or sets whether the item is locked.
		/// </summary>
		/// <remarks>
		/// XXX: Should this be saved? If so, we have to figure out where
		///   that goes in the item data.
		/// </remarks>
		public bool IsLocked { get; set; }

		/// <summary>
		/// Gets or sets an expiration date on the item
		/// </summary>
		public DateTime ExpirationDate { get; private set; } = DateTime.MaxValue;

		/// <summary>
		/// Checks if an item is expiring or can expire.
		/// </summary>
		public bool IsExpiring => this.ExpirationDate != DateTime.MaxValue;

		/// <summary>
		/// Checks if an item is expiring or can expire.
		/// </summary>
		public bool IsExpired => this.ExpirationDate < DateTime.Now || this.Properties[PropertyName.ItemLifeTimeOver] > 1;

		/// <summary>
		/// Returns the handle of the entity who originally dropped the item.
		/// </summary>
		public int OriginalOwnerHandle { get; private set; } = -1;

		/// <summary>
		/// Returns the time at which the owner can pick the item back up.
		/// </summary>
		public DateTime RePickUpTime { get; private set; }

		/// <summary>
		/// Gets or sets the owner of the item, who is the only one able
		/// to pick it up while the loot protection is active.
		/// </summary>
		public int OwnerHandle { get; private set; } = -1;

		/// <summary>
		/// Returns the time at which the item is free to be picked up
		/// by anyone.
		/// </summary>
		public DateTime LootProtectionEnd { get; private set; } = DateTime.MinValue;


		/// <summary>
		/// Returns the persistent ObjectId of the character who originally dropped the item.
		/// </summary>
		public long OriginalOwnerCharacterId { get; private set; } = 0;

		/// <summary>
		/// Returns the persistent ObjectId of the character who owns the loot.
		/// </summary>
		public long OwnerCharacterId { get; private set; } = 0;

		/// <summary>
		/// Returns the item's property collection.
		/// </summary>
		public Properties Properties { get; } = new Properties("Item");

		/// <summary>
		/// Returns reference to the item's cooldown data from the database
		/// database.
		/// </summary>
		public CooldownData CooldownData { get; private set; }

		/// <summary>
		/// Returns the if the item has random options generated
		/// </summary>
		public bool NeedRandomOptions => this.Properties.GetFloat(PropertyName.NeedRandomOption) != 0;

		/// <summary>
		/// Returns the if the item has requires appraisal before use.
		/// </summary>
		public bool NeedsAppraisal => this.Properties.GetFloat(PropertyName.NeedAppraisal) != 0;

		/// <summary>
		/// Returns the item's max sockets.
		/// </summary>
		public int MaxSockets => (int)this.Properties.GetFloat(PropertyName.MaxSocket);

		/// <summary>
		/// Returns the item's potential.
		/// </summary>
		public int Potential => (int)this.Properties.GetFloat(PropertyName.PR, this.Data.Potential);

		/// <summary>
		/// Returns the item's level.
		/// </summary>
		public int UseLevel => (int)this.Properties.GetFloat(PropertyName.UseLv, this.Data.MinLevel);

		/// <summary>
		/// Returns the item's hidden level.
		/// </summary>
		public int HiddenLevel => (int)this.Properties.GetFloat(PropertyName.ItemLv);

		/// <summary>
		/// Returns the item's "level".
		/// </summary>
		public int Level => this.HiddenLevel > 0 ? this.HiddenLevel : this.UseLevel;

		/// <summary>
		/// Returns the item's exp.
		/// </summary>
		public int Exp => (int)this.Properties.GetFloat(PropertyName.ItemExp);

		/// <summary>
		/// Returns the item's gem roasting level.
		/// </summary>
		public int GemRoastingLevel => (int)this.Properties.GetFloat(PropertyName.GemRoastingLv);

		/// <summary>
		/// Returns the gem's level.
		/// </summary>
		public int GemLevel => (int)this.Properties.GetFloat(PropertyName.Level);

		/// <summary>
		/// Returns the card's level or 0.
		/// </summary>
		public int CardLevel => (int)this.Properties.GetFloat(PropertyName.CardLevel);


		public bool HasSockets
		{
			get
			{
				lock (this._gemSockets) return this._gemSockets.Count != 0;
			}
		}

		/// <summary>
		/// Returns if the item is saved.
		/// </summary>
		public bool IsSaved => this.DbId != 0;

		public bool IsFirstDrop { get; set; }
		public bool IsRefinable
		{
			get
			{
				if (this.Data.Type != ItemType.Equip)
					return false;

				if (this.Data.ReinforceType != ReinforceType.Moru)
					return false;

				if (string.IsNullOrEmpty(this.Data.MainProperties))
					return false;

				var prop = this.Data.MainProperties;
				if (prop != PropertyName.DEF && prop != PropertyName.MDEF && prop != PropertyName.ADD_FIRE && prop != PropertyName.ADD_ICE
					&& prop != PropertyName.ADD_LIGHTNING && prop != "DEF;MDEF" && prop != "ATK;MATK"
					&& prop != PropertyName.MATK && prop != PropertyName.ATK)
					return false;

				var reinforceValue = this.Properties.GetFloat(PropertyName.Reinforce_2);
				if (reinforceValue < 0 || reinforceValue >= 40)
					return false;

				return true;
			}
		}

		public bool Refinforcing { get; set; }
		/// <summary>
		/// The item's name.
		/// </summary>
		public string Name { get { return this.Properties.GetString(PropertyName.CustomName, this.Data.Name); } }

		/// <summary>
		/// The item's repair price ratio
		/// </summary>
		public float RepairPriceRatio => 100;

		/// <summary>
		/// Creates new item.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		public Item(long dbId, int itemId, int amount = 1)
		{
			this.DbId = dbId;
			this.Id = itemId;
			this.LoadData();

			// Set amount after loading the data so we can clamp it
			// to the max stack size
			this.Amount = amount;
		}

		/// <summary>
		/// Creates new item.
		/// </summary>
		/// <param name="itemId"></param>
		/// <param name="amount"></param>
		public Item(int itemId, int amount = 1)
		{
			this.Id = itemId;
			this.LoadData();

			// Set amount after loading the data so we can clamp it
			// to the max stack size
			this.Amount = amount;
		}

		/// <summary>
		/// Creates a copy of the given item.
		/// </summary>
		/// <param name="other"></param>
		public Item(Item other, int amount = -1)
		{
			this.Id = other.Id;
			this.LoadData();

			this.Price = other.Price;
			this.IsLocked = other.IsLocked;
			this.OriginalOwnerHandle = other.OriginalOwnerHandle;
			this.RePickUpTime = other.RePickUpTime;
			this.OwnerHandle = other.OwnerHandle;
			this.LootProtectionEnd = other.LootProtectionEnd;

			this.Properties.CopyFrom(other.Properties);
			this.CopyGemSockets(other);

			if (amount == -1)
				this.Amount = other.Amount;
			else
				this.Amount = amount;
		}

		/// <summary>
		/// Loads item data from data files.
		/// </summary>
		protected virtual void LoadData()
		{
			if (this.Id == 0)
				throw new InvalidOperationException("Item id wasn't set before calling LoadData.");

			this.Data = ZoneServer.Instance.Data.ItemDb.Find(this.Id);
			if (this.Data == null)
				throw new NullReferenceException("No item data found for '" + this.Id + "'.");

			if (!string.IsNullOrEmpty(this.Data.CooldownGroup))
			{
				this.CooldownData = ZoneServer.Instance.Data.CooldownDb.Find(this.Data.CooldownGroup);
				if (this.CooldownData == null)
					throw new NullReferenceException($"Unknown item '{this.Id}' cooldown group '{this.Data.CooldownGroup}'.");
			}

			// This isn't working so reverting to loading properties manually, exec said I needed to update Yggdrasil before it worked.
			// this.Properties.CopyFrom(this.Data.Properties);

			if (this.Data.MinLevel != 0) this.Properties.SetFloat(PropertyName.UseLv, this.Data.MinLevel);
			if (this.Data.Grade != 0) this.Properties.SetFloat(PropertyName.ItemGrade, (int)this.Data.Grade);
			if (this.Data.SellPrice != 0) this.Properties.SetFloat(PropertyName.SellPrice, this.Data.SellPrice);

			//if (this.Data.MinAtk != 0) this.Properties.SetFloat(PropertyName.MINATK, this.Data.MinAtk);
			//if (this.Data.MaxAtk != 0) this.Properties.SetFloat(PropertyName.MAXATK, this.Data.MaxAtk);
			//if (this.Data.MAtk != 0) this.Properties.SetFloat(PropertyName.MATK, this.Data.MAtk);
			//if (this.Data.Def != 0) this.Properties.SetFloat(PropertyName.DEF, this.Data.Def);
			//if (this.Data.MDef != 0) this.Properties.SetFloat(PropertyName.MDEF, this.Data.MDef);
			if (this.Data.MinAtk != 0) this.Create(PropertyName.MINATK, "SCR_Get_Item_MINATK");
			if (this.Data.MaxAtk != 0) this.Create(PropertyName.MAXATK, "SCR_Get_Item_MAXATK");
			if (this.Data.MAtk != 0) this.Create(PropertyName.MATK, "SCR_Get_Item_MATK");
			if (this.Data.Def != 0) this.Create(PropertyName.DEF, "SCR_Get_Item_DEF");
			if (this.Data.MDef != 0) this.Create(PropertyName.MDEF, "SCR_Get_Item_MDEF");
			if (this.Data.PAtk != 0) this.Properties.SetFloat(PropertyName.PATK, this.Data.PAtk);
			if (this.Data.AddMinAtk != 0) this.Properties.SetFloat(PropertyName.ADD_MINATK, this.Data.AddMinAtk);
			if (this.Data.AddMaxAtk != 0) this.Properties.SetFloat(PropertyName.ADD_MAXATK, this.Data.AddMaxAtk);
			if (this.Data.AddMAtk != 0) this.Properties.SetFloat(PropertyName.ADD_MATK, this.Data.AddMAtk);
			if (this.Data.AddDef != 0) this.Properties.SetFloat(PropertyName.ADD_DEF, this.Data.AddDef);
			if (this.Data.AddMDef != 0) this.Properties.SetFloat(PropertyName.ADD_MDEF, this.Data.AddMDef);
			if (this.Data.CriticalHitRate != 0) this.Properties.SetFloat(PropertyName.CRTHR, this.Data.CriticalHitRate);
			if (this.Data.CriticalAttack != 0) this.Properties.SetFloat(PropertyName.CRTATK, this.Data.CriticalAttack);
			if (this.Data.CriticalDodgeRate != 0) this.Properties.SetFloat(PropertyName.CRTDR, this.Data.CriticalDodgeRate);
			if (this.Data.AddHitRate != 0) this.Properties.SetFloat(PropertyName.ADD_HR, this.Data.AddHitRate);
			if (this.Data.AddDodgeRate != 0) this.Properties.SetFloat(PropertyName.ADD_DR, this.Data.AddDodgeRate);
			if (this.Data.Str != 0) this.Properties.SetFloat(PropertyName.STR, this.Data.Str);
			if (this.Data.Dex != 0) this.Properties.SetFloat(PropertyName.DEX, this.Data.Dex);
			if (this.Data.Con != 0) this.Properties.SetFloat(PropertyName.CON, this.Data.Con);
			if (this.Data.Int != 0) this.Properties.SetFloat(PropertyName.INT, this.Data.Int);
			if (this.Data.Mna != 0) this.Properties.SetFloat(PropertyName.MNA, this.Data.Mna);
			if (this.Data.SR != 0) this.Properties.SetFloat(PropertyName.SR, this.Data.SR);
			if (this.Data.SDR != 0) this.Properties.SetFloat(PropertyName.SDR, this.Data.SDR);
			if (this.Data.CriticalMagicAttack != 0) this.Properties.SetFloat(PropertyName.CRTMATK, this.Data.CriticalMagicAttack);
			if (this.Data.MGP != 0) this.Properties.SetFloat(PropertyName.MGP, this.Data.MGP);
			if (this.Data.AddSkillMaxR != 0) this.Properties.SetFloat(PropertyName.AddSkillMaxR, this.Data.AddSkillMaxR);
			if (this.Data.SkillRange != 0) this.Properties.SetFloat(PropertyName.SkillRange, this.Data.SkillRange);
			if (this.Data.SkillAngle != 0) this.Properties.SetFloat(PropertyName.SkillAngle, this.Data.SkillAngle);
			if (this.Data.Luck != 0) this.Properties.SetFloat(PropertyName.Luck, this.Data.Luck);
			if (this.Data.BlockRate != 0) this.Properties.SetFloat(PropertyName.BlockRate, this.Data.BlockRate);
			if (this.Data.Block != 0) this.Properties.SetFloat(PropertyName.BLK, this.Data.Block);
			if (this.Data.BlockBreak != 0) this.Properties.SetFloat(PropertyName.BLK_BREAK, this.Data.BlockBreak);
			if (this.Data.Revive != 0) this.Properties.SetFloat(PropertyName.Revive, this.Data.Revive);
			if (this.Data.HitCount != 0) this.Properties.SetFloat(PropertyName.HitCount, this.Data.HitCount);
			if (this.Data.BackHit != 0) this.Properties.SetFloat(PropertyName.BackHit, this.Data.BackHit);
			if (this.Data.SkillPower != 0) this.Properties.SetFloat(PropertyName.SkillPower, this.Data.SkillPower);
			if (this.Data.ASPD != 0) this.Properties.SetFloat(PropertyName.ASPD, this.Data.ASPD);
			if (this.Data.MSPD != 0) this.Properties.SetFloat(PropertyName.MSPD, this.Data.MSPD);
			if (this.Data.KnockdownPower != 0) this.Properties.SetFloat(PropertyName.KDPow, this.Data.KnockdownPower);
			if (this.Data.MHp != 0) this.Properties.SetFloat(PropertyName.MHP, this.Data.MHp);
			if (this.Data.MSp != 0) this.Properties.SetFloat(PropertyName.MSP, this.Data.MSp);
			if (this.Data.Msta != 0) this.Properties.SetFloat(PropertyName.MSTA, this.Data.Msta);
			if (this.Data.RHp != 0) this.Properties.SetFloat(PropertyName.RHP, this.Data.RHp);
			if (this.Data.RSp != 0) this.Properties.SetFloat(PropertyName.RSP, this.Data.RSp);
			if (this.Data.RSptime != 0) this.Properties.SetFloat(PropertyName.RSPTIME, this.Data.RSptime);
			if (this.Data.RSta != 0) this.Properties.SetFloat(PropertyName.RSTA, this.Data.RSta);
			if (this.Data.ClothBonus != 0) this.Properties.SetFloat(PropertyName.ADD_CLOTH, this.Data.ClothBonus);
			if (this.Data.LeatherBonus != 0) this.Properties.SetFloat(PropertyName.ADD_LEATHER, this.Data.LeatherBonus);
			if (this.Data.ChainBonus != 0) this.Properties.SetFloat(PropertyName.ADD_CHAIN, this.Data.ChainBonus);
			if (this.Data.IronBonus != 0) this.Properties.SetFloat(PropertyName.ADD_IRON, this.Data.IronBonus);
			if (this.Data.AddGhost != 0) this.Properties.SetFloat(PropertyName.ADD_GHOST, this.Data.AddGhost);
			if (this.Data.SmallSizeBonus != 0) this.Properties.SetFloat(PropertyName.ADD_SMALLSIZE, this.Data.SmallSizeBonus);
			if (this.Data.MediumSizeBonus != 0) this.Properties.SetFloat(PropertyName.ADD_MIDDLESIZE, this.Data.MediumSizeBonus);
			if (this.Data.LargeSizeBonus != 0) this.Properties.SetFloat(PropertyName.ADD_LARGESIZE, this.Data.LargeSizeBonus);
			if (this.Data.AddForester != 0) this.Properties.SetFloat(PropertyName.ADD_FORESTER, this.Data.AddForester);
			if (this.Data.AddWidling != 0) this.Properties.SetFloat(PropertyName.ADD_WIDLING, this.Data.AddWidling);
			if (this.Data.AddVelias != 0) this.Properties.SetFloat(PropertyName.ADD_VELIAS, this.Data.AddVelias);
			if (this.Data.AddParamune != 0) this.Properties.SetFloat(PropertyName.ADD_PARAMUNE, this.Data.AddParamune);
			if (this.Data.AddKlaida != 0) this.Properties.SetFloat(PropertyName.ADD_KLAIDA, this.Data.AddKlaida);
			if (this.Data.AddFire != 0) this.Properties.SetFloat(PropertyName.ADD_FIRE, this.Data.AddFire);
			if (this.Data.AddIce != 0) this.Properties.SetFloat(PropertyName.ADD_ICE, this.Data.AddIce);
			if (this.Data.AddPoison != 0) this.Properties.SetFloat(PropertyName.ADD_POISON, this.Data.AddPoison);
			if (this.Data.AddLightning != 0) this.Properties.SetFloat(PropertyName.ADD_LIGHTNING, this.Data.AddLightning);
			if (this.Data.AddEarth != 0) this.Properties.SetFloat(PropertyName.ADD_EARTH, this.Data.AddEarth);
			if (this.Data.AddSoul != 0) this.Properties.SetFloat(PropertyName.ADD_SOUL, this.Data.AddSoul);
			if (this.Data.AddHoly != 0) this.Properties.SetFloat(PropertyName.ADD_HOLY, this.Data.AddHoly);
			if (this.Data.AddDark != 0) this.Properties.SetFloat(PropertyName.ADD_DARK, this.Data.AddDark);
			if (this.Data.BaseSocket != 0) this.Properties.SetFloat(PropertyName.BaseSocket, this.Data.BaseSocket);
			if (this.Data.MaxSocketCount != 0) this.Properties.SetFloat(PropertyName.MaxSocket_COUNT, this.Data.MaxSocketCount);
			if (this.Data.BaseSocketMa != 0) this.Properties.SetFloat(PropertyName.BaseSocket_MA, this.Data.BaseSocketMa);
			if (this.Data.MaxSocketMa != 0) this.Properties.SetFloat(PropertyName.MaxSocket_MA, this.Data.MaxSocketMa);
			if (this.Data.MinOption != 0) this.Properties.SetFloat(PropertyName.MinOption, this.Data.MinOption);
			if (this.Data.MaxOption != 0) this.Properties.SetFloat(PropertyName.MaxOption, this.Data.MaxOption);
			if (this.Data.Aries != 0) this.Properties.SetFloat(PropertyName.Aries, this.Data.Aries);
			if (this.Data.AriesDefense != 0) this.Properties.SetFloat(PropertyName.AriesDEF, this.Data.AriesDefense);
			if (this.Data.Slash != 0) this.Properties.SetFloat(PropertyName.Slash, this.Data.Slash);
			if (this.Data.SlashDefense != 0) this.Properties.SetFloat(PropertyName.SlashDEF, this.Data.SlashDefense);
			if (this.Data.Strike != 0) this.Properties.SetFloat(PropertyName.Strike, this.Data.Strike);
			if (this.Data.StrikeDefense != 0) this.Properties.SetFloat(PropertyName.StrikeDEF, this.Data.StrikeDefense);
			if (this.Data.AriesRange != 0) this.Properties.SetFloat(PropertyName.Aries_Range, this.Data.AriesRange);
			if (this.Data.SlashRange != 0) this.Properties.SetFloat(PropertyName.Slash_Range, this.Data.SlashRange);
			if (this.Data.StrikeRange != 0) this.Properties.SetFloat(PropertyName.Strike_Range, this.Data.StrikeRange);
			if (this.Data.MinRDmg != 0) this.Properties.SetFloat(PropertyName.MinRDmg, this.Data.MinRDmg);
			if (this.Data.MaxRDmg != 0) this.Properties.SetFloat(PropertyName.MaxRDmg, this.Data.MaxRDmg);
			if (this.Data.FDMinR != 0) this.Properties.SetFloat(PropertyName.FDMinR, this.Data.FDMinR);
			if (this.Data.FDMaxR != 0) this.Properties.SetFloat(PropertyName.FDMaxR, this.Data.FDMaxR);
			if (this.Data.FireResistence != 0) this.Properties.SetFloat(PropertyName.RES_FIRE, this.Data.FireResistence);
			if (this.Data.IceResistence != 0) this.Properties.SetFloat(PropertyName.RES_ICE, this.Data.IceResistence);
			if (this.Data.PoisonResistence != 0) this.Properties.SetFloat(PropertyName.RES_POISON, this.Data.PoisonResistence);
			if (this.Data.LightningResistence != 0) this.Properties.SetFloat(PropertyName.RES_LIGHTNING, this.Data.LightningResistence);
			if (this.Data.EarthResistence != 0) this.Properties.SetFloat(PropertyName.RES_EARTH, this.Data.EarthResistence);
			if (this.Data.SoulResistence != 0) this.Properties.SetFloat(PropertyName.RES_SOUL, this.Data.SoulResistence);
			if (this.Data.HolyResistence != 0) this.Properties.SetFloat(PropertyName.RES_HOLY, this.Data.HolyResistence);
			if (this.Data.DarkResistence != 0) this.Properties.SetFloat(PropertyName.RES_DARK, this.Data.DarkResistence);
			if (this.Data.LifeTime != 0) this.Properties.SetFloat(PropertyName.LifeTime, this.Data.LifeTime);
			if (this.Data.ItemLifeTimeOver != 0) this.Properties.SetFloat(PropertyName.ItemLifeTimeOver, this.Data.ItemLifeTimeOver);
			if (this.Data.NeedAppraisal != 0) this.Properties.SetFloat(PropertyName.NeedAppraisal, this.Data.NeedAppraisal);
			if (this.Data.NeedRandomOption != 0) this.Properties.SetFloat(PropertyName.NeedRandomOption, this.Data.NeedRandomOption);
			if (this.Data.LootingChance != 0) this.Properties.SetFloat(PropertyName.LootingChance, this.Data.LootingChance);
			if (this.Data.IsAlwaysHatVisible != 0) this.Properties.SetFloat(PropertyName.IsAlwaysHatVisible, this.Data.IsAlwaysHatVisible);
			if (this.Data.SkillWidthRange != 0) this.Properties.SetFloat(PropertyName.SkillWidthRange, this.Data.SkillWidthRange);
			if (this.Data.DynamicLifeTime != 0) this.Properties.SetFloat(PropertyName.DynamicLifeTime, this.Data.DynamicLifeTime);
			if (this.Data.AddBossAtk != 0) this.Properties.SetFloat(PropertyName.ADD_BOSS_ATK, this.Data.AddBossAtk);
			if (this.Data.TeamBelonging != 0) this.Properties.SetFloat(PropertyName.TeamBelonging, this.Data.TeamBelonging);
			if (this.Data.AddDamageAtk != 0) this.Properties.SetFloat(PropertyName.Add_Damage_Atk, this.Data.AddDamageAtk);
			if (this.Data.MagicEarthAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Earth_Atk, this.Data.MagicEarthAtk);
			if (this.Data.ResAddDamage != 0) this.Properties.SetFloat(PropertyName.ResAdd_Damage, this.Data.ResAddDamage);
			if (this.Data.JobGrade != 0) this.Properties.SetFloat(PropertyName.JobGrade, this.Data.JobGrade);
			if (this.Data.MagicIceAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Ice_Atk, this.Data.MagicIceAtk);
			if (this.Data.MagicSoulAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Soul_Atk, this.Data.MagicSoulAtk);
			if (this.Data.MagicDarkAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Dark_Atk, this.Data.MagicDarkAtk);
			if (this.Data.MagicMeleeAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Melee_Atk, this.Data.MagicMeleeAtk);
			if (this.Data.MagicFireAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Fire_Atk, this.Data.MagicFireAtk);
			if (this.Data.MagicLightningAtk != 0) this.Properties.SetFloat(PropertyName.Magic_Lightning_Atk, this.Data.MagicLightningAtk);
			if (this.Data.Cooldown != 0) this.Properties.SetFloat(PropertyName.CoolDown, this.Data.Cooldown);

			if (this.Data.CardGroup != CardGroup.None)
			{
				this.Properties.SetString(PropertyName.CardGroupName, this.Data.CardGroup.ToString());
				this.Properties.SetFloat(PropertyName.CardLevel, this.Data.CardLevel);
			}

			if (this.Data.Group == ItemGroup.Gem)
			{
				this.Properties.SetFloat(PropertyName.Level, this.Data.GemLevel);
				var level = this.Data.GemLevel;
				// Our custom gems
				if (this.Data.EquipExpGroup == EquipExpGroup.Gem)
					this.UpdateGemStatOptions();
			}

			if (this.Data.Potential != 0) this.Properties.SetFloat(PropertyName.PR, this.Data.Potential);
			if (this.Data.MaxPotential != 0) this.Properties.SetFloat(PropertyName.MaxPR, this.Data.MaxPotential);

			if (this.Data.MaxSocketCount != 0) this.Properties.SetFloat(PropertyName.MaxSocket, this.Data.MaxSocketCount);
			if (this.Data.BaseSocket != 0) this.Properties.SetFloat(PropertyName.BaseSocket, this.Data.BaseSocket);

			if (this.Data.MaxDurability > 0)
			{
				this.Properties.SetFloat(PropertyName.MaxDur, this.Data.MaxDurability);
				if (this.Data.Name.StartsWith("Superior "))
					this.Properties.Modify(PropertyName.MaxDur, this.Data.MaxDurability * 1.50f);
				if (this.Data.Name.StartsWith("Old "))
					this.Properties.Modify(PropertyName.MaxDur, -this.Data.MaxDurability * .50f);
				if (!this.IsSaved && this.Durability == 0)
					this.Durability = this.MaxDurability;
			}
		}

		/// <summary>
		/// Migrates incorrectly set properties after loading from database.
		/// Fixes issues where GemLevel was incorrectly set on cards or gems.
		/// </summary>
		public void MigrateProperties()
		{
			// Check if the item has the incorrect GemLevel property set
			if (!this.Properties.TryGetFloat(PropertyName.GemLevel, out var gemLevelValue) || gemLevelValue == 0)
				return;

			// For cards, GemLevel is invalid - remove it and ensure CardLevel is set correctly
			if (this.Data.Group == ItemGroup.Card)
			{
				this.Properties.Remove(PropertyName.GemLevel);

				// Recalculate CardLevel from ItemExp if needed
				if (this.Properties.TryGetFloat(PropertyName.ItemExp, out var itemExp) && itemExp > 0)
				{
					this.GetItemLevelExp((int)itemExp, out var level, out var curExp, out var maxExp);
					this.Properties.SetFloat(PropertyName.CardLevel, level);
				}
			}
			// For gems, GemLevel should be Level - migrate it
			else if (this.Data.Group == ItemGroup.Gem)
			{
				this.Properties.Remove(PropertyName.GemLevel);

				// Set the correct Level property if not already set
				if (!this.Properties.TryGetFloat(PropertyName.Level, out var levelValue) || levelValue == 0)
				{
					if (this.Properties.TryGetFloat(PropertyName.ItemExp, out var itemExp) && itemExp > 0)
					{
						this.GetItemLevelExp((int)itemExp, out var level, out var curExp, out var maxExp);
						this.Properties.SetFloat(PropertyName.Level, level);
					}
					else
					{
						this.Properties.SetFloat(PropertyName.Level, gemLevelValue);
					}
				}

				// Update gem stat options if needed
				if (this.Data.EquipExpGroup == EquipExpGroup.Gem)
					this.UpdateGemStatOptions();
			}
			// For any other item type, just remove the invalid property
			else
			{
				this.Properties.Remove(PropertyName.GemLevel);
			}
		}

		/// <summary>
		/// Returns the item's index in the inventory, using the given
		/// index as an offset for the category the item belongs to.
		/// </summary>
		/// <example>
		/// item = Drug_HP1
		/// item.GetInventoryIndex(5) => 265001 + 5 = 265006
		/// </example>
		/// <remarks>
		/// The client uses index ranges for categorizing the items.
		/// For example:
		/// 45000~109999:  Equipment/MainWeapon
		/// 265000~274999: Item/Consumable
		/// 
		/// The server needs to put the items indices into the correct
		/// ranges for them to appear where they belong, otherwise a
		/// potion might be put into the equip category.
		/// </remarks>
		/// <param name="index"></param>
		/// <returns></returns>
		public int GetInventoryIndex(int index)
		{
			// If the category is none, use the index. This will put
			// the item well below the first category at index 5000
			// and effectively hide it.
			if (this.Data.Category == InventoryCategory.None)
				return index;

			// Get the base id from the database, add the index and return it.
			if (!ZoneServer.Instance.Data.InvBaseIdDb.TryFind(this.Data.Category, out var invBaseData))
				throw new MissingFieldException($"Category '{this.Data.Category}' on item '{this.Id}' not found in base id database.");

			return invBaseData.BaseId + index;
		}

		public ItemMonster Drop(Map map, Position position, long ownerId = 0)
		{
			var rnd = RandomProvider.Get();
			var direction = new Direction(rnd.Next(0, 360));
			var dropRadius = ZoneServer.Instance.Conf.World.DropRadius;
			var distance = rnd.Next(dropRadius / 2, dropRadius + 1);

			return Drop(map, position, direction, distance, ownerId);
		}

		/// <summary>
		/// Drops item to the map as an ItemMonster.
		/// </summary>
		/// <remarks>
		/// Items are typically dropped by "tossing" them from the source,
		/// such as a killed monster. The given position is the initial
		/// position, and the item is then tossed in the given direction,
		/// by the distance.
		/// </remarks>
		/// <param name="map">Map to drop to the item on.</param>
		/// <param name="position">Initial position of the drop item.</param>
		/// <param name="direction">Direction to toss the item in.</param>
		/// <param name="distance">Distance to toss the item.</param>
		public ItemMonster Drop(Map map, Position position, Direction direction, float distance, long ownerId, int layer = 0)
		{
			// ZC_NORMAL_ItemDrop animates the item flying from its
			// initial drop position to its final position. To keep
			// everything in sync, we use the monster's position as
			// the drop position, then add the item to the map,
			// and then make it fly and set the final position.
			// the direction of the item becomes the direction
			// it flies in.
			// FromGround is necessary for the client to attempt to
			// pick up the item. Might act as "IsYourDrop" for items.

			var itemMonster = ItemMonster.Create(this);
			var flyDropPos = position.GetRelative(direction, distance);

			// Override received position to guarantee drop will not be out
			// of bounds.
			if (!map.Ground.IsValidPosition(flyDropPos))
			{
				if (!map.Ground.TryGetNearestValidPosition(flyDropPos, out flyDropPos))
					flyDropPos = position;
				distance = 0;
			}

			if (ownerId != 0)
				itemMonster.UniqueName = $"{ownerId}";
			itemMonster.Position = position;
			itemMonster.Direction = direction;
			itemMonster.FromGround = true;
			itemMonster.DisappearTime = DateTime.Now.AddSeconds(ZoneServer.Instance.Conf.World.DropDisappearSeconds);
			itemMonster.Layer = layer;

			map.AddMonster(itemMonster);

			itemMonster.Position = flyDropPos;
			Send.ZC_NORMAL.ItemDrop(itemMonster, direction, distance);

			return itemMonster;
		}

		/// <summary>
		/// Drops item to the map as an ItemMonster.
		/// </summary>
		/// <remarks>
		/// Items are typically dropped by "tossing" them from the source,
		/// such as a killed monster. The given position is the initial
		/// position, and the item is then tossed in the given direction,
		/// by the distance.
		/// </remarks>
		/// <param name="map">Map to drop to the item on.</param>
		/// <param name="position">Initial position of the drop item.</param>
		/// <param name="direction">Direction to toss the item in.</param>
		/// <param name="distance">Distance to toss the item.</param>
		public ItemMonster Drop(Map map, Position position, Direction direction, float distance, int monsterLevel, int layer = 0)
		{
			// ZC_NORMAL_ItemDrop animates the item flying from its
			// initial drop position to its final position. To keep
			// everything in sync, we use the monster's position as
			// the drop position, then add the item to the map,
			// and then make it fly and set the final position.
			// the direction of the item becomes the direction
			// it flies in.
			// FromGround is necessary for the client to attempt to
			// pick up the item. Might act as "IsYourDrop" for items.

			var itemMonster = ItemMonster.Create(this, monsterLevel);
			var flyDropPos = position.GetRelative(direction, distance);

			// Override received position to guarantee drop will not be out
			// of bounds.
			if (!map.Ground.IsValidPosition(flyDropPos))
			{
				if (!map.Ground.TryGetNearestValidPosition(flyDropPos, out flyDropPos))
					flyDropPos = position;
				distance = 0;
			}

			itemMonster.Position = position;
			itemMonster.Direction = direction;
			itemMonster.FromGround = true;
			itemMonster.DisappearTime = DateTime.Now.AddSeconds(ZoneServer.Instance.Conf.World.DropDisappearSeconds);
			itemMonster.Layer = layer;

			map.AddMonster(itemMonster);

			itemMonster.Position = flyDropPos;
			Send.ZC_NORMAL.ItemDrop(itemMonster, direction, distance);

			return itemMonster;
		}

		/// <summary>
		/// Drops item to the map as an ItemMonster.
		/// </summary>
		/// <param name="map">Map to drop to the item on.</param>
		/// <param name="fromPosition">Initial position of the drop item.</param>
		/// <param name="toPosition">Position the item gets tossed to.</param>
		public ItemMonster Drop(Map map, Position fromPosition, Position toPosition, int layer = 0)
		{
			var direction = fromPosition.GetDirection(toPosition);
			var distance = (float)fromPosition.Get2DDistance(toPosition);

			return this.Drop(map, fromPosition, direction, distance, layer);
		}

		/// <summary>
		/// Activates the loot protection for the item if actor is set.
		/// Deactivates it if actor is null.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="protectionTime"></param>
		public void SetLootProtection(IActor actor, TimeSpan protectionTime)
		{
			if (actor == null)
			{
				this.OwnerHandle = -1;
				this.OwnerCharacterId = 0;
				this.LootProtectionEnd = DateTime.MinValue;
			}
			else
			{
				this.OwnerHandle = actor.Handle;
				this.LootProtectionEnd = DateTime.Now.Add(protectionTime);

				// If the actor is a character, also store their persistent ObjectId
				if (actor is Character character)
				{
					this.OwnerCharacterId = character.ObjectId;
				}
				else
				{
					this.OwnerCharacterId = 0;
				}
			}
		}

		/// <summary>
		/// Sets up a protection, so that the actor won't pick the item
		/// right back up.
		/// </summary>
		/// <param name="actor"></param>
		public void SetRePickUpProtection(IActor actor)
		{
			if (actor == null)
			{
				this.OriginalOwnerHandle = -1;
				this.OriginalOwnerCharacterId = 0;
				this.RePickUpTime = DateTime.MinValue;
			}
			else
			{
				this.OriginalOwnerHandle = actor.Handle;
				this.RePickUpTime = DateTime.Now.AddSeconds(2);

				// If the actor is a character, also store their persistent ObjectId
				if (actor is Character character)
				{
					this.OriginalOwnerCharacterId = character.ObjectId;
				}
				else
				{
					this.OriginalOwnerCharacterId = 0;
				}
			}
		}

		/// <summary>
		/// Clears protections, so the item can be picked up by anyone.
		/// </summary>
		/// <param name="entity"></param>
		public void ClearProtections()
		{
			this.SetLootProtection(null, TimeSpan.Zero);
			this.SetRePickUpProtection(null);
		}

		/// <summary>
		/// Modify the durability of an item and
		/// sends update to connection after modification.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="amount">If set to -1, the durability will be reset to max.</param>
		public void ModifyDurability(Character character, int amount = -1)
		{
			if (amount > 0 && this.Durability == this.MaxDurability)
				return;
			var isBroken = this.Durability + amount == 0;
			var isFixed = this.Durability + amount == this.MaxDurability;
			if (amount == -1)
				this.Durability = this.MaxDurability;
			else
				this.Durability += amount;
			Send.ZC_OBJECT_PROPERTY(character.Connection, this);
			if (isBroken || isFixed)
				character.InvalidateProperties();
		}

		public void Appraisal()
		{
			if (!this.NeedsAppraisal)
				return;
			this.Properties.SetFloat(PropertyName.NeedAppraisal, 0);
		}

		/// <summary>
		/// Updates the custom stats a gem gives
		/// </summary>
		/// <param name="minOptions"></param>
		/// <param name="maxOptions"></param>
		public void UpdateGemStatOptions()
		{
			switch (this.Id)
			{
				// red = STR
				case 643501:
					this.AddGemRandomOption(1, PropertyName.STR, "STAT", 3 * this.GemLevel);
					break;
				// blue = INT
				case 643502:
					this.AddGemRandomOption(1, PropertyName.INT, "STAT", 3 * this.GemLevel);
					break;
				// green = CON
				case 643503:
					this.AddGemRandomOption(1, PropertyName.CON, "STAT", 3 * this.GemLevel);
					break;
				// yellow = DEX
				case 643504:
					this.AddGemRandomOption(1, PropertyName.DEX, "STAT", 3 * this.GemLevel);
					break;
				// white = SPR
				case 643817:
					this.AddGemRandomOption(1, PropertyName.MNA, "STAT", 3 * this.GemLevel);
					break;
			}
		}

		/// <summary>
		/// Generate random options on an item.
		/// </summary>
		/// <param name="minOptions"></param>
		/// <param name="maxOptions"></param>
		public void GenerateRandomOptions(int minOptions = 1, int maxOptions = 5)
		{
			if (!this.NeedRandomOptions)
				return;

			this.Properties.SetFloat(PropertyName.NeedRandomOption, 0);
			var random = RandomProvider.Get();
			var options = random.Next(minOptions, maxOptions);
			var utilOptions = new string[] { "CRTHR", "CRTDR", "BLK_BREAK", "BLK", "ADD_HR", "ADD_DR", "RHP", "MSP" };
			var atkOptions = new string[] { "ADD_CLOTH", "ADD_LEATHER", "ADD_IRON", "ADD_SMALLSIZE", "ADD_MIDDLESIZE",
				"ADD_LARGESIZE", "ADD_GHOST", "ADD_FORESTER", "ADD_WIDLING", "ADD_VELIAS",
				"ADD_PARAMUNE", "ADD_KLAIDA" };
			var dmgOptions = new string[] { "Add_Damage_Atk" };
			var statOptions = new string[] { "LootingChance", "STR", "DEX", "CON", "INT", "MNA", "RSP" };
			var staminaOption = "MSTA"; // Not used, seems useless
										// TODO: Weighted Random Options
			for (var i = 0; i < options; i++)
			{
				switch (i)
				{
					case 0:
						this.AddRandomOption(i + 1, atkOptions[random.Next(0, atkOptions.Length)], "ATK", ((this.Data.Group == ItemGroup.Weapon || this.Data.Group == ItemGroup.SubWeapon) ? random.Next(565, 1132) : random.Next(302, 605)));
						break;
					case 1:
						this.AddRandomOption(i + 1, utilOptions[random.Next(0, utilOptions.Length)],
							"UTIL_" + ((this.Data.Group == ItemGroup.Weapon || this.Data.Group == ItemGroup.SubWeapon) ? "WEAPON" : "ARMOR"),
							(this.Data.Group == ItemGroup.Weapon || this.Data.Group == ItemGroup.SubWeapon) ? random.Next(283, 567) : random.Next(131, 303));
						break;
					case 2:
						this.AddRandomOption(i + 1, dmgOptions[random.Next(0, dmgOptions.Length)], "ATK",
							(this.Data.Group == ItemGroup.Weapon || this.Data.Group == ItemGroup.SubWeapon) ? random.Next(565, 1132) : random.Next(302, 605));
						break;
					case 3:
						this.AddRandomOption(i + 1, statOptions[random.Next(0, statOptions.Length)], "STAT",
							(this.Data.Group == ItemGroup.Weapon || this.Data.Group == ItemGroup.SubWeapon) ? random.Next(85, 171) : random.Next(45, 92));
						break;
				}
			}
		}

		/// <summary>
		/// Generates random options based on the item's grade and level.
		/// </summary>
		public void GenerateGradeBasedRandomOptions()
		{
			if (!this.NeedRandomOptions)
				return;

			this.Properties.SetFloat(PropertyName.NeedRandomOption, 0);
			var random = RandomProvider.Get();
			var itemGrade = (ItemGrade)this.Properties.GetFloat(PropertyName.ItemGrade);

			if (itemGrade <= ItemGrade.Normal)
				return;

			var level = this.Level;
			var itemGradeModifier = 1 + ((int)itemGrade - 1) * 0.05f;

			// Define chances of multiple status based on itemGrade
			var numStatChances = new List<float>();
			switch (itemGrade)
			{
				case ItemGrade.Magic:
					numStatChances = new List<float> { 0.80f, 0.20f, 0f, 0f };
					break;
				case ItemGrade.Rare:
					numStatChances = new List<float> { 0.20f, 0.80f, 0f, 0f };
					break;
				case ItemGrade.Unique:
					numStatChances = new List<float> { 0f, 0.50f, 0.50f, 0f };
					break;
				case ItemGrade.Legend:
					numStatChances = new List<float> { 0f, 0f, 0.80f, 0.20f };
					break;
				case ItemGrade.Goddess:
					numStatChances = new List<float> { 0f, 0f, 0.20f, 0.80f };
					break;
				default:
					numStatChances = new List<float> { 1.0f, 0f, 0f, 0f };
					break;
			}

			// Defines what stats this item can have
			var availableStats = this.GetPossibleEquipStats();

			// Normalize array
			var sum = numStatChances.Sum();
			for (var i = 0; i < numStatChances.Count; i++)
			{
				numStatChances[i] /= sum;
			}

			// Determine number of stats to choose
			var numStats = 1;
			var roll = (float)random.NextDouble();
			for (var i = 0; i < numStatChances.Count; i++)
			{
				if (roll <= numStatChances[i])
				{
					numStats = i + 1;
					break;
				}
				roll -= numStatChances[i];
			}

			var chosenStats = new List<string>();
			var statAdded = new Dictionary<string, int>();
			var highestStat = "";
			var highestValue = float.MinValue;

			for (var k = 0; k < numStats && availableStats.Count > 0; k++)
			{
				// Choose a random stat from the remaining available stats
				var statIndex = random.Next(availableStats.Count);
				var chosenStat = availableStats[statIndex];
				availableStats.RemoveAt(statIndex);

				chosenStats.Add(chosenStat);

				var rndValue = this.GenerateRandomStatValue(chosenStat, level, itemGrade, out var rngModifier);
				if (rngModifier > highestValue)
				{
					highestValue = rngModifier;
					highestStat = chosenStat;
				}

				if (!statAdded.TryGetValue(chosenStat, out var value))
				{
					this.AddRandomOption(k + 1, chosenStat, "STAT", rndValue);
					statAdded.Add(chosenStat, k + 1);
				}
				else
				{
					this.ModifyRandomOption(value, chosenStat, "STAT", rndValue);
				}
			}

			if (highestStat != null)
			{
				var affixName = AffixGenerator.GetAffixName(highestStat, itemGrade, chosenStats.Count);
				this.Properties.SetString(PropertyName.CustomName, $"{affixName}");
			}
		}

		/// <summary>
		/// Returns array of property names that are available for this item
		/// </summary>
		/// <returns></returns>
		private List<string> GetPossibleEquipStats()
		{
			var isAccessory = this.Data.EquipType1 == EquipType.Ring || this.Data.EquipType1 == EquipType.Neck;
			var isWeapon = this.Data.Group == ItemGroup.Weapon;
			var isShield = this.Data.EquipType1 == EquipType.Shield;
			var isArmor = this.Data.Group == ItemGroup.Armor;

			if (isAccessory)
			{
				return new List<string> {
					"STR", "DEX", "CON", "INT", "MNA",
				};
			}
			else if (isShield)
			{
				return new List<string> {
					"STR", "DEX", "CON", "INT", "MNA",
					"RHP", "RSP",
					"CRTDR",
					"BLK",
					"ADD_DEF", "ADD_MDEF",
					"MHP", "MSP",
					"SlashDEF", "AriesDEF", "StrikeDEF",
					"RES_FIRE", "RES_ICE", "RES_LIGHTNING", "RES_POISON", "RES_EARTH", "RES_HOLY", "RES_DARK", "RES_SOUL",
				};
			}
			else if (isWeapon)
			{
				switch (this.Data.EquipType1)
				{
					case EquipType.THSword:
					case EquipType.Sword:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTATK",
							"ADD_DR", "BLK",
							"ADD_DEF",
							"MHP",
							"Slash",
							"ADD_MIDDLESIZE",
						};

					case EquipType.Dagger:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTATK",
							"ADD_HR", "ADD_DR",
							"ADD_MATK",
							"Aries",
							"ADD_SMALLSIZE",
						};

					case EquipType.Staff:
					case EquipType.THStaff:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"RSP",
							"ADD_MATK", "ADD_MDEF",
							"MSP",
							"ADD_FIRE", "ADD_ICE", "ADD_LIGHTNING", "ADD_POISON", "ADD_EARTH", "ADD_HOLY", "ADD_DARK", "ADD_SOUL",
						};

					case EquipType.Bow:
					case EquipType.THBow:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTATK",
							"ADD_HR", "ADD_DR",
							"PATK",
							"ADD_VELIAS", "ADD_WIDLING", "ADD_PARAMUNE", "ADD_FORESTER", "ADD_KLAIDA",
						};

					case EquipType.Mace:
					case EquipType.THMace:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"RSP",
							"BLK", "BLK_BREAK",
							"ADD_MATK", "ADD_DEF", "ADD_MDEF",
							"MHP",
							"Strike",
						};

					case EquipType.Spear:
					case EquipType.THSpear:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTATK",
							"ADD_HR", "BLK_BREAK",
							"ADD_DEF",
							"MHP",
							"Aries",
							"ADD_LARGESIZE",
						};

					case EquipType.Rapier:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTDR", "CRTATK",
							"ADD_HR", "ADD_DR",
							"Aries",
							"ADD_MIDDLESIZE",
						};

					case EquipType.Pistol:
					case EquipType.Gun:
					case EquipType.Musket:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"CRTHR", "CRTATK",
							"ADD_DR", "BLK_BREAK",
							"PATK",
							"ADD_SMALLSIZE", "ADD_MIDDLESIZE", "ADD_LARGESIZE",
						};

					case EquipType.Cannon:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"BLK_BREAK",
							"PATK", "ADD_DEF",
							"MHP",
							"ADD_SMALLSIZE", "ADD_MIDDLESIZE", "ADD_LARGESIZE",
						};

					case EquipType.Trinket:
						return new List<string> {
							"STR", "DEX", "CON", "INT", "MNA",
							"RHP", "RSP",
							"CRTHR", "CRTDR", "CRTATK",
							"ADD_HR", "ADD_DR", "BLK_BREAK",
							"PATK", "ADD_MATK", "ADD_DEF", "ADD_MDEF",
							"MHP", "MSP",
							"ADD_FIRE", "ADD_ICE", "ADD_LIGHTNING", "ADD_POISON", "ADD_EARTH", "ADD_HOLY", "ADD_DARK", "ADD_SOUL",
						};
				}
			}
			else if (isArmor)
			{
				return new List<string> {
					"STR", "DEX", "CON", "INT", "MNA",
					"RHP", "RSP",
					"CRTDR",
					"ADD_DR",
					"ADD_DEF", "ADD_MDEF",
					"MHP", "MSP",
					"SlashDEF", "AriesDEF", "StrikeDEF",
					"RES_FIRE", "RES_ICE", "RES_LIGHTNING", "RES_POISON", "RES_EARTH", "RES_HOLY", "RES_DARK", "RES_SOUL",
				};
			}

			// Defaults to all stats
			return new List<string>
			{
				"STR", "DEX", "CON", "INT", "MNA",
				"RHP", "RSP",
				"CRTHR", "CRTDR", "CRTATK",
				"ADD_HR", "ADD_DR", "BLK", "BLK_BREAK",
				"PATK", "ADD_MATK", "ADD_DEF", "ADD_MDEF",
				"MHP", "MSP",
				"Slash", "SlashDEF", "Aries", "AriesDEF", "Strike", "StrikeDEF",
				"ADD_SMALLSIZE", "ADD_MIDDLESIZE", "ADD_LARGESIZE",
				"ADD_VELIAS", "ADD_WIDLING", "ADD_PARAMUNE", "ADD_FORESTER", "ADD_KLAIDA",
				"ADD_FIRE", "ADD_ICE", "ADD_LIGHTNING", "ADD_POISON", "ADD_EARTH", "ADD_HOLY", "ADD_DARK", "ADD_SOUL",
				"RES_FIRE", "RES_ICE", "RES_LIGHTNING", "RES_POISON", "RES_EARTH", "RES_HOLY", "RES_DARK", "RES_SOUL",
			};
		}

		/// <summary>
		/// Returns a random value for a property considering an item level
		/// and item grade modifier
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="itemLevel"></param>
		/// <param name="itemGradeModifier"></param>
		/// <returns></returns>
		private float GenerateRandomStatValue(string propertyName, float itemLevel, ItemGrade itemGrade, out float rngModifier)
		{
			var random = RandomProvider.Get();

			// Natural oscilation of -20% to +20% to stat value
			rngModifier = (float)(random.NextDouble() * 0.4 - 0.2);

			var rndValue = 0f;

			// +5% Bonus per item grade above Normal
			var itemGradeModifier = 1 + ((int)itemGrade - 1) * 0.05f;
			var baseValue = 0f;
			var byLevel = 0f;
			var byRng = 1 + rngModifier;

			switch (propertyName)
			{

				case "STR":
				case "CON":
				case "INT":
				case "DEX":
				case "MNA":
					baseValue = 2.5f * itemGradeModifier;
					byLevel = (itemLevel / 8f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "RHP":
				case "RSP":
					baseValue = 15f * itemGradeModifier;
					byLevel = (itemLevel / 2f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "CRTHR":
				case "CRTDR":
					baseValue = 1 * itemGradeModifier;
					byLevel = (itemLevel / 8f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "CRTATK":
					baseValue = 10f * itemGradeModifier;
					byLevel = (itemLevel / 3f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_HR":
				case "ADD_DR":
					baseValue = 5f * itemGradeModifier;
					byLevel = (itemLevel / 4f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "BLK_BREAK":
				case "BLK":
					baseValue = 5f * itemGradeModifier;
					byLevel = (itemLevel / 4f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "PATK": // Yes, it's not ADD_PATK
				case "ADD_MATK":
					baseValue = 5f * itemGradeModifier;
					byLevel = (itemLevel / 3f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_DEF":
				case "ADD_MDEF":
					baseValue = 6.5f * itemGradeModifier;
					byLevel = (itemLevel / 2.5f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "Aries":
				case "Slash":
				case "Strike":
					baseValue = 5f * itemGradeModifier;
					byLevel = (itemLevel / 3f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "AriesDEF":
				case "SlashDEF":
				case "StrikeDEF":
					baseValue = 7.5f * itemGradeModifier;
					byLevel = (itemLevel / 2f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_SMALLSIZE":
				case "ADD_MIDDLESIZE":
				case "ADD_LARGESIZE":
					baseValue = 7.5f * itemGradeModifier;
					byLevel = (itemLevel / 2f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_VELIAS":
				case "ADD_WIDLING":
				case "ADD_PARAMUNE":
				case "ADD_FORESTER":
				case "ADD_KLAIDA":
					baseValue = 12.5f * itemGradeModifier;
					byLevel = (itemLevel) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_FIRE":
				case "ADD_ICE":
				case "ADD_LIGHTNING":
				case "ADD_POISON":
				case "ADD_EARTH":
				case "ADD_HOLY":
				case "ADD_DARK":
				case "ADD_SOUL":
					baseValue = 5.0f * itemGradeModifier;
					byLevel = (itemLevel / 3f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "RES_FIRE":
				case "RES_ICE":
				case "RES_LIGHTNING":
				case "RES_POISON":
				case "RES_EARTH":
				case "RES_HOLY":
				case "RES_DARK":
				case "RES_SOUL":
					baseValue = 7.5f * itemGradeModifier;
					byLevel = (itemLevel / 2f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "MHP":
				case "MSP":
					baseValue = 80 * itemGradeModifier;
					byLevel = (itemLevel * 4f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "ADD_CLOTH":
				case "ADD_LEATHER":
				case "ADD_IRON":
				case "ADD_GHOST":
					baseValue = 7.5f * itemGradeModifier;
					byLevel = (itemLevel / 2f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "Add_Damage_Atk":
					baseValue = 5f * itemGradeModifier;
					byLevel = (itemLevel / 3f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "LootingChance":
					baseValue = 1f * itemGradeModifier;
					byLevel = (itemLevel / 30f) * itemGradeModifier;
					rndValue = baseValue + (byLevel * byRng);
					break;

				case "SR":
					rndValue = 1;
					break;

				default:
					break;
			}


			// Reduce values depending on equip type
			switch (this.Data.EquipType1)
			{
				case EquipType.Shirt:
				case EquipType.Pants:
					rndValue /= 2.0f;
					break;

				case EquipType.Gloves:
				case EquipType.Boots:
					rndValue /= 3.0f;
					break;

				case EquipType.Ring:
				case EquipType.Neck:
				case EquipType.Hat:
				case EquipType.Armband:
					if (propertyName.Contains("RES", StringComparison.OrdinalIgnoreCase))
						rndValue /= 2.5f;
					else
						rndValue /= 5.0f;
					break;
			}

			// Increase values if it's a trinket
			if (this.Data.Group == ItemGroup.SubWeapon && this.Data.EquipType1 == EquipType.Trinket)
			{
				rndValue *= 2.0f;
			}

			rndValue = Math.Max(1, (float)Math.Round(rndValue));
			return rndValue;
		}

		/// <summary>
		/// Add a random option to an item.
		/// </summary>
		/// <param name="optionIndex"></param>
		/// <param name="optionType"></param>
		/// <param name="optionGroup"></param>
		/// <param name="optionValue"></param>
		private void AddRandomOption(int optionIndex, string optionType, string optionGroup, float optionValue)
		{
			var optionPropId = string.Format("RandomOption_{0}", optionIndex);
			var optionPropGroup = string.Format("RandomOptionGroup_{0}", optionIndex);
			var optionPropValue = string.Format("RandomOptionValue_{0}", optionIndex);

			this.Properties.SetString(optionPropId, optionType);
			this.Properties.SetString(optionPropGroup, optionGroup);
			this.Properties.SetFloat(optionPropValue, optionValue);

			this.Properties.Modify(optionType, optionValue);
		}

		/// <summary>
		/// Add a random option to a gem.
		/// For some reason, gems cannot have the properties directly on them.
		/// For example, +5 STR should only be properties:
		/// RandomOption_STR and RandomOptionValue_5
		/// If it also has 'STR' property in it, the gem will not be able
		/// to level up.
		/// </summary>
		/// <param name="optionIndex"></param>
		/// <param name="optionType"></param>
		/// <param name="optionGroup"></param>
		/// <param name="optionValue"></param>
		private void AddGemRandomOption(int optionIndex, string optionType, string optionGroup, float optionValue)
		{
			var optionPropId = string.Format("RandomOption_{0}", optionIndex);
			var optionPropGroup = string.Format("RandomOptionGroup_{0}", optionIndex);
			var optionPropValue = string.Format("RandomOptionValue_{0}", optionIndex);

			this.Properties.SetString(optionPropId, optionType);
			this.Properties.SetString(optionPropGroup, optionGroup);
			this.Properties.SetFloat(optionPropValue, optionValue);
		}

		/// <summary>
		/// Removes a random option to an item.
		/// </summary>
		/// <param name="optionIndex"></param>
		private void RemoveRandomOption(int optionIndex)
		{
			var optionPropId = string.Format("RandomOption_{0}", optionIndex);
			var optionPropGroup = string.Format("RandomOptionGroup_{0}", optionIndex);
			var optionPropValue = string.Format("RandomOptionValue_{0}", optionIndex);

			if (!this.Properties.TryGetString(optionPropId, out var optionType))
				return;
			if (!this.Properties.TryGetFloat(optionPropGroup, out var optionGroup))
				return;
			if (!this.Properties.TryGetFloat(optionPropValue, out var optionValue))
				return;

			this.Properties.Remove(optionPropId);
			this.Properties.Remove(optionPropGroup);
			this.Properties.Remove(optionPropValue);
			this.Properties.Modify(optionType, -optionValue);
		}

		/// <summary>
		/// Modify a random option to an item.
		/// </summary>
		/// <param name="optionIndex"></param>
		/// <param name="optionType"></param>
		/// <param name="optionGroup"></param>
		/// <param name="optionValue"></param>
		private void ModifyRandomOption(int optionIndex, string optionType, string optionGroup, float optionValue)
		{
			var optionPropId = string.Format("RandomOption_{0}", optionIndex);
			var optionPropGroup = string.Format("RandomOptionGroup_{0}", optionIndex);
			var optionPropValue = string.Format("RandomOptionValue_{0}", optionIndex);

			this.Properties.SetString(optionPropId, optionType);
			this.Properties.SetString(optionPropGroup, optionGroup);
			this.Properties.Modify(optionPropValue, optionValue);

			this.Properties.Modify(optionType, optionValue);
		}

		/// <summary>
		/// Generates random options for headgear (hats).
		/// Uses HatPropName_X and HatPropValue_X properties.
		/// </summary>
		public void GenerateRandomHatOptions(int minOptions = 1, int maxOptions = 2)
		{
			var random = RandomProvider.Get();
			var options = random.Next(minOptions, maxOptions);
			var utilOptions = new string[] { "CRTHR", "CRTDR", "BLK_BREAK", "BLK", "ADD_HR", "ADD_DR", "RHP", "SR" };
			var atkOptions = new string[] { "ADD_CLOTH", "ADD_LEATHER", "ADD_IRON", "ADD_SMALLSIZE", "ADD_MIDDLESIZE",
				"ADD_LARGESIZE", "ADD_GHOST", "ADD_FORESTER", "ADD_WIDLING", "ADD_VELIAS",
				"ADD_PARAMUNE", "ADD_KLAIDA" };
			var statOptions = new string[] { "Add_Damage_Atk", "LootingChance", "STR", "DEX", "CON", "INT", "MNA", "RSP" };

			// Treat as fixed level for stat calculation
			var itemLevel = 90f;
			var itemGrade = (ItemGrade)this.Properties.GetFloat(PropertyName.ItemGrade);

			for (var i = 0; i < options; i++)
			{
				var chosenStat = i switch
				{
					0 => atkOptions[random.Next(0, atkOptions.Length)],
					1 => utilOptions[random.Next(0, utilOptions.Length)],
					_ => statOptions[random.Next(0, statOptions.Length)]
				};

				var statValue = this.GenerateRandomStatValue(chosenStat, itemLevel, itemGrade, out _);
				this.AddOption("HatProp", i + 1, chosenStat, statValue);
			}
		}

		/// <summary>
		/// Add an option to the item.
		/// </summary>
		/// <param name="optionPrefix"></param>
		/// <param name="optionIndex"></param>
		/// <param name="optionPropertyName"></param>
		/// <param name="optionValue"></param>
		private void AddOption(string optionPrefix, int optionIndex, string optionPropertyName, float optionValue)
		{
			var nameProp = string.Format("{0}Name_{1}", optionPrefix, optionIndex);
			var valueProp = string.Format("{0}Value_{1}", optionPrefix, optionIndex);

			// Reset previous value if it exists
			var optionName = this.Properties.GetString(nameProp);
			if (optionName != null && optionName != "None")
			{
				var prevValue = this.Properties.GetFloat(optionName);
				// Can we just set it to 0, rather than modify it?
				this.Properties.Modify(optionName, -prevValue);
			}

			this.Properties.SetString(nameProp, optionPropertyName);
			this.Properties.SetFloat(valueProp, optionValue);
			this.Properties.SetFloat(optionPropertyName, optionValue);
		}

		/// <summary>
		/// Gets number of used sockets
		/// </summary>
		/// <returns></returns>
		public int GetUsedSockets()
		{
			var socketsUsed = 0;
			for (var i = 0; i < this.MaxSockets; i++)
			{
				if (this.Properties[$"Socket_{i}"] != 0)
					socketsUsed++;
			}
			return socketsUsed;
		}

		/// <summary>
		/// Finds index of next free socket
		/// </summary>
		/// <returns></returns>
		public int GetNextFreeSocket()
		{
			for (var i = 0; i < this.MaxSockets; i++)
			{
				if (this.Properties.GetFloat($"Socket_{i}") == 0)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Returns all sockets, with or without gems
		/// </summary>
		/// <returns></returns>
		public IList<Item> GetGemSockets()
		{
			lock (_gemSockets)
			{
				return _gemSockets;
			}
		}

		/// <summary>
		/// Returns all sockets that have gems in them
		/// </summary>
		/// <returns></returns>
		public IList<Item> GetUsedGemSockets()
		{
			lock (_gemSockets)
			{
				var list = new List<Item>();
				for (var i = 0; i < this.MaxSockets; i++)
				{
					if (this.Properties.GetFloat($"Socket_{i}") != 0)
					{
						var gem = this.GetGemAtSocket(i);
						if (gem != null)
							list.Add(gem);
					}
				}
				return list;
			}
		}

		/// <summary>
		/// Copies the sockets and gems from 'other' into this item.
		/// </summary>
		/// <param name="other"></param>
		public void CopyGemSockets(Item other)
		{
			if (other == null)
				return;

			lock (this._gemSockets)
			{
				lock (other._gemSockets)
				{
					this._gemSockets.Clear();
					for (var i = 0; i < other._gemSockets.Count; i++)
					{
						var gem = other._gemSockets[i];
						if (gem != null)
						{
							// Create a copy of the gem
							var gemCopy = new Item(gem);
							this._gemSockets.Add(gemCopy);
						}
						else
						{
							// Add empty socket
							this._gemSockets.Add(null);
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates a socket at given index
		/// </summary>
		/// <param name="index"></param>
		public void CreateSocket(int index)
		{
			lock (_gemSockets)
			{
				if (!this.Properties.TryGetFloat(PropertyName.Reinforce_2, out _))
					this.Properties.SetFloat(PropertyName.Reinforce_2, 0);
				this.Properties.SetFloat($"Socket_{index}", 5);
				_gemSockets.Add(null);
			}
		}

		/// <summary>
		/// Adds a gem to a socketed item
		/// </summary>
		/// <param name="gem"></param>
		public void SocketGem(Item gem)
		{
			lock (_gemSockets)
			{
				var index = -1;
				for (var i = 0; i < _gemSockets.Count; i++)
				{
					if (_gemSockets[i] == null)
					{
						_gemSockets[i] = gem;
						index = i;
						break;
					}
				}

				if (index == -1)
					return;

				this.Properties.SetFloat($"Socket_Equip_{index}", gem.Id);
				this.Properties.SetFloat($"SocketItemExp_{index}", gem.Exp);
				this.Properties.SetFloat($"Socket_JamLv_{index}", gem.GemRoastingLevel);
				this.Properties.SetFloat($"Socket_GemBelongingCount_{index}", gem.Properties.GetFloat(PropertyName.BelongingCount));
			}
		}

		/// <summary>
		/// Adds a gem to a socketed item at given socket index
		/// </summary>
		/// <param name="socket"></param>
		/// <param name="gem"></param>
		public void SocketGemAt(int socket, Item gem)
		{
			lock (_gemSockets)
			{
				_gemSockets.Insert(socket, gem);

				this.Properties.SetFloat($"Socket_Equip_{socket}", gem.Id);
				this.Properties.SetFloat($"SocketItemExp_{socket}", gem.Exp);
				this.Properties.SetFloat($"Socket_JamLv_{socket}", gem.GemRoastingLevel);
				this.Properties.SetFloat($"Socket_GemBelongingCount_{socket}", gem.Properties.GetFloat(PropertyName.BelongingCount));
			}
		}

		/// <summary>
		/// Gets the gem at given socket index.
		/// Returns null if index is an empty socket.
		/// </summary>
		/// <param name="selectedSlot"></param>
		/// <returns></returns>
		public Item GetGemAtSocket(int selectedSlot)
		{
			lock (_gemSockets)
			{
				if (selectedSlot < 0 || selectedSlot >= _gemSockets.Count)
					return null;
				return _gemSockets[selectedSlot];
			}
		}

		/// <summary>
		/// Removes the gem at given socket index
		/// </summary>
		/// <param name="selectedSlot"></param>
		public void RemoveGemAtSocket(int selectedSlot)
		{
			lock (_gemSockets)
			{
				if (selectedSlot < 0 || selectedSlot >= _gemSockets.Count)
					return;
				_gemSockets[selectedSlot] = null;
			}
		}

		/// <summary>
		/// Creates a new calculated float property that uses the given
		/// function.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="calcFuncName"></param>
		private void Create(string propertyName, string calcFuncName)
		{
			this.Properties.Create(new CFloatProperty(propertyName, () => this.CalculateProperty(calcFuncName)));
		}

		/// <summary>
		/// Calls the calculation function with the given name and returns
		/// the result.
		/// </summary>
		/// <param name="calcFuncName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">
		/// Thrown if the function doesn't exist.
		/// </exception>
		private float CalculateProperty(string calcFuncName)
		{
			if (!ScriptableFunctions.ItemCalc.TryGet(calcFuncName, out var func))
				throw new ArgumentException($"Scriptable monster function '{calcFuncName}' not found.");

			return func(this);
		}

		public EquipSlot GetEquipSlot()
		{
			switch (this.Data.EquipType1)
			{
				case EquipType.Sword:
				case EquipType.THSword:
				case EquipType.Dagger:
				case EquipType.Staff:
				case EquipType.THStaff:
				case EquipType.Bow:
				case EquipType.THBow:
				case EquipType.Mace:
				case EquipType.THMace:
				case EquipType.Spear:
				case EquipType.THSpear:
				case EquipType.Rapier:
				case EquipType.Musket:
				case EquipType.Gun:
				case EquipType.Pistol:
				case EquipType.Cannon:
					return EquipSlot.LeftHand;
				case EquipType.Shield:
				case EquipType.Trinket:
					return EquipSlot.RightHand;
				case EquipType.Shirt:
					return EquipSlot.Top;
				case EquipType.Pants:
					return EquipSlot.Pants;
				case EquipType.Gloves:
					return EquipSlot.Gloves;
				case EquipType.Boots:
					return EquipSlot.Shoes;
			}

			return EquipSlot.None;
		}
	}
}
