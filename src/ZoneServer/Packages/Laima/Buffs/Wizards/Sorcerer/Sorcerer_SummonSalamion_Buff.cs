using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Periodically heals the Salamion, its owner and nearby summons belonging to the same owner.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.SummonSalamion_Buff)]
	public class SummonSalamion_BuffOverride : BuffHandler
	{
		private const float HealRange = 150f;
		private const int BaseUpdateTime = 20000;
		private const int MinimumUpdateTime = 5000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var owner = GetSummonOwner(buff.Target);
			if (owner == null)
				return;

			var abilityLevel = Math.Max(1, owner.GetAbilityLevel(AbilityId.Sorcerer17));
			var updateTime = Math.Max(BaseUpdateTime - abilityLevel * 1000, MinimumUpdateTime);
			buff.SetUpdateTime(updateTime);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Summon salamion || salamion.IsDead)
				return;

			var owner = GetSummonOwner(salamion);
			if (owner == null || owner.IsDead)
				return;

			if (!owner.TryGetSkill(SkillId.Sorcerer_SummonSalamion, out var skill))
				return;

			var healRate = 0.025f + skill.Level * 0.005f;

			HealTarget(owner, healRate);
			HealTarget(salamion, healRate);

			var nearbySummons = salamion.Map.GetActorsInRange<Summon>(salamion.Position, HealRange)
				.Where(s => s.Handle != salamion.Handle && !s.IsDead && IsSameOwner(s, owner))
				.ToList();

			foreach (var summon in nearbySummons)
				HealTarget(summon, healRate);
		}

		public override void OnEnd(Buff buff)
		{
		}

		private Character GetSummonOwner(ICombatEntity entity)
		{
			if (entity is Summon summon && summon.Owner is Character character)
				return character;

			return null;
		}

		private bool IsSameOwner(Summon summon, Character owner)
		{
			return summon.Owner is Character summonOwner && summonOwner.Handle == owner.Handle;
		}

		private void HealTarget(ICombatEntity target, float healRate)
		{
			if (target == null || target.IsDead)
				return;

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);
			if (maxHp <= 0)
				return;

			var healAmount = Math.Max(1f, maxHp * healRate);
			target.Heal(healAmount, 0);
		}
	}
}
