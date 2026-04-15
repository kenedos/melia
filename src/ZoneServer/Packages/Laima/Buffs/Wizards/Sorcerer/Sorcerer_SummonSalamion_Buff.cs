using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the SummonSalamion_Buff.
	/// Periodically heals the Salamion and nearby summons owned by the same player.
	/// </summary>
	/// <remarks>
	/// Healing:
	/// - Heals for MHP * (2.5% + skillLevel * 0.5%)
	/// - Update interval: 20 seconds - (abilityLevel * 1 second)
	/// - Affects Salamion and all summons within 150 range owned by the same player
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.SummonSalamion_Buff)]
	public class SummonSalamion_BuffOverride : BuffHandler
	{
		private const float HealRange = 150f;

		/// <summary>
		/// Called when the buff is activated.
		/// Sets up the update interval based on ability level.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Get the owner (the character who summoned Salamion)
			var owner = GetSummonOwner(buff.Target);
			if (owner == null)
				return;

			// Calculate update time based on Sorcerer17 ability level
			var baseUpdateTime = 20000; // 20 seconds base
			var abilityLevel = owner.GetAbilityLevel(AbilityId.Sorcerer17);
			var updateTime = baseUpdateTime - (abilityLevel * 1000);

			// Ensure minimum update time of 5 seconds
			updateTime = Math.Max(updateTime, 5000);

			buff.SetUpdateTime(updateTime);
		}

		/// <summary>
		/// Called periodically while the buff is active.
		/// Heals the Salamion and nearby summons.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Summon salamion)
				return;

			var owner = GetSummonOwner(salamion);
			if (owner == null)
				return;

			// Get the SummonSalamion skill for heal calculation
			if (!owner.TryGetSkill(SkillId.Sorcerer_SummonSalamion, out var skill))
				return;

			// Calculate heal rate: MHP * (2.5% + skillLevel * 0.5%)
			var healRate = 0.025f + (skill.Level * 0.005f);

			// Heal Salamion
			HealTarget(salamion, healRate);

			// Find and heal nearby summons owned by the same player
			var nearbySummons = salamion.Map.GetActorsInRange<Summon>(salamion.Position, HealRange)
				.Where(s => !s.IsDead && IsSameOwner(s, owner))
				.ToList();

			foreach (var summon in nearbySummons)
			{
				HealTarget(summon, healRate);
			}
		}

		/// <summary>
		/// Called when the buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			// No cleanup needed
		}

		/// <summary>
		/// Gets the character owner of a summon.
		/// </summary>
		private Character GetSummonOwner(ICombatEntity entity)
		{
			if (entity is Summon summon && summon.Owner is Character character)
				return character;

			return null;
		}

		/// <summary>
		/// Checks if a summon has the same owner as the specified character.
		/// </summary>
		private bool IsSameOwner(Summon summon, Character owner)
		{
			if (summon.Owner is Character summonOwner)
				return summonOwner.Handle == owner.Handle;

			return false;
		}

		/// <summary>
		/// Heals a target based on their max HP and heal rate.
		/// </summary>
		private void HealTarget(ICombatEntity target, float healRate)
		{
			var maxHP = target.Properties.GetFloat(PropertyName.MHP);
			var healAmount = maxHP * healRate;

			target.Heal(healAmount, 0);

			// Play heal effect
			Send.ZC_NORMAL.PlayEffect(target, "F_recovery_green", 1f);
		}
	}
}
