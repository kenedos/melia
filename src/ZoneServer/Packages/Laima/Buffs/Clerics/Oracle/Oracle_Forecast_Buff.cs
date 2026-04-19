using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Forecast buff, which increases the target's
	/// damage reduction (DR) by 100 while active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Forecast_Buff)]
	public class Oracle_Forecast_BuffOverride : BuffHandler
	{
		private const float AddDR = 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, AddDR);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc_Defense, BuffId.Forecast_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Forecast_Buff, out var buff))
				return;

			if (buff.Caster is ICombatEntity caster)
			{
				var abilLevel = caster.GetAbilityLevel(AbilityId.Oracle6);
				if (abilLevel > 0)
					modifier.BonusDodgeChance += 100 * abilLevel;
			}
		}
	}
}
