using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handle for the Rampage After Buff, which increases damage
	/// dealt but reduces evasion
	/// </summary>
	/// <remarks>
	/// NumArg1: Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Rampage_After_Buff)]
	public class Rampage_After_BuffOverride : BuffHandler
	{
		private const float DamageBonus = 0.5f;
		private const float EvasionReduction = 0.50f;

		/// <summary>
		/// Starts buff, increasing dodge rate.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var evasionRate = EvasionReduction;

			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, -evasionRate);
		}

		/// <summary>
		/// Ends the buff, resetting dodge rate.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
		}

		/// <summary>
		/// Adds the damage bonus
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Rampage_After_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Rampage_After_Buff, out var buff))
				return;

			modifier.FinalDamageMultiplier += DamageBonus;
		}
	}
}
