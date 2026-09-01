using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Doppelsoeldner
{
	/// <summary>
	/// Handler for the Deeds of Valor buff, which increases the final
	/// damage the target deals, as well as the damage they take.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DeedsOfValor)]
	public class DeedsOfValorOverride : BuffHandler
	{
		public const float DamageTakenPerLevel = 0.03f;

		/// <summary>
		/// Increases the final damage dealt by the attacker and the damage
		/// taken by the target.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.DeedsOfValor)]
		public void OnBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker.TryGetBuff(BuffId.DeedsOfValor, out var attackerBuff))
				modifier.FinalDamageMultiplier *= attackerBuff.NumArg2;

			if (target.TryGetBuff(BuffId.DeedsOfValor, out var targetBuff))
				modifier.DamageMultiplier += targetBuff.NumArg1 * DamageTakenPerLevel;
		}
	}
}
