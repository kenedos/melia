//--- Melia Script ----------------------------------------------------------
// Quest State Command
//--- Description -----------------------------------------------------------
// GM command that puts a character's quest chain into a given state, for
// testing a step without playing the chain up to it.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Prerequisites;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

public class QuestStateCommandScript : GeneralScript
{
	protected override void Load()
	{
		AddChatCommand("queststate", "<quest id|class name|\"title\"> <before|active|success|done>", "Sets a quest chain to the given state.", 50, 50, HandleQuestState);
	}

	private CommandResult HandleQuestState(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (args.Count < 2)
			return CommandResult.InvalidArgument;

		var input = string.Join(" ", Enumerable.Range(0, args.Count - 1).Select(i => args.Get(i))).Trim().Trim('"').Trim();

		if (!TryResolveQuest(input, out var questScript, out var candidates))
		{
			if (candidates.Count == 0)
				sender.ServerMessage(L("Quest '{0}' not found."), input);
			else
				sender.ServerMessage(L("'{0}' matches several quests: {1}"), input, string.Join(", ", candidates.Take(6).Select(c => c.Data.Name + " (" + c.QuestId.Value + ")")));

			return CommandResult.Okay;
		}

		var state = args.Get(args.Count - 1).ToLowerInvariant();
		if (state != "before" && state != "active" && state != "success" && state != "done")
			return CommandResult.InvalidArgument;

		var questId = questScript.QuestId;
		var quests = target.Quests;

		foreach (var prior in GetPriorQuests(questId).Where(QuestScript.Exists))
			quests.ForceComplete(prior);

		foreach (var later in GetLaterQuests(questId))
			quests.Remove(later);

		quests.Remove(questId);

		if (state == "done")
		{
			quests.ForceComplete(questId);
		}
		else if (state != "before")
		{
			var unmet = questScript.Data.Prerequisites.Where(p => !p.Met(target)).ToList();
			foreach (var prerequisite in unmet)
			{
				if (prerequisite is ItemPrerequisite itemPrerequisite)
					target.Inventory.Add(itemPrerequisite.ItemId, itemPrerequisite.MinItemAmount);
			}

			if (!quests.MeetsPrerequisites(questId))
			{
				sender.ServerMessage(L("'{0}' can't be started, its prerequisites aren't met."), questScript.Data.Name);
				return CommandResult.Okay;
			}

			quests.Start(questId);

			if (state == "success" && quests.TryGetById(questId, out var quest))
			{
				foreach (var progress in quest.Progresses.ToList())
					quests.CompleteObjective(questId, progress.Objective.Ident);
			}
		}

		target.LookAround();

		sender.ServerMessage(L("'{0}' ({1}) set to '{2}' for {3}."), questScript.Data.Name, questId.Value, state, target.Name);
		if (sender != target)
			target.ServerMessage(L("A GM set your quest '{0}' to '{1}'."), questScript.Data.Name, state);

		return CommandResult.Okay;
	}

	/// <summary>
	/// Finds a quest script by its client id, class name or title, the
	/// title matching exactly first and then by a unique partial match.
	/// </summary>
	private static bool TryResolveQuest(string input, out QuestScript questScript, out List<QuestScript> candidates)
	{
		questScript = null;
		candidates = new List<QuestScript>();

		if (long.TryParse(input, out var id))
			return QuestScript.TryGet(new QuestId(id), out questScript);

		if (ZoneServer.Instance.Data.QuestDb.TryFind(input, out var questData) && QuestScript.TryGet(new QuestId(questData.Id), out questScript))
			return true;

		var all = QuestScript.GetAll().Where(s => !string.IsNullOrEmpty(s.Data.Name)).ToList();

		candidates = all.Where(s => string.Equals(s.Data.Name, input, StringComparison.OrdinalIgnoreCase)).ToList();
		if (candidates.Count == 0)
			candidates = all.Where(s => s.Data.Name.Contains(input, StringComparison.OrdinalIgnoreCase)).ToList();

		if (candidates.Count != 1)
			return false;

		questScript = candidates[0];
		return true;
	}

	/// <summary>
	/// Returns every quest the given one requires, directly or through
	/// the quests it requires in turn.
	/// </summary>
	private static HashSet<QuestId> GetPriorQuests(QuestId questId)
	{
		var result = new HashSet<QuestId>();
		var queue = new Queue<QuestId>();
		queue.Enqueue(questId);

		while (queue.Count != 0)
		{
			if (!QuestScript.TryGet(queue.Dequeue(), out var questScript))
				continue;

			foreach (var required in GetRequiredQuests(questScript.Data.Prerequisites))
			{
				if (required != questId && result.Add(required))
					queue.Enqueue(required);
			}
		}

		return result;
	}

	/// <summary>
	/// Returns every quest that requires the given one, directly or
	/// through a quest that requires it.
	/// </summary>
	private static HashSet<QuestId> GetLaterQuests(QuestId questId)
	{
		var dependents = new Dictionary<QuestId, List<QuestId>>();

		foreach (var questScript in QuestScript.GetAll())
		{
			foreach (var required in GetRequiredQuests(questScript.Data.Prerequisites))
			{
				if (!dependents.TryGetValue(required, out var list))
					dependents[required] = list = new List<QuestId>();

				list.Add(questScript.QuestId);
			}
		}

		var result = new HashSet<QuestId>();
		var queue = new Queue<QuestId>();
		queue.Enqueue(questId);

		while (queue.Count != 0)
		{
			if (!dependents.TryGetValue(queue.Dequeue(), out var list))
				continue;

			foreach (var dependent in list)
			{
				if (dependent != questId && result.Add(dependent))
					queue.Enqueue(dependent);
			}
		}

		return result;
	}

	/// <summary>
	/// Returns the quests named by the given prerequisites, taking the
	/// first quest of an Or.
	/// </summary>
	private static IEnumerable<QuestId> GetRequiredQuests(IEnumerable<QuestPrerequisite> prerequisites)
	{
		foreach (var prerequisite in prerequisites)
		{
			switch (prerequisite)
			{
				case QuestStatusPrerequisite status:
					yield return status.QuestId;
					break;

				case CompletedPrerequisite completed:
					yield return completed.QuestId;
					break;

				case OrPrerequisite or:
					var first = GetRequiredQuests(or.Prerequisites).FirstOrDefault();
					if (first != default)
						yield return first;
					break;
			}
		}
	}
}
