//--- Melia Script ----------------------------------------------------------
// Mixing Table Scripts
//--- Description -----------------------------------------------------------
// Handles "Dialog TX" requests from the client.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

public class MixingTableFunctionsScript : GeneralScript
{
	private static readonly string[] ColoredGemClassNames = new[]
	{
		"gem_circle_1",
		"gem_square_1",
		"gem_diamond_1",
		"gem_star_1",
		"gem_White_1",
	};

	private static Dictionary<CardGroup, List<ItemData>> _cardAlbumByGroup;

	private static Dictionary<CardGroup, List<ItemData>> GetCardAlbumByGroup()
	{
		return _cardAlbumByGroup;
	}

	private static void BuildCardAlbumMap()
	{
		_cardAlbumByGroup = new Dictionary<CardGroup, List<ItemData>>();
		var itemDb = ZoneServer.Instance.Data.ItemDb;
		var seen = new HashSet<int>();

		foreach (var cardItemId in CardAlbumBossRewardsScript.CardAlbumDrops.Values)
		{
			if (!seen.Add(cardItemId))
				continue;

			if (!itemDb.TryFind(cardItemId, out var itemData))
				continue;

			if (itemData.Group != ItemGroup.Card || itemData.CardGroup == CardGroup.None)
				continue;

			if (!_cardAlbumByGroup.TryGetValue(itemData.CardGroup, out var list))
			{
				list = new List<ItemData>();
				_cardAlbumByGroup[itemData.CardGroup] = list;
			}

			list.Add(itemData);
		}
	}

	private class CatalystInfo
	{
		public string WeaponStat { get; set; }
		public string ArmorStat { get; set; }

		public CatalystInfo(string stat) { WeaponStat = stat; ArmorStat = stat; }
		public CatalystInfo(string weaponStat, string armorStat) { WeaponStat = weaponStat; ArmorStat = armorStat; }
	}

	private static readonly Dictionary<string, CatalystInfo> CatalystMap = new()
	{
		// Colored gems (Lv5 required)
		{ "gem_circle_1", new CatalystInfo("STR") },
		{ "gem_square_1", new CatalystInfo("INT") },
		{ "gem_diamond_1", new CatalystInfo("CON") },
		{ "gem_star_1", new CatalystInfo("DEX") },
		{ "gem_White_1", new CatalystInfo("MNA") },

		// Elemental (weapon → atk, armor → resist)
		{ "misc_jore14", new CatalystInfo("ADD_FIRE", "RES_FIRE") },
		{ "misc_jore15", new CatalystInfo("ADD_ICE", "RES_ICE") },
		{ "misc_jore03", new CatalystInfo("ADD_LIGHTNING", "RES_LIGHTNING") },
		{ "misc_jore07", new CatalystInfo("ADD_POISON", "RES_POISON") },
		{ "misc_jore06", new CatalystInfo("ADD_EARTH", "RES_EARTH") },
		{ "misc_jore16", new CatalystInfo("ADD_HOLY", "RES_HOLY") },
		{ "misc_jore10", new CatalystInfo("ADD_DARK", "RES_DARK") },

		// Physical (weapon → atk type, armor → def type)
		{ "misc_ore01", new CatalystInfo("Slash", "SlashDEF") },
		{ "misc_ore02", new CatalystInfo("Aries", "AriesDEF") },
		{ "misc_ore03", new CatalystInfo("Strike", "StrikeDEF") },

		// Substats (same for weapon and armor)
		{ "misc_jore18", new CatalystInfo("CRTHR") },
		{ "misc_jore17", new CatalystInfo("CRTDR") },
		{ "misc_jore08", new CatalystInfo("CRTATK") },
		{ "misc_jore04", new CatalystInfo("BLK") },
		{ "misc_jore05", new CatalystInfo("BLK_BREAK") },
		{ "misc_jore02", new CatalystInfo("ADD_HR") },
		{ "misc_jore01", new CatalystInfo("ADD_DR") },
	};

	protected override void Load()
	{
		base.Load();

		BuildCardAlbumMap();

		if (!Feature.IsEnabled(FeatureId.MixingTable))
			return;
		AddChatCommand("mixing", "", "Opens the mixing table.", 0, 99, HandleMixingTable);
	}

	private CommandResult HandleMixingTable(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (!Feature.IsEnabled(FeatureId.MixingTable))
			return CommandResult.Okay;

		sender.AddonMessage("OPEN_DLG_MIXINGTABLE");
		return CommandResult.Okay;
	}

