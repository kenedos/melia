function M_CHASE_GET_TRACKED(includeClient)
	if Melia.Quests == nil or M_QUEST_TYPE_ORDER == nil then
		return {}
	end

	local all = Melia.Quests.GetAll()
	local result = {}

	for i = 1, #M_QUEST_TYPE_ORDER do
		local typeName = M_QUEST_TYPE_ORDER[i]
		local section = {}

		for j = 1, #all do
			local quest = all[j]
			local questType = M_QUEST_TYPE_STYLE[quest.Type] and quest.Type or "Sub"
			local hasObjectives = quest.Objectives ~= nil and #quest.Objectives > 0

			if questType == typeName and quest.Tracked and (includeClient or quest.ClientId == nil) and hasObjectives then
				table.insert(section, quest)
			end
		end

		table.sort(section, function(a, b)
			if a.Done ~= b.Done then
				return a.Done
			end
			return (a.Name or "") < (b.Name or "")
		end)

		for j = 1, #section do
			table.insert(result, section[j])
		end
	end

	return result
end

function M_CHASE_GET_RETURNABLE()
	local quests = M_CHASE_GET_TRACKED(true)
	local result = {}

	for i = 1, #quests do
		if quests[i].Done and #result < M_CHASE_WARP_MAX_BTN then
			table.insert(result, quests[i])
		end
	end

	return result
end
