using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Epee Garde buff, which raises the final damage of the
	/// target's attacks while a rapier is equipped.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EpeeGarde_Buff)]
	public class Fencer_EpeeGarde_BuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			if (!buff.Target.TryGetEquipItem(EquipSlot.RightHand, out var equipItem) || equipItem.Data.EquipType1 != EquipType.Rapier)
				buff.End();
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.EpeeGarde_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.EpeeGarde_Buff, out var buff))
				return;

			skillHitResult.Damage *= 1f + GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
