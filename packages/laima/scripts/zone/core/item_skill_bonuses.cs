//--- Melia Script ----------------------------------------------------------
// Item Equip Effects Registry
//--- Description -----------------------------------------------------------
// Central registry defining ALL equipment special effects. Each item's
// effects are defined together in one place. The effects are:
//
//   JobSkillUp  — +N to all skills of a job (auto-calculated in SCR_Get_SkillLv)
//   SkillUp     — +N to a specific skill (auto-calculated in SCR_Get_SkillLv)
//   ExProp      — set a custom character property (applied/removed on equip/unequip)
//   PropMod     — modify a standard property (applied/removed on equip/unequip)
//   Buff        — apply a permanent buff (applied/removed on equip/unequip)
//
// item_equip.cs reads from this registry to apply/remove effects.
// calc_skill.cs reads from this registry for auto-calculated skill levels.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

public enum ItemEffectType
{
	JobSkillUp,
	SkillUp,
	ExProp,
	PropMod,
	Buff,
}

public struct ItemEffect
{
	public ItemEffectType Type;
	public JobId Job;
	public SkillId Skill;
	public BuffId BuffId;
	public string PropName;
	public float Value;

	public static ItemEffect JobSkillUp(JobId job, int bonus)
		=> new() { Type = ItemEffectType.JobSkillUp, Job = job, Value = bonus };

	public static ItemEffect SkillUp(SkillId skill, int bonus)
		=> new() { Type = ItemEffectType.SkillUp, Skill = skill, Value = bonus };

	public static ItemEffect ExProp(string propName, float value)
		=> new() { Type = ItemEffectType.ExProp, PropName = propName, Value = value };

	public static ItemEffect PropMod(string propName, float value)
		=> new() { Type = ItemEffectType.PropMod, PropName = propName, Value = value };

	public static ItemEffect Buff(BuffId buffId)
		=> new() { Type = ItemEffectType.Buff, BuffId = buffId };
}

