using System;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Prerequisites
{
	/// <summary>
	/// A prerequisite that's met if the given condition holds for the
	/// character.
	/// </summary>
	public class PredicatePrerequisite : QuestPrerequisite
	{
		private readonly Func<Character, bool> _condition;

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="condition"></param>
		public PredicatePrerequisite(Func<Character, bool> condition)
		{
			_condition = condition;
		}

		/// <summary>
		/// Returns true if the condition holds for the character.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public override bool Met(Character character)
		{
			return _condition(character);
		}
	}
}
