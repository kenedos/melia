//--- Melia Script ----------------------------------------------------------
// Doll Items
//--- Description -----------------------------------------------------------
// Item scripts that add and remove skills on equipping.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

public class SkillItemScript : GeneralScript
{

	[ScriptableFunction("SCP_ON_EQUIP_ITEM_SKILL")]
	public ItemEquipResult SCP_ON_EQUIP_ITEM_SKILL(Character character, Item item, EquipSlot equipSlot)
	{
		var skillClassname = item.Data.Script.StrArg2;

		if (ZoneServer.Instance.Data.SkillDb.TryFind(skillClassname, out var skillData))
		{
			if (!character.HasSkill(skillData.Id))
				character.Skills.AddSilent(new Skill(character, skillData.Id, 1, isEquipSkill: true));
		}

		return ItemEquipResult.Okay;
	}

	[ScriptableFunction("SCP_ON_UNEQUIP_ITEM_SKILL")]
	public ItemUnequipResult SCP_ON_UNEQUIP_ITEM_SKILL(Character character, Item item, EquipSlot equipSlot)
	{
		var skillClassname = item.Data.Script.StrArg2;

		if (ZoneServer.Instance.Data.SkillDb.TryFind(skillClassname, out var skillData))
		{
			if (character.TryGetSkill(skillData.Id, out var skill) && skill.IsEquipSkill)
				character.Skills.RemoveSilent(skillData.Id);
		}

		return ItemUnequipResult.Okay;
	}
}