	public enum MixingTableRecipe
	{
		RerollGem = 1,
		ImproveQuality = 2,
		RerollMods = 3,
		GuaranteedStat = 4,
		ColoredCardCombine = 5,
		ClassSkillGemReroll = 6,
		GoldenAnvil = 7,
		SilverAnvil = 8,
		RubyAnvil = 9,
		GenericCardCombine = 10,
	}

	[ScriptableFunction]
	public DialogTxResult SCR_MIXINGTABLE_CRAFT(Character character, DialogTxArgs args)
	{
		if (!Feature.IsEnabled(FeatureId.MixingTable))
			return DialogTxResult.Okay;

		if (args.TxItems.Length < 1 || args.NumArgs.Length < 1)
		{
			Log.Debug("SCR_MIXINGTABLE_CRAFT: Invalid args. TxItems={0}, NumArgs={1}", args.TxItems.Length, args.NumArgs.Length);
			return DialogTxResult.Okay;
		}

		var seenObjectIds = new HashSet<long>();
		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.NeedsAppraisal)
			{
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			if (txItem.Item.Data.MaxStack <= 1 && !seenObjectIds.Add(txItem.Item.ObjectId))
			{
				Log.Warning("SCR_MIXINGTABLE_CRAFT: User '{0}' sent duplicate item ObjectId {1}.", character.Username, txItem.Item.ObjectId);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}
		}

		var recipeType = (MixingTableRecipe)args.NumArgs[0];

