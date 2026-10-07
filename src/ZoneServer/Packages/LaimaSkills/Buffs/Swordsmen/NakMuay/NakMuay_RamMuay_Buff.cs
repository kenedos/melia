using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Ram Muay, which swaps the basic attacks to the Nak Muay
	/// strikes, raises their speed by the skill's ratio and lets Nak Muay
	/// skills ignore 10% of the target's defense.
	/// </summary>
	/// <remarks>
	/// The strikes' factor is Ram Muay's, see SCR_Get_SkillFactor_NakMuay_Attack.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.RamMuay_Buff)]
	public class NakMuay_RamMuay_BuffOverride : BuffHandler
	{
		private const float DefensePenetration = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM, GetCaptionRatio(buff, 1));

			if (buff.Target is not Character character)
				return;

			foreach (var attackId in new[] { SkillId.NakMuay_Attack, SkillId.NakMuay_Attack2 })
			{
				if (!character.Skills.Has(attackId))
					character.Skills.Add(new Skill(character, attackId));
			}

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.NakMuay_Attack);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.NakMuay_Attack2);

			if (character.Components.TryGet<SkillComponent>(out var skillComponent))
				skillComponent.InvalidateAll();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM);

			if (buff.Target is not Character character)
				return;

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.RamMuay_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Data.ClassName.StartsWith("NakMuay_", StringComparison.Ordinal))
				modifier.DefensePenetrationRate += DefensePenetration;
		}
	}
}
