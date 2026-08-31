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
	/// target's attacks and turns part of an equipped shield's defense into
	/// evasion, while a rapier is equipped.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EpeeGarde_Buff)]
	public class Fencer_EpeeGarde_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			RefreshShieldEvasion(buff);
		}

		public override void WhileActive(Buff buff)
		{
			if (Fencer_RapierGuard.EndWithoutRapier(buff))
				return;

			RefreshShieldEvasion(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			buff.Target.InvalidateProperties();
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.EpeeGarde_Buff)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.EpeeGarde_Buff, out var buff))
				return;

			skillHitResult.Damage *= 1f + GetCaptionRatio(buff, 1) / 100f;
		}

		/// <summary>
		/// Rewrites the evasion the buff grants from the shield that is
		/// equipped right now, so swapping shields is reflected.
		/// </summary>
		/// <param name="buff"></param>
		private static void RefreshShieldEvasion(Buff buff)
		{
			var evasion = GetShieldEvasion(buff);

			if (buff.Vars.TryGetFloat(ModifierVarPrefix + PropertyName.DR_BM, out var current) && current == evasion)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, evasion);

			buff.Target.InvalidateProperties();
		}

		/// <summary>
		/// Returns the evasion converted from the defense of the shield the
		/// buff's target is carrying, or zero if they carry none.
		/// </summary>
		/// <param name="buff"></param>
		/// <returns></returns>
		private static float GetShieldEvasion(Buff buff)
		{
			if (!buff.Target.TryGetEquipItem(EquipSlot.LeftHand, out var shield) || shield.Data.EquipType1 != EquipType.Shield)
				return 0;

			return shield.Data.Def * GetCaptionRatio(buff, 2) / 100f;
		}
	}
}
