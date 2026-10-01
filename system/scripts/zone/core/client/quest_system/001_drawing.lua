M_QUESTS_SEARCH_TEXT = ""
M_QUESTS_COLLAPSED = {}

function M_QUESTS_SET_SEARCH(text)
	M_QUESTS_SEARCH_TEXT = string.lower(text or "")
	M_QUESTS_UPDATE_LIST()
end

function M_QUESTS_IS_FILTERED(quest, filters)
	if filters[quest.Type] == false then
		return true
	end

	if quest.Tracked and filters["Chase"] ~= true then
		return true
	end

	if M_QUESTS_SEARCH_TEXT == "" then
		return false
	end

	local name = string.lower(quest.Name or "")
	local location = string.lower(quest.Location or "")
	local inName = string.find(name, M_QUESTS_SEARCH_TEXT, 1, true)
	local inLocation = string.find(location, M_QUESTS_SEARCH_TEXT, 1, true)

	return inName == nil and inLocation == nil
end

function M_QUESTS_DRAW_LIST(frame, quests)
	local y = 0

	frame:DeleteAllControl()

	local filters = GET_QUEST_MODE_OPTION()
	local sections = {}

	for i = 1, #quests do
		local quest = quests[i]

		if not M_QUESTS_IS_FILTERED(quest, filters) then
			local typeName = M_QUEST_TYPE_STYLE[quest.Type] and quest.Type or "Sub"
			sections[typeName] = sections[typeName] or {}
			table.insert(sections[typeName], quest)
		end
	end

	for i = 1, #M_QUEST_TYPE_ORDER do
		local typeName = M_QUEST_TYPE_ORDER[i]
		local section = sections[typeName]

		if section ~= nil and #section > 0 then
			M_QUESTS_SORT_SECTION(section)
			y = y + M_QUESTS_DRAW_SECTION(frame, typeName, section, y)
		end
	end

	frame:Invalidate()
end
