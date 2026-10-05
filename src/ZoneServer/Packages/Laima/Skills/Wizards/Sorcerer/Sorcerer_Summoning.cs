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
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Summoning.
	/// Summons a creature based on the equipped boss card.
	/// </summary>
	/// <remarks>
	/// The summoned creature's stats are based on:
	/// - Caster's level
	/// - Skill level
	/// - Card level
	/// - Whether the card is a Legend card
	///
	/// The summon has a 15 minute (900 second) lifetime.
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Summoning)]
	public class Sorcerer_SummoningOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Summon lifetime in seconds.
		/// </summary>
		private const int SummonLifetimeSeconds = 900;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			// Check for equipped boss card
			var etc = character.Etc.Properties;
			var cardName = etc.GetString(PropertyName.Sorcerer_bosscardName1, "None");
			var cardGuid = etc.GetString(PropertyName.Sorcerer_bosscardGUID1, "None");

			if (cardName == "None" || cardGuid == "None")
			{
				caster.ServerMessage(Localization.Get("No boss card equipped. Place a card in the grimoire slot."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Verify the card is still in inventory
			var cardId = long.Parse(cardGuid);
			var card = character.Inventory.GetItem(cardId);
			if (card == null)
			{
				// Card no longer exists, clear the reference
				etc.SetString(PropertyName.Sorcerer_bosscardName1, "None");
				etc.SetString(PropertyName.Sorcerer_bosscardGUID1, "None");
				etc.SetFloat(PropertyName.Sorcerer_bosscard1, 0);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Sorcerer_bosscardName1, PropertyName.Sorcerer_bosscard1);
				caster.ServerMessage(Localization.Get("Boss card no longer available."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			// Check if we're in a city (can't summon in towns)
			var mapType = character.Map.Data.Type;
			if (mapType == MapType.City)
			{
				caster.ServerMessage(Localization.Get("Cannot summon in this area."));
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

			skill.Run(this.HandleSkill(character, skill, originPos, farPos, cardName, card));
		}

		private async Task HandleSkill(Character character, Skill skill, Position originPos, Position farPos, string monsterClassName, Item card)
		{
			// Kill any existing sorcerer summons first
			KillExistingSummons(character, "SORCERER_SUMMONING");

			await skill.Wait(TimeSpan.FromMilliseconds(900));

			// Wait a bit more before spawning
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			// Calculate spawn position
			var spawnPos = originPos.GetRelative(farPos, distance: 35f);

			// Get the monster ID from the class name
			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterClassName, out var monsterData))
			{
				character.ServerMessage(Localization.Get("Invalid monster data."));
				return;
			}

			// Create the summon
			var summon = new Summon(character, monsterData.Id, RelationType.Friendly);

			// Set up summon properties
			summon.Position = spawnPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;

			// Mark as sorcerer summon
			summon.Vars.SetInt("SORCERER_SUMMONING", 1);
			summon.Vars.SetInt("SORCERER_MON", 1);

			// Check if it's a legend card
			var isLegendCard = card.Data.EquipExpGroup == EquipExpGroup.Legend_Card;
			if (isLegendCard)
				summon.Vars.SetInt("LEGEND_CARD", 1);


			// Set level to caster's level
			summon.Level = character.Level;

			// Calculate stats based on caster's INT, SPR and skill level
			CalculateSummonStats(summon, character, skill, card);
			summon.InvalidateProperties();

			//Calculate scale based on Summoning skill level
			var summoningLevelForScale = Math.Clamp(skill.Level, 1, 10);
			var visualScale = 1f + summoningLevelForScale * 0.30f;

			if (isLegendCard)
				visualScale *= 1.15f;

			summon.Properties.SetFloat(PropertyName.Scale, visualScale);

			// Add lifetime component
			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(SummonLifetimeSeconds)));

			// Activate the summon
			summon.SetState(true, canMove: true, hasAi: false);
			summon.Components.Add(new AiComponent(summon, "PC_Summon_Necromancer", character));

			// Fill the summon's current HP after applying the MHP multiplier
			var summonMaxHp = summon.Properties.GetFloat(PropertyName.MHP);
			summon.Heal(summonMaxHp, 0);

			// Add to character's summon list
			character.Summons.AddSummon(summon);

			// Apply PC_Summon buff
			summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);

			// Enables Morph.
			Send.ZC_IS_SUMMON_SORCERER_MONSTER(character, summon);
			Send.ZC_IS_SUMMONING_MONSTER(character, summon, true);

			Send.ZC_EXEC_CLIENT_SCP(
				character.Connection,
				"SetExProp(GetMyPCObject(), 'SUMMON_MAINCARD', 1)"
			);

			if (character.IsAbilityActive(AbilityId.Sorcerer18))
			{
				var abilityLevel = character.GetAbilityLevel(AbilityId.Sorcerer18);

				summon.StartBuff(
					BuffId.Summoning_Overwork_Buff,
					abilityLevel,
					0,
					TimeSpan.Zero,
					character
				);
			}

			summon.InvalidateProperties();

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
				PropertyName.STR,
				PropertyName.CON,
				PropertyName.INT,
				PropertyName.DEX,
				PropertyName.MNA
			};

			Send.ZC_OBJECT_PROPERTY(
				character.Connection,
				summon.Handle,
				summon.Properties.GetSelect(summonProperties)
			);

			var grimoireDef = (int)Math.Floor(summon.Properties.GetFloat(PropertyName.DEF));
			var grimoireMdef = (int)Math.Floor(summon.Properties.GetFloat(PropertyName.MDEF));

			Send.ZC_EXEC_CLIENT_SCP(
				character.Connection,
				$"SetExProp(GetMyPCObject(), 'MELIA_SUMMON_DEF', {grimoireDef}); SetExProp(GetMyPCObject(), 'MELIA_SUMMON_MDEF', {grimoireMdef})"
			);

			character.AddonMessage(AddonMessage.UPDATE_GRIMOIRE_UI);

			Send.ZC_ADDON_MSG(
				character,
				"QUICKSLOT_MONSTER_RESET_COOLDOWN",
				argStr: monsterClassName
			);
		}

		/// <summary>
		/// Calculates and sets the summon's stats based on skill level and card level.
		/// </summary>
		private void CalculateSummonStats(Summon summon, Character caster, Skill skill, Item card)
		{
			var intelligence = Math.Max(0f, caster.Properties.GetFloat(PropertyName.INT));
			var constitution = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CON));
			var spirit = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MNA));
			var skillLevel = Math.Clamp(skill.Level, 1, 10);
			var cardLevel = Math.Clamp(card.CardLevel, 1, 10);
			var cardScale = 1f + cardLevel * 0.50f;
			var inheritanceRate = 0.05f + skillLevel * 0.015f;

			var hpFactor = 1f + intelligence / 50f + constitution / 10f;
			var offensiveFactor = 1f + intelligence / 150f + skill.Level * 0.05f;
			var defensiveFactor = 1f + spirit / 50f + skill.Level * 0.25f;
			var inheritedMHP = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MHP)) * inheritanceRate;
			var inheritedDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEF)) * inheritanceRate;
			var inheritedMDEF = Math.Max(0f, caster.Properties.GetFloat(PropertyName.MDEF)) * inheritanceRate;
			var inheritedCRTDR = Math.Max(0f, caster.Properties.GetFloat(PropertyName.CRTDR)) * inheritanceRate;

			var currentMHPBonus = summon.Properties.GetFloat(PropertyName.MHP_BM);
			var currentPATKBonus = summon.Properties.GetFloat(PropertyName.PATK_BM);
			var currentMATKBonus = summon.Properties.GetFloat(PropertyName.MATK_BM);
			var currentDEFBonus = summon.Properties.GetFloat(PropertyName.DEF_BM);
			var currentMDEFBonus = summon.Properties.GetFloat(PropertyName.MDEF_BM);
			var currentCRTDRBonus = summon.Properties.GetFloat(PropertyName.CRTDR_BM);

			var baseMHP = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MHP) - currentMHPBonus);
			var basePATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINPATK) - currentPATKBonus);
			var baseMATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINMATK) - currentMATKBonus);
			var baseDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.DEF) - currentDEFBonus);
			var baseMDEF = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MDEF) - currentMDEFBonus);

			var finalMHP = baseMHP * cardScale * hpFactor;
			var finalPATK = basePATK * cardScale * offensiveFactor;
			var finalMATK = baseMATK * cardScale * offensiveFactor;
			var finalDEF = baseDEF * cardScale * defensiveFactor;
			var finalMDEF = baseMDEF * cardScale * defensiveFactor;

			summon.Properties.SetFloat(PropertyName.MHP_BM, currentMHPBonus + finalMHP - baseMHP + inheritedMHP);
			summon.Properties.SetFloat(PropertyName.PATK_BM, currentPATKBonus + finalPATK - basePATK);
			summon.Properties.SetFloat(PropertyName.MATK_BM, currentMATKBonus + finalMATK - baseMATK);
			summon.Properties.SetFloat(PropertyName.DEF_BM, currentDEFBonus + finalDEF - baseDEF + inheritedDEF);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, currentMDEFBonus + finalMDEF - baseMDEF + inheritedMDEF);
			summon.Properties.SetFloat(PropertyName.CRTDR_BM, currentCRTDRBonus + inheritedCRTDR);
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);

			System.Console.WriteLine($"[SUMMON_DEF] baseDEF={baseDEF} finalDEF={finalDEF} DEF_BM={summon.Properties.GetFloat(PropertyName.DEF_BM)} DEF={summon.Properties.GetFloat(PropertyName.DEF)} baseMDEF={baseMDEF} finalMDEF={finalMDEF} MDEF_BM={summon.Properties.GetFloat(PropertyName.MDEF_BM)} MDEF={summon.Properties.GetFloat(PropertyName.MDEF)}");
		}

		/// <summary>
		/// Kills existing summons with the specified property.
		/// </summary>
		private void KillExistingSummons(Character character, string propertyName)
		{
			var existingSummons = character.Summons.GetSummons(s => s.Vars.TryGetInt(propertyName, out var value) && value == 1);

			foreach (var summon in existingSummons)
			{
				Send.ZC_IS_SUMMONING_MONSTER(character, summon, false);
				summon.Kill(character);
			}
		}
	}
}