		switch (recipeType)
		{
			case MixingTableRecipe.RerollGem:
				return HandleRerollGem(character, args);
			case MixingTableRecipe.ImproveQuality:
				return HandleImproveQuality(character, args);
			case MixingTableRecipe.RerollMods:
				return HandleRerollMods(character, args);
			case MixingTableRecipe.GuaranteedStat:
				return HandleGuaranteedStat(character, args);
			case MixingTableRecipe.ColoredCardCombine:
				return HandleColoredCardCombine(character, args);
			case MixingTableRecipe.GenericCardCombine:
				return HandleGenericCardCombine(character, args);
			case MixingTableRecipe.ClassSkillGemReroll:
				return HandleClassSkillGemReroll(character, args);
			case MixingTableRecipe.GoldenAnvil:
				return HandleAnvilCraft(character, args, "misc_goldbar", "misc_ore22", 10, "Moru_Gold");
			case MixingTableRecipe.SilverAnvil:
				return HandleAnvilCraft(character, args, "misc_silverbar", "misc_ore22", 1, "Moru_Silver_NoDay");
			case MixingTableRecipe.RubyAnvil:
				return HandleAnvilCraft(character, args, "misc_jore18", "misc_ore22", 20, "Moru_Ruby_Charge");
			default:
				Log.Debug("SCR_MIXINGTABLE_CRAFT: Unknown recipeType {0}", recipeType);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
		}
	}

	private static bool IsSkillGem(ItemData itemData)
	{
		return itemData.Group == ItemGroup.Gem && itemData.EquipExpGroup == EquipExpGroup.Gem_Skill;
	}

	private static bool IsColoredGem(ItemData itemData)
	{
		return itemData.Group == ItemGroup.Gem && itemData.EquipExpGroup == EquipExpGroup.Gem;
	}

	private static string GetSkillGemJobClass(string className)
	{
		if (!className.StartsWith("Gem_", StringComparison.OrdinalIgnoreCase))
			return null;

		var parts = className.Split('_');
		if (parts.Length < 3)
			return null;

		var jobClass = parts[1];
		return SkillGemConst.AllowedClasses.Contains(jobClass) ? jobClass : null;
	}

	private static bool TryReducePotential(Character character, Item equipItem)
	{
		var potential = (int)equipItem.Properties.GetFloat(PropertyName.PR);
		if (potential <= 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return false;
		}

		equipItem.Properties.SetFloat(PropertyName.PR, potential - 1);
		return true;
	}

	private DialogTxResult HandleRerollGem(Character character, DialogTxArgs args)
	{
		string gemClassName = null;
		var totalGemCount = 0;
		var isSkillGemRecipe = false;
		var isColoredGemRecipe = false;
		var requiredGemLevel = -1;

		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item.Data.Group != ItemGroup.Gem)
			{
				Log.Warning("HandleRerollGem: User '{0}' sent non-gem item '{1}'.", character.Username, item.Data.ClassName);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			if (gemClassName == null)
			{
				gemClassName = item.Data.ClassName;
				isSkillGemRecipe = IsSkillGem(item.Data);
				isColoredGemRecipe = IsColoredGem(item.Data);
				requiredGemLevel = item.GemLevel;
			}

			if (item.GemLevel != requiredGemLevel)
			{
				Log.Warning("HandleRerollGem: User '{0}' sent gems of different levels.", character.Username);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			if (isSkillGemRecipe)
			{
				if (!IsSkillGem(item.Data))
				{
					Log.Warning("HandleRerollGem: User '{0}' mixed skill and non-skill gems.", character.Username);
					character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
					return DialogTxResult.Okay;
				}
			}
			else if (isColoredGemRecipe)
			{
				if (!IsColoredGem(item.Data))
				{
					Log.Warning("HandleRerollGem: User '{0}' mixed colored and non-colored gems.", character.Username);
					character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
					return DialogTxResult.Okay;
				}
			}

			totalGemCount += txItem.Amount;
		}

		if (totalGemCount < 3 || gemClassName == null)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var sourceGemData = ZoneServer.Instance.Data.ItemDb.FindByClass(gemClassName);
		if (sourceGemData == null)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		List<ItemData> candidates;

		if (isSkillGemRecipe)
		{
			candidates = ZoneServer.Instance.Data.ItemDb.Entries.Values
				.Where(i => i.Group == ItemGroup.Gem
					&& i.EquipExpGroup == EquipExpGroup.Gem_Skill
					&& i.ClassName != gemClassName
					&& i.Journal
					&& GetSkillGemJobClass(i.ClassName) != null)
				.ToList();
		}
		else
		{
			candidates = ColoredGemClassNames
				.Select(cn => ZoneServer.Instance.Data.ItemDb.FindByClass(cn))
				.Where(i => i != null)
				.ToList();
		}

		if (candidates.Count == 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var gemsToRemove = 3;
		foreach (var txItem in args.TxItems)
		{
			var removeCount = Math.Min(txItem.Amount, gemsToRemove);
			character.Inventory.Remove(txItem.Item.ObjectId, removeCount, InventoryItemRemoveMsg.Given);
			gemsToRemove -= removeCount;
			if (gemsToRemove <= 0)
				break;
		}

		var rnd = RandomProvider.Get();
		var resultGem = candidates[rnd.Next(candidates.Count)];

		var newGem = new Item(resultGem.Id);
		if (requiredGemLevel > 1)
			newGem.SetLevel(requiredGemLevel);
		character.Inventory.Add(newGem, InventoryAddType.PickUp);

		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultGem.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleImproveQuality(Character character, DialogTxArgs args)
	{
		DialogTxItem equipTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore22")
				powderTxItem = txItem;
			else if (txItem.Item.Data.Type == ItemType.Equip)
				equipTxItem = txItem;
		}

		if (equipTxItem == null || powderTxItem == null || powderTxItem.Amount < 10)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var equipItem = equipTxItem.Item;
		var currentGrade = (int)equipItem.Properties.GetFloat(PropertyName.ItemGrade);

		if (currentGrade >= (int)ItemGrade.Rare)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		if (!TryReducePotential(character, equipItem))
			return DialogTxResult.Okay;

		character.Inventory.Remove(powderTxItem.Item.ObjectId, 10, InventoryItemRemoveMsg.Given);

		var newGrade = currentGrade + 1;
		equipItem.Properties.SetFloat(PropertyName.ItemGrade, newGrade);
		equipItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
		equipItem.GenerateGradeBasedRandomOptions();

		Send.ZC_OBJECT_PROPERTY(character, equipItem);
		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, equipItem.Data.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleRerollMods(Character character, DialogTxArgs args)
	{
		DialogTxItem equipTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore23")
				powderTxItem = txItem;
			else if (txItem.Item.Data.Type == ItemType.Equip)
				equipTxItem = txItem;
		}

		if (equipTxItem == null || powderTxItem == null || powderTxItem.Amount < 5)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var equipItem = equipTxItem.Item;

		if (!TryReducePotential(character, equipItem))
			return DialogTxResult.Okay;

		character.Inventory.Remove(powderTxItem.Item.ObjectId, 5, InventoryItemRemoveMsg.Given);

		equipItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
		equipItem.GenerateGradeBasedRandomOptions();

		Send.ZC_OBJECT_PROPERTY(character, equipItem);
		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, equipItem.Data.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleGuaranteedStat(Character character, DialogTxArgs args)
	{
		DialogTxItem equipTxItem = null;
		DialogTxItem powderTxItem = null;
		DialogTxItem catalystTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore23")
				powderTxItem = txItem;
			else if (txItem.Item.Data.Type == ItemType.Equip)
				equipTxItem = txItem;
			else if (CatalystMap.ContainsKey(txItem.Item.Data.ClassName))
				catalystTxItem = txItem;
		}

		if (equipTxItem == null || powderTxItem == null || catalystTxItem == null || powderTxItem.Amount < 10)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var catalystClassName = catalystTxItem.Item.Data.ClassName;
		var isColoredGemCatalyst = IsColoredGem(catalystTxItem.Item.Data);

		if (isColoredGemCatalyst && catalystTxItem.Item.GemLevel < 5)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var equipItem = equipTxItem.Item;
		var catalystInfo = CatalystMap[catalystClassName];
		var isWeapon = equipItem.Data.Group == ItemGroup.Weapon || equipItem.Data.Group == ItemGroup.SubWeapon;
		var guaranteedStat = isWeapon ? catalystInfo.WeaponStat : catalystInfo.ArmorStat;

		if (!TryReducePotential(character, equipItem))
			return DialogTxResult.Okay;

		character.Inventory.Remove(powderTxItem.Item.ObjectId, 10, InventoryItemRemoveMsg.Given);
		character.Inventory.Remove(catalystTxItem.Item.ObjectId, 1, InventoryItemRemoveMsg.Given);

		equipItem.Properties.SetFloat(PropertyName.NeedRandomOption, 1);
		equipItem.GenerateGradeBasedRandomOptions();

		var hasGuaranteedStat = false;
		for (var i = 1; i <= 4; i++)
		{
			var optionName = equipItem.Properties.GetString($"RandomOption_{i}");
			if (optionName == guaranteedStat)
			{
				hasGuaranteedStat = true;
				break;
			}
		}

		if (!hasGuaranteedStat)
		{
			var oldStatName = equipItem.Properties.GetString("RandomOption_1");
			if (oldStatName != null && oldStatName != "None" && oldStatName != "")
			{
				var oldValue = equipItem.Properties.GetFloat("RandomOptionValue_1");
				equipItem.Properties.Modify(oldStatName, -oldValue);
			}

			var itemGrade = (ItemGrade)equipItem.Properties.GetFloat(PropertyName.ItemGrade);
			var statValue = (float)Math.Floor(equipItem.GenerateRandomStatValue(guaranteedStat, equipItem.Level, itemGrade, out _));

			equipItem.Properties.SetString("RandomOption_1", guaranteedStat);
			equipItem.Properties.SetString("RandomOptionGroup_1", "STAT");
			equipItem.Properties.SetFloat("RandomOptionValue_1", statValue);
			equipItem.Properties.Modify(guaranteedStat, statValue);
		}

		Send.ZC_OBJECT_PROPERTY(character, equipItem);
		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, equipItem.Data.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleColoredCardCombine(Character character, DialogTxArgs args)
	{
		CardGroup cardGroup = CardGroup.None;
		int cardLevel = 0;
		var totalCardCount = 0;
		var isFirst = true;

		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item.Data.Group != ItemGroup.Card)
			{
				Log.Warning("HandleColoredCardCombine: User '{0}' sent non-card item '{1}'.", character.Username, item.Data.ClassName);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			if (isFirst)
			{
				cardGroup = item.Data.CardGroup;
				cardLevel = item.CardLevel;
				isFirst = false;
			}

			if (item.Data.CardGroup != cardGroup || item.CardLevel != cardLevel)
			{
				Log.Warning("HandleColoredCardCombine: User '{0}' sent cards of different group/level.", character.Username);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			totalCardCount += txItem.Amount;
		}

		if (totalCardCount < 3 || cardGroup == CardGroup.None)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var cardMap = GetCardAlbumByGroup();
		if (!cardMap.TryGetValue(cardGroup, out var candidates) || candidates.Count == 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var cardsToRemove = 3;
		foreach (var txItem in args.TxItems)
		{
			var removeCount = Math.Min(txItem.Amount, cardsToRemove);
			character.Inventory.Remove(txItem.Item.ObjectId, removeCount, InventoryItemRemoveMsg.Given);
			cardsToRemove -= removeCount;
			if (cardsToRemove <= 0)
				break;
		}

		var rnd = RandomProvider.Get();
		var resultCard = candidates[rnd.Next(candidates.Count)];

		var newCard = new Item(resultCard.Id);
		if (cardLevel > 1)
			newCard.SetLevel(cardLevel);
		character.Inventory.Add(newCard, InventoryAddType.PickUp);

		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultCard.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleGenericCardCombine(Character character, DialogTxArgs args)
	{
		var totalCardCount = 0;
		var requiredCardLevel = -1;

		foreach (var txItem in args.TxItems)
		{
			var item = txItem.Item;
			if (item.Data.Group != ItemGroup.Card)
			{
				Log.Warning("HandleGenericCardCombine: User '{0}' sent non-card item '{1}'.", character.Username, item.Data.ClassName);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			if (requiredCardLevel == -1)
				requiredCardLevel = item.CardLevel;

			if (item.CardLevel != requiredCardLevel)
			{
				Log.Warning("HandleGenericCardCombine: User '{0}' sent cards of different levels.", character.Username);
				character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
				return DialogTxResult.Okay;
			}

			totalCardCount += txItem.Amount;
		}

		if (totalCardCount < 3)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var cardMap = GetCardAlbumByGroup();
		var candidates = cardMap.Values.SelectMany(list => list).ToList();

		if (candidates.Count == 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var cardsToRemove = 3;
		foreach (var txItem in args.TxItems)
		{
			var removeCount = Math.Min(txItem.Amount, cardsToRemove);
			character.Inventory.Remove(txItem.Item.ObjectId, removeCount, InventoryItemRemoveMsg.Given);
			cardsToRemove -= removeCount;
			if (cardsToRemove <= 0)
				break;
		}

		var rnd = RandomProvider.Get();
		var resultCard = candidates[rnd.Next(candidates.Count)];

		var newCard = new Item(resultCard.Id);
		if (requiredCardLevel > 1)
			newCard.SetLevel(requiredCardLevel);
		character.Inventory.Add(newCard, InventoryAddType.PickUp);

		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultCard.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleClassSkillGemReroll(Character character, DialogTxArgs args)
	{
		DialogTxItem gemTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == "misc_ore23")
				powderTxItem = txItem;
			else if (IsSkillGem(txItem.Item.Data))
				gemTxItem = txItem;
		}

		if (gemTxItem == null || powderTxItem == null || powderTxItem.Amount < 5)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var sourceClassName = gemTxItem.Item.Data.ClassName;
		var jobClass = GetSkillGemJobClass(sourceClassName);

		if (jobClass == null)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var prefix = $"Gem_{jobClass}_";
		var candidates = ZoneServer.Instance.Data.ItemDb.Entries.Values
			.Where(i => i.Group == ItemGroup.Gem
				&& i.EquipExpGroup == EquipExpGroup.Gem_Skill
				&& i.ClassName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
				&& i.ClassName != sourceClassName
				&& i.Journal)
			.ToList();

		if (candidates.Count == 0)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		var sourceGemLevel = gemTxItem.Item.GemLevel;

		character.Inventory.Remove(powderTxItem.Item.ObjectId, 5, InventoryItemRemoveMsg.Given);
		character.Inventory.Remove(gemTxItem.Item.ObjectId, 1, InventoryItemRemoveMsg.Given);

		var rnd = RandomProvider.Get();
		var resultGem = candidates[rnd.Next(candidates.Count)];

		var newGem = new Item(resultGem.Id);
		if (sourceGemLevel > 1)
			newGem.SetLevel(sourceGemLevel);
		character.Inventory.Add(newGem, InventoryAddType.PickUp);

		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultGem.ClassName);

		return DialogTxResult.Okay;
	}

	private DialogTxResult HandleAnvilCraft(Character character, DialogTxArgs args, string barClassName, string powderClassName, int powderAmount, string resultClassName)
	{
		DialogTxItem barTxItem = null;
		DialogTxItem powderTxItem = null;

		foreach (var txItem in args.TxItems)
		{
			if (txItem.Item.Data.ClassName == powderClassName)
				powderTxItem = txItem;
			else if (txItem.Item.Data.ClassName == barClassName)
				barTxItem = txItem;
		}

		if (barTxItem == null || powderTxItem == null || powderTxItem.Amount < powderAmount)
		{
			character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_FAIL, "");
			return DialogTxResult.Okay;
		}

		character.Inventory.Remove(barTxItem.Item.ObjectId, 1, InventoryItemRemoveMsg.Given);
		character.Inventory.Remove(powderTxItem.Item.ObjectId, powderAmount, InventoryItemRemoveMsg.Given);

		character.AddItem(resultClassName, 1);

		Send.ZC_ITEM_INVENTORY_INDEX_LIST(character);
		character.AddonMessage(AddonMessage.JOURNAL_DETAIL_CRAFT_EXEC_SUCCESS, resultClassName);

		return DialogTxResult.Okay;
	}
}
