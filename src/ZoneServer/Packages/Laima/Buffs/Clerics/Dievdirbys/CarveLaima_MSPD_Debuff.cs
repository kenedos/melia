using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for CarveLaima_MSPD_Debuff, which reduces movement speed
	/// by a fixed amount based on skill level, increased by Dievdirbys26 ability.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CarveLaima_MSPD_Debuff)]
	public class CarveLaima_MSPD_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is not ICombatEntity caster) return;

			// The caster could be a statue; if so, get its owner.
			if (caster is IMonster monster)
			{
				caster.Map.TryGetCombatEntity(monster.OwnerHandle, out caster);
			}

			if (caster == null || !caster.TryGetSkill(SkillId.Dievdirbys_CarveLaima, out var skill))
				return;

			// Base MSPD reduction: 10 + skillLevel
			var mspdReduction = 10f + skill.Level;

			// Check for Dievdirbys26 ability: +0.5% effectiveness per ability level
			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Dievdirbys26, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			mspdReduction *= byAbility;

			// Apply MSPD penalty
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -mspdReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
