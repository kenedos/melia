using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Morph.
	/// Transforms the current summon into a different creature based on the sub-card.
	/// </summary>
	/// <remarks>
	/// Morph:
	/// - Kills the current main summon
	/// - Creates a new summon based on the sub-card
	/// - The new summon retains properties similar to the original
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Morph)]
	public class Sorcerer_MorphOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Morphed summon lifetime in seconds.
		/// </summary>
		private const int MorphLifetimeSeconds = 900;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			// Check for equipped sub-card (slot 2) for morph target
			var etc = character.Etc.Properties;
			var subCardName = etc.GetString(PropertyName.Sorcerer_bosscardName2, "None");
			var subCardGuid = etc.GetString(PropertyName.Sorcerer_bosscardGUID2, "None");

			if (subCardName == "None" || subCardGuid == "None")
			{
				caster.ServerMessage(Localization.Get("No sub-card equipped for morph target."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Verify the sub-card is still in inventory
			var cardId = long.Parse(subCardGuid);
			var card = character.Inventory.GetItem(cardId);
			if (card == null)
			{
				etc.SetString(PropertyName.Sorcerer_bosscardName2, "None");
				etc.SetString(PropertyName.Sorcerer_bosscardGUID2, "None");
				etc.SetFloat(PropertyName.Sorcerer_bosscard2, 0);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Sorcerer_bosscardName2, PropertyName.Sorcerer_bosscard2);
				caster.ServerMessage(Localization.Get("Sub-card no longer available."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Check if main summon exists
			var mainSummons = character.Summons.GetSummons(s =>
				s.Vars.TryGetInt("SORCERER_SUMMONING", out var val) && val == 1);

			if (mainSummons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("No summon to morph."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(character, skill, originPos, farPos, subCardName, card, mainSummons.First()));
		}

		private async Task HandleSkill(Character character, Skill skill, Position originPos, Position farPos, string newMonsterClassName, Item card, Summon oldSummon)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(900));

			// Play transformation effect on old summon
			var transformPos = oldSummon.Position;
			Send.ZC_NORMAL.PlayEffect(oldSummon, "F_warrior_ninja_shot_explosion_light", 1f);

			await skill.Wait(TimeSpan.FromMilliseconds(400));

			// Kill the old summon
			Send.ZC_IS_SUMMONING_MONSTER(character, oldSummon, false);
			oldSummon.Kill(character);

			// Get the monster data for the new form
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(newMonsterClassName, out var monsterData))
			{
				character.ServerMessage(Localization.Get("Invalid monster data for morph."));
				return;
			}

			// Calculate spawn position
			var spawnPos = originPos.GetRelative(farPos, distance: 33.08f);

			// Create the new morphed summon
			var newSummon = new Summon(character, monsterData.Id, RelationType.Friendly);

			// Generate display name based on sub-card and owner
			var monsterDisplayName = monsterData.Name;
			newSummon.Name = $"{monsterDisplayName} of {character.Name}";

			newSummon.Position = spawnPos;
			newSummon.Direction = character.Direction;
			newSummon.Map = character.Map;
			newSummon.OwnerHandle = character.Handle;
			newSummon.Faction = FactionType.Law;
			newSummon.Level = character.Level;

			// Mark as sorcerer summon (morphed)
			newSummon.Vars.SetInt("SORCERER_SUMMONING", 1);
			newSummon.Vars.SetInt("SORCERER_MON", 1);

			// Check if it's a legend card
			var isLegendCard = card.Data.EquipExpGroup == EquipExpGroup.Legend_Card;
			if (isLegendCard)
				newSummon.Vars.SetInt("LEGEND_CARD", 1);

			var summoningSkill = character.GetSkill(SkillId.Sorcerer_Summoning);

			// Calculate stats
			CalculateSummonStats(newSummon, character, summoningSkill?.Level ?? 1, skill.Level, card);
			newSummon.InvalidateProperties();

			// Calculate scale based on Summoning skill level			
			var morphLevelForScale = Math.Clamp(skill.Level, 1, 10);
			var scale = 1f + morphLevelForScale * 0.30f;

			if (isLegendCard)
				scale *= 1.15f;

			newSummon.Properties.SetFloat(PropertyName.Scale, scale);

			// Set movement speed
			newSummon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			newSummon.Properties.SetFloat(PropertyName.RunMSPD, 160f);

			// Add lifetime component
			newSummon.Components.Add(new LifeTimeComponent(newSummon, TimeSpan.FromSeconds(MorphLifetimeSeconds)));

			// Activate the summon
			newSummon.SetState(true, canMove: true, hasAi: false);
			newSummon.Components.Add(new AiComponent(newSummon, "PC_Summon_Necromancer", character));

			var summonMaxHp = newSummon.Properties.GetFloat(PropertyName.MHP);
			newSummon.Heal(summonMaxHp, 0);

			// Add to character's summon list
			character.Summons.AddSummon(newSummon);

			// Apply PC_Summon buff
			newSummon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, newSummon);

			if (character.IsAbilityActive(AbilityId.Sorcerer18))
			{
				var abilityLevel = character.GetAbilityLevel(AbilityId.Sorcerer18);
				newSummon.StartBuff(BuffId.Summoning_Overwork_Buff, abilityLevel, 0, TimeSpan.Zero, character);
			}

			Send.ZC_IS_SUMMON_SORCERER_MONSTER(character, newSummon);
			Send.ZC_IS_SUMMONING_MONSTER(character, newSummon, true);

			Send.ZC_EXEC_CLIENT_SCP(
				character.Connection,
				"SetExProp(GetMyPCObject(), 'SUMMON_MAINCARD', 1)"
			);

			newSummon.InvalidateProperties();

			var summonProperties = new[]
			{
				PropertyName.MHP,
				PropertyName.MINPATK,
				PropertyName.MAXPATK,
				PropertyName.MINMATK,
				PropertyName.MAXMATK,
				PropertyName.DEF,
				PropertyName.MDEF,
				PropertyName.CRTDR,
				PropertyName.HR,
				PropertyName.STR,
				PropertyName.CON,
				PropertyName.INT,
				PropertyName.DEX,
				PropertyName.MNA,
				PropertyName.Scale
			};

			Send.ZC_OBJECT_PROPERTY(
				character.Connection,
				newSummon.Handle,
				newSummon.Properties.GetSelect(summonProperties)
			);

			var grimoireDef = (int)Math.Floor(newSummon.Properties.GetFloat(PropertyName.DEF));
			var grimoireMdef = (int)Math.Floor(newSummon.Properties.GetFloat(PropertyName.MDEF));

			Send.ZC_EXEC_CLIENT_SCP(
				character.Connection,
				$"SetExProp(GetMyPCObject(), 'MELIA_SUMMON_DEF', {grimoireDef}); SetExProp(GetMyPCObject(), 'MELIA_SUMMON_MDEF', {grimoireMdef})"
			);

			character.AddonMessage(AddonMessage.UPDATE_GRIMOIRE_UI);

			// Send property updates
			//Send.ZC_OBJECT_PROPERTY(newSummon, PropertyName.Scale);

			// Attach effects based on card type
			//AttachSummonEffects(newSummon, isLegendCard);

			// Set color for morphed summon (slightly different tint)
			newSummon.AddEffect(ColorEffect.FromRgba(1.0f, 0.9f, 0.8f, 1.0f));
		}

		/// <summary>
		/// Calculates and sets the summon's stats based on skill level and card level.
		/// </summary>
		private void CalculateSummonStats(Summon summon, Character caster, int summoningLevel, int morphLevel, Item card)
		{
			var intelligence = Math.Max(0f, caster.Properties.GetFloat(PropertyName.INT));
			var constitution = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CON));
			var spirit = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MNA));
			var normalizedSummoningLevel = Math.Max(1, summoningLevel);
			var normalizedMorphLevel = Math.Clamp(morphLevel, 1, 10);
			var cardLevel = Math.Clamp(card.CardLevel, 1, 10);
			var cardScale = 1f + cardLevel * 0.50f;
			var morphFactor = 1f + normalizedMorphLevel * 0.10f;
			var inheritanceRate = 0.05f + normalizedMorphLevel * 0.015f;

			var hpFactor = 1f + intelligence / 50f + constitution / 10f;
			var offensiveFactor = 1f + intelligence / 250f + normalizedSummoningLevel * 0.05f;
			var defensiveFactor = 1f + spirit / 50f + normalizedSummoningLevel * 0.25f;
			var inheritedMHP = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MHP)) * inheritanceRate;
			var inheritedDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEF)) * inheritanceRate;
			var inheritedMDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MDEF)) * inheritanceRate;
			var inheritedCRTDR = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CRTDR)) * inheritanceRate;
			var casterMinimumMagicAttack = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MINMATK));
			var casterMaximumMagicAttack = Math.Max(casterMinimumMagicAttack, caster.Properties.GetFloat(PropertyName.MAXMATK));
			var inheritedMagicAttack = ((casterMinimumMagicAttack + casterMaximumMagicAttack) / 2f) * inheritanceRate;
			var inheritedAccuracy = Math.Max(0f, caster.Properties.GetFloat(PropertyName.HR)) * inheritanceRate;

			var currentMHPBonus = summon.Properties.GetFloat(PropertyName.MHP_BM);
			var currentPATKBonus = summon.Properties.GetFloat(PropertyName.PATK_BM);
			var currentMATKBonus = summon.Properties.GetFloat(PropertyName.MATK_BM);
			var currentDEFBonus = summon.Properties.GetFloat(PropertyName.DEF_BM);
			var currentMDEFBonus = summon.Properties.GetFloat(PropertyName.MDEF_BM);
			var currentCRTDRBonus = summon.Properties.GetFloat(PropertyName.CRTDR_BM);
			var currentAccuracyBonus = summon.Properties.GetFloat(PropertyName.HR_BM);

			var baseMHP = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MHP) - currentMHPBonus);
			var basePATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINPATK) - currentPATKBonus);
			var baseMATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINMATK) - currentMATKBonus);
			var baseDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.DEF) - currentDEFBonus);
			var baseMDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MDEF) - currentMDEFBonus);

			var scaledBaseMHP = baseMHP * cardScale;
			var scaledBasePATK = basePATK * cardScale;
			var scaledBaseMATK = baseMATK * cardScale;
			var scaledBaseDEF = baseDEF * cardScale;
			var scaledBaseMDEF = baseMDEF * cardScale;

			var finalMHP = scaledBaseMHP * hpFactor * morphFactor;
			var finalPATK = scaledBasePATK * offensiveFactor * morphFactor;
			var finalMATK = scaledBaseMATK * offensiveFactor * morphFactor;
			var finalDEF = scaledBaseDEF * defensiveFactor * morphFactor;
			var finalMDEF = scaledBaseMDEF * defensiveFactor * morphFactor;

			summon.Properties.SetFloat(PropertyName.MHP_BM, currentMHPBonus + finalMHP - baseMHP + inheritedMHP);
			summon.Properties.SetFloat(PropertyName.PATK_BM, currentPATKBonus + finalPATK - basePATK + inheritedMagicAttack);
			summon.Properties.SetFloat(PropertyName.MATK_BM, currentMATKBonus + finalMATK - baseMATK + inheritedMagicAttack);
			summon.Properties.SetFloat(PropertyName.DEF_BM, currentDEFBonus + finalDEF - baseDEF + inheritedDEF);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, currentMDEFBonus + finalMDEF - baseMDEF + inheritedMDEF);
			summon.Properties.SetFloat(PropertyName.CRTDR_BM, currentCRTDRBonus + inheritedCRTDR);
			summon.Properties.SetFloat(PropertyName.HR_BM, currentAccuracyBonus + inheritedAccuracy);
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
		}

		/// <summary>
		/// Attaches visual effects to the summon based on card type.
		/// </summary>
		private void AttachSummonEffects(Summon summon, bool isLegendCard)
		{
			if (isLegendCard)
				Send.ZC_NORMAL.AttachEffect(summon, "F_pc_summon_legend", 1f);
			else
				Send.ZC_NORMAL.AttachEffect(summon, "F_pc_summon_normal", 1f);
		}
	}
}
