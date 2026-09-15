using System;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Prerequisites
{
	/// <summary>
	/// A prerequisite that requires another quest to have reached a
	/// certain status.
	/// </summary>
	public class QuestStatusPrerequisite : QuestPrerequisite
	{
		/// <summary>
		/// Returns the id of the quest whose status is checked.
		/// </summary>
		public QuestId QuestId { get; }

		/// <summary>
		/// Returns the status the quest has to have reached.
		/// </summary>
		public QuestStatus MinStatus { get; }

		/// <summary>
		/// Creates a new instance for a quest in a namespace.
		/// </summary>
		/// <param name="questNamespace"></param>
		/// <param name="id"></param>
		/// <param name="minStatus"></param>
		public QuestStatusPrerequisite(string questNamespace, long id, QuestStatus minStatus)
		{
			this.QuestId = new QuestId(questNamespace, id);
			this.MinStatus = minStatus;
		}

		/// <summary>
		/// Creates a new instance for a quest identified by its raw id.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="minStatus"></param>
		public QuestStatusPrerequisite(long id, QuestStatus minStatus)
		{
			this.QuestId = new QuestId(id);
			this.MinStatus = minStatus;
		}

		/// <summary>
		/// Creates a new instance for a quest identified by its class name.
		/// </summary>
		/// <param name="questClassName"></param>
		/// <param name="minStatus"></param>
		public QuestStatusPrerequisite(string questClassName, QuestStatus minStatus)
		{
			if (!ZoneServer.Instance.Data.QuestDb.TryFind(questClassName, out var questData))
				throw new ArgumentException($"Unknown quest '{questClassName}'.");

			this.QuestId = new QuestId(questData.Id);
			this.MinStatus = minStatus;
		}

		/// <summary>
		/// Returns true if the character's quest has reached the required
		/// status at least once.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public override bool Met(Character character)
		{
			if (this.MinStatus >= QuestStatus.Completed)
				return character.Quests.HasCompleted(this.QuestId);

			if (!character.Quests.TryGetById(this.QuestId, out var quest))
				return false;

			return quest.Status >= this.MinStatus;
		}
	}
}
