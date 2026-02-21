using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Steady Aim, Increases damage when Two-Handed Bow or Crossbow is equipped..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SteadyAim_Buff)]
	public class SteadyAim_Buff : BuffHandler, IBuffCombatAttackBeforeCalcHandler
	{
		public void OnAttackBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker.TryGetEquipItem(EquipSlot.RightHand, out var equipItem)
				&& (equipItem.Data.EquipType1 == EquipType.Bow
				|| equipItem.Data.EquipType1 == EquipType.THBow))
			{
				modifier.CritDamageMultiplier += buff.NumArg2;
			}
		}
	}
}
