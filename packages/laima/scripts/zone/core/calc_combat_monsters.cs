//--- Melia Script ----------------------------------------------------------
// Monster Combat Calculation Script
//--- Description -----------------------------------------------------------
// Functions that override default combat calculations for specific
// monsters.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

public class MonsterCombatCalculationsScript : GeneralScript
{
	// Example for overriding all damage dealt to anvils. We might want to
	// implement more flexible ways to do this, but this does demonstrate
	// one way of doing it.

	[ScriptableFunction("SCR_CalculateDamage_Monster_Anvil")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_anvil_mon")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_anvil_gold_mon")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_anvil_platinum_mon")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_anvil_ruby_mon")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_anvil_diamond_mon")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_Event_LuckyBreak_Moru")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_goddess_Moru")]
	[ScriptableFunction("SCR_CalculateDamage_Monster_goddess_Moru")]
	public float DamageFixedTo1(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		if (target is Mob mob
			&& attacker is Character character
			&& mob.Vars.GetLong("OWNERAID") == character.AccountObjectId)
		{
			skillHitResult.Damage = 1;
		}
		else
		{
			skillHitResult.Damage = 0;
		}
		return 0;
	}

	[ScriptableFunction("SCR_CalculateDamage_Monster_pcskill_wood_bakarine")]
	public float DamageCheckOwner(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
	{
		var vakarine = target as Mob;
		var owner = vakarine?.Map?.GetCombatEntity(vakarine.OwnerHandle);
		if (owner != null && owner.GetRelation(attacker) != RelationType.Enemy)
		{
			skillHitResult.Damage = 0;
		}
		return 0;
	}
}
