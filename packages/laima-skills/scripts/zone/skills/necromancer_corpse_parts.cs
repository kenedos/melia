//--- Melia Script ----------------------------------------------------------
// Necromancer Corpse Parts
//--- Description -----------------------------------------------------------
// Collects corpse parts from monsters a Necromancer's summons defeat.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

public class NecromancerCorpsePartsScript : GeneralScript
{
	[On("EntityKilled")]
	private void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob monster || monster is Summon)
			return;

		if (args.Attacker is not Summon summon || summon.Id != MonsterId.Pcskill_Skullsoldier)
			return;

		if (summon.Owner is not Character owner || !owner.TryGetActiveAbilityLevel(AbilityId.Necromancer22, out var level))
			return;

		if (NecromancerSkillHelper.AddCorpseParts(owner, monster.Id, level))
			Send.ZC_NORMAL.PlayGatherCorpseParts(owner, monster);
	}
}
