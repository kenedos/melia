using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Brutality buff.
	/// Grants defense penetration and looting chance bonus.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Brutality_Buff)]
	public class Brutality_BuffOverride : BuffHandler
	{
		private const float BaseDefPenRate = 0.05f;
		private const float DefPenRatePerLevel = 0.005f;
		private const float BaseLootingChance = 100f;
		private const float LootingChancePerLevel = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var lootingBonus = BaseLootingChance + (buff.NumArg1 * LootingChancePerLevel);
			AddPropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM, lootingBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.LootingChance_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Brutality_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.Brutality_Buff, out var buff))
				return;

			var defPenRate = BaseDefPenRate + (buff.NumArg1 * DefPenRatePerLevel);
			modifier.DefensePenetrationRate += defPenRate;
		}
	}
}
