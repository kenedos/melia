//--- Melia Script ----------------------------------------------------------
// Refining Items (Moru - Anvil)
//--- Description -----------------------------------------------------------
// Item-related scripts that refine items.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;

public class RefiningItemScripts : GeneralScript
{
	private static string SofS(string originalName, string characterName)
	{
		return ScpArgMsg("{Auto_1}_of_{Auto_2}", "Auto_1", characterName, "Auto_2", originalName);
	}

	private static void SetupAnvil(Mob mon, int hitCount, string pcName)
	{
		mon.Faction = FactionType.HitMe;
		mon.Properties.SetFloat(PropertyName.HitCount, hitCount);
		mon.Name = SofS(mon.Data.Name, pcName);
	}

	private static BuffId CheckBuffs(Character character)
	{
		if (character.IsBuffActive(BuffId.Forgery_Buff))
			return BuffId.Forgery_Buff;

		if (character.IsBuffActive(BuffId.OverEstimate_Buff))
			return BuffId.OverEstimate_Buff;

		if (character.IsBuffActive(BuffId.OverReinforce_Buff))
			return BuffId.OverReinforce_Buff;


		if (character.IsBuffActive(BuffId.Devaluation_Debuff))
			return BuffId.Devaluation_Debuff;

		return BuffId.None;
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_REINFORCE_131014(Character character, DialogTxArgs args)
	{
		if (character.Map.IsGTW || ZoneServer.Instance.World.IsPVP)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Cannot reinforce in GTW/PVP map. Map: {0}, IsGTW: {1}, IsPVP: {2}",
				character.Map.Data.Name, character.Map.IsGTW, ZoneServer.Instance.World.IsPVP);
			return DialogTxResult.Fail;
		}

