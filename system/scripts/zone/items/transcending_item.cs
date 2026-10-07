//--- Melia Script ----------------------------------------------------------
// Transcending Items
//--- Description -----------------------------------------------------------
// Item transcendence through the client's transcend window: Goddesses'
// Blessed Gems raise an item's transcend stage, each stage adding 10% to
// its base attack and defense.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Util;
using static Melia.Zone.Scripting.Shortcuts;

public class TranscendingItemScripts : GeneralScript
{
	private const int MaxStage = 10;
	private const int MaterialItemId = ItemId.Premium_Item_Transcendence_Stone;

	protected override void Load()
	{
		if (!Feature.IsEnabled("ItemTranscendence"))
			return;

		AddNpc(20105, L("[Transcendence] Elder"), "c_Klaipe", 440, 139, 270, this.ElderDialog);
	}

	private async Task ElderDialog(Dialog dialog)
	{
		dialog.SetTitle(L("Transcendence"));
		await dialog.Msg(L("Bring me your equipment and Goddesses' Blessed Gems. Every stage of transcendence strengthens its base power by a tenth. A failed attempt costs the item some of its potential, and an item with none left is lost."));

		dialog.Player.AddonMessage("OPEN_DLG_ITEMTRANSCEND", "", 0);
	}

	[ScriptableFunction]
	public DialogTxResult SCR_ITEM_TRANSCEND_TX(Character character, DialogTxArgs args)
	{
		if (!Feature.IsEnabled("ItemTranscendence") || args.TxItems.Length < 2)
			return DialogTxResult.Fail;

		var item = args.TxItems[0].Item;
		var material = args.TxItems[1].Item;

		if (material.Id != MaterialItemId || item.ObjectId == material.ObjectId)
			return DialogTxResult.Fail;

		var stage = (int)item.Properties.GetFloat(PropertyName.Transcend, 0);
		if (stage >= MaxStage)
		{
			character.SystemMessage("CantTrasncendMore");
			return DialogTxResult.Fail;
		}

		if (!TryGetMaterialCount(item, stage, out var needed))
		{
			character.SystemMessage("ThisItemIsNotAbleToTranscend");
			return DialogTxResult.Fail;
		}

		var count = args.TxItems[1].Amount;
		if (args.StrArgs.Length > 0 && int.TryParse(args.StrArgs[0], out var requested))
			count = requested;

		count = Math.Min(Math.Min(count, needed), material.Amount);
		if (count <= 0)
			return DialogTxResult.Fail;

		if (character.Inventory.Remove(material, count, InventoryItemRemoveMsg.Used) != Melia.Zone.World.Actors.Characters.Components.InventoryResult.Success)
			return DialogTxResult.Fail;

		var success = RandomProvider.Get().Next(100) < count * 100 / needed;

		if (success)
		{
			item.Properties.SetFloat(PropertyName.Transcend, stage + 1);
			item.Properties.Modify(PropertyName.Transcend_SucessCount, count);
			character.SystemMessage("SuccessToTranscend");
		}
		else
		{
			character.SystemMessage("FailedToTranscend");

			if (item.Potential <= 0)
			{
				character.Inventory.Remove(item, item.Amount, InventoryItemRemoveMsg.Destroyed);
				character.ExecuteClientScript("TRANSCEND_UPDATE(0)");
				return DialogTxResult.Okay;
			}

			item.Properties.Modify(PropertyName.PR, -1);
		}

		item.Properties.InvalidateAll();
		Send.ZC_OBJECT_PROPERTY(character, item);
		character.ExecuteClientScript($"TRANSCEND_UPDATE({(success ? 1 : 0)})");

		return DialogTxResult.Okay;
	}

	/// <summary>
	/// Returns the number of Blessed Gems that give a certain transcendence
	/// from the given stage, following the client's formula.
	/// </summary>
	private static bool TryGetMaterialCount(Item item, int stage, out int count)
	{
		count = 0;

		var grade = (int)item.Properties.GetFloat(PropertyName.ItemGrade);
		if (grade < 1 || grade >= (int)ItemGrade.Goddess || item.Data.Potential <= 0)
			return false;

		if (!ZoneServer.Instance.Data.ItemGradeDb.TryFindByGrade(grade, out var gradeData))
			return false;

		var equipRatio = GetEquipRatio(item.Data);
		if (equipRatio <= 0)
			return false;

		var level = item.Data.MinLevel;
		var exponent = 0.2 + (stage / 3) * 0.03 + stage * 0.05;
		var value = (1 + (stage + Math.Pow(level, exponent)) * equipRatio) * (gradeData.TranscendCostRatio / 100.0) * 0.5;

		count = Math.Max(1, (int)Math.Floor(value));
		return true;
	}

	private static double GetEquipRatio(Melia.Shared.Data.Database.ItemData data)
	{
		switch (data.Group)
		{
			case ItemGroup.Weapon:
				switch (data.EquipType1)
				{
					case EquipType.Sword:
					case EquipType.Staff:
					case EquipType.Rapier:
					case EquipType.Spear:
					case EquipType.Bow:
					case EquipType.Mace:
						return 0.8;
				}
				return data.IsTwoHanded ? 1 : 0;

			case ItemGroup.SubWeapon:
				if (data.EquipType1 == EquipType.Shield)
					return 0.6;
				return data.EquipType1 == EquipType.Trinket ? 0.4 : 0.6;

			case ItemGroup.Armor:
				return data.EquipType1 == EquipType.Shield ? 0.6 : 0.33;

			default:
				return 0;
		}
	}
}
