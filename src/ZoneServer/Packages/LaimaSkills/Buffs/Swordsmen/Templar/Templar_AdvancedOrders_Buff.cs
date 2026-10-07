using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Advanced Orders buff on the Templar's party, which
	/// raises movement speed and reduces the damage taken.
	/// </summary>
	/// <remarks>
	/// With Advanced Orders: Unity, evasion rises by skill level x 2%
	/// instead of movement speed.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.AdvancedOrders_Buff)]
	public class Templar_AdvancedOrders_BuffOverride : BuffHandler
	{
		private const float EvasionRatePerLevel = 2f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is ICombatEntity templar && templar.IsAbilityActive(AbilityId.Templar13))
				AddPairedPropertyModifier(buff, buff.Target, PropertyName.DR_BM, PropertyName.DR_RATE_BM, buff.NumArg1 * EvasionRatePerLevel);
			else
				AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, GetCaptionRatio(buff, 2));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePairedPropertyModifier(buff, buff.Target, PropertyName.DR_BM, PropertyName.DR_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.AdvancedOrders_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.AdvancedOrders_Buff, out var buff))
				return;

			skillHitResult.Damage *= 1f - GetCaptionRatio(buff, 1) / 100f;
		}
	}
}
