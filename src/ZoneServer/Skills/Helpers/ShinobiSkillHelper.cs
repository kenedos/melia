using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Shinobi's clones.
	/// </summary>
	public static class ShinobiSkillHelper
	{
		private const int GenChance = 10;
		private static readonly TimeSpan GenDuration = TimeSpan.FromMilliseconds(3500);
		private static readonly TimeSpan GenStunDuration = TimeSpan.FromSeconds(2);
		private static readonly BuffId[] GenDebuffs = [BuffId.Common_Slow, BuffId.UC_blind, BuffId.Common_Silence, BuffId.Stun];

		/// <summary>
		/// Returns the character's living Bunshin clones.
		/// </summary>
		/// <param name="owner"></param>
		/// <returns></returns>
		public static List<DummyCharacter> GetClones(ICombatEntity owner)
		{
			if (owner is not Character character || owner.Map == null)
				return [];

			return owner.Map.GetCharacters(a => a is DummyCharacter dummy && dummy.Owner == character && dummy.IsBuffActive(BuffId.Bunshin_Buff))
				.OfType<DummyCharacter>()
				.ToList();
		}

		/// <summary>
		/// Has every clone of the owner perform the same attack, from where
		/// it stands and facing where the owner faces.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="skillId"></param>
		/// <param name="attack"></param>
		public static void ReplicateOnClones(ICombatEntity owner, SkillId skillId, Func<Skill, ICombatEntity, Task> attack)
		{
			if (owner is DummyCharacter)
				return;

			foreach (var clone in GetClones(owner))
			{
				if (clone.IsDead || !clone.TryGetSkill(skillId, out var cloneSkill))
					continue;

				clone.Direction = owner.Direction;
				Send.ZC_ROTATE(clone);

				cloneSkill.Run(attack(cloneSkill, clone));
			}
		}

		/// <summary>
		/// Takes the owner's clones out of the world.
		/// </summary>
		/// <param name="owner"></param>
		public static void RemoveClones(ICombatEntity owner)
		{
			foreach (var clone in GetClones(owner))
			{
				Send.ZC_LEAVE(clone);
				clone.Map.RemoveCharacter(clone);
			}
		}

		/// <summary>
		/// Called when a clone hits an enemy: stacks Ninjutsu: Baku on the
		/// owner and, with Bunshin no Jutsu: Gen, may afflict the enemy.
		/// </summary>
		/// <param name="clone"></param>
		/// <param name="target"></param>
		/// <param name="skillId"></param>
		public static void OnCloneHit(DummyCharacter clone, ICombatEntity target, SkillId skillId)
		{
			var owner = clone.Owner;
			if (owner == null)
				return;

			owner.StartBuff(BuffId.Bunshin_Stack_Buff, 1, 0, TimeSpan.Zero, owner, SkillId.Shinobi_Bunshin_no_jutsu);

			if (!owner.IsAbilityActive(AbilityId.Shinobi18) || GameRandom.Get().Next(100) >= GenChance)
				return;

			var debuffId = skillId switch
			{
				SkillId.Shinobi_Kunai => BuffId.Common_Slow,
				SkillId.Shinobi_Katon_no_jutsu => BuffId.UC_blind,
				SkillId.Shinobi_Raiton_no_Jutsu => BuffId.Common_Silence,
				SkillId.Shinobi_Mijin_no_jutsu => BuffId.Stun,
				_ => GenDebuffs.PickRandom(),
			};

			var duration = debuffId == BuffId.Stun ? GenStunDuration : GenDuration;
			target.StartBuff(debuffId, 1, 0, duration, owner, SkillId.Shinobi_Bunshin_no_jutsu);
		}
	}
}