		var itemList = args.TxItems;
		if (itemList == null || itemList.Length != 2)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Invalid item list. ItemList is null: {0}, Length: {1} (expected 2)",
				itemList == null, itemList?.Length ?? 0);
			return DialogTxResult.Fail;
		}

		if (character.Variables.Temp.Get<Mob>("Melia.Anvil") != null)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Blocked - Character '{0}' already has an active anvil.", character.Name);
			character.SystemMessage("CannotCreateMoru");
			return DialogTxResult.Okay;
		}

		var fromItem = itemList[0].Item;
		var anvilItem = itemList[1].Item;

		Log.Debug("SCR_ITEM_REINFORCE_131014: Attempting reinforce - Character: {0}, Item: {1} (Id: {2}, ObjectId: {3}), Anvil: {4} (Id: {5}, ObjectId: {6})",
			character.Name,
			fromItem.Data.ClassName, fromItem.Data.Id, fromItem.ObjectId,
			anvilItem.Data.ClassName, anvilItem.Data.Id, anvilItem.ObjectId);

		if (!anvilItem.Data.ClassName.StartsWith("Moru_"))
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Anvil item class name '{0}' does not start with 'Moru_'",
				anvilItem.Data.ClassName);
			return DialogTxResult.Fail;
		}

		if (fromItem.IsLocked || anvilItem.IsLocked || fromItem.Refinforcing || anvilItem.Refinforcing)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Item lock/reinforcing state. FromItem.IsLocked: {0}, AnvilItem.IsLocked: {1}, FromItem.Refinforcing: {2}, AnvilItem.Refinforcing: {3}",
				fromItem.IsLocked, anvilItem.IsLocked, fromItem.Refinforcing, anvilItem.Refinforcing);
			return DialogTxResult.Fail;
		}

		if (!IsReinforceAble(fromItem, anvilItem))
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - IsReinforceAble returned false. Item: {0}, Type: {1}, ReinforceType: {2}, MainProps: {3}, Reinforce_2: {4}, LifeTime: {5}, ItemStrArg: {6}, ItemStrArg2: {7}, AnvilStrArg: {8}, AnvilStrArg2: {9}",
				fromItem.Data.ClassName,
				fromItem.Data.Type,
				fromItem.Data.ReinforceType,
				fromItem.Data.MainProperties ?? "null",
				fromItem.Properties.GetFloat(PropertyName.Reinforce_2, 0),
				fromItem.Properties.GetFloat(PropertyName.LifeTime),
				fromItem.Data?.Script?.StrArg ?? "null",
				fromItem.Data?.Script?.StrArg2 ?? "null",
				anvilItem.Data?.Script?.StrArg ?? "null",
				anvilItem.Data?.Script?.StrArg2 ?? "null");
			return DialogTxResult.Fail;
		}

		if (!IsReinforceAbleByUseLevel(anvilItem, fromItem))
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - IsReinforceAbleByUseLevel returned false. Anvil: {0} (Id: {1}), Item UseLevel: {2}",
				anvilItem.Data.ClassName, anvilItem.Data.Id, fromItem.UseLevel);
			return DialogTxResult.Fail;
		}

		if (fromItem.NeedsAppraisal || fromItem.NeedRandomOptions)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Item needs appraisal. NeedsAppraisal: {0}, NeedRandomOptions: {1}",
				fromItem.NeedsAppraisal, fromItem.NeedRandomOptions);
			character.SystemMessage("NeedAppraisd");
			return DialogTxResult.Fail;
		}

		var buffId = CheckBuffs(character);
		if (buffId != BuffId.None)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Character has blocking buff: {0}", buffId);
			if (ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffCls))
				character.SystemMessage("CannotReinforceAndTranscendBy{BUFFNAME}", new MsgParameter("BUFFNAME", buffCls.Name));
			return DialogTxResult.Fail;
		}

		// Checks if using an anvil meant for 0-potential items on an item that still has potential.
		if (IsAnvilForZeroPotential(anvilItem) && fromItem.Potential > 0)
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Zero-potential anvil used on item with potential. Anvil: {0}, Item Potential: {1}",
				anvilItem.Data.ClassName, fromItem.Potential);
			return DialogTxResult.Fail;
		}

		var totalPrice = GetReinforcePrice(fromItem, anvilItem, character);
		if (!character.HasSilver(totalPrice))
		{
			Log.Debug("SCR_ITEM_REINFORCE_131014: Failed - Insufficient silver. Required: {0}", totalPrice);
			return DialogTxResult.Fail;
		}

		Log.Debug("SCR_ITEM_REINFORCE_131014: All checks passed. Creating anvil for character '{0}', price: {1}", character.Name, totalPrice);

		var script = string.Format(ClientScripts.REINFORCE_131014_ITEM_LOCK, anvilItem.ObjectId, fromItem.ObjectId);
		character.ExecuteClientScript(script);

		var hitCount = 3;

		var position = character.Position.GetRelative(character.Direction, 20);
		// Note: The original script checked IS_MORU_NOT_DESTROY_TARGET_ITEM, which we've renamed for clarity.
		var anvilName = IsAnvilForZeroPotential(anvilItem) ? "anvil_gold_mon" : "anvil_mon";

		var anvilMonster = CreateMonster(character, anvilName, position, 0, 1, 1, 0);
		SetupAnvil(anvilMonster, hitCount, character.Name);

		anvilMonster.Vars.Set("OWNERAID", character.AccountObjectId);
		anvilMonster.Vars.Set("ITEM_GUID", fromItem.ObjectId);
		anvilMonster.Vars.Set("MORU_GUID", anvilItem.ObjectId);
		character.Variables.Temp.Set("Melia.Anvil", anvilMonster);
		fromItem.Refinforcing = true;
		anvilItem.Refinforcing = true;

		anvilMonster.Components.Add(new LifeTimeComponent(anvilMonster, TimeSpan.FromSeconds(300)));
		anvilMonster.Died += AnvilMonster_Died;
		character.Map.AddMonster(anvilMonster);
		anvilMonster.PlaySound("anvil_ground");

		UpdateMoruDigitEffect(anvilMonster);

		return DialogTxResult.Okay;
	}

	private static void AnvilMonster_Died(Mob anvil, ICombatEntity killer)
	{
		if (killer is not Character owner)
			return;

		var savedOwnerAid = anvil.Vars.GetLong("OWNERAID");
		if (owner.AccountObjectId != savedOwnerAid)
			return;

		if (anvil.GetTempVar("ITEM_GET") == 1)
			return;

		anvil.SetTempVar("ITEM_GET", 1);

		var guid = anvil.Vars.GetLong("ITEM_GUID");
		var fromItem = owner.Inventory.GetItem(guid);
		if (fromItem == null)
			fromItem = owner.Inventory.GetEquip().Values.FirstOrDefault(a => a.ObjectId == guid);
		if (fromItem == null || fromItem.IsLocked)
			return;

		var anvilItemGuid = anvil.Vars.GetLong("MORU_GUID");
		var anvilItem = owner.Inventory.GetItem(anvilItemGuid);
		if (anvilItem == null || anvilItem.IsLocked)
			return;

		var successRate = GetSuccessRate(fromItem, anvilItem);

		var reinForceMode = owner.Variables.Temp.GetString("OPERATOR_REINFORCE_MODE");
		if (reinForceMode == "YES")
			successRate = 100;

		var isSuccess = RandomProvider.Get().Next(1, 101) <= successRate; // Use 101 for <= comparison with 100

		ReinforceResult(owner, fromItem, isSuccess, anvil, anvilItem);
	}

	private static float GetSuccessRate(Item fromItem, Item anvil)
	{
		var currentReinforcement = fromItem.Properties.GetFloat(PropertyName.Reinforce_2, 0);
		float successRatio;

		if (fromItem.Data?.Script?.StrArg == "Moru_goddess")
		{
			if (currentReinforcement < 5) return 100f;

			successRatio = 100f - (currentReinforcement - 4) * 4f;
			successRatio = MathF.Pow(successRatio / 100f, 3f) * 100f;
			return MathF.Max(successRatio, 51.2f);
		}

		var isWeaponType = fromItem.Data.Group == ItemGroup.Weapon ||
							(fromItem.Data.Group == ItemGroup.SubWeapon && fromItem.Data.EquipType1 != EquipType.Shield);

		if (isWeaponType)
		{
			if (currentReinforcement < 5) return 100f;

			successRatio = 100f - (currentReinforcement - 4) * 4f;
			successRatio = MathF.Pow(successRatio / 100f, 3f) * 100f;
			return MathF.Max(successRatio, 51.2f);
		}
		else
		{
			if (currentReinforcement < 5) return 100f;

			successRatio = 100f - (currentReinforcement - 2) * 4f;
			successRatio = MathF.Pow(successRatio / 100f, 3f) * 100f;
			return MathF.Max(successRatio, 51.2f);
		}
	}

	private static void UpdateMoruDigitEffect(Mob anvil, bool playEft = false)
	{
		if (anvil.Vars.TryGetString("LAST_EFT", out var lastEft))
			anvil.DetachEffect(lastEft);

		var curHP = anvil.Hp;
		var eftName = $"F_moru_{curHP}";
		anvil.AttachEffect(eftName, 5, EffectLocation.Top, 0, 10, 0);
		anvil.Vars.SetString("LAST_EFT", eftName);

		if (playEft)
		{
			anvil.DetachEffect("F_light011");
			anvil.AttachEffect("F_light011", 5, EffectLocation.Top, 0, 10, 0);
		}
	}

	private static int GetReinforcePrice(Item fromItem, Item anvilItem, Character character)
	{
		var reinforceCount = fromItem.Properties.GetFloat(PropertyName.Reinforce_2, 0);

		var reinforceCountDiamond = Math.Max(0, reinforceCount - 1);

		var slot = fromItem.Data.EquipSlot;
		if (string.IsNullOrEmpty(slot))
			return 0;

		var grade = fromItem.Properties.GetFloat(PropertyName.ItemGrade, 0);
		var gradeRatio = 1f;
		if (ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade((int)grade, out var itemGradeData))
			gradeRatio = itemGradeData.ReinforceCostRatio / 100f;

		var level = (float)fromItem.UseLevel;
		if (level == 0) return 0;

		// Placeholder for special item level overrides like Kupole/PCBang
		// var kupoleItemLevel = SRC_KUPOLE_GROWTH_ITEM(fromItem, 0);
		// if (kupoleItemLevel > 0) level = kupoleItemLevel;

		var priceRatio = 1f;
		var isGoddessAnvil = IsGoddessAnvil(anvilItem);

		if (isGoddessAnvil)
		{
			level = 500;
			priceRatio = 1.0f;
		}
		else
		{
			switch (slot)
			{
				case "RH":
				case "RH LH":
					priceRatio = fromItem.Data.IsTwoHanded ? 1.2f : 1.0f;
					break;
				case "LH":
				case "LH RH":
					if (fromItem.Data.EquipType1 == EquipType.Shield)
						priceRatio = 0.8f;
					else if (fromItem.Data.EquipType1 == EquipType.Trinket)
						priceRatio = 0.6f;
					else
						priceRatio = 0.8f;
					break;
				case "SHIRT":
				case "PANTS":
				case "GLOVES":
				case "BOOTS":
					priceRatio = 0.75f;
					break;
				case "NECK":
				case "RING":
					priceRatio = 0.5f;
					break;
				default:
					Log.Debug($"GetReinforcePrice: Unknown Slot {slot}");
					return 0;
			}
		}

		var value = MathF.Floor((500 + (MathF.Pow(level, 1.1f) * (5 + (reinforceCount * 2.5f)))) * (2 + (MathF.Max(0, reinforceCount - 9) * 0.5f))) * priceRatio * gradeRatio;
		var valueDiamond = MathF.Floor((500 + (MathF.Pow(level, 1.1f) * (5 + (reinforceCountDiamond * 2.5f)))) * (2 + (MathF.Max(0, reinforceCount - 9) * 0.5f))) * priceRatio * gradeRatio;

		if (IsAnvilDiscounted(anvilItem, out var discountRatio))
			value *= discountRatio;

		if (IsAnvilFreePrice(anvilItem))
			value = 0;

		if (anvilItem.Data?.Script?.StrArg == "DIAMOND" && reinforceCount > 1)
			value += (valueDiamond * 2.1f);

		if (level >= 440)
		{
			// Placeholder for IncreaseCost.xml lookup
			// var increaseRatio = GetIncreaseCostRatioForLevel((int)level);
			// value *= increaseRatio;
		}

		var hasEventDiscountBuff = character.IsBuffActive(BuffId.Event_Reinforce_Discount_50);
		if (hasEventDiscountBuff)
			value /= 2;

		if (fromItem.Data?.Script?.StrArg == "FreePvP")
			value *= 0.1f;

		var anvilStrArg = anvilItem.Data?.Script?.StrArg;
		if (!hasEventDiscountBuff)
		{
			if (anvilStrArg == "blessed_ruby_Moru") value *= 0.7f;
			else if (anvilStrArg == "blessed_gold_Moru") value *= 0.5f;
		}

		if (isGoddessAnvil)
			value *= 0.04f;

		return (int)MathF.Floor(value);
	}

	private static bool IsGoddessAnvil(Item anvilItem)
	{
		// This is a placeholder for the `is_goddess_moru` function in Lua.
		// Implement your logic based on how you identify these anvils.
		return anvilItem.Data?.Script?.StrArg == "Moru_goddess" || anvilItem.Data.ClassName == "Event_9th_Moru_goddess";
	}

	private static bool IsAnvilFreePrice(Item anvilItem)
	{
		if (anvilItem == null) return false;

		if (anvilItem.Data.Script.StrArg == "SILVER" || anvilItem.Data?.Script?.StrArg2 == "free_Moru")
			return true;

		var classNames = new HashSet<string>
		{
			"Moru_Silver", "Moru_Silver_test", "Moru_Silver_NoDay", "Moru_Silver_TA", "Moru_Silver_TA2",
			"Moru_Silver_Event_1704", "Moru_Silver_TA_Recycle", "Moru_Silver_TA_V2", "Moru_Gold_TA",
			"Moru_Gold_TA_NR", "Moru_Gold_TA_NR_Team_Trade", "Moru_Gold_EVENT_1710_NEWCHARACTER",
			"Moru_Event160609", "Moru_Event160929_14d", "Moru_Potential", "Moru_Potential14d",
			"Moru_Silver_Team", "Moru_Silver_Team_event1909", "Moru_Ruby_noCharge"
		};

		return classNames.Contains(anvilItem.Data.ClassName);
	}

	private static bool IsAnvilDiscounted(Item anvilItem, out float discountRatio)
	{
		discountRatio = 1.0f;
		if (anvilItem == null) return false;

		if (anvilItem.Data.Id == ItemId.Moru_Platinum_Premium || anvilItem.Data?.Script?.StrArg == "Reinforce_Discount_50")
		{
			discountRatio = 0.5f;
			return true;
		}

		if (anvilItem.Data.ClassName == "Event_9th_Moru_goddess")
		{
			discountRatio = 0.7f;
			return true;
		}

		return false;
	}

	private static bool IsAnvilForZeroPotential(Item moruItem)
	{
		if (moruItem == null) return false;

		var strArg = moruItem.Data?.Script?.StrArg;
		if (strArg == "gold_Moru" || strArg == "blessed_gold_Moru" || strArg == "unique_gold_Moru" || strArg == "blessed_ruby_Moru")
			return true;

		var classNames = new HashSet<string>
		{
			"Moru_Premium", "Moru_Gold", "Moru_Gold_14d", "Moru_Gold_TA", "Moru_Gold_TA_NR",
			"Moru_Gold_TA_NR_Team_Trade", "Moru_Gold_Team_Trade", "Moru_Gold_14d_Team",
			"Moru_Gold_EVENT_1710_NEWCHARACTER", "Moru_Gold_14d_Team_event1909"
		};

		return classNames.Contains(moruItem.Data.ClassName);
	}

	private static bool IsReinforceAble(Item fromItem, Item anvilItem)
	{
		if (fromItem.Data.Type != ItemType.Equip || fromItem.Data.ReinforceType != ReinforceType.Moru)
		{
			Log.Debug("IsReinforceAble: Failed - Item type check. Type: {0} (expected Equip), ReinforceType: {1} (expected Moru)",
				fromItem.Data.Type, fromItem.Data.ReinforceType);
			return false;
		}

		// Corresponds to Lua: `item.LifeTime > 0`
		if (fromItem.Properties.GetFloat(PropertyName.LifeTime) > 0)
		{
			Log.Debug("IsReinforceAble: Failed - Item has LifeTime > 0. LifeTime: {0}",
				fromItem.Properties.GetFloat(PropertyName.LifeTime));
			return false;
		}

		var mainProp = fromItem.Data.MainProperties;
		if (string.IsNullOrEmpty(mainProp))
		{
			Log.Debug("IsReinforceAble: Failed - MainProperties is null or empty");
			return false;
		}

		var validProps = new HashSet<string> { "DEF", "MDEF", "ADD_FIRE", "ADD_ICE", "ADD_LIGHTNING", "DEF;MDEF", "ATK;MATK", "MATK", "ATK" };
		if (!validProps.Contains(mainProp))
		{
			Log.Debug("IsReinforceAble: Failed - Invalid MainProperties '{0}'. Valid values: {1}",
				mainProp, string.Join(", ", validProps));
			return false;
		}

		var reinforceValue = fromItem.Properties.GetFloat(PropertyName.Reinforce_2, 0);
		if (reinforceValue >= 40)
		{
			Log.Debug("IsReinforceAble: Failed - Reinforce_2 value {0} >= 40 (max)", reinforceValue);
			return false;
		}

		if (anvilItem != null)
		{
			var anvilStrArg2 = anvilItem.Data?.Script?.StrArg2 ?? "None";
			var itemStrArg2 = fromItem.Data?.Script?.StrArg2 ?? "None";

			// TODO: Why is this check here?
			/**
			if (anvilStrArg2 != "None" || itemStrArg2 != "None")
			{
				if (anvilStrArg2 != itemStrArg2)
				{
					Log.Debug("IsReinforceAble: Failed - StrArg2 mismatch. Anvil StrArg2: '{0}', Item StrArg2: '{1}'",
						anvilStrArg2, itemStrArg2);
					return false;
				}
				return true;
			}
			**/

			var anvilStrArg = anvilItem.Data?.Script?.StrArg ?? "None";
			var itemStrArg = fromItem.Data?.Script?.StrArg ?? "None";

			if (itemStrArg == "Moru_goddess" && !anvilStrArg.Contains("Certificate"))
			{
				Log.Debug("IsReinforceAble: Failed - Goddess item requires Certificate anvil. Item StrArg: '{0}', Anvil StrArg: '{1}'",
					itemStrArg, anvilStrArg);
				return false;
			}

			if (anvilStrArg.Contains("Certificate"))
			{
				var requiredLevel = anvilItem.Properties.GetFloat(PropertyName.NumberArg1, 0);
				if (requiredLevel < fromItem.UseLevel)
				{
					Log.Debug("IsReinforceAble: Failed - Certificate anvil level too low. Anvil NumberArg1: {0}, Item UseLevel: {1}",
						requiredLevel, fromItem.UseLevel);
					return false;
				}
			}
		}

		return true;
	}

	private static bool IsReinforceAbleByUseLevel(Item anvilItem, Item fromItem)
	{
		if (anvilItem == null || fromItem == null)
			return false;

		return (anvilItem.Data.Id, fromItem.UseLevel) switch
		{
			(ItemId.Moru_Diamond_14d_Team_Lv400, > 400) => false,
			(ItemId.Moru_Diamond_14d_Team_Lv430, > 440) => false,
			(ItemId.Moru_Diamond_30d_Team_Lv440, > 440) => false,
			(ItemId.Moru_Diamond_30d_Team_Lv450, > 450) => false,
			_ => true,
		};
	}

	private static void ReinforceResult(Character character, Item invItem, bool refinforceSucceded, Mob monster, Item moruItem)
	{
		character.Variables.Temp.Remove("Melia.Anvil");

		if (invItem == null || invItem.IsLocked || moruItem == null || moruItem.IsLocked)
			return;

		invItem.Refinforcing = false;
		moruItem.Refinforcing = false;

		var scp = $"REINFORCE_131014_ITEM_LOCK('None')";
		character.ExecuteClientScript(scp);

		var itemReinCount = invItem.Properties.GetFloat(PropertyName.Reinforce_2);

		var retPrice = GetReinforcePrice(invItem, moruItem, character);
		if (!character.HasSilver(retPrice))
		{
			character.SystemMessage("Auto_SilBeoKa_BuJogHapNiDa.");
			return;
		}

		if (retPrice > 0)
			character.RemoveItem(ItemId.Vis, retPrice);

		character.Inventory.Remove(moruItem, 1, InventoryItemRemoveMsg.Used);

		var isBreakItem = false;
		if (refinforceSucceded)
		{
			invItem.Properties.Modify(PropertyName.Reinforce_2, 1);
		}
		else
		{
			// Anvils that prevent potential loss are handled by IsAnvilForZeroPotential.
			// The base logic is that failure on an item with potential loses potential.
			// If potential is 0, the item breaks unless a special anvil is used.
			if (invItem.Potential > 0)
				invItem.Properties.Modify(PropertyName.PR, -1);
			else if (!IsAnvilForZeroPotential(moruItem))
				isBreakItem = true;
		}

		var delaySec = 1.0f;
		if (refinforceSucceded)
		{
			character.AddAchievementPoint("ReinforceWin", 1);
			invItem.Properties.InvalidateAll();
			Send.ZC_OBJECT_PROPERTY(character, invItem);
			character.ShowItemBalloon("{@st43}", "SucessReinforce!!!", "", invItem, 5, delaySec, "enchant_itembox");
			monster.PlayAnimation("success", true, 1, 0);
			character.SystemMessage("Reinforce{ISSUCCESS}{ITEM}{BEFORE}{AFTER}", true, "", new MsgParameter("ISSUCCESS", "SUCCESS"),
				new MsgParameter("ITEM", invItem.Name), new MsgParameter("BEFORE", itemReinCount), new MsgParameter("AFTER", itemReinCount + 1));
		}
		else
		{
			if (isBreakItem)
				character.ShowItemBalloon("{@st43_red}", "SucessFail!!!", "", null, 5, delaySec, "enchant_itembox"); // Pass null if item is destroyed
			else
			{
				character.ShowItemBalloon("{@st43_red}", "SucessFail!!!", "", invItem, 5, delaySec, "enchant_itembox");
				invItem.Properties.InvalidateAll();
				Send.ZC_OBJECT_PROPERTY(character, invItem);
			}

			monster.PlayAnimation("fail", true, 1, 0);

			var reinfStr = itemReinCount > 0 ? "+" + itemReinCount.ToString() : "";
			character.SystemMessage("Reinforce{ISSUCCESS}{ITEM}{BEFORE}{AFTER}", false, "", new MsgParameter("ISSUCCESS", "FAIL"),
				new MsgParameter("ITEM", invItem.Name), new MsgParameter("BEFORE", itemReinCount), new MsgParameter("AFTER", reinfStr));
		}

		var isEquipped = character.Inventory.GetEquip().Values.Any(a => a.ObjectId == invItem.ObjectId);
		if (isEquipped)
		{
			if (isBreakItem)
			{
				var equipKeyValue = character.Inventory.GetEquip().FirstOrDefault(a => a.Value.ObjectId == invItem.ObjectId);
				if (equipKeyValue.Key != default)
					character.Inventory.Unequip(equipKeyValue.Key);
			}
			else
			{
				invItem.Properties.InvalidateAll();
				Send.ZC_OBJECT_PROPERTY(character, invItem);
				character.InvalidateProperties();
			}
		}

		if (isBreakItem)
		{
			character.SystemMessage("ItemDeleted", true, "FF0000", new MsgParameter("ITEM", invItem.Name));
			character.Inventory.Remove(invItem, 1, InventoryItemRemoveMsg.Destroyed);
		}

		character.AddonMessage("EQUIP_ITEM_LIST_UPDATE");
	}
}