public static class ItemEquipEffects
{
	/// <summary>
	/// Central registry of all item equip effects.
	/// Key: item numeric ID, Value: array of effects.
	/// </summary>
	public static readonly Dictionary<int, ItemEffect[]> Registry = new()
	{
		// Swords
		{ 103102, new[] { ItemEffect.JobSkillUp(JobId.Swordsman, 2) } },  // SWD03_102
		{ 104123, new[] { ItemEffect.ExProp("ITEM_STORMBOLT_ADDBLOW", 12) } },  // SWD04_123

		// Two-Handed Swords
		{ 124119, new[] { ItemEffect.ExProp("ITEM_Doppelsoeldner_Cyclone_MovingShot", 500) } },  // TSW04_119
		{ 124123, new[] { ItemEffect.ExProp("ITEM_BABARIAN_FRENZY_STACK", 5) } },  // TSW04_123

		// Daggers
		{ 114013, new[] { ItemEffect.SkillUp(SkillId.Corsair_ImpaleDagger, 1) } },  // DAG04_113
		{ 114019, new[] { ItemEffect.ExProp("ITEM_HASISAS_CRTHRRATIO_UP", 3) } },  // DAG04_119

		// Rods
		{ 142102, new[] { ItemEffect.JobSkillUp(JobId.Cryomancer, 1) } },  // STF02_102
		{ 142105, new[] { ItemEffect.SkillUp(SkillId.Wizard_EarthQuake, 3) } },  // STF02_105
		{ 144123, new[] { ItemEffect.ExProp("ITEM_RAISESKELETON_SUMMONCOUNT", 1) } },  // STF04_123
		{ 274124, new[] { ItemEffect.ExProp("ITEM_METEOR_MULTIHIT", 5) } },  // TSF04_124

		// Two-Handed Bows
		{ 162107, new[] { ItemEffect.JobSkillUp(JobId.Hunter, 2) } },  // TBW02_107
		{ 164122, new[] { ItemEffect.ExProp("ITEM_AROORWRAIN_SKLRANGE", 9) } },  // TBW04_122
		{ 164124, new[] { ItemEffect.ExProp("ITEM_MERGEN_TRICKSHOT", 25) } },  // TBW04_124

		// Crossbows
		{ 182103, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 2) } },  // BOW02_103
		{ 182109, new[] { ItemEffect.SkillUp(SkillId.Rogue_Backstab, 3) } },  // BOW02_109
		{ 183110, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 2) } },  // BOW03_110
		{ 183201, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 2) } },  // BOW03_201
		{ 183303, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 1) } },  // BOW03_303
		{ 184108, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 1) } },  // BOW04_108
		{ 184117, new[] { ItemEffect.JobSkillUp(JobId.QuarrelShooter, 2) } },  // BOW04_117
		{ 184122, new[] { ItemEffect.ExProp("ITEM_QuarrelShooter_DeployPavise_Ballista", 1) } },  // BOW04_122
		{ 184123, new[] { ItemEffect.ExProp("ITEM_CROSSBOW_ATTACK_DMG_UP", 0.35f), ItemEffect.ExProp("ITEM_CROSSBOW_ATTACK_TARGET_UP", 2) } },  // BOW04_123

		// Maces
		{ 202109, new[] { ItemEffect.JobSkillUp(JobId.Monk, 2) } },  // MAC02_109 (Suncus Maul)
		{ 203203, new[] { ItemEffect.JobSkillUp(JobId.Priest, 2) } },  // MAC03_203
		{ 204124, new[] { ItemEffect.JobSkillUp(JobId.Monk, 2) } },  // MAC04_124
		{ 204125, new[] { ItemEffect.ExProp("ITEM_PLAGUEVAPOURS_COOLDOWN", 1000) } },  // MAC04_125

		// Two-Handed Maces
		{ 210411, new[] { ItemEffect.SkillUp(SkillId.Paladin_Demolition, 2), ItemEffect.SkillUp(SkillId.Inquisitor_GodSmash, 2) } },  // TMAC04_111
		{ 210416, new[] { ItemEffect.ExProp("ITEM_CHAPLAIN_BULIDCAPPELLA_TIME_UP", 15000) } },  // TMAC04_116

		// Shields
		{ 224104, new[] { ItemEffect.JobSkillUp(JobId.Peltasta, 1), ItemEffect.JobSkillUp(JobId.Rodelero, 1) } },  // SHD04_104
		{ 224118, new[] { ItemEffect.ExProp("ITEM_GUARDIAN_DURATIONTIMEUP", 1) } },  // SHD04_118

		// Spears
		{ 242106, new[] { ItemEffect.JobSkillUp(JobId.Hoplite, 2) } },  // SPR02_106
		{ 244123, new[] { ItemEffect.ExProp("ITEM_THROWINGFISHINGNET_TIMEUP", 1) } },  // SPR04_123
		{ 244124, new[] { ItemEffect.ExProp("ITEM_HOPLITE_FINESTRA_CRTHR_UP", 0.01f) } },  // SPR04_124

		// Two-Handed Spears
		{ 252105, new[] { ItemEffect.JobSkillUp(JobId.Cataphract, 1) } },  // TSP02_105
		{ 254125, new[] { ItemEffect.ExProp("ITEM_RANCER_CHARGE_STUN", 2000) } },  // TSP04_125

		// Two-Handed Staves
		{ 272103, new[] { ItemEffect.JobSkillUp(JobId.Pyromancer, 1) } },  // TSF02_103
		{ 272104, new[] { ItemEffect.SkillUp(SkillId.Cryomancer_IceBlast, 3) } },  // TSF02_104
		{ 273102, new[] { ItemEffect.JobSkillUp(JobId.Cryomancer, 2) } },  // TSF03_102
		{ 274122, new[] { ItemEffect.ExProp("ITEM_INVOCATION_ADDSUMMON", 1) } },  // TSF04_122
		{ 274123, new[] { ItemEffect.ExProp("ITEM_HEXING_INFECTION", 5) } },  // TSF04_123
		{ 274127, new[] { ItemEffect.ExProp("ITEM_HEAVYGRAVITY_TIME_UP", 5000) } },  // TSF04_127

		// Pistols
		{ 302103, new[] { ItemEffect.JobSkillUp(JobId.SchwarzerReiter, 1) } },  // PST02_103
		{ 304102, new[] { ItemEffect.JobSkillUp(JobId.SchwarzerReiter, 1) } },  // PST04_102
		{ 304103, new[] { ItemEffect.JobSkillUp(JobId.SchwarzerReiter, 1) } },  // PST04_103
		{ 304110, new[] { ItemEffect.SkillUp(SkillId.Corsair_PistolShot, 1), ItemEffect.SkillUp(SkillId.Schwarzereiter_Limacon, 1), ItemEffect.SkillUp(SkillId.Bulletmarker_RestInPeace, 1) } },  // PST04_110
		{ 304112, new[] { ItemEffect.SkillUp(SkillId.Bulletmarker_RestInPeace, 2), ItemEffect.SkillUp(SkillId.Schwarzereiter_Caracole, 2) } },  // PST04_112
		{ 304118, new[] { ItemEffect.ExProp("ITEM_OUTLAW_AGGRESS_DEBUFF_ADDDMG", 30) } },  // PST04_118
		{ 304119, new[] { ItemEffect.JobSkillUp(JobId.Corsair, 2) } },  // PST04_119

		// Rapiers
		{ 313101, new[] { ItemEffect.JobSkillUp(JobId.Fencer, 1) } },  // RAP03_101
		{ 314101, new[] { ItemEffect.JobSkillUp(JobId.Fencer, 2) } },  // RAP04_101
		{ 314104, new[] { ItemEffect.JobSkillUp(JobId.Fencer, 2) } },  // RAP04_104
		{ 314105, new[] { ItemEffect.JobSkillUp(JobId.Fencer, 2) } },  // RAP04_105
		{ 314115, new[] { ItemEffect.SkillUp(SkillId.Fencer_EpeeGarde, 2) } },  // RAP04_115
		{ 314119, new[] { ItemEffect.ExProp("ITEM_ODERSSKILL_ALLWOOVERLAP", 1) } },  // RAP04_119
		{ 314120, new[] { ItemEffect.ExProp("ITEM_EPEEGARDE_DMG_UP", 10) } },  // RAP04_120
		{ 314121, new[] { ItemEffect.ExProp("ITEM_OLE_TIME_UP", 10000) } },  // RAP04_121
		{ 314122, new[] { ItemEffect.ExProp("ITEM_FENCER_ATTAQUECOMPOSEE_HIT_UP", 2) } },  // RAP04_122

		// Muskets
		{ 333103, new[] { ItemEffect.SkillUp(SkillId.Musketeer_CoveringFire, 1) } },  // MUS03_103
		{ 334101, new[] { ItemEffect.JobSkillUp(JobId.Musketeer, 2) } },  // MUS04_101
		{ 334102, new[] { ItemEffect.JobSkillUp(JobId.Musketeer, 2) } },  // MUS04_102
		{ 334114, new[] { ItemEffect.ExProp("ITEM_EYEOFBEAST_TIMEUP", 3000) } },  // MUS04_114

		// Armor — Hate rate sets
		{ 504151, new[] { ItemEffect.ExProp("HateRate_BM", -50) } },  // HAND04_151
		{ 504152, new[] { ItemEffect.ExProp("HateRate_BM", 50) } },  // HAND04_152
		{ 514149, new[] { ItemEffect.ExProp("HateRate_BM", -50) } },  // FOOT04_149
		{ 514150, new[] { ItemEffect.ExProp("HateRate_BM", 50) } },  // FOOT04_150
		{ 524149, new[] { ItemEffect.ExProp("HateRate_BM", -50) } },  // LEG04_149
		{ 524150, new[] { ItemEffect.ExProp("HateRate_BM", 50) } },  // LEG04_150
		{ 534149, new[] { ItemEffect.ExProp("HateRate_BM", -50) } },  // TOP04_149
		{ 534150, new[] { ItemEffect.ExProp("HateRate_BM", 50) } },  // TOP04_150

		// Necklaces
		{ 582123, new[] { ItemEffect.Buff(BuffId.NECK02_123_buff) } },  // NECK02_123
		{ 582124, new[] { ItemEffect.Buff(BuffId.NECK02_124_buff) } },  // NECK02_124

		// Cannons
		{ 324101, new[] { ItemEffect.JobSkillUp(JobId.Cannoneer, 1) } },  // CAN04_101
		{ 324102, new[] { ItemEffect.JobSkillUp(JobId.Cannoneer, 1) } },  // CAN04_102
		{ 324108, new[] { ItemEffect.SkillUp(SkillId.Cannoneer_SweepingCannon, 1), ItemEffect.SkillUp(SkillId.Cannoneer_CannonBarrage, 1) } },  // CAN04_108
		{ 324111, new[] { ItemEffect.SkillUp(SkillId.Cannoneer_SweepingCannon, 2), ItemEffect.SkillUp(SkillId.Cannoneer_SiegeBurst, 2) } },  // CAN04_111
		{ 324114, new[] { ItemEffect.ExProp("ITEM_SIEGEBURST_FLASHBOMB", 1) } },  // CAN04_114
		{ 324115, new[] { ItemEffect.ExProp("ITEM_CANISTERSHOT_HITCOUNT", 5) } },  // CAN04_115

		// Glacier equipment — commented out: IDs are wrong (point to New Year/Grimoire items),
		// and EP12_xxx classnames don't exist in XML. Need correct IDs.
		// { 636801, new[] { ItemEffect.ExProp("ITEM_GLACIER_CROSSBOW", 1) } },  // EP12_BOW04_001
		// { 636802, new[] { ItemEffect.ExProp("ITEM_GLACIER_DAGGER", 1) } },  // EP12_DAG04_001
		// { 636803, new[] { ItemEffect.ExProp("ITEM_GLACIER_MACE", 1) } },  // EP12_MAC04_001
		// { 636804, new[] { ItemEffect.ExProp("ITEM_GLACIER_ROD", 1) } },  // EP12_STF04_001
		// { 636805, new[] { ItemEffect.ExProp("ITEM_GLACIER_SPEAR", 1) } },  // EP12_SPR04_001
		// { 636806, new[] { ItemEffect.ExProp("ITEM_GLACIER_STAFF", 1) } },  // EP12_TSF04_001
		// { 636807, new[] { ItemEffect.ExProp("ITEM_GLACIER_SWORD", 1) } },  // EP12_SWD04_001
		// { 636808, new[] { ItemEffect.ExProp("ITEM_GLACIER_THMACE", 1) } },  // EP12_TMAC04_001
		// { 636809, new[] { ItemEffect.ExProp("ITEM_GLACIER_THSWORD", 1) } },  // EP12_TSW04_001
		// { 636810, new[] { ItemEffect.ExProp("ITEM_GLACIER_TRINKET", 1) } },  // EP12_TRK04_001
		// { 636901, new[] { ItemEffect.ExProp("ITEM_GLACIER_PLATE", 1) } },  // EP12_TOP04_006
		// { 636902, new[] { ItemEffect.ExProp("ITEM_GLACIER_PLATE", 1) } },  // EP12_LEG04_006
		// { 636903, new[] { ItemEffect.ExProp("ITEM_GLACIER_PLATE", 1) } },  // EP12_FOOT04_006
		// { 636904, new[] { ItemEffect.ExProp("ITEM_GLACIER_PLATE", 1) } },  // EP12_HAND04_006

		// PVP Weapons
		{ 636647, new[] { ItemEffect.SkillUp(SkillId.BlossomBlader_BlossomSlash, 2), ItemEffect.ExProp("ITEM_VIBORA_THSWORD", 1) } },  // PVP_TSW04_126
		{ 636648, new[] { ItemEffect.SkillUp(SkillId.Chronomancer_QuickCast, 2), ItemEffect.ExProp("ITEM_VIBORA_ROD", 1) } },  // PVP_STF04_127
		{ 636649, new[] { ItemEffect.SkillUp(SkillId.Fletcher_BarbedArrow, 3), ItemEffect.SkillUp(SkillId.Fletcher_BodkinPoint, 3) } },  // PVP_TBW04_126
		{ 636650, new[] { ItemEffect.SkillUp(SkillId.Arbalester_GuidedShot, 3), ItemEffect.ExProp("ITEM_VIBORA_CROSSBOW", 1) } },  // PVP_BOW04_126
		{ 636651, new[] { ItemEffect.SkillUp(SkillId.Priest_MassHeal, 1), ItemEffect.ExProp("ITEM_VIBORA_MACE", 1000) } },  // PVP_MAC04_129
		{ 636652, new[] { ItemEffect.JobSkillUp(JobId.Crusader, 1) } },  // PVP_TMAC04_118
		{ 636654, new[] { ItemEffect.JobSkillUp(JobId.Retiarius, 1) } },  // PVP_SPR04_127
		{ 636655, new[] { ItemEffect.JobSkillUp(JobId.Dragoon, 1) } },  // PVP_TSP04_128
		{ 636656, new[] { ItemEffect.SkillUp(SkillId.Rangda_Keletihan, 2), ItemEffect.SkillUp(SkillId.Rangda_Luka, 3) } },  // PVP_DAG04_123
		{ 636657, new[] { ItemEffect.SkillUp(SkillId.TerraMancer_StoneShower, 2), ItemEffect.ExProp("ITEM_USE_STONESHOWER", 1) } },  // PVP_TSF04_129
		{ 636658, new[] { ItemEffect.SkillUp(SkillId.Bulletmarker_RestInPeace, 3) } },  // PVP_PST04_122
		{ 636659, new[] { ItemEffect.JobSkillUp(JobId.Fencer, 1) } },  // PVP_RAP04_124
		{ 636660, new[] { ItemEffect.SkillUp(SkillId.Cannoneer_CannonShot, 3), ItemEffect.SkillUp(SkillId.Matross_Explosion, 3), ItemEffect.ExProp("ITEM_CANNONHOLD_BUFF", 1) } },  // PVP_CAN04_118
		{ 636661, new[] { ItemEffect.SkillUp(SkillId.Musketeer_Snipe, 1), ItemEffect.SkillUp(SkillId.Musketeer_HeadShot, 3), ItemEffect.ExProp("ITEM_VIBORA_MUSKET", 1) } },  // PVP_MUS04_118
		{ 636663, new[] { ItemEffect.JobSkillUp(JobId.Rodelero, 1), ItemEffect.SkillUp(SkillId.Rodelero_Montano, 2), ItemEffect.ExProp("ITEM_VIBORA_SWORD_MONTANO", 1) } },  // PVP_SWD04_126_1
		{ 636664, new[] { ItemEffect.SkillUp(SkillId.Rancer_Quintain, 3), ItemEffect.ExProp("ITEM_VIBORA_THSPEAR_RUSH", 1) } },  // PVP_TSP04_128_1
		{ 636665, new[] { ItemEffect.SkillUp(SkillId.Shadowmancer_ShadowThorn, 2), ItemEffect.ExProp("ITEM_VIBORA_THSTAFF_SHADOWTHORN", 1) } },  // PVP_TSF04_129_1
		{ 636666, new[] { ItemEffect.JobSkillUp(JobId.Shinobi, 1) } },  // PVP_DAG04_123_1
		{ 636667, new[] { ItemEffect.SkillUp(SkillId.Pyromancer_FirePillar, 2), ItemEffect.ExProp("ITEM_VIBORA_STAFF_FIREPILLAR", 1) } },  // PVP_TSF04_129_2
		{ 636668, new[] { ItemEffect.JobSkillUp(JobId.Sorcerer, 1) } },  // PVP_STF04_127_1
		{ 636669, new[] { ItemEffect.JobSkillUp(JobId.Sorcerer, -1), ItemEffect.ExProp("ITEM_VIBORA_MACE_BINATIO", 1) } },  // PVP_MAC04_129_1
		{ 636670, new[] { ItemEffect.SkillUp(SkillId.Inquisitor_GodSmash, 3), ItemEffect.ExProp("ITEM_VIBORA_THMACE_GODSMASH", 1) } },  // PVP_TMAC04_118_1
		{ 636672, new[] { ItemEffect.SkillUp(SkillId.Arditi_Granata, 3), ItemEffect.ExProp("ITEM_VIBORA_DAGGER_TAGLIO", 1) } },  // PVP_DAG04_123_2
		{ 636673, new[] { ItemEffect.JobSkillUp(JobId.Rogue, 1) } },  // PVP_DAG04_123_3
		{ 636674, new[] { ItemEffect.SkillUp(SkillId.Mergen_ArrowRain, 2), ItemEffect.ExProp("ITEM_VIBORA_ARROW_RAIN", 1) } },  // PVP_TBW04_126_1
		{ 636675, new[] { ItemEffect.JobSkillUp(JobId.Hackapell, 1), ItemEffect.ExProp("ITEM_VIBORA_SWORD_Hackapell", 5000) } },  // PVP_SWD04_126_2
		{ 636676, new[] { ItemEffect.SkillUp(SkillId.Cryomancer_IciclePike, 3), ItemEffect.ExProp("ITEM_VIBORA_ROD_CRYO", 1) } },  // PVP_STF04_127_2
		{ 636677, new[] { ItemEffect.SkillUp(SkillId.Highlander_CartarStroke, 3), ItemEffect.SkillUp(SkillId.Highlander_SkyLiner, 2), ItemEffect.ExProp("ITEM_VIBORA_THSWOIRD_DUAL", 1) } },  // PVP_TSW04_126_1
		{ 636678, new[] { ItemEffect.JobSkillUp(JobId.Hoplite, 1), ItemEffect.SkillUp(SkillId.Hoplite_ThrouwingSpear, 2) } },  // PVP_SPR04_127_1
		{ 636679, new[] { ItemEffect.JobSkillUp(JobId.TigerHunter, 1), ItemEffect.ExProp("ITEM_VIBORA_MUSKET_TIGERHUNTER", 1) } },  // PVP_MUS04_118_1
		{ 636680, new[] { ItemEffect.SkillUp(SkillId.Exorcist_Katadikazo, 2), ItemEffect.ExProp("ITEM_VIBORA_THMACE_KATADIKAZO", 1) } },  // PVP_TMAC04_118_2

		// Trinkets
		{ 694005, new[] { ItemEffect.PropMod(PropertyName.DashRun, 2) } },  // TRK04_105
		{ 694007, new[] { ItemEffect.PropMod(PropertyName.CastingSpeed_BM, 20) } },  // TRK04_107
		{ 694008, new[] { ItemEffect.SkillUp(SkillId.Rancer_Quintain, 1), ItemEffect.SkillUp(SkillId.Dragoon_Dethrone, 1) } },  // TRK04_108
	};

	/// <summary>
	/// Skill-to-job lookup cache, built on first use.
	/// </summary>
	private static Dictionary<SkillId, JobId> _skillToJob;

	private static Dictionary<SkillId, JobId> GetSkillToJobMap()
	{
		if (_skillToJob != null)
			return _skillToJob;

		_skillToJob = new Dictionary<SkillId, JobId>();
		foreach (var entry in ZoneServer.Instance.Data.SkillTreeDb.Entries)
		{
			if (!_skillToJob.ContainsKey(entry.SkillId))
				_skillToJob[entry.SkillId] = entry.JobId;
		}
		return _skillToJob;
	}

	/// <summary>
	/// Returns true if the given item ID has any registered effects.
	/// </summary>
	public static bool HasEffects(int itemId)
		=> Registry.ContainsKey(itemId);

	/// <summary>
	/// Returns true if the given item ID has any skill level effects.
	/// </summary>
	public static bool HasSkillEffects(int itemId)
	{
		if (!Registry.TryGetValue(itemId, out var effects))
			return false;
		return effects.Any(e => e.Type == ItemEffectType.JobSkillUp || e.Type == ItemEffectType.SkillUp);
	}

	/// <summary>
	/// Returns the total skill level bonus from equipped items for the
	/// given skill. Called by SCR_Get_SkillLv.
	/// </summary>
	public static int GetSkillBonus(Character character, SkillId skillId)
	{
		var bonus = 0;
		var skillToJob = GetSkillToJobMap();
		skillToJob.TryGetValue(skillId, out var jobId);

		foreach (var kvp in Registry)
		{
			if (!character.Inventory.IsEquipped(kvp.Key))
				continue;

			foreach (var effect in kvp.Value)
			{
				if (effect.Type == ItemEffectType.JobSkillUp && effect.Job == jobId)
					bonus += (int)effect.Value;
				else if (effect.Type == ItemEffectType.SkillUp && effect.Skill == skillId)
					bonus += (int)effect.Value;
			}
		}

		return bonus;
	}

	/// <summary>
	/// Applies all equip-time effects (ExProp, PropMod, Buff) for the item.
	/// Called by item_equip.cs on equip.
	/// </summary>
	public static void ApplyEffects(Character character, int itemId)
	{
		if (!Registry.TryGetValue(itemId, out var effects))
			return;

		foreach (var effect in effects)
		{
			switch (effect.Type)
			{
				case ItemEffectType.ExProp:
					var current = character.Properties.GetFloat(effect.PropName);
					character.Properties.SetFloat(effect.PropName, current + effect.Value);
					break;
				case ItemEffectType.PropMod:
					character.Properties.Modify(effect.PropName, effect.Value);
					break;
				case ItemEffectType.Buff:
					character.StartBuff(effect.BuffId, TimeSpan.Zero);
					break;
			}
		}
	}

	/// <summary>
	/// Removes all equip-time effects (ExProp, PropMod, Buff) for the item.
	/// Called by item_equip.cs on unequip.
	/// </summary>
	public static void RemoveEffects(Character character, int itemId)
	{
		if (!Registry.TryGetValue(itemId, out var effects))
			return;

		foreach (var effect in effects)
		{
			switch (effect.Type)
			{
				case ItemEffectType.ExProp:
					var current = character.Properties.GetFloat(effect.PropName);
					character.Properties.SetFloat(effect.PropName, current - effect.Value);
					break;
				case ItemEffectType.PropMod:
					character.Properties.Modify(effect.PropName, -effect.Value);
					break;
				case ItemEffectType.Buff:
					character.StopBuff(effect.BuffId);
					break;
			}
		}
	}
}
