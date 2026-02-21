//--- Melia Script ----------------------------------------------------------
// Skill Calculation Script
//--- Description -----------------------------------------------------------
// Functions that calculate skill-related values, such as properties.
//---------------------------------------------------------------------------

using System;
using System.Reflection.Emit;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Logging;
using static g4.RoundRectGenerator;

public class SkillCalculationsScript : GeneralScript
{
	/// <summary>
	/// Returns skill's AoE Attack Ratio?
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SR_LV(Skill skill)
	{
		var baseValue = skill.Properties.GetFloat(PropertyName.SklSR);

		var byOwner = 0f;
		byOwner += skill.Owner.Properties.GetFloat(PropertyName.SR);

		return Math.Max(1, baseValue + byOwner);
	}

	[ScriptableFunction]
	public float SCR_Get_SkillLv(Skill skill)
	{
		var fixedLevel = skill.Vars.GetFloat("FixedLevel");
		if (fixedLevel > 0)
			return fixedLevel;

		var value = skill.Properties.GetFloat(PropertyName.LevelByDB, 1);

		if (!skill.IsExpertSkill || !skill.LimitedInstanceLevelUp)
			value += skill.Properties.GetFloat(PropertyName.Level_BM, 0);

		value += skill.Properties.GetFloat(PropertyName.GemLevel_BM, 0);

		if (value == 0)
			return 0;

		if (value < 1)
			value = 1;

		return value;
	}

	/// <summary>
	/// Returns skill factor, which is a multiplier typically applied
	/// to an actors base damage.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SklFactor(Skill skill)
	{
		var baseValue = skill.Data.Factor;
		var byConfRate = 1f;

		if (skill.IsNormalAttack)
			byConfRate = ZoneServer.Instance.Conf.World.NormalAttackSkillFactorRate;
		else if (skill.Owner is IMonster)
			byConfRate = ZoneServer.Instance.Conf.World.MonsterSkillFactorRate;
		else
			byConfRate = ZoneServer.Instance.Conf.World.PlayerSkillFactorRate;

		return baseValue * byConfRate;
	}

	/// <summary>
	/// Returns skill factor by level, which is a multiplier typically applied
	/// to an actors base damage for each level of the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SklFactorByLevel(Skill skill)
	{
		var baseValue = skill.Data.FactorByLevel;
		var byConfRate = 1f;

		if (skill.IsNormalAttack)
			byConfRate = ZoneServer.Instance.Conf.World.NormalAttackSkillFactorRate;
		else if (skill.Owner is IMonster)
			byConfRate = ZoneServer.Instance.Conf.World.MonsterSkillFactorRate;
		else
			byConfRate = ZoneServer.Instance.Conf.World.PlayerSkillFactorRate;

		return baseValue * byConfRate;
	}

	/// <summary>
	/// Returns skill's skill factor, which in most cases in equivalent
	/// to the skill's damage in percentage.
	/// </summary>
	/// <example>
	/// var damage = SCR_GetRandomAtk(attacker, target, skill) * SCR_Get_SkillFactor(skill) / 100f;
	/// </example>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SkillFactor(Skill skill)
	{
		var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");

		var sklFactor = skill.Properties.GetFloat(PropertyName.SklFactor);
		var sklFactorByLevel = skill.Properties.GetFloat(PropertyName.SklFactorByLevel);

		var value = sklFactor + (sklFactorByLevel * (skill.Level - 1));

		var byReinforceRate = SCR_Get_AbilityReinforceRate(skill);
		value += value * byReinforceRate;

		return value;
	}

	/// <summary>
	/// Returns the reinforce ability rate for the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_AbilityReinforceRate(Skill skill)
	{
		if (skill.Data.ReinforceAbility == 0)
			return 0;

		if (!skill.Owner.Components.TryGet<AbilityComponent>(out var abilities))
			return 0;

		if (!abilities.TryGetActive(skill.Data.ReinforceAbility, out var reinforceAbility))
			return 0;

		var byAbility = reinforceAbility.Level * 0.005f;

		if (reinforceAbility.Level == 100)
			byAbility += 0.10f;

		var byHidden = 0f;
		if (skill.Data.HiddenReinforceAbility != 0 && abilities.TryGetActive(skill.Data.HiddenReinforceAbility, out var hiddenReinforceAbility))
		{
			var level = hiddenReinforceAbility.Level;
			var factorByLevel = skill.Data.HiddenReinforceAbilityFactorByLevel;

			byHidden = level * factorByLevel / 100f;
		}

		return byAbility + byHidden;
	}

