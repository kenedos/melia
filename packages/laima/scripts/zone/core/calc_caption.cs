//--- Melia Script ----------------------------------------------------------
// Caption Ratio Override Script
//--- Description -----------------------------------------------------------
// Per-skill overrides for caption ratios/time whose value depends on more
// than skill level, reinforce ability and a static cap - a live stat, an
// ability check, and so on. A plain maximum (e.g. a damage reduction buff
// that never exceeds some %) is data, not code: see captionRatio1Max/2Max/
// 3Max in skills_overrides.txt, applied by the generic SCR_Get_CaptionRatioN
// in calc_skill.cs. SkillProperties.CalculateProperty looks for a function
// named "SCR_Get_CaptionRatioN_{ClassName}" (or "SCR_Get_CaptionTime_
// {ClassName}") before falling back to that generic formula, so a skill
// only needs an entry here if its caption reads something the data alone
// can't express.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

public class CaptionCalculationsScript : GeneralScript
{
	/// <summary>
	/// Swell Hands' second slot is flat PATK per point of the caster's
	/// INT, not a plain per-level coefficient. CaptionOverrideDependsOn
	/// declares that live, right here, so a buff that changes INT resends
	/// this override without BuffHandler having to know about it.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_Get_CaptionRatio2_Thaumaturge_SwellHands")]
	[CaptionOverrideDependsOn(PropertyName.INT, PropertyName.INT_BM)]
	public float SCR_Get_SwellHands_CaptionRatio2(Skill skill)
	{
		var coefficient = skill.Data.CaptionRatio2 + (skill.Data.CaptionRatio2ByLevel * skill.Level);
		var casterInt = skill.Owner.Properties.GetFloat(PropertyName.INT);

		return MathF.Floor(coefficient * casterInt);
	}

	/// <summary>
	/// Resist Elements' reduction is boosted by the caster's Paladin37
	/// ability (0.5% per level) and capped at 90% by
	/// ResistElements_BuffOverride. The ability check is what makes this
	/// an override; the cap alone would just be captionRatio1Max.
	/// </summary>
	/// <remarks>
	/// No CaptionOverrideDependsOn here - ability levels aren't a property
	/// AddPropertyModifier touches, so learning/resetting Paladin37 doesn't
	/// currently resend this override. Same class of staleness as a live
	/// stat, just not wired to a trigger yet.
	/// </remarks>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_Get_CaptionRatio_Paladin_ResistElements")]
	public float SCR_Get_ResistElements_CaptionRatio(Skill skill)
	{
		var baseReduction = skill.Data.CaptionRatio1 + (skill.Data.CaptionRatio1ByLevel * skill.Level);
		baseReduction += baseReduction * ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate")(skill);

		var abilityMultiplier = 1f;
		if (skill.Owner.TryGetActiveAbilityLevel(AbilityId.Paladin37, out var abilityLevel))
			abilityMultiplier += abilityLevel * 0.005f;

		return MathF.Min(90f, baseReduction * abilityMultiplier);
	}

	/// <summary>
	/// Oblation's offering box holds a flat seven items per skill level.
	/// The client sizes its own grid from the same figure, so the reinforce
	/// ability the generic formula folds in would put the two out of step.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction("SCR_Get_CaptionRatio_Pardoner_Oblation")]
	public float SCR_Get_Oblation_CaptionRatio(Skill skill)
		=> skill.Data.CaptionRatio1 + (skill.Data.CaptionRatio1ByLevel * skill.Level);
}
