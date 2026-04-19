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

			// Calculate scale based on Summoning skill level
			var summoningSkill = character.GetSkill(SkillId.Sorcerer_Summoning);
			var skillLevelForScale = summoningSkill?.Level ?? skill.Level;
			var scale = 0.7f + (skillLevelForScale / 15f) * 0.3f;
			if (isLegendCard)
				scale *= 1.15f;
			newSummon.Properties.SetFloat(PropertyName.Scale, scale);

			// Calculate MHP based on caster's MHP (same as Summoning)
			var casterMHP = character.Properties.GetFloat(PropertyName.MHP) - character.Properties.GetFloat(PropertyName.MHP_BM);
			var summonMHP = casterMHP * (8.5f + ((summoningSkill?.Level ?? skill.Level) * 0.5f));
			newSummon.Vars.SetFloat("SUMMON_SET_MHP_BY_CASTER", summonMHP);

			// Calculate stats
			CalculateSummonStats(newSummon, character, skill, card);

			// Set movement speed
			newSummon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			newSummon.Properties.SetFloat(PropertyName.RunMSPD, 160f);

			// Add lifetime component
			newSummon.Components.Add(new LifeTimeComponent(newSummon, TimeSpan.FromSeconds(MorphLifetimeSeconds)));

			// Activate the summon
			newSummon.SetState(true);

			// Add to character's summon list
			character.Summons.AddSummon(newSummon);

			// Apply PC_Summon buff
			newSummon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, newSummon);

			// Send property updates
			//Send.ZC_OBJECT_PROPERTY(newSummon, PropertyName.Scale);

			// Attach effects based on card type
			AttachSummonEffects(newSummon, isLegendCard);

			// Set color for morphed summon (slightly different tint)
			newSummon.AddEffect(ColorEffect.FromRgba(1.0f, 0.9f, 0.8f, 1.0f));
		}

		/// <summary>
		/// Calculates and sets the summon's stats based on skill level and card level.
		/// </summary>
		private void CalculateSummonStats(Summon summon, Character caster, Skill skill, Item card)
		{
			var bySkillLevel = 0.05f + (skill.Level * 0.02f);
			var byCardLevel = card.Level * 0.05f;
			var totalFactor = bySkillLevel + byCardLevel;

			var basePATK = summon.Properties.GetFloat(PropertyName.MINPATK);
			var baseMATK = summon.Properties.GetFloat(PropertyName.MINMATK);
			var baseDEF = summon.Properties.GetFloat(PropertyName.DEF);
			var baseMDEF = summon.Properties.GetFloat(PropertyName.MDEF);

			summon.Properties.SetFloat(PropertyName.PATK_BM, basePATK * totalFactor);
			summon.Properties.SetFloat(PropertyName.MATK_BM, baseMATK * totalFactor);
			summon.Properties.SetFloat(PropertyName.DEF_BM, baseDEF * totalFactor);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, baseMDEF * totalFactor);
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
