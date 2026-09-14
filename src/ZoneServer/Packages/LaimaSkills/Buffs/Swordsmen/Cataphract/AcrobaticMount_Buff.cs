using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Cataphract
{
	/// <summary>
	/// Handler for the Acrobatic Mount buff.
	/// Grants damage and block penetration bonuses after mounted dash.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.AcrobaticMount_Buff)]
	public class AcrobaticMount_BuffOverride : BuffHandler
	{
		private const float MovementSpeedBonus = 5f;

		/// <summary>
		/// Applies property modifiers when buff activates.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			AddPairedPropertyModifier(buff, target, PropertyName.BLK_BREAK_BM, PropertyName.BLK_BREAK_RATE_BM, GetCaptionRatio(buff, 2));
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, MovementSpeedBonus);
		}

		/// <summary>
		/// Removes property modifiers when buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePairedPropertyModifier(buff, target, PropertyName.BLK_BREAK_BM, PropertyName.BLK_BREAK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);
		}

		/// <summary>
		/// Applies final damage bonus before attribute/race bonuses.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.AcrobaticMount_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.AcrobaticMount_Buff, out var buff))
				return;

			var skillLevel = buff.NumArg1;
			var damageIncrease = GetCaptionRatio(buff, 1) / 100f;

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				damageIncrease *= 1f + SCR_Get_AbilityReinforceRate(buffSkill);
			}

			skillHitResult.Damage *= 1f + damageIncrease;
		}
	}
}
