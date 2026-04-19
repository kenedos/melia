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

			// Calculate stats based on skill level and card level
			CalculateSummonStats(summon, character, skill, card);

			// Calculate MHP based on caster's MHP
			var casterMHP = character.Properties.GetFloat(PropertyName.MHP) - character.Properties.GetFloat(PropertyName.MHP_BM);
			var summonMHP = casterMHP * (8.5f + (skill.Level * 0.5f));
			summon.Vars.SetFloat("SUMMON_SET_MHP_BY_CASTER", summonMHP);

			// Add lifetime component
			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromSeconds(SummonLifetimeSeconds)));

			// Check for Overwork ability (Sorcerer18)
			if (character.IsAbilityActive(AbilityId.Sorcerer18))
			{
				var abilityLevel = character.GetAbilityLevel(AbilityId.Sorcerer18);
				summon.StartBuff(BuffId.Summoning_Overwork_Buff, abilityLevel, 0, TimeSpan.Zero, character);
			}

			// Activate the summon
			summon.SetState(true);

			// Add to character's summon list
			character.Summons.AddSummon(summon);

			// Apply PC_Summon buff
			summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);

			// Reset quickslot cooldown for this monster
			Send.ZC_ADDON_MSG(character, "QUICKSLOT_MONSTER_RESET_COOLDOWN", argStr: monsterClassName);
		}

		/// <summary>
		/// Calculates and sets the summon's stats based on skill level and card level.
		/// </summary>
		private void CalculateSummonStats(Summon summon, Character caster, Skill skill, Item card)
		{
			// Base calculation factors
			var bySkillLevel = 0.05f + (skill.Level * 0.02f);
			var byCardLevel = card.Level * 0.05f;
			var totalFactor = bySkillLevel + byCardLevel;

			// Apply stat bonuses
			var basePATK = summon.Properties.GetFloat(PropertyName.MINPATK);
			var baseMATK = summon.Properties.GetFloat(PropertyName.MINMATK);
			var baseDEF = summon.Properties.GetFloat(PropertyName.DEF);
			var baseMDEF = summon.Properties.GetFloat(PropertyName.MDEF);

			summon.Properties.SetFloat(PropertyName.PATK_BM, basePATK * totalFactor);
			summon.Properties.SetFloat(PropertyName.MATK_BM, baseMATK * totalFactor);
			summon.Properties.SetFloat(PropertyName.DEF_BM, baseDEF * totalFactor);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, baseMDEF * totalFactor);

			// Set movement speed
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
		}

		/// <summary>
		/// Kills existing summons with the specified property.
		/// </summary>
		private void KillExistingSummons(Character character, string propertyName)
		{
			var existingSummons = character.Summons.GetSummons(s =>
				s.Vars.TryGetInt(propertyName, out var value) && value == 1);

			foreach (var summon in existingSummons)
			{
				summon.Kill(character);
			}
		}
	}
}