	/// <summary>
	/// Returns skill's bonus damage.
	/// </summary>
	/// <example>
	/// var damage = SCR_GetRandomAtk(attacker, target, skill) + SCR_Get_SkillAtkAdd(skill);
	/// </example>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SkillAtkAdd(Skill skill)
	{
		var sklAtkAdd = skill.Properties.GetFloat(PropertyName.SklAtkAdd);
		var sklAtkAddByLevel = skill.Properties.GetFloat(PropertyName.SklAtkAddByLevel);

		return sklAtkAdd + (sklAtkAddByLevel * (skill.Level - 1));
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP(Skill skill)
	{
		var value = skill.Data.BasicSp;

		if (value == 0)
			return 0;

		// TODO: Abilities multiplier
		// var abilAddSP = this.GetAbilityAddSpendValue(pc, skill.Data.ClassName, "SP");
		// var value = basicSp + (level - 1) * lvUpSpendSp + abilAddSP;

		var owner = skill.Owner;

		// CarveZemina buff: Reduce SP cost by 20% + 2% per skill level
		if (skill.Data.CooldownGroup != CooldownId.ItemSetSkill && owner.TryGetBuff(BuffId.CarveZemina_Buff, out var zeminaBuff))
		{
			var zeminaSkillLevel = zeminaBuff.NumArg1;
			var reductionRate = 0.20f + (0.02f * zeminaSkillLevel);
			var reduction = value * reductionRate;
			value -= reduction;
		}

		if (value < 1)
			value = 0;

		if (skill.Id == SkillId.Scout_Cloaking)
			return 0;

		return (float)Math.Floor(value);
	}

	/// <summary>
	/// Returns the amount of SP spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SpendSP_Magic(Skill skill)
	{
		var value = SCR_Get_SpendSP(skill);
		var owner = skill.Owner;

		if (owner.IsBuffActive(BuffId.Wizard_Wild_buff))
			return (int)MathF.Floor(value * 1.5f);

		if (owner.IsBuffActive(BuffId.MalleusMaleficarum_Debuff))
			return (int)MathF.Floor(value * 2);

		return (int)Math.Max(0, value);
	}

	public float SCR_Get_SpendSP_Common_MovingForward(Skill skill)
	{
		var baseValue = .07f * skill.Owner.Properties.GetFloat(PropertyName.MSP);
		return (int)Math.Max(0, baseValue);
	}

	/// <summary>
	/// Returns the amount of stamina spent when using the skill.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Skill_STA(Skill skill)
	{
		var baseValue = skill.Data.BasicStamina;
		if (baseValue == 0)
			return 0;

		// The value in the database is in "displayed stamina", so they
		// need to be multiplied to get the actual value, which is a
		// thousand times the display value. Alternatively we could
		// adjust the skill data, but it's safer to leave the game's
		// data untouched.
		return baseValue * 1000;
	}

	/// <summary>
	/// Returns the skill's wave length, which is related to its area
	/// of effect.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_WaveLength(Skill skill)
	{
		var baseValue = skill.Properties.GetFloat(PropertyName.SklWaveLength);

		var byOwner = 0f;
		if (skill.Data.SplashType == SplashType.Square)
		{
			byOwner += skill.Owner.Properties.GetFloat(PropertyName.SkillRange);
			byOwner += skill.Owner.Properties.GetFloat(skill.Data.AttackType + "_Range");
		}

		return baseValue + byOwner;
	}

	/// <summary>
	/// Returns the skill's splash angle, which is related to its area
	/// of effect.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_SPLANGLE(Skill skill)
	{
		if (skill.Data.SplashType != SplashType.Fan)
			return skill.Properties.GetFloat(PropertyName.SklSplAngle);

		var baseValue = skill.Properties.GetFloat(PropertyName.SklSplAngle);
		var byOwner = skill.Owner.Properties.GetFloat(PropertyName.SkillAngle);

		return baseValue + byOwner;
	}

	/// <summary>
	/// Returns the skill's wave length, which is related to its attack
	/// range.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_Get_SplRange(Skill skill)
	{
		var baseValue = skill.Properties.GetFloat(PropertyName.SklSplRange);

		var byOwner = 0f;
		if (skill.Data.SplashType == SplashType.Fan)
		{
			byOwner += skill.Owner.Properties.GetFloat(PropertyName.SkillRange);
			byOwner += skill.Owner.Properties.GetFloat(skill.Data.AttackType + "_Range");
		}
		else if (skill.Data.SplashType == SplashType.Square)
		{
			byOwner += skill.Owner.Properties.GetFloat(PropertyName.SkillAngle);
		}

		return baseValue + byOwner;
	}

	/// <summary>
	/// Returns the skill's delay time, which appears to be used as the
	/// delay until a monster can use a skill again.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_DELAY_TIME(Skill skill)
	{
		if (skill.Owner is not IMonster monster)
			return skill.Properties.GetFloat(PropertyName.DelayTime);

		float baseValue;

		if (skill.Data.ClassType == SkillClassType.Missile || skill.Data.UseType == SkillUseType.Force || skill.Data.UseType == SkillUseType.ForceGround)
		{
			if (monster.Level < 75)
				baseValue = 3000;
			else if (monster.Level < 170)
				baseValue = 2500;
			else if (monster.Level < 220)
				baseValue = 2000;
			else
				baseValue = 1500;
		}
		else
		{
			if (monster.Level < 40)
				baseValue = 3000;
			else if (monster.Level < 75)
				baseValue = 2500;
			else if (monster.Level < 170)
				baseValue = 2000;
			else if (monster.Level < 220)
				baseValue = 1500;
			else
				baseValue = 1000;
		}

		var byDelayRate = ZoneServer.Instance.Conf.World.MonsterSkillDelayRate;

		return baseValue * byDelayRate;
	}

	/// <summary>
	/// Calculates and returns the skill's speed rate.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_SklSpdRate(Skill skill)
	{
		// Formulas based on player accounts and experimentation. They're
		// not 100% accurate, but the goal was to get them close to the
		// values seen in the skill packets, and the result kinda works.
		// And isn't that all that matters? Yes. Yes it is. ^__^
		//
		// https://forum.treeofsavior.com/t/attack-speed-formula/395989
		// 
		// We can see the skill speed rate in the skill packets, and we
		// can see it change with the dex stat, so this is definitely a
		// factor, and the formulas found by the community somewhat match
		// the logged values.
		// The bonuses from skills and buffs seem weird at first glance
		// (for example, Hasisas adds "175 Attack Speed"), but given our
		// formula this value actually makes sense, as Hasisas was logged
		// to increase the speed rate by ~0.18, which is a close match
		// for bonus/1000.
		// 
		// Theory on how attack speed works:
		// Every skill, including normal attack skills, have a "shoot time",
		// which seems to be the time until the skill can be used again.
		// For example, if you have a skill speed rate of 1 and you use
		// a skill with a shoot time of 1000, the client will use the
		// skill every ~1000ms. If you then raise the skill speed rate,
		// the client will use the skill every ~1000ms divided by the
		// speed rate. This means you'd reach a hit rate of 2 hits per
		// second if you get your speed rate to 2.
		// For a skill with a much lower shoot time you attack faster
		// out of the gate and higher speed rates will have a lower
		// impact on your hits/second. For example, the default shoot
		// time of the dagger attack skill is 400ms, which would be
		// lowered to 200ms with a speed rate of 2.
		// It's currently unknown whether there's a speed cap, though
		// the hits get a little unrealiable at higher values, above a
		// speed rate of 3. It's safe to assume that there is a limit
		// somewhere in the 2~3 speed rate range. But for now we'll
		// leave it like this for funsies.

		var baseValue = skill.Properties.GetFloat(PropertyName.SklSpdRateValue);
		var byDex = 0f;
		var byCharBonuses = 0f;

		if (skill.Owner is IMonster)
		{
			baseValue *= ZoneServer.Instance.Conf.World.MonsterSkillSpeedRate;
		}

		if (skill.Data.SpeedRateAffectedByDex)
		{
			var dex = skill.Owner.Properties.GetFloat(PropertyName.DEX);
			byDex = (float)dex / 500;
		}

		if (skill.Data.SpeedRateAffectedByBuff)
		{
			var aspd = skill.Owner.Properties.GetFloat(PropertyName.NormalASPD, 0);
			byCharBonuses = aspd / 1000f;
		}

		// Don't let the result go to 0, as such a speed rate would result
		// in an infinite damage delay. Limiting it to 0.01 effectively
		// means that a skill can't be modified to be more than 100
		// times slower than normal.
		return (float)Math.Max(0.01f, baseValue + byDex + byCharBonuses);
	}

	/// <summary>
	/// Calculates and returns the skill's shoot time.
	/// </summary>
	/// <param name="skill"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public float SCR_GET_ShootTime(Skill skill)
	{
		var sklSpdRate = skill.Properties.GetFloat(PropertyName.SklSpdRate, 1);
		var baseValue = skill.Data.ShootTime.TotalMilliseconds;

		return (float)(baseValue / sklSpdRate);
	}

	/// <summary>
	/// Calculates and returns the skill's cooldown time in milliseconds.
	/// </summary>
	/// <param name="skill">The skill to calculate the cooldown for.</param>
	/// <returns>The calculated cooldown in milliseconds.</returns>
	[ScriptableFunction]
	public float SCR_GET_COOLDOWN(Skill skill)
	{
		var owner = skill.Owner;
		var basicCooldown = (float)skill.Data.CooldownTime.TotalMilliseconds;

		var (coolDownClassify, zoneAddCoolDown) = ("None", 0f);

		if (skill.Id == SkillId.Cleric_Cure)
		{
			if (owner is Character character)
			{
				var jobList = new[] { JobId.Dievdirbys, JobId.Miko, JobId.Oracle, JobId.Kabbalist, JobId.Pardoner, JobId.Priest, JobId.PlagueDoctor };

				foreach (var job in jobList)
				{
					if (character.Jobs.Has(job, JobCircle.Second))
					{
						basicCooldown -= 3000;
					}
				}
			}

			basicCooldown = Math.Max(1000, basicCooldown);
		}

		if (skill.Data.Tags.Has(SkillTag.BasicSkill))
		{
			return basicCooldown;
		}
		// Seems to be a Legacy System, couldn't find an modern ies ref.
		// In Lua: GetClass('enchant_skill_list', skill_name) ~= nil
		// Return early for enchant skills.
		else
		{
			// CarveLaima buff: Reduce cooldown by 20% + 2% per skill level
			if (owner.IsBuffActive(BuffId.CarveLaima_Buff) && skill.Data.CooldownGroup != CooldownId.ItemSetSkill)
			{
				if (owner.TryGetBuff(BuffId.CarveLaima_Buff, out var laimaBuff))
				{
					var laimaSkillLevel = laimaBuff.NumArg1;
					var reductionRate = 0.20f + (0.02f * laimaSkillLevel);
					basicCooldown *= (1f - reductionRate);
				}
			}

			// AyinSof CoolTime Buff
			var ayinSofCoolTime = owner.GetTempVar("AyinSof_BUFF_COOLDOWN");
			if (ayinSofCoolTime != 0 && skill.Data.CooldownGroup != CooldownId.ItemSetSkill)
			{
				basicCooldown *= (1 - ayinSofCoolTime);
			}

			// Laima CoolTime Debuff
			if (!owner.IsBuffActive(BuffId.CarveLaima_Buff) && owner.IsBuffActive(BuffId.CarveLaima_Debuff) && skill.Data.CooldownGroup != CooldownId.ItemSetSkill)
			{
				basicCooldown *= 1.2f;
			}

			// Various Buffs (Event, Field Dungeon)
			// TODO: Implement logic for these buffs by calling their respective script functions.
			if (owner.IsBuffActive(BuffId.Event_Cooldown_SPamount_Decrease)) { /* SCR_COOLDOWN_SPAMOUNT_DECREASE */ }
			if (owner.IsBuffActive(BuffId.FIELD_COOLDOWNREDUCE_BUFF) || owner.IsBuffActive(BuffId.FIELD_DEFAULTCOOLDOWN_BUFF) || owner.IsBuffActive(BuffId.FIELD_COOLDOWNREDUCE_MIN_BUFF)) { /* SCR_FIELD_DUNGEON_CONSUME_DECREASE */ }

			// (WEEKLY BOSS RAID) Star Fall
			var monCoolDownRate = Math.Max(-9, owner.GetTempVar("MON_COOLDOWN_RATE")) * 0.1f;
			if (monCoolDownRate != 0)
			{
				basicCooldown += (basicCooldown * monCoolDownRate);
			}

			// GM Buff
			if (owner.IsBuffActive(BuffId.GM_Cooldown_Buff))
			{
				basicCooldown *= 0.9f;
			}

			// RootCrystal
			if (owner.IsBuffActive(BuffId.RootCrystalCoolDown_BUFF))
			{
				basicCooldown *= 0.5f;
			}

			// Goddess Armor Set Effects
			// TODO: Implement logic for Goddess Armor Set effects by adding support for skill data for casting categories in the database.
			if (owner is Character character && !owner.Map.IsPVP && !ZoneServer.Instance.World.IsPVP) // Hypothetical checks
			{
				var daliaStack = character.GetTempVar("ep12_dalia_leather_stack");
				if (daliaStack > 0 && skill.Data.CastingType == SkillCastingType.Channeling && skill.Data.Type == SkillType.Attack)
				{
					basicCooldown *= (1 - (0.05f * daliaStack));
				}

				var gabijaStack = character.GetTempVar("ep12_gabija_casting_stack");
				if (gabijaStack > 0 && (skill.Data.CastingType == SkillCastingType.Casting || skill.Data.CastingType == SkillCastingType.DynamicCasting) && skill.Data.Type == SkillType.Attack)
				{
					basicCooldown *= (1 - (0.05f * gabijaStack));
				}
			}

			// Rada/Jurate/Earring Options
			var radaCooldown = owner.GetTempVar("rada_cooldown");
			if (radaCooldown > 0) basicCooldown *= (1 - radaCooldown / 100f);

			var jurateCooldown = owner.GetTempVar("jurate_cooldown");
			if (jurateCooldown > 0) basicCooldown *= (1 - jurateCooldown / 100f);

			var earringRaidCooldown = owner.GetTempVar("earring_raid_cooldown");
			if (earringRaidCooldown > 0) basicCooldown *= (1 - earringRaidCooldown / 100f);

			// Tribulation Cooldown Increase
			var tribulationCooldown = owner.GetTempVar("tribulation_cooldown");
			if (tribulationCooldown > 0) basicCooldown *= (1 + (tribulationCooldown / 100f));

			// 2021 Seal Buff
			if (owner.IsBuffActive(BuffId.premium_seal_2021_buff) && !owner.IsBuffActive(BuffId.Event_Cooldown_SPamount_Decrease) && owner.Map.IsInstance) // Hypothetical check
			{
				basicCooldown *= 0.5f;
			}

			// Buff doesn't seem to exist in the database, but it is used in the client.
			// if (owner.IsBuffActive(BuffId.TOSHero_MonsterBuff_SkillCoolDownUp)) basicCooldown *= 1.1f;

			// Hero's Tale Zone
			if (owner.Map != null
				&& owner.Map.IsTOSHeroZone
				&& skill.Data.Type == SkillType.Attack
				&& !skill.IsNormalAttack) // Hypothetical checks
			{
				var tosHeroCooldownRate = owner.GetTempVar("TOSHero_CoolDownRate");
				if (tosHeroCooldownRate != 0) basicCooldown *= tosHeroCooldownRate;
			}

			// Legend Card
			var cardSkillCooldown = owner.GetTempVar("card_SkillCoolDown");
			if (cardSkillCooldown > 0) basicCooldown *= (1 - (cardSkillCooldown / 100f));

			// Hero's Tale Necklace
			if (owner.Map.IsTOSHeroZone
				&& skill.Data.Type == SkillType.Attack
				&& basicCooldown <= 20000
				&& owner.TryGetEquipItem(EquipSlot.Necklace, out var neck)
				&& owner.TryGetBuff(BuffId.TOSHero_Buff_Tear3_AttackSPD, out var buff)
				&& buff.OverbuffCounter >= 5 && neck.Id == ItemId.TOSHero_NECK_AS)
			{
				basicCooldown = 5000;
			}

			// Minimum cooldown for certain expert skills
			// TODO: Add support CooldownStartType in the skill data.
			// if (skill.Data.CoolDownStartType == "None" && 
			if (skill.IsExpertSkill && basicCooldown > 0 && basicCooldown < 1000)
			{
				basicCooldown = 1000;
			}
		}

		if (ZoneServer.Instance.World.IsPVP)
		{
			if (skill.Id == SkillId.Cleric_Heal) basicCooldown += 2000;
			if (skill.Id == SkillId.Priest_Revive) basicCooldown = 900000;
			if (skill.Id == SkillId.Priest_Resurrection && owner.IsAbilityActive(AbilityId.Priest39))
				basicCooldown = 600000;
		}

		if (owner.Map.IsGTW && skill.Id == SkillId.Priest_Resurrection && owner.IsAbilityActive(AbilityId.Priest39))
		{
			basicCooldown = 600000;
		}

		// Floor cooldown to the nearest second, then convert back to milliseconds.
		var ret = (float)Math.Floor(basicCooldown / 1000f) * 1000f;

		if (coolDownClassify == "Fix") ret = zoneAddCoolDown;
		else if (coolDownClassify == "Add") ret += zoneAddCoolDown;

		// Trial Skill Cooldown Increase
		var tbAddCoolDownRate = owner.GetTempVar("tb_add_cool_down_rate");
		if (tbAddCoolDownRate > 0)
		{
			ret += (float)Math.Floor(ret * tbAddCoolDownRate);
		}

		return (int)Math.Floor(ret);
	}
}
