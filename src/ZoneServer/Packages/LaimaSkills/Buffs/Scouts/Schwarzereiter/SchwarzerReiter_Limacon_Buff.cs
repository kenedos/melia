using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Limacon buff, which swaps the main attack to the
	/// pistol shot that Limacon: Enhance and Limacon: Spread act on.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Limacon_Buff)]
	public class SchwarzerReiter_Limacon_BuffOverride : BuffHandler
	{
		private const float SpreadAttackSpeedPenalty = -200f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;

			if (buff.Target.IsAbilityActive(AbilityId.Schwarzereiter18))
				AddPropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM, SpreadAttackSpeedPenalty);

			if (caster is Character character)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Pistol_Attack2);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var caster = buff.Caster;

			RemovePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM);

			if (caster is Character character)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
			}
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Limacon_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id != SkillId.Pistol_Attack2)
				return;

			if (!attacker.TryGetSkill(SkillId.Schwarzereiter_Limacon, out var limacon))
				return;

			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
			modifier.DamageMultiplier *= 1f + SCR_Get_AbilityReinforceRate(limacon);
		}
	}
}
