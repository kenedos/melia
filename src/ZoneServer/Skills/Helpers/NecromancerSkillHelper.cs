using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Necromancer's corpse parts and summons.
	/// </summary>
	public static class NecromancerSkillHelper
	{
		private const int BaseCorpsePartCapacity = 300;
		private const int CorpsePartCapacityPerLevel = 100;
		private const int MaxSkeletonsPerType = 5;
		private static readonly TimeSpan DemoralizeDuration = TimeSpan.FromSeconds(4);

		/// <summary>
		/// Returns how many corpse parts the character's Necronomicon holds,
		/// matching the client's GET_NECRONOMICON_TOTAL_COUNT.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public static int GetCorpsePartCapacity(Character character)
		{
			var capacity = BaseCorpsePartCapacity;

			if (character.Abilities.TryGet(AbilityId.Necromancer21, out var ability))
				capacity += ability.Level * CorpsePartCapacityPerLevel;

			return capacity;
		}

		/// <summary>
		/// Adds corpse parts taken from the given monster to the character's
		/// Necronomicon, up to its capacity. Returns false if it was full.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monsterId"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public static bool AddCorpseParts(Character character, int monsterId, int amount)
		{
			var currentParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);
			var addedParts = Math.Min(amount, GetCorpsePartCapacity(character) - currentParts);

			if (addedParts <= 0)
				return false;

			for (var i = 1; i <= addedParts; i++)
			{
				var slotName = "NecroDParts_" + (currentParts + i);
				if (character.Etc.Properties.Has(slotName))
					character.Etc.Properties.SetFloat(slotName, monsterId);
			}

			character.ModifyEtcProperty(PropertyName.Necro_DeadPartsCnt, addedParts);
			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);

			return true;
		}

		/// <summary>
		/// Returns true if the caster has a card they still own in the
		/// Necronomicon's main slot. Casters other than characters always do.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		public static bool HasNecronomiconCard(ICombatEntity caster)
		{
			if (caster is not Character character)
				return true;

			var guid = character.Etc.Properties.GetString(PropertyName.Necro_bosscardGUID1, "None");
			if (!long.TryParse(guid, out var objectId))
				return false;

			return character.Inventory.TryGetItem(objectId, out var card) && card.Data.Group == ItemGroup.Card;
		}

		/// <summary>
		/// Returns true if the caster holds at least the given amount of
		/// corpse parts. Casters other than characters always have enough.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public static bool HasCorpseParts(ICombatEntity caster, int amount)
		{
			if (caster is not Character character)
				return true;

			return character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt) >= amount;
		}

		/// <summary>
		/// Removes the given amount of corpse parts from the caster's Necronomicon.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="amount"></param>
		public static void SpendCorpseParts(ICombatEntity caster, int amount)
		{
			if (caster is not Character character)
				return;

			var currentParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);

			character.ModifyEtcProperty(PropertyName.Necro_DeadPartsCnt, -Math.Min(amount, currentParts));
			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);
		}

		/// <summary>
		/// Returns true if the caster can summon another skeleton of the given type.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="monsterId"></param>
		/// <returns></returns>
		public static bool CanSummonSkeleton(ICombatEntity caster, int monsterId)
		{
			if (caster is not Character character)
				return true;

			return character.Summons.GetSummons(s => !s.IsDead && s.Id == monsterId).Count < MaxSkeletonsPerType;
		}

		/// <summary>
		/// Kills the caster's other summons of the same type as the given one.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="summon"></param>
		public static void RemovePreviousSummons(ICombatEntity caster, Mob summon)
		{
			if (caster is not Character character)
				return;

			foreach (var previous in character.Summons.GetSummons(s => s.Id == summon.Id && s.Handle != summon.Handle && !s.IsDead))
				previous.Kill(caster);
		}

		/// <summary>
		/// Weakens the target's attack if the caster has Flesh: Demoralize active.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		public static void ApplyDemoralize(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			if (target.IsDead || !caster.TryGetActiveAbilityLevel(AbilityId.Necromancer2, out var level))
				return;

			target.StartBuff(BuffId.Debrave_Debuff, level, 0, DemoralizeDuration, caster, skill.Id);
		}

		/// <summary>
		/// Returns the skill's caption ratio, scaled by its reinforce ability.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="captionRatio"></param>
		/// <returns></returns>
		public static float GetReinforcedRatio(Skill skill, string captionRatio)
		{
			var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");

			return skill.Properties.GetFloat(captionRatio) * (1f + SCR_Get_AbilityReinforceRate(skill));
		}

		/// <summary>
		/// Sets a summon's attack, defense and max HP from the summoner's.
		/// </summary>
		/// <remarks>
		/// A transfer rate of zero leaves the summon's own stat in place.
		/// </remarks>
		/// <param name="summon"></param>
		/// <param name="caster"></param>
		/// <param name="attackRate">Attack transfer, in percent.</param>
		/// <param name="defenseRate">Physical/magic defense transfer, in percent.</param>
		/// <param name="lifeRate">Max HP transfer, in percent.</param>
		public static void ApplySummonTransfer(Mob summon, ICombatEntity caster, float attackRate, float defenseRate, float lifeRate)
		{
			if (attackRate > 0)
			{
				var minAttack = (int)caster.Properties.GetFloat(PropertyName.MINMATK);
				var maxAttack = Math.Max(minAttack, (int)caster.Properties.GetFloat(PropertyName.MAXMATK));
				var attack = GameRandom.Get().Next(minAttack, maxAttack + 1) * attackRate / 100f;

				summon.Properties.SetFloat(PropertyName.FixedAttack, attack);
			}

			if (defenseRate > 0)
			{
				var defense = (caster.Properties.GetFloat(PropertyName.DEF) + caster.Properties.GetFloat(PropertyName.MDEF)) / 2 * defenseRate / 100f;
				summon.Properties.SetFloat(PropertyName.FixedDefence, defense);
			}

			if (lifeRate > 0)
				summon.Properties.SetFloat(PropertyName.FixedLife, caster.Properties.GetFloat(PropertyName.MHP) * lifeRate / 100f);

			summon.Properties.InvalidateAll();
			summon.Properties.SetFloat(PropertyName.HP, summon.Properties.GetFloat(PropertyName.MHP));
			summon.Properties.SetFloat(PropertyName.SP, summon.Properties.GetFloat(PropertyName.MSP));
		}
	}
}
