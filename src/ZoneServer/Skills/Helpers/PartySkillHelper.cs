using System.Collections.Generic;
using Melia.Shared.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for skills that affect the caster's party.
	/// </summary>
	public static class PartySkillHelper
	{
		/// <summary>
		/// Returns the caster and their living party members within range
		/// of the given position.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public static List<ICombatEntity> GetAlliesInRange(ICombatEntity caster, Position position, float range)
		{
			var allies = new List<ICombatEntity>();

			if (caster is Character character)
				allies.AddRange(caster.Map.GetPartyMembersInRange(character, position, range));

			if (!allies.Contains(caster) && !caster.IsDead && caster.Position.InRange2D(position, range))
				allies.Add(caster);

			return allies;
		}
	}
}
