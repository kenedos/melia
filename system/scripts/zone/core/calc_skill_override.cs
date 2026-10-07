//--- Melia Script ----------------------------------------------------------
// Skill Calculation Script
//--- Description -----------------------------------------------------------
// Functions that calculate skill-related values, such as properties.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;

public class SkillOverrideCalculationsScript : GeneralScript
{
	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Cleric_Heal(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		// Not sure if this is correct in any shape or form
		var value = SCR_Get_SpendSP(skill);

		var overloadBuffCount = skill.Owner.Components.Get<BuffComponent>().GetOverbuffCount(BuffId.Heal_Overload_Buff);
		value += (value * 0.5f * overloadBuffCount);

		return value;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Cleric_Cure(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);

		var overloadBuffCount = skill.Owner.Components.Get<BuffComponent>().GetOverbuffCount(BuffId.Cure_Overload_Buff);
		value += (value * 0.5f * overloadBuffCount);

		return value;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Wizard_EarthQuake(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);

		// Ability "Earthquake: Remove Knockdown"
		if (skill.Owner.IsAbilityActive(AbilityId.Wizard23))
			value += value * 0.10f;

		return value;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Necromancer_FleshHoop(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Necromancer2, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Necromancer_FleshCannon(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Necromancer2, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Necromancer_CreateShoggoth(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Necromancer8, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Necromancer_RaiseDead(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Necromancer22, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Necromancer_RaiseSkullwizard(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Necromancer24, 0.20f);

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, doubled by Gather Corpse: Expand.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Necromancer_GatherCorpse(Skill skill)
	{
		var SCR_Get_SR_LV = ScriptableFunctions.Skill.Get("SCR_Get_SR_LV");

		var value = SCR_Get_SR_LV(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Necromancer34))
			value *= 2;

		return value;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Schwarzereiter_Limacon(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);

		// Ability "Limacon: Spread"
		if (skill.Owner.IsAbilityActive(AbilityId.Schwarzereiter18))
			value += 5;

		return value;
	}

	/// <summary>
	/// Returns the skill's cooldown in milliseconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Schwarzereiter_AssaultFire(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		// Ability "Limacon: Spread"
		if (skill.Owner.IsAbilityActive(AbilityId.Schwarzereiter18))
			value += 10000;

		return value;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sorcerer_SummonFamiliar(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Sorcerer1, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Musketeer_Snipe(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Musketeer39, 0.30f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Templer_Retribution(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Templar20, 0.50f);

	/// <summary>
	/// Returns Expose Weakness's duration in seconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Appraiser_Blindside(Skill skill)
		=> 10 + skill.Level * 2;

	/// <summary>
	/// Returns Expose Weakness's minimum critical chance, halved by
	/// Expose Weakness: Dispersion.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Appraiser_Blindside(Skill skill)
	{
		var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");

		var value = 6 + (skill.Level - 1) * 0.5f;
		value += value * SCR_Get_AbilityReinforceRate(skill);

		// Ability "[Arts] Expose Weakness: Dispersion"
		if (skill.Owner.IsAbilityActive(AbilityId.Appraiser7))
			value /= 2;

		return value;
	}

	/// <summary>
	/// Returns High Scale Magnifying Glass's accuracy and block
	/// penetration bonus, which is the skill's factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Appraiser_HighMagnifyingGlass(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, raised by Zenith.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Mergen_Unload(Skill skill)
		=> GetSrWithZenith(skill);

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, raised by Zenith.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Mergen_TrickShot(Skill skill)
		=> GetSrWithZenith(skill);

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, raised by Zenith.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Mergen_FocusFire(Skill skill)
		=> GetSrWithZenith(skill);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Mergen_FocusFire(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Targeted Arrow: Quick Charge"
		if (skill.Owner.IsAbilityActive(AbilityId.Mergen12))
			rate += 0.20f;

		// Ability "Homing Arrow: Shackle"
		if (skill.Owner.IsAbilityActive(AbilityId.Mergen29))
			rate += 0.30f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the skill's AoE Attack Ratio with Zenith's bonus, which
	/// Zenith: Fire For Effect trades for damage, capped at 21.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	private static float GetSrWithZenith(Skill skill)
	{
		const float MaxZenithSr = 21;

		var SCR_Get_SR_LV = ScriptableFunctions.Skill.Get("SCR_Get_SR_LV");
		var value = SCR_Get_SR_LV(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Mergen26) || !skill.Owner.TryGetSkill(SkillId.Mergen_Zenith, out var zenith))
			return value;

		return Math.Min(MaxZenithSr, value + zenith.Properties.GetFloat(PropertyName.CaptionRatio));
	}

	/// <summary>
	/// Returns Down Fall's duration in seconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Mergen_DownFall(Skill skill)
		=> 3.5f + skill.Level * 0.5f;

	/// <summary>
	/// Returns the seconds between Down Fall's volleys.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Mergen_DownFall(Skill skill)
		=> 1;

	/// <summary>
	/// Returns Invulnerable's accuracy bonus in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Zealot_Invulnerable(Skill skill)
		=> 10 + (skill.Level - 1) * 10 / 9f;

	/// <summary>
	/// Returns Immolation's fire property resistance bonus, granted by
	/// Immolation: Fire Property Resistance.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Zealot_Immolation(Skill skill)
		=> skill.Owner.TryGetActiveAbilityLevel(AbilityId.Zealot4, out var level) ? level * 300 : 0;

	/// <summary>
	/// Returns the SP Fanatic Illusion drains per second.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Zealot_FanaticIllusion(Skill skill)
		=> MathF.Round(15 - (skill.Level - 1) * 10 / 9f);

	/// <summary>
	/// Returns Fanatic Illusion's lightning property resistance bonus in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Zealot_FanaticIllusion(Skill skill)
		=> skill.Level * 10;

	/// <summary>
	/// Returns the number of enemies Fanatic Illusion hits, doubled by
	/// [Arts] Fanatic Illusion: Blind Faith.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Zealot_FanaticIllusion(Skill skill)
		=> skill.Owner.IsAbilityActive(AbilityId.Zealot16) ? 12 : 6;

	/// <summary>
	/// Returns Emphatic Trust's duration in seconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Zealot_EmphasisTrust(Skill skill)
		=> 15 + skill.Level * 2;

	/// <summary>
	/// Returns the number of enemies Emphatic Trust marks.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Zealot_EmphasisTrust(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Zealot_Immolation(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Immolation: Fire Property Attack"
		if (skill.Owner.IsAbilityActive(AbilityId.Zealot1))
			rate += 0.20f;

		// Ability "Immolation: Fire Property Resistance"
		if (skill.Owner.IsAbilityActive(AbilityId.Zealot4))
			rate += 0.10f;

		// Ability "Immolation: Melt Armor"
		if (skill.Owner.IsAbilityActive(AbilityId.Zealot9))
			rate += 0.10f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Zealot_BeadyEyed(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Zealot8, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Zealot_Fanaticism(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Zealot10, 0.10f);

	/// <summary>
	/// Returns the skill's cooldown, raised by 5 seconds with [Arts] Fanatic
	/// Illusion: Blind Faith.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Zealot_FanaticIllusion(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Zealot16))
			value += 5000;

		return value;
	}

	/// <summary>
	/// Returns Dissonanz's stun duration in seconds, halved in PvP.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PiedPiper_Dissonanz(Skill skill)
		=> skill.Owner.Map?.IsPVP == true ? 2.5f : 5f;

	/// <summary>
	/// Returns the number of monsters Hypnotische Floete hypnotizes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PiedPiper_HypnotischeFlote(Skill skill)
		=> 1 + skill.Level / 2;

	/// <summary>
	/// Returns Marschierendeslied's block count, which is the skill's factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PiedPiper_Marschierendeslied(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns Lied des Weltbaum's damage bonus, which is the skill's factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PiedPiper_LiedDerWeltbaum(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns Lied des Weltbaum's duration in seconds, extended by
	/// Lied des Weltbaum: Duration.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_PiedPiper_LiedDerWeltbaum(Skill skill)
		=> 10 + (skill.Owner.TryGetActiveAbilityLevel(AbilityId.PiedPiper15, out var level) ? level : 0);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PiedPiper_HypnotischeFlote(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.PiedPiper6, 0.30f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PiedPiper_LiedDerWeltbaum(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.PiedPiper15, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PiedPiper_Improvisation(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.PiedPiper16, 0.20f);

	/// <summary>
	/// Returns the skill's cooldown, 10 seconds shorter with Stegreifspiel:
	/// Reduce Cooldown.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_PiedPiper_Improvisation(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.PiedPiper16))
			value = Math.Max(0, value - 10000);

		return value;
	}

	/// <summary>
	/// Returns the number of enemies Incineration ignites.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PlagueDoctor_Incineration(Skill skill)
		=> 8;

	/// <summary>
	/// Returns Incineration's base duration in seconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_PlagueDoctor_Incineration(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the number of allies Fumigate cures.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PlagueDoctor_Fumigate(Skill skill)
		=> 5;

	/// <summary>
	/// Returns the number of enemies Fumigate: Perfusion hits.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_PlagueDoctor_Fumigate(Skill skill)
		=> 10;

	/// <summary>
	/// Returns Black Death Steam's critical resistance reduction in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_PlagueDoctor_PlagueVapours(Skill skill)
		=> skill.Level * 2;

	/// <summary>
	/// Returns the number of enemies Black Death Steam poisons.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_PlagueDoctor_PlagueVapours(Skill skill)
		=> 4;

	/// <summary>
	/// Returns Black Death Steam's duration in seconds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_PlagueDoctor_PlagueVapours(Skill skill)
		=> 15;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PlagueDoctor_Incineration(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.PlagueDoctor13, 0.40f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PlagueDoctor_Fumigate(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Fumigate: Purification"
		if (skill.Owner.IsAbilityActive(AbilityId.PlagueDoctor6))
			rate += 0.10f;

		// Ability "Fumigate: Sanitize"
		if (skill.Owner.IsAbilityActive(AbilityId.PlagueDoctor22))
			rate += 5f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_PlagueDoctor_Pandemic(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.PlagueDoctor14, 0.30f);

	/// <summary>
	/// Returns the skill's cooldown, 15 seconds shorter with Fumigate:
	/// Perfusion.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_PlagueDoctor_Fumigate(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.PlagueDoctor29))
			value = Math.Max(0, value - 15000);

		return value;
	}

	/// <summary>
	/// Returns the number of enemies Rubric strikes, raised by Rubric:
	/// Propagate.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Exorcist_Rubric(Skill skill)
		=> 5 + (skill.Owner.TryGetActiveAbilityLevel(AbilityId.Exorcist2, out var level) ? level : 0);

	/// <summary>
	/// Returns the tenths of a second between Rubric's strikes in the
	/// tooltip's "0.N" format, faster with Rubric: Speed Reading.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Exorcist_Rubric(Skill skill)
		=> skill.Owner.IsAbilityActive(AbilityId.Exorcist3) ? 25 : 5;

	/// <summary>
	/// Returns Rubric's maximum duration in seconds, shorter with Rubric:
	/// Speed Reading.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Exorcist_Rubric(Skill skill)
		=> skill.Owner.IsAbilityActive(AbilityId.Exorcist3) ? 2 : 4;

	/// <summary>
	/// Returns the number of enemies Aqua Benedicta damages.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Exorcist_AquaBenedicta(Skill skill)
		=> 10;

	/// <summary>
	/// Returns Engkrateia's duration in seconds, extended by Engkrateia:
	/// Patience.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Exorcist_Engkrateia(Skill skill)
		=> 3 + (skill.Owner.TryGetActiveAbilityLevel(AbilityId.Exorcist8, out var level) ? level : 0);

	/// <summary>
	/// Returns the damage bonus of Gregorate: Magic in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Exorcist_Gregorate(Skill skill)
		=> 17.5f + skill.Level * 1.75f;

	/// <summary>
	/// Returns the number of enemies Gregorate strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Exorcist_Gregorate(Skill skill)
		=> 5;

	/// <summary>
	/// Returns the number of enemies Katadikazo strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Exorcist_Katadikazo(Skill skill)
		=> 15;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Exorcist_Rubric(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Rubric: Propagate"
		if (skill.Owner.IsAbilityActive(AbilityId.Exorcist2))
			rate += 0.30f;

		// Ability "Rubric: Speed Reading"
		if (skill.Owner.IsAbilityActive(AbilityId.Exorcist3))
			rate += 0.50f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Exorcist_Engkrateia(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Engkrateia: Patience"
		if (skill.Owner.IsAbilityActive(AbilityId.Exorcist8))
			rate += 0.20f;

		// Ability "Engkrateia: The Goddess' Reply"
		if (skill.Owner.IsAbilityActive(AbilityId.Exorcist9))
			rate += 0.50f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, raised by Bazooka and by
	/// [Arts] Cannon Shot: Howitzer.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Cannoneer_CannonShot(Skill skill)
	{
		var value = GetSrWithBazooka(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Cannoneer32))
			value += 5;

		return value;
	}

	/// <summary>
	/// Returns the skill's AoE Attack Ratio, raised by Bazooka.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV_Cannoneer_CannonBarrage(Skill skill)
		=> GetSrWithBazooka(skill);

	/// <summary>
	/// Returns the skill's AoE Attack Ratio with Bazooka's bonus while the
	/// Bazooka is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	private static float GetSrWithBazooka(Skill skill)
	{
		var SCR_Get_SR_LV = ScriptableFunctions.Skill.Get("SCR_Get_SR_LV");
		var value = SCR_Get_SR_LV(skill);

		if (skill.Owner.IsBuffActive(BuffId.Bazooka_Buff) && skill.Owner.TryGetSkill(SkillId.Cannoneer_Bazooka, out var bazooka))
			value += bazooka.Properties.GetFloat(PropertyName.CaptionRatio2);

		return value;
	}

	/// <summary>
	/// Returns the skill's cooldown, doubled while Bazooka is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Cannoneer_CannonShot(Skill skill)
		=> GetCooldownWithBazooka(skill);

	/// <summary>
	/// Returns the skill's cooldown, doubled while Bazooka is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Cannoneer_CannonBarrage(Skill skill)
		=> GetCooldownWithBazooka(skill);

	/// <summary>
	/// Returns the skill's cooldown, doubled while Bazooka is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	private static float GetCooldownWithBazooka(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");
		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsBuffActive(BuffId.Bazooka_Buff))
			value *= 2;

		return value;
	}

	/// <summary>
	/// Returns the skill's cooldown, 10 seconds longer with [Arts] Sweeping
	/// Cannon: Siege Shot.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Cannoneer_SweepingCannon(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Cannoneer25))
			value += 10000;

		return value;
	}

	/// <summary>
	/// Returns the number of enemies Smoke Grenade blinds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Cannoneer_SmokeGrenade(Skill skill)
		=> 20;

	/// <summary>
	/// Returns the double pistol basic attack's factor, which is Double Gun
	/// Stance's.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SkillFactor_DoubleGun_Attack(Skill skill)
	{
		if (skill.Owner.TryGetSkill(SkillId.Bulletmarker_DoubleGunStance, out var stance))
			return stance.Properties.GetFloat(PropertyName.SkillFactor);

		var SCR_Get_SkillFactor = ScriptableFunctions.Skill.Get("SCR_Get_SkillFactor");
		return SCR_Get_SkillFactor(skill);
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Bulletmarker_BloodyOverdrive(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Bloody Overdrive: Ricochet"
		if (skill.Owner.IsAbilityActive(AbilityId.Bulletmarker8))
			rate += 0.30f;

		// Ability "Bloody Overdrive: Invincible"
		if (skill.Owner.IsAbilityActive(AbilityId.Bulletmarker12))
			rate += 0.30f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Bulletmarker_MozambiqueDrill(Skill skill)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		// Ability "Mozambique Drill: Ignore Defense"
		if (skill.Owner.IsAbilityActive(AbilityId.Bulletmarker9))
			rate += 0.30f;

		// Ability "Mozambique Drill: Ricochet"
		if (skill.Owner.IsAbilityActive(AbilityId.Bulletmarker10))
			rate += 0.30f;

		return value + value * rate;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Bulletmarker_FreezeBullet(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Bulletmarker24, 0.20f);

	/// <summary>
	/// Returns the evasion Flowering takes per stack, in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_BlossomBlader_Flowering(Skill skill)
		=> skill.Level * 2;

	/// <summary>
	/// Returns the number of Flowering stacks an enemy can carry.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_BlossomBlader_Flowering(Skill skill)
		=> skill.Level + 1;

	/// <summary>
	/// Returns StartUp's critical damage bonus at full charge, which is
	/// the skill's factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_BlossomBlader_StartUp(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_BlossomBlader_FallenBlossom(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Blossomblader4, 0.50f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_BlossomBlader_Flash(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Blossomblader7, 0.50f);

	/// <summary>
	/// Returns the skill's cooldown, 10 seconds longer with [Arts] Fallen
	/// Blossom: Blossom Flows.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_BlossomBlader_FallenBlossom(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Blossomblader21))
			value += 10000;

		return value;
	}

	/// <summary>
	/// Returns the attack speed Ram Muay adds to basic attacks.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_NakMuay_RamMuay(Skill skill)
		=> skill.Level * 10;

	/// <summary>
	/// Returns the Nak Muay strike's factor, which is Ram Muay's.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SkillFactor_NakMuay_Attack(Skill skill)
		=> GetRamMuayFactor(skill);

	/// <summary>
	/// Returns the Nak Muay strike's factor, which is Ram Muay's.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SkillFactor_NakMuay_Attack2(Skill skill)
		=> GetRamMuayFactor(skill);

	/// <summary>
	/// Returns Ram Muay's factor, or the skill's own without Ram Muay.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	private static float GetRamMuayFactor(Skill skill)
	{
		if (skill.Owner.TryGetSkill(SkillId.NakMuay_RamMuay, out var ramMuay))
			return ramMuay.Properties.GetFloat(PropertyName.SkillFactor);

		var SCR_Get_SkillFactor = ScriptableFunctions.Skill.Get("SCR_Get_SkillFactor");
		return SCR_Get_SkillFactor(skill);
	}

	/// <summary>
	/// Returns Muay Boran's final damage bonus in percent, which grows with
	/// each of Te Kha, Te Trong and Sok Chiang learned.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_NakMuay_MuayThai(Skill skill)
	{
		var count = 0;

		foreach (var skillId in new[] { SkillId.NakMuay_TeKha, SkillId.NakMuay_TeTrong, SkillId.NakMuay_SokChiang })
		{
			if (skill.Owner.TryGetSkill(skillId, out _))
				count++;
		}

		return 10 + skill.Level * count * 0.35f;
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Shinobi_Bunshin_no_jutsu(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Shinobi6, 0.10f);

	/// <summary>
	/// Returns the skill's cooldown, 5 seconds longer with [Arts] Raiton no
	/// Jutsu: Hirai.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Shinobi_Raiton_no_Jutsu(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Shinobi19))
			value += 5000;

		return value;
	}

	/// <summary>
	/// Returns Floral Scent's healing factor per second, in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Druid_Chortasmata(Skill skill)
		=> 41 + 7.6f * (skill.Level - 1);

	/// <summary>
	/// Returns the number of enemies Chortasmata's rash reaches.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Druid_Chortasmata(Skill skill)
		=> 9;

	/// <summary>
	/// Returns the number of enemies Seed Bomb seeds.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Druid_Seedbomb(Skill skill)
		=> 5;

	/// <summary>
	/// Returns the number of enemies a Seed Bomb burst hits.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Druid_Seedbomb(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the wolf's damage bonus in percent, which is the skill's
	/// factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Druid_Lycanthropy(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns the wolf's critical rate bonus in percent, which is the
	/// skill's factor.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Druid_Lycanthropy(Skill skill)
		=> skill.Properties.GetFloat(PropertyName.SkillFactor);

	/// <summary>
	/// Returns the critical resistance Hamaya takes, in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Miko_Hamaya(Skill skill)
		=> skill.Level * 2;

	/// <summary>
	/// Returns how many seconds Hamaya's circle lasts.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Miko_Hamaya(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the number of enemies Hamaya's circle burns.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Miko_Hamaya(Skill skill)
		=> 10;

	/// <summary>
	/// Returns Great Blessing: Honor's attack bonus in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Miko_Omikuji(Skill skill)
		=> 7.5f + 2.5f * skill.Level;

	/// <summary>
	/// Returns the bonus of Great Blessing: Hope and Great Blessing: Safety.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Miko_Omikuji(Skill skill)
		=> 90 + 10 * skill.Level;

	/// <summary>
	/// Returns Great Blessing: Health's damage reduction in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Miko_Omikuji(Skill skill)
		=> 22.5f + 2.5f * skill.Level;

	/// <summary>
	/// Returns Kagura's damage bonus at the start of the dance, in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Miko_KaguraDance(Skill skill)
		=> 12 + skill.Level * 3;

	/// <summary>
	/// Returns Kagura's damage bonus at the end of the dance, in percent.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Miko_KaguraDance(Skill skill)
		=> 16 + skill.Level * 3;

	/// <summary>
	/// Returns the number of enemies Kagura: Ken strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio3_Miko_KaguraDance(Skill skill)
		=> 10;

	/// <summary>
	/// Returns how many seconds Omikuji's blessings last, extended by
	/// Omikuji: Enhance.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_Miko_Omikuji(Skill skill)
	{
		var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");

		return 20 * (1 + SCR_Get_AbilityReinforceRate(skill));
	}

	/// <summary>
	/// Returns how many seconds Kagura can be danced, shorter with Kagura:
	/// Ken.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_Miko_KaguraDance(Skill skill)
		=> skill.Owner.IsAbilityActive(AbilityId.Miko18) ? 5 : 15;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Miko_Gohei(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Miko9, 1f);

	/// <summary>
	/// Returns Over-Reinforce's attack bonus, a share of the Enchanter's
	/// average physical attack without buffs.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Enchanter_OverReinforce(Skill skill)
		=> (float)Math.Floor(EnchanterSkillHelper.GetBasePAtk(skill.Owner) * (0.015f + skill.Level * 0.004f));

	/// <summary>
	/// Returns the SP Enchant Aura drains per tick, which scales with the
	/// Enchanter's level.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Enchanter_EnchantAura(Skill skill)
	{
		var levelOffset = skill.Owner.Properties.GetFloat(PropertyName.Lv) - 300;
		var levelRate = levelOffset < 0 ? 2.75f : 1.25f;

		return Math.Max(0, (float)Math.Floor(100 * (1 + levelOffset * levelRate / 1000)));
	}

	/// <summary>
	/// Returns the seconds between Enchant Aura's ticks.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Enchanter_EnchantAura(Skill skill)
		=> 2;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_GodSmash(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor12, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_MalleusMaleficarum(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor11, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_Judgment(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor15, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_IronMaiden(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor18, 0.30f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_BreastRipper(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor19, 0.20f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Inquisitor_BreakingWheel(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Inquisitor20, 0.20f);

	/// <summary>
	/// Returns the number of pears Pear of Anguish can keep installed.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Inquisitor_PearofAnguish(Skill skill)
		=> 5;

	/// <summary>
	/// Returns Breaking Wheel's duration in seconds, raised by Breaking
	/// Wheel: Reinforce.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_Inquisitor_BreakingWheel(Skill skill)
		=> 10 + (skill.Owner.TryGetActiveAbilityLevel(AbilityId.Inquisitor20, out var level) ? level : 0);

	/// <summary>
	/// Returns the number of enemies Ripper cuts, raised by a third of the
	/// Inquisitor's AoE Attack Ratio.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Inquisitor_BreastRipper(Skill skill)
		=> (float)Math.Floor(8 + Math.Max(0, skill.Owner.Properties.GetFloat(PropertyName.SR) / 3));

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Onmyoji_GreenwoodShikigami(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Onmyoji5, 0.50f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Onmyoji_WhiteTigerHowling(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Onmyoji8, 1f);

	/// <summary>
	/// Returns the number of enemies Greenwood Shikigami strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Onmyoji_GreenwoodShikigami(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the number of enemies Howling White Tiger strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Onmyoji_WhiteTigerHowling(Skill skill)
		=> 4 + skill.Level;

	/// <summary>
	/// Returns the number of enemies Wind Shikigami strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Onmyoji_WaterShikigami(Skill skill)
		=> 12;

	/// <summary>
	/// Returns the number of enemies Toyou strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Onmyoji_Toyou(Skill skill)
		=> 15;

	/// <summary>
	/// Returns the number of enemies Yin Yang Harmony strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Onmyoji_YinYangConsonance(Skill skill)
		=> 15;

	/// <summary>
	/// Returns Rune of Protection's duration in minutes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_RuneCaster_Algiz(Skill skill)
		=> 30;

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sage_MicroDimension(Skill skill)
		=> GetSpendSpWithAbilities(skill, (AbilityId.Sage4, 0.20f), (AbilityId.Sage11, 0.30f));

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sage_UltimateDimension(Skill skill)
		=> GetSpendSpWithAbilities(skill, (AbilityId.Sage9, 0.30f), (AbilityId.Sage12, 0.30f));

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sage_Blink(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Sage5, 0.10f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sage_MissileHole(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Sage13, 0.30f);

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Sage_DimensionCompression(Skill skill)
		=> GetSpendSpWithAbility(skill, AbilityId.Sage18, 0.30f);

	/// <summary>
	/// Returns the skill's cooldown, raised by Blink: Looming.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Sage_Blink(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Sage14))
			value += 10000;

		return value;
	}

	/// <summary>
	/// Returns the skill's cooldown, raised by [Arts] Missile Hole: Master
	/// of Dimensions.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN_Sage_MissileHole(Skill skill)
	{
		var SCR_GET_COOLDOWN = ScriptableFunctions.Skill.Get("SCR_GET_COOLDOWN");

		var value = SCR_GET_COOLDOWN(skill);

		if (skill.Owner.IsAbilityActive(AbilityId.Sage22))
			value += 45000;

		return value;
	}

	/// <summary>
	/// Returns the number of enemies Micro Dimension strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Sage_MicroDimension(Skill skill)
		=> 3;

	/// <summary>
	/// Returns the number of enemies Ultimate Dimension strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Sage_UltimateDimension(Skill skill)
		=> 10;

	/// <summary>
	/// Returns Blink's maximum teleport distance.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Sage_Blink(Skill skill)
		=> 100 + skill.Level * 10;

	/// <summary>
	/// Returns how long Blink's apparition stays, raised by Blink: Duration.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionTime_Sage_Blink(Skill skill)
		=> skill.Level * 2 + (skill.Owner.TryGetActiveAbilityLevel(AbilityId.Sage5, out var level) ? level : 0);

	/// <summary>
	/// Returns the number of enemies Dimension Compression pulls in.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio2_Sage_DimensionCompression(Skill skill)
		=> 15;

	/// <summary>
	/// Returns the number of enemies Hole of Darkness strikes.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_CaptionRatio_Sage_HoleOfDarkness(Skill skill)
		=> 10;

	/// <summary>
	/// Returns the skill's SP cost, raised by the given rate while the
	/// ability is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <param name="abilityId"></param>
	/// <param name="rate"></param>
	/// <returns></returns>
	private static float GetSpendSpWithAbility(Skill skill, AbilityId abilityId, float rate)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);

		if (skill.Owner.IsAbilityActive(abilityId))
			value += value * rate;

		return value;
	}

	/// <summary>
	/// Returns the skill's SP cost, raised by each given rate whose ability
	/// is active.
	/// </summary>
	/// <param name="skill"></param>
	/// <param name="surcharges"></param>
	/// <returns></returns>
	private static float GetSpendSpWithAbilities(Skill skill, params (AbilityId AbilityId, float Rate)[] surcharges)
	{
		var SCR_Get_SpendSP = ScriptableFunctions.Skill.Get("SCR_Get_SpendSP");

		var value = SCR_Get_SpendSP(skill);
		var rate = 0f;

		foreach (var surcharge in surcharges)
		{
			if (skill.Owner.IsAbilityActive(surcharge.AbilityId))
				rate += surcharge.Rate;
		}

		return value + value * rate;
	}

}
