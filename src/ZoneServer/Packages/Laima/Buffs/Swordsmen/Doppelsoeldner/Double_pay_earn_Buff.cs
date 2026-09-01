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
	/// Handler for the Double Pay Earn buff, which increases the target's
	/// looting chance, as well as the damage they take.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Double_pay_earn_Buff)]
	public class Double_pay_earn_BuffOverride : BuffHandler
	{
		public const float LootingChancePerLevel = 50f;
		public const float DamageTakenPerLevel = 0.10f;

		/// <summary>
		/// Starts the buff, increasing the target's looting chance.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var lootingBonus = buff.NumArg1 * LootingChancePerLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM, lootingBonus);
		}

		/// <summary>
		/// Ends the buff, removing the looting chance bonus.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM);
		}

		/// <summary>
		/// Increases the damage the target takes.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Double_pay_earn_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Double_pay_earn_Buff, out var buff))
				return;

			modifier.DamageMultiplier += buff.NumArg1 * DamageTakenPerLevel;
		}
	}
}
